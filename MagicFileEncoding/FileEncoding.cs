using System;
using System.IO;
using System.Text;
using static MagicFileEncoding.Tools.EncodingTools;

namespace MagicFileEncoding;

/// <summary>
/// Magic File Encoding Static Class
/// </summary>
public static class FileEncoding
{
    /// <summary>
    /// The fallback encoding<br />
    /// <i>ISO-8859-1 (Latin-1) if available or UTF8 by default</i>
    /// </summary>
    internal static readonly Encoding DefaultFallback = AdditionalEncoding.ISO_8859_1 ?? Encoding.UTF8;
        
    /// <summary>
    /// Find a acceptable encoding to open a given file
    /// </summary>
    /// <param name="filename">The file to check</param>
    /// <param name="fallbackEncoding">The fallback encoding (ISO-8859-1 by default)</param>
    /// <returns>Best suitable encoding</returns>
    public static Encoding GetAcceptableEncoding(string filename, Encoding? fallbackEncoding = null) 
        => DetectTextEncoding(filename, out _, false) ?? fallbackEncoding ?? DefaultFallback;

    /// <summary>
    /// Automatic detect acceptable encoding and read all text from a given file and transform it into
    /// Unicode UTF16 encoding
    /// </summary>
    /// <param name="filename">The file to read text</param>
    /// <returns>Returns the text</returns>
    public static string ReadAllText(string filename) 
        => Encoding.Unicode.GetString(AutomaticTransformBytes(filename, Encoding.Unicode));

    /// <summary>
    /// Automatic detect acceptable encoding and read all text from a given file and transform it into
    /// a given target encoding
    /// </summary>
    /// <param name="filename">The file to read text</param>
    /// <param name="targetEncoding">The target encoding to transform to the return value</param>
    /// <param name="fallbackEncoding">Fallback encoding for the input</param>
    /// <returns>Returns the text</returns>
    public static string ReadAllText(string filename, Encoding targetEncoding, Encoding? fallbackEncoding = null)
        => targetEncoding
            .GetString(AutomaticTransformBytes(filename, targetEncoding, fallbackEncoding))
            .Trim(new[]{'\uFEFF'});

    /// <summary>
    /// Write all text to a given file in a specific encoding
    /// </summary>
    /// <param name="path">The path to the text file</param>
    /// <param name="text">Text to encode and write to file</param>
    /// <param name="targetEncoding">Target encoding</param>
    public static void WriteAllText(string path, Encoding? targetEncoding, string text) 
        => Write(path, targetEncoding, writer => writer.Write(text));
    
    /// <summary>
    /// Gives writer access to a given file in a specific encoding
    /// </summary>
    /// <param name="path">The path to the text file</param>
    /// <param name="targetEncoding">Target encoding</param>
    /// <param name="writerAction">Access to the writer (using/closing is handled)</param>
    /// <exception cref="ArgumentNullException">If path or target encoding is null</exception>
    /// <exception cref="ArgumentException">If a empty path is supplied</exception>
    public static void Write(string path, Encoding? targetEncoding, Action<StreamWriter> writerAction)
    {
        if (path == null)
            throw new ArgumentNullException(nameof(path));
        if (path.Trim().Length == 0)
            throw new ArgumentException("Can't write file because path is empty!", nameof(path));
        if (targetEncoding == null)
            throw new ArgumentNullException(nameof(targetEncoding));
        
        using var sw = new StreamWriter(path, false, targetEncoding);
        writerAction.Invoke(sw);
    }
}