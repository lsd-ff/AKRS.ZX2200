using AKRS.ZX2200.SupportFeature.Statistics;
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

namespace AKRS.ZX2200.SupportFeature.Compensate.MotionAreaCompensate
{
    /// <summary>
    /// 输入数据
    /// </summary>
    public partial class FrmInputData : XtraForm
    {
        /// <summary>
        /// 输入数据
        /// </summary>
        public FrmInputData()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 数据
        /// </summary>
        public List<DefectStatisticsEntity> list { get; set; }

        /// <summary>
        /// 检测结果组件
        /// </summary>
        private UcDefectStatistics ucDefectStatistics;

        /// <summary>
        /// 数据加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmInputData_Load(object sender, EventArgs e)
        {
            this.ucDefectStatistics = new UcDefectStatistics() { Dock = DockStyle.Fill };
            this.panelControl1.Controls.Add(ucDefectStatistics);
        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmInputData_FormClosed(object sender, FormClosedEventArgs e)
        {
           this.list = this.ucDefectStatistics.DefectStatisticsEntities;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtInput_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
