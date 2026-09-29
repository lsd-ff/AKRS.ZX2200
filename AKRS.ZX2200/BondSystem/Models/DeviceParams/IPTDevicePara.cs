using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.Infrastructure.CommonModel;

namespace AKRS.ZX2200.BondSystem.Models.DeviceParams
{
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    /// <summary>
    /// 中转台设备参数
    /// </summary>
    public class IPTDevicePara
    {
        /// <summary>
        /// 中转台测高位置
        /// </summary>
        [TreeProgramListArgs("左中转台测高位置(G0)", (string)null, false, UnitHelper.mm)]
        public AKRSPoint3D IPTPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 中转台中心圆拍照位,G0
        /// </summary>
        [TreeProgramListArgs("左中转台中心圆拍照位(G0)", (string)null, false, UnitHelper.mm)]
        public AKRSPoint3D IPTVisionPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 中转台测高位置
        /// </summary>
        [TreeProgramListArgs("右中转台测高位置(G0)", (string)null, false, UnitHelper.mm)]
        public AKRSPoint3D RightIPTPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 中转台中心圆拍照位,G0
        /// </summary>
        [TreeProgramListArgs("右中转台中心圆拍照位(G0)", (string)null, false, UnitHelper.mm)]
        public AKRSPoint3D RightIPTVisionPos { get; set; } = new AKRSPoint3D();

        ///// <summary>
        ///// 参数是否为空
        ///// </summary>
        ///// <returns>结果</returns>
        //public bool IsEmpty()
        //{
        //    //bool ret1 = this.IPTPos.IsEmpty;
          
        //    //return ret1;
        //}
    }
}
