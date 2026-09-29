using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AKRS.ZX2200.TransportSystem.Models.Enums;
using Newtonsoft.Json;

namespace AKRS.ZX2200.TransportSystem.Models.Programs
{
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.TransportSystem.Models.DatasetModels.StockBin;

    /// <summary>
    ///  下料仓程式
    /// </summary>
    public class UnLoaderBinProgram
    {
        /// <summary>
        /// 当前下料仓名称
        /// </summary>
        public string CurUnLoaderName { get; set; }

        /// <summary>
        /// 当前下料仓
        /// </summary>
        [JsonIgnore]
        public UnloaderBin UnloaderBin => UnLoaderBinRepository.GetInstance().GetLoaderBin(this.CurUnLoaderName);

        /// <summary>
        /// 当前上料盒
        /// </summary>
        [TreeProgramListArgs("Current stock bin")]
        public BinTypeEnum CurBinType { get; set; } = BinTypeEnum.BinA;

        /// <summary>
        /// 当前放置层（从1开始）
        /// </summary>
        [TreeProgramListArgs("Current place layer", (string)null, 1, 100, "")]
        public int CurPlaceLayer { get; set; } = 1;
    }
}
