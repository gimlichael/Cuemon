using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Cuemon.IO;

namespace Cuemon.Integrity
{
    public static class CacheValidatorConverter
    {
        public static CacheValidator FromFile(string fileName, int bytesToRead = 0, Action<CacheValidatorOptions> setup = null)
        {
            Validator.ThrowIfNullOrWhitespace(fileName, nameof(fileName));

            return FileInfoConverter.FromFile(fileName, bytesToRead, (fi, checksumBytes) =>
            {
                if (checksumBytes.Length > 0)
                {
                    return new CacheValidator(fi.CreationTimeUtc, fi.LastWriteTimeUtc, StructUtility.GetHashCode64(checksumBytes), setup);
                }
                var fileNameHashCode64 = StructUtility.GetHashCode64(fileName);
                return new CacheValidator(fi.CreationTimeUtc, fi.LastWriteTimeUtc, fileNameHashCode64, setup);
            });
        }

        public static CacheValidator FromAssembly(Assembly assembly, bool readByteForByteChecksum = false, Action<CacheValidatorOptions> setup = null)
        {
            Validator.ThrowIfNull(assembly, nameof(assembly));
            var assemblyHashCode64 = StructUtility.GetHashCode64(assembly.FullName);
            var assemblyLocation = assembly.Location;
            return assembly.IsDynamic
                ? new CacheValidator(DateTime.MinValue, DateTime.MaxValue, assemblyHashCode64, setup)
                : FromFile(assemblyLocation, readByteForByteChecksum ? int.MaxValue : 0, setup);
        }
    }
}
