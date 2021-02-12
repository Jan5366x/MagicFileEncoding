using System.Text;

namespace MagicFileEncoding
{
    public class ByteOrderMaskInfo
    {
        /// <summary>
        /// The encoding which is represented by this byte order mask
        /// </summary>
        public Encoding Encoding { get; }

        /// <summary>
        /// The byte order mask signature
        /// </summary>
        public byte[] Signature { get; }
        
        /// <summary>
        /// ByteOrderMaskInfo Constructor
        /// </summary>
        /// <param name="encoding">The encoding which is represented by this byte order mask</param>
        /// <param name="signature">The byte order mask signature</param>
        public ByteOrderMaskInfo(Encoding encoding, params byte[] signature)
        {
            Encoding = encoding;
            Signature = signature;
        }

        /// <summary>
        /// The byte order mask signature length
        /// </summary>
        /// <returns>Returns the BOM signature length</returns>
        public int SignatureLength() => Signature.Length;
    }
}