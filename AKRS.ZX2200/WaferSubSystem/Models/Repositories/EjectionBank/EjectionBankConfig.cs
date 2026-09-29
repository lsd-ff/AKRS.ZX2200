using System;
using System.Collections.Generic;
using System.Linq;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;

namespace AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using DevExpress.Office.Utils;

    /// <summary>
    /// 顶针架类
    /// </summary>
    [Serializable]
    public class EjectionBankConfig : BaseDsSetting
    {
        /// <summary>
        /// 顶针架
        /// </summary>
        [TreeProgramListArgs("顶针架槽位", "顶针架设置")]
        public EjectionBankSlotConfig[] EjectionBankSlots { get; set; } = new EjectionBankSlotConfig[]
                                                                       {
                                                                           new EjectionBankSlotConfig() { Index = 0 },
                                                                           new EjectionBankSlotConfig() { Index = 1 },
                                                                           new EjectionBankSlotConfig() { Index = 2 },
                                                                           new EjectionBankSlotConfig() { Index = 3 },
                                                                           new EjectionBankSlotConfig() { Index = 4 }
                                                                       };
    }
}
