using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities
{
    using AKRS.Galaxy2.Infrastructure.Helper;

    using Newtonsoft.Json;

    /// <summary>
    /// EjectionBankSlotEntity
    /// </summary>
    public class EjectionBankSlotEntity
    {
        /// <summary>
        /// EjectionBankSlotEntity
        /// </summary>
        /// <param name="ejectionBankSlotConfig">ejectionBankSlotConfig</param>
        public EjectionBankSlotEntity(EjectionBankSlotConfig ejectionBankSlotConfig)
        {
            if (ejectionBankSlotConfig == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(ejectionBankSlotConfig.Name))
            {
                this.SlotState = EjectSlotStatuEnum.UnUsing;
            }
            else
            {
                this.SlotState = EjectSlotStatuEnum.Empty;
            }

            this.EjectionBankSlotConfig = ejectionBankSlotConfig;
        }

        /// <summary>
        /// EjectionBankSlotConfig
        /// </summary>
        public EjectionBankSlotConfig EjectionBankSlotConfig { get; set; }

        /// <summary>
        /// 槽位状态
        /// </summary>
        public EjectSlotStatuEnum SlotState { get; set; } = EjectSlotStatuEnum.Empty;

        /// <summary>
        /// 槽位状态描述
        /// </summary>
        [JsonIgnore]
        public string SlotStateStr => this.SlotState.GetDescription();
    }
}
