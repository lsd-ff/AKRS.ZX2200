using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle;

namespace AKRS.ZX2200.WaferSubSystem.Models.Repositories.Carrier
{
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;

    /// <summary>
    /// 静态华夫盒类
    /// </summary>
    [Serializable]
    public class CarrierWithStaticWaffleConfig : CarrierWithWaffleConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CarrierWithStaticWaffleConfig()
        {
            this.CarrierType = CarrierTypeEnum.StaticWaffle;
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
    }
}
