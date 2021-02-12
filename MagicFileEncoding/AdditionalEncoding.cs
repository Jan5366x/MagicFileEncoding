using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace MagicFileEncoding
{  
    /// <summary>
    /// <para>List of additional encodings</para>
    /// Encoding will be <i>null</i> if required codepage can't be retrieved
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class AdditionalEncoding
    {
        /// <summary>
        /// <para>(Latin-1)</para>
        /// This character set contains the script-specific characters for Western European and American languages.
        /// The character set covers Albanian, Catalan, Danish, Dutch, English, Faroese, Finnish, French, Galician,
        /// Icelandic, German, Italian, Norwegian, Portuguese, Spanish and Swedish. Only single characters like the
        /// Dutch "ij" or the German quotation marks below are missing.
        /// </summary>
        public static readonly Encoding ISO_8859_1 = SoftFetchEncoding("iso-8859-1");
        
        /// <summary>
        /// <para>(Latin-2)</para>
        /// This character set contains the script-specific characters for most Central European and Slavic languages.
        /// The character set covers Croatian, Polish, Romanian, Slovak, Slovenian, Czech and Hungarian.
        /// </summary>
        public static readonly Encoding ISO_8859_2 = SoftFetchEncoding("iso-8859-2");
        
        /// <summary>
        /// <para>(Latin-3)</para>
        /// This character set covers the languages Esperanto, Galician, Maltese and Turkish.
        /// </summary>
        public static readonly Encoding ISO_8859_3 = SoftFetchEncoding("iso-8859-3");
        
        /// <summary>
        /// <para>(Latin-4)</para>
        /// This character set contains some characters of Estonian, Latvian and Lithuanian languages.
        /// Compare this character set also with ISO 8859-10, which is very similar.
        /// </summary>
        public static readonly Encoding ISO_8859_4 = SoftFetchEncoding("iso-8859-4");
        
        /// <summary>
        /// This character set contains Cyrillic characters.
        /// It largely covers the Bulgarian, Macedonian, Russian, Serbian and Ukrainian languages.
        /// </summary>
        public static readonly Encoding ISO_8859_5 = SoftFetchEncoding("iso-8859-5");
        
        /// <summary>
        /// This character set contains characters of Arabic script. However, the representation of the characters
        /// in the following table is "abstract" because the characters vary in writing practice depending on whether
        /// they are at the beginning, middle, or end of a word, or individually.
        /// Arabic is further characterized by the fact that the direction of writing is from right to left.
        /// </summary>
        public static readonly Encoding ISO_8859_6 = SoftFetchEncoding("iso-8859-6");
        
        /// <summary>
        /// This character set contains the characters of the Modern Greek script.
        /// </summary>
        public static readonly Encoding ISO_8859_7 = SoftFetchEncoding("iso-8859-7");
        
        /// <summary>
        /// This character set contains the characters of the Hebrew script.
        /// As with the Arabic script, the direction of writing is from right to left.
        /// </summary>
        public static readonly Encoding ISO_8859_8 = SoftFetchEncoding("iso-8859-8");
        
        /// <summary>
        /// <para>(Latin-5)</para>
        /// This character set is specially designed for Turkish. It is based on ISO 8859-1,
        /// but contains Turkish characters instead of the Icelandic special characters.
        /// </summary>
        public static readonly Encoding ISO_8859_9 = SoftFetchEncoding("iso-8859-9");
        
        /// <summary>
        /// <para>(Latin-6)</para>
        /// This character set specifically contains characters for
        /// the Greenlandic (Inuit) and Lappish (Sami) languages.
        /// </summary>
        public static readonly Encoding ISO_8859_10 = SoftFetchEncoding("iso-8859-10");
        
        /// <summary>
        /// UTF-32 always encodes a character in exactly 32 bits and is thus the simplest, since no variable character
        /// length is used and no intelligent algorithm is required, but at the expense of memory size
        /// if only characters of the ASCII character set are used, more than four times as much memory is required
        /// as with encoding in ASCII (7 bits required). Depending on the sequence of the bytes, whether the least
        /// significant byte or the most significant byte is transmitted first,
        /// one speaks of Little Endian (UTF-32LE) or <b>Big Endian (UTF-32BE)</b>. 
        /// </summary>
        public static readonly Encoding UTF32BE = SoftFetchEncoding("utf-32BE");

        /// <summary>
        /// Get the requested encoding and consume exception if it can't be found in code pages 
        /// </summary>
        /// <param name="encoding">The encoding name</param>
        /// <returns>The encoding object or <i>null</i></returns>
        private static Encoding SoftFetchEncoding(string encoding)
        {
            try
            {
               return Encoding.GetEncoding(encoding);
            }
            catch
            {
                return null;
            }
        }
    }
}