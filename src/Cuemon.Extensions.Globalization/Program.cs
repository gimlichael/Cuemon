using Cuemon.Globalization;
using System.IO;
using Cuemon.Extensions.IO;
using Cuemon.Text.Yaml.Formatters;

namespace Cuemon.Extensions.Globalization
{
    internal class Program
    {
        private static readonly string SurrogatesPath;

        static Program()
        {
            var assemblyPath = typeof(Program).Assembly.Location;
            SurrogatesPath = Path.GetFullPath(Path.Combine(assemblyPath, "..", "..", "..", "..", "Surrogates"));
            YamlFormatterOptions.DefaultConverters += list =>
            {
                list.Add(new CultureInfoSurrogateConverter());
            };
            Directory.CreateDirectory(SurrogatesPath);
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

                    var ms = YamlFormatter.SerializeObject(ciSurrogate);

                    using var cms = ms.CompressGZip();
                    using var fs = new FileStream(Path.Combine(SurrogatesPath, $"{cultureInfo.Name.ToLowerInvariant()}.bin"), FileMode.Create);
                    fs.Write(cms.ToByteArray(o => o.LeaveOpen = true), 0, (int)cms.Length);
                    fs.Flush();
                }
            }
        }
    }
}
