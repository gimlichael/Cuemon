using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Cuemon.ComponentModel;
using Cuemon.ComponentModel.Codecs;
using Cuemon.ComponentModel.TypeConverters;
using Cuemon.Text;

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
			return ConvertFactory.UseCodec<StringToByteArrayCodec>().Encode(Generate.RandomString(BlockSize / 8), options =>
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
					result = Generate.RandomString(128 / 8);
					break;
				case AdvancedEncryptionStandardKeySize.AES192:
					result = Generate.RandomString(192 / 8);
					break;
				case AdvancedEncryptionStandardKeySize.AES256:
					goto default;
				default:
					result = Generate.RandomString(256 / 8);
					break;
			}
            return ConvertFactory.UseCodec<StringToByteArrayCodec>().Encode(result, options =>
            {
                options.Encoding = Encoding.UTF8;
                options.Preamble = PreambleSequence.Remove;
            });
		}

		private static byte[] CryptoTransformCore(byte[] value, byte[] key, byte[] initializationVector, AdvancedEncryptionStandardCommand command)
		{
			ValidateInput(value, key, initializationVector);
			using (var aes = Aes.Create())
			{
                aes.BlockSize = BlockSize;
                aes.Key = key;
                aes.IV = initializationVector;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;

                using (var sms = Disposable.SafeInvoke(() => new MemoryStream(), (ms, rijndael, bytes) =>
                {
                    CryptoStream cryptoStream;
                    switch (command)
                    {
                        case AdvancedEncryptionStandardCommand.Decrypt:
                            ms.Write(bytes, 0, bytes.Length);
                            cryptoStream = new CryptoStream(ms, rijndael.CreateDecryptor(), CryptoStreamMode.Read);
                            var cryptoBytes = new byte[bytes.Length];
                            cryptoStream.Read(cryptoBytes, 0, cryptoBytes.Length);
                            return new MemoryStream(RemoveTrailingZeros(cryptoBytes));
                        case AdvancedEncryptionStandardCommand.Encrypt:
                            cryptoStream = new CryptoStream(ms, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
                            cryptoStream.Write(bytes, 0, bytes.Length);
                            cryptoStream.FlushFinalBlock();
                            return ms;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(command), command, null);
                    }
                }, aes, value))
                {
                    return sms.ToArray();
                }
            }
        }

		/// <summary>
		/// Encrypts the specified value from the provided <paramref name="key"/> and <paramref name="initializationVector"/>.
		/// </summary>
		/// <param name="bytes">The value to encrypt.</param>
		/// <param name="key">The key to use in the encryption algorithm.</param>
		/// <param name="initializationVector">The initialization vector (IV) to use in the encryption algorithm.</param>
		/// <returns>The encrypted value.</returns>
		public static byte[] Encrypt(byte[] bytes, byte[] key, byte[] initializationVector)
		{
			return CryptoTransformCore(bytes, key, initializationVector, AdvancedEncryptionStandardCommand.Encrypt);
		}

		/// <summary>
		/// Decrypts the specified value from the provided <paramref name="key"/> and <paramref name="initializationVector"/>.
		/// </summary>
		/// <param name="bytes">The value to decrypt.</param>
		/// <param name="key">The key to use in the decryption algorithm.</param>
		/// <param name="initializationVector">The initialization vector (IV) to use in the decryption algorithm.</param>
		/// <returns>The decrypted value.</returns>
		public static byte[] Decrypt(byte[] bytes, byte[] key, byte[] initializationVector)
		{
			return CryptoTransformCore(bytes, key, initializationVector, AdvancedEncryptionStandardCommand.Decrypt);
		}

        /// <summary>
        /// Removes trailing zero information (if any) from the specified <see cref="byte"/> array.
        /// </summary>
        /// <param name="bytes">The <see cref="T:byte[]"/> to process.</param>
        /// <returns>A <see cref="byte"/> array without trailing zeros.</returns>
        public static byte[] RemoveTrailingZeros(byte[] bytes)
        {
            if (bytes == null) { throw new ArgumentNullException(nameof(bytes)); }
            if (bytes.Length <= 1) { throw new ArgumentException("Size must be larger than 1.", nameof(bytes)); }
            var hasTrailingZeros = false;
            var marker = bytes.Length - 1;
            while (bytes[marker] == 0)
            {
                if (!hasTrailingZeros) { hasTrailingZeros = true; }
                marker--;
            }
            if (!hasTrailingZeros) { return bytes; }
            marker++;
            var output = new byte[marker];
            Array.Copy(bytes, output, marker);
            return output;
        }

        private static void ValidateInput(byte[] bytes, byte[] key, byte[] initializationVector)
		{
            Validator.ThrowIfNull(bytes, nameof(bytes));
			Validator.ThrowIfNull(key, nameof(key));
			Validator.ThrowIfNull(initializationVector, nameof(initializationVector));
            var keyBits = key.Length * 8;
			var initializationVectorBits = initializationVector.Length * 8;
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