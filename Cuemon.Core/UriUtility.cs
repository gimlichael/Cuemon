using System;
using System.Collections.Generic;
using Cuemon.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="Uri"/> operations easier to work with.
    /// </summary>
    public static class UriUtility
    {
        /// <summary>
        /// Gets all URI schemes currently supported by the .NET framework.
        /// </summary>
        /// <returns>A sequence of all URI schemes currently supported by the .NET framework.</returns>
        public static IEnumerable<UriScheme> AllUriSchemes
        {
            get
            {
                return EnumerableConverter.FromArray(UriScheme.File, UriScheme.Ftp, UriScheme.Gopher, UriScheme.Http, UriScheme.Https, UriScheme.Mailto, UriScheme.NetPipe, UriScheme.NetTcp, UriScheme.News, UriScheme.Nntp);
            }
        }

        /// <summary>
        /// Determines whether the specified value is an absolute URI string from all known URI schemes.
        /// </summary>
        /// <param name="value">The string value representing the URI.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value evaluates to an URI; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUri(string value)
        {
            return IsUri(value, UriKind.Absolute);
        }

        /// <summary>
        /// Determines whether the specified value is an URI string from all known URI schemes.
        /// </summary>
        /// <param name="value">The string value representing the URI.</param>
        /// <param name="uriKind">The type of the URI.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value evaluates to an URI; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUri(string value, UriKind uriKind)
        {
            return IsUri(value, uriKind, AllUriSchemes);
        }

        /// <summary>
        /// Determines whether the specified value is an absolute URI string.
        /// </summary>
        /// <param name="value">The string value representing the URI.</param>
        /// <param name="uriSchemes">A sequence of <see cref="UriScheme"/> values to use in the validation of the URI.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value evaluates to an absolute URI; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUri(string value, IEnumerable<UriScheme> uriSchemes)
        {
            return IsUri(value, UriKind.Absolute, uriSchemes);
        }

        /// <summary>
        /// Determines whether the specified value is an URI string.
        /// </summary>
        /// <param name="value">The string value representing the URI.</param>
        /// <param name="uriKind">The type of the URI.</param>
        /// <param name="uriSchemes">A sequence of <see cref="UriScheme"/> values to use in the validation of the URI.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value evaluates to an URI; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUri(string value, UriKind uriKind, IEnumerable<UriScheme> uriSchemes)
        {
            Uri ignoreUri;
            return TryParse(value, uriKind, uriSchemes, out ignoreUri);
        }

        /// <summary>
        /// Determines whether the specified value is a protocol-relative URI string.
        /// </summary>
        /// <param name="value">The string value representing the protocol-relative URI.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is a protocol-relative URI; otherwise, <c>false</c>.</returns>
        public static bool IsProtocolRelativeUri(string value)
        {
            return IsProtocolRelativeUri(value, StringUtility.NetworkPathReference);
        }

        /// <summary>
        /// Determines whether the specified value is a protocol-relative URI string.
        /// </summary>
        /// <param name="value">The string value representing the protocol-relative URI.</param>
        /// <param name="relativeReference">The relative reference that <paramref name="value"/> must begin with. Default is <see cref="StringUtility.NetworkPathReference"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="value"/> is a protocol-relative URI; otherwise, <c>false</c>.</returns>
        public static bool IsProtocolRelativeUri(string value, string relativeReference)
        {
            Validator.ThrowIfNullOrEmpty(value, nameof(value));
            Validator.ThrowIfNullOrEmpty(relativeReference, nameof(relativeReference));
            return Patterns.TryParse(() => UriConverter.FromProtocolRelativeUri(value, UriScheme.Https, relativeReference), out _);
        }

        /// <summary>
        /// Converts the specified string representation of an URI value to its <see cref="Uri"/> equivalent.
        /// </summary>
        /// <param name="uriString">A string containing the URI to convert.</param>
        /// <param name="uriKind">The type of the URI.</param>
        /// <param name="result">When this method returns, contains the constructed <see cref="Uri"/>.</param>
        /// <returns>
        ///     <c>true</c> if the <see cref="Uri"/> was successfully created; otherwise, false.
        /// </returns>
        /// <remarks>
        /// If this method returns true, the new <see cref="Uri"/> is in result.
        /// </remarks>
        public static bool TryParse(string uriString, UriKind uriKind, out Uri result)
        {
            return TryParse(uriString, uriKind, AllUriSchemes, out result);
        }

        /// <summary>
        /// Converts the specified string representation of an URI value to its <see cref="Uri"/> equivalent, limited to what is specified in the <see paramref="schemes"/> parameter.
        /// </summary>
        /// <param name="uriString">A string containing the URI to convert.</param>
        /// <param name="uriKind">The type of the URI.</param>
        /// <param name="uriSchemes">A sequence of <see cref="UriScheme"/> values to use in the parsing of the URI.</param>
        /// <param name="result">When this method returns, contains the constructed <see cref="Uri"/>.</param>
        /// <returns>
        ///     <c>true</c> if the <see cref="Uri"/> was successfully created; otherwise, false.
        /// </returns>
        /// <remarks>
        /// If this method returns true, the new <see cref="Uri"/> is in result.
        /// </remarks>
        public static bool TryParse(string uriString, UriKind uriKind, IEnumerable<UriScheme> uriSchemes, out Uri result)
        {
            return Patterns.TryParse(() => Parse(uriString, uriKind, uriSchemes), out result);
        }

        /// <summary>
        /// Converts the specified string representation of an URI value to its <see cref="Uri"/> equivalent, limited to what is specified in the <see paramref="schemes"/> parameter.
        /// </summary>
        /// <param name="uriString">A string containing the URI to convert.</param>
        /// <param name="uriKind">The type of the URI.</param>
        /// <returns>An <see cref="Uri"/> that is equivalent to the value contained in <paramref name="uriString"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="uriString"/> is null.
        /// </exception>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="uriString"/> is empty.
        /// </exception>
        public static Uri Parse(string uriString, UriKind uriKind)
        {
            return Parse(uriString, uriKind, AllUriSchemes);
        }

        /// <summary>
        /// Converts the specified string representation of an URI value to its <see cref="Uri"/> equivalent, limited to what is specified in the <see paramref="schemes"/> parameter.
        /// </summary>
        /// <param name="uriString">A string containing the URI to convert.</param>
        /// <param name="uriKind">The type of the URI.</param>
        /// <param name="uriSchemes">A sequence of <see cref="UriScheme"/> values to use in the parsing of the URI.</param>
        /// <returns>An <see cref="Uri"/> that is equivalent to the value contained in <paramref name="uriString"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="uriString"/> is null - or - <paramref name="uriSchemes"/> is null.
        /// </exception>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="uriString"/> is empty.
        /// </exception>
        public static Uri Parse(string uriString, UriKind uriKind, IEnumerable<UriScheme> uriSchemes)
        {
            Validator.ThrowIfNullOrEmpty(uriString, nameof(uriString));
            Validator.ThrowIfNull(uriSchemes, nameof(uriSchemes));

            bool isValid = false;
            foreach (UriScheme scheme in uriSchemes)
            {
                switch (scheme)
                {
                    case UriScheme.File:
                    case UriScheme.Ftp:
                    case UriScheme.Gopher:
                    case UriScheme.Http:
                    case UriScheme.Https:
                    case UriScheme.Mailto:
                    case UriScheme.NetPipe:
                    case UriScheme.NetTcp:
                    case UriScheme.News:
                    case UriScheme.Nntp:
                        string validUriScheme = StringConverter.FromUriScheme(scheme);
                        isValid = uriString.StartsWith(validUriScheme, StringComparison.OrdinalIgnoreCase);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(uriSchemes));
                }
                if (isValid) { break; }
            }

            Uri result;
            if (!isValid ||
                !Uri.TryCreate(uriString, uriKind, out result)) { throw new ArgumentException("The specified uriString is not a valid URI.", nameof(uriString)); }
            return result;
        }
    }
}