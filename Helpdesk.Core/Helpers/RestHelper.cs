using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Helpdesk.Core.Helpers
{
    public class RestHelper<T, TResourceIdentifier> : IDisposable where T : class
    {
        private bool disposed = false;
        private HttpClient httpClient;
        protected readonly string serviceBaseAddress;
        private readonly string addressSuffix;
        private readonly string jsonMediaType = "application/json";
        private List<DelegatingHandler> localHttpHandlers;

        //private readonly GenericRestfulCrudHttpClient<CatalogueResourceCollection, string> _catalogueClient;

        //public CatalogueProxyService(string rootVodMediaUrl, List<DelegatingHandler> handlers)
        //{
        //    _catalogueClient = new GenericRestfulCrudHttpClient<CatalogueResourceCollection, string>(rootVodMediaUrl, "cs-vod-media/catalogue", handlers);
        //}

        public RestHelper(string serviceBaseAddress, string addressSuffix, List<DelegatingHandler> httpHandlers)
        {
            this.serviceBaseAddress = serviceBaseAddress;
            this.addressSuffix = addressSuffix;
            localHttpHandlers = httpHandlers;
            httpClient = MakeHttpClient(serviceBaseAddress);
        }

        protected virtual HttpClient MakeHttpClient(string serviceBaseAddress)
        {
            httpClient = HttpClientFactory.Create(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            }, localHttpHandlers.ToArray());

            httpClient.BaseAddress = new Uri(serviceBaseAddress);
            httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(jsonMediaType));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("defalte"));
            httpClient.Timeout = new TimeSpan(0, 0, 1, 0);
            return httpClient;
        }

        public void AddHeader(string header, string headerValue)
        {
            if (!string.IsNullOrEmpty(header) && !string.IsNullOrEmpty(headerValue))
                httpClient.DefaultRequestHeaders.Add(header, headerValue);
        }

        public void RemoveHeader(string header)
        {
            if (!string.IsNullOrEmpty(header))
                httpClient.DefaultRequestHeaders.Remove(header);
        }

        public async Task<T> GetAsync(TResourceIdentifier identifier, string header = null, string headerValue = null)
        {
            try
            {
                AddHeader(header, headerValue);
                var responseMessage = await httpClient.GetAsync(addressSuffix + identifier);
                responseMessage.EnsureSuccessStatusCode();
                RemoveHeader(header);
                return await responseMessage.Content.ReadAsAsync<T>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                if (httpClient != null)
                {
                    var hc = httpClient;
                    httpClient = null;
                    hc.Dispose();
                }
                disposed = true;
            }
        }

        #endregion IDisposable Members
    }
}