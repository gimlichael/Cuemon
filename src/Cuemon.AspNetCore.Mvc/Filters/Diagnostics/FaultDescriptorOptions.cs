using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics
{
    /// <summary>
    /// Specifies options that is related to <see cref="FaultDescriptorFilter" /> operations.
    /// </summary>
    /// <seealso cref="FaultDescriptorFilter"/>.
    public class FaultDescriptorOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaultDescriptorOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="FaultDescriptorOptions"/>.
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
        ///     <item>
        ///         <term><see cref="IncludeRequest"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="RequestBodyParser"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public FaultDescriptorOptions()
        {
            HttpStatusCodeResolver = e =>
            {
                if (IsValidationException(e)) { return HttpStatusCode.BadRequest; }
                return HttpStatusCode.InternalServerError;
            };
            ExceptionCallback = null;
            RequestBodyParser = null;
            ExceptionDescriptorResolver = null;
            ExceptionDescriptorHandler = null;
        }

        /// <summary>
        /// Gets or sets the delegate that provides a way to handle and customize an <see cref="ExceptionDescriptor"/>.
        /// In the default implementation, this is invoked just before the <see cref="ExceptionDescriptorResult"/> is assigned to <see cref="ExceptionContext.Result"/>.
        /// </summary>
        /// <value>The delegate that provides a way to handle and customize an <see cref="ExceptionDescriptor"/>.</value>
        public Action<ExceptionContext, ExceptionDescriptor> ExceptionDescriptorHandler { get; set; }

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

        /// <summary>
        /// Gets or sets a value indicating whether to mark ASP.NET Core MVC <see cref="ExceptionContext.ExceptionHandled"/> to <c>true</c>.
        /// </summary>
        /// <value><c>true</c> if <see cref="ExceptionContext.ExceptionHandled"/> should be set; otherwise, <c>false</c>.</value>
        public bool MarkExceptionHandled  { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the request that caused the exception should be included as evidence.
        /// </summary>
        /// <value><c>true</c> if the request that caused the exception should be included as evidence; otherwise, <c>false</c>.</value>
        public bool IncludeRequest { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that, when <see cref="IncludeRequest"/> is <c>true</c>, will determines the string result of a HTTP request body.
        /// </summary>
        /// <value>The function delegate that determines the string result of a HTTP request body.</value>
        public Func<Stream, string> RequestBodyParser { get; set; }

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