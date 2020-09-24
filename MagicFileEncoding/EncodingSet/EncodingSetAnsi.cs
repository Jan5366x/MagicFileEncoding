using System.Text;

namespace MagicFileEncoding.EncodingSet
{
    public class EncodingSetAnsi : EncodingSet
    {
        public override Encoding GetEncoding()
        {
            return CodePagesEncodingProvider.Instance.GetEncoding(1252);
        }

        public override int Order()
        {
            return 8;
        }
    }
}