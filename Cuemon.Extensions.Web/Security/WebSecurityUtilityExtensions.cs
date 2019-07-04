using System;
using System.Security;
using Cuemon.Integrity;
using Cuemon.Security;
using Cuemon.Security.Cryptography;

namespace Cuemon.Extensions.Web.Security
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="WebSecurityUtility"/> class.
    /// </summary>
    public static class WebSecurityUtilityExtensions// todo refactor
    {
        /// <summary>
        /// Converts the specified <paramref name="location"/> to a tampering protected <see cref="Uri"/>.
        /// </summary>
        /// <param name="location">The URI to protect from tampering.</param>
        /// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
        /// <returns>a URI equivalent to the <paramref name="location"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
        public static Uri ToProtectedUri(this Uri location, byte[] securityKey)
        {
            return WebSecurityUtility.CreateTamperingProtectedUri(location, securityKey);
        }

        /// <summary>
        /// Converts the specified <paramref name="location"/> to a tampering protected <see cref="Uri"/>.
        /// </summary>
        /// <param name="location">The URI to protect from tampering.</param>
        /// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
        /// <param name="settings">The settings to apply to the <see cref="SecurityToken"/>.</param>
        /// <returns>a URI equivalent to the <paramref name="location"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
        public static Uri ToProtectedUri(this Uri location, byte[] securityKey, SecurityTokenSettings settings)
        {
            return WebSecurityUtility.CreateTamperingProtectedUri(location, securityKey, settings);
        }

        /// <summary>
        /// Converts the specified <paramref name="location"/> to a tampering protected <see cref="Uri"/>.
        /// </summary>
        /// <param name="location">The URI to protect from tampering.</param>
        /// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
        /// <param name="settings">The settings to apply to the <see cref="SecurityToken"/>.</param>
        /// <param name="algorithm">The hash algorithm to use for the URI checksum computation. Default is <b><see cref="HashAlgorithmType.SHA1"/></b>.</param>
        /// <returns>a URI equivalent to the <paramref name="location"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
        public static Uri ToProtectedUri(this Uri location, byte[] securityKey, SecurityTokenSettings settings, CryptoAlgorithm algorithm)
        {
            return WebSecurityUtility.CreateTamperingProtectedUri(location, securityKey, settings, algorithm);
        }

        /// <summary>
        /// Converts the specified <paramref name="location"/> to a tampering protected <see cref="Uri"/>.
        /// </summary>
        /// <param name="location">The URI to protect from tampering.</param>
        /// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
        /// <param name="settings">The settings to apply to the <see cref="SecurityToken"/>.</param>
        /// <param name="algorithm">The hash algorithm to use for the URI checksum computation. Default is <b><see cref="HashAlgorithmType.SHA1"/></b>.</param>
        /// <param name="secureUriFormat">The naming format of the required query string parameters of the tamper protected URI. Default is <b>?token={0}&amp;iv={1}&amp;salt={2}</b>, where you can change the naming of the query string parameters.</param>
        /// <returns>a URI equivalent to the <paramref name="location"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
        public static Uri ToProtectedUri(this Uri location, byte[] securityKey, SecurityTokenSettings settings, CryptoAlgorithm algorithm, string secureUriFormat)
        {
            return WebSecurityUtility.CreateTamperingProtectedUri(location, securityKey, settings, algorithm, secureUriFormat);
        }

        /// <summary>
        /// Converts the specified <paramref name="location"/> to a tampering protected <see cref="Uri"/>.
        /// </summary>
        /// <param name="location">The URI to protect from tampering.</param>
        /// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
        /// <param name="settings">The settings to apply to the <see cref="SecurityToken"/>.</param>
        /// <param name="algorithm">The hash algorithm to use for the URI checksum computation. Default is <b><see cref="HashAlgorithmType.SHA1"/></b>.</param>
        /// <param name="secureUriFormat">The naming format of the required query string parameters of the tamper protected URI. Default is <b>?token={0}&amp;iv={1}&amp;salt={2}</b>, where you can change the naming of the query string parameters.</param>
        /// <param name="querystringParameterHashName">The name of the checksum parameter to append to the tampering protected URI. Default is <b>hash</b>.</param>
        /// <returns>a URI equivalent to the <paramref name="location"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
        public static Uri ToProtectedUri(this Uri location, byte[] securityKey, SecurityTokenSettings settings, CryptoAlgorithm algorithm, string secureUriFormat, string querystringParameterHashName)
        {
            return WebSecurityUtility.CreateTamperingProtectedUri(location, securityKey, settings, algorithm, secureUriFormat, querystringParameterHashName);
        }

        /// <summary>
        /// Converts the specified <paramref name="uriLocation"/> to a tampering protected <see cref="Uri"/>.
        /// </summary>
        /// <param name="uriLocation">The URI to protect from tampering.</param>
        /// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
        /// <returns>a URI equivalent to the <paramref name="uriLocation"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
        public static Uri ToProtectedUri(this string uriLocation, byte[] securityKey)
        {
            return WebSecurityUtility.CreateTamperingProtectedUri(uriLocation, securityKey);
        }

        /// <summary>
        /// Converts the specified <paramref name="uriLocation"/> to a tampering protected <see cref="Uri"/>.
        /// </summary>
        /// <param name="uriLocation">The URI to protect from tampering.</param>
        /// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
        /// <param name="settings">The settings to apply to the <see cref="SecurityToken"/>.</param>
        /// <returns>a URI equivalent to the <paramref name="uriLocation"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
        public static Uri ToProtectedUri(this string uriLocation, byte[] securityKey, SecurityTokenSettings settings)
        {
            return WebSecurityUtility.CreateTamperingProtectedUri(uriLocation, securityKey, settings);
        }

        /// <summary>
        /// Converts the specified <paramref name="uriLocation"/> to a tampering protected <see cref="Uri"/>.
        /// </summary>
        /// <param name="uriLocation">The URI to protect from tampering.</param>
        /// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
        /// <param name="settings">The settings to apply to the <see cref="SecurityToken"/>.</param>
        /// <param name="algorithm">The hash algorithm to use for the URI checksum computation. Default is <b><see cref="HashAlgorithmType.SHA1"/></b>.</param>
        /// <returns>a URI equivalent to the <paramref name="uriLocation"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
        public static Uri ToProtectedUri(this string uriLocation, byte[] securityKey, SecurityTokenSettings settings, CryptoAlgorithm algorithm)
        {
            return WebSecurityUtility.CreateTamperingProtectedUri(uriLocation, securityKey, settings, algorithm);
        }

        /// <summary>
        /// Converts the specified <paramref name="uriLocation"/> to a tampering protected <see cref="Uri"/>.
        /// </summary>
        /// <param name="uriLocation">The URI to protect from tampering.</param>
        /// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
        /// <param name="settings">The settings to apply to the <see cref="SecurityToken"/>.</param>
        /// <param name="algorithm">The hash algorithm to use for the URI checksum computation. Default is <b><see cref="HashAlgorithmType.SHA1"/></b>.</param>
        /// <param name="secureUriFormat">The naming format of the required query string parameters of the tamper protected URI. Default is <b>?token={0}&amp;iv={1}&amp;salt={2}</b>, where you can change the naming of the query string parameters.</param>
        /// <returns>a URI equivalent to the <paramref name="uriLocation"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
        public static Uri ToProtectedUri(this string uriLocation, byte[] securityKey, SecurityTokenSettings settings, CryptoAlgorithm algorithm, string secureUriFormat)
        {
            return WebSecurityUtility.CreateTamperingProtectedUri(uriLocation, securityKey, settings, algorithm, secureUriFormat);
        }

        /// <summary>
        /// Converts the specified <paramref name="uriLocation"/> to a tampering protected <see cref="Uri"/>.
        /// </summary>
        /// <param name="uriLocation">The URI to protect from tampering.</param>
        /// <param name="securityKey">The security key to use for the <see cref="SecurityToken"/> encryption.</param>
        /// <param name="settings">The settings to apply to the <see cref="SecurityToken"/>.</param>
        /// <param name="algorithm">The hash algorithm to use for the URI checksum computation. Default is <b><see cref="HashAlgorithmType.SHA1"/></b>.</param>
        /// <param name="secureUriFormat">The naming format of the required query string parameters of the tamper protected URI. Default is <b>?token={0}&amp;iv={1}&amp;salt={2}</b>, where you can change the naming of the query string parameters.</param>
        /// <param name="querystringParameterHashName">The name of the checksum parameter to append to the tampering protected URI. Default is <b>hash</b>.</param>
        /// <returns>a URI equivalent to the <paramref name="uriLocation"/> but protected from tampering - including but not limited to - MITM attacks.</returns>
        public static Uri ToProtectedUri(this string uriLocation, byte[] securityKey, SecurityTokenSettings settings, CryptoAlgorithm algorithm, string secureUriFormat, string querystringParameterHashName)
        {
            return WebSecurityUtility.CreateTamperingProtectedUri(uriLocation, securityKey, settings, algorithm, secureUriFormat, querystringParameterHashName);
        }

        /// <summary>
        /// Parses and verifies the tampering protected URI.
        /// </summary>
        /// <param name="protectedUri">The tampering protected URI.</param>
        /// <param name="securityKey">The security key to use in the decryption of a <see cref="SecurityToken"/>.</param>
        /// <exception cref="SecurityException">This exception is thrown when an unsucessfull parse is meet, hence values has been tampered with, <paramref name="protectedUri"/> is invalid, token has expired or one or more of the necessary parameters is missing.</exception>
        public static void VerifyProtectedUri(this Uri protectedUri, byte[] securityKey)
        {
            WebSecurityUtility.ParseTamperingProtectedUri(protectedUri, securityKey);
        }

        /// <summary>
        /// Parses and verifies the tampering protected URI.
        /// </summary>
        /// <param name="protectedUri">The tampering protected URI.</param>
        /// <param name="securityKey">The security key to use in the decryption of the <paramref name="token"/>.</param>
        /// <param name="token">The security token to decrypt and parse for a <see cref="SecurityToken"/>.</param>
        /// <exception cref="SecurityException">This exception is thrown when an unsucessfull parse is meet, hence values has been tampered with, <paramref name="protectedUri"/> is invalid, token has expired or one or more of the necessary parameters is missing.</exception>
        public static void VerifyProtectedUri(this Uri protectedUri, byte[] securityKey, string token)
        {
            WebSecurityUtility.ParseTamperingProtectedUri(protectedUri, securityKey, token);
        }

        /// <summary>
        /// Parses and verifies the tampering protected URI.
        /// </summary>
        /// <param name="protectedUri">The tampering protected URI.</param>
        /// <param name="securityKey">The security key to use in the decryption of the <paramref name="token"/>.</param>
        /// <param name="token">The security token to decrypt and parse for a <see cref="SecurityToken"/>.</param>
        /// <param name="iv">The initialization vector (IV) to use in the decryption of the <paramref name="token"/>.</param>
        /// <exception cref="SecurityException">This exception is thrown when an unsucessfull parse is meet, hence values has been tampered with, <paramref name="protectedUri"/> is invalid, token has expired or one or more of the necessary parameters is missing.</exception>
        public static void VerifyProtectedUri(this Uri protectedUri, byte[] securityKey, string token, string iv)
        {
            WebSecurityUtility.ParseTamperingProtectedUri(protectedUri, securityKey, token, iv);
        }

        /// <summary>
        /// Parses and verifies the tampering protected URI.
        /// </summary>
        /// <param name="protectedUri">The tampering protected URI.</param>
        /// <param name="securityKey">The security key to use in the decryption of the <paramref name="token"/>.</param>
        /// <param name="token">The security token to decrypt and parse for a <see cref="SecurityToken"/>.</param>
        /// <param name="iv">The initialization vector (IV) to use in the decryption of the <paramref name="token"/>.</param>
        /// <param name="salt">The salt used in the computation of the integrity verification of <paramref name="protectedUri"/>.</param>
        /// <exception cref="SecurityException">This exception is thrown when an unsucessfull parse is meet, hence values has been tampered with, <paramref name="protectedUri"/> is invalid, token has expired or one or more of the necessary parameters is missing.</exception>
        public static void VerifyProtectedUri(this Uri protectedUri, byte[] securityKey, string token, string iv, string salt)
        {
            WebSecurityUtility.ParseTamperingProtectedUri(protectedUri, securityKey, token, iv, salt);
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
        /// <exception cref="SecurityException">This exception is thrown when an unsucessfull parse is meet, hence values has been tampered with, <paramref name="protectedUri"/> is invalid, token has expired or one or more of the necessary parameters is missing.</exception>
        public static void VerifyProtectedUri(this Uri protectedUri, byte[] securityKey, string token, string iv, string salt, string hash)
        {
            WebSecurityUtility.ParseTamperingProtectedUri(protectedUri, securityKey, token, iv, salt, hash);
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
        /// <param name="algorithm">The hash algorithm to use for the <paramref name="protectedUri"/> checksum computation. Default is <b><see cref="HashAlgorithmType.SHA1"/></b>.</param>
        /// <exception cref="SecurityException">This exception is thrown when an unsucessfull parse is meet, hence values has been tampered with, <paramref name="protectedUri"/> is invalid, token has expired or one or more of the necessary parameters is missing.</exception>
        public static void VerifyProtectedUri(this Uri protectedUri, byte[] securityKey, string token, string iv, string salt, string hash, CryptoAlgorithm algorithm)
        {
            WebSecurityUtility.ParseTamperingProtectedUri(protectedUri, securityKey, token, iv, salt, hash, algorithm);
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
        /// <param name="algorithm">The hash algorithm to use for the <paramref name="protectedUri"/> checksum computation. Default is <b><see cref="HashAlgorithmType.SHA1"/></b>.</param>
        /// <param name="querystringParameterHashName">The name of the checksum parameter to remove from the <paramref name="protectedUri"/> before integrity verification. Default is <b>hash</b>.</param>
        /// <exception cref="SecurityException">This exception is thrown when an unsucessfull parse is meet, hence values has been tampered with, <paramref name="protectedUri"/> is invalid, token has expired or one or more of the necessary parameters is missing.</exception>
        public static void VerifyProtectedUri(this Uri protectedUri, byte[] securityKey, string token, string iv, string salt, string hash, CryptoAlgorithm algorithm, string querystringParameterHashName)
        {
            WebSecurityUtility.ParseTamperingProtectedUri(protectedUri, securityKey, token, iv, salt, hash, algorithm, querystringParameterHashName);
        }
    }
}