using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Cuemon.Collections.Generic;
using Cuemon.IO;

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
        /// Gets or sets the <see cref="Assembly"/> that will serve as the ideal candidate for a <see cref="CacheValidator"/> reference point. Default is <see cref="Assembly.GetEntryAssembly"/> with a fallback to <c>Cuemon.Core.dll</c>.
        /// </summary>
        /// <value>The assembly to use as a <see cref="CacheValidator"/> reference point.</value>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        public static Assembly AssemblyReference
        {
            get => _assemblyValue ?? (_assemblyValue = LazyAssembly.Value);
            set
            {
                Validator.ThrowIfNull(value, nameof(value));
                _assemblyValue = value;
                _referencePointCacheValidator = null;
            }
        }

        /// <summary>
        /// Converts the specified <paramref name="file"/> to a <see cref="CacheValidator"/> representation.
        /// </summary>
        /// <param name="file">The <see cref="FileInfo"/> to convert.</param>
        /// <param name="setup">The <see cref="FileChecksumOptions"/> which may be configured.</param>
        /// <returns>A <see cref="CacheValidator"/> that represents the <paramref name="file"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="file"/> cannot be null.
        /// </exception>
        public static CacheValidator FromFileInfo(FileInfo file, Action<FileChecksumOptions> setup = null)
        {
            Validator.ThrowIfNull(file, nameof(file));
            var options = Patterns.Configure(setup);
            return HashResult.FromFileInfo(file, fio =>
            {
                fio.BytesToRead = options.BytesToRead;
                fio.IntegrityConverter = (fi, checksumBytes) =>
                {
                    if (checksumBytes.Length > 0)
                    {
                        return new CacheValidator(fi.CreationTimeUtc, fi.LastWriteTimeUtc, Generate.HashCode64(checksumBytes.Cast<IConvertible>()), o =>
                        {
                            o.Method = options.Method;
                            o.Algorithm = options.Algorithm;
                        });
                    }
                    var fileNameHashCode64 = Generate.HashCode64(file.FullName);
                    return new CacheValidator(fi.CreationTimeUtc, fi.LastWriteTimeUtc, fileNameHashCode64, o =>
                    {
                        o.Method = options.Method;
                        o.Algorithm = options.Algorithm;
                    });
                };
            }) as CacheValidator;
        }

        /// <summary>
        /// Converts the specified <paramref name="assembly"/> to a <see cref="CacheValidator"/> representation.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to convert.</param>
        /// <param name="setup">The <see cref="FileChecksumOptions"/> which may be configured.</param>
        /// <returns>A <see cref="CacheValidator"/> that represents the <paramref name="assembly"/>.</returns>
        public static CacheValidator FromAssembly(Assembly assembly, Action<FileChecksumOptions> setup = null)
        {
            var assemblyHashCode64 = Generate.HashCode64(assembly.FullName);
            var assemblyLocation = assembly.Location;
            return assembly.IsDynamic
                ? new CacheValidator(DateTime.MinValue, DateTime.MaxValue, assemblyHashCode64, Patterns.ConfigureExchange<FileChecksumOptions, CacheValidatorOptions>(setup))
                : FromFileInfo(new FileInfo(assemblyLocation), setup);
        }

        private CacheValidator()
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
        /// <param name="checksum">A <see cref="double"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, double checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, Convertible.GetBytes(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="short"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, short checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, Convertible.GetBytes(checksum), setup)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="string"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, string checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, Generate.HashCode64(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="int"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, int checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, Convertible.GetBytes(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="long"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, long checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, Convertible.GetBytes(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="float"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, float checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, Convertible.GetBytes(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="ushort"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, ushort checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, Convertible.GetBytes(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="uint"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, uint checksum, Action<CacheValidatorOptions> setup = null)
            : this(created, modified, Convertible.GetBytes(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidator"/> class.
        /// </summary>
        /// <param name="created">A <see cref="DateTime"/> value for when data this instance represents was first created.</param>
        /// <param name="modified">A <see cref="DateTime"/> value for when data this instance represents was last modified.</param>
        /// <param name="checksum">A <see cref="ulong"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="CacheValidatorOptions"/> which need to be configured.</param>
        public CacheValidator(DateTime created, DateTime? modified, ulong checksum, Action<CacheValidatorOptions> setup = null) 
            : this(created, modified, Convertible.GetBytes(checksum), setup)
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
                    checksum = Convertible.GetBytes(Created.Ticks ^ Modified?.Ticks ?? DateTime.MinValue.Ticks);
                    strength = ChecksumStrength.Weak;
                    break;
                case ChecksumMethod.Combined:
                    var checksumValue = isChecksumNullOrZeroLength ? NullOrZeroLengthChecksum : Generate.HashCode64(checksum.Cast<IConvertible>());
                    checksum = Convertible.GetBytes(Created.Ticks ^ Modified?.Ticks ?? DateTime.MinValue.Ticks ^ checksumValue);
                    strength = isChecksumNullOrZeroLength ? ChecksumStrength.Weak : ChecksumStrength.Strong;
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(setup), (int)options.Method, typeof(ChecksumMethod));
            }
            Bytes = checksum == null ? new List<byte>() : new List<byte>(checksum);
            Strength = strength;
            Method = options.Method;
            Options = options;
            Algorithm = options.Algorithm;
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
        /// Gets a <see cref="CacheValidator"/> object that represents an <see cref="Assembly"/> reference point.
        /// </summary>
        /// <value>A <see cref="CacheValidator"/> object that represents an <see cref="Assembly"/> reference point.</value>
        public static CacheValidator ReferencePoint
        {
            get
            {
                if (_referencePointCacheValidator == null)
                {
                    _referencePointCacheValidator = FromAssembly(AssemblyReference);
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
                ComputedHash = ComputedHash
            };
        }

        /// <summary>
        /// Gets the most significant (largest) value of either <see cref="Created"/> or <see cref="Modified"/>.
        /// </summary>
        /// <returns>The most significant (largest) value of either <see cref="Created"/> or <see cref="Modified"/>.</returns>
        public DateTime GetMostSignificant()
        {
            return Arguments.ToEnumerableOf(Created, Modified ?? DateTime.MinValue).Max();
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