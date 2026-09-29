using AKRS.ZX2200.Models;

using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    /// <summary>
    /// 顶针架库
    /// </summary>
    public class EjectionBankConfigRepository : BaseSettingRepository<EjectionBankConfig>
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static EjectionBankConfigRepository instance;

        /// <summary>
        /// 存储文件名称
        /// </summary>
        [JsonIgnore]
        public static string FileName => ZX2200PathConfig.EjectionBankRepository;

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
        private EjectionBankConfigRepository()
        {
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static EjectionBankConfigRepository GetInstance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new EjectionBankConfigRepository();
                }

                return instance;
            }
        }
    }
}
