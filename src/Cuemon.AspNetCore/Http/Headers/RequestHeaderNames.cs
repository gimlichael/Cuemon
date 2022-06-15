namespace Cuemon.AspNetCore.Http.Headers
{
    /// <summary>
    /// Defines constants for well-known HTTP request headers.
    /// </summary>
    /// <remarks>This is primarily a statement to the (IMO) bad design decision that was made effective from .NET Core 3.0 and forward. Discussion: https://github.com/dotnet/aspnetcore/issues/9514</remarks>
    public static class RequestHeaderNames
    {
        /// <summary>
        /// The <c>Accept</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept</remarks>
        public const string Accept = "Accept";

        /// <summary>
        /// The <c>Accept-Charset</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept-Charset</remarks>
        public const string AcceptCharset = "Accept-Charset";

        /// <summary>
        /// The <c>Accept-Encoding</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept-Encoding</remarks>
        public const string AcceptEncoding = "Accept-Encoding";

        /// <summary>
        /// The <c>Accept-Language</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept-Language</remarks>
        public const string AcceptLanguage = "Accept-Language";

        /// <summary>
        /// The <c>Access-Control-Request-Headers</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Request-Headers</remarks>
        public const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
        
        /// <summary>
        /// The <c>Access-Control-Request-Method</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Request-Method</remarks>
        public const string AccessControlRequestMethod = "Access-Control-Request-Method";
        
        /// <summary>
        /// The <c>Authorization</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Authorization</remarks>
        public const string Authorization = "Authorization";
        
        /// <summary>
        /// The <c>Cache-Control</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Cache-Control</remarks>
        public const string CacheControl = "Cache-Control";

        /// <summary>
        /// The <c>Connection</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Connection</remarks>
        public const string Connection = "Connection";
        
        /// <summary>
        /// The <c>Content-Encoding</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Encoding</remarks>
        public const string ContentEncoding = "Content-Encoding";
        
        /// <summary>
        /// The <c>Content-Length</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Length</remarks>
        public const string ContentLength = "Content-Length";
        
        /// <summary>
        /// The <c>Content-MD5</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-MD5</remarks>
        public const string ContentMD5 = "Content-MD5";
        
        /// <summary>
        /// The <c>Content-Type</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Type</remarks>
        public const string ContentType = "Content-Type";
        
        /// <summary>
        /// The <c>Cookie</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Cookie</remarks>
        public const string Cookie = "Cookie";
        
        /// <summary>
        /// The <c>Date</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Date</remarks>
        public const string Date = "Date";

        /// <summary>
        /// The <c>Expect</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Expect</remarks>
        public const string Expect = "Expect";

        /// <summary>
        /// The <c>Forwarded</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Forwarded</remarks>
        public const string Forwarded = "Forwarded";
        
        /// <summary>
        /// The <c>From</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/From</remarks>
        public const string From = "From";
        
        /// <summary>
        /// The <c>Host</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Host</remarks>
        public const string Host = "Host";
        
        /// <summary>
        /// The <c>HTTP2-Settings</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/HTTP2-Settings</remarks>
        public const string Http2Settings = "HTTP2-Settings";
        
        /// <summary>
        /// The <c>If-Match</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/If-Match</remarks>
        public const string IfMatch = "If-Match";
        
        /// <summary>
        /// The <c>If-Modified-Since</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/If-Modified-Since</remarks>
        public const string IfModifiedSince = "If-Modified-Since";
        
        /// <summary>
        /// The <c>If-None-Match</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/If-None-Match</remarks>
        public const string IfNoneMatch = "If-None-Match";
        
        /// <summary>
        /// The <c>If-Range</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/If-Range</remarks>
        public const string IfRange = "If-Range";
        
        /// <summary>
        /// The <c>If-Unmodified-Since</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/If-Unmodified-Since</remarks>
        public const string IfUnmodifiedSince = "If-Unmodified-Since";
        
        /// <summary>
        /// The <c>Max-Forwards</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Max-Forwards</remarks>
        public const string MaxForwards = "Max-Forwards";
        
        /// <summary>
        /// The <c>Origin</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Origin</remarks>
        public const string Origin = "Origin";
        
        /// <summary>
        /// The <c>Pragma</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Pragma</remarks>
        public const string Pragma = "Pragma";
        
        /// <summary>
        /// The <c>Prefer</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Prefer</remarks>
        public const string Prefer = "Prefer";
        
        /// <summary>
        /// The <c>Proxy-Authorization</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Proxy-Authorization</remarks>
        public const string ProxyAuthorization = "Proxy-Authorization";
        
        /// <summary>
        /// The <c>Range</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Range</remarks>
        public const string Range = "Range";
        
        /// <summary>
        /// The <c>Referer</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referer</remarks>
        public const string Referer = "Referer";
        
        /// <summary>
        /// The <c>TE</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/TE</remarks>
        public const string TE = "TE";
        
        /// <summary>
        /// The <c>Trailer</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Trailer</remarks>
        public const string Trailer = "Trailer";
        
        /// <summary>
        /// The <c>Transfer-Encoding</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Transfer-Encoding</remarks>
        public const string TransferEncoding = "Transfer-Encoding";
        
        /// <summary>
        /// The <c>User-Agent</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/User-Agent</remarks>
        public const string UserAgent = "User-Agent";
        
        /// <summary>
        /// The <c>Upgrade</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Upgrade</remarks>
        public const string Upgrade = "Upgrade";
        
        /// <summary>
        /// The <c>Via</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Via</remarks>
        public const string Via = "Via";
        
        /// <summary>
        /// The <c>Warning</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Warning</remarks>
        public const string Warning = "Warning";

        /// <summary>
        /// The de facto standard <c>X-Csrf-Token</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://en.wikipedia.org/wiki/Cross-site_request_forgery#Cookie-to-header_token</remarks>
        public const string XCsrfToken = "X-CSRF-Token";
        
        /// <summary>
        /// The de facto standard <c>X-Correlation-ID</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://www.rapid7.com/blog/post/2016/12/23/the-value-of-correlation-ids/</remarks>
        public const string XCorrelationId = "X-Correlation-ID";
        
        /// <summary>
        /// The de facto standard <c>X-Forwarded-For</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Forwarded-For</remarks>
        public const string XForwardedFor = "X-Forwarded-For";
        
        /// <summary>
        /// The de facto standard <c>X-Forwarded-Host</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Forwarded-Host</remarks>
        public const string XForwardedHost = "X-Forwarded-Host";
        
        /// <summary>
        /// The de facto standard <c>X-Forwarded-Proto</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Forwarded-Proto</remarks>
        public const string XForwardedProto = "X-Forwarded-Proto";
        
        /// <summary>
        /// The de facto standard <c>X-Request-ID</c> request HTTP header name.
        /// </summary>
        /// <remarks>https://stackoverflow.com/questions/56068619/should-i-use-request-id-x-request-id-or-x-correlation-id-in-the-request-header</remarks>
        public const string XRequestId = "X-Request-ID";
    }
}
