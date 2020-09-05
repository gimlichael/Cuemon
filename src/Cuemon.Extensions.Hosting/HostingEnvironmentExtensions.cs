using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Hosting
{
    #if NETSTANDARD
    /// <summary>
    /// Extension methods for the <see cref="IHostingEnvironment"/> interface.
    /// </summary>
    public static class HostingEnvironmentExtensions
    {
        /// <summary>
        /// Determines whether the specified <paramref name="environment"/> is equal to <c>LocalDevelopment</c>.
        /// </summary>
        /// <param name="environment">The <see cref="IHostingEnvironment"/> to extend.</param>
        /// <returns><c>true</c> if <paramref name="environment"/> is <c>LocalDevelopment</c>; otherwise <c>false</c></returns>
        public static bool IsLocalDevelopment(this IHostingEnvironment environment)
        {
            return environment.IsEnvironment("LocalDevelopment");
        }

        /// <summary>
        /// Determines whether the specified <paramref name="environment"/> is different from <c>Production</c>.
        /// </summary>
        /// <param name="environment">The <see cref="IHostingEnvironment"/> to extend.</param>
        /// <returns><c>true</c> if <paramref name="environment"/> is not <c>Production</c>; otherwise <c>false</c></returns>
        public static bool IsNonProduction(this IHostingEnvironment environment)
        {
            return !environment.IsProduction();
        }
    }
    #endif
}