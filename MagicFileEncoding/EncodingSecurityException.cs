using System;

namespace MagicFileEncoding;

[Serializable]
public class EncodingSecurityException : Exception
{
    public EncodingSecurityException(string message) : base(message)
    {
    }
}