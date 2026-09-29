using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.Galaxy.SoftKey
{
    public partial class FormSoftKey : UIForm
    {
        public FormSoftKey()
        {
            InitializeComponent();
            this.lblAlarm.Visible = false;
            this.lblCurDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        public string UserId
        {
            set
            {
                this.lblUserIdShow.Text = value;
            }
        }

        public DateTime AuthorizeDate
        {
            set
            {
                DateTime authoreseDate = value;
                this.lblAuthorizeDateShow.Text = authoreseDate.ToString("yyyy-MM-dd");

                if (authoreseDate < DateTime.Now)
                {
                    this.lblAlarm.Visible = true;
                    this.btnConfirm.Enabled = false;
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
