using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace MagicFileEncoding
{  
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class AdditionalEncoding
    {
        public static readonly Encoding ISO_8859_1 = Encoding.GetEncoding("iso-8859-1");
        public static readonly Encoding UTF32BE = Encoding.GetEncoding("utf-32BE");
    }
}