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
    public class ProxyCrawler_ProxyLists : ProxyCrawlerBase
    {
        public ProxyCrawler_ProxyLists()
        {
            Urls.AddRange(new string[] {
                "http://www.proxylists.net/http_highanon.txt"
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

        protected override string ToPageString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes); ;
        }

        protected override void ParseWebPage(string webPage, string url)
        {
            var proxyList = webPage.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in proxyList) {
                if (item.Trim().Length == 0)
                    continue;
                var proxyInfo = item.Trim().Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                OnProxyCrawled(proxyInfo[0], proxyInfo[1].ConvertTo<int>(), "www.proxylists.net");
            }
        }
    }
}
