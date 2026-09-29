using System;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using DevExpress.XtraRichEdit.Model;

namespace AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle
{
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Models;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;

    using Newtonsoft.Json;

    /// <summary>
    /// 载具类-华夫盒
    /// </summary>
    [Serializable]
    public class CarrierWithWaffleConfig : BaseCarrierConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CarrierWithWaffleConfig()
        {
            this.CarrierType = CarrierTypeEnum.Waffle;
            this.SearchMode = SearchMode.Box;

            this.ComponentGeometry = new AssistantState() { Name = "Component geometry", State = AssistantStateEnum.UnAble };
            this.ComponentWaferMap = new AssistantState() { Name = "Component wafer map", State = AssistantStateEnum.ForBidden };
            if (this.IsInkDotSearch)
            {
                this.InkDot = new AssistantState() { Name = "Ink dot", State = AssistantStateEnum.UnAble };
            }
            else
            {
                this.InkDot = new AssistantState() { Name = "Ink dot", State = AssistantStateEnum.ForBidden };
            }

            this.WaferEdge = new AssistantState() { Name = "Wafer edge", State = AssistantStateEnum.ForBidden };
            this.ReferenceDie = new AssistantState() { Name = "Reference die", State = AssistantStateEnum.ForBidden };
            this.ComponentTransportUnitGeometry = new AssistantState() { Name = "Component transportUnit geometry", State = AssistantStateEnum.UnAble };
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
        }

        /// <summary>
        /// 华夫盒类型
        /// </summary>
        [TreeProgramListArgs("华夫盒类型", "华夫盒芯片载具属性")]
        public WaffleTypeEnum WaffleType { get; set; } = new WaffleTypeEnum();

        /// <summary>
        /// 行数
        /// </summary>
        [TreeProgramListArgs("行数", "华夫盒芯片载具属性", false, UnitHelper.times)]
        public int RowCount { get; set; }

        /// <summary>
        /// 列数
        /// </summary>
        [TreeProgramListArgs("列数", "华夫盒芯片载具属性", false, UnitHelper.times)]
        public int ColumnCount { get; set; }

        /// <summary>
        /// Bond预取料位置
        /// </summary>
        [TreeProgramListArgs("Bond预取料位置", "华夫盒芯片载具属性", UnitHelper.mm)]
        public AKRSPoint3D ReadyBondPickPosition { get; set; } = new AKRSPoint3D();
    }
}
