namespace Cuemon
{
    /// <summary>
    /// Specifies the severity of the generation of a random number.
    /// </summary>
    public enum RandomSeverity
    {
        /// <summary>
        /// A fast but less accurate method of generating a random number.
        /// </summary>
        Simple,
        /// <summary>
        /// A slower but also more accurate way of generating a random number.
        /// </summary>
        Strong
    }
}