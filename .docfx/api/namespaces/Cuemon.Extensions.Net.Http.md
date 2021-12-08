---
uid: Cuemon.Extensions.Net.Http
summary: *content
---
The Cuemon.Extensions.Net.Http namespace contains both types and extension methods that complements the Cuemon.Net namespace. Includes support for both traditional and factory based ways of working with HttpMangager instances while also including a simple and lightweight implementation of the IHttpClientFactory interface named SlimHttpClientFactory (that provides "managed" HttpClient instances).

Availability: NET Standard 2.0, .NET 5.0, .NET 6.0

## Github branches 🖇️

[development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.Net/Http) 🧪\
[release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.Net/Http) 🎬\
[master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.Net/Http) 🛡️

## NuGet packages

📦 Focus Pack\
[Cuemon.Extensions.Net (CI)](https://nuget.cuemon.net/packages/Cuemon.Extensions.Net)\
[Cuemon.Extensions.Net (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Extensions.Net)\
![NuGet Version](https://img.shields.io/nuget/v/Cuemon.Extensions.Net?logo=nuget) ![NuGet Preview Version](https://img.shields.io/nuget/vpre/Cuemon.Extensions.Net?logo=nuget) ![NuGet Downloads](https://img.shields.io/nuget/dt/Cuemon.Extensions.Net?color=blueviolet&logo=nuget)
\
\
🏭 Productivity Pack\
[Cuemon.App (CI)](https://nuget.cuemon.net/packages/Cuemon.Core.App)\
[Cuemon.App (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Core.App)\
![NuGet Version](https://img.shields.io/nuget/v/Cuemon.Core.App?logo=nuget) ![NuGet Preview Version](https://img.shields.io/nuget/vpre/Cuemon.Core.App?logo=nuget) ![NuGet Downloads](https://img.shields.io/nuget/dt/Cuemon.Core.App?color=blueviolet&logo=nuget)

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|HttpMethod|⬇️|`ToHttpMethod`|
|Uri|⬇️|`HttpDeleteAsync`, `HttpGetAsync`, `HttpHeadAsync`, `HttpOptionsAsync`, `HttpPostAsync`, `HttpPutAsync`, `HttpPatchAsync`, `HttpTraceAsync`, `HttpAsync`|