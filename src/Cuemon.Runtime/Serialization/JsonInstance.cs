using System.Collections.Generic;
using System.Globalization;

namespace Cuemon.Runtime.Serialization
{
	/// <summary>
	/// Represents a JSON instance from a structural data source.
	/// </summary>
	public abstract class JsonInstance
	{
		private readonly string _name;
        private readonly object _value;
        private readonly int _nodeNumber;
        private JsonInstance _parent;
		private JsonInstanceCollection _instances;

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonInstance"/> class.
		/// </summary>
		/// <param name="name">The name of the JSON object.</param>
		/// <param name="value">The value of the JSON object.</param>
		/// <param name="nodeNumber">The logical node number of the JSON object placement in the originating structural data source.</param>
		protected JsonInstance(string name, object value, int nodeNumber)
		{
			_name = name;
			_value = value;
			_nodeNumber = nodeNumber;
			_instances = new JsonInstanceCollection();
		}

		/// <summary>
		/// Gets the name of the JSON object.
		/// </summary>
		public string Name 
		{ 
			get { return _name; }
		}

		/// <summary>
		/// Gets the value of the JSON object.
		/// </summary>
		public object Value
		{
			get { return _value; }
		}

		/// <summary>
		/// Gets the logical node number of the JSON object placement in the originating structural data source.
		/// </summary>
		protected int NodeNumber
		{
			get { return _nodeNumber; }
		}

		/// <summary>
		/// Computes and returns a MD5 signature of the following properties: <see cref="Name"/>, <see cref="Value"/> and <see cref="NodeNumber"/>.
		/// </summary>
		/// <returns>A MD5 signature of the following properties: <see cref="Name"/>, <see cref="Value"/> and <see cref="NodeNumber"/>.</returns>
		public virtual string GetSignature()
		{
            return StructUtility.GetHashCode64(string.Concat(Name, Value, NodeNumber)).ToString(CultureInfo.InvariantCulture);
        }

		/// <summary>
		/// Gets the children of the current <see cref="JsonInstance"/> object.
		/// </summary>
		public JsonInstanceCollection Instances
		{
			get { return _instances; }
		}

		/// <summary>
		/// Gets or sets the parent of this <see cref="JsonInstance"/> object.
		/// </summary>
		/// <value>The parent of this <see cref="JsonInstance"/> object.</value>
		public JsonInstance Parent
		{
			get { return _parent; }
			set { _parent = value; }
		}

		/// <summary>
		/// Determines whether this <see cref="JsonInstance"/> should be part of an array.
		/// </summary>
		/// <returns>
		///   <c>true</c> if this <see cref="JsonInstance"/> should be part of an array; otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>Because of the complexity when generating JSON, this property should be treated as a hint-only. Additional logic is assumed necessary.</remarks>
		public virtual bool IsPartOfArray()
		{
			if (Parent == null) { return false; }
			if (Parent.Instances.FilterByName(Name).Count > 1) { return true; }
			return false;
		}

		/// <summary>
		/// Determines whether this <see cref="JsonInstance"/> is in a state, where a <see cref="JsonWriter.BeginArray"/> should be written.
		/// </summary>
		/// <returns>
		///   <c>true</c> if this <see cref="JsonInstance"/> is in a state, where a <see cref="JsonWriter.BeginArray"/> should be written; otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>Because of the complexity when generating JSON, this property should be treated as a hint-only. Additional logic is assumed necessary.</remarks>
		public virtual bool WriteStartArray()
		{
			if (IsPartOfArray())
			{
				IList<JsonInstance> instances = Parent.Instances.FilterByName(Name);
				for (int i = 0; i < instances.Count; i++)
				{
					if (i == 0)
					{
						if (instances[i].GetSignature() == GetSignature()) { return true; }
					}
					return false;
				}
			}
			return false;
		}

		/// <summary>
		/// Determines whether this <see cref="JsonInstance"/> is in a state, where a <see cref="JsonWriter.EndArray"/> should be written.
		/// </summary>
		/// <returns>
		///   <c>true</c> if this <see cref="JsonInstance"/> is in a state, where a <see cref="JsonWriter.EndArray"/> should be written; otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>Because of the complexity when generating JSON, this property should be treated as a hint-only. Additional logic is assumed necessary.</remarks>
		public virtual bool WriteEndArray()
		{
			if (IsPartOfArray())
			{
				IList<JsonInstance> instances = Parent.Instances.FilterByName(Name);
				for (int i = 0; i < instances.Count; i++)
				{
					if (instances[i].GetSignature() == GetSignature()) { return ((i + 1) == instances.Count); }
				}
			}
			return false;
		}

		/// <summary>
		/// Determines whether this <see cref="JsonInstance"/> is in a state, where a <see cref="JsonWriter.ValueSeperator"/> should be written.
		/// </summary>
		/// <returns>
		///   <c>true</c> if this <see cref="JsonInstance"/> is in a state, where a <see cref="JsonWriter.ValueSeperator"/> should be written; otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>Because of the complexity when generating JSON, this property should be treated as a hint-only. Additional logic is assumed necessary.</remarks>
		public virtual bool WriteValueSeperator()
		{
			if (Parent == null) { return false; }
			IList<JsonInstance> instances = Parent.Instances;
			for (int i = 0; i < instances.Count; i++)
			{
				if ((i + 1) < instances.Count)
				{
					if (instances[i].GetSignature() == GetSignature()) { return true; }
				}
			}
			return false;
		}
	}
}