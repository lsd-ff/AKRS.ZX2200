using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using ch.etel.edi.dsa.v40;
using DevExpress.XtraEditors;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using TransportUnit = AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit;

namespace AKRS.ZX2200.BondSystem.Controllers
{
    using System.Threading.Tasks;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionControllerInterface;
    using AKRS.Galaxy2.LogicHardware.Services;
    using AKRS.Galaxy2.MachineSupport;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.SupportFeature.Statistics;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using static DevExpress.Xpo.Helpers.CannotLoadObjectsHelper;

    /// <summary>
    /// Bond模组控制器
    /// </summary>
    public partial class BondModuleController
    {
        /// <summary>
        /// 固晶模组
        /// </summary>
        private BondModule bondModule => System2Module.GetInstance().BondModule;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// Bond程式
        /// </summary>
        private BondProgram bondProgram => BondProgram.GetInstance();

        /// <summary>
        /// 设备参数
        /// </summary>
        private BondDevicePara bondDevicePara => BondDevicePara.GetInstance();

        /// <summary>
        /// ULM运动点位
        /// </summary>
        private ULMPara uLMPara => BondDevicePara.GetInstance().ULMPara;

        /// <summary>
        /// 在实体上测高
        /// </summary>
        /// <param name="baseEntity">对象</param>
        /// <returns>结果</returns>
        [Obsolete]
        public ExcuteResult MeasureHeightOnEntity(BaseMatter baseEntity)
        {
            // 如果没有测高的点位直接返回
            // todo:如果测高不开启
            if (baseEntity.Config.HeightMeasurementPoints == null
                || baseEntity.Config.HeightMeasurementPoints.Count == 0)
            {
                return ExcuteResult.Success;
            }

            // 未测试过，暂时不开启
            return ExcuteResult.Success;

            // 获取当前吸嘴
            Nozzle nozzle = this.bondModule.BondHead.CurrentNozzle;

            // 测试位置的结果集合
            List<double> heightsList = new List<double>();

            // 原来的集合
            List<double> oldHeightList = new List<double>();

            foreach (AKRSPoint3D pos in baseEntity.Config.HeightMeasurementPoints)
            {
                // 计算G0坐标
                AKRSPoint3D g0Pos = baseEntity.CoordinateSystem.SelfPosToG0(pos);

                // 将结果储存起来，后续计算
                oldHeightList.Add(g0Pos.Z);

                // 实际测高位置计算
                AKRSPoint3D measHeightPos = g0Pos + nozzle.MeasureHeightOffset + 5;

                // 移动到测高的位置
                this.MoveToG0Pos(measHeightPos);

                // 执行测高
                (ExcuteResult excuteResult, double height) result = this.bondHeadController.MeasureHeight(
                    this.bondHeadController.GetAxisZRealPos(),
                    HeightMeasurementFunctionEnum.WithTDSensor);

                // 如果测高不成功
                if (result.excuteResult != ExcuteResult.Success)
                {
                    return ExcuteResult.Alarm;
                }

                heightsList.Add(result.height);
            }

            // 测高值计算
            double heightResult = heightsList.Average();

            double oldHeightResult = oldHeightList.Average();

            double upDataHeight = heightResult - oldHeightResult;

            // 刷新TU坐标系
            baseEntity.CoordinateSystem.UpdateCoordinateSystem(
                new ElementCoordinate(0, new AKRSPoint3D(0, 0, upDataHeight)));

            return ExcuteResult.Success;
        }

        /// <summary>
        /// Bond坐标转成G0坐标
        /// </summary>
        /// <param name="pos">Bond坐标</param>
        /// <returns>G0坐标</returns>
        public double ConvertMachineToG0Pos(double pos)
        {
            return this.bondModule.ConvertMachineToG0Pos(pos);
        }

        /// <summary>
        /// Bond坐标转成G0坐标
        /// </summary>
        /// <param name="pos">Bond坐标</param>
        /// <returns>G0坐标</returns>
        public AKRSPoint3D ConvertMachineToG0Pos(AKRSPoint3D pos)
        {
            return this.bondModule.ConvertMachineToG0Pos(pos);
        }

        /// <summary>
        /// G0坐标转回Bond坐标
        /// </summary>
        /// <param name="pos">G0坐标</param>
        /// <returns>Bond坐标</returns>
        public AKRSPoint3D ConvertG0ToMachinePos(AKRSPoint3D pos)
        {
            return this.bondModule.ConvertG0ToMachinePos(pos);
        }

        /// <summary>
        /// G0坐标转回Bond坐标
        /// </summary>
        /// <param name="pos">G0坐标</param>
        /// <returns>Bond坐标</returns>
        public double ConvertZAxisG0ToMachinePos(double pos)
        {
            return this.bondModule.ConvertG0ToMachinePos(new AKRSPoint3D(0, 0, pos)).Z;
        }

        /// <summary>
        /// 获取相机硬件
        /// </summary>
        /// <returns>硬件集合</returns>
        public PRHardware GetHardware()
        {
            return this.bondModule.GetHardware();
        }

        /// <summary>
        /// 获取Z-XY相机硬件
        /// </summary>
        /// <returns>硬件集合</returns>
        public PRHardware GetHardwareZWithXY()
        {
            return this.bondModule.GetHardwareZWithXY();
        }

        /// <summary>
        /// 相机设置硬件
        /// </summary>
        /// <param name="pRName">模版名称</param>
        public void SetHardware(string pRName)
        {
            this.bondModule.SetHardware(pRName);
        }

        /// <summary>
        /// 将Bond相机的结果转换成G0中的位置
        /// </summary>
        /// <param name="visionPos">轴的真实坐标</param>
        /// <param name="result">定位的结果</param>
        /// <returns>结果</returns>
        public AKRSPoint3D ConvertPixelToG0Pos(AKRSPoint3D visionPos, MatchResult result)
        {
            return this.bondModule.ConvertPixelToG0Pos(visionPos, result);
        }

        /// <summary>
        /// 检测数据的合理性
        /// 如果超出设置的软限位直接报警
        /// </summary>
        /// <param name="point">点位</param>
        /// <returns>结果</returns>
        public bool CheckSoftLimit(AKRSPoint3D point)
        {
            return this.bondModule.CheckSoftLimit(point);
        }

        /// <summary>
        /// 检测数据的合理性
        /// 如果超出设置的软限位直接报警
        /// </summary>
        /// <param name="point">点位</param>
        /// <returns>结果</returns>
        public bool CheckSoftLimit(AKRSPoint4D point)
        {
            return this.bondModule.CheckSoftLimit(point);
        }

        /// <summary>
        /// 获取相机像素比
        /// </summary>
        /// <returns>结果</returns>
        public double GetCameraRatio()
        {
            return this.bondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(new AKRSPoint3D(1225, 1024, 0))
                .X;
        }

        /// <summary>
        /// 获取焊头安全高度
        /// </summary>
        /// <returns>安全高度</returns>
        public double GetBondheadSafeLevel(AKRSPoint4D targetPos)
        {
            // 获取当前平台上的TU
            TransportUnit transportUnit =
                TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

            // 当前位置
            AKRSPoint3D curPos = this.bondModule.Get3DRealPosition();

            // 安全高度转到G0坐标系
            double safeHeight = this.bondModule.BondCoordinateSystem
                .SelfPosToG0(this.bondDevicePara.BondHeadParam.AxisSafePos).Z;

            if (transportUnit != null)
            {
                // 如果固晶区点胶气缸弹下来，则用点胶气缸的高度
                if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense
                    && System2Domain.GetInstance().S2DispenseController.IsDispenseHeightMeasurementOpen()
                    && System2Domain.GetInstance().BondProgram.S2DispenserProgram.Dispenser != null)
                {
                    safeHeight = transportUnit.CoordinateSystem.SelfPosToG0(new AKRSPoint3D()).Z
                                 + ProductConfiguration.GetInstance().TransportUnitConfig.SafeHeight - System2Domain
                                     .GetInstance().S2DispenseController.DistanceForBondHandAndDispenser.Z;
                }
                else
                {
                    safeHeight = transportUnit.CoordinateSystem.SelfPosToG0(new AKRSPoint3D()).Z
                                 + ProductConfiguration.GetInstance().TransportUnitConfig.SafeHeight
                                 + (this.bondModule.BondHead.CurrentNozzle?.MeasureHeightOffset ?? 0);
                }

                safeHeight = this.bondModule.ConvertG0ToMachinePos(safeHeight);

                if (safeHeight > this.bondModule.BondHead.AxisZ.AxisSetPara.PLimit
                    || safeHeight < this.bondModule.BondHead.AxisZ.AxisSetPara.NLimit)
                {
                    throw new Exception("载具安全高度过高，超出轴极限，请重新设置载具的安全高度");
                }
            }
            else
            {
                safeHeight = this.bondModule.ConvertG0ToMachinePos(safeHeight);
            }

            if (Machine.GetInstance().IsWorking() == false)
            {
                // 非工作状态安全高度置为轴安全高度
                safeHeight = this.bondDevicePara.BondHeadParam.AxisSafePos.Z;
            }

            if (MachineHardwareConfiguration.GetInstance().IsStaticWaffleConfigrated)
            {
                AKRSPoint3D avoidPos = this.bondModule.ConvertG0ToMachinePos(
                    WaferSubDevicePara.GetInstance().WaferTableDevicePara.BondAvoidStaticWafflePos);

                if (targetPos.Y >= avoidPos.Y || curPos.Y >= avoidPos.Y)
                {
                    double nozzleOffset = this.bondModule.BondHead.CurrentNozzle != null
                                              ? this.bondModule.BondHead.CurrentNozzle.MeasureHeightOffset
                                              : 0;

                    double staticWaffleLevel = avoidPos.Z + nozzleOffset;

                    // 静态华夫盒高度高于安全高度时，安全高度提升到静态华夫盒高度
                    safeHeight = staticWaffleLevel > safeHeight ? staticWaffleLevel : safeHeight;
                }
            }

            return safeHeight;
        }

        /// <summary>
        /// 获取焊头安全高度
        /// </summary>
        /// <returns>安全高度</returns>
        public double GetBondheadSafeLevel(AKRSPoint3D targetPos)
        {
            // 获取当前平台上的TU
            TransportUnit transportUnit =
                TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

            // 当前位置
            AKRSPoint3D curPos = this.bondModule.Get3DRealPosition();

            // 安全高度转到G0坐标系
            double safeHeight = this.bondModule.BondCoordinateSystem
                .SelfPosToG0(this.bondDevicePara.BondHeadParam.AxisSafePos).Z;

            if (transportUnit != null)
            {
                // 如果固晶区点胶气缸弹下来，则用点胶气缸的高度
                if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense
                    && System2Domain.GetInstance().S2DispenseController.IsDispenseHeightMeasurementOpen()
                    && System2Domain.GetInstance().BondProgram.S2DispenserProgram.Dispenser != null)
                {
                    safeHeight = transportUnit.CoordinateSystem.SelfPosToG0(new AKRSPoint3D()).Z
                                 + ProductConfiguration.GetInstance().TransportUnitConfig.SafeHeight - System2Domain
                                     .GetInstance().S2DispenseController.DistanceForBondHandAndDispenser.Z;
                }
                else
                {
                    safeHeight = transportUnit.CoordinateSystem.SelfPosToG0(new AKRSPoint3D()).Z
                                 + ProductConfiguration.GetInstance().TransportUnitConfig.SafeHeight
                                 + (this.bondModule.BondHead.CurrentNozzle?.MeasureHeightOffset ?? 0);
                }

                safeHeight = this.bondModule.ConvertG0ToMachinePos(safeHeight);
            }
            else
            {
                safeHeight = this.bondModule.ConvertG0ToMachinePos(safeHeight);
            }

            if (Machine.GetInstance().IsWorking() == false)
            {
                // 非工作状态安全高度置为轴安全高度
                safeHeight = this.bondDevicePara.BondHeadParam.AxisSafePos.Z;
            }

            if (MachineHardwareConfiguration.GetInstance().IsStaticWaffleConfigrated)
            {
                AKRSPoint3D avoidPos = this.bondModule.ConvertG0ToMachinePos(
                    WaferSubDevicePara.GetInstance().WaferTableDevicePara.BondAvoidStaticWafflePos);

                if (targetPos.Y >= avoidPos.Y || curPos.Y >= avoidPos.Y)
                {
                    double nozzleOffset = this.bondModule.BondHead.CurrentNozzle != null
                                              ? this.bondModule.BondHead.CurrentNozzle.MeasureHeightOffset
                                              : 0;

                    double staticWaffleLevel = avoidPos.Z + nozzleOffset;

                    // 静态华夫盒高度高于安全高度时，安全高度提升到静态华夫盒高度
                    safeHeight = staticWaffleLevel > safeHeight ? staticWaffleLevel : safeHeight;
                }
            }

            if (safeHeight > this.bondModule.BondHead.AxisZ.AxisSetPara.PLimit
                || safeHeight < this.bondModule.BondHead.AxisZ.AxisSetPara.NLimit)
            {
                throw new Exception("载具安全高度过高，超出轴极限，请重新设置载具的安全高度");
            }

            return safeHeight;
        }

        /// <summary>
        /// 获取焊头安全高度
        /// </summary>
        /// <returns>安全高度</returns>
        public double GetBondheadSafeLevel(AKRSPoint2D targetPos)
        {
            // 获取当前平台上的TU
            TransportUnit transportUnit =
                TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

            // 当前位置
            AKRSPoint3D curPos = this.bondModule.Get3DRealPosition();

            // 安全高度转到G0坐标系
            double safeHeight = this.bondModule.BondCoordinateSystem
                .SelfPosToG0(this.bondDevicePara.BondHeadParam.AxisSafePos).Z;

            if (transportUnit != null)
            {
                // 如果固晶区点胶气缸弹下来，则用点胶气缸的高度
                if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense
                    && System2Domain.GetInstance().S2DispenseController.IsDispenseHeightMeasurementOpen()
                    && System2Domain.GetInstance().BondProgram.S2DispenserProgram.Dispenser != null)
                {
                    safeHeight = transportUnit.CoordinateSystem.SelfPosToG0(new AKRSPoint3D()).Z
                                 + ProductConfiguration.GetInstance().TransportUnitConfig.SafeHeight - System2Domain
                                     .GetInstance().S2DispenseController.DistanceForBondHandAndDispenser.Z;
                }
                else
                {
                    safeHeight = transportUnit.CoordinateSystem.SelfPosToG0(new AKRSPoint3D()).Z
                                 + ProductConfiguration.GetInstance().TransportUnitConfig.SafeHeight
                                 + (this.bondModule.BondHead.CurrentNozzle?.MeasureHeightOffset ?? 0);
                }

                safeHeight = this.bondModule.ConvertG0ToMachinePos(safeHeight);

            }
            else
            {
                safeHeight = this.bondModule.ConvertG0ToMachinePos(safeHeight);
            }

            if (Machine.GetInstance().IsWorking() == false)
            {
                // 非工作状态安全高度置为轴安全高度
                safeHeight = this.bondDevicePara.BondHeadParam.AxisSafePos.Z;
            }

            if (MachineHardwareConfiguration.GetInstance().IsStaticWaffleConfigrated)
            {
                AKRSPoint3D avoidPos = this.bondModule.ConvertG0ToMachinePos(
                    WaferSubDevicePara.GetInstance().WaferTableDevicePara.BondAvoidStaticWafflePos);

                if (targetPos.Y >= avoidPos.Y || curPos.Y >= avoidPos.Y)
                {
                    double nozzleOffset = this.bondModule.BondHead.CurrentNozzle != null
                                              ? this.bondModule.BondHead.CurrentNozzle.MeasureHeightOffset
                                              : 0;

                    double staticWaffleLevel = avoidPos.Z + nozzleOffset;

                    // 静态华夫盒高度高于安全高度时，安全高度提升到静态华夫盒高度
                    safeHeight = staticWaffleLevel > safeHeight ? staticWaffleLevel : safeHeight;
                }
            }


            if (safeHeight > this.bondModule.BondHead.AxisZ.AxisSetPara.PLimit
                || safeHeight < this.bondModule.BondHead.AxisZ.AxisSetPara.NLimit)
            {
                throw new Exception("载具安全高度过高，超出轴极限，请重新设置载具的安全高度");
            }

            return safeHeight;
        }

        /// <summary>
        /// 获取焊头安全高度
        /// </summary>
        /// <returns>安全高度</returns>
        public double GetBondheadSafeLevel()
        {
            // 获取当前平台上的TU
            TransportUnit transportUnit =
                TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

            // 当前位置
            AKRSPoint3D curPos = this.bondModule.Get3DRealPosition();

            // 安全高度转到G0坐标系
            double safeHeight = this.bondModule.BondCoordinateSystem
                .SelfPosToG0(this.bondDevicePara.BondHeadParam.AxisSafePos).Z;

            if (transportUnit != null)
            {
                // 如果固晶区点胶气缸弹下来，则用点胶气缸的高度
                if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense
                    && System2Domain.GetInstance().S2DispenseController.IsDispenseHeightMeasurementOpen()
                    && System2Domain.GetInstance().BondProgram.S2DispenserProgram.Dispenser != null)
                {
                    safeHeight = transportUnit.CoordinateSystem.SelfPosToG0(new AKRSPoint3D()).Z
                                 + ProductConfiguration.GetInstance().TransportUnitConfig.SafeHeight - System2Domain
                                     .GetInstance().S2DispenseController.DistanceForBondHandAndDispenser.Z;
                }
                else
                {
                    safeHeight = transportUnit.CoordinateSystem.SelfPosToG0(new AKRSPoint3D()).Z
                                 + ProductConfiguration.GetInstance().TransportUnitConfig.SafeHeight
                                 + (this.bondModule.BondHead.CurrentNozzle?.MeasureHeightOffset ?? 0);
                }

                safeHeight = this.bondModule.ConvertG0ToMachinePos(safeHeight);
            }
            else
            {
                safeHeight = this.bondModule.ConvertG0ToMachinePos(safeHeight);
            }

            // 静态华夫盒
            if (MachineHardwareConfiguration.GetInstance().IsStaticWaffleConfigrated)
            {
                AKRSPoint3D avoidPos = this.bondModule.ConvertG0ToMachinePos(
                    WaferSubDevicePara.GetInstance().WaferTableDevicePara.BondAvoidStaticWafflePos);

                if (curPos.Y >= avoidPos.Y)
                {
                    double nozzleOffset = this.bondModule.BondHead.CurrentNozzle != null
                                              ? this.bondModule.BondHead.CurrentNozzle.MeasureHeightOffset
                                              : 0;

                    double staticWaffleLevel = avoidPos.Z + nozzleOffset;

                    // 静态华夫盒高度高于安全高度时，安全高度提升到静态华夫盒高度
                    safeHeight = staticWaffleLevel > safeHeight ? staticWaffleLevel : safeHeight;
                }
            }

            if (safeHeight > this.bondModule.BondHead.AxisZ.AxisSetPara.PLimit
                || safeHeight < this.bondModule.BondHead.AxisZ.AxisSetPara.NLimit)
            {
                throw new Exception("载具安全高度过高，超出轴极限，请重新设置载具的安全高度");
            }

            return safeHeight;
        }

        /// <summary>
        /// 获取Bond空间位置
        /// </summary>
        /// <returns>坐标</returns>
        public AKRSPoint4D Get4DRealPosition()
        {
            return this.bondModule.Get4DRealPosition();
        }

        /// <summary>
        /// 获取Bond空间位置
        /// </summary>
        /// <returns>坐标</returns>
        public AKRSPoint3D Get3DRealPosition()
        {
            return this.bondModule.Get3DRealPosition();
        }

        /// <summary>
        /// 获取Bond 平面位置
        /// </summary>
        /// <returns>位置</returns>
        public AKRSPoint2D Get2DRealPosition()
        {
            return this.bondModule.Get2DRealPosition();
        }

        /// <summary>
        /// 获取平面位置的G0坐标
        /// </summary>
        /// <returns>位置</returns>
        public AKRSPoint3D GetG0RealPosition()
        {
            return this.bondModule.GetG0RealPosition();
        }

        /// <summary>
        /// 获取空间位置的G0坐标
        /// </summary>
        /// <returns>位置</returns>
        public AKRSPoint2D Get2DG0RealPosition()
        {
            return new AKRSPoint2D(this.bondModule.GetG0RealPosition().X, this.bondModule.GetG0RealPosition().Y);
        }

        /// <summary>
        /// 获取X轴坐标
        /// </summary>
        /// <returns>坐标</returns>
        public double GetAxisXRealPos()
        {
            return this.bondModule.BondAxisX.GetCmdPosition();
        }

        /// <summary>
        /// 获取Y轴坐标
        /// </summary>
        /// <returns>坐标</returns>
        public double GetAxisYRealPos()
        {
            return this.bondModule.BondAxisY.GetCmdPosition();
        }

        /// <summary>
        /// 将像素offset转到worldOffset(G0)
        /// </summary>
        /// <param name="results">定位结果</param>
        /// <returns>像素差值对应的物理差值</returns>
        public AKRSPoint3D MatchResultToWorld(MatchResult result)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork || MachineStateModel.GetInstance().IsDryCycle)
            {
                return new AKRSPoint3D();
            }

            AKRSPoint3D point1 =
                this.bondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(
                    new AKRSPoint3D(result.CenterX, result.CenterY, 0));
            AKRSPoint3D point2 =
                this.bondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(new AKRSPoint3D(1224, 1024, 0));
            return point2 - point1;
        }

        /// <summary>
        /// Bond模组是否在目标位置附近
        /// </summary>
        /// <param name="pos">位置</param>
        /// <returns>结果</returns>
        public bool IsBondModuleAtG0Pos(AKRSPoint3D pos)
        {
            AKRSPoint3D curPos = this.GetG0RealPosition();

            AKRSPoint3D distance = curPos - pos;

            if (Math.Abs(distance.X) < 0.2 && Math.Abs(distance.Y) < 0.2 && Math.Abs(distance.Z) < 0.2)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Bond模组是否在目标位置附近
        /// </summary>
        /// <param name="pos">位置</param>
        /// <returns>结果</returns>
        public bool IsBondModuleAtG0Pos(AKRSPoint2D pos)
        {
            AKRSPoint2D curPos = this.Get2DG0RealPosition();

            AKRSPoint2D distance = curPos - pos;

            if (Math.Abs(distance.X) < 0.2 && Math.Abs(distance.Y) < 0.2)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Bond模组是否在目标位置附近(轴坐标)
        /// </summary>
        /// <param name="pos">位置</param>
        /// <returns>结果</returns>
        public bool IsBondModuleAtAxisPos(AKRSPoint3D pos)
        {
            AKRSPoint3D curPos = this.Get3DRealPosition();

            AKRSPoint3D distance = curPos - pos;

            if (Math.Abs(distance.X) < 0.2 && Math.Abs(distance.Y) < 0.2 && Math.Abs(distance.Z) < 0.2)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Bond模组是否在目标位置附近
        /// </summary>
        /// <param name="pos">位置</param>
        /// <returns>结果</returns>
        public bool IsBondModuleAtAxisPos(AKRSPoint2D pos)
        {
            AKRSPoint2D curPos = this.Get2DRealPosition();

            AKRSPoint2D distance = curPos - pos;

            if (Math.Abs(distance.X) < 0.2 && Math.Abs(distance.Y) < 0.2)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 连续插补
        /// </summary>
        /// <param name="posArr">位置数组</param>
        /// <param name="velArr">速度数组</param>
        public void MoveContinuousAbsolute(double[] posArr, double[] velArr)
        {
            // 核号即卡号
            short coreTemp = (short)this.bondModule.BondHead.AxisZ.CardNum;
            short rtn;
            short axisNum = (short)this.bondModule.BondHead.AxisZ.AxisNum;

            short listTemp = 1;

            GTN.mc.TAxisMotionConstraint[] axisMotionConstraint = new GTN.mc.TAxisMotionConstraint[24];
            GTN.mc.TListInfo listInfo = new GTN.mc.TListInfo();
            GTN.mc.TListInfo listInfoNull = new GTN.mc.TListInfo();
            listInfo.reserve1 = new short[2];
            listInfo.reserve2 = new short[3];
            listInfo.reserve3 = new double[4];
            listInfo.reserve1 = new short[2];
            listInfo.reserve2 = new short[3];
            listInfo.reserve3 = new double[4];
            listInfoNull.reserve1 = new short[2];
            listInfoNull.reserve2 = new short[3];
            listInfoNull.reserve3 = new double[4];
            GTN.mc.TMoveContinuousPrmsmooth moveContinuousAbsolutePrm = new GTN.mc.TMoveContinuousPrmsmooth();
            GTN.mc.TVelprofileModeSmooth smooth = new GTN.mc.TVelprofileModeSmooth();

            //声明结构体数组长度
            GTN.mc.TProfileScale[] scale = new GTN.mc.TProfileScale[24];
            GTN.mc.TProfileScale[] scaleRead = new GTN.mc.TProfileScale[24];
            GTN.mc.TWaitTimeout timeout = new GTN.mc.TWaitTimeout();
            double velMax = 2000;
            double accMax = 10000;
            double jerkMax = 100000;
            // 1、基本轴初始化
            //for (int i = 0;  i < 5; ++i)
            //{
            //memset(&scale[i],0,sizeof(scale[i]));

            scale[0].alpha = new double[4];
            scale[0].beta = new double[4];
            scale[0].reverse1 = new short[3];

            scale[0].count = 2;
            scale[0].alpha[0] = 1; // 脉冲当量，alpha可以认为是mm的单位，beta是脉冲的单位。beta / alpha
            scale[0].beta[0] = 10000;
            scale[0].alpha[1] = 1;
            scale[0].beta[1] = 1;

            // 设置脉冲当量，毫米 对应的 脉冲数
            rtn = GTN.mc.GTN_SetAxisScale(coreTemp, axisNum, ref scale[0], ref listInfoNull);
            // rtn = GTN.mc.GTN_GetAxisScale(coreTemp, axisNum, out scaleRead[0]);
            axisMotionConstraint[0].reserve1 = new short[3];
            axisMotionConstraint[0].reserve2 = new double[8];
            axisMotionConstraint[0].velMax = velMax; // 单位：mm/s   或者 度/s
            axisMotionConstraint[0].accMax = accMax; // 单位：mm/s^2 或者 度/s^2
            axisMotionConstraint[0].decMax = accMax; // 单位：mm/s^2 或者 度/s^2
            axisMotionConstraint[0].jerkMax = jerkMax; // 单位：mm/s^3 或者 度/s^3
            axisMotionConstraint[0].dvMax = 10; // 单位：mm/s   或者 度/s   轴的最大速度跳变量
            rtn = GTN.mc.GTN_SetAxisMotionConstraint(coreTemp, axisNum, ref axisMotionConstraint[0], ref listInfoNull);
            // rtn = GTN.mc.GTN_GetAxisMotionConstraint(coreTemp, axisNum, out axisMotionConstraint[0]);

            // }

            listInfo.modal = 1;
            listInfo.segNum = 0;
            listInfo.list = listTemp;
            rtn = GTN.mc.GTN_StopCommandList(coreTemp, listTemp, 0, ref listInfo);
            rtn = GTN.mc.GTN_ClearCommandListData(coreTemp, listTemp, ref listInfoNull);
            smooth.reserve = new double[18];


            moveContinuousAbsolutePrm.acc = velArr[0] * 10;
            moveContinuousAbsolutePrm.dec = velArr[0] * 10;
            moveContinuousAbsolutePrm.overrideSelect = 0;
            moveContinuousAbsolutePrm.velProfileMode = GTN.mc.VEL_PROFILE_MODE_TRAP; //平滑模式  
            //moveContinuousAbsolutePrm.velProfile.smooth.accTime = 10;// 加速度变化时间，类似Trap模式的smoothTime
            //moveContinuousAbsolutePrm.velProfile.smooth.k = 0;// 形态系数,保留
            // 第1段终点速度为 1，快下，最大速度velOffsetTemp,
            listInfo.modal = 1;
            listInfo.segNum++;
            moveContinuousAbsolutePrm.vel = velArr[0];

            if (posArr[0] >= posArr[1])
            {
                moveContinuousAbsolutePrm.velEnd = Math.Sqrt(Math.Abs(posArr[1] - posArr[0]) * velArr[1] * 10);
            }
            else
            {
                moveContinuousAbsolutePrm.velEnd = /*Math.Sqrt(Math.Abs(posArr[1]- posArr[0])* velArr[1]*10)*/
                    velArr[1];
            }


            moveContinuousAbsolutePrm.pos = posArr[0];

            //A-B
            rtn = GTN.mc.GTN_MoveContinuousAbsolute(
                coreTemp,
                axisNum,
                ref moveContinuousAbsolutePrm,
                ref listInfo,
                0); //第一段快下

            // 低速探测，最大速度为velOffsetTemp / 2，终点速度为0
            listInfo.segNum++;

            //加速度
            moveContinuousAbsolutePrm.acc = velArr[1] * 10;
            moveContinuousAbsolutePrm.dec = velArr[1] * 10;
            moveContinuousAbsolutePrm.overrideSelect = 0;
            moveContinuousAbsolutePrm.velProfileMode = GTN.mc.VEL_PROFILE_MODE_TRAP; //平滑模式  
            //BCSpeed
            moveContinuousAbsolutePrm.vel = velArr[1];
            moveContinuousAbsolutePrm.velEnd = 0;
            moveContinuousAbsolutePrm.pos = posArr[1];
            listInfo.modal = 1;

            //B-C
            rtn = GTN.mc.GTN_MoveContinuousAbsolute(
                coreTemp,
                axisNum,
                ref moveContinuousAbsolutePrm,
                ref listInfo,
                0); //第二段慢速
            do
            {
                rtn = GTN.mc.GTN_CommandListDataEnd(coreTemp, listTemp);
            }
            while (0 != rtn);

            GTN.mc.TCommandListStatus stat;
            listInfo.list = 0;
            rtn = GTN.mc.GTN_StartCommandList(coreTemp, listTemp, ref listInfo);

            GTN.mc.TCommandListStatus Stat = new GTN.mc.TCommandListStatus();
            short sRtn;
            do
            {
                sRtn = GTN.mc.GTN_GetCommandListStatus(coreTemp, listTemp, out Stat);
            }
            while (0 != Stat.execute); //等待指令流执行结束

            rtn = GTN.mc.GTN_StopCommandList(coreTemp, listTemp, 0, ref listInfoNull);
            //sRtn = GTN.mc.GTN_GetCommandListStatus(coreTemp, listTemp, out Stat);
            rtn = GTN.mc.GTN_ClearCommandListData(coreTemp, listTemp, ref listInfoNull);
            //sRtn = GTN.mc.GTN_GetCommandListStatus(coreTemp, listTemp, out Stat);

        }

        /// <summary>
        /// 等待激光干涉尺预热完成
        /// </summary>
        /// <returns>线程</returns>
        public async Task WaitLaserEncoderWarmupAsync()
        {
            // 如果不配置，直接返回
            if (!MachineHardwareConfiguration.GetInstance().IsLaserEncoderConfigured)
            {
                return;
            }

            var sp = Stopwatch.StartNew();
            while (!this.bondModule.LaserEncoder.GetWarmupStatus())
            {
                await Task.Delay(1000); // 异步等待，不阻塞线程

                if (sp.Elapsed.TotalSeconds > 242)
                {
                    throw new TimeoutException("激光干涉尺预热超时！");
                }
            }
        }
    }
}
