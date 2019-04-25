using System.Threading;

namespace Cuemon.Security
{
    /// <summary>
    /// A simple container for the original value of an obfuscated entry as well as the obfuscated value.
    /// This is used for reversing an obfuscated document to its original value.
    /// </summary>
    public sealed class ObfuscatorMapping
    {
        private int _count;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObfuscatorMapping"/> class.
        /// </summary>
        /// <param name="obfuscated">The obfuscated value of <paramref name="original"/>.</param>
        /// <param name="original">The original value of <paramref name="obfuscated"/>.</param>
        public ObfuscatorMapping(string obfuscated, string original)
        {
            Obfuscated = obfuscated;
            Original = original;
            IncrementCount();
        }

        /// <summary>
        /// Gets the obfuscated value of <see cref="Original"/>.
        /// </summary>
        /// <value>The obfuscated value of <see cref="Original"/>.</value>
        public string Obfuscated { get; }

        /// <summary>
        /// Gets the original value of <see cref="Obfuscated"/>.
        /// </summary>
        /// <value>The original value of <see cref="Obfuscated"/>.</value>
        public string Original { get; }

        /// <summary>
        /// Gets the total representation count of this mapping.
        /// </summary>
        /// <value>The total representation count of this mapping.</value>
        public int Count => _count;

        /// <summary>
        /// Increments the representation count of this mapping.
        /// </summary>
        public void IncrementCount()
        {
            Interlocked.Increment(ref _count);
        }
    }
}