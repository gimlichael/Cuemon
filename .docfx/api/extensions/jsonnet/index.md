---
uid: extensions-jsonnet-md
title: Extensions for Json.NET API
---
# Extensions for Json.NET API

I am a huge fan of [Json.NET](https://www.newtonsoft.com/json) written by [James Newton-King](https://github.com/JamesNK) and the flexible architecture this JSON framework adds to the toolbelt.

So even though Microsoft decided to write their own [JSON framework](https://docs.microsoft.com/en-us/dotnet/api/system.text.json) (first seen with the release of ASP.NET Core 3), Cuemon for .NET will continue to support and extend Json.NET as the natural companion of our framework.

+ Newtonsoft.Json
  + [Cuemon.Extensions.Newtonsoft.Json](#cuemonextensionsnewtonsoftjson)
  + Cuemon.Extensions.Newtonsoft.Json.Diagnostics
  + Cuemon.Extensions.Newtonsoft.Json.Formatters
  + [Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json](#cuemonextensionsaspnetcoremvcformattersnewtonsoftjson)
+ Newtonsoft.Json.Converters
  + Cuemon.Extensions.Newtonsoft.Json.Converters
+ Newtonsoft.Json.Serialization
  + Cuemon.Extensions.Newtonsoft.Json.Serialization

## Cuemon.Extensions.Newtonsoft.Json

The Cuemon.Extensions.Newtonsoft.Json namespace contains both types and extension methods that complements the Newtonsoft.Json namespace by adding new ways of working with JSON; both in terms of serialization and parsing.

Availability: **.NET Standard 2.0, .NET 5.0**

Assembly: Cuemon.Extensions.Newtonsoft.Json

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.Extensions.Newtonsoft.Json)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.Newtonsoft.Json) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.Newtonsoft.Json) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.Newtonsoft.Json)

## Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json

The Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json namespace contains both types and extension methods that complements the Cuemon.Extensions.Newtonsoft.Json namespace while being an addition to the Microsoft.AspNetCore.Mvc namespace. Provides JSON formatters for ASP.NET Core that is powered by Newtonsoft.Json.

Availability: **.NET Standard 2.0, NET Core 3.0, .NET 5.0**

Assembly: Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json)