using System.IO;
using System.Text;
using MagicFileEncoding;
using MagicFileEncoding.Tools;
using NUnit.Framework;
using UnitTests.TestHelper;

namespace UnitTests;

[TestFixture]
public class SimpleFilesTest
{
    [TestCase("/TestFiles/SimpleFiles/A_ANSI.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_UTF-8.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_UTF-8-BOM.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_ISO-8859-1.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_Windows-1252.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_UTF-16-LE-BOM.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_UTF-16-BE-BOM.txt")]
    public void AutomaticReadAllText(string subFilePath)
    {
        // Arrange
        var filePath = TestContext.CurrentContext.WorkDirectory + subFilePath
            .Replace('/', Path.DirectorySeparatorChar);
        
        // Act
        var result = FileEncoding.ReadAllText(filePath, Encoding.Unicode);
        
        // Assert
        Assert.That(result, Is.EqualTo("Kleiner Test äöüÄÖÜ?ß"));
    }

    [TestCase("/TestFiles/SimpleFiles/A_ANSI.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_UTF-8.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_UTF-8-BOM.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_ISO-8859-1.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_Windows-1252.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_UTF-16-LE-BOM.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_UTF-16-BE-BOM.txt")]
    public void LoadAsUnicodeSaveToUtf8(string subFilePath)
    {
        // Arrange
        var expectedResultPath = TestContext.CurrentContext.WorkDirectory + "/TestFiles/SimpleFiles/A_UTF-8-BOM.txt"
            .Replace('/', Path.DirectorySeparatorChar);
        
        var filePath = TestContext.CurrentContext.WorkDirectory + subFilePath
            .Replace('/', Path.DirectorySeparatorChar);

        using var tmpFile = new TempFile();
        
        // Act
        var text = FileEncoding.ReadAllText(filePath, Encoding.Unicode);
        FileEncoding.WriteAllText(tmpFile.Path, Encoding.UTF8, text);
            
        // Assert
        Assert.That(IoTools.FileCompare(expectedResultPath, tmpFile.Path));
    }
        
    [TestCase("/TestFiles/SimpleFiles/A_ANSI.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_UTF-8.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_UTF-8-BOM.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_ISO-8859-1.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_Windows-1252.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_UTF-16-LE-BOM.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_UTF-16-BE-BOM.txt")]
    public void LoadAsUtf8SaveToUtf8(string subFilePath)
    {
        // Arrange
        var expectedResultPath = TestContext.CurrentContext.WorkDirectory + "/TestFiles/SimpleFiles/A_UTF-8-BOM.txt"
            .Replace('/', Path.DirectorySeparatorChar);
        
        var filePath = TestContext.CurrentContext.WorkDirectory + subFilePath
            .Replace('/', Path.DirectorySeparatorChar);

        using var tmpFile = new TempFile();
        
        // Act
        var text = FileEncoding.ReadAllText(filePath, Encoding.UTF8);
        FileEncoding.WriteAllText(tmpFile.Path, Encoding.UTF8, text);
            
        // Assert
        Assert.That(IoTools.FileCompare(expectedResultPath, tmpFile.Path));
    }
        
    [TestCase("/TestFiles/SimpleFiles/A_ANSI.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_UTF-8.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_UTF-8-BOM.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_ISO-8859-1.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_Windows-1252.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_UTF-16-LE-BOM.txt")]
    [TestCase("/TestFiles/SimpleFiles/A_UTF-16-BE-BOM.txt")]
    public void LoadAsUnicodeSaveToISO_8859_1(string subFilePath)
    {
        // Arrange
        var expectedResultPath = TestContext.CurrentContext.WorkDirectory + "/TestFiles/SimpleFiles/A_ISO-8859-1.txt"
            .Replace('/', Path.DirectorySeparatorChar);
        
        var filePath = TestContext.CurrentContext.WorkDirectory + subFilePath
            .Replace('/', Path.DirectorySeparatorChar);

        using var tmpFile = new TempFile();
        
        // Act
        var text = FileEncoding.ReadAllText(filePath, Encoding.Unicode);
        FileEncoding.WriteAllText(tmpFile.Path, AdditionalEncoding.ISO_8859_1, text);
            
        // Assert
        Assert.That(IoTools.FileCompare(expectedResultPath, tmpFile.Path));
    }
}