using System.IO;

namespace Cuemon.Runtime.Serialization
{
    /// <summary>
    /// Represents a reader that provides fast, non-cached, forward-only access to YAML data.
    /// Implements the <see cref="TextReader" />
    /// </summary>
    /// <seealso cref="TextReader" />
    public class YamlTextReader : TextReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YamlTextReader"/> class.
        /// </summary>
        public YamlTextReader()
        {
        }
    }
}
