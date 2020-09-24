using System.Text;

namespace MagicFileEncoding.EncodingSet
{
    public class EncodingSetUtf7 : EncodingSet
    {
        public bool Match(string filename)
        {
            throw new System.NotImplementedException();
        }

        public bool MatchByBom(byte[] bom)
        {
            throw new System.NotImplementedException();
        }

        public Encoding GetEncoding()
        {
            throw new System.NotImplementedException();
        }
    }
}