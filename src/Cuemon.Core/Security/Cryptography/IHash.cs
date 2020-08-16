using System.IO;

namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Defines the bare minimum of cryptographic transformations.
    /// </summary>
    public interface IHash
    {
        /// <summary>
        /// Computes the hash value for the specified <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="T:byte[]"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        HashResult ComputeHash(byte[] input);

        /// <summary>
        /// Computes the hash value for the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="input">The <see cref="Stream"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        HashResult ComputeHash(Stream input);
    }
}