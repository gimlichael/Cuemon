using System;

namespace Cuemon
{
    /// <summary>
    /// Specifies options that is related to <see cref="TransientFaultHandling"/> operations.
    /// </summary>
    /// <seealso cref="TransientFaultHandling"/>.
    public class TransientFaultHandlingOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransientFaultHandlingOptions"/> class.
        /// </summary>
        public TransientFaultHandlingOptions()
        {
            RetryAttempts = DefaultRetryAttempts;
            EnableTransientFaultRecovery = RetryAttempts > 0;
            RecoveryWaitTimeCallback = currentAttempt => TimeSpan.FromSeconds(Math.Pow(2, currentAttempt > 5 ? 5 : currentAttempt));
            TransientFaultParserCallback = exception => false;
        }

        /// <summary>
        /// Gets or sets the amount of retry attempts for transient faults. Default value is <see cref="DefaultRetryAttempts"/>.
        /// </summary>
        /// <value>The retry attempts for transient faults.</value>
        public int RetryAttempts { get; set; }

        /// <summary>
        /// Gets or sets the default amount of retry attempts for transient faults. Default is 5 retry attempts.
        /// </summary>
        /// <value>The default amount of retry attempts for transient faults.</value>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="value"/> is zero.
        /// </exception>
        public static byte DefaultRetryAttempts { get; set; } = 5;

        /// <summary>
        /// Gets or sets a value indicating whether transient faults should be attempted gracefully recovered.
        /// </summary>
        /// <value><c>true</c> if transient faults should be attempted gracefully recovered; otherwise, <c>false</c>.</value>
        /// <remarks>For testing or diagnostic purposes, it can sometimes come in handy to turn off transient fault recovery.</remarks>
        public bool EnableTransientFaultRecovery { get; set; }

        /// <summary>
        /// Gets or sets the callback function delegate that determines the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.
        /// </summary>
        /// <returns>A <see cref="Func{TResult}"/> that determines the amount of time to wait for a transient fault to recover gracefully.</returns>
        /// <remarks>Default implementation is <see cref="int"/> + 2^ to a maximum of 5; eg. 1, 2, 4, 8, 16 to a total of 32 seconds.</remarks>
        public Func<int, TimeSpan> RecoveryWaitTimeCallback { get; set; }

        /// <summary>
        /// Gets or sets the callback function delegate that returns <c>true</c> if the <see cref="Exception"/>, when parsed, contains clues that would suggest a transient fault; otherwise, <c>false</c>.
        /// </summary>
        /// <value>A <see cref="Func{TResult}"/> that parses an <see cref="Exception"/> for clues that would suggest a transient fault.</value>
        /// <remarks>Default implementation is fixed to none-transient failure.</remarks>
        public Func<Exception, bool> TransientFaultParserCallback { get; set; }
    }
}