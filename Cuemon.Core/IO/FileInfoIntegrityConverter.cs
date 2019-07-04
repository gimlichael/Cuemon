using System;
using System.IO;
using Cuemon.ComponentModel;
using Cuemon.Integrity;

namespace Cuemon.IO
{
    /// <summary>
    /// Provides a converter that converts a <see cref="FileInfo"/> to an <see cref="IIntegrity"/>.
    /// Implements the <see cref="IConverter{TInput,TOutput,TOptions}" />
    /// </summary>
    /// <seealso cref="IConverter{TInput,TOutput,TOptions}" />
    public class FileInfoIntegrityConverter : IConverter<FileInfo, IIntegrity, FileIntegrityOptions>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to an object implementing the <see cref="IIntegrity"/> interface.
        /// </summary>
        /// <param name="input">The <see cref="FileInfo"/> to convert.</param>
        /// <param name="setup">The <see cref="FileIntegrityOptions"/> which need to be configured.</param>
        /// <returns>An object implementing the <see cref="IIntegrity"/> interface that represents the integrity of <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null -or-
        /// <paramref name="setup"/> property, <see cref="FileIntegrityOptions.IntegrityConverter"/>, cannot be null.
        /// </exception>
        public IIntegrity ChangeType(FileInfo input, Action<FileIntegrityOptions> setup)
        {
            Validator.ThrowIfNull(input, nameof(input));
            var options = Patterns.Configure(setup, o =>
            {
                Validator.ThrowIfNull(o.IntegrityConverter, nameof(o.IntegrityConverter));
            });

            if (options.BytesToRead > 0)
            {
                long buffer = options.BytesToRead;
                if (input.Length < buffer) { buffer = input.Length; }

                var checksumBytes = new byte[buffer];
                using (var openFile = input.OpenRead())
                {
                    openFile.Read(checksumBytes, 0, (int)buffer);
                }
                return options.IntegrityConverter(input, checksumBytes);
            }
            return options.IntegrityConverter(input, new byte[0]);
        }
    }
}