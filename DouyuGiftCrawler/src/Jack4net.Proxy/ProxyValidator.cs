using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Jack4net.Log;
using System.Threading;
using Jack4net.Proxy.Crawlers;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;

namespace Jack4net.Proxy
{
    /// <summary>
    /// proxy validator
    /// </summary>
    public class ProxyValidator
    {
        static ProxyValidator()
        {
            ProxyCrawler.ProxyCrawled += new EventHandler<ProxyCrawledEventArgs>(ProxyCrawler_ProxyCrawled);
            MaxCount = 20;
        }

        static void Load(string proxyFile)
        {
            if (!File.Exists(proxyFile))
                return;
            foreach (var line in File.ReadAllLines(proxyFile)) {
                if (line.Trim() == "")
                    continue;
                var items = line.Split(',');
                _proxyInfos.Enqueue(new ProxyInfo(items[0], items[1]));
            }
        }

        public static int MaxCount { get; private set; }

        public static void Save()
        {
            Save("valid_proxy.txt");
        }

        static void Save(string filePath)
        {
            if (File.Exists(filePath)) 
                File.Delete(filePath);
            ProxyInfo proxyInfo;
            while (_validProxyInfos.TryDequeue(out proxyInfo)) {
                File.AppendAllText(filePath, string.Format("{0},{1}:{2}\n", proxyInfo.ProxySite, proxyInfo.Ip, proxyInfo.Port));
            }
        }

        static ConcurrentQueue<ProxyInfo> _proxyInfos = new ConcurrentQueue<ProxyInfo>();
        static ConcurrentQueue<ProxyInfo> _validProxyInfos = new ConcurrentQueue<ProxyInfo>();

        static void ProxyCrawler_ProxyCrawled(object sender, ProxyCrawledEventArgs e)
        {
            _proxyInfos.Enqueue(new ProxyInfo(e.ProxySite, e.Ip, e.Port));
        }

        public static void BeginValidate()
        {
            Load("proxy.txt");

            var tasks = new List<Task>();
            for (int i = 0; i < MaxCount; i++) {
                tasks.Add(new Task(Validate));
            }

            for (int i = 0; i < MaxCount; i++) {
                //LogService.DebugFormat("启动代理验证器: Proxy-Validator-{0}", i);
                tasks[i].Start();
            }

            Task.WaitAll(tasks.ToArray());
        }

        static void Validate()
        {
            while (true) {
                ProxyInfo proxyInfo;
                if (!_proxyInfos.TryDequeue(out proxyInfo)) {
                    return;
                    Thread.Sleep(1000);
                    continue;
                }
                var valid = Validate(proxyInfo.Ip, proxyInfo.Port, proxyInfo.ProxySite);
                if (valid) {
                    _validProxyInfos.Enqueue(proxyInfo);
                }
                OnProxyValidated(proxyInfo.ProxySite, proxyInfo.Ip, proxyInfo.Port, valid);
            }
        }

        public static bool Validate(string ip, int port, string proxySite)
        {
            try {
                LogService.DebugFormat("[代理] 开始验证, http://{0}:{1}", ip, port);
                var address = "http://open.douyucdn.cn/api/RoomApi/room/0";
                var request = HttpWebRequest.Create(address) as HttpWebRequest;
                request.Timeout = 1000;
                request.Method = "GET";
                request.Proxy = new WebProxy(string.Format("http://{0}:{1}", ip, port));
                var response = request.GetResponse() as HttpWebResponse;
                var stream = response.GetResponseStream();
                var page = "";
                using (var reader = new StreamReader(stream, Encoding.GetEncoding("UTF-8"))) {
                    page = reader.ReadToEnd();
                }
                if (page.Contains("房间未找到")) {
                    LogService.DebugFormat("[代理] 验证代理, {0}:{1}, PASS", ip, port);
                    return true;
                } else {
                    LogService.DebugFormat("[代理] 验证代理, {0}:{1}, FAIL", ip, port);
                    return false;
                }
            } catch (Exception ex) {
                LogService.DebugFormat("[代理] 验证代理, {0}:{1}, ERROR, {2}", ip, port, ex.Message);
                return false;
            }


            //using (WebResponse wr = request.GetResponse()) {
            //    //在这里对接收到的页面内容进行处理
            //    var buffer = new byte[short.MaxValue];
            //    int count = 0;
            //    count = wr.GetResponseStream().Read(buffer, 0, short.MaxValue);
            //}

            //try {
            //    LogService.DebugFormat("[代理] 开始验证, http://{0}:{1}", ip, port);
            //    var webClient = new WebClient();
            //    webClient.Proxy = new WebProxy(string.Format("http://{0}:{1}", ip, port));
            //    var page = Encoding.UTF8.GetString(webClient.DownloadData(@"http://open.douyucdn.cn/api/RoomApi/room/0"));
            //    if (page.Contains("房间未找到")) {
            //        LogService.DebugFormat("[代理] 验证代理, {0}:{1}, PASS", ip, port);
            //        return true;
            //    } else {
            //        LogService.DebugFormat("[代理] 验证代理, {0}:{1}, FAIL", ip, port);
            //        return false;
            //    }
            //} catch (Exception ex) {
            //    LogService.DebugFormat("[代理] 验证代理, {0}:{1}, ERROR, {2}", ip, port, ex.Message);
            //    return false;
            //}
        }

        public static event EventHandler<ProxyValidatedEventArgs> ProxyValidated;

        static void OnProxyValidated(string proxySite, string ip, int port, bool valid)
        {
            if (ProxyValidated != null) {
                ProxyValidated(null, new ProxyValidatedEventArgs(proxySite, ip, port, valid));
            }
        }
    }



    /// <summary>
    /// proxy validated event args
    /// </summary>
    public class ProxyValidatedEventArgs : EventArgs
    {
        public ProxyValidatedEventArgs(string proxySite, string ip, int port, bool isValid)
        {
            Ip = ip;
            Port = port;
            ProxySite = proxySite;
            IsValid = isValid;
        }

        public string Ip { get; private set; }
        public int Port { get; private set; }
        public string ProxySite { get; private set; }
        public bool IsValid { get; private set; }
    }
}
