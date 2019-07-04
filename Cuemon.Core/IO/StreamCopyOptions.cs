using System;

namespace Cuemon.IO
{
    public class StreamCopyOptions
    {
        private int _bufferSize;

        public StreamCopyOptions()
        {
            BufferSize = 81920;
        }

        /// <summary>
        /// Gets or sets the size of the buffer.
        /// </summary>
        /// <value>The size of the buffer.</value>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value" /> is lower than or equal to 0.
        /// </exception>
        public int BufferSize
        {
            get => _bufferSize;
            set
            {
                Validator.ThrowIfLowerThanOrEqual(value, 0, nameof(value));
                _bufferSize = value;
            }
        }
    }
}