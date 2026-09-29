using System;

using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;

namespace AKRS.ZX2200.TransportSystem.Modules
{
    using AKRS.Galaxy2.Infrastructure.Enums;

    /// <summary>
    /// 上料仓模组
    /// </summary>
    public class LoaderBinModule
    {
        #region 硬件

        /// <summary>
        /// 上料载台X
        /// </summary>
        public Axis LoaderTableAxisY => HardwareRepositoryService.GetHardware<Axis>("上料Y");

        /// <summary>
        /// 下料载台Z
        /// </summary>
        public Axis LoaderTableAxisZ => HardwareRepositoryService.GetHardware<Axis>("上料Z");

        /// <summary>
        /// 上料推杆
        /// </summary>
        public Axis LoaderPushRod => HardwareRepositoryService.GetHardware<Axis>("上料推料");

        /// <summary>
        /// 料盒A检测
        /// </summary>
        public Sensor LoaderBinACheckSensor => HardwareRepositoryService.GetHardware<Sensor>("前上料盒检测");

        /// <summary>
        /// 料盒A检测
        /// </summary>
        public Sensor LoaderBinBCheckSensor => HardwareRepositoryService.GetHardware<Sensor>("后上料盒检测");

        #endregion

        /// <summary>
        /// 是否感应到料盒A
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool IsBinAOnTable()
        {
             return this.LoaderBinACheckSensor.GetInputValue();
        }

        /// <summary>
        /// 是否感应到料盒B
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool IsBinBOnTable()
        {
            return this.LoaderBinBCheckSensor.GetInputValue();
        }

        /// <summary>
        /// 移动Y轴到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        public ExcuteResult MoveAxisY(double pos)
        {
            if (!this.IsReady())
            {
                return ExcuteResult.Exception;
            }

            this.CheckYSoftLimit(pos);

            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return ExcuteResult.None;
            }

            return  this.LoaderTableAxisY.AbsoluteMove(pos);
        }

        /// <summary>
        /// 移动Z轴到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        public ExcuteResult MoveAxisZ(double pos)
        {
            if (!this.IsReady())
            {
                return ExcuteResult.Exception;
            }

            this.CheckZSoftLimit(pos);

            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return ExcuteResult.None;
            }

          return  this.LoaderTableAxisZ.AbsoluteMove(pos);
        }

        /// <summary>
        /// 发送推料推杆到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        public ExcuteResult SendMovePushRodCommand(double pos)
        {
            if (!this.IsReady())
            {
                return ExcuteResult.Exception;
            }

            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return ExcuteResult.None;
            }

           return  this.LoaderPushRod.SendAbsoluteMoveCommand(pos);
        }

        /// <summary>
        /// 推料推杆到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        public ExcuteResult  MovePushRod(double pos)
        {
            if (!this.IsReady())
            {
                return ExcuteResult.Exception;
            }

            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return ExcuteResult.None;
            }

         return    this.LoaderPushRod.AbsoluteMove(pos);
        }

        /// <summary>
        /// 推料推杆到指定位置
        /// </summary>
        /// <returns>是否到位</returns>
        public bool IsPushRodInPosition()
        {
            if (!this.IsReady())
            {
                return false;
            }

            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return true;
            }

            bool ret = this.LoaderPushRod.IsInCommandPosition();
            return ret;
        }

        /// <summary>
        /// 判断轴能不能移动
        /// </summary>
        /// <returns>结果</returns>
        public bool IsReady()
        {
            if (this.LoaderTableAxisZ == null || this.LoaderTableAxisY == null || this.LoaderPushRod == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检测数据的合理性
        /// 如果超出设置的软限位直接报警
        /// </summary>
        /// <param name="x">Y轴坐标</param>
        /// <returns>结果</returns>
        public bool CheckYSoftLimit(double x)
        {
            if (x > this.LoaderTableAxisY.AxisSetPara.PLimit || x < this.LoaderTableAxisY.AxisSetPara.NLimit)
            {
                throw new Exception(
                    "The  unloader axis Y is about to move exceeds the limit \r\n"
                    + $"targetPos:{x},positive  limit:{this.LoaderTableAxisY.GetSoftLimit().PLimit},negative  limit:{this.LoaderTableAxisY.GetSoftLimit().NLimit}");
            }

            return true;
        }

        /// <summary>
        /// 检测数据的合理性
        /// 如果超出设置的软限位直接报警
        /// </summary>
        /// <param name="x">Z轴坐标</param>
        /// <returns>结果</returns>
        public bool CheckZSoftLimit(double x)
        {
            if (x > this.LoaderTableAxisZ.AxisSetPara.PLimit || x < this.LoaderTableAxisZ.AxisSetPara.NLimit)
            {
                throw new Exception(
                    "The  unloader axis Z is about to move exceeds the limit \r\n"
                    + $"targetPos:{x},positive  limit:{this.LoaderTableAxisY.GetSoftLimit().PLimit},negative  limit:{this.LoaderTableAxisY.GetSoftLimit().NLimit}");
            }

            return true;
        }

        /// <summary>
        /// 推杆清错
        /// </summary>
        public void PushRodResetError()
        {
            this.LoaderPushRod.ResetError();
        }

        /// <summary>
        /// 推杆上使能
        /// </summary>
        public void PushRodServoOn()
        {
            this.LoaderPushRod.ServoOn();
        }
    }
}
