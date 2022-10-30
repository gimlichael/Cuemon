using System;
using Cuemon.Globalization;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Cuemon.Extensions.IO;

namespace Cuemon.Extensions.Globalization
{
    internal class Program
    {
        private static readonly string SurrogatesPath;

        static Program()
        {
            var assemblyPath = typeof(Program).Assembly.Location;
            SurrogatesPath = Path.GetFullPath(Path.Combine(assemblyPath, "..", "..", "..", "..", "Surrogates"));
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

                    var bf = new BinaryFormatter();
                    using var ms = new MemoryStream();
#pragma warning disable SYSLIB0011
                    bf.Serialize(ms, ciSurrogate);
#pragma warning restore SYSLIB0011
                    ms.Position = 0;
                    using var cms = ms.CompressGZip();
                    using var fs = new FileStream(Path.Combine(SurrogatesPath, $"{cultureInfo.Name.ToLowerInvariant()}.bin"), FileMode.Create);
                    fs.Write(cms.ToByteArray(o => o.LeaveOpen = true), 0, (int)cms.Length);
                    fs.Flush();
                }
            }
        }
    }
}
