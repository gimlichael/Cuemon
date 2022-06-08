using System;
using System.IO;
using Cuemon.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics
{
    /// <summary>
    /// Specifies options that is related to <see cref="FaultDescriptorFilter" /> operations.
    /// </summary>
    /// <seealso cref="FaultDescriptorExceptionHandlerOptions" />
    public class FaultDescriptorOptions : FaultDescriptorExceptionHandlerOptions
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
        ///         <term><see cref="ExceptionDescriptorHandler"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="MarkExceptionHandled"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="FaultDescriptorExceptionHandlerOptions.ExceptionDescriptorResolver"/></term>
        ///         <description>The default implementation iterates over <see cref="FaultDescriptorExceptionHandlerOptions.HttpFaultResolvers"/> until a match is found from an <see cref="Exception"/>; then the associated <see cref="HttpExceptionDescriptor"/> is returned. If no match is found, a <see cref="HttpExceptionDescriptor"/> initialized to status 500 InternalServerError is returned</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="FaultDescriptorExceptionHandlerOptions.ExceptionCallback"/></term>
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
        ///     <item>
        ///         <term><see cref="FaultDescriptorExceptionHandlerOptions.UseBaseException"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="FaultDescriptorExceptionHandlerOptions.RootHelpLink"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public FaultDescriptorOptions()
        {
            MarkExceptionHandled = false;
            IncludeRequest = false;
            RequestBodyParser = null;
            ExceptionDescriptorHandler = null;
        }

        /// <summary>
        /// Gets or sets the delegate that provides a way to handle and customize an <see cref="HttpExceptionDescriptor"/>.
        /// In the default implementation, this is invoked just before the <see cref="ExceptionDescriptorResult"/> is assigned to <see cref="Microsoft.AspNetCore.Mvc.Filters.ExceptionContext.Result"/>.
        /// </summary>
        /// <value>The delegate that provides a way to handle and customize an <see cref="HttpExceptionDescriptor"/>.</value>
        public Action<ExceptionContext, HttpExceptionDescriptor> ExceptionDescriptorHandler { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that, when <see cref="IncludeRequest"/> is <c>true</c>, will determines the string result of a HTTP request body.
        /// </summary>
        /// <value>The function delegate that determines the string result of a HTTP request body.</value>
        public Func<Stream, string> RequestBodyParser { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the request that caused the exception should be included as evidence.
        /// </summary>
        /// <value><c>true</c> if the request that caused the exception should be included as evidence; otherwise, <c>false</c>.</value>
        public bool IncludeRequest { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to mark ASP.NET Core MVC <see cref="ExceptionContext.ExceptionHandled"/> to <c>true</c>.
        /// </summary>
        /// <value><c>true</c> if <see cref="ExceptionContext.ExceptionHandled"/> should be set; otherwise, <c>false</c>.</value>
        public bool MarkExceptionHandled  { get; set; }
    }
}