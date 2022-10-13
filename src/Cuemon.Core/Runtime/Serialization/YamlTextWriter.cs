using System.CodeDom.Compiler;
using System.IO;

namespace Cuemon.Runtime.Serialization
{
    /// <summary>
    /// Represents a writer that provides a fast, non-cached, forward-only way to generate streams or files that contain YAML data.
    /// Implements the <see cref="IndentedTextWriter" />
    /// </summary>
    /// <seealso cref="IndentedTextWriter" />
    public class YamlTextWriter : IndentedTextWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YamlTextWriter"/> class.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.IO.TextWriter" /> to use for output.</param>
        public YamlTextWriter(TextWriter writer) : base(writer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YamlTextWriter"/> class.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.IO.TextWriter" /> to use for output.</param>
        /// <param name="tabString">The tab string to use for indentation.</param>
        public YamlTextWriter(TextWriter writer, string tabString) : base(writer, tabString)
        {
        }

        /// <summary>
        /// Serializes the specified <paramref name="value"/> into a YAML format.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="so">The <see cref="YamlSerializerOptions"/> to use.</param>
        public void WriteObject(object value, YamlSerializerOptions so)
        {
            var serializer = new YamlSerializer(Patterns.ConfigureRevert(so));
            serializer.Serialize(this, value);
        }

        /// <summary>
        /// Writes the beginning of a YAML object.
        /// </summary>
        public void WriteStartObject()
        {
            WriteLine();
            Indent++;
        }

        /// <summary>
        /// Denotes the end of a YAML object.
        /// </summary>
        public void WriteEndObject()
        {
            Indent--;
        }

        /// <summary>
        /// Writes the beginning of a YAML array.
        /// </summary>
        public void WriteStartArray()
        {
            WriteLine();
            Indent++;
        }

        /// <summary>
        /// Denotes the end of a YAML array.
        /// </summary>
        public void WriteEndArray()
        {
            Indent--;
        }

        /// <summary>
        /// Writes a property name specified as a string and a string text value as part of a name/value pair of a YAML object.
        /// </summary>
        /// <param name="propertyName">The name of the YAML object.</param>
        /// <param name="value">The value to be written as part of the name/value pair of a YAML object.</param>
        public void WriteString(string propertyName, string value)
        {
            WriteLine($"{propertyName}: {value}");
        }

        /// <summary>
        /// Writes the property name as the first part of a name/value pair of a YAML object.
        /// </summary>
        /// <param name="propertyName">The name of the YAML object.</param>
        public void WritePropertyName(string propertyName)
        {
            Write($"{propertyName}: ");
        }
    }
}
