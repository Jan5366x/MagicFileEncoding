using System;
using System.IO;
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

        [Test]
        public void TestUtf8Bom()
        {
            string filePath = TestContext.CurrentContext.WorkDirectory + "\\TestFiles\\A_UTF-8-BOM.txt";
            var acceptableEncoding = mfe.GetAcceptableEncoding(filePath);
            string text = mfe.AutomaticTransform(filePath,Encoding.UTF8);
            Assert.AreEqual("Kleiner Test äöüÄÖÜ?ß", text);
        }
        
        [Test]
        public void TestUtf8()
        {
            string filePath = TestContext.CurrentContext.WorkDirectory + "\\TestFiles\\A_UTF-8.txt";
            var acceptableEncoding = mfe.GetAcceptableEncoding(filePath);
            string text = mfe.AutomaticTransform(filePath,Encoding.UTF8);
            Assert.AreEqual("Kleiner Test äöüÄÖÜ?ß", text);
        }
        
        [Test]
        public void TestAnsi()
        {
            string filePath = TestContext.CurrentContext.WorkDirectory + "\\TestFiles\\A_ANSI.txt";
            var acceptableEncoding = mfe.GetAcceptableEncoding(filePath);
            string text = mfe.AutomaticTransform(filePath,Encoding.UTF8);
            Assert.AreEqual("Kleiner Test äöüÄÖÜ?ß", text);
        }
    }
}