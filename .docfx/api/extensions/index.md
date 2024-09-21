---
uid: other-projects-md
title: Other Projects
---

# Other Projects

Here is a collection of various non-Microsoft APIs that was adapted by Cuemon for .NET to provide an abundance of enhancements, new features and extension methods.

## xUnit API

[xUnit.net](https://xunit.net/) has from day one been the preferred unit test platform for Cuemon for .NET and it was only natural to extend upon xUnit for even more advanced unit test scenarios.

[!INCLUDE [availability-hybrid](../../includes/availability-hybrid.md)]

Complements: [xUnit.net](https://github.com/xunit/xunit) 🔗

> **Note**
> Since `Cuemon for .NET` has always been about extending official .NET APIs, this project is no longer maintained as part of the Cuemon assembly family. It has been moved to its own repository and is now called [Extensions for xUnit API by Codebelt](https://github.com/codebeltnet/xunit).

## Json.NET API

I am a huge fan of [Json.NET](https://www.newtonsoft.com/json) written by [James Newton-King](https://github.com/JamesNK) and the flexible architecture this JSON framework adds to the toolbelt.

So even though Microsoft decided to write their own [JSON framework](https://docs.microsoft.com/en-us/dotnet/api/system.text.json) (first seen with the release of ASP.NET Core 3), Cuemon for .NET will continue to support and extend Json.NET as the natural companion of our framework.

[!INCLUDE [availability-hybrid](../../includes/availability-hybrid.md)]

Complements: [Json.NET](https://github.com/JamesNK/Newtonsoft.Json) 🔗

## Asp.Versioning API

Our preferred way of versioning Open API/Swagger for RESTful APIs is done through [Asp.Versioning](https://github.com/dotnet/aspnet-api-versioning).

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Asp.Versioning](https://github.com/dotnet/aspnet-api-versioning) 🔗

## Swashbuckle.AspNetCore API

[Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) defined the standard for Open API/Swagger which we built upon to provide an even more powerful and efficient way of documenting your RESTful APIs.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) 🔗

## YamlDotNet API

[YamlDotNet](https://github.com/aaubry/YamlDotNet) is the most matured YAML library for .NET, why we decided to abandon own efforts to write a YAML library from scratch. That written, we happily built upon `YamlDotNet` to provide an even better developer experience when working with YAML.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [YamlDotNet](https://github.com/aaubry/YamlDotNet/wiki) 🔗

> **Note**
> Since `Cuemon for .NET` has always been about extending official .NET APIs, this project is no longer maintained as part of the Cuemon assembly family. It has been moved to its own repository and is now called [Extensions for YamlDotNet API by Codebelt](https://github.com/codebeltnet/yamldotnet).

## AWS Signature API

Providing an additional HTTP HMAC Authentication header that provides a fluent way to use [AWS Signature Version 4](https://docs.aws.amazon.com/general/latest/gr/reference-for-signature-version-4.html) was a fun challenge to write and add to Cuemon for .NET .

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

Complements: [Authenticating Requests (AWS Signature Version 4)](https://docs.aws.amazon.com/AmazonS3/latest/API/sig-v4-authenticating-requests.html) 🔗