---
uid: Cuemon.Extensions.Net
summary: *content
---
The Cuemon.Extensions.Net namespace contains both types and extension methods that complements the Cuemon.Net namespace while being an addition to the System.Net namespace. Includes support for both traditional and factory based ways of working with HttpMangager instances while also including a simple and lightweight implementation of the IHttpClientFactory interface named SlimHttpClientFactory (that provides "managed" HttpClient instances).

Availability: NET Standard 2.0, .NET 5.0, .NET 6.0

Complements: [Cuemon.Net namespace](https://docs.cuemon.net/api/dotnet/Cuemon.Net.html) 🔗

## Github branches 🖇️

[development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.Net) 🧪\
[release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.Net) 🎬\
[master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.Net) 🛡️

## NuGet packages

📦 Focus Pack\
[Cuemon.Extensions.Net (CI)](https://nuget.cuemon.net/packages/Cuemon.Extensions.Net)\
[Cuemon.Extensions.Net (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Extensions.Net)\
![NuGet Version](https://img.shields.io/nuget/v/Cuemon.Extensions.Net?logo=nuget) ![NuGet Preview Version](https://img.shields.io/nuget/vpre/Cuemon.Extensions.Net?logo=nuget) ![NuGet Downloads](https://img.shields.io/nuget/dt/Cuemon.Extensions.Net?color=blueviolet&logo=nuget)
\
\
🏭 Productivity Pack\
[Cuemon.Core.App (CI)](https://nuget.cuemon.net/packages/Cuemon.Core.App)\
[Cuemon.Core.App (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Core.App)\
![NuGet Version](https://img.shields.io/nuget/v/Cuemon.Core.App?logo=nuget) ![NuGet Preview Version](https://img.shields.io/nuget/vpre/Cuemon.Core.App?logo=nuget) ![NuGet Downloads](https://img.shields.io/nuget/dt/Cuemon.Core.App?color=blueviolet&logo=nuget)

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|byte[]|⬇️|`UrlEncode`|
|IDictionary{string, string[]}|⬇️|`ToQueryString`|
|HttpStatusCode|⬇️|`IsInformationStatusCode`, `IsSuccessStatusCode`, `IsRedirectionStatusCode`, `IsClientErrorStatusCode`, `IsServerErrorStatusCode`|
|NameValueCollection|⬇️|`ToQueryString`|
|String|⬇️|`UrlDecode`, `UrlEncode`|