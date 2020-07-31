using System.Net.Http;

namespace Cuemon.Extensions.Net.Http
{
    internal class TrackingHttpMessageHandler : DelegatingHandler
    {
        public TrackingHttpMessageHandler(HttpMessageHandler inner) : base(inner)
        {
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}