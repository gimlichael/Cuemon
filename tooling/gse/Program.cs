using System.IO;
using Cuemon.Extensions.Globalization;
using Cuemon.Extensions.IO;
using Cuemon.Globalization;
using Cuemon.Reflection;
using Cuemon.Text.Yaml.Formatters;

namespace gse
{
    internal class Program
    {
        private static readonly string SurrogatesPath;
        private static readonly string SurrogatesPathRaw;

        static Program()
        {
            var assemblyPath = typeof(Program).Assembly.Location;
            SurrogatesPath = Path.GetFullPath(Path.Combine(assemblyPath, "..", "..", "..", "..", "Surrogates"));
            SurrogatesPathRaw = Path.Combine(SurrogatesPath, "raw");
            YamlFormatterOptions.DefaultConverters += list =>
            {
                list.Add(new CultureInfoSurrogateConverter());
            };
            Directory.CreateDirectory(SurrogatesPath);
            Directory.CreateDirectory(SurrogatesPathRaw);
        }

        static void Main(string[] args)
        {
            var regions = World.Regions;
            foreach (var region in regions)
            {
                var cultureInfos = World.GetCultures(region);
                foreach (var cultureInfo in cultureInfos)
                {
                    var dtSurrogate = new DateTimeFormatInfoSurrogate(cultureInfo.DateTimeFormat);
                    var nfSurrogate = new NumberFormatInfoSurrogate(cultureInfo.NumberFormat);
                    var ciSurrogate = new CultureInfoSurrogate(dtSurrogate, nfSurrogate);

                    var ms = YamlFormatter.SerializeObject(ciSurrogate, o => o.Settings.ReflectionRules = new MemberReflection());
                    
                    using var fsRawYaml = new FileStream(Path.Combine(SurrogatesPathRaw, $"{cultureInfo.Name.ToLowerInvariant()}.yml"), FileMode.Create);
                    fsRawYaml.Write(ms.ToByteArray(o => o.LeaveOpen = true), 0, (int)ms.Length);
                    fsRawYaml.Flush();

                    using var cms = ms.CompressGZip();
                    using var fs = new FileStream(Path.Combine(SurrogatesPath, $"{cultureInfo.Name.ToLowerInvariant()}.bin"), FileMode.Create);
                    fs.Write(cms.ToByteArray(o => o.LeaveOpen = true), 0, (int)cms.Length);
                    fs.Flush();
                }
            }
        }
    }
}
