namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="DelegateUtility"/> class.
    /// </summary>
    public static class DelegateUtilityExtension
    {
        /// <summary>
        /// Provides an easy and reflection less way to get a value from a property that is delegate compatible (such as <see cref="Doer{TResult}"/> and the likes thereof).
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of <paramref name="output"/>.</typeparam>
        /// <param name="output">The return value of a member to be routed as output through this Wrap{TResult} method.</param>
        /// <returns>The value from <paramref name="output"/>.</returns>
        public static TResult Wrap<TResult>(this TResult output)
        {
            return DelegateUtility.Wrap(output);
        }

        /// <summary>
        /// Provides a way to dynamically wrap a return value (typically from a property or field) inside an anonymous method.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of <paramref name="output" />.</typeparam>
        /// <param name="output">The value to dynamically wrap as a return value (typically from a property of field) inside an anonymous method.</param>
        /// <returns>An anonymous method that returns the value of <paramref name="output" />.</returns>
        public static Doer<TResult> DynamicWrap<TResult>(this TResult output)
        {
            return DelegateUtility.DynamicWrap(output);
        }
    }
}