#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/15 13:00:11
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

using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.Bondinsert;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.InOutPut;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.TansportBelt;
using DevExpress.XtraEditors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportSystem.Models.Programs
{
    /// <summary>
    /// 描述：下料等待载台
    /// </summary>
    public class WaitingUnloadSubSectionProgram : BaseSubSectionProgram
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
        /// 结果
        /// </summary>
        /// <returns>是否为Null</returns>
        public bool IsSettingNull()
        {
            if (TransportBeltSetting == null)
            {
                AKRSXtraMessageBox.Show("Waiting Unload SubSection Transport BeltSetting is Null");
                return false;
            }

            return true;
        }
    }
}
