using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.ZX2200.WaferSubSystem.Models.Programs;
using WaferSystemProgram = AKRS.ZX2200.WaferSubSystem.Models.WaferSystemProgram;

namespace AKRS.ZX2200.WaferSubSystem.Services
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Controls;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBox;
    using AKRS.ZX2200.WaferSubSystem.Modules;
    using log4net.Core;
    using Newtonsoft.Json;
    using System.Diagnostics;
    using System.Drawing;

    // todo  改成WaferSystemProvider

    /// <summary>
    /// 静态
    /// </summary>
    public static class Static
    {
        /// <summary>
        /// CurrentWaferMapName
        /// </summary>
        [JsonIgnore]
        public static string CurrentWaferMapName { get; set; }

        /// <summary>
        /// IsWaferChangeTestSignal
        /// </summary>
        [JsonIgnore]
        public static bool IsWaferChangeTestSignal { get; set; }

        /// <summary>
        /// 静态华夫盒是否备料成功
        /// </summary>
        [JsonIgnore]
        public static bool IsNeedChangeStaticWaffle { get; set; }

        /// <summary>
        /// 晶圆系统计时器
        /// </summary>
        public static Stopwatch WaferSystemStopwatch { get; set; } = new Stopwatch();

        /// <summary>
        /// 翻转工具上是否有芯片
        /// </summary>
        public static bool IsComponentOnFlipTool { get; set; } = false;

        /// <summary>
        /// 时间记录
        /// </summary>
        /// <param name="item">动作信息</param>
        /// <param name="subItem">动作信息</param>
        public static void RecordTime(string item, string subItem)
        {
            LogHelper.Post(
                Level.Info,
                $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.fff")}: {item} - {subItem} 耗时：{WaferSystemStopwatch.ElapsedMilliseconds} ms",
                LogCategory.Component,
                ViewType.InFileAndUI);

            WaferSystemStopwatch.Restart();
        }
    }
}
