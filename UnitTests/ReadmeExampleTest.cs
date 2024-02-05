using System;
using System.IO;
using System.Text;
using MagicFileEncoding;
using NUnit.Framework;

namespace UnitTests;

[TestFixture]
public class ReadmeExampleTest
{
    // TODO adjust files and assert result
    
    [Test]
    public void Example1()
    {
        // Arrange
        
        // Act
        // -> Readme Code
        string filename = "example.txt";
        Encoding fallbackEncoding = Encoding.UTF8;

        Encoding acceptableEncoding = FileEncoding.GetAcceptableEncoding(filename, fallbackEncoding);
        Console.WriteLine("Acceptable encoding: " + acceptableEncoding.EncodingName);
        // <-
        
        // Assert
    }
    
    [Test]
    public void Example2()
    {
        // Arrange
        
        // Act
        // -> Readme Code
        string filename = "example.txt";

        string text = FileEncoding.ReadAllText(filename);
        Console.WriteLine("Text: " + text);
        // <-
        
        // Assert
    }
    
    [Test]
    public void Example3()
    {
        // Arrange
        
        // Act
        // -> Readme Code
        string filename = "example.txt";
        Encoding targetEncoding = Encoding.UTF8;
        Encoding fallbackEncoding = Encoding.GetEncoding("ISO-8859-1");

        string text = FileEncoding.ReadAllText(filename, targetEncoding, fallbackEncoding);
        Console.WriteLine("Text: " + text);
        // <-
        
        // Assert
    }
    
    [Test]
    public void Example4()
    {
        // Arrange
        
        // Act
        // -> Readme Code
        string path = "output.txt";
        Encoding targetEncoding = Encoding.UTF8;
        string text = "Hello, world!";

        FileEncoding.WriteAllText(path, targetEncoding, text);
        Console.WriteLine("Text written to file.");
        // <-
        
        // Assert
    }
    
    [Test]
    public void Example5()
    {
        // Arrange
        
        // Act
        // -> Readme Code
        string path = "output.txt";
        Encoding targetEncoding = Encoding.UTF8;

        FileEncoding.Write(path, targetEncoding, writer =>
        {
            writer.WriteLine("Line 1");
            writer.WriteLine("Line 2");
            writer.WriteLine("Line 3");
        });

        Console.WriteLine("Text written to file.");
        // <-
        
        // Assert
    }
    
    [Test]
    public void Example6()
    {
        // Arrange
        
        // Act
        // -> Readme Code
        byte[] bytes = File.ReadAllBytes("example.txt");
        Encoding fallbackEncoding = Encoding.UTF8;

        Encoding acceptableEncoding = FileEncoding.GetAcceptableEncoding(bytes, fallbackEncoding);
        Console.WriteLine("Acceptable encoding: " + acceptableEncoding.EncodingName);
        // <-
        
        // Assert
    }
    
    [Test]
    public void Example7()
    {
        // Arrange
        
        // Act
        // -> Readme Code
        byte[] bytes = File.ReadAllBytes("example.txt");

        string text = FileEncoding.ReadAllBytes(bytes);
        Console.WriteLine("Text: " + text);
        // <-
        
        // Assert
    }
    
    [Test]
    public void Example8()
    {
        // Arrange
        
        // Act
        // -> Readme Code
        byte[] bytes = File.ReadAllBytes("example.txt");
        Encoding targetEncoding = Encoding.UTF8;
        Encoding fallbackEncoding = Encoding.GetEncoding("ISO-8859-1");

        string text = FileEncoding.ReadAllBytes(bytes, targetEncoding, fallbackEncoding);
        Console.WriteLine("Text: " + text);
        // <-
        
        // Assert
    }

}