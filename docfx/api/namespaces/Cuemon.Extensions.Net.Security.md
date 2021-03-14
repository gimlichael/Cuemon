---
uid: Cuemon.Extensions.Net.Security
summary: *content
---
The Cuemon.Extensions.Net.Security namespace contains extension methods that provides a generic way to make a Uniform Resource Identifier signed and tampering protected. This could be used to make your own lightweight concept of a Azure shared access signatures (SAS). Originally part of Cuemon .NET Framework: https://github.com/gimlichael/CuemonNetFramework/blob/master/Cuemon.Web/Security/WebSecurityUtility.cs. Greatly simplified anno 2020.

Availability: NET Standard 2.0, .NET 5.0

## Github branches 🖇️

[development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.Net/Security) 🧪\
[release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.Net/Security) 🎬\
[master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.Net/Security) 🛡️

![Github Checks](https://img.shields.io/github/checks-status/gimlichael/Cuemon/development?logo=github)

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
|String|⬇️|`ToSignedUri`, `ValidateSignedUri`|
|Uri|⬇️|`ToSignedUri`, `ValidateSignedUri`|