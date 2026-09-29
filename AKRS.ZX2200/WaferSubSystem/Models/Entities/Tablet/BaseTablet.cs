using System;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    /// <summary>
    /// 料片类
    /// </summary>
    [Serializable]
    public abstract class BaseTablet : BaseDsSetting
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseTablet()
        {
            this.Name = string.Empty;
        }

        /// <summary>
        /// 料片类型
        /// </summary>
        [TreeProgramListArgs("TabletType", "BaseTablet")]
        public TabletTypeEnum TabletType { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 槽号
        /// </summary>
        [JsonIgnore]
        public int SlotNum => this.Index + 1;

        ///// <summary>
        ///// 槽位位置
        ///// </summary>
        //[JsonIgnore]
        //public AKRSPoint3D SlotPosition => WaferSubDevicePara.GetInstance().MagazineDevicePara.Position?[this.Index];

        /// <summary>
        /// 槽位状态
        /// </summary>
        [TreeProgramListArgs("SlotState", "BaseTablet")]
        public SlotStatuEnum SlotState { get; set; } = SlotStatuEnum.None;

        /// <summary>
        /// 是否可以使用
        /// </summary> 
        public bool IsOKSlotState { get; set; }
    }
}
