using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.DispenseSystem.Models.Repositories.Dispenser
{
    using AKRS.ZX2200.DispenseSystem.Models.Enums;

    /// <summary>
    /// 点胶头里面的预点胶参数
    /// </summary>
    [Serializable]
    public class PreDispense
    {
        #region Pre-dispense

        /// <summary>
        /// 预点胶模式
        /// 这个应该会根据自己的要求去加的
        /// </summary>
        public PreDispensingModeEnum PreDispensingMode { get; set; } = PreDispensingModeEnum.Off;

        /// <summary>
        /// 预点胶模式
        /// 这个应该会根据自己的要求去加的
        /// </summary>
        public PreDispensingTimingEnum PreDispensingTiming { get; set; } = PreDispensingTimingEnum.Off;

        /// <summary>
        /// 预点胶要用的哪种图案
        /// </summary>
        public string EpoxyApplicationName { get; set; }


        /// <summary>
        /// 预点胶周期时间，单位为ms，界面自动转换
        /// 当预点胶模式选择周期性的时候才会打开
        /// 单位为毫秒
        /// </summary>
        public int PreDispensingTimeInterval { get; set; } = 90000;

        /// <summary>
        /// 挤胶时间
        /// </summary>
        public int EpoxySqueezeTime { get; set; } = 1000;

        /// <summary>
        /// 挤胶压力
        /// </summary>
        public int EpoxySqueezePressure { get; set; } = 100;

        /// <summary>
        /// 挤胶功能是否开启
        /// </summary>
        public bool EnableEpoxySqueeze { get; set; } = false;

        /// <summary>
        /// 预点胶重复的次数
        /// </summary>
        public int RepeatPreDispensing { get; set; } = 1;

        /// <summary>
        /// 预点胶偏移X，这个是针对上一次的偏移
        /// </summary>
        public double PreDispensingOffsetX { get; set; }

        /// <summary>
        /// 预点胶偏移Y，这个是针对上一次的偏移
        /// </summary>
        public double PreDispensingOffsetY { get; set; }


        #endregion

        #region Pre-dispense Check

        /// <summary>
        /// 预点胶之后的检查
        /// 这个是调用焊后检测的模板
        /// </summary>
        public string PreDispenseCheckName { get; set; } = "Null";

        #endregion
    }
}
