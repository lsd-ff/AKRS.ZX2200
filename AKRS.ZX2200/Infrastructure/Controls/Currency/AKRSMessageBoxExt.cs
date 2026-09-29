using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Infrastructure.Service;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Infrastructure.Controls.Currency
{
    using AKRS.Galaxy2.Component.Simple.MessageBox;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.Infrastructure.EventBus;
    using AKRS.ZX2200.Main.Controls;
    using AKRS.ZX2200.SupportFeature.Statistics;
    using DevExpress.ExpressApp.Utils;
    using PostSharp.Aspects.Advices;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// 提示弹框
    /// </summary>
    public partial class AKRSMessageBoxExt : AKRSMessageBox
    {
        /// <summary>
        /// 报警弹框的存储
        /// </summary>
        private static Dictionary<Guid, AKRSMessageBox> msgBoxDict = new Dictionary<Guid, AKRSMessageBox>();

        /// <summary>
        /// 锁
        /// </summary>
        public readonly static object UiLock = new object();

        /// <summary>
        /// 清空报警弹窗
        /// </summary>
        public static void ClearMessageBox()
        {
            foreach (var kv in msgBoxDict)
            {
                kv.Value.BeginInvoke(new System.Action(() =>
                {
                    kv.Value.Hide();
                    kv.Value.DialogResult = DialogResult.Abort;
                    kv.Value.Dispose();
                }));
            }

            msgBoxDict.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SetAlarmState()
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            Alarmer alarmer = null;

            // 恢复三色灯、蜂鸣器状态
            List<Alarmer> alarmerList = HardwareRepositoryService.GetHardwaresByType<Alarmer>();
            if (alarmerList == null || alarmerList.Any<Alarmer>())
            {
                alarmer = alarmerList[0];
            }

            if (msgBoxDict.Count == 0)
            {
                if (Machine.GetInstance().IsStop())
                {
                    alarmer?.SetStopState();
                }
                else if (Machine.GetInstance().IsWorking())
                {
                    alarmer?.SetRunState();
                }
            }
            else 
            {
                // 屏蔽三色灯、蜂鸣器
                if (MachineSoftwareConfiguration.GetInstance().IsBlockAlarmer)
                {
                    return;
                }

                if (msgBoxDict.Values.Exists(a => a.AlarmLevel == AlarmLevel.ThirdLevel))
                {
                    alarmer?.SetThirdLevelAlarm();
                }
                else if (msgBoxDict.Values.Exists(a => a.AlarmLevel == AlarmLevel.SecondLevel))
                {
                    alarmer?.SetSecondLevelAlarm();
                }
                else if (msgBoxDict.Values.Exists(a => a.AlarmLevel == AlarmLevel.FirstLevel))
                {
                    alarmer?.SetFirstLevelAlarm();
                }
                else if (msgBoxDict.Values.Exists(a => a.AlarmLevel == AlarmLevel.NoneLevel))
                {
                    alarmer?.SetRunState();
                }
            }
        } 

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="infoText">说明文字</param>
        /// <param name="caption">标题</param>
        /// <param name="buttonTexts">按钮名称</param>
        /// <param name="drs">drs</param>
        /// <param name="alarmLevel">报警等级</param>
        public AKRSMessageBoxExt(string infoText, string caption, string[] buttonTexts, DialogResult[] drs, AlarmLevel alarmLevel = AlarmLevel.SecondLevel) : base(infoText, caption, buttonTexts, drs, alarmLevel)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 报警弹窗
        /// </summary>
        /// <param name="infoText">报警信息</param>
        /// <param name="caption">报警窗口信息</param>
        /// <param name="buttonTexts">按钮</param>
        /// <param name="drs">dialogResult</param>
        /// <param name="alarmLevel">报警等级</param>
        /// <returns>dialogResult</returns>
        public static DialogResult Show(string infoText, string caption, string[] buttonTexts, DialogResult[] drs, AlarmLevel alarmLevel = AlarmLevel.SecondLevel/*, int alarmCode = -1, string alarmCatetory = "未分类"*/)
        {
            // 如果是工作线程不用竞争锁
            if (Thread.CurrentThread.ManagedThreadId == 1)
            {
                return Alarm(infoText, caption, buttonTexts, drs, alarmLevel);
            }
            else
            {
                lock (UiLock)
                {
                    return Alarm(infoText, caption, buttonTexts, drs, alarmLevel);
                }
            }
        }

        /// <summary>
        /// 报警弹窗
        /// </summary>
        /// <param name="infoText">报警信息</param>
        /// <param name="caption">报警窗口信息</param>
        /// <param name="buttonTexts">按钮</param>
        /// <param name="drs">dialogResult</param>
        /// <param name="alarmLevel">报警等级</param>
        /// <param name="alarmCode">报警代码</param>
        /// <param name="alarmCatetory">报警分类</param>
        /// <returns>dialogResult</returns>
        private static DialogResult Alarm(
            string infoText,
            string caption,
            string[] buttonTexts,
            DialogResult[] drs,
            AlarmLevel alarmLevel = AlarmLevel.SecondLevel)
        {
            AlarmLogEntity alarmLog = new AlarmLogEntity();
            alarmLog.StartTime = DateTime.Now;
            alarmLog.Message = infoText;

            DialogResult dr = DialogResult.Abort;

            if (MainForm.SendUiAction == null)
            {
                AKRSMessageBox msg = new AKRSMessageBox(infoText, caption, buttonTexts, drs, AlarmLevel.SecondLevel);
                msgBoxDict.Add(msg.Guid, msg);
                msg.TopMost = false;
                dr = msg.ShowDialog();
                msgBoxDict.Remove(msg.Guid);
                SetAlarmState();
                msg.Dispose();
            }
            else
            {
                MainForm.SendUiAction(
                   () =>
                   {
                       AKRSMessageBox msg = new AKRSMessageBox(infoText, caption, buttonTexts, drs, AlarmLevel.SecondLevel);
                       msgBoxDict.Add(msg.Guid, msg);
                       msg.TopMost = false;
                       dr = msg.ShowDialog();
                       msgBoxDict.Remove(msg.Guid);
                       SetAlarmState();
                       msg.Dispose();
                   });
            }

            alarmLog.HandleTime = DateTime.Now;
            alarmLog.HandleType = dr.ToString();
            alarmLog.Category = caption;
            DBService.Insert(alarmLog);
           // StatisticsDomain.GetInstance().CalculateMTBA();
            return dr;
        }

        /// <summary>
        /// 报警弹窗
        /// </summary>
        /// <param name="infoText">报警信息</param>
        /// <param name="caption">报警窗口信息</param>
        /// <param name="buttonTexts">按钮</param>
        /// <param name="drs">dialogResult</param>
        /// <param name="alarmLevel">报警等级</param>
        /// <param name="alarmCode">报警代码</param>
        /// <param name="alarmCatetory">报警分类</param>
        /// <returns>dialogResult</returns>
        public static DialogResult ShowWarn(string infoText, string caption, string[] buttonTexts, DialogResult[] drs, AlarmLevel alarmLevel = AlarmLevel.SecondLevel/*, int alarmCode = -1, string alarmCatetory = "未分类"*/)
        {
            // 如果是工作线程不用竞争锁
            if (Thread.CurrentThread.ManagedThreadId == 1)
            {
                return Warn(infoText, caption, buttonTexts, drs, alarmLevel);
            }
            else
            {
                lock (UiLock)
                {
                    return Warn(infoText, caption, buttonTexts, drs, alarmLevel);
                }
            }
        }

        /// <summary>
        /// 提示弹窗
        /// </summary>
        /// <param name="infoText">报警信息</param>
        /// <param name="caption">报警窗口信息</param>
        /// <param name="buttonTexts">按钮</param>
        /// <param name="drs">dialogResult</param>
        /// <param name="alarmLevel">报警等级</param>
        /// <param name="alarmCode">报警代码</param>
        /// <param name="alarmCatetory">报警分类</param>
        /// <returns>dialogResult</returns>
        private static DialogResult Warn(
            string infoText,
            string caption,
            string[] buttonTexts,
            DialogResult[] drs,
            AlarmLevel alarmLevel = AlarmLevel.SecondLevel)
        {
            DialogResult dr = DialogResult.Abort;

            if (MainForm.SendUiAction == null)
            {
                AKRSMessageBox msg = new AKRSMessageBox(infoText, caption, buttonTexts, drs, AlarmLevel.SecondLevel);
                msgBoxDict.Add(msg.Guid, msg);
                msg.TopMost = false;
                dr = msg.ShowDialog();
                msgBoxDict.Remove(msg.Guid);
                SetAlarmState();
                msg.Dispose();
            }
            else
            {
                MainForm.SendUiAction(
                   () =>
                   {
                       AKRSMessageBox msg = new AKRSMessageBox(infoText, caption, buttonTexts, drs, AlarmLevel.SecondLevel);
                       msgBoxDict.Add(msg.Guid, msg);
                       msg.TopMost = false;
                       dr = msg.ShowDialog();
                       msgBoxDict.Remove(msg.Guid);
                       SetAlarmState();
                       msg.Dispose();
                   });
            }

            return dr;
        }
    }
}