using AKRS.ZX2200.Models;

using Newtonsoft.Json;

namespace AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    /// <summary>
    /// 画胶图形库
    /// </summary>
    public class EpoxyApplicationRepository : BaseSettingRepository<EpoxyApplication>
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static EpoxyApplicationRepository instance;

        /// <summary>
        /// 存储文件名称
        /// </summary>
        [JsonIgnore]
        public static string FileName => ZX2200PathConfig.EpoxyApplicationRepositoryPath;

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
        public static EpoxyApplicationRepository GetInstance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new EpoxyApplicationRepository();
                }
                return instance;
            }
        }
    }
}
