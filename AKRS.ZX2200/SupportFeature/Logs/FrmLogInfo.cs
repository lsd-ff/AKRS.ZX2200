namespace AKRS.ZX2200.SupportFeature.Logs
{
    using AKRS.Galaxy2.Log;
    using log4net.Core;
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using System.Windows.Forms;

    using DevExpress.Utils.Extensions;
    using DevExpress.XtraEditors;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using DevExpress.Data.ExpressionEditor;

    /// <summary>
    /// 日志显示
    /// </summary>
    public partial class FrmLogInfo : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 释放对象
        /// </summary>
        private IDisposable idDisposable;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmLogInfo()
        {
            this.InitializeComponent();
            this.ChkReceive.BringToFront();
            this.idDisposable = LogsManager.LogDataBroadcastBlock.LinkTo(
                new ActionBlock<LogDataEntity>(
                    logData =>
                        {
                            if (!this.IsHandleCreated || this.Disposing || !this.ChkReceive.Checked)
                            {
                                return;
                            }

                            this.Invoke(
                                new Action(
                                    () =>
                                        {
                                            if (logData.Vtype == ViewType.InFileAndUI || logData.Vtype == ViewType.All)
                                            {
                                                if (logData.LogCategory == LogCategory.Dispense)
                                                {
                                                    this.DispenseWorkLogAppend(logData.Message);
                                                }
                                                else if (logData.LogCategory == LogCategory.Bond)
                                                {
                                                    this.BondWorkLogAppend(logData.Message);
                                                }
                                                else if (logData.LogCategory == LogCategory.Component)
                                                {
                                                    this.ComponentWorkLogAppend(logData.Message);
                                                }
                                                else if (logData.LogCategory == LogCategory.Transport)
                                                {
                                                    this.TransportLogAppend(logData.Message);
                                                }
                                                else if (logData.LogCategory == LogCategory.MainSoftWare)
                                                {
                                                    this.OperatorLogAppend(logData.Message);
                                                }
                                            }
                                        }));
                        }));
        }

        /// <summary>
        /// 点胶追加日志
        /// </summary>
        /// <param name="logMessage">日志信息</param>
        private void DispenseWorkLogAppend(string logMessage)
        {
            this.ControlRows(this.MeDispense);
            this.MeDispense.AppendText("\r\n" + logMessage.ToString());
        }

        /// <summary>
        /// 固晶追加日志
        /// </summary>
        /// <param name="logMessage">日志信息</param>
        private void BondWorkLogAppend(string logMessage)
        {
            this.ControlRows(this.MeBond);
            this.MeBond.AppendText("\r\n" + logMessage.ToString());
        }

        /// <summary>
        /// 晶圆追加日志
        /// </summary>
        /// <param name="logMessage">日志信息</param>
        private void ComponentWorkLogAppend(string logMessage)
        {
            this.ControlRows(this.MeComponent);
            this.MeComponent.AppendText("\r\n" + logMessage.ToString());
        }

        /// <summary>
        /// 传输追加日志
        /// </summary>
        /// <param name="logMessage">日志信息</param>
        private void TransportLogAppend(string logMessage)
        {
            this.ControlRows(this.MeTransport);
            this.MeTransport.AppendText("\r\n" + logMessage.ToString());
        }

        /// <summary>
        /// 视觉追加日志
        /// </summary>
        /// <param name="logMessage">日志信息</param>
        private void VisionLogAppend(string logMessage)
        {
            this.ControlRows(this.MeVision);
            this.MeVision.AppendText("\r\n" + logMessage.ToString());
        }

        /// <summary>
        /// 操作追加日志
        /// </summary>
        /// <param name="logMessage">日志信息</param>
        private void OperatorLogAppend(string logMessage)
        {
            this.ControlRows(this.MeOperate);
            this.MeOperate.AppendText("\r\n" + logMessage.ToString());
        }

        /// <summary>
        /// 操作记录
        /// </summary>
        /// <param name="memo">日志</param>
        private void ControlRows(MemoEdit memo)
        {
            int maxLine = 200;
            if (memo.Lines.Length > maxLine)
            {
                int curLines = memo.Lines.Length - maxLine;
                string[] lines = memo.Lines;
                Array.Copy(lines, curLines, lines, 0, maxLine);
                Array.Resize(ref lines, maxLine);
                memo.Lines = lines;
            }
        }
    }
}
