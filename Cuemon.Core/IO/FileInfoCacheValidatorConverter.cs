using System;
using System.IO;
using System.Linq;
using Cuemon.Collections.Generic;
using Cuemon.ComponentModel;
using Cuemon.Integrity;

namespace Cuemon.IO
{
    /// <summary>
    /// Provides a converter that converts a <see cref="FileInfo"/> to a <see cref="CacheValidator"/>.
    /// Implements the <see cref="IConverter{TInput,TOutput,TOptions}" />
    /// </summary>
    /// <seealso cref="IConverter{TInput,TOutput,TOptions}" />
    public class FileInfoCacheValidatorConverter : IConverter<FileInfo, CacheValidator, FileChecksumOptions>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to a <see cref="CacheValidator"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="FileInfo"/> to convert.</param>
        /// <param name="setup">The <see cref="FileChecksumOptions"/> which may be configured.</param>
        /// <returns>A <see cref="CacheValidator"/> that represents the <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <seealso cref="FileInfoIntegrityConverter"/>
        public CacheValidator ChangeType(FileInfo input, Action<FileChecksumOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            var options = Patterns.Configure(setup);
            return ConvertFactory.UseConverter<FileInfoIntegrityConverter>().ChangeType(input, fio =>
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
                    var fileNameHashCode64 = Generate.HashCode64(input.FullName);
                    return new CacheValidator(fi.CreationTimeUtc, fi.LastWriteTimeUtc, fileNameHashCode64, o =>
                    {
                        o.Method = options.Method;
                        o.Algorithm = options.Algorithm;
                    });
                };
            }) as CacheValidator;
        }
    }
}