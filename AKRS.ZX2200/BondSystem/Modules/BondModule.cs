using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.LogicHardware.Services;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AKRS.ZX2200.BondSystem.Modules
{
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
    using AKRS.Galaxy2.LogicHardware.Hardwares.DispenseControllers;
    using AKRS.Galaxy2.LogicHardware.Hardwares.LaserInterferometerEncoderControllers;
    using AKRS.Galaxy2.LogicHardware.HardWares.LaserMeasureHeightControllers;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.DispenseSystem.Modules;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using ch.etel.edi.dsa.v40;
    using System.Diagnostics;

    /// <summary>
    /// 固晶模组
    /// </summary>
    public class BondModule
    {
        /// <summary>
        /// BondX轴
        /// </summary>
        [JsonIgnore]
        public Axis BondAxisX => HardwareRepositoryService.GetHardware<Axis>("BondX");

        /// <summary>
        /// BondY轴
        /// </summary>
        [JsonIgnore]
        public Axis BondAxisY => HardwareRepositoryService.GetHardware<Axis>("BondY");

        /// <summary>
        /// 点胶控制阀
        /// </summary>
        public Electric DripElectric { get; set; } = HardwareRepositoryService.GetHardware<Electric>("点胶控制2");

        /// <summary>
        /// 邦头点胶胶量检测
        /// </summary>
        public Sensor 邦头点胶胶量检测 { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("邦头点胶胶量检测");

        /// <summary>
        /// 武藏点胶器
        /// </summary>
        public Dispenser DispenserControl { get; set; } = HardwareRepositoryService.GetHardware<Dispenser>("点胶器2");

        /// <summary>
        /// 蘸胶盘旋转电机
        /// </summary>
        private Axis PrintTool => HardwareRepositoryService.GetHardware<Axis>("焊头蘸胶T");

        /// <summary>
        /// 激光测高
        /// </summary>
        public LaserMeasureHeight LaserMeasureHeight { get; set; } = HardwareRepositoryService.GetHardware<LaserMeasureHeight>("Bond激光测高");

        /// <summary>
        /// 激光干涉尺
        /// </summary>
        public LaserEncoder LaserEncoder { get; set; } = HardwareRepositoryService.GetHardware<LaserEncoder>("激光干涉尺");

        /// <summary>
        /// 坐标转换系统
        /// </summary>
        [JsonIgnore]
        public GeneralCoordinateSystem BondCoordinateSystem =>
            (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name == "BondCoordinateSystem");

        /// <summary>
        /// 固晶域Bond相机的坐标系
        /// </summary>
        [JsonIgnore]
        public DependentCoordinateSystem BondCameraCoordinateSystem =>
            (DependentCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name == "BondCameraCoordinateSystem");

        /// <summary>
        /// Bond头
        /// </summary>
        [JsonIgnore]
        public BondHead BondHead { get; set; } = new BondHead();

        /// <summary>
        /// Bond相机
        /// </summary>
        [JsonIgnore]
        public AKRSCamera BondCamera => HardwareRepositoryService.GetHardware<AKRSCamera>("BOND相机");

        /// <summary>
        /// BondZ标定相机
        /// </summary>
        [JsonIgnore]
        public AKRSCamera BondZCamera => HardwareRepositoryService.GetHardware<AKRSCamera>("BondZ标定相机");

        /// <summary>
        /// Bond相机环光
        /// </summary>
        [JsonIgnore]
        public Light AmbientLightRed => HardwareRepositoryService.GetHardware<Light>("邦头三色环光-红");

        /// <summary>
        /// Bond相机环光
        /// </summary>
        [JsonIgnore]
        public Light AmbientLightGreen => HardwareRepositoryService.GetHardware<Light>("邦头三色环光-绿");

        /// <summary>
        /// Bond相机环光
        /// </summary>
        [JsonIgnore]
        public Light AmbientLightBlue => HardwareRepositoryService.GetHardware<Light>("邦头三色环光-蓝");

        /// <summary>
        /// Bond相机点光
        /// </summary>
        [JsonIgnore]
        public Light SpotLightRed => HardwareRepositoryService.GetHardware<Light>("邦头三色点光-红");

        /// <summary>
        /// Bond相机点光
        /// </summary>
        [JsonIgnore]
        public Light SpotLightGreen => HardwareRepositoryService.GetHardware<Light>("邦头三色点光-绿");

        /// <summary>
        /// Bond相机点光
        /// </summary>
        [JsonIgnore]
        public Light SpotLightBlue => HardwareRepositoryService.GetHardware<Light>("邦头三色点光-蓝");

        /// <summary>
        /// 相机硬件
        /// </summary>
        private PRHardware hardware;

        #region  方法

        /// <summary>
        /// 获取相机硬件
        /// </summary>
        /// <returns>硬件集合</returns>
        public PRHardware GetHardware()
        {
            if (this.hardware == null)
            {
                List<string> list;

                // 区分三色光和单色光
                if (MachineHardwareConfiguration.GetInstance().LightConfig == LightConfigEnum.TrichromaticLight)
                {
                    list = new List<string>
                               {
                                   this.SpotLightRed?.HardwareName,
                                   this.SpotLightGreen?.HardwareName,
                                   this.SpotLightBlue?.HardwareName,
                                   this.AmbientLightRed?.HardwareName,
                                   this.AmbientLightGreen?.HardwareName,
                                   this.AmbientLightBlue?.HardwareName
                               };
                }
                else
                {
                    list = new List<string>
                               {
                                   this.SpotLightRed.HardwareName,
                                   this.AmbientLightRed.HardwareName,
                               };
                }

                this.hardware = new PRHardware(
                    this.BondCamera.HardwareName,
                    this.BondAxisX.HardwareName,
                    this.BondAxisY.HardwareName,
                    this.BondHead.AxisZ.HardwareName,
                    list);
            }

            return this.hardware;
        }

        /// <summary>
        /// 获取Z-XY相机硬件
        /// </summary>
        /// <returns>硬件集合</returns>
        public PRHardware GetHardwareZWithXY()
        {
            VisionModule visionModule = new VisionModule();
            if (this.hardware == null)
            {
                List<string> list;

                // 区分三色光和单色光
                if (MachineHardwareConfiguration.GetInstance().LightConfig == LightConfigEnum.TrichromaticLight)
                {
                    list = new List<string>
                               {
                        visionModule.SpotLightRed?.HardwareName,
                                   visionModule.SpotLightGreed?.HardwareName,
                                   visionModule.SpotLightBlue?.HardwareName,
                                   visionModule.RingLightRed?.HardwareName,
                                   visionModule.RingLightGreed?.HardwareName,
                                   visionModule.RingLightBlue?.HardwareName,
                               };
                }
                else
                {
                    list = new List<string>
                               {
                                   visionModule.SpotLightRed?.HardwareName,
                                   visionModule.RingLightRed?.HardwareName,
                               };
                }

                this.hardware = new PRHardware(
                    this.BondZCamera.HardwareName,
                    this.BondAxisX.HardwareName,
                    this.BondAxisY.HardwareName,
                    this.BondHead.AxisZ.HardwareName,
                    list);
            }

            return this.hardware;
        }

        /// <summary>
        /// 相机设置硬件
        /// </summary>
        /// <param name="pRName">模版名称</param>
        public void SetHardware(string pRName)
        {
            // 寻找Pr模板
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(pRName);

            pREntity.SetHardware(this.GetHardware());
        }

        /// <summary>
        /// 获取Bond 平面位置
        /// </summary>
        /// <returns>位置</returns>
        public AKRSPoint2D Get2DRealPosition()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return new AKRSPoint2D();
            }

            double x = this.BondAxisX.GetCmdPosition();
            double y = this.BondAxisY.GetCmdPosition();
            AKRSPoint2D pos = new AKRSPoint2D(x, y);

            return pos;
        }

        /// <summary>
        /// 获取Bond 空间位置
        /// </summary>
        /// <returns>位置</returns>
        public AKRSPoint3D Get3DRealPosition()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return new AKRSPoint3D();
            }

            double x = this.BondAxisX.GetCmdPosition();
            double y = this.BondAxisY.GetCmdPosition();
            double z = this.BondHead.AxisZ.GetRealPosition();

            AKRSPoint3D pos = new AKRSPoint3D(x, y, z);

            return pos;
        }

        /// <summary>
        /// 获取Bond 空间位置
        /// </summary>
        /// <returns>位置</returns>
        public AKRSPoint4D Get4DRealPosition()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return new AKRSPoint4D();
            }

            double x = this.BondAxisX.GetCmdPosition();
            double y = this.BondAxisY.GetCmdPosition();
            double z = this.BondHead.AxisZ.GetRealPosition();
            double t = this.BondHead.AxisT.GetRealPosition();

            AKRSPoint4D pos = new AKRSPoint4D(x, y, z,t);

            return pos;
        }

        /// <summary>
        /// 获取空间位置的G0坐标
        /// </summary>
        /// <returns>位置</returns>
        public AKRSPoint3D GetG0RealPosition()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return new AKRSPoint3D();
            }

            double x = this.BondAxisX.GetCmdPosition();
            double y = this.BondAxisY.GetCmdPosition();
            double z = this.BondHead.AxisZ.GetRealPosition();

            AKRSPoint3D pos = new AKRSPoint3D(x, y, z);

            AKRSPoint3D g0AkrsPoint3D = this.ConvertMachineToG0Pos(pos);

            return g0AkrsPoint3D;
        }

        /// <summary>
        /// Bond坐标转成G0坐标
        /// </summary>
        /// <param name="pos">Bond坐标</param>
        /// <returns>G0坐标</returns>
        public AKRSPoint3D ConvertMachineToG0Pos(AKRSPoint3D pos)
        {
            if (pos == null)
            {
                return null;
            }

            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return new AKRSPoint3D();
            }

            return this.BondCoordinateSystem.ForwardConvertCoordinate(pos);
        }

        /// <summary>
        /// Bond坐标转成G0坐标
        /// </summary>
        /// <param name="pos">Bond坐标</param>
        /// <returns>G0坐标</returns>
        [Obsolete]
        public AKRSPoint2D ConvertMachineToG0Pos(AKRSPoint2D pos)
        {
            if (pos == null)
            {
                return null;
            }

            return this.BondCoordinateSystem.ForwardConvertCoordinate(pos);
        }

        /// <summary>
        /// BondZ坐标转成G0坐标
        /// </summary>
        /// <param name="pos">Bond坐标</param>
        /// <returns>G0坐标</returns>
        public double ConvertMachineToG0Pos(double pos)
        {
            return this.ConvertMachineToG0Pos(new AKRSPoint3D(0, 0, pos)).Z;
        }

        /// <summary>
        /// G0坐标转回Bond坐标
        /// </summary>
        /// <param name="pos">G0坐标</param>
        /// <returns>Bond坐标</returns>
        public AKRSPoint3D ConvertG0ToMachinePos(AKRSPoint3D pos)
        {
            if (pos == null)
            {
                return null;
            }

            return this.BondCoordinateSystem.BackConvertCoordinate(pos);
        }

        /// <summary>
        /// G0坐标转回Bond坐标
        /// </summary>
        /// <param name="pos">G0坐标</param>
        /// <returns>Bond坐标</returns>
        [Obsolete]
        public AKRSPoint4D ConvertG0ToMachinePos(AKRSPoint4D pos)
        {
            if (pos == null)
            {
                return null;
            }

            AKRSPoint3D point3DInG0 = new AKRSPoint3D(pos.X, pos.Y, pos.Z);

            AKRSPoint3D point3D = this.BondCoordinateSystem.BackConvertCoordinate(point3DInG0);

            return new AKRSPoint4D(point3D.X, point3D.Y, point3D.Z, pos.T);
        }

        /// <summary>
        /// G0坐标转回Bond坐标
        /// </summary>
        /// <param name="pos">G0坐标</param>
        /// <returns>Bond坐标</returns>
        [Obsolete]
        public AKRSPoint2D ConvertG0ToMachinePos(AKRSPoint2D pos)
        {
            if (pos == null)
            {
                return null;
            }

            return this.BondCoordinateSystem.BackConvertCoordinate(pos);
        }

        /// <summary>
        /// G0坐标转回Bond坐标
        /// </summary>
        /// <param name="pos">G0坐标</param>
        /// <returns>Bond坐标</returns>
        public double ConvertG0ToMachinePos(double pos)
        {
            AKRSPoint3D point = this.BondCoordinateSystem.BackConvertCoordinate(new AKRSPoint3D(0, 0, pos));

            return point.Z;
        }

        /// <summary>
        /// 移动BondModule头XY轴到指定的位置
        /// </summary>
        /// <param name="posX">X轴位置</param>
        /// <param name="posY">Y轴位置</param>
        public void MoveBondXY(double posX, double posY)
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            if (!this.IsReady())
            {
                return;
            }

            this.CheckSoftLimit(posX, posY);

            //MotionService.MoveAxesToTargetPosition(
            //       (this.BondAxisX, true, posX, AccuracyMode.HighAccuracy),
            //       (this.BondAxisY, true, posY, AccuracyMode.HighAccuracy));

            double velX = this.BondAxisX.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;


            double velY = this.BondAxisY.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            double accX = this.BondAxisX.AxisMovePara.ACC
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;
            double accY = this.BondAxisY.AxisMovePara.ACC
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;


            double decX = this.BondAxisX.AxisMovePara.DEC
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;
            double decY = this.BondAxisY.AxisMovePara.DEC
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            MovePara moveParaX = new MovePara { Vel = velX, Acc = accX, Dec = decX, Jerk = this.BondAxisY.AxisMovePara.Jerk, TargetPosition = posX };

            MovePara moveParaY = new MovePara { Vel = velY, Acc = accY, Dec = decY, Jerk = this.BondAxisY.AxisMovePara.Jerk, TargetPosition = posY };

            // 开始运动
            this.BondAxisX.SendAbsoluteMoveCommand(moveParaX);
            this.BondAxisY.SendAbsoluteMoveCommand(moveParaY);
           

            // 等轴到位
            MotionService.WaitAxesArrival(
                (this.BondAxisX, true, posX, AccuracyMode.HighAccuracy),
                (this.BondAxisY, true, posY, AccuracyMode.HighAccuracy));
        }

        /// <summary>
        /// Etel移动XXy
        /// </summary>
        public void MoveBondXYByEtel(double posX, double posY)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            DsaDrive moveDriveX = ((ETELAxis)this.BondAxisX.AxisDrive).GetDrive();
            DsaDrive moveDriveY = ((ETELAxis)this.BondAxisY.AxisDrive).GetDrive();

            // 获取ETEL轴
            AxisCard card = HardwareRepositoryService.GetHardware<AxisCard>("ETEL");

            // 获取控制器
            DsaMaster dsaMaster = ((ETELController)card.MotionController.MotionControllerDrive).GetMaster();

            // 设置群组
            DsaIpolGroup iGroup = new DsaIpolGroup(moveDriveX, moveDriveY);

            // 设置控制者
            iGroup.setMaster(dsaMaster);

            // 开始插补
            iGroup.ipolBegin();

            // 设置为绝对坐标系 ，不设置绝对坐标系
            iGroup.ipolSetAbsMode(true, -1);

            iGroup.ipolTanVelocity(0.8);
            iGroup.ipolTanAcceleration(6);
            iGroup.ipolTanDeceleration(6);

            // 设置抖动时间，不知道什么意思
            iGroup.ipolTanJerkTime(0.012);
            iGroup.ipolLine(
           posX / 1000.0,posY / 1000.0);

            // 等待插补结束
            iGroup.ipolWaitMovement(100000);

            iGroup.ipolEnd();

            stopwatch.Stop();

           // Console.WriteLine($"XY插补用时:{stopwatch.ElapsedMilliseconds}"); 
        }

        /// <summary>
        /// 移动BondModule头XY轴到指定的位置
        /// </summary>
        /// <param name="xPara">X运动设置</param>
        /// <param name="yPara">Y运动设置</param>
        public void MoveBondXY(MovePara xPara, MovePara yPara)
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            if (!this.IsReady())
            {
                return;
            }

            this.CheckSoftLimit(xPara.TargetPosition, yPara.TargetPosition);

            // 机器速度百分比
            xPara.Vel = xPara.Vel
                        * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;


            yPara.Vel = yPara.Vel
                        * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;


            this.BondAxisX.SendAbsoluteMoveCommand(xPara);
            this.BondAxisY.SendAbsoluteMoveCommand(yPara);

            MotionService.WaitAxesArrival(
                (this.BondAxisX, true, xPara.TargetPosition, AccuracyMode.HighAccuracy),
                (this.BondAxisY, true, yPara.TargetPosition, AccuracyMode.HighAccuracy));
        }

        /// <summary>
        /// 移动XYZ轴到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        public void MoveBondXYZT(AKRSPoint4D pos)
        {
            if (!this.IsReady())
            {
                return;
            }

            this.CheckSoftLimit(pos);

            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            //MotionService.MoveAxesToTargetPosition(
            //    (this.BondAxisX, true, pos.X, AccuracyMode.HighAccuracy),
            //    (this.BondAxisY, true, pos.Y, AccuracyMode.HighAccuracy),
            //    (this.BondHead.AxisZ, true, pos.Z, AccuracyMode.HighAccuracy));

            double velX = this.BondAxisX.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;


            double velY = this.BondAxisY.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            double velZ = this.BondHead.AxisZ.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            double velT = this.BondHead.AxisT.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            // 开始运动
            this.BondAxisX.SendAbsoluteMoveCommand(pos.X, velX);
            this.BondAxisY.SendAbsoluteMoveCommand(pos.Y, velY);
            this.BondHead.AxisZ.SendAbsoluteMoveCommand(pos.Z, velZ);
            this.BondHead.AxisT.SendAbsoluteMoveCommand(pos.T, velT);

            // 等轴到位
            MotionService.WaitAxesArrival(
                (this.BondAxisX, true, pos.X, AccuracyMode.HighAccuracy),
                (this.BondAxisY, true, pos.Y, AccuracyMode.HighAccuracy),
                (this.BondHead.AxisZ, true, pos.Z, AccuracyMode.HighAccuracy),
                (this.BondHead.AxisT, true, pos.T, AccuracyMode.HighAccuracy));
        }

        /// <summary>
        /// 通过判断Y轴位置安全移动XYZT轴到指定位置
        /// </summary>
        /// <param name="point4D">位置</param>
        /// <param name="safeY">Y轴安全位置</param>
        /// <returns>结果</returns>
        [Obsolete]
        public ExcuteResult MoveBondXYZTBySafeY(AKRSPoint4D point4D, double safeY)
        {
            double velX = this.BondAxisX.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            double velY = this.BondAxisY.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;


            double velZ = this.BondHead.AxisZ.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            double velT = this.BondHead.AxisT.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;


            // 开始运动
            this.BondAxisX.SendAbsoluteMoveCommand(point4D.X, velX);
            this.BondAxisY.SendAbsoluteMoveCommand(point4D.Y, velY);
            this.BondHead.AxisT.SendAbsoluteMoveCommand(point4D.T, velT);

            Stopwatch sp = Stopwatch.StartNew();

            while (true)
            {
                // todo;符号待确认
                if (this.BondAxisY.GetRealPosition() < safeY) 
                {
                    // Y轴安全开始移动Z轴
                    this.BondHead.AxisT.SendAbsoluteMoveCommand(point4D.Z, velZ);
                    break;
                }

                Thread.Sleep(1);

                // 超时
                if (sp.Elapsed.TotalSeconds > 10)
                {
                    return ExcuteResult.Exception;
                }
            }

            // 等轴到位
            MotionService.WaitAxesArrival(
                (this.BondAxisX, true, point4D.X, AccuracyMode.HighAccuracy),
                (this.BondAxisY, true, point4D.Y, AccuracyMode.HighAccuracy),
                (this.BondHead.AxisZ, true, point4D.Z, AccuracyMode.HighAccuracy),
                (this.BondHead.AxisT, true, point4D.T, AccuracyMode.HighAccuracy));

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 通过判断Z轴位置安全移动XYZT轴到指定位置
        /// </summary>
        /// <param name="point4D">位置</param>
        /// <param name="safeZ">Y轴安全位置</param>
        /// <returns>结果</returns>
        [Obsolete]
        public ExcuteResult MoveBondXYZTBySafeZ(AKRSPoint4D point4D, double safeZ)
        {
            double velX = this.BondAxisX.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            double velY = this.BondAxisY.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;


            double velZ = this.BondHead.AxisZ.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            double velT = this.BondHead.AxisT.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            // 当前z的位置
            double curZPos = this.BondHead.AxisZ.GetRealPosition();

            if (curZPos < safeZ)
            {
                // Z轴先去安全高度
                this.BondHead.AxisZ.SendAbsoluteMoveCommand(safeZ, velZ);
            }


            Stopwatch sp = Stopwatch.StartNew();
            while (true)
            {
                if (this.BondHead.AxisZ.GetRealPosition() >= safeZ)
                {
                    // 开始运动
                    this.BondAxisX.SendAbsoluteMoveCommand(point4D.X, velX);
                    this.BondAxisY.SendAbsoluteMoveCommand(point4D.Y, velY);
                    this.BondHead.AxisT.SendAbsoluteMoveCommand(point4D.T, velT);

                    break;
                }

                Thread.Sleep(1);

                if (sp.Elapsed.TotalSeconds > 10)
                {
                    return ExcuteResult.Exception;
                }
            }

            sp.Restart();
            while (true)
            {
                if (Math.Abs(this.BondAxisX.GetRealPosition() - point4D.X) < 2)
                {
                    // x轴安全开始移动Z轴
                    this.BondHead.AxisZ.SendAbsoluteMoveCommand(point4D.Z, velZ);
                    break;
                }

                Thread.Sleep(1);

                // 超时
                if (sp.Elapsed.TotalSeconds > 10)
                {
                    return ExcuteResult.Exception;
                }
            }

            // 等轴到位
            MotionService.WaitAxesArrival(
                (this.BondAxisX, true, point4D.X, AccuracyMode.HighAccuracy),
                (this.BondAxisY, true, point4D.Y, AccuracyMode.HighAccuracy),
                (this.BondHead.AxisZ, true, point4D.Z, AccuracyMode.HighAccuracy),
                (this.BondHead.AxisT, true, point4D.T, AccuracyMode.HighAccuracy));

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 移动XYT轴到指定位置
        /// </summary>
        /// <param name="posX">X坐标</param>
        /// <param name="posY">Y坐标</param>
        /// <param name="posT">Z坐标</param>
        public void MoveBondXYT(double posX, double posY, double posT)
        {
            if (!this.IsReady())
            {
                return;
            }

            this.CheckSoftLimit(new AKRSPoint4D(posX, posY, 0, posT));

            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            //MotionService.MoveAxesToTargetPosition(
            //    (this.BondAxisX, true, pos.X, AccuracyMode.HighAccuracy),
            //    (this.BondAxisY, true, pos.Y, AccuracyMode.HighAccuracy),
            //    (this.BondHead.AxisZ, true, pos.Z, AccuracyMode.HighAccuracy));

            double velX = this.BondAxisX.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            double velY = this.BondAxisY.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            double velT = this.BondHead.AxisT.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            // 开始运动
            this.BondAxisX.SendAbsoluteMoveCommand(posX, velX);
            this.BondAxisY.SendAbsoluteMoveCommand(posY, velY);
            this.BondHead.AxisT.SendAbsoluteMoveCommand(posT, velT);

            // 等轴到位
            MotionService.WaitAxesArrival(
                (this.BondAxisX, true, posX, AccuracyMode.HighAccuracy),
                (this.BondAxisY, true, posY, AccuracyMode.HighAccuracy),
                (this.BondHead.AxisT, true, posT, AccuracyMode.HighAccuracy));
        }


        /// <summary>
        /// 移动XYZ轴到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        public void MoveBondXYZ(AKRSPoint3D pos)
        {
            if (!this.IsReady())
            {
                return;
            }

            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            // 检测限位
            this.CheckSoftLimit(pos);

            double velZ = this.BondHead.AxisZ.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            this.BondHead.AxisZ.SendAbsoluteMoveCommand(pos.Z, velZ);

            // 等Z轴到位
            MotionService.WaitAxesArrival((this.BondHead.AxisZ, true, pos.Z, AccuracyMode.HighAccuracy));
            
            double velX = this.BondAxisX.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;


            double velY = this.BondAxisY.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            // 开始运动
            this.BondAxisX.SendAbsoluteMoveCommand(pos.X, velX);
            this.BondAxisY.SendAbsoluteMoveCommand(pos.Y, velY);
           
            // 等轴到位
            MotionService.WaitAxesArrival(
                (this.BondAxisX, true, pos.X, AccuracyMode.HighAccuracy),
                (this.BondAxisY, true, pos.Y, AccuracyMode.HighAccuracy));
        }

        /// <summary>
        /// 设置轴速度
        /// </summary>
        /// <param name="axisVels">轴</param>
        public void SetAxisVel(List<double> axisVels)
        {
            this.BondAxisX.SetVel(axisVels[0]);
            this.BondAxisY.SetVel(axisVels[1]);
            this.BondHead.AxisZ.SetVel(axisVels[2]);
        }

        /// <summary>
        /// Bond模组移动到指定G0坐标系下的位置
        /// </summary>
        /// <param name="point3D">G0点位</param>
        public void MoveToG0Pos(AKRSPoint3D point3D)
        {
            // 将G0坐标系的坐标转到轴坐标
            AKRSPoint3D point = this.ConvertG0ToMachinePos(point3D);

            // 移动XYZ的位置
            this.MoveBondXYZ(point);
        }

        /// <summary>
        /// 判断轴能不能移动
        /// </summary>
        /// <returns>结果</returns>
        public bool IsReady()
        {
            if (this.BondAxisX == null || this.BondAxisY == null || this.BondHead.AxisZ == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检测数据的合理性
        /// 如果超出设置的软限位直接报警
        /// </summary>
        /// <param name="point">点位</param>
        /// <returns>结果</returns>
        public bool CheckSoftLimit(AKRSPoint4D point)
        {
            if (point.X > this.BondAxisX.AxisSetPara.PLimit || point.X < this.BondAxisX.AxisSetPara.NLimit)
            {
                throw new Exception(
                    "系统2：X轴目标位置超出软限位！\r\n"
                    + $"目标位置:{point.X},正限位:{this.BondAxisX.GetSoftLimit().PLimit},负限位:{this.BondAxisX.GetSoftLimit().NLimit}");
            }

            if (point.Y > this.BondAxisY.AxisSetPara.PLimit || point.Y < this.BondAxisY.AxisSetPara.NLimit)
            {
                throw new Exception(
                    "系统2：Y轴目标位置超出软限位！\r\n"
                    + $"目标位置:{point.Y},正限位:{this.BondAxisY.GetSoftLimit().PLimit},负限位:{this.BondAxisY.GetSoftLimit().NLimit}");
            }

            if (point.Z > this.BondHead.AxisZ.AxisSetPara.PLimit || point.Z < this.BondHead.AxisZ.AxisSetPara.NLimit)
            {
                throw new Exception(
                    "系统2：Z轴目标位置超出软限位！\r\n"
                    + $"目标位置:{point.Z},正限位:{this.BondHead.AxisZ.GetSoftLimit().PLimit},负限位:{this.BondHead.AxisZ.GetSoftLimit().NLimit}");
            }

            return true;
        }

        /// <summary>
        /// 检测数据的合理性
        /// 如果超出设置的软限位直接报警
        /// </summary>
        /// <param name="point">点位</param>
        /// <returns>结果</returns>
        public bool CheckSoftLimit(AKRSPoint3D point)
        {          
                if (point.X > this.BondAxisX.AxisSetPara.PLimit || point.X < this.BondAxisX.AxisSetPara.NLimit)
            {
                throw new Exception(
                    "系统2：X轴目标位置超出软限位！\r\n"
                    + $"目标位置:{point.X},正限位:{this.BondAxisX.GetSoftLimit().PLimit},负限位:{this.BondAxisX.GetSoftLimit().NLimit}");
            }

            if (point.Y > this.BondAxisY.AxisSetPara.PLimit || point.Y < this.BondAxisY.AxisSetPara.NLimit)
            {
                throw new Exception(
                    "系统2：Y轴目标位置超出软限位！\r\n"
                    + $"目标位置:{point.Y},正限位:{this.BondAxisY.GetSoftLimit().PLimit},负限位:{this.BondAxisY.GetSoftLimit().NLimit}");
            }

            if (point.Z > this.BondHead.AxisZ.AxisSetPara.PLimit || point.Z < this.BondHead.AxisZ.AxisSetPara.NLimit)
            {
                throw new Exception(
                    "系统2：Z轴目标位置超出软限位！\r\n"
                    + $"目标位置:{point.Z},正限位:{this.BondHead.AxisZ.GetSoftLimit().PLimit},负限位:{this.BondHead.AxisZ.GetSoftLimit().NLimit}");
            }

            return true;
        }

        /// <summary>
        /// 检测数据的合理性
        /// 如果超出设置的软限位直接报警
        /// </summary>
        /// <param name="point">点位</param>
        /// <returns>结果</returns>
        public bool CheckSoftLimit(AKRSPoint2D point)
        {
            if (point.X > this.BondAxisX.AxisSetPara.PLimit || point.X < this.BondAxisX.AxisSetPara.NLimit)
            {
                throw new Exception(
                    "系统2：X轴目标位置超出软限位！\r\n"
                    + $"目标位置:{point.X},正限位:{this.BondAxisX.GetSoftLimit().PLimit},负限位:{this.BondAxisX.GetSoftLimit().NLimit}");
            }

            if (point.Y > this.BondAxisY.AxisSetPara.PLimit || point.Y < this.BondAxisY.AxisSetPara.NLimit)
            {
                throw new Exception(
                    "系统2：Y轴目标位置超出软限位！\r\n"
                    + $"目标位置:{point.Y},正限位:{this.BondAxisY.GetSoftLimit().PLimit},负限位:{this.BondAxisY.GetSoftLimit().NLimit}");
            }

            return true;
        }

        /// <summary>
        /// 检测数据的合理性
        /// 如果超出设置的软限位直接报警
        /// </summary>
        /// <param name="x">X轴坐标</param>
        /// <param name="y">Y轴坐标</param>
        /// <returns>结果</returns>
        public bool CheckSoftLimit(double x, double y)
        {         
            if (x > this.BondAxisX.AxisSetPara.PLimit || x < this.BondAxisX.AxisSetPara.NLimit)
            {
                throw new Exception(
                    "系统2：X轴目标位置超出软限位！\r\n"
                    + $"目标位置:{x},正限位:{this.BondAxisX.GetSoftLimit().PLimit},负限位:{this.BondAxisX.GetSoftLimit().NLimit}");
            }

            if (y > this.BondAxisY.AxisSetPara.PLimit || y < this.BondAxisY.GetSoftLimit().NLimit)
            {
                throw new Exception(
                    "系统2：Y轴目标位置超出软限位！\r\n"
                    + $"目标位置:{y},正限位:{this.BondAxisY.GetSoftLimit().PLimit},负限位:{this.BondAxisY.GetSoftLimit().NLimit}");
            }

            return true;
        }

        /// <summary>
        /// 将Bond相机的结果转换成G0中的位置
        /// </summary>
        /// <param name="visionPos">轴的真实坐标</param>
        /// <param name="result">定位的结果</param>
        /// <returns>结果</returns>
        public AKRSPoint3D ConvertPixelToG0Pos(AKRSPoint3D visionPos, MatchResult result)
        {
            return this.BondCameraCoordinateSystem.SelfPosToG0(
                new AKRSPoint3D(result.CenterX, result.CenterY, 0),
                visionPos);
        }

        /// <summary>
        /// 获取点胶阀的序号
        /// </summary>
        /// <returns>结果</returns>
        public int GetDispenseIndex()
        {
            return this.DripElectric.ElectricIO;
        }

        /// <summary>
        /// 获取灯光集合
        /// </summary>
        /// <returns>结果</returns>
        public List<Light> GetLights()
        {
            return new List<Light>()
                       {
                           this.SpotLightRed,
                           this.SpotLightGreen,
                           this.SpotLightBlue,
                           this.AmbientLightRed,
                           this.AmbientLightGreen,
                           this.AmbientLightBlue,
                       };
        }

        /// <summary>
        /// 蘸胶盘一直转动
        /// </summary>
        public void PrintToolContinueMove(double speed)
        {
            if (this.PrintTool == null)
            {
                throw new System.Exception("蘸胶盘硬件不存在,请检查硬件是否存在异常");
            }

            this.PrintTool.Jog(MoveDirection.Negative);
        }

        /// <summary>
        /// 蘸胶盘停止转动
        /// </summary>
        public void PrintToolStop()
        {
            if (this.PrintTool == null)
            {
                throw new System.Exception("蘸胶盘硬件不存在,请检查硬件是否存在异常");
            }

            this.PrintTool.StopMove();
        }

        /// <summary>
        /// 蘸胶盘是否在转动
        /// </summary>
        /// <returns>结果</returns>
        public bool IsPrintToolMove()
        {
            if (this.PrintTool == null)
            {
                throw new System.Exception("蘸胶盘硬件不存在,请检查硬件是否存在异常");
            }

            return !this.PrintTool.IsInCommandPosition();
        }
    }


    #endregion
}

