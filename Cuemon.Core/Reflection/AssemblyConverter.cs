using System;
using System.IO;
using System.Reflection;
using Cuemon.ComponentModel;
using Cuemon.Integrity;
using Cuemon.IO;

namespace Cuemon.Reflection
{
    public class AssemblyConverter : IConverter<Assembly, CacheValidator, FileChecksumOptions>
    {
        public CacheValidator ChangeType(Assembly input, Action<FileChecksumOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            var assemblyHashCode64 = Generate.HashCode64(input.FullName);
            var assemblyLocation = input.Location;
            return input.IsDynamic
                ? new CacheValidator(DateTime.MinValue, DateTime.MaxValue, assemblyHashCode64, Patterns.ConfigureExchange<FileChecksumOptions, CacheValidatorOptions>(setup))
                : ConvertFactory.UseConverter<FileInfoCacheValidatorConverter>().ChangeType(new FileInfo(assemblyLocation), setup);
        }
    }
}