using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using Cuemon.Collections.Generic;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Authentication
{
    /// <summary>
    /// Provides a way to fluently represent a HTTP Digest Access Authentication header.
    /// </summary>
    public class DigestHeaderBuilder
    {
        private readonly IDictionary<string, string> _dictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="DigestHeaderBuilder"/> class.
        /// </summary>
        /// <param name="algorithm">The algorithm to use when either computing HA1, HA2 and/or RESPONSE value(s).</param>
        /// <param name="init">The dictionary to initialize this instance from.</param>
        /// <remarks>Allowed values for <paramref name="algorithm"/> are: <see cref="UnkeyedCryptoAlgorithm.Md5"/>, <see cref="UnkeyedCryptoAlgorithm.Sha256"/> and <see cref="UnkeyedCryptoAlgorithm.Sha512"/>.</remarks>
        public DigestHeaderBuilder(UnkeyedCryptoAlgorithm algorithm = UnkeyedCryptoAlgorithm.Sha256, IDictionary<string, string> init = null)
        {
            Validator.ThrowIfEqual(algorithm, UnkeyedCryptoAlgorithm.Sha1, nameof(algorithm));
            Validator.ThrowIfEqual(algorithm, UnkeyedCryptoAlgorithm.Sha384, nameof(algorithm));
            Algorithm = algorithm;
            _dictionary = init == null ? new Dictionary<string, string>() : new Dictionary<string, string>(init);
        }

        /// <summary>
        /// Gets the algorithm of the HTTP Digest Access Authentication.
        /// </summary>
        /// <value>The algorithm of the HTTP Digest Access Authentication.</value>
        public UnkeyedCryptoAlgorithm Algorithm { get; }

        /// <summary>
        /// Gets the name of the authentication scheme.
        /// </summary>
        /// <value>The name of the authentication scheme.</value>
        public string AuthenticationScheme => "Digest";

        /// <summary>
        /// Associates the <see cref="DigestHeaders.UserName"/> field with the specified <paramref name="username"/>.
        /// </summary>
        /// <param name="username">The username to use in the authentication process.</param>
        /// <returns>An <see cref="DigestHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
        public DigestHeaderBuilder AddUserName(string username)
        {
            Validator.ThrowIfNullOrWhitespace(username, nameof(username));
            Decorator.Enclose(_dictionary).TryAdd(DigestHeaders.UserName, username);
            return this;
        }

        /// <summary>
        /// Associates the <see cref="DigestHeaders.Realm"/> field with the specified <paramref name="realm"/>.
        /// </summary>
        /// <param name="realm">The realm to use in the authentication process.</param>
        /// <returns>An <see cref="DigestHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
        public DigestHeaderBuilder AddRealm(string realm)
        {
            Decorator.Enclose(_dictionary).TryAdd(DigestHeaders.Realm, realm);
            return this;
        }

        /// <summary>
        /// Associates the <see cref="DigestHeaders.DigestUri"/> field with the specified <paramref name="digestUri"/>.
        /// </summary>
        /// <param name="digestUri">The effective request URI to use in the authentication process.</param>
        /// <returns>An <see cref="DigestHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
        public DigestHeaderBuilder AddUri(string digestUri)
        {
            Validator.ThrowIfNullOrWhitespace(digestUri, nameof(digestUri));
            Decorator.Enclose(_dictionary).TryAdd(DigestHeaders.DigestUri, digestUri);
            return this;
        }

        /// <summary>
        /// Associates the <see cref="DigestHeaders.Nonce"/> field with the specified <paramref name="nonce"/>.
        /// </summary>
        /// <param name="nonce">The cryptographic nonce to use in the authentication process.</param>
        /// <returns>An <see cref="DigestHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
        public DigestHeaderBuilder AddNonce(string nonce)
        {
            Validator.ThrowIfNullOrWhitespace(nonce, nameof(nonce));
            Decorator.Enclose(_dictionary).TryAdd(DigestHeaders.Nonce, nonce);
            return this;
        }

        /// <summary>
        /// Associates the <see cref="DigestHeaders.NonceCount"/> field with the specified <paramref name="nonceCount"/>.
        /// </summary>
        /// <param name="nonceCount">The count of the number of requests to use in the authentication process.</param>
        /// <returns>An <see cref="DigestHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
        public DigestHeaderBuilder AddNc(int nonceCount)
        {
            Validator.ThrowIfLowerThan(nonceCount, 0, nameof(nonceCount));
            Decorator.Enclose(_dictionary).TryAdd(DigestHeaders.NonceCount, nonceCount.ToString("x8"));
            return this;
        }

        /// <summary>
        /// Associates the <see cref="DigestHeaders.ClientNonce"/> field with the specified <paramref name="clientNonce"/>.
        /// </summary>
        /// <param name="clientNonce">The cryptographic client nonce to use in the authentication process.</param>
        /// <returns>An <see cref="DigestHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
        public DigestHeaderBuilder AddCnonce(string clientNonce = null)
        {
            if (clientNonce == null) { clientNonce = Generate.RandomString(32); }
            Decorator.Enclose(_dictionary).TryAdd(DigestHeaders.ClientNonce, clientNonce);
            return this;
        }

        /// <summary>
        /// Associates the <see cref="DigestHeaders.QualityOfProtection"/> field with "auth".
        /// </summary>
        /// <returns>An <see cref="DigestHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
        public DigestHeaderBuilder AddQopAuthentication()
        {
            Decorator.Enclose(_dictionary).TryAdd(DigestHeaders.QualityOfProtection, "auth");
            return this;
        }

        /// <summary>
        /// Associates the <see cref="DigestHeaders.QualityOfProtection"/> field with "auth-int".
        /// </summary>
        /// <returns>An <see cref="DigestHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
        public DigestHeaderBuilder AddQopAuthenticationIntegrity()
        {
            Decorator.Enclose(_dictionary).TryAdd(DigestHeaders.QualityOfProtection, "auth-int");
            return this;
        }

        /// <summary>
        /// Associates the <see cref="DigestHeaders.Response"/> field with the specified <paramref name="response"/>.
        /// </summary>
        /// <param name="response">The response to use in the authentication process.</param>
        /// <returns>An <see cref="DigestHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
        public DigestHeaderBuilder AddResponse(string response)
        {
            Validator.ThrowIfNullOrWhitespace(response, nameof(response));
            Decorator.Enclose(_dictionary).TryAdd(DigestHeaders.Response, response);
            return this;
        }

        /// <summary>
        /// Associates any Digest fields found in the HTTP WWW-Authenticate header from the specified <paramref name="response"/>.
        /// </summary>
        /// <param name="response">An instance of <see cref="HttpResponse"/>.</param>
        /// <returns>An <see cref="DigestHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
        public DigestHeaderBuilder AddFromWwwAuthenticateHeader(HttpResponse response)
        {
            Validator.ThrowIfNull(response, nameof(response));
            return AddFromDigestHeader(response.Headers[HeaderNames.WWWAuthenticate], true);
        }

        /// <summary>
        /// Associates any Digest fields found in the HTTP Authorization header from the specified <paramref name="request"/>.
        /// </summary>
        /// <param name="request">An instance of <see cref="HttpRequest"/>.</param>
        /// <returns>An <see cref="DigestHeaderBuilder"/> that can be used to further build the HTTP Digest Access Authentication header.</returns>
        public DigestHeaderBuilder AddFromAuthorizationHeader(HttpRequest request)
        {
            Validator.ThrowIfNull(request, nameof(request));
            return AddFromDigestHeader(request.Headers[HeaderNames.Authorization]);
        }

        /// <summary>
        /// Associates any Digest fields found in the <paramref name="header"/>.
        /// </summary>
        /// <param name="header">The header containing Digest fields.</param>
        /// <param name="skipQop">if set to <c>true</c> and <see cref="DigestHeaders.QualityOfProtection"/> is part of the <paramref name="header"/>, this field is not being added to this instance.</param>
        /// <returns>DigestHeaderBuilder.</returns>
        public DigestHeaderBuilder AddFromDigestHeader(string header, bool skipQop = false)
        {
            Validator.ThrowIfNullOrWhitespace(header, nameof(header));
            Validator.ThrowIfFalse(() => header.StartsWith(AuthenticationScheme), nameof(header), $"Header did not start with {AuthenticationScheme}.");
            var headerWithoutScheme = header.Remove(0, AuthenticationScheme.Length + 1);

            var fields = DelimitedString.Split(headerWithoutScheme);
            foreach (var field in fields)
            {
                var kvp = DelimitedString.Split(field, o => o.Delimiter = "=");
                var key = kvp[0].Trim();
                var value = kvp[1].Trim('"');
                if (skipQop && key == DigestHeaders.QualityOfProtection) { continue; }
                Decorator.Enclose(_dictionary).TryAdd(key, value);
            }
            return this;
        }

        /// <summary>
        /// Converts this instance to an <see cref="ImmutableDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <returns>An <see cref="ImmutableDictionary{TKey,TValue}"/> equivalent of this instance.</returns>
        public ImmutableDictionary<string, string> ToImmutableDictionary()
        {
            return _dictionary.ToImmutableDictionary();
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="UnkeyedCryptoAlgorithm"/> hash value of the required values for the HTTP Digest access authentication HA1.
        /// </summary>
        /// <param name="password">The password to include in the HA1 computed value.</param>
        /// <returns>A <see cref="string"/> in the format of H(<see cref="DigestHeaders.UserName"/>:<see cref="DigestHeaders.Realm"/>:<paramref name="password"/>). H is determined by <see cref="Algorithm"/>.</returns>
        public string ComputeHash1(string password)
        {
            ValidateFields(DigestHeaders.UserName, DigestHeaders.Realm);
            return UnkeyedHashFactory.CreateCrypto(Algorithm).ComputeHash(string.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}", _dictionary[DigestHeaders.UserName], _dictionary[DigestHeaders.Realm], password), o =>
            {
                o.Encoding = Encoding.UTF8;
            }).ToHexadecimalString();
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="UnkeyedCryptoAlgorithm"/> hash value of the required values for the HTTP Digest access authentication HA2.
        /// </summary>
        /// <param name="method">The HTTP method to include in the HA2 computed value.</param>
        /// <param name="entityBody">The entity body to apply in the signature when qop is set to auth-int.</param>
        /// <returns>A <see cref="string"/> in the format of H(<paramref name="method"/>:<see cref="DigestHeaders.DigestUri"/>) OR H(<paramref name="method"/>:<see cref="DigestHeaders.DigestUri"/>:H(<paramref name="entityBody"/>)). H is determined by <see cref="Algorithm"/>.</returns>
        public string ComputeHash2(string method, string entityBody = null)
        {
            ValidateFields(DigestHeaders.QualityOfProtection, DigestHeaders.DigestUri);
            var qop = _dictionary[DigestHeaders.QualityOfProtection];
            var hasIntegrityProtection = qop.Equals("auth-int", StringComparison.OrdinalIgnoreCase);
            if (hasIntegrityProtection && entityBody == null) { throw new ArgumentNullException(nameof(entityBody), "The entity body cannot be null when qop is set to auth-int."); }

            var hashFields =  !hasIntegrityProtection
                ? FormattableString.Invariant($"{method}:{_dictionary[DigestHeaders.DigestUri]}") 
                : FormattableString.Invariant($"{method}:{_dictionary[DigestHeaders.DigestUri]}:{UnkeyedHashFactory.CreateCrypto(Algorithm).ComputeHash(entityBody, o => o.Encoding = Encoding.UTF8).ToHexadecimalString()}");
            return UnkeyedHashFactory.CreateCrypto(Algorithm).ComputeHash(hashFields, o =>
            {
                o.Encoding = Encoding.UTF8;
            }).ToHexadecimalString();
        }
        
        /// <summary>
        /// Computes a by parameter defined <see cref="UnkeyedCryptoAlgorithm"/> hash value of the required values for the HTTP Digest access authentication RESPONSE.
        /// </summary>
        /// <param name="hash1">The HA1 to include in the RESPONSE computed value.</param>
        /// <param name="hash2">The HA2 to include in the RESPONSE computed value.</param>
        /// <returns>A <see cref="string"/> in the format of H(<paramref name="hash1"/>:<see cref="DigestHeaders.Nonce"/>:<see cref="DigestHeaders.NonceCount"/>:<see cref="DigestHeaders.ClientNonce"/>:<see cref="DigestHeaders.QualityOfProtection"/>:<paramref name="hash2"/>). H is determined by <see cref="Algorithm"/>.</returns>
        public string ComputeResponse(string hash1, string hash2)
        {
            ValidateFields(DigestHeaders.Nonce, DigestHeaders.NonceCount, DigestHeaders.ClientNonce, DigestHeaders.QualityOfProtection);
            return UnkeyedHashFactory.CreateCrypto(Algorithm).ComputeHash(FormattableString.Invariant($"{hash1}:{_dictionary[DigestHeaders.Nonce]}:{_dictionary[DigestHeaders.NonceCount]}:{_dictionary[DigestHeaders.ClientNonce]}:{_dictionary[DigestHeaders.QualityOfProtection]}:{hash2}"), o =>
            {
                o.Encoding = Encoding.UTF8;
            }).ToHexadecimalString();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            var header = DelimitedString.Create(_dictionary, o =>
            {
                o.Delimiter = ", ";
                o.StringConverter = kvp => $"{kvp.Key}=\"{kvp.Value}\"";
            });
            return $"{AuthenticationScheme} {header}";
        }

        private void ValidateFields(params string[] requiredFieldNames)
        {
            foreach (var requiredFieldName in requiredFieldNames)
            {
                if (!_dictionary.ContainsKey(requiredFieldName)) { throw new ArgumentException("Required field is missing.", requiredFieldName); }
            }
        }
    }
}