using System.Text;

namespace MagicFileEncoding.EncodingSet
{
    public class EncodingSetBigEndianUnicode : EncodingSet
    {
        public override Encoding GetEncoding()
        {
            return Encoding.BigEndianUnicode;
        }
        
        public override int Order()
        {
            return 16;
        }
    }
}