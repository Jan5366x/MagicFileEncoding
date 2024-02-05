using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MagicFileEncoding.Tools;

internal static class EncodingTools
{
    /// <summary>
    /// Automatic transform bytes
    /// </summary>
    /// <param name="bytes">The byte array to analyze</param>
    /// <param name="targetEncoding">The output target encoding</param>
    /// <param name="fallbackEncoding">Fallback encoding for the input</param>
    /// <returns>Transformed byte array</returns>
    internal static byte[] AutomaticTransformBytes(byte[] bytes, Encoding targetEncoding,
        Encoding? fallbackEncoding = null)
        => Encoding.Convert(FileEncoding.GetAcceptableEncoding(bytes, fallbackEncoding), targetEncoding,
            bytes);
    
    /// <summary>
    /// Function to detect the encoding for UTF-7, UTF-8/16/32 (bom, no bom, little
    /// and big endian), and local default codepage, and potentially other codepages.
    /// 'taster' = number of bytes to check of the file (to save processing). Higher
    /// value is slower, but more reliable (especially UTF-8 with special characters
    /// later on may appear to be ASCII initially). If taster = 0, then taster
    /// becomes the length of the file (for maximum reliability). 'text' is simply
    /// the string with the discovered encoding applied to the file.
    /// </summary>
    /// <param name="bytes">Byte array to analyze</param>
    /// <param name="text">The text output</param>
    /// <param name="provideText">Flag if text should be provided to the text output</param>
    /// <param name="taster">The taster depth</param>
    /// <param name="fallbackEncoding">The fallback encoding which should be used if we have no match</param>
    /// <returns>Returns the detected encoding</returns>
    internal static Encoding? DetectTextEncoding(byte[] bytes, out string? text, bool provideText, int taster = 0,
        Encoding? fallbackEncoding = null)
    {

        var encodingByBom = GetEncodingByBom(bytes, null, out text, provideText);
        if (encodingByBom != null)
            return encodingByBom;

        // If the code reaches here, no BOM/signature was found, so now
        // we need to 'taste' the file to see if can manually discover
        // the encoding. A high taster value is desired for UTF-8
        if (taster == 0 || taster > bytes.Length)
            taster = bytes.Length; // Taster size can't be bigger than the filesize obviously.

        // Some text files are encoded in UTF8, but have no BOM/signature. Hence
        // the below manually checks for a UTF8 pattern.
        // For the below, false positives should be exceedingly rare (and would
        // be either slightly malformed UTF-8 (which would suit our purposes
        // anyway) or 8-bit extended ASCII/UTF-16/32 at a vanishingly long shot).
        

        if (CheckForUtf8(bytes, taster))
        {
            text = provideText ? Encoding.UTF8.GetString(bytes) : null;
            return Encoding.UTF8;
        }

        // The next check is a heuristic attempt to detect UTF-16 without a BOM.
        // We simply look for zeroes in odd or even byte places, and if a certain
        // threshold is reached, the code is 'probably' UF-16.

        // proportion of chars step 2 which must be zeroed to be diagnosed as utf-16. 0.1 = 10%
        const double threshold = 0.1;

        double count = 0;
        for (var n = 0; n < taster; n += 2)
            if (bytes[n] == 0)
                count++;
        if (count / taster > threshold)
        {
            text = provideText ? Encoding.BigEndianUnicode.GetString(bytes) : null;
            return Encoding.BigEndianUnicode;
        }

        count = 0;
        for (var n = 1; n < taster; n += 2)
            if (bytes[n] == 0)
                count++;

        if (count / taster > threshold)
        {
            // unicode little-endian
            text = provideText ? Encoding.Unicode.GetString(bytes) : null;
            return Encoding.Unicode;
        }

        if (LongShot(ref text, provideText, taster, bytes, out var encoding))
            return encoding;

        // use the fallback encoding
        text = provideText ? (fallbackEncoding ?? FileEncoding.DefaultFallback).GetString(bytes) : null;
        return fallbackEncoding ?? FileEncoding.DefaultFallback;
    }

    private static bool CheckForUtf8(byte[] bytes, int taster)
    {
        var utf8 = false;
        var i = 0;
        while (i < taster - 4)
        {
            if (bytes[i] <= 0x7F)
            {
                i += 1;
                continue;
            }

            // If all characters are below 0x80, then it is valid UTF8,
            // but UTF8 is not 'required' (and therefore the text is more desirable to be treated as
            // the default codepage of the computer). Hence, there's no "utf8 = true;"
            // code unlike the next three checks.

            if (bytes[i] >= 0xC2 && bytes[i] <= 0xDF && bytes[i + 1] >= 0x80 && bytes[i + 1] < 0xC0)
            {
                i += 2;
                utf8 = true;
                continue;
            }

            if (bytes[i] >= 0xE0 && bytes[i] <= 0xF0 && bytes[i + 1] >= 0x80 && bytes[i + 1] < 0xC0 && bytes[i + 2] >= 0x80 &&
                bytes[i + 2] < 0xC0)
            {
                i += 3;
                utf8 = true;
                continue;
            }

            if (bytes[i] >= 0xF0 && bytes[i] <= 0xF4 && bytes[i + 1] >= 0x80 && bytes[i + 1] < 0xC0 && 
                bytes[i + 2] >= 0x80 && bytes[i + 2] < 0xC0 && bytes[i + 3] >= 0x80 && bytes[i + 3] < 0xC0)
            {
                i += 4;
                utf8 = true;
                continue;
            }

            utf8 = false;
            break;
        }

        return utf8;
    }

    /// <summary>
    /// A long shot - let's see if we can find "charset=xyz" or
    /// "encoding=xyz" to identify the encoding:
    /// </summary>
    /// <param name="text">The text output</param>
    /// <param name="provideText">Flag if text should be provided to the text output</param>
    /// <param name="taster">Taster depth</param>
    /// <param name="bytes">The byte array</param>
    /// <param name="encoding">The encoding</param>
    /// <returns>Returns <i>true</i> if the long shot was successful</returns>
    private static bool LongShot(ref string? text, bool provideText, int taster, byte[] bytes, out Encoding? encoding)
    {
        for (var n = 0; n < taster - 9; n++)
        {
            if (!IsCharsetMarker(bytes, n) && !IsEncodingMarker(bytes, n))
            {
                continue;
            }

            if (bytes[n + 0] == 'c' || bytes[n + 0] == 'C')
            {
                n += 8;
            }
            else
            {
                n += 9;
            }

            if (bytes[n] == '"' || bytes[n] == '\'')
            {
                n++;
            }

            var oldN = n;

            while (IsCharsetNameRange(taster, bytes, n))
            {
                n++;
            }

            var nb = new byte[n - oldN];
            Array.Copy(bytes, oldN, nb, 0, n - oldN);
            try
            {
                var internalEnc = Encoding.ASCII.GetString(nb);
                text = provideText ? Encoding.GetEncoding(internalEnc).GetString(bytes) : null;
                
                encoding = Encoding.GetEncoding(internalEnc);
                return true;
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
    /// Is encoding marker
    /// </summary>
    /// <param name="bytes">The byte array</param>
    /// <param name="n">Location</param>
    /// <returns>Returns <i>true</i> if encoding marker was found</returns>
    private static bool IsEncodingMarker(byte[] bytes, int n)
    {
        return (bytes[n + 0] == 'e' || bytes[n + 0] == 'E') && (bytes[n + 1] == 'n' || bytes[n + 1] == 'N') && 
               (bytes[n + 2] == 'c' || bytes[n + 2] == 'C') && (bytes[n + 3] == 'o' || bytes[n + 3] == 'O') &&
               (bytes[n + 4] == 'd' || bytes[n + 4] == 'D') && (bytes[n + 5] == 'i' || bytes[n + 5] == 'I') &&
               (bytes[n + 6] == 'n' || bytes[n + 6] == 'N') && (bytes[n + 7] == 'g' || bytes[n + 7] == 'G') && bytes[n + 8] == '=';
    }

    /// <summary>
    /// Is charset marker
    /// </summary>
    /// <param name="bytes">The byte array</param>
    /// <param name="n">Location</param>
    /// <returns>Returns <i>true</i> if charset marker was found</returns>
    private static bool IsCharsetMarker(byte[] bytes, int n)
    {
        return (bytes[n + 0] == 'c' || bytes[n + 0] == 'C') && (bytes[n + 1] == 'h' || bytes[n + 1] == 'H') &&
               (bytes[n + 2] == 'a' || bytes[n + 2] == 'A') && (bytes[n + 3] == 'r' || bytes[n + 3] == 'R') &&
               (bytes[n + 4] == 's' || bytes[n + 4] == 'S') && (bytes[n + 5] == 'e' || bytes[n + 5] == 'E') &&
               (bytes[n + 6] == 't' || bytes[n + 6] == 'T') && bytes[n + 7] == '=';
    }

    /// <summary>
    /// Is charset name in range
    /// </summary>
    /// <param name="taster">The byte array</param>
    /// <param name="bytes">The byte array</param>
    /// <param name="n">Location</param>
    /// <returns>Returns <i>true</i> if charset name in range</returns>
    private static bool IsCharsetNameRange(int taster, byte[] bytes, int n)
    {
        return n < taster && (bytes[n] == '_' || bytes[n] == '-' || bytes[n] >= '0' && bytes[n] <= '9'
                              || bytes[n] >= 'a' && bytes[n] <= 'z' || bytes[n] >= 'A' && bytes[n] <= 'Z');
    }

    /// <summary>
    /// Get the encoding by byte order mark
    /// </summary>
    /// <param name="filename">The file to analyze</param>
    /// <param name="fallbackEncoding">The fallback encoding</param>
    /// <returns>Returns the encoding by bom or the fallback</returns>
    internal static Encoding? GetEncodingByBom(string filename, Encoding? fallbackEncoding = null)
    {
        using var file = new FileStream(filename, FileMode.Open, FileAccess.Read);
        return GetEncodingByBom(file, fallbackEncoding ?? FileEncoding.DefaultFallback);
    }

    /// <summary>
    /// Try to get the encoding by byte order mark
    /// </summary>
    /// <param name="fileStream">The file stream to read bytes from</param>
    /// <param name="fallbackEncoding">The fallback encoding</param>
    /// <returns>Returns the encoding by bom or the fallback</returns>
    private static Encoding? GetEncodingByBom(FileStream fileStream, Encoding? fallbackEncoding)
    {
        ArgumentNullException.ThrowIfNull(fileStream);

        var bom = new byte[4];
        fileStream.Position = 0;

        // ReSharper disable once MustUseReturnValue
        fileStream.Read(bom, 0, 4);

        return GetEncodingByBom(bom, fallbackEncoding, out _, false);
    }

    /// <summary>
    /// Try to get the encoding by byte order mark and provide text if available and needed
    /// </summary>
    /// <param name="bytes">File byte array</param>
    /// <param name="fallback">Fallback encoding</param>
    /// <param name="text">Text output if text should be provided</param>
    /// <param name="provideText">Boolean value to indicate if text should be provided</param>
    /// <returns>Returns the encoding by bom or the fallback</returns>
    private static Encoding? GetEncodingByBom(byte[] bytes, Encoding? fallback, out string? text, bool provideText)
    {
        foreach (var bom in ByteOrderMask.List)
        {
            if (SignatureMatch(bytes, bom.Signature))
            {
                return GetEncodingAndProvideText(bom, bytes, out text, provideText);
            }
        }

        text = provideText ? fallback?.GetString(bytes) : null;

        // We actually have no idea what the encoding is if we reach this point, so return default
        return fallback;
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
    /// Get Encoding and provide encoded text if wanted
    /// </summary>
    /// <param name="orderMaskInfo">The order mask information to use</param>
    /// <param name="bytes">The byte array</param>
    /// <param name="text">Text output</param>
    /// <param name="provideText">Flag if text should be provided to the text output</param>
    /// <returns></returns>
    private static Encoding GetEncodingAndProvideText(ByteOrderMaskInfo orderMaskInfo, byte[] bytes,
        out string? text, bool provideText)
    {
        if (orderMaskInfo?.Encoding == null)
        {
            throw new ArgumentException("Order mask encoding is null!");
        }

        text = provideText
            ? orderMaskInfo.Encoding.GetString(bytes, orderMaskInfo.SignatureLength(),
                bytes.Length - orderMaskInfo.SignatureLength())
            : null;

        return orderMaskInfo.Encoding;
    }

#pragma warning disable SYSLIB0001
    /// <summary>
    /// Validate Encoding Security for low hanging fruits<br /><br />
    /// <b>Currently handled cases:</b><br /><br />
    /// SYSLIB0001 UTF-7 encoding is not safe<br />
    /// https://docs.microsoft.com/de-de/dotnet/fundamentals/syslib-diagnostics/syslib0001
    /// </summary>
    /// <param name="encoding">The encoding to check</param>
    /// <exception cref="EncodingSecurityException">Will throw exceptions if a risk is detected</exception>
    internal static void ValidateEncodingSecurity(Encoding encoding)
    {
        if (Equals(encoding, Encoding.UTF7))
        {
            throw new EncodingSecurityException("SYSLIB0001: The UTF-7-encoding is not safe");
        }
    }
#pragma warning restore SYSLIB0001
}