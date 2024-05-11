using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FT_Function.Xml;
using log4net;
using log4net.Config;

namespace HNCBKMS
{
    public partial class FormSouceSetting : Form
    {
        private XMLProcess xml = null;
        private string InitialFolder = string.Empty;
        private string SetupFolder = string.Empty;
        private Database DB_Host = null;
        private SqlDataReader DB_Reader = null;
        private string DB_Command = string.Empty;
        private DataTable DB_Table = null;

        private String logMessage = String.Empty;
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

        public FormSouceSetting()
        {
            InitializeComponent();
            InitialSetting();
        }

        private void InitialSetting()
        {
            try
            {
                XmlConfigurator.Configure(new System.IO.FileInfo("./log4net.config"));
                logger.Info("密碼管理系統 開啟來源檔設定功能。");

                xml = new XMLProcess(Application.StartupPath + "/SETUP.XML");
                InitialFolder = xml.SelectInnerText("setup/initial-folder");
                SetupFolder = xml.SelectInnerText("setup/setup-folder");

                #region Host Database Setting
                xml = new XMLProcess(SetupFolder + "DATABASE.XML");
                string DB_Server_Host = xml.SelectInnerText("database/host/server-location").Trim();
                string DB_Name_Host = xml.SelectInnerText("database/host/database-name").Trim();
                string DB_User_Host = xml.SelectInnerText("database/host/user-name").Trim();
                string DB_Password_Host = xml.SelectInnerText("database/host/password").Trim();
                int DB_Timeout_Host = 0;
                Int32.TryParse(xml.SelectInnerText("database/host/connect-timeout"), out DB_Timeout_Host);

                if ((DB_User_Host == "") || (DB_Password_Host == ""))
                {
                    DB_Host = new Database(DB_Server_Host, DB_Name_Host,
                        (DB_Timeout_Host == 0) ? DB_Timeout_Host = 10 : DB_Timeout_Host);
                }
                else
                {
                    DB_Host = new Database(DB_Server_Host, DB_Name_Host, DB_User_Host, DB_Password_Host,
                        (DB_Timeout_Host == 0) ? DB_Timeout_Host = 10 : DB_Timeout_Host);
                }
                #endregion

                #region List *.DAF File
                foreach (String fname in System.IO.Directory.GetFileSystemEntries(InitialFolder, "*.DAF"))
                {
                    ComboFileFormat.Items.Add(Path.GetFileName(fname.ToUpper()));
                }
                #endregion

                #region Show DataGridView Data
                DB_Host.Connect();

                DB_Command = string.Format("SELECT FILE_NAME, FILE_FORMAT, FILE_DESCRIPTION FROM FILE_FORMAT ORDER BY FILE_NAME");
                DB_Reader = DB_Host.ExecuteQuery(DB_Command);
                DB_Table = new DataTable();
                DB_Table.Load(DB_Reader);
                GridViewer.DataSource = DB_Table;
                GridViewer.Refresh();
                DB_Reader.Close();

                DB_Host.Disconnect();
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            PanelSetting.Enabled = false;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            logger.Info("密碼管理系統 結束來源檔設定功能。");
            this.Close();
            this.Dispose();
        }

        private void BtnNewSource_Click(object sender, EventArgs e)
        {
            if (BtnNewSource.Text == "新增設定")
            {
                BtnNewSource.Text = "確認新增";
                BtnDeleteSource.Enabled = false;
                BtnModifySource.Enabled = false;
                PanelSetting.Enabled = true;
                TextFileName.Enabled = true;
                ComboFileFormat.SelectedIndex = 0;
                TextFileName.Focus();
            }
            else if (BtnNewSource.Text == "確認新增")
            {
                if (TextFileName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("檔案名稱空白", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    return;
                }

                DB_Host.Connect();

                if (DB_Host.isConnection)
                {
                    DB_Command = string.Format("SELECT * FROM FILE_FORMAT WHERE FILE_NAME = '{0}' ", TextFileName.Text.Trim());

                    DB_Reader = DB_Host.ExecuteQuery(DB_Command);

                    if (DB_Reader.HasRows)
                    {
                        MessageBox.Show("檔案：[" + TextFileName.Text.Trim() + "]重覆新增。", "錯誤",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                        KeyPressEventArgs eKey = new KeyPressEventArgs((Char)Keys.Escape);
                        FormSouceSetting_KeyPress(sender, eKey);
                    }
                    else
                    {
                        DB_Command = string.Format(
                            "INSERT INTO FILE_FORMAT (TIME_STAMP, FILE_NAME, FILE_FORMAT, FILE_DESCRIPTION) VALUES (Getdate(), '{0}', '{1}', '{2}') ",
                                TextFileName.Text.Trim(), ComboFileFormat.Text.Trim(), TextFileDescription.Text.Trim());
                        logMessage = string.Format("=====新增設定=====\x0D\x0A檔案：[{0}]\x0D\x0A格式：[{1}]\x0D\x0A說明：[{2}]",
                                            TextFileName.Text.Trim(), ComboFileFormat.Text.Trim(), TextFileDescription.Text.Trim());
                        logger.Info(logMessage);

                    }

                    DB_Reader.Close();
                    DB_Host.ExecuteNonQuery(DB_Command);
                }

                DB_Command = string.Format("SELECT FILE_NAME, FILE_FORMAT, FILE_DESCRIPTION FROM FILE_FORMAT ORDER BY FILE_NAME");
                DB_Reader = DB_Host.ExecuteQuery(DB_Command);
                DB_Table = new DataTable();
                DB_Table.Load(DB_Reader);
                GridViewer.DataSource = DB_Table;
                GridViewer.Refresh();

                /* ================= Get add new row index =================
                 * http://stackoverflow.com/questions/13952660/get-row-index-in-datatable-from-a-certain-column
                 *  int? index = new System.Data.DataView(dt).ToTable(false, new[] { "1" })
                 * .AsEnumerable()
                 * .Select(row => row.Field<string>("1")) // ie. project the col(s) needed
                 * .ToList()
                 * .FindIndex(col => col == "G"); // returns 2
                 *
                 */

                int index = new System.Data.DataView(DB_Table).ToTable(false, new[] { "FILE_NAME" })
                                        .AsEnumerable()
                                        .Select(row => row.Field<string>("FILE_NAME")) // ie. project the col(s) needed
                                        .ToList()
                                        .FindIndex(col => col == TextFileName.Text.Trim()); // returns 2

                GridViewer.CurrentCell = GridViewer.Rows[index].Cells[0];
                GridViewer.Rows[index].Selected = true;


                DB_Host.Disconnect();

                BtnNewSource.Text = "新增設定";
                BtnDeleteSource.Enabled = true;
                BtnModifySource.Enabled = true;
                PanelSetting.Enabled = false;
                TextFileName.Text = "";
                ComboFileFormat.SelectedIndex = -1;
                TextFileDescription.Text = "";
            }

        }

        private void FormSouceSetting_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Escape)
            {
                GridViewer.Enabled = true;
                PanelSetting.Enabled = false;
                BtnNewSource.Enabled = true;
                BtnNewSource.Text = "新增設定";
                BtnModifySource.Enabled = true;
                BtnModifySource.Text = "修改設定";
                BtnDeleteSource.Enabled = true;
                BtnDeleteSource.Text = "刪除設定";
                TextFileName.Text = "";
                TextFileDescription.Text = "";

                ComboFileFormat.SelectedIndex = -1;
            }
        }

        private void BtnModifySource_Click(object sender, EventArgs e)
        {
            if (BtnModifySource.Text == "修改設定")
            {
                TextFileName.Text = GridViewer[0, GridViewer.CurrentRow.Index].Value.ToString();
                ComboFileFormat.SelectedIndex = ComboFileFormat.FindString(
                                    GridViewer[1, GridViewer.CurrentRow.Index].Value.ToString());
                TextFileDescription.Text = GridViewer[2, GridViewer.CurrentRow.Index].Value.ToString();

                BtnModifySource.Text = "確認修改";
                BtnNewSource.Enabled = false;
                BtnDeleteSource.Enabled = false;
                PanelSetting.Enabled = true;
                TextFileName.Enabled = false;
                TextFileName.Focus();
            }
            else if (BtnModifySource.Text == "確認修改")
            {
                DB_Host.Connect();

                if (DB_Host.isConnection)
                {

                    DB_Command = string.Format(
                            "UPDATE FILE_FORMAT SET TIME_STAMP = Getdate(), FILE_FORMAT = '{0}', FILE_DESCRIPTION = '{1}' WHERE FILE_NAME = '{2}'",
                                ComboFileFormat.Text.Trim(), TextFileDescription.Text.Trim(), TextFileName.Text.Trim());

                    DB_Reader.Close();
                    DB_Host.ExecuteNonQuery(DB_Command);
                }
                string fileName = TextFileName.Text.Trim();

                DB_Command = string.Format("SELECT FILE_NAME, FILE_FORMAT, FILE_DESCRIPTION FROM FILE_FORMAT ORDER BY FILE_NAME");
                DB_Reader = DB_Host.ExecuteQuery(DB_Command);
                DB_Table = new DataTable();
                DB_Table.Load(DB_Reader);
                GridViewer.DataSource = DB_Table;
                GridViewer.Refresh();

                /* ================= Get add new row index =================
                 * http://stackoverflow.com/questions/13952660/get-row-index-in-datatable-from-a-certain-column
                 *  int? index = new System.Data.DataView(dt).ToTable(false, new[] { "1" })
                 * .AsEnumerable()
                 * .Select(row => row.Field<string>("1")) // ie. project the col(s) needed
                 * .ToList()
                 * .FindIndex(col => col == "G"); // returns 2
                 *
                 */

                int index = new System.Data.DataView(DB_Table).ToTable(false, new[] { "FILE_NAME" })
                                        .AsEnumerable()
                                        .Select(row => row.Field<string>("FILE_NAME"))
                                        .ToList()
                                        .FindIndex(col => col == fileName);

                GridViewer.CurrentCell = GridViewer.Rows[index].Cells[0];
                GridViewer.Rows[index].Selected = true;

                DB_Host.Disconnect();


                BtnModifySource.Text = "修改設定";
                BtnNewSource.Enabled = true;
                BtnDeleteSource.Enabled = true;
                PanelSetting.Enabled = false;
                TextFileName.Enabled = true;
                TextFileName.Text = "";
                ComboFileFormat.SelectedIndex = -1;
                TextFileDescription.Text = "";
            }
        }

        private void GridViewer_SelectionChanged(object sender, EventArgs e)
        {
            if (BtnModifySource.Text == "確認修改")
            {
                TextFileName.Text = GridViewer[0, GridViewer.CurrentRow.Index].Value.ToString();
                ComboFileFormat.SelectedIndex = ComboFileFormat.FindString(
                                    GridViewer[1, GridViewer.CurrentRow.Index].Value.ToString());
                TextFileDescription.Text = GridViewer[2, GridViewer.CurrentRow.Index].Value.ToString();
            }
        }

        private void BtnDeleteSource_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("確定要刪除此設定?", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                DB_Host.Connect();

                if (DB_Host.isConnection)
                {

                    DB_Command = string.Format(
                            "DELETE FROM FILE_FORMAT WHERE FILE_NAME = '{0}'",
                                GridViewer[0, GridViewer.CurrentRow.Index].Value.ToString());

                    DB_Reader.Close();
                    DB_Host.ExecuteNonQuery(DB_Command);
                }
                string fileName = TextFileName.Text.Trim();

                DB_Command = string.Format("SELECT FILE_NAME, FILE_FORMAT, FILE_DESCRIPTION FROM FILE_FORMAT ORDER BY FILE_NAME");
                DB_Reader = DB_Host.ExecuteQuery(DB_Command);
                DB_Table = new DataTable();
                DB_Table.Load(DB_Reader);
                GridViewer.DataSource = DB_Table;
                GridViewer.Refresh();

                DB_Host.Disconnect();
            }
        }
    }
}
