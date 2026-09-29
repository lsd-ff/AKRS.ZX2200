namespace AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf
{
    using System.Collections.Generic;

    using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;
    using AKRS.ZX2200.Models;

    using Newtonsoft.Json;

    /// <summary>
    /// 吸嘴库
    /// </summary>
    public class NozzleShelfRepository : BaseSettingRepository<NozzleShelf>
    {
        /// <summary>
        /// 存储文件名称
        /// </summary>
        [JsonIgnore]
        public static string FileName => ZX2200PathConfig.NozzleShelfRepositoryPath;

        /// <summary>
        /// 获取存储文件名称
        /// </summary>
        /// <returns>存储文件名称</returns>
        protected override string GetFileName()
        {
            return FileName;
        }

        /// <summary>
        /// 获取吸嘴架，不存在就返回空
        /// </summary>
        /// <param name="name">吸嘴名</param>
        /// <returns>吸嘴对象</returns>
        public NozzleShelf GetNozzleShelf(string name)
        {
            return (NozzleShelf)Find(name);
        }

        /// <summary>
        /// 单例
        /// </summary>
        private static NozzleShelfRepository instance;

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static NozzleShelfRepository GetInstance()
        {
            lock (NozzleShelfRepository.locker)
            {
                if (instance == null)
                {
                    instance = new NozzleShelfRepository();
                }

                return instance;
            }
        }
    }
}