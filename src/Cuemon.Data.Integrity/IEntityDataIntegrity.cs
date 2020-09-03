namespace Cuemon.Data.Integrity
{
    /// <summary>
    /// An interface that represents the integrity od data that is normally associated with an entity/resource.
    /// </summary>
    public interface IEntityDataIntegrity : IDataIntegrity
    {
        /// <summary>
        /// Gets the validation strength of the integrity of this resource.
        /// </summary>
        /// <value>The validation strength of the integrity of this resource.</value>
        EntityDataIntegrityStrength Validation { get; }
    }
}