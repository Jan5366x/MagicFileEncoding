using System.Text;

namespace MagicFileEncoding.EncodingSet
{
    public abstract class EncodingSet
    {
        public bool Match(string filename)
        {
            return GetEncoding().Equals(MagicFileEncoding.GetEncodingByParsing(filename, GetEncoding()));
        }
        
        public abstract Encoding GetEncoding();
    }
}