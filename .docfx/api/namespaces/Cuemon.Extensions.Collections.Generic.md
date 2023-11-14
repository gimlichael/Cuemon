---
uid: Cuemon.Extensions.Collections.Generic
summary: *content
---
The `Cuemon.Extensions.Collections.Generic` namespace contains extension methods that complements the `Cuemon.Collections.Generic` namespace while being an addition to the `System.Collections.Generic` namespace.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Cuemon.Collections.Generic namespace](/api/dotnet/Cuemon.Collections.Generic.html) 📘

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|ICollection{T}|⬇️|`ToPartitioner{T}`, `AddRange{T}`|
|IDictionary{TKey, TValue}|⬇️|`CopyTo{TKey, TValue}`, `GetValueOrDefault{TKey, TValue}`, `TryGetValueOrFallback{TKey, TValue}`, `ToEnumerable{TKey, TValue}`, `TryAdd{TKey, TValue}`, `AddOrUpdate{TKey, TValue}`|
|IEnumerable{T}|⬇️|`Chunk{T}`, `Shuffle{T}`, `OrderAscending{T}`, `OrderDescending{T}`, `RandomOrDefault{T}`, `Yield{T}`, `ToDictionary{TKey, TValue}`, `ToPagination{T}`, `ToPaginationList{T}`|
|IList{T}|⬇️|`Remove{T}`, `HasIndex{T}`, `Next{T}`, `Previous{T}`, `TryAdd{T}`|
|Queue{T}|⬇️|`TryPeek{T}`|
