using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using System.ComponentModel;

namespace Douyu.Gift
{
    public static class GiftCrawlResult
    {
        static readonly object _dicLocker = new object();
        static Dictionary<string, GiftCrawlResultItem> _resultDic = new Dictionary<string, GiftCrawlResultItem>();

        public static void UpdateRoomCount(string crawlerName, int count)
        {
            lock (_dicLocker) {
                if (!_resultDic.ContainsKey(crawlerName)) {
                    var item = new GiftCrawlResultItem(crawlerName);
                    item.RoomCount = 1;
                    _resultDic.Add(crawlerName, item);
                    return;
                } else {
                    var item = _resultDic[crawlerName];
                    item.RoomCount += count;
                }
            }
        }

        public static void UpdateGiftCount(string crawlerName, int count)
        {
            lock (_dicLocker) {
                if (!_resultDic.ContainsKey(crawlerName)) {
                    var item = new GiftCrawlResultItem(crawlerName);
                    item.GiftCount = 1;
                    _resultDic.Add(crawlerName, item);
                    return;
                } else {
                    var item = _resultDic[crawlerName];
                    item.GiftCount += count;
                }
            }
        }

        public static void UpdateAverageSpeed(string crawlerName, int avgSpeed)
        {
            lock (_dicLocker) {
                if (!_resultDic.ContainsKey(crawlerName)) {
                    var item = new GiftCrawlResultItem(crawlerName);
                    _resultDic.Add(crawlerName, item);
                    return;
                } else {
                    var item = _resultDic[crawlerName];
                    item.AverageSpeed = avgSpeed;
                }
            }
        }

        public static SortableBindingList<GiftCrawlResultItem> GetAllResult()
        {
            lock (_resultDic) {
                SortableBindingList<GiftCrawlResultItem> items = new SortableBindingList<GiftCrawlResultItem>();
                foreach (KeyValuePair<string, GiftCrawlResultItem> item in _resultDic) {
                    items.Add(item.Value);
                }
                return items;
            }
        }
    }


    public class GiftCrawlResultItem
    {
        public GiftCrawlResultItem(string crawlerName)
        {
            CrawlerName = crawlerName;
            RoomCount = GiftCount = 0;
            AverageSpeed = 0;
        }

        public string CrawlerName { get; private set; }
        public int RoomCount { get; set; }
        public int GiftCount { get; set; }
        public double AverageSpeed { get; set; }
    }
}
