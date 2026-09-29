using System.Collections.Generic;

using AKRS.ZX2200.DispenseSystem.Models.DispenseAction.DispenseBPAction;
using AKRS.ZX2200.DispenseSystem.Models.DispenseAction.DispenseUpAction;
using AKRS.ZX2200.DispenseSystem.Models.DispenseAction.HeightMeasurementActionNode;
using AKRS.ZX2200.DispenseSystem.Models.DispenseAction.OtherAction;
using AKRS.ZX2200.Models;


namespace AKRS.ZX2200.DispenseSystem.Models.DispenseAction
{
    using AKRS.ZX2200.DispenseSystem.Models.DispenseAction.IdentityActionNode;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;

    /// <summary>
    /// 点胶动作的集合
    /// </summary>
    public class DispenseActionNodes
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="processStepNames">名字</param>
        public DispenseActionNodes(List<string> processStepNames)
        {
            this.Init();
        }

        /// <summary>
        /// 预点胶动作
        /// </summary>
        public PreDispenseAction PreDispenseAction { get; set; } = new PreDispenseAction();

        /// <summary>
        /// 纠偏动作动作
        /// </summary>
        public S1DispenseCorrectionActionNode S1DispenseCorrectionActionNode { get; set; } =
            new S1DispenseCorrectionActionNode()
                {
                    ActionLevel = ActionLevelEnum.BondPosition, Name = "S1DispenseCorrectionActionNode"
                };

        /// <summary>
        /// 点胶动作
        /// </summary>
        public S1DispenseActionNode S1DispenseActionNode { get; set; } =
            new S1DispenseActionNode() { ActionLevel = ActionLevelEnum.BondPosition, Name = "S1DispenseActionNode" };

        /// <summary>
        /// 检测动作
        /// </summary>
        public S1DispenseDefectActionNode S1DispenseDefectActionNode { get; set; } =
            new S1DispenseDefectActionNode()
                {
                    ActionLevel = ActionLevelEnum.BondPosition, Name = "S1DispenseDefectActionNode"
                };

        /// <summary>
        /// 测高动作
        /// </summary>
        public S1DispenseMeasureActionNode S1DispenseMeasureActionNode { get; set; } =
            new S1DispenseMeasureActionNode()
                {
                    ActionLevel = ActionLevelEnum.BondPosition, Name = "S1DispenseMeasureActionNode"
                };

        /// <summary>
        /// 基板定位动作
        /// </summary>
        public S1SubstrateVisionActionNode S1SubstrateVisionActionNode { get; set; } =
            new S1SubstrateVisionActionNode()
                {
                    ActionLevel = ActionLevelEnum.Substrate, Name = "S1SubstrateVisionActionNode"
                };

        /// <summary>
        /// 基岛定位动作
        /// </summary>
        public S1ModuleVisionActionNode S1ModuleVisionActionNode { get; set; } =
            new S1ModuleVisionActionNode() { ActionLevel = ActionLevelEnum.Module, Name = "S1ModuleVisionActionNode" };

        /// <summary>
        /// 载具定位动作
        /// </summary>
        public S1CarrierVisionActionNode S1CarrierVisionActionNode { get; set; } =
            new S1CarrierVisionActionNode()
                {
                    ActionLevel = ActionLevelEnum.Carrier, Name = "S1CarrierVisionActionNode"
                };

        /// <summary>
        /// 交互动作
        /// </summary>
        public InteractiveAction InteractiveAction { get; set; } =
            new InteractiveAction() { ActionLevel = ActionLevelEnum.Other, Name = "InteractiveAction" };


        /// <summary>
        /// 载具测高
        /// </summary>
        public S1CarrierMeasureHeightActionNode S1CarrierMeasureHeightActionNode { get; set; } =
            new S1CarrierMeasureHeightActionNode() { ActionLevel = ActionLevelEnum.Carrier, Name = "S1CarrierMeasureHeightActionNode" };

        /// <summary>
        /// 基板测高
        /// </summary>
        public S1SubstrateMeasureHeightActionNode S1SubstrateMeasureHeightActionNode { get; set; } =
            new S1SubstrateMeasureHeightActionNode() { ActionLevel = ActionLevelEnum.Substrate, Name = "S1SubstrateMeasureHeightActionNode" };

        /// <summary>
        /// 基岛测高
        /// </summary>
        public S1ModuleMeasureHeightActionNode S1ModuleMeasureHeightActionNode { get; set; } =
            new S1ModuleMeasureHeightActionNode() { ActionLevel = ActionLevelEnum.Module, Name = "S1ModuleMeasureHeightActionNode" };

        #region 身份识别动作节点

        /// <summary>
        /// 载具身份识别
        /// </summary>
        public S1TransportIdentityActionNode S1TransportIdentityActionNode { get; set; } =
            new S1TransportIdentityActionNode()
            {
                ActionLevel = ActionLevelEnum.Carrier,
                Name = "S1TransportIdentityActionNode",
                IsCloseCylinder = true
            };

        /// <summary>
        /// 基板身份识别
        /// </summary>
        public S1SubstrateIdentityActionNode S1SubstrateIdentityActionNode { get; set; } =
            new S1SubstrateIdentityActionNode()
            {
                ActionLevel = ActionLevelEnum.Substrate,
                Name = "S1SubstrateIdentityActionNode",
                IsCloseCylinder = true
            };

        /// <summary>
        /// 基岛身份识别
        /// </summary>
        public S1ModuleIdentityActionNode S1ModuleIdentityActionNode { get; set; } =
            new S1ModuleIdentityActionNode()
            {
                ActionLevel = ActionLevelEnum.Module,
                Name = "S1ModuleIdentityActionNode",
                IsCloseCylinder = true
            };

        /// <summary>
        /// 焊点身份识别
        /// </summary>
        public S1BondPositionIdentityActionNode S1BondPositionIdentityActionNode { get; set; } =
            new S1BondPositionIdentityActionNode()
            {
                ActionLevel = ActionLevelEnum.BondPosition,
                Name = "S1BondPositionIdentityActionNode",
                IsCloseCylinder = true
            };

        #endregion

        /// <summary>
        /// 框架动作
        /// </summary>
        public List<ActionNode> TransportActonNodes { get; set; } = new List<ActionNode>();

        /// <summary>
        /// 基板动作
        /// </summary>
        public List<ActionNode> SubstrateActonNodes { get; set; } = new List<ActionNode>();

        /// <summary>
        /// 基岛动作
        /// </summary>
        public List<ActionNode> ModuleActonNodes { get; set; } = new List<ActionNode>();

        /// <summary>
        /// 基岛动作
        /// </summary>
        public List<ActionNode> BondPositionActonNodes { get; set; } = new List<ActionNode>();

        /// <summary>
        /// 点胶动作节点集合
        /// </summary>
        public List<ActionNode> ActionNodes { get; set; } = new List<ActionNode>();

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            if (this.ActionNodes == null)
            {
                this.ActionNodes = new List<ActionNode>();
            }

            this.ActionNodes.Clear();

            // 将全部动作注入到集合中去
            this.ActionNodes.Add(this.S1DispenseCorrectionActionNode);
            this.ActionNodes.Add(this.S1DispenseMeasureActionNode);
            this.ActionNodes.Add(this.S1DispenseActionNode);
            this.ActionNodes.Add(this.S1DispenseDefectActionNode);
            this.ActionNodes.Add(this.S1ModuleVisionActionNode);
            this.ActionNodes.Add(this.S1ModuleMeasureHeightActionNode);
            this.ActionNodes.Add(this.S1SubstrateVisionActionNode);
            this.ActionNodes.Add(this.S1SubstrateMeasureHeightActionNode);
            this.ActionNodes.Add(this.S1CarrierVisionActionNode);
            this.ActionNodes.Add(this.S1CarrierMeasureHeightActionNode);
            this.ActionNodes.Add(this.InteractiveAction);
            this.ActionNodes.Add(this.S1TransportIdentityActionNode);
            this.ActionNodes.Add(this.S1SubstrateIdentityActionNode);
            this.ActionNodes.Add(this.S1ModuleIdentityActionNode);
            this.ActionNodes.Add(this.S1BondPositionIdentityActionNode);


            // 每个层次各自的动作
            this.TransportActonNodes.Add(this.S1CarrierVisionActionNode);
            this.TransportActonNodes.Add(this.S1CarrierMeasureHeightActionNode);
            this.TransportActonNodes.Add(this.S1TransportIdentityActionNode);

            this.SubstrateActonNodes.Add(this.S1SubstrateVisionActionNode);
            this.SubstrateActonNodes.Add(this.S1SubstrateMeasureHeightActionNode);
            this.SubstrateActonNodes.Add(this.S1SubstrateIdentityActionNode);

            this.ModuleActonNodes.Add(this.S1ModuleVisionActionNode);
            this.ModuleActonNodes.Add(this.S1ModuleMeasureHeightActionNode);
            this.ModuleActonNodes.Add(this.S1ModuleIdentityActionNode);

            this.BondPositionActonNodes.Add(this.S1DispenseCorrectionActionNode);
            this.BondPositionActonNodes.Add(this.S1DispenseMeasureActionNode);
            this.BondPositionActonNodes.Add(this.S1BondPositionIdentityActionNode);
        }
    }
}
