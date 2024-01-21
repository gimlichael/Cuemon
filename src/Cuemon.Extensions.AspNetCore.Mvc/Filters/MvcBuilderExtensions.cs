using System;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.AspNetCore.Mvc.Filters.Cacheable;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.AspNetCore.Mvc.Filters
{
	/// <summary>
	/// Extension methods for the <see cref="IMvcBuilder"/> interface.
	/// </summary>
	public static class MvcBuilderExtensions
	{
		/// <summary>
		/// Registers the specified <paramref name="setup" /> to configure <see cref="ApiKeySentinelOptions"/> in the underlying service collection of <paramref name="builder" />.
		/// </summary>
		/// <param name="builder">The <see cref="IMvcBuilder"/> to extend.</param>
		/// <param name="setup">The <see cref="ApiKeySentinelOptions"/> which need to be configured.</param>
		/// <returns>A reference to <paramref name="builder" /> so that additional configuration calls can be chained.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="builder"/> cannot be null.
		/// <paramref name="setup"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="setup"/> failed to configure an instance of <see cref="ApiKeySentinelOptions"/> in a valid state.
		/// </exception>
		public static IMvcBuilder AddApiKeySentinelOptions(this IMvcBuilder builder, Action<ApiKeySentinelOptions> setup)
		{
			Validator.ThrowIfNull(builder);
			Validator.ThrowIfNull(setup);
			Validator.ThrowIfInvalidConfigurator(setup, out _);
			builder.Services.Configure(setup);
			return builder;
		}

		/// <summary>
		/// Registers the specified <paramref name="setup" /> to configure <see cref="ThrottlingSentinelOptions"/> in the underlying service collection of <paramref name="builder" />.
		/// </summary>
		/// <param name="builder">The <see cref="IMvcBuilder"/> to extend.</param>
		/// <param name="setup">The <see cref="ThrottlingSentinelOptions"/> which need to be configured.</param>
		/// <returns>A reference to <paramref name="builder" /> so that additional configuration calls can be chained.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="builder"/> cannot be null.
		/// <paramref name="setup"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="setup"/> failed to configure an instance of <see cref="ThrottlingSentinelOptions"/> in a valid state.
		/// </exception>
		public static IMvcBuilder AddThrottlingSentinelOptions(this IMvcBuilder builder, Action<ThrottlingSentinelOptions> setup)
		{
			Validator.ThrowIfNull(builder);
			Validator.ThrowIfNull(setup);
			Validator.ThrowIfInvalidConfigurator(setup, out _);
			builder.Services.Configure(setup);
			return builder;
		}

		/// <summary>
		/// Registers the specified <paramref name="setup" /> to configure <see cref="UserAgentSentinelOptions"/> in the underlying service collection of <paramref name="builder" />.
		/// </summary>
		/// <param name="builder">The <see cref="IMvcBuilder"/> to extend.</param>
		/// <param name="setup">The <see cref="UserAgentSentinelOptions"/> which need to be configured.</param>
		/// <returns>A reference to <paramref name="builder" /> so that additional configuration calls can be chained.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="builder"/> cannot be null.
		/// <paramref name="setup"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="setup"/> failed to configure an instance of <see cref="UserAgentSentinelOptions"/> in a valid state.
		/// </exception>
		public static IMvcBuilder AddUserAgentSentinelOptions(this IMvcBuilder builder, Action<UserAgentSentinelOptions> setup)
		{
			Validator.ThrowIfNull(builder);
			Validator.ThrowIfNull(setup);
			Validator.ThrowIfInvalidConfigurator(setup, out _);
			builder.Services.Configure(setup);
			return builder;
		}

		/// <summary>
		/// Registers the specified <paramref name="setup" /> to configure <see cref="ServerTimingOptions"/> in the underlying service collection of <paramref name="builder" />.
		/// </summary>
		/// <param name="builder">The <see cref="IMvcBuilder"/> to extend.</param>
		/// <param name="setup">The <see cref="ServerTimingOptions"/> which need to be configured.</param>
		/// <returns>A reference to <paramref name="builder" /> so that additional configuration calls can be chained.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="builder"/> cannot be null.
		/// <paramref name="setup"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="setup"/> failed to configure an instance of <see cref="ServerTimingOptions"/> in a valid state.
		/// </exception>
		public static IMvcBuilder AddServerTimingOptions(this IMvcBuilder builder, Action<ServerTimingOptions> setup)
		{
			Validator.ThrowIfNull(builder);
			Validator.ThrowIfNull(setup);
			Validator.ThrowIfInvalidConfigurator(setup, out _);
			builder.Services.Configure(setup);
			return builder;
		}

		/// <summary>
		/// Registers the specified <paramref name="setup" /> to configure <see cref="MvcFaultDescriptorOptions"/> in the underlying service collection of <paramref name="builder" />.
		/// </summary>
		/// <param name="builder">The <see cref="IMvcBuilder"/> to extend.</param>
		/// <param name="setup">The <see cref="MvcFaultDescriptorOptions"/> which need to be configured.</param>
		/// <returns>A reference to <paramref name="builder" /> so that additional configuration calls can be chained.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="builder"/> cannot be null.
		/// <paramref name="setup"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="setup"/> failed to configure an instance of <see cref="MvcFaultDescriptorOptions"/> in a valid state.
		/// </exception>
		public static IMvcBuilder AddMvcFaultDescriptorOptions(this IMvcBuilder builder, Action<MvcFaultDescriptorOptions> setup)
		{
			Validator.ThrowIfNull(builder);
			Validator.ThrowIfNull(setup);
			Validator.ThrowIfInvalidConfigurator(setup, out _);
			builder.Services.Configure(setup);
			return builder;
		}

		/// <summary>
		/// Registers the specified <paramref name="setup" /> to configure <see cref="HttpCacheableOptions"/> in the underlying service collection of <paramref name="builder" />.
		/// </summary>
		/// <param name="builder">The <see cref="IMvcBuilder"/> to extend.</param>
		/// <param name="setup">The <see cref="HttpCacheableOptions"/> which need to be configured.</param>
		/// <returns>A reference to <paramref name="builder" /> so that additional configuration calls can be chained.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="builder"/> cannot be null.
		/// <paramref name="setup"/> cannot be null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="setup"/> failed to configure an instance of <see cref="HttpCacheableOptions"/> in a valid state.
		/// </exception>
		public static IMvcBuilder AddHttpCacheableOptions(this IMvcBuilder builder, Action<HttpCacheableOptions> setup)
		{
			Validator.ThrowIfNull(builder);
			Validator.ThrowIfNull(setup);
			Validator.ThrowIfInvalidConfigurator(setup, out _);
			builder.Services.Configure(setup);
			return builder;
		}
	}
}
