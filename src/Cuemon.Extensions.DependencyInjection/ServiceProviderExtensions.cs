using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceProvider"/> interface.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Gets an enumeration of ALL <see cref="ServiceDescriptor"/> instances from the specified <paramref name="provider"/>.
        /// </summary>
        /// <param name="provider">The <see cref="IServiceProvider"/> to extend.</param>
        /// <returns>An enumeration of ALL <see cref="ServiceDescriptor"/> instances from the specified <paramref name="provider"/>.</returns>
        /// <exception cref="Cuemon.Validator"></exception>
        /// <exception cref="System.NotSupportedException">This method does not support {providerType.FullName}.</exception>
        public static IEnumerable<ServiceDescriptor> GetServiceDescriptors(this IServiceProvider provider)
        {
            Validator.ThrowIfNull(provider);
            var providerType = provider.GetType();

            if (TryLocateEmbeddedServiceProvider(provider, providerType, out var embeddedProvider))
            {
                provider = embeddedProvider.ServiceProvider;
                providerType = embeddedProvider.ProviderType;
            }
            
            var callSiteFactory = Decorator.Enclose(providerType).GetAllProperties().SingleOrDefault(pi => pi.Name == "CallSiteFactory")?.GetValue(provider);
            if (callSiteFactory != null)
            {
                var callSiteFactoryType = callSiteFactory.GetType();
                return Decorator.Enclose(callSiteFactoryType).GetAllProperties().SingleOrDefault(pi => pi.Name == "Descriptors")?.GetValue(callSiteFactory) as IEnumerable<ServiceDescriptor>;
            }

            throw new NotSupportedException($"This method does not support {providerType.FullName}.");
        }

        private static bool TryLocateEmbeddedServiceProvider(IServiceProvider originatingProvider, Type originatingProviderType, out (IServiceProvider ServiceProvider, Type ProviderType) embeddedProvider)
        {
            if (originatingProviderType.Name == "ServiceProviderEngineScope" && 
                Decorator.Enclose(originatingProviderType).GetAllProperties().SingleOrDefault(pi => pi.Name == "RootProvider")?.GetValue(originatingProvider) is IServiceProvider rootProvider)
            {
                embeddedProvider = new ValueTuple<IServiceProvider, Type>(rootProvider, rootProvider.GetType());
                return true;
            }
            embeddedProvider = default;
            return false;
        }
    }
}
