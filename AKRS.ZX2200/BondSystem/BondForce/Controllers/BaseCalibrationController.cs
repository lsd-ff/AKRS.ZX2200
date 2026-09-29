namespace AKRS.ZX2200.BondSystem.BondForce.Controllers
{
    /// <summary>
    /// 力控控制器基类
    /// </summary>
    public abstract class BaseCalibrationController
    {
        /// <summary>
        /// 标定
        /// </summary>
        public abstract void DoWork();

        /// <summary>
        /// 开启运动,基类不实现
        /// </summary>
        public virtual void TurnOn()
        {
        }

        /// <summary>
        /// 关闭运动，基类不实现
        /// </summary>
        public virtual void TurnOff()
        {
        }
    }
}
