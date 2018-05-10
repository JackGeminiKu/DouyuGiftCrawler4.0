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

        public static void Initialize(string crawlerName, DateTime startTime)
        {
            lock (_dicLocker) {
                if (!_resultDic.ContainsKey(crawlerName)) {
                    var item = new GiftCrawlResultItem(crawlerName);
                    item.StartTime = startTime;
                    _resultDic.Add(crawlerName, item);
                    return;
                } else {
                    var item = _resultDic[crawlerName];
                    item.StartTime = startTime;
                }
            }
        }

        public static void UpdateRoomCount(string crawlerName, int count)
        {
            lock (_dicLocker) {
                var item = _resultDic[crawlerName];
                item.RoomCount += count;
            }
        }

        public static void UpdateGiftCount(string crawlerName, int count)
        {
            lock (_dicLocker) {
                var item = _resultDic[crawlerName];
                item.GiftCount += count;
            }
        }

        public static SortableBindingList<GiftCrawlResultItem> GetAllResult()
        {
            lock (_dicLocker) {
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
        }

        public string CrawlerName { get; private set; }
        public int RoomCount { get; set; }
        public int GiftCount { get; set; }
        public DateTime StartTime { get; set; }
        public long ElapsedTime
        {
            get
            {
                return (long)((DateTime.Now - StartTime).TotalSeconds);
            }
        }

        public string RoomSpeed
        {
            get { return (RoomCount / (DateTime.Now - StartTime).TotalSeconds).ToString("0.00"); }
        }
    }
}
