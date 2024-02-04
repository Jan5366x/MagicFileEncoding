using System.IO;
using System.Text;
using MagicFileEncoding;
using MagicFileEncoding.Tools;
using NUnit.Framework;
using UnitTests.TestHelper;

namespace UnitTests;

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
        // Arrange
        var expectedResultPath = TestContext.CurrentContext.WorkDirectory + "/TestFiles/LargeFiles/L_UTF-8-BOM.txt"
            .Replace('/', Path.DirectorySeparatorChar);
        
        var filePath = TestContext.CurrentContext.WorkDirectory + subFilePath
            .Replace('/', Path.DirectorySeparatorChar);

        using var tmpFile = new TempFile();
        var text = FileEncoding.ReadAllText(filePath, Encoding.Unicode);
        
        // Act
        FileEncoding.WriteAllText(tmpFile.Path, Encoding.UTF8, text);
            

        // Assert
        Assert.That(IoTools.FileCompare(expectedResultPath, tmpFile.Path));
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
        // Arrange
        var expectedResultPath = TestContext.CurrentContext.WorkDirectory + "/TestFiles/LargeFiles/L_UTF-8-BOM.txt"
            .Replace('/', Path.DirectorySeparatorChar);
        
        var filePath = TestContext.CurrentContext.WorkDirectory + subFilePath
            .Replace('/', Path.DirectorySeparatorChar);

        using var tmpFile = new TempFile();
        var text = FileEncoding.ReadAllText(filePath, Encoding.UTF8);
        
        // Act
        FileEncoding.WriteAllText(tmpFile.Path, Encoding.UTF8, text);
            
        // Assert
        Assert.That(IoTools.FileCompare(expectedResultPath, tmpFile.Path));
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
        // Arrange
        var expectedResultPath = TestContext.CurrentContext.WorkDirectory + "/TestFiles/LargeFiles/L_ISO-8859-1.txt"
            .Replace('/', Path.DirectorySeparatorChar);
        
        var filePath = TestContext.CurrentContext.WorkDirectory + subFilePath
            .Replace('/', Path.DirectorySeparatorChar);

        using var tmpFile = new TempFile();
        var text = FileEncoding.ReadAllText(filePath, Encoding.Unicode);
        
        // Act
        FileEncoding.WriteAllText(tmpFile.Path, AdditionalEncoding.ISO_8859_1, text);
            
        // Assert
        Assert.That(IoTools.FileCompare(expectedResultPath, tmpFile.Path), Is.True);
    }
}