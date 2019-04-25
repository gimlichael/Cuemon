using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Cuemon.Collections.Generic
{
    /// <summary>
    /// Represents an <see cref="IEnumerable{T}"/> partitioned data sequence. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T">The type of the elements of this instance.</typeparam>
    public sealed class PartitionCollection<T> : IEnumerable<T>
    {
        private IEnumerable<T> _source;
        private int _iteratedCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionCollection{T}"/> class.
        /// </summary>
        /// <param name="source">The source of the sequence.</param>
        public PartitionCollection(IEnumerable<T> source) : this(source, 128)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionCollection{T}"/> class.
        /// </summary>
        /// <param name="source">The source of the sequence.</param>
        /// <param name="partitionSize">The number of elements a partition can hold.</param>
        public PartitionCollection(IEnumerable<T> source, int partitionSize)
        {
            Validator.ThrowIfNull(source, nameof(source));
            _source = source;
            Count = source.Count();
            PartitionSize = partitionSize;
        }

        /// <summary>
        /// Gets the number of elements a partition can hold.
        /// </summary>
        /// <value>The number of elements a partition can hold.</value>
        public int PartitionSize { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has partitions remaining to be iterated.
        /// </summary>
        /// <value><c>true</c> if this instance has partitions remaining to be iterated; otherwise, <c>false</c>.</value>
        public bool HasPartitions => (Remaining > 0);

        /// <summary>
        /// Gets the total number of elements in the sequence before partitioning is applied.
        /// </summary>
        /// <value>The total number of elements in the sequence before partitioning is applied.</value>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the number of elements remaining in the partitioned sequence.
        /// </summary>
        /// <value>The number of elements remaining in the partitioned sequence.</value>
        public int Remaining => (Count - _iteratedCount);

        /// <summary>
        /// Gets the total amount of partitions for the elements in this sequence.
        /// </summary>
        /// <returns>The total amount of partitions for the elements in this sequence.</returns>
        public int PartitionCount()
        {
            var presult = Count / Converter.FromObject<double>(PartitionSize);
            return Converter.FromObject<int>(Math.Ceiling(presult));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            int count;
            var result = EnumerableUtility.Chunk(ref _source, PartitionSize, out count);
            Interlocked.Add(ref _iteratedCount, count);
            return result.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}