using System;
using System.Collections.Generic;
using Cuemon.AspNetCore.Http;

namespace Cuemon.Extensions.AspNetCore.Mvc.Filters.Diagnostics
{
    /// <summary>
    /// Extension methods for the <see cref="IList{FaultResolver}"/> interface.
    /// </summary>
    public static class FaultResolverListExtensions
    {
        /// <summary>
        /// Adds a new <see cref="HttpExceptionDescriptor"/> to the collection of <paramref name="descriptors"/> from the parameters provided.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="HttpStatusCodeException"/> to associate with a <see cref="FaultResolver"/>.</typeparam>
        /// <param name="descriptors">The collection to extend.</param>
        /// <param name="code">The error code that uniquely identifies the type of failure.</param>
        /// <param name="message">The message that explains the reason for the failure.</param>
        /// <param name="helpLink">The optional link to a help page associated with this failure.</param>
        /// <param name="exceptionValidator">The function delegate that evaluates an <see cref="Exception"/>.</param>
        /// <returns>The <see cref="IList{FaultResolver}"/> instance.</returns>
        /// <remarks>
        /// The following table shows the initial property values for the added instance of <see cref="HttpExceptionDescriptor"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Parameter</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><paramref name="code"/></term>
        ///         <description><c>code ?? ReasonPhrases.GetReasonPhrase(statusCode)</c></description>
        ///     </item>
        ///     <item>
        ///         <term><paramref name="message"/></term>
        ///         <description><c>message ?? failure.Message</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public static IList<FaultResolver> Add<T>(this IList<FaultResolver> descriptors, string code = null, string message = null, Uri helpLink = null, Func<Exception, bool> exceptionValidator = null) where T : HttpStatusCodeException
        {
            return Add<T>(descriptors, ex => new HttpExceptionDescriptor(ex, ex.StatusCode, code, message, helpLink), exceptionValidator);
        }

        /// <summary>
        /// Adds a new <see cref="HttpExceptionDescriptor"/> to the collection of <paramref name="descriptors"/> from the parameters provided.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Exception"/> to associate with a <see cref="FaultResolver"/>.</typeparam>
        /// <param name="descriptors">The collection to extend.</param>
        /// <param name="statusCode">The status code of the HTTP request.</param>
        /// <param name="code">The error code that uniquely identifies the type of failure.</param>
        /// <param name="message">The message that explains the reason for the failure.</param>
        /// <param name="helpLink">The optional link to a help page associated with this failure.</param>
        /// <param name="exceptionValidator">The function delegate that evaluates an <see cref="Exception"/>.</param>
        /// <returns>The <see cref="IList{FaultResolver}"/> instance.</returns>
        /// <remarks>
        /// The following table shows the initial property values for the added instance of <see cref="HttpExceptionDescriptor"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Parameter</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><paramref name="code"/></term>
        ///         <description><c>code ?? ReasonPhrases.GetReasonPhrase(statusCode)</c></description>
        ///     </item>
        ///     <item>
        ///         <term><paramref name="message"/></term>
        ///         <description><c>message ?? failure.Message</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public static IList<FaultResolver> Add<T>(this IList<FaultResolver> descriptors, int statusCode, string code = null, string message = null, Uri helpLink = null, Func<Exception, bool> exceptionValidator = null) where T : Exception
        {
            return Add<T>(descriptors, ex => new HttpExceptionDescriptor(ex, statusCode, code, message, helpLink), exceptionValidator);
        }

        /// <summary>
        /// Adds the specified function delegate <paramref name="exceptionDescriptorResolver"/> and function delegate <paramref name="exceptionValidator"/> to the collection of <paramref name="descriptors"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Exception"/> to associate with a <see cref="FaultResolver"/>.</typeparam>
        /// <param name="descriptors">The collection to extend.</param>
        /// <param name="exceptionDescriptorResolver">The function delegate that associates an <see cref="Exception"/> of type <typeparamref name="T"/> with an <see cref="HttpExceptionDescriptor"/>.</param>
        /// <param name="exceptionValidator">The function delegate that evaluates an <see cref="Exception"/>.</param>
        /// <returns>The <see cref="IList{FaultResolver}"/> instance.</returns>
        public static IList<FaultResolver> Add<T>(this IList<FaultResolver> descriptors, Func<T, HttpExceptionDescriptor> exceptionDescriptorResolver, Func<Exception, bool> exceptionValidator) where T : Exception
        {
            if (exceptionValidator == null) { exceptionValidator = ex => ex is T; }
            descriptors.Add(new FaultResolver(exceptionValidator, ex => exceptionDescriptorResolver((T)ex)));
            return descriptors;
        }
    }
}