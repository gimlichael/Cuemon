using Cuemon.Text.Yaml;

namespace Cuemon.Serialization.Converters
{
    /// <summary>
    /// Extension methods for the <see cref="YamlNamingPolicy"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class YamlNamingPolicyDecoratorExtensions
    {
        /// <summary>
        /// Returns the specified <paramref name="name"/> following the underlying <see cref="YamlNamingPolicy"/> of the <paramref name="decorator"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="name">The name to apply to a YAML property.</param>
        /// <returns>When the underlying <see cref="YamlNamingPolicy"/> of the <paramref name="decorator"/> is null, the specified <paramref name="name"/> is returned unaltered; otherwise it is converted according to the <see cref="YamlNamingPolicy"/>.</returns>
        public static string DefaultOrConvertName(this IDecorator<YamlNamingPolicy> decorator, string name)
        {
            var policy = decorator?.Inner;
            return policy == null ? name : policy.ConvertName(name);
        }
    }
}
