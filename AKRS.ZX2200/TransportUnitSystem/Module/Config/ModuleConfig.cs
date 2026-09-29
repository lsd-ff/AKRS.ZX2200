using AKRS.ZX2200.TransportUnitSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Config
{
    using AKRS.ZX2200.Infrastructure.AOP.Module;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    public class ModuleConfig : ArrayConfig
    {
        /// <summary>
        /// 墨点检测
        /// </summary>
        [TreeProgramListArgs("Module ink dot", (string)null)]
        public bool IsModuleInkDot { get; set; } = false;

        /// <summary>
        /// 是否距离检查
        /// </summary>
        [TreeProgramListArgs("Is DistanceCheck", (string)null)]
        public bool IsDistanceCheck { get; set; } = false;

        /// <summary>
        /// 距离容差
        /// </summary>
        [TreeProgramListArgs("DistanceTolerance", (string)null)]
        public double DistanceTolerance { get; set; }

        /// <summary>
        /// 对称排列
        /// </summary>
        [TreeProgramListArgs("IsSymmetric", (string)null)]
        public bool IsSymmetric { get; set; }


        /// <summary>
        /// 普通排列
        /// </summary>
        [TreeProgramListArgs("CommonDataSet", (string)null)]
        public bool CommonDataSet { get; set; }

        /// <summary>
        /// 排列方式
        /// </summary>
        [TreeProgramListArgs("排列方式", (string)null)]
        public ArrangementEnum Arrangement { get; set; } = ArrangementEnum.Normal;


        /// <summary>
        /// 工作方向
        /// </summary>
        [TreeProgramListArgs("工作方向", (string)null)]
        public WorkOrderEnum WorkOrderEnum { get; set; } = WorkOrderEnum.LeftDownToRight;
    }
}
