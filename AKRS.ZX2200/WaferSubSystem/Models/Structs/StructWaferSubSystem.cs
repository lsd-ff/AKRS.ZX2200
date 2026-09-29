using AKRS.Galaxy2.Infrastructure.CommonModel;
using System;

namespace AKRS.ZX2200.WaferSubSystem.Models.Structs
{
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.WM;

    /// <summary>
    /// 晶圆上的单颗芯片信息结构体
    /// </summary>
    public struct SingleDieInfo
    {
        /// <summary>
        /// 芯片是否存在
        /// </summary>
        public bool Exist;

        /// <summary>
        /// 芯片WFX/WFY的位置
        /// </summary>
        public AKRSPoint3D Pos;

        /// <summary>
        /// 芯片的角度
        /// </summary>
        public MatchResult MatchResult;
    }
}
