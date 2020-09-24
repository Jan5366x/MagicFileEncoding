using System.Text;

namespace MagicFileEncoding.EncodingSet
{
    public class EncodingSetAscII : EncodingSet
    {
        public override Encoding GetEncoding()
        {
            return Encoding.ASCII;
        }
    }
}