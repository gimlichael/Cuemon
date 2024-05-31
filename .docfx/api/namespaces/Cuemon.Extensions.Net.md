---
uid: Cuemon.Extensions.Net
summary: *content
---
The `Cuemon.Extensions.Net` namespace contains both types and extension methods that complements the `Cuemon.Net` namespace while being an addition to the `System.Net` namespace. Includes support for both traditional and factory based ways of working with HttpMangager instances while also including a simple and lightweight implementation of the IHttpClientFactory interface named SlimHttpClientFactory (that provides "managed" HttpClient instances).

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Complements: [Cuemon.Net namespace](/api/dotnet/Cuemon.Net.html) 📘

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|byte[]|⬇️|`UrlEncode`|
|IDictionary{string, string[]}|⬇️|`ToQueryString`|
|HttpStatusCode|⬇️|`IsInformationStatusCode`, `IsSuccessStatusCode`, `IsRedirectionStatusCode`, `IsClientErrorStatusCode`, `IsServerErrorStatusCode`|
|NameValueCollection|⬇️|`ToQueryString`|
|String|⬇️|`UrlDecode`, `UrlEncode`|
