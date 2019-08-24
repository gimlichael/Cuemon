using System;
using System.Collections.Generic;
using Cuemon.Collections.Generic;

namespace Cuemon.ComponentModel.Parsers
{
    /// <summary>
    /// Configuration options for <see cref="UriParser"/>.
    /// </summary>
    public class UriOptions
    {
        /// <summary>
        /// Gets all supported URI schemes.
        /// </summary>
        /// <returns>A sequence of all supported URI schemes.</returns>
        public static IEnumerable<UriScheme> AllUriSchemes => Arguments.FromParams(UriScheme.File, UriScheme.Ftp, UriScheme.Gopher, UriScheme.Http, UriScheme.Https, UriScheme.Mailto, UriScheme.NetPipe, UriScheme.NetTcp, UriScheme.News, UriScheme.Nntp);

        /// <summary>
        /// Initializes a new instance of the <see cref="UriOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="UriOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Kind"/></term>
        ///         <description><see cref="UriKind.Absolute"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Schemes"/></term>
        ///         <description><see cref="AllUriSchemes"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public UriOptions()
        {
            Kind = UriKind.Absolute;
            Schemes = new List<UriScheme>(AllUriSchemes);
        }

        /// <summary>
        /// Gets or sets the kind of the URI.
        /// </summary>
        /// <value>The kind of the URI.</value>
        public UriKind Kind { get; set; }

        /// <summary>
        /// Gets a collection of <see cref="UriScheme"/> values that determines the outcome when parsing a URI.
        /// </summary>
        /// <value>The <see cref="UriScheme"/> values that determines the outcome when parsing a URI.</value>
        public IList<UriScheme> Schemes { get; }
    }
}