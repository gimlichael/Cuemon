using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Net.Http
{
    /// <summary>
    /// Provides ways for sending HTTP requests and receiving HTTP responses from a resource identified by a URI. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public sealed class HttpManager : IDisposable
    {
        private volatile bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpManager"/> class.
        /// </summary>
        public HttpManager() 
            : this(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                MaxAutomaticRedirections = 10
            })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpManager"/> class.
        /// </summary>
        /// <param name="handler">The HTTP handler stack to use for sending requests.</param>
        public HttpManager(HttpMessageHandler handler) 
            : this(handler, new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Connection", "Keep-Alive" ) })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpManager"/> class.
        /// </summary>
        /// <param name="handler">The HTTP handler stack to use for sending requests.</param>
        /// <param name="defaultRequestHeaders">The default headers which should be sent with each request.</param>
        public HttpManager(HttpMessageHandler handler, IEnumerable<KeyValuePair<string, string>> defaultRequestHeaders) 
            : this(handler, defaultRequestHeaders, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpManager"/> class.
        /// </summary>
        /// <param name="handler">The HTTP handler stack to use for sending requests.</param>
        /// <param name="defaultRequestHeaders">The default headers which should be sent with each request.</param>
        /// <param name="disposeHandler"><c>true</c>if the inner handler should be disposed of by Dispose(), <c>false</c> if you intend to reuse the inner handler.</param>
        public HttpManager(HttpMessageHandler handler, IEnumerable<KeyValuePair<string, string>> defaultRequestHeaders, bool disposeHandler)
        {
            var client = new HttpClient(handler, disposeHandler);
            if (defaultRequestHeaders != null)
            {
                foreach (var header in defaultRequestHeaders)
                {
                    if (client.DefaultRequestHeaders.Contains(header.Key)) { continue; }
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            Client = client;
        }

        private HttpClient Client { get; set; }

        /// <summary>
        /// Gets the headers which should be sent with each request.
        /// </summary>
        /// <value>The headers which should be sent with each request.</value>
        public HttpRequestHeaders DefaultRequestHeaders
        {
            get { return Client.DefaultRequestHeaders; }
        }

        /// <summary>
        /// Gets or sets the timespan to wait before the request times out.
        /// </summary>
        /// <value>The timespan to wait before the request times out.</value>
        public TimeSpan Timeout
        {
            get
            {
                return Client.Timeout;
            }
            set
            {
                ValidateTimeout(value, nameof(value));
                Client.Timeout = value;
            }
        }

        /// <summary>
        /// Send a DELETE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> HttpDelete(Uri location)
        {
            return await HttpDelete(location, Client.Timeout);
        }

        /// <summary>
        /// Send a DELETE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> HttpDelete(Uri location, TimeSpan timeout)
        {
            return await Http(HttpMethod.Delete, location, timeout);
        }

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> HttpGet(Uri location)
        {
            return await HttpGet(location, Client.Timeout);
        }

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> HttpGet(Uri location, TimeSpan timeout)
        {
            return await Http(HttpMethod.Get, location, timeout);
        }

        /// <summary>
        /// Send a HEAD request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> HttpHead(Uri location)
        {
            return await HttpHead(location, Client.Timeout);
        }

        /// <summary>
        /// Send a HEAD request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> HttpHead(Uri location, TimeSpan timeout)
        {
            return await Http(HttpMethod.Head, location, timeout);
        }

        /// <summary>
        /// Send an OPTIONS request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> HttpOptions(Uri location)
        {
            return await HttpOptions(location, Client.Timeout);
        }

        /// <summary>
        /// Send an OPTIONS request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> HttpOptions(Uri location, TimeSpan timeout)
        {
            return await Http(HttpMethod.Options, location, timeout);
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> HttpPost(Uri location, string contentType, Stream content)
        {
            return await HttpPost(location, contentType, content, Client.Timeout);
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> HttpPost(Uri location, string contentType, Stream content, TimeSpan timeout)
        {
            return await Http(HttpMethod.Post, location, contentType, content, timeout);
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> HttpPost(Uri location, MediaTypeHeaderValue contentType, Stream content)
        {
            return await HttpPost(location, contentType, content, Client.Timeout);
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> HttpPost(Uri location, MediaTypeHeaderValue contentType, Stream content, TimeSpan timeout)
        {
            return await Http(HttpMethod.Post, location, contentType, content, timeout);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> HttpPut(Uri location, string contentType, Stream content)
        {
            return await HttpPut(location, contentType, content, Client.Timeout);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> HttpPut(Uri location, string contentType, Stream content, TimeSpan timeout)
        {
            return await Http(HttpMethod.Put, location, contentType, content, timeout);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> HttpPut(Uri location, MediaTypeHeaderValue contentType, Stream content)
        {
            return await HttpPut(location, contentType, content, Client.Timeout);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> HttpPut(Uri location, MediaTypeHeaderValue contentType, Stream content, TimeSpan timeout)
        {
            return await Http(HttpMethod.Put, location, contentType, content, timeout);
        }

        /// <summary>
        /// Send a TRACE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> HttpTrace(Uri location)
        {
            return await HttpTrace(location, Client.Timeout);
        }

        /// <summary>
        /// Send a TRACE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> HttpTrace(Uri location, TimeSpan timeout)
        {
            return await Http(HttpMethod.Trace, location, timeout);
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> Http(string method, Uri location)
        {
            return await Http(method, location, Client.Timeout);
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> Http(string method, Uri location, TimeSpan timeout)
        {
            Validator.ThrowIfNullOrEmpty(method, nameof(method));
            return await Http(new HttpMethod(method), location, timeout);
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> Http(string method, Uri location, string contentType, Stream content)
        {
            return await Http(method, location, contentType, content, Client.Timeout);
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> Http(string method, Uri location, string contentType, Stream content, TimeSpan timeout)
        {
            Validator.ThrowIfNullOrEmpty(method, nameof(method));
            return await Http(new HttpMethod(method), location, contentType, content, timeout);
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> Http(HttpMethod method, Uri location, string contentType, Stream content)
        {
            return await Http(method, location, contentType, content, Client.Timeout);
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> Http(HttpMethod method, Uri location, string contentType, Stream content, TimeSpan timeout)
        {
            Validator.ThrowIfNullOrEmpty(contentType, nameof(contentType));
            return await Http(method, location, MediaTypeHeaderValue.Parse(contentType), content, Client.Timeout);
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> Http(string method, Uri location, MediaTypeHeaderValue contentType, Stream content)
        {
            return await Http(method, location, contentType, content, Client.Timeout);
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> Http(string method, Uri location, MediaTypeHeaderValue contentType, Stream content, TimeSpan timeout)
        {
            Validator.ThrowIfNullOrEmpty(method, nameof(method));
            return await Http(new HttpMethod(method), location, contentType, content, timeout);
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> Http(HttpMethod method, Uri location, MediaTypeHeaderValue contentType, Stream content)
        {
            return await Http(method, location, contentType, content, Client.Timeout);
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> Http(HttpMethod method, Uri location, MediaTypeHeaderValue contentType, Stream content, TimeSpan timeout)
        {
            Validator.ThrowIfNull(method, nameof(method));
            Validator.ThrowIfNull(location, nameof(location));
            Validator.ThrowIfNull(contentType, nameof(contentType));
            Validator.ThrowIfNull(content, nameof(content));
            ValidateTimeout(timeout, nameof(timeout));
            HttpRequestMessage message = new HttpRequestMessage(method, location);
            message.Content.Headers.ContentType = contentType;
            message.Content = new StreamContent(content);
            return await Http(message, timeout);
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> Http(HttpMethod method, Uri location)
        {
            return await Http(method, location, Client.Timeout);
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> Http(HttpMethod method, Uri location, TimeSpan timeout)
        {
            Validator.ThrowIfNull(method, nameof(method));
            Validator.ThrowIfNull(location, nameof(location));
            HttpRequestMessage message = new HttpRequestMessage(method, location);
            return await Http(message, timeout);
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="message">The HTTP request message to send.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> Http(HttpRequestMessage message)
        {
            return await Http(message, Client.Timeout);
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="message">The HTTP request message to send.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> Http(HttpRequestMessage message, TimeSpan timeout)
        {
            ValidateTimeout(timeout, nameof(timeout));
            return await Http(message, new CancellationTokenSource(timeout));
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="message">The HTTP request message to send.</param>
        /// <param name="cts">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<HttpResponseMessage> Http(HttpRequestMessage message, CancellationTokenSource cts)
        {
            Validator.ThrowIfNull(message, nameof(message));
            Validator.ThrowIfNull(cts, nameof(cts));
            return await Client.SendAsync(message, ParseCompletionOption(message.Method), cts.Token);
        }

        private HttpCompletionOption ParseCompletionOption(HttpMethod method)
        {
            return (method == HttpMethod.Head || method == HttpMethod.Trace)
                ? HttpCompletionOption.ResponseHeadersRead
                : HttpCompletionOption.ResponseContentRead;
        }

        private static void ValidateTimeout(TimeSpan timeout, string paramName)
        {
            Validator.ThrowIfNull(timeout, paramName);
            Validator.ThrowIfLowerThanOrEqual(timeout.Milliseconds, -1, paramName);
            Validator.ThrowIfGreaterThan(timeout.Milliseconds, int.MaxValue, paramName);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (_isDisposed || !disposing) { return; }
            _isDisposed = true;
            if (Client != null) { Client.Dispose(); }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
    }
}