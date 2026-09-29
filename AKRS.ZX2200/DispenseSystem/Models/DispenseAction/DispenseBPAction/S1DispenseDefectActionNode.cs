using System;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
using AKRS.ZX2200.Models;
using log4net.Core;
using Newtonsoft.Json;

namespace AKRS.ZX2200.DispenseSystem.Models.DispenseAction.DispenseBPAction
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Controls.Manual;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Process;
    using AKRS.ZX2200.SupportFeature.Statistics;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
    using DevExpress.XtraEditors;
    using UcMainSystem = AKRS.ZX2200.Main.Controls.Ucmain.MainControls.UcMainSystem;

    /// <summary>
    /// 胶型检测
    /// </summary>
    public class S1DispenseDefectActionNode : ActionNode
    {
        /// <summary>
        /// 设备参数
        /// </summary>
        [JsonIgnore]
        public DispenseDevicePara DispenseDevicePara => DispenseDevicePara.GetInstance();

        /// <summary>
        /// 系统1动作控制器
        /// </summary>
        private S1ActionNodeController ActionNodeController => System1Domain.GetInstance().ActionNodeController;

        /// <summary>
        /// 对话结果
        /// </summary>
        private DialogResult alertResult;

        /// <summary>
        /// 当前载具
        /// </summary>
        private TransportUnit TransportUnit => System1Domain.GetInstance().ActionNodeController.CurrentTransportUnit;

        /// <summary>
        /// 当前基板
        /// </summary>
        private Substrate Substrate => System1Domain.GetInstance().ActionNodeController.CurrentSubstrate;

        /// <summary>
        /// 当前基岛
        /// </summary>
        private Module Module => System1Domain.GetInstance().ActionNodeController.CurrentModule;

        /// <summary>
        /// 当前焊点
        /// </summary>
        private BondPosition BondPosition => System1Domain.GetInstance().ActionNodeController.CurrentBondPosition;

        /// <summary>
        /// 焊后检测对象,应该是从ProcessStep获取
        /// </summary>
        private PostBondInspection PostBondInspection =>
            System1Domain.GetInstance().ActionNodeController.CurrentPostBondInspection;

        /// <summary>
        /// 点胶视觉控制器
        /// </summary>
        private DispenseVisionController DispenseVisionController =>
            System1Domain.GetInstance().DispenseVisionController;

        /// <summary>
        /// 焊后检查
        /// </summary>
        /// <returns>结果</returns>
        public override ExcuteResult DoWork()
        {
            this.WorkStart?.Invoke();
            this.State = RunStateEnum.Running;
            try
            {
                DispenseRunTimeProvider.RecordTime("系统1胶量检测", "Start ----------------");

                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    // 修改当前焊点的状态为点胶完成
                    this.BondPosition.S1FinishedStep(this.ActionNodeController.SingleProcessStepName);
                }

                DispenseRunTimeProvider.RecordTime($"系统1胶量检测", $"焊点信息 名称:{this.BondPosition.Name},基板号{this.BondPosition.SubstrateNum},基岛号{this.BondPosition.ModuleNum}");

                switch (this.PostBondInspection.PostBondInspectionMode)
                {
                    case PostBondInspectionModeEnum.WithoutReferenceSearch:

                        // 检测位置的计算
                        AKRSPoint3D point = BondPosition.CoordinateSystem.SelfPosToG0(
                            this.PostBondInspection.VisionConfig.P1VisionPos);
                        
                        AKRSPoint3D p1VisionPosInG01 = System1Domain.GetInstance().DispenseController.GetG0VisionPos(point);

                        DispenseRunTimeProvider.RecordTime("系统1胶量检测", $"胶量检测位置{p1VisionPosInG01}");

                        // 去拍照位置执行拍照
                        List<BaseAlgResult> baseAlgResults = this.DispenseVisionController.DispenseVisionDefect(
                        p1VisionPosInG01,
                        this.PostBondInspection.VisionConfig.P1PRName);

                        DispenseRunTimeProvider.RecordTime("系统1胶量检测", $"胶量检测视觉处理结束");

                        // 结果检测
                        return this.EpoxyApplicationDefect(baseAlgResults);
                }

                DispenseRunTimeProvider.RecordTime("系统1胶量检测", "End ----------------");
                return ExcuteResult.Success;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程{this.Name}运行故障！", ex, LogCategory.Dispense);
                AKRSMessageBoxExt.Show(
                    ex.Message,
                    "异常",
                    new string[] { "异常" },
                    new DialogResult[] { DialogResult.Yes });
                return ExcuteResult.Exception;
            }
            finally
            {
                this.State = RunStateEnum.Stop;
                this.WorkStop?.Invoke();
                UcMainSystem.ReFreshSystem1TuAction();
            }
        }

        /// <summary>
        /// 完成动作的条件是否满足
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsDoWork()
        {
            // 焊点
            BondPosition bp = System1Domain.GetInstance().ActionNodeController.CurrentBondPosition;

            if (bp == null)
            {
                throw new Exception("获取当前焊点失败！请检查当前焊点是否已经被删除或者工作步骤配置有问题。\r\n 请清空载具后重试");
            }

            // 判断当前工艺制程是否开启
            if (bp.MatterProductState == MatterProductState.Disable
                || bp.MatterProductState == MatterProductState.EnableInSystem2)
            {
                return false;
            }

            if (!bp.IsS1NeedStep(this.ActionNodeController.SingleProcessStepName))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 补胶
        /// </summary>
        private void ReDispense()
        {
            // 焊点
            BondPosition bp = System1Domain.GetInstance().ActionNodeController.CurrentBondPosition;
            
            // 重置前面的动作节点,重新取贴
            List<ActionNode> actionNodeList = new List<ActionNode>
                                                  {
                                                      System1Domain.GetInstance().DispenseActionNodes.S1DispenseActionNode,
                                                      System1Domain.GetInstance().DispenseActionNodes.S1DispenseDefectActionNode,
                                                  };
            
            // 将所有的都变成没有完成，后面后修改
            foreach (var key in bp.BondPositionInfo.S1RemainingSteps.Keys)
            {
                bp.BondPositionInfo.S1RemainingSteps[key] = false;
                DispenseRunTimeProvider.RecordTime("系统1胶量检测", $"补胶将步骤{key}改为未完成");
            }

            System1Domain.GetInstance().ActionNodeController.SetActionNodeReWork(actionNodeList);
        }

        /// <summary>
        /// 检测结果处理
        /// </summary>
        /// <param name="baseAlgResults">检测结果</param>
        /// <returns>结果</returns>
        /// <exception cref="Exception">未知异常</exception>
        private ExcuteResult EpoxyApplicationDefect(List<BaseAlgResult> baseAlgResults)
        {
            ExcuteResult excuteResult = this.PostBondInspection.PostBondEpoxyCheck(baseAlgResults, BondPosition, System1Domain.GetInstance().DispenseVisionController.GetHardware());

            DispenseRunTimeProvider.RecordTime("系统1胶量检测", $"胶量检测处理结果为{excuteResult}");

            if (excuteResult == ExcuteResult.Success)
            {
                BondPosition.S1FinishedStep(this.ActionNodeController.SingleProcessStepName);
                return ExcuteResult.Success;
            }
            else if (excuteResult == ExcuteResult.Fail)
            {
                BondPosition.SetMatterDisable();
                return ExcuteResult.Success;
            }
            else if (excuteResult == ExcuteResult.Abort)
            {
                Machine.GetInstance().Stop();
                return ExcuteResult.Abort;
            }
            else if (excuteResult == ExcuteResult.Retry)
            {
                this.ReDispense();
                return ExcuteResult.Retry;
            }
            else
            {
                throw new Exception("胶量检测未知错误");
            }
        }
    }
}
