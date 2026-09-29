using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using Newtonsoft.Json;

namespace AKRS.ZX2200.BondSystem.Modules
{
    /// <summary>
    /// 吸嘴架模组
    /// </summary>
    public class NozzleShelfModule
    {
        /// <summary>
        /// 坐标转换系统
        /// </summary>
        [JsonIgnore]
        public GeneralCoordinateSystem ToolBankCoordinateSystem  =>
            (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems
                .Find(it => it.Name == "ToolBankCoordinateSystem");

        /// <summary>
        /// 吸嘴架Y轴
        /// </summary>
        [JsonIgnore]
        public Axis AxisY => HardwareRepositoryService.GetHardware<Axis>("焊头放置架Y");

        /// <summary>
        /// 吸嘴架Y轴回零传感器
        /// </summary>
        [JsonIgnore]
        public Sensor ZeroSensorPickerHolderAxisY => HardwareRepositoryService.GetHardware<Sensor>("吸嘴架Y轴回零传感器");

        /// <summary>
        /// 吸嘴架Y轴正限位传感器
        /// </summary>
        [JsonIgnore]
        public Sensor PositiveLimitSensorPickerHolderAxisY => HardwareRepositoryService.GetHardware<Sensor>("吸嘴架Y轴正限位传感器");

        /// <summary>
        /// 吸嘴检测传感器1
        /// </summary>
        [JsonIgnore]
        public Sensor PickerHolderSensor1 => HardwareRepositoryService.GetHardware<Sensor>("焊头吸嘴从右到左1检测");

        /// <summary>
        /// 吸嘴检测传感器2
        /// </summary>
        [JsonIgnore]
        public Sensor PickerHolderSensor2 => HardwareRepositoryService.GetHardware<Sensor>("焊头吸嘴从右到左2检测");

        /// <summary>
        /// 吸嘴检测传感器3
        /// </summary>
        [JsonIgnore]
        public Sensor PickerHolderSensor3 => HardwareRepositoryService.GetHardware<Sensor>("焊头吸嘴从右到左3检测");

        /// <summary>
        /// 吸嘴检测传感器4
        /// </summary>
        [JsonIgnore]
        public Sensor PickerHolderSensor4 => HardwareRepositoryService.GetHardware<Sensor>("焊头吸嘴从右到左4检测");

        /// <summary>
        /// 吸嘴检测传感器5
        /// </summary>
        [JsonIgnore]
        public Sensor PickerHolderSensor5 => HardwareRepositoryService.GetHardware<Sensor>("焊头吸嘴从右到左5检测");

        /// <summary>
        /// 吸嘴检测传感器6
        /// </summary>
        [JsonIgnore]
        public Sensor PickerHolderSensor6 => HardwareRepositoryService.GetHardware<Sensor>("焊头吸嘴从右到左6检测");

        /// <summary>
        /// 吸嘴检测传感器7
        /// </summary>
        [JsonIgnore]
        public Sensor PickerHolderSensor7 => HardwareRepositoryService.GetHardware<Sensor>("焊头吸嘴从右到左7检测");

        /// <summary>
        /// 吸嘴检测传感器8
        /// </summary>
        [JsonIgnore]
        public Sensor PickerHolderSensor8 => HardwareRepositoryService.GetHardware<Sensor>("焊头吸嘴从右到左8检测");

        /// <summary>
        /// 吸嘴检测传感器9
        /// </summary>
        [JsonIgnore]
        public Sensor PickerHolderSensor9 => HardwareRepositoryService.GetHardware<Sensor>("焊头吸嘴从右到左9检测");

        /// <summary>
        /// 吸嘴检测传感器10
        /// </summary>
        [JsonIgnore]
        public Sensor PickerHolderSensor10 => HardwareRepositoryService.GetHardware<Sensor>("焊头吸嘴从右到左10检测");

        /// <summary>
        /// 吸嘴检测传感器11
        /// </summary>
        [JsonIgnore]
        public Sensor PickerHolderSensor11 => HardwareRepositoryService.GetHardware<Sensor>("焊头吸嘴从右到左11检测");

        /// <summary>
        /// 吸嘴检测传感器12
        /// </summary>
        [JsonIgnore]
        public Sensor PickerHolderSensor12 => HardwareRepositoryService.GetHardware<Sensor>("焊头吸嘴从右到左12检测");

        #region 方法

        /// <summary>
        /// 吸嘴架回0
        /// </summary>
        /// <returns>结果</returns>
        public ExcuteResult MoveShelfToHome()
        {            
            // 先移动到0位
            return this.AxisY.AbsoluteMove(-1,true);
        }

        /// <summary>
        /// 吸嘴架回0不等待
        /// </summary>
        /// <returns>结果</returns>
        public ExcuteResult MoveShelfToHomeNoWait()
        {
            return this.AxisY.SendAbsoluteMoveCommand(0);
        }

        /// <summary>
        /// 吸嘴架是否已满
        /// </summary>
        /// <returns>结果</returns>
        //public bool IsNozzleShelfFull()
        //{
        //    if (this.PickerHolderSensor1.GetInputValue() && this.PickerHolderSensor2.GetInputValue()
        //                                                 && this.PickerHolderSensor3.GetInputValue()
        //                                                 && this.PickerHolderSensor4.GetInputValue()
        //                                                 && this.PickerHolderSensor5.GetInputValue()
        //                                                 && this.PickerHolderSensor6.GetInputValue()
        //                                                 && this.PickerHolderSensor7.GetInputValue())
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        /// <summary>
        /// 吸嘴槽上是否有吸嘴-从1开始
        /// </summary>
        /// <param name="slotNum">吸嘴槽位号</param>
        /// <returns>结果</returns>
        public bool IsSlotHaveNozzle(int slotNum)
        {
            switch (slotNum)
            {
                case 1:
                    return this.PickerHolderSensor1.GetInputValue();

                case 2:
                    return this.PickerHolderSensor2.GetInputValue();

                case 3:
                    return this.PickerHolderSensor3.GetInputValue();

                case 4:
                    return this.PickerHolderSensor4.GetInputValue();

                case 5:
                    return this.PickerHolderSensor5.GetInputValue();

                case 6:
                    return this.PickerHolderSensor6.GetInputValue();

                case 7:
                    return this.PickerHolderSensor7.GetInputValue();

                case 8:
                    return this.PickerHolderSensor8.GetInputValue();

                case 9:
                    return this.PickerHolderSensor9.GetInputValue();

                case 10:
                    return this.PickerHolderSensor10.GetInputValue();

                case 11:
                    return this.PickerHolderSensor11.GetInputValue();

                case 12:
                    return this.PickerHolderSensor12.GetInputValue();
            }

            return false;
        }

        #endregion
    }
}
