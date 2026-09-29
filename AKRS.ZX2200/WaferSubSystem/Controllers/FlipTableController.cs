using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Modules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.Infrastructure.Enums;

namespace AKRS.ZX2200.WaferSubSystem.Controllers
{
    using System.Threading;

    using AKRS.Galaxy2.Drive.Common;
    using AKRS.Galaxy2.Log;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Programs;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool;
    using AKRS.ZX2200.WaferSubSystem.Services;

    using log4net.Core;

    /// <summary>
    /// 翻转台控制器
    /// </summary>
    public class FlipTableController
    {
        /// <summary>
        /// 翻转模组
        /// </summary>
        [JsonIgnore]
        private FlipModule FlipModule => WaferSubModule.GetInstance().FlipModule;

        /// <summary>
        /// 设备参数
        /// </summary>
        [JsonIgnore]
        private FlipTableDevicePara flipChipDevicePara => WaferSubDevicePara.GetInstance().FlipChipDevicePara;

        /// <summary>
        /// 程式
        /// </summary>
        private FlipModuleProgram flipModuleProgram => WaferSystemProgram.GetInstance().FlipModuleProgram;

        /// <summary>
        /// 移动翻转T轴电机到指定位置
        /// </summary>
        /// <param name="position">位置</param>
        /// <returns>结果</returns>
        public ExcuteResult MoveFlipTAxis(double position)
        {
            return this.FlipModule.MoveT(position);
        }

        /// <summary>
        /// 移动翻转T轴电机到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="vel">速度</param>
        /// <param name="accuracy">模式</param>
        /// <returns>结果</returns>
        public ExcuteResult MoveFlipTAxis(double pos, double vel, AccuracyMode accuracy = AccuracyMode.HighAccuracy)
        {
            return this.FlipModule.FlipTableAxisT.AbsoluteMove(pos, false, vel, accuracy);
        }

        /// <summary>
        /// 获取T轴坐标
        /// </summary>
        /// <returns>位置</returns>
        public double GetTRealPos()
        {
            return this.FlipModule.FlipTableAxisT.GetRealPosition();
        }

        /// <summary>
        /// 复位翻转机构
        /// </summary>
        public void ResetFlip()
        {
            this.FlipModule.ResetFlip();
        }

        /// <summary>
        /// 关翻转台真空
        /// </summary>
        public void CloseFlipTableVacuum()
        {
            this.FlipModule.CloseFlipModuleVacuum();
        }

        /// <summary>
        /// 关翻转台真空
        /// </summary>
        public void OpenFlipTableVacuum()
        {
            this.FlipModule.OpenFlipModuleVacuum();
        }


        /// <summary>
        /// 获取真空状态
        /// </summary>
        /// <returns>结果</returns>
        public bool GetFlipTableVacuumState()
        {
            return this.FlipModule.FlipTableVacuum.GetOutputValue();
        }

        /// <summary>
        /// 翻转台回零
        /// </summary>
        public void FlipTableGoHome()
        {
            this.FlipModule.FlipModuleGoHome();
        }

        /// <summary>
        /// 翻转台去交接位置
        /// </summary>
        public void MoveFlipToTransferPos()
        {
            if (this.flipModuleProgram.CurrentFlipTool == null)
            {
                this.FlipModule.FlipModuleGoHome();
            }
            else
            {
                this.FlipModule.MoveT(this.flipModuleProgram.CurrentFlipTool.FlipToolTransferPos);
            }
        }

        /// <summary>
        /// 翻转台是否在交接位
        /// </summary>
        /// <returns>结果</returns>
        public bool IsFlipTableAtTransferPos()
        {
            if (this.flipModuleProgram.CurrentFlipTool == null)
            {
              throw new Exception("当前翻转工具为空，无法判断翻转台是否在交接位置");
            }

            return Math.Abs(
                       this.FlipModule.FlipTableAxisT.GetRealPosition()
                       - this.flipModuleProgram.CurrentFlipTool.FlipToolTransferPos) < 0.1;
        }

        /// <summary>
        /// 翻转台是否在0位
        /// </summary>
        /// <returns>结果</returns>
        public bool IsFlipTableAtHome()
        {
            return Math.Abs(this.FlipModule.FlipTableAxisT.GetRealPosition()) < 0.1;
        }

        /// <summary>
        /// 翻转工具上是否有芯片
        /// </summary>
        /// <returns>结果</returns>
        public bool IsComponentOnFlipTool()
        {
            Thread.Sleep(this.flipModuleProgram.CurrentFlipTool.ComponentCheckDelay);

            // 有芯片
            bool hasComponent = this.ReadVacuumValue() < this.flipModuleProgram.CurrentFlipTool.ComponentCheckVal;
            return hasComponent;
        }

        /// <summary>
        /// 开翻转台吹气
        /// </summary>
        public void OpenFlipModuleBlow()
        {
            this.FlipModule.OpenFlipModuleBlow();
        }

        /// <summary>
        /// 关翻转台吹气
        /// </summary>
        public void CloseFlipModuleBlow()
        {
            this.FlipModule.CloseFlipModuleBlow();
        }

        /// <summary>
        /// 获取吹气状态
        /// </summary>
        /// <returns>结果</returns>
        public bool GetFlipTableBlowState()
        {
            return this.FlipModule.GetFlipTableBlowState();
        }

        /// <summary>
        /// 设置吹气比例
        /// </summary>
        /// <param name="value">值</param>
        public void SetBlowProportion(int value)
        {
            this.FlipModule.SetBlowProportion(value);
        }

        /// <summary>
        /// 按指定比例吹气多少毫秒
        /// </summary>
        /// <param name="proportion">比例</param>
        /// <param name="delay">延时</param>
        public void Blow(int proportion, int delay)
        {
            int read = this.ReadWeakBlowProportion();

            if (read != proportion) 
            {
                this.FlipModule.SetBlowProportion(proportion);

                Thread.Sleep(10);
            }

            this.OpenFlipModuleBlow();

            // 延时
            Thread.Sleep(delay);

            this.CloseFlipModuleBlow();
        }

        /// <summary>
        ///  抛料
        /// </summary>
        public void Throw()
        {
            if (this.GetFlipTableVacuumState())
            {
                this.CloseFlipTableVacuum();
            }

            this.SetBlowProportion(this.flipChipDevicePara.ThrowBlowProportion);
            this.OpenFlipModuleBlow();
            Thread.Sleep(200);
            this.CloseFlipModuleBlow();

            Static.IsComponentOnFlipTool = false;
        }

        /// <summary>
        /// 读漏晶值
        /// </summary>
        /// <returns>值</returns>
        public int ReadVacuumValue()
        {
            return this.FlipModule.ReadVacuumValue();
        }

        /// <summary>
        /// 读弱吹比例
        /// </summary>
        /// <returns>弱吹比例</returns>
        public int ReadWeakBlowProportion()
        {
            return this.FlipModule.FlipTableBlowProportionalElectric.ReadRxPDO((ushort)this.FlipModule.FlipTableBlowProportionalElectric.ElectricIO, 1);
        }
    }
}
