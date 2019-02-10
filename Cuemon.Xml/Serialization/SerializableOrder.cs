namespace Cuemon.Xml.Serialization
{
    /// <summary>
    /// Specifies the order of additional serialization information.
    /// </summary>
    public enum SerializableOrder
    {
        /// <summary>
        /// The new serialization operation is applied after the original serialization.
        /// </summary>
        Append = 0,
        /// <summary>
        /// The new serialization operation is applied before the original serialization.
        /// </summary>
        Prepend = 1
    }
}
