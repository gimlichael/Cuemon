using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.Extensions;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics
{
    /// <summary>
    /// Specifies options that is related to <see cref="FaultDescriptorFilter" /> operations.
    /// </summary>
    /// <seealso cref="FaultDescriptorFilter" />
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
        ///         <term><see cref="ExceptionDescriptorHandler"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ExceptionDescriptorResolver"/></term>
        ///         <description>The default implementation iterates over <see cref="FaultResolvers"/> until a match is found from an <see cref="Exception"/>; then the associated <see cref="HttpExceptionDescriptor"/> is returned. If no match is found, a <see cref="HttpExceptionDescriptor"/> initialized to status 500 InternalServerError is returned</description>
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
        ///     <item>
        ///         <term><see cref="UseBaseException"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="RootHelpLink"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public FaultDescriptorOptions()
        {
            FaultResolvers
                .Add<ThrottlingException>()
                .Add<UserAgentException>()
                .Add<ValidationException>(StatusCodes.Status400BadRequest)
                .Add<FormatException>(StatusCodes.Status400BadRequest)
                .Add<ArgumentException>(StatusCodes.Status400BadRequest, exceptionValidator: ex => ex.GetType().HasTypes(typeof(ArgumentException)));
            ExceptionCallback = null;
            RequestBodyParser = null;
            ExceptionDescriptorResolver = e =>
            {
                if (e != null)
                {
                    foreach (var descriptor in FaultResolvers)
                    {
                        if (descriptor.Validator.Invoke(e)) { return descriptor.Descriptor.Invoke(e); }
                    }
                }
                return new HttpExceptionDescriptor(e, StatusCodes.Status500InternalServerError, message: FormattableString.Invariant($"An unhandled exception was raised by {Assembly.GetEntryAssembly()?.GetName().Name}."), helpLink: RootHelpLink);
            };
            ExceptionDescriptorHandler = null;
            UseBaseException = false;
        }

        /// <summary>
        /// Gets or sets the root link to a help file associated with an API.
        /// </summary>
        /// <value>The root link to a help file associated with an API.</value>
        public Uri RootHelpLink { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is initialized with <see cref="RootHelpLink"/>.
        /// </summary>
        /// <value><c>true</c> if this instance is initialized with <see cref="RootHelpLink"/>; otherwise, <c>false</c>.</value>
        public bool HasRootHelpLink => RootHelpLink != null;

        /// <summary>
        /// Gets or sets a value indicating whether to expose only the base exception that caused the faulted operation.
        /// </summary>
        /// <value><c>true</c> if only the base exception is exposed; otherwise, <c>false</c>, to include the entire exception tree.</value>
        public bool UseBaseException { get; set; }

        /// <summary>
        /// Gets a collection of <see cref="FaultResolver"/> that can ease the usage of <see cref="ExceptionDescriptorResolver"/>.
        /// </summary>
        /// <value>The collection of <see cref="FaultResolver"/>.</value>
        public IList<FaultResolver> FaultResolvers { get; } = new List<FaultResolver>();

        /// <summary>
        /// Gets or sets the delegate that provides a way to handle and customize an <see cref="HttpExceptionDescriptor"/>.
        /// In the default implementation, this is invoked just before the <see cref="ExceptionDescriptorResult"/> is assigned to <see cref="Microsoft.AspNetCore.Mvc.Filters.ExceptionContext.Result"/>.
        /// </summary>
        /// <value>The delegate that provides a way to handle and customize an <see cref="HttpExceptionDescriptor"/>.</value>
        public Action<Microsoft.AspNetCore.Mvc.Filters.ExceptionContext, HttpExceptionDescriptor> ExceptionDescriptorHandler { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that will resolve a <see cref="HttpExceptionDescriptor"/> from the specified <see cref="Exception"/>.
        /// </summary>
        /// <value>The function delegate that will resolve a <see cref="HttpExceptionDescriptor"/> from the specified <see cref="Exception"/>.</value>
        public Func<Exception, HttpExceptionDescriptor> ExceptionDescriptorResolver { get; set; }

        /// <summary>
        /// Gets or sets the callback delegate that is invoked when an exception has been thrown.
        /// </summary>
        /// <value>A <see cref="Action{T}"/>. The default value is <c>null</c>.</value>
        public Action<Exception, HttpExceptionDescriptor> ExceptionCallback { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to mark ASP.NET Core MVC <see cref="Microsoft.AspNetCore.Mvc.Filters.ExceptionContext.ExceptionHandled"/> to <c>true</c>.
        /// </summary>
        /// <value><c>true</c> if <see cref="Microsoft.AspNetCore.Mvc.Filters.ExceptionContext.ExceptionHandled"/> should be set; otherwise, <c>false</c>.</value>
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
    }
}