namespace AKRS.ZX2200.TransportSystem.Models.DatasetModels.StockBin
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    using Newtonsoft.Json;

    /// <summary>
    /// 下料盒库
    /// </summary>
    public class UnLoaderBinRepository : BaseSettingRepository<UnloaderBin>
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static UnLoaderBinRepository instance;

        /// <summary>
        /// 存储文件名称
        /// </summary>
        [JsonIgnore]
        public static string FileName => ZX2200PathConfig.UnLoaderBinRepositoryFilePath;

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
        private UnLoaderBinRepository()
        {
        }

        /// <summary>
        /// 获取料盒，不存在就返回空
        /// </summary>
        /// <param name="name">吸嘴名</param>
        /// <returns>吸嘴对象</returns>
        public UnloaderBin GetLoaderBin(string name)
        {
            return (UnloaderBin)this.Find(name);
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static UnLoaderBinRepository GetInstance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new UnLoaderBinRepository();
                }

                return instance;
            }
        }
    }
}
