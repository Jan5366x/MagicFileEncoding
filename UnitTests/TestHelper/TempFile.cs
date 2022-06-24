using System;
using System.IO;

namespace UnitTests.TestHelper;

public sealed class TempFile : IDisposable
{
    public TempFile()
    {
        Path = System.IO.Path.GetTempFileName();
    }

    public string Path { get; }

    void IDisposable.Dispose()
    {
        if(File.Exists(Path))
        {
            File.Delete(Path);
        }
    }
}