using System;
using Cuemon.AspNetCore.Authentication;
using Cuemon.AspNetCore.Authentication.Hmac;
using Cuemon.Collections.Generic;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Net.Http;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.AspNetCore.Authentication.AwsSignature4
{
    public class Aws4HmacAuthorizationHeaderBuilderTest : Test
    {
        public Aws4HmacAuthorizationHeaderBuilderTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Build_ShouldGenerateValidAuthorizationHeader()
        {
            using var mw = MiddlewareTestFactory.Create();
            var context = mw.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;

            var timestamp = DateTime.Parse("2022-07-10T12:50:42.2737531Z"); // <-- change this to current date/time

            context.Request.Headers.Add(HttpHeaderNames.Host, "cuemon.s3.amazonaws.com");
            context.Request.Headers.Add("x-amz-date", timestamp.ToAwsDateTimeString());
            context.Request.Headers.Add("x-amz-content-sha256", UnkeyedHashFactory.CreateCryptoSha256().ComputeHash("").ToHexadecimalString());
            context.Request.QueryString = QueryString.Create("list-type", "2");

            var headerBuilder = new Aws4HmacAuthorizationHeaderBuilder()
                .AddFromRequest(context.Request)
                .AddClientId("AKIAIOSFODNN7EXAMPLE") // <-- change this to valid access key
                .AddClientSecret("wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY") // <-- change this to valid secret
                .AddCredentialScope(timestamp);

            var header = headerBuilder.Build();

            TestOutput.WriteLine("-- HEADER --");
            TestOutput.WriteLine("");
            TestOutput.WriteLine(header.ToString());
            TestOutput.WriteLine("");
            TestOutput.WriteLine("-- BUILDER --");
            TestOutput.WriteLine("");
            TestOutput.WriteLine(headerBuilder.ToString());
            TestOutput.WriteLine("");

            Assert.Equal(header, Aws4HmacAuthorizationHeader.Create(header.ToString()), DynamicEqualityComparer.Create<AuthorizationHeader>(header => Generate.HashCode32(header.ToString()), (h1, h2) => h1.ToString() == h2.ToString()));
            Assert.Equal(@"AWS4-HMAC-SHA256 Credential=AKIAIOSFODNN7EXAMPLE/20220710/eu-west-1/s3/aws4_request, SignedHeaders=host;x-amz-content-sha256;x-amz-date, Signature=feeb4c8ba41733fadc73cba6631ddfc9a729f371206bbaa77f216a69dd5299c5", header.ToString());
            Assert.Equal(@"httpRequestMethod=GET

canonicalUri=/

canonicalQueryString=list-type=2

canonicalHeaders=host:cuemon.s3.amazonaws.com
x-amz-content-sha256:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855
x-amz-date:20220710T145042Z


requestPayload=e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855

clientId=AKIAIOSFODNN7EXAMPLE

clientSecret=wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY

iso8601BasicDateTimeFormat=20220710T145042Z

iso8601BasicDateFormat=20220710

awsRegion=eu-west-1

awsService=s3

credentialScope=20220710/eu-west-1/s3/aws4_request

signedHeaders=host;x-amz-content-sha256;x-amz-date

canonicalRequest=GET
/
list-type=2
host:cuemon.s3.amazonaws.com
x-amz-content-sha256:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855
x-amz-date:20220710T145042Z

host;x-amz-content-sha256;x-amz-date
e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855

stringToSign=AWS4-HMAC-SHA256
20220710T145042Z
20220710/eu-west-1/s3/aws4_request
4b2faff35f0b197b5498e13fac18706b6c3930c2d05168f130665400d436ac4e", headerBuilder.ToString(), ignoreLineEndingDifferences: true);
        }
    }
}
