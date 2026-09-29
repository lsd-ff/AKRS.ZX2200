using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.CalibSystem.Models;

using Newtonsoft.Json;

namespace AKRS.ZX2200.BondSystem.Models.DeviceParams
{
    /// <summary>
    /// 相机设备参数
    /// </summary>
    public class CameraDevicePara
    {
        /// <summary>
        /// 上视旋转中心位置
        /// </summary>
        [JsonIgnore]
        public AKRSPoint3D UpLookPos => CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos;

        /// <summary>
        /// Bond相机跟X方向的角度
        /// </summary>
        public double BondCamAngleX { get; set; }

        /// <summary>
        /// Bond相机跟Y方向的角度
        /// </summary>
        public double BondCamAngleY { get; set; }
    }
}
