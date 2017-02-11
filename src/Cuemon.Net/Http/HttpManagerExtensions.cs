using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Cuemon.Net.Http
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="HttpManager"/> class.
    /// </summary>
    public static class HttpManagerExtensions
    {
        /// <summary>
        /// Send a DELETE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttpDelete(this Uri location)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpDelete(location).Result;
            }
        }

        /// <summary>
        /// Send a DELETE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttpDelete(this Uri location, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpDelete(location, timeout).Result;
            }
        }

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttpGet(this Uri location)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpGet(location).Result;
            }
        }

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttpGet(this Uri location, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpGet(location, timeout).Result;
            }
        }

        /// <summary>
        /// Send a HEAD request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttpHead(this Uri location)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpHead(location).Result;
            }
        }

        /// <summary>
        /// Send a HEAD request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttpHead(this Uri location, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpHead(location, timeout).Result;
            }
        }

        /// <summary>
        /// Send an OPTIONS request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttpOptions(this Uri location)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpOptions(location).Result;
            }
        }

        /// <summary>
        /// Send an OPTIONS request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttpOptions(this Uri location, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpOptions(location, timeout).Result;
            }
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttpPost(this Uri location, string contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPost(location, contentType, content).Result;
            }
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttpPost(this Uri location, string contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPost(location, contentType, content, timeout).Result;
            }
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttpPost(this Uri location, MediaTypeHeaderValue contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPost(location, contentType, content).Result;
            }
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttpPost(this Uri location, MediaTypeHeaderValue contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPost(location, contentType, content, timeout).Result;
            }
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttpPut(this Uri location, string contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPut(location, contentType, content).Result;
            }
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttpPut(this Uri location, string contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPut(location, contentType, content, timeout).Result;
            }
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttpPut(this Uri location, MediaTypeHeaderValue contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPut(location, contentType, content).Result;
            }
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttpPut(this Uri location, MediaTypeHeaderValue contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPut(location, contentType, content, timeout).Result;
            }
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttPatch(this Uri location, string contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttPatch(location, contentType, content).Result;
            }
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttPatch(this Uri location, string contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttPatch(location, contentType, content, timeout).Result;
            }
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttPatch(this Uri location, MediaTypeHeaderValue contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttPatch(location, contentType, content).Result;
            }
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttPatch(this Uri location, MediaTypeHeaderValue contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttPatch(location, contentType, content, timeout).Result;
            }
        }

        /// <summary>
        /// Send a TRACE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttpTrace(this Uri location)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpTrace(location).Result;
            }
        }

        /// <summary>
        /// Send a TRACE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage HttpTrace(this Uri location, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpTrace(location, timeout).Result;
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="method">The HTTP method.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage Http(this Uri location, string method)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.Http(method, location).Result;
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage Http(this Uri location, string method, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.Http(method, location, timeout).Result;
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage Http(this Uri location, string method, string contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.Http(method, location, contentType, content).Result;
            }
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
        public static HttpResponseMessage Http(this Uri location, string method, string contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.Http(method, location, contentType, content, timeout).Result;
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage Http(this Uri location, HttpMethod method, string contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.Http(method, location, contentType, content).Result;
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage Http(this Uri location, HttpMethod method, string contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.Http(method, location, contentType, content, timeout).Result;
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage Http(this Uri location, string method, MediaTypeHeaderValue contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.Http(method, location, contentType, content).Result;
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage Http(this Uri location, string method, MediaTypeHeaderValue contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.Http(method, location, contentType, content, timeout).Result;
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage Http(this Uri location, HttpMethod method, MediaTypeHeaderValue contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.Http(method, location, contentType, content).Result;
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage Http(this Uri location, HttpMethod method, MediaTypeHeaderValue contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.Http(method, location, contentType, content, timeout).Result;
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="method">The HTTP method.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage Http(this Uri location, HttpMethod method)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.Http(method, location).Result;
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static HttpResponseMessage Http(this HttpMethod method, Uri location, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.Http(method, location, timeout).Result;
            }
        }
    }
}