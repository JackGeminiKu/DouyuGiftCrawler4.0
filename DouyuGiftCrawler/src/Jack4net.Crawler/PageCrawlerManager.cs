using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jack4net.Crawler
{
    public class PageCrawlerManager
    {
        /// <summary>
        /// 增加一条爬虫
        /// </summary>
        public void AddCrawler()
        {
            var pageCrawler = new PageCrawler();
            var uri = "";
            var encodingName = "";
            pageCrawler.CrawlPage(uri, encodingName);

        }

        /// <summary>
        ///  减少一条爬虫
        /// </summary>
        public void RemoveCrawler()
        {

        }

    }
}
