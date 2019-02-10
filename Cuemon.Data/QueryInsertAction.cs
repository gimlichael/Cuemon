namespace Cuemon.Data
{
    /// <summary>
    /// Defines the available insert actions for the <see cref="DataAdapter"/> class.
    /// </summary>
    public enum QueryInsertAction
    {
        /// <summary>
        /// Indicates that no value will be returned by the insert query.
        /// </summary>
        Void = 0,
        /// <summary>
        /// Indicates that a <see cref="int"/> value will be returned by the insert query.
        /// </summary>
        IdentityInt32 = 1,
        /// <summary>
        /// Indicates that a <see cref="long"/> value will be returned by the insert query.
        /// </summary>
        IdentityInt64 = 2,
        /// <summary>
        /// Indicates that a <see cref="decimal"/> value will be returned by the insert query.
        /// </summary>
        IdentityDecimal = 3,
        /// <summary>
        /// Indicates that a <see cref="int"/> value will be returned with the amount of affected rows by the insert query.
        /// </summary>
        AffectedRows = 4
    }
}