namespace Cuemon.Security.Cryptography
{
    /// <summary>
    /// Defines the common ways to express a CRC generator polynomial representation.
    /// </summary>
    /// <remarks>
    /// For more information regarding these representations, please refer to these Wikipedia articles:<br/>
    /// Mathematics of CRC - http://en.wikipedia.org/wiki/Mathematics_of_CRC#Polynomial_representations<br/>
    /// Cyclic redundancy check - http://en.wikipedia.org/wiki/Cyclic_redundancy_check#Designing_CRC_polynomials
    /// </remarks>
    public enum PolynomialRepresentation
    {
        /// <summary>
        /// The most-significant byte (MSB) first of a CRC polynomial representation.
        /// </summary>
        Normal,
        /// <summary>
        /// The least-significant byte (LSB) first of a CRC polynomial representation.
        /// </summary>
        Reversed,
        /// <summary>
        /// The Koopman notation of a CRC polynomial representation.
        /// </summary>
        ReversedReciprocal
    }
}