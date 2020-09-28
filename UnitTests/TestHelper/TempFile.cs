using System;
using System.IO;

namespace UnitTests.TestHelper
{
    public class TempFile : IDisposable
    {
        private string _path;

        public TempFile()
        {
            _path = System.IO.Path.GetTempFileName();
        }

        public string Path => _path;
        
        public void Dispose()
        {
            if(File.Exists(_path))
            {
                File.Delete(_path);
            }
        }
    }
}