# Magic File Encoding

The Magic File Encoding helps to load and transform from simple an closed scope char set text files.

Be aware of possible transformation issues if the target encoding is more simple than the source encoding.

I recommend to write unit tests for your given tasks to ensure the load transformation works in expected ways for your use case.

### Nuget Package
[MagicFileEncoding at nuget.org](https://www.nuget.org/packages/MagicFileEncoding/)

### Usage (Preview Version)

```csharp
var mfe = new global::MagicFileEncoding.MagicFileEncoding();
var str = mfe.AutomaticReadAllText(filename);
```

### Fallback Encoding
The fallback encoding is ISO-8859-1 (Latin-1) by default but can be changed via property.

(Because this lib was designed for the german culture space)

# Credits
This work is heavily based on the flowing stack overflow articles:<br />
[effective-way-to-find-any-files-encoding](https://stackoverflow.com/questions/3825390/effective-way-to-find-any-files-encoding) <br />
[determine-a-strings-encoding-in-c-sharp](https://stackoverflow.com/questions/1025332/determine-a-strings-encoding-in-c-sharp) <br />
[check-for-invalid-utf8](https://stackoverflow.com/questions/6555015/check-for-invalid-utf8) <br />
[how-to-detect-utf-8-in-plain-c](https://stackoverflow.com/questions/1031645/how-to-detect-utf-8-in-plain-c) <br />
[strip-byte-order-mark-from-string-in-c-sharp](https://stackoverflow.com/questions/1317700/strip-byte-order-mark-from-string-in-c-sharp)
