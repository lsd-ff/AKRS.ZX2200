using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.CalibSystem
{
    /// <summary>
    /// 日志基类
    /// </summary>
    internal abstract class BaseLog
    {
        /// <summary>
        /// 存储路径
        /// </summary>
        public abstract string FilePath { get; set; }

        /// <summary>
        /// 次数,每个区域测几次
        /// </summary>
        public abstract int Number { get; set; }
    }
}
