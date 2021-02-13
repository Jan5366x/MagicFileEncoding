# Upgrade Guide v1.0.0 to v1.1.1

#### Read a text file

```csharp
// v1.0.0
var text = new FileEncoding().AutomaticReadAllText(filename);
```
changes to:
```csharp
// v1.1.1
var text = FileEncoding.ReadAllText(filePath);
// or 
var text = FileEncoding.ReadAllText(filePath, Encoding.Unicode);
```
#### Write a text file
```csharp
// v1.0.0
new FileEncoding().WriteAllText(tmpFile.Path, text, Encoding.UTF8);
```
changes to:
```csharp
// v1.1.1
FileEncoding.WriteAllText(tmpFile.Path, text, Encoding.UTF8);
```
### Just detect suitable encoding
```csharp
// v1.0.0
var encoding = new FileEncoding().GetAcceptableEncoding(filename);
```
changes to:
```csharp
// v1.1.1
var encoding = FileEncoding.GetAcceptableEncoding(filename);
```
#### Change fallback (default) encoding
```csharp
// v1.0.0
var fe = new FileEncoding();
fe.FallbackEncoding = Encoding.UTF8;
var text = fe.AutomaticReadAllText(filename);
```
changes to:
```csharp
// v1.1.1
var text = FileEncoding.ReadAllText(filePath, Encoding.Unicode, Encoding.UTF8);
```