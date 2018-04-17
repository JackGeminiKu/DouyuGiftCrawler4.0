using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jack4net.Log;
using System.Net;
using AngleSharp.Parser.Html;
using System.Threading;
using Newtonsoft;
using Newtonsoft.Json;

namespace Jack4net.Proxy.Crawlers
{
    public class ProxyCrawler_UsProxy : ProxyCrawler
    {
        public ProxyCrawler_UsProxy()
        {
            Urls.AddRange(new string[] {
                "https://www.us-proxy.org/"
            });
        }

        protected override WebClient CreateWebClient()
        {
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.KeepAlive, "FALSE");
            client.Headers.Add("Cache-Control", "max-age=0");
            client.Headers.Add("Upgrade-Insecure-Requests", "1");
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_3) AppleWebKit/537.36 (KHTML, like Gecko)");
            client.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            //client.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            client.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            return client;
        }

        protected override string CrawlProxy(WebClient client, string url)
        {
            return Encoding.UTF8.GetString(client.DownloadData(url));
        }

        protected override void ParseWebPage(string webPage, string url)
        {
            var document = new HtmlParser().Parse(webPage);
            var proxyes = document.QuerySelectorAll(@"table tbody tr");
            for (int j = 1; j < proxyes.Length; ++j) {
                var proxyInfo = proxyes[j].QuerySelectorAll("td");
                string ip = proxyInfo[0].TextContent;
                int port = int.Parse(proxyInfo[1].TextContent);
                OnProxyCrawled(ip, port, "www.us-proxy.org");
            }
        }
    }
}
