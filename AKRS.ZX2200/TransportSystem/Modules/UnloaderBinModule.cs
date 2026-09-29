using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.LogicHardware.Services;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;

namespace AKRS.ZX2200.TransportSystem.Modules
{
    using AKRS.Galaxy2.Infrastructure.Enums;

    /// <summary>
    ///  下料仓模组
    /// </summary>
    public class UnloaderBinModule
    {
        #region 配置上下料

        /// <summary>
        /// 下料载台X
        /// </summary>
        public Axis UnLoaderTableAxisY => HardwareRepositoryService.GetHardware<Axis>("下料Y");

        /// <summary>
        /// 下料载台Z
        /// </summary>
        public Axis UnLoaderTableAxisZ => HardwareRepositoryService.GetHardware<Axis>("下料Z");

        /// <summary>
        /// 下料载台Z
        /// </summary>
        public Axis UnLoaderPushRod => HardwareRepositoryService.GetHardware<Axis>("下料推料");

        /// <summary>
        /// 料盒A检测
        /// </summary>
        public Sensor UnloaderBinACheckSensor => HardwareRepositoryService.GetHardware<Sensor>("前下料盒检测");

        /// <summary>
        /// 料盒B检测
        /// </summary>
        public Sensor UnloaderBinBCheckSensor => HardwareRepositoryService.GetHardware<Sensor>("后下料盒检测");

        #endregion

        /// <summary>
        /// 是否感应到料盒A
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool IsBinAOnTable()
        {
            return this.UnloaderBinACheckSensor.GetInputValue();
        }

        /// <summary>
        /// 是否感应到料盒B
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool IsBinBOnTable()
        {
            return this.UnloaderBinBCheckSensor.GetInputValue();
        }

        /// <summary>
        /// 移动Y轴到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        public void MoveAxisY(double pos)
        {
            if (!this.IsReady())
            {
                return;
            }

            this.CheckYSoftLimit(pos);

            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            this.UnLoaderTableAxisY.AbsoluteMove(pos);
        }

        /// <summary>
        /// 移动Z轴到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        public void MoveAxisZ(double pos)
        {
            if (!this.IsReady())
            {
                return;
            }

            this.CheckZSoftLimit(pos);

            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            this.UnLoaderTableAxisZ.AbsoluteMove(pos);
        }

        /// <summary>
        /// 移动推杆到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        public ExcuteResult MovePushRod(double pos)
        {
            if (!this.IsReady())
            {
                return ExcuteResult.Abort;
            }

            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return ExcuteResult.Success;
            }

           return this.UnLoaderPushRod.AbsoluteMove(pos);
        }


        /// <summary>
        /// 判断轴能不能移动
        /// </summary>
        /// <returns>结果</returns>
        public bool IsReady()
        {
            if (this.UnLoaderTableAxisZ == null || this.UnLoaderTableAxisY == null || this.UnLoaderPushRod == null)
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
            if (x > this.UnLoaderTableAxisY.AxisSetPara.PLimit || x < this.UnLoaderTableAxisY.AxisSetPara.NLimit)
            {
                throw new Exception(
                    "The  unloader axis Y is about to move exceeds the limit \r\n"
                    + $"targetPos:{x},positive  limit:{this.UnLoaderTableAxisY.GetSoftLimit().PLimit},negative  limit:{this.UnLoaderTableAxisY.GetSoftLimit().NLimit}");
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
            if (x > this.UnLoaderTableAxisZ.AxisSetPara.PLimit || x < this.UnLoaderTableAxisZ.AxisSetPara.NLimit)
            {
                throw new Exception(
                    "The  unloader axis Z is about to move exceeds the limit \r\n"
                    + $"targetPos:{x},positive  limit:{this.UnLoaderTableAxisY.GetSoftLimit().PLimit},negative  limit:{this.UnLoaderTableAxisY.GetSoftLimit().NLimit}");
            }

            return true;
        }
    }
}
