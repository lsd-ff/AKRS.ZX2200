using System;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.ZX2200.DispenseSystem.Models.Repositories.Dispenser;
using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
using AKRS.ZX2200.Models;
using Newtonsoft.Json;

namespace AKRS.ZX2200.DispenseSystem.Models.Programs
{
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.Infrastructure.AOP.Module;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Setting;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using PropertyChanged;

    /// <summary>
    /// 点胶头实体
    /// </summary>
    public class DispenserProgram
    {
        /// <summary>
        /// 点胶参数的名称
        /// </summary>
        public string DispenserName { get; set; }

        /// <summary>
        /// 点胶头的参数
        /// </summary>
        [JsonIgnore]
        public Dispenser Dispenser => (Dispenser)DispenserRepository.GetInstance().Find(this.DispenserName);

        /// <summary>
        /// 最后一次预点胶的时间
        /// </summary>
        public DateTime LastPreDispenseTime { get; set; }

        /// <summary>
        /// 初始化点胶头
        /// </summary>
        public void Init()
        {
            this.LastPreDispenseTime = DateTime.Now;
        }

        /// <summary>
        /// 点胶头是否准备好工作
        /// </summary>
        /// <returns>结果</returns>
        public bool IsReady()
        {
            if (this.Dispenser == null)
            {
                AKRSMessageBoxExt.Show(
                    $"点胶针未配置",
                    "提示",
                    new string[] { "确定" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);
                return false;
            }

            if (!this.Dispenser.IsAssistantSucceed)
            {
                AKRSMessageBoxExt.Show(
                    $"点胶针未示教",
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
