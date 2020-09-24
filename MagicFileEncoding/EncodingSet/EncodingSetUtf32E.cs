using System.Text;

namespace MagicFileEncoding.EncodingSet
{
    public class EncodingSetUtf32E : EncodingSet
    {
        private Encoding utf32Encoding = new UTF32Encoding(true, true);

        public override Encoding GetEncoding()
        {
            return utf32Encoding;
        }
    }
}