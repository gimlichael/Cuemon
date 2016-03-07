namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Specifies the key size to be used in the Advanced Encryption Standard (AES) cipher.
    /// </summary>
    public enum AdvancedEncryptionStandardKeySize
    {
        /// <summary>
        /// A key size of 128 bit.
        /// </summary>
        AES128 = 0,
        /// <summary>
        /// A key size of 192 bit.
        /// </summary>
        AES192 = 1,
        /// <summary>
        /// A key size of 256 bit.
        /// </summary>
        AES256 = 2,
    }
}