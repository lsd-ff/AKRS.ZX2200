using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Main.Machine.Process
{
    /// <summary>
    /// 系统制程
    /// </summary>
    public class SystemProcess
    {
        /// <summary>
        /// 步骤集合
        /// </summary>
        public List<SingleProcessStep> ProcessSteps { get; set; } = new List<SingleProcessStep>();

        /// <summary>
        /// 工作步骤
        /// </summary>
        public WorkProcessEnum WorkProcess { get; set; } = WorkProcessEnum.BondPositionFirst;

        /// <summary>
        /// 工作步骤模式
        /// </summary>
        public WorkProcessModuleEnum WorkProcessModule { get; set; } = WorkProcessModuleEnum.StepFirst;

        /// <summary>
        /// 系统1工作循环次数
        /// </summary>
        public bool IsWorkCycleNumber { get; set; } = false;

        /// <summary>
        /// 系统1工作循环次数
        /// </summary>
        public int WorkCycleNumber { get; set; } = 1;
    }
}
