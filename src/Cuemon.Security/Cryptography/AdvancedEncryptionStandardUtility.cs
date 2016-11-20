using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// This utility class is designed to make <see cref="Aes"/> operations easier to work with.
    /// </summary>
    public static class AdvancedEncryptionStandardUtility
    {
        /// <summary>
        /// Gets the block size (bits) for the Advanced Encryption Standard (AES)
        /// </summary>
        public static readonly byte BlockSize = 128;

        /// <summary>
        /// Generates a random 128 bit initialization vector (IV) to be used for the algorithm.
        /// </summary>
        /// <returns>A random 128 bit generated initialization vector (IV).</returns>
        public static byte[] GenerateInitializationVector()
        {
            return ByteConverter.FromString(StringUtility.CreateRandomString(BlockSize / 8), options =>
            {
                options.Encoding = Encoding.UTF8;
                options.Preamble = PreambleSequence.Remove;
            });
        }

        /// <summary>
        /// Generates a random key to be used for the algorithm.
        /// </summary>
        /// <param name="keySize">The size of the key.</param>
        /// <returns>A random generated key with the specified <paramref name="keySize"/>.</returns>
        public static byte[] GenerateKey(AdvancedEncryptionStandardKeySize keySize)
        {
            if (keySize > AdvancedEncryptionStandardKeySize.AES256 || keySize < AdvancedEncryptionStandardKeySize.AES128) { throw new ArgumentOutOfRangeException(nameof(keySize), "Specified argument was out of the range of valid values."); }
            string result;
            switch (keySize)
            {
                case AdvancedEncryptionStandardKeySize.AES128:
                    result = StringUtility.CreateRandomString(128 / 8);
                    break;
                case AdvancedEncryptionStandardKeySize.AES192:
                    result = StringUtility.CreateRandomString(192 / 8);
                    break;
                case AdvancedEncryptionStandardKeySize.AES256:
                    goto default;
                default:
                    result = StringUtility.CreateRandomString(256 / 8);
                    break;
            }
            return ByteConverter.FromString(result, options =>
            {
                options.Encoding = Encoding.UTF8;
                options.Preamble = PreambleSequence.Remove;
            });
        }

        private static byte[] CryptoTransformCore(byte[] value, byte[] key, byte[] initializationVector, AdvancedEncryptionStandardCommand command)
        {
            ValidateInput(value, key, initializationVector);
            byte[] output = null;

            using (Aes rijndael = Aes.Create())
            {

                rijndael.BlockSize = BlockSize;
                rijndael.Key = key;
                rijndael.IV = initializationVector;
                rijndael.Padding = PaddingMode.PKCS7;
                rijndael.Mode = CipherMode.CBC;

                MemoryStream tempStream = null;
                CryptoStream cryptoStream = null;
                try
                {
                    switch (command)
                    {
                        case AdvancedEncryptionStandardCommand.Decrypt:
                            tempStream = new MemoryStream(value);
                            cryptoStream = new CryptoStream(tempStream, rijndael.CreateDecryptor(), CryptoStreamMode.Read);
                            output = new byte[value.Length];
                            cryptoStream.Read(output, 0, output.Length);
                            tempStream = null;
                            output = ByteUtility.RemoveTrailingZeros(output);
                            break;
                        case AdvancedEncryptionStandardCommand.Encrypt:
                            tempStream = new MemoryStream();
                            cryptoStream = new CryptoStream(tempStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
                            cryptoStream.Write(value, 0, value.Length);
                            cryptoStream.FlushFinalBlock();
                            output = tempStream.ToArray();
                            tempStream = null;
                            break;
                    }
                }
                finally
                {
                    if (cryptoStream != null) { cryptoStream.Dispose(); }
                    if (tempStream != null) { tempStream.Dispose(); }
                }
            }
            return output;
        }

        /// <summary>
        /// Encrypts the specified value from the provided <paramref name="key"/> and <paramref name="initializationVector"/>.
        /// </summary>
        /// <param name="value">The value to encrypt.</param>
        /// <param name="key">The key to use in the encryption algorithm.</param>
        /// <param name="initializationVector">The initialization vector (IV) to use in the encryption algorithm.</param>
        /// <returns>The encrypted value.</returns>
        public static byte[] Encrypt(byte[] value, byte[] key, byte[] initializationVector)
        {
            return CryptoTransformCore(value, key, initializationVector, AdvancedEncryptionStandardCommand.Encrypt);
        }

        /// <summary>
        /// Decrypts the specified value from the provided <paramref name="key"/> and <paramref name="initializationVector"/>.
        /// </summary>
        /// <param name="value">The value to decrypt.</param>
        /// <param name="key">The key to use in the decryption algorithm.</param>
        /// <param name="initializationVector">The initialization vector (IV) to use in the decryption algorithm.</param>
        /// <returns>The decrypted value.</returns>
        public static byte[] Decrypt(byte[] value, byte[] key, byte[] initializationVector)
        {
            return CryptoTransformCore(value, key, initializationVector, AdvancedEncryptionStandardCommand.Decrypt);
        }

        private static void ValidateInput(byte[] value, byte[] key, byte[] initializationVector)
        {
            if (value == null) { throw new ArgumentNullException(nameof(value)); }
            if (key == null) { throw new ArgumentNullException(nameof(key)); }
            if (initializationVector == null) { throw new ArgumentNullException(nameof(initializationVector)); }

            int keyBits = key.Length * 8;
            int initializationVectorBits = initializationVector.Length * 8;
            if (!(keyBits == 128 || keyBits == 192 || keyBits == 256)) { throw new CryptographicException("The key does not meet the required size of either 128 bits, 192 bits or 256 bits."); }
            if (initializationVectorBits != BlockSize) { throw new CryptographicException("The initialization vector does not meet the required size of 128 bits."); }
        }

        private enum AdvancedEncryptionStandardCommand
        {
            Encrypt,
            Decrypt
        }
    }
}