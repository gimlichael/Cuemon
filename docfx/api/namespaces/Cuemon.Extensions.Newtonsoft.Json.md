---
uid: Cuemon.Extensions.Newtonsoft.Json
summary: *content
---
The Cuemon.Extensions.Newtonsoft.Json namespace contains both types and extension methods that complements the Newtonsoft.Json namespace by adding new ways of working with JSON; both in terms of serialization and parsing.

Availability: NET Standard 2.0, .NET 5.0

Complements: [Newtonsoft.Json namespace](https://www.newtonsoft.com/json/help/html/N_Newtonsoft_Json.htm) 🔗

Github branches: 🌱\
[development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.Newtonsoft.Json) 🧪\
[release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.Newtonsoft.Json) 🎬\
[master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.Newtonsoft.Json) 🛡️

![Github Checks](https://img.shields.io/github/checks-status/gimlichael/Cuemon/development?logo=github)

## NuGet packages

📦 Focus Pack\
[Cuemon.Extensions.Newtonsoft.Json (CI)](https://nuget.cuemon.net/packages/Cuemon.Extensions.Newtonsoft.Json)\
[Cuemon.Extensions.Newtonsoft.Json (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Extensions.Newtonsoft.Json)\
![NuGet Version](https://img.shields.io/nuget/v/Cuemon.Extensions.Newtonsoft.Json?logo=nuget) ![NuGet Preview Version](https://img.shields.io/nuget/vpre/Cuemon.Extensions.Newtonsoft.Json?logo=nuget) ![NuGet Downloads](https://img.shields.io/nuget/dt/Cuemon.Extensions.Newtonsoft.Json?color=blueviolet&logo=nuget)
\
\
🏭 Productivity Pack\
[Cuemon.Core.App (CI)](https://nuget.cuemon.net/packages/Cuemon.Core.App)\
[Cuemon.Core.App (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Core.App)\
![NuGet Version](https://img.shields.io/nuget/v/Cuemon.Core.App?logo=nuget) ![NuGet Preview Version](https://img.shields.io/nuget/vpre/Cuemon.Core.App?logo=nuget) ![NuGet Downloads](https://img.shields.io/nuget/dt/Cuemon.Core.App?color=blueviolet&logo=nuget)

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|JDataResult|⬇️|`Flatten`, `ExtractArrayValues`, `ExtractObjectValues`|
|JsonReader|⬇️|`ToHierarchy`|
|JsonSerializerSettings|⬇️|`ApplyToDefaultSettings`, `UseCamelCase`|
|JsonWriter|⬇️|`WriteObject`, `WritePropertyName`|
|Validator|⬇️|`InvalidJsonDocument`|