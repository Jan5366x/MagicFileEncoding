using System.Text;

namespace MagicFileEncoding.EncodingSet
{
    public abstract class EncodingSet
    {
        public bool IsAcceptable(MagicFileEncoding mfe, string filename)
        {
            return mfe.GetEncodingByParsing(filename, GetEncoding()) != null;
        }
        
        public abstract Encoding GetEncoding();

        public abstract int Order();
    }
}