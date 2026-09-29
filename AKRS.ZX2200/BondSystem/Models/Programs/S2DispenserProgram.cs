using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
using AKRS.ZX2200.DispenseSystem.Models.Enums;
using AKRS.ZX2200.DispenseSystem.Models.Repositories.Dispenser;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.Models.Programs
{
    using AKRS.ZX2200.BondSystem.Models.Parameter;

    /// <summary>
    /// 系统2点胶程式
    /// </summary>
    public class S2DispenserProgram
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
        /// 这个点胶头和标准点胶头的高度差值
        /// </summary>
        public double DispenserHeightDifference { get; set; }

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
                string message = "点胶头没有示教完成";
                AKRSXtraMessageBox.Show(message);
                return false;
            }

            return true;
        }
    }
}
