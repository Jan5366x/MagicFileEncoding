using System.Text;

namespace MagicFileEncoding.EncodingSet
{
    public class EncodingSetUtf32 : EncodingSet
    {
        public override Encoding GetEncoding()
        {
            return Encoding.UTF32;
        }
        
        public override int Order()
        {
            return 32;
        }
    }
}