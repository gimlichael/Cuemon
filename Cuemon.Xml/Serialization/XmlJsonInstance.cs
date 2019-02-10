using System;
using System.Xml.XPath;

namespace Cuemon.Runtime.Serialization
{
	/// <summary>
	/// Represents a JSON instance from an XML data source.
	/// </summary>
	public sealed class XmlJsonInstance : JsonInstance
	{
	    /// <summary>
		/// Initializes a new instance of the <see cref="XmlJsonInstance"/> class.
		/// </summary>
		/// <param name="name">The name of the JSON object.</param>
		/// <param name="value">The value of the JSON object.</param>
		/// <param name="nodeNumber">The logical node number of the JSON object placement in the originating structural data source.</param>
		public XmlJsonInstance(string name, object value, int nodeNumber) : this(name, value, nodeNumber, XPathNodeType.Text)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlJsonInstance"/> class.
		/// </summary>
		/// <param name="name">The name of the JSON object.</param>
		/// <param name="value">The value of the JSON object.</param>
		/// <param name="nodeNumber">The logical node number of the JSON object placement in the originating structural data source.</param>
		/// <param name="nodeType">The node type of the XML document to convert into a JSON representation.</param>
		public XmlJsonInstance(string name, object value, int nodeNumber, XPathNodeType nodeType) : base(name, value, nodeNumber)
		{
			NodeType = nodeType;
		}

		/// <summary>
		/// Gets the originating node type of the XML document.
		/// </summary>
		/// <value>The originating node type of the XML document.</value>
		public XPathNodeType NodeType { get; }

	    /// <summary>
		/// Computes and returns a MD5 signature of the following properties: <see cref="P:Cuemon.IO.JsonInstance.Name"/>, <see cref="P:Cuemon.IO.JsonInstance.Value"/>, <see cref="P:Cuemon.IO.JsonInstance.NodeNumber"/> and <see cref="NodeType"/>.
		/// </summary>
		/// <returns>
		/// A MD5 signature of the following properties: <see cref="P:Cuemon.IO.JsonInstance.Name"/>, <see cref="P:Cuemon.IO.JsonInstance.Value"/>, <see cref="P:Cuemon.IO.JsonInstance.NodeNumber"/> and <see cref="NodeType"/>.
		/// </returns>
		public override string GetSignature()
		{
			return StructUtility.GetHashCode64(string.Concat(Name, Value, NodeNumber, Enum.GetName(typeof(XPathNodeType), NodeType))).ToString();
		}
	}
}