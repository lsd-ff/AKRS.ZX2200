using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;

    using DevExpress.CodeParser;

    /// <summary>
    /// 华夫盘槽位类
    /// </summary>
    [Serializable]
    public class AdapterSlotConfig : BaseDsSetting
    {
        /// <summary>
        /// 华夫盒
        /// </summary>
        [JsonIgnore]
        public CarrierWithWaffleConfig CarrierWithWaffleConfig => (CarrierWithWaffleConfig)CarrierConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == this.Name);

        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 首料位
        /// </summary>
        [TreeProgramListArgs("FirstPosition", "", UnitHelper.mm)]
        public AKRSPoint3D FirstPosition { get; set; } = new AKRSPoint3D();
    }
}
