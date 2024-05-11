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
using System.Net.Sockets;
using HSMClient;
using log4net;
using log4net.Config;
using System.Threading;
using System.Diagnostics;

namespace HNCBKMS
{
    public partial class FormMain : Form
    {

        #region 全域變數
        private string Auto = string.Empty;
        private string ProcessStartInfo = string.Empty;
        private string InputFolder = string.Empty;
        private string OutputFolder = string.Empty;
        private string InitialFolder = string.Empty;
        private string SetupFolder = string.Empty;
        private string CardCaryFolder = string.Empty;
        private string PPCardFolder = string.Empty;
        private string HSM_IP = string.Empty;
        private int HSM_Port = 0;
        private XMLProcess xml = null;
        private Database DB_Host = null;
        private Database DB_Remote = null;
        private SqlDataReader DB_Reader = null;
        private string DB_Command = string.Empty;
        private DataTable DB_Table = null;
        private bool LoginStatus = true;
        private int PIN_Length = 0;


        private String logMessage = String.Empty;
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));
        #endregion

        public FormMain()
        {
            InitializeComponent();
            InitialSetting();
            if (Auto == "1")
            {
                object sender = new object();
                EventArgs e = new EventArgs();
                AutoProcess(sender, e);
            }

            //Login();
        }

        private void AutoProcess(object sender, EventArgs e)
        {
            BtnDownload_Click(sender, e);
            BtnConvert_Click(sender, e);
            BtnTransPIN_Click(sender, e);
            //BtnExit_Click(sender, e);
            RunMergeProcess();
        }

        private void RunMergeProcess()
        {
            ProcessStartInfo start = new ProcessStartInfo(ProcessStartInfo);
            start.WindowStyle = ProcessWindowStyle.Normal;
            Process.Start(start);
        }

        private void Login()
        {
            FormLogin formLogin = new FormLogin();
            formLogin.ShowDialog();
            LoginStatus = formLogin.LoginStatus();
            LabelUser.Text = "登入者：" + formLogin.UserName;
        }

        private void InitialSetting()
        {
            try
            {

                //Directory.CreateDirectory(@"Log");
                XmlConfigurator.Configure(new System.IO.FileInfo("./log4net.config"));
                logger.Info("密碼管理系統 執行程式");

                xml = new XMLProcess(Application.StartupPath + "/SETUP.XML");
                Auto = xml.SelectInnerText("setup/auto");
                InputFolder = xml.SelectInnerText("setup/input-folder");
                OutputFolder = xml.SelectInnerText("setup/output-folder");
                InitialFolder = xml.SelectInnerText("setup/initial-folder");
                SetupFolder = xml.SelectInnerText("setup/setup-folder");
                CardCaryFolder = xml.SelectInnerText("setup/card-carry-folder");
                PPCardFolder = xml.SelectInnerText("setup/pp-card-folder");
                ProcessStartInfo = xml.SelectInnerText("setup/process-start-info");

                #region List Input File
                foreach (string fname in System.IO.Directory.GetFileSystemEntries(InputFolder))
                {
                    if (Directory.Exists(fname)) continue;
                    FileList.Items.Add(Path.GetFileName(fname));
                }
                #endregion

                #region HSM Setting
                xml = new XMLProcess(SetupFolder + "HSM.XML");
                HSM_IP = xml.SelectInnerText("hsm/ip");
                Int32.TryParse(xml.SelectInnerText("hsm/port"), out HSM_Port);
                #endregion

                #region Remote Database Setting
                xml = new XMLProcess(SetupFolder + "DATABASE.XML");
                string DB_Server_Remote = xml.SelectInnerText("database/remote/server-location").Trim();
                string DB_Name_Remote = xml.SelectInnerText("database/remote/database-name").Trim();
                string DB_User_Remote = xml.SelectInnerText("database/remote/user-name").Trim();
                string DB_Password_Remote = xml.SelectInnerText("database/remote/password").Trim();
                int DB_Timeout_Remote = 0;
                Int32.TryParse(xml.SelectInnerText("database/remote/connect-timeout"), out DB_Timeout_Remote);

                if ((DB_User_Remote.Trim() == "") || (DB_Password_Remote.Trim() == ""))
                {
                    DB_Remote = new Database(DB_Server_Remote, DB_Name_Remote,
                        (DB_Timeout_Remote == 0) ? DB_Timeout_Remote = 10 : DB_Timeout_Remote);
                }
                else
                {
                    DB_Remote = new Database(DB_Server_Remote, DB_Name_Remote, DB_User_Remote, DB_Password_Remote,
                        (DB_Timeout_Remote == 0) ? DB_Timeout_Remote = 10 : DB_Timeout_Remote);
                }
                #endregion

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

                PanelLog.Visible = false;

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            PanelLog.Visible = false;

            xml = new XMLProcess(SetupFolder + "FTP.XML");

            string ftpServer = xml.SelectInnerText("ftp/server-location");
            string ftpPort = xml.SelectInnerText("ftp/server-port");
            string ftpUserID = xml.SelectInnerText("ftp/user-name");
            string ftpPassword = xml.SelectInnerText("ftp/password");

            FTP ftp = new FTP(ftpServer, ftpPort, ftpUserID, ftpPassword);

            string[] ftpFileList = ftp.ListDirectory("");

            foreach (string fname in ftpFileList)
            {
                ftp.DownloadFile(fname, InputFolder+fname);
            }

            FileList.Items.Clear();
            #region List Input File
            foreach (string fname in System.IO.Directory.GetFileSystemEntries(InputFolder))
            {
                if (Directory.Exists(fname)) continue;
                FileList.Items.Add(Path.GetFileName(fname));
            }
            #endregion
            FileList.Refresh();
            FileList.ClearSelected();
        }

        private void PanelMain_Resize(object sender, EventArgs e)
        {
            FileList.Height = PanelMain.Height - 65;
            BtnRefresh.Top = PanelMain.Height - 45;
            BtnSelectAll.Top = PanelMain.Height - 45;
            StatusLabel.Width = PanelMain.Width / 2;
            DataViewer.Width = PanelMain.Width - 420;
            DataViewer.Height = PanelMain.Height - 35;

            LogViewer.Width = PanelLog.Width - 35;
            LogViewer.Height = PanelLog.Height - 120;

        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            FileList.Items.Clear();
            #region List Input File
            foreach (string fname in System.IO.Directory.GetFileSystemEntries(InputFolder))
            {
                if (Directory.Exists(fname)) continue;
                FileList.Items.Add(Path.GetFileName(fname));
            }
            #endregion
            FileList.Refresh();
            FileList.ClearSelected();
        }

        private void BtnConvert_Click(object sender, EventArgs e)
        {
            FileStream ifs = null;
            FileStream ofs = null;
            int recordLength = 0;
            int fileTotalRecord = 0;
            int recordCount = 0;
            string record = string.Empty;
            int[] card_type = new int[2] { 0, 0 };
            int[] card_nmbr = new int[2] { 0, 0 };
            int[] expir_yymm = new int[2] { 0, 0 };
            int[] service_code = new int[2] { 0, 0 };
            int[] pvki = new int[2] { 0, 0 };
            int[] trk1_pvv = new int[2] { 0, 0 };
            int[] trk1_cvv = new int[2] { 0, 0 };
            int[] trk2_pvv = new int[2] { 0, 0 };
            int[] trk2_cvv = new int[2] { 0, 0 };
            int[] cvc2 = new int[2] { 0, 0 };
            int[] icvv = new int[2] { 0, 0 };
            int[] pin = new int[2] { 0, 0 };
            string fileFormat = string.Empty;
            string isICCard = string.Empty;
            string isPPCard = string.Empty;
            string isCardCarry = string.Empty;

            PanelLog.Visible = false;

            Array.ForEach(Directory.GetFiles(OutputFolder), File.Delete);

            if (Auto == "1")
            {
                for (int i = 0; i < FileList.Items.Count; i++)
                {
                    FileList.SetSelected(i, true);
                }
            }

            if (FileList.SelectedItems.Count == 0)
            {
                MessageBox.Show("沒有選擇檔案", "警告", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                return;
            }

            if (!DB_Host.Connect())
            {
                logMessage = "資料庫無法連線";
                logger.Error(logMessage);
                MessageBox.Show(logMessage, "錯誤", MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            foreach (string fname in FileList.SelectedItems)
            {
                logMessage = string.Format("檔案：[{0}]開始轉檔", fname);
                StatusLabel.Text = logMessage;
                logger.Info(logMessage);
                if (DB_Host.isConnection)
                {
                    DB_Command = string.Format("SELECT FILE_FORMAT FROM FILE_FORMAT WHERE FILE_NAME = '{0}' ", fname);
                    DB_Reader = DB_Host.ExecuteQuery(DB_Command);
                    if (DB_Reader.HasRows)
                    {
                        DB_Table = new DataTable();
                        DB_Table.Load(DB_Reader);
                        fileFormat = DB_Table.Rows[0][0].ToString();
                        DB_Reader.Close();
                    }
                    else
                    {
                        logMessage = "找不到檔案：[" + fname + "]的資料格式設定，請到[來源檔設定]新增設定。";
                        logger.Error(logMessage);
                        MessageBox.Show(logMessage, "錯誤",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                }

                try
                {
                    xml = new XMLProcess(InitialFolder + fileFormat);
                    #region 讀取EMBOSSIN.DAF欄位定義
                    Int32.TryParse(xml.SelectInnerText("format/info/record-length"), out recordLength);
                    isICCard = xml.SelectInnerText("format/info/ic-card");
                    isPPCard = xml.SelectInnerText("format/info/pp-card");
                    isCardCarry = xml.SelectInnerText("format/info/card-carry");

                    if (isCardCarry == "1")
                    {
                        File.Copy(InputFolder + fname, CardCaryFolder + fname, true);
                        continue;
                    }

                    if (isPPCard == "1")
                    {
                        File.Copy(InputFolder + fname, PPCardFolder + fname, true);
                        continue;
                    }

                    Int32.TryParse(xml.SelectInnerText("format/field/card-type"), out card_type[0]);
                    Int32.TryParse(xml.SelectInnerText("format/field-length/card-type"), out card_type[1]);

                    Int32.TryParse(xml.SelectInnerText("format/field/trk1-card-nmbr"), out card_nmbr[0]);
                    Int32.TryParse(xml.SelectInnerText("format/field-length/trk1-card-nmbr"), out card_nmbr[1]);

                    Int32.TryParse(xml.SelectInnerText("format/field/trk1-expir-yymm"), out expir_yymm[0]);
                    Int32.TryParse(xml.SelectInnerText("format/field-length/trk1-expir-yymm"), out expir_yymm[1]);

                    Int32.TryParse(xml.SelectInnerText("format/field/trk1-service"), out service_code[0]);
                    Int32.TryParse(xml.SelectInnerText("format/field-length/trk1-service"), out service_code[1]);

                    Int32.TryParse(xml.SelectInnerText("format/field/trk1-pvki"), out pvki[0]);
                    Int32.TryParse(xml.SelectInnerText("format/field-length/trk1-pvki"), out pvki[1]);

                    Int32.TryParse(xml.SelectInnerText("format/field/trk1-pvv"), out trk1_pvv[0]);
                    Int32.TryParse(xml.SelectInnerText("format/field-length/trk1-pvv"), out trk1_pvv[1]);

                    Int32.TryParse(xml.SelectInnerText("format/field/trk1-cvv"), out trk1_cvv[0]);
                    Int32.TryParse(xml.SelectInnerText("format/field-length/trk1-cvv"), out trk1_cvv[1]);

                    Int32.TryParse(xml.SelectInnerText("format/field/trk2-pvv"), out trk2_pvv[0]);
                    Int32.TryParse(xml.SelectInnerText("format/field-length/trk2-pvv"), out trk2_pvv[1]);

                    Int32.TryParse(xml.SelectInnerText("format/field/trk2-cvv"), out trk2_cvv[0]);
                    Int32.TryParse(xml.SelectInnerText("format/field-length/trk2-cvv"), out trk2_cvv[1]);

                    Int32.TryParse(xml.SelectInnerText("format/field/cvc2"), out cvc2[0]);
                    Int32.TryParse(xml.SelectInnerText("format/field-length/cvc2"), out cvc2[1]);

                    Int32.TryParse(xml.SelectInnerText("format/field-length/pin"), out pin[1]);
                    PIN_Length = pin[1];

                    Int32.TryParse(xml.SelectInnerText("format/field/pin"), out pin[0]);

                    Int32.TryParse(xml.SelectInnerText("format/field/icvv"), out icvv[0]);
                    Int32.TryParse(xml.SelectInnerText("format/field-length/icvv"), out icvv[1]);

                    #endregion

                    if (recordLength == 0)
                        throw new Exception("EMBOSSIN.DAF的資料長度設定錯誤。");

                    ifs = new FileStream(InputFolder + Path.GetFileName(fname), FileMode.Open, FileAccess.Read);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return;
                }

                StreamReader br = new StreamReader(ifs);

                recordCount = 0;

                while ((record = br.ReadLine()) != null)
                {
                    recordCount++;

                    if (record.Length != (recordLength - 2))          //減去\x0D\x0A兩個Bytes
                    {
                        logMessage = "檔案：[" + fname + "]-第[" + recordCount + "]資料長度錯誤";
                        StatusLabel.Text = logMessage;
                        logger.Error(logMessage);
                        MessageBox.Show(logMessage, "錯誤",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    record = record.PadRight(800, ' ');

                    if (!FinishTrackData(ref record, card_nmbr, expir_yymm, service_code, pvki,
                                            trk1_pvv, trk1_cvv, trk2_pvv, trk2_cvv, cvc2, pin, icvv, isICCard))
                    {
                        logMessage = "CVV、PVV計算錯誤，轉檔失敗，可能是HSM無法建立連線，或參數錯誤。";
                        logger.Error(logMessage);
                        MessageBox.Show(logMessage, "錯誤",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    string outputFilePath = OutputFolder + Path.GetFileName(record.Substring(card_type[0], card_type[1]) + ".TXT");
                    using (ofs = File.Open(outputFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        ofs.Seek(0, SeekOrigin.End);
                        StreamWriter sw = new StreamWriter(ofs);
                        sw.WriteLine(record);
                        sw.Flush();
                    }
                    Application.DoEvents();
                    Thread.Sleep(1);
                }

                logMessage = string.Format("檔案：[{0}]轉檔成功，共[{1}]筆資料。", fname, recordCount);
                //Thread.Sleep(1000);
                StatusLabel.Text = logMessage;
                logger.Info(logMessage);
                ifs.Close();
                ifs.Dispose();
            }

            DB_Command = string.Format("SELECT * FROM ATM_PIN ORDER BY TIME_STAMP");
            DB_Reader = DB_Host.ExecuteQuery(DB_Command);
            DB_Table = new DataTable();
            DB_Table.Load(DB_Reader);
            DataViewer.DataSource = DB_Table;
            DataViewer.Columns[0].DefaultCellStyle.Format = "yyyy/MM/dd HH:mm:ss";
            DataViewer.Refresh();
            DB_Reader.Close();

            DB_Host.Disconnect();
            FileList.Refresh();
            FileList.ClearSelected();
            if (Auto == "1")
                StatusLabel.Text = "轉檔成功！";
            else
                MessageBox.Show("轉檔成功！");
        }

        private bool FinishTrackData(ref string record, int[] card_nmbr, int[] expir_yymm, int[] service_code,
                                        int[] pvki, int[] trk1_pvv, int[] trk1_cvv, int[] trk2_pvv, int[] trk2_cvv,
                                        int[] cvc2, int[] pin, int[] icvv, string isICCard)
        {
            int PPK_index = 0, DPK_index = 0, CVK_index = 0, PVK_index = 0;
            int clear_pin = 0;
            string recPinBlock = string.Empty, clearPinBlock = string.Empty;
            string recCVV = string.Empty;
            string recCVC2 = string.Empty;
            string recICVV = string.Empty;
            string recPVV = string.Empty;

            string recPAN = record.Substring(card_nmbr[0], card_nmbr[1]);
            string recExpire = record.Substring(expir_yymm[0], expir_yymm[1]);
            string recServiceCode = record.Substring(service_code[0], service_code[1]);
            string recPVKI = record.Substring(pvki[0], pvki[1]);

            GetKeyIndex(recPAN, ref PPK_index, ref DPK_index, ref CVK_index, ref PVK_index);

            //logMessage = string.Format("PPK = {0}, DPK = {1},CVK = {2}, PVK = {3}",
            //            PPK_index.ToString(), DPK_index.ToString(), CVK_index.ToString(), PVK_index.ToString());
            //logger.Info(logMessage);

            try
            {
                LunaEFT HSM = new LunaEFT(new TcpClient(HSM_IP, HSM_Port));
                HSM.initOps();

                recPinBlock = HSM.LunaEFT_EE0E04_PIN_GENERATE(pin[1], LunaEFT.PIN_Block_Formats.ISO3, recPAN, LunaEFT.HSM_Key_Specifier_Formats.Long_BCD, PPK_index);
                clearPinBlock = HSM.LunaEFT_EE0801_DECIPHER(LunaEFT.HSM_Key_Specifier_Formats.Long_BCD, DPK_index, recPinBlock, LunaEFT.Chiper_Mode.ECB);

                clearPinBlock = AXorB(clearPinBlock, recPAN.Substring(4).PadLeft(16, '0'));

                recCVV = HSM.LunaEFT_EE0802_CVV_GENERATE(LunaEFT.HSM_Key_Specifier_Formats.Long_BCD, CVK_index, recPAN, recExpire, recServiceCode);
                recCVC2 = HSM.LunaEFT_EE0802_CVV_GENERATE(LunaEFT.HSM_Key_Specifier_Formats.Long_BCD, CVK_index, recPAN, recExpire, "000");
                recICVV = HSM.LunaEFT_EE0802_CVV_GENERATE(LunaEFT.HSM_Key_Specifier_Formats.Long_BCD, CVK_index, recPAN, recExpire, "999");
                recPVV = HSM.LunaEFT_EE0607_PVV_CALC(recPinBlock, LunaEFT.HSM_Key_Specifier_Formats.Long_BCD, PPK_index, LunaEFT.PIN_Block_Formats.ISO3,
                                                        recPAN, recPVKI, LunaEFT.HSM_Key_Specifier_Formats.Long_BCD, PVK_index);

                HSM.TermClient();
                HSM = null;

                record = record.Remove(trk1_cvv[0], trk1_cvv[1]).Insert(trk1_cvv[0], recCVV);
                record = record.Remove(trk1_pvv[0], trk1_pvv[1]).Insert(trk1_pvv[0], recPVV);
                record = record.Remove(trk2_cvv[0], trk2_cvv[1]).Insert(trk2_cvv[0], recCVV);
                record = record.Remove(trk2_pvv[0], trk2_pvv[1]).Insert(trk2_pvv[0], recPVV);
                record = record.Remove(cvc2[0], cvc2[1]).Insert(cvc2[0], recCVC2);
                //==========================================================================================
                //測試時寫入明碼PIN於資料中，正式上線必須移除。
                record = record.Remove(pin[0], pin[1]).Insert(pin[0], clearPinBlock.Substring(2, pin[1]));
                //==========================================================================================
                record = record.Remove(icvv[0], icvv[1]).Insert(icvv[0], recICVV);

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return false;
            }

            #region 備份資料到Host Database
            if (DB_Host.isConnection)
            {
                DB_Command = string.Format("SELECT * FROM ATM_PIN WHERE CARD_NBR = '{0}' AND EXPIRY_DAY = '{1}'", recPAN, recExpire);

                DB_Reader = DB_Host.ExecuteQuery(DB_Command);

                if (DB_Reader.HasRows)
                {
                    DB_Command = string.Format(
                        "UPDATE ATM_PIN SET TIME_STAMP = Getdate(), CVV1_SVC = '{0}', PVKI = '{1}', ENCRYPT_PIN = '{2}', CVV = '{3}', CVC2 = '{4}', ICVV = '{5}', PVV = '{6}' WHERE CARD_NBR = '{7}' AND EXPIRY_DAY = '{8}'",
                        recServiceCode, recPVKI, recPinBlock, recCVV, recCVC2, recICVV, recPVV, recPAN, recExpire);
                }
                else
                {
                    DB_Command = string.Format(
                        "INSERT INTO ATM_PIN (TIME_STAMP, CARD_NBR, EXPIRY_DAY, CVV1_SVC, PVKI, ENCRYPT_PIN, CVV, CVC2, ICVV, PVV) VALUES ( Getdate() ,'{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}') ",
                         recPAN, recExpire, recServiceCode, recPVKI, recPinBlock, recCVV, recCVC2, recICVV, recPVV);
                }
                DB_Reader.Close();

                DB_Host.ExecuteNonQuery(DB_Command);
            }
            #endregion

            return true;
        }

        private void GetKeyIndex(string PAN, ref int PPK_index, ref int DPK_index, ref int CVK_index, ref int PVK_index)
        {
            xml = new XMLProcess(SetupFolder + "KEY.XML");
            Int32.TryParse(xml.SelectInnerText("key/ppk-index"), out PPK_index);
            Int32.TryParse(xml.SelectInnerText("key/dpk-index"), out DPK_index);

            switch (PAN.Substring(0, 1))
            {
                case "3":
                    Int32.TryParse(xml.SelectInnerText("key/jcb/cvvk-index"), out CVK_index);
                    Int32.TryParse(xml.SelectInnerText("key/jcb/pvvk-index"), out PVK_index);
                    break;
                case "4":
                    Int32.TryParse(xml.SelectInnerText("key/visa/cvvk-index"), out CVK_index);
                    Int32.TryParse(xml.SelectInnerText("key/visa/pvvk-index"), out PVK_index);
                    break;
                case "5":
                    Int32.TryParse(xml.SelectInnerText("key/mastercard/cvvk-index"), out CVK_index);
                    Int32.TryParse(xml.SelectInnerText("key/mastercard/pvvk-index"), out PVK_index);
                    break;
            }

        }

        private void BtnSourceConfigure_Click(object sender, EventArgs e)
        {
            //PanelLog.Visible = false;
            FormSouceSetting formSource = new FormSouceSetting();
            formSource.Show();
            formSource.FormClosing += new FormClosingEventHandler(formSource_FormClosing);
            this.WindowState = FormWindowState.Minimized;
            this.Enabled = false;
        }

        private void formSource_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Enabled = true;
        }

        private void BtnLog_Click(object sender, EventArgs e)
        {
            PanelLog.Visible = true;
            LogViewer.Width = PanelLog.Width - 35;
            LogViewer.Height = PanelLog.Height - 120;
        }

        private void BtnTransPIN_Click(object sender, EventArgs e)
        {
            PanelLog.Visible = false;
            string clearPin = string.Empty;
            int DPK_index = 0;

            if (Auto == "0")
            {
                if (MessageBox.Show("確定今天已完成轉檔，才傳送預借現金密碼？", "傳送密碼", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            DB_Host.Connect();

            DB_Command = string.Format("SELECT * FROM ATM_PIN");
            DB_Reader = DB_Host.ExecuteQuery(DB_Command);
            DB_Table = new DataTable();
            DB_Table.Load(DB_Reader);

            DB_Reader.Close();
            DB_Host.Disconnect();


            DB_Remote.Connect();

            if (DB_Remote.isConnection)
            {
                for (int i = 0; i < DB_Table.Rows.Count; i++)
                {
                    DB_Command = string.Format("SELECT * FROM NX_ATM_PIN WHERE CARD_NBR = '{0}' AND EXPIRY_DAY = '{1}'",
                                                DB_Table.Rows[i]["CARD_NBR"], DB_Table.Rows[i]["EXPIRY_DAY"]);

                    DB_Reader = DB_Remote.ExecuteQuery(DB_Command);

                    try
                    {
                        xml = new XMLProcess(SetupFolder + "KEY.XML");
                        Int32.TryParse(xml.SelectInnerText("key/dpk-index"), out DPK_index);

                        LunaEFT HSM = new LunaEFT(new TcpClient(HSM_IP, HSM_Port));
                        HSM.initOps();

                        clearPin = HSM.LunaEFT_EE0801_DECIPHER(LunaEFT.HSM_Key_Specifier_Formats.Long_BCD, DPK_index,
                                                        DB_Table.Rows[i]["ENCRYPT_PIN"].ToString(), LunaEFT.Chiper_Mode.ECB);

                        clearPin = AXorB(clearPin, DB_Table.Rows[i]["CARD_NBR"].ToString().Substring(4).PadLeft(16, '0')).Substring(2, PIN_Length);

                        HSM.TermClient();
                        HSM = null;
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                        MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        MessageBox.Show("傳送PIN碼失敗", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    if (DB_Reader.HasRows)
                    {
                        DB_Command = string.Format(
                            "UPDATE NX_ATM_PIN SET PIN = '{0}' WHERE CARD_NBR = '{1}' AND EXPIRY_DAY = '{2}'",
                            clearPin, DB_Table.Rows[i]["CARD_NBR"], DB_Table.Rows[i]["EXPIRY_DAY"]);
                    }
                    else
                    {
                        DB_Command = string.Format(
                            "INSERT INTO NX_ATM_PIN (CARD_NBR, EXPIRY_DAY, CVV1_SVC, PIN) VALUES ('{0}', '{1}', '{2}', '{3}') ",
                            DB_Table.Rows[i]["CARD_NBR"], DB_Table.Rows[i]["EXPIRY_DAY"], DB_Table.Rows[i]["CVV1_SVC"], clearPin);
                    }
                    DB_Reader.Close();

                    DB_Remote.ExecuteNonQuery(DB_Command);
                }
            }

            DB_Remote.Disconnect();

            if (Auto == "1")
                StatusLabel.Text = "傳送預借現金密碼完成。";
            else
                MessageBox.Show("傳送預借現金密碼完成。", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            PanelLog.Visible = false;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {

            string startDateTime = PickerStartDate.Value.ToString("yyyy/MM/dd") + " " +
                                   PickerStartTime.Value.ToString("HH:mm:ss");

            string endDateTime = PickerEndDate.Value.ToString("yyyy/MM/dd") + " " +
                                 PickerEndTime.Value.ToString("HH:mm:ss");


            DB_Host.Connect();
            DB_Command = string.Format("SELECT Date, Thread, [Level], Logger, Message, Exception FROM LOG WHERE Date BETWEEN '{0}' AND '{1}' ORDER BY Date ", startDateTime, endDateTime);
            DB_Reader = DB_Host.ExecuteQuery(DB_Command);
            DB_Table = new DataTable();
            DB_Table.Load(DB_Reader);
            LogViewer.DataSource = DB_Table;
            LogViewer.Columns[0].DefaultCellStyle.Format = "yyyy/MM/dd HH:mm:ss";
            LogViewer.Refresh();
            LogViewer.Focus();
            DB_Reader.Close();

            DB_Host.Disconnect();


        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (LoginStatus == true)
            {
                StatusLabel.Text = "關閉程式";
                DB_Host.Connect();
                DB_Command = string.Format("TRUNCATE TABLE ATM_PIN; ");
                DB_Host.ExecuteNonQuery(DB_Command);
                DB_Host.Disconnect();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (LoginStatus == true)
            {
                if (MessageBox.Show("是否確定要關閉程式", "關閉程式", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                logger.Info("密碼管理系統 結束程式");
            }
        }

        /// <summary>
        /// Pack ex:string "10" -> Byte 0x10
        /// </summary>
        /// <param name="sInput">Input Data</param>
        /// <returns></returns>
        public byte[] Pack(string sInput)
        {
            byte[] returnBytes = new byte[sInput.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(sInput.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        /// <summary>
        /// AXorB
        /// </summary>
        /// <param name="A">Data A</param>
        /// <param name="B">Data B</param>
        /// <returns></returns>
        public string AXorB(string A, string B)
        {
            byte[] ByteA = Pack(A);
            byte[] ByteB = Pack(B);
            byte[] ByteC = new byte[8];
            for (int i = 0; i < ByteA.Length; i++)
            {
                ByteC[i] = (byte)(ByteA[i] ^ ByteB[i]);
            }
            return BitConverter.ToString(ByteC).Replace("-", "");
        }

        private void Btn_DeleteFile_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("確定要刪除來源檔", "刪除檔案", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            if (System.IO.Directory.Exists(InputFolder))
            {

                // Copy the files and overwrite destination files if they already exist.
                foreach (string file in System.IO.Directory.GetFiles(InputFolder))
                {
                    // Use static Path methods to extract only the file name from the path.
                    File.Delete(file);
                }
            }

            FileList.Items.Clear();
        }

        private void Btn_SelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < FileList.Items.Count; i++)
            {
                FileList.SetSelected(i, true);
            }
        }

    }
}
