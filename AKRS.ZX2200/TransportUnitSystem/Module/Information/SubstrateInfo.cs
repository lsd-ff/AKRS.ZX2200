using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Information
{
    public class SubstrateInfo : BaseInformation
    {
        /// <summary>
        /// 系统1已经将所有工艺完成
        /// </summary>
        public bool System1Finished { get; set; }

        /// <summary>
        /// 系统2已经将所有工艺完成
        /// </summary>
        public bool System2Finished { get; set; }

        /// <summary>
        /// 测试结果
        /// </summary>
        public int Test { get; set; } = 1;
    }
}
