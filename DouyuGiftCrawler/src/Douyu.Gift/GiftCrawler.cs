using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Jack4net.Log;
using Newtonsoft.Json;
using System.Threading;
using Jack4net.Proxy;
using Douyu.Gift;
using DouyuGiftCrawler;
using System.Diagnostics;

namespace Douyu.Gift
{
    public class GiftCrawler
    {
        WebProxy Proxy { get; set; }
        Stopwatch _watch = new Stopwatch();

        void CrawlOnePage()
        {
            _watch.Start();
            // 新建web client
            var webClient = CreateWebClient();
            Debug("创建了Webclient");

            // 获取要爬取的url
            var roomNumber = DouyuService.NextRoom();
            var url = DouyuService.GetRoomApiUrl(roomNumber);
            Debug("获取了房间页面: {0}", url);

            // 获取页面内容
            var page = "";
            while (true) {
                // 申请代理
                while (Proxy == null) {
                    Proxy = ProxyPool.GetProxy();
                    if (Proxy == null) {
                        MyThread.Wait(1000);
                        continue;
                    }
                    Debug("找到了代理 {0}", Proxy.Address);

                    // 使用代理
                    webClient.Proxy = Proxy;
                    Debug("设置了代理");
                    break;
                }

                // 获取网页
                try {
                    // Action<int> crawlingRoom = new Action<int>(GiftCrawler.OnCrawlingRoom);
                    //IAsyncResult result= crawlingRoom.BeginInvoke(roomNumber, null, null);                    
                    // crawlingRoom.EndInvoke(result);
                    GiftCrawler.OnCrawlingRoom(roomNumber);
                    Debug("on crawing room");

                    page = Encoding.UTF8.GetString(webClient.DownloadData(url));
                    Debug("dowanload data");

                    if (page.Contains("error") == false) {
                        webClient = CreateWebClient();
                        ProxyPool.RemoveProxy(Proxy);
                        Proxy = null;
                        Debug("页面都是乱码, 重新创建Webclient, 移除代理");
                        continue;
                    }

                    //Action<int> crawledRoom = GiftCrawler.OnCrawledRoom;
                    //crawledRoom.BeginInvoke(roomNumber, null, null);
                    GiftCrawler.OnCrawledRoom(roomNumber);
                    Debug("on crawled room");
                } catch (WebException webEx) {
                    // 代理无效了?
                    Debug("爬取礼物页面发生WebException, 异常信息 = {0}, url = {1}, proxy = {2}",
                        webEx.Message, url, Proxy.Address);
                    ProxyPool.RemoveProxy(Proxy);
                    Debug("移除了无效的代理 - {0}", Proxy.Address);
                    Proxy = null;
                    continue;
                } catch (Exception ex) {
                    Debug("爬取礼物页面发生Exception, 异常信息 = {0}, url = {1}, proxy = {2}",
                        ex.Message, url, Proxy.Address);
                }

                // 解析礼物 
                try {
                    // 解析礼物页面
                    dynamic roomInfo = JsonConvert.DeserializeObject<dynamic>(page);
                    Debug("deserialize object");

                    // 没有礼物
                    if (roomInfo["error"].Value != 0) {
                        Debug("没有爬到礼物, 错误信息 = {0}, url = {1}, proxy = {2}",
                            roomInfo["data"].Value, url, Proxy.Address);
                        return;
                    }

                    // 爬到礼物
                    foreach (dynamic item in roomInfo["data"]["gift"]) {
                        Gift gift = new Gift(
                            int.Parse(item["id"].Value),
                            item["name"].Value,
                            item["type"].Value,
                            item["pc"].Value,
                            item["gx"].Value,
                            item["desc"].Value,
                            item["intro"].Value,
                            item["mimg"].Value,
                            item["himg"].Value
                        );
                        //Action<Gift> crawlingGift = GiftCrawler.OnCrawlingGift;
                        //crawlingGift.BeginInvoke(gift, null, null);
                        GiftCrawler.OnCrawledGift(gift);
                        Debug("on crawled gift: {0}", gift.Name);

                        GiftService.SaveGift(gift);
                        Debug("save gift: {0}", gift.Name);
                    }

                    MyThread.Wait(3000);
                    Debug("delayed 3000ms...");
                    return;
                } catch (Exception ex) {
                    Debug("解析礼物页面出现Exception, 异常信息 = {0}, url = {1}, proxy = {2}",
                        ex.Message, url, Proxy.Address);
                    continue;
                }
            }
        }

        void Debug(string format, params object[] args)
        {
            Debug(string.Format(format, args));
        }

        void Debug(string message)
        {
            //var wacth = Stopwatch.StartNew();
            message += string.Format("\t{0}", _watch.ElapsedMilliseconds);
            LogService.Info(message);
            _watch.Restart();
            //Console.WriteLine("\t\t\t\t\t\t\t\t" + wacth.ElapsedMilliseconds + " ms");
        }

        WebClient CreateWebClient()
        {
            var webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.KeepAlive, "TRUE");
            webClient.Headers.Add("Cache-Control", "max-age=0");
            webClient.Headers.Add("Upgrade-Insecure-Requests", "1");
            webClient.Headers.Add("User-Agent",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_3) AppleWebKit/537.36 (KHTML, like Gecko)");
            webClient.Headers.Add("Accept",
                "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            webClient.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            //client.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            return webClient;
        }

        #region 启动礼物爬虫

        public static void StartGiftCrawler(int count)
        {
            for (int i = 0; i < count; i++) {
                Thread t = new Thread(new ThreadStart(StartCrawlGift));
                t.Name = "Gift Crawler " + (_giftCrawlerIndex++).ToString("D2");
                t.Priority = ThreadPriority.Highest;
                t.Start();
            }
        }

        static int _giftCrawlerIndex = 0;

        public static int CrawlerCount { get { return _giftCrawlerIndex; } }

        static void StartCrawlGift()
        {
            var giftCrawler = new GiftCrawler();
            //giftCrawler.CrawlingRoom += new EventHandler<CrawlingRoomEventArgs>(GiftCrawler_CrawlingRoom);
            GiftCrawlResult.Initialize(Thread.CurrentThread.Name, DateTime.Now);
            while (true) {
                var watch = Stopwatch.StartNew();
                giftCrawler.CrawlOnePage();
                LogService.InfoFormat("[时间] 爬取一个网页, 总耗时\t{0}", watch.ElapsedMilliseconds);
                LogService.Info("");
            }
        }

        #endregion

        #region "礼物爬虫事件"

        static void OnCrawlingRoom(int roomNumber)
        {
            if (CrawlingRoom != null)
                CrawlingRoom(null, new CrawlRoomEventArgs(Thread.CurrentThread.Name, roomNumber));
        }

        public static event EventHandler<CrawlRoomEventArgs> CrawlingRoom;

        static void OnCrawledRoom(int roomNumber)
        {
            if (CrawledRoom != null)
                CrawledRoom(null, new CrawlRoomEventArgs(Thread.CurrentThread.Name, roomNumber));
        }

        public static event EventHandler<CrawlRoomEventArgs> CrawledRoom;

        static void OnCrawledGift(Gift gift)
        {
            if (CrawledGift != null)
                CrawledGift(null, new CrawledGiftEventArgs(Thread.CurrentThread.Name, gift));
        }

        public static event EventHandler<CrawledGiftEventArgs> CrawledGift;
        //static void GiftCrawler_CrawlingRoom(object sender, CrawlingRoomEventArgs e)
        //{
        //    if (CrawlingRoom != null)
        //        CrawlingRoom(sender, e);
        //}

        #endregion
    }



    /// <summary>
    /// crawl gift evetn args
    /// </summary>
    public class CrawlRoomEventArgs : EventArgs
    {
        public CrawlRoomEventArgs(string crawlerName, int roomNumber)
        {
            CrawlerName = crawlerName;
            RoomNumber = roomNumber;
        }

        public string CrawlerName { get; private set; }

        public int RoomNumber { get; private set; }
    }



    public class CrawledGiftEventArgs : EventArgs
    {
        public CrawledGiftEventArgs(string crawlerName, Gift gift)
        {
            CrawlerName = crawlerName;
            Gift = gift;
        }

        public string CrawlerName { get; private set; }
        public Gift Gift { get; private set; }
    }
}
