using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Main.VisionControl
{
    /// <summary>
    /// 更新版本信息
    /// </summary>
    public class VersionInfo
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public Version VersionNumber { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdataTime { get; set; }

        /// <summary>
        /// 更新内容
        /// </summary>
        public List<string> UpdateContent { get; set; }
    }
}
