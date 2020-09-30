---
uid: Cuemon.Extensions.Net.Security
summary: *content
---
The Cuemon.Extensions.Net.Security namespace contains extension methods that provides a generic way to make a Uniform Resource Identifier signed and tampering protected. This could be used to make your own lightweight concept of a Azure shared access signatures (SAS). Originally part of Cuemon .NET Framework: https://github.com/gimlichael/CuemonNetFramework/blob/master/Cuemon.Web/Security/WebSecurityUtility.cs. Greatly simplified anno 2020.

Availability: NET Standard 2.0

Github branches 🌱\
[development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.Net/Security)\
[release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.Net/Security)\
[master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.Net/Security)

NuGet packages 📦\
[Cuemon.Extensions.Net (CI)](https://nuget.cuemon.net/packages/Cuemon.Extensions.Net)\
[Cuemon.Extensions.Net (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Extensions.Net)

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|String|⬇️|`ToSignedUri`, `ValidateSignedUri`|
|Uri|⬇️|`ToSignedUri`, `ValidateSignedUri`|