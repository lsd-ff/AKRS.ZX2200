using AKRS.ZX2200.TransportUnitSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Information
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;

    [Serializable]
    public class LocateResultInfo
    {
        /// <summary>
        /// P1定位信息
        /// </summary>
        public VisionResult P1Info { get; set; } = new VisionResult();

        /// <summary>
        /// P2定位信息
        /// </summary>
        public VisionResult P2Info { get; set; } = new VisionResult();

        /// <summary>
        /// P3定位信息
        /// </summary>
        public VisionResult P3Info { get; set; } = new VisionResult();

        /// <summary>
        /// P4定位信息
        /// </summary>
        public VisionResult P4Info { get; set; } = new VisionResult();

        /// <summary>
        /// 两个定位点之间的距离
        /// </summary>
        public double TwoPointDistance { get; set; } = 0;

        /// <summary>
        /// 两个定位点之间的距离(原始值)
        /// </summary>
        public double OldTwoPointDistance { get; set; } = 0;
    }
}
