using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MagicFileEncoding
{
    public class MagicFileEncoding
    {
        List<EncodingSet.EncodingSet> encodingSets = new List<EncodingSet.EncodingSet>();
        
        public MagicFileEncoding()
        {
            SetupEncodingSets();

            // TODO remove test output
            encodingSets.ForEach(x => Console.WriteLine(x) );
        }

        private void SetupEncodingSets()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(type => type.IsClass
                               && !type.IsAbstract
                               && typeof(EncodingSet.EncodingSet).IsAssignableFrom(type));

            foreach (var type in types)
            {
                encodingSets.Add((EncodingSet.EncodingSet) Activator.CreateInstance(type));
            }
        }

        // https://stackoverflow.com/questions/3825390/effective-way-to-find-any-files-encoding
        public Encoding getEncoding(string filename)
        {
            var encodingByBom = GetEncodingByBom(filename);
            if (encodingByBom != null)
                return encodingByBom;

            // BOM not found :(, so try to parse characters into several encodings
            var encodingByParsingUtf8 = GetEncodingByParsing(filename, Encoding.UTF8);
            if (encodingByParsingUtf8 != null)
                return encodingByParsingUtf8;

            var encodingByParsingLatin1 = GetEncodingByParsing(filename, Encoding.GetEncoding("iso-8859-1"));
            if (encodingByParsingLatin1 != null)
                return encodingByParsingLatin1;

            var encodingByParsingUTF7 = GetEncodingByParsing(filename, Encoding.UTF7);
            if (encodingByParsingUTF7 != null)
                return encodingByParsingUTF7;

            return null;   // no encoding found
        }


        // For netcore we use UTF8 as default encoding since ANSI isn't available
        public static Encoding GetEncodingByBom(string filename)
        {
            return GetEncodingByBom(filename, Encoding.Default);
        }

        public static Encoding GetEncodingByBom(string filename, Encoding defaultEncoding)
        {
            // Read the BOM
            var bom = new byte[4];
            
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
                file.Read(bom, 0, 4);
            
            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return Encoding.UTF32; //UTF-32LE
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return new UTF32Encoding(true, true);  //UTF-32BE

          
            // We actually have no idea what the encoding is if we reach this point, so return default
            return defaultEncoding;
        }
        
        

        private static Encoding GetEncodingByParsing(string filename, Encoding encoding)
        {            
            var encodingVerifier = Encoding.GetEncoding(encoding.BodyName, new EncoderExceptionFallback(), new DecoderExceptionFallback());
            try
            {
                using var textReader = new StreamReader(filename, encodingVerifier, detectEncodingFromByteOrderMarks: true);
                while (!textReader.EndOfStream)
                    textReader.ReadLine(); // in order to increment the stream position

                // all text parsed ok
                return textReader.CurrentEncoding;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }


  
}
