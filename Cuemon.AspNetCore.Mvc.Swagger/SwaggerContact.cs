using System;
using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Contact information for the exposed API.
    /// </summary>
    public class SwaggerContact
    {
        /// <summary>
        /// Gets or sets the identifying name of the contact person/organization.
        /// </summary>
        /// <value>The identifying name of the contact person/organization.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL pointing to the contact information.
        /// </summary>
        /// <value>The URL pointing to the contact information.</value>
        public Uri Url { get; set; }

        /// <summary>
        /// Gets or sets the email address of the contact person/organization. MUST be in the format of an email address.
        /// </summary>
        /// <value>The email address of the contact person/organization. MUST be in the format of an email address.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets the extensions to the Swagger Schema.
        /// </summary>
        /// <value>The extensions to the Swagger Schema.</value>
        public IDictionary<SwaggerExtension, object> Extensions { get; } = new Dictionary<SwaggerExtension, object>();
    }
}