using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;

using Newtonsoft.Json;
using System.Threading;
using System;

namespace AKRS.ZX2200.WaferSubSystem.Modules
{
    using System.Drawing;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Services;

    /// <summary>
    /// 上晶圆模组集合
    /// </summary>
    public class WaferSubModule : SingletonNoSave<WaferSubModule>
    {
        /// <summary>
        /// 顶针台模组
        /// </summary>
        [JsonIgnore]
        public EjectModule Eject { get; set; } = new EjectModule();

        /// <summary>
        /// 倒装模组
        /// </summary>
        [JsonIgnore]
        public FlipModule FlipModule { get; set; } = new FlipModule();

        /// <summary>
        /// 料架模组
        /// </summary>
        [JsonIgnore]
        public MagazineBoxModule MagazineBox { get; set; } = new MagazineBoxModule();

        /// <summary>
        /// 晶圆台模组
        /// </summary>
        [JsonIgnore]
        public WaferTableModule WaferTable { get; set; } = new WaferTableModule();

        /// <summary>
        /// point转换成AkrsPoint2D
        /// </summary>
        /// <param name="point">输入</param>
        /// <returns>result</returns>
        public AKRSPoint3D PointConvertAkrsPoint3D(Point point)
        {
            AKRSPoint3D temp = new AKRSPoint3D();
            temp.X = ((double)point.X) / 1000;
            temp.Y = ((double)point.Y) / 1000;
            return temp;
        }

        /// <summary>
        /// AkrsPoint3DConvertPoint
        /// </summary>
        /// <param name="temp">temp</param>
        /// <returns>result</returns>
        public Point AkrsPoint3DConvertPoint(AKRSPoint3D temp)
        {
            return new Point((int)(temp.X * 1000), (int)(temp.Y * 1000));
        }
    }
}
