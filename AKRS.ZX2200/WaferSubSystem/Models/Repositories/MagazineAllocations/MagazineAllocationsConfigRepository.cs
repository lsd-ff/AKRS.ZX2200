using AKRS.ZX2200.Models;

using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    /// <summary>
    /// MagazineAllocationsRepository
    /// </summary>
    public class MagazineAllocationsConfigRepository : BaseSettingRepository<MagazineAllocationsConfig>
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static MagazineAllocationsConfigRepository instance;

        /// <summary>
        /// 存储文件名称
        /// </summary>
        [JsonIgnore]
        public static string FileName => ZX2200PathConfig.MagazineAllocationsRepository;

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
        private MagazineAllocationsConfigRepository()
        {
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static MagazineAllocationsConfigRepository GetInstance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new MagazineAllocationsConfigRepository();
                }

                return instance;
            }
        }
    }
}
