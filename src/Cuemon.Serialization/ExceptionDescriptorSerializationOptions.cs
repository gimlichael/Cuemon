using Cuemon.Diagnostics;

namespace Cuemon.Serialization
{
    /// <summary>
    /// Specifies options that is related to <see cref="ExceptionDescriptor"/> operations.
    /// </summary>
    public class ExceptionDescriptorSerializationOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDescriptorSerializationOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="ExceptionDescriptorSerializationOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="IncludeFailure"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="IncludeEvidence"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public ExceptionDescriptorSerializationOptions()
        {
            IncludeFailure = true;
            IncludeEvidence = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="ExceptionDescriptor.Failure"/> property is included in the serialized result.
        /// </summary>
        /// <value><c>true</c> if the <see cref="ExceptionDescriptor.Failure"/> property is included in the serialized result; otherwise, <c>false</c>.</value>
        public bool IncludeFailure { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="ExceptionDescriptor.Evidence"/> property is included in the serialized result.
        /// </summary>
        /// <value><c>true</c> if the <see cref="ExceptionDescriptor.Evidence"/> property is included in the serialized result; otherwise, <c>false</c>.</value>
        public bool IncludeEvidence { get; set; }
    }
}