---
uid: extensions-xunit-md
title: Extensions for xUnit API
---
# Extensions for xUnit API

Using [xUnit.net](https://xunit.net/) as the preferred unit test platform of Cuemon for .NET, it was only natural to extend upon xUnit for more advanced unit test scenarios.

+ Xunit
  + [Cuemon.Extensions.Xunit.Hosting](#cuemonextensionsxunithosting)
  + [Cuemon.Extensions.Xunit.Hosting.AspNetCore](#cuemonextensionsxunithostingaspnetcore)
  + Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http
  + Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http.Features
  + [Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc](#cuemonextensionsxunithostingaspnetcoremvc)
+ Xunit.Abstractions
  + [Cuemon.Extensions.Xunit](#cuemonextensionsxunit)

## Cuemon.Extensions.Xunit

The Cuemon.Extensions.Xunit namespace contains types that provides a uniform way of doing unit testing. The namespace relates to the Xunit.Abstractions namespace.

Availability: **.NET Standard 2.0, .NET 5.0**

Assembly: Cuemon.Extensions.Xunit

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.Extensions.Xunit) | [Productivity](https://www.nuget.org/packages/Cuemon.Extensions.Xunit.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.Xunit) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.Xunit) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.Xunit)

## Cuemon.Extensions.Xunit.Hosting

The Cuemon.Extensions.Xunit.Hosting namespace contains types that provides a uniform way of doing unit testing that is used in conjunction with Microsoft Dependency Injection. The namespace relates to the Xunit.Abstractions namespace.

Availability: **.NET Standard 2.0, .NET Core 3.0, .NET 5.0**

Assembly: Cuemon.Extensions.Xunit.Hosting

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.Extensions.Xunit.Hosting) | [Productivity](https://www.nuget.org/packages/Cuemon.Extensions.Xunit.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.Xunit.Hosting) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.Xunit.Hosting) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.Xunit.Hosting)

## Cuemon.Extensions.Xunit.Hosting.AspNetCore

The Cuemon.Extensions.Xunit.Hosting.AspNetCore namespace contains types that provides a uniform way of doing unit testing that depends on ASP.NET Core and used in conjunction with Microsoft Dependency Injection. The namespace relates to the Microsoft.AspNetCore.TestHost namespace.

Availability: **.NET Core 3.1, .NET 5.0**

Assembly: Cuemon.Extensions.Xunit.Hosting.AspNetCore

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.Extensions.Xunit.Hosting.AspNetCore) | [Productivity](https://www.nuget.org/packages/Cuemon.Extensions.Xunit.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.Xunit.Hosting.AspNetCore) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.Xunit.Hosting.AspNetCore) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.Xunit.Hosting.AspNetCore)

## Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc

The Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc namespace contains types that provides a uniform way of doing unit testing that depends on ASP.NET Core MVC and used in conjunction with Microsoft Dependency Injection. The namespace relates to the Microsoft.AspNetCore.Mvc.Testing namespace.

Availability: **.NET Core 3.1, .NET 5.0**

Assembly: Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc

NuGet packages: [Focus](https://www.nuget.org/packages/Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc) | [Productivity](https://www.nuget.org/packages/Cuemon.Extensions.Xunit.App)

Github: [Development](https://github.com/gimlichael/Cuemon/tree/development/src/Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc) | [Release](https://github.com/gimlichael/Cuemon/tree/release/src/Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc) | [Master](https://github.com/gimlichael/Cuemon/tree/master/src/Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc)