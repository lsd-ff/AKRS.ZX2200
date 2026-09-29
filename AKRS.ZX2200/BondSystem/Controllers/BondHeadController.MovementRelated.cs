using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using DevExpress.XtraRichEdit.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.BondSystem.Controllers
{
    /// <summary>
    /// 轴运动相关
    /// </summary>
    public partial class BondHeadController
    {
        /// <summary>
        /// 将Z轴移动到安全高度
        /// </summary>
        public void MoveBondZToSafePos()
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

            double safeLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

            if (MachineHardwareConfiguration.GetInstance().IsStaticWaffleConfigrated)
            {
                AKRSPoint3D avoidPos = System2Domain.GetInstance().BondModuleController.ConvertG0ToMachinePos(
                    WaferSubDevicePara.GetInstance().WaferTableDevicePara.BondAvoidStaticWafflePos);

                AKRSPoint2D curPos = System2Domain.GetInstance().BondModuleController.Get2DRealPosition();
                if (curPos.Y >= avoidPos.Y)
                {
                    double nozzleOffset = this.bondHead.CurrentNozzle != null
                                              ? this.bondHead.CurrentNozzle.MeasureHeightOffset
                                              : 0;

                    // 若在静态华夫盒区域，则安全高度设置为避让位高度
                    safeLevel = avoidPos.Z + nozzleOffset;
                }
            }

            double curLevel = this.bondHead.AxisZ.GetRealPosition();

            if (curLevel < safeLevel)
            {
                this.MoveAxisZ(safeLevel);
            }
        }

        /// <summary>
        /// 将Z轴移动到换吸嘴安全高度
        /// </summary>
        public void MoveBondZToChangeNozzleSafeHeight()
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

            double vel = this.bondHead.AxisZ.AxisMovePara.AbsoluteMoveSpeed * this.machineSoftwareConfiguration.MachineMoveSpeedPercentage;

            this.bondHead.AxisZ.AbsoluteMove(
                this.bondHeadParam.ChangeNozzleSafePos.Z,
                true,
                vel,
                AccuracyMode.HighAccuracy);
        }

        /// <summary>
        /// T轴回0
        /// </summary>
        public void MoveTAxisToHome()
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            this.bondHead.AxisT.SendAbsoluteMoveCommand(0);
        }

        /// <summary>
        /// 焊头T旋转一定角度（绝对位置）
        /// </summary>
        /// <param name="angle">角度</param>
        public ExcuteResult RotateAxisT(double angle)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            this.CheckTAxisSoftLimit(angle);

            // 机器速度百分比
            double vel = this.GetAxisTAbsoluteSpeed() * this.machineSoftwareConfiguration.MachineMoveSpeedPercentage;

            return this.bondHead.AxisT.AbsoluteMove(angle, true, vel);
        }

        /// <summary>
        /// 焊头T去准备位
        /// </summary>
        public ExcuteResult RotateAxisTToPreparePos()
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            double pos = this.bondHeadParam.TAxisPreparePos;

            this.CheckTAxisSoftLimit(pos);

            // 机器速度百分比
            double vel = this.GetAxisTAbsoluteSpeed() * this.machineSoftwareConfiguration.MachineMoveSpeedPercentage;

            return this.bondHead.AxisT.AbsoluteMove(pos, true, vel);
        }

        /// <summary>
        /// 焊头T旋转一定角度不等轴到位（绝对位置）
        /// </summary>
        /// <param name="angle">角度</param>
        public void RotateAxisTNoWait(double angle)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            this.CheckTAxisSoftLimit(angle);

            // 机器速度百分比
            double vel = this.GetAxisTAbsoluteSpeed() * this.machineSoftwareConfiguration.MachineMoveSpeedPercentage;

            this.bondHead.AxisT.SendAbsoluteMoveCommand(angle, vel);
        }

        /// <summary>
        /// 等T轴到位
        /// </summary>
        /// <returns>结果</returns>
        public ExcuteResult WaitTAxisArrive()
        {
            return this.bondHead.AxisT.WaitForArrival();
        }

        /// <summary>
        /// 等Z轴到位
        /// </summary>
        /// <returns>结果</returns>
        public ExcuteResult WaitZAxisArrive()
        {
            return this.bondHead.AxisZ.WaitForArrival();
        }

        /// <summary>
        /// T轴移动到目标位
        /// </summary>
        /// <param name="runPara">位置、参数</param>
        /// <returns>结果</returns>
        public ExcuteResult RotateAxisT(MovePara runPara)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            runPara.Vel = runPara.Vel * this.machineSoftwareConfiguration.MachineMoveSpeedPercentage;

            return this.bondHead.AxisT.AbsoluteMove(runPara);
        }

        /// <summary>
        /// 焊头T相对旋转一定角度
        /// </summary>
        /// <param name="angle">角度</param>
        public void RelativeRotateAxisT(double angle)
        {
            double curAngle = this.bondHead.AxisT.GetRealPosition();

            this.RotateAxisT(curAngle + angle);
        }

        /// <summary>
        /// 移动Z轴到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        public ExcuteResult MoveZAxis(double pos)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            this.CheckZAxisSoftLimit(pos);

            // 机器速度百分比
            double vel = this.GetAxisZAbsoluteSpeed() * this.machineSoftwareConfiguration.MachineMoveSpeedPercentage;

            return this.bondHead.AxisZ.AbsoluteMove(pos, true, vel);
        }

        /// <summary>
        /// 相对移动Z轴
        /// </summary>
        /// <param name="distance">距离</param>
        public ExcuteResult RelativeMoveZAxis(double distance)
        {
            double targetPos = this.GetAxisZRealPos() + distance;
            return this.MoveAxisZ(targetPos);
        }

        /// <summary>
        /// 获取T轴坐标
        /// </summary>
        /// <returns>坐标</returns>
        public double GetAxisTRealPos()
        {
            return this.bondHead.AxisT.GetRealPosition();
        }

        /// <summary>
        /// 获取Z轴坐标
        /// </summary>
        /// <returns>坐标</returns>
        public double GetAxisZRealPos()
        {
            return this.bondHead.AxisZ.GetRealPosition();
        }

        /// <summary>
        /// 获取Z轴配置的绝对运动速度
        /// </summary>
        /// <returns>坐标</returns>
        public double GetAxisZAbsoluteSpeed()
        {
            return this.bondHead.AxisZ.AxisMovePara.AbsoluteMoveSpeed;
        }

        /// <summary>
        /// 获取Z轴配置的绝对运动速度
        /// </summary>
        /// <returns>坐标</returns>
        public double GetAxisTAbsoluteSpeed()
        {
            return this.bondHead.AxisT.AxisMovePara.AbsoluteMoveSpeed;
        }

        /// <summary>
        /// 设置Z轴速度
        /// </summary>
        /// <param name="speed">速度</param>
        public void SetAxisZSpeed(double speed)
        {
            this.bondHead.AxisZ.SetVel(speed);
        }

        /// <summary>
        /// 获取Z轴加速度
        /// </summary>
        /// <returns>坐标</returns>
        public double GetAxisZAcc()
        {
            return this.bondHead.AxisZ.GetAcc();
        }

        /// <summary>
        /// 获取Z轴减速度
        /// </summary>
        /// <returns>坐标</returns>
        public double GetAxisZDec()
        {
            return this.bondHead.AxisZ.GetDec();
        }

        /// <summary>
        /// Z轴移动到目标位
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="accuracy">精度模式</param>
        public ExcuteResult MoveAxisZ(double pos, AccuracyMode accuracy = AccuracyMode.HighAccuracy)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            // 机器速度百分比
            double vel = this.GetAxisZAbsoluteSpeed() * this.machineSoftwareConfiguration.MachineMoveSpeedPercentage;

            MovePara runPara = new()
            {
                TargetPosition = pos,
                Acc = this.bondHead.AxisZ.AxisMovePara.ACC * this.machineSoftwareConfiguration.MachineMoveSpeedPercentage,
                Dec = this.bondHead.AxisZ.AxisMovePara.DEC * this.machineSoftwareConfiguration.MachineMoveSpeedPercentage,
                Jerk = this.bondHead.AxisZ.AxisMovePara.Jerk,
                Vel = this.GetAxisZAbsoluteSpeed() * this.machineSoftwareConfiguration.MachineMoveSpeedPercentage
            };

            // 加速度不匹配会导致轴掉使能，所以用下面那个方法
            //return this.bondHead.AxisZ.AbsoluteMove(pos, true, vel, accuracy);

            return this.bondHead.AxisZ.AbsoluteMove(runPara);
        }

        /// <summary>
        /// Z轴移动到目标位
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="vel">速度</param>
        /// <param name="accuracy">精度模式</param>
        /// <param name="isUseSpeedPercentage">是否能使用速度百分比</param>
        public void MoveAxisZ(
            double pos,
            double vel,
            AccuracyMode accuracy = AccuracyMode.HighAccuracy,
            bool isUseSpeedPercentage = true)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            if (isUseSpeedPercentage)
            {
                // 机器速度百分比
                vel = vel * this.machineSoftwareConfiguration.MachineMoveSpeedPercentage;
            }

            this.bondHead.AxisZ.AbsoluteMove(pos, true, vel, accuracy);
        }

        /// <summary>
        /// Z轴移动到目标位
        /// </summary>
        /// <param name="runPara">位置、参数</param>
        /// <returns>结果</returns>
        public ExcuteResult MoveAxisZ(MovePara runPara)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            // 机器速度百分比
            runPara.Vel = runPara.Vel * this.machineSoftwareConfiguration.MachineMoveSpeedPercentage;

            return this.bondHead.AxisZ.AbsoluteMove(runPara);
        }

        /// <summary>
        /// T轴限位检查
        /// 如果超出设置的软限位直接报警
        /// </summary>
        /// <param name="point">点位</param>
        /// <returns>结果</returns>
        public bool CheckTAxisSoftLimit(double point)
        {
            if (point > this.bondHead.AxisT.AxisSetPara.PLimit
                || point < this.bondHead.AxisT.AxisSetPara.NLimit)
            {
                throw new Exception(
                    "系统2：T轴的目标位置超出软限位！\r\n" + $"目标位置:{point},正限位:{this.bondHead.AxisT.GetSoftLimit().PLimit},负限位:{this.bondHead.AxisT.GetSoftLimit().NLimit}");
            }


            return true;
        }

        /// <summary>
        /// Z轴限位检查
        /// 如果超出设置的软限位直接报警
        /// </summary>
        /// <param name="point">点位</param>
        /// <returns>结果</returns>
        public bool CheckZAxisSoftLimit(double point)
        {
            if (point > this.bondHead.AxisZ.AxisSetPara.PLimit
                || point < this.bondHead.AxisZ.AxisSetPara.NLimit)
            {
                throw new Exception(
                    "系统2：Z轴的目标位置超出软限位！\r\n" + $"目标位置:{point},正限位:{this.bondHead.AxisZ.GetSoftLimit().PLimit},负限位:{this.bondHead.AxisZ.GetSoftLimit().NLimit}");
            }

            return true;
        }

        /// <summary>
        /// T轴限位检查
        /// 用于示教过程中的检查
        /// </summary>
        /// <param name="point">点位</param>
        /// <returns>结果</returns>
        public bool CheckZAxisSoftLimitAssitance(double point)
        {
            if (point > this.bondHead.AxisZ.AxisSetPara.PLimit
                || point < this.bondHead.AxisZ.AxisSetPara.NLimit)
            {
                return false;
            }

            return true;
        }
    }
}
