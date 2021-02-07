using System.Text;

namespace MagicFileEncoding
{
    public class ByteOrderMaskInfo
    {
        public Encoding Encoding { get; }

        public byte[] Mask { get; }
        
        public ByteOrderMaskInfo(Encoding encoding, params byte[] mask)
        {
            Encoding = encoding;
            Mask = mask;
        }

        public int MaskLength() => Mask.Length;
    }
}