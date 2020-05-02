namespace Cuemon
{
    /// <summary>
    /// Defines a decorator that exposes the inner decorated type.
    /// </summary>
    /// <typeparam name="T">The type of the inner decorated object. This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived.</typeparam>
    /// <seealso cref="Decorator"/>
    /// <seealso cref="Decorator{T}"/>
    public interface IDecorator<out T>
    {
        /// <summary>
        /// Gets the inner object of this decorator.
        /// </summary>
        /// <value>The inner object of this decorator.</value>
        T Inner { get; }
    }
}