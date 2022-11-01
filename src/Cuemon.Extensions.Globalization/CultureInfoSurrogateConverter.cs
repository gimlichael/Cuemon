using System;
using System.Linq;
using System.Reflection;
using Cuemon.Runtime.Serialization;
using Cuemon.Text.Yaml.Converters;

namespace Cuemon.Extensions.Globalization
{
    internal class CultureInfoSurrogateConverter : YamlConverter<CultureInfoSurrogate>
    {
        public override void WriteYaml(YamlTextWriter writer, CultureInfoSurrogate value, YamlSerializerOptions so)
        {
            writer.WriteObject(value, so);
        }

        public override CultureInfoSurrogate ReadYaml(YamlTextReader reader, Type typeToConvert, YamlSerializerOptions so)
        {
            var df = new DateTimeFormatInfoSurrogate();
            var nf = new NumberFormatInfoSurrogate();

            var dfProperties = df.GetType().GetRuntimeProperties();
            var nfProperties = nf.GetType().GetRuntimeProperties();

            while (!reader.EndOfStream)
            {
                var trimmedLine = reader.ReadLine()?.TrimStart();
                if (trimmedLine != null)
                {
                    var indexOfFirstColon = trimmedLine.IndexOf(':');
                    if (indexOfFirstColon == -1) { continue; }
                    var key = trimmedLine.Substring(0, indexOfFirstColon);
                    var value = trimmedLine.Substring(indexOfFirstColon + 2); // +1 for colon and +1 for space
                    var dfProperty = dfProperties.FirstOrDefault(pi => pi.Name == key);
                    if (dfProperty != null)
                    {
                        dfProperty.SetValue(df, Decorator.Enclose(value).ChangeType(dfProperty.PropertyType));
                        continue;
                    }
                    var nfProperty = nfProperties.FirstOrDefault(pi => pi.Name == key);
                    if (nfProperty != null)
                    {
                        nfProperty.SetValue(nf, Decorator.Enclose(value).ChangeType(nfProperty.PropertyType));
                    }
                }
            }

            return new CultureInfoSurrogate(df, nf);
        }
    }
}
