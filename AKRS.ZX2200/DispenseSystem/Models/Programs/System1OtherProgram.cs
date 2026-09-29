using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.DispenseSystem.Models.Programs
{
    /// <summary>
    /// System1其他参数会放在这个类里面
    /// </summary>
    public class System1OtherProgram
    {
        /// <summary>
        /// System1是否启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 拍照失败重试次数
        /// </summary>
        public int VisionFailRetryCount { get; set; }

        /// <summary>
        /// 相机拍照延迟
        /// </summary>
        public int VisionDelay { get; set; }
    }
}
