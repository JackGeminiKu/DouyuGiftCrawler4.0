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

namespace Douyu.Gift
{
    public class GiftCrawler
    {
        WebProxy Proxy { get; set; }

        void CrawlOnePage()
        {
            // 新建web client
            var webClient = CreateWebClient();

            // 获取要爬取的url
            var roomNumber = DouyuService.NextRoom();
            var url = DouyuService.GetRoomApiUrl(roomNumber);

            // 获取页面内容
            var page = "";
            while (true) {
                // 申请代理
                LogService.Debug("[爬礼物]: 申请代理...");
                while (Proxy == null) {
                    Proxy = ProxyPool.GetProxy();
                    if (Proxy == null) MyThread.Wait(1000);
                }

                // 使用代理
                LogService.DebugFormat("[爬礼物] 使用代理, {0}", Proxy.Address);
                webClient.Proxy = Proxy;

                // 获取网页
                try {
                    LogService.DebugFormat("[爬礼物] 准备爬取礼物页面, {0}", url);
                    GiftCrawler.OnCrawlingRoom(roomNumber);
                    page = Encoding.UTF8.GetString(webClient.DownloadData(url));

                    if (page.Contains("error") == false) {
                        LogService.ErrorFormat("[爬礼物] 礼物页面是乱码!!!");
                        webClient = CreateWebClient();
                        ProxyPool.RemoveProxy(Proxy);
                        Proxy = null;
                        continue;
                    }

                    LogService.DebugFormat("[爬礼物] 成功爬取礼物页面, {0}", url);
                } catch (WebException webEx) {
                    // 代理无效了?
                    LogService.ErrorFormat("[爬礼物] 爬取礼物页面发生异常({0}), url = {1}, proxy = {2}",
                        webEx.Message, url, Proxy.Address);
                    LogService.DebugFormat("[爬礼物] 移除无效的代理 - {0}", Proxy.Address);
                    ProxyPool.RemoveProxy(Proxy);
                    Proxy = null;
                    continue;
                } catch (Exception ex) {
                    LogService.ErrorFormat("[爬礼物] 爬取礼物页面发生异常({0}), url = {1}, proxy = {2}",
                        ex.Message, url, Proxy.Address);
                }

                // 解析礼物 
                try {
                    // 解析礼物页面
                    LogService.DebugFormat("[爬礼物] 解析礼物页面, {0}", url);
                    dynamic roomInfo = JsonConvert.DeserializeObject<dynamic>(page);

                    // 没有礼物
                    if (roomInfo["error"].Value != 0) {
                        LogService.DebugFormat("[爬礼物] 没有爬到礼物, {0}, url = {1}, proxy = {2}",
                            roomInfo["data"].Value, url, Proxy.Address);
                        return;
                    }

                    // 爬到礼物
                    bool found = false;
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
                        GiftCrawler.OnCrawlingGift(gift);
                        GiftService.SaveGift(gift);
                        found = true;
                    }
                    if (found) {
                        LogService.DebugFormat("[爬礼物] 结束解析礼物页面, 找到礼物, url = {0}, proxy = {1}",
                            url, Proxy.Address);
                    } else {
                        LogService.DebugFormat("[爬礼物] 结束解析礼物页面, 没有找到礼物, url = {0}, proxy = {1}",
                            url, Proxy.Address);
                    }
                    MyThread.Wait(3000);
                    return;
                } catch (Exception ex) {
                    LogService.ErrorFormat("[爬礼物] 解析礼物页面出现异常({0}), url = {1}, proxy = {2}",
                        ex.Message, url, Proxy.Address);
                    continue;
                }
            }
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
                t.Start();
            }
        }

        static int _giftCrawlerIndex = 0;

        public static int CrawlerCount { get { return _giftCrawlerIndex; } }

        static void StartCrawlGift()
        {
            var giftCrawler = new GiftCrawler();
            //giftCrawler.CrawlingRoom += new EventHandler<CrawlingRoomEventArgs>(GiftCrawler_CrawlingRoom);
            while (true) {
                giftCrawler.CrawlOnePage();
            }
        }

        #endregion

        #region "礼物爬虫事件"

        public static event EventHandler<CrawlingRoomEventArgs> CrawlingRoom;

        static void OnCrawlingRoom(int roomNumber)
        {
            if (CrawlingRoom != null)
                CrawlingRoom(null, new CrawlingRoomEventArgs(Thread.CurrentThread.Name, roomNumber));
        }

        public static event EventHandler<CrawlingGiftEventArgs> CrawlingGift;

        static void OnCrawlingGift(Gift gift)
        {
            if (CrawlingGift != null)
                CrawlingGift(null, new CrawlingGiftEventArgs(gift));
        }

        //static void GiftCrawler_CrawlingRoom(object sender, CrawlingRoomEventArgs e)
        //{
        //    if (CrawlingRoom != null)
        //        CrawlingRoom(sender, e);
        //}

        #endregion
    }



    /// <summary>
    /// crawling gift evetn args
    /// </summary>
    public class CrawlingRoomEventArgs : EventArgs
    {
        public CrawlingRoomEventArgs(string crawlerName, int roomNumber)
        {
            CrawlerName = crawlerName;
            RoomNumber = roomNumber;
        }

        public int RoomNumber { get; private set; }

        public string CrawlerName { get; private set; }
    }



    public class CrawlingGiftEventArgs : EventArgs
    {
        public CrawlingGiftEventArgs(Gift gift)
        {
            Gift = gift;
        }

        public Gift Gift { get; private set; }
    }
}
