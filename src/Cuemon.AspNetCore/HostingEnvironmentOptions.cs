using System;
using Microsoft.Extensions.Hosting;

namespace Cuemon.AspNetCore
{
    /// <summary>
    /// Configuration options for <see cref="HostingEnvironmentMiddleware"/>.
    /// </summary>
    public class HostingEnvironmentOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HostingEnvironmentOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="HostingEnvironmentOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="HeaderName"/></term>
        ///         <description><c>X-Hosting-Environment</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SuppressHeaderPredicate"/></term>
        ///         <description><c>_ => false</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public HostingEnvironmentOptions()
        {
            HeaderName = "X-Hosting-Environment";
            SuppressHeaderPredicate = _ => false;
        }

        /// <summary>
        /// Gets or sets the name of the hosting environment HTTP header.
        /// </summary>
        /// <value>The name of the hosting environment HTTP header.</value>
        public string HeaderName { get; set; }

        /// <summary>
        /// Gets or sets the predicate that can suppress the hosting environment HTTP header.
        /// </summary>
        /// <value>The function delegate that can determine if the hosting environment HTTP header should be suppressed.</value>
        public Func<IHostingEnvironment, bool> SuppressHeaderPredicate { get; set; }
    }
}