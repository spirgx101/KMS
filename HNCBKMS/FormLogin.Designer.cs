namespace HNCBKMS
{
    partial class FormLogin
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
            this.BtnCancelLogin = new System.Windows.Forms.Button();
            this.BtnLogin = new System.Windows.Forms.Button();
            this.TextPassword = new System.Windows.Forms.TextBox();
            this.TextUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnCancelLogin
            // 
            this.BtnCancelLogin.Location = new System.Drawing.Point(141, 97);
            this.BtnCancelLogin.Name = "BtnCancelLogin";
            this.BtnCancelLogin.Size = new System.Drawing.Size(103, 31);
            this.BtnCancelLogin.TabIndex = 13;
            this.BtnCancelLogin.Text = "取        消";
            this.BtnCancelLogin.UseVisualStyleBackColor = true;
            this.BtnCancelLogin.Click += new System.EventHandler(this.BtnCancelLogin_Click);
            // 
            // BtnLogin
            // 
            this.BtnLogin.Location = new System.Drawing.Point(12, 97);
            this.BtnLogin.Name = "BtnLogin";
            this.BtnLogin.Size = new System.Drawing.Size(103, 31);
            this.BtnLogin.TabIndex = 12;
            this.BtnLogin.Text = "登        入";
            this.BtnLogin.UseVisualStyleBackColor = true;
            this.BtnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // TextPassword
            // 
            this.TextPassword.Location = new System.Drawing.Point(71, 52);
            this.TextPassword.Name = "TextPassword";
            this.TextPassword.PasswordChar = '●';
            this.TextPassword.Size = new System.Drawing.Size(173, 33);
            this.TextPassword.TabIndex = 11;
            // 
            // TextUser
            // 
            this.TextUser.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.TextUser.Location = new System.Drawing.Point(71, 11);
            this.TextUser.Name = "TextUser";
            this.TextUser.Size = new System.Drawing.Size(173, 33);
            this.TextUser.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 24);
            this.label2.TabIndex = 9;
            this.label2.Text = "密碼：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 24);
            this.label1.TabIndex = 8;
            this.label1.Text = "帳號：";
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 140);
            this.Controls.Add(this.BtnCancelLogin);
            this.Controls.Add(this.BtnLogin);
            this.Controls.Add(this.TextPassword);
            this.Controls.Add(this.TextUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登入";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnCancelLogin;
        private System.Windows.Forms.Button BtnLogin;
        private System.Windows.Forms.TextBox TextPassword;
        private System.Windows.Forms.TextBox TextUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}