using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Structs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip
{
    /// <summary>
    /// 标记芯片
    /// </summary>
    public class MarkedDie
    {
        /// <summary>
        /// 晶圆上的单颗芯片信息
        /// </summary>
        [JsonIgnore]
        public SingleDieInfo DieInfo;

        /// <summary>
        /// 行数
        /// </summary>
        [JsonIgnore]
        public int RowNum;

        /// <summary>
        /// 行方向
        /// </summary>
        [JsonIgnore]
        public RowDirection RowDirection;
    }
}
