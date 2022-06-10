using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.AspNetCore.Http.Throttling;
using Cuemon.Threading;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Diagnostics
{
    /// <summary>
    /// Specifies options that is related to <see cref="ExceptionHandlerMiddleware" /> operations.
    /// </summary>
    public class FaultDescriptorExceptionHandlerOptions : AsyncOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaultDescriptorExceptionHandlerOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="FaultDescriptorExceptionHandlerOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="ExceptionDescriptorResolver"/></term>
        ///         <description>The default implementation iterates over <see cref="HttpFaultResolvers"/> until a match is found from an <see cref="Exception"/>; then the associated <see cref="HttpExceptionDescriptor"/> is returned. If no match is found, a <see cref="HttpExceptionDescriptor"/> initialized to status 500 InternalServerError is returned</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ExceptionCallback"/></term>
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
        public FaultDescriptorExceptionHandlerOptions()
        {
            Decorator.Enclose(HttpFaultResolvers)
                .AddHttpFaultResolver<BadRequestException>()
                .AddHttpFaultResolver<ConflictException>()
                .AddHttpFaultResolver<ForbiddenException>()
                .AddHttpFaultResolver<GoneException>()
                .AddHttpFaultResolver<NotFoundException>()
                .AddHttpFaultResolver<MethodNotAllowedException>()
                .AddHttpFaultResolver<NotAcceptableException>()
                .AddHttpFaultResolver<PayloadTooLargeException>()
                .AddHttpFaultResolver<PreconditionFailedException>()
                .AddHttpFaultResolver<PreconditionRequiredException>()
                .AddHttpFaultResolver<TooManyRequestsException>()
                .AddHttpFaultResolver<UnauthorizedException>()
                .AddHttpFaultResolver<UnsupportedMediaTypeException>()
                .AddHttpFaultResolver<ThrottlingException>()
                .AddHttpFaultResolver<UserAgentException>()
                .AddHttpFaultResolver<ValidationException>(StatusCodes.Status400BadRequest)
                .AddHttpFaultResolver<FormatException>(StatusCodes.Status400BadRequest)
                .AddHttpFaultResolver<ArgumentException>(StatusCodes.Status400BadRequest, exceptionValidator: ex => Decorator.Enclose(ex.GetType()).HasTypes(typeof(ArgumentException)));
            ExceptionCallback = null;
            ExceptionDescriptorResolver = e =>
            {
                if (e != null)
                {
                    foreach (var resolver in HttpFaultResolvers)
                    {
                        if (resolver.TryResolveFault(e, out var descriptor)) { return descriptor; }
                    }
                }
                return new HttpExceptionDescriptor(e, StatusCodes.Status500InternalServerError, message: FormattableString.Invariant($"An unhandled exception was raised by {Assembly.GetEntryAssembly()?.GetName().Name}."), helpLink: RootHelpLink);
            };
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
        /// Gets a collection of <see cref="HttpFaultResolver"/> that can ease the usage of <see cref="ExceptionDescriptorResolver"/>.
        /// </summary>
        /// <value>The collection of <see cref="HttpFaultResolver"/>.</value>
        public IList<HttpFaultResolver> HttpFaultResolvers { get; } = new List<HttpFaultResolver>();

        /// <summary>
        /// Gets or sets the function delegate that will resolve a <see cref="HttpExceptionDescriptor"/> from the specified <see cref="Exception"/>.
        /// </summary>
        /// <value>The function delegate that will resolve a <see cref="HttpExceptionDescriptor"/> from the specified <see cref="Exception"/>.</value>
        public Func<Exception, HttpExceptionDescriptor> ExceptionDescriptorResolver { get; set; }

        /// <summary>
        /// Gets or sets the callback delegate that is invoked when an exception has been thrown.
        /// </summary>
        /// <value>A <see cref="Action{T1,T2,T3}"/>. The default value is <c>null</c>.</value>
        public Action<HttpContext, Exception, HttpExceptionDescriptor> ExceptionCallback { get; set; }
    }
}