using System;
using System.IO;
using System.Security.Cryptography;
using Cuemon.Collections.Generic;
using Cuemon.IO;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// This utility class is designed to make HMAC (Hash-based Message Authentication Code) <see cref="KeyedHashAlgorithm"/> operations easier to work with.
    /// </summary>
    public static class HmacUtility
    {
        /// <summary>
        /// Computes a keyed-hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length, but it is strongly recommended to use a size of either 64 bytes (for <see cref="HmacAlgorithmType.SHA1"/> and <see cref="HmacAlgorithmType.SHA256"/>) or 128 bytes (for <see cref="HmacAlgorithmType.SHA384"/> and <see cref="HmacAlgorithmType.SHA512"/>).</param>
        /// <param name="setup">The <see cref="StreamKeyedHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed keyed-hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeKeyedHash(Stream value, byte[] sharedKey, Action<StreamKeyedHashOptions> setup = null)
        {
            var options = setup.ConfigureOptions();
            return ComputeHashCore(value, null, sharedKey, options.AlgorithmType, options.LeaveStreamOpen);
        }

        /// <summary>
        /// Computes a keyed-hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="byte"/> array to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length, but it is strongly recommended to use a size of either 64 bytes (for <see cref="HmacAlgorithmType.SHA1"/> and <see cref="HmacAlgorithmType.SHA256"/>) or 128 bytes (for <see cref="HmacAlgorithmType.SHA384"/> and <see cref="HmacAlgorithmType.SHA512"/>).</param>
        /// <param name="setup">The <see cref="KeyedHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed keyed-hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeKeyedHash(byte[] value, byte[] sharedKey, Action<KeyedHashOptions> setup = null)
        {
            var options = setup.ConfigureOptions();
            return ComputeHashCore(null, value, sharedKey, options.AlgorithmType, false);
        }

        /// <summary>
        /// Computes a keyed-hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length, but it is strongly recommended to use a size of either 64 bytes (for <see cref="HmacAlgorithmType.SHA1"/> and <see cref="HmacAlgorithmType.SHA256"/>) or 128 bytes (for <see cref="HmacAlgorithmType.SHA384"/> and <see cref="HmacAlgorithmType.SHA512"/>).</param>
        /// <param name="setup">The <see cref="StringKeyedHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed keyed-hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeKeyedHash(string value, byte[] sharedKey, Action<StringKeyedHashOptions> setup = null)
        {
            var options = setup.ConfigureOptions();
            return ComputeKeyedHash(ByteConverter.FromString(value, o =>
            {
                o.Encoding = options.Encoding;
                o.Preamble = options.Preamble;
            }), sharedKey, o => o.AlgorithmType = options.AlgorithmType);
        }

        /// <summary>
        /// Computes a keyed-hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The object to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length, but it is strongly recommended to use a size of either 64 bytes (for <see cref="HmacAlgorithmType.SHA1"/> and <see cref="HmacAlgorithmType.SHA256"/>) or 128 bytes (for <see cref="HmacAlgorithmType.SHA384"/> and <see cref="HmacAlgorithmType.SHA512"/>).</param>
        /// <param name="setup">The <see cref="KeyedHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed keyed-hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeKeyedHash(object value, byte[] sharedKey, Action<KeyedHashOptions> setup = null)
        {
            var options = setup.ConfigureOptions();
            return ComputeKeyedHash(EnumerableConverter.AsArray(value), sharedKey, o => o.AlgorithmType = options.AlgorithmType);
        }

        /// <summary>
        /// Combines a sequence of objects into one object, and computes a keyed-hash value of the specified <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The objects to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length, but it is strongly recommended to use a size of either 64 bytes (for <see cref="HmacAlgorithmType.SHA1"/> and <see cref="HmacAlgorithmType.SHA256"/>) or 128 bytes (for <see cref="HmacAlgorithmType.SHA384"/> and <see cref="HmacAlgorithmType.SHA512"/>).</param>
        /// <param name="setup">The <see cref="KeyedHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed keyed-hash value of the specified <paramref name="values"/>.</returns>
        public static HashResult ComputeKeyedHash(object[] values, byte[] sharedKey, Action<KeyedHashOptions> setup = null)
        {
            Validator.ThrowIfNull(values, nameof(values));
            var options = setup.ConfigureOptions();
            long signature = StructUtility.GetHashCode64(EnumerableConverter.Parse(values, o => o.GetHashCode()));
            return ComputeKeyedHash(BitConverter.GetBytes(signature), sharedKey, o => o.AlgorithmType = options.AlgorithmType);
        }

        /// <summary>
        /// Computes a keyed-hash value of the specified <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The <see cref="string"/> array to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length, but it is strongly recommended to use a size of either 64 bytes (for <see cref="HmacAlgorithmType.SHA1"/> and <see cref="HmacAlgorithmType.SHA256"/>) or 128 bytes (for <see cref="HmacAlgorithmType.SHA384"/> and <see cref="HmacAlgorithmType.SHA512"/>).</param>
        /// <param name="setup">The <see cref="StringKeyedHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed keyed-hash value of <paramref name="values"/>.</returns>
        public static HashResult ComputeKeyedHash(string[] values, byte[] sharedKey, Action<StringKeyedHashOptions> setup = null)
        {
            var options = setup.ConfigureOptions();
            Validator.ThrowIfNull(values, nameof(values));
            MemoryStream tempStream = null;
            try
            {
                tempStream = new MemoryStream();
                using (StreamWriter writer = new StreamWriter(tempStream, options.Encoding))
                {
                    foreach (string value in values)
                    {
                        if (value == null) { continue; }
                        writer.Write(value);
                    }
                    writer.Flush();
                    tempStream.Position = 0;
                    Stream stream = StreamConverter.RemovePreamble(tempStream, options.Encoding);
                    tempStream = null;
                    return ComputeKeyedHash(stream, sharedKey, o => o.AlgorithmType = options.AlgorithmType);
                }
            }
            finally
            {
                if (tempStream != null) { tempStream.Dispose(); }
            }
        }

        private static HashResult ComputeHashCore(Stream value, byte[] hash, byte[] sharedKey, HmacAlgorithmType algorithmType, bool leaveStreamOpen)
        {
            if (algorithmType > HmacAlgorithmType.SHA512 || algorithmType < HmacAlgorithmType.SHA1) { throw new ArgumentOutOfRangeException(nameof(algorithmType), "Specified argument was out of the range of valid values."); }

            HashAlgorithm algorithm;
            switch (algorithmType)
            {
                case HmacAlgorithmType.SHA1:
                    goto default;
                case HmacAlgorithmType.SHA256:
                    algorithm = new HMACSHA256(sharedKey);
                    break;
                case HmacAlgorithmType.SHA384:
                    algorithm = new HMACSHA384(sharedKey);
                    break;
                case HmacAlgorithmType.SHA512:
                    algorithm = new HMACSHA512(sharedKey);
                    break;
                default:
                    algorithm = new HMACSHA1(sharedKey);
                    break;
            }

            return HashUtility.ComputeHashCore(value, hash, leaveStreamOpen, algorithm);
        }
    }
}