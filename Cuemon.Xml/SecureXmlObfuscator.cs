using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Cuemon.Security.Cryptography;

namespace Cuemon.Xml
{
	/// <summary>
	/// Provides methods for secure obfuscation (based on AES) of the otherwise similar class <see cref="XmlObfuscator"/>.
	/// </summary>
	/// <remarks>Logic used from the <see cref="AdvancedEncryptionStandardUtility"/> class.</remarks>
	public sealed class SecureXmlObfuscator : XmlObfuscator
	{
        private const string MappingEncryptedElement = "E";

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="SecureXmlObfuscator"/> class using <see cref="Encoding.UTF8"/> for the text encoding.
		/// </summary>
		/// <param name="key">The key to use in the encryption algorithm.</param>
		/// <param name="initializationVector">The initialization vector (IV) to use in the encryption algorithm.</param>
		public SecureXmlObfuscator(byte[] key, byte[] initializationVector) : this(key, initializationVector, Encoding.UTF8)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureXmlObfuscator"/> class.
		/// </summary>
		/// <param name="key">The key to use in the encryption algorithm.</param>
		/// <param name="initializationVector">The initialization vector (IV) to use in the encryption algorithm.</param>
		/// <param name="exclusions">A sequence of <see cref="T:System.String"/> values used for excluding matching original values in the obfuscation process.</param>
		public SecureXmlObfuscator(byte[] key, byte[] initializationVector, IEnumerable<string> exclusions) : this(key, initializationVector, Encoding.UTF8, exclusions)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureXmlObfuscator"/> class using <see cref="Encoding.UTF8"/> for the text encoding.
		/// </summary>
		/// <param name="key">The key to use in the encryption algorithm.</param>
		/// <param name="initializationVector">The initialization vector (IV) to use in the encryption algorithm.</param>
		/// <param name="encoding">The text encoding to use.</param>
		public SecureXmlObfuscator(byte[] key, byte[] initializationVector, Encoding encoding) : this(key, initializationVector, encoding, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureXmlObfuscator"/> class.
		/// </summary>
		/// <param name="key">The key to use in the encryption algorithm.</param>
		/// <param name="initializationVector">The initialization vector (IV) to use in the encryption algorithm.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <param name="exclusions">A sequence of <see cref="T:System.String"/> values used for excluding matching original values in the obfuscation process.</param>
		public SecureXmlObfuscator(byte[] key, byte[] initializationVector, Encoding encoding, IEnumerable<string> exclusions) : base(encoding, exclusions)
		{
			Key = key;
			InitializationVector = initializationVector;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the secret key for the AES algorithm.
		/// </summary>
		public byte[] Key { get; }

        /// <summary>
		/// Gets the initialization vector (IV) for the AES algorithm.
		/// </summary>
		public byte[] InitializationVector { get; }

        #endregion

		#region Methods
		/// <summary>
		/// Revert the obfuscated XML document of <paramref name="value"/> to its original state by applying the mappable XML document of <paramref name="mapping"/>.
		/// </summary>
		/// <param name="value">The obfuscated <see cref="Stream"/> to revert.</param>
		/// <param name="mapping">A <see cref="Stream"/> containing mappable values necessary to revert <paramref name="value"/> to its original state.</param>
		/// <returns>
		/// A <see cref="Stream"/> object where the obfuscated XML document has been reverted to its original XML document.
		/// </returns>
		public override Stream Revert(Stream value, Stream mapping)
		{
            Validator.ThrowIfNull(value, nameof(value));
            Validator.ThrowIfNull(mapping, nameof(mapping));

            mapping = Disposable.SafeInvoke(() => new MemoryStream(), (ms, m) =>
            {
                var document = XmlDocumentConverter.FromStream(m);
                var mappingNode = document.DocumentElement;

                var encryptedNode = mappingNode.GetElementsByTagName(MappingEncryptedElement).Item(0);
                if (encryptedNode != null)
                {
                    mappingNode.InnerXml = StringConverter.FromBytes(AdvancedEncryptionStandardUtility.Decrypt(Convert.FromBase64String(encryptedNode.InnerText), Key, InitializationVector), options =>
                    {
                        options.Encoding = Encoding;
                        options.Preamble = PreambleSequence.Remove;
                    });
                }

                using (var writer = XmlWriter.Create(ms, XmlWriterUtility.CreateSettings(o => o.Encoding = Encoding)))
                {
                    document.WriteTo(writer);
                }

                ms.Position = 0;
                return ms;
            }, mapping);

			return base.Revert(value, mapping);
		}
		
		/// <summary>
		/// Creates and returns a mappable XML document of the original values and the obfuscated values.
		/// </summary>
		/// <returns>A mappable XML document of the original values and the obfuscated values.</returns>
		public override Stream CreateMapping()
        {
            return Disposable.SafeInvoke(() => new MemoryStream(), ms =>
            {
                var document = XmlDocumentConverter.FromStream(base.CreateMapping());
                XmlNode mappingNode = document.DocumentElement;
                var innerXmlOfMappingNode = mappingNode.InnerXml;
                var innerXmlOfMappingNodeBytes = ByteConverter.FromString(innerXmlOfMappingNode, options =>
                {
                    options.Encoding = Encoding;
                    options.Preamble = PreambleSequence.Remove;
                });
                var encryptedNode = document.CreateElement(MappingEncryptedElement);
                encryptedNode.InnerText = Convert.ToBase64String(AdvancedEncryptionStandardUtility.Encrypt(innerXmlOfMappingNodeBytes, Key, InitializationVector));
                mappingNode.InnerXml = "";
                mappingNode.AppendChild(encryptedNode);
                using (var writer = XmlWriter.Create(ms, XmlWriterUtility.CreateSettings(o => o.Encoding = Encoding)))
                {
                    document.WriteTo(writer);
                }
                ms.Position = 0;
                return ms;
            });
        }
		#endregion
	}
}