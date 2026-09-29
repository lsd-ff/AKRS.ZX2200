using AKRS.ZX2200.Models;

using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    /// <summary>
    /// AdapterRepository
    /// </summary>
    public class AdapterConfigRepository : BaseSettingRepository<AdapterConfig>
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static AdapterConfigRepository instance;

        /// <summary>
        /// 存储文件名称
        /// </summary>
        [JsonIgnore]
        public static string FileName => ZX2200PathConfig.AdapterRepository;

        /// <summary>
        /// 存储文件名称
        /// </summary>
        /// <returns>存储文件名称</returns>
        protected override string GetFileName()
        {
            return FileName;
        }

        /// <summary>
        /// 私有化
        /// </summary>
        private AdapterConfigRepository()
        {
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static AdapterConfigRepository GetInstance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new AdapterConfigRepository();
                }

                return instance;
            }
        }
    }
}
