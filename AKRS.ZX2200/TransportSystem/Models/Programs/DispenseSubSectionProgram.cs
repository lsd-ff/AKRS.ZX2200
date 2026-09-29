#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/15 12:58:29
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.Bondinsert;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.TansportBelt;
using DevExpress.XtraEditors;
using Newtonsoft.Json;

namespace AKRS.ZX2200.TransportSystem.Models.Programs
{
    /// <summary>
    /// 描述：点胶载台程式
    /// </summary>
    public class DispenseSubSectionProgram : BaseSubSectionProgram
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
        /// 载具是否在第一段
        /// </summary>
        [JsonIgnore]
        public bool IsTuInDispense1 { get; set; } = true;

        /// <summary>
        /// 结果
        /// </summary>
        /// <returns>是否为Null</returns>
        public bool IsSettingNull()
        {
            if (TransportBeltSetting == null)
            {
                AKRSXtraMessageBox.Show("Dispense SubSection Transport BeltSetting is Null");
                return false;
            }

            if (BondinsertSetting == null)
            {
                AKRSXtraMessageBox.Show("Dispense SubSection BeltSetting is Null");
                return false;
            }

            return true;
        }
    }
}
