namespace AKRS.ZX2200.BondSystem.Services
{
    using System;
    using System.Collections.Generic;

    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Main.Machine.Product.ProcessStep;
    using AKRS.ZX2200.Models;
    using AKRS.ZX2200.Services;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    using Newtonsoft.Json;

    /// <summary>
    /// 系统1控制器
    /// </summary>
    public class System1ActionNodesService
    {
        /// <summary>
        /// 点胶动作集合
        /// </summary>
        private List<ActionNodeMessage> System1ActionNodeMessages { get; set; } = new List<ActionNodeMessage>();

        /// <summary>
        /// UI界面设置的排列方式
        /// </summary>
        private List<string> ProcessStepNames => ProcessStepProgram.GetInstance().GetS1ProcessStepNames();

        /// <summary>
        /// 系统1目前流道上的载具
        /// </summary>
        [JsonIgnore]
        private TransportUnit TransportUnit => TransportDomain.GetInstance().TransportProgram.DispenseSubSectionProgram.TransportUnit;

        /// <summary>
        /// 点胶当作做的动作信息
        /// 当它为null时，为第一次
        /// </summary>
        private ActionNodeMessage CurrentDispenseActionNode { get; set; }

        /// <summary>
        /// 当前程式
        /// </summary>
        private ProcessStep ProcessStep  => ProcessStepProgram.GetInstance().Find(this.CurrentDispenseActionNode.ProcessStepName);

        /// <summary>
        /// 动作节点
        /// </summary>
        private List<ActionNode> ActionNodes => System1Domain.GetInstance().DispenseActionNodes.ActionNodes;

        /// <summary>
        /// 当前作业的基岛
        /// </summary>
        public Module CurrentModule =>
            this.CurrentSubstrate.Modules.Find(it => it.Index == this.CurrentDispenseActionNode.Module);

        /// <summary>
        /// 当前作业的substrate
        /// </summary>
        public Substrate CurrentSubstrate =>
            this.TransportUnit.Substrates.Find(it => it.Index == this.CurrentDispenseActionNode.Substrate);

        /// <summary>
        /// 当前作业的substrate
        /// </summary>
        public TransportUnit CurrentTransportUnit =>
            this.TransportUnit;

        /// <summary>
        /// 当前作业的焊点
        /// </summary>
        public BondPosition CurrentBondPosition =>
            this.CurrentModule.BondPositions.Find(it => it.Name == this.ProcessStep.BondPositionName);

        /// <summary>
        /// 焊后检测
        /// </summary>
        public PostBondInspection CurrentPostBondInspection =>
            (PostBondInspection)PostBondInspectionRepository.GetInstance()
                .Find(this.ProcessStep.PostBondInspectionNameInS1);

        /// <summary>
        /// 点/画胶图像
        /// </summary>
        public EpoxyApplication CurrentEpoxyApplication =>
            (EpoxyApplication)EpoxyApplicationRepository.GetInstance().Find(this.ProcessStep.EpoxyApplicationNameInS1);

        /// <summary>
        /// 初始化动作的排序方式
        /// 由于工艺不确定需要哪些方式，后续有新的需求直接添加Switch就可以了
        /// </summary>
        public void Init()
        {
            ActionNodesSortModeEnum actionNodesSortModeEnum = ProcessStepProgram.GetInstance().ActionNodesSortModeInS1;

            if (actionNodesSortModeEnum == ActionNodesSortModeEnum.Normal)
            {
                this.SortActionNodeByNormal();
            }
            else if (actionNodesSortModeEnum == ActionNodesSortModeEnum.HighUPH)
            {
                this.SortActionNodeByHighUph();
            }
        }

        /// <summary>
        /// 对动作进行排序
        /// </summary>
        /// <param name="processingStrategy">排序方式</param>
        public void SortDispenseActionNode(ProcessingStrategyEnum processingStrategy)
        {
            ProductConfiguration productConfiguration = ProductConfiguration.GetInstance();

            // 首先将动作列表里面的都清空
            this.System1ActionNodeMessages.Clear();

            if (processingStrategy == ProcessingStrategyEnum.StepsFirst)
            {
                foreach (string processStepName in this.ProcessStepNames)
                {
                    if (productConfiguration.TransportUnitConfig.SubstrateProcessing != SubstrateProcessingEnum.Divide)
                    {
                        for (int i = 1; i <= productConfiguration.SubstrateConfig.Count; i++)
                        {
                            List<ActionNodeMessage> actionNodeMessages = ActionNodesSortService.SortBondPositionActionNode(
                                this.ActionNodes,
                                1,
                                productConfiguration.ModuleConfig.Count,
                                new List<string>() { processStepName },
                                i);

                            this.System1ActionNodeMessages.AddRange(actionNodeMessages);
                        }
                    }
                    else
                    {
                        int divideSub = productConfiguration.SubstrateConfig.Count / 2;
                        for (int i = divideSub; i <= productConfiguration.SubstrateConfig.Count; i++)
                        {
                            List<ActionNodeMessage> actionNodeMessages = ActionNodesSortService.SortBondPositionActionNode(
                                this.ActionNodes,
                                1,
                                productConfiguration.ModuleConfig.Count,
                                new List<string>() { processStepName },
                                i);

                            this.System1ActionNodeMessages.AddRange(actionNodeMessages);
                        }

                        for (int i = 1; i <= divideSub; i++)
                        {
                            List<ActionNodeMessage> actionNodeMessages = ActionNodesSortService.SortBondPositionActionNode(
                                this.ActionNodes,
                                1,
                                productConfiguration.ModuleConfig.Count,
                                new List<string>() { processStepName },
                                i);

                            this.System1ActionNodeMessages.AddRange(actionNodeMessages);
                        }
                    }
                }

                this.System1ActionNodeMessages = ActionNodesSortService.SortOtherActionNode(
                    this.ActionNodes,
                    this.System1ActionNodeMessages,
                    productConfiguration);
            }
            else if (processingStrategy == ProcessingStrategyEnum.ModulesBeforeSteps)
            {
                if (productConfiguration.TransportUnitConfig.SubstrateProcessing != SubstrateProcessingEnum.Divide)
                {
                    for (int i = 1; i <= productConfiguration.SubstrateConfig.Count; i++)
                    {
                        // 对所有动作焊点进行排序
                        this.System1ActionNodeMessages.AddRange(
                            ActionNodesSortService.SortBondPositionActionNode(
                                this.ActionNodes,
                                1,
                                productConfiguration.ModuleConfig.Count,
                                this.ProcessStepNames,
                                i).ToArray());
                    }
                }
                else
                {
                    int divideSub = productConfiguration.SubstrateConfig.Count / 2;
                    for (int i = divideSub + 1; i <= productConfiguration.SubstrateConfig.Count; i++)
                    {
                        // 对所有动作焊点进行排序
                        this.System1ActionNodeMessages.AddRange(
                            ActionNodesSortService.SortBondPositionActionNode(
                                this.ActionNodes,
                                1,
                                productConfiguration.ModuleConfig.Count,
                                this.ProcessStepNames,
                                i).ToArray());
                    }

                    for (int i = 1; i <= divideSub; i++)
                    {
                        // 对所有动作焊点进行排序
                        this.System1ActionNodeMessages.AddRange(
                            ActionNodesSortService.SortBondPositionActionNode(
                                this.ActionNodes,
                                1,
                                productConfiguration.ModuleConfig.Count,
                                this.ProcessStepNames,
                                i).ToArray());
                    }
                }
            }

            // 对其他动作进行排序
            this.System1ActionNodeMessages = ActionNodesSortService.SortOtherActionNode(
                this.ActionNodes,
                this.System1ActionNodeMessages,
                productConfiguration);

            if (this.System1ActionNodeMessages != null && this.System1ActionNodeMessages.Count > 0)
            {
                this.CurrentDispenseActionNode = this.System1ActionNodeMessages[0];
            }
        }

        /// <summary>
        /// 高UPH排序
        /// </summary>
        public void SortActionNodeByHighUph()
        {
            // 首先将动作列表里面的都清空
            this.System1ActionNodeMessages.Clear();

            ProductConfiguration productConfiguration = ProductConfiguration.GetInstance();

            List<int> list = new List<int>();

            if (productConfiguration.TransportUnitConfig.SubstrateProcessing != SubstrateProcessingEnum.Divide)
            {
                for (int i = 0; i < productConfiguration.SubstrateConfig.Count; i++)
                {
                    list.Add(i);
                }

                this.SortActionNodeByHighUph(list);
            }
            else
            {
                int divideSub = productConfiguration.SubstrateConfig.Count / 2;

                for (int i = divideSub; i < productConfiguration.SubstrateConfig.Count; i++)
                {
                    list.Add(i);
                }

                this.SortActionNodeByHighUph(list);
                
                // 添加分段动作
                this.System1ActionNodeMessages.Add(new ActionNodeMessage("InteractiveAction", 0, 0, null));

                list.Clear();
                for (int i = 0; i < divideSub; i++)
                   
                {
                    list.Add(i);
                }

                this.SortActionNodeByHighUph(list);
            }
        }

        /// <summary>
        /// 高UPH排序
        /// </summary>
        public void SortActionNodeByNormal()
        {
            // 首先将动作列表里面的都清空
            this.System1ActionNodeMessages.Clear();

            ProductConfiguration productConfiguration = ProductConfiguration.GetInstance();

            List<int> list = new List<int>();

            if (productConfiguration.TransportUnitConfig.SubstrateProcessing != SubstrateProcessingEnum.Divide)
            {
                for (int i = 0; i < productConfiguration.SubstrateConfig.Count; i++)
                {
                    list.Add(i);
                }

                this.SortActionNodeByNormal(list);
            }
            else
            {
                int divideSub = productConfiguration.SubstrateConfig.Count / 2;

                for (int i = divideSub; i < productConfiguration.SubstrateConfig.Count; i++)
                {
                    list.Add(i);
                }

                this.SortActionNodeByNormal(list);

                // 添加分段动作
                this.System1ActionNodeMessages.Add(new ActionNodeMessage("InteractiveAction", 0, 0, null));

                list.Clear();
                for (int i = 0; i < divideSub; i++)

                {
                    list.Add(i);
                }

                this.SortActionNodeByNormal(list);
            }
        }

        /// <summary>
        /// 高精度Sub排序
        /// </summary>
        /// <param name="subList">集合</param>
        public void SortActionNodeByHighUph(List<int> subList)
        {
            // 获取产品对象
            ProductConfiguration productConfiguration = ProductConfiguration.GetInstance();

            List<ActionNode> actionNodes = new List<ActionNode>();

            actionNodes.AddRange(this.ActionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.Carrier).ToArray());
            actionNodes.AddRange(this.ActionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.Substrate).ToArray());
            actionNodes.AddRange(this.ActionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.Module).ToArray());
            actionNodes.AddRange(this.ActionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.BondPosition).ToArray());

            // 遍历所有的动作
            foreach (ActionNode actionNode in actionNodes)
            {
                // 如果这个动作是Tu级别的直接加入
                if (actionNode.ActionLevel == ActionLevelEnum.Carrier)
                {
                    this.System1ActionNodeMessages.Add(new ActionNodeMessage(actionNode.Name, 0, 0, null));
                }
                else
                {
                    // 遍历所有的动作
                    foreach (int substrate in subList)
                    {
                        // 如果是sub基板的动作，直接注入
                        if (actionNode.ActionLevel == ActionLevelEnum.Substrate)
                        {
                            this.System1ActionNodeMessages.Add(new ActionNodeMessage(actionNode.Name, substrate + 1, 0, null));
                        }
                        else
                        {
                            for (int i = 0; i < productConfiguration.ModuleConfig.Count; i++)
                            {
                                // 如果是module的动作直接注入
                                if (actionNode.ActionLevel == ActionLevelEnum.Module)
                                {
                                    this.System1ActionNodeMessages.Add(
                                        new ActionNodeMessage(actionNode.Name, substrate + 1, i + 1, null));
                                }
                                else
                                {
                                    // 根据排序的动作注入
                                    foreach (string processStepName in this.ProcessStepNames)
                                    {
                                        this.System1ActionNodeMessages.Add(
                                            new ActionNodeMessage(actionNode.Name, substrate + 1, i + 1, processStepName));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 普通排序
        /// </summary>
        /// <param name="subList">集合</param>
        public void SortActionNodeByNormal(List<int> subList)
        {
            // 获取产品对象
            ProductConfiguration productConfiguration = ProductConfiguration.GetInstance();

            List<ActionNode> actionNodes = new List<ActionNode>();
            List<ActionNode> bondActionNodes = new List<ActionNode>();

            actionNodes.AddRange(this.ActionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.Carrier).ToArray());
            actionNodes.AddRange(this.ActionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.Substrate).ToArray());
            actionNodes.AddRange(this.ActionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.Module).ToArray());
            bondActionNodes.AddRange(this.ActionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.BondPosition).ToArray());

            // 遍历所有的动作
            foreach (ActionNode actionNode in actionNodes)
            {
                // 如果这个动作是Tu级别的直接加入
                if (actionNode.ActionLevel == ActionLevelEnum.Carrier)
                {
                    this.System1ActionNodeMessages.Add(new ActionNodeMessage(actionNode.Name, 0, 0, null));
                }
                else
                {
                    // 遍历所有的动作
                    foreach (int substrate in subList)
                    {
                        // 如果是sub基板的动作，直接注入
                        if (actionNode.ActionLevel == ActionLevelEnum.Substrate)
                        {
                            this.System1ActionNodeMessages.Add(new ActionNodeMessage(actionNode.Name, substrate + 1, 0, null));
                        }
                        else
                        {
                            for (int i = 0; i < productConfiguration.ModuleConfig.Count; i++)
                            {
                                // 如果是module的动作直接注入
                                if (actionNode.ActionLevel == ActionLevelEnum.Module)
                                {
                                    this.System1ActionNodeMessages.Add(
                                        new ActionNodeMessage(actionNode.Name, substrate + 1, i + 1, null));
                                }
                            }
                        }
                    }
                }
            }

            foreach (int substrate in subList)
            {
                for (int i = 0; i < productConfiguration.ModuleConfig.Count; i++)
                {
                    // 根据排序的动作注入
                    foreach (string processStepName in this.ProcessStepNames)
                    {
                        foreach (ActionNode actionNode in bondActionNodes)
                        {
                            this.System1ActionNodeMessages.Add(
                                new ActionNodeMessage(actionNode.Name, substrate + 1, i + 1, processStepName));
                        }
                    }
                }
            }
        }

        /// <summary>DispenseActionNode
        /// 完成
        /// </summary>
        public void Finish()
        {
            if (this.System1ActionNodeMessages == null)
            {
                return;
            }

            foreach (ActionNodeMessage actionNodeMessage in this.System1ActionNodeMessages)
            {
                actionNodeMessage.Finished = false;
            }

            this.CurrentDispenseActionNode = null;
        }

        /// <summary>
        /// 完成当前节点
        /// </summary>
        public void FinishCurrentActionNode()
        {
            this.CurrentDispenseActionNode.Finished = true;
        }

        /// <summary>
        /// 获取下一个动作
        /// </summary>
        /// <returns>动作</returns>
        public ActionNode GetNextActionNode()
        {
            this.CurrentDispenseActionNode = null;

            foreach (ActionNodeMessage actionNodeMessage in this.System1ActionNodeMessages)
            {
                if (!actionNodeMessage.Finished)
                {
                    this.CurrentDispenseActionNode = actionNodeMessage;
                    return this.ActionNodes.Find(it => it.Name == this.CurrentDispenseActionNode.ActionNodeName);
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
                int minIndex = this.System1ActionNodeMessages.Count;

                for (int i = 0; i < this.System1ActionNodeMessages.Count; i++)
                {
                    ActionNodeMessage actionNodeMessage = this.System1ActionNodeMessages.Find(
                        it => it.ActionNodeName == actionNode.Name
                              && it.Module == this.CurrentDispenseActionNode.Module && it.Substrate == this.CurrentDispenseActionNode.Substrate
                              && it.ProcessStepName == this.CurrentDispenseActionNode.ProcessStepName);

                    if (actionNodeMessage == null)
                    {
                        throw new Exception("ActionNode Sort Error");
                    }

                    actionNodeMessage.Finished = false;
                }
            }
        }
    }
}
