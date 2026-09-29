using System;
using System.Collections.Generic;
using System.Linq;

namespace AKRS.ZX2200.Main.Machine.Process
{
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.ActionNodes;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Services;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Module.Information;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using DevExpress.Office.Utils;
    using DevExpress.XtraEditors;
    using Newtonsoft.Json;
    using System.Windows.Forms;

    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Product;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    /// <summary>
    /// 动作执行执行器
    /// </summary>
    public class S2ActionNodeController
    {
        #region 动作信息

        /// <summary>
        /// 系统2目前流道上的载具
        /// </summary>
        [JsonIgnore]
        private TransportUnit TransportUnit =>
            TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

        /// <summary>
        /// 动作集合
        /// </summary>
        private List<ActionNode> ActionNodes => System2Domain.GetInstance().BondActionNodeRepository.ActionNodes;

        /// <summary>
        /// 点胶当作做的动作信息
        /// 当它为null时，为第一次
        /// </summary>
        private ActionNodeMessage CurrentBondActionNode { get; set; }

        /// <summary>
        /// 当前程式
        /// </summary>
        private SingleProcessStep ProcessStep { get; set; }

        /// <summary>
        /// 当前作业的基岛
        /// </summary>
        private Module CurrentModule { get; set; }

        /// <summary>
        /// 当前作业的substrate
        /// </summary>
        private Substrate CurrentSubstrate { get; set; }

        /// <summary>
        /// 当前作业的焊点
        /// </summary>
        private BondPosition BondPosition { get; set; }

        /// <summary>
        /// 焊后检测
        /// </summary>
        private PostBondInspection PostBondInspection { get; set; }

        /// <summary>
        /// 点胶图案
        /// </summary>
        private EpoxyApplication EpoxyApplication { get; set; }

        #endregion

        /// <summary>
        /// 步骤的集合
        /// </summary>
        private List<SingleProcessStep> ProcessSteps =>
            ProcessDomain.GetInstance().S2SystemProcess.ProcessSteps.FindAll(it => it.IsEnable);

        /// <summary>
        /// 准备动作
        /// </summary>
        private List<ActionNode> TransportPreActionNodes => System2Domain.GetInstance().BondActionNodeRepository.TransportActonNodes;

        /// <summary>
        /// 准备动作
        /// </summary>
        private List<ActionNode> SubstrateActonNodes => System2Domain.GetInstance().BondActionNodeRepository.SubstrateActonNodes;

        /// <summary>
        /// 准备动作
        /// </summary>
        private List<ActionNode> ModuleActonNodes => System2Domain.GetInstance().BondActionNodeRepository.ModuleActonNodes;

        /// <summary>
        /// 准备动作
        /// </summary>
        private List<ActionNode> BondPositionActonNodes => System2Domain.GetInstance().BondActionNodeRepository.BondPositionActonNodes;

        /// <summary>
        /// 执行步骤
        /// </summary>
        private List<ActionNodeMessage> WorkMessages { get; set; } = new List<ActionNodeMessage>();

        /// <summary>
        /// 基板数量
        /// </summary>
        private int SubstrateCount => this.GetSubstrateCount();

        /// <summary>
        /// 基岛数量
        /// </summary>
        private int ModuleCount => this.GetModuleCount();

        /// <summary>
        /// 获取基板的数量
        /// </summary>
        /// <returns>结果</returns>
        private int GetSubstrateCount()
        {
            return ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex
                       ? ProductConfiguration.GetInstance().OppositeSexConfiguration.GetSubstrateCount()
                       : ProductConfiguration.GetInstance().SubstrateConfig.Count;
        }

        /// <summary>
        /// 获取基板的数量
        /// </summary>
        /// <returns>结果</returns>
        private int GetModuleCount()
        {
            return ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex
                       ? ProductConfiguration.GetInstance().OppositeSexConfiguration.GetModuleCount()
                       : ProductConfiguration.GetInstance().ModuleConfig.Count;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            this.WorkMessages.Clear();

            if (this.ProcessSteps == null || this.ProcessSteps.Count == 0)
            {
                return;
            }

            this.ProcessStep = null;

            this.SortActionNodeByModule(
                ProcessDomain.GetInstance().S2SystemProcess.WorkProcess,
                ProcessDomain.GetInstance().S2SystemProcess.WorkProcessModule);

            this.RemoveOtherStep();

            this.RemoveDisableActionNode();

            this.SortRelativeBond();
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="workProcess">工作步骤</param>
        /// <param name="workProcessModule">工作模式</param>
        private void SortActionNodeByModule(WorkProcessEnum workProcess, WorkProcessModuleEnum workProcessModule)
        {
            switch (workProcess)
            {
                case WorkProcessEnum.TransportFirst:
                    if (workProcessModule == WorkProcessModuleEnum.StepFirst)
                    {
                        this.SortTransportFirsAndStepFirst();
                    }
                    else
                    {
                        this.SortTransportFirsAndPositionFirst();
                    }

                    break;

                case WorkProcessEnum.SubstrateFirst:
                    if (workProcessModule == WorkProcessModuleEnum.StepFirst)
                    {
                        this.SortSubstrateFirsAndStepFirst();
                    }
                    else
                    {
                        this.SortSubstrateFirsAndPositionFirst();
                    }

                    break;

                case WorkProcessEnum.ModuleFirst:
                    if (workProcessModule == WorkProcessModuleEnum.StepFirst)
                    {
                        this.SortModuleFirsAndStepFirst();
                    }
                    else
                    {
                        this.SortModuleFirsAndPositionFirst();
                    }

                    break;

                case WorkProcessEnum.BondPositionFirst:
                    if (workProcessModule == WorkProcessModuleEnum.StepFirst)
                    {
                        this.SortBondPositionFirsAndStepFirst();
                    }
                    else
                    {
                        this.SortBondPositionFirsAndPositionFirst();
                    }

                    break;
            }
        }

        #region 排列方法

        /// <summary>
        /// 基板优先和步骤优先
        /// </summary>
        private void SortTransportFirsAndStepFirst()
        {
            List<SingleProcessStep> bondPositionProcessSteps = this.GetWorkBondPosition();

            int workCycleNumber = ProcessDomain.GetInstance().S2SystemProcess.WorkCycleNumber;

            if (!ProcessDomain.GetInstance().S2SystemProcess.IsWorkCycleNumber)
            {
                workCycleNumber = this.SubstrateCount;
            }

            List<List<int>> cycleSubstrate = MathHelper.GetCycleList(
                this.SubstrateCount,
                workCycleNumber);

            foreach (ActionNode actionNode in this.TransportPreActionNodes)
            {
                this.WorkMessages.Add(new ActionNodeMessage(actionNode.Name, 0, 0, null));
            }

            foreach (List<int> list in cycleSubstrate)
            {
                foreach (ActionNode actionNodeSub in this.SubstrateActonNodes)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        this.WorkMessages.Add(new ActionNodeMessage(actionNodeSub.Name, list[i] + 1, 0, null));
                    }
                }

                foreach (ActionNode actionNodeMo in this.ModuleActonNodes)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        for (int j = 0; j < this.ModuleCount; j++)
                        {
                            this.WorkMessages.Add(new ActionNodeMessage(actionNodeMo.Name, list[i] + 1, j + 1, null));
                        }
                    }
                }

                foreach (ActionNode actionNodeMoBond in this.BondPositionActonNodes)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        for (int j = 0; j < this.ModuleCount; j++)
                        {
                            foreach (SingleProcessStep singleProcessStep in bondPositionProcessSteps)
                            {
                                this.WorkMessages.Add(
                                    new ActionNodeMessage(
                                        actionNodeMoBond.Name,
                                        list[i] + 1,
                                        j + 1,
                                        singleProcessStep.ProcessStepName));
                            }
                        }
                    }
                }

                foreach (SingleProcessStep singleProcessStep in this.ProcessSteps)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        for (int j = 0; j < this.ModuleCount; j++)
                        {
                            this.WorkMessages.AddRange(
                                this.GetActionNodeMessageByProcess(singleProcessStep, list[i] + 1, j + 1));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 基板优先和步骤优先
        /// </summary>
        private void SortTransportFirsAndPositionFirst()
        {
            List<SingleProcessStep> bondPositionProcessSteps = this.GetWorkBondPosition();

            int workCycleNumber = ProcessDomain.GetInstance().S2SystemProcess.WorkCycleNumber;

            if (!ProcessDomain.GetInstance().S2SystemProcess.IsWorkCycleNumber)
            {
                workCycleNumber = this.SubstrateCount;
            }

            List<List<int>> cycleSubstrate = MathHelper.GetCycleList(
                this.SubstrateCount,
                workCycleNumber);

            foreach (ActionNode actionNode in this.TransportPreActionNodes)
            {
                this.WorkMessages.Add(new ActionNodeMessage(actionNode.Name, 0, 0, null));
            }

            foreach (List<int> list in cycleSubstrate)
            {
                foreach (ActionNode actionNodeSub in this.SubstrateActonNodes)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        this.WorkMessages.Add(new ActionNodeMessage(actionNodeSub.Name, list[i] + 1, 0, null));
                    }
                }

                foreach (ActionNode actionNodeMo in this.ModuleActonNodes)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        for (int j = 0; j < this.ModuleCount; j++)
                        {
                            this.WorkMessages.Add(new ActionNodeMessage(actionNodeMo.Name, list[i] + 1, j + 1, null));
                        }
                    }
                }

                foreach (ActionNode actionNodeMoBond in this.BondPositionActonNodes)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        for (int j = 0; j < this.ModuleCount; j++)
                        {
                            foreach (SingleProcessStep singleProcessStep in bondPositionProcessSteps)
                            {
                                this.WorkMessages.Add(
                                    new ActionNodeMessage(
                                        actionNodeMoBond.Name,
                                        list[i] + 1,
                                        j + 1,
                                        singleProcessStep.ProcessStepName));
                            }
                        }
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = 0; j < this.ModuleCount; j++)
                    {
                        foreach (SingleProcessStep singleProcessStep in this.ProcessSteps)
                        {
                            this.WorkMessages.AddRange(
                                this.GetActionNodeMessageByProcess(singleProcessStep, list[i] + 1, j + 1));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 基板优先和步骤优先
        /// </summary>
        private void SortSubstrateFirsAndStepFirst()
        {
            List<SingleProcessStep> bondPositionProcessSteps = this.GetWorkBondPosition();

            int workCycleNumber = ProcessDomain.GetInstance().S2SystemProcess.WorkCycleNumber;

            if (!ProcessDomain.GetInstance().S2SystemProcess.IsWorkCycleNumber)
            {
                workCycleNumber = this.ModuleCount;
            }

            List<List<int>> cycleModule = MathHelper.GetCycleList(
                this.ModuleCount,
                workCycleNumber);

            foreach (ActionNode actionNode in this.TransportPreActionNodes)
            {
                this.WorkMessages.Add(new ActionNodeMessage(actionNode.Name, 0, 0, null));
            }

            for (int i = 0; i < this.SubstrateCount; i++)
            {
                foreach (ActionNode actionNodeSub in this.SubstrateActonNodes)
                {
                    this.WorkMessages.Add(new ActionNodeMessage(actionNodeSub.Name, i + 1, 0, null));
                }

                foreach (List<int> list in cycleModule)
                {
                    foreach (ActionNode actionNodeMo in this.ModuleActonNodes)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            this.WorkMessages.Add(new ActionNodeMessage(actionNodeMo.Name, i + 1, list[j] + 1, null));
                        }
                    }

                    foreach (ActionNode actionNodeMoBond in this.BondPositionActonNodes)
                    {
                        foreach (SingleProcessStep singleProcessStep in bondPositionProcessSteps)
                        {
                            for (int j = 0; j < list.Count; j++)
                            {
                                this.WorkMessages.Add(
                                    new ActionNodeMessage(
                                        actionNodeMoBond.Name,
                                        i + 1,
                                        list[j] + 1,
                                        singleProcessStep.ProcessStepName));
                            }
                        }
                    }

                    foreach (SingleProcessStep singleProcessStep in this.ProcessSteps)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            this.WorkMessages.AddRange(
                                this.GetActionNodeMessageByProcess(singleProcessStep, i + 1, list[j] + 1));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 基板优先和步骤优先
        /// </summary>
        private void SortSubstrateFirsAndPositionFirst()
        {
            int workCycleNumber = ProcessDomain.GetInstance().S2SystemProcess.WorkCycleNumber;

            if (!ProcessDomain.GetInstance().S2SystemProcess.IsWorkCycleNumber)
            {
                workCycleNumber = this.ModuleCount;
            }

            List<List<int>> cycleModule = MathHelper.GetCycleList(
                this.ModuleCount,
                workCycleNumber);

            List<SingleProcessStep> bondPositionProcessSteps = this.GetWorkBondPosition();
            foreach (ActionNode actionNode in this.TransportPreActionNodes)
            {
                this.WorkMessages.Add(new ActionNodeMessage(actionNode.Name, 0, 0, null));
            }

            for (int i = 0; i < this.SubstrateCount; i++)
            {
                foreach (ActionNode actionNodeSub in this.SubstrateActonNodes)
                {
                    this.WorkMessages.Add(new ActionNodeMessage(actionNodeSub.Name, i + 1, 0, null));
                }

                foreach (List<int> list in cycleModule)
                {
                    foreach (ActionNode actionNodeMo in this.ModuleActonNodes)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            this.WorkMessages.Add(new ActionNodeMessage(actionNodeMo.Name, i + 1, list[j] + 1, null));
                        }
                    }

                    foreach (ActionNode actionNodeMoBond in this.BondPositionActonNodes)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            foreach (SingleProcessStep singleProcessStep in bondPositionProcessSteps)
                            {
                                this.WorkMessages.Add(new ActionNodeMessage(actionNodeMoBond.Name, i + 1, list[j] + 1, singleProcessStep.ProcessStepName));
                            }
                        }
                    }

                    for (int j = 0; j < list.Count; j++)
                    {
                        foreach (SingleProcessStep singleProcessStep in this.ProcessSteps)
                        {
                            this.WorkMessages.AddRange(
                                this.GetActionNodeMessageByProcess(singleProcessStep, i + 1, list[j] + 1));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 基板优先和步骤优先
        /// </summary>
        private void SortModuleFirsAndStepFirst()
        {
            List<SingleProcessStep> bondPositionProcessSteps = this.GetWorkBondPosition();
            foreach (ActionNode actionNode in this.TransportPreActionNodes)
            {
                this.WorkMessages.Add(new ActionNodeMessage(actionNode.Name, 0, 0, null));
            }

            for (int i = 0; i < this.SubstrateCount; i++)
            {
                foreach (ActionNode actionNodeSub in this.SubstrateActonNodes)
                {
                    this.WorkMessages.Add(new ActionNodeMessage(actionNodeSub.Name, i + 1, 0, null));
                }

                for (int j = 0; j < this.ModuleCount; j++)
                {
                    foreach (ActionNode actionNodeMo in this.ModuleActonNodes)
                    {
                        this.WorkMessages.Add(new ActionNodeMessage(actionNodeMo.Name, i + 1, j + 1, null));

                    }

                    foreach (ActionNode actionNodeMoBond in this.BondPositionActonNodes)
                    {
                        foreach (SingleProcessStep singleProcessStep in bondPositionProcessSteps)
                        {
                            this.WorkMessages.Add(
                                new ActionNodeMessage(
                                    actionNodeMoBond.Name,
                                    i + 1,
                                    j + 1,
                                    singleProcessStep.ProcessStepName));
                        }
                    }

                    foreach (SingleProcessStep singleProcessStep in this.ProcessSteps)
                    {
                        this.WorkMessages.AddRange(
                            this.GetActionNodeMessageByProcess(singleProcessStep, i + 1, j + 1));
                    }
                }
            }
        }

        /// <summary>
        /// 基板优先和步骤优先
        /// </summary>
        private void SortModuleFirsAndPositionFirst()
        {
            List<SingleProcessStep> bondPositionProcessSteps = this.GetWorkBondPosition();
            foreach (ActionNode actionNode in this.TransportPreActionNodes)
            {
                this.WorkMessages.Add(new ActionNodeMessage(actionNode.Name, 0, 0, null));
            }

            for (int i = 0; i < this.SubstrateCount; i++)
            {
                foreach (ActionNode actionNodeSub in this.SubstrateActonNodes)
                {
                    this.WorkMessages.Add(new ActionNodeMessage(actionNodeSub.Name, i + 1, 0, null));
                }

                for (int j = 0; j < this.ModuleCount; j++)
                {
                    foreach (ActionNode actionNodeMo in this.ModuleActonNodes)
                    {
                        this.WorkMessages.Add(new ActionNodeMessage(actionNodeMo.Name, i + 1, j + 1, null));

                    }

                    foreach (ActionNode actionNodeMoBond in this.BondPositionActonNodes)
                    {
                        foreach (SingleProcessStep singleProcessStep in bondPositionProcessSteps)
                        {
                            this.WorkMessages.Add(new ActionNodeMessage(actionNodeMoBond.Name, i + 1, j + 1, singleProcessStep.ProcessStepName));
                        }
                    }

                    foreach (SingleProcessStep singleProcessStep in this.ProcessSteps)
                    {
                        this.WorkMessages.AddRange(
                            this.GetActionNodeMessageByProcess(singleProcessStep, i + 1, j + 1));
                    }
                }
            }
        }

        /// <summary>
        /// 基板优先和步骤优先
        /// </summary>
        private void SortBondPositionFirsAndStepFirst()
        {
            List<SingleProcessStep> bondPositionProcessSteps = this.GetWorkBondPosition();

            foreach (ActionNode actionNode in this.TransportPreActionNodes)
            {
                this.WorkMessages.Add(new ActionNodeMessage(actionNode.Name, 0, 0, null));
            }

            for (int i = 0; i < this.SubstrateCount; i++)
            {
                foreach (ActionNode actionNodeSub in this.SubstrateActonNodes)
                {
                    this.WorkMessages.Add(new ActionNodeMessage(actionNodeSub.Name, i + 1, 0, null));
                }

                for (int j = 0; j < this.ModuleCount; j++)
                {
                    foreach (ActionNode actionNodeMo in this.ModuleActonNodes)
                    {
                        this.WorkMessages.Add(new ActionNodeMessage(actionNodeMo.Name, i + 1, j + 1, null));
                    }

                    foreach (ActionNode actionNodeMoBond in this.BondPositionActonNodes)
                    {
                        foreach (SingleProcessStep bondPositionProcessStep in bondPositionProcessSteps)
                        {
                            this.WorkMessages.Add(
                                new ActionNodeMessage(
                                    actionNodeMoBond.Name,
                                    i + 1,
                                    j + 1,
                                    bondPositionProcessStep.ProcessStepName));
                        }
                    }

                    foreach (SingleProcessStep singleProcessStep in this.ProcessSteps)
                    {
                        this.WorkMessages.AddRange(
                            this.GetActionNodeMessageByProcess(singleProcessStep, i + 1, j + 1));
                    }
                }
            }
        }

        /// <summary>
        /// 焊点优先和位置优先
        /// </summary>
        private void SortBondPositionFirsAndPositionFirst()
        {
            foreach (ActionNode actionNode in this.TransportPreActionNodes)
            {
                this.WorkMessages.Add(new ActionNodeMessage(actionNode.Name, 0, 0, null));
            }

            for (int i = 0; i < this.SubstrateCount; i++)
            {
                foreach (ActionNode actionNodeSub in this.SubstrateActonNodes)
                {
                    this.WorkMessages.Add(new ActionNodeMessage(actionNodeSub.Name, i + 1, 0, null));
                }

                for (int j = 0; j < this.ModuleCount; j++)
                {
                    foreach (ActionNode actionNodeMo in this.ModuleActonNodes)
                    {
                        this.WorkMessages.Add(new ActionNodeMessage(actionNodeMo.Name, i + 1, j + 1, null));
                    }

                    List<SingleProcessStep> bondPositionProcessSteps = this.GetWorkBondPosition();

                    foreach (SingleProcessStep singleProcessStep in this.ProcessSteps)
                    {
                        if (bondPositionProcessSteps.Contains(singleProcessStep))
                        {
                            foreach (ActionNode actionNodeMoBond in this.BondPositionActonNodes)
                            {
                                this.WorkMessages.Add(new ActionNodeMessage(actionNodeMoBond.Name, i + 1, j + 1, singleProcessStep.ProcessStepName));
                            }

                            bondPositionProcessSteps.Remove(singleProcessStep);
                        }

                        this.WorkMessages.AddRange(
                            this.GetActionNodeMessageByProcess(singleProcessStep, i + 1, j + 1));
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// 根据步骤获取动作
        /// </summary>
        /// <param name="singleProcess">步骤名称</param>
        /// <param name="substrateIndex">基板索引</param>
        /// <param name="moduleIndex">基岛索引</param>
        /// <returns>结果</returns>
        private List<ActionNodeMessage> GetActionNodeMessageByProcess(SingleProcessStep singleProcess, int substrateIndex, int moduleIndex)
        {
            List<ActionNodeMessage> actionNode = new List<ActionNodeMessage>();

            BondActionNodeRepository bondActionNodeRepository = System2Domain.GetInstance().BondActionNodeRepository;

            if (singleProcess.ComponentName != "Null")
            {
                BaseCarrierConfig baseCarrierConfig =
                    (BaseCarrierConfig)CarrierConfigRepository.GetInstance().Find(singleProcess.ComponentName);

                if (baseCarrierConfig == null)
                {
                    throw new Exception($"芯片{singleProcess.ComponentName}未找到，可能已被删除，请检测并重新设置工作步骤。");
                }

                if (baseCarrierConfig.IsUseFlipTable)
                {
                    if (baseCarrierConfig.DipMode == DipModeEnum.AfterAccuracyMode)
                    {
                        actionNode.Add(
                            new ActionNodeMessage(
                                bondActionNodeRepository.PickActionNode.Name,
                                substrateIndex,
                                moduleIndex,
                                singleProcess.ProcessStepName));
                        actionNode.Add(
                            new ActionNodeMessage(
                                bondActionNodeRepository.UpLookCorrectionActionNode.Name,
                                substrateIndex,
                                moduleIndex,
                                singleProcess.ProcessStepName));
                        actionNode.Add(
                            new ActionNodeMessage(
                                bondActionNodeRepository.DipFluxActionNode.Name,
                                substrateIndex,
                                moduleIndex,
                                singleProcess.ProcessStepName));
                        actionNode.Add(
                            new ActionNodeMessage(
                                bondActionNodeRepository.SubstrateCameraCorrectionActionNode.Name,
                                substrateIndex,
                                moduleIndex,
                                singleProcess.ProcessStepName));
                        actionNode.Add(
                            new ActionNodeMessage(
                                bondActionNodeRepository.BondActionNode.Name,
                                substrateIndex,
                                moduleIndex,
                                singleProcess.ProcessStepName));
                    }
                    else if (baseCarrierConfig.DipMode == DipModeEnum.Off)
                    {
                        actionNode.Add(
                               new ActionNodeMessage(
                                   bondActionNodeRepository.PickActionNode.Name,
                                   substrateIndex,
                                   moduleIndex,
                                   singleProcess.ProcessStepName));
                        actionNode.Add(
                            new ActionNodeMessage(
                                bondActionNodeRepository.UpLookCorrectionActionNode.Name,
                                substrateIndex,
                                moduleIndex,
                                singleProcess.ProcessStepName));
                        actionNode.Add(
                            new ActionNodeMessage(
                                bondActionNodeRepository.SubstrateCameraCorrectionActionNode.Name,
                                substrateIndex,
                                moduleIndex,
                                singleProcess.ProcessStepName));
                        actionNode.Add(
                            new ActionNodeMessage(
                                bondActionNodeRepository.BondActionNode.Name,
                                substrateIndex,
                                moduleIndex,
                                singleProcess.ProcessStepName));
                    }
                    else
                    {
                        actionNode.Add(
                            new ActionNodeMessage(
                                bondActionNodeRepository.PickActionNode.Name,
                                substrateIndex,
                                moduleIndex,
                                singleProcess.ProcessStepName));
                        actionNode.Add(
                            new ActionNodeMessage(
                                bondActionNodeRepository.DipFluxActionNode.Name,
                                substrateIndex,
                                moduleIndex,
                                singleProcess.ProcessStepName));
                        actionNode.Add(
                            new ActionNodeMessage(
                                bondActionNodeRepository.UpLookCorrectionActionNode.Name,
                                substrateIndex,
                                moduleIndex,
                                singleProcess.ProcessStepName));
                        actionNode.Add(
                            new ActionNodeMessage(
                                bondActionNodeRepository.SubstrateCameraCorrectionActionNode.Name,
                                substrateIndex,
                                moduleIndex,
                                singleProcess.ProcessStepName));
                        actionNode.Add(
                            new ActionNodeMessage(
                                bondActionNodeRepository.BondActionNode.Name,
                                substrateIndex,
                                moduleIndex,
                                singleProcess.ProcessStepName));
                    }
                }
                else
                {
                    actionNode.Add(
                        new ActionNodeMessage(
                            bondActionNodeRepository.PickActionNode.Name,
                            substrateIndex,
                            moduleIndex,
                            singleProcess.ProcessStepName));
                    actionNode.Add(
                        new ActionNodeMessage(
                            bondActionNodeRepository.UpLookCorrectionActionNode.Name,
                            substrateIndex,
                            moduleIndex,
                            singleProcess.ProcessStepName));
                    actionNode.Add(
                        new ActionNodeMessage(
                            bondActionNodeRepository.SubstrateCameraCorrectionActionNode.Name,
                            substrateIndex,
                            moduleIndex,
                            singleProcess.ProcessStepName));
                    actionNode.Add(
                        new ActionNodeMessage(
                            bondActionNodeRepository.BondActionNode.Name,
                            substrateIndex,
                            moduleIndex,
                            singleProcess.ProcessStepName));
                }
            }
            else if (singleProcess.DefectName != "Null")
            {
                actionNode.Add(
                    new ActionNodeMessage(
                        bondActionNodeRepository.AfterBondCheckActionNode.Name,
                        substrateIndex,
                        moduleIndex,
                        singleProcess.ProcessStepName));
            }
            else if (singleProcess.EpoxyNameApplicationName != "Null")
            {
                actionNode.Add(
                    new ActionNodeMessage(
                        bondActionNodeRepository.S2DispenseActionNode.Name,
                        substrateIndex,
                        moduleIndex,
                        singleProcess.ProcessStepName));
            }

            return actionNode;
        }

        /// <summary>
        /// 获取需要的焊点
        /// </summary>
        /// <returns>结果</returns>
        private List<SingleProcessStep> GetWorkBondPosition()
        {
            return new List<SingleProcessStep>(
                this.ProcessSteps.GroupBy(it => it.BondPositionName).Select(d => d.FirstOrDefault()));
        }

        /// <summary>
        /// 获取第一颗要做的芯片名称
        /// </summary>
        /// <returns>芯片</returns>
        public BaseCarrierConfig GetFirstComponent()
        {
            if (this.ProcessSteps.Count != 0)
            {
                SingleProcessStep singleProcessStep = this.ProcessSteps.Find(it => it.ComponentName != "Null");

                if (singleProcessStep == null)
                {
                    return null;
                }

                return (BaseCarrierConfig)CarrierConfigRepository.GetInstance().Find(singleProcessStep.ComponentName);
            }

            return null;
        }

        /// <summary>
        /// 完成
        /// </summary>
        public void Finish()
        {
            if (this.WorkMessages == null)
            {
                return;
            }

            foreach (ActionNodeMessage actionNodeMessage in this.WorkMessages)
            {
                actionNodeMessage.Finished = false;
            }

            // 将当前动作改成0
            this.CurrentBondActionNode = this.WorkMessages[0];
        }

        /// <summary>
        /// 初始化当前信息
        /// </summary>
        public void InitCurrentMessage()
        {
            this.ProcessStep = this.ProcessSteps.Find(it => it.ProcessStepName == this.CurrentBondActionNode.ProcessStepName);

            if (this.ProcessStep != null)
            {
                this.PostBondInspection = (PostBondInspection)PostBondInspectionRepository.GetInstance()
                    .Find(this.ProcessStep.DefectName);

                this.EpoxyApplication = (EpoxyApplication)EpoxyApplicationRepository.GetInstance()
                    .Find(this.ProcessStep.EpoxyNameApplicationName);
            }

            int substrateIndex = this.CurrentBondActionNode.Substrate;

            try
            {
                foreach (Substrate substrate in this.TransportUnit.Substrates)
                {
                    if (substrate.WorkIndex == substrateIndex)
                    {
                        this.CurrentSubstrate = substrate;
                        foreach (Module island in substrate.Modules)
                        {
                            if (island.WorkIndex == this.CurrentBondActionNode.Module)
                            {
                                this.CurrentModule = island;
                                foreach (BondPosition bondPosition in island.BondPositions)
                                {
                                    if (this.ProcessStep != null && bondPosition.Name == this.ProcessStep.BondPositionName)
                                    {
                                        this.BondPosition = bondPosition;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 获取当前动作的BondPosition
        /// </summary>
        /// <returns>当前的module</returns>
        public BondPosition GetCurrentBondPosition()
        {
            return this.BondPosition;
        }

        /// <summary>
        /// 获取当前动作的Island
        /// </summary>
        /// <returns>当前的module</returns>
        public Module GetCurrentModule()
        {
            return this.CurrentModule;
        }

        /// <summary>
        /// 获取当前动作的Substrate
        /// </summary>
        /// <returns>当前的Substrate</returns>
        public Substrate GetCurrentSubstrate()
        {
            return this.CurrentSubstrate;
        }

        /// <summary>
        /// 获取当前动作的PostBondInspection
        /// </summary>
        /// <returns>当前的PostBondInspection</returns>
        public PostBondInspection GetCurrentPostBondInspection()
        {
            return this.PostBondInspection;
        }

        /// <summary>
        /// 当前画胶图形
        /// </summary>
        /// <returns>当前画胶图形的实体</returns>
        public EpoxyApplication GetCurrentEpoxyApplication()
        {
            return this.EpoxyApplication;
        }

        /// <summary>
        /// 获取当前步骤的名称
        /// </summary>
        /// <returns>名称</returns>
        public string GetCurrentProcessStepName()
        {
            return this.CurrentBondActionNode.ProcessStepName;
        }

        /// <summary>
        /// 完成当前节点
        /// </summary>
        public void FinishCurrentActionNode()
        {
            this.CurrentBondActionNode.Finished = true;
        }

        /// <summary>
        /// 获取下一个动作
        /// </summary>
        /// <returns>动作</returns>
        public ActionNode GetNextActionNode()
        {
            string currentActionNodeName = null;
            if (this.CurrentBondActionNode != null)
            {
                currentActionNodeName = this.CurrentBondActionNode.ActionNodeName;
            }

            this.CurrentBondActionNode = null;

            foreach (ActionNodeMessage actionNodeMessage in this.WorkMessages)
            {
                if (!actionNodeMessage.Finished)
                {
                    if (currentActionNodeName == actionNodeMessage.ActionNodeName && (currentActionNodeName == "UpLookCorrectionActionNode" || currentActionNodeName == "SubstrateCameraCorrectionActionNode"))
                    {
                        AKRSXtraMessageBox.Show("排序出错，请联系软件人员");

                        throw new Exception($"{currentActionNodeName} 重复");
                    }


                    this.CurrentBondActionNode = actionNodeMessage;
                    return this.ChangeNameToActionNode(this.CurrentBondActionNode);
                }
            }

            if (this.CurrentBondActionNode == null)
            {
                return null;
            }

            return null;
        }

        /// <summary>
        /// 将ActionNode的名字转化为动作
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>结果</returns>
        public ActionNode ChangeNameToActionNode(ActionNodeMessage name)
        {
            ActionNode actionNode = this.ActionNodes.Find(it => it.Name == name.ActionNodeName);

            if (this.ActionNodes == null)
            {
                throw new Exception("ActionNode Sort Error");
            }

            this.InitCurrentMessage();
            return actionNode;
        }

        /// <summary>
        /// 重置当前的某些动作
        /// </summary>
        /// <param name="actionNodes">重置的名称</param>
        public void SetActionNodeReWork(List<ActionNode> actionNodes)
        {
            // TODO 如果存在多次点胶，这个逻辑需要改动
            foreach (ActionNode actionNode in actionNodes)
            {
                ActionNodeMessage actionNodeMessage = this.WorkMessages.Find(
                    it => it.ActionNodeName == actionNode.Name
                          && it.Module == this.CurrentBondActionNode.Module && it.Substrate == this.CurrentBondActionNode.Substrate
                          && it.ProcessStepName == this.CurrentBondActionNode.ProcessStepName);

                if (actionNodeMessage == null)
                {
                    throw new Exception("ActionNode Sort Error");
                }

                actionNodeMessage.Finished = false;
            }
        }

        /// <summary>
        /// 重置当前的某些动作
        /// </summary>
        /// <param name="actionNodes">重置的名称</param>
        public void SetActionNodeReWork(List<(ActionNode,string)> actionNodes)
        {
            // TODO 如果存在多次点胶，这个逻辑需要改动
            foreach (var actionNode in actionNodes)
            {
                ActionNodeMessage actionNodeMessage = this.WorkMessages.Find(
                    it => it.ActionNodeName == actionNode.Item1.Name
                          && it.Module == this.CurrentBondActionNode.Module && it.Substrate == this.CurrentBondActionNode.Substrate
                          && it.ProcessStepName == actionNode.Item2);

                if (actionNodeMessage == null)
                {
                    throw new Exception("ActionNode Sort Error");
                }

                actionNodeMessage.Finished = false;
            }
        }

        /// <summary>
        /// 获取下一颗芯片的名称
        /// </summary>
        /// <returns>名称</returns>
        public string GetNextComponentName()
        {
            int correctIndex = this.WorkMessages.IndexOf(this.CurrentBondActionNode);

            string name = null;

            // 是否找到当前芯片名称
            bool isFindCurComponentName = false;

            for (int i = correctIndex + 1; i < this.WorkMessages.Count; i++)
            {
                if (this.WorkMessages[i].ActionNodeName == "BondActionNode" && !this.WorkMessages[i].Finished)
                {
                    SingleProcessStep processStep = ProcessDomain.GetInstance().S2SystemProcess.ProcessSteps.Find(it => it.ProcessStepName == this.WorkMessages[i].ProcessStepName);

                    BondPosition bondPosition = this.TransportUnit.GetBondPosition(
                        this.WorkMessages[i].Substrate,
                        this.WorkMessages[i].Module,
                        processStep.BondPositionName);

                    if (bondPosition.IsProduct(this.WorkMessages[i].ProcessStepName))
                    {
                        // 2024.10.18   唐鹏修改
                        if (isFindCurComponentName == false)
                        {
                            isFindCurComponentName = true;
                            continue;
                        }

                        name = this.WorkMessages[i].ProcessStepName;
                        break;
                    }
                }
            }

            // 翻转台最后一颗芯片没找到直接返回null
            if (MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated)
            {
                string componentName = ProcessDomain.GetInstance().S2SystemProcess.ProcessSteps.Find(it => (it.ComponentName != "Null") && (it.IsEnable == true)).ComponentName;

                BaseCarrierConfig component =
                    (BaseCarrierConfig)CarrierConfigRepository.GetInstance().Find(componentName);

                if (component.IsUseFlipTable)
                {
                    if (name == null)
                    {
                        return null;
                    }
                }
            }

            // 从头开始找
            if (name == null)
            {
                for (int i = 0; i < this.WorkMessages.Count; i++)
                {
                    if (this.WorkMessages[i].ActionNodeName == "BondActionNode" /*&& !this.System2ActionNodeMessages[i].Finished*/)
                    {
                        SingleProcessStep processStep = ProcessDomain.GetInstance().S2SystemProcess.ProcessSteps.Find(it => it.ProcessStepName == this.WorkMessages[i].ProcessStepName);

                        BondPosition bondPosition = this.TransportUnit.GetBondPosition(
                            this.WorkMessages[i].Substrate,
                            this.WorkMessages[i].Module,
                            processStep.BondPositionName);

                        name = this.WorkMessages[i].ProcessStepName;
                        break;
                    }
                }
            }

            SingleProcessStep process = ProcessDomain.GetInstance().S2SystemProcess.ProcessSteps.Find(it => it.ProcessStepName == name);

            if (process == null)
            {
                //AKRSMessageBoxExt.Show(
                //    $" 获取到最后一颗芯片为空，请确认产品是否做完！\r\n",
                //    "异常",
                //    new string[] { "确认" },
                //    new DialogResult[] { DialogResult.Abort },
                //    AlarmLevel.SecondLevel);

                return null;
            }

            //while (this.CurrentBondActionNode != null)
            //{
            //    // 当动作是上视或者中转台时，重取条件判断
            //    while ((this.CurrentBondActionNode.ActionNodeName == "UpLookCorrectionActionNode"
            //            || this.CurrentBondActionNode.ActionNodeName == "SubstrateCameraCorrectionActionNode")
            //           && process.ComponentName != this.GetCurrentComponent().component.Name)
            //    {
            //        if (this.IsOldComponent)
            //        {
            //            this.IsOldComponent = false;
            //            return this.GetCurrentComponent().component.Name;
            //        }

            //        Thread.Sleep(2);
            //    }
            //}

            //// 当动作是上视或者中转台时，重取条件判断
            //while (this.CurrentBondActionNode != null && (this.CurrentBondActionNode.ActionNodeName == "UpLookCorrectionActionNode"
            //                                              || this.CurrentBondActionNode.ActionNodeName == "SubstrateCameraCorrectionActionNode")
            //                                          && process.ComponentName != this.GetCurrentComponent().component.Name)
            //{
            //    if (this.IsOldComponent)
            //    {
            //        this.IsOldComponent = false;
            //        return this.GetCurrentComponent().component.Name;
            //    }

            //    Thread.Sleep(2);
            //}


            return process.ComponentName;
        }

        /// <summary>
        ///  获取当前芯片
        /// </summary>
        /// <returns>芯片名</returns>
        public string GetCurrentComponentName()
        {
            if (this.ProcessStep == null)
            {
                return string.Empty;
            }

            if (this.ProcessStep.ComponentName == null)
            {
                AKRSXtraMessageBox.Show(@"Current component  name is  null!");
            }

            return this.ProcessStep.ComponentName;
        }

        /// <summary>
        /// 动作节点里面获取当前芯片对象
        /// </summary>
        /// <returns>结果</returns>
        public (bool isSuccess, BaseCarrierConfig component) GetCurrentComponent()
        {
            BaseCarrierConfig baseCarrierConfig = null;

            // 遍历芯片数据集
            foreach (BaseCarrierConfig item in CarrierConfigRepository.GetInstance().BaseDsSettingList)
            {
                if (item.Name == this.GetCurrentComponentName())
                {
                    baseCarrierConfig = item;
                }
            }

            // bool isExist = CarrierConfigRepository.GetInstance().BaseDsSettingList.Exists(item => item.Name == this.Controller.GetCurrentComponentName());

            if (baseCarrierConfig == null)
            {
                return (false, null);
            }

            // 检查是否获取成功
            if (baseCarrierConfig.Name == this.GetCurrentComponentName())
            {
                return (true, baseCarrierConfig);
            }
            return (false, null);

        }

        /// <summary>
        /// 获取当前产品的名称
        /// 主要是为了存储
        /// </summary>
        /// <returns>名称</returns>
        public string CurrentMatter()
        {
            if (this.WorkMessages.Count == 0)
            {
                return null;
            }

            string name = string.Empty;

            //if (this.TransportUnit != null)
            //{
            //    name = name + this.TransportUnit.Name;
            //}

            if (this.CurrentSubstrate != null)
            {
                name = name + $"基板号{this.CurrentSubstrate.Index}";
            }

            if (this.CurrentModule != null)
            {
                name = name + $"基岛号{this.CurrentModule.Index}";
            }

            if (this.BondPosition != null)
            {
                name = name + $"焊点名称{this.BondPosition.Name}";
            }

            return name;
        }

        /// <summary>
        /// 自检
        /// </summary>
        /// <returns>结果</returns>
        public bool SelfCheck()
        {
            if (this.ProcessSteps == null || this.ProcessSteps.Count == 0)
            {
                AKRSXtraMessageBox.Show(
                     $"系统2没有可执行的工作步骤，请检查程式",
                     "Warning",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Warning);

                return false;
            }

            if (!ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
            {
                // 遍历所有process
                foreach (var processStep in this.ProcessSteps)
                {
                    if (string.IsNullOrEmpty(processStep.BondPositionName))
                    {
                        AKRSXtraMessageBox.Show(
                            $"{processStep.BondPositionName} 所选择的焊点为空，请重新编辑程式",
                            "Warn",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return false;
                    }

                    if (!ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList.Exists(it => it.Name == processStep.BondPositionName))
                    {
                        AKRSXtraMessageBox.Show(
                            $"{processStep.BondPositionName} 所选择的焊点可能已经被删除，请重新编辑程式",
                            "Warn",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return false;
                    }
                }
            }

            

            return true;
        }

        /// <summary>
        /// 注入步骤
        /// </summary>
        /// <param name="transportUnit">步骤</param>
        public void InjectSteps(TransportUnit transportUnit)
        {
            foreach (Substrate substrate in transportUnit.Substrates)
            {
                foreach (Module module in substrate.Modules)
                {
                    foreach (BondPosition bondPosition in module.BondPositions)
                    {
                        foreach (SingleProcessStep singleProcessStep in this.ProcessSteps)
                        {
                            if (singleProcessStep.BondPositionName == bondPosition.Name)
                            {
                                if (!bondPosition.BondPositionInfo.S2RemainingSteps.ContainsKey(singleProcessStep.ProcessStepName))
                                {
                                    bondPosition.BondPositionInfo.S2RemainingSteps.Add(
                                        singleProcessStep.ProcessStepName,
                                        false);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 相对贴片
        /// 由于相对贴片的焊点必须要之前的焊点固晶完成之后才能拍照
        /// 此方法将此焊点的拍照动作重新注入的
        /// </summary>
        public void SortRelativeBond()
        {
            // 寻找相对贴片的焊点集合
            List<SingleBondPositionConfig> bondPositionConfigs = ProductConfiguration.GetInstance().BondPositionConfig
                .SingleBpPositionConfigList.FindAll(it => it.RelativeBondPosition);

            if (bondPositionConfigs.Count == 0)
            {
                return;
            }

            List<SingleProcessStep> singleProcessSteps = new List<SingleProcessStep>();


            // 寻找相对贴片的焊点对应的步骤
            foreach (SingleBondPositionConfig singleBondPositionConfig in bondPositionConfigs)
            {
                SingleProcessStep singleProcess =
                    this.ProcessSteps.Find(it => it.BondPositionName == singleBondPositionConfig.Name);

                singleProcessSteps.Add(singleProcess);
            }

            if (singleProcessSteps.Count == 0)
            {
                return;
            }

            // 移除相对贴片的焊点拍照
            foreach (SingleProcessStep singleProcess in singleProcessSteps)
            {
                this.WorkMessages.RemoveAll(
                    it => it.ActionNodeName == "BondPositionVisionActionNode" && it.ProcessStepName == singleProcess.ProcessStepName);
            }

            // 将相对贴片的焊点拍照重新注入
            foreach (SingleProcessStep singleProcess in singleProcessSteps)
            {
                for (int i = 0; i < this.WorkMessages.Count; i++)
                {
                    if (this.WorkMessages[i].ProcessStepName == singleProcess.ProcessStepName
                        && this.WorkMessages[i].ActionNodeName == "PickActionNode")
                    {
                        ActionNodeMessage actionNodeMessage = new ActionNodeMessage(
                            "BondPositionVisionActionNode",
                            this.WorkMessages[i].Substrate,
                            this.WorkMessages[i].Module,
                            this.WorkMessages[i].ProcessStepName);
                        this.WorkMessages.Insert(i, actionNodeMessage);
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// 移除其他步骤
        /// </summary>
        public void RemoveOtherStep()
        {
            if (!ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
            {
                return;
            }

            OppositeSex tuOppositeSex = ProductConfiguration.GetInstance().OppositeSexConfiguration.OppositeSexConfigs;
            for (int i = 0; i < this.WorkMessages.Count; i++)
            {
                if (this.WorkMessages[i].Substrate == 0)
                {
                    continue;
                }

                if (tuOppositeSex.DownConfigs.Count < this.WorkMessages[i].Substrate)
                {
                    this.WorkMessages.RemoveAt(i);
                    i--;
                    continue;
                }

                if (this.WorkMessages[i].Module == 0)
                {
                    continue;
                }

                if (tuOppositeSex.DownConfigs[this.WorkMessages[i].Substrate - 1].DownConfigs.Count < this.WorkMessages[i].Module)
                {
                    this.WorkMessages.RemoveAt(i);
                    i--;
                    continue;
                }

                OppositeSex bondOppositeSex = tuOppositeSex.DownConfigs[this.WorkMessages[i].Substrate - 1]
                    .DownConfigs[this.WorkMessages[i].Module - 1];

                SingleProcessStep singleProcessStep = this.ProcessSteps.Find(it => it.ProcessStepName == this.WorkMessages[i].ProcessStepName);

                if (singleProcessStep == null)
                {
                    continue;
                }

                if (!bondOppositeSex.DownConfigs.Exists(it => it.Name == singleProcessStep.BondPositionName))
                {
                    this.WorkMessages.RemoveAt(i);
                    i--;
                }
            }
        }


        /// <summary>
        /// 移除不生效的动作
        /// </summary>
        private void RemoveDisableActionNode()
        {
            if (ProductConfiguration.GetInstance().TransportUnitConfig.LagerNumber)
            {
                TransportUnit transportUnit = TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

                if (transportUnit == null)
                {
                    return;
                }

                transportUnit.Substrates.Sort((item1, item2) => item1.WorkIndex.CompareTo(item2.WorkIndex));

                foreach (Substrate sub in transportUnit.Substrates)
                {
                    sub.Modules.Sort((item1, item2) => item1.WorkIndex.CompareTo(item2.WorkIndex));
                }

                Substrate substrate;

                Module module;

                BondPosition bondPosition;

                for (int i = 0; i < this.WorkMessages.Count; i++)
                {
                    if (this.WorkMessages[i].Substrate != 0)
                    {
                        substrate = transportUnit.Substrates[this.WorkMessages[i].Substrate - 1];
                    }
                    else
                    {
                        continue;
                    }

                    if (this.WorkMessages[i].Module != 0)
                    {
                        module = substrate.Modules[this.WorkMessages[i].Module - 1];
                    }
                    else
                    {
                        if (substrate.MatterProductState == TransportUnitSystem.Model.MatterProductState.Disable)
                        {
                            this.WorkMessages.RemoveAt(i);
                            i--;
                        }

                        continue;
                    }

                    if (!string.IsNullOrEmpty(this.WorkMessages[i].ProcessStepName))
                    {
                        SingleProcessStep singleProcessStep = this.ProcessSteps.Find(it => it.ProcessStepName == this.WorkMessages[i].ProcessStepName);
                        bondPosition = module.BondPositions.Find(it => it.Name == singleProcessStep.BondPositionName);
                        if (bondPosition.MatterProductState == TransportUnitSystem.Model.MatterProductState.Disable)
                        {
                            this.WorkMessages.RemoveAt(i);
                            i--;
                        }

                        continue;
                    }
                    else
                    {
                        if (module.MatterProductState == TransportUnitSystem.Model.MatterProductState.Disable)
                        {
                            this.WorkMessages.RemoveAt(i);
                            i--;
                        }

                        continue;
                    }
                }
            }
        }
    }
}
