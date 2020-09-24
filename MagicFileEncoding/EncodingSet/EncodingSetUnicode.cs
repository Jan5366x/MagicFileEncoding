using System.Text;

namespace MagicFileEncoding.EncodingSet
{
    public class EncodingSetUnicode : EncodingSet
    {
        public override Encoding GetEncoding()
        {
            return Encoding.Unicode;
        }
        
        public override int Order()
        {
            return 16;
        }
    }
}