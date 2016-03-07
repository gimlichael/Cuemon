using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.XPath;
using Cuemon.Runtime.Serialization;
using Cuemon.Xml.XPath;

namespace Cuemon.IO
{
    /// <summary>
    /// This utility class is designed to make JSON <see cref="Stream"/> related conversions easier to work with.
    /// </summary>
    public static class JsonStreamConverter
    {
        /// <summary>
        /// Returns a UTF-8 encoded JSON representation of the specified XML stream and whose value is equivalent to the specified XML stream.
        /// </summary>
        /// <param name="value">The XML to convert to a JSON representation.</param>
        /// <returns>A UTF-8 encoded JSON representation of the specified <paramref name="value"/> and whose value is equivalent to <paramref name="value"/>.</returns>
        /// <remarks>The JSON representation is in compliance with RFC 4627. Take note, that all string values is escaped using <see cref="StringUtility.Escape"/>. This is by design and to help ensure compatibility with a wide range of data.</remarks>
        public static Stream FromXmlStream(Stream value)
        {
            return FromXmlStream(value, Encoding.UTF8);
        }

        /// <summary>
        /// Returns a JSON representation of the specified XML stream and whose value is equivalent to the specified XML stream.
        /// </summary>
        /// <param name="value">The XML to convert to a JSON representation.</param>
        /// <param name="encoding">The text encoding to use.</param>
        /// <returns>A JSON representation of the specified <paramref name="value"/> and whose value is equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">This exception is thrown when <paramref name="encoding"/> is not within the boundaries of RFC 4627.</exception>
        /// <exception cref="ArgumentNullException">This exception is thrown should either of <paramref name="value"/> or <paramref name="encoding"/> have the value of null.</exception>
        /// <remarks>The JSON representation is in compliance with RFC 4627. Take note, that all string values is escaped using <see cref="StringUtility.Escape"/>. This is by design and to help ensure compatibility with a wide range of data.</remarks>
        public static Stream FromXmlStream(Stream value, Encoding encoding)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (encoding == null) { throw new ArgumentNullException(nameof(encoding)); }
            JsonWriter.ValidateEncoding(encoding);

            long startingPosition = value.Position;
            if (value.CanSeek) { value.Position = 0; }

            MemoryStream output = null;
            MemoryStream tempOutput = null;
            try
            {
                tempOutput = new MemoryStream();
                using (JsonWriter writer = JsonWriter.Create(tempOutput, encoding))
                {
                    IXPathNavigable navigable = XPathNavigableConverter.FromStream(value);
                    XPathNavigator rootNavigator = navigable.CreateNavigator();
                    XPathNodeIterator rootIterator = rootNavigator.Select("/node()");

                    XmlJsonInstance instance = BuildJsonInstance(rootIterator);
                    writer.WriteStartObject();
                    WriteJsonInstance(writer, instance);
                    writer.WriteEndObject();

                    writer.Flush();
                    tempOutput.Position = 0;
                    output = new MemoryStream(tempOutput.ToArray());
                    tempOutput = null;
                }
            }
            catch (Exception)
            {
                if (output != null) { output.Dispose(); }
                throw;
            }
            finally
            {
                if (tempOutput != null) { tempOutput.Dispose(); }
            }

            if (value.CanSeek) { value.Seek(startingPosition, SeekOrigin.Begin); } // reset to original position
            return output;
        }

        private static void WriteJsonInstance(JsonWriter writer, XmlJsonInstance instance)
        {
            switch (instance.NodeType)
            {
                case XPathNodeType.Attribute:
                    writer.WriteObject(instance.Name, instance.Value);
                    break;
                case XPathNodeType.Element:
                    if (instance.IsPartOfArray() && instance.WriteStartArray())
                    {
                        writer.WriteObjectName(instance.Name);
                    }
                    else if (!instance.IsPartOfArray())
                    {
                        writer.WriteObjectName(instance.Name);
                    }
                    if (instance.WriteStartArray())
                    {
                        writer.WriteStartArray();
                        writer.WriteStartObject();
                    }
                    else
                    {
                        writer.WriteStartObject();
                    }
                    break;
                case XPathNodeType.Text:
                    writer.WriteObject(instance.Name, instance.Value);
                    break;
            }

            if (instance.WriteValueSeperator())
            {
                if ((!instance.WriteStartArray() &&
                    (!instance.IsPartOfArray() && instance.NodeType == XPathNodeType.Attribute)) ||
                    instance.NodeType == XPathNodeType.Text)
                {
                    writer.WriteValueSeperator();
                }
            }

            if (instance.Instances.Count > 0)
            {
                instance.Instances.Sort(JsonInstanceCollection.Compare);
                foreach (XmlJsonInstance childInstance in instance.Instances)
                {
                    WriteJsonInstance(writer, childInstance);
                }
            }

            switch (instance.NodeType)
            {
                case XPathNodeType.Attribute:
                    break;
                case XPathNodeType.Element:
                    if (instance.WriteEndArray())
                    {
                        writer.WriteEndObject();
                        writer.WriteEndArray();
                    }
                    else
                    {
                        writer.WriteEndObject();
                    }

                    if (instance.WriteValueSeperator()) { writer.WriteValueSeperator(); }
                    break;
            }
        }

        private static XmlJsonInstance BuildJsonInstance(XPathNodeIterator iterator)
        {
            int nodeNumber = 0;
            return BuildJsonInstance(iterator, null, ref nodeNumber);
        }

        private static XmlJsonInstance BuildJsonInstance(XPathNodeIterator iterator, JsonInstance parent, ref int nodeNumber)
        {
            while (iterator.MoveNext())
            {
                XmlJsonInstance instance = null;
                XPathNavigator navigator = iterator.Current;
                if (navigator == null) { continue; }
                switch (navigator.NodeType)
                {
                    case XPathNodeType.Root:
                    case XPathNodeType.Namespace:
                    case XPathNodeType.SignificantWhitespace:
                    case XPathNodeType.ProcessingInstruction:
                    case XPathNodeType.Whitespace:
                    case XPathNodeType.Text:
                        continue;
                    case XPathNodeType.Attribute:
                        XPathNodeIterator attributes = navigator.Select("@*");
                        while (attributes.MoveNext())
                        {
                            if (attributes.Current == null) { continue; }
                            XPathNavigator attribute = attributes.Current;
                            string value = attribute.Value.Trim();
                            JsonInstance child = new XmlJsonInstance(attribute.Name, ObjectConverter.FromString(value, CultureInfo.InvariantCulture), nodeNumber, XPathNodeType.Attribute);
                            child.Parent = instance;
                            instance.Instances.Add(child);
                        }
                        //if (parent != null) { instance.Parent = parent; }
                        //instances.Add(instance);
                        break;
                    case XPathNodeType.Element:
                        string elementName = navigator.Name;
                        bool directElement = (!navigator.HasAttributes && navigator.Select("child::node()[text()]").Count == 0);
                        instance = new XmlJsonInstance(elementName, null, nodeNumber, XPathNodeType.Element);

                        if ((navigator.SelectSingleNode("text()") == null)) // (navigator.IsEmptyElement || !navigator.HasChildren) && 
                        {
                        }
                        else
                        {
                            XPathNodeIterator textIterator = navigator.Select("text()");
                            string textValue = null;
                            int textValueSpaceCount = -1;
                            if (textIterator.Count > 0)
                            {
                                StringBuilder text = new StringBuilder();
                                while (textIterator.MoveNext())
                                {
                                    if (textIterator.Current == null) { continue; }
                                    text.Append(textIterator.Current.Value.Trim());
                                }
                                textValue = text.ToString();
                                textValueSpaceCount = StringUtility.Count(textValue, ' ');
                            }

                            if (directElement && textValueSpaceCount == 0)
                            {
                                instance = new XmlJsonInstance(elementName, ObjectConverter.FromString(textValue, CultureInfo.InvariantCulture), nodeNumber);
                            }
                            else
                            {
                                JsonInstance child = new XmlJsonInstance("#text", ObjectConverter.FromString(textValue, CultureInfo.InvariantCulture), nodeNumber);
                                child.Parent = instance;
                                instance.Instances.Add(child);
                            }
                        }

                        if (parent != null)
                        {
                            instance.Parent = parent;
                            parent.Instances.Add(instance);
                        }

                        nodeNumber++;

                        if (navigator.HasAttributes) { goto case XPathNodeType.Attribute; }
                        break;
                }

                XPathNodeIterator children = navigator.Select("node()[not(self::text())]");
                if (children.Count > 0)
                {
                    BuildJsonInstance(children, instance, ref nodeNumber);
                }

                if (parent == null) { return instance; }
            }
            return null;
        }
    }
}