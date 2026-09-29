using System;
using System.Windows.Forms;
using AKRS.Galaxy2.PR.Controls;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.BondSystem.Controls.Manual;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Infrastructure.Service;
using AKRS.ZX2200.WaferSubSystem.Controls;

namespace AKRS.ZX2200.Main.Controls.Ucmain.MainControls
{
    using AKRS.ZX2200.BondSystem.Controls.Experiment;
    using AKRS.ZX2200.SupportFeature.Logs;
    using AKRS.ZX2200.SupportFeature.SensorCheckScan;
    using AKRS.ZX2200.SupportFeature.Statistics;

    using DevExpress.DashboardCommon.Native;

    using Machine = AKRS.ZX2200.Main.Machine.MachineSupport.Machine;

    /// <summary>
    /// 主界面的跨线程调用
    /// </summary>
    public partial class UcMainSystem
    {
        /// <summary>
        /// 初始化Action
        /// </summary>
        private void InitAction()
        {
            VmVisionShow += this.VisionShow;
            VisionAlarmFunc += this.VisionAlarm;
            VisionAlarmLockFunc += this.VisionAlarm;
            EditPr += this.EditPrProgram;
            ChangeCameraVision += this.ChangeCameraVisionByName;

            ActiveReadBondForce += this.ReadBondForce;
            SetChartControlConstantLine += this.SetConstantLine;

            SensorAlarmAction += this.SensorAlarm;
            UcGuideMoveOpen += this.GuideMoveOpen;
            UcGuideMoveClose += this.GuideMoveClose;
            ReFreshSystem1TuAction += this.ReFreshSystem1Tu;
            ReFreshSystem2TuAction += this.ReFreshSystem2Tu;
            InputDebugAction += this.InputDebug;

            ShowForceRealTimeCurve += this.ShowFrmForceRealTimeCurve;
            ShowLVDTRealTimeCurve += this.ShowFrmLVDTRealTimeCurve;
        }

        /// <summary>
        /// 定位失败报警控件
        /// </summary>
        private FrmVisionAlarm frmVisionAlarm;

        /// <summary>
        /// 传感器报警
        /// </summary>
        private FrmSensorAlarm frmSensorAlarm;

        /// <summary>
        /// 单个焊点补偿对象
        /// </summary>
        private FrmSingleBpData frmSingleBpData;

        /// <summary>
        /// 顶针实时状态
        /// </summary>
        private FrmEjectionRealtimeState frmEjectionRealtimeState;

        /// <summary>
        /// 焊后实时曲线
        /// </summary>
        private FrmPostBondResCurve frmPostBondResCurve;

        /// <summary>
        /// 吸嘴架调试
        /// </summary>
        private FrmNozzleBankManual frmNozzleBank;

        /// <summary>
        /// 调试界面
        /// </summary>
        private FrmMachineDebug frmMachineDebug;

        /// <summary>
        /// 定位失败执行的委托
        /// </summary>
        public static Func<PREntity, string, string, (DialogResult, BaseAlgResult)> VisionAlarmFunc { get; set; }

        /// <summary>
        /// 定位失败执行的委托
        /// </summary>
        public static Func<PREntity, string, string,PRHardware, (DialogResult, BaseAlgResult)> VisionAlarmLockFunc { get; set; }

        /// <summary>
        /// 编辑PR模板
        /// </summary>
        public static Action<PREntity> EditPr { get; set; }

        /// <summary>
        /// 海康视觉实时窗体显示
        /// </summary>
        public static Action VmVisionShow { get; set; }

        /// <summary>
        /// 方向盘关闭
        /// </summary>
        public static Action UcGuideMoveClose { get; set; }

        /// <summary>
        /// 方向盘打开
        /// </summary>
        public static Action<string> UcGuideMoveOpen { get; set; }

        /// <summary>
        /// 改变所选则的相机
        /// </summary>
        public static Action<string> ChangeCameraVision { get; set; }

        /// <summary>
        /// 读焊头力
        /// </summary>
        public static Action<bool> ActiveReadBondForce { get; set; }

        /// <summary>
        /// 设置限定线
        /// </summary>
        public static Action<double> SetChartControlConstantLine { get; set; }

        /// <summary>
        /// 检测报警
        /// </summary>
        public static Action<string> SensorAlarmAction { get; set; }

        /// <summary>
        /// 刷新系统1的载具
        /// </summary>
        public static Action ReFreshSystem1TuAction { get; set; }

        /// <summary>
        /// 刷新系统2的载具
        /// </summary>
        public static Action ReFreshSystem2TuAction { get; set; }

        /// <summary>
        /// 刷新调试信息
        /// </summary>
        public static Action<string> InputDebugAction { get; set; }

        /// <summary>
        /// 力控实时曲线
        /// </summary>
        public static Action ShowForceRealTimeCurve { get; set; }

        /// <summary>
        /// LVDT实时曲线
        /// </summary>
        public static Action ShowLVDTRealTimeCurve { get; set; }

        /// <summary>
        /// 相机实时显示
        /// </summary>
        private void VisionShow()
        {
            this.Invoke(
                new Action(
                    () =>
                        {
                            //if (this.VisionForm != null)
                            //{
                            //    this.VisionForm.Close();
                            //    this.VisionForm.Dispose();
                            //    this.VisionForm = null;
                            //}

                            //this.VisionForm = new RealTimeImageForm();
                            //this.VisionForm.Show();

                            RealTimeImageForm.ShowForm();
                        }));
        }

        /// <summary>
        /// 打开方向盘移动界面
        /// </summary>
        /// <param name="moduleName">模组名称</param>
        private void GuideMoveOpen(string moduleName = null)
        {
            // 打开方向盘移动界面
            this.Invoke(
                new Action(
                                                 () =>
                                                     {
                                                         this.ShowPanel(
                                                             "ucGuideMove",
                                                             () =>
                                                                 {
                                                                     UcGuideMove ucGuideMove = new UcGuideMove("UcMainSystem", true) { Dock = DockStyle.Fill };

                                                                     if (string.IsNullOrEmpty(moduleName) == false)
                                                                     {
                                                                         ucGuideMove.ChangeModuleName(moduleName);
                                                                     }

                                                                     return ucGuideMove;
                                                                 });
                                                     }));
        }

        /// <summary>
        /// 关闭方向盘移动界面
        /// </summary>
        public void GuideMoveClose()
        {
            // 关闭原来的方向盘界面
            this.Invoke(
                new Action(
                    () =>
                        {
                            this.DockManager_ClosedPanel("ucGuideMove");
                        }));
        }

        /// <summary>
        /// 定位失败所用到的方法
        /// </summary>
        /// <param name="pREntity">定位实体</param>
        /// <param name="message">报警信息</param>
        /// <param name="caption">报警标题</param>
        /// <returns>结果</returns>
        private (DialogResult, BaseAlgResult) VisionAlarm(PREntity pREntity, string message, string caption)
        {
            lock (AKRSMessageBoxExt.UiLock)
            {
                AlarmLogEntity alarm = new AlarmLogEntity();
                alarm.StartTime = DateTime.Now;
                alarm.Message = message;

                DialogResult dialogResult = DialogResult.No;
                BaseAlgResult baseAlgResult = null;

                this.Invoke(new Action(() =>
                    {
                        this.frmVisionAlarm = new FrmVisionAlarm(pREntity, message, caption, null); 
                        this.frmVisionAlarm.ShowDialog();
                        baseAlgResult = this.frmVisionAlarm.BaseAlgResult;
                        dialogResult = this.frmVisionAlarm.DialogResult;
                        this.frmVisionAlarm.Dispose();
                    }));

                alarm.HandleTime = DateTime.Now;
                alarm.HandleType = dialogResult.ToString();
                alarm.AlarmCode = -1;
                alarm.Category = caption;
                DBService.Insert(alarm);
              //  StatisticsDomain.GetInstance().CalculateMTBA(); 
                Machine.GetInstance().ResetAlarm();

                return (dialogResult, baseAlgResult);
            }
        }

        /// <summary>
        /// 定位失败所用到的方法
        /// </summary>
        /// <param name="pREntity">定位实体</param>
        /// <param name="message">报警信息</param>
        /// <param name="caption">报警标题</param>
        /// <returns>结果</returns>
        private (DialogResult, BaseAlgResult) VisionAlarm(PREntity pREntity, string message, string caption, PRHardware pRHardware = null)
        {
            lock (AKRSMessageBoxExt.UiLock)
            {
                AlarmLogEntity alarm = new AlarmLogEntity();
                alarm.StartTime = DateTime.Now;
                alarm.Message = message;

                DialogResult dialogResult = DialogResult.No;
                BaseAlgResult baseAlgResult = null;

                this.Invoke(new Action(() =>
                {
                    this.frmVisionAlarm = new FrmVisionAlarm(pREntity, message, caption, pRHardware);
                    this.frmVisionAlarm.ShowDialog();
                    baseAlgResult = this.frmVisionAlarm.BaseAlgResult;
                    dialogResult = this.frmVisionAlarm.DialogResult;
                    this.frmVisionAlarm.Dispose();
                }));

                alarm.HandleTime = DateTime.Now;
                alarm.HandleType = dialogResult.ToString();
                alarm.AlarmCode = -1;
                alarm.Category = caption;
                DBService.Insert(alarm);
                // StatisticsDomain.GetInstance().CalculateMTBA();
                Machine.GetInstance().ResetAlarm();

                return (dialogResult, baseAlgResult);
            }
        }

        /// <summary>
        /// 编辑PR
        /// </summary>
        /// <param name="pREntity">pR实体</param>
        private void EditPrProgram(PREntity pREntity)
        {
            this.Invoke(new Action(() =>
                {
                    FrmPREditor editor = new FrmPREditor(pREntity, false);
                    editor.ShowDialog();
                }));
        }

        /// <summary>
        /// 改变相机的名称
        /// </summary>
        /// <param name="cameraName">相机名称</param>
        private void ChangeCameraVisionByName(string cameraName)
        {
            this.BeginInvoke(new Action(() =>
            {
                RealTimeImageForm.ChangeCameraVisionByName(cameraName);

                //if (this.VisionForm != null && this.VisionForm.IsHandleCreated == true && this.VisionForm.IsDisposed == false)
                //{
                //    this.VisionForm.ChangeCameraVisionByName(cameraName);
                //}
            }));
        }

        /// <summary>
        /// 报警
        /// </summary>
        /// <param name="message">时间</param>
        private void SensorAlarm(string message)
        {
            this.Invoke(new Action(() =>
                {
                    if (this.frmSensorAlarm == null || this.frmSensorAlarm.IsDisposed)
                    {
                        this.frmSensorAlarm = new FrmSensorAlarm();
                        this.frmSensorAlarm.Show();
                    }

                    this.frmSensorAlarm.AddAlarmMessage(message);
                }));
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void ReFreshSystem1Tu()
        {
            if (this.frmTransportUnitMappingSystem1 != null 
                && this.frmTransportUnitMappingSystem1.IsHandleCreated
                && !this.frmTransportUnitMappingSystem1.IsDisposed 
                && !this.frmTransportUnitMappingSystem1.Disposing)
            {
                this.BeginInvoke(() => { this.frmTransportUnitMappingSystem1.ReFreshUi(); });
            }
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void ReFreshSystem2Tu()
        {
            if (this.frmTransportUnitMappingSystem2 != null
                && this.frmTransportUnitMappingSystem2.IsHandleCreated
                && !this.frmTransportUnitMappingSystem2.IsDisposed
                && !this.frmTransportUnitMappingSystem2.Disposing)
            {
                this.BeginInvoke(() => { this.frmTransportUnitMappingSystem2.ReFreshUi(); });
            }
        }

        /// <summary>
        /// 刷新调试界面
        /// </summary>
        private void ShowFrmDebug()
        {
            if (this.frmMachineDebug == null || this.frmMachineDebug.IsDisposed)
            {
                this.frmMachineDebug = new FrmMachineDebug();
                this.frmMachineDebug.Show();
            }
        }


        /// <summary>
        /// 增加调试信息
        /// </summary>
        /// <param name="message">信息</param>
        private void InputDebug(string message)
        {
            if (this.frmMachineDebug != null
                && this.frmMachineDebug.IsHandleCreated
                && !this.frmMachineDebug.IsDisposed
                && !this.frmMachineDebug.Disposing)
            {
                this.frmMachineDebug.InputDebugMessage(message);
            }
        }

        /// <summary>
        /// 力控实时曲线 
        /// </summary>
        private void ShowFrmForceRealTimeCurve()
        {
            if (this.frmForceRealTimeCurve == null || this.frmForceRealTimeCurve.IsDisposed)
            {
                this.frmForceRealTimeCurve = new FrmForceRealTimeCurve();
            }

            this.frmForceRealTimeCurve.Show();
        }

        /// <summary>
        /// 力控实时曲线 
        /// </summary>
        private void ShowFrmLVDTRealTimeCurve()
        {
            if (this.frmLVDTRealTimeCurve == null || this.frmLVDTRealTimeCurve.IsDisposed)
            {
                this.frmLVDTRealTimeCurve = new FrmLVDTRealTimeCurve();
            }

            this.frmLVDTRealTimeCurve.Show();
        }
    }
}
