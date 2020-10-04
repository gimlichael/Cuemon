---
uid: Cuemon.Extensions.AspNetCore.Http
summary: *content
---
The Cuemon.Extensions.AspNetCore.Http namespace contains extension methods that complements the Cuemon.AspNetCore.Http namespace while being an addition to the Microsoft.AspNetCore.Http namespace.

Availability: NET Standard 2.0, NET Core 3.0

Complements: [Cuemon.AspNetCore.Http namespace](https://docs.cuemon.net/api/aspnet/Cuemon.AspNetCore.Http.html) 🔗

Github branches 🌱\
[development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.AspNetCore/Http)\
[release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.AspNetCore/Http)\
[master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.AspNetCore/Http)

NuGet packages 📦\
[Cuemon.Extensions.AspNetCore (CI)](https://nuget.cuemon.net/packages/Cuemon.Extensions.AspNetCore)\
[Cuemon.Extensions.AspNetCore (Stable and Preview)](https://www.nuget.org/packages/Cuemon.Extensions.AspNetCore)

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|IHeaderDictionary|⬇️|`AddOrUpdateHeaders`, `AddOrUpdateHeader`|
|HttpRequest|⬇️|`IsGetOrHeadMethod`, `IsClientSideResourceCached`|
|HttpResponse|⬇️|`AddOrUpdateEntityTagHeader`, `AddOrUpdateLastModifiedHeader`, `WriteBodyAsync`, `OnStartingInvokeTransformer`|
|Int32|⬇️|`IsInformationStatusCode`, `IsSuccessStatusCode`, `IsRedirectionStatusCode`, `IsNotModifiedStatusCode`, `IsClientErrorStatusCode`, `IsServerErrorStatusCode`|