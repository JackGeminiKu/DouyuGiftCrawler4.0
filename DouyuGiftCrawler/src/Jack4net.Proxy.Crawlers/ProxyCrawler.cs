using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jack4net.Log;
using System.Net;
using System.Threading;
using AngleSharp.Parser.Html;

namespace Jack4net.Proxy.Crawlers
{
    /// <summary>
    /// proxy crawler
    /// </summary>
    public abstract class ProxyCrawler
    {
        public ProxyCrawler()
        {
            Urls = new List<string>();
        }

        public List<string> Urls { get; private set; }

        public void CrawlProxy()
        {
            foreach (var url in Urls) {
                WebClient client = CreateWebClient();
                do {
                    try {
                        LogService.DebugFormat("[代理] 爬取代理 " + url);
                        var webPage = CrawlProxy(client, url);

                        LogService.DebugFormat("[代理] 解析代理 " + url);
                        ParseWebPage(webPage, url);
                        break;
                    } catch (Exception ex) {
                        LogService.ErrorFormat(ex.ToString());
                        var proxy = ProxyPool.GetRandomProxy();
                        if (proxy != null) {
                            LogService.DebugFormat("[代理] 使用代理 - {0}", proxy.Address);
                            client.Proxy = proxy;
                        }
                        MyThread.Wait(3000);
                        continue;
                    }
                } while (true);

                MyThread.Wait(CrawlingInternal);
            }
        }

        protected abstract WebClient CreateWebClient();

        protected abstract string CrawlProxy(WebClient client, string url);

        protected abstract void ParseWebPage(string webPage, string url);

        protected virtual int CrawlingInternal { get { return 3000; } }

        public event EventHandler<ProxyCrawledEventArgs> ProxyCrawled;

        protected void OnProxyCrawled(string ip, int port, string proxySite)
        {
            ip = ip.Trim();
            LogService.DebugFormat("[代理] 爬到代理 - {0}:{1}", ip, port);
            if (ProxyCrawled != null)
                ProxyCrawled(null, new ProxyCrawledEventArgs(ip, port, proxySite));
        }

        protected void OnProxyCrawled(string address, string proxySite)
        {
            if (address.StartsWith("http://"))
                address = address.Substring(7);
            var proxyInfo = address.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            var ip = proxyInfo[0];
            var port = int.Parse(proxyInfo[1]);
            OnProxyCrawled(ip, port, proxySite);
        }
    }



    /// <summary>
    /// proxy event args
    /// </summary>
    public class ProxyCrawledEventArgs : EventArgs
    {
        public ProxyCrawledEventArgs(string ip, int port, string proxySite)
        {
            Ip = ip;
            Port = port;
            ProxySite = proxySite;
        }

        public string Ip { get; private set; }
        public int Port { get; private set; }
        public string ProxySite { get; private set; }
    }
}
