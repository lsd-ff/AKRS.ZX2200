using System;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;

namespace AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBoxGeo
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    /// <summary>
    /// this库
    /// </summary>
    [Serializable]
    public class MagazineBoxGeoConfig : BaseDsSetting
    {
        /// <summary>
        /// 槽间距
        /// </summary>
        [TreeProgramListArgs("槽间距", "上料盒硬件设置", UnitHelper.mm)]
        public double SlotPitch { get; set; }

        /// <summary>
        /// 最低槽位与基准位置的距离
        /// </summary>
        [TreeProgramListArgs("最低槽位与基准位置的距离", "上料盒硬件设置", UnitHelper.mm)]
        public double PositionOfLowestSlot { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        [TreeProgramListArgs("层数", "上料盒硬件设置", UnitHelper.times)]
        public int LayerCount { get; set; } = 25;

        /// <summary>
        /// 晶圆尺寸
        /// </summary>
        [TreeProgramListArgs("晶圆尺寸", "上料盒硬件设置")]
        public WaferSizeEnum WaferSize { get; set; } = WaferSizeEnum.EightInches;

        /// <summary>
        /// Magazine尺寸大小
        /// </summary>
        [TreeProgramListArgs("料盒尺寸大小", "上料盒硬件设置", UnitHelper.mm)]
        public AKRSPoint3D MagazineSize { get; set; } = new AKRSPoint3D();
    }
}
