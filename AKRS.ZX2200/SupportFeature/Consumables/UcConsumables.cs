namespace AKRS.ZX2200.SupportFeature.Consumables
{
    using System;
    using System.Drawing;

    /// <summary>
    /// 耗材
    /// </summary>
    public partial class UcConsumables : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public UcConsumables()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 耗材
        /// </summary>
        private FrequencyConsumables frequencyConsumable;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="frequencyConsumables">耗材</param>
        public void Init(FrequencyConsumables frequencyConsumables)
        {
            this.frequencyConsumable = frequencyConsumables;

            if (this.frequencyConsumable != null)
            {
                this.SpPPToolTotalTimes1.DataBindings.Add("EditValue", this.frequencyConsumable, "TotalUseTimes");
                this.RefreshData();
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public void RefreshData()
        {
            if (this.frequencyConsumable == null)
            {
                this.Visible = false;
                return;
            }

            this.Visible = true;

            this.LbPPTools1.Text = this.frequencyConsumable.Name;
            this.ChkPPTools1.Checked = this.frequencyConsumable.Enable;
           // this.SpPPToolTotalTimes1.Value = this.frequencyConsumable.TotalUseTimes;
            this.SpPPToolUseTimes1.Value = this.frequencyConsumable.CurrentUseTimes;


            if (!this.frequencyConsumable.Enable)
            {
                this.SpPPToolTotalTimes1.BackColor = Color.DarkGray; 
                this.SpPPToolUseTimes1.BackColor = Color.DarkGray;
            }
            else
            {
                this.SpPPToolTotalTimes1.BackColor = Color.Azure;
                if (this.frequencyConsumable.IsExpired())
                {
                    this.SpPPToolUseTimes1.BackColor = Color.Red;
                }
                else if (this.frequencyConsumable.IsRemind())
                {
                    this.SpPPToolUseTimes1.BackColor = Color.Yellow;
                }
                else
                {
                    this.SpPPToolUseTimes1.BackColor = Color.LightGreen;
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
            this.frequencyConsumable.Clear();
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkPPTools1_CheckedChanged(object sender, EventArgs e)
        {
            this.frequencyConsumable.Enable = this.ChkPPTools1.Checked;
        }
    }
}
