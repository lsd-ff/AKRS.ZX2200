using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.PR.Controls;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Utils;
using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.SupportFeature.MotionPlan;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using DevExpress.XtraEditors;
using GTN;
using LanguageExt;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Forms;

namespace AKRS.ZX2200.Experiment.SpeedImprovementExperiment
{
    /// <summary>
    /// 速度提升实验
    /// </summary>
    public partial class FrmSpeedImprovementExperiment : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 速度提升实验
        /// </summary>
        public FrmSpeedImprovementExperiment()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 取片
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtPickUp_Click(object sender, EventArgs e)
        {
            List<AKRSPoint3D> list =MotionPlanDomain.GetInstance().GetMoveLine(new AKRSPoint3D(29.7568000000001, -177.897075, 120), new AKRSPoint3D(29.7568000000001, -311.93955, 53.4002));

            System2Domain.GetInstance().BondModuleController.MoveToG0Pos(new AKRSPoint3D(29.7568000000001, -177.897075, 120));

            Thread.Sleep(2000);

            AKRSPoint4D pickupPos = new AKRSPoint4D(
                CalibrateRunPara.GetInstance().BondHeadRotateCenterInWC.X,
                CalibrateRunPara.GetInstance().BondHeadRotateCenterInWC.Y,
                0,
                0);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            InterpolationParam interpolationParam = new InterpolationParam();

            // 全局速度百分比
            double vel = /*BondDevicePara.GetInstance().ULMPara.JumpToPickupPosSpeed*/2000
                         * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            interpolationParam.ListNo = 2;
            interpolationParam.GrpCrd = 2;
            interpolationParam.Vel = vel;
            interpolationParam.Acc = BondDevicePara.GetInstance().ULMPara.JumpToPickupPosAccelerationTime;
            interpolationParam.AccAcc = BondDevicePara.GetInstance().ULMPara.JumpToPickupPosJerkTime;

            interpolationParam.AxisDrives = System2Domain.GetInstance().BondModuleController.GetBondIpolAxis();

            // 低于安全高度就先抬到安全高度
            if (true)
            {
                interpolationParam.SegmentConfigs = new SegmentConfig[list.Count + 1];

                AKRSPoint3D point3D = new AKRSPoint3D(29.7568000000001, -177.897075, 120);

                point3D = System2Domain.GetInstance().BondModuleController.ConvertG0ToMachinePos(point3D);

                // 第一段，Z上抬到安全高度
                interpolationParam.SegmentConfigs[0] = new SegmentConfig()
                {
                    Point = new AKRSPoint4D()
                    {
                        X = point3D.X,
                        Y = point3D.Y,
                        Z = point3D.Z,
                        T = 0
                    }
                };

                for (int i = 0; i < list.Count; i++)
                {
                    AKRSPoint3D pos = System2Domain.GetInstance().BondModuleController.ConvertG0ToMachinePos(list[i]);

                    SegmentConfig segment = new SegmentConfig() { Point = new AKRSPoint4D(pos.X, pos.Y, pos.Z, 0) };

                    interpolationParam.SegmentConfigs[i + 1] = segment;
                }

                //// XY运动到轨道外延(-150) 且T 运动到取料角度的 1 / 2 Z不动
                //interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D() { X = pickupPos.X, Y = BondDevicePara.GetInstance().ULMPara.TransportUnitEdgePos.Y, Z = -20, T = 0 };

                //// XYT运动到取料位置
                //interpolationParam.SegmentConfigs[2].Point = new AKRSPoint4D() { X = pickupPos.X, Y = BondDevicePara.GetInstance().ULMPara.TransportUnitEdgePos.Y, Z = -80, T = 0 };
            }
            else
            {
                interpolationParam.SegmentConfigs = new SegmentConfig[2] { new SegmentConfig(), new SegmentConfig() };

                // XY运动到轨道外延(-150) 且T 运动到取料角度的 1 / 2 Z不动
                interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = pickupPos.X, Y = BondDevicePara.GetInstance().ULMPara.TransportUnitEdgePos.Y, Z = 0, T = 0 };

                // XYT运动到取料位置
                interpolationParam.SegmentConfigs[1].Point = pickupPos;
            }

            interpolationParam.AheadParam = new AheadParam()
            {
                Time = BondDevicePara.GetInstance().ULMPara.JumpToPickupPosTime,
                RadiusRatio = BondDevicePara.GetInstance().ULMPara.JumpToPickupPosRadiusRatio
            };

            foreach (var segmentConfig in interpolationParam.SegmentConfigs)
            {
                // 加减速度默认*10
                segmentConfig.Velocity = vel;
                segmentConfig.Acc = vel * 10.0;
                segmentConfig.Dec = vel * 10.0;
            }

            // 获取卡
            //AxisCard card = HardwareRepositoryService.GetHardwaresByType<AxisCard>().Find(
            //    card =>
            //    card.AxisList.Select(axis => axis.AxisDrive).Exists(
            //        drive => drive == interpolationParam.AxisDrives[0]));

            //System2RunTimeProvider.RecordTime("PickAction", $"Jump到取片位开始");

            //card.MotionController.ContinueInterpolationMove(interpolationParam);

            GuGaoDrive.PickContinueInterpolationMove(System2Domain.GetInstance().BondModuleController.GetBondIpolAxis(), interpolationParam.SegmentConfigs);

            stopwatch.Stop();

            AKRSXtraMessageBox.Show($"{stopwatch.ElapsedMilliseconds}");
        }

        /// <summary>
        /// 移动到上视
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtMoveToUpLook_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            List<List<double>> doubles = new List<List<double>>();

            BondModule bondModule = new BondModule();

            for (int i = 0; i < 100; i++)
            {
                bondModule.BondAxisX.AbsoluteMove(0);
                bondModule.BondAxisX.AbsoluteMove(250);

                bondModule.BondAxisY.AbsoluteMove(0);
                bondModule.BondAxisY.AbsoluteMove(-180);

                List<double> list = new List<double>();

                // 温漂定位
                MatchResult driftMatchResult = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                    new AKRSPoint3D(40.9487499999999, -204.455075, 112.2508),
                    "图案DownLookMatch1",
                    "图案DownLookMatch1");
                list.Add(driftMatchResult.CenterX);
                list.Add(driftMatchResult.CenterY);
                list.Add(driftMatchResult.Angle);
                doubles.Add(list);
            }

            // 数据处理
            string path = FileHelper.CreateFileByDate("精度基础数据测试");

            FileHelper.SaveDoubleExcel(doubles, Path.Combine(path, DateTime.Now.ToFileTime().ToString() + ".xlsx"));

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                List<List<double>> doubles = new List<List<double>>();
                for (int k = 0; k < 10000; k++)
                {
                    List<double> list = new List<double>();
                    AKRSPoint3D visionPos = System2Domain.GetInstance().BondModuleController.ConvertMachineToG0Pos(BondDevicePara.GetInstance().BondHeadParam.UpLookMarkVisionPos1);

                    // 温漂定位
                    MatchResult driftMatchResult = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                        visionPos,
                        "温漂定位",
                        "上视温漂测试Mark");

                    if (driftMatchResult != null)
                    {
                        list.Add(driftMatchResult.CenterX);
                        list.Add(driftMatchResult.CenterY);
                    }

                    Thread.Sleep(500);

                    AKRSPoint3D visionPos2 = System2Domain.GetInstance().BondModuleController
                           .ConvertMachineToG0Pos(BondDevicePara.GetInstance().BondHeadParam.UpLookMarkVisionPos2);

                    // 温漂定位
                    MatchResult driftMatchResult2 = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                        visionPos2,
                        "温漂定位",
                        "上视温漂测试Mark2", true);

                    if (driftMatchResult2 != null)
                    {
                        list.Add(driftMatchResult2.CenterX);
                        list.Add(driftMatchResult2.CenterY);

                    }

                    Thread.Sleep(500);

                    // 温漂定位
                    MatchResult bmc = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                        new AKRSPoint3D(174.75915, -296.244425, 112.8909),
                        "大标定片模板",
                        "大标定片模板");

                    if (bmc != null)
                    {
                        list.Add(bmc.CenterX);
                        list.Add(bmc.CenterY);
                    }

                    Thread.Sleep(500);

                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 15; j++)
                        {
                            // 温漂定位
                            MatchResult mark = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                                new AKRSPoint3D(-89.16635 + j * 5.3 * 3 , -234.112525 + i * 5.3 * 3, 113.211),
                                "测试",
                                "测试");

                            if (mark != null)
                            {
                                list.Add(mark.CenterX);
                                list.Add(mark.CenterY);
                            }
                        }
                    }

                    doubles.Add(list);

                    // 数据处理
                    string path = FileHelper.CreateFileByDate("温漂测试");

                    FileHelper.SaveDoubleExcel(doubles, Path.Combine(path, DateTime.Now.ToFileTime().ToString() + ".xlsx"));

                    // 来回跑
                    DateTime dateTime = DateTime.Now;

                    BondModule bondModule = new BondModule();

                    while ((DateTime.Now - dateTime).TotalMinutes <= 5)
                    {
                        System2Domain.GetInstance().BondModuleController.MoveToG0Pos(new AKRSPoint3D(-102.33835, -227.366525, 124.3956));
                        Thread.Sleep(100);
                        System2Domain.GetInstance().BondModuleController.MoveToG0Pos(new AKRSPoint3D(185.05175, -227.366525, 124.3956));
                        Thread.Sleep(100);
                    }
                }
            });
           
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //Task.Run(() => {
            //    PRResultShowModel pRResultShowModel = new PRResultShowModel();
            //    pRResultShowModel.CameraName = "BOND相机";
            //    pRResultShowModel.PRImage = new Bitmap("C:\\Users\\liujiangxian\\Desktop\\20260104151141.jpg");
            //    FrmVmVisionResult.PrResultImageBlock.Post(pRResultShowModel);

            //    Thread.Sleep(3000);
            //    PRResultShowModel pRResultShowModel2 = new PRResultShowModel();
            //    pRResultShowModel2.CameraName = "BOND相机";
            //    pRResultShowModel2.PRImage = new Bitmap("C:\\Users\\liujiangxian\\Desktop\\20399fc3db4f4f5fa81c784bdd6ea493.png");
            //    FrmVmVisionResult.PrResultImageBlock.Post(pRResultShowModel2);
            //});

            Task.Run(() => { XtraMessageBox.Show("你好非"); });

            //Task.Run(() => { AKRSAKRSXtraMessageBox.Show("你好"); });
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            System1Domain.GetInstance().DispenseController.MoveToG0Pos3D(new AKRSPoint3D(121.0601, -314.7913, 86.3221));
            System2Domain.GetInstance().BondModuleController.MoveToG0Pos(new AKRSPoint3D(-152.391, -282.063475, 123.0883));

            //Task.Run(() =>
            //{
            //    CommonUtil.SetCurrentThreadName("点胶线程");

            //    System1Domain.GetInstance().DispenseController.MoveToG0Pos3D(new AKRSPoint3D(121.0601, -314.7913, 86.3221));

            //    for (int i = 0; i < 1000000; i++)
            //    {
            //        MatchResult matchResult = (MatchResult)System1Domain.GetInstance().DispenseVisionController.DispenseVision(null, "20251211ModuleMark1");

            //        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} --" + matchResult.CenterX);

            //        Random random = new Random();
            //        Thread.Sleep(random.Next(500));

            //        if (Math.Abs(matchResult.CenterX - 1315) > 10)
            //        {
            //            AKRSXtraMessageBox.Show("点胶");
            //        }

            //    }

            //});

            //Task.Run(() =>
            //{
            //    CommonUtil.SetCurrentThreadName("固精线程");

            //    System2Domain.GetInstance().BondModuleController.MoveToG0Pos(new AKRSPoint3D(-152.391, -282.063475, 123.0883));

            //    for (int i = 0; i < 1000000; i++)
            //    {
            //        MatchResult matchResult = (MatchResult)System2Domain.GetInstance().System2Controller.BondCameraVision(null, "20251211ModuleMark1");

            //        Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} --" + matchResult.CenterX);

            //        Random random = new Random();
            //        Thread.Sleep(random.Next(500));

            //        if (Math.Abs(matchResult.CenterX - 1251) > 10)
            //        {
            //            AKRSXtraMessageBox.Show("Bond");
            //        }


            //    }

            //});

            Task.Run(() => 
            {
                PREntity pREntityTemp = (PREntity)VisionEntityRepository.GetInstance().Find("20251211ModuleMark1");

                (DialogResult dialog, BaseAlgResult matchResult) result = UcMainSystem.VisionAlarmLockFunc(
                           pREntityTemp,
                           "测试",
                           $"视觉模板：固晶{pREntityTemp.GetName()}定位失败", System1Domain.GetInstance().DispenseVisionController.GetHardware());
            });

        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {

        }
    }
}