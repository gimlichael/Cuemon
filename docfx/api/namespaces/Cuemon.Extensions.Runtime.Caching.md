---
uid: Cuemon.Extensions.Runtime.Caching
summary: *content
---
The Cuemon.Extensions.Runtime.Caching namespace contains extension methods that complements the Cuemon.Runtime.Caching namespace by adding support for Memoization techniques and GetOrAdd convenience ; both with vast overloads and extended by the ICacheEnumerable{TKey} interface for loose coupling.

Availability: NET Standard 2.0

Complements: [Cuemon.Runtime.Caching namespace](https://docs.cuemon.net/api/dotnet/Cuemon.Runtime.Caching.html) 🔗

Github branches 🌱\
[development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.Runtime.Caching)\
[release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.Runtime.Caching)\
[master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.Runtime.Caching)

NuGet packages 📦\
[Cuemon.Extensions.Runtime.Caching (CI)](https://nuget.cuemon.net/packages/Cuemon.Extensions.Runtime.Caching)\
[Cuemon.Extensions.Runtime.Caching (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Extensions.Runtime.Caching)

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|ICacheEnumerable{TKey}|⬇️|`GetOrAdd{TKey, TResult}`, `Memoize{TKey, T, TResult}`, `Memoize{TKey, T, TResult}`, `Memoize{TKey, T1, T2, TResult}`, `Memoize{TKey, T1, T2, T3, TResult}`, `Memoize{TKey, T1, T2, T3, T4, TResult}`, `Memoize{TKey, T1, T2, T3, T4, T5, TResult}`|