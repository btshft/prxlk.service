using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MediatR;
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
        }
        
        /// <inheritdoc />
        public async Task<IReadOnlyCollection<ProxyTransportModel>> ParseAsync(
            ProxyParseRequest request, CancellationToken cancellation)
        {
            try
            {
                using (var response =  await _client.GetAsync(_option.Url, cancellation))
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

                    var rows = html.DocumentNode.SelectNodes("//table[@id = 'proxylisttable']//tbody//tr");
                    var proxies = new List<ProxyTransportModel>();
                    
                    foreach (var row in rows)
                    {
                        try
                        {
                            var parameters = row.SelectNodes("td");

                            var ip = parameters[0].InnerText;
                            var port = parameters[1].InnerLength;
                            var country = parameters[3].InnerText; // 2 - country shortcut 
                            var protocol = parameters[6].InnerText == "yes" ? "https" : "http";

                            proxies.Add(new ProxyTransportModel
                            {
                                Ip = ip,
                                Port = port,
                                Protocol = protocol,
                                Country = country
                            });
                        }
                        catch (Exception e)
                        {
                            
                        }
                    }

                    return proxies;
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Unable to get proxies from {ProxySource.SslProxies}", e);
            }
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}