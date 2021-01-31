using System;
using System.IO;
using System.Text;

namespace MagicFileEncoding
{
    public class FileEncoding
    {
        /// <summary>
        /// The fallback encoding<br />
        /// <i>ISO-8859-1 (Latin-1)by default</i>
        /// </summary>
        public Encoding FallbackEncoding { get; set; } = AdditionalEncoding.ISO_8859_1;
        
        /// <summary>
        /// Find a acceptable encoding to open a given file
        /// https://stackoverflow.com/questions/3825390/effective-way-to-find-any-files-encoding
        /// </summary>
        public Encoding GetAcceptableEncoding(string filename)
        {
            var encoding = DetectTextEncoding(filename, out _, false);

            // We have no idea what this is so we use the fallback encoding
            return encoding ?? FallbackEncoding;
        }

        /// <summary>
        /// target encoding is Unicode UTF16
        /// </summary>
        public string AutomaticReadAllText(string filename)
        {
            return Encoding.Unicode.GetString(AutomaticTransformBytes(filename, Encoding.Unicode));
        }
        
        /// <summary>
        /// Automatic detect acceptable encoding and read all text from a given file
        /// </summary>
        public string AutomaticReadAllText(string filename, Encoding targetEncoding)
        { 
            return targetEncoding
                .GetString(AutomaticTransformBytes(filename, targetEncoding))
                .Trim(new[]{'\uFEFF'});
        }

        /// <summary>
        /// Write all text to a given file in the default encoding
        /// </summary>
        /// <see cref="Encoding.Default"/>
        public void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents, Encoding.Default);
        }
        
        /// <summary>
        /// Write all text to a given file in a specific encoding
        /// </summary>
        public void WriteAllText(string path, string contents, Encoding targetEncoding)
        {
            File.WriteAllText(path, contents, targetEncoding);
        }

        /// <summary>
        /// Automatic Transform Bytes
        /// </summary>
        public byte[] AutomaticTransformBytes(string filename, Encoding targetEncoding)
        { 
            return Encoding.Convert(GetAcceptableEncoding(filename), 
                targetEncoding, File.ReadAllBytes(filename));
        }

        /// <summary>
        /// https://stackoverflow.com/questions/1025332/determine-a-strings-encoding-in-c-sharp
        /// Function to detect the encoding for UTF-7, UTF-8/16/32 (bom, no bom, little
        /// & big endian), and local default codepage, and potentially other codepages.
        /// 'taster' = number of bytes to check of the file (to save processing). Higher
        /// value is slower, but more reliable (especially UTF-8 with special characters
        /// later on may appear to be ASCII initially). If taster = 0, then taster
        /// becomes the length of the file (for maximum reliability). 'text' is simply
        /// the string with the discovered encoding applied to the file.
        /// </summary>
        public Encoding DetectTextEncoding(string filename, out string text, bool provideText, int taster = 0)
        {
            var b = File.ReadAllBytes(filename);

            var encodingByBom = GetEncodingByBom(b, null, out text, provideText);
            if (encodingByBom != null)
                return encodingByBom;


            // If the code reaches here, no BOM/signature was found, so now
            // we need to 'taste' the file to see if can manually discover
            // the encoding. A high taster value is desired for UTF-8
            if (taster == 0 || taster > b.Length)
                taster = b.Length; // Taster size can't be bigger than the filesize obviously.


            // Some text files are encoded in UTF8, but have no BOM/signature. Hence
            // the below manually checks for a UTF8 pattern. This code is based off
            // the top answer at: https://stackoverflow.com/questions/6555015/check-for-invalid-utf8
            // For our purposes, an unnecessarily strict (and terser/slower)
            // implementation is shown at: https://stackoverflow.com/questions/1031645/how-to-detect-utf-8-in-plain-c
            // For the below, false positives should be exceedingly rare (and would
            // be either slightly malformed UTF-8 (which would suit our purposes
            // anyway) or 8-bit extended ASCII/UTF-16/32 at a vanishingly long shot).
            var i = 0;
            var utf8 = false;
            while (i < taster - 4)
            {
                if (b[i] <= 0x7F)
                {
                    i += 1;
                    continue;
                } 
                
                // If all characters are below 0x80, then it is valid UTF8,
                // but UTF8 is not 'required' (and therefore the text is more desirable to be treated as
                // the default codepage of the computer). Hence, there's no "utf8 = true;"
                // code unlike the next three checks.

                if (b[i] >= 0xC2 && b[i] <= 0xDF && b[i + 1] >= 0x80 && b[i + 1] < 0xC0)
                {
                    i += 2;
                    utf8 = true;
                    continue;
                }

                if (b[i] >= 0xE0 && b[i] <= 0xF0 && b[i + 1] >= 0x80 && b[i + 1] < 0xC0 && b[i + 2] >= 0x80 &&
                    b[i + 2] < 0xC0)
                {
                    i += 3;
                    utf8 = true;
                    continue;
                }

                if (b[i] >= 0xF0 && b[i] <= 0xF4 && b[i + 1] >= 0x80 && b[i + 1] < 0xC0 && b[i + 2] >= 0x80 &&
                    b[i + 2] < 0xC0 && b[i + 3] >= 0x80 && b[i + 3] < 0xC0)
                {
                    i += 4;
                    utf8 = true;
                    continue;
                }

                utf8 = false;
                break;
            }

            if (utf8)
            {
                text = provideText ? Encoding.UTF8.GetString(b) : null;
                return Encoding.UTF8;
            }


            // The next check is a heuristic attempt to detect UTF-16 without a BOM.
            // We simply look for zeroes in odd or even byte places, and if a certain
            // threshold is reached, the code is 'probably' UF-16.          
            var threshold = 0.1; // proportion of chars step 2 which must be zeroed to be diagnosed as utf-16. 0.1 = 10%
            var count = 0;
            for (var n = 0; n < taster; n += 2)
                if (b[n] == 0)
                    count++;
            if ((double) count / taster > threshold)
            {
                text = provideText ? Encoding.BigEndianUnicode.GetString(b) : null;
                return Encoding.BigEndianUnicode;
            }

            count = 0;
            for (var n = 1; n < taster; n += 2)
                if (b[n] == 0)
                    count++;
            if ((double) count / taster > threshold)
            {
                // unicode little-endian
                text = provideText ? Encoding.Unicode.GetString(b) : null;
                return Encoding.Unicode;
            } 
            
            // Finally, a long shot - let's see if we can find "charset=xyz" or
            // "encoding=xyz" to identify the encoding:
            for (var n = 0; n < taster - 9; n++)
                if (
                    (b[n + 0] == 'c' || b[n + 0] == 'C') && (b[n + 1] == 'h' || b[n + 1] == 'H') &&
                    (b[n + 2] == 'a' || b[n + 2] == 'A') && (b[n + 3] == 'r' || b[n + 3] == 'R') &&
                    (b[n + 4] == 's' || b[n + 4] == 'S') && (b[n + 5] == 'e' || b[n + 5] == 'E') &&
                    (b[n + 6] == 't' || b[n + 6] == 'T') && b[n + 7] == '=' ||
                    (b[n + 0] == 'e' || b[n + 0] == 'E') && (b[n + 1] == 'n' || b[n + 1] == 'N') &&
                    (b[n + 2] == 'c' || b[n + 2] == 'C') && (b[n + 3] == 'o' || b[n + 3] == 'O') &&
                    (b[n + 4] == 'd' || b[n + 4] == 'D') && (b[n + 5] == 'i' || b[n + 5] == 'I') &&
                    (b[n + 6] == 'n' || b[n + 6] == 'N') && (b[n + 7] == 'g' || b[n + 7] == 'G') && b[n + 8] == '='
                )
                {
                    if (b[n + 0] == 'c' || b[n + 0] == 'C') n += 8;
                    else n += 9;
                    if (b[n] == '"' || b[n] == '\'') n++;
                    var oldn = n;
                    while (n < taster && (b[n] == '_' || b[n] == '-' || b[n] >= '0' && b[n] <= '9' ||
                                          b[n] >= 'a' && b[n] <= 'z' || b[n] >= 'A' && b[n] <= 'Z'))
                        n++;

                    var nb = new byte[n - oldn];
                    Array.Copy(b, oldn, nb, 0, n - oldn);
                    try
                    {
                        var internalEnc = Encoding.ASCII.GetString(nb);
                        text = provideText ? Encoding.GetEncoding(internalEnc).GetString(b) : null;
                        return Encoding.GetEncoding(internalEnc);
                    }
                    catch
                    {
                        // ... doesn't recognize the name of the encoding, break.
                        break;
                    } 
                }


            // If all else fails, the encoding is probably (though certainly not
            // definitely) the user's local codepage! One might present to the user a
            // list of alternative encodings as shown here:
            // https://stackoverflow.com/questions/8509339/what-is-the-most-common-encoding-of-each-language
            // A full list can be found using Encoding.GetEncodings();
            text = provideText ? FallbackEncoding.GetString(b) : null;
            return FallbackEncoding;
        }
        
        /// <summary>
        /// Get the encoding by byte order mark
        /// </summary>
        public Encoding GetEncodingByBom(string filename)
        {
            return GetEncodingByBom(filename, FallbackEncoding);
        }

        /// <summary>
        /// Get the encoding by byte order mark
        /// </summary>
        public Encoding GetEncodingByBom(string filename, Encoding defaultEncoding)
        {
            using var file = new FileStream(filename, FileMode.Open, FileAccess.Read);
            return GetEncodingByBom(file, defaultEncoding);
        }
        
        /// <summary>
        /// Get the encoding by byte order mark
        /// </summary>
        public static Encoding GetEncodingByBom(FileStream fileStream, Encoding defaultEncoding)
        {
            // Read the BOM
            var bom = new byte[4];
            fileStream.Position = 0;
            fileStream.Read(bom, 0, 4);

            return GetEncodingByBom( bom, defaultEncoding, out _,false);
        }

        private static Encoding GetEncodingByBom(byte[] b, Encoding fallback, out string text,
            bool provideText)
        {

            // BOM/signature exists (sourced from http://www.unicode.org/faq/utf_bom.html#bom4)
            if (b.Length >= 4 && b[0] == 0x00 && b[1] == 0x00 && b[2] == 0xFE && b[3] == 0xFF)
            {
                // UTF-32, big-endian
                text = provideText ? AdditionalEncoding.UTF_32BE.GetString(b, 4, b.Length - 4) : null;
                return AdditionalEncoding.UTF_32BE;
            }  

            if (b.Length >= 4 && b[0] == 0xFF && b[1] == 0xFE && b[2] == 0x00 && b[3] == 0x00)
            {
                text = provideText ? Encoding.UTF32.GetString(b, 4, b.Length - 4) : null;
                return Encoding.UTF32;
            }

            if (b.Length >= 2 && b[0] == 0xFE && b[1] == 0xFF)
            {
                text = provideText ? Encoding.BigEndianUnicode.GetString(b, 2, b.Length - 2) : null;
                return Encoding.BigEndianUnicode;
            }

            if (b.Length >= 2 && b[0] == 0xFF && b[1] == 0xFE)
            {
                text = provideText ? Encoding.Unicode.GetString(b, 2, b.Length - 2) : null;
                return Encoding.Unicode;
            }

            if (b.Length >= 3 && b[0] == 0xEF && b[1] == 0xBB && b[2] == 0xBF)
            {
                text = provideText ? Encoding.UTF8.GetString(b, 3, b.Length - 3) : null;
                return Encoding.UTF8;
            }

            if (b.Length >= 3 && b[0] == 0x2b && b[1] == 0x2f && b[2] == 0x76)
            {
                text = provideText ? Encoding.UTF7.GetString(b, 3, b.Length - 3) : null;
                return Encoding.UTF7;
            }
            
            text = provideText ? fallback?.GetString(b) : null;
            
            // We actually have no idea what the encoding is if we reach this point, so return default
            return fallback;
        }

        /// <summary>
        /// 
        /// </summary>
        public Encoding GetEncodingByParsing(string filename, Encoding encoding)
        {
            var encodingVerifier = Encoding.GetEncoding(encoding.BodyName, new EncoderExceptionFallback(),
                new DecoderExceptionFallback());
            try
            {
                using var textReader =
                    new StreamReader(filename, encodingVerifier, true);
                while (!textReader.EndOfStream)
                    textReader.ReadLine(); // in order to increment the stream position

                // all text parsed ok
                return textReader.CurrentEncoding;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}