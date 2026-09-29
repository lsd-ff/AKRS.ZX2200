using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Services;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Main.Machine.Process
{
    using AKRS.ZX2200.DispenseSystem.Models.DispenseAction;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.TransportUnitSystem.Service;

    /// <summary>
    /// 系统1控制器
    /// </summary>
    public class S1ActionNodeController
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
        private DispenseActionNodes DispenseActionNodes => System1Domain.GetInstance().DispenseActionNodes;

        /// <summary>
        /// 点胶当作做的动作信息
        /// 当它为null时，为第一次
        /// </summary>
        private ActionNodeMessage CurrentActionNodeMessage { get; set; }

        /// <summary>
        /// 当前的动作
        /// </summary>
        public ActionNode CurrentActionNode => this.DispenseActionNodes.ActionNodes.Find(it => it.Name == this.CurrentActionNodeMessage?.ActionNodeName);

        /// <summary>
        /// 当前步骤的信息
        /// </summary>
        public string SingleProcessStepName => this.CurrentActionNodeMessage?.ProcessStepName;

        /// <summary>
        /// 当前程式
        /// </summary>
        public SingleProcessStep CurrentProcessStep =>
            this.ProcessSteps?.Find(it => it.ProcessStepName == this.CurrentActionNodeMessage?.ProcessStepName);

        /// <summary>
        /// 当前作业的基岛
        /// </summary>
        public Module CurrentModule =>
            this.CurrentSubstrate?.Modules.Find(it => it.WorkIndex == this.CurrentActionNodeMessage?.Module);

        /// <summary>
        /// 当前作业的substrate
        /// </summary>
        public Substrate CurrentSubstrate =>
            this.CurrentTransportUnit?.Substrates?.Find(it => it.WorkIndex == this.CurrentActionNodeMessage?.Substrate);

        /// <summary>
        /// 当前作业的substrate
        /// </summary>
        public TransportUnit CurrentTransportUnit =>
            TransportProgram.GetInstance().DispenseSubSectionProgram.TransportUnit;

        /// <summary>
        /// 当前作业的焊点
        /// </summary>
        public BondPosition CurrentBondPosition =>
            this.CurrentModule?.BondPositions.Find(it => it.Name == this.CurrentProcessStep?.BondPositionName);

        /// <summary>
        /// 焊后检测
        /// </summary>
        public PostBondInspection CurrentPostBondInspection => (PostBondInspection)PostBondInspectionRepository.GetInstance().Find(this.CurrentProcessStep?.DefectName);

        /// <summary>
        /// 点胶图案
        /// </summary>
        public EpoxyApplication CurrentEpoxyApplication => (EpoxyApplication)EpoxyApplicationRepository.GetInstance().Find(this.CurrentProcessStep?.EpoxyNameApplicationName);

        #endregion

        /// <summary>
        /// 步骤的集合
        /// </summary>
        private List<SingleProcessStep> ProcessSteps =>
            ProcessDomain.GetInstance().S1SystemProcess.ProcessSteps.FindAll(it => it.IsEnable);

        /// <summary>
        /// 框架动作
        /// </summary>
        private List<ActionNode> TransportPreActionNodes => this.DispenseActionNodes.TransportActonNodes;

        /// <summary>
        /// 基板动作
        /// </summary>
        private List<ActionNode> SubstrateActonNodes => this.DispenseActionNodes.SubstrateActonNodes;

        /// <summary>
        /// 基岛动作
        /// </summary>
        private List<ActionNode> ModuleActonNodes => this.DispenseActionNodes.ModuleActonNodes;

        /// <summary>
        /// 焊点动作
        /// </summary>
        private List<ActionNode> BondPositionActonNodes => this.DispenseActionNodes.BondPositionActonNodes;

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

            this.SortActionNodeByModule(
                ProcessDomain.GetInstance().S1SystemProcess.WorkProcess,
                ProcessDomain.GetInstance().S1SystemProcess.WorkProcessModule);

            this.RemoveOtherStep();

            this.CurrentActionNodeMessage = null;
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

            // 如果是分段的情况下
            if (ProductConfiguration.GetInstance().TransportUnitConfig.SubstrateProcessing == SubstrateProcessingEnum.Divide)
            {
                //// TODO 这样直接分段是有问题的，后续更改
                //int divideSubstrate = ProductConfiguration.GetInstance().SubstrateConfig.Count / 2;
                //List<ActionNodeMessage> first = this.WorkMessages.FindAll(it => it.Substrate > divideSubstrate);
                //first.Add(new ActionNodeMessage(DispenseActionNodes.InteractiveAction.Name, 0, 0, ""));
                //List<ActionNodeMessage> last = this.WorkMessages.FindAll(it => it.Substrate <= divideSubstrate);

                //// 找到中间的行列数
                //int divideNumber = ProductConfiguration.GetInstance().SubstrateConfig.ColumnCount / 2;

                //this.WorkMessages.Clear();
                //this.WorkMessages.AddRange(first);
                //this.WorkMessages.AddRange(last);


                List<int> first = TuService.GetFirstMatrixIndex();

                List<ActionNodeMessage> firstActionNodeMessages = new List<ActionNodeMessage>();

                List<ActionNodeMessage> lastActionNodeMessages = new List<ActionNodeMessage>();

                while (this.WorkMessages.Count != 0)
                {
                    if (first.Contains(this.WorkMessages[0].Substrate))
                    {
                        firstActionNodeMessages.Add(this.WorkMessages[0]);
                        this.WorkMessages.RemoveAt(0);
                    }
                    else
                    {
                        lastActionNodeMessages.Add(this.WorkMessages[0]);
                        this.WorkMessages.RemoveAt(0);
                    }
                }

                this.WorkMessages.AddRange(lastActionNodeMessages);

                this.WorkMessages.Add(new ActionNodeMessage(DispenseActionNodes.InteractiveAction.Name, 0, 0, null));

                this.WorkMessages.AddRange(firstActionNodeMessages);
            }
        }

        #region 排列方法

        /// <summary>
        /// 基板优先和步骤优先
        /// </summary>
        private void SortTransportFirsAndStepFirst()
        {
            List<SingleProcessStep> bondPositionProcessSteps = this.GetWorkBondPosition();

            foreach (ActionNode actionNode in this.TransportPreActionNodes)
            {
                this.WorkMessages.Add(new ActionNodeMessage(actionNode.Name, 0, 0, null));
            }

            foreach (ActionNode actionNodeSub in this.SubstrateActonNodes)
            {
                for (int i = 0; i < this.SubstrateCount; i++)
                {
                    this.WorkMessages.Add(new ActionNodeMessage(actionNodeSub.Name, i + 1, 0, null));
                }
            }

            foreach (ActionNode actionNodeMo in this.ModuleActonNodes)
            {
                for (int i = 0; i < this.SubstrateCount; i++)
                {
                    for (int j = 0; j < this.ModuleCount; j++)
                    {
                        this.WorkMessages.Add(new ActionNodeMessage(actionNodeMo.Name, i + 1, j + 1, null));
                    }
                }
            }

            foreach (ActionNode actionNodeMoBond in this.BondPositionActonNodes)
            {
                for (int i = 0; i < this.SubstrateCount; i++)
                {
                    for (int j = 0; j < this.ModuleCount; j++)
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
                }
            }

            foreach (SingleProcessStep singleProcessStep in this.ProcessSteps)
            {
                for (int i = 0; i < this.SubstrateCount; i++)
                {
                    for (int j = 0; j < this.ModuleCount; j++)
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
        private void SortTransportFirsAndPositionFirst()
        {
            List<SingleProcessStep> bondPositionProcessSteps = this.GetWorkBondPosition();

            foreach (ActionNode actionNode in this.TransportPreActionNodes)
            {
                this.WorkMessages.Add(new ActionNodeMessage(actionNode.Name, 0, 0, null));
            }

            foreach (ActionNode actionNodeSub in this.SubstrateActonNodes)
            {
                for (int i = 0; i < this.SubstrateCount; i++)
                {
                    this.WorkMessages.Add(new ActionNodeMessage(actionNodeSub.Name, i + 1, 0, null));
                }
            }

            foreach (ActionNode actionNodeMo in this.ModuleActonNodes)
            {
                for (int i = 0; i < this.SubstrateCount; i++)
                {
                    for (int j = 0; j < this.ModuleCount; j++)
                    {
                        this.WorkMessages.Add(new ActionNodeMessage(actionNodeMo.Name, i + 1, j + 1, null));
                    }
                }
            }

            foreach (ActionNode actionNodeMoBond in this.BondPositionActonNodes)
            {
                for (int i = 0; i < this.SubstrateCount; i++)
                {
                    for (int j = 0; j < this.ModuleCount; j++)
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
                }
            }

            for (int i = 0; i < this.SubstrateCount; i++)
            {
                for (int j = 0; j < this.ModuleCount; j++)
                {
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
        private void SortSubstrateFirsAndStepFirst()
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

                foreach (ActionNode actionNodeMo in this.ModuleActonNodes)
                {
                    for (int j = 0; j < this.ModuleCount; j++)
                    {
                        this.WorkMessages.Add(new ActionNodeMessage(actionNodeMo.Name, i + 1, j + 1, null));
                    }
                }

                foreach (ActionNode actionNodeMoBond in this.BondPositionActonNodes)
                {
                    foreach (SingleProcessStep singleProcessStep in bondPositionProcessSteps)
                    {
                        for (int j = 0; j < this.ModuleCount; j++)
                        {
                            this.WorkMessages.Add(
                                new ActionNodeMessage(
                                    actionNodeMoBond.Name,
                                    i + 1,
                                    j + 1,
                                    singleProcessStep.ProcessStepName));
                        }
                    }
                }

                foreach (SingleProcessStep singleProcessStep in this.ProcessSteps)
                {
                    for (int j = 0; j < this.ModuleCount; j++)
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
        private void SortSubstrateFirsAndPositionFirst()
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

                foreach (ActionNode actionNodeMo in this.ModuleActonNodes)
                {
                    for (int j = 0; j < this.ModuleCount; j++)
                    {
                        this.WorkMessages.Add(new ActionNodeMessage(actionNodeMo.Name, i + 1, j + 1, null));
                    }
                }

                foreach (ActionNode actionNodeMoBond in this.BondPositionActonNodes)
                {
                    for (int j = 0; j < this.ModuleCount; j++)
                    {
                        foreach (SingleProcessStep singleProcessStep in bondPositionProcessSteps)
                        {
                            this.WorkMessages.Add(new ActionNodeMessage(actionNodeMoBond.Name, i + 1, j + 1, singleProcessStep.ProcessStepName));
                        }
                    }
                }

                for (int j = 0; j < this.ModuleCount; j++)
                {
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
        /// 基板优先和步骤优先
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

            DispenseActionNodes dispenseActionNodes = System1Domain.GetInstance().DispenseActionNodes;

            if (singleProcess.EpoxyNameApplicationName != "Null")
            {
                actionNode.Add(
                    new ActionNodeMessage(
                        dispenseActionNodes.S1DispenseActionNode.Name,
                        substrateIndex,
                        moduleIndex,
                        singleProcess.ProcessStepName));
            }
            else if (singleProcess.DefectName != "Null")
            {
                actionNode.Add(
                    new ActionNodeMessage(
                        dispenseActionNodes.S1DispenseDefectActionNode.Name,
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

        /// <summary>DispenseActionNode
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

            this.CurrentActionNodeMessage = null;
        }

        /// <summary>
        /// 完成当前节点
        /// </summary>
        public void FinishCurrentActionNode()
        {
            this.CurrentActionNodeMessage.Finished = true;
        }

        /// <summary>
        /// 获取下一个动作
        /// </summary>
        /// <returns>动作</returns>
        public ActionNode GetNextActionNode()
        {
            this.CurrentActionNodeMessage = null;
            foreach (ActionNodeMessage actionNodeMessage in this.WorkMessages)
            {
                if (!actionNodeMessage.Finished)
                {
                    this.CurrentActionNodeMessage = actionNodeMessage;
                    return this.DispenseActionNodes.ActionNodes.Find(it => it.Name == this.CurrentActionNodeMessage.ActionNodeName);
                }
            }

            return null;
        }

        /// <summary>
        /// 重置当前的某些动作
        /// </summary>
        /// <param name="actionNodes">重置的名称</param>
        public void SetActionNodeReWork(List<ActionNode> actionNodes)
        {
            foreach (ActionNode actionNode in actionNodes)
            {
                int minIndex = this.WorkMessages.Count;

                for (int i = 0; i < this.WorkMessages.Count; i++)
                {
                    ActionNodeMessage actionNodeMessage = this.WorkMessages.Find(
                        it => it.ActionNodeName == actionNode.Name
                              && it.Module == this.CurrentActionNodeMessage.Module && it.Substrate == this.CurrentActionNodeMessage.Substrate
                              && it.ProcessStepName == this.CurrentActionNodeMessage.ProcessStepName);

                    if (actionNodeMessage == null)
                    {
                        throw new Exception("ActionNode Sort Error");
                    }

                    actionNodeMessage.Finished = false;
                }
            }
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

            if (this.CurrentBondPosition != null)
            {
                name = name + $"焊点名称{this.CurrentBondPosition.Name}";
            }

            return name;
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
                                if (!bondPosition.BondPositionInfo.S1RemainingSteps.ContainsKey(singleProcessStep.ProcessStepName))
                                {
                                    bondPosition.BondPositionInfo.S1RemainingSteps.Add(
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
    }
}
