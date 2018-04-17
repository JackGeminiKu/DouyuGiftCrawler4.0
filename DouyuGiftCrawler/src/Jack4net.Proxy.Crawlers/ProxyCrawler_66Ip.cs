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
    public class ProxyCrawler_66Ip : ProxyCrawler
    {
        public ProxyCrawler_66Ip()
        {
            for (int i = 1; i <= 10; ++i) {
                Urls.Add(string.Format(@"http://www.66ip.cn/{0}.html", i));
            }
        }

        protected override WebClient CreateWebClient()
        {
            return new WebClient();
        }

        protected override string CrawlProxy(WebClient client, string url)
        {
            return Encoding.GetEncoding(936).GetString(client.DownloadData(url));
        }

        protected override void ParseWebPage(string webPage, string url)
        {
            var document = new HtmlParser().Parse(webPage);
            var proxyes = document.QuerySelectorAll("table")[2].QuerySelectorAll("tr");
            for (int j = 1; j < proxyes.Length; ++j) {
                var proxyInfo = proxyes[j].QuerySelectorAll("td");
                string ip = proxyInfo[0].TextContent;
                int port = int.Parse(proxyInfo[1].TextContent);
                OnProxyCrawled(ip, port, "www.66ip.cn");
            }
        }
    }
}
