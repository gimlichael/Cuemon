using System;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the <see cref="ConvertibleConverter"/> class.
    /// </summary>
    public static class ConvertibleConverterExtensions
    {
        /// <summary>
        /// Returns an <see cref="IConvertible"/> primitive converted from the specified array <paramref name="value"/> of bytes.
        /// </summary>
        /// <typeparam name="T">The type of the expected return <paramref name="value"/> after conversion.</typeparam>
        /// <param name="value">The value to convert into an <see cref="IConvertible"/>.</param>
        /// <returns>An <see cref="IConvertible"/> primitive formed by n-bytes beginning at 0.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentException">
        /// <typeparamref name="T"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Boolean"/>, <see cref="Char"/>, <see cref="double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="ushort"/>, <see cref="UInt32"/> and <see cref="UInt64"/>.
        /// </exception>
        public static T ToConvertible<T>(this byte[] value) where T : struct, IConvertible
        {
            return ConvertibleConverter.FromBytes<T>(value);
        }
    }
}