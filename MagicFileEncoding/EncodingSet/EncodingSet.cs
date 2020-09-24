using System.Text;

namespace MagicFileEncoding.EncodingSet
{
    public interface EncodingSet
    {
        bool Match(string filename);
        bool MatchByBom(byte[] bom);
        
        Encoding GetEncoding();
    }
}