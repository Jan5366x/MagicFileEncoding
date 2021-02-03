﻿using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MagicFileEncoding
{
    
    /// <summary>
    /// Magic File Encoding Static Class
    /// </summary>
    public static class FileEncoding
    {
        /// <summary>
        /// The fallback encoding<br />
        /// <i>ISO-8859-1 (Latin-1) by default</i>
        /// </summary>
        private static readonly Encoding DefaultFallback = AdditionalEncoding.ISO_8859_1;
        
        /// <summary>
        /// Find a acceptable encoding to open a given file
        /// </summary>
        /// <param name="filename">The file to check</param>
        /// <param name="fallbackEncoding">The fallback encoding (ISO-8859-1 by default)</param>
        /// <returns>Best suitable encoding</returns>
        public static Encoding GetAcceptableEncoding(string filename, Encoding fallbackEncoding = null) 
            => DetectTextEncoding(filename, out _, false) ?? fallbackEncoding ?? DefaultFallback;

        /// <summary>
        /// Automatic detect acceptable encoding and read all text from a given file and transform it into
        /// Unicode UTF16 encoding
        /// </summary>
        /// <param name="filename">The file to read text</param>
        /// <returns></returns>
        public static string ReadAllText(string filename) 
            => Encoding.Unicode.GetString(AutomaticTransformBytes(filename, Encoding.Unicode));

        /// <summary>
        /// Automatic detect acceptable encoding and read all text from a given file and transform it into
        /// a given target encoding
        /// </summary>
        /// <param name="filename">The file to read text</param>
        /// <param name="targetEncoding">The target encoding to transform to the return value</param>
        /// <returns></returns>
        public static string ReadAllText(string filename, Encoding targetEncoding)
            => targetEncoding
                .GetString(AutomaticTransformBytes(filename, targetEncoding))
                .Trim(new[]{'\uFEFF'});

        /// <summary>
        /// Write all text to a given file in a specific encoding
        /// </summary>
        /// <param name="path">The path to the text file</param>
        /// <param name="text">Text to encode and write to file</param>
        /// <param name="targetEncoding">Target encoding</param>
        public static void WriteAllText(string path, string text, Encoding targetEncoding) 
            => File.WriteAllText(path, text, targetEncoding);

        /// <summary>
        /// Automatic transform bytes
        /// </summary>
        /// <param name="filename">The file to analyze</param>
        /// <param name="targetEncoding"></param>
        /// <returns></returns>
        private static byte[] AutomaticTransformBytes(string filename, Encoding targetEncoding) 
            => Encoding.Convert(GetAcceptableEncoding(filename), targetEncoding, File.ReadAllBytes(filename));

        /// <summary>
        /// Function to detect the encoding for UTF-7, UTF-8/16/32 (bom, no bom, little
        /// and big endian), and local default codepage, and potentially other codepages.
        /// 'taster' = number of bytes to check of the file (to save processing). Higher
        /// value is slower, but more reliable (especially UTF-8 with special characters
        /// later on may appear to be ASCII initially). If taster = 0, then taster
        /// becomes the length of the file (for maximum reliability). 'text' is simply
        /// the string with the discovered encoding applied to the file.
        /// </summary>
        /// <param name="filename">The file to analyze</param>
        /// <param name="text"></param>
        /// <param name="provideText"></param>
        /// <param name="taster"></param>
        /// <param name="fallbackEncoding"></param>
        /// <returns></returns>
        private static Encoding DetectTextEncoding(string filename, out string text, bool provideText, int taster = 0,
            Encoding fallbackEncoding = null)
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
            // the below manually checks for a UTF8 pattern.
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
            const double threshold = 0.1; // proportion of chars step 2 which must be zeroed to be diagnosed as utf-16. 0.1 = 10%
            double count = 0;
            for (var n = 0; n < taster; n += 2)
                if (b[n] == 0)
                    count++;
            if (count / taster > threshold)
            {
                text = provideText ? Encoding.BigEndianUnicode.GetString(b) : null;
                return Encoding.BigEndianUnicode;
            }

            count = 0;
            for (var n = 1; n < taster; n += 2)
                if (b[n] == 0)
                    count++;
            
            if (count / taster > threshold)
            {
                // unicode little-endian
                text = provideText ? Encoding.Unicode.GetString(b) : null;
                return Encoding.Unicode;
            } 
            
            if (LongShot(ref text, provideText, taster, b, out var encoding))
                return encoding;
            
            // use the fallback encoding
            text = provideText ? (fallbackEncoding ?? DefaultFallback).GetString(b) : null;
            return fallbackEncoding ?? DefaultFallback;
        }

        /// <summary>
        /// A long shot - let's see if we can find "charset=xyz" or
        /// "encoding=xyz" to identify the encoding:
        /// </summary>
        /// <param name="text"></param>
        /// <param name="provideText"></param>
        /// <param name="taster"></param>
        /// <param name="b"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private static bool LongShot(ref string text, bool provideText, int taster, byte[] b, out Encoding encoding)
        {
            for (var n = 0; n < taster - 9; n++)
            {
                if (((b[n + 0] != 'c' && b[n + 0] != 'C') || (b[n + 1] != 'h' && b[n + 1] != 'H') ||
                     (b[n + 2] != 'a' && b[n + 2] != 'A') || (b[n + 3] != 'r' && b[n + 3] != 'R') ||
                     (b[n + 4] != 's' && b[n + 4] != 'S') || (b[n + 5] != 'e' && b[n + 5] != 'E') ||
                     (b[n + 6] != 't' && b[n + 6] != 'T') || b[n + 7] != '=') &&
                    ((b[n + 0] != 'e' && b[n + 0] != 'E') || (b[n + 1] != 'n' && b[n + 1] != 'N') ||
                     (b[n + 2] != 'c' && b[n + 2] != 'C') || (b[n + 3] != 'o' && b[n + 3] != 'O') ||
                     (b[n + 4] != 'd' && b[n + 4] != 'D') || (b[n + 5] != 'i' && b[n + 5] != 'I') ||
                     (b[n + 6] != 'n' && b[n + 6] != 'N') || (b[n + 7] != 'g' && b[n + 7] != 'G') ||
                     b[n + 8] != '=')) continue;

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
                    {
                        encoding = Encoding.GetEncoding(internalEnc);
                        return true;
                    }
                }
                catch
                {
                    // ... doesn't recognize the name of the encoding, break.
                    break;
                }
            }

            encoding = null;
            return false;
        }

        /// <summary>
        /// Get the encoding by byte order mark
        /// </summary>
        /// <param name="filename">The file to analyze</param>
        /// <param name="fallbackEncoding"></param>
        /// <returns></returns>
        private static Encoding GetEncodingByBom(string filename, Encoding fallbackEncoding = null)
        {
            using var file = new FileStream(filename, FileMode.Open, FileAccess.Read);
            return GetEncodingByBom(file, fallbackEncoding ?? DefaultFallback);
        }
        
        /// <summary>
        /// Try to get the encoding by byte order mark
        /// </summary>
        /// <param name="fileStream">The file stream to read bytes from</param>
        /// <param name="defaultEncoding"></param>
        /// <returns></returns>
        private static Encoding GetEncodingByBom(FileStream fileStream, Encoding defaultEncoding)
        {
            // Read the BOM
            var bom = new byte[4];
            fileStream.Position = 0;
            fileStream.Read(bom, 0, 4);

            return GetEncodingByBom( bom, defaultEncoding, out _,false);
        }

        /// <summary>
        /// Check if a byte array starts with a given byte signature
        /// </summary>
        /// <param name="bytes">The byte array</param>
        /// <param name="signature">The byte signature</param>
        /// <returns>Returns <i>true</i> if the array starts with a given byte signature</returns>
        private static bool SignatureMatch(byte[] bytes, params byte[] signature) 
            => bytes.Length >= signature.Length && !signature.Where((t, i) => bytes[i] != t).Any();

        /// <summary>
        /// Try to get the encoding by byte order mark and provide text if available and needed
        /// </summary>
        /// <param name="bytes">File byte array</param>
        /// <param name="fallback">Fallback encoding</param>
        /// <param name="text">Text output if text should be provided</param>
        /// <param name="provideText">Boolean value to indicate if text sould be provided</param>
        /// <returns>Encoding by bom or fallback</returns>
        private static Encoding GetEncodingByBom(byte[] bytes, Encoding fallback, out string text, bool provideText)
        {
            // BOM signature exists
            if (SignatureMatch(bytes, 0x00, 0x00, 0xFE, 0xFF))
            {
                text = provideText ? AdditionalEncoding.UTF_32BE.GetString(bytes, 4, bytes.Length - 4) : null;
                return AdditionalEncoding.UTF_32BE;
            }  

            if (SignatureMatch(bytes, 0xFF, 0xFE, 0x00, 0x00))
            {
                text = provideText ? Encoding.UTF32.GetString(bytes, 4, bytes.Length - 4) : null;
                return Encoding.UTF32;
            }

            if (SignatureMatch(bytes, 0xFE, 0xFF))
            {
                text = provideText ? Encoding.BigEndianUnicode.GetString(bytes, 2, bytes.Length - 2) : null;
                return Encoding.BigEndianUnicode;
            }

            if (SignatureMatch(bytes, 0xFF, 0xFE))
            {
                text = provideText ? Encoding.Unicode.GetString(bytes, 2, bytes.Length - 2) : null;
                return Encoding.Unicode;
            }

            if (SignatureMatch(bytes, 0xEF, 0xBB, 0xBF))
            {
                text = provideText ? Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3) : null;
                return Encoding.UTF8;
            }

            if (SignatureMatch(bytes,0x2b, 0x2f, 0x76))
            {
                text = provideText ? Encoding.UTF7.GetString(bytes, 3, bytes.Length - 3) : null;
                return Encoding.UTF7;
            }
            
            text = provideText ? fallback?.GetString(bytes) : null;
            
            // We actually have no idea what the encoding is if we reach this point, so return default
            return fallback;
        }
    }
}