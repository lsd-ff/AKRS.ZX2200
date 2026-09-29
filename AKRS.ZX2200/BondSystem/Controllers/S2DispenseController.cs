using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.LogicHardware.Services;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
using AKRS.ZX2200.DispenseSystem.Models.Enums;
using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
using AKRS.ZX2200.DispenseSystem.Services;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AKRS.ZX2200.BondSystem.Controllers
{
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.BondForce.Modbus;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.ActionNodes.Commons;
    using AKRS.ZX2200.BondSystem.Models.ActionNodes.DispenseAction;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using LanguageExt.ClassInstances.Const;
    using System.Windows.Forms;

    /// <summary>
    /// Bond上面的点胶头
    /// </summary>
    public class S2DispenseController
    {
        /// <summary>
        /// 邦头模组控制器
        /// </summary>
        private BondModuleController BondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 邦头模组
        /// </summary>
        private BondModule BondModule => System2Module.GetInstance().BondModule;

        /// <summary>
        /// 焊头和相机之间的距离
        /// </summary>
        private AKRSPoint3D DistanceForBondHandAndCamera => CalibrateRunPara.GetInstance().BondRotateCenterToCamOffset;

        /// <summary>
        /// 焊头和点胶针之间的距离
        /// </summary>
        public AKRSPoint3D DistanceForBondHandAndDispenser =>
            this.DistanceForBondHandAndCamera + this.DistanceForCameraAndDispenser;

        /// <summary>
        /// 焊头和测高传感器之间的距离
        /// </summary>
        private AKRSPoint3D DistanceBondHandAndForMh => BondDevicePara.GetInstance().S2DispenseDevicePara.DistanceByMhToBh;
        
        /// <summary>
        /// 点胶头和相机之间的距离
        /// </summary>
        private AKRSPoint3D DistanceForCameraAndDispenser => BondProgram.GetInstance().S2DispenserProgram.Dispenser?.DispensingNeedleOffset;

        /// <summary>
        /// 测高针和相机之间的距离
        /// </summary>
        private AKRSPoint3D DistanceForCameraAndMh => this.DistanceBondHandAndForMh - this.DistanceForBondHandAndCamera;

        /// <summary>
        /// 点胶头和相机之间的距离
        /// </summary>
        private AKRSPoint3D DistanceForDispenserAndMh =>
            this.DistanceBondHandAndForMh - this.DistanceForCameraAndDispenser;

        /// <summary>
        /// 点胶头移动到Go位置
        /// </summary>
        /// <param name="point3DInGo">G0中的点位</param>
        public void DispenserMoveToG0Pos(AKRSPoint3D point3DInGo)
        {
            AKRSPoint3D point = point3DInGo + this.DistanceForBondHandAndDispenser;
            this.BondModuleController.MoveToG0Pos(point);
        }

        /// <summary>
        /// 添加焊头和点胶头之间的差值
        /// </summary>
        /// <param name="point3DInGo">位置</param>
        /// <returns>结果</returns>
        public AKRSPoint3D GetDispenserPosByBondHand(AKRSPoint3D point3DInGo)
        {
            return point3DInGo - this.DistanceForBondHandAndDispenser;
        }

        /// <summary>
        /// 测高针移动到Go位置
        /// </summary>
        /// <param name="point3DInGo">G0中的点位</param>
        public void MeasureHeightMoveToG0Pos(AKRSPoint3D point3DInGo)
        {
            AKRSPoint3D point = point3DInGo + this.DistanceBondHandAndForMh;

            // 这个移动是不安全的，请确保高度正确
            this.BondModuleController.MoveToG0PosWithoutSafe(point);
        }

        /// <summary>
        /// 相机移动到Go位置
        /// </summary>
        /// <param name="point3DInGo">G0中的点位</param>
        public void VisionMoveToG0Pos(AKRSPoint3D point3DInGo)
        {
            AKRSPoint3D point = point3DInGo - this.DistanceForBondHandAndCamera;
            this.BondModuleController.MoveToG0Pos(point);
        }

        /// <summary>
        /// 焊头移动到相机所看到的位置
        /// </summary>
        /// <param name="point3DInGo">G0中的点位</param>
        public void BondHandMoveToVisionPos(AKRSPoint3D point3DInGo)
        {
            AKRSPoint3D point = point3DInGo + this.DistanceForBondHandAndCamera;
            this.BondModuleController.MoveToG0Pos(point);
        }

        /// <summary>
        /// 激光测高
        /// </summary>
        /// <returns>结果</returns>
        public double LaserMeasureHeight()
        {
           return this.BondModule.LaserMeasureHeight.GetValue();
        }

        /// <summary>
        /// 激光测高
        /// </summary>
        /// <returns>结果</returns>
        public double LaserMeasureHeightInGo()
        {
            double height = this.BondModule.LaserMeasureHeight.GetValue();

            return this.BondModule.GetG0RealPosition().Z - this.DistanceBondHandAndForMh.Z - height;
        }

        /// <summary>
        /// 压力测高
        /// </summary>
        /// <returns>结果</returns>
        public double ForceMeasureHeight()
        {
            // TODO 力传感值清空
            ModbusService.GetInstance().ConnectManometer();
            // 发送Z轴往下移动指令
            double pos = BondModule.BondHead.AxisZ.GetRealPosition();

            MovePara movePara = new MovePara()
                                    {
                                        Vel = 1,
                                        Dec = 10,
                                        Acc = 10,
                                        Jerk = 0.02,
                                        TargetPosition = pos - 5
                                    };

            this.BondModule.BondHead.AxisZ.SendAbsoluteMoveCommand(movePara);

            // 压力值到了轴停止移动
            while (true)
            {
                if (ModbusService.GetInstance().ReadManometer()[0] / 10.0 > 10)
                {
                    this.BondModule.BondHead.AxisZ.StopMove();
                    break;
                }

                if (this.BondModule.BondHead.AxisZ.IsInRealPosition())
                {
                    throw new Exception("压力测高失败");
                }
            }

            // 精调
            for (int i = 0; i < 100; i++)
            {
                if (ModbusService.GetInstance().ReadManometer()[0] / 10.0 >= 20)
                {
                    this.BondModule.BondHead.AxisZ.RelativeMove(0.003);
                }
                else if (ModbusService.GetInstance().ReadManometer()[0] / 10.0 < 20)
                {
                    this.BondModule.BondHead.AxisZ.RelativeMove(-0.003);
                }
            }

            return this.BondModule.BondHead.AxisZ.GetRealPosition();
        }

        /// <summary>
        /// 执行点胶、画胶动作、双轴插补，目前值支持双轴插补
        /// 只能画连续的曲线
        /// </summary>
        /// <param name="epoxyApplication">图形</param>
        /// <param name="point">点胶点</param>
        /// <param name="isDrip">是否出胶</param>
        /// <param name="isPrePlant">是否在预点胶板上出胶</param>
        /// <param name="isDispenseDelay">是否多停留一段时间</param>
        /// <param name="angle">角度</param>
        /// <returns>是否成功</returns>
        public ExcuteResult InterpolationApplication(
            EpoxyApplication epoxyApplication,
            AKRSPoint3D point,
            bool isDrip = true,
            bool isPrePlant = false,
            bool isDispenseDelay = false,
            double angle = 0)
        {
            try
            {
                System2RunTimeProvider.RecordTime("系统2点胶流程", "准备开始点胶");
                
                #region 数据准备

                // 将G0坐标转换到机械坐标
                AKRSPoint3D interpolationPos = this.BondModule.ConvertG0ToMachinePos(point);

                bool isNozzle = epoxyApplication.EpoxyApplicationStrategy == EpoxyApplicationTypeEnum.Printting;

                Nozzle nozzle = System2Domain.GetInstance().BondHeadController.GetCurrentNozzle();
                
                if (isNozzle)
                {
                    // 加上吸嘴的偏移
                    AKRSPoint2D nozzleOffsetRotated =
                        System2Domain.GetInstance().BondHeadController.GetNozzleOffset(nozzle.Name, angle + nozzle.AlignAngle - nozzle.AlignAngle);
                    AKRSPoint3D offset = new AKRSPoint3D(nozzleOffsetRotated.X, nozzleOffsetRotated.Y, nozzle.MeasureHeightOffset);

                    interpolationPos = interpolationPos + offset;
                }
                else
                {
                    // 将位置转换到点胶头
                    interpolationPos = this.GetDispenserPosByBondHand(interpolationPos);
                }

                // 数据处理，归一化
                epoxyApplication = DispenserMoveHelper.EpoxyPretreatment(epoxyApplication, interpolationPos, angle);

                #endregion

                #region 高度补偿

                // 高度补偿
                if (isPrePlant)
                {
                    interpolationPos.Z += epoxyApplication.PrePlantOffSetZ;
                }
                else
                {
                    interpolationPos.Z += epoxyApplication.OffsetZ;
                }

                // 移动到准备开始点胶的高度
                double preDispenseHeight = interpolationPos.Z + epoxyApplication.SecurityHeightForEpoxyApplication;

                #endregion

                for (int i = 0; i < epoxyApplication.DispensePatternParas.Count; i++)
                {
                    this.PrintEpoxy(epoxyApplication);

                    #region 移动到第一个点胶点并异步设置胶压

                    if (i == 0 || isNozzle)
                    {
                        // 将这个位置告诉点胶头
                        AKRSPoint3D preDispensePos =
                            new AKRSPoint3D(
                                epoxyApplication.DispensePatternParas[i][0].X,
                                epoxyApplication.DispensePatternParas[i][0].Y,
                                preDispenseHeight);

                        System2RunTimeProvider.RecordTime("系统2点胶流程", "开始设置胶压");

                        // 异步设置胶压
                        Task task = new Task(
                            () =>
                            {
                                if (isNozzle)
                                {
                                    return;
                                }

                                this.SetDispensePressure(
                                    (int)epoxyApplication.DispensePressure,
                                    (int)epoxyApplication.Vacuum);
                            });

                        task.Start();

                        // 单步工作
                        if (!System2Domain.GetInstance().WaitSingleStep())
                        {
                            return ExcuteResult.Alarm;
                        }

                        if (isNozzle)
                        {
                            AKRSPoint4D point4D = new AKRSPoint4D(preDispensePos.X, preDispensePos.Y, preDispensePos.Z, nozzle.AlignAngle + angle);
                            this.BondModuleController.MoveSafeBondXYZT(point4D);
                        }
                        else if (isPrePlant)
                        {
                            // 移动到最开始的位置
                            this.BondModuleController.MoveToG0Pos(this.BondModule.ConvertMachineToG0Pos(preDispensePos));
                        }
                        else
                        {
                            // 移动到最开始的位置
                            this.BondModuleController.MoveToG0PosWithoutSafe(this.BondModule.ConvertMachineToG0Pos(preDispensePos));
                        }

                        if (!isNozzle)
                        {
                            // 弹出点胶针
                            System2Domain.GetInstance().S2DispenseController.OpenDispenseHeightMeasurementCylinder();
                        }

                        System2RunTimeProvider.RecordTime("系统2点胶流程", "移动到最开始的位置完成");

                        task.Wait();

                        System2RunTimeProvider.RecordTime("系统2点胶流程", "设置胶压完成");
                    }

                    #endregion

                    #region 移动到点胶位置

                    // 移动到开始点胶的位置，第一次因为已经移动过了，不需要移动
                    else
                    {
                        this.BondModule.BondHead.AxisZ.AbsoluteMove(preDispenseHeight);

                        System2RunTimeProvider.RecordTime("系统2点胶流程", "点胶Z轴移动到预点胶位置完成");

                        MotionService.MoveAxesToTargetPosition(
                            (this.BondModule.BondAxisX, false, epoxyApplication.DispensePatternParas[i][0].X,
                                AccuracyMode.HighAccuracy),
                            (this.BondModule.BondAxisY, false, epoxyApplication.DispensePatternParas[i][0].Y, AccuracyMode.HighAccuracy));

                        System2RunTimeProvider.RecordTime("系统2点胶流程", "点胶过程中 点胶XY轴插补移动到指令位置");

                        this.OpenDispenseHeightMeasurementCylinder();
                    }

                    #endregion

                    #region 提前开胶

                    // 移动参数，这个目前不知道需不需要考虑分辨率
                    MovePara movePara = new MovePara()
                    {
                        // 缓慢下降的速度
                        Vel = epoxyApplication.SlowTravelSpeedBeforeDispensing,

                        // 加速度
                        Acc = epoxyApplication.SlowTravelSpeedBeforeDispensing * 10,

                        // 减速度
                        Dec = epoxyApplication.SlowTravelSpeedBeforeDispensing * 10,

                        // 高度补偿
                        TargetPosition = interpolationPos.Z,
                    };

                    // 提前开胶
                    if (epoxyApplication.AdvanceOpenDistanceNew != 0 && !isNozzle)
                    {
                        // 单步工作
                        if (!System1Domain.GetInstance().WaitSingleStep())
                        {
                            return ExcuteResult.Alarm;
                        }

                        // 提前开胶，这个理论上是按时间去处理的，但是以前都是这样写的，先按照这个去写
                        this.BondModule.BondHead.AxisZ.SendAbsoluteMoveCommand(movePara);

                        System2RunTimeProvider.RecordTime("系统2点胶流程", "提前开胶-点胶Z轴开始缓慢下降");

                        while (true)
                        {
                            // 循环判断有没有到位
                            if (this.BondModule.BondHead.AxisZ.GetRealPosition()
                                <= interpolationPos.Z + epoxyApplication.AdvanceOpenDistanceNew)
                            {
                                this.BondModule.DripElectric.SetOutputValue(true);
                                break;
                            }

                            Thread.Sleep(1);
                        }

                        this.BondModule.BondHead.AxisZ.WaitForArrival();

                        System2RunTimeProvider.RecordTime("系统2点胶流程", "点胶Z轴开始缓慢下降到位");
                    }
                    else
                    {
                        this.BondModule.BondHead.AxisZ.AbsoluteMove(movePara);

                        Console.WriteLine($"点胶高度为 ：{movePara.TargetPosition}");
                    }

                    #endregion

                    #region 滞后运动

                    // 单步工作
                    if (!System1Domain.GetInstance().WaitSingleStep())
                    {
                        return ExcuteResult.Alarm;
                    }

                    // 判断是否需要到位等待
                    if (epoxyApplication.DispenserLeadTime > 0 && !isNozzle)
                    {
                        if (epoxyApplication.IsDispenserLeadTimeOpen)
                        {
                            this.BondModule.DripElectric.SetOutputValue(true);
                            System2RunTimeProvider.RecordTime("系统2点胶流程", "Z轴移动到位，开始出胶，等待结束");
                        }
                        else
                        {
                            this.BondModule.DripElectric.SetOutputValue(false);
                            System2RunTimeProvider.RecordTime("系统2点胶流程", "Z轴移动到位，不出胶，等待结束");
                        }

                        Thread.Sleep(epoxyApplication.DispenserLeadTime);

                        System2RunTimeProvider.RecordTime("系统2点胶流程", $"点胶前到位等待结束");
                    }

                    #endregion

                    // 如果画胶里面只有两个数且XY相等，则认为是点胶，不开启插补
                    if (epoxyApplication.DispensePatternParas[i].Length == 2
                        && epoxyApplication.DispensePatternParas[i][0].X
                            .CompareTo(epoxyApplication.DispensePatternParas[i][1].X) == 0
                        && epoxyApplication.DispensePatternParas[i][0].Y
                            .CompareTo(epoxyApplication.DispensePatternParas[i][1].Y) == 0
                        || epoxyApplication.EpoxyApplicationStrategy == EpoxyApplicationTypeEnum.SingleDotOnly
                        || isNozzle)
                    {
                        #region 点胶

                        System2RunTimeProvider.RecordTime("系统2点胶流程", $"开始单颗点胶");

                        // 单步工作
                        if (!System1Domain.GetInstance().WaitSingleStep())
                        {
                            return ExcuteResult.Alarm;
                        }

                        if (!isNozzle)
                        {
                            // 开启点胶阀
                            this.BondModule.DripElectric.SetOutputValue(isDrip);
                        }

                        // 保持一定时间,这个在画胶软件里面去设置
                        DelayHelper.Delay(epoxyApplication.DispensePatternParas[i][0].DripTime);

                        isDispenseDelay = false;

                        System2RunTimeProvider.RecordTime(
                            "系统1点胶流程",
                            $"点胶延时{epoxyApplication.DispensePatternParas[i][0].DripTime }");

                        if (!isNozzle)
                        {
                            // 点胶完成，关闭点胶阀IO
                            this.BondModule.DripElectric.SetOutputValue(false);
                        }

                        System2RunTimeProvider.RecordTime("系统1点胶流程", $"关闭点胶阀完成");

                        #endregion
                    }
                    else
                    {
                        #region 画胶

                        // 单步工作
                        if (!System1Domain.GetInstance().WaitSingleStep())
                        {
                            return ExcuteResult.Alarm;
                        }

                        if (this.BondModule.BondAxisX.AxisDrive is ETELAxis)
                        {
                            throw new Exception(" EtEL 轴 没有开发");
                        }
                        else
                        {
                            this.OpenDispensingElectric();

                            System2RunTimeProvider.RecordTime("系统2点胶流程", $"点胶开始插补");
                            GuGaoDrive.Interpolation(
                                2,
                                epoxyApplication.DispensePatternParas[i],
                                this.BondModule.GetDispenseIndex());

                            this.CloseDispensingElectric();

                            System2RunTimeProvider.RecordTime("系统2点胶流程", $"点胶结束插补");
                        }

                        #endregion

                    }

                    #region 一段断尾

                    if (epoxyApplication.TearOff1Open)
                    {
                        if (epoxyApplication.TearOff1Speed == 0)
                        {
                            throw new Exception("断尾高度不能为0");
                        }

                        // 移动参数，这个目前不知道需不需要考虑分辨率
                        MovePara tearOff1 = new MovePara()
                        {
                            Vel = epoxyApplication.TearOff1Speed,
                            Acc = epoxyApplication.TearOff1Speed * 10.0,
                            Dec = epoxyApplication.TearOff1Speed * 10.0,
                            TargetPosition = interpolationPos.Z + epoxyApplication.TearOff1Height,
                        };

                        System2RunTimeProvider.RecordTime("系统1点胶流程", $"一段断尾开始，速度{tearOff1.Vel}，高度{epoxyApplication.TearOff1Height}");

                        // 一段断尾
                        this.BondModule.BondHead.AxisZ.AbsoluteMove(tearOff1);

                        System2RunTimeProvider.RecordTime("系统1点胶流程", $"一段断尾到达指定位置");

                        DelayHelper.Delay(epoxyApplication.TearOff1Delay);

                        System2RunTimeProvider.RecordTime("系统1点胶流程", $"一段段尾延时{epoxyApplication.TearOff1Delay}ms");

                        System2RunTimeProvider.RecordTime("系统1点胶流程", $"一段断尾结束");
                    }

                    #endregion

                    #region 二段断尾

                    if (epoxyApplication.TearOff2Open)
                    {
                        if (epoxyApplication.TearOff2Speed == 0)
                        {
                            throw new Exception("断尾高度不能为0");
                        }

                        // 移动参数，这个目前不知道需不需要考虑分辨率
                        MovePara tearOff2 = new MovePara()
                        {
                            Vel = epoxyApplication.TearOff2Speed,
                            Acc = epoxyApplication.TearOff2Speed * 10.0,
                            Dec = epoxyApplication.TearOff2Speed * 10.0,
                            TargetPosition = interpolationPos.Z + epoxyApplication.TearOff2Height + epoxyApplication.TearOff1Height,
                        };

                        System2RunTimeProvider.RecordTime("系统1点胶流程", $"二段断尾开始，速度{tearOff2.Vel}，高度{epoxyApplication.TearOff2Height}");

                        // 二段短尾
                        this.BondModule.BondHead.AxisZ.AbsoluteMove(tearOff2);

                        System2RunTimeProvider.RecordTime("系统1点胶流程", $"二段断尾到达指定位置");

                        DelayHelper.Delay(epoxyApplication.TearOff2Delay);

                        System2RunTimeProvider.RecordTime("系统1点胶流程", $"二段段尾延时{epoxyApplication.TearOff2Delay}ms");

                        System2RunTimeProvider.RecordTime("系统1点胶流程", $"二段短尾结束");
                    }

                    #endregion
                }

                // 点完胶移动到安全位置
                this.BondModule.BondHead.AxisZ.AbsoluteMove(preDispenseHeight);

                System2RunTimeProvider.RecordTime("系统1点胶流程", $"点胶动作结束");

                return ExcuteResult.Success;
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(ex.Message + "点胶/画胶出现异常，请联系设备管理人员");
                return ExcuteResult.Fail;
            }
        }

        /// <summary>
        /// 打开点胶阀
        /// </summary>
        public void OpenDispensingElectric()
        {
            if (this.BondModule.DripElectric == null)
            {
                throw new Exception("没有找到点胶控制器IO，请检查硬件");
            }

            if (!this.BondModule.DripElectric.GetOutputValue())
            {
                this.BondModule.DripElectric.SetOutputValue(true);
            }
        }

        /// <summary>
        /// 关闭点胶阀
        /// </summary>
        public void CloseDispensingElectric()
        {
            if (this.BondModule.DripElectric == null)
            {
                throw new Exception("没有找到点胶控制器IO，请检查硬件");
            }

            if (this.BondModule.DripElectric.GetOutputValue())
            {
                this.BondModule.DripElectric.SetOutputValue(false);
            }
        }

        /// <summary>
        /// 点胶阀是否开启
        /// </summary>
        /// <returns>结果</returns>
        public bool IsDispensingElectricOpen()
        {
            if (this.BondModule.DripElectric == null)
            {
                throw new Exception("没有找到点胶控制器IO，请检查硬件");
            }

            return this.BondModule.DripElectric.GetOutputValue();
        }

        /// <summary>
        /// 设置点胶阀压力值
        /// </summary>
        /// <param name="dispensePressure">压力值</param>
        /// <param name="vacuum">负压</param>
        public void SetDispensePressure(double dispensePressure, double vacuum)
        {
            if (this.BondModule.DispenserControl == null)
            {
                throw new Exception("未找到点胶控制器，请检查硬件是否有问题");
            }

            try
            {
                this.BondModule.DispenserControl.Pressure = dispensePressure;
                this.BondModule.DispenserControl.Vacuum = vacuum;
                this.BondModule.DispenserControl.Time = 1;
                this.BondModule.DispenserControl.SetPressureVacuumTime(dispensePressure, vacuum, 1, 1);
            }
            catch (Exception ex)
            {
                throw new Exception("点胶控制器设置失败，请检查硬件是否有问题");
            }
        }

        /// <summary>
        /// 系统2点胶是否已经弹出
        /// </summary>
        /// <returns>结果</returns>
        public bool IsDispenseHeightMeasurementOpen()
        {
            return !(this.BondModule.BondHead.DispenseMeasureHeightCylinderPLimit.GetInputValue()
                     && !this.BondModule.BondHead.DispenseMeasureHeightCylinder.CurrentOutputValue);
        }

        /// <summary>
        /// 点胶测高气缸打开并延时
        /// </summary>
        public void OpenDispenseHeightMeasurementCylinder()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            // 如果已经弹出，直接返回
            if (this.BondModule.BondHead.DispenseMeasureHeightCylinderNLimit.GetInputValue()
                && this.BondModule.BondHead.DispenseMeasureHeightCylinder.CurrentOutputValue)
            {
                return;
            }

            RetryCommand:
            this.BondModule.BondHead.DispenseMeasureHeightCylinder.SetOutputValue(true);

            bool isCancelWait = false;

            bool isSuccess = this.BondModule.BondHead.DispenseMeasureHeightCylinderNLimit.WaitSignal(true, 2000, ref isCancelWait);

            if (isSuccess == false)
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                             $"焊头点胶气缸打开失败\n\r" +
                             $"重试: 重新打开点胶气缸" +
                             $"忽略: 点胶头已经伸出，忽略并继续工作" +
                             $"停止: 停止工作",
                             "Alarm",
                             new string[] { "重试", "忽略", "停止" },
                             new DialogResult[] { DialogResult.Retry, DialogResult.Ignore, DialogResult.Abort },
                             AlarmLevel.SecondLevel);

                if (dialogResult == DialogResult.Retry)
                {
                    goto RetryCommand;
                }
                else if (dialogResult == DialogResult.Ignore)
                {
                    return;
                }
                else
                {
                    throw new Exception("点胶针打开失败");
                }
            }
            Console.WriteLine("气缸状态打开成功");

        }

        /// <summary>
        /// 点胶气缸关闭并延时
        /// </summary>
        public void CloseDispenseHeightMeasurementCylinder()
        {
            if (!MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                return;
            }

            // 如果已经关闭，直接返回
            if (this.BondModule.BondHead.DispenseMeasureHeightCylinderPLimit.GetInputValue()
                && !this.BondModule.BondHead.DispenseMeasureHeightCylinder.CurrentOutputValue)
            {
                return;
            }

            RetryCommand:

            this.BondModule.BondHead.DispenseMeasureHeightCylinder.SetOutputValue(false);

            bool isCancelWait = false;

            bool isSuccess = this.BondModule.BondHead.DispenseMeasureHeightCylinderPLimit.WaitSignal(true, 2000, ref isCancelWait);

            if (isSuccess == false)
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"焊头点胶气缸打开失败\n\r" +
                    $"重试: 重新打开点胶气缸\n\r" +
                    $"停止: 停止工作\n\r",
                    "Alarm",
                    new string[] { "重试", "停止" },
                    new DialogResult[] { DialogResult.Retry, DialogResult.Abort },
                    AlarmLevel.SecondLevel);

                if (dialogResult == DialogResult.Retry)
                {
                    goto RetryCommand;
                }
                else
                {
                    throw new Exception("点胶针打开失败");
                }
            }

            Console.WriteLine("气缸状态关闭成功");
        }

        /// <summary>
        /// 蘸胶
        /// </summary>
        /// <param name="epoxyApplication">胶型</param>
        private void PrintEpoxy(EpoxyApplication epoxyApplication)
        {
            if (epoxyApplication.EpoxyApplicationStrategy == EpoxyApplicationTypeEnum.Printting)
            {
                // 停止蘸胶盘转动
                this.PrintToolStop();

                AKRSPoint3D printingToolPos = BondDevicePara.GetInstance().S2DispenseDevicePara.DippingPosition;

                Nozzle nozzle = System2Domain.GetInstance().BondHeadController.GetCurrentNozzle();

                // 加上吸嘴的偏移
                AKRSPoint2D nozzleOffsetRotated =
                    System2Domain.GetInstance().BondHeadController.GetNozzleOffset(nozzle.Name, nozzle.AlignAngle - nozzle.AlignAngle);
                AKRSPoint3D offset = new AKRSPoint3D(nozzleOffsetRotated.X, nozzleOffsetRotated.Y, nozzle.MeasureHeightOffset);

                AKRSPoint3D machinePos = this.BondModuleController.ConvertG0ToMachinePos(printingToolPos);

                // 移动到预蘸胶位置
                this.BondModuleController.MoveSafeBondXYZT(
                    new AKRSPoint4D(
                        machinePos.X + offset.X,
                        machinePos.Y + offset.Y,
                        machinePos.Z + offset.Z + epoxyApplication.PrintSlowDownHeight + epoxyApplication.PrintOffsetZ,
                        nozzle.AlignAngle));

                double height = machinePos.Z + offset.Z + epoxyApplication.PrintOffsetZ;

                // 缓慢下降
                System2Domain.GetInstance().BondHeadController.MoveAxisZ(
                    height, 
                    epoxyApplication.PrintSlowDownSpeed, 
                    AccuracyMode.HighSpeed,
                    false);

                // 停留
                DelayHelper.Delay(epoxyApplication.PrintDelayTime);

                // 缓慢上升
                System2Domain.GetInstance().BondHeadController.MoveAxisZ(
                    height + epoxyApplication.PrintSlowUpHeight,
                    epoxyApplication.PrintSlowUpSpeed,
                    AccuracyMode.HighSpeed,
                    false);

                // 停留
                DelayHelper.Delay(epoxyApplication.PrintSlowUpDelayTime);

                // 停止蘸胶盘转动
                this.PrintToolContinueMove();
            }
        }

        #region 蘸胶方法

        /// <summary>
        /// 蘸胶盘一直转动
        /// </summary>
        public void PrintToolContinueMove()
        {
            this.BondModule.PrintToolContinueMove(DispenseDevicePara.GetInstance().DispenseModulePara.PrintSpinSpeed);
        }

        /// <summary>
        /// 蘸胶盘停止转动
        /// </summary>
        public void PrintToolStop()
        {
            this.BondModule.PrintToolStop();
        }

        /// <summary>
        /// 蘸胶盘是否在转动
        /// </summary>
        /// <returns>结果</returns>
        public bool IsPrintToolMove()
        {
            return this.BondModule.IsPrintToolMove();
        }

        #endregion
    }
}
