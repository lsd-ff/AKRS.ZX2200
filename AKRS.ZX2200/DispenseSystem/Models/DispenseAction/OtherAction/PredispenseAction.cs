using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.ZX2200.DispenseSystem.Models.Programs;
using log4net.Core;
using Newtonsoft.Json;
using System;
using System.Windows.Forms;


namespace AKRS.ZX2200.DispenseSystem.Models.DispenseAction
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using System.Collections.Generic;
    using System.Threading;

    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.ZX2200.DispenseSystem.Models.DispenseAction.DispenseBPAction;
    using AKRS.ZX2200.Main.Machine.Product;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.ZX2200.BondSystem.Models;
    using System.Linq;

    /// <summary>
    /// 预点胶动作
    /// </summary>
    public class PreDispenseAction : ActionNode
    {
        /// <summary>
        /// 目前正在使用的点胶头程式
        /// </summary>
        public DispenserProgram DispenserProgram => System1Domain.GetInstance().System1Program.DispenserProgram;

        /// <summary>
        /// 预点胶板
        /// </summary>
        public PreDispensePlateProgram PreDispensePlateProgram =>
            System1Domain.GetInstance().System1Program.PreDispensePlateProgram;

        /// <summary>
        /// 预点胶板参数
        /// </summary>
        [JsonIgnore]
        public PreDispensePlatePara PreDispensePlatePara => DispenseDevicePara.GetInstance().PreDispensePlatePara;

        /// <summary>
        /// 点胶设备参数
        /// </summary>
        [JsonIgnore]
        public DispenseDevicePara DispenseDevicePara => DispenseDevicePara.GetInstance();

        /// <summary>
        /// 是否为手动模式
        /// </summary>
        public bool IsManual { get; set; } = false;

        /// <summary>
        /// 最后一次点胶时间
        /// </summary>
        private DateTime LastDispenseTime { get; set; } = DateTime.Now;

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

                // 判断预点胶的必要条件
                if (this.IsAlarm())
                {
                    return ExcuteResult.Abort;
                }

                // 挤胶
                this.EpoxySqueeze();
                
                DispenseRunTimeProvider.RecordTime("预点胶", $"预点胶开始----------------");

                // 获取点胶位置的集合
                List<AKRSPoint3D> lists = this.PreDispensePlateProgram
                    .GetEpoxy(this.DispenserProgram.Dispenser.PreDispense.RepeatPreDispensing);

                if (lists == null || lists.Count == 0)
                {
                    throw new Exception("预点胶位置获取失败，请检测程式是否存在问题");
                }

                // 获取预点胶的图案
                EpoxyApplication epoxyApplication = (EpoxyApplication)EpoxyApplicationRepository.GetInstance()
                    .Find(this.DispenserProgram.Dispenser.PreDispense.EpoxyApplicationName);

                DispenseRunTimeProvider.RecordTime("预点胶", $"获取预点胶图案：{this.DispenserProgram.Dispenser.PreDispense.EpoxyApplicationName}完成");

                foreach (AKRSPoint3D point in lists)
                {
                    // 空跑模式不出胶
                    bool isDrip = MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.DryCycle;

                    // 开始点胶动作
                    if (System1Domain.GetInstance().DispenseController.InterpolationApplication(epoxyApplication, point, isDrip, true, 0) != ExcuteResult.Success)
                    {
                        return ExcuteResult.Alarm;
                    }
                }

                DispenseRunTimeProvider.RecordTime("预点胶", $"预点胶结束----------------");

                // 获取胶后检测对象
                PostBondInspection postBondInspection = (PostBondInspection)PostBondInspectionRepository.GetInstance()
                    .Find(this.DispenserProgram.Dispenser.PreDispense.PreDispenseCheckName);

                DispenseRunTimeProvider.RecordTime("预点胶", $"获取胶后检测对象：{this.DispenserProgram.Dispenser.PreDispense.PreDispenseCheckName}完成");

                if (postBondInspection != null)
                {
                    this.CheckEpoxyApplication(postBondInspection, postBondInspection.VisionConfig.P1VisionPos + lists.Last());

                    DispenseRunTimeProvider.RecordTime("预点胶", $"胶后检测完成");
                }
                else if (this.IsManual)
                {
                    System1Domain.GetInstance().DispenseController.MoveZToSafePos();

                    DispenseRunTimeProvider.RecordTime("预点胶", $"手动模式-Z轴移到安全高度完成");

                    AKRSPoint3D visionPos = System1Domain.GetInstance().DispenseController.GetG0VisionPos(lists[lists.Count - 1]);

                    visionPos.Z += epoxyApplication.PrePlantOffSetZ;

                    System1Domain.GetInstance().DispenseController.MoveToG0Pos3D(visionPos);

                    DispenseRunTimeProvider.RecordTime("预点胶", $"手动模式-去视觉位完成");
                }

                this.LastDispenseTime = DateTime.Now;
                this.IsManual = false;
                
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
            if (this.IsManual)
            {
                // 手动模式直接执行预点胶，并记录时间
                DispenseRunTimeProvider.RecordTime("预点胶", $"预点胶手动开始");
                return true;
            }
            else if (System1Program.GetInstance().DispenserProgram.Dispenser.PreDispense.PreDispensingMode == PreDispensingModeEnum.Off
                && System1Program.GetInstance().DispenserProgram.Dispenser.PreDispense.PreDispensingTiming == PreDispensingTimingEnum.Off)
            {
                // 预点胶模式和时机都为Off，不执行预点胶
                return false;
            }
            else if (DispenseRunTimeProvider.IsFirstDispense
                     && System1Domain.GetInstance().ActionNodeController.CurrentActionNode is S1DispenseActionNode
                     && System1Program.GetInstance().DispenserProgram.Dispenser.PreDispense.PreDispensingTiming == PreDispensingTimingEnum.BeforeComingFirstDispense
                     )
            {
                // 预点胶时机为入料后第一次点胶前，并且是第一次点胶，执行预点胶，并记录时间
                DispenseRunTimeProvider.RecordTime("预点胶", $"预点胶条件通关 - 入料后第一次点胶前");
                return true;
            }
            else if ((DateTime.Now - this.LastDispenseTime).TotalMilliseconds
                     > System1Domain.GetInstance().System1Program.DispenserProgram.Dispenser.PreDispense.PreDispensingTimeInterval
                     && System1Program.GetInstance().DispenserProgram.Dispenser.PreDispense.PreDispensingMode == PreDispensingModeEnum.Periodic)
            {
                // 预点胶模式为周期性，并且距离上次点胶时间超过预设的时间间隔，执行预点胶，并记录时间
                DispenseRunTimeProvider.RecordTime("预点胶", $"预点胶条件通关 - 预点胶时间条件满足");
                return true;
            }

            return false;
        }

        /// <summary>
        /// 是否报警
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsAlarm()
        {
            if (System1Program.GetInstance().DispenserProgram.Dispenser == null
                || !System1Program.GetInstance().DispenserProgram.Dispenser.IsAssistantSucceed)
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"未配置点胶头或点胶头未示教，无法执行预点胶 \r\n",
                    "预点胶执行失败报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);
                return true;
            }

            if (string.IsNullOrEmpty(this.DispenserProgram.Dispenser.PreDispense.EpoxyApplicationName))
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"未配置预点胶图形，无法执行预点胶 \r\n",
                    "预点胶执行失败报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);
                return true;
            }

            if (!System1Domain.GetInstance().System1Program.PreDispensePlateProgram.IsReady())
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
            if (this.DispenserProgram.Dispenser.PreDispense.EnableEpoxySqueeze)
            {
                DispenseRunTimeProvider.RecordTime("预点胶", $"挤胶开始----------------");

                System1Domain.GetInstance().DispenseController.SetDispensePressure(this.DispenserProgram.Dispenser.PreDispense.EpoxySqueezePressure, 0);

                // 移动到挤胶位置
                System1Domain.GetInstance().DispenseController
                    .MoveToG0Pos3D(this.DispenseDevicePara.DispenserPara.ThrustPosition);
                System1Domain.GetInstance().DispenseController.OpenDispensingElectric();
                Thread.Sleep(this.DispenserProgram.Dispenser.PreDispense.EpoxySqueezeTime);
                System1Domain.GetInstance().DispenseController.CloseDispensingElectric();

                // 平移到擦胶的位置
                System1Domain.GetInstance().DispenseController
                    .MoveToG0Pos3DWithoutSafe(this.DispenseDevicePara.DispenserPara.ErasePosition);

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
            AKRSPoint3D vison3D = System1Domain.GetInstance().DispenseController.GetG0VisionPos(point3D);

            // 去拍照位置执行拍照
            List<BaseAlgResult> baseAlgResults = System1Domain.GetInstance().DispenseVisionController.DispenseVisionDefect(
                vison3D,
                postBondInspection.VisionConfig.P1PRName);

            return postBondInspection.PostBondEpoxyCheck(baseAlgResults, null, System1Domain.GetInstance().DispenseVisionController.GetHardware());
        }
    }
}
