namespace AKRS.ZX2200.Main.Controls
{
    using System;
    using System.Drawing;
    using System.Threading.Tasks;

    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models;

    /// <summary>
    ///  交互信号状态显示
    /// </summary>
    public partial class FrmSignalDisplay : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        public FrmSignalDisplay()
        {
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        private void InitControl()
        {
            this.GcSignal.DataSource = SignalPool.GetInstance().SignalList;
            this.GcSignal.Refresh();
            this.GvSignal.RefreshData();
        }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (System1Domain.GetInstance().DispenseWorkTask.DispenseTask != null)
            {
                this.BtnDispenseTaskState.BackColor = System1Domain.GetInstance().DispenseWorkTask.DispenseTask.Status == TaskStatus.Running
                                                          ? Color.Yellow
                                                          : Color.Transparent;
            }

            if (System2Domain.GetInstance().BondTask.BondWorkTask != null)
            {
                this.BtnBondTaskState.BackColor =
                    System2Domain.GetInstance().BondTask.BondWorkTask.Status == TaskStatus.Running
                        ? Color.Yellow
                        : Color.Transparent;
            }

            //if (WaferSystemDomain.GetInstance().WaferSubSystemTask.waferSubSystemWorkThread != null)
            //{
            //    this.BtnWaferTaskState.BackColor = WaferSystemDomain.GetInstance().WaferSubSystemTask.waferSubSystemWorkThread.IsAlive
            //                                           ? Color.Yellow
            //                                           : Color.Transparent;
            //}

            if (WaferSystemDomain.GetInstance().WaferSubSystemTask.waferSubTask != null)
            {
                this.BtnWaferTaskState.BackColor = WaferSystemDomain.GetInstance().WaferSubSystemTask.waferSubTask.Status == TaskStatus.Running
                                                       ? Color.Yellow
                                                       : Color.Transparent;
            }

            if (TransportDomain.GetInstance().transportTask != null)
            {
                this.BtnTransportTaskState.BackColor = TransportDomain.GetInstance().transportTask.IsAlive()
                                                           ? Color.Yellow
                                                           : Color.Transparent;
            }

            this.InitControl();
        }

        /// <summary>
        ///  窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSignalDisplay_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            this.timer1.Stop();
            this.timer1.Tick -= timer1_Tick;
            this.timer1.Dispose();
        }
    }
}