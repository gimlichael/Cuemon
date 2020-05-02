namespace Cuemon.Integrity
{
    /// <summary>
    /// An interface that represents both the timestamp and data integrity that is normally associated with a resource.
    /// </summary>
    /// <seealso cref="ICacheableTimestamp" />
    /// <seealso cref="ICacheableIntegrity" />
    public interface ICacheableEntity : ICacheableTimestamp, ICacheableIntegrity
    {
    }
}