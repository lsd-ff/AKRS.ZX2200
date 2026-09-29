namespace AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Models.BondLevelMeasure
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    /// <summary>
    /// 焊头水平检测参数
    /// </summary>
    public class BondLevelMeasureSetting : Singleton<BondLevelMeasureSetting>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static BondLevelMeasureSetting()
        {
            Singleton<BondLevelMeasureSetting>.FilePath = ZX2200PathConfig.BondLevelMeasureFilePath;
        }

        /// <summary>
        /// BMC凸针位置对准蘑菇头的中心位置，关于G0
        /// </summary>
        public AKRSPoint2D BmcPinCenter2DG0Pos { get; set; }

        /// <summary>
        /// 将治具边缘移动到BMC凸针上方5mm处，记录此时的要下压的焊头位置，关于G0
        /// </summary>
        //public AKRSPoint2D BmcPinEdge2DG0Pos { get; set; }

        /// <summary>
        /// 测高搜索高度
        /// </summary>
        public double ZG0Pos { get; set; }
    }
}
