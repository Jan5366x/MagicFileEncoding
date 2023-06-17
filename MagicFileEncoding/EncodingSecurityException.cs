using System;
using System.Runtime.Serialization;

namespace MagicFileEncoding;

[Serializable]
public class EncodingSecurityException : Exception
{
    public EncodingSecurityException(string message) : base(message)
    {
    }
}