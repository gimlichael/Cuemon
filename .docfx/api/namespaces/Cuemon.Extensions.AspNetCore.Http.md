---
uid: Cuemon.Extensions.AspNetCore.Http
summary: *content
---
The `Cuemon.Extensions.AspNetCore.Http` namespace contains extension methods that complements the `Cuemon.AspNetCore.Http` namespace while being an addition to the `Microsoft.AspNetCore.Http` namespace.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Cuemon.AspNetCore.Http namespace](https://docs.cuemon.net/api/aspnet/Cuemon.AspNetCore.Http.html) 📘

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|IHeaderDictionary|⬇️|`AddOrUpdateHeaders`, `AddOrUpdateHeader`|
|HttpRequest|⬇️|`IsGetOrHeadMethod`, `IsClientSideResourceCached`|
|HttpResponse|⬇️|`AddOrUpdateEntityTagHeader`, `AddOrUpdateLastModifiedHeader`, `WriteBodyAsync`, `OnStartingInvokeTransformer`|
|Int32|⬇️|`IsInformationStatusCode`, `IsSuccessStatusCode`, `IsRedirectionStatusCode`, `IsNotModifiedStatusCode`, `IsClientErrorStatusCode`, `IsServerErrorStatusCode`|
