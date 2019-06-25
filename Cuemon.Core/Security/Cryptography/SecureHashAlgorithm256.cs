using System;
using System.Security.Cryptography;
using Cuemon.Integrity;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Provides a SHA-256 implementation of the SHA (Secure Hash Algorithm) cryptographic hashing algorithm for 256-bit hash values. This class cannot be inherited.
    /// Implements the <see cref="UnkeyedCryptoHash{TAlgorithm}" />
    /// </summary>
    /// <seealso cref="UnkeyedCryptoHash{TAlgorithm}" />
    /// <seealso cref="Hash{TOptions}" />
    public sealed class SecureHashAlgorithm256 : UnkeyedCryptoHash<SHA256>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureHashAlgorithm256"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
        public SecureHashAlgorithm256(Action<ConvertibleOptions> setup = null) : base(SHA256.Create, setup)
        {
        }
    }
}