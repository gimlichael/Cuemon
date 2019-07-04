using System;
using System.IO;
using Cuemon.Integrity;

namespace Cuemon.IO
{
    /// <summary>
    /// Configuration options for <see cref="FileInfoIntegrityConverter"/>.
    /// </summary>
    public class FileIntegrityOptions : FileInfoOptions
    {
        private Func<FileInfo, byte[], IIntegrity> _integrityConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileIntegrityOptions"/> class.
        /// </summary>
        public FileIntegrityOptions()
        {
        }

        /// <summary>
        /// Gets or sets the function delegate that will convert an instance of <see cref="FileInfo"/> and a <see cref="T:byte[]"/> into an object implementing the <see cref="IIntegrity"/> interface.
        /// </summary>
        /// <value>The function delegate that returns an object implementing the <see cref="IIntegrity"/> interface.</value>
        public Func<FileInfo, byte[], IIntegrity> IntegrityConverter
        {
            get => _integrityConverter;
            set
            {
                Validator.ThrowIfNull(value, nameof(value));
                _integrityConverter = value;
            }
        }
    }
}