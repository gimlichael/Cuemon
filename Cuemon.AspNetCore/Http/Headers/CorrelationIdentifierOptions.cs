using System;
using Cuemon.Messaging;

namespace Cuemon.AspNetCore.Http.Headers
{
    /// <summary>
    /// Configuration options for <see cref="CorrelationIdentifierMiddleware"/>.
    /// </summary>
    public class CorrelationIdentifierOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CorrelationIdentifierOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="CorrelationIdentifierOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="HeaderName"/></term>
        ///         <description><c>X-Correlation-ID</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="CorrelationProvider"/></term>
        ///         <description><c>DynamicCorrelation.Create(Guid.NewGuid().ToString("N")</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public CorrelationIdentifierOptions()
        {
            HeaderName = "X-Correlation-ID";
            CorrelationProvider = () => DynamicCorrelation.Create(Guid.NewGuid().ToString("N"));
        }

        /// <summary>
        /// Gets or sets the name of the correlation identifier HTTP header.
        /// </summary>
        /// <value>The name of the correlation identifier HTTP header.</value>
        public string HeaderName { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that provides the correlation implementation.
        /// </summary>
        /// <value>The function delegate that provides the correlation implementation.</value>
        public Func<ICorrelation> CorrelationProvider { get; set; }
    }
}