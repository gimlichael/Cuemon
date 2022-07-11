using System;
using Cuemon.AspNetCore.Authentication.Hmac;

namespace Cuemon.Extensions.AspNetCore.Authentication.AwsSignature4
{
    /// <summary>
    /// A collection of constants for <see cref="Aws4HmacAuthorizationHeaderBuilder"/> and related.
    /// </summary>
    public static class Aws4HmacFields
    {
        /// <summary>
        /// The date-time value of the request expressed as, what Amazon calls, an ISO8601 basic date time format.
        /// </summary>
        /// <remarks>https://github.com/aws/aws-sdk-net/blob/master/sdk/src/Core/Amazon.Util/AWSSDKUtils.cs</remarks>
        public const string DateTimeStamp = "iso8601BasicDateTimeFormat";

        /// <summary>
        /// The AWS date-time format used when converting a <see cref="DateTime"/> value to its equivalent string representation.
        /// </summary>
        /// <remarks>https://github.com/aws/aws-sdk-net/blob/master/sdk/src/Core/Amazon.Util/AWSSDKUtils.cs</remarks>
        public const string DateTimeStampFormat = "yyyyMMddTHHmmssZ";

        /// <summary>
        /// The date only value of the request expressed as, what Amazon calls, an ISO8601 basic date format.
        /// </summary>
        /// <remarks>https://github.com/aws/aws-sdk-net/blob/master/sdk/src/Core/Amazon.Util/AWSSDKUtils.cs</remarks>
        public const string DateStamp = "iso8601BasicDateFormat";

        /// <summary>
        /// The AWS date only format used when converting a <see cref="DateTime"/> value to its equivalent string representation.
        /// </summary>
        /// <remarks>https://github.com/aws/aws-sdk-net/blob/master/sdk/src/Core/Amazon.Util/AWSSDKUtils.cs</remarks>
        public const string DateStampFormat = "yyyyMMdd";

        /// <summary>
        /// The region part of the signing key tied to a <see cref="HmacFields.CredentialScope"/>.
        /// </summary>
        public const string Region = "awsRegion";

        /// <summary>
        /// The service part of the signing key tied to a <see cref="HmacFields.CredentialScope"/>.
        /// </summary>
        public const string Service = "awsService";

        /// <summary>
        /// The final part of the signing key tied to a <see cref="HmacFields.CredentialScope"/>.
        /// </summary>
        public const string Aws4Request = "aws4_request";

        /// <summary>
        /// The authentication scheme of the <see cref="Aws4HmacAuthorizationHeader"/>.
        /// </summary>
        /// <remarks>https://docs.aws.amazon.com/AmazonS3/latest/API/sigv4-auth-using-authorization-header.html</remarks>
        public const string Scheme = "AWS4-HMAC-SHA256";
    }
}
