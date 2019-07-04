using System;
using System.Text;
using Cuemon.ComponentModel;
using Cuemon.ComponentModel.Codecs;
using Cuemon.ComponentModel.TypeConverters;
using Cuemon.Security.Cryptography;
using Cuemon.Text;

namespace Cuemon.Security
{
	/// <summary>
	/// This utility class is designed to make security operations easier to work with.
	/// </summary>
	public static class SecurityUtility
	{
		/// <summary>
		/// Creates a security token easily adopted into various services in the format of the <see cref="SecurityToken.ToString()"/> method.
		/// The token itself is encrypted using the <see cref="AdvancedEncryptionStandardUtility.Encrypt"/>
		/// </summary>
        /// <param name="settings">The to apply to <see cref="SecurityToken"/> instance.</param>
		/// <param name="securityKey">The key to use in the encryption algorithm.</param>
		/// <param name="securityInitializationVector">The initialization vector (IV) to use in the encryption algorithm.</param>
		/// <returns>A security token easily adopted into various services in the format of the <see cref="SecurityToken.ToString()"/> method.</returns>
		public static byte[] CreateEncryptedSecurityToken(SecurityTokenSettings settings, string securityKey, string securityInitializationVector)
		{
			return CreateEncryptedSecurityToken(settings, securityKey, securityInitializationVector, Encoding.UTF8);
		}

		/// <summary>
		/// Creates a security token easily adopted into various services in the format of the <see cref="SecurityToken.ToString()"/> method.
		/// The token itself is encrypted using the <see cref="AdvancedEncryptionStandardUtility.Encrypt"/>
		/// </summary>
        /// <param name="settings">The to apply to <see cref="SecurityToken"/> instance.</param>
		/// <param name="securityKey">The key to use in the encryption algorithm.</param>
		/// <param name="securityInitializationVector">The initialization vector (IV) to use in the encryption algorithm.</param>
		/// <param name="encoding">The encoding used for the <paramref name="securityKey"/> and <paramref name="securityInitializationVector"/>. Default is <see cref="Encoding.UTF8"/>.</param>
		/// <returns>A security token easily adopted into various services in the following XML format of the <see cref="SecurityToken.ToString()"/> method.</returns>
		public static byte[] CreateEncryptedSecurityToken(SecurityTokenSettings settings, string securityKey, string securityInitializationVector, Encoding encoding)
		{
            var token = SecurityToken.Create(settings);
			return CreateEncryptedSecurityToken(token, securityKey, securityInitializationVector, encoding);
		}

		/// <summary>
		/// Creates a security token easily adopted into various services in the format of the <see cref="SecurityToken.ToString()"/> method.
		/// The token itself is encrypted using the <see cref="AdvancedEncryptionStandardUtility.Encrypt"/>
		/// </summary>
		/// <param name="token">The <see cref="SecurityToken"/> to convert and encrypt to a byte[] representation.</param>
		/// <param name="securityKey">The key to use in the encryption algorithm.</param>
		/// <param name="securityInitializationVector">The initialization vector (IV) to use in the encryption algorithm.</param>
		/// <returns>A security token easily adopted into various services in the format of the <see cref="SecurityToken.ToString()"/> method.</returns>
		public static byte[] CreateEncryptedSecurityToken(SecurityToken token, string securityKey, string securityInitializationVector)
		{
			return CreateEncryptedSecurityToken(token, securityKey, securityInitializationVector, Encoding.UTF8);
		}

		/// <summary>
		/// Creates a security token easily adopted into various services in the format of the <see cref="SecurityToken.ToString()"/> method.
		/// The token itself is encrypted using the <see cref="AdvancedEncryptionStandardUtility.Encrypt"/>
		/// </summary>
		/// <param name="token">The <see cref="SecurityToken"/> to convert and encrypt to a byte[] representation.</param>
		/// <param name="securityKey">The key to use in the encryption algorithm.</param>
		/// <param name="securityInitializationVector">The initialization vector (IV) to use in the encryption algorithm.</param>
		/// <param name="encoding">The encoding used for the <paramref name="securityKey"/> and <paramref name="securityInitializationVector"/>. Default is <see cref="Encoding.UTF8"/>.</param>
		/// <returns>A security token easily adopted into various services in the following XML format of the <see cref="SecurityToken.ToString()"/> method.</returns>
		public static byte[] CreateEncryptedSecurityToken(SecurityToken token, string securityKey, string securityInitializationVector, Encoding encoding)
		{
            return CreateEncryptedSecurityToken(token, ConvertFactory.UseCodec<StringToByteArrayCodec>().Encode(securityKey, options =>
            {
                options.Encoding = encoding;
                options.Preamble = PreambleSequence.Remove;
            }), ConvertFactory.UseCodec<StringToByteArrayCodec>().Encode(securityInitializationVector, options =>
            {
                options.Encoding = encoding;
                options.Preamble = PreambleSequence.Remove;
            }));
		}

		/// <summary>
		/// Creates a security token easily adopted into various services in the format of the <see cref="SecurityToken.ToString()"/> method.
		/// The token itself is encrypted using the <see cref="AdvancedEncryptionStandardUtility.Encrypt"/> method.
		/// </summary>
        /// <param name="settings">The to apply to <see cref="SecurityToken"/> instance.</param>
		/// <param name="securityKey">The key to use in the encryption algorithm.</param>
		/// <param name="securityInitializationVector">The initialization vector (IV) to use in the encryption algorithm.</param>
		/// <returns>A security token easily adopted into various services in the format of the <see cref="SecurityToken.ToString()"/> method.</returns>
		public static byte[] CreateEncryptedSecurityToken(SecurityTokenSettings settings, byte[] securityKey, byte[] securityInitializationVector)
		{
            var token = SecurityToken.Create(settings);
			return CreateEncryptedSecurityToken(token, securityKey, securityInitializationVector);
		}

		/// <summary>
		/// Creates a security token easily adopted into various services in the format of the <see cref="SecurityToken.ToString()"/> method.
		/// The token itself is encrypted using the <see cref="AdvancedEncryptionStandardUtility.Encrypt"/> method.
		/// </summary>
		/// <param name="token">The <see cref="SecurityToken"/> to convert and encrypt to a byte[] representation.</param>
		/// <param name="securityKey">The key to use in the encryption algorithm.</param>
		/// <param name="securityInitializationVector">The initialization vector (IV) to use in the encryption algorithm.</param>
		/// <returns>A security token easily adopted into various services in the format of the <see cref="SecurityToken.ToString()"/> method.</returns>
		public static byte[] CreateEncryptedSecurityToken(SecurityToken token, byte[] securityKey, byte[] securityInitializationVector)
		{
			if (token == null) { throw new ArgumentNullException(nameof(token)); }
            var securityToken = AdvancedEncryptionStandardUtility.Encrypt(ConvertFactory.UseCodec<StringToByteArrayCodec>().Encode(token.ToString(), options =>
            {
                options.Encoding = Encoding.UTF8;
                options.Preamble = PreambleSequence.Remove;
            }), securityKey, securityInitializationVector);
			return securityToken;
		}

		/// <summary>
		/// Creates a <see cref="SecurityToken"/> instance from the parsed encrypted <paramref name="securityToken"/>.
		/// The token itself is decrypted using the <see cref="AdvancedEncryptionStandardUtility.Decrypt"/> method.
		/// </summary>
		/// <param name="securityToken">The security token to parse.</param>
		/// <param name="securityKey">The key to use in the decryption algorithm.</param>
		/// <param name="securityInitializationVector">The initialization vector (IV) to use in the decryption algorithm.</param>
		/// <returns>An instance of the <see cref="SecurityToken"/> object.</returns>
		public static SecurityToken ParseEncryptedSecurityToken(byte[] securityToken, string securityKey, string securityInitializationVector)
		{
			return ParseEncryptedSecurityToken(securityToken, securityKey, securityInitializationVector, Encoding.UTF8);
		}

		/// <summary>
		/// Creates a <see cref="SecurityToken"/> instance from the parsed encrypted <paramref name="securityToken"/>.
		/// The token itself is decrypted using the <see cref="AdvancedEncryptionStandardUtility.Decrypt"/> method.
		/// </summary>
		/// <param name="securityToken">The security token to parse.</param>
		/// <param name="securityKey">The key to use in the decryption algorithm.</param>
		/// <param name="securityInitializationVector">The initialization vector (IV) to use in the decryption algorithm.</param>
		/// <param name="encoding">The encoding used for the <paramref name="securityKey"/> and <paramref name="securityInitializationVector"/>. Default is <see cref="Encoding.UTF8"/>.</param>
		/// <returns>An instance of the <see cref="SecurityToken"/> object.</returns>
		public static SecurityToken ParseEncryptedSecurityToken(byte[] securityToken, string securityKey, string securityInitializationVector, Encoding encoding)
		{
            return ParseEncryptedSecurityToken(securityToken, ConvertFactory.UseCodec<StringToByteArrayCodec>().Encode(securityKey, options =>
            {
                options.Encoding = encoding;
                options.Preamble = PreambleSequence.Remove;
            }), ConvertFactory.UseCodec<StringToByteArrayCodec>().Encode(securityInitializationVector, options =>
            {
                options.Encoding = encoding;
                options.Preamble = PreambleSequence.Remove;
            }));
		}

		/// <summary>
		/// Creates a <see cref="SecurityToken"/> instance from the parsed encrypted <paramref name="securityToken"/>.
		/// The token itself is decrypted using the <see cref="AdvancedEncryptionStandardUtility.Decrypt"/> method.
		/// </summary>
		/// <param name="securityToken">The security token to parse.</param>
		/// <param name="securityKey">The key to use in the decryption algorithm.</param>
		/// <param name="securityInitializationVector">The initialization vector (IV) to use in the decryption algorithm.</param>
		/// <returns>An instance of the <see cref="SecurityToken"/> object.</returns>
		public static SecurityToken ParseEncryptedSecurityToken(byte[] securityToken, byte[] securityKey, byte[] securityInitializationVector)
		{
			return SecurityToken.Parse(ConvertFactory.UseCodec<StringToByteArrayCodec>().Decode(AdvancedEncryptionStandardUtility.Decrypt(securityToken, securityKey, securityInitializationVector), options =>
            {
                options.Encoding = Encoding.UTF8;
                options.Preamble = PreambleSequence.Remove;
            }));
		}
	}
}