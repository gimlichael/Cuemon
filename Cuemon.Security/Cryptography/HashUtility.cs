using System;
using System.IO;
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
        /// Computes a hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to compute a hash code for.</param>
        /// <param name="setup">The <see cref="StreamHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeHash(Stream value, Action<StreamHashOptions> setup = null)
        {
            var options = setup.Configure();
            return ComputeHashCore(value, null, options.AlgorithmType, options.LeaveStreamOpen);
        }

        /// <summary>
        /// Computes a hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="byte"/> array to compute a hash code for.</param>
        /// <param name="setup">The <see cref="HashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeHash(byte[] value, Action<HashOptions> setup = null)
        {
            var options = setup.Configure();
            return ComputeHashCore(null, value, options.AlgorithmType, false);
        }

        /// <summary>
        /// Computes a hash value of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to compute a hash code for.</param>
        /// <param name="setup">The <see cref="StringHashOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash value of the specified <paramref name="value"/>.</returns>
        public static HashResult ComputeHash(string value, Action<StringHashOptions> setup = null)
        {
            var options = setup.Configure();
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
            var options = setup.Configure();
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
            var options = setup.Configure();
            long signature = StructUtility.GetHashCode64(EnumerableConverter.Parse(values, o => o.GetHashCode()));
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
            var options = setup.Configure();
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
                    return ComputeHash(stream, o => o.AlgorithmType = options.AlgorithmType);
                }
            }
            finally 
            {
                if (tempStream != null) { tempStream.Dispose(); }
            }
        }

        private static HashResult ComputeHashCore(Stream value, byte[] hash, HashAlgorithmType algorithmType, bool leaveStreamOpen)
        {
            if (algorithmType > HashAlgorithmType.CRC32 || algorithmType < HashAlgorithmType.MD5) { throw new ArgumentOutOfRangeException(nameof(algorithmType), "Specified argument was out of the range of valid values."); }
            
            HashAlgorithm algorithm;
            switch (algorithmType)
            {
                case HashAlgorithmType.MD5:
                    goto default;
                case HashAlgorithmType.SHA1:
                    algorithm = SHA1.Create();
                    break;
                case HashAlgorithmType.SHA256:
                    algorithm = SHA256.Create();
                    break;
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
                    algorithm = MD5.Create();
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
                    long startingPosition = value.Position;

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