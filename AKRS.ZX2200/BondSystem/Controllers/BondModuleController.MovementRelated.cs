using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionControllerInterface;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.SupportFeature.Statistics;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem;
using AKRS.ZX2200.TransportUnitSystem.Controls.Setting;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using ch.etel.edi.dsa.v40;
using log4net.Core;
using PostSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace AKRS.ZX2200.BondSystem.Controllers
{
    /// <summary>
    /// Bond模组控制器，存放跟运动相关的方法
    ///  方法里面没有标注G0默认走的是轴坐标
    /// </summary>
    public partial class BondModuleController
    {
        /// <summary>
        /// 安全移动XYZ轴到指定位置
        /// </summary>
        /// <param name="point3D">位置</param>
        public void MoveSafeBondXYZ(AKRSPoint3D point3D)
        {
            // 当前位置
            AKRSPoint3D curPos = this.bondModule.Get3DRealPosition();
            double curLevel = curPos.Z;

            #region 旧

            //// Z轴零点为安全位置
            //double safeHeight = this.bondModule.BondCoordinateSystem.SelfPosToG0(this.bondDevicePara.BondHeadParam.AxisSafePos).Z;


            //// 获取当前平台上的TU
            //TransportUnit transportUnit = TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

            //if (transportUnit != null)
            //{
            //    // 如果固晶区点胶气缸弹下来，则用点胶气缸的高度
            //    if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense
            //    && System2Domain.GetInstance().S2DispenseController.IsDispenseHeightMeasurementOpen()
            //    && System2Domain.GetInstance().BondProgram.S2DispenserProgram.Dispenser != null)
            //    {
            //        safeHeight = transportUnit.CoordinateSystem.SelfPosToG0(new AKRSPoint3D()).Z
            //                     + ProductConfiguration.GetInstance().TransportUnitConfig.SafeHeight - System2Domain
            //                         .GetInstance().S2DispenseController.DistanceForBondHandAndDispenser.Z;
            //    }
            //    else
            //    {
            //        safeHeight = transportUnit.CoordinateSystem.SelfPosToG0(new AKRSPoint3D()).Z
            //                     + ProductConfiguration.GetInstance().TransportUnitConfig
            //                         .SafeHeight + (this.bondModule.BondHead.CurrentNozzle?.MeasureHeightOffset ?? 0);
            //    }

            //    safeHeight = this.bondModule.ConvertG0ToMachinePos(
            //        safeHeight);

            //    if (safeHeight > this.bondModule.BondHead.AxisZ.AxisSetPara.PLimit
            //        || safeHeight < this.bondModule.BondHead.AxisZ.AxisSetPara.NLimit)
            //    {
            //        throw new Exception("载具安全高度过高，超出轴极限，请重新设置载具的安全高度");
            //    }
            //}
            //else
            //{
            //    safeHeight = this.bondModule.ConvertG0ToMachinePos(
            //        safeHeight);
            //}

            //if (Machine.GetInstance().IsWorking() == false)
            //{
            //    // 非工作状态安全高度置为轴安全高度
            //    safeHeight = this.bondDevicePara.BondHeadParam.AxisSafePos.Z;
            //}

            //if (MachineHardwareConfiguration.GetInstance().IsStaticWaffleConfigrated)
            //{
            //    AKRSPoint3D avoidPos = this.bondModule.ConvertG0ToMachinePos(
            //        WaferSubDevicePara.GetInstance().WaferTableDevicePara.BondAvoidStaticWafflePos);

            //    if (point3D.Y >= avoidPos.Y || curPos.Y >= avoidPos.Y)
            //    {
            //        double nozzleOffset = this.bondModule.BondHead.CurrentNozzle != null
            //                                  ? this.bondModule.BondHead.CurrentNozzle.MeasureHeightOffset
            //                                  : 0;

            //        double staticWaffleLevel = avoidPos.Z + nozzleOffset;

            //        // 静态华夫盒高度高于安全高度时，安全高度提升到静态华夫盒高度
            //        safeHeight = staticWaffleLevel > safeHeight ? staticWaffleLevel : safeHeight;
            //    }
            //}

            #endregion

            // 安全高度
            double safeHeight = this.GetBondheadSafeLevel(point3D);

            this.bondModule.CheckSoftLimit(point3D);

            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            LogHelper.Post(
                Level.Info,
                $"系统2-轴准备移动，当前高度：{curLevel}，安全高度：{safeHeight}，目标位置：{point3D}",
                LogCategory.Bond,
                ViewType.InFileAndUI);

            if (curLevel >= safeHeight && point3D.Z >= safeHeight)
            {
                // 当前位置高于安全高度且目标位置位置也高于安全高度,先移动XY，再移动Z轴
                this.bondModule.MoveBondXY(point3D.X, point3D.Y);

                this.bondHeadController.MoveZAxis(point3D.Z);
            }
            else if (curLevel >= safeHeight && point3D.Z < safeHeight)
            {
                // 当前位置比安全位置高，目标位置比安全位置低，先移动XY，再移动Z轴
                this.bondModule.MoveBondXY(point3D.X, point3D.Y);

                this.bondHeadController.MoveZAxis(point3D.Z);
            }
            else if (curLevel < safeHeight && point3D.Z >= safeHeight)
            {
                // 当前位置比安全位置低，目标位置比安全位置高，先移动Z轴，再移动XY，这个地方应该是连续运动
                this.bondHeadController.MoveZAxis(point3D.Z);

                this.bondModule.MoveBondXY(point3D.X, point3D.Y);
            }
            else if (curLevel < safeHeight && point3D.Z < safeHeight)
            {
                // 如果当前位置和目标位置都比安全位置低，Z轴移动到安全位置，移动XY，最后移动Z轴
                this.bondHeadController.MoveZAxis(safeHeight);
                this.bondModule.MoveBondXY(point3D.X, point3D.Y);

                this.bondHeadController.MoveZAxis(point3D.Z);
            }
            else
            {
                // 理论上不会进到这个分支
                AKRSXtraMessageBox.Show(
                    $"致命错误!设备将退出工作，请联系供应商处理!",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                throw new Exception("安全移动XYZ轴到指定位置出错!");
            }
        }

        /// <summary>
        /// 安全移动XYZT轴到指定位置
        /// </summary>
        /// <param name="point4D">位置</param>
        public void MoveSafeBondXYZT(AKRSPoint4D point4D)
        {
            // 当前位置
            AKRSPoint3D curPos = this.bondModule.Get3DRealPosition();
            double curLevel = curPos.Z;

            #region 旧

            //// Z轴零点为安全位置
            //double safeHeight = this.bondModule.BondCoordinateSystem.SelfPosToG0(this.bondDevicePara.BondHeadParam.AxisSafePos).Z;

            //// 获取当前平台上的TU
            //TransportUnit transportUnit = TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;



            //if (transportUnit != null)
            //{
            //    // 如果固晶区点胶气缸弹下来，则用点胶气缸的高度
            //    if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense
            //    && System2Domain.GetInstance().S2DispenseController.IsDispenseHeightMeasurementOpen())
            //    {
            //        safeHeight = transportUnit.CoordinateSystem.SelfPosToG0(new AKRSPoint3D()).Z
            //                     + ProductConfiguration.GetInstance().TransportUnitConfig.SafeHeight - System2Domain
            //                         .GetInstance().S2DispenseController.DistanceForBondHandAndDispenser.Z;
            //    }
            //    else
            //    {
            //        safeHeight = transportUnit.CoordinateSystem.SelfPosToG0(new AKRSPoint3D()).Z
            //                     + ProductConfiguration.GetInstance().TransportUnitConfig
            //                         .SafeHeight + (this.bondModule.BondHead.CurrentNozzle?.MeasureHeightOffset ?? 0);
            //    }

            //    safeHeight = this.bondModule.ConvertG0ToMachinePos(
            //        safeHeight);

            //    if (safeHeight > this.bondModule.BondHead.AxisZ.AxisSetPara.PLimit
            //        || safeHeight < this.bondModule.BondHead.AxisZ.AxisSetPara.NLimit)
            //    {
            //        throw new Exception("载具安全高度过高，超出轴极限，请重新设置载具的安全高度");
            //    }
            //}
            //else
            //{
            //    safeHeight = this.bondModule.ConvertG0ToMachinePos(
            //        safeHeight);
            //}

            //if (Machine.GetInstance().IsWorking() == false)
            //{
            //    // 非工作状态安全高度置为轴安全高度
            //    safeHeight = this.bondDevicePara.BondHeadParam.AxisSafePos.Z;
            //}

            //if (MachineHardwareConfiguration.GetInstance().IsStaticWaffleConfigrated)
            //{
            //    AKRSPoint3D avoidPos = this.bondModule.ConvertG0ToMachinePos(
            //        WaferSubDevicePara.GetInstance().WaferTableDevicePara.BondAvoidStaticWafflePos);

            //    if (point4D.Y >= avoidPos.Y || curPos.Y >= avoidPos.Y)
            //    {
            //        double nozzleOffset = this.bondModule.BondHead.CurrentNozzle != null
            //                                  ? this.bondModule.BondHead.CurrentNozzle.MeasureHeightOffset
            //                                  : 0;

            //        double staticWaffleLevel = avoidPos.Z + nozzleOffset;

            //        // 静态华夫盒高度高于安全高度时，安全高度提升到静态华夫盒高度
            //        safeHeight = staticWaffleLevel > safeHeight ? staticWaffleLevel : safeHeight;
            //    }
            //}

            #endregion

            // 安全高度
            double safeHeight = this.GetBondheadSafeLevel(point4D);

            this.bondModule.CheckSoftLimit(point4D);

            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            System2RunTimeProvider.RecordTime($"系统2-轴移动", $"轴准备移动，当前高度：{curLevel}，安全高度：{safeHeight}，目标位置：{point4D}");

            if (curLevel >= safeHeight && point4D.Z >= safeHeight)
            {
                // 当前位置高于安全高度且目标位置位置也高于安全高度,先移动XY，再移动Z轴
                this.bondModule.MoveBondXYT(point4D.X, point4D.Y, point4D.T);

                this.bondHeadController.MoveZAxis(point4D.Z);
            }
            else if (curLevel >= safeHeight && point4D.Z < safeHeight)
            {
                // 当前位置比安全位置高，目标位置比安全位置低，先移动XY，再移动Z轴
                this.bondModule.MoveBondXYT(point4D.X, point4D.Y, point4D.T);

                this.bondHeadController.MoveZAxis(point4D.Z);
            }
            else if (curLevel < safeHeight && point4D.Z >= safeHeight)
            {
                // 当前位置比安全位置低，目标位置比安全位置高，先移动Z轴，再移动XY，这个地方应该是连续运动
                this.bondHeadController.MoveZAxis(point4D.Z);

                this.bondModule.MoveBondXYT(point4D.X, point4D.Y, point4D.T);
            }
            else if (curLevel < safeHeight && point4D.Z < safeHeight)
            {
                // 如果当前位置和目标位置都比安全位置低，Z轴移动到安全位置，移动XY，最后移动Z轴
                this.bondHeadController.MoveZAxis(safeHeight);
                this.bondModule.MoveBondXYT(point4D.X, point4D.Y, point4D.T);

                this.bondHeadController.MoveZAxis(point4D.Z);
            }
            else
            {
                // 理论上不会进到这个分支
                AKRSXtraMessageBox.Show(
                    $"致命错误!设备将退出工作，请联系供应商处理!",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                throw new Exception("安全移动XYZ轴到指定位置出错!");
            }
        }

        /// <summary>
        /// 移动到安全位
        /// </summary>
        public void MoveToSafePos()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            System2Domain.GetInstance().S2DispenseController.CloseDispenseHeightMeasurementCylinder();

            this.MoveSafeBondXYZ(this.bondDevicePara.BondHeadParam.AxisSafePos);
        }

        /// <summary>
        /// 移动到换吸嘴安全位
        /// </summary>
        public void MoveToChangeNozzleSafePos()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            this.bondHeadController.MoveBondZToChangeNozzleSafeHeight();

            this.MoveBondXY(BondDevicePara.GetInstance().BondHeadParam.ChangeNozzleSafePos.X, BondDevicePara.GetInstance().BondHeadParam.ChangeNozzleSafePos.Y);
        }

        /// <summary>
        /// 移动到上视旋转中心
        /// </summary>
        public void MoveToUpLookPos()
        {
            AKRSPoint3D aKRSPoint3D = this.bondDevicePara.CameraDevicePara.UpLookPos;

            // 移动到上视位置
            this.MoveSafeBondXYZ(aKRSPoint3D);
        }

        /// <summary>
        /// X轴移动到目标位
        /// </summary>
        /// <param name="runPara">位置、参数</param>
        /// <returns>结果</returns>
        public ExcuteResult MoveAxisX(MovePara runPara)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            return this.bondModule.BondAxisX.AbsoluteMove(runPara);
        }

        /// <summary>
        /// Y轴移动到目标位
        /// </summary>
        /// <param name="runPara">位置、参数</param>
        /// <returns>结果</returns>
        public ExcuteResult MoveAxisY(MovePara runPara)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            return this.bondModule.BondAxisY.AbsoluteMove(runPara);
        }

        /// <summary>
        /// 安全移动BondModule头XY轴到指定的位置
        /// </summary>
        /// <param name="posX">X轴位置</param>
        /// <param name="posY">Y轴位置</param>
        public void MoveSafeBondXY(double posX, double posY)
        {
            #region 旧

            //// Z轴零点为安全位置
            //double safeHeight = this.bondModule.BondCoordinateSystem.SelfPosToG0(this.bondDevicePara.BondHeadParam.AxisSafePos).Z;

            //// 获取当前平台上的TU
            //TransportUnit transportUnit = TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

            //// 当前位置
            //AKRSPoint3D curPos = this.bondModule.Get3DRealPosition();
            //double curLevel = curPos.Z;

            //if (transportUnit != null)
            //{
            //    safeHeight = transportUnit.CoordinateSystem.SelfPosToG0(new AKRSPoint3D()).Z
            //                 + ProductConfiguration.GetInstance().TransportUnitConfig.SafeHeight
            //                 + (this.bondModule.BondHead.CurrentNozzle?.MeasureHeightOffset ?? 0);
            //}

            //safeHeight = this.bondModule.ConvertG0ToMachinePos(safeHeight);

            //if (MachineHardwareConfiguration.GetInstance().IsStaticWaffleConfigrated)
            //{
            //    AKRSPoint3D avoidPos = this.bondModule.ConvertG0ToMachinePos(
            //        WaferSubDevicePara.GetInstance().WaferTableDevicePara.BondAvoidStaticWafflePos);

            //    if (posY >= avoidPos.Y || curPos.Y >= avoidPos.Y)
            //    {
            //        double nozzleOffset = this.bondModule.BondHead.CurrentNozzle != null
            //                                  ? this.bondModule.BondHead.CurrentNozzle.MeasureHeightOffset
            //                                  : 0;

            //        double staticWaffleLevel = avoidPos.Z + nozzleOffset;

            //        // 静态华夫盒高度高于安全高度时，安全高度提升到静态华夫盒高度
            //        safeHeight = staticWaffleLevel > safeHeight ? staticWaffleLevel : safeHeight;
            //    }
            //}

            #endregion

            // 当前位置
            AKRSPoint3D curPos = this.bondModule.Get3DRealPosition();
            double curLevel = curPos.Z;

            // 安全高度
            double safeHeight = this.GetBondheadSafeLevel(new AKRSPoint2D(posX, posY));

            // 限位检查
            this.bondModule.CheckSoftLimit(posX, posY);

            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            System2RunTimeProvider.RecordTime($"系统2-轴移动", $"轴准备移动，当前高度：{curLevel}，安全高度：{safeHeight}，目标位置X：{posX},目标位置Y：{posY}");

            // 判断位置，如果当前位置高于安全高度可以直接移动
            if (curLevel >= safeHeight)
            {
                this.bondModule.MoveBondXY(posX, posY);
            }
            else
            {
                this.bondHeadController.MoveZAxis(safeHeight);

                this.bondModule.MoveBondXY(posX, posY);

                this.bondHeadController.MoveZAxis(curLevel);
            }
        }

        /// <summary>
        /// 将Z轴移动到安全高度
        /// </summary>
        public void MoveBondZToTUSafeHeight()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            double safeHeight = this.bondModule.BondCoordinateSystem.SelfPosToG0(new AKRSPoint3D(0, 0, 0)).Z;

            // 获取当前平台上的TU
            TransportUnit transportUnit = TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

            if (transportUnit != null)
            {
                safeHeight = transportUnit.CoordinateSystem.SelfPosToG0(new AKRSPoint3D()).Z
                             + ProductConfiguration.GetInstance().TransportUnitConfig.SafeHeight
                             + (this.bondModule.BondHead.CurrentNozzle?.MeasureHeightOffset ?? 0);
            }

            double safeHeightInBond = this.bondModule.ConvertG0ToMachinePos(safeHeight);

            if (Machine.GetInstance().IsWorking() == false)
            {
                // 非工作状态安全高度置为Z轴原点
                safeHeightInBond = 0;
            }

            // 当前位置低于安全高度就抬升
            if (this.bondModule.BondHead.AxisZ.GetRealPosition() <= safeHeightInBond)
            {
                this.bondModule.BondHead.AxisZ.AbsoluteMove(safeHeightInBond, AccuracyMode.HighSpeed);
            }
        }

        /// <summary>
        /// 移动BondModule头XY轴到指定的位置（2轴同时移动）
        /// </summary>
        /// <param name="xPara">X运动设置</param>
        /// <param name="yPara">Y运动设置</param>
        public void MoveBondXY(MovePara xPara, MovePara yPara)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            this.bondModule.MoveBondXY(xPara, yPara);
        }

        /// <summary>
        /// 直接移动BondModule头XY轴到指定的位置（2轴同时移动）
        /// </summary>
        /// <param name="posX">X运动设置</param>
        /// <param name="posY">Y运动设置</param>
        public void MoveBondXY(double posX, double posY)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            this.bondModule.MoveBondXY(posX, posY);
        }

        /// <summary>
        /// 直接移动BondModule头XYZ轴到指定的位置（不安全）
        /// </summary>
        /// <param name="pos">位置</param>
        public void MoveBondXYZWithoutSafe(AKRSPoint3D pos)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            this.bondModule.MoveBondXYZ(pos);
        }

        /// <summary>
        /// 直接移动BondModule头XY轴到指定的G0位置
        /// </summary>
        /// <param name="posX">X运动设置</param>
        /// <param name="posY">Y运动设置</param>
        public void MoveBondXYToG0Pos(double posX, double posY)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            AKRSPoint3D targetPos = this.ConvertG0ToMachinePos(new AKRSPoint3D(posX, posY, 0));

            this.bondModule.MoveBondXY(targetPos.X, targetPos.Y);
        }

        /// <summary>
        /// 移动XYZT轴到指定位置
        /// </summary>
        /// <param name="point4D">位置</param>
        public void MoveBondXYZT(AKRSPoint4D point4D)
        {
            this.bondModule.MoveBondXYZT(point4D);
        }

        /// <summary>
        /// 通过判断Y轴位置安全移动XYZT轴到指定位置（去取片位会用到）
        /// </summary>
        /// <param name="point4D">位置</param>
        /// <param name="safeY">Y轴安全位置</param>
        /// <returns>结果</returns>
        [Obsolete]
        public ExcuteResult MoveBondXYZTBySafeY(AKRSPoint4D point4D, double safeY)
        {
            return this.bondModule.MoveBondXYZTBySafeY(point4D, safeY);
        }

        /// <summary>
        /// 通过判断Z轴位置安全移动XYZT轴到指定位置（去上视位会用到）
        /// </summary>
        /// <param name="point4D">位置</param>
        /// <param name="safeZ">Y轴安全位置</param>
        /// <returns>结果</returns>
        [Obsolete]
        public ExcuteResult MoveBondXYZTBySafeZ(AKRSPoint4D point4D, double safeZ)
        {
            return this.bondModule.MoveBondXYZTBySafeZ(point4D, safeZ);
        }

        /// <summary>
        /// bond xy 插补组 设置为静态 为了在所有对象中值存储一份
        /// </summary>
        private static DsaIpolGroup xyIGroup;

        /// <summary>
        /// 获取bond xy 插补组
        /// </summary>
        /// <returns> 插补组</returns>
        public DsaIpolGroup GetXYIpolGroup()
        {
            if (xyIGroup == null)
            {
                // 获取X，Y轴驱动
                DsaDrive moveDriveX = ((ETELAxis)this.bondModule.BondAxisX.AxisDrive).GetDrive();
                DsaDrive moveDriveY = ((ETELAxis)this.bondModule.BondAxisY.AxisDrive).GetDrive();

                // 获取ETEL轴
                AxisCard card = HardwareRepositoryService.GetHardware<AxisCard>("ETEL");

                // 获取控制器
                DsaMaster dsaMaster = ((ETELController)card.MotionController.MotionControllerDrive).GetMaster();

                // 设置群组
                xyIGroup = new DsaIpolGroup(moveDriveX, moveDriveY);

                // 设置控制者
                xyIGroup.setMaster(dsaMaster);
            }

            return xyIGroup;
        }

        /// <summary>
        /// bond xyz 插补组 设置为静态 为了在所有对象中值存储一份
        /// </summary>
        private static DsaIpolGroup xyzIGroup;

        /// <summary>
        /// 获取bond xyz 插补组
        /// </summary>
        /// <returns>插补组</returns>
        public DsaIpolGroup GetXYZIpolGroup()
        {
            if (xyzIGroup == null)
            {
                // 获取X，Y轴驱动
                DsaDrive moveDriveX = ((ETELAxis)this.bondModule.BondAxisX.AxisDrive).GetDrive();
                DsaDrive moveDriveY = ((ETELAxis)this.bondModule.BondAxisY.AxisDrive).GetDrive();
                DsaDrive moveDriveZ = ((ETELAxis)this.bondModule.BondHead.AxisZ.AxisDrive).GetDrive();

                // 获取ETEL轴
                AxisCard card = HardwareRepositoryService.GetHardware<AxisCard>("ETEL");

                // 获取控制器
                DsaMaster dsaMaster = ((ETELController)card.MotionController.MotionControllerDrive).GetMaster();

                // 设置群组
                xyzIGroup = new DsaIpolGroup(moveDriveX, moveDriveY, moveDriveZ);

                // 设置控制者
                xyzIGroup.setMaster(dsaMaster);
            }

            return xyzIGroup;
        }

        /// <summary>
        /// bond xyz 插补组 设置为静态 为了在所有对象中值存储一份
        /// </summary>
        private static DsaIpolGroup xyztIGroup;

        /// <summary>
        /// 获取bond xyz 插补组
        /// </summary>
        /// <returns>插补组</returns>
        public DsaIpolGroup GetXYZTIpolGroup()
        {
            if (xyztIGroup == null)
            {
                // 获取X，Y轴驱动
                DsaDrive moveDriveX = ((ETELAxis)this.bondModule.BondAxisX.AxisDrive).GetDrive();
                DsaDrive moveDriveY = ((ETELAxis)this.bondModule.BondAxisY.AxisDrive).GetDrive();
                DsaDrive moveDriveZ = ((ETELAxis)this.bondModule.BondHead.AxisZ.AxisDrive).GetDrive();
                DsaDrive moveDriveT = ((ETELAxis)this.bondModule.BondHead.AxisT.AxisDrive).GetDrive();

                // 获取ETEL轴
                AxisCard card = HardwareRepositoryService.GetHardware<AxisCard>("ETEL");

                // 获取控制器
                DsaMaster dsaMaster = ((ETELController)card.MotionController.MotionControllerDrive).GetMaster();

                // 设置群组
                xyztIGroup = new DsaIpolGroup(moveDriveX, moveDriveY, moveDriveZ, moveDriveT);

                // 设置控制者
                xyztIGroup.setMaster(dsaMaster);
            }

            return xyztIGroup;
        }

        /// <summary>
        /// 获取bond xyzt 插补组
        /// </summary>
        /// <returns>插补组</returns>
        public IAxisDrive[] GetBondIpolAxis()
        {
            IAxisDrive[] axisDrives = default;

            if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove == false)
            {
                axisDrives = new IAxisDrive[4]
                {
               this.bondModule.BondAxisX.AxisDrive,
              this.bondModule.BondAxisY.AxisDrive,
            this.bondModule.BondHead.AxisZ.AxisDrive,
            this.bondModule.BondHead.AxisT.AxisDrive
            };
            }
            else
            {
                axisDrives = new IAxisDrive[3]
                   {
               this.bondModule.BondAxisX.AxisDrive,
              this.bondModule.BondAxisY.AxisDrive,
            this.bondModule.BondHead.AxisZ.AxisDrive,
               };
            }

            return axisDrives;
        }

        /// <summary>
        /// 获取bond xyzt 插补组
        /// </summary>
        /// <returns>插补组</returns>
        public IAxisDrive[] GetXYIpolAxis()
        {
            IAxisDrive[] axisDrives = new IAxisDrive[2];

            axisDrives[0] = this.bondModule.BondAxisX.AxisDrive;
            axisDrives[1] = this.bondModule.BondAxisY.AxisDrive;

            return axisDrives;
        }

        /// <summary>
        /// 移动到G0中的某个位置
        /// </summary>
        /// <param name="posX">X</param>
        /// <param name="posY">Y</param>
        public void MoveToG0Pos(double posX, double posY)
        {
            posX = this.ConvertG0ToMachinePos(new AKRSPoint3D(posX, 0, posX)).X;
            posY = this.ConvertG0ToMachinePos(new AKRSPoint3D(0, posY, 0)).Y;

            this.MoveSafeBondXY(posX, posY);
        }

        /// <summary>
        /// 移动到取料位
        /// </summary>
        public void MoveToPickupPos()
        {
            AKRSPoint2D akrsPoint2D = CalibrateRunPara.GetInstance().BondHeadRotateCenterInWC;

            // 移动到取晶位
            this.MoveSafeBondXY(akrsPoint2D.X, akrsPoint2D.Y);
        }

        /// <summary>
        /// 将焊头移动到相机里看到的位置,Z轴默认移动到测高距离
        /// </summary>
        /// <param name="distance">距离</param>
        public void MoveToVisionPos(double distance = 0)
        {
            AKRSPoint3D curPos = this.bondModule.Get3DRealPosition();

            // 根据设备参数偏移
            AKRSPoint3D targetPos = new AKRSPoint3D()
            {
                X = curPos.X + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.X,
                Y = curPos.Y + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.Y,
                Z = curPos.Z + this.bondModule.BondHead.CurrentNozzle.MeasureHeightOffset
                                                         + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.Z + distance
            };

            // 移动到目标位置
            this.MoveSafeBondXYZ(targetPos);
        }

        /// <summary>
        /// 去抛料位置抛料
        /// </summary>
        /// <returns>结果</returns>
        public bool ThrowAction()
        {
            int curProportion = this.bondHeadController.ReadWeakBlowProportion();

            // 移动到抛料位
            this.MoveToThrowPos();

            // 打开吹气电磁阀
            this.bondHeadController.OpenToolBlowEle(this.bondDevicePara.BondHeadParam.ThrowBlowProportion);
            Thread.Sleep(this.bondDevicePara.BondHeadParam.ThrowBlowDelay);

            //// 关闭吹气电磁阀

            //// 检测吸嘴是否还有料
            //if (this.bondModule.BondHead.CheckVaccumSensor.GetInputValue())
            //{
            //    return false;
            //}

            //this.bondHeadController.CloseToolBlowEle();

            // 设置回弱吹
            this.bondHeadController.SetBlowProportion(curProportion);

            Thread.Sleep(this.bondDevicePara.BondHeadParam.ThrowBlowDelay);

            this.bondHeadController.CloseToolBlowEle();

            // 有些机台抛料位很低，怕撞抛料盒或者轨道，所以抬到安全高度
            this.bondHeadController.MoveBondZToSafePos();

            System2RunTimeProvider.IsMaterialOnBondhead = false;

            // 抛料次数增加
            StatisticsDomain.GetInstance().AbandonNumber++;

            return true;
        }

        /// <summary>
        /// 去抛料位置
        /// </summary>
        /// <returns>结果</returns>
        public bool MoveToThrowPos()
        {
            // 先抬Z轴
            AKRSPoint3D pos = this.ConvertG0ToMachinePos(this.bondDevicePara.BondHeadParam.ThrowPos);

            this.MoveSafeBondXYZ(pos);

            return true;
        }

        /// <summary>
        /// 相机去看相机中的某个位置
        /// </summary>
        /// <param name="point3D">G0中的位置</param>
        public void CameraMoveToG0Pos(AKRSPoint3D point3D)
        {
            AKRSPoint3D visionPoint3D = point3D - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

            this.MoveToG0Pos(visionPoint3D);
        }

        /// <summary>
        /// 移动到G0中的某个位置
        /// </summary>
        /// <param name="point3D">G0点位</param>
        public void MoveToG0Pos(AKRSPoint3D point3D)
        {
            // 将G0的点位转换到轴坐标
            AKRSPoint3D machinePos = this.bondModule.ConvertG0ToMachinePos(point3D);

            // 移动到位置
            this.MoveSafeBondXYZ(machinePos);
        }

        /// <summary>
        /// 移动到G0中的某个位置
        /// </summary>
        /// <param name="point3D">G0点位</param>
        public void MoveToG0PosWithoutSafe(AKRSPoint3D point3D)
        {
            // 将G0的点位转换到轴坐标
            AKRSPoint3D machinePos = this.bondModule.ConvertG0ToMachinePos(point3D);

            // 移动到位置
            this.MoveBondXYZWithoutSafe(machinePos);
        }

        /// <summary>
        /// 移动到G0中的某个位置，测试用，其他勿用
        /// </summary>
        /// <param name="point3D">G0点位</param>
        public void MoveToG0PosTest(AKRSPoint3D point3D)
        {
            // 将G0的点位转换到轴坐标
            AKRSPoint3D machinePos = this.bondModule.ConvertG0ToMachinePos(point3D);

            // Bond轴移动到安全位置
            this.bondHeadController.MoveZAxis(machinePos.Z);

            // 设置群组
            DsaIpolGroup iGroup = this.GetXYIpolGroup();

            // 开始插补
            iGroup.ipolBegin();

            // 设置为绝对坐标系 ，不设置绝对坐标系
            iGroup.ipolSetAbsMode(true, -1);

            iGroup.ipolTanVelocity(1);
            iGroup.ipolTanAcceleration(10);
            iGroup.ipolTanDeceleration(10);

            // 设置抖动时间，不知道什么意思
            iGroup.ipolTanJerkTime(0.06);

            iGroup.ipolLine(machinePos.X / 1000.0, machinePos.Y / 1000.0);

            // 等待插补结束
            iGroup.ipolWaitMovement(100000);

            // 退出插补模式
            iGroup.ipolEnd();
        }

        /// <summary>
        /// 去换静态华夫盒避让位
        /// </summary>
        public void MoveToChangeStaticWaffleAvoidancePos()
        {
            this.MoveToG0Pos(this.bondDevicePara.BondHeadParam.ChangeStaticWaffleAvoidancePos);
        }

        /// <summary>
        /// 去测高位置
        /// </summary>
        public void MoveToMeasureHeightPos()
        {
            double nozzleOffset = this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset;

            // 测高点正上方5mm
            AKRSPoint3D measureHeightPos = this.Get3DRealPosition()
                                           + this.bondDevicePara.BondHeadParam.HeadToCameraOffset
                                           + new AKRSPoint3D(0, 0, 5 + nozzleOffset);

            this.MoveSafeBondXYZ(measureHeightPos);
        }
    }
}
