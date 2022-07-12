---
uid: aspnet-md
title: ASP.NET Core API
---
# ASP.NET Core API

ASP.NET Core API, that is part of Cuemon for .NET, is a toolbelt of assemblies designed to be intuitive and follow the same namespace declarations as we have been accustomed by [Microsoft.AspNetCore](https://docs.microsoft.com/en-us/dotnet/api/?view=aspnetcore-5.0) which is included in the .NET platform by Microsoft.

+ Microsoft.AspNetCore
  + [Cuemon.AspNetCore](#cuemonaspnetcore)
  + Cuemon.AspNetCore.Configuration
+ Microsoft.AspNetCore.Authentication
  + [Cuemon.AspNetCore.Authentication](#cuemonaspnetcoreauthentication)
  + Cuemon.AspNetCore.Authentication.Basic
  + Cuemon.AspNetCore.Authentication.Digest
  + Cuemon.AspNetCore.Authentication.Hmac
+ Microsoft.AspNetCore.Builder
  + Cuemon.AspNetCore.Builder
+ Microsoft.AspNetCore.Diagnostics
  + Cuemon.AspNetCore.Diagnostics
+ Microsoft.AspNetCore.Hosting
  + Cuemon.AspNetCore.Hosting
+ Microsoft.AspNetCore.Http
  + Cuemon.AspNetCore.Http
  + Cuemon.AspNetCore.Http.Throttling
+ Microsoft.AspNetCore.Http.Headers
  + Cuemon.AspNetCore.Http.Headers
+ Microsoft.AspNetCore.Mvc
  + [Cuemon.AspNetCore.Mvc](#cuemonaspnetcoremvc)
+ Microsoft.AspNetCore.Mvc.Filters
  + Cuemon.AspNetCore.Mvc.Filters
  + Cuemon.AspNetCore.Mvc.Filters.Cacheable
  + Cuemon.AspNetCore.Mvc.Filters.Diagnostics
  + Cuemon.AspNetCore.Mvc.Filters.Headers
  + Cuemon.AspNetCore.Mvc.Filters.Throttling
+ Microsoft.AspNetCore.Mvc.Filters.ModelBinding
  + Cuemon.AspNetCore.Mvc.Filters.ModelBinding
+ Microsoft.AspNetCore.Mvc.Formatters
  + Cuemon.AspNetCore.Mvc.Formatters
+ Microsoft.AspNetCore.Razor.TagHelpers
  + [Cuemon.AspNetCore.Razor.TagHelpers](#cuemonaspnetcorerazortaghelpers)

## Cuemon.AspNetCore

The Cuemon.AspNetCore namespace contains types focusing on providing means for easier plumber coding in the ASP.NET Core pipeline while serving some concrete implementation of the shell as well. The namespace is an addition to the Microsoft.AspNetCore namespace.

Availability: **.NET Standard 2.0, .NET Core 3.1, .NET 6.0**

Assembly: Cuemon.AspNetCore

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.AspNetCore) | [Productivity](https://www.nuget.org/packages/Cuemon.AspNetCore.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.AspNetCore) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.AspNetCore) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.AspNetCore)

## Cuemon.AspNetCore.Authentication

The Cuemon.AspNetCore.Authentication namespace contains types that enable support for authentication using the concept of an Authenticator, AuthorizationHeader and (to tie the knots) an AuthorizationHeaderBuilder. Basic-, Digest Access- and HMAC Authentication is provided out-of-the-box. The namespace is an addition to the Microsoft.AspNetCore.Authentication namespace.

Availability: **.NET Standard 2.0, .NET Core 3.1, .NET 6.0**

Assembly: Cuemon.AspNetCore.Authentication

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.AspNetCore.Authentication) | [Productivity](https://www.nuget.org/packages/Cuemon.AspNetCore.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.AspNetCore.Authentication) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.AspNetCore.Authentication) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.AspNetCore.Authentication)

## Cuemon.AspNetCore.Mvc

The Cuemon.AspNetCore,Mvc namespace contains types that specializes in cache expiration and validation models and an abundant range of ready-to-use filters in the ASP.NET Core MVC pipeline. The namespace is an addition to the Microsoft.AspNetCore.Mvc namespace.

Availability: **.NET Standard 2.0, .NET Core 3.1, .NET 6.0**

Assembly: Cuemon.AspNetCore.Mvc

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.AspNetCore.Mvc) | [Productivity](https://www.nuget.org/packages/Cuemon.AspNetCore.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.AspNetCore.Mvc) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.AspNetCore.Mvc) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.AspNetCore.Mvc)

## Cuemon.AspNetCore.Razor.TagHelpers

The Cuemon.AspNetCore.Razor.TagHelpers namespace contains types tailored tag helper implementations. The namespace is an addition to the Microsoft.AspNetCore.Razor.TagHelpers namespace.

Availability: **.NET Standard 2.0, .NET Core 3.1, .NET 6.0**

Assembly: Cuemon.AspNetCore.Razor.TagHelpers

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.AspNetCore.Razor.TagHelpers) | [Productivity](https://www.nuget.org/packages/Cuemon.AspNetCore.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.AspNetCore.Razor.TagHelpers) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.AspNetCore.Razor.TagHelpers) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.AspNetCore.Razor.TagHelpers)