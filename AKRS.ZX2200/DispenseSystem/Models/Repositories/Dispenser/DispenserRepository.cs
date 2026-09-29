using AKRS.ZX2200.Models;

using Newtonsoft.Json;
using System;

namespace AKRS.ZX2200.DispenseSystem.Models.Repositories.Dispenser
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    /// <summary>
    /// 点胶头库
    /// </summary>
    [Serializable]
    public class DispenserRepository : BaseSettingRepository<Dispenser>
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static DispenserRepository instance;

        /// <summary>
        /// 存储文件名称
        /// </summary>
        [JsonIgnore]
        public static string FileName => ZX2200PathConfig.DispenserRepositoryFilePath;

        /// <summary>
        /// 获取文件存储名称
        /// </summary>
        /// <returns>名称</returns>
        protected override string GetFileName()
        {
            return FileName;
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static DispenserRepository GetInstance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new DispenserRepository();
                }

                return instance;
            }
        }
    }
}
