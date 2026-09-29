using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Infrastructure.AOP.Module;
using AKRS.ZX2200.SupportFeature.Parameters.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.BondSystem.Models.Parameter
{
    /// <summary>
    /// 系统2点胶板参数
    /// </summary>
    public class S2PreDispensePlatePara
    {
        /// <summary>
        /// 起始点位
        /// </summary>
        [TreeProgramListArgs("预点胶板开始点位")]
        public AKRSPoint3D PreDispensePlateStartPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 结束点位
        /// </summary>
        [TreeProgramListArgs("预点胶板结束点位")]
        public AKRSPoint3D PreDispensePlateEndPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 列数
        /// </summary>
        [TreeProgramListArgs("预点胶板列数")]
        public int Columns { get; set; }

        /// <summary>
        /// 行数
        /// </summary>
        [TreeProgramListArgs("预点胶板行数")]
        public int Rows { get; set; }

        /// <summary>
        /// 传感器收到信号和平时的距离
        /// </summary>
        [TreeProgramListArgs("预点胶板平面到信号触发的距离", (string)null)]
        public double DistanceInSensor { get; set; }

        /// <summary>
        /// 是否示教完成
        /// </summary>
        public bool AssistanceFinish { get; set; } = false;
    }
}
