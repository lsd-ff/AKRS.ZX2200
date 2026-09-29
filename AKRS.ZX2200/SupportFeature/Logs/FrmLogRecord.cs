namespace AKRS.ZX2200.SupportFeature.Logs
{
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;

    using DevExpress.XtraEditors;

    /// <summary>
    /// log记录窗体
    /// </summary>
    public partial class FrmLogRecord : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 工作线程
        /// </summary>
        private Thread task;

        /// <summary>
        /// 线程信号
        /// </summary>
        private bool isStop;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmLogRecord()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    this.task.Abort();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    this.Dowork();
                    btn.Appearance.BackColor = Color.Yellow;
                }
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            LogRecordDataHelper.ClearLogRecord();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void FrmLogRecord_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.task?.Abort();
        }

        /// <summary>
        /// 开启线程
        /// </summary>
        private void Dowork()
        {
            this.isStop = false;
            this.task = new Thread(
                () =>
                    {
                        while (!this.isStop)
                        {
                            Thread.Sleep(100);
                            this.Invoke(new Action(() =>
                                {
                                    this.GcLogRecord.DataSource = LogRecordDataHelper.GetLogRecord();
                                }));
                        }
                    }
            );
            this.task.Start();
        }
    }
}
