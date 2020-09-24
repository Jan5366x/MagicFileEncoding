using System.Text;

namespace MagicFileEncoding.EncodingSet
{
    public abstract class EncodingSet
    {
        public bool IsAcceptable(string filename)
        {
            return MagicFileEncoding.GetEncodingByParsing(filename, GetEncoding()) != null;
        }
        
        public abstract Encoding GetEncoding();

        public abstract int Order();
    }
}