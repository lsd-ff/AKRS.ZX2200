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

namespace AKRS.ZX2200.SupportFeature.Statistics
{
    /// <summary>
    /// 温飘统计
    /// </summary>
    public partial class UcTemperatureCompensation : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 温飘统计
        /// </summary>
        public UcTemperatureCompensation()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">时间源</param>
        /// <param name="e">封装参数</param>
        private void UcTemperatureCompensation_Load(object sender, EventArgs e)
        {
            this.DeStartTime.DateTime = DateTime.Now.Date;
            this.DeEndTime.DateTime = DateTime.Now.Date.AddDays(1);
            this.RefreshData();
        }


        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            this.gridControl3.DataSource = StatisticsDomain.GetInstance()
                .QueryTemperatureCompensationTime(this.DeStartTime.DateTime, this.DeEndTime.DateTime);
        }

        /// <summary>
        /// 开始时间改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void DeStartTime_EditValueChanged(object sender, EventArgs e)
        {
            this.RefreshData();
        }

        /// <summary>
        /// 结束时间改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void DeEndTime_EditValueChanged(object sender, EventArgs e)
        {
            this.RefreshData();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtQuery_Click(object sender, EventArgs e)
        {
            this.RefreshData();
        }
    }
}
