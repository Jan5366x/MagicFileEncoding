# Magic File Encoding

The Magic File Encoding lib helps to load and transform simple and closed scope char set text files (like EDIFACT).

Be aware of possible transformation issues if the target encoding is simpler than the source encoding.

It is strongly recommended to write unit tests for your use case to ensure the load and transformation works as expected.

## Nuget Package
[MagicFileEncoding at nuget.org](https://www.nuget.org/packages/MagicFileEncoding/)

## Usage

#### Read a text file
```csharp
var text = FileEncoding.ReadAllText(filePath);
// or 
var text = FileEncoding.ReadAllText(filePath, Encoding.Unicode);
```
#### Write a text file
```csharp
FileEncoding.WriteAllText(tmpFile.Path, text, Encoding.UTF8);
```
#### Just detect suitable encoding
```csharp
var encoding = FileEncoding.GetAcceptableEncoding(filename);
```

## Fallback Encoding
The fallback encoding is ISO-8859-1 (Latin-1) by default but can be changed via optional method argument.

(Because this lib was designed for the german culture space)

## Credits
This work is heavily based on the following stack overflow and web articles:<br />
[determine-a-strings-encoding-in-c-sharp](https://stackoverflow.com/questions/1025332/determine-a-strings-encoding-in-c-sharp) <br />
[check-for-invalid-utf8](https://stackoverflow.com/questions/6555015/check-for-invalid-utf8) <br />
[how-to-detect-utf-8-in-plain-c](https://stackoverflow.com/questions/1031645/how-to-detect-utf-8-in-plain-c) <br />
[strip-byte-order-mark-from-string-in-c-sharp](https://stackoverflow.com/questions/1317700/strip-byte-order-mark-from-string-in-c-sharp) <br />
[what-is-the-most-common-encoding-of-each-language](https://stackoverflow.com/questions/8509339/what-is-the-most-common-encoding-of-each-language) <br />
[utf-bom4](http://www.unicode.org/faq/utf_bom.html#bom4) 