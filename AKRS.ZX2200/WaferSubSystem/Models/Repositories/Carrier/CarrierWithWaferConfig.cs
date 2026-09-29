using System;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;

namespace AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer
{
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Models;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;

    /// <summary>
    /// 载具类-晶圆
    /// </summary>
    [Serializable]
    public class CarrierWithWaferConfig : BaseCarrierConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CarrierWithWaferConfig()
        {
            this.CarrierType = CarrierTypeEnum.Wafer;
            this.SearchMode = SearchMode.WaferMap;

            this.ComponentGeometry = new AssistantState() { Name = "Component geometry", State = AssistantStateEnum.UnAble };
            if (this.IsMapping)
            {
                this.ComponentWaferMap = new AssistantState() { Name = "Component wafer map", State = AssistantStateEnum.ForBidden };
            }
            else
            {
                this.ComponentWaferMap = new AssistantState() { Name = "Component wafer map", State = AssistantStateEnum.ForBidden };
            }

            if (this.IsInkDotSearch)
            {
                this.InkDot = new AssistantState() { Name = "Ink dot", State = AssistantStateEnum.UnAble };
            }
            else
            {
                this.InkDot = new AssistantState() { Name = "Ink dot", State = AssistantStateEnum.ForBidden };
            }

            this.WaferEdge = new AssistantState() { Name = "Wafer edge", State = AssistantStateEnum.UnAble };
            this.ReferenceDie = new AssistantState() { Name = "Reference die", State = AssistantStateEnum.ForBidden };
            this.ComponentTransportUnitGeometry = new AssistantState() { Name = "Component transportUnit geometry", State = AssistantStateEnum.ForBidden };
            this.BadComponentTable = new AssistantState() { Name = "bad-component table", State = AssistantStateEnum.ForBidden };
            this.ComponentTransportUnitAdjust = new AssistantState() { Name = "Component transportUnit adjust", State = AssistantStateEnum.ForBidden };
            this.ComponentPickup = new AssistantState() { Name = "Component pickup", State = AssistantStateEnum.UnAble };
            if (this.AccuracyMode == AccuracyModeEnum.Off)
            {
                this.ComponentAccuracyMode = new AssistantState() { Name = "Component accuracy mode", State = AssistantStateEnum.ForBidden };
            }
            else
            {
                this.ComponentAccuracyMode = new AssistantState() { Name = "Component accuracy mode", State = AssistantStateEnum.UnAble };
            }

            this.ComponentPlacement = new AssistantState() { Name = "Component placement", State = AssistantStateEnum.UnAble };

            this.FlipToWafer = new AssistantState() { Name = "Flip to wafer", State = AssistantStateEnum.ForBidden };
        }        

        /// <summary>
        /// 顶针名称
        /// </summary>
        [TreeProgramListArgs("顶针名称", "晶圆芯片载具属性")]
        public string EjectionName { get; set; }

        /// <summary>
        /// 顶针顶起速度
        /// </summary>
        [TreeProgramListArgs("顶针顶起速度", "晶圆芯片载具属性", 0.1, 200, UnitHelper.speed)]
        public double EjectLiftSpeed { get; set; } = 50;

        /// <summary>
        /// 与顶针预顶位置的相对高度
        /// 顶起距离
        /// </summary>
        [TreeProgramListArgs("顶针相对预顶位置的顶起高度", "晶圆芯片载具属性", 0, 7, UnitHelper.mm)]
        public double RelativeHeightWithEjectionReadyLiftPosition { get; set; } = 0;

        /// <summary>
        /// 顶针台工作位置
        /// </summary>
        [TreeProgramListArgs("顶针台工作位置", "晶圆芯片载具属性", UnitHelper.mm)]
        public AKRSPoint3D EjectionTableWorkPosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 扩晶高位
        /// </summary>
        [TreeProgramListArgs("扩晶高度", "晶圆芯片载具属性", UnitHelper.mm)]
        public AKRSPoint3D ExpandUpPosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 晶圆中心坐标
        /// </summary>
        [TreeProgramListArgs("晶圆中心坐标", "晶圆芯片载具属性", UnitHelper.mm)]
        public AKRSPoint3D WaferCenter { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 晶圆半径
        /// </summary>
        [TreeProgramListArgs("晶圆半径", "晶圆芯片载具属性", UnitHelper.mm)]
        public double WaferRadius { get; set; }

        /// <summary>
        /// 晶圆半径补偿
        /// </summary>
        [TreeProgramListArgs("晶圆半径补偿", "晶圆芯片载具属性", -100, 100, UnitHelper.mm)]
        public double WaferRadiusOffset { get; set; } = 0;

        /// <summary>
        /// 间隔行个数
        /// </summary>
        [TreeProgramListArgs("间隔行个数", "晶圆芯片载具属性", UnitHelper.times)]
        public double AfterNumberOfRows { get; set; } = 0;

        /// <summary>
        /// 间隔列个数
        /// </summary>
        [TreeProgramListArgs("间隔列个数", "晶圆芯片载具属性", UnitHelper.times)]
        public double AfterNumberOfColumns { get; set; } = 0;

        /// <summary>
        /// 单颗搜晶方式的搜索方向
        /// </summary>
        [TreeProgramListArgs("单颗搜晶方式的搜索方向", "晶圆芯片载具属性")]
        public SingleSearchDirection SingleSearchDirection { get; set; } = SingleSearchDirection.ToRightDown;

        /// <summary>
        /// 九颗搜晶方式的搜索方向
        /// </summary>
        [TreeProgramListArgs("九颗搜晶方式的搜索方向", "晶圆芯片载具属性")]
        public NineSearchDirection NineSearchDirection { get; set; } = NineSearchDirection.NineUpToDown;

        /// <summary>
        /// 载具形状
        /// </summary>
        [TreeProgramListArgs("载具形状类型", "晶圆芯片载具属性")]
        public CarrierShapeTypeEnum CarrierShapeType { get; set; } = CarrierShapeTypeEnum.Circular;

        /// <summary>
        /// 矩形区域上边的位置
        /// </summary>
        [TreeProgramListArgs("矩形区域上边的位置", "晶圆芯片载具属性", UnitHelper.mm)]
        public AKRSPoint3D TopEdge { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 矩形区域下边的位置
        /// </summary>
        [TreeProgramListArgs("矩形区域下边的位置", "晶圆芯片载具属性", UnitHelper.mm)]
        public AKRSPoint3D BottomEdge { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 矩形区域左边的位置
        /// </summary>
        [TreeProgramListArgs("矩形区域左边的位置", "晶圆芯片载具属性", UnitHelper.mm)]
        public AKRSPoint3D LeftEdge { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 矩形区域右边的位置
        /// </summary>
        [TreeProgramListArgs("矩形区域右边的位置", "晶圆芯片载具属性", UnitHelper.mm)]
        public AKRSPoint3D RightEdge { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 晶圆图数据配置
        /// </summary>
        [TreeProgramListArgs("晶圆图数据配置", "晶圆芯片载具属性")]
        public WaferMapDataConfig WaferMapDataConfig { get; set; } = new WaferMapDataConfig();

        /// <summary>
        /// 参考点1物理坐标
        /// </summary>
        [TreeProgramListArgs("参考点1物理坐标", "晶圆芯片载具属性", UnitHelper.mm)]
        public AKRSPoint3D ReferencePosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 参考点模板名称
        /// </summary>
        [TreeProgramListArgs("参考点模板名称", "晶圆芯片载具属性")]
        public string ReferenceName => this.GetReferenceName();

        /// <summary>
        /// 力控模式下Z轴低速上抬距离
        /// </summary>
        [TreeProgramListArgs("力控模式下Z轴低速上抬距离", "取晶", 0, 10, UnitHelper.mm)]
        public double ForecResetDistanceDuringForceMode { get; set; } = 0;

        /// <summary>
        /// 芯片和蓝膜总厚度
        /// </summary>
        //[TreeProgramListArgs("芯片和蓝膜总厚度", "晶圆芯片载具属性",  false, UnitHelper.mm)]
        public double ComponentAndCarrierThickness { get; set; } = 0;

        /// <summary>
        /// 获取参考点模板名称
        /// </summary>
        /// <returns>result</returns>
        private string GetReferenceName()
        {
            return this.Name + "Reference";
        }

        /// <summary>
        /// 是否需要人工确认起点
        /// </summary>
        [TreeProgramListArgs("是否需要人工确认起点", "晶圆芯片载具属性")]
        public bool IsConfirmStartPointWithManual { get; set; } = false;

        #region Bond

        /// <summary>
        /// 是否开启同步顶
        /// </summary>
        [TreeProgramListArgs("是否开启同步顶", "晶圆芯片载具属性")]
        public bool IsActivateSynchronousEjection { get; set; }

        /// <summary>
        /// 顶起速度模式枚举
        /// </summary>
        [TreeProgramListArgs("顶起速度模式枚举", "晶圆芯片载具属性")]
        public SpeedModeEnum EjectionSpeedMode { get; set; }
        #endregion
    }
}
