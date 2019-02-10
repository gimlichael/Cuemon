using System.Text;

namespace Cuemon
{

    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="ByteUtility"/> class.
    /// </summary>
    public static class ByteUtilityExtensions
    {
		/// <summary>
		/// Removes the preamble information (if present) from the specified <see cref="byte"/> array.
		/// </summary>
		/// <param name="input">The input <see cref="byte"/> array to process.</param>
		/// <param name="encoding">The encoding to use when determining the preamble to remove.</param>
		/// <returns>A <see cref="byte"/> array without preamble information.</returns>
		public static byte[] RemovePreamble(this byte[] input, Encoding encoding)
		{
		    return ByteUtility.RemovePreamble(input, encoding);
		}

		/// <summary>
		/// Removes trailing zero information (if any) from the specified <see cref="byte"/> array.
		/// </summary>
		/// <param name="input">The input <see cref="byte"/> array to process.</param>
		/// <returns>A <see cref="byte"/> array without trailing zeros.</returns>
		public static byte[] RemoveTrailingZeros(this byte[] input)
		{
		    return ByteUtility.RemoveTrailingZeros(input);
		}
	}
}