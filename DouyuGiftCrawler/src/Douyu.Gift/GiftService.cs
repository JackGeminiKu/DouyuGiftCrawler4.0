using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Jack4net.Log;
using System.Data.SqlClient;
using System.Windows.Forms;
using Douyu.Gift;
using System.Diagnostics;

namespace Douyu.Gift
{
    public static class GiftService
    {
        static SqlConnection _conn;
        static SqlDataAdapter _adapter;
        static SqlCommandBuilder _builder;
        static DataSet _dataSet;

        static readonly object _locker = new object();

        static GiftService()
        {
            try {
                _conn = new SqlConnection(DouyuGiftCrawler.Properties.Settings.Default.ConnectionString);
                //_conn.Open();
                _adapter = new SqlDataAdapter("select * from GiftCategory", _conn);
                _builder = new SqlCommandBuilder(_adapter);
                _dataSet = new DataSet();
                _adapter.Fill(_dataSet, "gift_category");

                // set up keys for defining primary key
                DataColumn[] keys = new DataColumn[1];
                keys[0] = _dataSet.Tables["gift_category"].Columns["id"];
                _dataSet.Tables["gift_category"].PrimaryKey = keys;
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void SaveGift(Gift gift)
        {
            lock (_locker) {
                DataRow findRow = _dataSet.Tables["gift_category"].Rows.Find(gift.Id);

                // 添加新礼物 
                if (findRow == null) {
                    DataRow newRow = _dataSet.Tables["gift_category"].NewRow();
                    newRow["id"] = gift.Id;
                    newRow["name"] = gift.Name;
                    newRow["type"] = gift.Type;
                    newRow["price"] = gift.Price;
                    newRow["experience"] = gift.Experience;
                    newRow["description"] = gift.Desc;
                    newRow["introduction"] = gift.Intro;
                    newRow["mimg"] = gift.Mimg;
                    newRow["himg"] = gift.Himg;
                    newRow["update_time"] = DateTime.Now;
                    _dataSet.Tables["gift_category"].Rows.Add(newRow);
                    _adapter.Update(_dataSet, "gift_category");
                    return;
                }

                // 礼物信息更新了?
                if ((float)findRow["price"] != gift.Price || (float)findRow["experience"] != gift.Experience) {
                    var watch = Stopwatch.StartNew();
                    findRow["price"] = gift.Price;
                    findRow["experience"] = gift.Experience;
                    findRow["update_time"] = DateTime.Now;
                    _adapter.Update(_dataSet, "gift_category");
                }
            }
        }
    }
}
