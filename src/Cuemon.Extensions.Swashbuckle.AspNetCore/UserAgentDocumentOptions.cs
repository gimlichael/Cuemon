using Cuemon.Configuration;

namespace Cuemon.Extensions.Swashbuckle.AspNetCore
{
    /// <summary>
    /// Provides programmatic configuration for the <see cref="UserAgentDocumentFilter"/> class.
    /// </summary>
    public class UserAgentDocumentOptions : IParameterObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgentDocumentOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="UserAgentDocumentOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Description"/></term>
        ///         <description><c>The identifier of the calling client.</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Example"/></term>
        ///         <description><c>Your-Awesome-Client/1.0.0</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Required"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public UserAgentDocumentOptions()
        {
            Description = "The identifier of the calling client.";
            Example = "Your-Awesome-Client/1.0.0";
            Required = false;
        }

        /// <summary>
        /// Gets or sets the description of the User-Agent field.
        /// </summary>
        /// <value>The description of the User-Agent field.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the example to associate with the User-Agent field.
        /// </summary>
        /// <value>The example to associate with the User-Agent field.</value>
        public string Example { get; set; }

        /// <summary>
        /// Gets or sets whether the User-Agent field is mandatory.
        /// </summary>
        /// <value><c>true</c> if the User-Agent field is mandatory; otherwise, <c>false</c>.</value>
        public bool Required { get; set; }
    }
}
