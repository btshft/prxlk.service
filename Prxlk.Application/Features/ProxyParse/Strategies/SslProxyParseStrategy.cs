using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using Prxlk.Application.Shared.Options;
using Prxlk.Contracts;

namespace Prxlk.Application.Features.ProxyParse.Strategies
{
    [ProxyParseStrategy(ProxySource.SslProxies)]
    public class SslProxyParseStrategy : IProxyParseStrategy, IDisposable
    {
        private readonly HttpClient _client;
        private readonly ProxySourceOption _option;
        private readonly Lazy<Task<HtmlNodeCollection>> _proxyNodesProvider;
        private int _position;
        
        public SslProxyParseStrategy(IOptions<ServiceOptions> options)
        {
            _option = options.Value.GetSource(ProxySource.SslProxies);

            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            };
            
            _client = new HttpClient(handler);

            _client.DefaultRequestHeaders.TryAddWithoutValidation(
                "Accept",
                "text/html,application/xhtml+xml,application/xml");
            
            _client.DefaultRequestHeaders.TryAddWithoutValidation(
                "User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
            
            _proxyNodesProvider = new Lazy<Task<HtmlNodeCollection>>(GetProxyNodesAsync);
        }

        /// <inheritdoc />
        public async Task<ProxyTransportModel> GetNextAsync(CancellationToken cancellation)
        {
            var proxies = _proxyNodesProvider.IsValueCreated 
                ? (_proxyNodesProvider.Value.Result) 
                : await _proxyNodesProvider.Value;

            if (proxies == null || proxies.Count == 0)
                return null;

            var retProxyRow = proxies.ElementAtOrDefault(_position);
            if (retProxyRow == null)
                return null;

            _position++;

            var parameters = retProxyRow.SelectNodes("td");

            var ip = parameters[0].InnerText;
            var port = parameters[1].InnerLength;
            var country = parameters[3].InnerText; // 2 - country shortcut 
            var protocol = parameters[6].InnerText == "yes" ? "https" : "http";

            return new ProxyTransportModel
            {
                Ip = ip,
                Port = port,
                Protocol = protocol,
                Country = country
            };
        }
        
        private async Task<HtmlNodeCollection> GetProxyNodesAsync()
        {
            using (var response = await _client.GetAsync(_option.Url))
            {
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = response.Content != null
                        ? await response.Content.ReadAsStringAsync()
                        : string.Empty;

                    throw new HttpRequestException(
                        $"Request to URI '{_option.Url}' failed with code '{response.StatusCode}'. Response: {errorContent}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var html = new HtmlDocument();

                html.LoadHtml(content);

                return html.DocumentNode.SelectNodes("//table[@id = 'proxylisttable']//tbody//tr");
            }
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}