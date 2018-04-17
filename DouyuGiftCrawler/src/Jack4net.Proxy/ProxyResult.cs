using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.ComponentModel;

namespace Jack4net.Proxy
{
    /// <summary>
    /// proxy result
    /// </summary>
    public static class ProxyResult
    {
        static IDbConnection _connection;
        static readonly object _dbLocker = new object();

        static ProxyResult()
        {
            _connection = new SqlConnection(DouyuGiftCrawler.Properties.Settings.Default.ConnectionString);
        }

        public static void Clear()
        {
            lock (_dbLocker) {
                _connection.Execute("delete from proxy_result");
            }
        }

        public static void UpdateCrawledCount(string proxySite, int count)
        {
            lock (_dbLocker) {
                var rowCount = _connection.ExecuteScalar<int>(
                    "select count(*) from proxy_result where ProxySite = @ProxySite",
                    new { ProxySite = proxySite }
                );
                if (rowCount == 0) {
                    _connection.Execute(
                        "insert into proxy_result(ProxySite, CrawledCount, ValidCount, InvalidCount) " +
                        "values(@ProxySite, @CrawledCount, 0, 0)",
                        new { ProxySite = proxySite, CrawledCount = count }
                    );
                } else {
                    _connection.Execute(
                        "update proxy_result set CrawledCount = CrawledCount + @Count where ProxySite = @ProxySite",
                        new { Count = count, ProxySite = proxySite }
                    );
                }
            }
        }

        public static void UpdateValidCount(string proxySite, int count)
        {
            lock (_dbLocker) {
                _connection.Execute("update proxy_result set ValidCount = ValidCount + @Count where ProxySite = @ProxySite",
                    new { Count = count, ProxySite = proxySite }
                );
            }
        }

        public static void UpdateInvalidCount(string proxySite, int count)
        {
            lock (_dbLocker) {
                _connection.Execute(
                    "update proxy_result set InvalidCount = InvalidCount + @Count where ProxySite = @ProxySite",
                    new { Count = count, ProxySite = proxySite }
                );
            }
        }

        public static SortableBindingList<ProxyResultItem> GetAll()
        {
            lock (_dbLocker) {
                var allInfo = _connection.Query(
                    "select ProxySite, CrawledCount, ValidCount, InvalidCount, " +
                    "CrawledCount - ValidCount - InvalidCount as UnvalidatedCount, " +
                    "ValidCount * 100 / CrawledCount as ValidPercent " +
                    "from proxy_result order by ValidPercent desc");

                var infoList = new SortableBindingList<ProxyResultItem>();
                for (var i = 0; i < allInfo.Count(); ++i) {
                    var item = allInfo.ElementAt(i);
                    infoList.Add(new ProxyResultItem(
                        item.ProxySite,
                        item.CrawledCount,
                        item.ValidCount,
                        item.InvalidCount,
                        item.UnvalidatedCount,
                        item.ValidPercent)
                    );
                }
                return infoList;
            }
        }
    }


    /// <summary>
    /// proxy result item
    /// </summary>
    public class ProxyResultItem
    {
        public ProxyResultItem(
            string proxySite, int crawledCount, int validCount, int invalidCount,
            int unvalidatedCount, int validPercent)
        {
            ProxySite = proxySite;
            CrawledCount = crawledCount;
            ValidCount = validCount;
            InvalidCount = invalidCount;
            UnvalidatedCount = unvalidatedCount;
            ValidPercent = validPercent;
        }

        public string ProxySite { get; private set; }
        public int CrawledCount { get; private set; }
        public int ValidCount { get; private set; }
        public int InvalidCount { get; private set; }
        public int UnvalidatedCount { get; private set; }
        public int ValidPercent { get; private set; }
    }
}
