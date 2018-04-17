using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using Dapper;
using System.Threading;

namespace Jack4net.Proxy
{
    internal static class ProxyCache
    {
        static IDbConnection _connection;

        static ProxyCache()
        {
            _connection = new SqlConnection(DouyuGiftCrawler.Properties.Settings.Default.ConnectionString);
        }

        public static WebProxy GetProxy()
        {
            try {
                ProxyMutex.WaitOne();
                var proxyList = _connection.Query("select Ip, Port from Proxy where IsUsing = 0");
                if (proxyList.Count() == 0) {
                    return null;
                }

                var proxy = proxyList.First();
                var count = _connection.Execute("update Proxy Set IsUsing = 1 where Ip = @Ip and Port = @Port",
                    new { Ip = proxy.Ip, Port = proxy.Port });
                if (count != 1)
                    throw new ProxyCacheException("Proxy cache: get proxy failed!");

                return new WebProxy(string.Format("http://{0}:{1}", proxy.Ip, proxy.Port));
            } finally {
                ProxyMutex.ReleaseMutex();
            }
        }


        public static WebProxy GetRandomProxy()
        {
            try {
                ProxyMutex.WaitOne();
                var proxyList = _connection.Query("select Ip, Port from Proxy");
                if (proxyList.Count() == 0) {
                    return null;
                }

                int i = new Random().Next(0, proxyList.Count());
                var proxy = proxyList.ElementAt(i);
                return new WebProxy(string.Format("http://{0}:{1}", proxy.Ip, proxy.Port));
            } finally {
                ProxyMutex.ReleaseMutex();
            }
        }

        public static void AddProxy(string ip, int port, string proxySite)
        {
            try {
                ProxyMutex.WaitOne();
                var count = _connection.ExecuteScalar<int>(
                    "select count(*) from Proxy where Ip = @Ip and Port = @Port",
                    new { Ip = ip, Port = port }
                );
                if (count != 0) {
                    return;
                }

                count = _connection.Execute(
                    "insert into Proxy(IP, Port, IsUsing, CrawlFrom) values(@Ip, @Port, 0, @CrawlFrom)",
                    new { Ip = ip, Port = port, CrawlFrom = proxySite }
                );

                if (count != 1)
                    throw new ProxyCacheException("Proxy cache: add proxy failed!");
            } finally {
                ProxyMutex.ReleaseMutex();
            }
        }

        public static void Clear()
        {
            try {
                ProxyMutex.WaitOne();
                _connection.Execute("delete from Proxy");
            } finally {
                ProxyMutex.ReleaseMutex();
            }
        }

        public static void RemoveProxy(string ip, int port)
        {
            try {
                ProxyMutex.WaitOne();
                var count = _connection.ExecuteScalar<int>(
                    "select count(*) from Proxy where Ip = @Ip and Port = @Port",
                    new { Ip = ip, Port = port }
                );
                if (count == 0) {
                    return;
                }

                count = _connection.Execute(
                    "delete from Proxy where Ip = @Ip and Port = @Port",
                    new { Ip = ip, Port = port }
                );

                if (count != 1)
                    throw new ProxyCacheException("Proxy cache: remove proxy failed!");
            } finally {
                ProxyMutex.ReleaseMutex();
            }
        }

        public static int TotalCount
        {
            get
            {
                try {
                    ProxyMutex.WaitOne();
                    var count = _connection.ExecuteScalar<int>("select count(*) from Proxy");
                    return count;
                } finally {
                    ProxyMutex.ReleaseMutex();
                }
            }
        }

        public static int FreeCount
        {
            get
            {
                try {
                    ProxyMutex.WaitOne();
                    var count = _connection.ExecuteScalar<int>("select count(*) from Proxy where IsUsing = 0");
                    return count;
                } finally {
                    ProxyMutex.ReleaseMutex();
                }
            }
        }

        private static Lazy<Mutex> _lazyMutex = new Lazy<Mutex>(() => new Mutex(false, "MyMutex"));
        static Mutex ProxyMutex { get { return _lazyMutex.Value; } }
    }
}
