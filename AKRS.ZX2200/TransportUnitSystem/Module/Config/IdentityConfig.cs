using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.ZX2200.SupportFeature.Parameters.Model;
using AKRS.ZX2200.TransportUnitSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Config
{
    [Serializable]
    public class IdentityConfig
    {
        /// <summary>
        /// 定位配置的程序
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 识别类型
        /// </summary>
        [TreeProgramListArgs("识别类型", (string)null)]
        public IdentificationEnum Identification { get; set; }

        /// <summary>
        /// P1 PR名称
        /// </summary>
        [TreeProgramListArgs("P1PRName", (string)null)]
        public string P1PRName => MachineConfigContext.GetInstance().RecipeName + this.Name + "ID识别1";

        /// <summary>
        /// P1 视觉定位点位
        /// 相对坐标
        /// </summary>
        [TreeProgramListArgs("P1VisionRelativePos", (string)null)]
        public AKRSPoint3D P1VisionRelativePos { get; set; } = new AKRSPoint3D();
    }
}
