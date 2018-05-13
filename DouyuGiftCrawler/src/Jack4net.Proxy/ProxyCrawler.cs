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
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Jack4net.Proxy
{
    public static class ProxyCrawler
    {
        static ConcurrentDictionary<string, ProxyInfo> _proxyInfos = new ConcurrentDictionary<string, ProxyInfo>();
        static List<ProxyCrawlerBase> _proxyCrawlers = new List<ProxyCrawlerBase>();
        static int _pageCount = 0;
        static readonly object _pageCountLocker = new object();

        public static int ProxyCount { get { return _proxyInfos.Count; } }

        public static int PageCount
        {
            get
            {
                lock (_pageCountLocker) {
                    return _pageCount;
                }
            }
        }

        public static void BeginCrawl()
        {
            _pageCount = 0;
            if (CrawlBegin != null)
                CrawlBegin(null, new CrawlBeginEventArgs());

            if (_proxyCrawlers.Count == 0) {
                _proxyCrawlers.Add(new ProxyCrawler_66Ip());
                _proxyCrawlers.Add(new ProxyCrawlerData5u());
                _proxyCrawlers.Add(new ProxyCrawler_GouBanJia());
                _proxyCrawlers.Add(new ProxyCrawler_Ip181());
                _proxyCrawlers.Add(new ProxyCrawler_Xici());
                _proxyCrawlers.Add(new ProxyCrawler_Xdaili());
                _proxyCrawlers.Add(new ProxyCrawler_Kuaidaili());
                _proxyCrawlers.Add(new ProxyCrawler_Ip3366());
                _proxyCrawlers.Add(new ProxyCrawler_Iphai());
                _proxyCrawlers.Add(new ProxyCrawler_ab57());
                _proxyCrawlers.Add(new ProxyCrawler_UsProxy());
                _proxyCrawlers.Add(new ProxyCrawler_ProxyLists());
                _proxyCrawlers.Add(new ProxyCrawler_AtomInterSoft());

                foreach (var proxyCrawler in _proxyCrawlers) {
                    proxyCrawler.ProxyCrawled += new EventHandler<ProxyCrawledEventArgs>(proxyCrawler_ProxyCrawled);
                    proxyCrawler.PageCrawlCompleted += new EventHandler<PageCrawlCompletedEventArgs>(proxyCrawler_PageCrawlCompleted);
                }
            }

            List<Task> taskList = new List<Task>();
            foreach (var proxyCrawler in _proxyCrawlers) {
                var task = new Task(proxyCrawler.CrawlProxy);
                taskList.Add(task);
            }

            foreach (var task in taskList) {
                task.Start();
            }
            Task.WaitAll(taskList.ToArray());

            if (CrawlCompleted != null)
                CrawlCompleted(null, new CrawlCompletedEventArgs());
        }

        public static void Save()
        {
            if (File.Exists("proxy.txt"))
                File.Delete("proxy.txt");
            foreach (var proxyInfo in _proxyInfos) {
                File.AppendAllText("proxy.txt",
                    string.Format("{0},{1}:{2}\n", proxyInfo.Value.ProxySite, proxyInfo.Value.Ip, proxyInfo.Value.Port)
                );
            }
        }

        public static event EventHandler<CrawlBeginEventArgs> CrawlBegin;

        public static event EventHandler<CrawlCompletedEventArgs> CrawlCompleted;

        public static event EventHandler<ProxyCrawledEventArgs> ProxyCrawled;

        static void proxyCrawler_ProxyCrawled(object sender, ProxyCrawledEventArgs e)
        {

            LogService.GetLogger("debug").DebugFormat("{0}:\t{1}:{2}", e.ProxySite, e.Ip, e.Port);
            _proxyInfos.AddOrUpdate(e.Ip + ":" + e.Port, new ProxyInfo(e.ProxySite, e.Ip, e.Port), (arg1, arg2) => arg2);
            OnProxyCrawled(e);
        }

        static void OnProxyCrawled(ProxyCrawledEventArgs e)
        {
            ProxyCrawlResult.UpdateCrawledCount(e.ProxySite, 1);

            if (ProxyCrawled != null)
                ProxyCrawled(null, e);
        }

        static void proxyCrawler_PageCrawlCompleted(object sender, PageCrawlCompletedEventArgs e)
        {
            lock (_pageCountLocker) {
                _pageCount++;
            }
        }
    }



    ///// <summary>
    ///// begin crawl event args
    ///// </summary>
    //public class CrawlBeginEventArgs : EventArgs
    //{

    //}

    public class CrawlCompletedEventArgs : EventArgs
    {

    }
}
