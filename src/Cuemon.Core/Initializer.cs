namespace Cuemon
{
    /// <summary>
    /// Provides a generic way to wrap and initialize a class for countless scenarios.
    /// </summary>
    public static class Initializer
    {
        /// <summary>
        /// Creates a new instance of the <see cref="InitializerBuilder{T}"/> wrapping the specified <paramref name="instance"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to wrap.</typeparam>
        /// <param name="instance">The instance to initialize within a protective wrapping.</param>
        /// <returns>A new instance of <see cref="InitializerBuilder{T}"/> with the specified <paramref name="instance"/> wrapped.</returns>
        public static InitializerBuilder<T> Create<T>(T instance) where T : class
        {
            return new InitializerBuilder<T>(instance);
        }
    }
}