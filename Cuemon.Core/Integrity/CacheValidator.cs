using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cuemon.Collections.Generic;

namespace Cuemon.Integrity
{
    /// <summary>
    /// Provides a way to represent cacheable data-centric content that can be validated by cache-aware applications.
    /// </summary>
    public class CacheValidator : ChecksumBuilder, ICacheableTimestamp
    {
        private const long NullOrZeroLengthChecksum = 23719;
        private static readonly CacheValidator DefaultCacheValidatorValue = new CacheValidator(DateTime.MinValue, DateTime.MinValue);
        private static CacheValidator _referencePointCacheValidator;
        private static Assembly _assemblyValue;
        private static readonly Lazy<Assembly> LazyAssembly = new Lazy<Assembly>(() => Assembly.GetEntryAssembly() ?? typeof(CacheValidator).GetTypeInfo().Assembly);

        /// <summary>
        /// Gets or sets the <see cref="System.Reflection.Assembly"/> that will serve as the ideal candidate for a <see cref="CacheValidator"/> reference point. Default is <see cref="Cuemon"/>.
        /// </summary>
        /// <value>The assembly to use as a <see cref="CacheValidator"/> reference point.</value>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        public static Assembly Assembly
        {
            get => _assemblyValue ?? (_assemblyValue = LazyAssembly.Value);
            set
            {
                Validator.ThrowIfNull(value, nameof(value));
                _assemblyValue = value;
                _referencePointCacheValidator = null;
            }
        }

        CacheValidator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, Action<CacheValidatorOptions> setup = null)
            : this(created, created, setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, (byte[])null, setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="Double"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, double checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, ByteConverter.FromConvertibles(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="Int16"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, short checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, ByteConverter.FromConvertibles(checksum), setup)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="String"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, string checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, checksum == null ? StructUtility.HashCodeForNullValue : StructUtility.GetHashCode64(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="Int32"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, int checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, ByteConverter.FromConvertibles(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="Int64"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, long checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, ByteConverter.FromConvertibles(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="Single"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, float checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, ByteConverter.FromConvertibles(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="UInt16"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, ushort checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, ByteConverter.FromConvertibles(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="UInt32"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, uint checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, ByteConverter.FromConvertibles(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="UInt64"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, ulong checksum, Action<CacheValidatorOptions> setup = null) 
            : this(created, modified, ByteConverter.FromConvertibles(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">An array of bytes containing a checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which may be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, byte[] checksum, Action<CacheValidatorOptions> setup = null)
        {
            var options = Patterns.Configure(setup);
            var isChecksumNullOrZeroLength = (checksum == null || checksum.Length == 0);

            Created = created.ToUniversalTime();
            Modified = modified?.ToUniversalTime();

            var strength = isChecksumNullOrZeroLength ? ChecksumStrength.None : ChecksumStrength.Strong;
            switch (options.Method)
            {
                case ChecksumMethod.Default:
                    break;
                case ChecksumMethod.Timestamp:
                    checksum = ByteConverter.FromConvertibles(Created.Ticks ^ Modified?.Ticks ?? DateTime.MinValue.Ticks);
                    strength = ChecksumStrength.Weak;
                    break;
                case ChecksumMethod.Combined:
                    var checksumValue = isChecksumNullOrZeroLength ? NullOrZeroLengthChecksum : checksum.GetHashCode64();
                    checksum = ByteConverter.FromConvertibles(Created.Ticks ^ Modified?.Ticks ?? DateTime.MinValue.Ticks ^ checksumValue);
                    strength = isChecksumNullOrZeroLength ? ChecksumStrength.Weak : ChecksumStrength.Strong;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(options.Method));
            }
            Bytes = checksum == null ? new List<byte>() : new List<byte>(checksum);
            Strength = strength;
            Method = options.Method;
            Options = options;
            AlgorithmType = options.AlgorithmType;
        }
        
        private CacheValidatorOptions Options { get; set; }

        /// <summary>
        /// Gets a <see cref="DateTime"/> value from when data this instance represents was first created, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <value>A <see cref="DateTime"/> value from when data this instance represents was first created, expressed as the Coordinated Universal Time (UTC).</value>
        public DateTime Created { get; private set; }

        /// <summary>
        /// Gets a <see cref="DateTime"/> value from when data this instance represents was last modified, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <value>A <see cref="DateTime"/> value from when data this instance represents was last modified, expressed as the Coordinated Universal Time (UTC).</value>
        public DateTime? Modified { get; private set; }

        /// <summary>
        /// Gets a <see cref="CacheValidator"/> object that is initialized to a default representation that should be considered invalid for usage beyond this check.
        /// </summary>
        /// <value>A <see cref="CacheValidator"/> object that is initialized to a default representation.</value>
        public static CacheValidator Default => DefaultCacheValidatorValue.Clone();

        /// <summary>
        /// Gets a <see cref="CacheValidator"/> object that represents an <see cref="System.Reflection.Assembly"/> reference point.
        /// </summary>
        /// <value>A <see cref="CacheValidator"/> object that represents an <see cref="System.Reflection.Assembly"/> reference point.</value>
        public static CacheValidator ReferencePoint
        {
            get
            {
                if (_referencePointCacheValidator == null)
                {
                    _referencePointCacheValidator = CacheValidatorConverter.FromAssembly(Assembly);
                }
                return _referencePointCacheValidator.Clone();
            }
        }

        /// <summary>
        /// Gets an enumeration value of <see cref="ChecksumMethod"/> indicating the usage method of this instance.
        /// </summary>
        /// <value>One of the enumeration values of <see cref="ChecksumMethod"/> that indicates the usage method of this instance.</value>
        public ChecksumMethod Method { get; private set; }

        /// <summary>
        /// Gets an enumeration value of <see cref="ChecksumStrength"/> indicating the strength of this instance.
        /// </summary>
        /// <value>One of the enumeration values of <see cref="ChecksumStrength"/> that specifies the strength of this instance.</value>
        public ChecksumStrength Strength { get; private set; }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="CacheValidator"/> object.
        /// </summary>
        /// <returns>A new <see cref="CacheValidator"/> that is a copy of this instance.</returns>
        public virtual CacheValidator Clone()
        {
            return new CacheValidator()
            {
                Options = Options,
                Method = Method,
                Modified = Modified,
                Created = Created,
                Strength = Strength,
                Bytes = Bytes.ToList(),
                ComputedChecksum = ComputedChecksum
            };
        }

        /// <summary>
        /// Gets the most significant (largest) value of either <see cref="Created"/> or <see cref="Modified"/>.
        /// </summary>
        /// <returns>The most significant (largest) value of either <see cref="Created"/> or <see cref="Modified"/>.</returns>
        public DateTime GetMostSignificant()
        {
            return EnumerableConverter.FromArray(Created, Modified ?? DateTime.MinValue).Max();
        }

        /// <summary>
        /// Gets the most significant <see cref="CacheValidator"/> object from the most significant (largest) value of either <see cref="Created"/> or <see cref="Modified"/> in the specified <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">A sequence of  <see cref="CacheValidator"/> objects to parse for the most significant (largest) value of either <see cref="Created"/> or <see cref="Modified"/>.</param>
        /// <returns>The most significant <see cref="CacheValidator"/> object from the specified <paramref name="sequence"/>.</returns>
        public static CacheValidator GetMostSignificant(params CacheValidator[] sequence)
        {
            Validator.ThrowIfNull(sequence, nameof(sequence));
            var mostSignificant = Default;
            foreach (var candidate in sequence)
            {
                if (candidate.GetMostSignificant().Ticks > mostSignificant.GetMostSignificant().Ticks) { mostSignificant = candidate; }
            }
            return mostSignificant;
        }
    }
}