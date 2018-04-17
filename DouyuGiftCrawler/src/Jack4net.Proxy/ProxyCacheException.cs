using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jack4net.Proxy
{
    public class ProxyCacheException : Exception
    {
        public ProxyCacheException(string message) :
            base(message) { }

        public ProxyCacheException(string format, object[] args) :
            base(string.Format(format, args)) { }
    }
}
