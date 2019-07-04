using System;
using System.Collections.Generic;
using Cuemon.ComponentModel.TypeConverters;
using Cuemon.Security.Cryptography;

namespace Cuemon.Integrity
{
    /// <summary>
    /// Provides a way to fluently represent checksum values of arbitrary data.
    /// </summary>
    public class ChecksumBuilder : IIntegrity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksumBuilder"/> class.
        /// </summary>
        public ChecksumBuilder() : this((byte[])null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksumBuilder"/> class.
        /// </summary>
        /// <param name="checksum">A <see cref="double"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="ChecksumBuilderOptions"/> which need to be configured.</param>
        public ChecksumBuilder(double checksum, Action<ChecksumBuilderOptions> setup = null) : this(ConvertFactory.UseConverter<BaseConvertibleConverter>().ChangeType(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksumBuilder"/> class.
        /// </summary>
        /// <param name="checksum">A <see cref="short"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="ChecksumBuilderOptions"/> which need to be configured.</param>
        public ChecksumBuilder(short checksum, Action<ChecksumBuilderOptions> setup = null) : this(ConvertFactory.UseConverter<BaseConvertibleConverter>().ChangeType(checksum), setup)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksumBuilder"/> class.
        /// </summary>
        /// <param name="checksum">A <see cref="string"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="ChecksumBuilderOptions"/> which need to be configured.</param>
        public ChecksumBuilder(string checksum, Action<ChecksumBuilderOptions> setup = null) : this(Generate.HashCode64(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksumBuilder"/> class.
        /// </summary>
        /// <param name="checksum">A <see cref="int"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="ChecksumBuilderOptions"/> which need to be configured.</param>
        public ChecksumBuilder(int checksum, Action<ChecksumBuilderOptions> setup = null) : this(ConvertFactory.UseConverter<BaseConvertibleConverter>().ChangeType(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksumBuilder"/> class.
        /// </summary>
        /// <param name="checksum">A <see cref="long"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="ChecksumBuilderOptions"/> which need to be configured.</param>
        public ChecksumBuilder(long checksum, Action<ChecksumBuilderOptions> setup = null) : this(ConvertFactory.UseConverter<BaseConvertibleConverter>().ChangeType(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksumBuilder"/> class.
        /// </summary>
        /// <param name="checksum">A <see cref="float"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="ChecksumBuilderOptions"/> which need to be configured.</param>
        public ChecksumBuilder(float checksum, Action<ChecksumBuilderOptions> setup = null) : this(ConvertFactory.UseConverter<BaseConvertibleConverter>().ChangeType(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksumBuilder"/> class.
        /// </summary>
        /// <param name="checksum">A <see cref="ushort"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="ChecksumBuilderOptions"/> which need to be configured.</param>
        public ChecksumBuilder(ushort checksum, Action<ChecksumBuilderOptions> setup = null) : this(ConvertFactory.UseConverter<BaseConvertibleConverter>().ChangeType(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksumBuilder"/> class.
        /// </summary>
        /// <param name="checksum">A <see cref="uint"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="ChecksumBuilderOptions"/> which need to be configured.</param>
        public ChecksumBuilder(uint checksum, Action<ChecksumBuilderOptions> setup = null) : this(ConvertFactory.UseConverter<BaseConvertibleConverter>().ChangeType(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksumBuilder"/> class.
        /// </summary>
        /// <param name="checksum">A <see cref="ulong"/> value containing a byte-for-byte checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="ChecksumBuilderOptions"/> which need to be configured.</param>
        public ChecksumBuilder(ulong checksum, Action<ChecksumBuilderOptions> setup = null) : this(ConvertFactory.UseConverter<BaseConvertibleConverter>().ChangeType(checksum), setup)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksumBuilder"/> class.
        /// </summary>
        /// <param name="checksum">An array of bytes containing a checksum of the data this instance represents.</param>
        /// <param name="setup">The <see cref="ChecksumBuilderOptions"/> which may be configured.</param>
        public ChecksumBuilder(byte[] checksum, Action<ChecksumBuilderOptions> setup = null)
        {
            var options = Patterns.Configure(setup);
            Algorithm = options.Algorithm;
            Bytes = checksum == null ? new List<byte>() : new List<byte>(checksum);
        }

        /// <summary>
        /// Gets the hash algorithm to use for the checksum computation.
        /// </summary>
        /// <value>The hash algorithm to use for the checksum computation.</value>
        public CryptoAlgorithm Algorithm { get; protected set; }

        /// <summary>
        /// Gets a byte array that is the result of the associated <see cref="ChecksumBuilder"/>.
        /// </summary>
        /// <value>The byte array that is the result of the associated <see cref="ChecksumBuilder"/>.</value>
        internal List<byte> Bytes { get; set; }

        /// <summary>
        /// Gets a <see cref="HashResult"/> containing a computed hash value of the data this instance represents.
        /// </summary>
        /// <value>A <see cref="HashResult"/> containing a computed hash value of the data this instance represents.</value>
        public HashResult Checksum => ComputedHash ?? (ComputedHash = HashFactory.CreateCrypto(Algorithm).ComputeHash(Bytes.ToArray()));

        /// <summary>
        /// Gets or sets the computed checksum of <see cref="Bytes"/>.
        /// </summary>
        /// <value>The computed checksum of <see cref="Bytes"/>.</value>
        protected HashResult ComputedHash { get; set; }

        /// <summary>
        /// Provides a way to append additional checksum to the representation of this instance.
        /// </summary>
        /// <param name="checksum">A <see cref="T:byte[]"/> containing a checksum of the additional data this instance must represent.</param>
        public void AppendChecksum(byte[] checksum)
        {
            ComputedHash = null;
            Bytes.AddRange(checksum);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Checksum.ToHexadecimalString().GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var builder = obj as ChecksumBuilder;
            if (builder == null) { return false; }
            return Equals(builder);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>. </returns>
        public bool Equals(ChecksumBuilder other)
        {
            if (other == null) { return false; }
            return (Checksum.ToHexadecimalString() == other.Checksum.ToHexadecimalString());
        }

        /// <summary>
        /// Converts the the <see cref="Bytes"/> of this instance to its equivalent hexadecimal representation.
        /// </summary>
        /// <returns>A hexadecimal representation of this instance.</returns>
        public override string ToString()
        {
            return Checksum.ToHexadecimalString();
        }
    }
}