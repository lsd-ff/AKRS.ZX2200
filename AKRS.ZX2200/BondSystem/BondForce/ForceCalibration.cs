using AKRS.ZX2200.BondSystem.BondForce.Controllers;

namespace AKRS.ZX2200.BondSystem.BondForce
{
    /// <summary>
    /// 标定实体
    /// </summary>
    public class ForceCalibration
    {
        /// <summary>
        /// 力控标定控制器基类
        /// </summary>
        private BaseCalibrationController ForceCalibrationController { get; set; }

        /// <summary>
        /// 初始化,用依赖注入方式注入控制器
        /// </summary>
        /// <param name="forceCalibrationController">力控标定控制器</param>
        public ForceCalibration(BaseCalibrationController forceCalibrationController)
        {
            this.ForceCalibrationController = forceCalibrationController;
            this.ForceCalibrationController.TurnOn();
        }

        /// <summary>
        /// 开始力控标定
        /// </summary>
        public void DoWork()
        {
            this.ForceCalibrationController.DoWork();
        }
    }
}
