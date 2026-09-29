using DevExpress.XtraEditors;
using MathNet.Numerics;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.Infrastructure.Controls.Common
{
    /// <summary>
    /// 报警历史查询窗体
    /// </summary>
    public partial class FrmAlarmHistoryQuery : XtraForm
    {
        /// <summary>
        /// 查询窗体
        /// </summary>
        private UcAlarmHistory queryControl;

        /// <summary>
        /// 单例
        /// </summary>
        private static FrmAlarmHistoryQuery instance;

        /// <summary>
        /// 单例
        /// </summary>
        public static FrmAlarmHistoryQuery Instance
        {
            get
            {
                if (instance == null || instance.IsDisposed)
                {
                    instance = new FrmAlarmHistoryQuery();
                }

                return instance;
            }
        }

        /// <summary>
        /// 私有构造
        /// </summary>
        private FrmAlarmHistoryQuery()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmAlarmHistoryQuery_Load(object sender, EventArgs e)
        {
            this.queryControl = new UcAlarmHistory();
            this.queryControl.Dock = DockStyle.Fill;
            this.queryControl.Dock = DockStyle.Fill;
            this.Controls.Add(this.queryControl);
        }

        /// <summary>
        /// on shown
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }

        /// <summary>
        /// 只关闭窗口，不释放资源
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.FormOwnerClosing)
            {
                return;
            }

            e.Cancel = true; 
            this.Hide();
        }
    }
}