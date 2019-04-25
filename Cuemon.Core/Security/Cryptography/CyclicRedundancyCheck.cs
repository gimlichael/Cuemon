using System.Collections.Generic;
using System.Security.Cryptography;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Represents the abstract class from which all implementations of the CRC hash algorithm inherit.
    /// </summary>
    public abstract class CyclicRedundancyCheck : HashAlgorithm
    {
        private static readonly object Sync = new object();
        private static readonly IDictionary<string, IList<long>> LookupTables = new Dictionary<string, IList<long>>();

        #region Contructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CyclicRedundancyCheck"/> class.
        /// </summary>
        protected CyclicRedundancyCheck() : this(PolynomialRepresentation.Normal)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CyclicRedundancyCheck"/> class.
        /// </summary>
        /// <param name="representation">The CRC generator polynomial representation.</param>
        protected CyclicRedundancyCheck(PolynomialRepresentation representation)
        {
            Representation = representation;
            Initialize();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes an implementation of the <see cref="T:System.Security.Cryptography.HashAlgorithm" /> class.
        /// </summary>
        public sealed override void Initialize()
        {
            lock (Sync)
            {
                LookupTableKey = GetType().Name;
                HashCoreResult = DefaultSeed;
                InitializePolynomial();
                if (LookupTable.Count == 256) { return; }
                LoopUtility.For<ushort>(256, InitializeLookupTable);
            }
        }

        private void InitializeLookupTable(ushort tableIndex)
        {
            LoopUtility.For<byte, ushort>(8, InitializePolynomialLookupTable, tableIndex);
        }

        /// <summary>
        /// Initializes the implementation of the polynomial representation details.
        /// </summary>
        protected abstract void InitializePolynomial();

        /// <summary>
        /// Initializes the implementation details of a <see cref="CyclicRedundancyCheck"/> related polynomial lookup table.
        /// </summary>
        /// <param name="currentBit">The current bit ranging from 0 to 7.</param>
        /// <param name="currentTableIndex">The current index of the associated polynomial <see cref="LookupTable"/> ranging from 0 to 255.</param>
        /// <remarks>This method is - on first run - invoked 8 times per entry in the associated polynomial <see cref="LookupTable"/>, given a total of 2048 times.</remarks>
        protected abstract void InitializePolynomialLookupTable(byte currentBit, ushort currentTableIndex);

        #endregion

        #region Properties
        private string LookupTableKey { get; set; }

        /// <summary>
        /// Gets or sets the resolved hash result.
        /// </summary>
        /// <value>The resolved hash result.</value>
        protected long HashCoreResult { get; set; }

        /// <summary>
        /// Gets the lookup table for the associated CRC implementation.
        /// </summary>
        /// <value>The lookup table for the associated CRC implementation.</value>
        protected IList<long> LookupTable
        {
            get
            {
                var key = string.Concat(LookupTableKey, Representation.ToString());
                IList<long> result;
                if (!LookupTables.TryGetValue(key, out result))
                {
                    result = new List<long>(256);
                    LookupTables.Add(key, result);
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the CRC polynomial generator representation.
        /// </summary>
        /// <value>The CRC polynomial generator representation.</value>
        public PolynomialRepresentation Representation { get; }

        /// <summary>
        /// Gets the CRC polynomial hexadecimal value equal to CRC implementation and <see cref="Representation"/>.
        /// </summary>
        /// <value>The CRC polynomial hexadecimal value equal to CRC implementation and <see cref="Representation"/>.</value>
        public long Polynomial { get; protected set; }

        /// <summary>
        /// Gets the CRC default seed value.
        /// </summary>
        /// <value>The CRC default seed value.</value>
        public abstract long DefaultSeed { get; }
        #endregion
    }
}