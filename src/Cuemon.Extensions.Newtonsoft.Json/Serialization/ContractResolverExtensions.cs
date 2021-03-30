using System.Linq;
using Newtonsoft.Json.Serialization;

namespace Cuemon.Extensions.Newtonsoft.Json.Serialization
{
    /// <summary>
    /// Extension methods for the <see cref="IContractResolver"/> interface.
    /// </summary>
    public static class ContractResolverExtensions
    {
        /// <summary>
        /// Resolves the <see cref="NamingStrategy"/> from the specified <paramref name="contractResolver"/> when possible; otherwise a default <see cref="NamingStrategy"/> implementation is provided based on context.
        /// Eg. for <see cref="DefaultContractResolver"/> an instance of <see cref="DefaultNamingStrategy"/> is returned; for all other cases an instance of <see cref="CamelCaseNamingStrategy"/> is returned.
        /// </summary>
        /// <param name="contractResolver">The <see cref="IContractResolver"/> to resolve a <see cref="NamingStrategy"/> from.</param>
        /// <returns>An instance of <see cref="NamingStrategy"/>.</returns>
        public static NamingStrategy ResolveNamingStrategyOrDefault(this IContractResolver contractResolver)
        {
            if (contractResolver is DefaultContractResolver dcr) // odd that IContractResolver does not implement - this make this method vulnerable to custom implementations of IContractResolver
            {
                return dcr.NamingStrategy ?? new DefaultNamingStrategy(); // stay true to default in library (pass-through)
            }

            var namingStrategyProperty = contractResolver?.GetType().GetProperties().FirstOrDefault(pi => pi.PropertyType == typeof(NamingStrategy));
            var namingStrategy = namingStrategyProperty?.GetValue(contractResolver) as NamingStrategy;
            return namingStrategy ?? new CamelCaseNamingStrategy();
        }
    }
}