using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Jack4net.Crawler
{
    public class PageCrawler
    {
        private ManualResetEvent _allDone = new ManualResetEvent(false);

        public PageCrawler()
        {
            Timeout = 3000;
        }

        public int Timeout { get; set; }
        public long CrawledTime { get; private set; }
        WebProxy WebProxy { get; set; }
        string EncodingName { get; set; }

        public void SetProxy(WebProxy webProxy)
        {
            WebProxy = webProxy;    
        }

        public void CrawlPage(string uri, string encodingName)
        {
            var watch = Stopwatch.StartNew();
            try {
                var request = HttpWebRequest.Create(uri) as HttpWebRequest;
                request.Timeout = Timeout;
                request.Proxy = WebProxy;
                //request.ServicePoint.ConnectionLimit = 100;

                request.KeepAlive = true;
                request.ProtocolVersion = HttpVersion.Version11;
                request.Method = "GET";
                request.Accept = "*/* ";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/536.5 (KHTML, like Gecko) Chrome/19.0.1084.56 Safari/536.5";
                request.Referer = uri;

                // Start the asynchronous operation to get the response
                EncodingName = encodingName;
                request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
                _allDone.WaitOne();

                //var response = request.GetResponse();
                //var stream = response.GetResponseStream();
                //var page = "";
                //using (var reader = new StreamReader(stream, Encoding.GetEncoding(EncodingName))) {
                //    page = reader.ReadToEnd();
                //}

                //OnPageCrawlComplete(Url, page);
                return;
            } finally {
                CrawledTime = watch.ElapsedMilliseconds;
            }

        }

        void GetResponseCallback(IAsyncResult asyncResult)
        {
            var request = asyncResult.AsyncState as HttpWebRequest;

            // End the operation
            var response = request.EndGetResponse(asyncResult) as HttpWebResponse;
            var responseStream = response.GetResponseStream();
            var streamReader = new StreamReader(responseStream, Encoding.GetEncoding(EncodingName));
            var webPage = streamReader.ReadToEnd();

            // close the stream object
            responseStream.Close();
            streamReader.Close();

            // Release the HttpWebResponse
            response.Close();
            _allDone.Set();

            OnPageCrawlCompleted(request.Address.ToString(), webPage);
        }


        public event EventHandler<PageCrawlCompletedEventArgs> PageCrawlCompleted;

        protected void OnPageCrawlCompleted(string url, string page)
        {
            if (PageCrawlCompleted != null)
                PageCrawlCompleted(this, new PageCrawlCompletedEventArgs(url, page));
        }
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
