using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Drive.Common;
//using ACS.SPiiPlusNET;
using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Drive.Exceptions;
using AKRS.Galaxy2.Drive.Exceptions;
using AKRS.Galaxy2.Drive.Exceptions;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionControllerInterface;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionControllerInterface;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionControllerInterface;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.GT;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.GT;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
using AKRS.Galaxy2.Drive.PiezoControllers.NewPort;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.Aops;
using AKRS.Galaxy2.LogicHardware.Aops;
using AKRS.Galaxy2.LogicHardware.Attributes;
using AKRS.Galaxy2.LogicHardware.Attributes;
using AKRS.Galaxy2.LogicHardware.Exceptions;
using AKRS.Galaxy2.LogicHardware.Exceptions;
using AKRS.Galaxy2.LogicHardware.Hardwares;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers.Parameters;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers.Parameters;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.ZX2200.Infrastructure.Models.BaseModels;
using AKRS.ZX2200.Infrastructure.Models.BaseModels;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.SupportFeature.Parameters;
using AKRS.ZX2200.SupportFeature.Parameters;
using AKRS.ZX2200.SupportFeature.Parameters.Model;
using AKRS.ZX2200.SupportFeature.Parameters.Model;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors;
using GTN;
using GTN;
using LanguageExt;
using LanguageExt.ClassInstances;
using LanguageExt.Common;
using log4net.Core;
using log4net.Core;
using Newtonsoft.Json;
using Newtonsoft.Json;
using System;
using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics;
using System.Diagnostics;
using System.Reflection;
using System.Reflection;
using System.Threading;
using System.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms;
using static GTN.mc;
using static GTN.mc;

namespace AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;


    /// <summary>
    /// Adapter
    /// </summary>
    [Serializable]
    public class AdapterConfig : BaseDsSetting
    {
        /// <summary>
        /// 适配器类型
        /// </summary>
        //[TreeProgramListArgs("适配器类型", "适配器")]
        [TreeProgramListArgs("Adapter Type", "AdapterSetting")]
        public AdapterTypeEnum AdapterType { get; set; } = AdapterTypeEnum.WaferTable;

        /// <summary>
        /// Waffle个数
        /// </summary>
        //[TreeProgramListArgs("最大使用槽数", "适配器", UnitHelper.times)]
        [TreeProgramListArgs("Max Use Waffle Count", "AdapterSetting", UnitHelper.times)]
        public int MaxUseWaffleCount { get; set; } = 0;

        /// <summary>
        /// WaffleArray
        /// </summary>
        //[TreeProgramListArgs("槽数组", "适配器")]
        [TreeProgramListArgs("Waffle Array", "AdapterSetting")]
        public AdapterSlotConfig[] WaffleArray { get; set; } = new AdapterSlotConfig[]
                                                                  {
                                                                      new AdapterSlotConfig() { Index = 0 },
                                                                      new AdapterSlotConfig() { Index = 1 },
                                                                      new AdapterSlotConfig() { Index = 2 },
                                                                      new AdapterSlotConfig() { Index = 3 },
                                                                      new AdapterSlotConfig() { Index = 4 },
                                                                      new AdapterSlotConfig() { Index = 5 },
                                                                      new AdapterSlotConfig() { Index = 6 },
                                                                      new AdapterSlotConfig() { Index = 7 },
                                                                      new AdapterSlotConfig() { Index = 8 },
                                                                      new AdapterSlotConfig() { Index = 9 },
                                                                      new AdapterSlotConfig() { Index = 10 },
                                                                      new AdapterSlotConfig() { Index = 11 },
                                                                      new AdapterSlotConfig() { Index = 12 },
                                                                      new AdapterSlotConfig() { Index = 13 },
                                                                      new AdapterSlotConfig() { Index = 14 },
                                                                      new AdapterSlotConfig() { Index = 15 },
                                                                      new AdapterSlotConfig() { Index = 16 },
                                                                      new AdapterSlotConfig() { Index = 17 },
                                                                      new AdapterSlotConfig() { Index = 18 },
                                                                      new AdapterSlotConfig() { Index = 19 },
                                                                      new AdapterSlotConfig() { Index = 20 },
                                                                      new AdapterSlotConfig() { Index = 21 },
                                                                      new AdapterSlotConfig() { Index = 22 },
                                                                      new AdapterSlotConfig() { Index = 23 },
                                                                      new AdapterSlotConfig() { Index = 24 },
                                                                      new AdapterSlotConfig() { Index = 25 },
                                                                      new AdapterSlotConfig() { Index = 26 },
                                                                      new AdapterSlotConfig() { Index = 27 },
                                                                      new AdapterSlotConfig() { Index = 28 },
                                                                      new AdapterSlotConfig() { Index = 29 },
                                                                  };
    }
}


