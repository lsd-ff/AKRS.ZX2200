using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Models.Programs
{
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
    using DevExpress.Office.Utils;
    using System.Collections.Generic;
    using System.Linq;

    using AKRS.ZX2200.Consumables;
    using AKRS.ZX2200.SupportFeature.Consumables;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    /// <summary>
    /// 描述：顶针架程式
    /// </summary>
    public class EjectionBankProgram
    {
        /// <summary>
        /// 当前Recipe 使用的顶针架名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 当前使用的顶针架配置
        /// </summary>
        [JsonIgnore]
        [TreeProgramListArgs("CurrentBankSetting", "EjectionBankProgram")]
        public EjectionBankConfig CurrentBankConfig => (EjectionBankConfig)EjectionBankConfigRepository.GetInstance().Find(this.Name);

        /// <summary>
        /// EjectionBankEntity
        /// </summary>
        public EjectionBankEntity EjectionBankEntity { get; set; }

        /// <summary>
        /// CreateEjectionBankEntity
        /// </summary>
        public void CreateEjectionBankEntity()
        {
            this.EjectionBankEntity = new EjectionBankEntity(5);
            for (int i = 0; i < this.CurrentBankConfig.EjectionBankSlots.Length; i++)
            {
                this.EjectionBankEntity.EjectionBankSlotEntities[i] = new EjectionBankSlotEntity(this.CurrentBankConfig.EjectionBankSlots[i]);
            }

            WaferSystemProgram.GetInstance().Save();
        }

        /// <summary>
        /// 获取顶针耗材list
        /// </summary>
        /// <returns>result</returns>
        public List<FrequencyConsumables> GetEjectFrequencyList()
        {
            List<FrequencyConsumables> list = new List<FrequencyConsumables>();

            if (this.CurrentBankConfig != null)
            {
                foreach (var item in this.CurrentBankConfig.EjectionBankSlots)
                {
                    if (item.EjectionConfig != null)
                    {
                        if (item.EjectionConfig?.Frequency == null || item.EjectionConfig?.Frequency?.Name != item.EjectionConfig.Name)
                        {
                            item.EjectionConfig.Frequency = new FrequencyConsumables(item.EjectionConfig.Name);

                            EjectionBankConfigRepository.GetInstance().Save();
                        }

                        list.Add(item.EjectionConfig?.Frequency);
                    }
                }

                list = list?.Filter(a => a != null).Distinct().ToList();
            }

            return list;
        }
    }
}
