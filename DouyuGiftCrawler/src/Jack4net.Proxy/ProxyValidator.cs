using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Jack4net.Log;

namespace Jack4net.Proxy
{
    /// <summary>
    /// proxy validator
    /// </summary>
    public class ProxyValidator
    {
        public static bool Validate(string ip, int port, string proxySite)
        {
            try {
                LogService.DebugFormat("[代理] 开始验证, http://{0}:{1}", ip, port);
                var webClient = new WebClient();
                webClient.Proxy = new WebProxy(string.Format("http://{0}:{1}", ip, port));
                var page = Encoding.UTF8.GetString(webClient.DownloadData(@"http://open.douyucdn.cn/api/RoomApi/room/0"));
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
        }
    }


    public class ProxyValidatedEventArgs : EventArgs
    {
        public ProxyValidatedEventArgs(string ip, int port, string proxySite, bool isValid)
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
