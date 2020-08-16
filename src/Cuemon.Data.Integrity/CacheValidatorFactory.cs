using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Cuemon.Data.Integrity
{
    /// <summary>
    /// Provides access to factory methods for creating and configuring <see cref="CacheValidator"/> instances.
    /// </summary>
    public static class CacheValidatorFactory
    {
        /// <summary>
        /// Creates and returns an instance of <see cref="CacheValidator"/> from the specified <paramref name="file"/>.
        /// </summary>
        /// <param name="file">The <see cref="FileInfo"/> to convert.</param>
        /// <param name="setup">The <see cref="FileChecksumOptions"/> which may be configured.</param>
        /// <returns>A <see cref="CacheValidator"/> that represents the <paramref name="file"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="file"/> cannot be null.
        /// </exception>
        public static CacheValidator CreateValidator(FileInfo file, Action<FileChecksumOptions> setup = null)
        {
            Validator.ThrowIfNull(file, nameof(file));
            var options = Patterns.Configure(setup);
            return DataIntegrityFactory.CreateIntegrity(file, fio =>
            {
                fio.BytesToRead = options.BytesToRead;
                fio.IntegrityConverter = (fi, checksumBytes) =>
                {
                    if (checksumBytes.Length > 0)
                    {
                        return new CacheValidator(fi.CreationTimeUtc, fi.LastWriteTimeUtc, Generate.HashCode64(checksumBytes.Cast<IConvertible>()), o =>
                        {
                            o.Method = options.Method;
                            o.Algorithm = options.Algorithm;
                        });
                    }
                    var fileNameHashCode64 = Generate.HashCode64(file.FullName);
                    return new CacheValidator(fi.CreationTimeUtc, fi.LastWriteTimeUtc, fileNameHashCode64, o =>
                    {
                        o.Method = options.Method;
                        o.Algorithm = options.Algorithm;
                    });
                };
            }) as CacheValidator;
        }

        /// <summary>
        /// Creates and returns an instance of <see cref="CacheValidator"/> from the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to convert.</param>
        /// <param name="setup">The <see cref="FileChecksumOptions"/> which may be configured.</param>
        /// <returns>A <see cref="CacheValidator"/> that represents the <paramref name="assembly"/>.</returns>
        public static CacheValidator CreateValidator(Assembly assembly, Action<FileChecksumOptions> setup = null)
        {
            var assemblyHashCode64 = Generate.HashCode64(assembly.FullName);
            var assemblyLocation = assembly.Location;
            return assembly.IsDynamic
                ? new CacheValidator(DateTime.MinValue, DateTime.MaxValue, assemblyHashCode64, Patterns.ConfigureExchange<FileChecksumOptions, CacheValidatorOptions>(setup))
                : CreateValidator(new FileInfo(assemblyLocation), setup);
        }
    }
}