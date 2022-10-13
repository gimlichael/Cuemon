using System;
using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Cuemon.Extensions.Asp.Versioning
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a compound service API versioning to the specified <paramref name="services"/> collection that is optimized for RESTful APIs.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="RestfulApiVersioningOptions"/> that may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>This is a convenient method to add API versioning to your ASP.NET Core WebApi. Call <c>AddApiVersioning</c>, <c>AddMvc</c> and <c>AddApiExplorer</c>. Configuration, which is optimized for RESTful APIs, are done through <paramref name="setup"/>.</remarks>
        public static IServiceCollection AddRestfulApiVersioning(this IServiceCollection services, Action<RestfulApiVersioningOptions> setup = null)
        {
            Validator.ThrowIfNull(services, nameof(services));

            var options = Patterns.Configure(setup);

            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = options.DefaultApiVersion;
                o.ReportApiVersions = options.ReportApiVersions;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ApiVersionReader = new RestfulApiVersionReader(options.ValidAcceptHeaders, options.ParameterName);
                o.ApiVersionSelector = (Activator.CreateInstance(options.ApiVersionSelectorType, o) as IApiVersionSelector)!;
            }).AddMvc(o =>
            {
                o.Conventions = options.Conventions;
            }).AddApiExplorer(o =>
            {
                o.GroupNameFormat = $"'{options.ParameterName}'VVV";
                o.DefaultApiVersion = options.DefaultApiVersion;
                o.SubstituteApiVersionInUrl = true;
            });

            if (options.ProblemDetailsFactoryType != null && options.ProblemDetailsFactoryType != typeof(DefaultProblemDetailsFactory)) { services.Replace(ServiceDescriptor.Singleton(_ => Activator.CreateInstance(options.ProblemDetailsFactoryType) as IProblemDetailsFactory)); }
            
            return services;
        }
    }
}
