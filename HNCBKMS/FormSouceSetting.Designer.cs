namespace HNCBKMS
{
    partial class FormSouceSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSouceSetting));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.BtnNewSource = new System.Windows.Forms.ToolStripButton();
            this.BtnModifySource = new System.Windows.Forms.ToolStripButton();
            this.BtnDeleteSource = new System.Windows.Forms.ToolStripButton();
            this.BtnExit = new System.Windows.Forms.ToolStripButton();
            this.GridViewer = new System.Windows.Forms.DataGridView();
            this.PanelSetting = new System.Windows.Forms.Panel();
            this.TextFileDescription = new System.Windows.Forms.TextBox();
            this.ComboFileFormat = new System.Windows.Forms.ComboBox();
            this.TextFileName = new System.Windows.Forms.TextBox();
            this.LabelFileDescription = new System.Windows.Forms.Label();
            this.LabelFileFormat = new System.Windows.Forms.Label();
            this.LabelFileName = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewer)).BeginInit();
            this.PanelSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnNewSource,
            this.BtnModifySource,
            this.BtnDeleteSource,
            this.BtnExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(684, 95);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // BtnNewSource
            // 
            this.BtnNewSource.AutoSize = false;
            this.BtnNewSource.Image = ((System.Drawing.Image)(resources.GetObject("BtnNewSource.Image")));
            this.BtnNewSource.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnNewSource.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnNewSource.Name = "BtnNewSource";
            this.BtnNewSource.Size = new System.Drawing.Size(100, 92);
            this.BtnNewSource.Text = "新增設定";
            this.BtnNewSource.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnNewSource.Click += new System.EventHandler(this.BtnNewSource_Click);
            // 
            // BtnModifySource
            // 
            this.BtnModifySource.AutoSize = false;
            this.BtnModifySource.Image = ((System.Drawing.Image)(resources.GetObject("BtnModifySource.Image")));
            this.BtnModifySource.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnModifySource.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnModifySource.Name = "BtnModifySource";
            this.BtnModifySource.Size = new System.Drawing.Size(100, 92);
            this.BtnModifySource.Text = "修改設定";
            this.BtnModifySource.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnModifySource.Click += new System.EventHandler(this.BtnModifySource_Click);
            // 
            // BtnDeleteSource
            // 
            this.BtnDeleteSource.AutoSize = false;
            this.BtnDeleteSource.Image = ((System.Drawing.Image)(resources.GetObject("BtnDeleteSource.Image")));
            this.BtnDeleteSource.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnDeleteSource.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnDeleteSource.Name = "BtnDeleteSource";
            this.BtnDeleteSource.Size = new System.Drawing.Size(100, 92);
            this.BtnDeleteSource.Text = "刪除設定";
            this.BtnDeleteSource.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnDeleteSource.Click += new System.EventHandler(this.BtnDeleteSource_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.AutoSize = false;
            this.BtnExit.Image = ((System.Drawing.Image)(resources.GetObject("BtnExit.Image")));
            this.BtnExit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(100, 92);
            this.BtnExit.Text = "離開";
            this.BtnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // GridViewer
            // 
            this.GridViewer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.GridViewer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewer.Location = new System.Drawing.Point(12, 98);
            this.GridViewer.Name = "GridViewer";
            this.GridViewer.ReadOnly = true;
            this.GridViewer.RowTemplate.Height = 24;
            this.GridViewer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridViewer.Size = new System.Drawing.Size(660, 217);
            this.GridViewer.TabIndex = 1;
            this.GridViewer.SelectionChanged += new System.EventHandler(this.GridViewer_SelectionChanged);
            // 
            // PanelSetting
            // 
            this.PanelSetting.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelSetting.Controls.Add(this.TextFileDescription);
            this.PanelSetting.Controls.Add(this.ComboFileFormat);
            this.PanelSetting.Controls.Add(this.TextFileName);
            this.PanelSetting.Controls.Add(this.LabelFileDescription);
            this.PanelSetting.Controls.Add(this.LabelFileFormat);
            this.PanelSetting.Controls.Add(this.LabelFileName);
            this.PanelSetting.Location = new System.Drawing.Point(12, 332);
            this.PanelSetting.Name = "PanelSetting";
            this.PanelSetting.Size = new System.Drawing.Size(660, 118);
            this.PanelSetting.TabIndex = 2;
            // 
            // TextFileDescription
            // 
            this.TextFileDescription.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.TextFileDescription.Location = new System.Drawing.Point(221, 80);
            this.TextFileDescription.Name = "TextFileDescription";
            this.TextFileDescription.Size = new System.Drawing.Size(313, 33);
            this.TextFileDescription.TabIndex = 5;
            // 
            // ComboFileFormat
            // 
            this.ComboFileFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboFileFormat.FormattingEnabled = true;
            this.ComboFileFormat.Location = new System.Drawing.Point(221, 43);
            this.ComboFileFormat.Name = "ComboFileFormat";
            this.ComboFileFormat.Size = new System.Drawing.Size(313, 32);
            this.ComboFileFormat.TabIndex = 4;
            // 
            // TextFileName
            // 
            this.TextFileName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.TextFileName.Location = new System.Drawing.Point(221, 6);
            this.TextFileName.Name = "TextFileName";
            this.TextFileName.Size = new System.Drawing.Size(313, 33);
            this.TextFileName.TabIndex = 3;
            // 
            // LabelFileDescription
            // 
            this.LabelFileDescription.AutoSize = true;
            this.LabelFileDescription.Location = new System.Drawing.Point(110, 80);
            this.LabelFileDescription.Name = "LabelFileDescription";
            this.LabelFileDescription.Size = new System.Drawing.Size(105, 24);
            this.LabelFileDescription.TabIndex = 2;
            this.LabelFileDescription.Text = "檔案說明：";
            // 
            // LabelFileFormat
            // 
            this.LabelFileFormat.AutoSize = true;
            this.LabelFileFormat.Location = new System.Drawing.Point(110, 47);
            this.LabelFileFormat.Name = "LabelFileFormat";
            this.LabelFileFormat.Size = new System.Drawing.Size(105, 24);
            this.LabelFileFormat.TabIndex = 1;
            this.LabelFileFormat.Text = "資料格式：";
            // 
            // LabelFileName
            // 
            this.LabelFileName.AutoSize = true;
            this.LabelFileName.Location = new System.Drawing.Point(110, 14);
            this.LabelFileName.Name = "LabelFileName";
            this.LabelFileName.Size = new System.Drawing.Size(105, 24);
            this.LabelFileName.TabIndex = 0;
            this.LabelFileName.Text = "檔案名稱：";
            // 
            // FormSouceSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 462);
            this.Controls.Add(this.PanelSetting);
            this.Controls.Add(this.GridViewer);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSouceSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "來源檔設定";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormSouceSetting_KeyPress);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewer)).EndInit();
            this.PanelSetting.ResumeLayout(false);
            this.PanelSetting.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton BtnNewSource;
        private System.Windows.Forms.ToolStripButton BtnModifySource;
        private System.Windows.Forms.ToolStripButton BtnExit;
        private System.Windows.Forms.ToolStripButton BtnDeleteSource;
        private System.Windows.Forms.DataGridView GridViewer;
        private System.Windows.Forms.Panel PanelSetting;
        private System.Windows.Forms.Label LabelFileDescription;
        private System.Windows.Forms.Label LabelFileFormat;
        private System.Windows.Forms.Label LabelFileName;
        private System.Windows.Forms.TextBox TextFileDescription;
        private System.Windows.Forms.ComboBox ComboFileFormat;
        private System.Windows.Forms.TextBox TextFileName;
    }
}