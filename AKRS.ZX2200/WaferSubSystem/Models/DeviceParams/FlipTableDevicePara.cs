using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.WaferSubSystem.Models.DeviceParams
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    /// <summary>
    /// 翻转模组设备参数
    /// </summary>
    public class FlipTableDevicePara
    {
        /// <summary>
        /// 晶圆芯片交接位,T轴
        /// </summary> 
        public double FilpGetComponentPos { get; set; }

        /// <summary>
        /// 焊头芯片交接位
        /// </summary>
        public AKRSPoint3D BondheadGetComponentPos { get; set; }

        /// <summary>
        /// 翻转台位置
        /// </summary>
        public AKRSPoint3D FlipTablePos { get; set; }

        /// <summary>
        /// 是否完成示教
        /// </summary> 
        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// 翻转工具抛料吹气比例
        /// </summary>
        [TreeProgramListArgs("翻转工具抛料吹气比例", "翻转模组", 1, 200000.0)]
        public int ThrowBlowProportion { get; set; } = 300;

        /// <summary>
        /// 搜晶避让位，G0坐标
        /// </summary>
        [TreeProgramListArgs("搜晶避让位", "翻转模组", UnitHelper.mm)]
        public double SearchComponentAvoidancePos { get; set; } = -21;

        /// <summary>
        /// 翻转台左上位置
        /// </summary>
        [TreeProgramListArgs("翻转台左上位置(G0)", "翻转模组")]
        public AKRSPoint3D FlipTableLeftTopPos { get; set; } = new AKRSPoint3D(-81.12, -136.63, -31);

        /// <summary>
        /// 右下
        /// </summary>
        [TreeProgramListArgs("翻转台右下位置(G0)", "翻转模组")]
        public AKRSPoint3D FlipTableRightBottomPos { get; set; } = new AKRSPoint3D(-8.54, -164.99, -31);
    }
}
