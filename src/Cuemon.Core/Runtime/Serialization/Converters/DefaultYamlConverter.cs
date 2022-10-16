using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cuemon.Collections.Generic;
using Cuemon.Text.Yaml;
using Cuemon.Text.Yaml.Converters;

namespace Cuemon.Runtime.Serialization.Converters
{
    internal class DefaultYamlConverter : YamlConverter
    {
        private readonly List<YamlConverter> _converters = new()
        {
            {
                YamlConverterFactory.Create(type => type.IsPrimitive, (writer, value, so) =>
                {
                    writer.Write(value);
                    writer.WriteLine();
                })
            },
            {
                YamlConverterFactory.Create(type => type == typeof(string), (writer, value, so) =>
                {
                    writer.Write(value);
                    writer.WriteLine();
                })
            },
            {
                YamlConverterFactory.Create<IEnumerable>((writer, sequence, so) =>
                {
                    var sequenceType = sequence.GetType();
                    var hasKeyValuePairType = sequenceType.GetGenericArguments().Any(gt => Decorator.Enclose(gt).HasKeyValuePairImplementation());
                    
                    var hasArrayWritten = false;
                    if (Decorator.Enclose(sequenceType).HasDictionaryImplementation() || hasKeyValuePairType)
                    {
                        foreach (var element in sequence)
                        {
                            if (!hasArrayWritten)
                            {
                                writer.WriteStartArray();
                                hasArrayWritten = true;
                            }
                            var elementType = element.GetType();
                            var keyProperty = elementType.GetProperty("Key");
                            var valueProperty = elementType.GetProperty("Value");
                            var keyValue = keyProperty.GetValue(element, null);
                            var valueValue = valueProperty.GetValue(element, null);
                            writer.WritePropertyName(so.SetPropertyName(keyValue.ToString()));
                            writer.WriteObject(valueValue, so);
                        }

                        if (hasArrayWritten)
                        {
                            writer.WriteEndArray();
                        }
                        else
                        {
                            writer.WriteLine("{}");
                        }
                    }
                    else
                    {
                        foreach (var item in sequence)
                        {
                            if (!hasArrayWritten)
                            {
                                writer.WriteStartArray();
                                hasArrayWritten = true;
                            }
                            if (item == null) { continue; }
                            writer.Write("- ");
                            writer.WriteObject(item, so);
                        }
                        if (hasArrayWritten)
                        {
                            writer.WriteEndArray();
                        }
                        else
                        {
                            writer.WriteLine("[]");
                        }
                    }
                })
            },
            {
                YamlConverterFactory.Create(type => Decorator.Enclose(type).IsComplex(), (writer, value, so) =>
                {
                    writer.WriteStartObject();
                    var properties = value.GetType().GetRuntimeProperties();
                    foreach (var property in properties)
                    {
                        var propertyValue = property.GetValue(value);
                        if (propertyValue == null) { continue; }
                        writer.WritePropertyName(so.SetPropertyName(property.Name));
                        writer.WriteObject(propertyValue, so);
                    }
                    writer.WriteEndObject();
                })
            }
        };

        internal DefaultYamlConverter(IEnumerable<YamlConverter> otherConverters)
        {
            _converters.AddRange(otherConverters);
        }

        internal override void WriteYamlCore(YamlTextWriter writer, object value, YamlSerializerOptions so)
        {
            if (value == null) { return; }
            var typeToConvert = value.GetType();
            var converter = _converters.FirstOrDefault(c => c.CanConvert(typeToConvert));
            if (converter != null)
            {
                converter.WriteYamlCore(writer, value, so);
                return;
            }
            throw ExceptionInsights.Embed(new InvalidOperationException("Unable to serialize the specified object to a YAML document."), MethodBase.GetCurrentMethod(), Arguments.ToArray(writer, value, so));
        }

        internal override object ReadYamlCore(YamlTextReader reader, Type typeToConvert, YamlSerializerOptions so)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }
    }
}
