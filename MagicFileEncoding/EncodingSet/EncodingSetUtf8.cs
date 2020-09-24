using System.Text;

namespace MagicFileEncoding.EncodingSet
{
    public class EncodingSetUtf8 : EncodingSet
    {
        public override Encoding GetEncoding()
        {
            return Encoding.UTF8;
        }
        
        public override int Order()
        {
            return 8;
        }
    }
}