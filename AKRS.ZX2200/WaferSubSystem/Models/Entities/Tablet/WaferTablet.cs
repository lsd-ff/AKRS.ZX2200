using System;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet
{
    using System.Drawing;

    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;

    /// <summary>
    /// 晶圆料片类
    /// </summary>
    [Serializable]
    public class WaferTablet : BaseTablet
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WaferTablet()
        {
            this.TabletType = TabletTypeEnum.Componet;
            this.Name = string.Empty;
        }

        /// <summary>
        /// 晶圆
        /// </summary>
        [JsonIgnore]
        [TreeProgramListArgs("CarrierWithWafer", "WaferTablet")]
        public CarrierWithWaferConfig CarrierConfigWithWafer => (CarrierWithWaferConfig)CarrierConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == this.Name);

        /// <summary>
        /// 当前点
        /// </summary>
        [JsonIgnore]
        public Point CurrentPoint { get; set; } = new Point();

        /// <summary>
        /// 当前点位置
        /// </summary>
        public AKRSPoint3D CurrentPosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 是否是新的料片
        /// </summary>
        public bool IsNewTablet { get; set; } = true;

        public SingleSearchDirection SingleSearchDirectionCurrent { get; set; } = SingleSearchDirection.ToRightDown;

        /// <summary>
        /// 重置料片信息
        /// </summary>
        public void InitTabletInfo()
        {
            this.SingleSearchDirectionCurrent = this.CarrierConfigWithWafer.SingleSearchDirection;
            this.CurrentPosition = this.CarrierConfigWithWafer.WaferCenter;
            this.IsNewTablet = true;
        }
    }
}
