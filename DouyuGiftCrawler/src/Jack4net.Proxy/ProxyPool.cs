using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Jack4net.Log;
using AngleSharp.Parser.Html;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Newtonsoft;
using Newtonsoft.Json;
using System.Net;
using System.Runtime.Remoting.Messaging;
using Jack4net.Proxy.Crawlers;

namespace Jack4net.Proxy
{
    public static class ProxyPool
    {
        static System.Threading.Timer _tmrGetProxy;
        static Dictionary<string, string> _proxyTable = new Dictionary<string, string>();

        #region Crawl & Validate Proxy

        public static void BeginCrawl()
        {
            ProxyCache.Clear();
            ProxyCrawlResult.Clear();
            _tmrGetProxy = new System.Threading.Timer(new TimerCallback(CrawlProxy), null, 0, 1000 * 60 * 30);
        }

        static List<ProxyCrawlerBase> _proxyCrawlerList = new List<ProxyCrawlerBase>();

        static void CrawlProxy(object obj)
        {
            if (CrawlBegin != null)
                CrawlBegin(null, new CrawlBeginEventArgs());

            if (_proxyCrawlerList.Count == 0) {
                _proxyCrawlerList.Add(new ProxyCrawler_66Ip());
                _proxyCrawlerList.Add(new ProxyCrawlerData5u());
                _proxyCrawlerList.Add(new ProxyCrawler_GouBanJia());
                _proxyCrawlerList.Add(new ProxyCrawler_Ip181());
                _proxyCrawlerList.Add(new ProxyCrawler_Xici());
                _proxyCrawlerList.Add(new ProxyCrawler_Xdaili());
                _proxyCrawlerList.Add(new ProxyCrawler_Kuaidaili());
                _proxyCrawlerList.Add(new ProxyCrawler_Ip3366());
                _proxyCrawlerList.Add(new ProxyCrawler_Iphai());
                _proxyCrawlerList.Add(new ProxyCrawler_ab57());
                _proxyCrawlerList.Add(new ProxyCrawler_UsProxy());
                _proxyCrawlerList.Add(new ProxyCrawler_ProxyLists());
                _proxyCrawlerList.Add(new ProxyCrawler_AtomInterSoft());

                foreach (var crawler in _proxyCrawlerList) {
                    crawler.ProxyCrawled += new EventHandler<ProxyCrawledEventArgs>(proxyCrawler_ProxyCrawled);
                }
            }

            foreach (var crawler in _proxyCrawlerList) {
                crawler.CrawlProxy();
                //Action action = new Action(crawler.CrawlProxy);
                //action.BeginInvoke(null, null);
            }
        }

        public static event EventHandler<CrawlBeginEventArgs> CrawlBegin;

        static void proxyCrawler_ProxyCrawled(object sender, ProxyCrawledEventArgs e)
        {
            //Action<ProxyCrawledEventArgs> proxyCrawled = OnProxyCrawled;
            //proxyCrawled.BeginInvoke(e, null, null);
            OnProxyCrawled(e);

            //Func<string, int, string, bool> validateProxy = new Func<string, int, string, bool>(ProxyValidator.Validate);
            //validateProxy.BeginInvoke(e.Ip, e.Port, e.ProxySite, ProxyValidateCallback,
            //   new object[] { e.Ip, e.Port, e.ProxySite });
            ProxyValidator.Validate(e.Ip, e.Port, e.ProxySite);
        }

        public static event EventHandler<ProxyCrawledEventArgs> ProxyCrawled;

        static void OnProxyCrawled(ProxyCrawledEventArgs e)
        {
            ProxyCrawlResult.UpdateCrawledCount(e.ProxySite, 1);

            if (ProxyCrawled != null)
                ProxyCrawled(null, e);
        }

        static void ProxyValidateCallback(IAsyncResult ar)
        {
            AsyncResult result = (AsyncResult)ar;

            Func<string, int, string, bool> func = (Func<string, int, string, bool>)result.AsyncDelegate;
            var isValid = func.EndInvoke(ar);

            var proxyItems = (object[])ar.AsyncState;
            string ip = (string)proxyItems[0];
            int port = (int)proxyItems[1];
            string proxySite = (string)proxyItems[2];

            OnProxyValidated(ip, port, proxySite, isValid);
            if (isValid) ProxyCache.AddProxy(ip, port, proxySite);
        }

        public static event EventHandler<ProxyValidatedEventArgs> ProxyValidated;

        static void OnProxyValidated(string ip, int port, string proxySite, bool isValid)
        {
            if (isValid) {
                ProxyCrawlResult.UpdateValidCount(proxySite, 1);
            } else {
                ProxyCrawlResult.UpdateInvalidCount(proxySite, 1);
            }

            if (ProxyValidated != null)
                ProxyValidated(null, new ProxyValidatedEventArgs(proxySite, ip, port, isValid));
        }

        #endregion

        public static int TotalCount { get { return ProxyCache.TotalCount; } }

        public static int FreeCount { get { return ProxyCache.FreeCount; } }

        public static WebProxy GetProxy()
        {
            return ProxyCache.GetProxy();
        }

        public static WebProxy GetRandomProxy()
        {
            return ProxyCache.GetRandomProxy();
        }

        public static void RemoveProxy(WebProxy webProxy)
        {
            ProxyCache.RemoveProxy(webProxy.Address.Host, webProxy.Address.Port);
        }
    }



    /// <summary>
    /// begin crawl event args
    /// </summary>
    public class CrawlBeginEventArgs : EventArgs
    {

    }
}
