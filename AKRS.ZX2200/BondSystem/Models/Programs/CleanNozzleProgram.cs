using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.Infrastructure.Models.Enums;
using AKRS.ZX2200.SupportFeature.Parameters.Model;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.Models.Programs
{
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.Infrastructure.AOP.Module;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using PropertyChanged;

    /// <summary>
    /// 吸嘴清洁程式
    /// </summary>
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class CleanNozzleProgram : PropertyChangeAop
    {
        /// <summary>
        /// 清洁的区域
        /// </summary>
        public AKRSPoint3D[,] CleanArea { get; set; }

        /// <summary>
        /// 取片前擦拭吸嘴
        /// </summary>
        [TreeProgramListArgs("取片前擦拭吸嘴功能", (string)null, null, RoleEnum.Admin)]
        public bool IsCleanNozzleBeforePickup { get; set; } = false;

        /// <summary>
        /// 取片前擦拭吸嘴
        /// </summary>
        [TreeProgramListArgs("取片前擦拭吸嘴次数", (string)null, null, RoleEnum.Admin)]
        public int CleanNozzleBeforePickupTimes { get; set; } = 1;

        /// <summary>
        /// 取片前擦拭吸嘴
        /// </summary>
        [TreeProgramListArgs("取多少次之后擦拭吸嘴", (string)null, null, RoleEnum.Admin)]
        public int CleanNozzleAfterPickupNum{ get; set; } = 5;

        /// <summary>
        /// 取片前擦拭吸嘴
        /// </summary>
        [TreeProgramListArgs("取片前擦拭吸嘴模式", (string)null, null, RoleEnum.Admin)]
        public ForceModeEnum CleanNozzleBeforePickupForceMode { get; set; } = ForceModeEnum.Force;

        /// <summary>
        /// 取片前擦拭吸嘴力
        /// </summary>
        [TreeProgramListArgs("取片前擦拭吸嘴力", (string)null, null, RoleEnum.Admin)]
        public double CleanNozzleBeforePickupForce { get; set; } = 50;

        /// <summary>
        /// 热熔胶带使用次数限制
        /// </summary>
        [TreeProgramListArgs("热熔胶带同一点最大使用次数限制", (string)null, null, RoleEnum.Admin)]
        public int HeatActivatedFilmUseLimit { get; set; } = 100;

        /// <summary>
        ///  列数
        /// </summary>
        [TreeProgramListArgs("行数", (string)null, false)]
        public int Row { get; set; } = 2;

        /// <summary>
        ///  行数
        /// </summary>
        [TreeProgramListArgs("列数", (string)null, false)]
        public int Column { get; set; } = 2;

        /// <summary>
        ///  Z向硬补偿
        /// </summary>
        [TreeProgramListArgs("Z向补偿", (string)null, false)]
        public double CleanZCompensation { get; set; } = 0;

        /// <summary>
        /// 使用次数
        /// </summary>
        [TreeProgramListArgs("使用次数", (string)null, false)]
        public int Count { get; set; }
    }
}
