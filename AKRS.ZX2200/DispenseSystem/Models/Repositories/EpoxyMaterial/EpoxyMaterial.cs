using AKRS.ZX2200.Models;
using DevExpress.CodeParser;
using DevExpress.Utils.Filtering.Internal;
using LanguageExt.Pipes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.DispenseSystem.Models.Repositories.EpoxyMaterial
{
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.SupportFeature.Consumables;

    /// <summary>
    /// 点胶胶水
    /// </summary>
    [Serializable]

    public class EpoxyMaterial : BaseDsSetting
    {
        /// <summary>
        /// 胶水的耗材
        /// </summary>
        public TimeConsumable EpoxyMaterialConsumable { get; set; } = new TimeConsumable();

        /// <summary>
        /// 按时间填充还是按周期填充
        /// 这个是刷胶的
        /// </summary>
        public RefillCriteriaEnum RefillCriteria;

        /// <summary>
        /// 比重
        /// </summary>
        public double SpecificWeight;

        /// <summary>
        /// 胶水寿命获取的方式
        /// </summary>
        public  EpoxyLifetimeEntryMethodEnum EpoxyLifetimeEntryMethod;

        /// <summary>
        /// 可能是某些胶水是按次数来限制的
        /// </summary>
        public double MaxNumberOfCycles;

        /// <summary>
        /// 防止胶水变干的延迟参数
        /// </summary>
        public int DryOutPreventionDelay;

        /// <summary>
        /// 胶水的编号,这个应该是批次号
        /// </summary>
        public int GlueIdMask;

        /// <summary>
        /// 胶水已经使用的时间
        /// </summary>
        public double EpoxyOrFluxUsableTime;

        /// <summary>
        /// 胶水类型，刷胶特指点胶头那边的参数
        /// </summary>
        public EpoxyUseTypeEnum EpoxyUseType;

        /// <summary>
        /// 胶水余量报警
        /// </summary>
        public bool IsGlueResidueAlarm;

        /// <summary>
        /// 胶水寿命到期报警
        /// </summary>
        public bool IsEpoxyServiceLifeAlarm;

        /// <summary>
        /// 胶水寿命时间
        /// </summary>
        public int EpoxyServiceLife;

        /// <summary>
        /// 胶水隔多长时间没有点胶报警
        /// </summary>
        public bool IsEpoxyMaximumIntervalTimeAlarm;

        /// <summary>
        /// 胶水隔多长时间没有点胶
        /// </summary>
        public double EpoxyMaximumIntervalTime;

        /// <summary>
        /// 使用寿命预提示
        /// </summary>
        public bool IsServiceLifePreWarning;

        /// <summary>
        /// 使用寿命预提示时间
        /// </summary>
        public int PreWarningTimePeriod;

        /// <summary>
        /// 使用寿命预提示次数
        /// </summary>
        public int NumberOfPreWarningTimes;

    }
}
