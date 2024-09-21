---
uid: Cuemon.Extensions.AspNetCore.Diagnostics
summary: *content
---
The `Cuemon.Extensions.AspNetCore.Diagnostics` namespace contains extension methods that complements the `Cuemon.AspNetCore.Diagnostics` namespace.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Cuemon.AspNetCore.Diagnostics namespace](https://docs.cuemon.net/api/aspnet/Cuemon.AspNetCore.Diagnostics.html) 📘

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|IApplicationBuilder|⬇️|`UseServerTiming`, `UseFaultDescriptorExceptionHandler`|
|IServiceCollection|⬇️|`AddServerTiming`, `AddServerTiming{T}`, `AddServerTimingOptions`, `AddFaultDescriptorOptions`, `AddExceptionDescriptorOptions`, `PostConfigureAllExceptionDescriptorOptions`|
|IServiceProvider|⬇️|`GetExceptionResponseFormatters`|
