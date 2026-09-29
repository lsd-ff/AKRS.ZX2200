using AKRS.ZX2200.WaferSubSystem.Models.Structs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.PR.Models.MatchResults;

    /// <summary>
    /// 搜索结果类（9颗芯片信息）
    /// </summary>
    public class NineDie
    {
        /// <summary>
        /// 左上
        /// </summary>
        [JsonIgnore]
        public SingleDieInfo LeftUpDie = new SingleDieInfo(){ Exist = true, Pos = new AKRSPoint3D(), MatchResult = new MatchResult() };

        /// <summary>
        /// 上
        /// </summary>
        [JsonIgnore]
        public SingleDieInfo UpDie = new SingleDieInfo() { Exist = true, Pos = new AKRSPoint3D(), MatchResult = new MatchResult() };

        /// <summary>
        /// 右上
        /// </summary>
        [JsonIgnore]
        public SingleDieInfo RightUpDie = new SingleDieInfo() { Exist = true, Pos = new AKRSPoint3D(), MatchResult = new MatchResult() };

        /// <summary>
        /// 左中
        /// </summary>
        [JsonIgnore]
        public SingleDieInfo LeftCenterDie = new SingleDieInfo() { Exist = true, Pos = new AKRSPoint3D(), MatchResult = new MatchResult() };

        /// <summary>
        /// 中
        /// </summary>
        [JsonIgnore]
        public SingleDieInfo CenterDie = new SingleDieInfo() { Exist = true, Pos = new AKRSPoint3D(), MatchResult = new MatchResult() };

        /// <summary>
        /// 右中
        /// </summary>
        [JsonIgnore]
        public SingleDieInfo RightCenterDie = new SingleDieInfo() { Exist = true, Pos = new AKRSPoint3D(), MatchResult = new MatchResult() };

        /// <summary>
        /// 左下
        /// </summary>
        [JsonIgnore]
        public SingleDieInfo LeftDownDie = new SingleDieInfo() { Exist = true, Pos = new AKRSPoint3D(), MatchResult = new MatchResult() };

        /// <summary>
        /// 下
        /// </summary>
        [JsonIgnore]
        public SingleDieInfo DownDie = new SingleDieInfo() { Exist = true, Pos = new AKRSPoint3D(), MatchResult = new MatchResult() };

        /// <summary>
        /// 右下
        /// </summary>
        [JsonIgnore]
        public SingleDieInfo RightDownDie = new SingleDieInfo() { Exist = true, Pos = new AKRSPoint3D(), MatchResult = new MatchResult() };
    }
}
