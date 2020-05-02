using System;

namespace Cuemon.Integrity
{
    /// <summary>
    /// An interface that represents the timestamp that is normally associated with a resource.
    /// </summary>
    public interface ICacheableTimestamp
    {
        /// <summary>
        /// Gets a <see cref="DateTime"/> value from when data this resource represents was first created, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <value>The timestamp from when data this resource represents was first created.</value>
        DateTime Created { get; }

        /// <summary>
        /// Gets a <see cref="DateTime"/> value from when data this resource represents was last modified, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <value>The timestamp from when data this resource represents was last modified.</value>
        DateTime? Modified { get; }
    }
}