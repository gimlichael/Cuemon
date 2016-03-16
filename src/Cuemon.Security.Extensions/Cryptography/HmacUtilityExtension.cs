using System.IO;
using System.Text;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="HmacUtility"/> class.
    /// </summary>
    public static class HmacUtilityExtension
    {
        /// <summary>
        /// Computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> object to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for HMACSHA1 encryption. The key can be any length. However, the recommended size is 64 bytes. If the key is more than 64 bytes long, it is hashed (using SHA-1) to derive a 64-byte key.</param>
        /// <returns>A <see cref="HashValue"/> containing the computed HMACSHA1 hash value of the specified <see cref="Stream"/> <paramref name="value"/>.</returns>
        public static HashValue ComputeKeyedHash(this Stream value, byte[] sharedKey)
        {
            return HmacUtility.ComputeKeyedHash(value, sharedKey);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified <see cref="Stream"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> object to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length. However, the recommended size dependes on the desired <paramref name="algorithmType"/>. If the key is more than the recommended length, it is hashed using the specified <paramref name="algorithmType"/>.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <returns>A <see cref="HashValue"/> containing the computed hash value of the specified <see cref="Stream"/> <paramref name="value"/>.</returns>
        public static HashValue ComputeKeyedHash(this Stream value, byte[] sharedKey, HmacAlgorithmType algorithmType)
        {
            return HmacUtility.ComputeKeyedHash(value, sharedKey, algorithmType);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified <see cref="Stream"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> object to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length. However, the recommended size dependes on the desired <paramref name="algorithmType"/>. If the key is more than the recommended length, it is hashed using the specified <paramref name="algorithmType"/>.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <param name="leaveStreamOpen">if <c>true</c>, the <see cref="Stream"/> object is being left open; otherwise it is being closed and disposed.</param>
        /// <returns>A <see cref="HashValue"/> containing the computed hash value of the specified <see cref="Stream"/> <paramref name="value"/>.</returns>
        public static HashValue ComputeKeyedHash(this Stream value, byte[] sharedKey, HmacAlgorithmType algorithmType, bool leaveStreamOpen)
        {
            return HmacUtility.ComputeKeyedHash(value, sharedKey, algorithmType, leaveStreamOpen);
        }

        /// <summary>
        /// Computes a MD5 hash value of the specified <see cref="byte"/> sequence, <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="byte"/> array to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for HMACSHA1 encryption. The key can be any length. However, the recommended size is 64 bytes. If the key is more than 64 bytes long, it is hashed (using SHA-1) to derive a 64-byte key.</param>
        /// <returns>A <see cref="HashValue"/> containing the computed HMACSHA1 hash value of the specified <see cref="byte"/> sequence <paramref name="value"/>.</returns>
        public static HashValue ComputeKeyedHash(this byte[] value, byte[] sharedKey)
        {
            return HmacUtility.ComputeKeyedHash(value, sharedKey);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified <see cref="byte"/> sequence, <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="byte"/> array to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length. However, the recommended size dependes on the desired <paramref name="algorithmType"/>. If the key is more than the recommended length, it is hashed using the specified <paramref name="algorithmType"/>.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <returns>A <see cref="HashValue"/> containing the computed hash value of the specified <see cref="byte"/> sequence <paramref name="value"/>.</returns>
        public static HashValue ComputeKeyedHash(this byte[] value, byte[] sharedKey, HmacAlgorithmType algorithmType)
        {
            return HmacUtility.ComputeKeyedHash(value, sharedKey, algorithmType);
        }

        /// <summary>
        /// Computes a MD5 hash value of the specified <see cref="string"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for HMACSHA1 encryption. The key can be any length. However, the recommended size is 64 bytes. If the key is more than 64 bytes long, it is hashed (using SHA-1) to derive a 64-byte key.</param>
        /// <returns>A <see cref="HashValue"/> containing the computed HMACSHA1 hash value of the specified <see cref="string"/> <paramref name="value"/>.</returns>
        public static HashValue ComputeKeyedHash(this string value, byte[] sharedKey)
        {
            return HmacUtility.ComputeKeyedHash(value, sharedKey);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified <see cref="string"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length. However, the recommended size dependes on the desired <paramref name="algorithmType"/>. If the key is more than the recommended length, it is hashed using the specified <paramref name="algorithmType"/>.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <returns>A <see cref="HashValue"/> containing the computed hash value of the specified <see cref="string"/> <paramref name="value"/>.</returns>
        public static HashValue ComputeKeyedHash(this string value, byte[] sharedKey, HmacAlgorithmType algorithmType)
        {
            return HmacUtility.ComputeKeyedHash(value, sharedKey, algorithmType);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified <see cref="string"/> <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length. However, the recommended size dependes on the desired <paramref name="algorithmType"/>. If the key is more than the recommended length, it is hashed using the specified <paramref name="algorithmType"/>.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <param name="encoding">The encoding to use when computing the <paramref name="value"/>.</param>
        /// <returns>A <see cref="HashValue"/> containing the computed hash value of the specified <see cref="string"/> <paramref name="value"/>.</returns>
        public static HashValue ComputeKeyedHash(this string value, byte[] sharedKey, HmacAlgorithmType algorithmType, Encoding encoding)
        {
            return HmacUtility.ComputeKeyedHash(value, sharedKey, algorithmType, encoding);
        }

        /// <summary>
        /// Computes a MD5 hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The object to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for HMACSHA1 encryption. The key can be any length. However, the recommended size is 64 bytes. If the key is more than 64 bytes long, it is hashed (using SHA-1) to derive a 64-byte key.</param>
        /// <returns>A <see cref="HashValue"/> containing the computed HMACSHA1 hash value of the specified <paramref name="value"/>.</returns>
        public static HashValue ComputeKeyedHash(this object value, byte[] sharedKey)
        {
            return HmacUtility.ComputeKeyedHash(value, sharedKey);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The object to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length. However, the recommended size dependes on the desired <paramref name="algorithmType"/>. If the key is more than the recommended length, it is hashed using the specified <paramref name="algorithmType"/>.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <returns>A <see cref="HashValue"/> containing the computed hash value of the specified <paramref name="value"/>.</returns>
        public static HashValue ComputeKeyedHash(this object value, byte[] sharedKey, HmacAlgorithmType algorithmType)
        {
            return HmacUtility.ComputeKeyedHash(value, sharedKey, algorithmType);
        }

        /// <summary>
        /// Combines a sequence of objects into one object, and computes a MD5 hash value of the specified sequence, <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The objects to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for HMACSHA1 encryption. The key can be any length. However, the recommended size is 64 bytes. If the key is more than 64 bytes long, it is hashed (using SHA-1) to derive a 64-byte key.</param>
        /// <returns>A <see cref="HashValue"/> containing the computed HMACSHA1 hash value of the specified object sequence <paramref name="values"/>.</returns>
        public static HashValue ComputeKeyedHash(this object[] values, byte[] sharedKey)
        {
            return HmacUtility.ComputeKeyedHash(values, sharedKey);
        }

        /// <summary>
        /// Combines a sequence of objects into one object, and computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified sequence, <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The objects to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length. However, the recommended size dependes on the desired <paramref name="algorithmType"/>. If the key is more than the recommended length, it is hashed using the specified <paramref name="algorithmType"/>.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <returns>A <see cref="HashValue"/> containing the computed hash value of the specified object sequence <paramref name="values"/>.</returns>
        public static HashValue ComputeKeyedHash(this object[] values, byte[] sharedKey, HmacAlgorithmType algorithmType)
        {
            return HmacUtility.ComputeKeyedHash(values, sharedKey, algorithmType);
        }

        /// <summary>
        /// Computes a MD5 hash value of the specified <see cref="string"/> sequence, <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The <see cref="string"/> sequence to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for HMACSHA1 encryption. The key can be any length. However, the recommended size is 64 bytes. If the key is more than 64 bytes long, it is hashed (using SHA-1) to derive a 64-byte key.</param>
        /// <returns>A <see cref="HashValue"/> containing the computed HMACSHA1 hash value of the specified <see cref="string"/> sequence, <paramref name="values"/>.</returns>
        public static HashValue ComputeKeyedHash(this string[] values, byte[] sharedKey)
        {
            return HmacUtility.ComputeKeyedHash(values, sharedKey);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified <see cref="string"/> sequence, <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The <see cref="string"/> sequence to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length. However, the recommended size dependes on the desired <paramref name="algorithmType"/>. If the key is more than the recommended length, it is hashed using the specified <paramref name="algorithmType"/>.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <returns>A <see cref="HashValue"/> containing the computed hash value of the specified <see cref="string"/> sequence, <paramref name="values"/>.</returns>
        public static HashValue ComputeKeyedHash(this string[] values, byte[] sharedKey, HmacAlgorithmType algorithmType)
        {
            return HmacUtility.ComputeKeyedHash(values, sharedKey, algorithmType);
        }

        /// <summary>
        /// Computes a by parameter defined <see cref="HmacAlgorithmType"/> hash value of the specified <see cref="string"/> sequence, <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The <see cref="string"/> sequence to compute a hash code for.</param>
        /// <param name="sharedKey">The secret key for the hashed encryption. The key can be any length. However, the recommended size dependes on the desired <paramref name="algorithmType"/>. If the key is more than the recommended length, it is hashed using the specified <paramref name="algorithmType"/>.</param>
        /// <param name="algorithmType">The hash algorithm to use for the computation.</param>
        /// <param name="encoding">The encoding to use when computing the <paramref name="values"/> sequence.</param>
        /// <returns>A <see cref="HashValue"/> containing the computed hash value of the specified <see cref="string"/> sequence, <paramref name="values"/>.</returns>
        public static HashValue ComputeKeyedHash(this string[] values, byte[] sharedKey, HmacAlgorithmType algorithmType, Encoding encoding)
        {
            return HmacUtility.ComputeKeyedHash(values, sharedKey, algorithmType, encoding);
        }
    }
}