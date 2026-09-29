using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.Models;

using Newtonsoft.Json;

namespace AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle
{
    using System.Collections.Generic;
    using AKRS.ZX2200.Consumables;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;
    using AKRS.ZX2200.SupportFeature.Consumables;

    /// <summary>
    /// 吸嘴库
    /// </summary>
    public class NozzleRepository : BaseSettingRepository<Nozzle>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NozzleRepository()
        {
            Init();
        }

        /// <summary>
        /// 存储文件名称
        /// </summary>
        [JsonIgnore]
        public static string FileName => ZX2200PathConfig.NozzleRepositoryPath;

        /// <summary>
        /// 获取存储文件名称
        /// </summary>
        /// <returns>存储文件名称</returns>
        protected override string GetFileName()
        {
            return FileName;
        }

        /// <summary>
        /// 获取吸嘴，不存在就返回空
        /// </summary>
        /// <param name="name">吸嘴名</param>
        /// <returns>吸嘴对象</returns>
        public Nozzle GetNozzle(string name)
        {
            return (Nozzle)Find(name);
        }

        /// <summary>
        /// 单例
        /// </summary>
        private static NozzleRepository instance;

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static NozzleRepository GetInstance()
        {
            lock (BaseSettingRepository<Nozzle>.locker)
            {
                if (instance == null)
                {
                    instance = new NozzleRepository();
                }

                return instance;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            if (this.Find("TouchDown") == null)
            {
                // 系统自带的TouchDown
                Nozzle touchDown = new Nozzle()
                {
                    Name = "TouchDown",
                    NozzleType = NozzleTypeEnum.Calibrate,
                    NozzleSize = NozzleSizeEnum.Small,
                    MeasureHeightOffset = 0,
                    IsSystemConfiguration = true
                };

                this.BaseDsSettingList.Add(touchDown);
                touchDown.AddBelongRecipeIds();
            }

            if (this.Find("BMC") == null)
            {
                // 系统自带的BMC 测试工具
                Nozzle bMC = new Nozzle()
                                 {
                                     Name = "BMC",
                                     NozzleType = NozzleTypeEnum.Calibrate,
                                     NozzleSize = NozzleSizeEnum.Small,
                                     MeasureHeightOffset = 0,
                                     IsSystemConfiguration = true
                                 };

                this.BaseDsSettingList.Add(bMC);
                bMC.AddBelongRecipeIds();
            }
        }

        /// <summary>
        ///  获取使用频率
        /// </summary>
        /// <returns>使用频率</returns>
        public List<FrequencyConsumables> GetFrequencyConsumables()
        {
            List<FrequencyConsumables> frequencyConsumablesList = new List<FrequencyConsumables>();

            foreach (var nozzle in this.GetDataSourceByCurrentRecipe()) 
            {
                frequencyConsumablesList.Add(nozzle.FrequencyConsumables); 
            }

            return frequencyConsumablesList;
        }
    }
}
