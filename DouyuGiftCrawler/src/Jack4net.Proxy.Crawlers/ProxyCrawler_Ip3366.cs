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
using System.Diagnostics;

namespace Jack4net.Proxy.Crawlers
{
    public class ProxyCrawler_Ip3366 : ProxyCrawler
    {
        public ProxyCrawler_Ip3366()
        {
            Urls.AddRange(new string[] {
                "http://www.ip3366.net/free/?stype=1",
                "http://www.ip3366.net/free/?stype=2",
                "http://www.ip3366.net/free/?stype=3",
                "http://www.ip3366.net/free/?stype=4"
            });
        }

        protected override WebClient CreateWebClient()
        {
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.KeepAlive, "TRUE");
            client.Headers.Add("Cache-Control", "max-age=0");
            client.Headers.Add("Upgrade-Insecure-Requests", "1");
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_3) AppleWebKit/537.36 (KHTML, like Gecko)");
            client.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            //client.Headers.Add("Accept-Encoding", "gzip, deflate, sdch"); 
            return client;
        }

        protected override string CrawlProxy(WebClient client, string url)
        {
            return Encoding.GetEncoding("GB2312").GetString(client.DownloadData(url));
        }

        protected override void ParseWebPage(string webPage, string url)
        {
            var document = new HtmlParser().Parse(webPage);
            var proxyes = document.QuerySelectorAll(@"table tbody tr");
            for (int j = 0; j < proxyes.Length; ++j) {
                var proxyInfo = proxyes[j].QuerySelectorAll("td");
                string ip = proxyInfo[0].TextContent;
                int port = int.Parse(proxyInfo[1].TextContent);
                OnProxyCrawled(ip, port, "wwww.ip3366.net");
            }
        }
    }
}
