using AKRS.ZX2200.BondSystem.Models.ActionNodes.AdjustActionNode;
using AKRS.ZX2200.BondSystem.Models.ActionNodes.BPActionNode;
using AKRS.ZX2200.BondSystem.Models.ActionNodes.HeightMeasurementActionNode;
using AKRS.ZX2200.Models;
using System;
using System.Collections.Generic;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes
{
    using AKRS.ZX2200.BondSystem.Models.ActionNodes.DispenseAction;
    using AKRS.ZX2200.BondSystem.Models.ActionNodes.IdentityActionNode;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;

    using PostSharp.Aspects.Advices;

    /// <summary>
    /// 固晶动作节点库
    /// </summary>
    [Serializable]
    public class BondActionNodeRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="processStepNames">名字</param>
        public BondActionNodeRepository(List<string> processStepNames)
        {
            this.Init();
        }

        /// <summary>
        /// 固晶动作节点集合
        /// </summary>
        public List<ActionNode> ActionNodes { get; set; } = new List<ActionNode>();

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
        /// 取料动作节点
        /// </summary>
        public PickActionNode PickActionNode { get; set; } = new PickActionNode()
        {
            ActionLevel = ActionLevelEnum.BondPosition,
            Name = "PickActionNode",
        };

        /// <summary>
        /// 上视矫正动作节点
        /// </summary>
        public UpLookCorrectionActionNode UpLookCorrectionActionNode { get; set; } =
            new UpLookCorrectionActionNode()
            {
                ActionLevel = ActionLevelEnum.BondPosition,
                Name = "UpLookCorrectionActionNode",
            };

        /// <summary>
        /// 中转台矫正动作节点
        /// </summary>
        public SubstrateCameraCorrectionActionNode SubstrateCameraCorrectionActionNode { get; set; } =
            new SubstrateCameraCorrectionActionNode()
            {
                ActionLevel = ActionLevelEnum.BondPosition,
                Name = "SubstrateCameraCorrectionActionNode",
            };

        /// <summary>
        /// 固晶矫正动作节点
        /// </summary>
        public BondPositionVisionActionNode BondPositionVisionActionNode { get; set; } =
            new BondPositionVisionActionNode()
            {
                ActionLevel = ActionLevelEnum.BondPosition,
                Name = "BondPositionVisionActionNode"
            };

        /// <summary>
        /// 固晶动作节点
        /// </summary>
        public BondActionNode BondActionNode { get; set; } = new BondActionNode()
        {
            ActionLevel = ActionLevelEnum.BondPosition,
            Name = "BondActionNode",
        };

        /// <summary>
        /// 焊后检测动作节点
        /// </summary>
        public AfterBondCheckActionNode AfterBondCheckActionNode { get; set; } = new AfterBondCheckActionNode() { ActionLevel = ActionLevelEnum.BondPosition, Name = "PostBondInspection" };

        /// <summary>
        /// 载具定位动作节点
        /// </summary>
        public S2CarrierVisionActionNode S2CarrierVisionActionNode { get; set; } = new S2CarrierVisionActionNode() { ActionLevel = ActionLevelEnum.Carrier, Name = "BondCarrierVision" };

        /// <summary>
        /// 基板定位动作节点
        /// </summary>
        public S2SubstrateVisionActionNode S2SubstrateVisionActionNode { get; set; } = new S2SubstrateVisionActionNode() { ActionLevel = ActionLevelEnum.Substrate, Name = "BondSubstrateVision" };

        /// <summary>
        /// 基岛定位动作节点
        /// </summary>
        public S2ModuleVisionActionNode S2ModuleVisionActionNode { get; set; } = new S2ModuleVisionActionNode() { ActionLevel = ActionLevelEnum.Module, Name = "BondIslandVision" };

        /// <summary>
        /// 墨点检测动作节点,应该没有这个功能，后续跟工艺确认！
        /// </summary>
        public CheckInkDotActionNode CheckInkDotActionNode { get; set; } = new CheckInkDotActionNode() { ActionLevel = ActionLevelEnum.Module, Name = "CheckInkDotActionNode" };


        #region 系统2点胶动作

        /// <summary>
        /// 系统2点胶动作
        /// </summary>
        public S2DispenseActionNode S2DispenseActionNode { get; set; } =
            new S2DispenseActionNode()
            {
                ActionLevel = ActionLevelEnum.Module,
                Name = "S2DispenseActionNode",
                IsCloseCylinder = true
            };

        /// <summary>
        /// 系统2预点胶
        /// </summary>
        public S2PreDispenseActionNode S2PreDispenseActionNode { get; set; } = new S2PreDispenseActionNode() { ActionLevel = ActionLevelEnum.Module, Name = "S2PreDispenseActionNode" };

        #endregion

        /// <summary>
        /// 载具测高
        /// </summary>
        public S2CarrierMeasureHeightActionNode S2CarrierMeasureHeightActionNode { get; set; } =
            new S2CarrierMeasureHeightActionNode()
            {
                ActionLevel = ActionLevelEnum.Carrier,
                Name = "S2CarrierMeasureHeightActionNode",
                IsCloseCylinder = true
            };

        /// <summary>
        /// 基板测高
        /// </summary>
        public S2SubstrateMeasureHeightActionNode S2SubstrateMeasureHeightActionNode { get; set; } =
            new S2SubstrateMeasureHeightActionNode()
            {
                ActionLevel = ActionLevelEnum.Substrate,
                Name = "S2SubstrateMeasureHeightActionNode",
                IsCloseCylinder = true
            };

        /// <summary>
        /// 基岛测高
        /// </summary>
        public S2ModuleMeasureHeightActionNode S2ModuleMeasureHeightActionNode { get; set; } =
            new S2ModuleMeasureHeightActionNode()
            {
                ActionLevel = ActionLevelEnum.Module,
                Name = "S2ModuleMeasureHeightActionNode",
                IsCloseCylinder = true
            };

        /// <summary>
        /// 焊点测高
        /// </summary>
        public S2BondPositionMeasureHeightActionNode S2BondPositionMeasureHeightActionNode { get; set; } =
            new S2BondPositionMeasureHeightActionNode()
            {
                ActionLevel = ActionLevelEnum.BondPosition,
                Name = "S2BondPositionMeasureHeightActionNode",
                IsCloseCylinder = true
            };

        #region 身份识别动作节点

        /// <summary>
        /// 载具身份识别
        /// </summary>
        public S2TransportIdentityActionNode S2TransportIdentityActionNode { get; set; } =
            new S2TransportIdentityActionNode()
            {
                ActionLevel = ActionLevelEnum.Carrier,
                Name = "S2TransportIdentityActionNode",
                IsCloseCylinder = true
            };

        /// <summary>
        /// 基板身份识别
        /// </summary>
        public S2SubstrateIdentityActionNode S2SubstrateIdentityActionNode { get; set; } =
            new S2SubstrateIdentityActionNode()
            {
                ActionLevel = ActionLevelEnum.Substrate,
                Name = "S2SubstrateIdentityActionNode",
                IsCloseCylinder = true
            };

        /// <summary>
        /// 基岛身份识别
        /// </summary>
        public S2ModuleIdentityActionNode S2ModuleIdentityActionNode { get; set; } =
            new S2ModuleIdentityActionNode()
            {
                ActionLevel = ActionLevelEnum.Module,
                Name = "S2ModuleIdentityActionNode",
                IsCloseCylinder = true
            };

        /// <summary>
        /// 焊点身份识别
        /// </summary>
        public S2BondPositionIdentityActionNode S2BondPositionIdentityActionNode { get; set; } =
            new S2BondPositionIdentityActionNode()
            {
                ActionLevel = ActionLevelEnum.BondPosition,
                Name = "S2BondPositionIdentityActionNode",
                IsCloseCylinder = true
            };

        #endregion

        /// <summary>
        /// 蘸胶
        /// </summary>
        public DipFluxActionNode DipFluxActionNode { get; set; } =
            new DipFluxActionNode()
                {
                    ActionLevel = ActionLevelEnum.BondPosition,
                    Name = "DipFluxActionNode"
                };

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
            this.ActionNodes.Add(this.S2CarrierVisionActionNode);
            this.ActionNodes.Add(this.S2CarrierMeasureHeightActionNode);
            this.ActionNodes.Add(this.S2TransportIdentityActionNode);
            this.ActionNodes.Add(this.S2SubstrateVisionActionNode);
            this.ActionNodes.Add(this.S2SubstrateMeasureHeightActionNode);
            this.ActionNodes.Add(this.S2SubstrateIdentityActionNode);
            this.ActionNodes.Add(this.S2ModuleVisionActionNode);
            this.ActionNodes.Add(this.S2ModuleMeasureHeightActionNode);
            this.ActionNodes.Add(this.S2ModuleIdentityActionNode);
            this.ActionNodes.Add(this.S2BondPositionMeasureHeightActionNode);
            this.ActionNodes.Add(this.BondPositionVisionActionNode);
            this.ActionNodes.Add(this.S2DispenseActionNode);
            this.ActionNodes.Add(this.PickActionNode);
            this.ActionNodes.Add(this.UpLookCorrectionActionNode);
            this.ActionNodes.Add(this.SubstrateCameraCorrectionActionNode);
            this.ActionNodes.Add(this.BondActionNode);
            this.ActionNodes.Add(this.S2BondPositionIdentityActionNode);
            this.ActionNodes.Add(this.AfterBondCheckActionNode);

            this.ActionNodes.Add(this.DipFluxActionNode);

            #region 动作基础功能

            this.TransportActonNodes.Add(this.S2CarrierVisionActionNode);
            this.TransportActonNodes.Add(this.S2CarrierMeasureHeightActionNode);
            this.TransportActonNodes.Add(this.S2TransportIdentityActionNode);

            this.SubstrateActonNodes.Add(this.S2SubstrateVisionActionNode);
            this.SubstrateActonNodes.Add(this.S2SubstrateMeasureHeightActionNode);
            this.SubstrateActonNodes.Add(this.S2SubstrateIdentityActionNode);

            this.ModuleActonNodes.Add(this.S2ModuleVisionActionNode);
            this.ModuleActonNodes.Add(this.S2ModuleMeasureHeightActionNode);
            this.ModuleActonNodes.Add(this.S2ModuleIdentityActionNode);

            this.BondPositionActonNodes.Add(this.BondPositionVisionActionNode);
            this.BondPositionActonNodes.Add(this.S2BondPositionMeasureHeightActionNode);
            this.BondPositionActonNodes.Add(this.S2BondPositionIdentityActionNode);

            #endregion
        }
    }
}
