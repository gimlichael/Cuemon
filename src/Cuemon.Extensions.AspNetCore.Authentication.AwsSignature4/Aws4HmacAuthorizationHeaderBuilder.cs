using System;
using System.Linq;
using System.Text;
using Cuemon.AspNetCore.Authentication;
using Cuemon.AspNetCore.Authentication.Hmac;
using Cuemon.Collections.Generic;
using Cuemon.Net;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Http;

namespace Cuemon.Extensions.AspNetCore.Authentication.AwsSignature4
{
    /// <summary>
    /// Provides a way to fluently represent a HTTP AWS4-HMAC-SHA256 Authentication header.
    /// </summary>
    /// <seealso cref="HmacAuthorizationHeaderBuilder{TAuthorizationHeaderBuilder}"/>
    /// <remarks>https://docs.aws.amazon.com/AmazonS3/latest/API/sig-v4-header-based-auth.html</remarks>
    public class Aws4HmacAuthorizationHeaderBuilder : HmacAuthorizationHeaderBuilder<Aws4HmacAuthorizationHeaderBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Aws4HmacAuthorizationHeaderBuilder"/> class.
        /// </summary>
        public Aws4HmacAuthorizationHeaderBuilder() : base(Aws4HmacFields.Scheme)
        {
            MapRelation(nameof(AddFromRequest), HmacFields.HttpMethod, HmacFields.UriPath, HmacFields.UriQuery, HmacFields.HttpHeaders, HmacFields.Payload);
            MapRelation(nameof(AddCredentialScope), HmacFields.CredentialScope, Aws4HmacFields.DateStamp, Aws4HmacFields.DateTimeStamp, Aws4HmacFields.Region, Aws4HmacFields.Service);
        }

        /// <summary>
        /// Adds the credential scope that defines the remote resource.
        /// </summary>
        /// <param name="timestamp">The <see cref="DateTime"/> value part of the credential scope.</param>
        /// <param name="region">The AWS region part of the credential scope. Default is <c>eu-west-1</c>.</param>
        /// <param name="service">The service name part of the credential scope. Default is <c>s3</c> (Simple Storage Service).</param>
        /// <param name="termination">The termination string part of the credential scope. Default is <c>aws4_request</c>.</param>
        /// <returns>A reference to this instance so that additional calls can be chained.</returns>
        /// <remarks>The following string represents the scope part of the Credential parameter for a S3 request in the eu-west-1 Region: <c>20220710/eu-west-1/s3/aws4_request</c></remarks>
        public Aws4HmacAuthorizationHeaderBuilder AddCredentialScope(DateTime timestamp, string region = "eu-west-1", string service = "s3", string termination = Aws4HmacFields.Aws4Request)
        {
            Validator.ThrowIfNullOrWhitespace(region, nameof(region));
            return AddOrUpdate(Aws4HmacFields.DateTimeStamp, timestamp.ToAwsDateTimeString())
                .AddOrUpdate(Aws4HmacFields.DateStamp, timestamp.ToAwsDateString())
                .AddOrUpdate(Aws4HmacFields.Region, region)
                .AddOrUpdate(Aws4HmacFields.Service, service)
                .AddOrUpdate(HmacFields.CredentialScope, $"{timestamp.ToAwsDateString()}/{region}/{service}/{Aws4HmacFields.Aws4Request}");
        }

        /// <summary>
        /// Adds the necessary fields that is part of an HTTP request.
        /// </summary>
        /// <param name="request">An instance of the <see cref="T:Microsoft.AspNetCore.Http.HttpRequest" /> object.</param>
        /// <returns>A reference to this instance so that additional calls can be chained.</returns>
        public override Aws4HmacAuthorizationHeaderBuilder AddFromRequest(HttpRequest request)
        {
            Validator.ThrowIfNull(request, nameof(request));
            return AddOrUpdate(HmacFields.HttpMethod, request.Method)
                .AddOrUpdate(HmacFields.UriPath, request.Path.ToUriComponent())
                .AddOrUpdate(HmacFields.UriQuery, string.Concat(request.Query.OrderBy(pair => pair.Key).Select(pair => $"{Decorator.Enclose(pair.Key).UrlEncode()}={Decorator.Enclose(pair.Value.ToString()).UrlEncode()}")))
                .AddOrUpdate(HmacFields.HttpHeaders, request.Headers.Count == 0 ? null : string.Concat(request.Headers.OrderBy(pair => pair.Key).Select(pair => $"{pair.Key.ToLowerInvariant()}:{DelimitedString.Create(pair.Value, o => o.StringConverter = s => $"{s.Trim()}{Alphanumeric.Linefeed}")}")))
                .AddOrUpdate(HmacFields.Payload, UnkeyedHashFactory.CreateCryptoSha256().ComputeHash(request.Body).ToHexadecimalString());
        }

        /// <summary>
        /// Converts the request to a standardized (canonical) format and computes a message digest.
        /// </summary>
        /// <returns>A <see cref="T:System.String" /> representation, in hexadecimal, of the computed canonical request.</returns>
        /// <remarks>https://docs.aws.amazon.com/general/latest/gr/sigv4-create-canonical-request.html</remarks>
        public override string ComputeCanonicalRequest()
        {
            ValidateData(HmacFields.UriPath, HmacFields.UriQuery, HmacFields.HttpHeaders, HmacFields.Payload);
            EnsureSignedHeaders(out var signedHeaders);
            var signedHeadersLookup = signedHeaders.Split(HmacFields.SignedHeadersDelimiter).ToList();
            var headersToSign = DelimitedString.Create(Data[HmacFields.HttpHeaders].Split(Alphanumeric.Linefeed.ToCharArray()).Where(header =>
            {
                var kvp = header.Split(HmacFields.HttpHeadersDelimiter);
                return signedHeadersLookup.Contains(kvp[0]);
            }), o => o.Delimiter = Alphanumeric.Linefeed) + Alphanumeric.Linefeed;

            var canonicalRequest = new StringBuilder(Data[HmacFields.HttpMethod])
                .Append(Alphanumeric.LinefeedChar)
                .Append(Data[HmacFields.UriPath])
                .Append(Alphanumeric.LinefeedChar)
                .Append(Data[HmacFields.UriQuery])
                .Append(Alphanumeric.LinefeedChar)
                .Append(headersToSign)
                .Append(Alphanumeric.LinefeedChar)
                .Append(signedHeaders)
                .Append(Alphanumeric.LinefeedChar)
                .Append(Data[HmacFields.Payload]).ToString();

            AddOrUpdate(HmacFields.CanonicalRequest, canonicalRequest);

            return UnkeyedHashFactory.CreateCryptoSha256().ComputeHash(canonicalRequest).ToHexadecimalString();
        }

        /// <summary>
        /// Computes the signature of this instance using a series of hash-based message authentication codes (HMACs).
        /// </summary>
        /// <returns>A <see cref="T:System.String" /> representation, in hexadecimal, of the computed signature of this instance.</returns>
        /// <remarks>https://docs.aws.amazon.com/general/latest/gr/sigv4-create-string-to-sign.html, https://docs.aws.amazon.com/general/latest/gr/sigv4-calculate-signature.html</remarks>
        public override string ComputeSignature()
        {
            ValidateData(Aws4HmacFields.DateTimeStamp, HmacFields.ClientSecret, Aws4HmacFields.DateStamp, Aws4HmacFields.Region, Aws4HmacFields.Service);
            var stringToSign = string.Concat(AuthenticationScheme,
                Alphanumeric.Linefeed,
                Data[Aws4HmacFields.DateTimeStamp],
                Alphanumeric.Linefeed,
                Decorator.Enclose(Data).GetValueOrDefault(HmacFields.CredentialScope),
                Alphanumeric.Linefeed,
                ComputeCanonicalRequest());
            var secret = Decorator.Enclose($"AWS4{Data[HmacFields.ClientSecret]}").ToByteArray();
            var dateSecret = KeyedHashFactory.CreateHmacCryptoSha256(secret).ComputeHash(Data[Aws4HmacFields.DateStamp]).GetBytes();
            var dateRegionSecret = KeyedHashFactory.CreateHmacCryptoSha256(dateSecret).ComputeHash(Data[Aws4HmacFields.Region]).GetBytes();
            var dateRegionServiceSecret = KeyedHashFactory.CreateHmacCryptoSha256(dateRegionSecret).ComputeHash(Data[Aws4HmacFields.Service]).GetBytes();
            var signingSecret = KeyedHashFactory.CreateHmacCryptoSha256(dateRegionServiceSecret).ComputeHash(Aws4HmacFields.Aws4Request).GetBytes();

            AddOrUpdate(HmacFields.StringToSign, stringToSign);

            return KeyedHashFactory.CreateHmacCryptoSha256(signingSecret).ComputeHash(stringToSign).ToHexadecimalString();
        }

        /// <summary>
        /// Builds an instance of <see cref="HmacAuthorizationHeader"/> that implements <see cref="AuthorizationHeader" />.
        /// </summary>
        /// <returns>An instance of <see cref="HmacAuthorizationHeader"/>.</returns>
        /// <remarks>https://docs.aws.amazon.com/general/latest/gr/sigv4-add-signature-to-request.html</remarks>
        public override HmacAuthorizationHeader Build()
        {
            ValidateData(HmacFields.UriPath, HmacFields.UriQuery, HmacFields.HttpHeaders, HmacFields.Payload, Aws4HmacFields.DateTimeStamp, HmacFields.ClientSecret, Aws4HmacFields.DateStamp, Aws4HmacFields.Region, Aws4HmacFields.Service);
            EnsureSignedHeaders(out var signedHeaders);
            return new Aws4HmacAuthorizationHeader(Data[HmacFields.ClientId], Decorator.Enclose(Data).GetValueOrDefault(HmacFields.CredentialScope), signedHeaders, ComputeSignature()); 
        }

        private void EnsureSignedHeaders(out string signedHeaders)
        {
            if (!Data.TryGetValue(HmacFields.SignedHeaders, out signedHeaders))
            {
                signedHeaders = "host;x-amz-content-sha256;x-amz-date";
                AddSignedHeaders(signedHeaders.Split(HmacFields.SignedHeadersDelimiter));
            }
        }
    }
}
