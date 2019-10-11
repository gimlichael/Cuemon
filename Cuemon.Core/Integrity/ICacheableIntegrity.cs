namespace Cuemon.Integrity
{
    /// <summary>
    /// An interface that represents the data integrity that is normally associated with a data-set.
    /// </summary>
    public interface ICacheableIntegrity : IIntegrity
    {
        /// <summary>
        /// Gets the validation strength of the integrity of this instance.
        /// </summary>
        /// <value>The validation strength of the integrity of this instance.</value>
        ChecksumStrength Validation { get; }
    }
}