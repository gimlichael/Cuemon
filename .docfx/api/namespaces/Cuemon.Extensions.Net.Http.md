---
uid: Cuemon.Extensions.Net.Http
summary: *content
---
The `Cuemon.Extensions.Net.Http` namespace contains both types and extension methods that complements the `Cuemon.Net` namespace. Includes support for both traditional and factory based ways of working with HttpMangager instances while also including a simple and lightweight implementation of the IHttpClientFactory interface named SlimHttpClientFactory (that provides "managed" HttpClient instances).

[!INCLUDE [availability-default](../../includes/availability-default.md)]

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|HttpMethod|⬇️|`ToHttpMethod`|
|Uri|⬇️|`HttpDeleteAsync`, `HttpGetAsync`, `HttpHeadAsync`, `HttpOptionsAsync`, `HttpPostAsync`, `HttpPutAsync`, `HttpPatchAsync`, `HttpTraceAsync`, `HttpAsync`|
