using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Cuemon.Collections.Generic;
using Cuemon.Security.Cryptography;

namespace Cuemon.Integrity
{
    /// <summary>
    /// Provides a way to represent cacheable data-centric content that can be validated by cache-aware applications.
    /// </summary>
    public class CacheValidator
    {
        private static readonly CacheValidator DefaultCacheValidatorValue = new CacheValidator(DateTime.MinValue, DateTime.MinValue);
        private static CacheValidator _referencePointCacheValidator;
        private static Assembly _assemblyValue = typeof(CacheValidator).GetTypeInfo().Assembly;
        private const long NullOrZeroLengthChecksum = 23719;

        /// <summary>
        /// Gets or sets the <see cref="System.Reflection.Assembly"/> that will serve as the ideal candidate for a <see cref="CacheValidator"/> reference point. Default is <see cref="Cuemon"/>.
        /// </summary>
        /// <value>The assembly to use as a <see cref="CacheValidator"/> reference point.</value>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        public static Assembly Assembly
        {
            get { return _assemblyValue; }
            set
            {
                Validator.ThrowIfNull(value, nameof(value));
                _assemblyValue = value;
                _referencePointCacheValidator = null;
            }
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
        public CacheValidator(DateTime created, DateTime modified, Action<CacheValidatorOptions> setup = null)
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
        public CacheValidator(DateTime created, DateTime modified, double checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, ByteConverter.FromConvertible(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="Int16"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime modified, short checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, ByteConverter.FromConvertible(checksum), setup)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="String"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime modified, string checksum, Action<CacheValidatorOptions> setup = null)
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
        public CacheValidator(DateTime created, DateTime modified, int checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, ByteConverter.FromConvertible(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="Int64"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime modified, long checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, ByteConverter.FromConvertible(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="Single"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime modified, float checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, ByteConverter.FromConvertible(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="UInt16"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime modified, ushort checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, ByteConverter.FromConvertible(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="UInt32"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime modified, uint checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, ByteConverter.FromConvertible(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="UInt64"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime modified, ulong checksum, Action<CacheValidatorOptions> setup = null) : this(created, modified, ByteConverter.FromConvertible(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">An array of bytes containing a checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime modified, byte[] checksum, Action<CacheValidatorOptions> setup = null)
        {
            var options = setup.ConfigureOptions();
            bool isChecksumNullOrZeroLength = (checksum == null || checksum.Length == 0);

            Created = created.ToUniversalTime();
            Modified = modified.ToUniversalTime();

            ChecksumStrength strength = isChecksumNullOrZeroLength ? ChecksumStrength.None : ChecksumStrength.Strong;
            switch (options.Method)
            {
                case ChecksumMethod.Default:
                    break;
                case ChecksumMethod.Timestamp:
                    checksum = ByteConverter.FromConvertible(Created.Ticks ^ Modified.Ticks);
                    strength = ChecksumStrength.Weak;
                    break;
                case ChecksumMethod.Combined:
                    var checksumValue = isChecksumNullOrZeroLength ? NullOrZeroLengthChecksum : checksum.GetHashCode64();
                    checksum = ByteConverter.FromConvertible(Created.Ticks ^ Modified.Ticks ^ checksumValue);
                    strength = ChecksumStrength.Weak;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(options.Method));
            }
            Bytes = checksum == null ? new List<byte>() : new List<byte>(checksum);
            Strength = strength;
            Method = options.Method;
            Options = options;
        }

        private CacheValidatorOptions Options { get; }

        /// <summary>
        /// Gets a <see cref="DateTime"/> value from when data this instance represents was first created, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <value>A <see cref="DateTime"/> value from when data this instance represents was first created, expressed as the Coordinated Universal Time (UTC).</value>
        public DateTime Created { get; }

        /// <summary>
        /// Gets a <see cref="DateTime"/> value from when data this instance represents was last modified, expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        /// <value>A <see cref="DateTime"/> value from when data this instance represents was last modified, expressed as the Coordinated Universal Time (UTC).</value>
        public DateTime Modified { get; }

        /// <summary>
        /// Gets a <see cref="HashResult"/> containing a computed hash value of the data this instance represents.
        /// </summary>
        /// <value>A <see cref="HashResult"/> containing a computed hash value of the data this instance represents.</value>
        public HashResult Checksum => ComputedChecksum ?? (ComputedChecksum = HashUtility.ComputeHash(Bytes.ToArray(), Options.AlgorithmType));

        private HashResult ComputedChecksum { get; set; }

        /// <summary>
        /// Gets a <see cref="CacheValidator"/> object that is initialized to a default representation that should be considered invalid for usage beyond this check.
        /// </summary>
        /// <value>A <see cref="CacheValidator"/> object that is initialized to a default representation.</value>
        public static CacheValidator Default => DefaultCacheValidatorValue.Clone();

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to resolve a <see cref="CacheValidator"/> from.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified <paramref name="assembly"/>.</returns>
        public static CacheValidator DefaultOr(Assembly assembly)
        {
            return DefaultOr(assembly, DateTime.MinValue);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to resolve a <see cref="CacheValidator"/> from.</param>
        /// <param name="created">The creation time, in coordinated universal time (UTC), of the <paramref name="assembly"/>.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified <paramref name="assembly"/>.</returns>
        public static CacheValidator DefaultOr(Assembly assembly, DateTime created)
        {
            return DefaultOr(assembly, DateTime.MinValue, DateTime.MaxValue);
        }

        /// <summary>
        /// Returns a <see cref="CacheValidator"/> from the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly to resolve a <see cref="CacheValidator"/> from.</param>
        /// <param name="created">The creation time, in coordinated universal time (UTC), of the <paramref name="assembly"/>.</param>
        /// <param name="modified">The time, in coordinated universal time (UTC), of when the <paramref name="assembly"/> was last modified.</param>
        /// <returns>A <see cref="CacheValidator"/> that fully represents the integrity of the specified <paramref name="assembly"/>.</returns>
        public static CacheValidator DefaultOr(Assembly assembly, DateTime created, DateTime modified)
        {
            if ((assembly == null) || (assembly.ManifestModule is ModuleBuilder)) { return Default; }
            return new CacheValidator(created, modified, StructUtility.GetHashCode64(assembly.FullName));
        }

        /// <summary>
        /// The reference point callback that is invoked from <see cref="ReferencePoint"/>.
        /// </summary>
        public static Func<CacheValidator> ReferencePointCallback = () => DefaultOr(Assembly);

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
                    _referencePointCacheValidator = ReferencePointCallback();
                }
                return _referencePointCacheValidator.Clone();
            }
        }

        /// <summary>
        /// Gets a byte array that is the result of the associated <see cref="CacheValidator"/>.
        /// </summary>
        /// <value>The byte array that is the result of the associated <see cref="CacheValidator"/>.</value>
        internal List<byte> Bytes { get; set; }


        /// <summary>
        /// Gets an enumeration value of <see cref="ChecksumMethod"/> indicating the usage method of this instance.
        /// </summary>
        /// <value>One of the enumeration values of <see cref="ChecksumMethod"/> that indicates the usage method of this instance.</value>
        public ChecksumMethod Method { get; }

        /// <summary>
        /// Gets an enumeration value of <see cref="ChecksumStrength"/> indicating the strength of this instance.
        /// </summary>
        /// <value>One of the enumeration values of <see cref="ChecksumStrength"/> that specifies the strength of this instance.</value>
        public ChecksumStrength Strength { get; }

        /// <summary>
        /// Converts the the <see cref="Bytes"/> of this instance to its equivalent hexadecimal representation.
        /// </summary>
        /// <returns>A hexadecimal representation of this instance.</returns>
        public override string ToString()
        {
            return Checksum.ToHexadecimal();
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="CacheValidator"/> object.
        /// </summary>
        /// <returns>A new <see cref="CacheValidator"/> that is a copy of this instance.</returns>
        public virtual CacheValidator Clone()
        {
            return new CacheValidator(Created, Modified, Bytes.ToArray());
        }

        /// <summary>
        /// Gets the most significant (largest) value of either <see cref="Created"/> or <see cref="Modified"/>.
        /// </summary>
        /// <returns>The most significant (largest) value of either <see cref="Created"/> or <see cref="Modified"/>.</returns>
        public DateTime GetMostSignificant()
        {
            return EnumerableConverter.FromArray(Created, Modified).Max();
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <param name="additionalChecksum">A <see cref="Double"/> array that contains zero or more checksums of the additional data this instance must represent.</param>
        public CacheValidator CombineWith(params double[] additionalChecksum)
        {
            return CombineWith(ByteConverter.FromConvertibles(additionalChecksum));
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <param name="additionalChecksum">An <see cref="Int16"/> array that contains zero or more checksums of the additional data this instance must represent.</param>
        public CacheValidator CombineWith(params short[] additionalChecksum)
        {
            return CombineWith(ByteConverter.FromConvertibles(additionalChecksum));
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <param name="additionalChecksum">A <see cref="String"/> array that contains zero or more checksums of the additional data this instance must represent.</param>
        public CacheValidator CombineWith(params string[] additionalChecksum)
        {
            List<long> result = new List<long>();
            if (additionalChecksum == null)
            {
                result.Add(StructUtility.HashCodeForNullValue);
            }
            else
            {
                for (int i = 0; i < additionalChecksum.Length; i++)
                {
                    result.Add(StructUtility.GetHashCode64(additionalChecksum[i]));
                }
            }
            return CombineWith(result.ToArray());
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <param name="additionalChecksum">An <see cref="Int32"/> array that contains zero or more checksums of the additional data this instance must represent.</param>
        public CacheValidator CombineWith(params int[] additionalChecksum)
        {
            return CombineWith(ByteConverter.FromConvertibles(additionalChecksum));
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <param name="additionalChecksum">An <see cref="Int64"/> array that contains zero or more checksums of the additional data this instance must represent.</param>
        public CacheValidator CombineWith(params long[] additionalChecksum)
        {
            return CombineWith(ByteConverter.FromConvertibles(additionalChecksum));
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <param name="additionalChecksum">A <see cref="Single"/> array that contains zero or more checksums of the additional data this instance must represent.</param>
        public CacheValidator CombineWith(params float[] additionalChecksum)
        {
            return CombineWith(ByteConverter.FromConvertibles(additionalChecksum));
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <param name="additionalChecksum">An <see cref="UInt16"/> array that contains zero or more checksums of the additional data this instance must represent.</param>
        public CacheValidator CombineWith(params ushort[] additionalChecksum)
        {
            return CombineWith(ByteConverter.FromConvertibles(additionalChecksum));
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <param name="additionalChecksum">An <see cref="UInt32"/> array that contains zero or more checksums of the additional data this instance must represent.</param>
        public CacheValidator CombineWith(params uint[] additionalChecksum)
        {
            return CombineWith(ByteConverter.FromConvertibles(additionalChecksum));
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <param name="additionalChecksum">An <see cref="UInt64"/> array that contains zero or more checksums of the additional data this instance must represent.</param>
        public CacheValidator CombineWith(params ulong[] additionalChecksum)
        {
            return CombineWith(ByteConverter.FromConvertibles(additionalChecksum));
        }

        /// <summary>
        /// Combines the <paramref name="additionalChecksum"/> to the representation of this instance.
        /// </summary>
        /// <param name="additionalChecksum">An array of bytes containing a checksum of the additional data this instance must represent.</param>
        public CacheValidator CombineWith(params byte[] additionalChecksum)
        {
            if (additionalChecksum == null) { return this; }
            if (additionalChecksum.Length == 0) { return this; }
            ComputedChecksum = null;
            Bytes.AddRange(additionalChecksum);
            return this;
        }

        /// <summary>
        /// Gets the most significant <see cref="CacheValidator"/> object from the most significant (largest) value of either <see cref="Created"/> or <see cref="Modified"/> in the specified <paramref name="sequence"/>.
        /// </summary>
        /// <param name="sequence">A sequence of  <see cref="CacheValidator"/> objects to parse for the most significant (largest) value of either <see cref="Created"/> or <see cref="Modified"/>.</param>
        /// <returns>The most significant <see cref="CacheValidator"/> object from the specified <paramref name="sequence"/>.</returns>
        public static CacheValidator GetMostSignificant(params CacheValidator[] sequence)
        {
            Validator.ThrowIfNull(sequence, nameof(sequence));
            CacheValidator mostSignificant = Default;
            foreach (CacheValidator candidate in sequence)
            {
                if (candidate.GetMostSignificant().Ticks > mostSignificant.GetMostSignificant().Ticks) { mostSignificant = candidate; }
            }
            return mostSignificant;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Checksum.ToHexadecimal().GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            CacheValidator validator = obj as CacheValidator;
            if (validator == null) { return false; }
            return Equals(validator);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>. </returns>
        public bool Equals(CacheValidator other)
        {
            if (other == null) { return false; }
            return (Checksum.ToHexadecimal() == other.Checksum.ToHexadecimal());
        }
    }
}