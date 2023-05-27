# Magic File Encoding

[![NuGet Downloads](https://img.shields.io/nuget/dt/MagicFileEncoding.svg)](https://www.nuget.org/packages/MagicFileEncoding/)
[![Actions Status](https://github.com/Jan5366x/MagicFileEncoding/workflows/Build%20and%20Test/badge.svg)](https://github.com/Jan5366x/MagicFileEncoding/actions)

The Magic File Encoding Library is a powerful tool designed to assist you in loading and transforming simple and closed scope
character set text files. Whether you're working with EDIFACT files or similar text formats, this library provides a
comprehensive solution to handle various encoding scenarios effortlessly.

## Transformation Considerations
When performing encoding transformations, it is important to be mindful of potential issues
that may arise if the target encoding is simpler than the source encoding. Certain characters or language-specific symbols
in the source encoding may not be accurately represented or fully preserved in the target encoding.

> Thorough testing and validation are recommended to ensure the desired outcome during the transformation process.

## Fallback Encoding
The Magic File Encoding Library incorporates a fallback encoding system. By default, it uses ISO-8859-1 (Latin-1) as the fallback encoding.
This fallback encoding is specifically designed to cater to the encoding requirements within the German cultural space.

> However, it provides the flexibility to modify the fallback encoding through an optional method argument,
enabling adaptation to different encoding needs like UTF-8 fallbacks.

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

## Nuget Package
[MagicFileEncoding at nuget.org](https://www.nuget.org/packages/MagicFileEncoding/)

## Versioning & Breaking Changes

> Major.Minor.Patch-Suffix

* Major: **Breaking changes**
* Minor: New features, but backwards compatible
* Patch: Backwards compatible bug fixes only
* -Suffix (optional): a hyphen followed by a string denoting a pre-release version

See: https://docs.microsoft.com/en-us/nuget/concepts/package-versioning

## Credits
This work is heavily based on the following stack overflow and web articles:<br />
[determine-a-strings-encoding-in-c-sharp](https://stackoverflow.com/questions/1025332/determine-a-strings-encoding-in-c-sharp) <br />
[check-for-invalid-utf8](https://stackoverflow.com/questions/6555015/check-for-invalid-utf8) <br />
[how-to-detect-utf-8-in-plain-c](https://stackoverflow.com/questions/1031645/how-to-detect-utf-8-in-plain-c) <br />
[strip-byte-order-mark-from-string-in-c-sharp](https://stackoverflow.com/questions/1317700/strip-byte-order-mark-from-string-in-c-sharp) <br />
[what-is-the-most-common-encoding-of-each-language](https://stackoverflow.com/questions/8509339/what-is-the-most-common-encoding-of-each-language) <br />
[utf-bom4](http://www.unicode.org/faq/utf_bom.html#bom4) 

## Contributions and Support
Contributions to the Magic File Encoding Library are welcome! If you encounter any issues, have suggestions for improvements,
or would like to contribute to its development, please visit our GitHub repository.