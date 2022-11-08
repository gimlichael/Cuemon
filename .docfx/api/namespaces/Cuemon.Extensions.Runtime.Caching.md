---
uid: Cuemon.Extensions.Runtime.Caching
summary: *content
---
The Cuemon.Extensions.Runtime.Caching namespace contains extension methods that complements the Cuemon.Runtime.Caching namespace by adding support for Memoization techniques and GetOrAdd convenience ; both with vast overloads and extended by the ICacheEnumerable{TKey} interface for loose coupling.

Availability: NET Standard 2.0, .NET 6.0

Complements: [Cuemon.Runtime.Caching namespace](https://docs.cuemon.net/api/dotnet/Cuemon.Runtime.Caching.html) 🔗

## Github branches 🖇️

[development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.Runtime.Caching) 🧪\
[release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.Runtime.Caching) 🎬\
[master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.Runtime.Caching) 🛡️

## NuGet packages

📦 Focus Pack\
[Cuemon.Extensions.Runtime.Caching (CI)](https://nuget.cuemon.net/packages/Cuemon.Extensions.Runtime.Caching)\
[Cuemon.Extensions.Runtime.Caching (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Extensions.Runtime.Caching)\
![NuGet Version](https://img.shields.io/nuget/v/Cuemon.Extensions.Runtime.Caching?logo=nuget) ![NuGet Preview Version](https://img.shields.io/nuget/vpre/Cuemon.Extensions.Runtime.Caching?logo=nuget) ![NuGet Downloads](https://img.shields.io/nuget/dt/Cuemon.Extensions.Runtime.Caching?color=blueviolet&logo=nuget)
\
\
🏭 Productivity Pack\
[Cuemon.App (CI)](https://nuget.cuemon.net/packages/Cuemon.Core.App)\
[Cuemon.App (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Core.App)\
![NuGet Version](https://img.shields.io/nuget/v/Cuemon.Core.App?logo=nuget) ![NuGet Preview Version](https://img.shields.io/nuget/vpre/Cuemon.Core.App?logo=nuget) ![NuGet Downloads](https://img.shields.io/nuget/dt/Cuemon.Core.App?color=blueviolet&logo=nuget)

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|ICacheEnumerable{TKey}|⬇️|`GetOrAdd{TKey, TResult}`, `Memoize{TKey, T, TResult}`, `Memoize{TKey, T, TResult}`, `Memoize{TKey, T1, T2, TResult}`, `Memoize{TKey, T1, T2, T3, TResult}`, `Memoize{TKey, T1, T2, T3, T4, TResult}`, `Memoize{TKey, T1, T2, T3, T4, T5, TResult}`|