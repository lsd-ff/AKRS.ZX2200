#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/15 12:58:49
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

using AKRS.ZX2200.TransportSystem.Models.DatasetModels.Bondinsert;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.TansportBelt;
using DevExpress.XtraEditors;
using Newtonsoft.Json;

namespace AKRS.ZX2200.TransportSystem.Models.Programs
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using System.ComponentModel;

    /// <summary>
    /// 描述：Bond 载台
    /// </summary>
    public class BondSubSectionProgram : BaseSubSectionProgram
    {
        /// <summary>
        /// 传输系统皮带设置的名称
        /// </summary>
        public string TransportBeltSettingName { get; set; } = string.Empty;

        /// <summary>
        /// 传输系统皮带设置的对象
        /// </summary>
        [JsonIgnore]
        public TransportBeltSetting TransportBeltSetting =>
            (TransportBeltSetting)TransportBeltSettingRepository.GetInstance().Find(this.TransportBeltSettingName);

        /// <summary>
        /// 载具类型
        /// </summary>
        public BondMaxSubSectionSizeEnum Size { get; set; } = BondMaxSubSectionSizeEnum.Middle;

        /// <summary>
        /// 加热温度
        /// </summary>
        public int HeaterTemperature { get; set; } = 100;

        /// <summary>
        /// 报警温度
        /// </summary>
        public int AlarmTemperature { get; set; } = 5;

        /// <summary>
        /// 补偿温度
        /// </summary>
        public double CompensateTemperature { get; set; } = 0;

        /// <summary>
        /// 补偿斜率
        /// </summary>
        public double CompensateSlope { get; set; } = 1.0;

        /// <summary>
        /// 预热时间
        /// </summary>
        public int PreheatTime { get; set; } = 5;

        /// <summary>
        /// 检测真空时间
        /// </summary>
        public int CheckVacuumTime { get; set; } = 1000;

        /// <summary>
        /// 挡料气缸相关设置的名称
        /// </summary>
        public string BondinsertSettingName { get; set; } = string.Empty;

        /// <summary>
        /// 挡料气缸相关设置的对象
        /// </summary>
        [JsonIgnore]
        public BondinsertSetting BondinsertSetting =>
            (BondinsertSetting)BondinsertSettingRepository.GetInstance().Find(this.BondinsertSettingName);


        /// <summary>
        /// 结果
        /// </summary>
        /// <returns>是否为Null</returns>
        public bool IsSettingNull()
        {
            if (TransportBeltSetting == null)
            {
                AKRSXtraMessageBox.Show("Bond SubSection Transport BeltSetting is Null");
                return false;
            }

            if (BondinsertSetting == null)
            {
                AKRSXtraMessageBox.Show("Bond SubSection BeltSetting is Null");
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// 载具大小
    /// </summary>
    public enum BondMaxSubSectionSizeEnum
    {
        /// <summary>
        /// 中
        /// </summary>
        [Description("中")]
        Middle,

        /// <summary>
        /// 中
        /// </summary>
        [Description("中前")]
        MiddleAndFront,

        /// <summary>
        /// 大
        /// </summary>
        [Description("中右")]
        MaxRight,

        /// <summary>
        /// 大
        /// </summary>
        [Description("全部")]
        All
    }
}
