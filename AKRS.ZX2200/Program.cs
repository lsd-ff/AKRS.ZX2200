using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using DataAnalysis.Acquisition;
using DevExpress.LookAndFeel;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace AKRS.ZX2200
{
    using AKRS.Galaxy.SoftKey;
    using AKRS.Galaxy2.Component.Simple.MessageBox;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.GT;
    using AKRS.Galaxy2.HardwareEditor;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.BondForce.Models.DevicePara;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Controls;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.VisionControl;
    using DevExpress.XtraEditors;
    using DevExpress.XtraSplashScreen;
    using HarmonyLib;
    using log4net.Core;
    using System.Reflection;
    using System.Runtime.InteropServices;

    using Accord.Statistics.Kernels;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.BondSystem.Models;

    /*DevExpress的皮肤
     * 
    |DevExpress Style|Caramel|Money Twins|DevExpress Dark Style|iMaginary

    |Lilian|Black|Blue|Office 2010 Blue|Office 2010 Black|Office 2010 Silver

    |Office 2007 Blue|Office 2007 Black|Officmetre 2007 Silver|Office 2007 Green

    |Office 2007 Pink|Seven|Seven Classic|Darkroom|McSkin|Sharp|Sharp Plus

    |Foggy|Dark Side|Xmas (Blue)|Springtime|Summer|Pumpkin|Valentine|Stardust

    |Coffee|Glass Oceans|High Contrast|Liquid Sky|London Liquid Sky|The Asphalt World|Blueprint|
     * */

    /// <summary>
    /// 主入口
    /// </summary>
    internal static class Program
    {
        public static MainForm MainForm;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">参数</param>
        [STAThread]
        // ReSharper disable once StyleCop.SA1400
        static void Main(string[] args)
        {
            // 互斥体
            using (Mutex mutex = new Mutex(true, Application.ProductName, out bool createNew))
            {
                if (createNew)
                {
                    // DevExpress.Utils.AppearanceObject.DefaultFont = new System.Drawing.Font("宋体", 9);

                    //// - 指定控件的默认字体，除了菜单（menu）和工具栏（toolbar)
                    // DevExpress.XtraEditors.WindowsFormsSettings.DefaultFont = new Font("宋体", 9, FontStyle.Bold);

                    //// - 指定菜单（menu）和工具栏（toolbar）的默认字体
                    // DevExpress.XtraEditors.WindowsFormsSettings.DefaultMenuFont = new Font("宋体", 9, FontStyle.Bold);

                     DevExpress.Utils.AppearanceObject.DefaultFont = new System.Drawing.Font("Tahoma", 9);

                     // DevExpress.XtraEditors.WindowsFormsSettings.SetDPIAware();

                    // - 指定控件的默认字体，除了菜单（menu）和工具栏（toolbar)
                    DevExpress.XtraEditors.WindowsFormsSettings.DefaultFont = new Font("Tahoma", 9, FontStyle.Bold);

                     // - 指定菜单（menu）和工具栏（toolbar）的默认字体
                     DevExpress.XtraEditors.WindowsFormsSettings.DefaultMenuFont = new Font("Tahoma", 9, FontStyle.Bold);

                    DevExpress.UserSkins.BonusSkins.Register();
                    DevExpress.Skins.SkinManager.EnableFormSkins();

                    //UserLookAndFeel.Default.SetSkinStyle("DevExpress Dark Style");

                    UserLookAndFeel.Default.SetSkinStyle("Office 2010 Blue");

                    #region 未处理异常注册事件

                    Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

                    // 处理UI线程异常
                    Application.ThreadException += new ThreadExceptionEventHandler(AppUIThreadException);

                    // 处理非UI线程异常
                    AppDomain.CurrentDomain.UnhandledException += new System.UnhandledExceptionEventHandler(AppThreadException);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    // 检查内存
                    if (!Machine.GetInstance().IsDiskCapacitySufficient())
                    {
                        return;
                    }

                    // 检查设备参数是否是迁移过来的
                    if (Machine.GetInstance().JudgeDataMigrated())
                    {
                        FrmParaChangeTipAfterDataMigrate frmParaChangeTip = new FrmParaChangeTipAfterDataMigrate();
                        frmParaChangeTip.StartPosition = FormStartPosition.CenterScreen;
                        frmParaChangeTip.ShowDialog();
                    }

                    // 暂时屏蔽所有记录
                    // DataLogManager.Instance.SuspendAllRecord();
                    DataLogManager.Instance.CreateDataLog("重复标定实验").ConfigureFileOutput("精度实验\\重复定位结果.json");

                    Thread thread = new Thread(FindFormThread);
                    thread.IsBackground = true;
                    thread.Name = "自动关闭DEVExPress";
                    thread.Start();

                    // 允许日志记录
                    LogsManager.IsLogWarn = true;
                    LogsManager.IsLogInfo = true;

                    #region 检测加密狗

                    if (MachineSoftwareConfiguration.GetInstance().IsDetectingEncryption)
                    {
                        // 加密狗检测不成功退出
                        if (!SoftkeyManager.CheckYtSoftKey())
                        {
                            mutex.ReleaseMutex();
                            return;
                        }
                        else
                        {
                            SoftkeyManager.InitSoftKey();
                            SoftkeyManager.GetDateDue(out bool permanent, out DateTime authDate);

                            int dueTime = (authDate - DateTime.Now).Days;

                            if (MachineSoftwareConfiguration.GetInstance().IsPromptEncryption)
                            {
                                if (dueTime < 7)
                                {
                                    AKRSXtraMessageBox.Show($"软件加密到限仅剩{dueTime},请联系设备人员进行处理");
                                }

                                SoftkeyManager.GetUserId(out int userId);

                                Machine.GetInstance().AuthenticatorDueDate = permanent ? "永久授权" : $"{authDate:yyyy/MM/dd}";
                                Machine.GetInstance().AuthenticatorId = userId.ToString();
                            }
                        }
                    }

                    #endregion

                    // 检查版本信息
                    VersionUpdateAnnouncement.CheckVersionUpdate();

                    if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.OffLineWork)
                    {
                        try
                        {
                            Machine.GetInstance().InitPR();

                            SplashScreenManager.ShowForm(typeof(SplashScreen1));
                            HardwareRepositoryService.InitAllHardware(HardwareEditor.initNoticeAction);

                            SplashScreenManager.CloseForm();

                            // 重置报警
                            Machine.GetInstance().SetStopStateAlarm();

                            AxisCard gtEthercatCard = HardwareRepositoryService.GetAxisCard(1);

                            // 重新设置限位
                            AxisCard gtGlinkCard = HardwareRepositoryService.GetAxisCard(2);

                            MachineHardwareConfiguration mc = MachineHardwareConfiguration.GetInstance();

                            if (HardwareRepositoryService.GetAxisCard(1).MotionController.MotionControllerDrive is GTController)
                            {
                                Axis bondZ = HardwareRepositoryService.GetHardware<Axis>("BondZ");
                                GTAxis bondZAxisDrive = (GTAxis)bondZ.AxisDrive;
                                ForceConfig forceConfig = ForceConfig.GetInstance();
                                bondZ.ServoOff();

                                // 设置Z 的平滑时间，否则在插补的时候Z会响动
                                ((GTAxis)bondZ.AxisDrive).SetMotionSmooth(30, 15);

                                bondZAxisDrive.ChangeMode(mc.PositionClosedLoopKp, mc.PositionClosedLoopKvff, 0, mc.PositionClosedLoopKaff, (int)forceConfig.BondheadMaxPress, (short)bondZ.AxisNum, (short)System2Module.GetInstance().BondModule.BondHead.ReadBondheadForce.SensorIO);

                                bondZ.ServoOn();

                                gtGlinkCard.SetAllSoftLimit();
                            }
                            else 
                            {
                                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                                {
                                    // ---------3、4号机焊头 雷赛需要启用， 56 号机 固高不用----------
                                    Axis bondT = HardwareRepositoryService.GetHardware<Axis>("焊头T");
                                    bondT.SetOffsetPos(0);
                                    bondT.AbsoluteMove(0);
                                    // ----------------------------
                                }
                            }

                            gtEthercatCard.SetAllSoftLimit();


                        }
                        catch (Exception ex)
                        {
                            LogHelper.Post(Level.Error, $"硬件初始化异常：{ex.ToString()}", LogCategory.Hardware, ViewType.InFileAndUI);

                            AKRSMessageBoxExt.Show(
                                $"硬件初始化异常：{ex.ToString()}\r\n",
                                "异常",
                                new string[] { "确认" },
                                new DialogResult[] { DialogResult.Abort },
                                AlarmLevel.SecondLevel);
                        }
                    }

                    var harmony = new Harmony("一线码农聊技术");
                    Type applicationType = typeof(Application);
                    Type marshalingControlType = applicationType.GetNestedType("MarshalingControl", BindingFlags.NonPublic);
                    ConstructorInfo constructor = marshalingControlType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
                    var prefix = typeof(HookMarshalingControl).GetMethod("OnActionExecuting");
                    harmony.Patch(constructor, new HarmonyMethod(prefix));

                    // 委托
                    PREntity.IsSaveLocateImage += MachineSoftwareConfiguration.GetInstance().SaveLoacteImage;
                    PREntity.SaveLocateImageAction += VisionService.SaveVisionImage;

                    if (MachineSoftwareConfiguration.GetInstance().IsAxisResetBeforeWork)
                    {
                        Machine.GetInstance().AllAxisGoHome();
                    }

                    MainForm = new MainForm();
                    Application.Run(MainForm);

                    #endregion
                }
                else
                {
                    MessageBox.Show(@"软件重复打开");
                    Thread.Sleep(100);
                    Environment.Exit(1);
                }

                #region 主窗体程序退出时执行的部分，这里用于程序关闭再次重启

                mutex.ReleaseMutex();
                
                #endregion
            }
        }

        /// <summary>
        /// UI线程异常处理
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        #region 未处理异常事件
        private static void AppUIThreadException(object sender, ThreadExceptionEventArgs e)
        {
            AKRSXtraMessageBox.Show(
                $@"程序出现异常：{System.Environment.NewLine}{e.Exception.Message}{System.Environment.NewLine}{e.Exception.StackTrace}");
        }

        /// <summary>
        /// APP线程异常处理
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private static void AppThreadException(object sender, System.UnhandledExceptionEventArgs e)
        {
            AKRSXtraMessageBox.Show(
                $@"程序出现异常：{System.Environment.NewLine}{((Exception)e.ExceptionObject).Message}{System.Environment.NewLine}{((Exception)e.ExceptionObject).StackTrace}");
        }
        #endregion


        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        private static extern void PostMessage(IntPtr hwnd, int msg, int wParam, int lParam);
        private const int WM_CLOSE = 0x0010;
        private static int m_nWaitTime = 0;

        private static void FindFormThread()
        {
            IntPtr handle = FindWindow(null, "About DevExpress");
            while (handle == IntPtr.Zero && m_nWaitTime <= 5000)
            {
                Thread.Sleep(1);
                handle = FindWindow(null, "About DevExpress");
                m_nWaitTime++;
            }
            PostMessage(handle, WM_CLOSE, 0, 0);
        }
    }


    /// <summary>
    /// Hook MarshalingControl 的描述类
    /// </summary>
    public class HookMarshalingControl
    {
        /// <summary>
        /// 原生方法之前执行的 action
        /// </summary>
        public static void OnActionExecuting()
        {
            int managedThreadId = Thread.CurrentThread.ManagedThreadId;

            // managedThreadId为1是主线程
            if (managedThreadId != 1)
            {
                // 检测非工作线程创建控件
                LogHelper.Post(
                    Level.Info,
                    $"控件创建线程:{managedThreadId}" + $"控件创建线程名称:{Thread.CurrentThread.Name}",
                    LogCategory.Global,
                    ViewType.InFileAndUI);

                //AKRSMessageBoxExt.Show(
                //    $"非法控件被创建，请联系软件人员并重启软件。控件创建线程:{managedThreadId}" + $"控件创建线程名称:{Thread.CurrentThread.Name}",
                //    "非法创建控件报警",
                //    new string[] { "确认" },
                //    new DialogResult[] { DialogResult.OK },
                //    AlarmLevel.FirstLevel);
            }
        }
    }
}
