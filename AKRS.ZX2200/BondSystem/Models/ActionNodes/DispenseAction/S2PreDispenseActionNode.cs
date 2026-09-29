using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using log4net.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.DispenseAction
{
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.MachineSupport;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using DevExpress.XtraPrinting;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// 系统2预点胶
    /// </summary>
    public class S2PreDispenseActionNode : ActionNode
    { 
        /// <summary>
        /// 目前正在使用的点胶头程式
        /// </summary>
        public S2DispenserProgram S2DispenserProgram => BondProgram.GetInstance().S2DispenserProgram;

        /// <summary>
        /// 预点胶板
        /// </summary>
        public S2PreDispensePlateProgram PreDispensePlateProgram =>
            BondProgram.GetInstance().S2PreDispensePlateProgram;

        /// <summary>
        /// 预点胶板参数
        /// </summary>
        [JsonIgnore]
        public PreDispensePlatePara PreDispensePlatePara => DispenseDevicePara.GetInstance().PreDispensePlatePara;

        /// <summary>
        /// 是否为手动模式
        /// </summary>
        public bool IsManual { get; set; } = false;


        /// <summary>
        /// 最后一次点胶时间
        /// </summary>
        public DateTime LastDispenseTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 预点胶开始工作
        /// </summary>
        /// <returns>是否成功执行动作</returns>
        public override ExcuteResult DoWork()
        {
            this.WorkStart?.Invoke();
            this.State = RunStateEnum.Running;
            try
            {
                // 判断是否需要执行预点胶动作
                if (!this.IsDispense())
                {
                    return ExcuteResult.Success;
                }

                System2RunTimeProvider.RecordTime("预点胶", $"预点胶开始----------------");

                // 获取点胶位置的集合
                List<AKRSPoint3D> lists = this.PreDispensePlateProgram.GetEpoxy(
                    this.S2DispenserProgram.Dispenser.PreDispense.RepeatPreDispensing);

                if (lists == null)
                {
                    return ExcuteResult.Abort;
                }

                // 获取预点胶的图案
                EpoxyApplication epoxyApplication = (EpoxyApplication)EpoxyApplicationRepository.GetInstance()
                    .Find(this.S2DispenserProgram.Dispenser.PreDispense.EpoxyApplicationName);

                bool isPrintTool = MachineHardwareConfiguration.GetInstance().IsSystem2ConfigPrintingTool;
                if (isPrintTool)
                {
                    if (string.IsNullOrEmpty(epoxyApplication.PrintNozzleName) || epoxyApplication.PrintNozzleName == "Null")
                    {
                        throw new Exception("蘸胶吸嘴为空，无法执行蘸胶动作");
                    }

                    // 换吸嘴动作
                    bool ret = System2Domain.GetInstance().System2Controller.ChangeNozzle(epoxyApplication.PrintNozzleName);
                    if (!ret)
                    {
                        return ExcuteResult.Abort;
                    }
                }

                foreach (AKRSPoint3D point in lists)
                {
                    // 空跑模式不出胶
                    bool isDrip = MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.DryCycle;

                    // 开始点胶动作
                    if (System2Domain.GetInstance().S2DispenseController.InterpolationApplication(epoxyApplication, point, isDrip, true, false,0) != ExcuteResult.Success)
                    {
                        return ExcuteResult.Alarm;
                    }
                }

                // 获取胶后检测对象
                PostBondInspection postBondInspection = (PostBondInspection)PostBondInspectionRepository.GetInstance()
                    .Find(this.S2DispenserProgram.Dispenser.PreDispense.PreDispenseCheckName);

                if (postBondInspection != null)
                {
                    this.CheckEpoxyApplication(postBondInspection, postBondInspection.VisionConfig.P1VisionPos + lists.Last());
                }

                // 如果是手动模式，相机移动到此处
                if (this.IsManual)
                {
                    this.IsManual = false;
                    System2Domain.GetInstance().S2DispenseController.VisionMoveToG0Pos(lists[lists.Count - 1]);
                }

                this.LastDispenseTime = DateTime.Now;

                System2RunTimeProvider.RecordTime("预点胶", $"预点胶结束----------------");
                return ExcuteResult.Success;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程{this.Name}运行故障！", ex, LogCategory.Dispense);
                AKRSMessageBoxExt.Show(ex.Message, "预点胶异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return ExcuteResult.Exception;
            }
            finally
            {
                this.IsManual = false;
                this.State = RunStateEnum.Stop;
                WorkStop?.Invoke();
            }
        }

        /// <summary>
        /// 是否执行
        /// </summary>
        /// <returns>结果</returns>
        private bool IsDispense()
        {
            if (!MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                return false;
            }

            // 手动的情况下执行出胶
            if (this.IsManual && BondProgram.GetInstance().S2DispenserProgram.Dispenser.PreDispense.PreDispensingTiming == PreDispensingTimingEnum.Off)
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"预点胶功能未打开，请打开后重试 \r\n",
                    "预点胶执行失败报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);
                return false;
            }
            else if (this.IsManual)
            {
                return true;
            }
            else if (BondProgram.GetInstance().S2DispenserProgram.Dispenser.PreDispense.PreDispensingMode == PreDispensingModeEnum.Off
                && BondProgram.GetInstance().S2DispenserProgram.Dispenser.PreDispense.PreDispensingTiming == PreDispensingTimingEnum.Off)
            {
                return false;
            }
            else if (System2RunTimeProvider.IsFirstDispense)
            {
                return true;
            }
            else if (BondProgram.GetInstance().S2DispenserProgram.Dispenser.PreDispense.PreDispensingMode == PreDispensingModeEnum.Periodic)
            {
                int time = System2Domain.GetInstance().BondProgram.S2DispenserProgram.Dispenser.PreDispense.PreDispensingTimeInterval;
                if ((DateTime.Now - this.LastDispenseTime).TotalMilliseconds > time)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否报警
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsAlarm()
        {
            // 如果系统2没有点胶，则直接返回
            if (!MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                return false;
            }

            // 如果预点胶没有开启则返回
            if (BondProgram.GetInstance().S2DispenserProgram.Dispenser.PreDispense.PreDispensingMode == PreDispensingModeEnum.Off
               && BondProgram.GetInstance().S2DispenserProgram.Dispenser.PreDispense.PreDispensingTiming == PreDispensingTimingEnum.Off)
            {
                return false;
            }

            if (!BondProgram.GetInstance().S2DispenserProgram.Dispenser.IsAssistantSucceed)
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"点胶头未示教，无法执行预点胶 \r\n",
                    "预点胶无法执行报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);
                return true;
            }

            if (string.IsNullOrEmpty(BondProgram.GetInstance().S2DispenserProgram.Dispenser.PreDispense.EpoxyApplicationName))
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"未配置预点胶图形，无法执行预点胶 \r\n",
                    "预点胶执行失败报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);
                return true;
            }

            if (!System2Domain.GetInstance().BondProgram.S2PreDispensePlateProgram.IsReady())
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"预点胶板未示教成功，无法执行预点胶 \r\n",
                    "预点胶执行失败报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 挤胶
        /// </summary>
        private void EpoxySqueeze()
        {
            if (BondProgram.GetInstance().S2DispenserProgram.Dispenser.PreDispense.EnableEpoxySqueeze)
            {
                DispenseRunTimeProvider.RecordTime("预点胶", $"挤胶开始----------------");

                System2Domain.GetInstance().S2DispenseController.SetDispensePressure(BondProgram.GetInstance().S2DispenserProgram.Dispenser.PreDispense.EpoxySqueezePressure, 0);

                // 移动到挤胶位置
                System2Domain.GetInstance().BondModuleController
                    .MoveToG0Pos(BondDevicePara.GetInstance().S2DispenseDevicePara.ThrustPosition);
                System1Domain.GetInstance().DispenseController.OpenDispensingElectric();
                Thread.Sleep(BondProgram.GetInstance().S2DispenserProgram.Dispenser.PreDispense.EpoxySqueezeTime);
                System1Domain.GetInstance().DispenseController.CloseDispensingElectric();

                // 平移到擦胶的位置
                System1Domain.GetInstance().DispenseController
                    .MoveToG0Pos3DWithoutSafe(BondDevicePara.GetInstance().S2DispenseDevicePara.ErasePosition);

                DispenseRunTimeProvider.RecordTime("预点胶", $"挤胶结束----------------");
            }
        }

        /// <summary>
        /// 检测胶形
        /// </summary>
        /// <param name="postBondInspection">焊后检测</param>
        /// <param name="point3D">点位</param>
        /// <returns>结果</returns>
        private ExcuteResult CheckEpoxyApplication(PostBondInspection postBondInspection, AKRSPoint3D point3D)
        {
            AKRSPoint3D vison3D = point3D - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

            // 去拍照位置执行拍照
            List<BaseAlgResult> baseAlgResults = System2Domain.GetInstance().System2Controller.BondCameraVisionDefect(
                vison3D,
                postBondInspection.VisionConfig.P1PRName, false, true);

            return postBondInspection.PostBondEpoxyCheck(baseAlgResults, null, System2Domain.GetInstance().BondModuleController.GetHardware());
        }
    }
}
