using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace Jack4net.Crawler
{
    public class PageCrawler
    {
        public PageCrawler()
        {
            Timeout = 3000;
            EncodingName = "UTF-8";
        }

        public void CrawlPage()
        {
            var watch = Stopwatch.StartNew();
            try {
                var request = HttpWebRequest.Create(Url) as HttpWebRequest;
                request.Timeout = Timeout;
                request.Proxy = WebProxy;

                request.KeepAlive = true;
                request.ProtocolVersion = HttpVersion.Version11;
                request.Method = "GET";
                request.Accept = "*/* ";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/536.5 (KHTML, like Gecko) Chrome/19.0.1084.56 Safari/536.5";
                request.Referer = Url;

                var response = request.GetResponse();
                var stream = response.GetResponseStream();
                var page = "";
                using (var reader = new StreamReader(stream, Encoding.GetEncoding(EncodingName))) {
                    page = reader.ReadToEnd();
                }

                OnPageCrawlComplete(Url, page);
                return;
            } finally {
                CrawledTime = watch.ElapsedMilliseconds;
            }
        }

        public event EventHandler<PageCrawlCompletedEventArgs> PageCrawlCompleted;

        protected void OnPageCrawlComplete(string url, string page)
        {
            if (PageCrawlCompleted != null)
                PageCrawlCompleted(this, new PageCrawlCompletedEventArgs(url, page));
        }

        public int Timeout { get; set; }
        public WebProxy WebProxy { get; set; }
        public string EncodingName { get; set; }
        public long CrawledTime { get; private set; }
        public string Url { get; set; }
    }



    public class PageCrawlCompletedEventArgs : EventArgs
    {
        public PageCrawlCompletedEventArgs(string url, string page)
        {
            Url = url;
            Page = page;
        }

        public string Url { get; private set; }
        public string Page { get; private set; }
    }
}
