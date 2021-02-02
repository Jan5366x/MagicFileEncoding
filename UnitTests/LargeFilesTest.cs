using System.IO;
using System.Text;
using MagicFileEncoding;
using MagicFileEncoding.Tools;
using NUnit.Framework;
using UnitTests.TestHelper;

namespace UnitTests
{
    [TestFixture]
    public class LargeFilesTest
    {
        [TestCase("/TestFiles/LargeFiles/L_ANSI.txt")]
        [TestCase("/TestFiles/LargeFiles/L_UTF-8.txt")]
        [TestCase("/TestFiles/LargeFiles/L_UTF-8-BOM.txt")]
        [TestCase("/TestFiles/LargeFiles/L_ISO-8859-1.txt")]
        [TestCase("/TestFiles/LargeFiles/L_Windows-1252.txt")]
        [TestCase("/TestFiles/LargeFiles/L_UTF-16-LE-BOM.txt")]
        [TestCase("/TestFiles/LargeFiles/L_UTF-16-BE-BOM.txt")]
        public void LoadAsUnicodeSaveToUtf8(string subFilePath)
        {
            var filePath = TestContext.CurrentContext.WorkDirectory + subFilePath
                .Replace('/', Path.DirectorySeparatorChar);

            using var tmpFile = new TempFile();
            
            var text = FileEncoding.ReadAllText(filePath, Encoding.Unicode);
            
            FileEncoding.WriteAllText(tmpFile.Path, text, Encoding.UTF8);
            
            var expectedResultPath = TestContext.CurrentContext.WorkDirectory + "/TestFiles/LargeFiles/L_UTF-8-BOM.txt"
                .Replace('/', Path.DirectorySeparatorChar);
            
            Assert.IsTrue(IOTools.FileCompare(expectedResultPath, tmpFile.Path));
        }
        
        [TestCase("/TestFiles/LargeFiles/L_ANSI.txt")]
        [TestCase("/TestFiles/LargeFiles/L_UTF-8.txt")]
        [TestCase("/TestFiles/LargeFiles/L_UTF-8-BOM.txt")]
        [TestCase("/TestFiles/LargeFiles/L_ISO-8859-1.txt")]
        [TestCase("/TestFiles/LargeFiles/L_Windows-1252.txt")]
        [TestCase("/TestFiles/LargeFiles/L_UTF-16-LE-BOM.txt")]
        [TestCase("/TestFiles/LargeFiles/L_UTF-16-BE-BOM.txt")]
        public void LoadAsUtf8SaveToUtf8(string subFilePath)
        {
            var filePath = TestContext.CurrentContext.WorkDirectory + subFilePath
                .Replace('/', Path.DirectorySeparatorChar);

            using var tmpFile = new TempFile();
            
            var text = FileEncoding.ReadAllText(filePath, Encoding.UTF8);
            
            FileEncoding.WriteAllText(tmpFile.Path, text, Encoding.UTF8);
            
            var expectedResultPath = TestContext.CurrentContext.WorkDirectory + "/TestFiles/LargeFiles/L_UTF-8-BOM.txt"
                .Replace('/', Path.DirectorySeparatorChar);
            
            Assert.IsTrue(IOTools.FileCompare(expectedResultPath, tmpFile.Path));
        }
        
        [TestCase("/TestFiles/LargeFiles/L_ANSI.txt")]
        [TestCase("/TestFiles/LargeFiles/L_UTF-8.txt")]
        [TestCase("/TestFiles/LargeFiles/L_UTF-8-BOM.txt")]
        [TestCase("/TestFiles/LargeFiles/L_ISO-8859-1.txt")]
        [TestCase("/TestFiles/LargeFiles/L_Windows-1252.txt")]
        [TestCase("/TestFiles/LargeFiles/L_UTF-16-LE-BOM.txt")]
        [TestCase("/TestFiles/LargeFiles/L_UTF-16-BE-BOM.txt")]
        public void LoadAsUnicodeSaveToISO_8859_1(string subFilePath)
        {
            var filePath = TestContext.CurrentContext.WorkDirectory + subFilePath
                .Replace('/', Path.DirectorySeparatorChar);

            using var tmpFile = new TempFile();
            
            var text = FileEncoding.ReadAllText(filePath, Encoding.Unicode);
            
            FileEncoding.WriteAllText(tmpFile.Path, text, AdditionalEncoding.ISO_8859_1);
            
            var expectedResultPath = TestContext.CurrentContext.WorkDirectory + "/TestFiles/LargeFiles/L_ISO-8859-1.txt"
                .Replace('/', Path.DirectorySeparatorChar);
            
            Assert.IsTrue(IOTools.FileCompare(expectedResultPath, tmpFile.Path));
        }
    }
}