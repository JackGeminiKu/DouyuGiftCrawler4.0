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
    public class ProxyCrawler_Xici : ProxyCrawlerBase
    {
        public ProxyCrawler_Xici()
        {
            for (int i = 1; i <= 10; ++i) {
                Urls.Add("http://www.xicidaili.com/nn/" + i);
            }
            for (int i = 1; i <= 10; ++i) {
                Urls.Add("http://www.xicidaili.com/nt/" + i);
            }
            for (int i = 1; i <= 10; ++i) {
                Urls.Add("http://www.xicidaili.com/wt/" + i);
            }
        }

        protected override WebClient CreateWebClient()
        {
            return new WebClient();
        }

        protected override string ToPageString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        protected override void ParseWebPage(string webPage, string url)
        {
            var document = new HtmlParser().Parse(webPage);
            var proxyes = document.QuerySelectorAll("tr");
            for (int j = 1; j < proxyes.Length; ++j) {
                var proxyInfo = proxyes[j].QuerySelectorAll("td");
                string ip = proxyInfo[1].TextContent;
                int port = int.Parse(proxyInfo[2].TextContent);
                OnProxyCrawled(ip, port, "www.xicidaili.com");
            }
        }
    }
}
