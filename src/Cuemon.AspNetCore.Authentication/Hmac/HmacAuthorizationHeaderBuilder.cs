using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Cuemon.AspNetCore.Authentication.Digest;
using Cuemon.Collections.Generic;
using Cuemon.Net;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Authentication.Hmac
{
    /// <summary>
    /// Provides a way to fluently represent a HTTP HMAC Authentication header.
    /// Inspired by AWS Signature Version 4 (https://docs.aws.amazon.com/AmazonS3/latest/API/sigv4-auth-using-authorization-header.html, https://docs.aws.amazon.com/general/latest/gr/sigv4_signing.html).
    /// </summary>
    public class HmacAuthorizationHeaderBuilder : AuthorizationHeaderBuilder<HmacAuthorizationHeader, HmacAuthorizationHeaderBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DigestAuthorizationHeaderBuilder"/> class.
        /// </summary>
        /// <param name="authenticationScheme">The name of the authentication scheme. Default is HMAC.</param>
        /// <param name="hmacAlgorithm">The algorithm to use when computing the final signature of the HMAC Authentication header.</param>
        /// <param name="algorithm">The algorithm to use when computing in-between signatures as part of the final signing of the HMAC Authentication header.</param>
        public HmacAuthorizationHeaderBuilder(string authenticationScheme = HmacAuthorizationHeader.Scheme, KeyedCryptoAlgorithm hmacAlgorithm = KeyedCryptoAlgorithm.HmacSha256, UnkeyedCryptoAlgorithm algorithm = UnkeyedCryptoAlgorithm.Sha256) : base(authenticationScheme)
        {
            HmacAlgorithm = hmacAlgorithm;
            Algorithm = algorithm;
            MapRelation(nameof(AddCredentialScope), HmacFields.CredentialScope);
            MapRelation(nameof(AddClientId), HmacFields.ClientId);
            MapRelation(nameof(AddClientSecret), HmacFields.ClientSecret);
            MapRelation(nameof(AddFromRequest), HmacFields.UriPath, HmacFields.UriQuery, HmacFields.HttpHeaders, HmacFields.Payload, HmacFields.ServerDateTime);
        }

        /// <summary>
        /// Gets the non-keyed algorithm of the HTTP HMAC Authentication.
        /// </summary>
        /// <value>The non-keyed algorithm of the HTTP HMAC Authentication.</value>
        public UnkeyedCryptoAlgorithm Algorithm { get; }

        /// <summary>
        /// Gets the keyed algorithm of the HTTP HMAC Authentication.
        /// </summary>
        /// <value>The keyed algorithm of the HTTP HMAC Authentication.</value>
        public KeyedCryptoAlgorithm HmacAlgorithm { get; }

        
        /// <summary>
        /// Adds the credential scope that defines the remote resource.
        /// </summary>
        /// <param name="credentialScope">The credential scope that defines the remote resource.</param>
        /// <returns>An <see cref="HmacAuthorizationHeaderBuilder"/> that can be used to further build the HTTP HMAC Authentication header.</returns>
        public HmacAuthorizationHeaderBuilder AddCredentialScope(string credentialScope)
        {
            return AddOrUpdate(HmacFields.CredentialScope, credentialScope);
        }

        /// <summary>
        /// Adds the client identifier that is the public key of the signing process.
        /// </summary>
        /// <param name="clientId">The client identifier that is the public key of the signing process.</param>
        /// <returns>An <see cref="HmacAuthorizationHeaderBuilder"/> that can be used to further build the HTTP HMAC Authentication header.</returns>
        public HmacAuthorizationHeaderBuilder AddClientId(string clientId)
        {
            return AddOrUpdate(HmacFields.ClientId, clientId);
        }

        /// <summary>
        /// Adds the client secret that is the private key of the signing process.
        /// </summary>
        /// <param name="clientSecret">The client secret that is the private key of the signing process.</param>
        /// <returns>An <see cref="HmacAuthorizationHeaderBuilder"/> that can be used to further build the HTTP HMAC Authentication header.</returns>
        public HmacAuthorizationHeaderBuilder AddClientSecret(string clientSecret)
        {
            return AddOrUpdate(HmacFields.ClientSecret, clientSecret);
        }

        /// <summary>
        /// Adds the necessary fields that is part of an HTTP request.
        /// </summary>
        /// <param name="request">An instance of the <see cref="HttpRequest"/> object.</param>
        /// <returns>An <see cref="HmacAuthorizationHeaderBuilder"/> that can be used to further build the HTTP HMAC Authentication header.</returns>
        public HmacAuthorizationHeaderBuilder AddFromRequest(HttpRequest request)
        {
            Validator.ThrowIfNull(request, nameof(request));
            return AddOrUpdate(HmacFields.HttpMethod, request.Method)
                .AddOrUpdate(HmacFields.UriPath, request.Path.ToUriComponent())
                .AddOrUpdate(HmacFields.UriQuery, string.Concat(request.Query.OrderBy(pair => pair.Key).Select(pair => $"{Decorator.Enclose(pair.Value.ToString()).UrlEncode()}")))
                .AddOrUpdate(HmacFields.HttpHeaders, request.Headers.Count == 0 ? null : string.Concat(request.Headers.OrderBy(pair => pair.Key).Select(pair => $"{pair.Key.ToLowerInvariant()}:{DelimitedString.Create(pair.Value, o => o.StringConverter = s => $"{s.Trim()}{Alphanumeric.Linefeed}")}")))
                .AddOrUpdate(HmacFields.Payload, UnkeyedHashFactory.CreateCrypto(Algorithm).ComputeHash(request.Body).ToHexadecimalString())
                .AddOrUpdate(HmacFields.ServerDateTime, DateTime.Parse(request.Headers[HeaderNames.Date].ToString(), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind).ToString("O"));
        }

        /// <summary>
        /// Adds the headers that will be part of the signing process.
        /// </summary>
        /// <param name="signedHeaders">The headers that will be part of the signing process. Default is <c>host</c> and <c>date</c></param>
        /// <returns>An <see cref="HmacAuthorizationHeaderBuilder"/> that can be used to further build the HTTP HMAC Authentication header.</returns>
        public HmacAuthorizationHeaderBuilder AddSignedHeaders(params string[] signedHeaders)
        {
            if (signedHeaders == null) { return this; }
            return AddOrUpdate(HmacFields.SignedHeaders, DelimitedString.Create(signedHeaders, o =>
            {
                o.Delimiter = ";";
                o.StringConverter = s => s.ToLowerInvariant();
            }));
        }

        /// <summary>
        /// Converts the request to a standardized (canonical) format and computes a message digest using <see cref="Algorithm"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in hexadecimal, of the computed canonical request.</returns>
        public virtual string ComputeCanonicalRequest()
        {
            ValidateData(HmacFields.UriPath, HmacFields.UriQuery, HmacFields.HttpHeaders, HmacFields.Payload);
            EnsureSignedHeaders(out var signedHeaders);
            var signedHeadersLookup = signedHeaders.Split(';').ToList();
            var headersToSign = DelimitedString.Create(Data[HmacFields.HttpHeaders].Split(Alphanumeric.Linefeed.ToCharArray()).Where(header =>
            {
                var kvp = header.Split(':');
                return signedHeadersLookup.Contains(kvp[0]);
            }), o => o.Delimiter = Alphanumeric.Linefeed) + Alphanumeric.Linefeed;

            var stringToSign = new StringBuilder(Data[HmacFields.HttpMethod])
                .Append(Alphanumeric.Linefeed)
                .Append(Data[HmacFields.UriPath])
                .Append(Alphanumeric.Linefeed)
                .Append(Data[HmacFields.UriQuery])
                .Append(Alphanumeric.Linefeed)
                .Append(headersToSign)
                .Append(Alphanumeric.Linefeed)
                .Append(signedHeaders)
                .Append(Alphanumeric.Linefeed)
                .Append(Data[HmacFields.Payload]).ToString();

            return UnkeyedHashFactory.CreateCrypto(Algorithm).ComputeHash(stringToSign).ToHexadecimalString();
        }

        /// <summary>
        /// Computes the signature of this instance using a series of hash-based message authentication codes (HMACs) using <see cref="HmacAlgorithm"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> representation, in hexadecimal, of the computed signature of this instance.</returns>
        public virtual string ComputeSignature()
        {
            ValidateData(HmacFields.ServerDateTime, HmacFields.ClientSecret);
            var secret = Decorator.Enclose(Data[HmacFields.ClientSecret]).ToByteArray();
            var stringToSign = string.Concat(Algorithm,
                Alphanumeric.Linefeed,
                Data[HmacFields.ServerDateTime],
                Alphanumeric.Linefeed,
                Decorator.Enclose(Data).GetValueOrDefault(HmacFields.CredentialScope),
                Alphanumeric.Linefeed,
                ComputeCanonicalRequest());
            var date = DateTime.Parse(Data[HmacFields.ServerDateTime], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind).Date.ToString("yyyyMMdd");
            var dateSecret = KeyedHashFactory.CreateHmacCrypto(secret, HmacAlgorithm).ComputeHash(date).GetBytes();
            return KeyedHashFactory.CreateHmacCrypto(dateSecret, HmacAlgorithm).ComputeHash(stringToSign).ToHexadecimalString();
        }

        /// <summary>
        /// Builds an instance of <see cref="HmacAuthorizationHeader"/> that implements <see cref="AuthorizationHeader" />.
        /// </summary>
        /// <returns>An instance of <see cref="HmacAuthorizationHeader"/>.</returns>
        public override HmacAuthorizationHeader Build()
        {
            ValidateData(HmacFields.ClientId, HmacFields.ServerDateTime, HmacFields.ClientSecret, HmacFields.UriPath, HmacFields.UriQuery, HmacFields.HttpHeaders, HmacFields.Payload);
            EnsureSignedHeaders(out var signedHeaders);
            return new HmacAuthorizationHeader(Data[HmacFields.ClientId], Decorator.Enclose(Data).GetValueOrDefault(HmacFields.CredentialScope), signedHeaders, ComputeSignature(), AuthenticationScheme); 
        }

        private void EnsureSignedHeaders(out string signedHeaders)
        {
            if (!Data.TryGetValue(HmacFields.SignedHeaders, out signedHeaders))
            {
                signedHeaders = "host;date";
                AddSignedHeaders(signedHeaders);
            }
        }
    }
}