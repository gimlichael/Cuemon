namespace Cuemon.Web
{
    /// <summary>
    /// Specifies the filter action to perform on either query strings, headers or form data.
    /// </summary>
    public enum FieldValueFilter
    {
        /// <summary>
        /// Sanitizes the request so that all keys (with matching values) is removed.
        /// </summary>
        Remove,

        /// <summary>
        /// Sanitizes the request so that all keys is assured only the latest value applied.
        /// </summary>
        RemoveDublets
    }
}