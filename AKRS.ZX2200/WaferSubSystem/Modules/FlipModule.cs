namespace AKRS.ZX2200.WaferSubSystem.Modules
{
    using System;
    using System.Threading;

    using AKRS.Galaxy2.Drive.Common;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;

    using Newtonsoft.Json;

    /// <summary>
    /// 倒装模组
    /// </summary>
    public class FlipModule
    {
        /// <summary>
        /// 翻转台T轴
        /// </summary>        
        [JsonIgnore]
        public Axis FlipTableAxisT  => HardwareRepositoryService.GetHardware<Axis>("翻转台T");

        /// <summary>
        /// 翻转台吸嘴真空
        /// </summary>        
        [JsonIgnore]
        public Electric FlipTableVacuum => HardwareRepositoryService.GetHardware<Electric>("翻转台真空电磁阀");

        /// <summary>
        /// 弱吹比例阀
        /// </summary>
        [JsonIgnore]
        public Electric FlipTableBlowProportionalElectric => HardwareRepositoryService.GetHardware<Electric>("FC比例阀设置");

        /// <summary>
        /// 翻转台吸嘴真空
        /// </summary>        
        [JsonIgnore]
        public Electric FlipTableBlow => HardwareRepositoryService.GetHardware<Electric>("翻转台吹气");

        /// <summary>
        /// 翻转台吸嘴真空
        /// </summary>        
        [JsonIgnore]
        public Sensor FlipTableCheckVaccumSensor => HardwareRepositoryService.GetHardware<Sensor>("翻转台漏晶检测");

        /// <summary>
        /// 移动翻转T轴电机到指定位置
        /// </summary>
        /// <param name="position">位置</param>
        /// <returns>结果</returns>
        public ExcuteResult MoveT(double position)
        {
            return this.FlipTableAxisT.AbsoluteMove(position, AccuracyMode.HighAccuracy);
        }

        /// <summary>
        /// 复位翻转机构
        /// </summary>
        public void ResetFlip()
        {
           this.FlipTableVacuum.SetOutputValue(false);
           this.FlipTableAxisT.AbsoluteMove(0);
        }

        /// <summary>
        /// 关翻转台真空
        /// </summary>
        public void CloseFlipModuleVacuum()
        {
            this.FlipTableVacuum.SetOutputValue(false);
        }

        /// <summary>
        /// 关翻转台真空
        /// </summary>
        public void OpenFlipModuleVacuum()
        {
            this.FlipTableVacuum.SetOutputValue(true);
        }

        /// <summary>
        /// 关翻转台吹气
        /// </summary>
        public void OpenFlipModuleBlow()
        {
            this.FlipTableBlow.SetOutputValue(true);
        }

        /// <summary>
        /// 关翻转台吹气
        /// </summary>
        public void CloseFlipModuleBlow()
        {
            this.FlipTableBlow.SetOutputValue(false);
        }

        /// <summary>
        /// 顶针T轴回零
        /// </summary>
        public void FlipModuleGoHome()
        {
            this.FlipTableAxisT.AbsoluteMove(0);
        }

        /// <summary>
        ///  获取吹气状态
        /// </summary>
        /// <returns>结果</returns>
        public bool GetFlipTableBlowState()
        {
            return this.FlipTableBlow.GetOutputValue();
        }


        /// <summary>
        /// 设置吹气比例
        /// </summary>
        /// <param name="value">值</param>
        public void SetBlowProportion(int value)
        {
            this.FlipTableBlowProportionalElectric.WriteRxPDO((ushort)this.FlipTableBlowProportionalElectric.ElectricIO, 1, value);
        }

        /// <summary>
        /// 读漏晶值
        /// </summary>
        /// <returns>值</returns>
        public int ReadVacuumValue()
        {
            //bool a = this.FlipTableCheckVaccumSensor.GetInputValue();

            return this.FlipTableCheckVaccumSensor.ReadTxPDO((ushort)this.FlipTableCheckVaccumSensor.SensorIO, 1);

        }
    }
}
