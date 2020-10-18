using Microsoft.AspNetCore.Http;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http.Features
{
    /// <summary>
    /// Configuration options for <see cref="FakeHttpResponseFeature"/>.
    /// </summary>
    public class FakeHttpResponseOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpResponseOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="FakeHttpResponseOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="ShortCircuitOnStarting"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public FakeHttpResponseOptions()
        {
            StatusCode = StatusCodes.Status200OK;
            ShortCircuitOnStarting = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="FakeHttpResponseFeature.OnStarting"/> is invoked immediately upon initialization.
        /// </summary>
        /// <value><c>true</c> if <see cref="FakeHttpResponseFeature.OnStarting"/> is invoked immediately upon initialization; otherwise, <c>false</c>.</value>
        public bool ShortCircuitOnStarting { get; set; }

        /// <summary>
        /// Gets or sets the default status code to set in the feature response.
        /// </summary>
        /// <value>The default status code to set in the feature response.</value>
        public int StatusCode { get; set; }
    }
}