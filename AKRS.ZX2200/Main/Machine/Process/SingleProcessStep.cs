using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Main.Machine.Process
{
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.ZX2200.Infrastructure.Models.Enums;

    /// <summary>
    /// 单个流程
    /// </summary>
    public class SingleProcessStep
    {
        /// <summary>
        /// 步骤名称
        /// </summary>
        public SingleProcessStep()
        {
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string ProcessStepName { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 系统
        /// </summary>
        public ProcessSystemEnum PreProcessSystem { get; set; }

        /// <summary>
        /// 工作模式
        /// </summary>
        public ProcessingStrategyEnum PreProcessingStrategy { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; } = true;

        /// <summary>
        /// 焊点名称
        /// </summary>
        public string BondPositionName { get; set; } = "Null";

        /// <summary>
        /// 芯片名称
        /// </summary>
        public string ComponentName { get; set; } = "Null";

        /// <summary>
        /// 检测
        /// </summary>
        public string DefectName { get; set; } = "Null";

        /// <summary>
        /// 胶型
        /// </summary>
        public string EpoxyNameApplicationName { get; set; } = "Null";
    }
}
