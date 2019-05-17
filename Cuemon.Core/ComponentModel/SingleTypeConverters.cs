using System;
using Cuemon.ComponentModel.TypeConverters;

namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Extension methods for the <see cref="T:IConversion{System.Single}"/> interface.
    /// </summary>
    public static class SingleTypeConverters
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="float"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="float"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <seealso cref="BaseConvertibleConverter"/>
        /// <seealso cref="EndianOptions"/>
        public static byte[] ToByteArray(this IConversion<float> _, float input, Action<EndianOptions> setup = null)
        {
            return Converter<float, byte[], EndianOptions>.UseConverter<BaseConvertibleConverter>(input, setup);
        }
    }
}