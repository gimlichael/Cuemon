## About

An open-source family of assemblies (MIT license) that targets and complements the Microsoft .NET platform (.NET 7, .NET 6, .NET Core 3.1, .NET Standard 2, Universal Windows Platform and .NET Framework 4.6.1 and newer) by providing vast ways of possibilities for all breeds of coders, programmers, developers and the likes thereof.

It is, by heart, free, flexible and built to extend and boost your agile codebelt.

## **Cuemon.Extensions.AspNetCore.Authentication.AwsSignature4** for .NET

The `Cuemon.Extensions.AspNetCore.Authentication.AwsSignature4` namespace complements the Cuemon.AspNetCore.Authentication namespace while providing a way making and signing HTTP requests in the context of specific AWS services using Signature Version 4.

More documentation available at [Cuemon for .NET documentation](https://docs.cuemon.net/api/extensions/awssignature4/Cuemon.Extensions.AspNetCore.Authentication.AwsSignature4.html).

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

## Related Packages

* [Cuemon.AspNetCore](https://www.nuget.org/packages/Cuemon.AspNetCore/) 📦
* [Cuemon.AspNetCore.App](https://www.nuget.org/packages/Cuemon.AspNetCore.App/) 🏭
* [Cuemon.AspNetCore.Authentication](https://www.nuget.org/packages/Cuemon.AspNetCore.Authentication/) 📦
* [Cuemon.AspNetCore.Mvc](https://www.nuget.org/packages/Cuemon.AspNetCore.Mvc/) 📦
* [Cuemon.AspNetCore.Razor.TagHelpers](https://www.nuget.org/packages/Cuemon.AspNetCore.Razor.TagHelpers/) 📦
* [Cuemon.Core](https://www.nuget.org/packages/Cuemon.Core/) 📦
* [Cuemon.Core.App](https://www.nuget.org/packages/Cuemon.Core.App/) 🏭
* [Cuemon.Data](https://www.nuget.org/packages/Cuemon.Data/) 📦
* [Cuemon.Data.Integrity](https://www.nuget.org/packages/Cuemon.Data.Integrity/) 📦
* [Cuemon.Data.SqlClient](https://www.nuget.org/packages/Cuemon.Data.SqlClient/) 📦
* [Cuemon.Diagnostics](https://www.nuget.org/packages/Cuemon.Diagnostics/) 📦
* [Cuemon.Extensions.AspNetCore](https://www.nuget.org/packages/Cuemon.Extensions.AspNetCore/) 📦
* [Cuemon.Extensions.AspNetCore.Authentication](https://www.nuget.org/packages/Cuemon.Extensions.AspNetCore.Authentication/) 📦
* [Cuemon.Extensions.AspNetCore.Authentication.AwsSignature4](https://www.nuget.org/packages/Cuemon.Extensions.AspNetCore.Authentication.AwsSignature4/) 📦
* [Cuemon.Extensions.AspNetCore.Mvc](https://www.nuget.org/packages/Cuemon.Extensions.AspNetCore.Mvc/) 📦
* [Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json](https://www.nuget.org/packages/Cuemon.Extensions.AspNetCore.Mvc.Formatters.Newtonsoft.Json/) 📦
* [Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json](https://www.nuget.org/packages/Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text.Json/) 📦
* [Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml](https://www.nuget.org/packages/Cuemon.Extensions.AspNetCore.Mvc.Formatters.Xml/) 📦
* [Cuemon.Extensions.AspNetCore.Mvc.RazorPages](https://www.nuget.org/packages/Cuemon.Extensions.AspNetCore.Mvc.RazorPages/) 📦
* [Cuemon.Extensions.Collections.Generic](https://www.nuget.org/packages/Cuemon.Extensions.Collections.Generic/) 📦
* [Cuemon.Extensions.Collections.Specialized](https://www.nuget.org/packages/Cuemon.Extensions.Collections.Specialized/) 📦
* [Cuemon.Extensions.Core](https://www.nuget.org/packages/Cuemon.Extensions.Core/) 📦
* [Cuemon.Extensions.Data](https://www.nuget.org/packages/Cuemon.Extensions.Data/) 📦
* [Cuemon.Extensions.Data.Integrity](https://www.nuget.org/packages/Cuemon.Extensions.Data.Integrity/) 📦
* [Cuemon.Extensions.DependencyInjection](https://www.nuget.org/packages/Cuemon.Extensions.DependencyInjection/) 📦
* [Cuemon.Extensions.Diagnostics](https://www.nuget.org/packages/Cuemon.Extensions.Diagnostics/) 📦
* [Cuemon.Extensions.Asp.Versioning](https://www.nuget.org/packages/Cuemon.Extensions.Asp.Versioning/) 📦
* [Cuemon.Extensions.Hosting](https://www.nuget.org/packages/Cuemon.Extensions.Hosting/) 📦
* [Cuemon.Extensions.IO](https://www.nuget.org/packages/Cuemon.Extensions.IO/) 📦
* [Cuemon.Extensions.Net](https://www.nuget.org/packages/Cuemon.Extensions.Net/) 📦
* [Cuemon.Extensions.Newtonsoft.Json](https://www.nuget.org/packages/Cuemon.Extensions.Newtonsoft.Json/) 📦
* [Cuemon.Extensions.Newtonsoft.Json.App](https://www.nuget.org/packages/Cuemon.Extensions.Newtonsoft.Json.App/) 🏭
* [Cuemon.Extensions.Reflection](https://www.nuget.org/packages/Cuemon.Extensions.Reflection/) 📦
* [Cuemon.Extensions.Runtime.Caching](https://www.nuget.org/packages/Cuemon.Extensions.Runtime.Caching/) 📦
* [Cuemon.Extensions.Swashbuckle.AspNetCore](https://www.nuget.org/packages/Cuemon.Extensions.Swashbuckle.AspNetCore/) 📦
* [Cuemon.Extensions.Text](https://www.nuget.org/packages/Cuemon.Extensions.Text/) 📦
* [Cuemon.Extensions.Text.Json](https://www.nuget.org/packages/Cuemon.Extensions.Text.Json/) 📦
* [Cuemon.Extensions.Threading](https://www.nuget.org/packages/Cuemon.Extensions.Threading/) 📦
* [Cuemon.Extensions.Xml](https://www.nuget.org/packages/Cuemon.Extensions.Xml/) 📦
* [Cuemon.Extensions.Xunit](https://www.nuget.org/packages/Cuemon.Extensions.Xunit/) 📦
* [Cuemon.Extensions.Xunit.App](https://www.nuget.org/packages/Cuemon.Extensions.Xunit.App/) 🏭
* [Cuemon.Extensions.Xunit.Hosting](https://www.nuget.org/packages/Cuemon.Extensions.Xunit.Hosting/) 📦
* [Cuemon.Extensions.Xunit.Hosting.AspNetCore](https://www.nuget.org/packages/Cuemon.Extensions.Xunit.Hosting.AspNetCore/) 📦
* [Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc](https://www.nuget.org/packages/Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc/) 📦
* [Cuemon.IO](https://www.nuget.org/packages/Cuemon.IO/) 📦
* [Cuemon.Net](https://www.nuget.org/packages/Cuemon.Net/) 📦
* [Cuemon.Resilience](https://www.nuget.org/packages/Cuemon.Resilience/) 📦
* [Cuemon.Runtime.Caching](https://www.nuget.org/packages/Cuemon.Runtime.Caching/) 📦
* [Cuemon.Security.Cryptography](https://www.nuget.org/packages/Cuemon.Security.Cryptography/) 📦
* [Cuemon.Threading](https://www.nuget.org/packages/Cuemon.Threading/) 📦
* [Cuemon.Xml](https://www.nuget.org/packages/Cuemon.Xml/) 📦