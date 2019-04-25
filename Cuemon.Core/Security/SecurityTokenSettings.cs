using System;

namespace Cuemon.Security
{
    /// <summary>
    /// Specifies a set of features to support on the <see cref="SecurityToken"/> object created by the <see cref="SecurityToken.Create"/> method.
    /// </summary>
    public sealed class SecurityTokenSettings // todo: refactor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenSettings"/> class.
        /// </summary>
        public SecurityTokenSettings() : this(TimeSpan.FromSeconds(15))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityToken"/> class.
        /// </summary>
        /// <param name="timeToLive">The amount of time this token remains usable. Default is 15 seconds.</param>
        public SecurityTokenSettings(TimeSpan timeToLive) : this(timeToLive, 24)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityToken"/> class.
        /// </summary>
        /// <param name="timeToLive">The amount of time this token remains usable.</param>
        /// <param name="lengthOfToken">The length of the random generated token. Default is 24.</param>
	    public SecurityTokenSettings(TimeSpan timeToLive, int lengthOfToken) : this(timeToLive, lengthOfToken, "")
	    {
	    }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityToken"/> class.
        /// </summary>
        /// <param name="timeToLive">The amount of time this token remains usable.</param>
        /// <param name="reference">The reference of this token.</param>
	    public SecurityTokenSettings(TimeSpan timeToLive, string reference) : this(timeToLive, 24, reference)
	    {
	    }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenSettings"/> class.
        /// </summary>
        /// <param name="timeToLive">The amount of time this token remains usable.</param>
        /// <param name="lengthOfToken">The length of the random generated token. Default is 24.</param>
        /// <param name="reference">The reference of this token.</param>
        public SecurityTokenSettings(TimeSpan timeToLive, int lengthOfToken, string reference)
        {
            Validator.ThrowIfLowerThanOrEqual(timeToLive.Ticks, 0, nameof(timeToLive), "Token must have a TTL larger than 0.");
            Validator.ThrowIfLowerThan(lengthOfToken, 4, nameof(lengthOfToken), "Token must have a length of at least 4.");
            Validator.ThrowIfGreaterThan(lengthOfToken, 1024, nameof(lengthOfToken), "Token cannot exceed the length of 1024.");
            if (reference == null) { reference = ""; }

            TimeToLive = timeToLive;
            LengthOfToken = lengthOfToken;
            Reference = reference;
        }

        /// <summary>
        /// Gets the amount of time to keep this token alive.
        /// </summary>
        /// <value>The amount of time to keep this token alive.</value>
        public TimeSpan TimeToLive { get; private set; }

        /// <summary>
        /// Gets the reference of this token.
        /// </summary>
        /// <value>The reference of this token.</value>
        public string Reference { get; private set; }


        /// <summary>
        /// Gets the length of this token.
        /// </summary>
        /// <value>The length of this token.</value>
        public int LengthOfToken { get; private set; }
    }
}
