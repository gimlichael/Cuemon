using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Authentication;
using Cuemon.Collections.Generic;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore;
using Cuemon.Net.Http;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Authentication.Hmac
{
    public class HmacAuthorizationHeaderBuilderTest : Test
    {
        public HmacAuthorizationHeaderBuilderTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Build_ShouldGenerateValidAuthorizationHeader()
        {
            using var mw = MiddlewareTestFactory.Create();
            var context = mw.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;

            var timestamp = DateTime.Parse("2022-07-10T12:50:42.2737531Z");

            context.Request.Headers.Add(HttpHeaderNames.Host, "api.cuemon.net");
            context.Request.Headers.Add(HttpHeaderNames.Date, timestamp.ToString("R"));

            var hb = new HmacAuthorizationHeaderBuilder()
                .AddFromRequest(context.Request)
                .AddClientId("AKIAIOSFODNN7EXAMPLE")
                .AddClientSecret("wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY")
                .AddCredentialScope("some-limiting-scope");

            var h = hb.Build();

            TestOutput.WriteLine("-- HEADER --");
            TestOutput.WriteLine("");
            TestOutput.WriteLine(h.ToString());
            TestOutput.WriteLine("");
            TestOutput.WriteLine("-- BUILDER --");
            TestOutput.WriteLine("");
            TestOutput.WriteLine(hb.ToString());
            TestOutput.WriteLine("");

            Assert.Equal(h, HmacAuthorizationHeader.Create(HmacFields.Scheme, h.ToString()), DynamicEqualityComparer.Create<AuthorizationHeader>(header => Generate.HashCode32(header.ToString()), (h1, h2) => h1.ToString() == h2.ToString()));
            Assert.Equal(@"HMAC Credential=AKIAIOSFODNN7EXAMPLE/some-limiting-scope, SignedHeaders=host;date, Signature=b6ba629ce0f9a7824bf5772679c1789cd99aa6bfbbf5bf41b88cd282713b097e", h.ToString());
            Assert.Equal(@"httpRequestMethod=GET

canonicalUri=/

canonicalQueryString=

canonicalHeaders=date:Sun, 10 Jul 2022 14:50:42 GMT
host:api.cuemon.net


requestPayload=e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855

serverDateTime=2022-07-10T14:50:42.0000000Z

clientId=AKIAIOSFODNN7EXAMPLE

clientSecret=wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY

credentialScope=some-limiting-scope

signedHeaders=date;host

canonicalRequest=GET
/

date:Sun, 10 Jul 2022 14:50:42 GMT
host:api.cuemon.net

date;host
e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855

stringToSign=Sha256
2022-07-10T14:50:42.0000000Z
some-limiting-scope
fcde075ae6685470f0d4ef1dd4531b920f60bd580fc467238e0d33410edda5c6", hb.ToString(), ignoreLineEndingDifferences: true);
        }
    }
}
