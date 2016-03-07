using System;
using System.Collections.Generic;
using System.Text;
using Cuemon.Collections.Generic;

namespace Cuemon
{
	/// <summary>
	/// This utility class is designed to make common <see cref="byte"/> operations easier to work with.
	/// </summary>
	public static class ByteUtility
	{
		/// <summary>
		/// Combines a variable number of byte arrays into one byte array.
		/// </summary>
		/// <param name="bytes">The byte arrays to combine.</param>
		/// <returns>A variable number of <b>byte arrays</b> combined into one <b>byte array</b>.</returns>
		public static byte[] CombineByteArrays(params byte[][] bytes)
		{
			List<byte> combinedBytes = new List<byte>(EnumerableUtility.Concat<byte>(bytes));
			return combinedBytes.ToArray();
		}

		/// <summary>
		/// Removes the preamble information (if present) from the specified <see cref="byte"/> array.
		/// </summary>
		/// <param name="input">The input <see cref="byte"/> array to process.</param>
		/// <param name="encoding">The encoding to use when determining the preamble to remove.</param>
		/// <returns>A <see cref="byte"/> array without preamble information.</returns>
		public static byte[] RemovePreamble(byte[] input, Encoding encoding)
		{
			if (input == null) { throw new ArgumentNullException(nameof(input)); }
			if (encoding == null) { throw new ArgumentNullException(nameof(encoding)); }
			if (input.Length <= 1) { return input; }
			byte[] preamble = encoding.GetPreamble();
			if (preamble.Length == 0) { return input; }
			if ((preamble[0] == input[0] && preamble[1] == input[1]) || (preamble[0] == input[1] && preamble[1] == input[0]))
			{
				int bytesToRead = input.Length;
				bytesToRead -= preamble.Length;
				byte[] bytes = new byte[bytesToRead];
				Array.Copy(input, preamble.Length, bytes, 0, bytesToRead);
				return bytes;
			}
			return input;
		}

		/// <summary>
		/// Removes trailing zero information (if any) from the specified <see cref="byte"/> array.
		/// </summary>
		/// <param name="input">The input <see cref="byte"/> array to process.</param>
		/// <returns>A <see cref="byte"/> array without trailing zeros.</returns>
		public static byte[] RemoveTrailingZeros(byte[] input)
		{
			if (input == null) { throw new ArgumentNullException(nameof(input)); }
			if (input.Length <= 1) { throw new ArgumentException("Size must be larger than 1.", nameof(input)); }
			bool hasTrailingZeros = false;
			int marker = input.Length - 1;
			while (input[marker] == 0)
			{
				if (!hasTrailingZeros) { hasTrailingZeros = true; }
				marker--;
			}
			if (!hasTrailingZeros) { return input; }
			marker++;
			byte[] output = new byte[marker];
			Array.Copy(input, output, marker);
			return output;
		}
	}

	/// <summary>
	/// Specifies what action to take in regards to encoding preamble sequences.
	/// </summary>
	public enum PreambleSequence
	{
		/// <summary>
		/// Any encoding preamble sequences will be preserved.
		/// </summary>
		Keep = 0,
		/// <summary>
		/// Any encoding preamble sequences will be removed.
		/// </summary>
		Remove = 1
	}
}