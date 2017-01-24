using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace Cuemon.Net.Http
{
    /// <summary>
    /// Specifies options that is related to the <see cref="HttpManager"/> class.
    /// </summary>
    public class HttpManagerOptions
    {
        private HttpMessageHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpManagerOptions"/> class.
        /// </summary>
        public HttpManagerOptions()
        {
            Handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                MaxAutomaticRedirections = 10
            };
            DisposeHandler = true;
            DefaultRequestHeaders = new Dictionary<string, string>()
            {
                { "Connection", "Keep-Alive" }
            };
        }

        /// <summary>
        /// Gets or sets a value indicating whether the inner handler should be disposed of by Dispose().
        /// </summary>
        /// <value><c>true</c> if if the inner handler should be disposed of by Dispose(); otherwise, <c>false</c> if you intend to reuse the inner handler.</value>
        public bool DisposeHandler { get; set; }

        /// <summary>
        /// Gets the default headers which should be sent with each request.
        /// </summary>
        /// <value>The default headers which should be sent with each request.</value>
        public Dictionary<string, string> DefaultRequestHeaders { get; }

        /// <summary>
        /// Gets or sets the HTTP handler stack to use for sending requests.
        /// </summary>
        /// <value>The HTTP handler stack to use for sending requests.</value>
        public HttpMessageHandler Handler
        {
            get { return _handler; }
            set
            {
                Validator.ThrowIfNull(value, nameof(value));
                _handler = value;
            }
        }
    }
}