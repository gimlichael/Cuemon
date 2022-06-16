---
uid: Cuemon.Extensions.Collections.Generic
summary: *content
---
The Cuemon.Extensions.Collections.Generic namespace contains extension methods that complements the Cuemon.Collections.Generic namespace while being an addition to the System.Collections.Generic namespace.

Availability: NET Standard 2.0, .NET 6.0

Complements: [Cuemon.Collections.Generic namespace](https://docs.cuemon.net/api/dotnet/Cuemon.Collections.Generic.html) 🔗

## Github branches 🖇️

[development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.Collections.Generic) 🧪\
[release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.Collections.Generic) 🎬\
[master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.Collections.Generic) 🛡️

## NuGet packages

📦 Focus Pack\
[Cuemon.Extensions.Collections.Generic (CI)](https://nuget.cuemon.net/packages/Cuemon.Extensions.Collections.Generic)\
[Cuemon.Extensions.Collections.Generic (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Extensions.Collections.Generic)\
![NuGet Version](https://img.shields.io/nuget/v/Cuemon.Extensions.Collections.Generic?logo=nuget) ![NuGet Preview Version](https://img.shields.io/nuget/vpre/Cuemon.Extensions.Collections.Generic?logo=nuget) ![NuGet Downloads](https://img.shields.io/nuget/dt/Cuemon.Extensions.Collections.Generic?color=blueviolet&logo=nuget)
\
\
🏭 Productivity Pack\
[Cuemon.App (CI)](https://nuget.cuemon.net/packages/Cuemon.Core.App)\
[Cuemon.App (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Core.App)\
![NuGet Version](https://img.shields.io/nuget/v/Cuemon.Core.App?logo=nuget) ![NuGet Preview Version](https://img.shields.io/nuget/vpre/Cuemon.Core.App?logo=nuget) ![NuGet Downloads](https://img.shields.io/nuget/dt/Cuemon.Core.App?color=blueviolet&logo=nuget)

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|ICollection{T}|⬇️|`ToPartitioner{T}`, `AddRange{T}`|
|IDictionary{TKey, TValue}|⬇️|`GetValueOrDefault{TKey, TValue}`, `TryGetValueOrFallback{TKey, TValue}`, `ToEnumerable{TKey, TValue}`, `TryAdd{TKey, TValue}`, `TryAddOrUpdate{TKey, TValue}`|
|IEnumerable{T}|⬇️|`Chunk{T}`, `Shuffle{T}`, `OrderBy{T}`, `OrderByDescending{T}`, `RandomOrDefault{T}`, `Yield{T}`, `ToDictionary{TKey, TValue}`, `ToPartitioner{T}`, `ToPagedCollection{T}`|
|IList{T}|⬇️|`Remove{T}`, `HasIndex{T}`, `Next{T}`, `Previous{T}`|