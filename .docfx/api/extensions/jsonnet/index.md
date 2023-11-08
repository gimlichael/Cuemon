---
uid: extensions-jsonnet-md
title: Extensions for Json.NET API
---
# Extensions for Json.NET API

I am a huge fan of [Json.NET](https://www.newtonsoft.com/json) written by [James Newton-King](https://github.com/JamesNK) and the flexible architecture this JSON framework adds to the toolbelt.

So even though Microsoft decided to write their own [JSON framework](https://docs.microsoft.com/en-us/dotnet/api/system.text.json) (first seen with the release of ASP.NET Core 3), Cuemon for .NET will continue to support and extend Json.NET as the natural companion of our framework.

## Cuemon.Extensions.Newtonsoft.Json

The `Cuemon.Extensions.Newtonsoft.Json` namespace contains both types and extension methods that complements the `Newtonsoft.Json` namespace by adding new ways of working with JSON; both in terms of serialization and parsing.

[!INCLUDE [availability-default](../../../includes/availability-default.md)]

## Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json

The `Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json` namespace contains both types and extension methods that complements the `Cuemon.Extensions.Newtonsoft.Json` namespace while being an addition to the `Microsoft.AspNetCore.Mvc` namespace. Provides JSON formatters for ASP.NET Core that is powered by Newtonsoft.Json.

[!INCLUDE [availability-modern](../../../includes/availability-modern.md)]