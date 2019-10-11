using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Cuemon.Integrity;
using Cuemon.Text;

namespace Cuemon.AspNetCore.Authentication
{
    /// <summary>
    /// Provides an isolated set of members to work with HTTP Digest access authentication.
    /// </summary>
    public static class DigestAuthenticationUtility
    {
        internal static readonly byte[] DefaultPrivateKey = Convert.FromBase64String("ZHBGWDRrVGVxbFlhVEpWQ3hoYUc5VUlZM05penNOaUk=");

        /// <summary>
        /// The value of the header credential user name of a HTTP Digest access authentication.
        /// </summary>
        public const string CredentialUserName = "username";

        /// <summary>
        /// The value of the header credential realm of a HTTP Digest access authentication.
        /// </summary>
        public const string CredentialRealm = "realm";

        /// <summary>
        /// The value of the header credential response of a HTTP Digest access authentication.
        /// </summary>
        public const string CredentialResponse = "response";

        /// <summary>
        /// The value of the header credential quality of protection of a HTTP Digest access authentication.
        /// </summary>
        public const string CredentialQualityOfProtection = "qop";

        /// <summary>
        /// The value of the header credential quality of protection options of a HTTP Digest access authentication.
        /// </summary>
        public const string CredentialQualityOfProtectionOptions = "auth,auth-int";

        /// <summary>
        /// The value of the header credential client nonce of a HTTP Digest access authentication.
        /// </summary>
        public const string CredentialClientNonce = "cnonce";

        /// <summary>
        /// The value of the header credential nonce count of a HTTP Digest access authentication.
        /// </summary>
        public const string CredentialNonceCount = "nc";

        /// <summary>
        /// The value of the header credential nonce of a HTTP Digest access authentication.
        /// </summary>
        public const string CredentialNonce = "nonce";

        /// <summary>
        /// The value of the header credential digest URI of a HTTP Digest access authentication.
        /// </summary>
        public const string CredentialDigestUri = "uri";

        /// <summary>
        /// The value of the header credential opaque of a HTTP Digest access authentication.
        /// </summary>
        public const string CredentialOpaque = "opaque";

        /// <summary>
        /// The value of the header credential algorithm of a HTTP Digest access authentication.
        /// </summary>
        public const string CredentialAlgorithm = "algorithm";

        /// <summary>
        /// Computes a by parameter defined <see cref="CryptoAlgorithm"/> hash value of the required values for the HTTP Digest access authentication HA1.
        /// </summary>
        /// <param name="credentials">The credentials of the HA1 computed value (<see cref="CredentialUserName"/>, <see cref="CredentialRealm"/>).</param>
        /// <param name="password">The password to include in the HA1 computed value.</param>
        /// <param name="algorithm">The algorithm to use when computing the HA1 value.</param>
        /// <returns>A <see cref="string"/> in the format of H('<paramref name="credentials"/>[CredentialUserName]:<paramref name="credentials"/>[CredentialRealm]:<paramref name="password"/>').</returns>
        public static string ComputeHash1(IDictionary<string, string> credentials, string password, CryptoAlgorithm algorithm)
        {
            ValidateCredentials(credentials, CredentialUserName, CredentialRealm);
            return HashFactory.CreateCrypto(algorithm).ComputeHash(string.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}", credentials[CredentialUserName], credentials[CredentialRealm], password), o =>
            {
                o.Encoding = Encoding.UTF8;
            }).ToHexadecimalString();
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="CryptoAlgorithm"/> hash value of the required values for the HTTP Digest access authentication HA2.
        /// </summary>
        /// <param name="credentials">The credentials of the HA2 computed value (<see cref="CredentialDigestUri"/>).</param>
        /// <param name="httpMethod">The HTTP method to include in the HA2 computed value.</param>
        /// <param name="algorithm">The algorithm to use when computing the HA2 value.</param>
        /// <returns>A <see cref="string"/> in the format of H('<paramref name="httpMethod"/>:<paramref name="credentials"/>[CredentialDigestUri]').</returns>
        public static string ComputeHash2(IDictionary<string, string> credentials, string httpMethod, CryptoAlgorithm algorithm)
        {
            ValidateCredentials(credentials, CredentialDigestUri);
            return HashFactory.CreateCrypto(algorithm).ComputeHash(string.Format(CultureInfo.InvariantCulture, "{0}:{1}", httpMethod, credentials[CredentialDigestUri]), o =>
            {
                o.Encoding = Encoding.UTF8;
            }).ToHexadecimalString();
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="CryptoAlgorithm"/> hash value of the required values for the HTTP Digest access authentication RESPONSE.
        /// </summary>
        /// <param name="credentials">The credentials of the RESPONSE computed value (<see cref="CredentialNonce"/>, <see cref="CredentialNonceCount"/>, <see cref="CredentialClientNonce"/>, <see cref="CredentialQualityOfProtection"/>).</param>
        /// <param name="hash1">The HA1 to include in the RESPONSE computed value.</param>
        /// <param name="hash2">The HA2 to include in the RESPONSE computed value.</param>
        /// <param name="algorithm">The algorithm to use when computing the RESPONSE value.</param>
        /// <returns>A <see cref="string"/> in the format of H('<paramref name="hash1"/>:<paramref name="credentials"/>[CredentialNonce]:<paramref name="credentials"/>[CredentialNonceCount]:<paramref name="credentials"/>[CredentialClientNonce]:<paramref name="credentials"/>[CredentialQualityOfProtection]:<paramref name="hash2"/>').</returns>
        public static byte[] ComputeResponse(IDictionary<string, string> credentials, string hash1, string hash2, CryptoAlgorithm algorithm)
        {
            ValidateCredentials(credentials, CredentialNonce, CredentialNonceCount, CredentialClientNonce, CredentialQualityOfProtection);
            return HashFactory.CreateCrypto(algorithm).ComputeHash(FormattableString.Invariant($"{hash1}:{credentials[CredentialNonce]}:{credentials[CredentialNonceCount]}:{credentials[CredentialClientNonce]}:{credentials[CredentialQualityOfProtection]}:{hash2}"), o =>
            {
                o.Encoding = Encoding.UTF8;
            }).GetBytes();
        }

        /// <summary>
        /// A default implementation of a nonce parser.
        /// </summary>
        /// <param name="nonce">The nonce protocol.</param>
        /// <param name="timeToLive">The time-to-live (ttl) of the <paramref name="nonce"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="nonce"/> has expired compared to <paramref name="timeToLive"/>; otherwise, <c>false</c>.</returns>
        public static bool DefaultNonceExpiredParser(string nonce, TimeSpan timeToLive)
        {
            Validator.ThrowIfNullOrEmpty(nonce, nameof(nonce));
            if (ParseFactory.FromBase64().TryParse(nonce, out var rawNonce))
            {
                var nonceProtocol = Convertible.ToString(rawNonce, options =>
                {
                    options.Encoding = Encoding.UTF8;
                    options.Preamble = PreambleSequence.Remove;
                });
                var nonceTimestamp = DateTime.ParseExact(nonceProtocol.Substring(0, nonceProtocol.LastIndexOf(':')), "u", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
                var difference = (DateTime.UtcNow - nonceTimestamp);
                return (difference > timeToLive);
            }
            return false;
        }

        /// <summary>
        /// A default implementation of a nonce generator.
        /// </summary>
        /// <param name="timestamp">The <see cref="DateTime"/> value to include in the generated nonce.</param>
        /// <param name="entityTag">An opaque identifier to include in the generated nonce.</param>
        /// <param name="privateKey">A cryptographic private key to include as a cipher in the generated nonce.</param>
        /// <returns>A nonce protocol in the format of '<paramref name="timestamp"/>:H(<paramref name="timestamp"/><paramref name="entityTag"/><paramref name="privateKey"/>)'.</returns>
        public static string DefaultNonceGenerator(DateTime timestamp, string entityTag, byte[] privateKey)
        {
            Validator.ThrowIfNullOrEmpty(entityTag, nameof(entityTag));
            Validator.ThrowIfNull(privateKey, nameof(privateKey));
            var nonceHash = ComputeNonceHash(timestamp, entityTag, privateKey);
            var nonceProtocol = string.Format(CultureInfo.InvariantCulture, "{0}:{1}", timestamp.ToString("u", CultureInfo.InvariantCulture), nonceHash);
            return Convert.ToBase64String(Convertible.GetBytes(nonceProtocol, options =>
            {
                options.Encoding = Encoding.UTF8;
                options.Preamble = PreambleSequence.Remove;
            }));
        }

        /// <summary>
        /// A default implementation of opaque generator.
        /// </summary>
        /// <returns>An opaque value consisting of hexadecimal characters with a length of 32 bytes.</returns>
        public static string DefaultOpaqueGenerator()
        {
            return Generate.RandomString(32, Alphanumeric.Hexadecimal).ToLowerInvariant();
        }

        /// <summary>
        /// Converts the specified <paramref name="algorithm"/> to its HTTP Digest access authentication header credential algorithm equivalent.
        /// </summary>
        /// <param name="algorithm">The algorithm to convert.</param>
        /// <returns>A string containing either MD5, SHA-256 or SHA-512-256.</returns>
        public static string ParseAlgorithm(CryptoAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case CryptoAlgorithm.Sha256:
                    return "SHA-256";
                case CryptoAlgorithm.Sha512:
                    return "SHA-512-256";
                default:
                    return "MD5";
            }
        }

        private static void ValidateCredentials(IDictionary<string, string> credentials, params string[] requiredCredentials)
        {
            Validator.ThrowIfNull(credentials, nameof(credentials));
            foreach (var requiredCredential in requiredCredentials)
            {
                if (!credentials.ContainsKey(requiredCredential)) { throw new ArgumentException("One or more required credentials are missing.", nameof(credentials)); }
            }
        }

        private static string ComputeNonceHash(DateTime timeStamp, string entityTag, byte[] privateKey)
        {
            return HashFactory.CreateCryptoSha256().ComputeHash(timeStamp, entityTag, Convert.ToBase64String(privateKey)).ToHexadecimalString();
        }
    }
}