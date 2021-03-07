---
uid: Cuemon.Extensions.IO
summary: *content
---
The Cuemon.Extensions.IO namespace contains extension methods that complements the Cuemon.IO namespace while being an addition to the System.IO namespace.

Availability: NET Standard 2.0, NET Standard 2.1, .NET 5.0

Complements: [Cuemon.IO namespace](https://docs.cuemon.net/api/dotnet/Cuemon.IO.html) 🔗

## Github branches 🖇️

[development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.IO) 🧪\
[release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.IO) 🎬\
[master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.IO) 🛡️

![Github Checks](https://img.shields.io/github/checks-status/gimlichael/Cuemon/development?logo=github)

## NuGet packages

📦 Focus Pack\
[Cuemon.Extensions.IO (CI)](https://nuget.cuemon.net/packages/Cuemon.Extensions.IO)\
[Cuemon.Extensions.IO (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Extensions.IO)\
![NuGet Version](https://img.shields.io/nuget/v/Cuemon.Extensions.IO?logo=nuget) ![NuGet Preview Version](https://img.shields.io/nuget/vpre/Cuemon.Extensions.IO?logo=nuget) ![NuGet Downloads](https://img.shields.io/nuget/dt/Cuemon.Extensions.IO?color=blueviolet&logo=nuget)
\
\
🏭 Productivity Pack\
[Cuemon.Core.App (CI)](https://nuget.cuemon.net/packages/Cuemon.Core.App)\
[Cuemon.Core.App (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Core.App)\
![NuGet Version](https://img.shields.io/nuget/v/Cuemon.Core.App?logo=nuget) ![NuGet Preview Version](https://img.shields.io/nuget/vpre/Cuemon.Core.App?logo=nuget) ![NuGet Downloads](https://img.shields.io/nuget/dt/Cuemon.Core.App?color=blueviolet&logo=nuget)

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|byte[]|⬇️|`ToStream`, `ToStreamAsync`|
|Stream|⬇️|`Concat`, `ToCharArray`, `ToByteArray`, `ToByteArrayAsync`, `WriteAsync`, `TryDetectUnicodeEncoding`, `ToEncodedString`, `ToEncodedStringAsync`, `CompressBrotli`, `CompressBrotliAsync`, `CompressDeflate`, `CompressDeflateAsync`, `CompressGZip`, `CompressGZipAsync`, `DecompressBrotli`, `DecompressBrotliAsync`, `DecompressDeflate`, `DecompressDeflateAsync`, `DecompressGZip`, `DecompressGZipAsync`|
|String|⬇️|`ToStream`, `ToStreamAsync`, `ToTextReader`|
|TextReader|⬇️|`CopyToAsync`, `ReadAllLines`, `ReadAllLinesAsync`|