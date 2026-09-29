using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace AKRS.ZX2200.Infrastructure.Action
{
    using AKRS.Galaxy2.Log;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Service;
    using DevExpress.XtraBars;
    using DevExpress.XtraBars.Ribbon;
    using DevExpress.XtraReports.UI;
    using log4net.Core;
    using System.Reflection;

    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.Main.Machine.MachineSupport;

    using System.Collections.Generic;

    using AKRS.ZX2200.SupportFeature.Statistics;

    using MathNet.Numerics.Statistics;

    /// <summary>
    /// UI显示界面的委托
    /// </summary>
    public static class UiAction
    {
        ///// <summary>
        ///// Log事件
        ///// </summary>
        ///// <param name="sender">事件源</param>
        ///// <param name="e">封装参数</param>
        //private static void BarManagerLog(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    BarButtonItem barButtonItem = e.Item as BarButtonItem;
        //    if (barButtonItem != null)
        //    {
        //        LogHelper.Post(Level.Trace, $"点击了 {barButtonItem.Caption} 按钮", LogCategory.Global);
        //        OperateLogEntity operateLogEntity = new OperateLogEntity(
        //            DateTime.Now,
        //            $"点击了 {barButtonItem.Caption} 按钮",
        //            Machine.GetInstance().FrmLogin?.CurrentUser?.Name);

        //        StatisticsService.DbEntityBlockingCollection.Add(operateLogEntity);
        //    }
        //}

        ///// <summary>
        ///// Log事件
        ///// </summary>
        ///// <param name="sender">事件源</param>
        ///// <param name="e">封装参数</param>
        //private static void SimpleButtonLog(object sender, EventArgs e)
        //{
        //    SimpleButton simpleButton = (SimpleButton)sender;
        //    LogHelper.Post(Level.Trace, $"点击了 {simpleButton.Text} 按钮", LogCategory.Global);
        //    OperateLogEntity operateLogEntity = new OperateLogEntity(
        //        DateTime.Now,
        //        $"点击了 {simpleButton.Text} 按钮",
        //        Machine.GetInstance().FrmLogin?.CurrentUser?.Name);

        //    StatisticsService.DbEntityBlockingCollection.Add(operateLogEntity);
        //}

        ///// <summary>
        ///// 是否初始过
        ///// </summary>
        //private static bool installed = false;

        ///// <summary>
        ///// 注入日志
        ///// </summary>
        //public static void Install()
        //{
        //    if (!installed)
        //    {
        //        var harmony = new Harmony("com.yourcompany.formloadlogger");
        //        harmony.PatchAll(Assembly.GetExecutingAssembly());
        //        installed = true;
        //    }
        //}

        ///// <summary>
        ///// 创建加载时候
        ///// </summary>
        //[HarmonyPatch(typeof(Control), "OnClick")]
        //public class SimpleButtonClick
        //{
        //    /// <summary>
        //    /// 注入的方法
        //    /// </summary>
        //    /// <param name="__instance">实例</param>
        //    /// <param name="__args">事件</param>
        //    static void Prefix(Control __instance, EventArgs __args)
        //    {
        //        if (__instance is SimpleButton)
        //        {
        //            __instance.Click -= SimpleButtonLog;
        //            __instance.Click += SimpleButtonLog;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 创建加载时候
        ///// </summary>
        //[HarmonyPatch(typeof(BarButtonItem), "OnClick")]
        //public class BarButtonClick
        //{
        //    /// <summary>
        //    /// 注入的方法
        //    /// </summary>
        //    /// <param name="__instance">实例</param>
        //    /// <param name="__args">事件</param>
        //    static void Prefix(BarButtonItem __instance, EventArgs __args)
        //    {
        //        if (__instance is BarButtonItem)
        //        {
        //            __instance.ItemClick -= BarManagerLog;
        //            __instance.ItemClick += BarManagerLog;
        //        }
        //    }
        //}
    }
}
