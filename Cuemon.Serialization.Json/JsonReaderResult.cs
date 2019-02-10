using System;
using System.Collections.Generic;

namespace Cuemon.Serialization.Json
{
    /// <summary>
    /// Specifies a result set of a JSON reader operation.
    /// </summary>
    public class JsonReaderResult
    {
        /// <summary>
        /// Gets the path of the JSON token.
        /// </summary>
        /// <value>The path of the JSON token.</value>
        public string Path { get; internal set; }

        /// <summary>
        /// Gets the children of the JSON token.
        /// </summary>
        /// <value>The children of the JSON token.</value>
        public IList<JsonReaderResult> Children { get; internal set; } = new List<JsonReaderResult>();

        /// <summary>
        /// Gets the name of the JSON token property.
        /// </summary>
        /// <value>The name of the JSON token property.</value>
        public string PropertyName { get; internal set; }

        /// <summary>
        /// Gets the value of the JSON token.
        /// </summary>
        /// <value>The value of the JSON token.</value>
        public object Value { get; internal set; }

        /// <summary>
        /// Gets the CLR type of the JSON token.
        /// </summary>
        /// <value>The CLR type of the JSON token.</value>
        public Type Type { get; internal set; }

        /// <summary>
        /// Gets the parent of the JSON token.
        /// </summary>
        /// <value>The parent of the JSON token.</value>
        public JsonReaderResult Parent { get; internal set; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return "{0} ({1}), Children: {2}".FormatWith(Path, Type.Name.ToLowerInvariant(), Children.Count);
        }
    }
}