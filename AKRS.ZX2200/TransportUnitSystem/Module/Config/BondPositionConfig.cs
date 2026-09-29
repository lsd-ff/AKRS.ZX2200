using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Config
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using DevExpress.XtraEditors;

    /// <summary>
    /// 焊点配置对象
    /// </summary>
    public class BondPositionConfig
    {
        /// <summary>
        /// 单个焊点配置的集合
        /// </summary>
        public List<SingleBondPositionConfig> SingleBpPositionConfigList { get; set; } = new List<SingleBondPositionConfig>();

        /// <summary>
        /// 焊点是否示教完成
        /// </summary>
        /// <returns>结果</returns>
        public bool IsBpAssistanceFinish()
        {
            foreach (SingleBondPositionConfig singleBondPositionConfig in this.SingleBpPositionConfigList)
            {
                if (!singleBondPositionConfig.IsAssistantSucceed)
                {
                    AKRSXtraMessageBox.Show($"Please Assistant BondPosition Named {singleBondPositionConfig.Name} First");
                    return false;
                }
            }

            return true;
        }
    }
}
