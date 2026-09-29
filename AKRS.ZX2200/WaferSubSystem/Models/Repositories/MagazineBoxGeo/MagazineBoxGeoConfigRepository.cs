using AKRS.ZX2200.Models;

using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBoxGeo
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    /// <summary>
    /// 芯片模板-晶圆料片
    /// </summary>
    public class MagazineBoxGeoConfigRepository : BaseSettingRepository<MagazineBoxGeoConfig>
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static MagazineBoxGeoConfigRepository instance;

        /// <summary>
        /// 存储文件名称
        /// </summary>
        [JsonIgnore]
        public static string FileName => ZX2200PathConfig.MagazineBoxGeoRepository;

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
        private MagazineBoxGeoConfigRepository()
        {
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static MagazineBoxGeoConfigRepository GetInstance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new MagazineBoxGeoConfigRepository();
                }

                return instance;
            }
        }
    }
}
