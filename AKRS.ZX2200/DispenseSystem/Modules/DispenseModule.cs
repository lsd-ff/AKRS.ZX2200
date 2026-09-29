using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.LogicHardware.Services;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;

using System.Collections.Generic;

namespace AKRS.ZX2200.DispenseSystem.Modules
{
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
    using AKRS.Galaxy2.LogicHardware.Hardwares.DispenseControllers;
    using AKRS.Galaxy2.LogicHardware.HardWares.LaserMeasureHeightControllers;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.Main.Machine.MachineSupport;

    using Newtonsoft.Json;
    using PostSharp;
    using System;

    /// <summary>
    /// 点胶器模组
    /// </summary>
    public class DispenseModule
    {
        #region 硬件

        /// <summary>
        /// 点胶X轴
        /// </summary>
        private Axis DispenseAxisX => HardwareRepositoryService.GetHardware<Axis>("点胶X");

        /// <summary>
        /// 点胶Y轴
        /// </summary>
        private Axis DispenseAxisY => HardwareRepositoryService.GetHardware<Axis>("点胶Y");

        /// <summary>
        /// 点胶Z轴
        /// </summary>
        private Axis DispenseAxisZ => HardwareRepositoryService.GetHardware<Axis>("点胶Z");


        /// <summary>
        /// 蘸胶盘旋转电机
        /// </summary>
        private Axis PrintTool => HardwareRepositoryService.GetHardware<Axis>("蘸胶T");

        /// <summary>
        /// 点胶控制阀
        /// </summary>
        private Electric DripElectric { get; set; } = HardwareRepositoryService.GetHardware<Electric>("系统1点胶控制");

        /// <summary>
        /// 武藏点胶器
        /// </summary>
        private Dispenser DispenserControl { get; set; } = HardwareRepositoryService.GetHardware<Dispenser>("点胶器1");

        /// <summary>
        /// 点胶胶量检测
        /// </summary>        
        private Sensor EpoxyCheckSensor { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("点胶胶量检测");

        /// <summary>
        /// 视觉模组
        /// </summary>
        private VisionModule VisionModule { get; set; } = new VisionModule();

        #endregion

        /// <summary>
        /// 速度百分比
        /// </summary>
        [JsonIgnore]
        private double SpeedRatio => MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

        /// <summary>
        /// 获取点胶X轴
        /// </summary>
        /// <returns>结果</returns>
        /// <exception cref="System.Exception">空指针异常</exception>
        public Axis GetDispenseXAxis()
        {
            if (this.DispenseAxisX == null)
            {
                throw new System.Exception("点胶X轴不存在,请检查硬件是否存在异常");
            }

            return this.DispenseAxisX;
        }

        /// <summary>
        /// 获取点胶Y轴
        /// </summary>
        /// <returns>结果</returns>
        /// <exception cref="System.Exception">空指针异常</exception>
        public Axis GetDispenseYAxis()
        {
            if (this.DispenseAxisY == null)
            {
                throw new System.Exception("点胶Y轴不存在,请检查硬件是否存在异常");
            }

            return this.DispenseAxisY;
        }

        /// <summary>
        /// 获取点胶Z轴
        /// </summary>
        /// <returns>结果</returns>
        /// <exception cref="System.Exception">空指针异常</exception>
        public Axis GetDispenseZAxis()
        {
            if (this.DispenseAxisZ == null)
            {
                throw new System.Exception("点胶Z轴不存在,请检查硬件是否存在异常");
            }

            return this.DispenseAxisZ;
        }

        /// <summary>
        /// XY移动到指定位置，移动的前提一定要保证Z轴是安全的
        /// </summary>
        /// <param name="posX">X位置</param>
        /// <param name="posY">Y位置</param>
        public void MoveDispenseXAndY(double posX, double posY)
        {
            if (this.DispenseAxisX == null)
            {
                throw new System.Exception("点胶X轴不存在,请检查硬件是否存在异常");
            }

            if (this.DispenseAxisY == null)
            {
                throw new System.Exception("点胶Y轴不存在,请检查硬件是否存在异常");
            }

            double velX = this.DispenseAxisX.GetVel()
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;


            double velY = this.DispenseAxisY.GetVel()
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            // 开始运动
            this.DispenseAxisX.SendAbsoluteMoveCommand(posX, velX);
            this.DispenseAxisY.SendAbsoluteMoveCommand(posY, velY);

            // 等轴到位
            MotionService.WaitAxesArrival(
                (this.DispenseAxisX, true, posX, AccuracyMode.HighAccuracy),
                (this.DispenseAxisY, true, posY, AccuracyMode.HighAccuracy));

            //MotionService.MoveAxesToTargetPosition(
            //    (this.DispenseAxisX, false, posX, AccuracyMode.HighSpeed),
            //    (this.DispenseAxisY, false, posY, AccuracyMode.HighSpeed));
        }

        /// <summary>
        /// 获取轴的坐标
        /// </summary>
        /// <returns>结果</returns>
        public AKRSPoint3D GetAxisPos()
        {
            if (this.DispenseAxisX == null)
            {
                throw new System.Exception("点胶X轴不存在,请检查硬件是否存在异常");
            }

            if (this.DispenseAxisY == null)
            {
                throw new System.Exception("点胶Y轴不存在,请检查硬件是否存在异常");
            }

            if (this.DispenseAxisZ == null)
            {
                throw new System.Exception("点胶Z轴不存在,请检查硬件是否存在异常");
            }

            return new AKRSPoint3D(
                this.DispenseAxisX.GetRealPosition(),
                this.DispenseAxisY.GetRealPosition(),
                this.DispenseAxisZ.GetRealPosition());
        }

        /// <summary>
        /// 获取点胶阀的序号
        /// </summary>
        /// <returns>结果</returns>
        public int GetDispenseIndex()
        {
            if (this.DripElectric == null)
            {
                throw new System.Exception("点胶控制阀硬件不存在,请检查硬件是否存在异常");
            }

            return this.DripElectric.ElectricIO;
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

        /// <summary>
        /// Z轴移动
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="accuracy">精密</param>
        public void AxisZAbsoluteMove(double pos,bool accuracy = true)
        {
            if (this.DispenseAxisZ == null)
            {
                throw new System.Exception("点胶Z轴不存在,请检查硬件是否存在异常");
            }

            // 加速度没有改变
            MovePara movePara = new MovePara()
                                    {
                                        TargetPosition = pos,
                                        Vel = this.DispenseAxisZ.AxisMovePara.AbsoluteMoveSpeed * this.SpeedRatio,
                                        Acc = this.DispenseAxisZ.AxisMovePara.ACC,
                                        Dec = this.DispenseAxisZ.AxisMovePara.DEC,
                                        Jerk = this.DispenseAxisZ.AxisMovePara.Jerk
            };

            if (accuracy)
            {
                this.DispenseAxisZ.AbsoluteMove(movePara);
            }
            else
            {
                this.DispenseAxisZ.AbsoluteMove(movePara, false, AccuracyMode.HighSpeed);
            }
            

            DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"点胶Z移动到{pos}");
        }

        /// <summary>
        /// 点胶Z轴移动到一固定位置
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="speed">速度</param>
        public void AxisZAbsoluteMove(double position, double speed)
        {
            // 移动参数，这个目前不知道需不需要考虑分辨率
            MovePara movePara = new MovePara()
            {
                Vel = speed,
                Acc = MachineHardwareConfiguration.GetInstance().IsDispenseZGuGao ? speed * 10 : this.GetDispenseZAxis().GetAcc(),
                Dec = MachineHardwareConfiguration.GetInstance().IsDispenseZGuGao ? speed * 10 : this.GetDispenseZAxis().GetDec(),
                TargetPosition = position,
            };

            // 绝对移动
            this.AxisZAbsoluteMove(movePara);
        }

        /// <summary>
        /// Z轴移动
        /// </summary>
        /// <param name="movePara">移动参数</param>>
        public void AxisZAbsoluteMove(MovePara movePara)
        {
            if (this.DispenseAxisZ == null)
            {
                throw new System.Exception("点胶Z轴不存在,请检查硬件是否存在异常");
            }

            this.DispenseAxisZ.AbsoluteMove(movePara);

            DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"点胶Z移动到{movePara.TargetPosition}");
        }

        /// <summary>
        /// Z轴移动
        /// </summary>
        /// <param name="movePara">移动参数</param>>
        public void AxisZSendAbsoluteMoveCommand(MovePara movePara)
        {
            if (this.DispenseAxisZ == null)
            {
                throw new System.Exception("点胶Z轴不存在,请检查硬件是否存在异常");
            }

            this.DispenseAxisZ.SendAbsoluteMoveCommand(movePara);

            DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"点胶发送移动到{movePara.TargetPosition}指令");
        }

        /// <summary>
        /// 等待Z轴到位
        /// </summary>
        public void AxisZWaitForArrival()
        {
            if (this.DispenseAxisZ == null)
            {
                throw new System.Exception("点胶Z轴不存在,请检查硬件是否存在异常");
            }

            this.DispenseAxisZ.WaitForArrival();

            DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"点胶Z轴等待到位成功");
        }

        /// <summary>
        /// XY轴平移
        /// </summary>
        /// <param name="posX">位置X</param>
        /// <param name="posY">位置Y</param>
        /// <param name="accuracy">精密</param>
        public void AxisXYAbsoluteMove(double posX, double posY, bool accuracy = true)
        {
            if (this.DispenseAxisX == null)
            {
                throw new System.Exception("点胶X轴不存在,请检查硬件是否存在异常");
            }

            if (this.DispenseAxisY == null)
            {
                throw new System.Exception("点胶Y轴不存在,请检查硬件是否存在异常");
            }

            // 加速度没有改变
            MovePara moveParaX = new MovePara()
                                    {
                                        TargetPosition = posX,
                                        Vel = this.DispenseAxisX.AxisMovePara.AbsoluteMoveSpeed * this.SpeedRatio,
                                        Acc = this.DispenseAxisX.AxisMovePara.ACC,
                                        Dec = this.DispenseAxisX.AxisMovePara.DEC,
                                        Jerk = this.DispenseAxisX.AxisMovePara.Jerk
                                    };

            // 加速度没有改变
            MovePara moveParaY = new MovePara()
                                    {
                                        TargetPosition = posY,
                                        Vel = this.DispenseAxisY.AxisMovePara.AbsoluteMoveSpeed * this.SpeedRatio,
                                        Acc = this.DispenseAxisY.AxisMovePara.ACC,
                                        Dec = this.DispenseAxisY.AxisMovePara.DEC,
                                        Jerk = this.DispenseAxisY.AxisMovePara.Jerk
                                    };
            if (accuracy)
            {
                // 开始运动
                this.DispenseAxisX.SendAbsoluteMoveCommand(moveParaX);
                this.DispenseAxisY.SendAbsoluteMoveCommand(moveParaY);

                // 等轴到位
                MotionService.WaitAxesArrival(
                    (this.DispenseAxisX, true, posX, AccuracyMode.HighAccuracy),
                    (this.DispenseAxisY, true, posY, AccuracyMode.HighAccuracy));
            }
            else
            {
                MotionService.MoveAxesToTargetPosition(
                            (this.DispenseAxisX, false, posX, AccuracyMode.HighSpeed),
                            (this.DispenseAxisY, false, posY, AccuracyMode.HighSpeed));
            }

            DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"点胶XY移动到{posX}，{posY}");
        }

        /// <summary>
        /// XY轴平移
        /// </summary>
        /// <param name="point3D">XYZ点位</param>
        public void AxisXYZAbsoluteMove(AKRSPoint3D point3D)
        {
            if (this.DispenseAxisX == null)
            {
                throw new System.Exception("点胶X轴不存在,请检查硬件是否存在异常");
            }

            if (this.DispenseAxisY == null)
            {
                throw new System.Exception("点胶Y轴不存在,请检查硬件是否存在异常");
            }

            if (this.DispenseAxisZ == null)
            {
                throw new System.Exception("点胶Z轴不存在,请检查硬件是否存在异常");
            }

            // 加速度没有改变
            MovePara moveParaX = new MovePara()
                                     {
                                         TargetPosition = point3D.X,
                                         Vel = this.DispenseAxisX.AxisMovePara.AbsoluteMoveSpeed * this.SpeedRatio,
                                         Acc = this.DispenseAxisX.AxisMovePara.ACC,
                                         Dec = this.DispenseAxisX.AxisMovePara.DEC,
                                         Jerk = this.DispenseAxisX.AxisMovePara.Jerk
                                     };

            // 加速度没有改变
            MovePara moveParaY = new MovePara()
                                     {
                                         TargetPosition = point3D.Y,
                                         Vel = this.DispenseAxisY.AxisMovePara.AbsoluteMoveSpeed * this.SpeedRatio,
                                         Acc = this.DispenseAxisY.AxisMovePara.ACC,
                                         Dec = this.DispenseAxisY.AxisMovePara.DEC,
                                         Jerk = this.DispenseAxisY.AxisMovePara.Jerk
                                     };

            // 加速度没有改变
            MovePara moveParaZ = new MovePara()
                                     {
                                         TargetPosition = point3D.Z,
                                         Vel = this.DispenseAxisZ.AxisMovePara.AbsoluteMoveSpeed * this.SpeedRatio,
                                         Acc = this.DispenseAxisZ.AxisMovePara.ACC,
                                         Dec = this.DispenseAxisZ.AxisMovePara.DEC,
                                         Jerk = this.DispenseAxisZ.AxisMovePara.Jerk
                                     };

            // 开始运动
            this.DispenseAxisX.SendAbsoluteMoveCommand(moveParaX);
            this.DispenseAxisY.SendAbsoluteMoveCommand(moveParaY);
            this.DispenseAxisZ.SendAbsoluteMoveCommand(moveParaZ);
            
            // 等轴到位
            MotionService.WaitAxesArrival(
                (this.DispenseAxisX, true, point3D.X, AccuracyMode.HighAccuracy),
                (this.DispenseAxisY, true, point3D.Y, AccuracyMode.HighAccuracy),
                (this.DispenseAxisZ, true, point3D.Z, AccuracyMode.HighAccuracy));

            DispenseRunTimeProvider.RecordTime("系统1点胶流程", $"点胶XYZ移动到{point3D}");
        }


        /// <summary>
        /// 设置点胶阀压力值
        /// </summary>
        /// <param name="dispensePressure">压力值</param>
        /// <param name="vacuum">负压</param>
        public void SetDispensePressure(double dispensePressure, double vacuum)
        {
            if (this.DispenserControl == null)
            {
                throw new Exception("未找到点胶控制器，请检查硬件是否有问题");
            }

            try
            {
                this.DispenserControl.Pressure = dispensePressure;
                this.DispenserControl.Vacuum = vacuum;
                this.DispenserControl.Time = 10;

                this.DispenserControl.SetPressureVacuumTime(dispensePressure, vacuum, 10, 1);
            }
            catch (Exception ex)
            {
                throw new Exception("点胶控制器设置失败，请检查硬件是否有问题");
            }
        }

        /// <summary>
        /// 点胶阀是否开启
        /// </summary>
        /// <returns>结果</returns>
        public bool IsDispensingElectricOpen()
        {
            if (this.DripElectric == null)
            {
                throw new Exception("没有找到点胶控制器IO，请检查硬件");
            }

            return this.DripElectric.GetOutputValue();
        }

        /// <summary>
        /// 关闭点胶阀
        /// </summary>
        public void CloseDispensingElectric()
        {
            if (this.DripElectric == null)
            {
                throw new Exception("没有找到点胶控制器IO，请检查硬件");
            }

            this.DripElectric.SetOutputValue(false);
        }

        /// <summary>
        /// 打开点胶阀
        /// </summary>
        public void OpenDispensingElectric()
        {
            if (this.DripElectric == null)
            {
                throw new Exception("没有找到点胶控制器IO，请检查硬件");
            }

            if (!this.DripElectric.GetOutputValue())
            {
                this.DripElectric.SetOutputValue(true);
            }
        }
    }
}
