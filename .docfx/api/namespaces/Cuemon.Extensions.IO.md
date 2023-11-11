---
uid: Cuemon.Extensions.IO
summary: *content
---
The `Cuemon.Extensions.IO` namespace contains extension methods that complements the `Cuemon.IO` namespace while being an addition to the `System.IO` namespace.

[!INCLUDE [availability-all](../../includes/availability-all.md)]

Complements: [Cuemon.IO namespace](https://docs.cuemon.net/api/dotnet/Cuemon.IO.html) 🔗

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|byte[]|⬇️|`ToStream`, `ToStreamAsync`|
|Stream|⬇️|`Concat`, `ToCharArray`, `ToByteArray`, `ToByteArrayAsync`, `WriteAllAsync`, `TryDetectUnicodeEncoding`, `ToEncodedString`, `ToEncodedStringAsync`, `CompressBrotli`, `CompressBrotliAsync`, `CompressDeflate`, `CompressDeflateAsync`, `CompressGZip`, `CompressGZipAsync`, `DecompressBrotli`, `DecompressBrotliAsync`, `DecompressDeflate`, `DecompressDeflateAsync`, `DecompressGZip`, `DecompressGZipAsync`|
|String|⬇️|`ToStream`, `ToStreamAsync`, `ToTextReader`|
|TextReader|⬇️|`CopyToAsync`, `ReadAllLines`, `ReadAllLinesAsync`|
