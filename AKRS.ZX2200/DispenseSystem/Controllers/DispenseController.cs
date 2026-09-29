using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
using AKRS.ZX2200.DispenseSystem.Modules;
using AKRS.ZX2200.DispenseSystem.Services;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem;
using System;
using System.Windows.Forms;

namespace AKRS.ZX2200.DispenseSystem.Controllers
{
    using AKRS.Galaxy2.AutoFocusing;
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Drive.Common;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.GT;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.LeadShine.E5032;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.LogicHardware.Hardwares.DispenseControllers;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.LogicHardware.Services;
    using AKRS.ZX2200.BondSystem.Controls.Manual;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.Experiment.Test;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using ch.etel.edi.dsa.v40;
    using DevExpress.XtraEditors;
    using LanguageExt.TypeClasses;
    using log4net.Core;
    using PostSharp;
    using PostSharp.Aspects.Advices;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// 点胶模组控制器
    /// </summary>
    public class DispenseController
    {
        /// <summary>
        /// 点胶插补组 设置为静态 为了在所有对象中值存储一份
        /// </summary>
        private static DsaIpolGroup iGroup;

        /// <summary>
        /// 点胶模组
        /// </summary>
        public DispenseModule DispenseModule { get; set; } = new DispenseModule();

        /// <summary>
        /// 点胶设备参数
        /// </summary>
        public DispenseDevicePara DevicePara => DispenseDevicePara.GetInstance();

        /// <summary>
        /// 点胶程式
        /// </summary>
        public System1Program Program => System1Program.GetInstance();

        /// <summary>
        /// 测高控制器
        /// </summary>
        private readonly DispenseMeasureHeightController dispenseMeasureHeightController = new DispenseMeasureHeightController();

        /// <summary>
        /// 点胶域的坐标系
        /// </summary>
        private GeneralCoordinateSystem DispenseCoordinateSystem =>
            (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name == "DispenseCoordinateSystem");

        /// <summary>
        /// 点胶相机坐标系
        /// </summary>
        private DependentCoordinateSystem DispenseCameraCoordinateSystem =>
            (DependentCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name == "DispenseCameraCoordinateSystem");

        /// <summary>
        /// 视觉模组
        /// </summary>
        private VisionModule VisionModule = new VisionModule();

        /// <summary>
        /// 移动到安全位置
        /// </summary>
        public void MoveToSafePos()
        {
            DispenseRunTimeProvider.RecordTime("系统1动作", "移动到安全位置");
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            // 收回气缸
            DispenseRunTimeProvider.RecordTime("系统1动作", "收回点胶高度测量气缸");
            this.dispenseMeasureHeightController.CloseDispenseHeightMeasurementCylinder();
            DispenseRunTimeProvider.RecordTime("系统动作", "收回点胶高度测量气缸完成");

            // 移动到安全位置
            DispenseRunTimeProvider.RecordTime("系统1动作", "移动到安全位置");
            AKRSPoint3D safePos = this.DevicePara.DispenseModulePara.SafePoint3D;
            this.DispenseModule.AxisZAbsoluteMove(safePos.Z);
            this.DispenseModule.AxisXYAbsoluteMove(safePos.X, safePos.Y);
            DispenseRunTimeProvider.RecordTime("系统1动作", "移动到安全位置完成");
        }

        /// <summary>
        /// 移动到G0中的某个位置
        /// </summary>
        /// <param name="point3D">点位</param>
        /// <param name="isMeasureOpen">测高气缸是否打开</param>
        public void MoveToG0Pos3D(AKRSPoint3D point3D, bool isMeasureOpen = false)
        {
            DispenseRunTimeProvider.RecordTime("系统动作", $"移动到{point3D}");

            // 机械坐标
            AKRSPoint3D targetPos = this.DispenseCoordinateSystem.G0PosToSelf(point3D);

            DispenseRunTimeProvider.RecordTime("系统动作", $"移动到机械坐标{targetPos}");

            // 安全高度
            double safeHeight = this.DevicePara.DispenseModulePara.SafePoint3D.Z;
            DispenseRunTimeProvider.RecordTime("系统动作", $"绝对安全高度{safeHeight}");


            // 获取当前点胶平台上的TU
            TransportUnit transportUnit = TransportDomain.GetInstance().TransportProgram.DispenseSubSectionProgram.TransportUnit;

            // 如果TU上没有产品或者在自动运行的时候，先移动到Z轴的安全高度，移动XY，再移动Z轴
            if (Machine.GetInstance().IsWorking() == false
                || transportUnit == null)
            {
                // 如果当前位置和目标位置都比安全位置低，Z轴移动到安全位置，移动XY，最后移动Z轴
                this.DispenseModule.AxisZAbsoluteMove(safeHeight);
                this.DispenseModule.AxisXYAbsoluteMove(targetPos.X, targetPos.Y);
                this.DispenseModule.AxisZAbsoluteMove(targetPos.Z);

                DispenseRunTimeProvider.RecordTime("系统动作", $"先移动到安全高度{safeHeight}，XY移动到{targetPos.X},{targetPos.Y}，Z移动到{targetPos.Z}");
            }
            else if (isMeasureOpen && !MachineHardwareConfiguration.GetInstance().IsSystem1ConfigLaserMh)
            {
                // 先移动到跨越的高度,移动XY，最后下降
                this.DispenseModule.AxisZAbsoluteMove(targetPos.Z + this.DevicePara.DispenseModulePara.MeasureHeightCrossingHeights, false);
                this.DispenseModule.AxisXYAbsoluteMove(targetPos.X, targetPos.Y, false);
                this.DispenseModule.AxisZAbsoluteMove(targetPos.Z, false);
                DispenseRunTimeProvider.RecordTime("系统动作", $"先移动到跨越的高度{targetPos.Z + this.DevicePara.DispenseModulePara.MeasureHeightCrossingHeights}，XY移动到{targetPos.X},{targetPos.Y}，Z移动到{targetPos.Z}");
            }
            else
            {
                // 安全高度 = TU在GO中的位置 + TU设置的安全高度
                safeHeight = transportUnit.CoordinateSystem.SelfPosToG0(new AKRSPoint3D()).Z
                             + ProductConfiguration.GetInstance().TransportUnitConfig
                                 .SafeHeight;

                // 当前安全高度是测高针的安全高度，需要转换到点胶头上面
                safeHeight = this.GetG0DispensePos(new AKRSPoint3D(0, 0, safeHeight)).Z;

                // 将安全位置转到轴坐标
                safeHeight = this.DispenseCoordinateSystem.G0PosToSelf(new AKRSPoint3D(0, 0, safeHeight)).Z;

                DispenseRunTimeProvider.RecordTime("系统动作", $"安全高度{safeHeight}");

                // 移动到位置
                this.MoveToMachinePoint(targetPos, safeHeight);

                DispenseRunTimeProvider.RecordTime("系统动作", $"移动到{point3D}完成");
            }
        }

        /// <summary>
        /// 移动到G0中的某个位置(不安全)，慎用
        /// </summary>
        /// <param name="point3D">点位</param>
        public void MoveToG0Pos3DWithoutSafe(AKRSPoint3D point3D)
        {
            DispenseRunTimeProvider.RecordTime("系统动作", $"不安全移动到{point3D}");
            AKRSPoint3D targetPos = this.DispenseCoordinateSystem.G0PosToSelf(point3D);

            // 多轴移动
            this.DispenseModule.AxisXYZAbsoluteMove(targetPos);

            DispenseRunTimeProvider.RecordTime("系统动作", $"不安全移动完成");
        }

        /// <summary>
        /// 自动运行的时候才会调用此方法
        /// 根据情况可能会多轴同时移动
        /// </summary>
        /// <param name="point3D">G0中的位置</param>
        /// <param name="safeHeight">安全高度</param>
        public void MoveToMachinePoint(AKRSPoint3D point3D, double safeHeight)
        {
            // 将G0的点位转换到轴坐标
            AKRSPoint3D targetPos = point3D;

            // 当前位置
            double curPos = this.GetAxisPos().Z;

            DispenseRunTimeProvider.RecordTime("系统动作", $"当前高度{curPos}，目标高度{targetPos.Z}，安全高度{safeHeight}");

            // 判断位置，如果当前位置高于安全高度且目标位置位置也高于安全高度,可以三轴直接移动
            if (curPos >= safeHeight && targetPos.Z >= safeHeight)
            {
                this.DispenseModule.AxisXYZAbsoluteMove(targetPos);
                DispenseRunTimeProvider.RecordTime("系统动作", $"三轴一起移动");
            }
            else if (curPos > safeHeight && targetPos.Z < safeHeight)
            {
                // 当前位置比安全位置高，目标位置比安全位置低，先移动XY，再移动Z轴
                this.DispenseModule.AxisXYAbsoluteMove(targetPos.X, targetPos.Y);

                // XY移动到位之后移动Z轴
                this.DispenseModule.AxisZAbsoluteMove(targetPos.Z);

                DispenseRunTimeProvider.RecordTime("系统动作", $"移动XY，再移动Z");
            }
            else if (curPos < safeHeight && targetPos.Z > safeHeight)
            {
                // 当前位置比安全位置低，目标位置比安全位置高，先移动XY，再移动Z轴
                this.DispenseModule.AxisZAbsoluteMove(targetPos.Z);
                this.DispenseModule.AxisXYAbsoluteMove(targetPos.X, targetPos.Y);

                DispenseRunTimeProvider.RecordTime("系统动作", $"移动Z，再移动XY");
            }
            else if (curPos < safeHeight && targetPos.Z <= safeHeight)
            {
                // 如果当前位置和目标位置都比安全位置低，Z轴移动到安全位置，移动XY，最后移动Z轴
                this.DispenseModule.AxisZAbsoluteMove(safeHeight);
                this.DispenseModule.AxisXYAbsoluteMove(targetPos.X, targetPos.Y);
                this.DispenseModule.AxisZAbsoluteMove(targetPos.Z);
                DispenseRunTimeProvider.RecordTime("系统动作", $"移动Z，再移动XY，再移动Z");
            }
            else
            {
                DispenseRunTimeProvider.RecordTime("系统动作", $"意外情况");
                throw new Exception("点胶移动异常，请联系管理员");
            }
        }

        /// <summary>
        /// 点胶相机去看G0中的某个位置
        /// </summary>
        /// <param name="point3D">G0中的点位</param>
        /// <returns>相机看到的位置</returns>
        public AKRSPoint3D GetG0VisionPos(AKRSPoint3D point3D)
        {
            return point3D + CalibrateRunPara.GetInstance().DispenseMarkVisionMachinePos
                   - CalibrateRunPara.GetInstance().DispenseMeasureRealHeightMachinePos;
        }

        /// <summary>
        /// 将相机中的一个点转到G0坐标
        /// </summary>
        /// <param name="point3D">相机看到的位置</param>
        /// <returns>G0中的点位</returns>
        public AKRSPoint3D GetG0PosFromVision(AKRSPoint3D point3D)
        {
            return point3D - CalibrateRunPara.GetInstance().DispenseMarkVisionMachinePos
                   + CalibrateRunPara.GetInstance().DispenseMeasureRealHeightMachinePos;
        }

        /// <summary>
        /// 点胶头去点G0中的某一个点
        /// </summary>
        /// <param name="point3D">点位</param>
        /// <returns>值</returns>
        public AKRSPoint3D GetG0DispensePos(AKRSPoint3D point3D)
        {
            return this.GetG0VisionPos(point3D) - this.Program.DispenserProgram.Dispenser.DispensingNeedleOffset
                   + new AKRSPoint3D(0, 0, this.DevicePara.PreDispensePlatePara.DistanceInSensor);
        }

        /// <summary>
        /// 执行点胶、画胶动作、双轴插补，目前值支持双轴插补
        /// 只能画连续的曲线
        /// </summary>
        /// <param name="epoxyApplication">图形</param>
        /// <param name="point">点胶点</param>
        /// <param name="isDrip">是否出胶</param>
        /// <param name="isPrePlant">是否在预点胶板上出胶</param>
        /// <param name="angle">角度</param>
        /// <returns>是否成功</returns>
        public ExcuteResult InterpolationApplication(
            EpoxyApplication epoxyApplication,
            AKRSPoint3D point,
            bool isDrip = true,
            bool isPrePlant = false,
            double angle = 0)
        {
            try
            {
                DispenseRunTimeProvider.RecordTime("系统1点胶流程", "准备开始点胶");

                #region 数据准备

                // 将G0坐标转换到机械坐标
                AKRSPoint3D interpolationPos = this.ConvertG0ToMachinePos(point);

                // 将位置转换到点胶头
                interpolationPos = System1Domain.GetInstance().DispenseController.GetG0DispensePos(interpolationPos);

                // 数据处理，归一化
                epoxyApplication = DispenserMoveHelper.EpoxyPretreatment(epoxyApplication, interpolationPos, angle);

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
                    if (i == 0 || this.Program.DispenserProgram.Dispenser.DispenserType == DispenserTypeEnum.PrintingTool)
                    {
                        #region 移动到第一个点胶点并异步设置胶压

                        // 将这个位置告诉点胶头
                        AKRSPoint3D preDispensePos =
                            new AKRSPoint3D(
                                epoxyApplication.DispensePatternParas[0][0].X,
                                epoxyApplication.DispensePatternParas[0][0].Y,
                                preDispenseHeight);

                        DispenseRunTimeProvider.RecordTime("系统1点胶流程", "开始设置胶压");

                        // 异步设置胶压
                        Task task = new Task(
                            () =>
                                {
                                    if (MachineStateModel.GetInstance().IsDryCycle || this.Program.DispenserProgram.Dispenser.DispenserType == DispenserTypeEnum.PrintingTool)
                                    {
                                        return;
                                    }

                                    this.SetDispensePressure(
                                        (int)epoxyApplication.DispensePressure,
                                        (int)epoxyApplication.Vacuum);
                                });

                        task.Start();

                        // 单步工作
                        if (!System1Domain.GetInstance().WaitSingleStep())
                        {
                            return ExcuteResult.Alarm;
                        }

                        // 移动到最开始的位置
                        this.MoveToG0Pos3D(this.ConvertMachineToG0Pos(preDispensePos));

                        DispenseRunTimeProvider.RecordTime("系统1点胶流程", "移动到最开始的位置完成");

                        task.Wait();

                        DispenseRunTimeProvider.RecordTime("系统1点胶流程", "设置胶压完成");

                        if (MachineStateModel.GetInstance().IsDryCycle)
                        {
                            return ExcuteResult.Success;
                        }

                        #endregion
                    }
                    else
                    {
                        this.DispenseModule.AxisZAbsoluteMove(preDispenseHeight);

                        DispenseRunTimeProvider.RecordTime("系统1点胶流程", "点胶Z轴移动到预点胶位置完成");

                        this.DispenseModule.AxisXYAbsoluteMove(
                            epoxyApplication.DispensePatternParas[i][0].X,
                            epoxyApplication.DispensePatternParas[i][0].Y);

                        //MotionService.MoveAxesToTargetPosition(
                        //    (this.DispenseModule.DispenseAxisX, false, epoxyApplication.DispensePatternParas[i][0].X,
                        //        AccuracyMode.HighAccuracy),
                        //    (this.DispenseModule.DispenseAxisY, false, epoxyApplication.DispensePatternParas[i][0].Y, AccuracyMode.HighAccuracy));

                        DispenseRunTimeProvider.RecordTime("系统1点胶流程", "点胶过程中 点胶XY轴插补移动到指令位置");
                    }

                    #region 提前开胶

                    // 提前开胶
                    if (epoxyApplication.AdvanceOpenDistanceNew != 0)
                    {
                        // 单步工作
                        if (!System1Domain.GetInstance().WaitSingleStep())
                        {
                            return ExcuteResult.Alarm;
                        }

                        this.MoveAxisZToPositionWithoutDelay(
                            interpolationPos.Z,
                            epoxyApplication.SlowTravelSpeedBeforeDispensing);

                        DispenseRunTimeProvider.RecordTime("系统1点胶流程", "提前开胶-点胶Z轴开始缓慢下降");

                        while (true)
                        {
                            // 循环判断有没有到位
                            if (this.DispenseModule.GetAxisPos().Z
                                <= interpolationPos.Z + epoxyApplication.AdvanceOpenDistanceNew)
                            {
                                this.DispenseModule.OpenDispensingElectric();
                                break;
                            }

                            Thread.Sleep(1);
                        }

                        this.DispenseModule.AxisZWaitForArrival();

                        DispenseRunTimeProvider.RecordTime("系统1点胶流程", "点胶Z轴开始缓慢下降到位");
                    }
                    else
                    {
                        this.DispenseModule.AxisZAbsoluteMove(interpolationPos.Z, epoxyApplication.SlowTravelSpeedBeforeDispensing);
                    }

                    #endregion

                    #region 滞后运动

                    // 单步工作
                    if (!System1Domain.GetInstance().WaitSingleStep())
                    {
                        return ExcuteResult.Alarm;
                    }

                    // 判断是否需要到位等待
                    if (epoxyApplication.DispenserLeadTime > 0)
                    {
                        if (epoxyApplication.IsDispenserLeadTimeOpen)
                        {
                            this.DispenseModule.OpenDispensingElectric();
                            DispenseRunTimeProvider.RecordTime("系统1点胶流程", "Z轴移动到位，开始出胶，等待结束");
                        }
                        else
                        {
                            this.DispenseModule.CloseDispensingElectric();
                            DispenseRunTimeProvider.RecordTime("系统1点胶流程", "Z轴移动到位，不出胶，等待结束");
                        }

                        Thread.Sleep(epoxyApplication.DispenserLeadTime);

                        DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"点胶前到位等待结束");
                    }

                    #endregion

                    // 如果画胶里面只有两个数且XY相等，则认为是点胶，不开启插补
                    if (epoxyApplication.DispensePatternParas[i].Length == 2
                        && epoxyApplication.DispensePatternParas[i][0].X
                            .CompareTo(epoxyApplication.DispensePatternParas[i][1].X) == 0
                        && epoxyApplication.DispensePatternParas[i][0].Y
                            .CompareTo(epoxyApplication.DispensePatternParas[i][1].Y) == 0 
                        || epoxyApplication.EpoxyApplicationStrategy == EpoxyApplicationTypeEnum.SingleDotOnly)
                    {
                        #region 点胶

                        DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"开始单颗点胶");

                        // 单步工作
                        if (!System1Domain.GetInstance().WaitSingleStep())
                        {
                            return ExcuteResult.Alarm;
                        }

                        if (isDrip)
                        {
                            this.DispenseModule.OpenDispensingElectric();
                        }
                        else
                        {
                            this.DispenseModule.CloseDispensingElectric();
                        }

                        DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"开启点胶阀完成");

                        int firstDelay = (DispenseRunTimeProvider.IsFirstDispense && !isPrePlant) ? this.DevicePara.DispenseModulePara.FisrtDispenseDelay : 0;

                        // 保持一定时间,这个在画胶软件里面去设置
                        DelayHelper.Delay(epoxyApplication.DispensePatternParas[i][0].DripTime + firstDelay);

                        DispenseRunTimeProvider.RecordTime(
                            "系统1点胶流程",
                            $"点胶延时{epoxyApplication.DispensePatternParas[i][0].DripTime + firstDelay}");

                        // 点胶完成，关闭点胶阀IO
                        this.DispenseModule.CloseDispensingElectric();

                        DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"关闭点胶阀完成");

                        #endregion
                    }
                    else
                    {
                        // 单步工作
                        if (!System1Domain.GetInstance().WaitSingleStep())
                        {
                            return ExcuteResult.Alarm;
                        }

                        if (this.DispenseModule.GetDispenseXAxis().AxisDrive is ETELAxis)
                        {
                            iGroup = this.GetIpolGroup();

                            DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"准备开始点胶插补");

                            // 开始插补
                            iGroup.ipolBegin();


                            DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"点胶开始插补");

                            // 设置为绝对坐标系 ，不设置绝对坐标系
                            iGroup.ipolSetAbsMode(true, -1);

                            // 连续插补
                            for (int j = 0; j < epoxyApplication.DispensePatternParas[i].Length; j++)
                            {
                                // 由于第一段是到这个位置的参数，所以需要默认值
                                if (j == 0)
                                {
                                    iGroup.ipolTanVelocity(0.01);
                                    iGroup.ipolTanAcceleration(0.01);
                                    iGroup.ipolTanDeceleration(0.01);

                                    // 设置抖动时间，不知道什么意思
                                    iGroup.ipolTanJerkTime(0.01);
                                }
                                else
                                {
                                    // 设置速度,Etel的单位为M/s，外接传入为mm/s,需要转换
                                    iGroup.ipolTanVelocity(epoxyApplication.DispensePatternParas[i][j - 1].Speed / 1000.0);

                                    // 设置加速度
                                    iGroup.ipolTanAcceleration(
                                        epoxyApplication.DispensePatternParas[i][j - 1].Acc / 1000.0);
                                    iGroup.ipolTanDeceleration(
                                        epoxyApplication.DispensePatternParas[i][j - 1].Acc / 1000.0);

                                    iGroup.ipolTanJerkTime(0.01);
                                }

                                if (epoxyApplication.DispensePatternParas[i][j].OpenGlueFlag)
                                {
                                    iGroup.ipolMark2Param(5, 5, 0x0000001, 1000);
                                    // 点胶完成，关闭点胶阀IO
                                    this.DispenseModule.OpenDispensingElectric();
                                }
                                else
                                {
                                    iGroup.ipolMark2Param(5, 5, 0x0010000, 1000);
                                    // 点胶完成，关闭点胶阀IO
                                    this.DispenseModule.CloseDispensingElectric();
                                }

                                iGroup.ipolLine(
                                    epoxyApplication.DispensePatternParas[i][j].X / 1000,
                                    epoxyApplication.DispensePatternParas[i][j].Y / 1000);

                                // 等待插补结束
                                iGroup.ipolWaitMovement(100000);
                            }

                            // 最后关闭插补的IO
                             iGroup.ipolMark2Param(5, 5, 0x0010000, 1000);
                            // 点胶完成，关闭点胶阀IO
                            this.DispenseModule.CloseDispensingElectric();

                            DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"关闭点胶阀完成");

                            // 退出插补模式
                            iGroup.ipolEnd();

                            DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"点胶结束插补");
                        }
                        else
                        {
                            //this.DispenseModule.DripElectric.SetOutputValue(true);

                            GuGaoDrive.Interpolation(
                                1,
                                epoxyApplication.DispensePatternParas[i],
                                this.DispenseModule.GetDispenseIndex());

                            this.DispenseModule.CloseDispensingElectric();

                            DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"点胶结束插补");
                        }
                    }

                    #region 一段断尾

                    if (epoxyApplication.TearOff1Open)
                    {
                        this.DispenseModule.AxisZAbsoluteMove(
                            interpolationPos.Z + epoxyApplication.TearOff1Height,
                            epoxyApplication.TearOff1Speed);

                        DelayHelper.Delay(epoxyApplication.TearOff1Delay);

                        DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"一段断尾到达指定位置");

                        DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"一段段尾延时{epoxyApplication.TearOff1Delay}ms");
                    }

                    #endregion

                    #region 二段断尾

                    if (epoxyApplication.TearOff2Open)
                    {
                        this.DispenseModule.AxisZAbsoluteMove(
                            interpolationPos.Z + epoxyApplication.TearOff2Height,
                            epoxyApplication.TearOff2Speed);

                        DelayHelper.Delay(epoxyApplication.TearOff2Delay);

                        DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"二段断尾到达指定位置");

                        DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"二段段尾延时{epoxyApplication.TearOff2Delay}ms");
                    }

                    #endregion
                }

                // 点完胶移动到安全位置
                this.DispenseModule.AxisZAbsoluteMove(preDispenseHeight);

                DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"点胶动作结束");

                return ExcuteResult.Success;
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(ex.Message + "点胶/画胶出现异常，请联系设备管理人员");
                return ExcuteResult.Fail;
            }
        }

        /// <summary>
        /// 蘸胶
        /// </summary>
        /// <param name="epoxyApplication">胶型</param>
        private void PrintEpoxy(EpoxyApplication epoxyApplication)
        {
            if (MachineHardwareConfiguration.GetInstance().IsSystem1ConfigPrintingTool
                && this.Program.DispenserProgram.Dispenser.DispenserType == DispenserTypeEnum.PrintingTool)
            {
                // 停止蘸胶盘转动
                this.PrintToolStop();
                DispenseRunTimeProvider.RecordTime("系统1蘸胶流程", $"停止蘸胶盘转动");

                AKRSPoint3D printingToolPos = DispenseDevicePara.GetInstance().DispenserPara.PrintingToolPos;

                AKRSPoint3D prePrintingToolPos = new AKRSPoint3D(
                    printingToolPos.X,
                    printingToolPos.Y,
                    printingToolPos.Z + epoxyApplication.PrintSlowDownHeight);

                // 移动到预蘸胶位置
                this.MoveToG0Pos3D(prePrintingToolPos);

                DispenseRunTimeProvider.RecordTime("系统1蘸胶流程", $"移动到预蘸胶位置{prePrintingToolPos}完成");

                double preHeight = this.ConvertG0ToMachinePos(printingToolPos).Z;

                // 轴缓慢下降
                this.DispenseModule.AxisZAbsoluteMove(preHeight, epoxyApplication.PrintSlowDownSpeed);

                DispenseRunTimeProvider.RecordTime("系统1蘸胶流程", $"移动蘸胶高度{preHeight},速度{epoxyApplication.PrintSlowDownSpeed}完成");

                // 停留
                DelayHelper.Delay(epoxyApplication.PrintDelayTime);

                DispenseRunTimeProvider.RecordTime("系统1蘸胶流程", $"移动蘸胶停留{epoxyApplication.PrintDelayTime}完成");

                // 轴缓慢上升
                this.DispenseModule.AxisZAbsoluteMove(
                    this.ConvertG0ToMachinePos(printingToolPos).Z + epoxyApplication.PrintSlowUpHeight, epoxyApplication.PrintSlowUpSpeed);
                
                DispenseRunTimeProvider.RecordTime("系统1蘸胶流程", $"移动蘸胶高度{this.ConvertG0ToMachinePos(printingToolPos).Z + epoxyApplication.PrintSlowUpHeight},速度{epoxyApplication.PrintSlowUpSpeed}完成");

                // 停留
                DelayHelper.Delay(epoxyApplication.PrintSlowUpDelayTime);

                DispenseRunTimeProvider.RecordTime("系统1蘸胶流程", $"移动预蘸胶停留{epoxyApplication.PrintSlowUpDelayTime}完成");

                // 停止蘸胶盘转动
                this.PrintToolContinueMove();

                DispenseRunTimeProvider.RecordTime("系统1蘸胶流程", $"蘸胶完成，蘸胶盘继续转动");
            }
        }

        /// <summary>
        /// 点胶Z轴移动到一固定位置不等待
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="speed">速度</param>
        private void MoveAxisZToPositionWithoutDelay(double position, double speed)
        {
            // 移动参数，这个目前不知道需不需要考虑分辨率
            MovePara movePara = new MovePara()
                                    {
                                        Vel = speed,
                                        Acc = MachineHardwareConfiguration.GetInstance().IsDispenseZGuGao ? speed * 10 : this.DispenseModule.GetDispenseZAxis().GetAcc(),
                                        Dec = MachineHardwareConfiguration.GetInstance().IsDispenseZGuGao ? speed * 10 : this.DispenseModule.GetDispenseZAxis().GetDec(),
                                        TargetPosition = position,
                                    };

            DispenseRunTimeProvider.RecordTime("系统1流程", $"Z轴发送指令到位置{position},速度{speed}完成");

            // 一段断尾
            this.DispenseModule.AxisZSendAbsoluteMoveCommand(movePara);

            DispenseRunTimeProvider.RecordTime("系统1流程", $"发送指令完成");
        }

        /// <summary>
        /// 获取轴的坐标
        /// </summary>
        /// <returns>结果</returns>
        public AKRSPoint3D GetAxisPos()
        {
            return this.DispenseModule.GetAxisPos();
        }

        /// <summary>
        /// 获取在G0坐标系下的点位
        /// </summary>
        /// <returns>结果</returns>
        public AKRSPoint3D GetG0Pos()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return this.GetRanDomPos();
            }

            return this.DispenseCoordinateSystem.ForwardConvertCoordinate(
               this.DispenseModule.GetAxisPos());   
        }

        /// <summary>
        /// 获取随机位置，一般用于单机模式
        /// </summary>
        /// <returns>结果</returns> 
        public AKRSPoint3D GetRanDomPos()
        {
            Random random = new Random();

            return new AKRSPoint3D(random.Next(0, 100), random.Next(0, 100), random.Next(0, 100));
        }

        /// <summary>
        /// 将相机所看到的物体转换到现实中
        /// </summary>
        /// <param name="result">点胶轴的真实坐标</param>
        /// <param name="visionPos">定位的结果</param>
        /// <returns>结果</returns>
        public AKRSPoint3D ConvertVisionResultInG0(MatchResult result, AKRSPoint3D visionPos = null)
        {
            if (visionPos == null)
            {
                // 获取当前位置为定位位置
                visionPos = this.GetAxisPos();
            }

            return this.DispenseCameraCoordinateSystem.SelfPosToG0(
                new AKRSPoint3D(result.CenterX, result.CenterY, 0),
                visionPos);
        }

        /// <summary>
        /// 将G0转换到自己的坐标上去
        /// </summary>
        /// <param name="point3D">G0上面的点</param>
        /// <returns>轴上面的点</returns>
        public AKRSPoint3D ConvertG0ToMachinePos(AKRSPoint3D point3D)
        {
            return this.DispenseCoordinateSystem.G0PosToSelf(point3D);
        }

        /// <summary>
        /// 将自己坐标转换到G0
        /// </summary>
        /// <param name="point3D">轴上面的点</param>
        /// <returns>G0中的点位</returns>
        public AKRSPoint3D ConvertMachineToG0Pos(AKRSPoint3D point3D)
        {
            return this.DispenseCoordinateSystem.SelfPosToG0(point3D);
        }

        /// <summary>
        /// 打开点胶阀
        /// </summary>
        public void OpenDispensingElectric()
        {
           this.DispenseModule.OpenDispensingElectric();
        }

        /// <summary>
        /// 关闭点胶阀
        /// </summary>
        public void CloseDispensingElectric()
        {
            this.DispenseModule.CloseDispensingElectric();
        }

        /// <summary>
        /// 点胶阀是否开启
        /// </summary>
        /// <returns>结果</returns>
        public bool IsDispensingElectricOpen()
        {
           return this.DispenseModule.IsDispensingElectricOpen();
        }

        /// <summary>
        /// 设置点胶阀压力值
        /// </summary>
        /// <param name="dispensePressure">压力值</param>
        /// <param name="vacuum">负压</param>
        public void SetDispensePressure(double dispensePressure, double vacuum)
        {
            try
            {
                // 如果不连接点胶器直接返回
                if (!this.DevicePara.DispenseModulePara.IsLinkDispenser)
                {
                    return;
                }

                DispenseRunTimeProvider.RecordTime("系统1胶压流程", $"准备设置胶压{dispensePressure}，负压{vacuum}");

                // 总胶压 = 输入胶压 + 补偿胶压
                double dispensePressureTotal = dispensePressure + this.DevicePara.DispenseModulePara.GluePressCompensation;

                DispenseRunTimeProvider.RecordTime("系统1胶压流程", $"补偿后胶压{dispensePressureTotal}，负压{vacuum}");

                // 判断是否超过总胶压
                if (dispensePressureTotal > this.DevicePara.DispenseModulePara.DispenserMaxValue)
                {
                    dispensePressureTotal = this.DevicePara.DispenseModulePara.DispenserMaxValue;

                    DispenseRunTimeProvider.RecordTime("系统1胶压流程", $"胶压过大，改为最大胶压{dispensePressureTotal}，负压{vacuum}");
                }

                // 判断是否低于最低气压
                if (dispensePressureTotal < 30)
                {
                    dispensePressureTotal = 30;
                    DispenseRunTimeProvider.RecordTime("系统1胶压流程", $"胶压过小，改为最小胶压{dispensePressureTotal}，负压{vacuum}");
                }

                this.DispenseModule.SetDispensePressure(dispensePressureTotal, vacuum);
            }
            catch
            {
                throw new Exception("设置点胶阀参数出现问题，请修改后重试");
            }
        }

        /// <summary>
        /// 移动点胶Z轴到安全位置
        /// </summary>
        public void MoveZToSafePos()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            // 收回气缸
            this.dispenseMeasureHeightController.CloseDispenseHeightMeasurementCylinder();

            // 移动到安全位置
            this.DispenseModule.AxisZAbsoluteMove(this.DevicePara.DispenseModulePara.SafePoint3D.Z);
        }

        /// <summary>
        /// 自动对焦,示教界面专用
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="form">窗体</param>
        public void AutoFocusAssistance(object sender, Form form)
        {
            SimpleButton btn = sender as SimpleButton;
            if (btn == null)
            {
                throw new Exception("自动聚焦失败");
            }

            btn.Enabled = false;
            btn.Appearance.BackColor = Color.Yellow;

            Task.Run(
                () =>
                    {
                        try
                        {
                            this.AutoFocus();
                            form.Invoke(
                                new Action(
                                    () =>
                                        {
                                            btn.Enabled = true;
                                            btn.Appearance.BackColor = Color.White;
                                        }));
                        }
                        catch (Exception exception)
                        {
                            AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            form.Invoke(
                                new Action(
                                    () =>
                                        {
                                            btn.Appearance.BackColor = default;
                                            btn.Enabled = true;
                                        }));
                        }
                    });
        }

        /// <summary>
        /// 自动聚焦
        /// </summary>
        private void AutoFocus()
        {
            double z = this.DispenseModule.GetAxisPos().Z;
            DispenseRunTimeProvider.RecordTime("系统1流程", $"自动聚焦开始,当前Z轴位置{z}");
            AutoFocusing autofocus = new AutoFocusing();
            autofocus.AutoFocus(
            this.VisionModule.DispenseCamera,
            this.DispenseModule.GetDispenseZAxis(),
            z + 3,
                z - 3);
            DispenseRunTimeProvider.RecordTime("系统1流程", $"自动聚焦结束");
        }

        /// <summary>
        /// 获取点胶插补组
        /// </summary>
        /// <returns>插补群组</returns>
        public DsaIpolGroup GetIpolGroup() 
        {
            if (iGroup == null)
            {
                //// 获取X，Y轴驱动
                //DsaDrive moveDriveX = ((ETELAxis)this.DispenseModule.DispenseAxisX.AxisDrive).GetDrive();
                //DsaDrive moveDriveY = ((ETELAxis)this.DispenseModule.DispenseAxisY.AxisDrive).GetDrive();

                // 获取X，Y轴驱动
                DsaDrive moveDriveX = ((ETELAxis)this.DispenseModule.GetDispenseXAxis().AxisDrive).GetDrive();
                DsaDrive moveDriveY = ((ETELAxis)this.DispenseModule.GetDispenseYAxis().AxisDrive).GetDrive();

                // 获取ETEL轴
                AxisCard card = HardwareRepositoryService.GetHardware<AxisCard>("ETEL");

                // 获取控制器
                DsaMaster dsaMaster = ((ETELController)card.MotionController.MotionControllerDrive).GetMaster();

                // 设置群组
                iGroup = new DsaIpolGroup(moveDriveX, moveDriveY);

                // 设置控制者
                iGroup.setMaster(dsaMaster);
            }

            return iGroup;
        }

        /// <summary>
        /// 获取硬件集合
        /// </summary>
        /// <returns>结果</returns>
        public List<string> GetHardWareNames()
        {
            List<string> strings = new List<string>();
            if (!MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                return strings;
            }

            strings.Add("点胶X");
            strings.Add("点胶Y");
            strings.Add("点胶Z");
            strings.Add("系统1点胶控制");
            strings.Add("点胶器1");
            strings.Add("点胶胶量检测");
            strings.Add("点胶相机");

            if (MachineHardwareConfiguration.GetInstance().LightConfig == BondSystem.Models.Enums.LightConfigEnum.MonochromaticLight)
            {
                strings.Add("点胶三色点光-红");
                strings.Add("点胶三色环光-红");
            }
            else
            {
                strings.Add("点胶三色点光-红");
                strings.Add("点胶三色点光-绿");
                strings.Add("点胶三色点光-蓝");
                strings.Add("点胶三色环光-红");
                strings.Add("点胶三色环光-绿");
                strings.Add("点胶三色环光-蓝");
            }

            if (MachineHardwareConfiguration.GetInstance().IsSystem1ConfigLaserMh)
            {
                strings.Add("点胶激光测高");
            }
            else
            {
                strings.Add("点胶测高气缸电磁阀");
                strings.Add("点胶测高气缸动点下降检测");
                strings.Add("点胶测高气缸原点升起检测");
                strings.Add("点胶测高传感器");
            }
           
            strings.Add("预点胶平台检测");
            strings.Add("点胶相机");

            return strings;
        }

        #region 蘸胶方法

        /// <summary>
        /// 蘸胶盘一直转动
        /// </summary>
        public void PrintToolContinueMove()
        {
            this.DispenseModule.PrintToolContinueMove(DispenseDevicePara.GetInstance().DispenseModulePara.PrintSpinSpeed);
        }
        
        /// <summary>
        /// 蘸胶盘停止转动
        /// </summary>
        public void PrintToolStop()
        {
            this.DispenseModule.PrintToolStop();
        }

        /// <summary>
        /// 蘸胶盘是否在转动
        /// </summary>
        /// <returns>结果</returns>
        public bool IsPrintToolMove()
        {
            return this.DispenseModule.IsPrintToolMove();
        }

        #endregion
    }
}
