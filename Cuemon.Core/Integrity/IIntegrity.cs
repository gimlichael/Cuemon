namespace Cuemon.Integrity
{
    /// <summary>
    /// An interface that is used to determine the integrity of data.
    /// </summary>
    public interface IIntegrity
    {
        /// <summary>
        /// Gets a <see cref="HashResult"/> that represents the integrity of this instance.
        /// </summary>
        /// <value>The checksum that represents the integrity of this instance.</value>
        HashResult Checksum { get; }
    }
}