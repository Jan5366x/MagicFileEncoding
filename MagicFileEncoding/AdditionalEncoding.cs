using System.Text;
// ReSharper disable InconsistentNaming

namespace MagicFileEncoding
{
    public static class AdditionalEncoding
    {
        public static readonly Encoding ISO_8859_1 = Encoding.GetEncoding("iso-8859-1");
        public static readonly Encoding UTF_32BE = Encoding.GetEncoding("utf-32BE");
    }
}