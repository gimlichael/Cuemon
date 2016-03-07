using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Cuemon.Security.Cryptography;
using Cuemon.Xml;

namespace Cuemon.Security
{
	/// <summary>
	/// Provides methods for secure obfuscation (based on AES) of the otherwise similar class <see cref="XmlObfuscator"/>.
	/// </summary>
	/// <remarks>Logic used from the <see cref="AdvancedEncryptionStandardUtility"/> class.</remarks>
	public sealed class SecureXmlObfuscator : XmlObfuscator
	{
		private byte[] _key;
		private byte[] _initializationVector;
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
			_key = key;
			_initializationVector = initializationVector;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the secret key for the AES algorithm.
		/// </summary>
		public byte[] Key
		{
			get { return _key; }
		}

		/// <summary>
		/// Gets the initialization vector (IV) for the AES algorithm.
		/// </summary>
		public byte[] InitializationVector
		{
			get { return _initializationVector; }
		}
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
			if (value == null) { throw new ArgumentNullException(nameof(value)); }
			if (mapping == null) { throw new ArgumentNullException(nameof(mapping)); }

			MemoryStream tempOutput = null;
			try
			{
				XmlDocument document = XmlDocumentConverter.FromStream(mapping);
				var mappingNode = document.DocumentElement;
                
				XmlNode encryptedNode = mappingNode.GetElementsByTagName(MappingEncryptedElement).Item(0);
				if (encryptedNode != null)
				{
					mappingNode.InnerXml = StringConverter.FromBytes(AdvancedEncryptionStandardUtility.Decrypt(Convert.FromBase64String(encryptedNode.InnerText), Key, InitializationVector), PreambleSequence.Remove, Encoding);
				}

				tempOutput = new MemoryStream();
                using (XmlWriter writer = XmlWriter.Create(tempOutput, XmlWriterUtility.CreateSettings(Encoding)))
				{
					document.WriteTo(writer);
				}
				tempOutput.Position = 0;
				mapping = tempOutput;
				tempOutput = null;
			}
			finally
			{
				if (tempOutput != null) { tempOutput.Dispose(); }
			}

			return base.Revert(value, mapping);
		}
		
		/// <summary>
		/// Creates and returns a mappable XML document of the original values and the obfuscated values.
		/// </summary>
		/// <returns>A mappable XML document of the original values and the obfuscated values.</returns>
		public override Stream CreateMapping()
		{
			MemoryStream output;
			MemoryStream tempOutput = null;
			try
			{
				XmlDocument document = XmlDocumentConverter.FromStream(base.CreateMapping());
				XmlNode mappingNode = document.DocumentElement;
				string innerXmlOfMappingNode = mappingNode.InnerXml;
				byte[] innerXmlOfMappingNodeBytes = ByteConverter.FromString(innerXmlOfMappingNode, PreambleSequence.Remove, Encoding);
				XmlElement encryptedNode = document.CreateElement(MappingEncryptedElement);
				encryptedNode.InnerText = Convert.ToBase64String(AdvancedEncryptionStandardUtility.Encrypt(innerXmlOfMappingNodeBytes, Key, InitializationVector));
				mappingNode.InnerXml = "";
				mappingNode.AppendChild(encryptedNode);
				tempOutput = new MemoryStream();
                using (XmlWriter writer = XmlWriter.Create(tempOutput, XmlWriterUtility.CreateSettings(Encoding)))
				{
					document.WriteTo(writer);
				}
				tempOutput.Position = 0;
				output = tempOutput;
				tempOutput = null;
			}
			finally 
			{
				if (tempOutput != null) { tempOutput.Dispose(); }
			}
			return output;
		}
		#endregion
	}
}