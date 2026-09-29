using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.ZX2200.BondSystem.BondForce.Modbus;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using DevExpress.XtraEditors;
using OfficeOpenXml;

namespace AKRS.ZX2200.BondSystem.Controls.Experiment
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.GT;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.ZX2200.BondSystem.BondForce;
    using AKRS.ZX2200.BondSystem.BondForce.Models.DevicePara;
    using AKRS.ZX2200.BondSystem.BondForce.Services;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.BondSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using DevExpress.DataAccess.DataFederation;

    /// <summary>
    /// 焊头力控测试窗体
    /// </summary>
    public partial class FrmBondheadTest : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 采集线程
        /// </summary>
        private Task task;

        /// <summary>
        /// 采集线程2
        /// </summary>
        private Task task2;

        /// <summary>
        /// 采集线程3
        /// </summary>
        private Task task3;


        /// <summary>
        /// 采集线程4
        /// </summary>
        private Task task4;

        /// <summary>
        /// 采集线程5
        /// </summary>
        private Task task5;

        /// <summary>
        /// 是否暂停
        /// </summary>
        private bool isStop = false;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 模组控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// 焊头
        /// </summary>
        private BondHead bondHead = new BondHead();

        /// <summary>
        /// 通讯服务
        /// </summary>
        private ModbusService modbusService => ModbusService.GetInstance();

        /// <summary>
        /// Stopwatch
        /// </summary>
        private Stopwatch sp = new Stopwatch();


        /// <summary>
        /// Stopwatch
        /// </summary>
        private Stopwatch forceControlStopwatch = new Stopwatch();

        /// <summary>
        ///  最大力
        /// </summary>
        private double minForce;

        /// <summary>
        /// 最小力
        /// </summary>
        private double maxForce;

        /// <summary>
        ///  力间隔
        /// </summary>
        private double forceSpacing;

        /// <summary>
        ///  延时
        /// </summary>
        private int delay;

        /// <summary>
        /// 实验时间
        /// </summary>
        private double testTime;

        /// <summary>
        /// 二段距离
        /// </summary>
        private double slowTravelDistance;

        /// <summary>
        ///  角度间隔
        /// </summary>
        private double angleDistance;


        /// <summary>
        ///  焊头参数
        /// </summary>
        private BondHeadParam bondHeadParam => BondDevicePara.GetInstance().BondHeadParam;


        /// <summary>
        ///  角度间隔
        /// </summary>
        public static bool IsAutoStartTest = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmBondheadTest()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  窗体弹出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmBondheadTest_Shown(object sender, EventArgs e)
        {
            this.BringToFront();
            this.TopMost = true; // 可选，强制置顶
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmBondheadTest_Load(object sender, EventArgs e)
        {
            this.bondHeadController.OpenBondHeadVaccum();

            // 这里留一点余量是为了防止报错
            this.SpMinForce.Value = (decimal)(this.bondHeadParam.ForceControlMinVal + 0.1);
            this.SpMaxForce.Value = (decimal)(this.bondHeadParam.ForceControlMaxVal - 0.1);
            this.SpDelay.Value = ForceConfig.GetInstance().ForceKeepDelay;

            // 自动开启实验
            if (IsAutoStartTest)
            {
                this.BtnStart_Click(sender, e);
            }
        }

        /// <summary>
        ///  保存
        /// </summary>
        private void Save()
        {
            this.testTime = (double)this.SpEclispTime.Value;
            this.minForce = (double)this.SpMinForce.Value;
            this.maxForce = (double)this.SpMaxForce.Value;
            this.forceSpacing = (double)this.SpForceSpacing.Value;
            this.delay = (int)this.SpDelay.Value;
            this.slowTravelDistance = (double)this.SpSlowTravelDistance.Value;
            this.angleDistance = (double)this.SpAngleDistance.Value;
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        public void BtnStart_Click(object sender, EventArgs e)
        {
            this.Save();
            if (this.task == null || this.task.IsCompleted == true)
            {
                LbTip.Visible = true;

                // 开始采集
                this.isStop = false;
                this.BtnStart.Text = @"Stop";
                this.task = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("力控测试线程");
                            sp.Restart();
                            this.ForceTest();
                        });

                this.task.ContinueWith(t =>
                    {
                        if (this.BtnStart.InvokeRequired)
                        {
                            this.BtnStart.Invoke(
                                new Action(
                                    () =>
                                        {
                                            this.BtnStart.Text = @"Start";
                                            LbTip.Visible = false;
                                        }));
                        }
                        else
                        {
                            this.BtnStart.Text = @"Start";
                            LbTip.Visible = false;
                        }
                    });
            }
            else
            {
                // 暂停采集
                this.isStop = true;
                this.BtnStart.Text = @"Start";
                this.sp.Stop();

                LbTip.Visible = false;
            }
        }

        /// <summary>
        /// 检验力
        /// </summary>
        public void ForceTest()
        {
            // 打印资源
            List<(DateTime dateTime, double targetForce, double angle, double beforeTouchForce, double
                posZAfter, double touchBondheadForce, double touchForce, double riseForce, double time,
                double forceReference, double touchBondheadForceBefore, double touchForceBefore, double
                posZBefore)> printList = new();

            try
            {
                // 连接摩尔力设备
                if (!this.modbusService.ConnectManometer())
                {
                    return;
                }

                this.bondHeadController.MoveBondZToSafePos();

                AKRSPoint3D forceCalibratePos =
                    this.bondModuleController.ConvertG0ToMachinePos(
                        BondDevicePara.GetInstance().BMCDevicePara.ForceCalibratePos);

                // 抬起位置
                double liftLevel = forceCalibratePos.Z + 6;

                double bondLevel = forceCalibratePos.Z + 0.1;

                double preBondLevel = bondLevel + slowTravelDistance;

                double speed = this.bondHeadController.GetAxisZAbsoluteSpeed()
                               * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

                while (true)
                {
                    // 获取力值数据
                    for (double targetForce = this.minForce; targetForce <= this.maxForce; targetForce = targetForce + this.forceSpacing)
                    {
                        //double angle = 0;
                        for (double angle = -180; angle <= 180; angle = angle + this.angleDistance)
                        {
                            bool isSmallForce = ForceCalibrationService.JudgeIsSmallForce(targetForce);

                            // 移动到标定位
                            this.bondModuleController.MoveToG0Pos(
                                BondDevicePara.GetInstance().BMCDevicePara.ForceCalibratePos.X,
                                BondDevicePara.GetInstance().BMCDevicePara.ForceCalibratePos.Y);

                            this.bondHeadController.RotateAxisT(angle);

                            //// T轴到位后压力表可能还没稳定
                            //Thread.Sleep(100);

                            // todo：放在二段速这里清零，更符合贴片流程
                            if (this.bondHead.AxisZ.AxisDrive is ETELAxis)
                            {
                                if (this.system2Controller.GetBondheadForceValue() < -1)
                                {
                                    this.modbusService.ResetBondhead();
                                }
                            }
                            else
                            {
                                double read = this.system2Controller.GetBondheadForceValue();

                                if (isSmallForce == false && read < -1)
                                {
                                    // 焊头力控清零
                                    this.bondHeadController.ResetBondhead();
                                }
                            }

                            double calibrateTableForceInitial = this.system2Controller.GetCalibrateTableForceValue();

                            // 置零
                            if (Math.Abs(calibrateTableForceInitial) > 3)
                            {
                                this.modbusService.ResetManometer();
                            }

                            calibrateTableForceInitial = this.system2Controller.GetCalibrateTableForceValue();

                            double beforeTouchForce = this.system2Controller.GetBondheadForceValue();

                            double inputForce = targetForce;

                            // 如果是固高轴先运动到预固晶位
                            this.bondHeadController.MoveAxisZ(preBondLevel);

                            if (this.bondHead.AxisZ.AxisDrive is ETELAxis)
                            {
                                // etel的话在预固精位置开启力控
                                bondLevel = forceCalibratePos.Z + 0.1 + slowTravelDistance - 0.01;
                            }

                            // 开始计时
                            forceControlStopwatch.Restart();

                            #region 力控下压,这里为了打印中间数据复制了整个方法

                            this.bondHeadController.CheckZAxisSoftLimit(bondLevel);

                            // 力值转换,Etel的单位是N，固高的单位是g
                            double forceReference;
                            double curAngle = this.bondHeadController.GetAxisTRealPos();

                            // 判断驱动器类型 
                            if (this.bondHead.AxisZ.AxisDrive is ETELAxis)
                            {
                                double initialValue = this.bondHeadController.GetBondForceCurrentVal(inputForce);

                                forceReference = ForceCalibrationService.ActualForceToInputForce(inputForce, initialValue,angle);

                                // 这里加速度默认是速度的10倍
                                // 为了和标定统一，改成速度和加速度改成100和500
                                this.bondHead.EtelForceControlSet(forceReference, bondLevel,100 /*speed*/, 500/*speed * 10.0*/, 100);
                            }
                            else
                            {
                                // 判定是否小力
                                bool isSmallforce = ForceCalibrationService.JudgeIsSmallForce(inputForce);

                                // 切换通道、设置压力参数
                                this.bondHeadController.ChangeChannelAndSetForceControlPara(isSmallforce);

                                double initialValue = this.bondHeadController.GetBondForceCurrentVal(inputForce);

                                // 出来的单位是N要换算成g
                                forceReference = ForceCalibrationService.ActualForceToInputForce(inputForce, initialValue, angle) * 100.0;

                                this.bondHeadController.GTForceControlSet(forceReference, bondLevel, 0, isSmallforce);
                            }

                            #endregion

                            // 计时结束
                            forceControlStopwatch.Stop();

                            // 读焊头力
                            double touchBondheadForceBefore = this.bondHeadController.GetBondForceCurrentVal(isSmallForce);

                            double touchForceBefore = /*ModbusService.GetInstance().ReadManometer()[0] / 10.0*/
                                this.system2Controller.GetCalibrateTableForceValue() - calibrateTableForceInitial;

                            double posZBefore = this.bondHeadController.GetAxisZRealPos();

                            // 读取摩尔力设备读数
                            Thread.Sleep(this.delay);

                            double touchForce = /*ModbusService.GetInstance().ReadManometer()[0] / 10.0*/
                                this.system2Controller.GetCalibrateTableForceValue() - calibrateTableForceInitial;

                            // 读焊头力
                            double touchBondheadForce = this.bondHeadController.GetBondForceCurrentVal(isSmallForce);

                            double posZAfter = this.bondHeadController.GetAxisZRealPos();

                            // 加这句是为了防止Z轴抬起时没有恢复正常速度
                            this.bondHeadController.SetAxisZSpeed(100);

                            // 抬起
                            this.bondHeadController.ForceControlReset(preBondLevel, speed);

                            this.bondModuleController.MoveToSafePos();

                            // 读焊头力
                            double riseForce = /*this.modbusService.ReadBondForce()[0] / 10.0*/ this.system2Controller.GetBondheadForceValue();   

                            printList.Add(new ()
                                              {
                                dateTime = DateTime.Now,
                                targetForce = targetForce,
                                angle= angle,
                                beforeTouchForce = beforeTouchForce,
                                posZAfter = posZAfter,
                                touchBondheadForce = touchBondheadForce,
                                touchForce = touchForce,
                                riseForce = riseForce,
                                time = forceControlStopwatch.ElapsedMilliseconds,
                                forceReference = forceReference ,
                                touchBondheadForceBefore = touchBondheadForceBefore,
                                touchForceBefore= touchForceBefore,
                                posZBefore = posZBefore,
                                              } );

                            //this.SaveForceControlData(targetForce, angle, beforeTouchForce, posZAfter, touchBondheadForce, touchForce, riseForce, forceControlStopwatch.ElapsedMilliseconds,forceReference, touchBondheadForceBefore, touchForceBefore, posZBefore);

                            if (printList.Count > 500)
                            {
                                this.SaveForceControlData(printList);

                                printList.Clear();
                            }

                            if (this.isStop)
                            {
                                this.SaveForceControlData(printList);

                                printList.Clear();

                                // 点停止直接退出
                                return;
                            }

                            if (this.sp.Elapsed.TotalHours > this.testTime)
                            {
                                this.SaveForceControlData(printList);

                                printList.Clear();

                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.bondHeadController.MoveBondZToSafePos();

                AKRSXtraMessageBox.Show($"测试异常！\r\n{e.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Invoke(new Action(() => { this.LbTip.Visible = false; }));
                this.bondHeadController.MoveBondZToSafePos();
            }
        }

        /// <summary>
        /// 以特定力和角度去检验力
        /// </summary>
        private void ForceTest2()
        {
            try
            {
                // 连接摩尔力设备
                if (!this.modbusService.ConnectManometer())
                {
                    return;
                }

                this.bondHeadController.MoveBondZToSafePos();

                AKRSPoint3D forceCalibratePos =
                    this.bondModuleController.ConvertG0ToMachinePos(
                        BondDevicePara.GetInstance().BMCDevicePara.ForceCalibratePos);

                // 抬起位置
                double liftLevel = forceCalibratePos.Z + 6;

                double bondLevel = forceCalibratePos.Z + 0.1;

                double speed = this.bondHeadController.GetAxisZAbsoluteSpeed();

                Stopwatch sp = Stopwatch.StartNew();

                while (true)
                {
                    // 移动到标定位
                    this.bondModuleController.MoveToG0Pos(
                        BondDevicePara.GetInstance().BMCDevicePara.ForceCalibratePos.X,
                        BondDevicePara.GetInstance().BMCDevicePara.ForceCalibratePos.Y);

                    double calibrateTableForceInitial = this.system2Controller.GetCalibrateTableForceValue();

                    // 置零
                    if (Math.Abs(calibrateTableForceInitial) > 3)
                    {
                        this.modbusService.ResetManometer();
                    }

                    calibrateTableForceInitial = this.system2Controller.GetCalibrateTableForceValue();

                    this.bondHeadController.RotateAxisT((double)this.SpAngle.Value);

                    double beforeTouchForce = this.system2Controller.GetBondheadForceValue();
                    double inputForce = (double)this.SpForce.Value;

                    if (this.bondHead.AxisZ.AxisDrive is GTAxis)
                    {
                        // 如果是固高轴先运动到目标位
                        this.bondHeadController.MoveAxisZ(bondLevel + slowTravelDistance);
                    }

                    sp.Restart();

                    // 下压
                    this.bondHeadController.ForceControlSet(inputForce, bondLevel, speed, 50);

                    Thread.Sleep(this.delay);

                    // 读焊头力
                    int[] touchBondheadForce = this.modbusService.ReadBondForce();

                    double  touchForce = this.system2Controller.GetCalibrateTableForceValue();

                    double pos = this.bondHeadController.GetAxisZRealPos();

                    // 加这句是为了防止Z轴抬起时没有恢复正常速度
                    this.bondHeadController.SetAxisZSpeed(2);

                    // 抬起
                    this.bondHeadController.ForceControlReset(liftLevel, speed);

                    sp.Stop();

                    this.bondModuleController.MoveToSafePos();

                    // 读焊头力
                    int[] riseForce = this.modbusService.ReadBondForce();

                    this.SaveForceVal((int)this.SpForce.Value, (double)this.SpAngle.Value, beforeTouchForce, pos, touchBondheadForce[0]/10, touchForce, riseForce[0]/10, sp.Elapsed.TotalMilliseconds);

                    if (this.isStop)
                    {
                        this.Invoke(new Action(() => { this.LbTip.Visible = false; }));

                        // 点停止直接退出
                        return;
                    }

                    if (this.sp.Elapsed.TotalHours > (double)SpEclispTime.Value)
                    {
                        this.Invoke(new Action(() => { this.LbTip.Visible = false; }));
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                this.bondHeadController.MoveBondZToSafePos();

                AKRSXtraMessageBox.Show("测试异常！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.bondHeadController.MoveBondZToSafePos();
            }
        }

        /// <summary>
        /// 以特定角度去检验力
        /// </summary>
        private void ForceTest3()
        {
            try
            {
                // 连接摩尔力设备
                if (!this.modbusService.ConnectManometer())
                {
                    return;
                }

                this.bondHeadController.MoveBondZToSafePos();

                AKRSPoint3D forceCalibratePos =
                    this.bondModuleController.ConvertG0ToMachinePos(
                        BondDevicePara.GetInstance().BMCDevicePara.ForceCalibratePos);

                // 抬起位置
                double liftLevel = forceCalibratePos.Z + 6;

                double bondLevel = forceCalibratePos.Z + 0.1;

                double preBondLevel = bondLevel + slowTravelDistance;

                double speed = this.bondHeadController.GetAxisZAbsoluteSpeed();

                while (true)
                {
                    // 获取力值数据
                    for (double force = minForce; force <= maxForce; force = force + forceSpacing)
                    {
                        this.bondModuleController.MoveToThrowPos();

                        // 移动到标定位
                        this.bondModuleController.MoveSafeBondXYZ(
                            new AKRSPoint3D(
                               forceCalibratePos.X,
                               forceCalibratePos.Y,
                                liftLevel));

                        this.bondHeadController.RotateAxisT((double)this.SpAngle3.Value);

                        int resetCount = 0;
                    resetZero:

                        this.modbusService.ResetManometer();

                        double beforeTouchForce = this.system2Controller.GetBondheadForceValue();
                        double inputForce = force;


                        if (this.bondHead.AxisZ.AxisDrive is GTAxis)
                        {
                            // 如果是固高轴先运动到目标位
                            this.bondHeadController.MoveAxisZ(preBondLevel);
                        }

                        this.forceControlStopwatch.Restart();

                        // 下压
                        this.bondHeadController.ForceControlSet(inputForce, bondLevel, speed, 50);

                        // 读取摩尔力设备读数
                        Thread.Sleep(delay);

                        // 读焊头力
                        //int[] touchBondheadForce = this.ModbusService.ReadBondForce();

                        double touchBondheadForce = this.system2Controller.GetBondheadForceValue();

                        double touchForce = this.system2Controller.GetCalibrateTableForceValue();

                        double pos = this.bondHeadController.GetAxisZRealPos();

                        // 加这句是为了防止Z轴抬起时没有恢复正常速度
                        this.bondHeadController.SetAxisZSpeed(20);

                        // 抬起
                        this.bondHeadController.ForceControlReset(preBondLevel, speed);

                        this.forceControlStopwatch.Stop();

                        this.bondModuleController.MoveToSafePos();

                        // 读焊头力
                        double riseForce = this.system2Controller.GetBondheadForceValue();

                        this.SaveForceVal(force, (double)this.SpAngle.Value, beforeTouchForce, pos, touchBondheadForce, touchForce, riseForce, forceControlStopwatch.ElapsedMilliseconds);

                        if (this.isStop)
                        {
                            this.Invoke(new Action(() => { this.LbTip.Visible = false; }));

                            // 点停止直接退出
                            return;
                        }

                        if (this.sp.Elapsed.TotalHours > (double)SpEclispTime.Value)
                        {
                            this.Invoke(new Action(() => { this.LbTip.Visible = false; }));
                            return;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.bondHeadController.MoveBondZToSafePos();

                AKRSXtraMessageBox.Show($"测试异常！{e.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.bondHeadController.MoveBondZToSafePos();
            }
        }

        /// <summary>
        /// 不同角度下读焊头力
        /// </summary>
        private void ForceTest4()
        {
            try
            {
                // 连接摩尔力设备
                if (!this.modbusService.ConnectManometer())
                {
                    return;
                }

                this.bondHeadController.MoveBondZToSafePos();

                double readTime, resetTime;

                while (true)
                {
                    // 获取力值数据
                    for (double angle = -360; angle <= 360; angle = angle + 5)
                    {
                        //this.ModbusService.ResetManometer();

                        this.bondHeadController.RotateAxisT(angle);

                        this.sp.Restart();

                        int[] beforeTouchForce = this.modbusService.ReadBondForce();

                        this.sp.Stop();
                        readTime = this.sp.ElapsedMilliseconds;

                        this.sp.Restart();

                        //this.bondHeadController.ResetBondhead();

                        this.sp.Stop();
                        resetTime = this.sp.ElapsedMilliseconds;

                        Thread.Sleep(200);

                        this.SaveForceVal(angle, beforeTouchForce[0]);

                        this.SaveResetForceTime(readTime, resetTime);

                        if (this.isStop)
                        {
                            this.Invoke(new Action(() => { this.LbTip.Visible = false; }));
                            // 点停止直接退出
                            return;
                        }

                        if (this.sp.Elapsed.TotalHours > (double)SpEclispTime.Value)
                        {
                            this.Invoke(new Action(() => { this.LbTip.Visible = false; }));
                            return;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                this.bondHeadController.MoveBondZToSafePos();

                AKRSXtraMessageBox.Show("测试异常！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.bondHeadController.MoveBondZToSafePos();
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="inputForce"></param>
        /// <param name="angle"></param>
        /// <param name="beforeTouchForce"></param>
        /// <param name="touchForce"></param>
        /// <param name="riseForce"></param>
        private void SaveForceVal(double inputForce, double angle, double beforeTouchForce, double pos, double touchBondheadForce,double touchForce, double riseForce, double forceSetTime)
        {
            try
            {
                lock (this)
                {
                    // 添加一个工作表
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    ExcelPackage excelPackage = new ExcelPackage(
                        new FileInfo(
                            @"D:\" + "焊头力控稳定性测试数据" + ".xlsx"));

                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                                   ? excelPackage.Workbook.Worksheets[0]
                                                   : excelPackage.Workbook.Worksheets.Add("DataSheet");

                    // 设置列宽
                    for (int i = 1; i <= 10; i++)
                    {
                        worksheet.Column(i).Width = 15;
                    }

                    // 添加标题行
                    if (worksheet.Dimension == null)
                    {
                        worksheet.Cells[1, 1].Value = "Time";

                        worksheet.Cells[1, 2].Value = "输入力";

                        worksheet.Cells[1, 3].Value = "角度";

                        worksheet.Cells[1, 4].Value = "未接触前力g";

                        worksheet.Cells[1, 5].Value = "接触时的Z轴坐标";

                        worksheet.Cells[1, 6].Value = "接触时焊头的力g";

                        worksheet.Cells[1, 7].Value = "接触时平台压力传感器的力g";

                        worksheet.Cells[1, 8].Value = "抬起后力g";

                        worksheet.Cells[1, 9].Value = "力控下压到力控抬起时间(ms)";
                    }

                    int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

                    worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

                    worksheet.Cells[lastUsedRow + 1, 2].Value = inputForce;

                    worksheet.Cells[lastUsedRow + 1, 3].Value = angle;

                    worksheet.Cells[lastUsedRow + 1, 4].Value = beforeTouchForce;


                    worksheet.Cells[lastUsedRow + 1, 5].Value = pos;

                    worksheet.Cells[lastUsedRow + 1, 6].Value = touchBondheadForce;
                    worksheet.Cells[lastUsedRow + 1, 7].Value = touchForce;

                    worksheet.Cells[lastUsedRow + 1, 8].Value = riseForce;

                    worksheet.Cells[lastUsedRow + 1, 9].Value = forceSetTime;

                    excelPackage.Save();
                }
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
                    SaveForceVal(inputForce, angle, beforeTouchForce, pos, touchBondheadForce, touchForce, riseForce, forceSetTime);
                }
            }
        }

        ///// <summary>
        ///// 保存
        ///// </summary>
        ///// <param name="inputForce"></param>
        ///// <param name="angle"></param>
        ///// <param name="beforeTouchForce"></param>
        ///// <param name="touchForce"></param>
        ///// <param name="riseForce"></param>
        //private void SaveForceControlData(double inputForce, double angle, double beforeTouchForce, double pos, double touchBondheadForce, double touchForce, double riseForce, double forceSetTime,double forceReference, double touchBondheadForceBefore, double touchForceBefore, double posZBefore)
        //{
        //    try
        //    {
        //        lock (this)
        //        {
        //            // 添加一个工作表
        //            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        //            ExcelPackage excelPackage = new ExcelPackage(
        //                new FileInfo(
        //                    @"D:\" + "焊头力控稳定性测试数据" + ".xlsx"));

        //            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
        //                                           ? excelPackage.Workbook.Worksheets[0]
        //                                           : excelPackage.Workbook.Worksheets.Add("DataSheet");

        //            // 设置列宽
        //            for (int i = 1; i <= 10; i++)
        //            {
        //                worksheet.Column(i).Width = 15;
        //            }

        //            // 添加标题行
        //            if (worksheet.Dimension == null)
        //            {
        //                worksheet.Cells[1, 1].Value = "Time";

        //                worksheet.Cells[1, 2].Value = "输入力";

        //                worksheet.Cells[1, 3].Value = "角度";

        //                worksheet.Cells[1, 4].Value = "未接触前力g";

        //                worksheet.Cells[1, 5].Value = "延时后的Z轴坐标";

        //                worksheet.Cells[1, 6].Value = "延时后的LVDT值/焊头力";

        //                worksheet.Cells[1, 7].Value = "延时后平台压力传感器的力g";

        //                worksheet.Cells[1, 8].Value = "抬起后力g";

        //                worksheet.Cells[1, 9].Value = "力控下压到力控抬起时间(ms)";

        //                worksheet.Cells[1, 10].Value = "模拟量输入值";

        //                worksheet.Cells[1, 11].Value = "延时前LVDT/焊头力";

        //                worksheet.Cells[1, 12].Value = "延时前平台压力传感器的力g";

        //                worksheet.Cells[1, 13].Value = "延时前的Z轴坐标";
        //            }

        //            int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

        //            worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

        //            worksheet.Cells[lastUsedRow + 1, 2].Value = inputForce;

        //            worksheet.Cells[lastUsedRow + 1, 3].Value = angle;

        //            worksheet.Cells[lastUsedRow + 1, 4].Value = beforeTouchForce;


        //            worksheet.Cells[lastUsedRow + 1, 5].Value = pos;

        //            worksheet.Cells[lastUsedRow + 1, 6].Value = touchBondheadForce;
        //            worksheet.Cells[lastUsedRow + 1, 7].Value = touchForce;

        //            worksheet.Cells[lastUsedRow + 1, 8].Value = riseForce;

        //            worksheet.Cells[lastUsedRow + 1, 9].Value = forceSetTime;

        //            worksheet.Cells[lastUsedRow + 1, 10].Value = forceReference;

        //            worksheet.Cells[lastUsedRow + 1, 11].Value = touchBondheadForceBefore;

        //            worksheet.Cells[lastUsedRow + 1, 12].Value = touchForceBefore;

        //            worksheet.Cells[lastUsedRow + 1, 13].Value = posZBefore;

        //            excelPackage.Save();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        DialogResult dialog = AKRSXtraMessageBox.Show(
        //            $"文件已被打开，保存定位数据失败!\r\n Retry:关掉文件重新保存\r\nCancel:不保存数据",
        //            "报警",
        //            MessageBoxButtons.RetryCancel,
        //            MessageBoxIcon.Warning);

        //        if (dialog == DialogResult.Retry)
        //        {
        //            SaveForceControlData(inputForce, angle, beforeTouchForce, pos, touchBondheadForce, touchForce, riseForce, forceSetTime,forceReference,  touchBondheadForceBefore,  touchForceBefore, posZBefore);
        //        }
        //    }
        //}

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="inputForce"></param>
        /// <param name="angle"></param>
        /// <param name="beforeTouchForce"></param>
        /// <param name="touchForce"></param>
        /// <param name="riseForce"></param>
        private void SaveForceControlData(List<(DateTime dateTime, double targetForce, double angle, double beforeTouchForce, double
                                              posZAfter, double touchBondheadForce, double touchForce, double riseForce, double time,
                                              double forceReference, double touchBondheadForceBefore, double touchForceBefore, double
                                              posZBefore)> source)
        {
            try
            {
                lock (this)
                {
                    // 添加一个工作表
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    ExcelPackage excelPackage = new ExcelPackage(
                        new FileInfo(
                            @"D:\" + "焊头力控稳定性测试数据" + ".xlsx"));

                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                                   ? excelPackage.Workbook.Worksheets[0]
                                                   : excelPackage.Workbook.Worksheets.Add("DataSheet");

                    // 设置列宽
                    for (int i = 1; i <= 10; i++)
                    {
                        worksheet.Column(i).Width = 15;
                    }

                    // 添加标题行
                    if (worksheet.Dimension == null)
                    {
                        worksheet.Cells[1, 1].Value = "时间";

                        worksheet.Cells[1, 2].Value = "目标力";

                        worksheet.Cells[1, 3].Value = "角度";

                        worksheet.Cells[1, 4].Value = "未接触前力g";

                        worksheet.Cells[1, 5].Value = "延时后的Z轴坐标";

                        worksheet.Cells[1, 6].Value = "延时后的LVDT值/焊头力";

                        worksheet.Cells[1, 7].Value = "延时后平台压力传感器的力g";

                        worksheet.Cells[1, 8].Value = "抬起后力g";

                        worksheet.Cells[1, 9].Value = "力控下压到力控抬起时间(ms)";

                        worksheet.Cells[1, 10].Value = "模拟量输入值";

                        worksheet.Cells[1, 11].Value = "延时前LVDT/焊头力";

                        worksheet.Cells[1, 12].Value = "延时前平台压力传感器的力g";

                        worksheet.Cells[1, 13].Value = "延时前的Z轴坐标";
                    }

                    int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

                    // 写入数据
                    for (int i = 0; i < source.Count; i++)
                    {
                        int row = lastUsedRow + 1;

                        worksheet.Cells[row + i, 1].Value = source[i].dateTime.ToString("MM-dd HH:mm:ss");

                        worksheet.Cells[row + i, 2].Value = source[i].targetForce;

                        worksheet.Cells[row + i, 3].Value = source[i].angle;

                        worksheet.Cells[row + i, 4].Value = source[i].beforeTouchForce;


                        worksheet.Cells[row + i, 5].Value = source[i].posZAfter;

                        worksheet.Cells[row + i, 6].Value = source[i].touchBondheadForce;
                        worksheet.Cells[row + i, 7].Value = source[i].touchForce;

                        worksheet.Cells[row + i, 8].Value = source[i].riseForce;

                        worksheet.Cells[row + i, 9].Value = source[i].time;
                        worksheet.Cells[row + i, 10].Value = source[i].forceReference;
                        worksheet.Cells[row + i, 11].Value = source[i].touchBondheadForceBefore;
                        worksheet.Cells[row + i, 12].Value = source[i].touchForceBefore;
                   
                        worksheet.Cells[row + i, 13].Value = source[i].posZBefore;
                    }

                    excelPackage.Save();
                }
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
                    SaveForceControlData(source);
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="inputForce"></param>
        /// <param name="angle"></param>
        /// <param name="beforeTouchForce"></param>
        /// <param name="touchForce"></param>
        /// <param name="riseForce"></param>
        private void SaveForceVal(double angle, double force)
        {
            try
            {
                lock (this)
                {
                    // 添加一个工作表
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                    ExcelPackage excelPackage = new ExcelPackage(
                        new FileInfo(
                            @"D:\" + "焊头稳定性测试数据" + DateTime.Now.ToString("yyMMdd")
                            + ".xlsx"));

                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                                   ? excelPackage.Workbook.Worksheets[0]
                                                   : excelPackage.Workbook.Worksheets.Add("DataSheet");

                    // 设置列宽
                    for (int i = 1; i <= 10; i++)
                    {
                        worksheet.Column(i).Width = 15;
                    }

                    // 添加标题行
                    if (worksheet.Dimension == null)
                    {
                        worksheet.Cells[1, 1].Value = "Time";

                        worksheet.Cells[1, 2].Value = "角度";

                        worksheet.Cells[1, 3].Value = "力值g";
                    }

                    int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

                    worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

                    worksheet.Cells[lastUsedRow + 1, 2].Value = angle;

                    worksheet.Cells[lastUsedRow + 1, 3].Value = force / 10.0;

                    excelPackage.Save();
                }
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
                    SaveForceVal(angle, force);
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="inputForce"></param>
        /// <param name="angle"></param>
        /// <param name="beforeTouchForce"></param>
        /// <param name="touchForce"></param>
        /// <param name="riseForce"></param>
        private void SaveResetForceTime(double readTime, double resetTime)
        {
            try
            {
                lock (this)
                {
                    // 添加一个工作表
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    ExcelPackage excelPackage = new ExcelPackage(
                        new FileInfo(
                            @"D:\" + "焊头稳定性测试4数据" + DateTime.Now.ToString("yyMMdd")
                            + ".xlsx"));

                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                                   ? excelPackage.Workbook.Worksheets[0]
                                                   : excelPackage.Workbook.Worksheets.Add("DataSheet");

                    // 设置列宽
                    for (int i = 1; i <= 10; i++)
                    {
                        worksheet.Column(i).Width = 15;
                    }

                    // 添加标题行
                    if (worksheet.Dimension == null)
                    {
                        worksheet.Cells[1, 1].Value = "Time";

                        worksheet.Cells[1, 2].Value = "读取时间";

                        worksheet.Cells[1, 3].Value = "清零时间";
                    }

                    int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

                    worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

                    worksheet.Cells[lastUsedRow + 1, 2].Value = readTime;

                    worksheet.Cells[lastUsedRow + 1, 3].Value = resetTime;

                    excelPackage.Save();
                }
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
                    SaveForceVal(readTime, resetTime);
                }
            }
        }

        private void BtnStart2_Click(object sender, EventArgs e)
        {
            this.Save();
            if (this.task2 == null || this.task2.IsCompleted == true)
            {
                LbTip.Visible = true;

                // 开始采集
                this.isStop = false;
                this.BtnStart2.Text = @"Stop";
                this.task2 = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("力控测试线程");
                            sp.Restart();
                            this.ForceTest2();
                        });

                this.task2.ContinueWith(t =>
                    {
                        if (this.BtnStart.InvokeRequired)
                        {
                            this.BtnStart.Invoke(
                                new Action(
                                    () =>
                                        {
                                            this.BtnStart2.Text = @"Start";
                                            LbTip.Visible = false;
                                        }));
                        }
                        else
                        {
                            this.BtnStart2.Text = @"Start";
                            LbTip.Visible = false;
                        }
                    });
            }
            else
            {
                // 暂停采集
                this.isStop = true;
                this.BtnStart2.Text = @"Start";
                this.sp.Stop();

                LbTip.Visible = false;
            }
        }

        private void BtnStart3_Click(object sender, EventArgs e)
        {
            this.Save();
            if (this.task3 == null || this.task3.IsCompleted == true)
            {
                LbTip.Visible = true;

                // 开始采集
                this.isStop = false;
                this.BtnStart3.Text = @"Stop";
                this.task3 = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("力控测试线程");
                            sp.Restart();
                            this.ForceTest3();
                        });

                this.task3.ContinueWith(t =>
                    {
                        if (this.BtnStart.InvokeRequired)
                        {
                            this.BtnStart3.Invoke(
                                new Action(
                                    () =>
                                        {
                                            this.BtnStart3.Text = @"Start";
                                            LbTip.Visible = false;
                                        }));
                        }
                        else
                        {
                            this.BtnStart3.Text = @"Start";
                            LbTip.Visible = false;
                        }
                    });
            }
            else
            {
                // 暂停采集
                this.isStop = true;
                this.BtnStart3.Text = @"Start";
                this.sp.Stop();

                LbTip.Visible = false;
            }
        }

        private void BtnStart4_Click(object sender, EventArgs e)
        {
            this.Save();
            if (this.task4 == null || this.task4.IsCompleted == true)
            {
                LbTip.Visible = true;

                // 开始采集
                this.isStop = false;
                this.BtnStart4.Text = @"Stop";
                this.task4 = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("力控测试线程");
                            sp.Restart();
                            this.ForceTest4();
                        });

                this.task4.ContinueWith(t =>
                    {
                        if (this.BtnStart4.InvokeRequired)
                        {
                            this.BtnStart4.Invoke(
                                new Action(
                                    () =>
                                        {
                                            this.BtnStart4.Text = @"Start";
                                            LbTip.Visible = false;
                                        }));
                        }
                        else
                        {
                            this.BtnStart4.Text = @"Start";
                            LbTip.Visible = false;
                        }
                    });
            }
            else
            {
                // 暂停采集
                this.isStop = true;
                this.BtnStart4.Text = @"Start";
                this.sp.Stop();

                LbTip.Visible = false;
            }
        }
    }
}