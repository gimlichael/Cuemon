using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Security;
using System.Text;
using Cuemon.Security;
using Cuemon.Security.Cryptography;

namespace Cuemon.Web.Security
{
	/// <summary>This utility class is designed to make web related security operations easier to work with.</summary>
	public static class WebSecurityUtility
	{
		/// <summary>
		/// Converts the specified <paramref name="location"/> to a tampering protected <see cref="Uri"/>.
		/// </summary>
		/// <param name="location">The URI to protect from tampering.</param>
		/// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
		/// <returns>An URI equivalent to the <paramref name="location"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
		public static Uri CreateTamperingProtectedUri(Uri location, byte[] securityKey)
		{
			if (location == null) { throw new ArgumentNullException(nameof(location)); }
			return CreateTamperingProtectedUri(location.OriginalString, securityKey);
		}

		/// <summary>
		/// Converts the specified <paramref name="location"/> to a tampering protected <see cref="Uri"/>.
		/// </summary>
		/// <param name="location">The URI to protect from tampering.</param>
		/// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
        /// <param name="settings">The settings to apply to the <see cref="SecurityToken"/>.</param>
		/// <returns>An URI equivalent to the <paramref name="location"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
        public static Uri CreateTamperingProtectedUri(Uri location, byte[] securityKey, SecurityTokenSettings settings)
		{
			if (location == null) { throw new ArgumentNullException(nameof(location)); }
            return CreateTamperingProtectedUri(location.OriginalString, securityKey, settings);
		}

		/// <summary>
		/// Converts the specified <paramref name="location"/> to a tampering protected <see cref="Uri"/>.
		/// </summary>
		/// <param name="location">The URI to protect from tampering.</param>
		/// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
        /// <param name="settings">The settings to apply to the <see cref="SecurityToken"/>.</param>
		/// <param name="algorithmType">The hash algorithm to use for the URI checksum computation. Default is <b><see cref="HashAlgorithmType.SHA1"/></b>.</param>
		/// <returns>An URI equivalent to the <paramref name="location"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
        public static Uri CreateTamperingProtectedUri(Uri location, byte[] securityKey, SecurityTokenSettings settings, HashAlgorithmType algorithmType)
		{
			if (location == null) { throw new ArgumentNullException(nameof(location)); }
			return CreateTamperingProtectedUri(location.OriginalString, securityKey, settings, algorithmType);
		}

		/// <summary>
		/// Converts the specified <paramref name="location"/> to a tampering protected <see cref="Uri"/>.
		/// </summary>
		/// <param name="location">The URI to protect from tampering.</param>
		/// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
        /// <param name="settings">The settings to apply to the <see cref="SecurityToken"/>.</param>
		/// <param name="algorithmType">The hash algorithm to use for the URI checksum computation. Default is <b><see cref="HashAlgorithmType.SHA1"/></b>.</param>
		/// <param name="secureUriFormat">The naming format of the required query string parameters of the tamper protected URI. Default is <b>?token={0}&amp;iv={1}&amp;salt={2}</b>, where you can change the naming of the query string parameters.</param>
		/// <returns>An URI equivalent to the <paramref name="location"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
        public static Uri CreateTamperingProtectedUri(Uri location, byte[] securityKey, SecurityTokenSettings settings, HashAlgorithmType algorithmType, string secureUriFormat)
		{
			if (location == null) { throw new ArgumentNullException(nameof(location)); }
			return CreateTamperingProtectedUri(location.OriginalString, securityKey, settings, algorithmType, secureUriFormat);
		}

		/// <summary>
		/// Converts the specified <paramref name="location"/> to a tampering protected <see cref="Uri"/>.
		/// </summary>
		/// <param name="location">The URI to protect from tampering.</param>
		/// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
        /// <param name="settings">The settings to apply to the <see cref="SecurityToken"/>.</param>
		/// <param name="algorithmType">The hash algorithm to use for the URI checksum computation. Default is <b><see cref="HashAlgorithmType.SHA1"/></b>.</param>
		/// <param name="secureUriFormat">The naming format of the required query string parameters of the tamper protected URI. Default is <b>?token={0}&amp;iv={1}&amp;salt={2}</b>, where you can change the naming of the query string parameters.</param>
		/// <param name="querystringParameterHashName">The name of the checksum parameter to append to the tampering protected URI. Default is <b>hash</b>.</param>
		/// <returns>An URI equivalent to the <paramref name="location"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
        public static Uri CreateTamperingProtectedUri(Uri location, byte[] securityKey, SecurityTokenSettings settings, HashAlgorithmType algorithmType, string secureUriFormat, string querystringParameterHashName)
		{
			if (location == null) { throw new ArgumentNullException(nameof(location)); }
			return CreateTamperingProtectedUri(location.OriginalString, securityKey, settings, algorithmType, secureUriFormat, querystringParameterHashName);
		}

		/// <summary>
		/// Converts the specified <paramref name="uriLocation"/> to a tampering protected <see cref="Uri"/>.
		/// </summary>
		/// <param name="uriLocation">The URI to protect from tampering.</param>
		/// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
		/// <returns>An URI equivalent to the <paramref name="uriLocation"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
		public static Uri CreateTamperingProtectedUri(string uriLocation, byte[] securityKey)
		{
			return CreateTamperingProtectedUri(uriLocation, securityKey, SecurityToken.CreateSettings(TimeSpan.FromMinutes(5)));
		}

		/// <summary>
		/// Converts the specified <paramref name="uriLocation"/> to a tampering protected <see cref="Uri"/>.
		/// </summary>
		/// <param name="uriLocation">The URI to protect from tampering.</param>
		/// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
        /// <param name="settings">The settings to apply to the <see cref="SecurityToken"/>.</param>
		/// <returns>An URI equivalent to the <paramref name="uriLocation"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
        public static Uri CreateTamperingProtectedUri(string uriLocation, byte[] securityKey, SecurityTokenSettings settings)
		{
			return CreateTamperingProtectedUri(uriLocation, securityKey, settings, HashAlgorithmType.SHA1);
		}

		/// <summary>
		/// Converts the specified <paramref name="uriLocation"/> to a tampering protected <see cref="Uri"/>.
		/// </summary>
		/// <param name="uriLocation">The URI to protect from tampering.</param>
		/// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
        /// <param name="settings">The settings to apply to the <see cref="SecurityToken"/>.</param>
		/// <param name="algorithmType">The hash algorithm to use for the URI checksum computation. Default is <b><see cref="HashAlgorithmType.SHA1"/></b>.</param>
		/// <returns>An URI equivalent to the <paramref name="uriLocation"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
        public static Uri CreateTamperingProtectedUri(string uriLocation, byte[] securityKey, SecurityTokenSettings settings, HashAlgorithmType algorithmType)
		{
			return CreateTamperingProtectedUri(uriLocation, securityKey, settings, algorithmType, "?token={0}&iv={1}&salt={2}");
		}

		/// <summary>
		/// Converts the specified <paramref name="uriLocation"/> to a tampering protected <see cref="Uri"/>.
		/// </summary>
		/// <param name="uriLocation">The URI to protect from tampering.</param>
		/// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
        /// <param name="settings">The settings to apply to the <see cref="SecurityToken"/>.</param>
		/// <param name="algorithmType">The hash algorithm to use for the URI checksum computation. Default is <b><see cref="HashAlgorithmType.SHA1"/></b>.</param>
		/// <param name="secureUriFormat">The naming format of the required query string parameters of the tamper protected URI. Default is <b>?token={0}&amp;iv={1}&amp;salt={2}</b>, where you can change the naming of the query string parameters.</param>
		/// <returns>An URI equivalent to the <paramref name="uriLocation"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
        public static Uri CreateTamperingProtectedUri(string uriLocation, byte[] securityKey, SecurityTokenSettings settings, HashAlgorithmType algorithmType, string secureUriFormat)
		{
			return CreateTamperingProtectedUri(uriLocation, securityKey, settings, algorithmType, secureUriFormat, "hash");
		}

		/// <summary>
		/// Converts the specified <paramref name="uriLocation"/> to a tampering protected <see cref="Uri"/>.
		/// </summary>
		/// <param name="uriLocation">The URI to protect from tampering.</param>
		/// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
		/// <param name="settings">The settings to apply to the <see cref="SecurityToken"/>.</param>
		/// <param name="algorithmType">The hash algorithm to use for the URI checksum computation. Default is <b><see cref="HashAlgorithmType.SHA1"/></b>.</param>
		/// <param name="secureUriFormat">The naming format of the required query string parameters of the tamper protected URI. Default is <b>?token={0}&amp;iv={1}&amp;salt={2}</b>, where you can change the naming of the query string parameters.</param>
		/// <param name="querystringParameterHashName">The name of the checksum parameter to append to the tampering protected URI. Default is <b>hash</b>.</param>
		/// <returns>An URI equivalent to the <paramref name="uriLocation"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
		public static Uri CreateTamperingProtectedUri(string uriLocation, byte[] securityKey, SecurityTokenSettings settings, HashAlgorithmType algorithmType, string secureUriFormat, string querystringParameterHashName)
		{
            Validator.ThrowIfNullOrEmpty(uriLocation, nameof(uriLocation));
            Validator.ThrowIfNull(securityKey, nameof(securityKey));
            Validator.ThrowIfNull(settings, nameof(settings));
            Validator.ThrowIfEqual(securityKey.Length, 0, nameof(securityKey));
            Validator.ThrowIfNullOrEmpty(secureUriFormat, nameof(secureUriFormat));

			int foundArguments;
			if (!StringUtility.ParseFormat(secureUriFormat, 3, out foundArguments)) { throw new ArgumentException("You must - in this order - specify three arguments for; 'token', 'iv' and 'salt'. This value cannot be exceeded nor the opposite. 'token', 'iv' and 'salt' is the default values."); }
			NameValueCollection formatedQuerytring = QueryStringConverter.FromString(secureUriFormat);

			SecurityToken securityToken = SecurityToken.Create(settings);
			byte[] iv = AdvancedEncryptionStandardUtility.GenerateInitializationVector();
			byte[] encryptedSecurityToken = SecurityUtility.CreateEncryptedSecurityToken(securityToken, securityKey, iv);
			string ivAsString = HttpUtility.UrlEncode(Encoding.UTF8.GetString(iv, 0, iv.Length));
			string encryptedSecurityTokenAsString = HttpUtility.UrlEncode(Convert.ToBase64String(encryptedSecurityToken));
			string salt = HttpUtility.UrlEncode(StringUtility.CreateRandomString(18));
			int indexOfQuestionMark = uriLocation.IndexOf('?');
			string uriLocationQuerystring = indexOfQuestionMark > 0 ? uriLocation.Substring(indexOfQuestionMark) : "";
			uriLocation = indexOfQuestionMark > 0 ? uriLocation.Substring(0, indexOfQuestionMark) : uriLocation;

			NameValueCollection querystring = QueryStringConverter.FromString(uriLocationQuerystring);
			NameValueCollection secureQuerystring = QueryStringConverter.FromString(string.Format(CultureInfo.InvariantCulture, secureUriFormat, encryptedSecurityTokenAsString, ivAsString, salt));
			secureQuerystring.Add(querystring);
			querystring = QueryStringUtility.RemoveDublets(secureQuerystring, formatedQuerytring.AllKeys);

			string secureUri = string.Format(CultureInfo.InvariantCulture, "{0}{1}", uriLocation, QueryStringConverter.FromNameValueCollection(querystring));
			secureUri += string.Format(CultureInfo.InvariantCulture, "&{0}={1}", querystringParameterHashName, HashUtility.ComputeHash(secureUri + salt + securityToken.Token, o =>
			{
			    o.AlgorithmType = algorithmType;
			    o.Encoding = Encoding.UTF8;
			}).ToHexadecimal());
			return new Uri(secureUri);
		}


		/// <summary>
		/// Parses and verifies the tampering protected URI.
		/// </summary>
		/// <param name="protectedUri">The tampering protected URI.</param>
		/// <param name="securityKey">The security key to use in the decryption of a <see cref="SecurityToken"/>.</param>
        /// <returns>An instance of the <see cref="SecurityToken"/> object if the <paramref name="protectedUri"/> is valid.</returns>
		/// <exception cref="SecurityException">This exception is thrown when an unsucessfull parse is meet, hence values has been tampered with, <paramref name="protectedUri"/> is invalid, token has expired or one or more of the necessary parameters is missing.</exception>
        public static SecurityToken ParseTamperingProtectedUri(Uri protectedUri, byte[] securityKey)
		{
			if (protectedUri == null) { throw new ArgumentNullException(nameof(protectedUri)); }
			NameValueCollection querystring = QueryStringConverter.FromString(protectedUri.Query);
			return ParseTamperingProtectedUri(protectedUri, securityKey, HttpUtility.UrlDecode(querystring["token"]));
		}

		/// <summary>
		/// Parses and verifies the tampering protected URI.
		/// </summary>
		/// <param name="protectedUri">The tampering protected URI.</param>
		/// <param name="securityKey">The security key to use in the decryption of the <paramref name="token"/>.</param>
		/// <param name="token">The security token to decrypt and parse for a <see cref="SecurityToken"/>.</param>
        /// <returns>An instance of the <see cref="SecurityToken"/> object if the <paramref name="protectedUri"/> is valid.</returns>
		/// <exception cref="SecurityException">This exception is thrown when an unsucessfull parse is meet, hence values has been tampered with, <paramref name="protectedUri"/> is invalid, token has expired or one or more of the necessary parameters is missing.</exception>
        public static SecurityToken ParseTamperingProtectedUri(Uri protectedUri, byte[] securityKey, string token)
		{
			if (protectedUri == null) { throw new ArgumentNullException(nameof(protectedUri)); }
			NameValueCollection querystring = QueryStringConverter.FromString(protectedUri.Query);
			return ParseTamperingProtectedUri(protectedUri, securityKey, token, HttpUtility.UrlDecode(querystring["iv"]));
		}

		/// <summary>
		/// Parses and verifies the tampering protected URI.
		/// </summary>
		/// <param name="protectedUri">The tampering protected URI.</param>
		/// <param name="securityKey">The security key to use in the decryption of the <paramref name="token"/>.</param>
		/// <param name="token">The security token to decrypt and parse for a <see cref="SecurityToken"/>.</param>
		/// <param name="iv">The initialization vector (IV) to use in the decryption of the <paramref name="token"/>.</param>
        /// <returns>An instance of the <see cref="SecurityToken"/> object if the <paramref name="protectedUri"/> is valid.</returns>
		/// <exception cref="SecurityException">This exception is thrown when an unsucessfull parse is meet, hence values has been tampered with, <paramref name="protectedUri"/> is invalid, token has expired or one or more of the necessary parameters is missing.</exception>
        public static SecurityToken ParseTamperingProtectedUri(Uri protectedUri, byte[] securityKey, string token, string iv)
		{
			if (protectedUri == null) { throw new ArgumentNullException(nameof(protectedUri)); }
			NameValueCollection querystring = QueryStringConverter.FromString(protectedUri.Query);
            return ParseTamperingProtectedUri(protectedUri, securityKey, token, iv, HttpUtility.UrlDecode(querystring["salt"]));
		}

		/// <summary>
		/// Parses and verifies the tampering protected URI.
		/// </summary>
		/// <param name="protectedUri">The tampering protected URI.</param>
		/// <param name="securityKey">The security key to use in the decryption of the <paramref name="token"/>.</param>
		/// <param name="token">The security token to decrypt and parse for a <see cref="SecurityToken"/>.</param>
		/// <param name="iv">The initialization vector (IV) to use in the decryption of the <paramref name="token"/>.</param>
		/// <param name="salt">The salt used in the computation of the integrity verification of <paramref name="protectedUri"/>.</param>
        /// <returns>An instance of the <see cref="SecurityToken"/> object if the <paramref name="protectedUri"/> is valid.</returns>
		/// <exception cref="SecurityException">This exception is thrown when an unsucessfull parse is meet, hence values has been tampered with, <paramref name="protectedUri"/> is invalid, token has expired or one or more of the necessary parameters is missing.</exception>
        public static SecurityToken ParseTamperingProtectedUri(Uri protectedUri, byte[] securityKey, string token, string iv, string salt)
		{
			if (protectedUri == null) { throw new ArgumentNullException(nameof(protectedUri)); }
			NameValueCollection querystring = QueryStringConverter.FromString(protectedUri.Query);
            return ParseTamperingProtectedUri(protectedUri, securityKey, token, iv, salt, HttpUtility.UrlDecode(querystring["hash"]));
		}

		/// <summary>
		/// Parses and verifies the tampering protected URI.
		/// </summary>
		/// <param name="protectedUri">The tampering protected URI.</param>
		/// <param name="securityKey">The security key to use in the decryption of the <paramref name="token"/>.</param>
		/// <param name="token">The security token to decrypt and parse for a <see cref="SecurityToken"/>.</param>
		/// <param name="iv">The initialization vector (IV) to use in the decryption of the <paramref name="token"/>.</param>
		/// <param name="salt">The salt used in the computation of the integrity verification of <paramref name="protectedUri"/>.</param>
		/// <param name="hash">The checksum to verify the integrity of <paramref name="protectedUri"/>.</param>
        /// <returns>An instance of the <see cref="SecurityToken"/> object if the <paramref name="protectedUri"/> is valid.</returns>
		/// <exception cref="SecurityException">This exception is thrown when an unsucessfull parse is meet, hence values has been tampered with, <paramref name="protectedUri"/> is invalid, token has expired or one or more of the necessary parameters is missing.</exception>
        public static SecurityToken ParseTamperingProtectedUri(Uri protectedUri, byte[] securityKey, string token, string iv, string salt, string hash)
		{
            return ParseTamperingProtectedUri(protectedUri, securityKey, token, iv, salt, hash, HashAlgorithmType.SHA1);
		}

		/// <summary>
		/// Parses and verifies the tampering protected URI.
		/// </summary>
		/// <param name="protectedUri">The tampering protected URI.</param>
		/// <param name="securityKey">The security key to use in the decryption of the <paramref name="token"/>.</param>
		/// <param name="token">The security token to decrypt and parse for a <see cref="SecurityToken"/>.</param>
		/// <param name="iv">The initialization vector (IV) to use in the decryption of the <paramref name="token"/>.</param>
		/// <param name="salt">The salt used in the computation of the integrity verification of <paramref name="protectedUri"/>.</param>
		/// <param name="hash">The checksum to verify the integrity of <paramref name="protectedUri"/>.</param>
		/// <param name="algorithmType">The hash algorithm to use for the <paramref name="protectedUri"/> checksum computation. Default is <b><see cref="HashAlgorithmType.SHA1"/></b>.</param>
        /// <returns>An instance of the <see cref="SecurityToken"/> object if the <paramref name="protectedUri"/> is valid.</returns>
		/// <exception cref="SecurityException">This exception is thrown when an unsucessfull parse is meet, hence values has been tampered with, <paramref name="protectedUri"/> is invalid, token has expired or one or more of the necessary parameters is missing.</exception>
        public static SecurityToken ParseTamperingProtectedUri(Uri protectedUri, byte[] securityKey, string token, string iv, string salt, string hash, HashAlgorithmType algorithmType)
		{
            return ParseTamperingProtectedUri(protectedUri, securityKey, token, iv, salt, hash, algorithmType, "hash");
		}

		/// <summary>
		/// Parses and verifies the tampering protected URI.
		/// </summary>
		/// <param name="protectedUri">The tampering protected URI.</param>
		/// <param name="securityKey">The security key to use in the decryption of the <paramref name="token"/>.</param>
		/// <param name="token">The security token to decrypt and parse for a <see cref="SecurityToken"/>.</param>
		/// <param name="iv">The initialization vector (IV) to use in the decryption of the <paramref name="token"/>.</param>
		/// <param name="salt">The salt used in the computation of the integrity verification of <paramref name="protectedUri"/>.</param>
		/// <param name="hash">The checksum to verify the integrity of <paramref name="protectedUri"/>.</param>
		/// <param name="algorithmType">The hash algorithm to use for the <paramref name="protectedUri"/> checksum computation. Default is <b><see cref="HashAlgorithmType.SHA1"/></b>.</param>
		/// <param name="querystringParameterHashName">The name of the checksum parameter to remove from the <paramref name="protectedUri"/> before integrity verification. Default is <b>hash</b>.</param>
        /// <returns>An instance of the <see cref="SecurityToken"/> object if the <paramref name="protectedUri"/> is valid.</returns>
		/// <exception cref="SecurityException">This exception is thrown when an unsucessfull parse is meet, hence values has been tampered with, <paramref name="protectedUri"/> is invalid, token has expired or one or more of the necessary parameters is missing.</exception>
        public static SecurityToken ParseTamperingProtectedUri(Uri protectedUri, byte[] securityKey, string token, string iv, string salt, string hash, HashAlgorithmType algorithmType, string querystringParameterHashName)
		{
			if (protectedUri == null) { throw new ArgumentNullException(nameof(protectedUri)); }
			if (string.IsNullOrEmpty(hash)) { throw new SecurityException("Security checksum was not specified."); }
			if (string.IsNullOrEmpty(salt)) { throw new SecurityException("Security salt was not specified."); }
			if (string.IsNullOrEmpty(token)) { throw new SecurityException("Security token was not specified."); }
			if (string.IsNullOrEmpty(iv)) { throw new SecurityException("Security initialization vector was not specified."); }
			if (querystringParameterHashName == null) { throw new ArgumentNullException(nameof(querystringParameterHashName)); }
			if (querystringParameterHashName.Length == 0) { throw new ArgumentException("Value cannot be empty.", nameof(querystringParameterHashName)); }
		    
            SecurityToken securityToken;
			try
			{
				securityToken = SecurityUtility.ParseEncryptedSecurityToken(Convert.FromBase64String(token), securityKey, Encoding.UTF8.GetBytes(iv));
				string originalUriString = string.Format(CultureInfo.InvariantCulture, protectedUri.IsDefaultPort ? "{0}{1}{2}{4}" : "{0}{1}{2}:{3}{4}",
					protectedUri.Scheme,
					"://",
					protectedUri.Host,
					protectedUri.Port,
					protectedUri.PathAndQuery);
				string querystring = QueryStringConverter.FromNameValueCollection(QueryStringUtility.Remove(protectedUri.Query, querystringParameterHashName));
				Uri originalUriWithRemovedChecksum = new Uri(originalUriString);

                string urlToCompute = string.Format(CultureInfo.InvariantCulture, "{0}{1}", new Uri(originalUriWithRemovedChecksum, originalUriWithRemovedChecksum.AbsolutePath), querystring);
				string computedChecksum = HashUtility.ComputeHash(urlToCompute + salt + securityToken.Token, o =>
				{
				    o.AlgorithmType = algorithmType;
				    o.Encoding = Encoding.UTF8;
				}).ToHexadecimal();
				if (!string.Equals(hash, computedChecksum)) { throw new SecurityException("Security checksum is invalid."); }
				if (securityToken.HasExpired) { throw new SecurityException("Security token is expired."); }
			}
			catch (SecurityException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new SecurityException("Invalid protected URI specified.", ex);
			}

		    return securityToken;
		}
	}
}