namespace AKRS.ZX2200.DispenseSystem.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows.Forms;

    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.ActionNodes;
    using AKRS.ZX2200.BondSystem.Models.ActionNodes.BPActionNode;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Main.Machine.Product.ProcessStep;
    using AKRS.ZX2200.Models;
    using AKRS.ZX2200.Services;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;

    using DevExpress.XtraEditors;

    using Newtonsoft.Json;

    using TransportUnit = AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit;

    /// <summary>
    /// 系统2动作节点服务类
    /// </summary>
    public class System2ActionNodesService
    {
        /// <summary>
        /// 固晶动作集合
        /// </summary>
        public List<ActionNodeMessage> System2ActionNodeMessages { get; set; } = new List<ActionNodeMessage>();

        /// <summary>
        /// 系统2目前流道上的载具
        /// </summary>
        [JsonIgnore]
        private TransportUnit TransportUnit =>
            TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

        /// <summary>
        /// UI界面设置的排列方式
        /// </summary>
        private List<string> ProcessStepNames => ProcessStepProgram.GetInstance().GetS2ProcessStepNames();

        /// <summary>
        /// 动作节点
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
        private ProcessStep ProcessStep { get; set; }

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

        /// <summary>
        /// 初始化动作的排序方式
        /// 由于工艺不确定需要哪些方式，后续有新的需求直接添加Switch就可以了
        /// </summary>
        /// <param name="processingStrategy">排序类型</param>
        public void Init(ProcessingStrategyEnum processingStrategy)
        {
            BondActionNodeRepository bondActionNodeRepository = System2Domain.GetInstance().BondActionNodeRepository;

            if (bondActionNodeRepository.ActionNodes.Count == 0)
            {
                throw new Exception("ActionNode Sort Error");
            }
            
            if (ProcessStepProgram.GetInstance().ActionNodesSortModeInS2 == ActionNodesSortModeEnum.HighUPH)
            {
                this.SortActionNodeByHighUph();
                return;
            }
            else if (ProcessStepProgram.GetInstance().ActionNodesSortModeInS2 == ActionNodesSortModeEnum.Normal)
            {
                this.SortActionNodeByNormal();
                return;
            }

            //foreach (ActionNode actionNode in bondActionNodeRepository.ActionNodes)
            //{
            //    actionNode.ActionSortType = ActionTypeEnum.BondPosition;
            //}

            //this.SortBondActionNode(bondActionNodeRepository.ActionNodes, processingStrategy);

            //this.IsOldComponent = false;
        }

        /// <summary>
        /// 对动作进行排序
        /// </summary>
        /// <param name="actionNodes">动作类型</param>
        /// <param name="processingStrategy">排序方式</param>
        /// <param name="sortByColumn">是否按列执行</param>
        public void SortBondActionNode(List<ActionNode> actionNodes, ProcessingStrategyEnum processingStrategy, bool sortByColumn = false)
        {
            if (ProductConfiguration.GetInstance() == null)
            {
                throw new Exception("ActionNode Sort Error");
            }

            TransportUnit transportUnit = new TransportUnit();

            if (transportUnit.GetSubstrateCount() == 0 || transportUnit.GetModuleCount() == 0)
            {
                throw new Exception("ActionNode Sort Error");
            }

            // 首先将动作列表里面的都清空
            this.System2ActionNodeMessages.Clear();

            // 这个是到一个特定的地方去找的
            List<string> processStepNames = ProcessStepProgram.GetInstance().GetS2ProcessStepNames();

            if (processStepNames.Count == 0)
            {
                throw new Exception("ActionNode Sort Error");
            }

            if (processingStrategy == ProcessingStrategyEnum.StepsFirst)
            {
                foreach (string processStepName in processStepNames)
                {
                    for (int i = 1; i <= transportUnit.GetSubstrateCount(); i++)
                    {
                        List<ActionNodeMessage> actionNodeMessages = ActionNodesSortService.SortBondPositionActionNode(
                            actionNodes,
                            1,
                            transportUnit.Substrates[0].Modules.Count,
                            new List<string>() { processStepName },
                            i);

                        this.System2ActionNodeMessages.AddRange(actionNodeMessages);
                    }
                }

                this.System2ActionNodeMessages = ActionNodesSortService.SortS2OtherActionNode(
                    actionNodes,
                    this.System2ActionNodeMessages,
                    transportUnit);
                this.CurrentBondActionNode = this.System2ActionNodeMessages[0];
            }
            else if (processingStrategy == ProcessingStrategyEnum.ModulesBeforeSteps)
            {
                if (!sortByColumn)
                {
                    for (int i = 1; i <= transportUnit.GetSubstrateCount(); i++)
                    {
                        // 对所有动作焊点进行排序
                        this.System2ActionNodeMessages.AddRange(
                            ActionNodesSortService.SortBondPositionActionNode(
                                actionNodes,
                                1,
                                transportUnit.Substrates[0].Modules.Count,
                                processStepNames,
                                i).ToArray());
                    }

                    // 对其他动作进行排序
                    this.System2ActionNodeMessages = ActionNodesSortService.SortS2OtherActionNode(actionNodes, this.System2ActionNodeMessages, transportUnit);
                }
            }
        }

        /// <summary>
        /// 高速模式
        /// </summary>
        public void SortActionNodeByHighUph()
        {
            // 清空排序
            this.System2ActionNodeMessages.Clear();

            // 获取产品对象
            ProductConfiguration productConfiguration = ProductConfiguration.GetInstance();

            List<ActionNode> actionNodes = new List<ActionNode>();
            List<ActionNode> bondActionContinue = new List<ActionNode>();

            actionNodes.AddRange(this.ActionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.Carrier).ToArray());
            actionNodes.AddRange(this.ActionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.Substrate).ToArray());
            actionNodes.AddRange(this.ActionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.Module).ToArray());
            bondActionContinue.Add(this.ActionNodes.Find(it => it.Name == "PickActionNode"));
            bondActionContinue.Add(this.ActionNodes.Find(it => it.Name == "UpLookCorrectionActionNode"));
            bondActionContinue.Add(this.ActionNodes.Find(it => it.Name == "SubstrateCameraCorrectionActionNode"));
            bondActionContinue.Add(this.ActionNodes.Find(it => it.Name == "BondActionNode"));
            
            // 遍历所有的动作
            foreach (ActionNode actionNode in actionNodes)
            {
                // 如果这个动作是Tu级别的直接加入
                if (actionNode.ActionLevel == ActionLevelEnum.Carrier)
                {
                    this.System2ActionNodeMessages.Add(new ActionNodeMessage(actionNode.Name, 0, 0, null));
                }
                else
                {
                    // 遍历所有的动作
                    for (int k = 0; k < productConfiguration.SubstrateConfig.Count; k++)
                    {
                        // 如果是sub基板的动作，直接注入
                        if (actionNode.ActionLevel == ActionLevelEnum.Substrate)
                        {
                            this.System2ActionNodeMessages.Add(new ActionNodeMessage(actionNode.Name, k + 1, 0, null));
                        }
                        else
                        {
                            for (int i = 0; i < productConfiguration.ModuleConfig.Count; i++)
                            {
                                // 如果是module的动作直接注入
                                if (actionNode.ActionLevel == ActionLevelEnum.Module)
                                {
                                    this.System2ActionNodeMessages.Add(
                                        new ActionNodeMessage(actionNode.Name, k + 1, i + 1, null));
                                }
                            }
                        }
                    }
                }
            }

            foreach (string processStepName in this.ProcessStepNames)
            {
                // 焊点定位
                for (int i = 0; i < productConfiguration.SubstrateConfig.Count; i++)
                {
                    for (int j = 0; j < productConfiguration.ModuleConfig.Count; j++)
                    {
                        this.System2ActionNodeMessages.Add(
                            new ActionNodeMessage("BondPositionVisionActionNode", i + 1, j + 1, processStepName));
                    }
                }

                // 连续的动作，例如取，放贴
                for (int i = 0; i < productConfiguration.SubstrateConfig.Count; i++)
                {
                    for (int j = 0; j < productConfiguration.ModuleConfig.Count; j++)
                    {
                        foreach (ActionNode action in bondActionContinue)
                        {
                            this.System2ActionNodeMessages.Add(
                                new ActionNodeMessage(action.Name, i + 1, j + 1, processStepName));
                        }
                    }
                }
            }

            //// 焊后检测
            //foreach (string processStepName in this.ProcessStepNames)
            //{
            //    for (int i = 0; i < productConfiguration.SubstrateConfig.Count; i++)
            //    {
            //        for (int j = 0; j < productConfiguration.ModuleConfig.Count; j++)
            //        {
            //            this.System2ActionNodeMessages.Add(
            //                new ActionNodeMessage("PostBondInspection", i + 1, j + 1, processStepName));
            //        }
            //    }
            //}


            for (int i = 0; i < productConfiguration.SubstrateConfig.Count; i++)
            {
                for (int j = 0; j < productConfiguration.ModuleConfig.Count; j++)
                {
                    foreach (string processStepName in this.ProcessStepNames)
                    {
                        this.System2ActionNodeMessages.Add(
                            new ActionNodeMessage("PostBondInspection", i + 1, j + 1, processStepName));
                    }
                }
            }
        }

        /// <summary>
        /// 普通模式
        /// </summary>
        public void SortActionNodeByNormal()
        {
            // 清空排序
            this.System2ActionNodeMessages.Clear();

            // 获取产品对象
            ProductConfiguration productConfiguration = ProductConfiguration.GetInstance();

            List<ActionNode> actionNodes = new List<ActionNode>();
            List<ActionNode> bondActionContinue = new List<ActionNode>();

            actionNodes.AddRange(this.ActionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.Carrier).ToArray());
            actionNodes.AddRange(this.ActionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.Substrate).ToArray());
            actionNodes.AddRange(this.ActionNodes.FindAll(it => it.ActionLevel == ActionLevelEnum.Module).ToArray());
            bondActionContinue.Add(this.ActionNodes.Find(it => it.Name == "PickActionNode"));
            bondActionContinue.Add(this.ActionNodes.Find(it => it.Name == "UpLookCorrectionActionNode"));
            bondActionContinue.Add(this.ActionNodes.Find(it => it.Name == "SubstrateCameraCorrectionActionNode"));
            bondActionContinue.Add(this.ActionNodes.Find(it => it.Name == "BondActionNode"));
            bondActionContinue.Add(this.ActionNodes.Find(it => it.Name == "PostBondInspection"));

            // 遍历所有的动作
            foreach (ActionNode actionNode in actionNodes)
            {
                // 如果这个动作是Tu级别的直接加入
                if (actionNode.ActionLevel == ActionLevelEnum.Carrier)
                {
                    this.System2ActionNodeMessages.Add(new ActionNodeMessage(actionNode.Name, 0, 0, null));
                }
                else
                {
                    // 遍历所有的动作
                    for (int k = 0; k < productConfiguration.SubstrateConfig.Count; k++)
                    {
                        // 如果是sub基板的动作，直接注入
                        if (actionNode.ActionLevel == ActionLevelEnum.Substrate)
                        {
                            this.System2ActionNodeMessages.Add(new ActionNodeMessage(actionNode.Name, k + 1, 0, null));
                        }
                        else
                        {
                            for (int i = 0; i < productConfiguration.ModuleConfig.Count; i++)
                            {
                                // 如果是module的动作直接注入
                                if (actionNode.ActionLevel == ActionLevelEnum.Module)
                                {
                                    this.System2ActionNodeMessages.Add(
                                        new ActionNodeMessage(actionNode.Name, k + 1, i + 1, null));
                                }
                            }
                        }
                    }
                }
            }

            foreach (string processStepName in this.ProcessStepNames)
            {
                // 焊点定位
                for (int i = 0; i < productConfiguration.SubstrateConfig.Count; i++)
                {
                    for (int j = 0; j < productConfiguration.ModuleConfig.Count; j++)
                    {
                        this.System2ActionNodeMessages.Add(
                            new ActionNodeMessage("BondPositionVisionActionNode", i + 1, j + 1, processStepName));
                    }
                }

                // 连续的动作，例如取，放贴
                for (int i = 0; i < productConfiguration.SubstrateConfig.Count; i++)
                {
                    for (int j = 0; j < productConfiguration.ModuleConfig.Count; j++)
                    {
                        foreach (ActionNode action in bondActionContinue)
                        {
                            this.System2ActionNodeMessages.Add(
                                new ActionNodeMessage(action.Name, i + 1, j + 1, processStepName));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取第一颗要做的芯片名称
        /// </summary>
        /// <returns>芯片</returns>
        public BaseCarrierConfig GetFirstComponent()
        {
            if (ProcessStepProgram.GetInstance().GetS2ProcessStep().Count != 0)
            {
                string componentName = ProcessStepProgram.GetInstance().GetS2ProcessStep()[0].ComponentName;

                return (BaseCarrierConfig)CarrierConfigRepository.GetInstance().Find(componentName);
            }

            return null;
            
        }

        /// <summary>
        /// 完成
        /// </summary>
        public void Finish()
        {
            if (this.System2ActionNodeMessages == null)
            {
                return;
            }

            foreach (ActionNodeMessage actionNodeMessage in this.System2ActionNodeMessages)
            {
                actionNodeMessage.Finished = false;
            }

            // 将当前动作改成0
            this.CurrentBondActionNode = this.System2ActionNodeMessages[0];

            this.IsOldComponent = false;
        }

        /// <summary>
        /// 初始化当前信息
        /// </summary>
        public void InitCurrentMessage()
        {
            this.ProcessStep = ProcessStepProgram.GetInstance().Find(this.CurrentBondActionNode.ProcessStepName);

            if (this.ProcessStep != null)
            {
                this.PostBondInspection = (PostBondInspection)PostBondInspectionRepository.GetInstance()
                    .Find(this.ProcessStep.PostBondInspectionNameInS2);
            }
            int substrateIndex = this.CurrentBondActionNode.Substrate;

            try
            {
                foreach (Substrate substrate in this.TransportUnit.Substrates)
                {
                    if (substrate.Index == substrateIndex)
                    {
                        this.CurrentSubstrate = substrate;
                        foreach (Module island in substrate.Modules)
                        {
                            if (island.Index == this.CurrentBondActionNode.Module)
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
               XtraMessageBox.Show(ex.Message);
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
        /// <returns>当前画胶图形</returns>
        public EpoxyApplication GetCurrentEpoxyApplication()
        {
            return this.EpoxyApplication;
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

            foreach (ActionNodeMessage actionNodeMessage in this.System2ActionNodeMessages)
            {
                if (!actionNodeMessage.Finished)
                {
                    if (currentActionNodeName == actionNodeMessage.ActionNodeName && (currentActionNodeName == "UpLookCorrectionActionNode" || currentActionNodeName == "SubstrateCameraCorrectionActionNode") )
                    {

                        MessageBox.Show("排序出错，请联系软件人员");

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
            foreach (ActionNode actionNode in actionNodes)
            {
                int minIndex = this.System2ActionNodeMessages.Count;

                for (int i = 0; i < this.System2ActionNodeMessages.Count; i++)
                {
                    ActionNodeMessage actionNodeMessage = this.System2ActionNodeMessages.Find(
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
        }

        /// <summary>
        /// 是否需要切换芯片，弃用
        /// </summary>
        public bool IsOldComponent = false;

        /// <summary>
        /// 获取下一颗芯片的名称
        /// </summary>
        /// <returns>名称</returns>
        public string GetNextComponentName()
        {
            int correctIndex = this.System2ActionNodeMessages.IndexOf(this.CurrentBondActionNode);

            string name = null;

            // 是否找到当前芯片名称
            bool isFindCurComponentName = false;

            for (int i = correctIndex + 1; i < this.System2ActionNodeMessages.Count; i++)
            {
                if (this.System2ActionNodeMessages[i].ActionNodeName == "BondActionNode" && !this.System2ActionNodeMessages[i].Finished)
                {
                    ProcessStep processStep = ProcessStepProgram.GetInstance().Find(this.System2ActionNodeMessages[i].ProcessStepName);

                    BondPosition bondPosition = this.TransportUnit.GetBondPosition(
                        this.System2ActionNodeMessages[i].Substrate,
                        this.System2ActionNodeMessages[i].Module,
                        processStep.BondPositionName);

                    if (bondPosition.IsProduct())
                    {
                        // 2024.10.18   唐鹏修改
                        if(isFindCurComponentName==false)
                        {
                            isFindCurComponentName = true;
                            continue;
                        }
                    
                        name = this.System2ActionNodeMessages[i].ProcessStepName;
                        break;
                    }
                }
            }

            // 从头开始找
            if (name == null)
            {
                for (int i = 0; i < this.System2ActionNodeMessages.Count; i++)
                {
                    if (this.System2ActionNodeMessages[i].ActionNodeName == "BondActionNode" /*&& !this.System2ActionNodeMessages[i].Finished*/)
                    {
                        ProcessStep processStep = ProcessStepProgram.GetInstance().Find(this.System2ActionNodeMessages[i].ProcessStepName);

                        BondPosition bondPosition = this.TransportUnit.GetBondPosition(
                            this.System2ActionNodeMessages[i].Substrate,
                            this.System2ActionNodeMessages[i].Module,
                            processStep.BondPositionName);

                        name = this.System2ActionNodeMessages[i].ProcessStepName;
                        break;
                    }
                }
            }

            ProcessStep process = ProcessStepProgram.GetInstance().Find(name);

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
            if (this.ProcessStep.ComponentName == null)
            {
                MessageBox.Show(@"Current component  name is  null!");
            }

            return this.ProcessStep.ComponentName;
        }

        /// <summary>
        /// 动作节点里面获取当前芯片对象
        /// </summary>
        /// <returns>结果</returns>
        public (bool isSuccess, BaseCarrierConfig component) GetCurrentComponent()
        {
            BaseCarrierConfig baseCarrierConfig = new BaseCarrierConfig();

            // 遍历芯片数据集
            foreach (BaseCarrierConfig item in CarrierConfigRepository.GetInstance().BaseDsSettingList)
            {
                if (item.Name == this.GetCurrentComponentName())
                {
                    baseCarrierConfig = item;
                }
            }

            // bool isExist = CarrierConfigRepository.GetInstance().BaseDsSettingList.Exists(item => item.Name == this.Controller.GetCurrentComponentName());

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
            if (this.System2ActionNodeMessages.Count == 0)
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
            if (ProcessStepProgram.GetInstance().GetS2ProcessStep().Count == 0)
            {
               XtraMessageBox.Show(
                    $"Process  step  in  system  2  did  not  teach  yet!\r\nPlease  teach  process  step  before  starting  machine!",
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            // 遍历所有process
            foreach (var processStep in ProcessStepProgram.GetInstance().GetS2ProcessStep())
            {
                if (string.IsNullOrEmpty(processStep.BondPositionName))
                {
                    XtraMessageBox.Show(
                        $"Process  step:{processStep.Name}  did  not  match  bonding  position!\r\nPlease  teach  process  step  before  starting  machine!",
                        "Warn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }

                bool isExist = false;

                // 判断焊点是否存在于当前程式
                foreach (var singleBpPositionConfig in ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList)
                {
                    if (processStep.BondPositionName == singleBpPositionConfig.Name)
                    {
                        isExist = true;
                        break;
                    }
                }

                if (!isExist)
                {
                    XtraMessageBox.Show(
                        $"BondPosition：{processStep.BondPositionName} in  Process  step:{processStep.Name}  did  not  match  current  recipe!\r\nPlease  teach  process  step  before  starting  machine!",
                        "Warn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }

                // 芯片名称是否为空
                if (string.IsNullOrEmpty(processStep.ComponentName))
                {
                   XtraMessageBox.Show(
                        $"Process  step:{processStep.Name}  did  not  match  component!\r\nPlease  teach  process  step  before  starting  machine!",
                        "Warn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }

                BaseCarrierConfig component =
                    (BaseCarrierConfig)CarrierConfigRepository.GetInstance().Find(processStep.ComponentName);

                // 芯片是否为空
                if (component == null) 
                {
                   XtraMessageBox.Show(
                        $"Process  step:{processStep.Name}  did  not  find!\r\nPlease  teach  component  before  starting  machine!",
                        "Warn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }

                // 检查芯片拍照位
                if (component.CameraType == CameraTypeEnum.UpLookCamera
                    && component.UpLookAdjustConfig.P1VisionPos.IsEmpty)
                {
                    XtraMessageBox.Show(
                        $"Component :{component.Name}  uplook  vision  position  is  empty!\r\nPlease  teach  component  before  starting  machine!",
                        "Warn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }

                // 检查芯片拍照位
                if (component.CameraType == CameraTypeEnum.BondCamera
                    && component.DownLookAdjustConfig.P1VisionPos.IsEmpty)
                {
                   XtraMessageBox.Show(
                        $"Component :{component.Name}  down-look  vision  position  is  empty!\r\nPlease  teach  component  before  starting  machine!",
                        "Warn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }
            }

            return true;
        }
    }
}
