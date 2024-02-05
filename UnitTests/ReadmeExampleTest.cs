using System;
using System.IO;
using System.Text;
using MagicFileEncoding;
using NUnit.Framework;
using UnitTests.TestHelper;

namespace UnitTests;

[TestFixture]
public class ReadmeExampleTest
{
    /// <summary>
    /// Example 1: Getting the acceptable encoding of a file
    /// </summary>
    [Test]
    public void Example1()
    {
        // Arrange
        var filePath = TestContext.CurrentContext.WorkDirectory + "/TestFiles/SimpleFiles/A_UTF-8.txt"
            .Replace('/', Path.DirectorySeparatorChar);
        
        // Act
        // -> Readme Code
        //string filePath = "~/example.txt";
        Encoding fallbackEncoding = Encoding.UTF8;

        Encoding acceptableEncoding = FileEncoding.GetAcceptableEncoding(filePath, fallbackEncoding);
        
        Console.WriteLine("Acceptable encoding: " + acceptableEncoding.EncodingName);
        // <-
        
        // Assert
        Assert.That(acceptableEncoding.EncodingName, Is.EqualTo(Encoding.UTF8.EncodingName));
    }
    
    /// <summary>
    /// Example 2: Reading all text from a file using automatic encoding detection
    /// </summary>
    [Test]
    public void Example2()
    {
        // Arrange
        var filePath = TestContext.CurrentContext.WorkDirectory + "/TestFiles/SimpleFiles/A_UTF-8.txt"
            .Replace('/', Path.DirectorySeparatorChar);
        
        // Act
        // -> Readme Code
        //string filePath = "~/example.txt";

        string text = FileEncoding.ReadAllText(filePath);
        Console.WriteLine("Text: " + text);
        // <-
        
        // Assert
        Assert.That(text, Is.EqualTo("Kleiner Test äöüÄÖÜ?ß"));
    }
    
    /// <summary>
    /// Example 3: Reading all text from a file and transforming it into a target encoding
    /// </summary>
    [Test]
    public void Example3()
    {
        // Arrange
        var filePath = TestContext.CurrentContext.WorkDirectory + "/TestFiles/SimpleFiles/A_ANSI.txt"
            .Replace('/', Path.DirectorySeparatorChar);

        // Act
        // -> Readme Code
        // string filePath = "~/example.txt";
        Encoding targetEncoding = Encoding.UTF8;
        Encoding fallbackEncoding = Encoding.GetEncoding("ISO-8859-1");

        string text = FileEncoding.ReadAllText(filePath, targetEncoding, fallbackEncoding);
        Console.WriteLine("Text: " + text);
        // <-
        
        // Assert
        Assert.That(text, Is.EqualTo("Kleiner Test äöüÄÖÜ?ß"));
    }
    
    /// <summary>
    /// Example 4: Writing text to a file in a specific encoding
    /// </summary>
    [Test]
    public void Example4()
    {
        // Arrange
        using var tmpFile = new TempFile();
        var filePath = tmpFile.Path;
        
        // Act
        // -> Readme Code
        //string filePath = "~/output.txt";
        Encoding targetEncoding = Encoding.Unicode;
        string text = "\u2387 Hello, world!";

        FileEncoding.WriteAllText(filePath, targetEncoding, text);
        Console.WriteLine("Text written to file.");
        // <-
        
        // Assert
        Assert.That(FileEncoding.GetAcceptableEncoding(filePath).EncodingName, Is.EqualTo(Encoding.Unicode.EncodingName));
    }
    
    /// <summary>
    /// Example 5: Providing writer access to a file in a specific encoding
    /// </summary>
    [Test]
    public void Example5()
    {
        // Arrange
        using var tmpFile = new TempFile();
        var filePath = tmpFile.Path;
        var expectedResult = "Line 1" + Environment.NewLine + "Line 2" + Environment.NewLine + "Line 3" +
                             Environment.NewLine;
        
        // Act
        // -> Readme Code
        // string filePath = "~/output.txt";
        Encoding targetEncoding = Encoding.UTF8;

        FileEncoding.Write(filePath, targetEncoding, writer =>
        {
            writer.WriteLine("Line 1");
            writer.WriteLine("Line 2");
            writer.WriteLine("Line 3");
        });

        Console.WriteLine("Text written to file.");
        // <-
        
        // Assert
        Assert.That(FileEncoding.ReadAllText(filePath, Encoding.UTF8), Is.EqualTo(expectedResult));
    }
    
    /// <summary>
    /// Example 6: Getting the acceptable encoding of a byte array
    /// </summary>
    [Test]
    public void Example6()
    {
        // Arrange
        var filePath = TestContext.CurrentContext.WorkDirectory + "/TestFiles/SimpleFiles/A_UTF-8.txt"
            .Replace('/', Path.DirectorySeparatorChar);

        // Act
        // -> Readme Code
        // string filePath = "~/example.txt";
        byte[] bytes = File.ReadAllBytes(filePath);
        Encoding fallbackEncoding = Encoding.UTF8;

        Encoding acceptableEncoding = FileEncoding.GetAcceptableEncoding(bytes, fallbackEncoding);
        Console.WriteLine("Acceptable encoding: " + acceptableEncoding.EncodingName);
        // <-
        
        // Assert
        Assert.That(acceptableEncoding.EncodingName, Is.EqualTo(Encoding.UTF8.EncodingName));
    }
    
    /// <summary>
    /// Example 7: Reading all text from a byte array using automatic encoding detection
    /// </summary>
    [Test]
    public void Example7()
    {
        // Arrange
        var filePath = TestContext.CurrentContext.WorkDirectory + "/TestFiles/SimpleFiles/A_UTF-8.txt"
            .Replace('/', Path.DirectorySeparatorChar);
        
        // Act
        // -> Readme Code
        // string filePath = "~/example.txt";
        byte[] bytes = File.ReadAllBytes(filePath);

        string text = FileEncoding.ReadAllBytes(bytes);
        Console.WriteLine("Text: " + text);
        // <-
        
        // Assert
        Assert.That(text, Is.EqualTo("Kleiner Test äöüÄÖÜ?ß"));
    }
    
    /// <summary>
    ///  Example 8: Reading all text from a byte array and transforming it into a target encoding
    /// </summary>
    [Test]
    public void Example8()
    {
        // Arrange
        var filePath = TestContext.CurrentContext.WorkDirectory + "/TestFiles/SimpleFiles/A_ANSI.txt"
            .Replace('/', Path.DirectorySeparatorChar);

        
        // Act
        // -> Readme Code
        // string filePath = "~/example.txt";
        byte[] bytes = File.ReadAllBytes(filePath);
        Encoding targetEncoding = Encoding.UTF8;
        Encoding fallbackEncoding = Encoding.GetEncoding("ISO-8859-1");

        string text = FileEncoding.ReadAllBytes(bytes, targetEncoding, fallbackEncoding);
        Console.WriteLine("Text: " + text);
        // <-
        
        // Assert
        Assert.That(text, Is.EqualTo("Kleiner Test äöüÄÖÜ?ß"));
    }

}