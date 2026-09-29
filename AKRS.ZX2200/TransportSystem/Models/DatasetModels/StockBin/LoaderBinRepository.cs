namespace AKRS.ZX2200.TransportSystem.Models.DatasetModels.StockBin
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    using Newtonsoft.Json;

    /// <summary>
    /// 上料盒库
    /// </summary>
    public class LoaderBinRepository : BaseSettingRepository<LoaderBin>
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static LoaderBinRepository instance;

        /// <summary>
        /// 存储文件名称
        /// </summary>
        [JsonIgnore]
        public static string FileName => ZX2200PathConfig.LoaderBinRepositoryFilePath;

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
        private LoaderBinRepository()
        {
        }

        /// <summary>
        /// 获取料盒，不存在就返回空
        /// </summary>
        /// <param name="name">吸嘴名</param>
        /// <returns>吸嘴对象</returns>
        public LoaderBin GetLoaderBin(string name)
        {
            return (LoaderBin)this.Find(name);
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static LoaderBinRepository GetInstance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new LoaderBinRepository();
                }

                return instance;
            }
        }
    }
}
