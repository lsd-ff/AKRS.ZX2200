namespace AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Models.FlatLevelMeasure.System1Related
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    using Newtonsoft.Json;

    /// <summary>
    /// 系统1平面测试点位库
    /// </summary>
    public class System1FlatLevelMeasureRepository : BaseSettingRepository<System1FlatLevelMeasureSetting>
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static System1FlatLevelMeasureRepository instance;

        /// <summary>
        /// 存储文件名称
        /// </summary>
        [JsonIgnore]
        public static string FileName => ZX2200PathConfig.System1FlatLevelMeasureFilePath;

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
        private System1FlatLevelMeasureRepository()
        {
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static System1FlatLevelMeasureRepository GetInstance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new System1FlatLevelMeasureRepository();
                }

                return instance;
            }
        }
    }
}
