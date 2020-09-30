---
uid: Cuemon.Extensions.IO
summary: *content
---
The Cuemon.Extensions.IO namespace contains extension methods that complements the Cuemon.IO namespace while being an addition to the System.IO namespace.

Availability: NET Standard 2.0, NET Standard 2.1

Complements: [Cuemon.IO namespace](https://docs.cuemon.net/api/dotnet/Cuemon.IO.html) 🔗

Github branches 🌱\
[development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.IO)\
[release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.IO)\
[master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.IO)

NuGet packages 📦\
[Cuemon.Extensions.IO (CI)](https://nuget.cuemon.net/packages/Cuemon.Extensions.IO)\
[Cuemon.Extensions.IO (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Extensions.IO)

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|byte[]|⬇️|`ToStream`, `ToStreamAsync`|
|Stream|⬇️|`Concat`, `ToCharArray`, `ToByteArray`, `ToByteArrayAsync`, `WriteAsync`, `TryDetectUnicodeEncoding`, `ToEncodedString`, `ToEncodedStringAsync`, `CompressBrotli`, `CompressBrotliAsync`, `CompressDeflate`, `CompressDeflateAsync`, `CompressGZip`, `CompressGZipAsync`, `DecompressBrotli`, `DecompressBrotliAsync`, `DecompressDeflate`, `DecompressDeflateAsync`, `DecompressGZip`, `DecompressGZipAsync`|
|String|⬇️|`ToStream`, `ToStreamAsync`, `ToTextReader`|
|TextReader|⬇️|`CopyToAsync`, `ReadAllLines`, `ReadAllLinesAsync`|