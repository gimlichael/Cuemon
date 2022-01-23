namespace Cuemon.Threading
{
    /// <summary>
    /// Represents the base class to determine whether an async operation was a success or not.
    /// </summary>
    public abstract class ConditionalValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalValue" /> class.
        /// </summary>
        /// <param name="succeeded">Indicates whether the operation was successfull or not.</param>
        protected ConditionalValue(bool succeeded)
        {
            Succeeded = succeeded;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ConditionalValue"/> is succeeded.
        /// </summary>
        /// <value><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</value>
        public bool Succeeded { get; }
    }

    /// <summary>
    /// Represents the base class to support the Try-Parse pattern of an async operation.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the operation.</typeparam>
    /// <seealso cref="ConditionalValue" />
    /// <remarks>Try-Parse pattern: https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/exceptions-and-performance</remarks>
    public abstract class ConditionalValue<TResult> : ConditionalValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalValue{TResult}"/> class.
        /// </summary>
        /// <param name="succeeded">Indicates whether the operation was successfull (an instance of <typeparamref name="TResult"/> has been created) or not.</param>
        /// <param name="result">The value returned from the operation; otherwise the default value for <typeparamref name="TResult"/> of the <paramref name="result"/> parameter.</param>
        protected ConditionalValue(bool succeeded, TResult result) : base(succeeded)
        {
            Result = result;
        }

        /// <summary>
        /// Gets the result of the operation.
        /// </summary>
        /// <value>The result of the operation.</value>
        public TResult Result { get; }
    }
}
