using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Information
{
    public class ModuleInfo : BaseInformation
    {
        /// <summary>
        /// 系统1已经将所有工艺完成，后续将不会去定位
        /// </summary>
        public bool System1Finished { get; set; }

        /// <summary>
        /// 系统2已经将所有工艺完成，后续将不会去定位
        /// </summary>
        public bool System2Finished { get; set; }

        /// <summary>
        /// 所属基板号
        /// </summary>
        public int SubstrateNum { get; set; } = 0;
    }
}
