﻿using System.Text;

namespace MagicFileEncoding.EncodingSet
{
    public class EncodingSetUtf7 : EncodingSet
    {
        public override Encoding GetEncoding()
        {
            return Encoding.UTF7;
        }
    }
}