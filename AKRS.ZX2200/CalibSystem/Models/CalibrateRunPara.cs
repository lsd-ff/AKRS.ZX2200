using AKRS.Galaxy2.Infrastructure.CommonModel;

using HalconDotNet;

namespace AKRS.ZX2200.CalibSystem.Models
{
    using AKRS.ZX2200.Infrastructure.Models.Path;

    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// 运行参数
    /// </summary>
    public class CalibrateRunPara : Singleton<CalibrateRunPara>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static CalibrateRunPara()
        {
            Singleton<CalibrateRunPara>.FilePath = ZX2200PathConfig.CalibratePara;
        }

        #region System1
        /// <summary>
        /// 点胶Mark标定点中心位置（拍照位）
        /// </summary>
        public AKRSPoint3D DispenseMarkVisionMachinePos { get; set; }

        /// <summary>
        /// 点胶标定间距（九点标定的间距： mm）
        /// </summary>
        [JsonIgnore]
        public double DispenseCalibDistance { get; set; } = 0.5;

        /// <summary>
        /// 点胶探针测高搜索高度
        /// </summary>
        public AKRSPoint3D DispenseMeasureHeightSearchMachinePos { get; set; }

        /// <summary>
        /// 点胶探针测高的点（x，y，测高得出的高度） 组成
        /// </summary>
        public AKRSPoint3D DispenseMeasureRealHeightMachinePos { get; set; }

        /// <summary>
        /// 点胶相机测高针偏移 用点位表示 X Y 的偏移量， Z 预留  暂时不用 为0
        /// </summary>
        public AKRSPoint3D DispenseCameraToPinOffset { get; set; }

        #endregion

        #region System2

        /// <summary>
        /// Bond 相机拍BMC的位置
        /// </summary>
        public AKRSPoint3D BMCVisionMachinePos { get; set; } = new AKRSPoint3D(0, 0, 0);

        /// <summary>
        /// Mini-BMC 左上Mark拍照位
        /// </summary>
        public AKRSPoint3D BMCMarkTopLeftVisionMachinePos =>
            new AKRSPoint3D(this.BMCVisionMachinePos.X - 4, this.BMCVisionMachinePos.Y + 4, this.BMCVisionMachinePos.Z);

        /// <summary>
        /// Mini-BMC 右下Mark拍照位
        /// </summary>
        public AKRSPoint3D BMCMarkBotRightVisionMachinePos => 
            new AKRSPoint3D(this.BMCVisionMachinePos.X + 4, this.BMCVisionMachinePos.Y - 4, this.BMCVisionMachinePos.Z);

        /// <summary>
        /// BMC测高位置
        /// </summary>
        public AKRSPoint3D BMCMeasureHeightSearchMachinePos { get; set; }

        /// <summary>
        /// 邦头旋转中心到相机中心偏移 
        /// </summary>
        public AKRSPoint3D BondRotateCenterToCamOffset { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// TouchDown与旋转中心偏移量
        /// </summary>
        public AKRSPoint2D BondCenterToTouchDownOffset { get; set; } = new AKRSPoint2D(0, 0);

        /// <summary>
        /// BMC N点间距
        /// </summary>
        [JsonIgnore]
        public double BMCCalibDistance { get; set; } = 0.05;

        /// <summary>
        /// Bond相机 透明标定片中心位置 -- 拍玻璃标定片的位置
        /// </summary>
        public AKRSPoint3D GlassVisionMachinePos { get; set; }

        /// <summary>
        /// 透明标定片当前位置(变化)
        /// </summary>
        public AKRSPoint3D GlassCenterCurVisionMachinePos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 透明标定片左上Mark点当前拍照位
        /// </summary>
        [JsonIgnore]
        public AKRSPoint3D GlassMarkTopLeftCurVisionPos => new AKRSPoint3D(this.GlassCenterCurVisionMachinePos.X - 4, this.GlassCenterCurVisionMachinePos.Y + 4, this.GlassCenterCurVisionMachinePos.Z);

        /// <summary>
        /// 透明标定片右下Mark点当前拍照位
        /// </summary>
        [JsonIgnore]
        public AKRSPoint3D GlassMarkBotRightCurVisionPos => new AKRSPoint3D(this.GlassCenterCurVisionMachinePos.X + 4, this.GlassCenterCurVisionMachinePos.Y - 4, this.GlassCenterCurVisionMachinePos.Z);

        /// <summary>
        /// 取片高度
        /// </summary>
        public double GlassPickZMachinePos { get; set; }

        /// <summary>
        /// 透明标定片放置位置
        /// </summary>
        [JsonIgnore]
        public AKRSPoint3D PlaceToBMCMachinePos { get; set; }

        /// <summary>
        /// 透明标定片在上视相机的拍照位置
        /// </summary>
        public AKRSPoint3D GlassUpLookVisionMachinePos { get; set; }

        /// <summary>
        /// 上视相机旁Mark点机械拍照位
        /// </summary>
        public AKRSPoint3D UpLookMarkMachinePos { get; set; }

        /// <summary>
        /// 上视相机旁Mark点机械拍照位2
        /// </summary>
        public AKRSPoint3D UpLookMarkMachinePos2 { get; set; }

        /// <summary>
        /// Bond相机与上视相机三点一线位置
        /// </summary>
        public AKRSPoint3D BondCamUpLookCamMachinePos { get; set; }

        /// <summary>
        /// 上视 N点间距 : mm
        /// </summary>
        [JsonIgnore]
        public double UpLookCalibDistance { get; set; } = 0.4;

        /// <summary>
        /// 力传感器标定位置
        /// </summary>
        public AKRSPoint3D BondforceCalibMachinePos { get; set; }

        /// <summary>
        /// 晶圆台起始位置
        /// </summary>
        public AKRSPoint3D WaferTableReadyMachinePos { get; set; }

        /// <summary>
        /// 晶圆台左标记点Bond相机拍照位
        /// </summary>
        public AKRSPoint3D WaferTableLeftMarkBCVisionMachinePos { get; set; }

        /// <summary>
        /// 晶圆台右标记点Bond相机拍照位
        /// </summary>
        public AKRSPoint3D WaferTableRightMarkVisionMachinePos { get; set; }

        /// <summary>
        /// 晶圆台左右两标定点间距
        /// </summary>
        public AKRSPoint2D WaferTableMarksDistance { get; set; } = new AKRSPoint2D();

        /// <summary>
        /// 晶圆台左标定点晶圆相机中心位置
        /// </summary>
        public AKRSPoint3D WaferTableLeftMarkWCVisionMachinePos { get; set; }

        /// <summary>
        /// 晶圆台 N点间距
        /// </summary>
        [JsonIgnore]
        public double WaferTableCalibDistance { get; set; } = 0.3;

        /// <summary>
        /// 晶圆台邦头测高位
        /// </summary>
        public AKRSPoint3D WaferTableMeasureHeightSearchMachinePos { get; set; }

        /// <summary>
        /// 晶圆台右标记点位Bond相机拍照位
        /// </summary>
        public AKRSPoint3D WaferTableRightMarkBCVisionMachinePos { get; set; }

        /// <summary> 
        /// 旋转中心在晶圆相机的位置
        /// </summary>
        public AKRSPoint2D BondHeadRotateCenterInWC { get; set; }

        /// <summary>
        /// 晶圆相机高度补偿
        /// </summary>
        public double waferCameraOffsetZ { get; set; } = 0;

        #endregion

        #region
        /// <summary>
        /// 点胶标定矩阵
        /// </summary>
        public HTuple DispenseHomMatTrans { get; set; }

        /// <summary>
        /// Bond标定矩阵
        /// </summary>
        public HTuple BondHomMatTrans { get; set; }

        /// <summary>
        /// 上视标定矩阵
        /// </summary>
        public HTuple UpLookHomMatTrans { get; set; }

        /// <summary>
        /// 晶圆标定矩阵
        /// </summary>
        public HTuple WaferHomMatTrans { get; set; }

        #endregion

        #region System12联合标定

        /// <summary>
        /// 点胶 第一点
        /// </summary>
        public AKRSPoint3D DispenseFirstPoint { get; set; }

        /// <summary>
        /// Bond 第一点
        /// </summary>
        public AKRSPoint3D BondFirstPoint { get; set; }

        #endregion

        #region 模板

        /// <summary>
        /// 点胶模板
        /// </summary>
        [JsonIgnore]
        public string DispensePRName { get; set; } = "点胶标定模板";

        /// <summary>
        /// BMC模板
        /// </summary>
        public string BmcPRName { get; set; } = "大标定片模板";

        /// <summary>
        /// 玻璃片模板
        /// </summary>
        [JsonIgnore]
        public string GlassPRName { get; set; } = "小标定片模板";

        /// <summary>
        /// 上视模板
        /// </summary>
        [JsonIgnore]
        public string UpLookPRName { get; set; } = "上视标定模板";

        /// <summary>
        /// 晶圆左模板
        /// </summary>
        [JsonIgnore]
        public string WaferLeftPRName { get; set; } = "晶圆左标定模板";

        /// <summary>
        /// 晶圆右模板
        /// </summary>
        [JsonIgnore]
        public string WaferRightPRName { get; set; } = "晶圆右标定模板";

        /// <summary>
        /// 晶圆相机模板
        /// </summary>
        public string WaferPRName { get; set; } = "晶圆相机标定模板";

        #endregion

        #region 光源亮度比值
        /// <summary>
        /// 红色环光(点胶/Bond)
        /// </summary>
        public double RedRingLightRatio { get; set; }

        /// <summary>
        /// 蓝色环光(点胶/Bond)
        /// </summary>
        public double BlueRingLightRatio { get; set; }

        /// <summary>
        /// 蓝色环光(点胶/Bond)
        /// </summary>
        public double GreenRingLightRatio { get; set; }

        /// <summary>
        /// 红色点光(点胶/Bond)
        /// </summary>
        public double RedSpotLightRatio { get; set; }

        /// <summary>
        /// 蓝色点光(点胶/Bond)
        /// </summary>
        public double BlueSpotLightRatio { get; set; }

        /// <summary>
        /// 绿色点光(点胶/Bond)
        /// </summary>
        public double GreenSpotLightRatio { get; set; }

        #endregion
    }
}
