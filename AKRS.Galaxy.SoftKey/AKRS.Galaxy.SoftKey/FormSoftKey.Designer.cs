namespace AKRS.Galaxy.SoftKey
{
    partial class FormSoftKey
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
            this.lblCurDate = new System.Windows.Forms.Label();
            this.lblAuthorizeDateShow = new System.Windows.Forms.Label();
            this.lblUserIdShow = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblAlarm = new System.Windows.Forms.Label();
            this.lblUserId = new System.Windows.Forms.Label();
            this.btnCancel = new Sunny.UI.UIButton();
            this.btnConfirm = new Sunny.UI.UIButton();
            this.SuspendLayout();
            // 
            // lblCurDate
            // 
            this.lblCurDate.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCurDate.ForeColor = System.Drawing.Color.Blue;
            this.lblCurDate.Location = new System.Drawing.Point(212, 196);
            this.lblCurDate.Name = "lblCurDate";
            this.lblCurDate.Size = new System.Drawing.Size(129, 29);
            this.lblCurDate.TabIndex = 1;
            this.lblCurDate.Text = "2020-12-12";
            // 
            // lblAuthorizeDateShow
            // 
            this.lblAuthorizeDateShow.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAuthorizeDateShow.ForeColor = System.Drawing.Color.Red;
            this.lblAuthorizeDateShow.Location = new System.Drawing.Point(212, 142);
            this.lblAuthorizeDateShow.Name = "lblAuthorizeDateShow";
            this.lblAuthorizeDateShow.Size = new System.Drawing.Size(129, 29);
            this.lblAuthorizeDateShow.TabIndex = 2;
            this.lblAuthorizeDateShow.Text = "2021-12-12";
            // 
            // lblUserIdShow
            // 
            this.lblUserIdShow.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblUserIdShow.ForeColor = System.Drawing.Color.Blue;
            this.lblUserIdShow.Location = new System.Drawing.Point(221, 90);
            this.lblUserIdShow.Name = "lblUserIdShow";
            this.lblUserIdShow.Size = new System.Drawing.Size(129, 29);
            this.lblUserIdShow.TabIndex = 3;
            this.lblUserIdShow.Text = "99999";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(103, 196);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 21);
            this.label4.TabIndex = 4;
            this.label4.Text = "当前时间：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(103, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 21);
            this.label2.TabIndex = 5;
            this.label2.Text = "授权时间：";
            // 
            // lblAlarm
            // 
            this.lblAlarm.AutoSize = true;
            this.lblAlarm.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAlarm.ForeColor = System.Drawing.Color.Red;
            this.lblAlarm.Location = new System.Drawing.Point(48, 57);
            this.lblAlarm.Name = "lblAlarm";
            this.lblAlarm.Size = new System.Drawing.Size(351, 19);
            this.lblAlarm.TabIndex = 6;
            this.lblAlarm.Text = "授权已到期，请联系艾科瑞思重新授权！";
            // 
            // lblUserId
            // 
            this.lblUserId.AutoSize = true;
            this.lblUserId.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblUserId.ForeColor = System.Drawing.Color.Blue;
            this.lblUserId.Location = new System.Drawing.Point(104, 90);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(129, 29);
            this.lblUserId.TabIndex = 7;
            this.lblUserId.Text = "用户号：";
            // 
            // btnCancel
            // 
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(107, 232);
            this.btnCancel.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "退出";
            this.btnCancel.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfirm.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.Location = new System.Drawing.Point(250, 232);
            this.btnConfirm.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(100, 35);
            this.btnConfirm.TabIndex = 9;
            this.btnConfirm.Text = "继续";
            this.btnConfirm.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // FormSoftKey
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(469, 291);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblCurDate);
            this.Controls.Add(this.lblAuthorizeDateShow);
            this.Controls.Add(this.lblUserIdShow);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblAlarm);
            this.Controls.Add(this.lblUserId);
            this.Name = "FormSoftKey";
            this.Text = "授权提示";
            this.ZoomScaleRect = new System.Drawing.Rectangle(15, 15, 800, 450);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCurDate;
        private System.Windows.Forms.Label lblAuthorizeDateShow;
        private System.Windows.Forms.Label lblUserIdShow;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblAlarm;
        private System.Windows.Forms.Label lblUserId;
        private Sunny.UI.UIButton btnCancel;
        private Sunny.UI.UIButton btnConfirm;
    }
}