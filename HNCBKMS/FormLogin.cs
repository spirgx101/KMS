using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Principal;

using log4net;
using log4net.Config;


namespace HNCBKMS
{
    public partial class FormLogin : Form
    {
        public string UserName = string.Empty;
        public string Password = string.Empty;
        private bool isLogin = false;

        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

        public FormLogin()
        {
            InitializeComponent();
            TextUser.Focus();
            this.AcceptButton = BtnLogin;
            XmlConfigurator.Configure(new System.IO.FileInfo("./log4net.config"));
        }

        private void BtnCancelLogin_Click(object sender, EventArgs e)
        {
            logger.Info("取消登入");
            logger.Info("密碼管理系統 結束程式");
            Application.Exit();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            UserName = TextUser.Text;
            Password = TextPassword.Text;

            try
            {
                using (PrincipalContext adContext = new PrincipalContext(ContextType.Machine))
                {
                    if (adContext.ValidateCredentials(UserName, Password))
                    {
                        isLogin = true;
                        logger.Info("使用者：[" + UserName + "]登入成功。");
                        this.Close();
                    }
                    else
                    {
                        isLogin = false;
                        TextUser.Text = "";
                        TextPassword.Text = "";
                        this.AcceptButton = BtnLogin;
                        TextUser.Focus();
                        logger.Error("使用者：[" + UserName + "]登入失敗。");
                        MessageBox.Show("登入失敗，帳號或是密碼錯誤。", "錯誤",
                            MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public bool LoginStatus()
        {
            return isLogin;
        }
    }
}
