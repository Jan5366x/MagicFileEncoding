using System.Text;

namespace MagicFileEncoding
{
    public class ByteOrderMaskInfo
    {
        public Encoding Encoding { get; }

        public byte[] Signature { get; }
        
        public ByteOrderMaskInfo(Encoding encoding, params byte[] signature)
        {
            Encoding = encoding;
            Signature = signature;
        }

        public int SignatureLength() => Signature.Length;
    }
}