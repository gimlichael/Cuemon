using System;
using System.Xml.Serialization;

namespace Cuemon.Xml.Serialization
{
    /// <summary>
    /// A class designed to help assure qualified names in XML serializations.
    /// </summary>
    public sealed class XmlQualifiedEntity
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlQualifiedEntity"/> class.
        /// </summary>
        /// <param name="attribute">The XML related attribute to extract qualified name information about.</param>
        public XmlQualifiedEntity(XmlElementAttribute attribute) : this(ValidateArguments(attribute).ElementName, ValidateArguments(attribute).Namespace)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlQualifiedEntity"/> class.
        /// </summary>
        /// <param name="attribute">The XML related attribute to extract qualified name information about.</param>
        public XmlQualifiedEntity(XmlAttributeAttribute attribute) : this(ValidateArguments(attribute).AttributeName, ValidateArguments(attribute).Namespace)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlQualifiedEntity"/> class.
        /// </summary>
        /// <param name="attribute">The XML related attribute to extract qualified name information about.</param>
        public XmlQualifiedEntity(XmlRootAttribute attribute) : this(ValidateArguments(attribute).ElementName, ValidateArguments(attribute).Namespace)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlQualifiedEntity"/> class.
        /// </summary>
        /// <param name="attribute">The XML related attribute to extract qualified name information about.</param>
        public XmlQualifiedEntity(XmlAnyElementAttribute attribute) : this(ValidateArguments(attribute).Name, ValidateArguments(attribute).Namespace)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlQualifiedEntity"/> class.
        /// </summary>
        /// <param name="localName">The local name of the entity.</param>
        public XmlQualifiedEntity(string localName) : this(localName, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlQualifiedEntity"/> class.
        /// </summary>
        /// <param name="localName">The local name of the entity.</param>
        /// <param name="ns">The namespace URI to associate with the entity.</param>
        public XmlQualifiedEntity(string localName, string ns) : this(null, localName, ns)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlQualifiedEntity"/> class.
        /// </summary>
        /// <param name="prefix">The namespace prefix of the entity.</param>
        /// <param name="localName">The local name of the entity.</param>
        /// <param name="ns">The namespace URI to associate with the entity.</param>
        public XmlQualifiedEntity(string prefix, string localName, string ns)
        {
            Prefix = prefix;
            LocalName = localName;
            Namespace = ns;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the local name of the entity.
        /// </summary>
        /// <value>The local name of the entity.</value>
        public string LocalName { get; private set; }

        /// <summary>
        /// Gets the namespace URI to associate with the entity.
        /// </summary>
        /// <value>The namespace URI to associate with the entity.</value>
        public string Namespace { get; private set; }

        /// <summary>
        /// Gets the namespace prefix of the entity.
        /// </summary>
        /// <value>The namespace prefix of the entity.</value>
        public string Prefix { get; private set; }
        #endregion

        #region Methods
        private static T ValidateArguments<T>(T attribute) where T : Attribute
        {
            if (attribute == null) { throw new ArgumentNullException(nameof(attribute)); }
            return attribute;
        }
        #endregion
    }
}