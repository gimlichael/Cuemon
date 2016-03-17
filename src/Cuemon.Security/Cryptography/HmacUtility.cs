using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
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
        /// Computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> object to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for HMACSHA1 encryption. The key can be any length. However, the recommended size is 64 bytes. If the key is more than 64 bytes long, it is hashed (using SHA-1) to derive a 64-byte key.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed HMACSHA1 hash value of the specified <see cref="Stream"/> <paramref name="value"/>.</returns>
        public static HashResult ComputeKeyedHash(Stream value, byte[] sharedKey)
        {
            return ComputeKeyedHash(value, sharedKey, HmacAlgorithmType.SHA1);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified <see cref="Stream"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> object to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length. However, the recommended size dependes on the desired <paramref name="algorithmType"/>. If the key is more than the recommended length, it is hashed using the specified <paramref name="algorithmType"/>.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <see cref="Stream"/> <paramref name="value"/>.</returns>
        public static HashResult ComputeKeyedHash(Stream value, byte[] sharedKey, HmacAlgorithmType algorithmType)
        {
            return ComputeKeyedHash(value, sharedKey, algorithmType, false);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified <see cref="Stream"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> object to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length. However, the recommended size dependes on the desired <paramref name="algorithmType"/>. If the key is more than the recommended length, it is hashed using the specified <paramref name="algorithmType"/>.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <param name="leaveStreamOpen">if <c>true</c>, the <see cref="Stream"/> object is being left open; otherwise it is being closed and disposed.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <see cref="Stream"/> <paramref name="value"/>.</returns>
        public static HashResult ComputeKeyedHash(Stream value, byte[] sharedKey, HmacAlgorithmType algorithmType, bool leaveStreamOpen)
        {
            return ComputeHashCore(value, null, sharedKey, algorithmType, leaveStreamOpen);
        }

        /// <summary>
        /// Computes a MD5 hash value of the specified <see cref="byte"/> sequence, <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="byte"/> array to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for HMACSHA1 encryption. The key can be any length. However, the recommended size is 64 bytes. If the key is more than 64 bytes long, it is hashed (using SHA-1) to derive a 64-byte key.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed HMACSHA1 hash value of the specified <see cref="byte"/> sequence <paramref name="value"/>.</returns>
        public static HashResult ComputeKeyedHash(byte[] value, byte[] sharedKey)
        {
            return ComputeKeyedHash(value, sharedKey, HmacAlgorithmType.SHA1);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified <see cref="byte"/> sequence, <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="byte"/> array to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length. However, the recommended size dependes on the desired <paramref name="algorithmType"/>. If the key is more than the recommended length, it is hashed using the specified <paramref name="algorithmType"/>.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <see cref="byte"/> sequence <paramref name="value"/>.</returns>
        public static HashResult ComputeKeyedHash(byte[] value, byte[] sharedKey, HmacAlgorithmType algorithmType)
        {
            return ComputeHashCore(null, value, sharedKey, algorithmType, false);
        }

        /// <summary>
        /// Computes a MD5 hash value of the specified <see cref="string"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for HMACSHA1 encryption. The key can be any length. However, the recommended size is 64 bytes. If the key is more than 64 bytes long, it is hashed (using SHA-1) to derive a 64-byte key.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed HMACSHA1 hash value of the specified <see cref="string"/> <paramref name="value"/>.</returns>
        public static HashResult ComputeKeyedHash(string value, byte[] sharedKey)
        {
            return ComputeKeyedHash(value, sharedKey, HmacAlgorithmType.SHA1);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified <see cref="string"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length. However, the recommended size dependes on the desired <paramref name="algorithmType"/>. If the key is more than the recommended length, it is hashed using the specified <paramref name="algorithmType"/>.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <see cref="string"/> <paramref name="value"/>.</returns>
        public static HashResult ComputeKeyedHash(string value, byte[] sharedKey, HmacAlgorithmType algorithmType)
        {
            return ComputeKeyedHash(value, sharedKey, algorithmType, Encoding.Unicode);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified <see cref="string"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length. However, the recommended size dependes on the desired <paramref name="algorithmType"/>. If the key is more than the recommended length, it is hashed using the specified <paramref name="algorithmType"/>.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <param name="encoding">The encoding to use when computing the <paramref name="value"/>.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <see cref="string"/> <paramref name="value"/>.</returns>
        public static HashResult ComputeKeyedHash(string value, byte[] sharedKey, HmacAlgorithmType algorithmType, Encoding encoding)
        {
            return ComputeKeyedHash(ByteConverter.FromString(value, PreambleSequence.Remove, encoding), sharedKey, algorithmType);
        }

        /// <summary>
        /// Computes a MD5 hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The object to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for HMACSHA1 encryption. The key can be any length. However, the recommended size is 64 bytes. If the key is more than 64 bytes long, it is hashed (using SHA-1) to derive a 64-byte key.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed HMACSHA1 hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeKeyedHash(object value, byte[] sharedKey)
        {
            return ComputeKeyedHash(value, sharedKey, HmacAlgorithmType.SHA1);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The object to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length. However, the recommended size dependes on the desired <paramref name="algorithmType"/>. If the key is more than the recommended length, it is hashed using the specified <paramref name="algorithmType"/>.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeKeyedHash(object value, byte[] sharedKey, HmacAlgorithmType algorithmType)
        {
            return ComputeKeyedHash(EnumerableConverter.AsArray(value), sharedKey, algorithmType);
        }

        /// <summary>
        /// Combines a sequence of objects into one object, and computes a MD5 hash value of the specified sequence, <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The objects to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for HMACSHA1 encryption. The key can be any length. However, the recommended size is 64 bytes. If the key is more than 64 bytes long, it is hashed (using SHA-1) to derive a 64-byte key.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed HMACSHA1 hash value of the specified object sequence <paramref name="values"/>.</returns>
        public static HashResult ComputeKeyedHash(object[] values, byte[] sharedKey)
        {
            return ComputeKeyedHash(values, sharedKey, HmacAlgorithmType.SHA1);
        }

        /// <summary>
        /// Combines a sequence of objects into one object, and computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified sequence, <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The objects to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length. However, the recommended size dependes on the desired <paramref name="algorithmType"/>. If the key is more than the recommended length, it is hashed using the specified <paramref name="algorithmType"/>.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified object sequence <paramref name="values"/>.</returns>
        public static HashResult ComputeKeyedHash(object[] values, byte[] sharedKey, HmacAlgorithmType algorithmType)
        {
            Validator.ThrowIfNull(values, nameof(values));
            long signature = StructUtility.GetHashCode64(EnumerableConverter.Parse(values, o => o.GetHashCode()));
            return ComputeKeyedHash(BitConverter.GetBytes(signature), sharedKey, algorithmType);
        }

        /// <summary>
        /// Computes a MD5 hash value of the specified <see cref="string"/> sequence, <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The <see cref="string"/> sequence to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for HMACSHA1 encryption. The key can be any length. However, the recommended size is 64 bytes. If the key is more than 64 bytes long, it is hashed (using SHA-1) to derive a 64-byte key.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed HMACSHA1 hash value of the specified <see cref="string"/> sequence, <paramref name="values"/>.</returns>
        public static HashResult ComputeKeyedHash(string[] values, byte[] sharedKey)
        {
            return ComputeKeyedHash(values, sharedKey, HmacAlgorithmType.SHA1);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified <see cref="string"/> sequence, <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The <see cref="string"/> sequence to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length. However, the recommended size dependes on the desired <paramref name="algorithmType"/>. If the key is more than the recommended length, it is hashed using the specified <paramref name="algorithmType"/>.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <see cref="string"/> sequence, <paramref name="values"/>.</returns>
        public static HashResult ComputeKeyedHash(string[] values, byte[] sharedKey, HmacAlgorithmType algorithmType)
        {
            return ComputeKeyedHash(values, sharedKey, algorithmType, Encoding.Unicode);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified <see cref="string"/> sequence, <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The <see cref="string"/> sequence to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length. However, the recommended size dependes on the desired <paramref name="algorithmType"/>. If the key is more than the recommended length, it is hashed using the specified <paramref name="algorithmType"/>.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <param name="encoding">The encoding to use when computing the <paramref name="values"/> sequence.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <see cref="string"/> sequence, <paramref name="values"/>.</returns>
        public static HashResult ComputeKeyedHash(string[] values, byte[] sharedKey, HmacAlgorithmType algorithmType, Encoding encoding)
        {
            Validator.ThrowIfNull(values, nameof(values));
            Validator.ThrowIfNull(encoding, nameof(encoding));
            MemoryStream tempStream = null;
            try
            {
                tempStream = new MemoryStream();
                using (StreamWriter writer = new StreamWriter(tempStream, encoding))
                {
                    foreach (string value in values)
                    {
                        if (value == null) { continue; }
                        writer.Write(value);
                    }
                    writer.Flush();
                    tempStream.Position = 0;
                    Stream stream = StreamConverter.RemovePreamble(tempStream, encoding);
                    tempStream = null;
                    return ComputeKeyedHash(stream, sharedKey, algorithmType);
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