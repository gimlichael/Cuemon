﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cuemon.Collections.Generic
{
	/// <summary>
	/// Provides <see cref="IEnumerable{T}"/> size comparison.
	/// </summary>
    /// <typeparam name="T">The <see cref="IEnumerable{T}"/> type to compare.</typeparam>
	public class EnumerableSizeComparer<T> : Comparer<T> where T : IEnumerable
	{
		/// <summary>
		/// Returns a default comparer for the type specified by the generic argument.
		/// </summary>
		public static new IComparer<T> Default
		{
            get { return new EnumerableSizeComparer<T>(); }
		}

		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>
		/// A signed integer that indicates the relative values of x and y, as explained here: Less than zero - x is less than y. Zero - x equals y. Greater than zero - x is greater than y.
		/// </returns>
		public override int Compare(T x, T y)
		{
			var depthOfX = x.OfType<T>().Count();
            var depthOfY = y.OfType<T>().Count();

			if (depthOfX > depthOfY) { return 1; }
			if (depthOfX < depthOfY) { return -1; }
			return 0;
		}
	}
}