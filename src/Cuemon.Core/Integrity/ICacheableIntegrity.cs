namespace Cuemon.Integrity
{
    /// <summary>
    /// An interface that represents the data integrity that is normally associated with a resource.
    /// </summary>
    public interface ICacheableIntegrity : IIntegrity
    {
        /// <summary>
        /// Gets the validation strength of the integrity of this resource.
        /// </summary>
        /// <value>The validation strength of the integrity of this resource.</value>
        ChecksumStrength Validation { get; }
    }
}