---
uid: extensions-aws-signature-md
title: Extensions for AWS Signature API
---
# Extensions for AWS Signature API

Cuemon for .NET provides a fluent way to use [AWS Signature Version 4](https://docs.aws.amazon.com/general/latest/gr/reference-for-signature-version-4.html) in your code.

## Cuemon.Extensions.AspNetCore.Authentication.AwsSignature4

The Cuemon.Extensions.AspNetCore.Authentication.AwsSignature4 namespace complements the Cuemon.AspNetCore.Authentication namespace while providing a way making and signing HTTP requests in the context of specific AWS services using Signature Version 4.

[!INCLUDE [availability-modern](../../../includes/availability-modern.md)]

### CSharp Example
```csharp
using var mw = MiddlewareTestFactory.Create();
var context = mw.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;

var timestamp = DateTime.Parse("2022-07-10T12:50:42.2737531Z"); // <-- change this to valid date/time

context.Request.Headers.Add(HttpHeaderNames.Host, "cuemon.s3.amazonaws.com");
context.Request.Headers.Add("x-amz-date", timestamp.ToAwsDateTimeString());
context.Request.Headers.Add("x-amz-content-sha256", UnkeyedHashFactory.CreateCryptoSha256().ComputeHash("").ToHexadecimalString());
context.Request.QueryString = QueryString.Create("list-type", "2");

var headerBuilder = new Aws4HmacAuthorizationHeaderBuilder()
    .AddFromRequest(context.Request)
    .AddClientId("AKIAIOSFODNN7EXAMPLE") // <-- change this to valid access key
    .AddClientSecret("wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY") // <-- change this to valid secret
    .AddCredentialScope(timestamp);

var header = headerBuilder.Build().ToString(); // <-- send this AWS
```

### cURL Example
```powershell
curl --location --request GET 'https://cuemon.s3.amazonaws.com/?list-type=2' --header 'Authorization: AWS4-HMAC-SHA256 Credential=AKIAIOSFODNN7EXAMPLE/20220710/eu-west-1/s3/aws4_request, SignedHeaders=host;x-amz-content-sha256;x-amz-date, Signature=feeb4c8ba41733fadc73cba6631ddfc9a729f371206bbaa77f216a69dd5299c5' --header 'x-amz-date: 20220710T145042Z' --header 'x-amz-content-sha256: e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855'
```