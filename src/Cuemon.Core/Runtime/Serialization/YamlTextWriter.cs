using System;
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
        private int _writes;

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
        /// Gets the type of the last processed YAML token.
        /// </summary>
        /// <value>The type of the last processed YAML token.</value>
        public YamlTokenType TokenType { get; private set; }

        /// <summary>
        /// Serializes the specified <paramref name="value"/> into a YAML format.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="so">The <see cref="YamlSerializerOptions"/> to use.</param>
        public void WriteObject(object value, YamlSerializerOptions so)
        {
            if (value == null) { return; }
            WriteObject(value, value.GetType(), so);
        }

        /// <summary>
        /// Serializes the specified <paramref name="value"/> into a YAML format.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="valueType">The type of the <paramref name="value"/> to convert.</param>
        /// <param name="so">The <see cref="YamlSerializerOptions"/> to use.</param>
        public void WriteObject(object value, Type valueType, YamlSerializerOptions so)
        {
            var serializer = new YamlSerializer(Patterns.ConfigureRevert(so));
            serializer.Serialize(this, value, valueType);
            _writes++;
        }

        /// <summary>
        /// Writes the beginning of a YAML object.
        /// </summary>
        public void WriteStartObject()
        {
            TokenType = YamlTokenType.StartObject;
            if (_writes > 0)
            {
                WriteLine();
                Indent++;
            }
            _writes++;
        }

        /// <summary>
        /// Denotes the end of a YAML object.
        /// </summary>
        public void WriteEndObject()
        {
            TokenType = YamlTokenType.EndObject;
            Indent--;
        }

        /// <summary>
        /// Writes the beginning of a YAML array.
        /// </summary>
        public void WriteStartArray()
        {
            TokenType = YamlTokenType.StartArray;
            if (_writes > 0)
            {
                WriteLine();
                Indent++;
            }
            _writes++;
        }

        /// <summary>
        /// Denotes the end of a YAML array.
        /// </summary>
        public void WriteEndArray()
        {
            TokenType = YamlTokenType.EndArray;
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
            _writes++;
        }

        /// <summary>
        /// Writes the property name as the first part of a name/value pair of a YAML object.
        /// </summary>
        /// <param name="propertyName">The name of the YAML object.</param>
        public void WritePropertyName(string propertyName)
        {
            TokenType = YamlTokenType.PropertyName;
            Write($"{propertyName}: ");
            _writes++;
        }
    }
}
