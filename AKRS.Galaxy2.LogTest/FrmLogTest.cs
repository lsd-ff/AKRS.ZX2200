using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.Log.Control;
using log4net.Core;
using System;
using System.Windows.Forms;

namespace AKRS.Galaxy2.LogTest
{
    public partial class FrmLogTest : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public FrmLogTest()
        {
            this.InitializeComponent();
            UcLogControl ucLogControl = new UcLogControl() { Dock = DockStyle.Fill };
            this.GclLog.Controls.Add(ucLogControl);
            LogsManager.IsLogInfo = true;
            LogsManager.IsLogWarn = true;
        }
        
        /// <summary>
        /// 记录异常
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAnomalies_Click(object sender, EventArgs e)
        {
            Exception ex = new Exception("异常");
            LogHelper.Post(Level.Info, $"Exception", ex, LogCategory.Global, ViewType.InFileAndUI);
            LogHelper.Post(Level.Debug, $"Exception", ex, LogCategory.Global, ViewType.InFileAndUI);
            LogHelper.Post(Level.Error, $"Exception", ex, LogCategory.Global, ViewType.InFileAndUI);
            LogHelper.Post(Level.Alert, $"Exception", ex, LogCategory.Global, ViewType.InFileAndUI);
            LogHelper.Post(Level.Fatal, $"Exception", ex, LogCategory.Global, ViewType.InFileAndUI);
            LogHelper.Post(Level.All, $"Exception", ex, LogCategory.Global, ViewType.InFileAndUI);
            LogHelper.Post(Level.Critical, $"Exception", ex, LogCategory.Global, ViewType.InFileAndUI);
            LogHelper.Post(Level.Emergency, $"Exception", ex, LogCategory.Global, ViewType.InFileAndUI);
            LogHelper.Post(Level.Fine, $"Exception", ex, LogCategory.Global, ViewType.InFileAndUI);
            LogHelper.Post(Level.Off, $"Exception", ex, LogCategory.Global, ViewType.InFileAndUI);
            LogHelper.Post(Level.Finest, $"Exception", ex, LogCategory.Global, ViewType.InFileAndUI);
            LogHelper.Post(Level.Finer, $"Exception", ex, LogCategory.Global, ViewType.InFileAndUI);
            LogHelper.Post(Level.Log4Net_Debug, $"Exception", ex, LogCategory.Global, ViewType.InFileAndUI);
            LogHelper.Post(Level.Notice, $"Exception", ex, LogCategory.Global, ViewType.InFileAndUI);
            LogHelper.Post(Level.Warn, $"Exception", ex, LogCategory.Global, ViewType.InFileAndUI);
            LogHelper.Post(Level.Trace, $"Exception", ex, LogCategory.Global, ViewType.InFileAndUI);
            LogHelper.Post(Level.Verbose, $"Exception", ex, LogCategory.Global, ViewType.InFileAndUI);
            LogHelper.Post(Level.Severe, $"Exception", ex, LogCategory.Global, ViewType.InFileAndUI);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnLogs_Click(object sender, EventArgs e)
        {
            LogHelper.Post(Level.Info, $"InfoHardware", LogCategory.Hardware, ViewType.InFileAndUI);
            LogHelper.Post(Level.Warn, $"WarnHardware", LogCategory.Hardware, ViewType.InFileAndUI);
            LogHelper.Post(Level.Error, $"ErrorHardware", LogCategory.Hardware, ViewType.InFileAndUI);

            LogHelper.Post(Level.Info, $"InfoMainSoftWare", LogCategory.MainSoftWare, ViewType.InFileAndUI);
            LogHelper.Post(Level.Warn, $"WarnMainSoftWare", LogCategory.MainSoftWare, ViewType.InFileAndUI);
            LogHelper.Post(Level.Error, $"ErrorMainSoftWare", LogCategory.MainSoftWare, ViewType.InFileAndUI);

            //LogHelper.Post(Level.Info, $"InfoProcess", LogCategory.Process, ViewType.InFileAndUI);
            //LogHelper.Post(Level.Warn, $"WarnProcess", LogCategory.Process, ViewType.InFileAndUI);
            //LogHelper.Post(Level.Error, $"ErrorProcess", LogCategory.Process, ViewType.InFileAndUI);

            LogHelper.Post(Level.Info, $"InfoPR", LogCategory.PR, ViewType.InFileAndUI);
            LogHelper.Post(Level.Warn, $"WarnPR", LogCategory.PR, ViewType.InFileAndUI);
            LogHelper.Post(Level.Error, $"ErrorPR", LogCategory.PR, ViewType.InFileAndUI);

            //LogHelper.Post(Level.Info, $"InfoDetection", LogCategory.Detection, ViewType.InFileAndUI);
            //LogHelper.Post(Level.Warn, $"WarnDetection", LogCategory.Detection, ViewType.InFileAndUI);
            //LogHelper.Post(Level.Error, $"ErrorDetection", LogCategory.Detection, ViewType.InFileAndUI);

            LogHelper.Post(Level.Info, $"InfoDispense", LogCategory.Dispense, ViewType.InFileAndUI);
            LogHelper.Post(Level.Warn, $"WarnDispense", LogCategory.Dispense, ViewType.InFileAndUI);
            LogHelper.Post(Level.Error, $"ErrorDispense", LogCategory.Dispense, ViewType.InFileAndUI);

            LogHelper.Post(Level.Info, $"InfoGlobal", LogCategory.Global, ViewType.InFileAndUI);
            LogHelper.Post(Level.Warn, $"WarnGlobal", LogCategory.Global, ViewType.InFileAndUI);
            LogHelper.Post(Level.Error, $"ErrorGlobal", LogCategory.Global, ViewType.InFileAndUI);

            //LogHelper.Post(Level.Info, $"InfoMeasureHeight", LogCategory.MeasureHeight, ViewType.InFileAndUI);
            //LogHelper.Post(Level.Warn, $"WarnMeasureHeight", LogCategory.MeasureHeight, ViewType.InFileAndUI);
            //LogHelper.Post(Level.Error, $"ErrorMeasureHeight", LogCategory.MeasureHeight, ViewType.InFileAndUI);
        }
    }
}