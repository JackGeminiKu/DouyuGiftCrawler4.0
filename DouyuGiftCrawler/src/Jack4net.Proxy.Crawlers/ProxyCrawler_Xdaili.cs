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
    public class ProxyCrawler_Xdaili : ProxyCrawler
    {
        public ProxyCrawler_Xdaili()
        {

            List<string> urls = new List<string>();
            for (int i = 1; i <= 20; ++i) {
                urls.Add(string.Format("http://www.xdaili.cn/ipagent/freeip/getFreeIps?page={0}&rows=10", i));
            }
            Urls.AddRange(urls.ToArray());
        }

        protected override WebClient CreateWebClient()
        {
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.KeepAlive, "TRUE");
            client.Headers.Add("Cache-Control", "max-age=0");
            client.Headers.Add("Upgrade-Insecure-Requests", "1");
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_3) AppleWebKit/537.36 (KHTML, like Gecko)");
            client.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            client.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            return client;
        }

        protected override string CrawlProxy(WebClient client, string url)
        {
            return Encoding.UTF8.GetString(client.DownloadData(url));
        }

        protected override void ParseWebPage(string webPage, string url)
        {
            dynamic proxyInfo = JsonConvert.DeserializeObject(webPage);
            if (proxyInfo["ERRORCODE"].Value != "0") {
                throw new Exception("爬到的代理网页中ERRORCODE不为0");
            }

            foreach (dynamic item in proxyInfo["RESULT"]["rows"]) {
                string ip = item["ip"].Value;
                int port = int.Parse(item["port"].Value);
                OnProxyCrawled(ip, port, "www.xdaili.cn");
            }
        }
    }
}
