using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.ZX2200.BondSystem.Models.Repositories;
using AKRS.ZX2200.SupportFeature.Consumables;
using AKRS.ZX2200.SupportFeature.Parameters;
using AKRS.ZX2200.SupportFeature.Parameters.Model;
using PropertyChanged;

namespace AKRS.ZX2200.BondSystem.Models.Programs
{
    /// <summary>
    /// 刮胶程式
    /// </summary>
    public class SlideFluxerProgram
    {
        /// <summary>
        /// 刮胶胶时间间隔,单位h
        /// </summary>
        [TreeProgramListArgs("刮胶时间间隔", null, 0, 9999999, "h")]
        public double SlideFluxInterval { get; set; } = 1;

        ///// <summary>
        ///// 助焊剂使用时间限制,单位h
        ///// </summary>
        //[TreeProgramListArgs("助焊剂使用时间限制", null, 0, 9999999, "h")]
        //public double SlideFluxChangeTimeLimit { get; set; } = 99999;

        /// <summary>
        ///  助焊剂耗材
        /// </summary>
        public TimeConsumable FluxConsumable { get; set; } = new TimeConsumable() { Name="助焊剂"};

        /// <summary>
        /// 自动刮胶延时,单位min
        /// </summary>
        [TreeProgramListArgs("刮胶延时", null, 0, 9999999, "ms")]
        public int SlideDelay { get; set; } = 10;

        /// <summary>
        /// 刮胶次数
        /// </summary>
        [TreeProgramListArgs("刮胶次数", null, 0, 9999999, "s")]
        public int SlideFluxTimes { get; set; } = 60;
    }
}
