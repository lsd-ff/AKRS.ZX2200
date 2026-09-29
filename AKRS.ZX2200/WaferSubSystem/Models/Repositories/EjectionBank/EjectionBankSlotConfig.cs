using System;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.WaferSubSystem.Services;

    /// <summary>
    /// 顶针架槽位类
    /// </summary>
    [Serializable]
    public class EjectionBankSlotConfig : BaseDsSetting
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 槽号
        /// </summary>
        [JsonIgnore]
        public int SlotNum => this.Index + 1;

        /// <summary>
        /// 顶针
        /// </summary>
        [JsonIgnore]
        [TreeProgramListArgs("顶针配置", "顶针架槽")]
        public EjectionConfig EjectionConfig => EjectionConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == this.Name);

        /// <summary>
        /// 槽位位置
        /// </summary>
        [JsonIgnore]
        [TreeProgramListArgs("槽位位置", "顶针架槽", UnitHelper.mm)]
        public AKRSPoint3D SlotPosition => WaferSubDevicePara.GetInstance().EjectDevicePara.SlotPosition[this.Index];
    }
}
