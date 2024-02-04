# Upgrade Guide v1.1.* to v2.0.*

#### Write a text file

Last two arguments are swapped

```csharp
// v1.1.*
FileEncoding.WriteAllText(tmpFile.Path, text, Encoding.UTF8);
```
changes to:
```csharp
// v2.0.*
FileEncoding.WriteAllText(tmpFile.Path, Encoding.UTF8, text);
```