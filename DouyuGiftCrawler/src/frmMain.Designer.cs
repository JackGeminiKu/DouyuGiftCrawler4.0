namespace DouyuGiftCrawler
{
    partial class frmMain
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtProxyLog = new System.Windows.Forms.TextBox();
            this.btnStartCrawlProxy = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtProxyFree = new System.Windows.Forms.TextBox();
            this.txtProxyCount = new System.Windows.Forms.TextBox();
            this.btnAddGiftCrawler = new System.Windows.Forms.Button();
            this.tmrProxyPoolStatus = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.txtCrawlerCount = new System.Windows.Forms.TextBox();
            this.nudCrawlerCount = new System.Windows.Forms.NumericUpDown();
            this.btnResetRoom = new System.Windows.Forms.Button();
            this.dgvProxySiteInfo = new System.Windows.Forms.DataGridView();
            this.tmrResult = new System.Windows.Forms.Timer(this.components);
            this.btnRefreshProxySiteInfo = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCrawlSpeed = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMaxRoomNumber = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGiftLog = new System.Windows.Forms.TextBox();
            this.dgvGiftCrawlResult = new System.Windows.Forms.DataGridView();
            this.tmrCrawlSpeed = new System.Windows.Forms.Timer(this.components);
            this.btnDebug = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudCrawlerCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProxySiteInfo)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGiftCrawlResult)).BeginInit();
            this.SuspendLayout();
            // 
            // txtProxyLog
            // 
            this.txtProxyLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProxyLog.Location = new System.Drawing.Point(3, 154);
            this.txtProxyLog.Multiline = true;
            this.txtProxyLog.Name = "txtProxyLog";
            this.txtProxyLog.ReadOnly = true;
            this.txtProxyLog.Size = new System.Drawing.Size(549, 311);
            this.txtProxyLog.TabIndex = 1;
            // 
            // btnStartCrawlProxy
            // 
            this.btnStartCrawlProxy.Location = new System.Drawing.Point(27, 18);
            this.btnStartCrawlProxy.Name = "btnStartCrawlProxy";
            this.btnStartCrawlProxy.Size = new System.Drawing.Size(96, 23);
            this.btnStartCrawlProxy.TabIndex = 7;
            this.btnStartCrawlProxy.Text = "start crawl proxy";
            this.btnStartCrawlProxy.UseVisualStyleBackColor = true;
            this.btnStartCrawlProxy.Click += new System.EventHandler(this.btnStartCrawlProxy_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "有效代理总数";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "有效代理剩余";
            // 
            // txtProxyFree
            // 
            this.txtProxyFree.Location = new System.Drawing.Point(137, 99);
            this.txtProxyFree.Name = "txtProxyFree";
            this.txtProxyFree.ReadOnly = true;
            this.txtProxyFree.Size = new System.Drawing.Size(96, 20);
            this.txtProxyFree.TabIndex = 10;
            // 
            // txtProxyCount
            // 
            this.txtProxyCount.Location = new System.Drawing.Point(137, 73);
            this.txtProxyCount.Name = "txtProxyCount";
            this.txtProxyCount.ReadOnly = true;
            this.txtProxyCount.Size = new System.Drawing.Size(96, 20);
            this.txtProxyCount.TabIndex = 11;
            // 
            // btnAddGiftCrawler
            // 
            this.btnAddGiftCrawler.Location = new System.Drawing.Point(137, 18);
            this.btnAddGiftCrawler.Name = "btnAddGiftCrawler";
            this.btnAddGiftCrawler.Size = new System.Drawing.Size(96, 23);
            this.btnAddGiftCrawler.TabIndex = 12;
            this.btnAddGiftCrawler.Text = "add gift crawler";
            this.btnAddGiftCrawler.UseVisualStyleBackColor = true;
            this.btnAddGiftCrawler.Click += new System.EventHandler(this.btnAddGiftCrawler_Click);
            // 
            // tmrProxyPoolStatus
            // 
            this.tmrProxyPoolStatus.Interval = 3000;
            this.tmrProxyPoolStatus.Tick += new System.EventHandler(this.tmrProxyPoolStatus_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "爬虫数量";
            // 
            // txtCrawlerCount
            // 
            this.txtCrawlerCount.Location = new System.Drawing.Point(137, 47);
            this.txtCrawlerCount.Name = "txtCrawlerCount";
            this.txtCrawlerCount.ReadOnly = true;
            this.txtCrawlerCount.Size = new System.Drawing.Size(96, 20);
            this.txtCrawlerCount.TabIndex = 14;
            // 
            // nudCrawlerCount
            // 
            this.nudCrawlerCount.Location = new System.Drawing.Point(246, 21);
            this.nudCrawlerCount.Name = "nudCrawlerCount";
            this.nudCrawlerCount.Size = new System.Drawing.Size(96, 20);
            this.nudCrawlerCount.TabIndex = 15;
            this.nudCrawlerCount.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // btnResetRoom
            // 
            this.btnResetRoom.Location = new System.Drawing.Point(246, 96);
            this.btnResetRoom.Name = "btnResetRoom";
            this.btnResetRoom.Size = new System.Drawing.Size(96, 23);
            this.btnResetRoom.TabIndex = 16;
            this.btnResetRoom.Text = "reset room";
            this.btnResetRoom.UseVisualStyleBackColor = true;
            this.btnResetRoom.Click += new System.EventHandler(this.btnResetRoom_Click);
            // 
            // dgvProxySiteInfo
            // 
            this.dgvProxySiteInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProxySiteInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProxySiteInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProxySiteInfo.Location = new System.Drawing.Point(558, 154);
            this.dgvProxySiteInfo.Name = "dgvProxySiteInfo";
            this.dgvProxySiteInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProxySiteInfo.Size = new System.Drawing.Size(549, 311);
            this.dgvProxySiteInfo.TabIndex = 0;
            // 
            // tmrResult
            // 
            this.tmrResult.Interval = 60000;
            this.tmrResult.Tick += new System.EventHandler(this.tmrResult_Tick);
            // 
            // btnRefreshProxySiteInfo
            // 
            this.btnRefreshProxySiteInfo.Location = new System.Drawing.Point(3, 3);
            this.btnRefreshProxySiteInfo.Name = "btnRefreshProxySiteInfo";
            this.btnRefreshProxySiteInfo.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshProxySiteInfo.TabIndex = 0;
            this.btnRefreshProxySiteInfo.Text = "Refresh";
            this.btnRefreshProxySiteInfo.UseVisualStyleBackColor = true;
            this.btnRefreshProxySiteInfo.Click += new System.EventHandler(this.btnRefreshProxySiteInfo_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 166F));
            this.tableLayoutPanel1.Controls.Add(this.txtProxyLog, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dgvProxySiteInfo, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtGiftLog, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.dgvGiftCrawlResult, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32.31132F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 67.68868F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 305F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1276, 774);
            this.tableLayoutPanel1.TabIndex = 18;
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 3);
            this.panel1.Controls.Add(this.btnDebug);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtCrawlSpeed);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtMaxRoomNumber);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.btnStartCrawlProxy);
            this.panel1.Controls.Add(this.btnResetRoom);
            this.panel1.Controls.Add(this.nudCrawlerCount);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtCrawlerCount);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtProxyFree);
            this.panel1.Controls.Add(this.btnAddGiftCrawler);
            this.panel1.Controls.Add(this.txtProxyCount);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1270, 145);
            this.panel1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(780, 57);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(102, 23);
            this.button2.TabIndex = 22;
            this.button2.Text = "proxy validate";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(780, 28);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.TabIndex = 21;
            this.button1.Text = "crawl proxy";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(629, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "房间/秒";
            // 
            // txtCrawlSpeed
            // 
            this.txtCrawlSpeed.Location = new System.Drawing.Point(523, 77);
            this.txtCrawlSpeed.Name = "txtCrawlSpeed";
            this.txtCrawlSpeed.ReadOnly = true;
            this.txtCrawlSpeed.Size = new System.Drawing.Size(100, 20);
            this.txtCrawlSpeed.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(451, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "爬取速度";
            // 
            // txtMaxRoomNumber
            // 
            this.txtMaxRoomNumber.Location = new System.Drawing.Point(521, 47);
            this.txtMaxRoomNumber.Name = "txtMaxRoomNumber";
            this.txtMaxRoomNumber.ReadOnly = true;
            this.txtMaxRoomNumber.Size = new System.Drawing.Size(100, 20);
            this.txtMaxRoomNumber.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(451, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "爬到房间: ";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.btnRefreshProxySiteInfo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(1113, 154);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(160, 311);
            this.panel2.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(3, 232);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 49);
            this.label4.TabIndex = 1;
            this.label4.Text = "注意: 各代理网站中爬到代理可能有重复, 数据中没有剔除重复的代理!";
            // 
            // txtGiftLog
            // 
            this.txtGiftLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGiftLog.Location = new System.Drawing.Point(3, 471);
            this.txtGiftLog.Multiline = true;
            this.txtGiftLog.Name = "txtGiftLog";
            this.txtGiftLog.ReadOnly = true;
            this.txtGiftLog.Size = new System.Drawing.Size(549, 300);
            this.txtGiftLog.TabIndex = 3;
            // 
            // dgvGiftCrawlResult
            // 
            this.dgvGiftCrawlResult.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGiftCrawlResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGiftCrawlResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGiftCrawlResult.Location = new System.Drawing.Point(558, 471);
            this.dgvGiftCrawlResult.Name = "dgvGiftCrawlResult";
            this.dgvGiftCrawlResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGiftCrawlResult.Size = new System.Drawing.Size(549, 300);
            this.dgvGiftCrawlResult.TabIndex = 4;
            // 
            // tmrCrawlSpeed
            // 
            this.tmrCrawlSpeed.Interval = 3000;
            this.tmrCrawlSpeed.Tick += new System.EventHandler(this.tmrCrawlSpeed_Tick);
            // 
            // btnDebug
            // 
            this.btnDebug.Location = new System.Drawing.Point(780, 84);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(102, 23);
            this.btnDebug.TabIndex = 23;
            this.btnDebug.Text = "Debug";
            this.btnDebug.UseVisualStyleBackColor = true;
            this.btnDebug.Click += new System.EventHandler(this.btnDebug_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1276, 774);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "斗鱼礼物收集器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudCrawlerCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProxySiteInfo)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGiftCrawlResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtProxyLog;
        private System.Windows.Forms.Button btnStartCrawlProxy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtProxyFree;
        private System.Windows.Forms.TextBox txtProxyCount;
        private System.Windows.Forms.Button btnAddGiftCrawler;
        private System.Windows.Forms.Timer tmrProxyPoolStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCrawlerCount;
        private System.Windows.Forms.NumericUpDown nudCrawlerCount;
        private System.Windows.Forms.Button btnResetRoom;
        private System.Windows.Forms.DataGridView dgvProxySiteInfo;
        private System.Windows.Forms.Timer tmrResult;
        private System.Windows.Forms.Button btnRefreshProxySiteInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtGiftLog;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMaxRoomNumber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCrawlSpeed;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Timer tmrCrawlSpeed;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dgvGiftCrawlResult;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnDebug;
    }
}

