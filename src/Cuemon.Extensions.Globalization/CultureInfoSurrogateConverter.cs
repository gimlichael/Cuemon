using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Runtime.Serialization;
using Cuemon.Text.Yaml.Converters;

namespace Cuemon.Extensions.Globalization
{
    internal class CultureInfoSurrogateConverter : YamlConverter<CultureInfoSurrogate>
    {
        public override void WriteYaml(YamlTextWriter writer, CultureInfoSurrogate value, YamlSerializerOptions so)
        {
            writer.WriteObject(value, typeof(object), so);
        }

        public override CultureInfoSurrogate ReadYaml(YamlTextReader reader, Type typeToConvert, YamlSerializerOptions so)
        {
            var df = new DateTimeFormatInfoSurrogate();
            var nf = new NumberFormatInfoSurrogate();

            var dfProperties = df.GetType().GetProperties().ToList();
            var nfProperties = nf.GetType().GetProperties().ToList();

            string lastArrayTrimmedLine = null;

            while (!reader.EndOfStream)
            {
                var trimmedLine = lastArrayTrimmedLine ?? reader.ReadLine()?.TrimStart();
                if (trimmedLine != null)
                {
                    lastArrayTrimmedLine = null;
                    var indexOfFirstColon = trimmedLine.IndexOf(':');
                    if (indexOfFirstColon == -1) { continue; }
                    var key = trimmedLine.Substring(0, indexOfFirstColon);
                    var value = trimmedLine.Substring(indexOfFirstColon + 2); // +1 for colon and +1 for space
                    var dfProperty = dfProperties.FirstOrDefault(pi => pi.Name == key);
                    if (dfProperty != null)
                    {
                        var isArray = dfProperty.PropertyType.IsArray;
                        if (isArray)
                        {
                            var values = new List<string>();
                            while ((trimmedLine = reader.ReadLine()?.TrimStart()) != null && trimmedLine.StartsWith("-"))
                            {
                                values.Add(trimmedLine.Substring(2));
                            }
                            lastArrayTrimmedLine = trimmedLine;
                            dfProperty.SetValue(df, Decorator.Enclose(values.ToArray()).ChangeType(dfProperty.PropertyType));
                        }
                        else
                        {
                            dfProperty.SetValue(df, Decorator.Enclose(value).ChangeType(dfProperty.PropertyType));
                        }
                        continue;
                    }
                    var nfProperty = nfProperties.FirstOrDefault(pi => pi.Name == key);
                    nfProperty?.SetValue(nf, Decorator.Enclose(value).ChangeType(nfProperty.PropertyType));
                }
            }

            return new CultureInfoSurrogate(df, nf);
        }
    }
}
