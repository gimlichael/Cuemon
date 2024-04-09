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
    [Obsolete("All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version.")]
    internal class DefaultYamlConverter : YamlConverter
    {
        private readonly List<YamlConverter> _converters = new()
        {
            {
                YamlConverterFactory.Create(type => type == typeof(string), (writer, value, so) =>
                {
                    writer.WriteStringValue(value as string);
                })
            },
            {
                YamlConverterFactory.Create(type => !Decorator.Enclose(type).IsComplex(), (writer, value, _) =>
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
                            if (!hasArrayWritten && writer.TokenType != YamlTokenType.None) { writer.WriteLine(); }
                            hasArrayWritten = true;
                            writer.WriteStartArray();
                            var elementType = element.GetType();
                            var keyProperty = elementType.GetProperty("Key");
                            var valueProperty = elementType.GetProperty("Value");
                            var keyValue = keyProperty.GetValue(element, null);
                            var valueValue = valueProperty.GetValue(element, null);
                            writer.WritePropertyName(so.SetPropertyName(keyValue.ToString()));
                            writer.WriteObject(valueValue, so);
                            writer.WriteEndArray();
                        }

                        if (!hasArrayWritten)
                        {
                            writer.WriteLine("{}");
                        }
                    }
                    else
                    {
                        foreach (var item in sequence)
                        {
                            if (item == null) { continue; }
                            if (!hasArrayWritten && writer.TokenType != YamlTokenType.None) { writer.WriteLine(); }
                            hasArrayWritten = true;
                            writer.WriteStartArray();
                            writer.WriteObject(item, so);
                            writer.WriteEndArray();
                        }

                        if (!hasArrayWritten)
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

                    var valueHierarchy = new HierarchySerializer(value, o =>
                    {
                        o.MaxCircularCalls = 0;
                        o.MaxDepth = 1;
                        o.ReflectionRules = so.ReflectionRules;
                    });

                    foreach (var node in valueHierarchy.Nodes.GetChildren())
                    {
                        if (node.Instance == null) { continue; }

                        try
                        {
                            writer.WritePropertyName(so.SetPropertyName(node.MemberReference.Name));
                            writer.WriteObject(node.Instance, node.InstanceType, so);
                        }
                        catch (Exception)
                        {
                            // Intentionally swallow for now ..
                        }

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
