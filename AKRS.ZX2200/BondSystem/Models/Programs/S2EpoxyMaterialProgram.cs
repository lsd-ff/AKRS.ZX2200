using AKRS.ZX2200.DispenseSystem.Models.Repositories.EpoxyMaterial;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.Models.Programs
{
    /// <summary>
    /// 系统2胶水程式
    /// </summary>
    public class S2EpoxyMaterialProgram
    {
        /// <summary>
        /// 胶水参数
        /// </summary>
        [JsonIgnore]
        public EpoxyMaterial EpoxyMaterial => (EpoxyMaterial)EpoxyMaterialRepository.GetInstance().Find(EpoxyMaterialName);

        /// <summary>
        /// 点胶参数的名称
        /// </summary>
        public string EpoxyMaterialName { get; set; }

        /// <summary>
        /// 当前已经提醒的次数
        /// </summary>
        public int ServiceLifePreWarningTimes { get; set; }

        /// <summary>
        /// 胶水ID号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            this.Id = string.Empty;
        }

        /// <summary>
        /// 是否准备好了
        /// </summary>
        /// <returns>结果</returns>
        public bool IsReady()
        {
            // Todo 目前没有材料，后续改
            return true;
            if (this.EpoxyMaterial == null)
            {
                string message = "Please complete the EpoxyMaterial setup first";
                AKRSXtraMessageBox.Show(message);
                return false;
            }

            return true;
        }
    }
}
