using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Main.Machine.ObjectLevel
{
    /// <summary>
    /// 物体水平数据
    /// </summary>
    public class ObjectLevelData
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public string IsQualified { get; set; }

        /// <summary>
        /// 标准
        /// </summary>
        public string Standard { get; set; }

        /// <summary>
        /// 当前数据
        /// </summary>
        public string CurrentData { get; set; }
    }
}
