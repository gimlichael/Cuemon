using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Cuemon.Diagnostics;

namespace Cuemon.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// Specifies options that is related to <see cref="ExceptionDescriptorFilter" /> operations.
    /// </summary>
    /// <seealso cref="ExceptionDescriptorFilter"/>.
    public class ExceptionDescriptorFilterOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDescriptorFilterOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="ExceptionDescriptorFilterOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="HttpStatusCodeResolver"/></term>
        ///         <description>if an exception inherits from <see cref="ArgumentException"/>, is of type <see cref="ValidationException"/> or <see cref="FormatException"/>, a <see cref="HttpStatusCode.BadRequest"/> is returned; otherwise <see cref="HttpStatusCode.InternalServerError"/>.</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ExceptionDescriptorResolver"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ExceptionCallback"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public ExceptionDescriptorFilterOptions()
        {
            HttpStatusCodeResolver = e =>
            {
                if (IsValidationException(e)) { return HttpStatusCode.BadRequest; }
                return HttpStatusCode.InternalServerError;
            };
            ExceptionDescriptorResolver = null;
            ExceptionCallback = null;
        }

        /// <summary>
        /// Gets or sets the function delegate that will resolve a <see cref="ExceptionDescriptor"/> from the specified <see cref="Exception"/>.
        /// </summary>
        /// <value>The function delegate that will resolve a <see cref="ExceptionDescriptor"/> from the specified <see cref="Exception"/>.</value>
        public Func<Exception, ExceptionDescriptor> ExceptionDescriptorResolver { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that will resolve a <see cref="HttpStatusCode"/> from the specified <see cref="Exception"/>.
        /// </summary>
        /// <value>The function delegate that will resolve a <see cref="HttpStatusCode"/> from the specified <see cref="Exception"/>.</value>
        public Func<Exception, HttpStatusCode> HttpStatusCodeResolver { get; set; }

        /// <summary>
        /// Gets or sets the callback delegate that is invoked when an exception has been thrown.
        /// </summary>
        /// <value>A <see cref="Action{T}"/>. The default value is <c>null</c>.</value>
        public Action<Exception, ExceptionDescriptor> ExceptionCallback { get; set; }

        private static bool IsValidationException(Exception exception)
        {
            if (exception == null) { return false; }
            bool match = false;
            match |= exception.GetType().HasTypes(typeof(ArgumentException));
            match |= exception.Is<ValidationException>();
            match |= exception.Is<FormatException>();
            return match;
        }
    }
}