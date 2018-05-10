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
    public class ProxyCrawler_Ip181 : ProxyCrawlerBase
    {
        public ProxyCrawler_Ip181()
        {
            Urls.AddRange(new string[] {
                "http://www.ip181.com/" 
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
            return Encoding.UTF8.GetString(bytes);
        }

        protected override void ParseWebPage(string webPage, string url)
        {
            dynamic proxyInfo = JsonConvert.DeserializeObject(webPage);
            if (proxyInfo["ERRORCODE"].Value != "0") {
                throw new Exception("爬到的代理网页ERRORCODE不为0");
            }

            foreach (dynamic item in proxyInfo["RESULT"]) {
                string ip = item["ip"].Value;
                int port = int.Parse(item["port"].Value);
                OnProxyCrawled(ip, port, "www.ip181.com");
            }
        }
    }
}
