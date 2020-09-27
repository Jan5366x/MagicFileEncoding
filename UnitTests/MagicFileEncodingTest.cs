using System.Text;
using NUnit.Framework;

namespace UnitTests
{
    public class Tests
    {
        private MagicFileEncoding.MagicFileEncoding mfe;
        
        [SetUp]
        public void Setup()
        {
            mfe = new MagicFileEncoding.MagicFileEncoding();
        }
        
        [TestCase("\\TestFiles\\A_ANSI.txt")]
        [TestCase("\\TestFiles\\A_UTF-8.txt")]
        [TestCase("\\TestFiles\\A_UTF-8-BOM.txt")]
        [TestCase("\\TestFiles\\A_ISO-8859-1.txt")]
        [TestCase("\\TestFiles\\A_Windows-1252.txt")]
        [TestCase("\\TestFiles\\A_UTF-16-LE-BOM.txt")]
        [TestCase("\\TestFiles\\A_UTF-16-BE-BOM.txt")]
        public void AutomaticReadAllText(string subFilePath)
        {
            var filePath = TestContext.CurrentContext.WorkDirectory + subFilePath;
            Assert.AreEqual("Kleiner Test äöüÄÖÜ?ß", mfe.AutomaticReadAllText(filePath, Encoding.Unicode));
        }
    }
}