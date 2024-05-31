using System;
using System.IO;
using System.Text;

namespace Cuemon.Runtime.Serialization
{
    /// <summary>
    /// Represents a reader that provides fast, non-cached, forward-only access to YAML data.
    /// Implements the <see cref="TextReader" />
    /// </summary>
    /// <seealso cref="TextReader" />
    [Obsolete("All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version.")]
    public class YamlTextReader : StreamReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YamlTextReader" /> class.
        /// </summary>
        /// <param name="yaml">The YAML <see cref="Stream"/> to use for input.</param>
        /// <param name="encoding">The character encoding to use.</param>
        public YamlTextReader(Stream yaml, Encoding encoding) : base(yaml, encoding)
        {
        }
    }
}
