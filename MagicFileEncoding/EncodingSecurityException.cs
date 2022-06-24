using System;

namespace MagicFileEncoding;

public class EncodingSecurityException : Exception
{
    public EncodingSecurityException(string message) : base(message)
    {
    }
}