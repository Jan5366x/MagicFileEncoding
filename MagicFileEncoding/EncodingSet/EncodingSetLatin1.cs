using System.Text;

namespace MagicFileEncoding.EncodingSet
{
    public class EncodingSetLatin1 : EncodingSet
    {
        public override Encoding GetEncoding()
        {
            return Encoding.GetEncoding("iso-8859-1");
        }
    }
}