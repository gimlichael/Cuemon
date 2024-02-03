using System;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.AspNetCore.Mvc.Formatters.Text
{
    /// <summary>
    /// Extension methods for the <see cref="IMvcBuilder"/> interface.
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Adds configuration of <see cref="ExceptionDescriptorOptions"/> for the application.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorOptions"/> which need to be configured.</param>
        /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="builder"/> cannot be null.
        /// </exception>
        public static IMvcBuilder AddExceptionDescriptorOptions(this IMvcBuilder builder, Action<ExceptionDescriptorOptions> setup = null)
        {
            Validator.ThrowIfNull(builder);
            builder.Services.AddExceptionDescriptorOptions(setup);
            return builder;
        }
    }
}
