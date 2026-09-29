#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/11/29 15:19:27
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;

namespace AKRS.ZX2200.TransportSystem.Models
{
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Models.Path;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.TransportSystem.Models.Enums;

    /// <summary>
    /// 描述：基板流道的设备参数
    /// </summary>
    public class TransportDevicePara : Singleton<TransportDevicePara> 
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static TransportDevicePara()
        {
            Singleton<TransportDevicePara>.FilePath = ZX2200PathConfig.TransportDeviceParaPath;
        }

        /// <summary>
        /// 点胶两段传输轨道之间的距离
        /// 后续在参数界面展示出来
        /// </summary>
        public double DistanceOfSeparate { get; set; }

        /// <summary>
        /// 是否反找
        /// </summary>
        [TreeProgramListArgs("轨道反找材料", "")]
        public bool IsReverseSearch { get; set; } = true;

        /// <summary>
        /// 是否反找
        /// </summary>
        [TreeProgramListArgs("继续启动是否提示报警", "")]
        public bool IsReStartWarn { get; set; } = true;

        /// <summary>
        /// 手动释放真空
        /// </summary>
        [TreeProgramListArgs("手动释放真空", "")]
        public bool IsManuallyReleaseVacuum { get; set; } = false;

        /// <summary>
        /// 轨道安全高度
        /// </summary>
        public double TransportEdgeHeight { get; set; } = 100;
    }
}
