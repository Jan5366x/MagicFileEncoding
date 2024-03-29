﻿using System.IO;

namespace MagicFileEncoding.Tools;

internal static class IoTools
{
    /// <summary>
    /// This method accepts two strings the represent two files to
    /// compare. A return value of 0 indicates that the contents of the files
    /// are the same. A return value of any other value indicates that the
    /// files are not the same.
    /// </summary>
    internal static bool FileCompare(string file1, string file2)
    {
        int file1Byte;
        int file2Byte;

        // Determine if the same file was referenced two times.
        if (file1 == file2)
            return true;

        // Open the two files.
        using var fs1 = new FileStream(file1, FileMode.Open);
        using var fs2 = new FileStream(file2, FileMode.Open);

        // Check the file sizes. If they are not the same, the files
        // are not the same.
        if (fs1.Length != fs2.Length)
            return false;

        // Read and compare a byte from each file until either a
        // non-matching set of bytes is found or until the end of
        // file1 is reached.
        do
        {
            // Read one byte from each file.
            file1Byte = fs1.ReadByte();
            file2Byte = fs2.ReadByte();
        } while (file1Byte == file2Byte && file1Byte != -1);

        // Return the success of the comparison. "file1byte" is
        // equal to "file2byte" at this point only if the files are
        // the same.
        return file1Byte - file2Byte == 0;
    }
}