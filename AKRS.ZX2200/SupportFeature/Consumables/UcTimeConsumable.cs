using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.Consumables
{
    using AKRS.ZX2200.SupportFeature.Consumables;

    /// <summary>
    /// 事件耗材
    /// </summary>
    public partial class UcTimeConsumable : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UcTimeConsumable()
        {
            this.InitializeComponent();
            //this.dateEdit1.Properties.DisplayFormat.FormatString = "yy-MM-dd HH:mm:ss";
            //this.dateEdit1.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            //this.dateEdit2.Properties.DisplayFormat.FormatString = "yy-MM-dd HH:mm:ss";
            //this.dateEdit2.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            //this.dateEdit1.Properties.EditFormat.FormatString = "yy-MM-dd HH:mm:ss";
            //this.dateEdit1.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            //this.dateEdit2.Properties.EditFormat.FormatString = "yy-MM-dd HH:mm:ss";
            //this.dateEdit2.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;

            this.dateEdit1.Properties.DisplayFormat.FormatString = "yy-MM-dd HH:mm:ss";
            this.dateEdit1.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEdit1.Properties.EditFormat.FormatString = "yy-MM-dd HH:mm:ss";
            this.dateEdit1.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEdit1.Properties.Mask.EditMask = "yy-MM-dd HH:mm:ss";

            this.dateEdit2.Properties.DisplayFormat.FormatString = "yy-MM-dd HH:mm:ss";
            this.dateEdit2.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEdit2.Properties.EditFormat.FormatString = "yy-MM-dd HH:mm:ss";
            this.dateEdit2.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEdit2.Properties.Mask.EditMask = "yy-MM-dd HH:mm:ss";


            this.dateEdit2.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            this.dateEdit2.Properties.VistaEditTime = DevExpress.Utils.DefaultBoolean.True;

            this.dateEdit1.Enabled = false;
            // this.dateEdit2.Enabled = false;
            this.SpRemainingTime.Enabled = false;
        }

        /// <summary>
        /// 耗材
        /// </summary>
        private TimeConsumable timeConsumable;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="timeConsumables">耗材</param>
        public void Init(TimeConsumable timeConsumables)
        {
            this.timeConsumable = timeConsumables;
            this.SpLifeSpanTime.Value = this.timeConsumable.UseDateTime;
            this.RefreshData();

            this.dateEdit2.EditValueChanged += new System.EventHandler(this.dateEdit2_EditValueChanged);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public void RefreshData()
        {
            if (this.timeConsumable == null)
            {
                this.Visible = false;
                return;
            }

            this.Visible = true;

            this.LbPPTools1.Text = this.timeConsumable.Name;
            this.ChkPPTools1.Checked = this.timeConsumable.Enable;
            this.dateEdit1.EditValue = this.timeConsumable.DueDate;
            this.dateEdit2.EditValue = this.timeConsumable.StartDateTime;
            this.SpRemainingTime.Value = this.timeConsumable.GetRemainingTime();

            if (!this.timeConsumable.Enable)
            {
                this.dateEdit1.BackColor = Color.DarkGray;
                this.dateEdit2.BackColor = Color.DarkGray;
                this.SpRemainingTime.BackColor = Color.DarkGray;
            }
            else
            {
                this.dateEdit1.BackColor = Color.Azure;
                this.dateEdit2.BackColor = Color.Azure;
                if (this.timeConsumable.IsExpired())
                {
                    this.SpRemainingTime.BackColor = Color.Red;
                }
                else if (this.timeConsumable.IsRemind())
                {
                    this.SpRemainingTime.BackColor = Color.Yellow;
                }
                else
                {
                    this.SpRemainingTime.BackColor = Color.LightGreen;
                }
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnClearNozzle1_Click(object sender, EventArgs e)
        {
            this.timeConsumable.Clear();
        }

        /// <summary>
        /// 改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkPPTools1_CheckedChanged_1(object sender, EventArgs e)
        {
            this.timeConsumable.Enable = this.ChkPPTools1.Checked;
        }

        /// <summary>
        /// 寿命发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void SpLifeSpanTime_EditValueChanged(object sender, EventArgs e)
        {
            this.timeConsumable.UseDateTime = (int)this.SpLifeSpanTime.Value;
        }

        /// <summary>
        /// 当选择发生改变时
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void dateEdit2_EditValueChanged(object sender, EventArgs e)
        {
            this.timeConsumable.IsReminded = false;
            this.timeConsumable.StartDateTime = (DateTime)this.dateEdit2.EditValue;
        }
    }
}
