using System;

namespace Cuemon.IO
{
    /// <summary>
    /// Configuration options for <see cref="FileInfoIntegrityConverter"/>.
    /// </summary>
    public class FileInfoOptions
    {
        private int _bytesToRead;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileInfoOptions"/> class.
        /// </summary>
        public FileInfoOptions()
        {
            BytesToRead = 0;
        }

        /// <summary>
        /// Gets or sets the amount of bytes to read from a file.
        /// </summary>
        /// <value>The amount of bytes to read from a file.</value>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value" /> is lower than 0.
        /// </exception>
        public int BytesToRead
        {
            get => _bytesToRead;
            set
            {
                Validator.ThrowIfLowerThan(value, 0, nameof(value));
                _bytesToRead = value;
            }
        }
    }
}