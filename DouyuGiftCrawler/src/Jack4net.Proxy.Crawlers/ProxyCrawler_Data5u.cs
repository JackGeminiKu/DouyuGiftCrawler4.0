using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jack4net.Log;
using System.Net;
using AngleSharp.Parser.Html;
using System.Threading;

namespace Jack4net.Proxy.Crawlers
{
    public class ProxyCrawlerData5u : ProxyCrawlerBase
    {
        public ProxyCrawlerData5u()
        {
            Urls.AddRange(new string[] {
                "http://www.data5u.com/",
                "http://www.data5u.com/free/gngn/index.shtml",
                "http://www.data5u.com/free/gnpt/index.shtml",
                "http://www.data5u.com/free/gwgn/index.shtml",
                "http://www.data5u.com/free/gwpt/index.shtml"
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
            //client.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            client.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            return client;
        }

        protected override string ToPageString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        protected override void ParseWebPage(string webPage, string url)
        {
            var document = new HtmlParser().Parse(webPage);
            foreach (var item in document.QuerySelectorAll("ul.l2")) {
                var proxyInfo = item.QuerySelectorAll("li");
                string ip = proxyInfo[0].TextContent;
                int port = int.Parse(proxyInfo[1].TextContent);
                OnProxyCrawled(ip, port, "www.data5u.com");
            }
        }
    }
}
