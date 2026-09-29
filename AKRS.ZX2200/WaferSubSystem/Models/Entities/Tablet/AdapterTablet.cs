using System;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle;
using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet
{
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    /// <summary>
    /// 华夫盘类
    /// </summary>
    [Serializable]
    public class AdapterTablet : BaseTablet
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AdapterTablet()
        {
            this.TabletType = TabletTypeEnum.Adapter;
            this.Name = string.Empty;
        }

        /// <summary>
        /// 适配器
        /// </summary>
        [JsonIgnore]
        [TreeProgramListArgs("Adapter", "AdapterTablet")]
        public AdapterConfig AdapterSetting => AdapterConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == this.Name);

        /// <summary>
        /// 华夫料片状态
        /// </summary>
        public AdapterEntity AdapterState { get; set; }

        /// <summary>
        /// 创建料片状态
        /// </summary>
        public void CreateAdapterEntity()
        {
            if (this.AdapterSetting == null)
            {
                return;
            }

            this.AdapterState = new AdapterEntity(this.AdapterSetting.MaxUseWaffleCount);
            for (int i = 0; i < this.AdapterState.WafflePlateSlotsState.Length; i++)
            {
                if (this.AdapterSetting.WaffleArray[i].CarrierWithWaffleConfig != null)
                {
                    this.AdapterState.WafflePlateSlotsState[i] = new AdapterSlotEntity(this.AdapterSetting.WaffleArray[i]);
                }
            }
        }
    }
}
