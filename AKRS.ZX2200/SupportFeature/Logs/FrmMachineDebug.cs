using System;

namespace AKRS.ZX2200.SupportFeature.Logs
{
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using System.Collections.Generic;

    /// <summary>
    /// 设备调试窗体
    /// </summary>
    public partial class FrmMachineDebug : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 设备调试窗体
        /// </summary>
        public FrmMachineDebug()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 最后一次更新的时间
        /// </summary>
        private DateTime lastDateTime = DateTime.Now;

        /// <summary>
        /// 更新日志的缓存
        /// </summary>
        private string allMessage = string.Empty;

        /// <summary>
        /// 追加调试信息
        /// </summary>
        /// <param name="message">调试信息</param>
        public void InputDebugMessage(string message)
        {
            if (!this.IsHandleCreated || this.Disposing)
            {
                return;
            }

            this.BeginInvoke(() => { this.AppendDebugMessage(message); });
        }

        /// <summary>
        /// 追加日志
        /// </summary>
        /// <param name="debugMessage">事件源</param>
        private void AppendDebugMessage(string debugMessage)
        {
            int maxLine = 500;
            if (this.MeBond.Lines.Length > maxLine)
            {
                int curLines = this.MeBond.Lines.Length - maxLine;
                string[] lines = this.MeBond.Lines;
                Array.Copy(lines, curLines, lines, 0, maxLine);
                Array.Resize(ref lines, maxLine);
                this.MeBond.Lines = lines;
            }

            string message = DateTime.Now.ToString("HH:mm:ss:fff  ") + debugMessage.ToString() + "\r\n";

            this.allMessage += message;

            if ((this.lastDateTime - DateTime.Now).Milliseconds > 100 || this.allMessage.Length > 500)
            {
                this.MeBond.AppendText(this.allMessage);
                this.allMessage = string.Empty;
                this.lastDateTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 停留时间
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void SpPauseTime_EditValueChanged(object sender, EventArgs e)
        {
            Machine.GetInstance().DebugPauseTime = (int)this.SpPauseTime.Value;
        }

        /// <summary>
        /// 模式切换
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void LueDebugType_EditValueChanged(object sender, EventArgs e)
        {
            Machine.GetInstance().DebugModel = (DebugModelEnum)this.LueDebugType.EditValue;
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmMachineDebug_Load(object sender, EventArgs e)
        {
            MachineStateModel.GetInstance().MachineWorkMode = MachineWorkModeEnum.DeBugWork;

            this.LueDebugType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<DebugModelEnum>();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmMachineDebug_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            MachineStateModel.GetInstance().MachineWorkMode = MachineWorkModeEnum.NormalWork;
        }

        /// <summary>
        /// 继续
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtContinue_Click(object sender, EventArgs e)
        {
            Machine.GetInstance().ContinueDebug = true;
        }
    }
}
