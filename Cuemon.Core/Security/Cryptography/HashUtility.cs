using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Cuemon.Collections.Generic;
using Cuemon.IO;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// This utility class is designed to make <see cref="HashAlgorithm"/> operations easier to work with.
    /// </summary>
    public static class HashUtility
    {
        /// <summary>
        /// Computes a hash value of the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to compute a hash code for.</param>
        /// <param name="setup">The <see cref="StreamHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <paramref name="stream"/>.</returns>
        public static HashResult ComputeHash(Stream stream, Action<StreamHashOptions> setup = null)
        {
            Validator.ThrowIfNull(stream, nameof(stream));
            var options = Patterns.Configure(setup);
            return ComputeHashCore(stream, null, options.AlgorithmType, options.LeaveOpen);
        }

        /// <summary>
        /// Computes a hash value of the specified <paramref name="bytes"/>.
        /// </summary>
        /// <param name="bytes">The <see cref="byte"/> array to compute a hash code for.</param>
        /// <param name="setup">The <see cref="HashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <paramref name="bytes"/>.</returns>
        public static HashResult ComputeHash(byte[] bytes, Action<HashOptions> setup = null)
        {
            Validator.ThrowIfNull(bytes, nameof(bytes));
            var options = Patterns.Configure(setup);
            return ComputeHashCore(null, bytes, options.AlgorithmType, false);
        }

        /// <summary>
        /// Computes a hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to compute a hash code for.</param>
        /// <param name="setup">The <see cref="StringHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeHash(string value, Action<StringHashOptions> setup = null)
        {
            Validator.ThrowIfNull(value, nameof(value));
            var options = Patterns.Configure(setup);
            return ComputeHash(ByteConverter.FromString(value, o =>
            {
                o.Encoding = options.Encoding;
                o.Preamble = options.Preamble;
            }), o => o.AlgorithmType = options.AlgorithmType);
        }

        /// <summary>
        /// Computes a hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The object to compute a hash code for.</param>
        /// <param name="setup">The <see cref="HashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeHash(object value, Action<HashOptions> setup = null)
        {
            Validator.ThrowIfNull(value, nameof(value));
            var options = Patterns.Configure(setup);
            return ComputeHash(EnumerableConverter.AsArray(value), o => o.AlgorithmType = options.AlgorithmType);
        }

        /// <summary>
        /// Combines a sequence of objects into one object, and computes a hash value of the specified <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The objects to compute a hash code for.</param>
        /// <param name="setup">The <see cref="HashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified object sequence <paramref name="values"/>.</returns>
        public static HashResult ComputeHash(object[] values, Action<HashOptions> setup = null)
        {
            Validator.ThrowIfNull(values, nameof(values));
            var options = Patterns.Configure(setup);
            var signature = StructUtility.GetHashCode64(values.Select(o => o.GetHashCode()));
            return ComputeHash(BitConverter.GetBytes(signature), o => o.AlgorithmType = options.AlgorithmType);
        }

        /// <summary>
        /// Computes a hash value of the specified <paramref name="values"/>.
        /// </summary>
        /// <param name="values">The <see cref="string"/> array to compute a hash code for.</param>
        /// <param name="setup">The <see cref="StringHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <paramref name="values"/>.</returns>
        public static HashResult ComputeHash(string[] values, Action<StringHashOptions> setup = null)
        {
            Validator.ThrowIfNull(values, nameof(values));
            var options = Patterns.Configure(setup);
            var stream = Disposable.SafeInvoke(() => new MemoryStream(), (ms, strings) =>
            {
                
                using (var writer = new StreamWriter(ms, options.Encoding))
                {
                    foreach (var value in strings)
                    {
                        if (value == null) { continue; }
                        writer.Write(value);
                    }
                    writer.Flush();
                    ms.Position = 0;
                    if (options.Preamble == PreambleSequence.Keep)
                    {
                        return ms;
                    }
                    return StreamConverter.RemovePreamble(ms, options.Encoding) as MemoryStream;
                }
            }, values);
            return ComputeHash(stream, o => o.AlgorithmType = options.AlgorithmType);
        }

        private static HashResult ComputeHashCore(Stream value, byte[] hash, HashAlgorithmType algorithmType, bool leaveStreamOpen)
        {
            if (algorithmType > HashAlgorithmType.CRC32 || algorithmType < HashAlgorithmType.MD5) { throw new ArgumentOutOfRangeException(nameof(algorithmType), "Specified argument was out of the range of valid values."); }
            HashAlgorithm algorithm;
            switch (algorithmType)
            {
                case HashAlgorithmType.MD5:
                    algorithm = MD5.Create();
                    break;
                case HashAlgorithmType.SHA1:
                    algorithm = SHA1.Create();
                    break;
                case HashAlgorithmType.SHA256:
                    goto default;
                case HashAlgorithmType.SHA384:
                    algorithm = SHA384.Create();
                    break;
                case HashAlgorithmType.SHA512:
                    algorithm = SHA512.Create();
                    break;
                case HashAlgorithmType.CRC32:
                    algorithm = new CyclicRedundancyCheck32(PolynomialRepresentation.Reversed);
                    break;
                default:
                    algorithm = SHA256.Create();
                    break;
            }
            return ComputeHashCore(value, hash, leaveStreamOpen, algorithm);
        }

        internal static HashResult ComputeHashCore(Stream value, byte[] hash, bool leaveStreamOpen, HashAlgorithm algorithm)
        {
            using (algorithm)
            {
                if (value != null)
                {
                    var startingPosition = value.Position;

                    if (value.CanSeek) { value.Position = 0; }
                    hash = algorithm.ComputeHash(value);
                    if (value.CanSeek) { value.Seek(startingPosition, SeekOrigin.Begin); } // reset to original position

                    if (!leaveStreamOpen)
                    {
                        value.Dispose();
                    }
                }
                else
                {
                    hash = algorithm.ComputeHash(hash); // convert original byte value to hash value
                }
            }
            return new HashResult(hash);
        }
    }
}