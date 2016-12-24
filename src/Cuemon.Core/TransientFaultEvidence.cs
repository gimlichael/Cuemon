using System;

namespace Cuemon
{
    /// <summary>
    /// Provides evidence about a faulted <see cref="TransientOperation"/>.
    /// </summary>
    public class TransientFaultEvidence
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransientFaultEvidence"/> class.
        /// </summary>
        /// <param name="attempts">The number of attempts the <paramref name="member"/> was invoked.</param>
        /// <param name="recoveryWaitTime">The last wait time attempting recovery of <paramref name="member"/>.</param>
        /// <param name="totalRecoveryWaitTime">The total wait time attempting recovery of <paramref name="member"/>.</param>
        /// <param name="latency">The latency experienced with <paramref name="member"/>.</param>
        /// <param name="member">The member being protected from a transient fault.</param>
        public TransientFaultEvidence(int attempts, TimeSpan recoveryWaitTime, TimeSpan totalRecoveryWaitTime, TimeSpan latency, string member)
        {
            Attempts = attempts;
            RecoveryWaitTime = recoveryWaitTime;
            TotalRecoveryWaitTime = totalRecoveryWaitTime;
            Latency = latency;
            Member = member;
        }

        /// <summary>
        /// Gets the number of attempts the <see cref="Member"/> was invoked.
        /// </summary>
        /// <value>The number of attempts the <see cref="Member"/> was invoked.</value>
        public int Attempts { get; }

        /// <summary>
        /// Gets the last wait time attempting recovery of <see cref="Member"/>.
        /// </summary>
        /// <value>The last wait time attempting recovery of <see cref="Member"/>.</value>
        public TimeSpan RecoveryWaitTime { get; }

        /// <summary>
        /// Gets the total wait time attempting recovery of <see cref="Member"/>.
        /// </summary>
        /// <value>The total wait time attempting recovery of <see cref="Member"/>.</value>
        public TimeSpan TotalRecoveryWaitTime { get; }

        /// <summary>
        /// Gets the latency experienced with <see cref="Member"/>.
        /// </summary>
        /// <value>The latency experienced with <see cref="Member"/>.</value>
        public TimeSpan Latency { get; }

        /// <summary>
        /// Gets the member being protected from a transient fault.
        /// </summary>
        /// <value>The member being protected from a transient fault.</value>
        public string Member { get; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return "{0} was invoked {1} time(s) over a period of {2}. Last recovery wait time was {3}, giving a total recovery wait time of {4}. Latency was {5}.".FormatWith(
                Member,
                Attempts,
                Latency.Add(TotalRecoveryWaitTime),
                RecoveryWaitTime,
                TotalRecoveryWaitTime,
                Latency);
        }
    }
}