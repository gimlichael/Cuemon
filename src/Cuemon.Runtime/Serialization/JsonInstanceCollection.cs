using System;
using System.Collections.Generic;

namespace Cuemon.Runtime.Serialization
{
	/// <summary>
	/// Represents a list of JSON children from a structural data source.
	/// </summary>
	public sealed class JsonInstanceCollection : List<JsonInstance>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="JsonInstanceCollection"/> class.
		/// </summary>
		public JsonInstanceCollection()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonInstanceCollection"/> class.
		/// </summary>
		/// <param name="instances">The instances to fill this <see cref="JsonInstanceCollection"/> with.</param>
		public JsonInstanceCollection(IEnumerable<JsonInstance> instances) : base(instances)
		{
		}

		/// <summary>
		/// Returns a list of <see cref="JsonInstance"/> filtered by the specified <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The name of the <see cref="JsonInstance"/> to filter by.</param>
		/// <returns>A list of <see cref="JsonInstance"/> filtered by the specified <paramref name="name"/>.</returns>
		public IList<JsonInstance> FilterByName(string name)
		{
			return new List<JsonInstance>(FilterByNameCore(name));
		}

		private IEnumerable<JsonInstance> FilterByNameCore(string name)
		{
			foreach (JsonInstance instance in this) { if (instance.Name == name) { yield return instance; } }
		}

		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>
		/// Value Condition Less than zerox is less than y.Zerox equals y.Greater than zerox is greater than y.
		/// </returns>
		public static int Compare(JsonInstance x, JsonInstance y)
		{
			if (x == null) { throw new ArgumentNullException(nameof(x)); }
			if (y == null) { throw new ArgumentNullException(nameof(y)); }
			return string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
		}
	}
}