using System;
using Cuemon.Configuration;

namespace Cuemon.Extensions.Xunit
{
    /// <summary>
    /// Configuration options for <see cref="Test.Match"/>.
    /// </summary>
    public class WildcardOptions : IParameterObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WildcardOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="WildcardOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="GroupOfCharacters"/></term>
        ///         <description><c>\\*</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="SingleCharacter"/></term>
        ///         <description><c>\\?</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public WildcardOptions()
        {
            GroupOfCharacters = "\\*";
            SingleCharacter = "\\?";
            ThrowOnNoMatch = false;
        }

        /// <summary>
        /// Gets or sets the symbol used to represents any group of characters, including no character. The default value is <c>\\*</c>.
        /// </summary>
        /// <value>The symbol used to represents any group of characters, including no character.</value>
        public string GroupOfCharacters { get; set; }

        /// <summary>
        /// Gets or sets the symbol used to represents any single character. The default value is <c>\\?</c>.
        /// </summary>
        /// <value>The symbol used to  represents any single character.</value>
        public string SingleCharacter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a <see cref="ArgumentOutOfRangeException"/> is thrown when no match is found. The default value is <c>false</c>.
        /// </summary>
        /// <value><c>true</c> if a <see cref="ArgumentOutOfRangeException"/> is thrown when no match is found; otherwise, <c>false</c>.</value>
        public bool ThrowOnNoMatch { get; set; }
    }
}
