namespace Cuemon.Integrity
{
    /// <summary>
    /// An interface that represents the data integrity that is normally associated with a data-set.
    /// </summary>
    public interface ICacheableIntegrity
    {
        /// <summary>
        /// Gets the validation strength of the integrity of this instance.
        /// </summary>
        /// <value>The validation strength of the integrity of this instance.</value>
        ChecksumStrength Validation { get; }

        /// <summary>
        /// Gets a <see cref="ChecksumResult"/> that represents the integrity of this instance.
        /// </summary>
        /// <value>The checksum that represents the integrity of this instance.</value>
        ChecksumResult Checksum { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has a <see cref="Checksum"/> representation.
        /// </summary>
        /// <value><c>true</c> if this instance has a <see cref="Checksum"/> representation; otherwise, <c>false</c>.</value>
        bool HasChecksum { get; }
    }
}