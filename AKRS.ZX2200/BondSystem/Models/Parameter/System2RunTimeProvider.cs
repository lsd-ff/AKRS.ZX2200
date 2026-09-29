using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.BondSystem.Controllers;


namespace AKRS.ZX2200.BondSystem.Models.Parameter
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.ZX2200.BondSystem.BondForce.Modbus;
    using AKRS.ZX2200.BondSystem.Controls.Manual;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.BondSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.SupportFeature.Statistics;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Services;

    using DevExpress.Data.Helpers;
    using DevExpress.XtraEditors;
    using DevExpress.XtraSplashScreen.Utils;

    using log4net.Core;
    using MathNet.Numerics;
    using OfficeOpenXml;

    /// <summary>
    /// 提供工作时的参数和方法
    /// </summary>
    public static class System2RunTimeProvider
    {
        /// <summary>
        /// 系统2计时器
        /// </summary>
        public static Stopwatch System2Stopwatch { get; set; } = new Stopwatch();

        /// <summary>
        /// 是不是第一次点胶
        /// </summary>
        public static bool IsFirstDispense { get; set; } = true;

        /// <summary>
        /// Bond线程是否第一次启动
        /// </summary>
        public static bool IsBondTaskFirstStart { get; set; } = true;

        /// <summary>
        /// 刷新Map是否完成
        /// </summary>
        public static bool IsFinishRefreshMap { get; set; } = false;

        /// <summary>
        /// 换吸嘴吹气是否结束
        /// </summary>
        public static bool IsChangeToolBlowFinish { get; set; } = true;

        /// <summary>
        /// 上视觉灯光是否设置成功
        /// </summary>
        public static bool IsSetUpLookLightSuccess { get; set; }

        /// <summary>
        /// 焊头上是否有料
        /// </summary>
        public static bool IsMaterialOnBondhead { get; set; } = false;

        /// <summary>
        /// 上视定位失败自动跳过计数
        /// </summary>
        public static int UpLookAutoSkipCount { get; set; } = 0;

        /// <summary>
        /// 静态华夫盒芯片定位失败自动跳过计数
        /// </summary>
        public static int StaticWaffleComponentAutoSkipCount { get; set; } = 0;

        /// <summary>
        /// 重取类型，暂时不用
        /// </summary>
        public static RepickTypeEnum RePickType { get; set; } = RepickTypeEnum.None;

        /// <summary>
        /// 顶针计时
        /// </summary>
        public static Stopwatch EjectStopwatch { get; set; } = new Stopwatch();

        /// <summary>
        /// T轴单独运动线程计时
        /// </summary>
        public static Stopwatch AxisTMoveTaskStopwatch { get; set; } = new Stopwatch();

        /// <summary>
        /// T轴单独运动计时
        /// </summary>
        public static Stopwatch AxisTMoveStopwatch { get; set; } = new Stopwatch();

        /// <summary>
        /// 顶针顶起耗时
        /// </summary>
        public static List<(DateTime time, double liftTime)> EjectionLiftTime = new();

        /// <summary>
        /// 顶针缩回耗时
        /// </summary>
        public static List<double> EjectToReadyLiftTime { get; set; } = new List<double>();

        /// <summary>
        /// 是否需要预点胶
        /// </summary>
        public static bool IsPreDispense { get; set; } = false;

        /// <summary>
        /// 读焊头力线程
        /// </summary>
        public static Task ReadBondForceTask;

        /// <summary>
        /// 接触前的力
        /// </summary>
        public static double BeforeTouchForce { get; set; } = 0;

        /// <summary>
        /// 上次擦拭吸嘴后 取了多少颗芯片
        /// 擦拭吸嘴功能用
        /// </summary>
        public static int PickupNumAfterLastCleanNozzle = 0;

        /// <summary>
        /// 刮胶盘是否手动状态
        /// </summary>
        public static bool IsSlideFluxerManual = false;

        /// <summary>
        /// 看胶印计数
        /// </summary>
        public static int WatchFluxCount = 0;

        /// <summary>
        /// 在基板上点胶印计数
        /// </summary>
        public static int DipOnTUCount = 0;

        /// <summary>
        /// 是否重取芯片
        /// </summary>
        public static bool IsRepickComponent = false;

        /// <summary>
        /// 芯片是否蘸胶
        /// </summary>
        public static bool IsNeedDip = false;

        /// <summary>
        ///  焊头模拟量初始值
        /// </summary>
        public static double BondHeadInitialVal = 0;

        /// <summary>
        ///  是否是新产品
        /// </summary>
        public static bool IsNewProduct = false;

        /// <summary>
        /// 晶圆检查是否通过
        /// </summary>
        public static bool IsWaferCheckSucceed = false;

        /// <summary>
        /// Bond是否到达搜晶位置
        /// </summary>
        public static bool IsBondArriveSearchPos = false;

        /// <summary>
        /// 运行信息重置
        /// </summary>
        public static void ResetInformation()
        {
            IsBondTaskFirstStart = true;
            IsFinishRefreshMap = false;
            IsSetUpLookLightSuccess = false;
            IsChangeToolBlowFinish = true;
            IsRepickComponent = false;

            IsNeedDip = false;

            // 自动重取次数清零
            UpLookAutoSkipCount = 0;

            // 跳过次数清零
            System2RunTimeProvider.StaticWaffleComponentAutoSkipCount = 0;

            BeforeTouchForce = 0;

            IsBondArriveSearchPos = false;

            Stopwatch.StartNew();
        }

        /// <summary>
        /// 时间记录
        /// </summary>
        /// <param name="item">动作模块</param>
        /// <param name="subItem">动作信息</param>
        /// <param name="isAsync">是否为异步</param>
        public static void RecordTime(string item, string subItem, bool isAsync = false)
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DeBugWork)
            {
                if (MachineStateModel.GetInstance().MachineState != MachineStateEnum.Working)
                {
                    throw new Exception("调试模式中断");
                }

                DelayHelper.Delay(Machine.GetInstance().DebugPauseTime);

                UcMainSystem.InputDebugAction(
                    $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.fff")}: {item} - {subItem} 耗时：{System2Stopwatch.ElapsedMilliseconds - Machine.GetInstance().DebugPauseTime} ms");

                Machine.GetInstance().ContinueDebug = false;

                while (Machine.GetInstance().DebugModel == DebugModelEnum.PauseModel
                       && !Machine.GetInstance().ContinueDebug)
                {
                    if (MachineStateModel.GetInstance().MachineState != MachineStateEnum.Working)
                    {
                        throw new Exception("调试模式中断");
                    }

                    DelayHelper.Delay(100);
                }
            }
            
            LogHelper.Post(
                Level.Info,
                $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.fff")}: {item} - {subItem} 耗时：{System2Stopwatch.ElapsedMilliseconds} ms",
                LogCategory.Bond,
                ViewType.InFileAndUI);

            if (!isAsync)
            {
                System2Stopwatch.Restart();
            }
        }

        /// <summary>
        /// 保存顶起时间
        /// </summary>
        public static void SaveEjectionTime()
        {
            try
            {
                // 添加一个工作表
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                ExcelPackage excelPackage = new ExcelPackage(new FileInfo(@"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "-顶针顶起缩回计时.xlsx"));

                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                               ? excelPackage.Workbook.Worksheets[0]
                                               : excelPackage.Workbook.Worksheets.Add("DataSheet");

                // 设置列宽
                for (int i = 1; i <= 7; i++)
                {
                    worksheet.Column(i).Width = 15;
                }

                // 添加标题行
                if (worksheet.Dimension == null)
                {
                    worksheet.Cells[1, 1].Value = "时间";

                    worksheet.Cells[1, 2].Value = "顶起耗时";

                    worksheet.Cells[1, 3].Value = "缩回耗时";
                }

                int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

                int count = Math.Min(EjectionLiftTime.Count, EjectToReadyLiftTime.Count);

                for (int i = 0; i < count; i++)
                {
                    worksheet.Cells[lastUsedRow + 1 + i, 1].Value = EjectionLiftTime[i].time.ToString("MM-dd HH:mm:ss");

                    worksheet.Cells[lastUsedRow + 1 + i, 2].Value = EjectionLiftTime[i].liftTime;

                    worksheet.Cells[lastUsedRow + 1 + i, 3].Value = EjectToReadyLiftTime[i].ToString();
                }

                excelPackage.Save();

                EjectionLiftTime.Clear();
                EjectToReadyLiftTime.Clear();
            }
            catch (Exception e)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"文件已被打开，保存定位数据失败!\r\n Retry:关掉文件重新保存\r\nCancel:不保存数据",
                    "报警",
                    MessageBoxButtons.RetryCancel,
                    MessageBoxIcon.Warning);

                if (dialog == DialogResult.Retry)
                {
                    SaveEjectionTime();
                }
            }
        }

        /// <summary>
        /// 等允许取料信号
        /// </summary>
        /// <param name="component">芯片</param>
        /// <returns>结果</returns>
        public static ExcuteResult WaitAllowBondPickSignal(BaseCarrierConfig component)
        {
            if (component.IsUseFlipTable == false)
            {
                if (component.CarrierType == CarrierTypeEnum.StaticWaffle
                    && System2Configuration.GetInstance().IsAutoMoveToChangeStaticWaffleAvoidancePos)
                {
                    // 是否在换静态华夫盒避让位
                    bool isOnChangeStaticWaffleAvoidancePos = false;

                    // 超时返回False
                    while (!SignalPool.GetInstance().IsWaferAllowPickSignal.Wait(100))
                    {
                        // 判断是否需要换静态华夫盒
                        if (Static.IsNeedChangeStaticWaffle == true)
                        {
                            if (isOnChangeStaticWaffleAvoidancePos == false)
                            {
                                // 移动到避让位
                                System2Domain.GetInstance().BondModuleController.MoveToChangeStaticWaffleAvoidancePos();

                                // 设备暂停
                                Machine.GetInstance().Pause();

                                isOnChangeStaticWaffleAvoidancePos = true;
                            }
                        }

                        // 点停止返回Abort
                        if (MachineStateModel.GetInstance().MachineState == MachineStateEnum.Stop)
                        {
                            return ExcuteResult.Abort;
                        }
                    }
                }
                else
                {
                    // 用Bond相机搜晶不在这里等信号
                    if (component.SearchCamera == SearchCameraEnum.WaferCamera
                        || component.CarrierType == CarrierTypeEnum.StaticWaffle)
                    {
                        // 晶圆上料准备判断（从信号池获取）,这个信号会自动复位
                        if (!SignalPool.GetInstance().IsWaferAllowPickSignal.Wait())
                        {
                            return ExcuteResult.Abort;
                        }
                    }
                }
            }
            else
            {
                // 翻转台允许取料信号,这个信号会自动复位
                if (!SignalPool.GetInstance().IsFlipTableAllowPickSignal.Wait())
                {
                    return ExcuteResult.Abort;
                }
            }

            //if (MachineStateModel.GetInstance().MachineState == MachineStateEnum.Pause)
            //{
            //    Machine.GetInstance().Continue();
            //}

            // 点暂停阻塞
            if (Signal.WaitStart() == false)
            {
                return ExcuteResult.Abort;
            }

            return ExcuteResult.Success;
        }

        /// <summary>
        ///  根据配置结果获取定位结果
        /// </summary>
        /// <param name="matchResult">定位结果</param>
        /// <param name="locateUseConfig">定位使用情况</param>
        /// <returns></returns>
        public static MatchResult GetMatchResultByConfig(MatchResult matchResult,LocateUseConfig locateUseConfig)
        {
            if (!locateUseConfig.UseX)
            {
                matchResult.CenterX = 1224.0;
            }

            if (!locateUseConfig.UseY)
            {
                matchResult.CenterY = 1024.0;
            }

            if (!locateUseConfig.UseAngle)
            {
                matchResult.Angle = 0.0;
            }

            return matchResult;
        }
    }
}
