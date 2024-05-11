namespace HNCBKMS
{
    partial class FormMain
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
            if (disposing && (components != null))
            {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.LabelUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MenuBar = new System.Windows.Forms.ToolStrip();
            this.BtnDownload = new System.Windows.Forms.ToolStripButton();
            this.BtnConvert = new System.Windows.Forms.ToolStripButton();
            this.BtnTransPIN = new System.Windows.Forms.ToolStripButton();
            this.BtnUpload = new System.Windows.Forms.ToolStripButton();
            this.BtnLog = new System.Windows.Forms.ToolStripButton();
            this.BtnSourceConfigure = new System.Windows.Forms.ToolStripButton();
            this.BtnDeleteFile = new System.Windows.Forms.ToolStripButton();
            this.BtnExit = new System.Windows.Forms.ToolStripButton();
            this.PanelMain = new System.Windows.Forms.Panel();
            this.BtnSelectAll = new System.Windows.Forms.Button();
            this.PanelLog = new System.Windows.Forms.Panel();
            this.BtnQuery = new System.Windows.Forms.Button();
            this.LogViewer = new System.Windows.Forms.DataGridView();
            this.PickerEndTime = new System.Windows.Forms.DateTimePicker();
            this.PickerEndDate = new System.Windows.Forms.DateTimePicker();
            this.LabelEnd = new System.Windows.Forms.Label();
            this.PickerStartTime = new System.Windows.Forms.DateTimePicker();
            this.PickerStartDate = new System.Windows.Forms.DateTimePicker();
            this.LabelStart = new System.Windows.Forms.Label();
            this.FileList = new System.Windows.Forms.ListBox();
            this.DataViewer = new System.Windows.Forms.DataGridView();
            this.BtnRefresh = new System.Windows.Forms.Button();
            this.StatusBar.SuspendLayout();
            this.MenuBar.SuspendLayout();
            this.PanelMain.SuspendLayout();
            this.PanelLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogViewer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataViewer)).BeginInit();
            this.SuspendLayout();
            // 
            // StatusBar
            // 
            this.StatusBar.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LabelUser,
            this.StatusLabel});
            this.StatusBar.Location = new System.Drawing.Point(0, 520);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Padding = new System.Windows.Forms.Padding(2, 0, 25, 0);
            this.StatusBar.Size = new System.Drawing.Size(989, 29);
            this.StatusBar.TabIndex = 0;
            this.StatusBar.Text = "statusStrip1";
            // 
            // LabelUser
            // 
            this.LabelUser.AutoSize = false;
            this.LabelUser.Name = "LabelUser";
            this.LabelUser.Size = new System.Drawing.Size(200, 24);
            this.LabelUser.Text = "登入者：";
            this.LabelUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = false;
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(300, 24);
            this.StatusLabel.Text = "密碼管理系統";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MenuBar
            // 
            this.MenuBar.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.MenuBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.MenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnDownload,
            this.BtnConvert,
            this.BtnTransPIN,
            this.BtnUpload,
            this.BtnLog,
            this.BtnSourceConfigure,
            this.BtnDeleteFile,
            this.BtnExit});
            this.MenuBar.Location = new System.Drawing.Point(0, 0);
            this.MenuBar.Name = "MenuBar";
            this.MenuBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.MenuBar.Size = new System.Drawing.Size(989, 95);
            this.MenuBar.TabIndex = 1;
            this.MenuBar.Text = "toolStrip1";
            // 
            // BtnDownload
            // 
            this.BtnDownload.Image = ((System.Drawing.Image)(resources.GetObject("BtnDownload.Image")));
            this.BtnDownload.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnDownload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnDownload.Name = "BtnDownload";
            this.BtnDownload.Size = new System.Drawing.Size(109, 92);
            this.BtnDownload.Text = "下載製卡檔";
            this.BtnDownload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnDownload.ToolTipText = "從FTP下載製卡檔案";
            this.BtnDownload.Click += new System.EventHandler(this.BtnDownload_Click);
            // 
            // BtnConvert
            // 
            this.BtnConvert.AutoSize = false;
            this.BtnConvert.Image = ((System.Drawing.Image)(resources.GetObject("BtnConvert.Image")));
            this.BtnConvert.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnConvert.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnConvert.Name = "BtnConvert";
            this.BtnConvert.Size = new System.Drawing.Size(109, 92);
            this.BtnConvert.Text = "轉檔";
            this.BtnConvert.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnConvert.Click += new System.EventHandler(this.BtnConvert_Click);
            // 
            // BtnTransPIN
            // 
            this.BtnTransPIN.AutoSize = false;
            this.BtnTransPIN.Image = ((System.Drawing.Image)(resources.GetObject("BtnTransPIN.Image")));
            this.BtnTransPIN.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnTransPIN.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnTransPIN.Name = "BtnTransPIN";
            this.BtnTransPIN.Size = new System.Drawing.Size(165, 92);
            this.BtnTransPIN.Text = "儲存預借現金密碼";
            this.BtnTransPIN.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnTransPIN.Click += new System.EventHandler(this.BtnTransPIN_Click);
            // 
            // BtnUpload
            // 
            this.BtnUpload.AutoSize = false;
            this.BtnUpload.Image = ((System.Drawing.Image)(resources.GetObject("BtnUpload.Image")));
            this.BtnUpload.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnUpload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnUpload.Name = "BtnUpload";
            this.BtnUpload.Size = new System.Drawing.Size(109, 92);
            this.BtnUpload.Text = "上傳製卡檔";
            this.BtnUpload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnUpload.Visible = false;
            this.BtnUpload.Click += new System.EventHandler(this.BtnUpload_Click);
            // 
            // BtnLog
            // 
            this.BtnLog.AutoSize = false;
            this.BtnLog.Image = ((System.Drawing.Image)(resources.GetObject("BtnLog.Image")));
            this.BtnLog.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnLog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnLog.Name = "BtnLog";
            this.BtnLog.Size = new System.Drawing.Size(109, 92);
            this.BtnLog.Text = "Log查詢";
            this.BtnLog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnLog.Click += new System.EventHandler(this.BtnLog_Click);
            // 
            // BtnSourceConfigure
            // 
            this.BtnSourceConfigure.AutoSize = false;
            this.BtnSourceConfigure.Image = ((System.Drawing.Image)(resources.GetObject("BtnSourceConfigure.Image")));
            this.BtnSourceConfigure.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnSourceConfigure.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnSourceConfigure.Name = "BtnSourceConfigure";
            this.BtnSourceConfigure.Size = new System.Drawing.Size(109, 92);
            this.BtnSourceConfigure.Text = "來源檔設定";
            this.BtnSourceConfigure.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnSourceConfigure.Click += new System.EventHandler(this.BtnSourceConfigure_Click);
            // 
            // BtnDeleteFile
            // 
            this.BtnDeleteFile.AutoSize = false;
            this.BtnDeleteFile.Image = ((System.Drawing.Image)(resources.GetObject("BtnDeleteFile.Image")));
            this.BtnDeleteFile.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnDeleteFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnDeleteFile.Name = "BtnDeleteFile";
            this.BtnDeleteFile.Size = new System.Drawing.Size(109, 92);
            this.BtnDeleteFile.Text = "刪除來源檔";
            this.BtnDeleteFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnDeleteFile.Click += new System.EventHandler(this.Btn_DeleteFile_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.AutoSize = false;
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(109, 92);
            this.BtnExit.Text = "離開";
            this.BtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // PanelMain
            // 
            this.PanelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelMain.Controls.Add(this.PanelLog);
            this.PanelMain.Controls.Add(this.BtnSelectAll);
            this.PanelMain.Controls.Add(this.FileList);
            this.PanelMain.Controls.Add(this.DataViewer);
            this.PanelMain.Controls.Add(this.BtnRefresh);
            this.PanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelMain.Location = new System.Drawing.Point(0, 95);
            this.PanelMain.Name = "PanelMain";
            this.PanelMain.Size = new System.Drawing.Size(989, 425);
            this.PanelMain.TabIndex = 2;
            this.PanelMain.Resize += new System.EventHandler(this.PanelMain_Resize);
            // 
            // BtnSelectAll
            // 
            this.BtnSelectAll.Location = new System.Drawing.Point(13, 387);
            this.BtnSelectAll.Name = "BtnSelectAll";
            this.BtnSelectAll.Size = new System.Drawing.Size(100, 30);
            this.BtnSelectAll.TabIndex = 6;
            this.BtnSelectAll.Text = "全選檔案";
            this.BtnSelectAll.UseVisualStyleBackColor = true;
            this.BtnSelectAll.Click += new System.EventHandler(this.Btn_SelectAll_Click);
            // 
            // PanelLog
            // 
            this.PanelLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelLog.Controls.Add(this.BtnQuery);
            this.PanelLog.Controls.Add(this.LogViewer);
            this.PanelLog.Controls.Add(this.PickerEndTime);
            this.PanelLog.Controls.Add(this.PickerEndDate);
            this.PanelLog.Controls.Add(this.LabelEnd);
            this.PanelLog.Controls.Add(this.PickerStartTime);
            this.PanelLog.Controls.Add(this.PickerStartDate);
            this.PanelLog.Controls.Add(this.LabelStart);
            this.PanelLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelLog.Location = new System.Drawing.Point(0, 0);
            this.PanelLog.Name = "PanelLog";
            this.PanelLog.Size = new System.Drawing.Size(987, 423);
            this.PanelLog.TabIndex = 5;
            // 
            // BtnQuery
            // 
            this.BtnQuery.AutoSize = true;
            this.BtnQuery.Image = ((System.Drawing.Image)(resources.GetObject("BtnQuery.Image")));
            this.BtnQuery.Location = new System.Drawing.Point(662, 12);
            this.BtnQuery.Name = "BtnQuery";
            this.BtnQuery.Size = new System.Drawing.Size(104, 75);
            this.BtnQuery.TabIndex = 7;
            this.BtnQuery.UseVisualStyleBackColor = true;
            this.BtnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // LogViewer
            // 
            this.LogViewer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.LogViewer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LogViewer.Location = new System.Drawing.Point(16, 101);
            this.LogViewer.Name = "LogViewer";
            this.LogViewer.RowTemplate.Height = 24;
            this.LogViewer.Size = new System.Drawing.Size(950, 303);
            this.LogViewer.TabIndex = 6;
            // 
            // PickerEndTime
            // 
            this.PickerEndTime.CustomFormat = "HH 時 mm 分 ss 秒";
            this.PickerEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.PickerEndTime.Location = new System.Drawing.Point(443, 53);
            this.PickerEndTime.Name = "PickerEndTime";
            this.PickerEndTime.ShowUpDown = true;
            this.PickerEndTime.Size = new System.Drawing.Size(187, 33);
            this.PickerEndTime.TabIndex = 5;
            // 
            // PickerEndDate
            // 
            this.PickerEndDate.Location = new System.Drawing.Point(443, 14);
            this.PickerEndDate.Name = "PickerEndDate";
            this.PickerEndDate.Size = new System.Drawing.Size(187, 33);
            this.PickerEndDate.TabIndex = 4;
            // 
            // LabelEnd
            // 
            this.LabelEnd.AutoSize = true;
            this.LabelEnd.Location = new System.Drawing.Point(332, 20);
            this.LabelEnd.Name = "LabelEnd";
            this.LabelEnd.Size = new System.Drawing.Size(105, 24);
            this.LabelEnd.TabIndex = 3;
            this.LabelEnd.Text = "結束時間：";
            // 
            // PickerStartTime
            // 
            this.PickerStartTime.CustomFormat = "HH 時 mm 分 ss 秒";
            this.PickerStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.PickerStartTime.Location = new System.Drawing.Point(123, 53);
            this.PickerStartTime.Name = "PickerStartTime";
            this.PickerStartTime.ShowUpDown = true;
            this.PickerStartTime.Size = new System.Drawing.Size(187, 33);
            this.PickerStartTime.TabIndex = 2;
            // 
            // PickerStartDate
            // 
            this.PickerStartDate.Location = new System.Drawing.Point(123, 14);
            this.PickerStartDate.Name = "PickerStartDate";
            this.PickerStartDate.Size = new System.Drawing.Size(187, 33);
            this.PickerStartDate.TabIndex = 1;
            // 
            // LabelStart
            // 
            this.LabelStart.AutoSize = true;
            this.LabelStart.Location = new System.Drawing.Point(12, 20);
            this.LabelStart.Name = "LabelStart";
            this.LabelStart.Size = new System.Drawing.Size(105, 24);
            this.LabelStart.TabIndex = 0;
            this.LabelStart.Text = "開始時間：";
            // 
            // FileList
            // 
            this.FileList.FormattingEnabled = true;
            this.FileList.ItemHeight = 24;
            this.FileList.Location = new System.Drawing.Point(13, 17);
            this.FileList.Name = "FileList";
            this.FileList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.FileList.Size = new System.Drawing.Size(375, 364);
            this.FileList.TabIndex = 4;
            // 
            // DataViewer
            // 
            this.DataViewer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.DataViewer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataViewer.Location = new System.Drawing.Point(404, 17);
            this.DataViewer.Name = "DataViewer";
            this.DataViewer.RowTemplate.Height = 24;
            this.DataViewer.Size = new System.Drawing.Size(573, 400);
            this.DataViewer.TabIndex = 3;
            // 
            // BtnRefresh
            // 
            this.BtnRefresh.Location = new System.Drawing.Point(287, 387);
            this.BtnRefresh.Name = "BtnRefresh";
            this.BtnRefresh.Size = new System.Drawing.Size(100, 30);
            this.BtnRefresh.TabIndex = 1;
            this.BtnRefresh.Text = "重新整理";
            this.BtnRefresh.UseVisualStyleBackColor = true;
            this.BtnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 549);
            this.Controls.Add(this.PanelMain);
            this.Controls.Add(this.MenuBar);
            this.Controls.Add(this.StatusBar);
            this.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "華南銀行 密碼管理系統";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.StatusBar.ResumeLayout(false);
            this.StatusBar.PerformLayout();
            this.MenuBar.ResumeLayout(false);
            this.MenuBar.PerformLayout();
            this.PanelMain.ResumeLayout(false);
            this.PanelLog.ResumeLayout(false);
            this.PanelLog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogViewer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataViewer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.ToolStrip MenuBar;
        private System.Windows.Forms.ToolStripButton BtnDownload;
        private System.Windows.Forms.ToolStripButton BtnConvert;
        private System.Windows.Forms.ToolStripButton BtnUpload;
        private System.Windows.Forms.ToolStripButton BtnTransPIN;
        private System.Windows.Forms.ToolStripButton BtnSourceConfigure;
        private System.Windows.Forms.ToolStripButton BtnExit;
        private System.Windows.Forms.Panel PanelMain;
        private System.Windows.Forms.Button BtnRefresh;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.DataGridView DataViewer;
        private System.Windows.Forms.ListBox FileList;
        private System.Windows.Forms.ToolStripButton BtnLog;
        private System.Windows.Forms.Panel PanelLog;
        private System.Windows.Forms.DataGridView LogViewer;
        private System.Windows.Forms.DateTimePicker PickerEndTime;
        private System.Windows.Forms.DateTimePicker PickerEndDate;
        private System.Windows.Forms.Label LabelEnd;
        private System.Windows.Forms.DateTimePicker PickerStartTime;
        private System.Windows.Forms.DateTimePicker PickerStartDate;
        private System.Windows.Forms.Label LabelStart;
        private System.Windows.Forms.Button BtnQuery;
        private System.Windows.Forms.ToolStripStatusLabel LabelUser;
        private System.Windows.Forms.ToolStripButton BtnDeleteFile;
        private System.Windows.Forms.Button BtnSelectAll;
    }
}

