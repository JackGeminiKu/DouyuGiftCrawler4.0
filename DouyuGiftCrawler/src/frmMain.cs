using System;
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
            DouyuService.LoadSession();
            ProxyPool.ProxyCrawled += new EventHandler<ProxyCrawledEventArgs>(ProxyPool_ProxyCrawled);
            ProxyPool.ProxyValidated += new EventHandler<ProxyValidatedEventArgs>(ProxyPool_ProxyValidated);
            GiftCrawler.CrawlingGift += new EventHandler<CrawlingGiftEventArgs>(GiftCrawler_CrawlingGift);
            GiftCrawler.CrawlingRoom += new EventHandler<CrawlRoomEventArgs>(GiftCrawler_CrawlingRoom);
            GiftCrawler.CrawledRoom += new EventHandler<CrawlRoomEventArgs>(GiftCrawler_CrawledRoom);
            tmrCrawlSpeed.Start();
            tmrResult.Start();
        }

        void ProxyPool_ProxyCrawled(object sender, ProxyCrawledEventArgs e)
        {
            ShowLog(txtProxyLog, "爬到代理: {0}:{1}\t", e.Ip, e.Port, e.ProxySite);
        }

        void ProxyPool_ProxyValidated(object sender, ProxyValidatedEventArgs e)
        {
            ShowLog(txtProxyLog, "代理验证完成: {0}:{1}\t{2}", e.Ip, e.Port, e.IsValid ? "PASS" : "FAIL");
        }

        int _currentRoomNumber = 0;
        int _lastRoomNumber = -1;
        DateTime _startCrawlingTime;

        void GiftCrawler_CrawlingRoom(object sender, CrawlRoomEventArgs e)
        {
            var roomNumber = (e.RoomNumber > _currentRoomNumber) ?
                (_currentRoomNumber = e.RoomNumber) : _currentRoomNumber;
            txtMaxRoomNumber.SetTextCrossThread(roomNumber.ToString());

            // 记录开始爬取时的一些数据
            if (_lastRoomNumber < 0) {
                _lastRoomNumber = Properties.Settings.Default.CurrentRoom;
                _startCrawlingTime = DateTime.Now;
            }
        }

        void GiftCrawler_CrawledRoom(object sender, CrawlRoomEventArgs e)
        {
            GiftCrawlResult.UpdateRoomCount(e.CrawlerName, 1);
        }

        private void tmrCrawlSpeed_Tick(object sender, EventArgs e)
        {
            var speed = (_currentRoomNumber - _lastRoomNumber) / (DateTime.Now - _startCrawlingTime).TotalSeconds;
            txtCrawlSpeed.SetTextCrossThread(speed.ToString("0.0"));
        }

        void GiftCrawler_CrawlingGift(object sender, CrawlingGiftEventArgs e)
        {
            GiftCrawlResult.UpdateGiftCount(e.CrawlerName, 1);
            ShowLog(txtGiftLog, "找到礼物: {0}", e.Gift.Name);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DouyuService.SaveSession();
            Process.GetCurrentProcess().Kill();
        }

        private void btnStartCrawlProxy_Click(object sender, EventArgs e)
        {
            ProxyPool.BeginCrawl();
            tmrProxyPoolStatus.Start();
            tmrResult.Start();
        }

        #region 启动礼物爬虫

        private void btnAddGiftCrawler_Click(object sender, EventArgs e)
        {
            GiftCrawler.StartGiftCrawler((int)nudCrawlerCount.Value);
        }

        #endregion

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

        private void tmrProxyPoolStatus_Tick(object sender, EventArgs e)
        {
            txtProxyCount.SetTextCrossThread(ProxyPool.TotalCount.ToString());
            txtProxyFree.SetTextCrossThread(ProxyPool.FreeCount.ToString());
            txtCrawlerCount.SetTextCrossThread(GiftCrawler.CrawlerCount.ToString());
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

        #region 代理统计结果

        private void tmrResult_Tick(object sender, EventArgs e)
        {
            dgvProxySiteInfo.DataSource = ProxyCrawlResult.GetAllResult();
            dgvGiftCrawlResult.DataSource = GiftCrawlResult.GetAllResult();
        }

        private void btnRefreshProxySiteInfo_Click(object sender, EventArgs e)
        {
            dgvProxySiteInfo.DataSource = ProxyCrawlResult.GetAllResult();
            dgvGiftCrawlResult.DataSource = GiftCrawlResult.GetAllResult();
        }

        #endregion
    }



    /// <summary>
    /// proxy info
    /// </summary>
    public class ProxyInfo
    {
        public int TotalCount { get; set; }
        public int ValidCount { get; set; }
    }
}
