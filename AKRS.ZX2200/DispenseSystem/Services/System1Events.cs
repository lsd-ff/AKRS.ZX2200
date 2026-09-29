using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.DispenseSystem.Services
{
    /// <summary>
    /// 系统1事件类
    /// </summary>
    public static class System1Events
    {
        /// <summary>
        /// 普通事件
        /// </summary>
        public static Action<string> Message { get; set; }

        /// <summary>
        /// 报警事件
        /// </summary>
        public static Action<string> AlmMessage { get; set; }

        /// <summary>
        /// 系统错误事件
        /// </summary>
        public static Action<string> ErrorMessage { get; set; }
    }
}
