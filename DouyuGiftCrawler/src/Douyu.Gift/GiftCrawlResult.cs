using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Dapper;
using System.Data.SqlClient;

namespace Douyu.Gift
{
    class GiftCrawlResult
    {
        static IDbConnection _connection;
        static readonly object _dbLocker = new object();

        static GiftCrawlResult()
        {
            _connection = new SqlConnection(DouyuGiftCrawler.Properties.Settings.Default.ConnectionString);
        }

        public static void Clear()
        {
            lock (_dbLocker) {
                _connection.Execute("delete from gift_crawl_result");
            }
        }

        public static void UpdateRoomCount(string crawlerName, int count)
        {
            lock (_dbLocker) {
                var rowCount = _connection.ExecuteScalar<int>(
                    "select count(*) from gift_crawl_result where CrawlerName = @crawlerName",
                    new { CrawlerName = crawlerName }
                );
                if (rowCount == 0) {
                    _connection.Execute(
                        "insert into gift_crawl_result(CrawlerName, CrawledRoomCount ) " +
                        "values(@CrawlerName, @CrawledCount, 0, 0)",
                        new { CrawlerName = crawlerName, CrawledRoomCount = count }
                    );
                } else {
                    _connection.Execute(
                        "update gift_crawl_result " +
                        "set CrawledRoomCount = CrawledRoomCount + @Count " +
                        "where CrawlerName = @crawlerName",
                        new { Count = count, CrawlerName = crawlerName }
                    );
                }
            }
        }
    }
}
