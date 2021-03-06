﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using Jack4net.Log;
using System.Threading;
using System.Diagnostics;
using Jack4net.Proxy;
using Jack4net.Proxy.Crawlers;
using System.Runtime.Remoting.Messaging;
using Douyu.Gift;
using Jack4net.Crawler;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;

namespace DouyuGiftCrawler
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //DouyuService.LoadSession();
            //ProxyPool.ProxyCrawled += new EventHandler<ProxyCrawledEventArgs>(ProxyPool_ProxyCrawled);
            //ProxyPool.ProxyValidated += new EventHandler<ProxyValidatedEventArgs>(ProxyPool_ProxyValidated);
            //GiftCrawler.CrawledGift += new EventHandler<CrawledGiftEventArgs>(GiftCrawler_CrawlingGift);
            //GiftCrawler.CrawlingRoom += new EventHandler<CrawlRoomEventArgs>(GiftCrawler_CrawlingRoom);
            //GiftCrawler.CrawledRoom += new EventHandler<CrawlRoomEventArgs>(GiftCrawler_CrawledRoom);
            //tmrCrawlSpeed.Start();
            //tmrResult.Start();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DouyuService.SaveSession();
            Process.GetCurrentProcess().Kill();
        }

        #region proxy crawler events

        void ProxyPool_ProxyCrawled(object sender, ProxyCrawledEventArgs e)
        {
            ShowLog(txtProxyLog, "爬到代理: {0}:{1}\t", e.Ip, e.Port, e.ProxySite);
        }

        void ProxyPool_ProxyValidated(object sender, ProxyValidatedEventArgs e)
        {
            ShowLog(txtProxyLog, "代理验证完成: {0}:{1}\t{2}", e.Ip, e.Port, e.IsValid ? "PASS" : "FAIL");
        }

        #endregion

        #region gift crawler event

        void GiftCrawler_CrawlingRoom(object sender, CrawlRoomEventArgs e)
        {
            txtMaxRoomNumber.SetTextCrossThread(e.RoomNumber.ToString());
        }

        void GiftCrawler_CrawledRoom(object sender, CrawlRoomEventArgs e)
        {
            lock (_lockerRoomCount) {
                _roomCount++;
            }
            GiftCrawlResult.UpdateRoomCount(e.CrawlerName, 1);
        }

        int _roomCount = 0;
        static readonly object _lockerRoomCount = new object();

        void GiftCrawler_CrawlingGift(object sender, CrawledGiftEventArgs e)
        {
            GiftCrawlResult.UpdateGiftCount(e.CrawlerName, 1);
            ShowLog(txtGiftLog, "找到礼物: {0}", e.Gift.Name);
        }

        #endregion

        private void btnStartCrawlProxy_Click(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 512;
            btnStartCrawlProxy.Enabled = false;
            ProxyPool.BeginCrawl();
            tmrProxyPoolStatus.Start();
            tmrResult.Start();
        }

        private void btnAddGiftCrawler_Click(object sender, EventArgs e)
        {
            lock (_lockerRoomCount) {
                _startCrawlingTime = DateTime.Now;
                _roomCount = 0;
            }
            GiftCrawler.StartGiftCrawler((int)nudCrawlerCount.Value);
        }

        DateTime _startCrawlingTime;

        private void btnRefreshProxySiteInfo_Click(object sender, EventArgs e)
        {
            dgvProxySiteInfo.DataSource = ProxyCrawlResult.GetAllResult();
            dgvGiftCrawlResult.DataSource = GiftCrawlResult.GetAllResult();
        }

        private void btnResetRoom_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要重置房间号?", "重置房间号", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                == System.Windows.Forms.DialogResult.No) {
                return;
            }

            Properties.Settings.Default.CurrentRoom = 0;
            Properties.Settings.Default.Save();
            DouyuService.LoadSession();
        }

        private void tmrCrawlSpeed_Tick(object sender, EventArgs e)
        {
            double speed;
            lock (_lockerRoomCount) {
                speed = (_roomCount) / (DateTime.Now - _startCrawlingTime).TotalSeconds;
            }
            txtCrawlSpeed.SetTextCrossThread(speed.ToString("0.0"));
        }

        private void tmrResult_Tick(object sender, EventArgs e)
        {
            dgvProxySiteInfo.DataSource = ProxyCrawlResult.GetAllResult();
            dgvGiftCrawlResult.DataSource = GiftCrawlResult.GetAllResult();
            Properties.Settings.Default.Save();
        }

        private void tmrProxyPoolStatus_Tick(object sender, EventArgs e)
        {
            txtProxyCount.SetTextCrossThread(ProxyPool.TotalCount.ToString());
            txtProxyFree.SetTextCrossThread(ProxyPool.FreeCount.ToString());
            txtCrawlerCount.SetTextCrossThread(GiftCrawler.CrawlerCount.ToString());
        }

        void ShowLog(TextBox textBox, string message)
        {
            int lines = textBox.GetTextCrossThread().Count(p => p == '\n') + 1;
            if (lines > 100)
                textBox.ClearCrossThread();
            textBox.AppendLineCrossThread(message);
            Application.DoEvents();
        }

        void ShowLog(TextBox textBox, string format, params object[] args)
        {
            ShowLog(textBox, string.Format(format, args));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            var watch = new Stopwatch();
            var count = 0;
            ProxyCrawler.CrawlBegin += delegate {
                watch.Start();
            };
            ProxyCrawler.ProxyCrawled += delegate(object sender2, ProxyCrawledEventArgs e2) {
                count++;
                Console.WriteLine("{0}\t{1}:{2}", e2.ProxySite, e2.Ip, e2.Port);
            };
            ProxyCrawler.CrawlCompleted += delegate {
                watch.Stop();
                MessageBox.Show(string.Format("共处理页面{0}个, 爬到代理{1}个, 耗时{2}秒",
                    ProxyCrawler.PageCount, count, watch.Elapsed.TotalSeconds));
            };

            Console.WriteLine(ProxyValidator.MaxCount);

            ProxyCrawler.BeginCrawl();
            button1.Enabled = true;

            ProxyCrawler.Save();

            //button1.Enabled = false;
            //var watch = new Stopwatch();
            //for (var i = 0; i < 1000; ++i) {
            //    watch.Restart();
            //    LogService.Debug("balabala...");
            //    Console.WriteLine("time: {0}", watch.ElapsedMilliseconds);
            //}
            //button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            var watch = Stopwatch.StartNew();
            ProxyValidator.BeginValidate();
            MessageBox.Show(string.Format("proxy validate: 耗时{0}秒", watch.Elapsed.TotalSeconds));
            ProxyValidator.Save();
            button2.Enabled = true;
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {
            ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.Expect100Continue = false;
            Dictionary<string, string> pageInfo = new Dictionary<string, string>();
            pageInfo["http://www.66ip.cn/1.html"] = "GB2312";
            //pageInfo["http://ab57.ru/downloads/proxyold.txt"] = "UTF-8";
            //pageInfo["http://www.atomintersoft.com/high_anonymity_elite_proxy_list"] = "UTF-8";
            pageInfo["http://www.data5u.com/"] = "UTF-8";
            pageInfo["http://www.goubanjia.com/"] = "UTF-8";
            pageInfo["http://www.ip3366.net/free/?stype=1"] = "GB2312";
            pageInfo["https://www.kuaidaili.com/free/inha/1"] = "UTF-8";
            //pageInfo["http://www.proxylists.net/http_highanon.txt"] = "UTF-8";
            //pageInfo["https://www.us-proxy.org/"] = "UTF-8";
            pageInfo["http://www.xicidaili.com/nn/"] = "UTF-8";

            pageInfo["http://www.baidu.com"] = "UTF-8";
            //pageInfo["http://wwww.ganji.com"] = "UTF-8";
            pageInfo["http://www.ifeng.com"] = "UTF-8";
            pageInfo["http://www.douyu.com/"] = "UTF-8";
            //pageInfo["http://www.oschina.net/"] = "UTF-8";
            pageInfo["http://www.cnblogs.com"] = "UTF-8";
            pageInfo["https://www.tuhu.cn"] = "UTF-8";
            pageInfo["http://www.zuojiaju.com/"] = "UTF-8";
            pageInfo["http://www.new-farmer.com/portal.php"] = "UTF-8";
            pageInfo["http://www.fang.com"] = "UTF-8";
            pageInfo["http://www.beiwo.tv"] = "UTF-8";

            //var webClient = new WebClient();
            //var pageBytes = webClient.DownloadData("http://www.hao123.com");
            //var page = Encoding.UTF8.GetString(pageBytes);

            //var document = new HtmlParser().Parse(page);
            //var links = document.QuerySelectorAll("a");
            //foreach (var link in links) {
            //    pageInfo[link.GetAttribute("href")] = "UTF-8";
            //}

            var tasks = new List<Task>();
            foreach (var item in pageInfo) {
                var pageCrawler = new PageCrawler();
                tasks.Add(new Task(() => pageCrawler.CrawlPage(item.Key, item.Value)));
            }

            var watch = Stopwatch.StartNew();
            foreach (var item in tasks) {
                item.Start();
            }

            Task.WaitAll(tasks.ToArray());
            MessageBox.Show(string.Format("速度: {0}", pageInfo.Count / watch.Elapsed.TotalSeconds));
        }
    }



    ///// <summary>
    ///// proxy info
    ///// </summary>
    //public class ProxyInfo
    //{
    //    public int TotalCount { get; set; }
    //    public int ValidCount { get; set; }
    //}
}
