using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

#pragma warning disable SYSLIB0001

namespace MagicFileEncoding;

[SuppressMessage("ReSharper", "UseUtf8StringLiteral")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public static class ByteOrderMask
{
    public static readonly ByteOrderMaskInfo UTF32BE
        =  new (AdditionalEncoding.UTF32BE!, 0x00, 0x00, 0xFE, 0xFF);
    public static readonly ByteOrderMaskInfo UTF32 
        = new (Encoding.UTF32, 0xFF, 0xFE, 0x00, 0x00);
    public static readonly ByteOrderMaskInfo UTF16BE
        = new (Encoding.BigEndianUnicode, 0xFE, 0xFF);
    public static readonly ByteOrderMaskInfo UTF16 
        = new (Encoding.Unicode, 0xFF, 0xFE);
    public static readonly ByteOrderMaskInfo UTF8
        = new (Encoding.UTF8, 0xEF, 0xBB, 0xBF);
    public static readonly ByteOrderMaskInfo UTF7
        = new (Encoding.UTF7, 0x2b, 0x2f, 0x76);
        
    public static readonly IList<ByteOrderMaskInfo> List = new List<ByteOrderMaskInfo>()
    {
        UTF32BE, UTF32, UTF16BE, UTF16, UTF8, UTF7
    }.AsReadOnly();
}
#pragma warning restore SYSLIB0001