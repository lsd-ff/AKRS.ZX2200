using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.ZX2200.SupportFeature.Parameters.Model;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Models.Programs
{
    using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.SupportFeature.Consumables;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool;

    /// <summary>
    /// 翻转模组程式
    /// </summary>
    public class FlipModuleProgram
    {
        /// <summary>
        /// 当前Recipe 使用的翻转工具名称
        /// </summary>
        [TreeProgramListArgs("当前翻转工具", "翻转模组程式")]
        public string FlipToolName { get; set; } = string.Empty;

        /// <summary>
        /// 当前使用的翻转工具
        /// </summary>
        [JsonIgnore]
        public FlipTool CurrentFlipTool => (FlipTool)FlipToolRepository.GetInstance().Find(this.FlipToolName);

        /// <summary>
        /// 翻转工具测高用的吸嘴
        /// </summary>
        [JsonIgnore]
        public Nozzle NozzleForFlipToolMeasureHeight => (Nozzle)NozzleRepository.GetInstance().Find(this.CurrentFlipTool.MeasureHeightNozzle);

        /// <summary>
        /// 获取翻转工具耗材list
        /// </summary>
        /// <returns>result</returns>
        public List<FrequencyConsumables> GetFlipToolFrequencyList()
        {
            List<FrequencyConsumables> list = new List<FrequencyConsumables>();

            if (this.CurrentFlipTool != null)
            {
                list.Add(this.CurrentFlipTool.Frequency);

                list = list?.Filter(a => a != null).Distinct().ToList();
            }

            return list;
        }
    }
}
