#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/15 13:01:00
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>

using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;

namespace AKRS.ZX2200.TransportSystem.Modules
{
    /// <summary>
    /// 描述：基板流道载台基类
    /// </summary>
    public abstract class BaseSubSectionModule
    {
        /// <summary>
        /// 载台皮带转动X轴
        /// </summary>
        public abstract Axis BeltAxisX { get; }

        /// <summary>
        /// 当前载台传感器感应是否有料
        /// </summary>
        /// <returns>结果</returns>
        public abstract bool HasMaterialInModule();

        /// <summary>
        /// 皮带相对运动指令
        /// </summary>
        /// <param name="transportDistance">传送距离 单位：mm</param>
        /// <param name="velRate">速度比例</param>
        public virtual void SendRelativeMoveCommandBelt(double transportDistance, double velRate)
        {
            this.BeltAxisX.SendRelativeMoveCommand(transportDistance, this.BeltAxisX.AxisMovePara.AbsoluteMoveSpeed * velRate);
        }

        /// <summary>
        /// 皮带相对运动
        /// </summary>
        /// <param name="transportDistance">传送时间 单位：ms</param>
        /// <param name="velRate">速度比例</param>
        public virtual void RelativeMoveBelt(double transportDistance, double velRate)
        {
            this.BeltAxisX.RelativeMove(transportDistance, this.BeltAxisX.AxisMovePara.AbsoluteMoveSpeed * velRate);
        }

        /// <summary>
        /// 皮带初始化
        /// </summary>
        public virtual void BeltInit()
        {
            this.BeltAxisX?.GoHome();
        }

        /// <summary>
        /// 皮带停止运动
        /// </summary>
        public virtual void StopMove()
        {
            if (this.BeltAxisX != null)
            {
                this.BeltAxisX.StopMove(false, true);
            }
           
        }

        /// <summary>
        /// 皮带是否在运动
        /// </summary>
        /// <returns>结果</returns>
        public virtual bool IsMoving()
        {
           return this.BeltAxisX.GetAxisState().Moving;
        }

        /// <summary>
        /// 获取速度
        /// </summary>
        /// <returns>速度</returns>
        public virtual double GetVel()
        {
            return this.BeltAxisX.GetVel();
        }

        /// <summary>
        /// 设置温度
        /// </summary>
        /// <param name="value">温度</param>
        public void TemperatureSetting(int value)
        {
        
        }

        /// <summary>
        /// 获取温度
        /// </summary>
        /// <returns>结果</returns>
        public int GetTemperature()
        {
            return 0;
        }
    }
}
