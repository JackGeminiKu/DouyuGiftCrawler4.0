using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jack4net.Proxy
{
    public class ProxyInfo
    {
        public ProxyInfo(string proxySite, string ip, int port)
        {
            ProxySite = proxySite;
            Ip = ip;
            Port = port;
        }

        public ProxyInfo(string proxySite, string address)
        {
            ProxySite = proxySite;
            var items = address.Split(':');
            Ip = items[0];
            Port = int.Parse(items[1]);
        }

        public string ProxySite { get; private set; }
        public string Ip { get; private set; }
        public int Port { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}:{2}", ProxySite, Ip, Port);
        }
    }
}
