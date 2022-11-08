namespace Cuemon.Runtime.Serialization
{
    /// <summary>
    /// Defines the various YAML tokens that make up a YAML document.
    /// </summary>
    public enum YamlTokenType
    {
        /// <summary>
        /// The token type is the start of a YAML array.
        /// </summary>
        StartArray,
        /// <summary>
        /// The token type is the end of a YAML array.
        /// </summary>
        EndArray,
        /// <summary>
        /// The token type is the start of a YAML object.
        /// </summary>
        StartObject,
        /// <summary>
        /// The token type is the end of a YAML object.
        /// </summary>
        EndObject,
        /// <summary>
        /// The token type is a YAML property name.
        /// </summary>
        PropertyName
    }
}
