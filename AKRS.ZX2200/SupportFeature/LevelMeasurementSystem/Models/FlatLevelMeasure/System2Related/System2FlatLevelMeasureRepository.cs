namespace AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Models.FlatLevelMeasure.System2Related
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;
    using AKRS.ZX2200.LevelMeasurementSystem.Models.FlatLevelMeasure;

    using Newtonsoft.Json;

    /// <summary>
    /// 系统2平面测试点位库
    /// </summary>
    public class System2FlatLevelMeasureRepository : BaseSettingRepository<System2FlatLevelMeasureSetting>
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static System2FlatLevelMeasureRepository instance;

        /// <summary>
        /// 存储文件名称
        /// </summary>
        [JsonIgnore]
        public static string FileName => ZX2200PathConfig.System2FlatLevelMeasureFilePath;

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
        private System2FlatLevelMeasureRepository()
        {
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static System2FlatLevelMeasureRepository GetInstance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new System2FlatLevelMeasureRepository();
                }

                return instance;
            }
        }
    }
}
