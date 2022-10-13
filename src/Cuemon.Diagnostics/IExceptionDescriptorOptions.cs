namespace Cuemon.Diagnostics
{
    /// <summary>
    /// Defines options that is related to <see cref="ExceptionDescriptor"/> operations.
    /// </summary>
    public interface IExceptionDescriptorOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="ExceptionDescriptor.Failure"/> property is included in the serialized result.
        /// </summary>
        /// <value><c>true</c> if the <see cref="ExceptionDescriptor.Failure"/> property is included in the serialized result; otherwise, <c>false</c>.</value>
        public bool IncludeFailure { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the stack of the <see cref="ExceptionDescriptor.Failure"/> property is included in the serialized result.
        /// </summary>
        /// <value><c>true</c> if the stack of the <see cref="ExceptionDescriptor.Failure"/> property is included in the serialized result; otherwise, <c>false</c>.</value>
        public bool IncludeStackTrace { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="ExceptionDescriptor.Evidence"/> property is included in the serialized result.
        /// </summary>
        /// <value><c>true</c> if the <see cref="ExceptionDescriptor.Evidence"/> property is included in the serialized result; otherwise, <c>false</c>.</value>
        public bool IncludeEvidence { get; set; }
    }
}
