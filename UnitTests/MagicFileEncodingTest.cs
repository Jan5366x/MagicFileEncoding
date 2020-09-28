using System.IO;
using System.Text;
using MagicFileEncoding;
using NUnit.Framework;
using UnitTests.TestHelper;

namespace UnitTests
{
    public class Tests
    {
        private MagicFileEncoding.MagicFileEncoding _mfe;
        
        [SetUp]
        public void Setup()
        {
            _mfe = new MagicFileEncoding.MagicFileEncoding();
        }
        
        [TestCase("/TestFiles/A_ANSI.txt")]
        [TestCase("/TestFiles/A_UTF-8.txt")]
        [TestCase("/TestFiles/A_UTF-8-BOM.txt")]
        [TestCase("/TestFiles/A_ISO-8859-1.txt")]
        [TestCase("/TestFiles/A_Windows-1252.txt")]
        [TestCase("/TestFiles/A_UTF-16-LE-BOM.txt")]
        [TestCase("/TestFiles/A_UTF-16-BE-BOM.txt")]
        public void AutomaticReadAllText(string subFilePath)
        {
            var filePath = TestContext.CurrentContext.WorkDirectory + subFilePath
                .Replace('/', Path.DirectorySeparatorChar);
            
            Assert.AreEqual("Kleiner Test äöüÄÖÜ?ß", _mfe.AutomaticReadAllText(filePath, Encoding.Unicode));
        }

        [Test]
        public void LoadAndSaveTextAnsiToUnicodeSaveToUtf8()
        {
            var filePath = TestContext.CurrentContext.WorkDirectory + "/TestFiles/A_ANSI.txt"
                .Replace('/', Path.DirectorySeparatorChar);

            using var tmpFile = new TempFile();
            
            var text = _mfe.AutomaticReadAllText(filePath, Encoding.Unicode);
            
            _mfe.WriteAllText(tmpFile.Path, text, Encoding.UTF8);
            
            var expectedResultPath = TestContext.CurrentContext.WorkDirectory + "/TestFiles/A_UTF-8-BOM.txt"
                .Replace('/', Path.DirectorySeparatorChar);
            
            Assert.IsTrue(IOTools.FileCompare(expectedResultPath, tmpFile.Path));
        }
    }
}