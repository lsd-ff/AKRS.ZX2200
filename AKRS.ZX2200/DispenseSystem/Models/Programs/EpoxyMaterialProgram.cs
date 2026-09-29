using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.ZX2200.DispenseSystem.Models.Repositories.EpoxyMaterial;
using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
using Newtonsoft.Json;

namespace AKRS.ZX2200.DispenseSystem.Models.Programs
{
    using AKRS.ZX2200.DispenseSystem.Modules;
    using AKRS.ZX2200.Models;

    using DevExpress.CodeParser;
    using DevExpress.XtraPrinting.Native;
    using System.Windows.Forms;

    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    /// <summary>
    /// 胶水实体类
    /// </summary>
    public class EpoxyMaterialProgram
    {
        /// <summary>
        /// 胶水参数
        /// </summary>
        [JsonIgnore]
        public EpoxyMaterial EpoxyMaterial =>
            (EpoxyMaterial)EpoxyMaterialRepository.GetInstance().Find(this.EpoxyMaterialName);

        /// <summary>
        /// 点胶参数的名称
        /// </summary>
        public string EpoxyMaterialName { get; set; }

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
            if (this.EpoxyMaterial == null)
            {
                AKRSMessageBoxExt.Show(
                    $"胶水未配置",
                    "提示",
                    new string[] { "确定" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);
                return false;
            }

            return true;
        }
    }
}
