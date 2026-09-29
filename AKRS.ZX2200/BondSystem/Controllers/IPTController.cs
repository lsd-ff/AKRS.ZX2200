using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Modules;

namespace AKRS.ZX2200.BondSystem.Controllers
{
    /// <summary>
    /// IPT控制器
    /// </summary>
    public class IPTController
    {
        /// <summary>
        /// 焊头
        /// </summary>
        private IPTModule iPTModule => System2Module.GetInstance().IPTModule;

        /// <summary>
        /// 开IPT平台真空
        /// </summary>
        public void OpenIPTVacuum()
        {
            this.iPTModule.OpenIPTVacuum();
        }

        /// <summary>
        /// 关IPT平台真空
        /// </summary>
        public void CloseIPTVacuum()
        {
            this.iPTModule.CloseIPTVacuum();
        }

        /// <summary>
        /// 获取真空状态
        /// </summary>
        /// <returns>结果</returns>
        public bool GetVacuumState()
        {
            return this.iPTModule.IPTVacuumElectric.GetOutputValue();
        }

        /// <summary>
        /// 开IPT平台吹气
        /// </summary>
        public void OpenIPTBlow()
        {
            this.iPTModule.OpenIPTBlow();
        }

        /// <summary>
        /// 关IPT平台吹气
        /// </summary>
        public void CloseIPTBlow()
        {
            this.iPTModule.CloseIPTBlow();
        }

        /// <summary>
        /// 获取吹气状态
        /// </summary>
        /// <returns>结果</returns>
        public bool GetBlowState()
        {
            return this.iPTModule.IPTBlowElectric.GetOutputValue();
        }

        /// <summary>
        /// 中转台旋转一定的角度
        /// </summary>
        /// <param name="angle">角度</param>
        public void IPTRotary(double angle)
        {
            this.iPTModule.IPTRotary(angle);
        }

        // ==================== 新增右中转台方法 ====================

        /// <summary>
        /// 开右中转台真空
        /// </summary>
        public void OpenRightIPTVacuum()
        {
            this.iPTModule.RightIPTVacuumElectric.SetOutputValue(true);
        }

        /// <summary>
        /// 关右中转台真空
        /// </summary>
        public void CloseRightIPTVacuum()
        {
            this.iPTModule.RightIPTVacuumElectric.SetOutputValue(false);
        }

        /// <summary>
        /// 获取右中转台真空状态
        /// </summary>
        /// <returns>true: 开启, false: 关闭</returns>
        public bool GetRightVacuumState()
        {
            return this.iPTModule.RightIPTVacuumElectric.GetOutputValue();
        }

        /// <summary>
        /// 开右中转台吹气
        /// </summary>
        public void OpenRightIPTBlow()
        {
            this.iPTModule.RightIPTBlowElectric.SetOutputValue(true);
        }

        /// <summary>
        /// 关右中转台吹气
        /// </summary>
        public void CloseRightIPTBlow()
        {
            this.iPTModule.RightIPTBlowElectric.SetOutputValue(false);
        }

        /// <summary>
        /// 获取右中转台吹气状态
        /// </summary>
        /// <returns>true: 开启, false: 关闭</returns>
        public bool GetRightBlowState()
        {
            return this.iPTModule.RightIPTBlowElectric.GetOutputValue();
        }
    }
}