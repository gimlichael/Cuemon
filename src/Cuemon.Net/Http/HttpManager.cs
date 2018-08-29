using System;
using System.IO;
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
        private readonly Lazy<HttpClient> _httpClient;
        private const string HttpPatchVerb = "PATCH";

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpManager"/> class.
        /// </summary>
        public HttpManager(Action<HttpManagerOptions> setup = null)
        {
            var options = setup.ConfigureOptions();
            _httpClient = new Lazy<HttpClient>(() =>
            {
                var client = new HttpClient(options.Handler, options.DisposeHandler);
                foreach (var header in options.DefaultRequestHeaders)
                {
                    if (client.DefaultRequestHeaders.Contains(header.Key)) { continue; }
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                return client;
            });
        }

        private HttpClient Client => _httpClient.Value;

        /// <summary>
        /// Gets the headers which should be sent with each request.
        /// </summary>
        /// <value>The headers which should be sent with each request.</value>
        public HttpRequestHeaders DefaultRequestHeaders => Client.DefaultRequestHeaders;

        /// <summary>
        /// Gets or sets the timespan to wait before the request times out.
        /// </summary>
        /// <value>The timespan to wait before the request times out.</value>
        public TimeSpan Timeout
        {
            get => Client.Timeout;
            set
            {
                ValidateTimeout(value, nameof(value));
                Client.Timeout = value;
            }
        }

        /// <summary>
        /// Send a DELETE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> HttpDeleteAsync(Uri location, CancellationToken ct = default(CancellationToken))
        {
            return HttpAsync(location, o =>
            {
                o.Request.Method = HttpMethod.Delete;
                o.CancellationToken = ct;
            });
        }

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> HttpGetAsync(Uri location, CancellationToken ct = default(CancellationToken))
        {
            return HttpAsync(location, o =>
            {
                o.Request.Method = HttpMethod.Get;
                o.CancellationToken = ct;
            });
        }

        /// <summary>
        /// Send a HEAD request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> HttpHeadAsync(Uri location, CancellationToken ct = default(CancellationToken))
        {
            return HttpAsync(location, o =>
            {
                o.Request.Method = HttpMethod.Head;
                o.CancellationToken = ct;
            });
        }

        /// <summary>
        /// Send an OPTIONS request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> HttpOptionsAsync(Uri location, CancellationToken ct = default(CancellationToken))
        {
            return HttpAsync(location, o =>
            {
                o.Request.Method = HttpMethod.Options;
                o.CancellationToken = ct;
            });
        }

        /// <summary>
        /// Send a TRACE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> HttpTraceAsync(Uri location, CancellationToken ct = default(CancellationToken))
        {
            return HttpAsync(location, o =>
            {
                o.Request.Method = HttpMethod.Trace;
                o.CancellationToken = ct;
            });
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> HttpPostAsync(Uri location, string contentType, Stream content, CancellationToken ct = default(CancellationToken))
        {
            return HttpAsync(HttpMethod.Post, location, contentType, content, ct);
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> HttpPostAsync(Uri location, MediaTypeHeaderValue contentType, Stream content, CancellationToken ct = default(CancellationToken))
        {
            return HttpAsync(HttpMethod.Post, location, contentType, content, ct);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> HttpPutAsync(Uri location, string contentType, Stream content, CancellationToken ct = default(CancellationToken))
        {
            return HttpAsync(HttpMethod.Put, location, contentType, content, ct);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> HttpPutAsync(Uri location, MediaTypeHeaderValue contentType, Stream content, CancellationToken ct = default(CancellationToken))
        {
            return HttpAsync(HttpMethod.Put, location, contentType, content, ct);
        }

        /// <summary>
        /// Send a PATCH request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> HttpPatchAsync(Uri location, string contentType, Stream content, CancellationToken ct = default(CancellationToken))
        {
            return HttpAsync(new HttpMethod(HttpPatchVerb), location, contentType, content, ct);
        }

        /// <summary>
        /// Send a PATCH request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> HttpPatchAsync(Uri location, MediaTypeHeaderValue contentType, Stream content, CancellationToken ct = default(CancellationToken))
        {
            return HttpAsync(new HttpMethod(HttpPatchVerb), location, contentType, content, ct);
        }

        /// <summary>
        /// Send a request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> HttpAsync(HttpMethod method, Uri location, string contentType, Stream content, CancellationToken ct = default(CancellationToken))
        {
            Validator.ThrowIfNullOrEmpty(contentType, nameof(contentType));
            return HttpAsync(method, location, MediaTypeHeaderValue.Parse(contentType), content, ct);
        }

        /// <summary>
        /// Send a request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> HttpAsync(HttpMethod method, Uri location, MediaTypeHeaderValue contentType, Stream content, CancellationToken ct = default(CancellationToken))
        {
            Validator.ThrowIfNull(method, nameof(method));
            Validator.ThrowIfNull(contentType, nameof(contentType));
            Validator.ThrowIfNull(content, nameof(content));
            return HttpAsync(location, o =>
            {
                o.Request.Method = method;
                o.Request.Content = new StreamContent(content);
                o.Request.Content.Headers.ContentType = contentType;
                o.CancellationToken = ct;
            });
        }

        /// <summary>
        /// Send a request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="setup">The <see cref="HttpRequestOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> HttpAsync(Uri location, Action<HttpRequestOptions> setup)
        {
            Validator.ThrowIfNull(setup, nameof(setup));
            var options = setup.ConfigureOptions();
            options.Request.RequestUri = location;
            return Client.SendAsync(options.Request, options.CompletionOption, options.CancellationToken);
        }

        private static void ValidateTimeout(TimeSpan timeout, string paramName)
        {
            Validator.ThrowIfNull(timeout, paramName);
            Validator.ThrowIfLowerThanOrEqual(timeout.TotalMilliseconds, -1, paramName);
            Validator.ThrowIfGreaterThan(timeout.TotalMilliseconds, int.MaxValue, paramName);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (_isDisposed || !disposing) { return; }
            _isDisposed = true;
            Client?.Dispose();
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