using System;
using System.IO;
using Cuemon.Integrity;

namespace Cuemon.IO
{
    /// <summary>
    /// Configuration options for <see cref="FileInfo"/>.
    /// </summary>
    public class FileIntegrityOptions : FileInfoOptions
    {
        private Func<FileInfo, byte[], IIntegrity> _integrityConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileIntegrityOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="FileIntegrityOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="IntegrityConverter"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public FileIntegrityOptions()
        {
        }

        /// <summary>
        /// Gets or sets the function delegate that will convert an instance of <see cref="FileInfo"/> and a <see cref="T:byte[]"/> into an object implementing the <see cref="IIntegrity"/> interface.
        /// </summary>
        /// <value>The function delegate that returns an object implementing the <see cref="IIntegrity"/> interface.</value>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
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