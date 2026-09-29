using System;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities
{
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;

    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Drawing.Text;

    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    using DevExpress.CodeParser;
    using PropertyChanged;

    /// <summary>
    /// 芯片载具基类
    /// </summary>
    [Serializable]
    public class BaseCarrierConfig : BaseDsSetting
    {
        /// <summary>
        /// 是否需要开真空检测
        /// </summary>
        [TreeProgramListArgs("定位前是否需要开静态华夫盒/晶圆台真空", "晶圆芯片载具属性")]
        public bool IsMatchWithVacuum { get; set; } = true;

        /// <summary>
        /// 取片前是否开静态华夫盒真空
        /// </summary>
        [TreeProgramListArgs("取片前是否开静态华夫盒/晶圆台真空", "晶圆芯片载具属性", true)]
        public bool IsOpenWaferTableVacuumBeforePick { get; set; } = true;

        /// <summary>
        /// 取片前是否开静态华夫盒真空
        /// </summary>
        [TreeProgramListArgs("取片前开静态华夫盒/晶圆台真空延时", "晶圆芯片载具属性", true)]
        public int OpenWaferTableVacuumBeforePickDelay { get; set; } = 0;

        /// <summary>
        /// RaiseDistanceToRefreshMapAfterBondPicked
        /// </summary>
        [TreeProgramListArgs("Bond取料后刷新料片状态的抬高距离", "芯片载具公共属性", 0, 50, UnitHelper.mm)]
        public double RaiseDistanceToRefreshMapAfterBondPicked { get; set; } = 3;

        /// <summary>
        /// Bond取料旋转角度补偿
        /// </summary>
        [TreeProgramListArgs("Bond取料旋转角度补偿", "芯片载具公共属性", UnitHelper.degree)]
        public double BondSpinAngleOffset { get; set; } = 0;

        /// <summary>
        /// 芯片中心补偿(只关乎取料)
        /// </summary>
        [TreeProgramListArgs("顶针中心位置补偿", "芯片载具公共属性", UnitHelper.mm)]
        public AKRSPoint3D CenterOffset { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 芯片Mark点与芯片中心的偏差
        /// </summary>
        [TreeProgramListArgs("芯片Mark点与芯片中心的偏差", "芯片载具公共属性", UnitHelper.mm)]
        public AKRSPoint3D RelativeDistanceMarkWithCenter { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 芯片载具类型
        /// </summary>
        [TreeProgramListArgs("芯片载具类型", "芯片载具公共属性")]
        public CarrierTypeEnum CarrierType { get; set; }

        /// <summary>
        /// 晶圆相机Z轴位置
        /// </summary>
        [TreeProgramListArgs("晶圆相机拍照位置", "芯片载具公共属性", UnitHelper.mm)]
        public AKRSPoint3D WaferAxisZPosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// Bond相机拍照位置
        /// </summary>
        [TreeProgramListArgs("Bond相机拍照位置", "芯片载具公共属性", UnitHelper.mm)]
        public AKRSPoint3D BondCameraSearchPosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 芯片尺寸
        /// </summary>
        [TreeProgramListArgs("芯片尺寸", "芯片载具公共属性", UnitHelper.mm)]
        public AKRSPoint2D ComponentSize { get; set; } = new AKRSPoint2D();

        /// <summary>
        /// 芯片厚度
        /// </summary>
        [TreeProgramListArgs("芯片厚度", "芯片载具公共属性", false, UnitHelper.mm)]
        public double ComponentThickness { get; set; } = 1;

        /// <summary>
        /// 芯片载具厚度
        /// </summary>
        [TreeProgramListArgs("芯片载具厚度", "芯片载具公共属性", UnitHelper.mm)]
        public double CarrierThickness { get; set; } = 1;

        /// <summary>
        /// 行间距xy
        /// </summary>
        [TreeProgramListArgs("芯片载具行间距", "芯片载具公共属性", UnitHelper.mm)]
        public AKRSPoint3D CarrierRowSpacing { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 列间距xy
        /// </summary>
        [TreeProgramListArgs("芯片载具列间距", "芯片载具公共属性", UnitHelper.mm)]
        public AKRSPoint3D CarrierColSpacing { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 吸嘴名称
        /// </summary>
        [TreeProgramListArgs("使用吸嘴名称", "芯片载具公共属性")]
        public string NozzleName { get; set; }

        /// <summary>
        /// 芯片P1点相对中心距离
        /// </summary>
        [TreeProgramListArgs("芯片P1点相对中心距离", "芯片载具公共属性", UnitHelper.mm)]
        public AKRSPoint3D DistanceDieP1RelativeCenter { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 芯片P2点相对中心距离
        /// </summary>
        [TreeProgramListArgs("芯片P2点相对中心距离", "芯片载具公共属性", UnitHelper.mm)]
        public AKRSPoint3D DistanceDieP2RelativeCenter { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 芯片定位检测模板名称
        /// </summary>
        [JsonIgnore]
        //[TreeProgramListArgs("芯片定位模板名称", "芯片载具公共属性")]
        public string DieMatchName => this.GetDieMatchName();

        /// <summary>
        /// 芯片定位检测模板名称P2
        /// </summary>
        [JsonIgnore]
        //[TreeProgramListArgs("芯片定位模板名称P2", "芯片载具公共属性")]
        public string DieMatchNameP2 => this.GetDieMatchNameP2();

        /// <summary>
        /// 芯片墨点检测模板名称
        /// </summary>
        [JsonIgnore]
        //[TreeProgramListArgs("芯片墨点检测模板名称", "芯片载具公共属性")]
        public string DieBlobName => this.GetDieBlobName();

        /// <summary>
        /// 芯片外框检测模板名称
        /// </summary>
        [JsonIgnore]
        //[TreeProgramListArgs("芯片外框检测模板名称", "芯片载具公共属性")]
        public string DieFrameName => this.Name + "DieFrame";

        /// <summary>
        /// 是否外框检测
        /// </summary>
        //[TreeProgramListArgs("是否外框检测", "芯片载具公共属性")]
        public bool IsFrameSearch { get; set; }

        /// <summary>
        /// 是否定位检测
        /// </summary>
        [TreeProgramListArgs("是否定位检测", "芯片载具公共属性")]
        public bool IsPositionSearch { get; set; } = true;

        /// <summary>
        /// 是否搜索墨点
        /// </summary>
        [TreeProgramListArgs("是否搜索墨点", "芯片载具公共属性")]
        public bool IsInkDotSearch { get; set; }

        /// <summary>
        /// 是否是RotationSearch
        /// </summary>
        [TreeProgramListArgs("是否旋转搜索", "芯片载具公共属性")]
        public bool IsRotationSearch { get; set; }

        /// <summary>
        /// 墨点检测数量
        /// </summary>
        [TreeProgramListArgs("墨点检测数量", "芯片载具公共属性", UnitHelper.times)]
        public int NumberOfInkDot { get; set; } = 0;

        /// <summary>
        /// 空晶最大跳过次数
        /// </summary>
        [TreeProgramListArgs("空晶最大跳过次数", "芯片载具公共属性", UnitHelper.times)]
        public int BlankDieAutoSkipMaxTimes { get; set; } = 0;

        /// <summary>
        /// 搜晶方式
        /// </summary>
        public SearchMode SearchMode { get; set; }

        /// <summary>
        /// 是否有map
        /// </summary>
        public bool IsMapping { get; set; }

        /// <summary>
        /// 是否两点定位
        /// </summary> 
        [TreeProgramListArgs("是否两点定位", "芯片载具公共属性", false)]
        public bool IsTwoPointAdjust { get; set; }

        /// <summary>
        /// 是否是多次定位
        /// </summary>
        [TreeProgramListArgs("是否多次定位", "多次定位")]
        public bool IsTimesLocation { get; set; } = false;

        /// <summary>
        /// 最大定位次数
        /// </summary>
        [TreeProgramListArgs("最大定位次数", "多次定位", UnitHelper.times)]
        public int MaxLocationTimes { get; set; } = 10;

        /// <summary>
        /// 精度
        /// </summary>
        [TreeProgramListArgs("目标精度", "多次定位", UnitHelper.mm)]
        public AKRSPoint3D TarAccuracy { get; set; } = new AKRSPoint3D(0.01, 0.01, 0);

        /// <summary>
        /// 角度
        /// </summary>
        [TreeProgramListArgs("目标角度", "多次定位", UnitHelper.degree)]
        public double TarAngle { get; set; } = 360;

        /// <summary>
        /// PositionSearchMode
        /// </summary>
        [TreeProgramListArgs("芯片定位模式", "芯片载具公共属性")]
        public PositionSearchModeEnum PositionSearchMode { get; set; } = PositionSearchModeEnum.StandardSearch;

        /// <summary>
        /// 更换料片方式
        /// </summary>
        [TreeProgramListArgs("料片更换方式", "芯片载具公共属性")]
        public TabletChangeTypeEnum TabletChangeType { get; set; } = TabletChangeTypeEnum.ManualChange;

        /// <summary>
        /// AutoRunWithDieMode
        /// </summary>
        [TreeProgramListArgs("芯片自动定位模式", "芯片载具公共属性")]
        public AutoRunWithDieModeEnum AutoRunWithDieMode { get; set; } = AutoRunWithDieModeEnum.Manual;

        /// <summary>
        /// 是否开启四周定位
        /// </summary>
        [TreeProgramListArgs("是否开启四周定位", "芯片载具公共属性")]
        public bool IsAroundLocate { get; set; } = false;

        /// <summary>
        /// 四周定位的半径
        /// </summary>
        [TreeProgramListArgs("四周定位半径", "芯片载具公共属性", UnitHelper.mm)]
        public double AroundLocateDistance { get; set; } = 1.0;

        /// <summary>
        /// 是否检测墨点数量
        /// </summary>
        [TreeProgramListArgs("是否检测墨点数量", "芯片载具公共属性")]
        public bool IsCheckBlobNum { get; set; } = false;

        /// <summary>
        /// 检测墨点数量阈值上限
        /// </summary>
        [TreeProgramListArgs("检测墨点数量阈值上限", "芯片载具公共属性", 0, 1000)]
        public int CheckBlobNumUpperLimit { get; set; } = 0;

        /// <summary>
        /// 检测墨点数量阈值下限
        /// </summary>
        [TreeProgramListArgs("检测墨点数量阈值下限", "芯片载具公共属性", 0, 1000)]
        public int CheckBlobNumLowerLimit { get; set; } = 0;

        #region Bond

        /// <summary>
        /// 取料力控模式
        /// </summary>
        [TreeProgramListArgs("取料力控模式", "取晶")]
        public ForceModeEnum PickupForceMode { get; set; }

        /// <summary>
        /// 取料力度
        /// </summary>
        [TreeProgramListArgs("取料力度", "取晶", 9, 1000, UnitHelper.force)]
        public double PickupForce { get; set; } = 50;

        /// <summary>
        /// 取料距离补偿
        /// </summary>
        [TreeProgramListArgs("取料距离补偿", "取晶", -20, 20, UnitHelper.mm)]
        public double PickupDistance { get; set; }

        /// <summary>
        /// 取料停留延时
        /// </summary>
        [TreeProgramListArgs("取料停留延时", "取晶", 0, 9999, UnitHelper.ms)]
        public int PickupDelay { get; set; } = 20;

        /// <summary>
        /// 取片吸嘴吸真空时间,弃用
        /// </summary>
        //public int PickupVacuumDelay { get; set; } = 100;

        /// <summary>
        /// 力控超时时间
        /// </summary>
        [TreeProgramListArgs("力控超时时间", "芯片载具公共属性", 0, 10000, UnitHelper.ms)]
        public int ForceControlOutTime { get; set; } = 50;

        /// <summary>
        /// 是否激活取料前慢速下降
        /// </summary>
        [TreeProgramListArgs("是否开启取料前慢速下降", "取晶")]
        public bool IsActivateSlowTravelBeforePickup { get; set; } = true;

        /// <summary>
        /// 是否激活取料前慢速上升
        /// </summary>
        [TreeProgramListArgs("是否开启取料后慢速上升", "取晶")]
        public bool IsActivateSlowTravelAfterPickup { get; set; } = true;

        /// <summary>
        /// 取料前慢速下降速度
        /// </summary>
        [TreeProgramListArgs("取料前慢速下降速度 ", "取晶", 0.1, 500.0, UnitHelper.speed)]
        public double SlowTravelSpeedBeforePickup { get; set; } = 50;

        /// <summary>
        /// 取料前慢速下降距离
        /// </summary>
        [TreeProgramListArgs("取料前慢速下降距离", "取晶", 0.1, 20.0, UnitHelper.mm)]
        public double SlowTravelDistanceBeforePickup { get; set; } = 2;

        /// <summary>
        /// 取料后慢速上升速度
        /// </summary>
        [TreeProgramListArgs("取料后慢速上升速度", "取晶", 0.1, 500.0, UnitHelper.speed)]
        public double SlowTravelSpeedAfterPickup { get; set; } = 50;

        /// <summary>
        /// 取料后慢速上升距离
        /// </summary>
        [TreeProgramListArgs("取料后慢速上升距离", "取晶", 0.1, 20.0, UnitHelper.mm)]
        public double SlowTravelDistanceAfterPickup { get; set; } = 2;

        /// <summary>
        /// 取片角度
        /// </summary>
        [TreeProgramListArgs("取片角度", "取晶", UnitHelper.degree)]
        public double RotaryPosition { get; set; }

        /// <summary>
        /// 取片硬补偿
        /// </summary>
        [TreeProgramListArgs("取片硬补偿", "取晶", UnitHelper.mm)]
        public AKRSPoint2D PickupOffset { get; set; } = new AKRSPoint2D();

        /// <summary>
        /// 取片角度补偿
        /// </summary>
        [TreeProgramListArgs("取片角度补偿", "取晶", "°")]
        public double PickAngleOffset { get; set; }

        ///// <summary>
        ///// 吸嘴吸真空延时(跟工艺沟通后取消)
        ///// </summary>
        //[TreeProgramListArgs("取片吸真空延时", "取晶", UnitHelper.ms)]
        //public int NozzleVacuumBuildUpTime { get; set; } = 100;

        /// <summary>
        /// 提前退出力控
        /// </summary>
        [TreeProgramListArgs("取片提前退出力控（顶针顶起前）", (string)null)]
        public bool IsResetForceControlAdvanceDuringPickup { get; set; } = false;

        /// <summary>
        /// 固晶力控模式
        /// </summary>
        [TreeProgramListArgs("固晶力控模式", "固晶")]
        public ForceModeEnum BondingForceMode { get; set; } = ForceModeEnum.Distance;

        /// <summary>
        /// 固晶力度
        /// </summary>
        [TreeProgramListArgs("固晶力度", "固晶", 9, 1000, UnitHelper.force)]
        public double BondingForce { get; set; } = 50;

        /// <summary>
        /// 固晶距离补偿
        /// </summary>
        [TreeProgramListArgs("固晶距离补偿", "固晶", -20, 20.0, UnitHelper.mm)]
        public double BondingDistance { get; set; }

        /// <summary>
        /// 固晶停留延时
        /// </summary>
        [TreeProgramListArgs("固晶停留延时", "固晶", 0, 9999, UnitHelper.ms)]
        public int PlacementDelay { get; set; } = 20;

        /// <summary>
        /// 吸嘴关真空延时
        /// </summary>
        [TreeProgramListArgs("吸嘴关真空延时", "固晶", 0, 9999, UnitHelper.ms)]
        public int VacuumOffDelay { get; set; } = 20;

        /// <summary>
        /// 弱吹延时
        /// </summary>
        [TreeProgramListArgs("弱吹延时", "固晶", 0, 350, UnitHelper.ms)]
        public int BondingBlowDelay { get; set; } = 20;

        /// <summary>
        /// 是否激活固晶前慢速下降
        /// </summary>
        [TreeProgramListArgs("是否开启固晶前慢速下降", "固晶")]
        public bool IsActivateSlowTravelBeforeBonding { get; set; } = true;

        /// <summary>
        /// 是否激活固晶后慢速上升
        /// </summary>
        [TreeProgramListArgs("是否开启固晶后慢速上升", "固晶")]
        public bool IsActivateSlowTravelAfterBonding { get; set; } = true;

        /// <summary>
        /// 固晶前慢速下降速度
        /// </summary>
        [TreeProgramListArgs("固晶前慢速下降速度", "固晶", 0.1, 500.0, UnitHelper.speed)]
        public double SlowTravelSpeedBeforeBonding { get; set; } = 50;

        /// <summary>
        /// 固晶前慢速下降距离
        /// </summary>
        [TreeProgramListArgs("固晶前慢速下降距离", "固晶", 0.1, 20.0, UnitHelper.mm)]
        public double SlowTravelDistanceBeforeBonding { get; set; } = 2;

        /// <summary>
        /// 固晶后慢速上升速度
        /// </summary>
        [TreeProgramListArgs("固晶后慢速上升速度", "固晶", 0.1, 500.0, UnitHelper.speed)]
        public double SlowTravelSpeedAfterBonding { get; set; } = 50;

        /// <summary>
        /// 固晶后慢速上升距离
        /// </summary>
        [TreeProgramListArgs("固晶后慢速上升距离", "固晶", 0.1, 20.0, UnitHelper.mm)]
        public double SlowTravelDistanceAfterBonding { get; set; } = 2;

        /// <summary>
        /// 弱吹比例
        /// </summary>
        [TreeProgramListArgs("弱吹比例", "固晶", 0.1, 200000.0)]
        public int WeakBlowProportion { get; set; } = 5000;

        ///// <summary>
        ///// 贴片超力控自动跳过
        ///// </summary>
        //public bool ExceedForceLimitAutoSkip { get; set; } = false;

        /// <summary>
        /// 上视定位配置
        /// P1模板名称：芯片名+UpLookMatch1
        /// P2模板名称：芯片名+UpLookMatch2
        /// </summary>
        [TreeProgramListArgs("上视定位配置", "精度模式")]
        public AdjustConfig UpLookAdjustConfig { get; set; } = new AdjustConfig();

        /// <summary>
        /// 下视视定位配置
        /// P1模板名称：芯片名+DownLookMatch1
        /// P2模板名称：芯片名+DownLookMatch1
        /// </summary>
        [TreeProgramListArgs("下视视定位配置", "精度模式")]
        public AdjustConfig DownLookAdjustConfig { get; set; } = new AdjustConfig();

        /// <summary>
        /// 上视精度模式
        /// </summary>
        [TreeProgramListArgs("上视精度模式", "精度模式")]
        public AccuracyModeEnum AccuracyMode { get; set; }

        /// <summary>
        /// 纠偏相机
        /// </summary>
        [TreeProgramListArgs("纠偏相机", "精度模式")]
        public CameraTypeEnum AdjustCamera { get; set; } = CameraTypeEnum.UpLookCamera;

        /// <summary>
        /// 搜晶相机
        /// </summary>
        [TreeProgramListArgs("搜晶相机", "精度模式")]
        public SearchCameraEnum SearchCamera { get; set; } = SearchCameraEnum.WaferCamera;

        /// <summary>
        /// 是否上视两点定位
        /// </summary>
        [TreeProgramListArgs("是否上视两点定位", "精度模式")]
        public bool IsTwoPointSearch { get; set; } = false;

        /// <summary>
        /// 是否确定参考点
        /// </summary>
        [TreeProgramListArgs("是否确定参考点", "精度模式")]
        public bool IsConfirmReferencePoint { get; set; } = false;

        /// <summary>
        /// 是否识别二维码
        /// </summary>
        [TreeProgramListArgs("是否识别二维码", "精度模式")]
        public bool IsRecognizeQRCode { get; set; } = false;

        /// <summary>
        /// 纠偏视觉定位延时
        /// </summary>
        [TreeProgramListArgs("上视/下视纠偏视觉定位延时", "精度模式", UnitHelper.ms)]
        public int AdjustVisionDelay { get; set; } = 20;

        /// <summary>
        /// 中转台类型
        /// </summary>
        [TreeProgramListArgs("中转台类型", "中转台")]
        public IPTTypeEnum IPTType { get; set; } = IPTTypeEnum.LeftIPT;

        /// <summary>
        /// 中转台放片力控模式
        /// </summary>
        [TreeProgramListArgs("放片力控模式", "中转台")]
        public ForceModeEnum IPTPlacementForceMode { get; set; } = ForceModeEnum.Force;

        /// <summary>
        /// 中转台取片放片力控-g
        /// </summary>
        [TreeProgramListArgs("中转台放片力值", "中转台", 9, 1000, UnitHelper.force)]
        public double IPTPlaceForce { get; set; } = 50;

        /// <summary>
        /// 是否激活中转台放片前慢速下降
        /// </summary>
        [TreeProgramListArgs("是否开启中转台放片前慢速下降", "中转台")]
        public bool IsActivateSlowTravelBeforePlaceOnIPT { get; set; } = true;

        /// <summary>
        /// 中转台放片前低速段距离
        /// </summary>
        [TreeProgramListArgs("中转台放片前低速段距离", "中转台", 0.1, 20.0, UnitHelper.mm)]
        public double IPTSlowTravelDistanceBeforeBonding { get; set; } = 2;

        /// <summary>
        /// 中转台放片前低速段速度
        /// </summary>
        [TreeProgramListArgs("中转台放片前低速段速度", "中转台", 0.1, 500.0, UnitHelper.speed)]
        public double IPTSlowTravelSpeedBeforeBonding { get; set; } = 50;

        /// <summary>
        /// 是否激活中转台放片后慢速上升
        /// </summary>
        [TreeProgramListArgs("是否开启中转台放片后慢速上升", "中转台")]
        public bool IsActivateSlowTravelAfterPlaceOnIPT { get; set; } = true;

        /// <summary>
        /// 中转台放片后低速段距离
        /// </summary>
        [TreeProgramListArgs("中转台放片后低速段距离", "中转台", 0.1, 20.0, UnitHelper.mm)]
        public double IPTSlowTravelDistanceAfterBonding { get; set; } = 2;

        /// <summary>
        /// 中转台放片后低速段速度
        /// </summary>
        [TreeProgramListArgs("中转台放片后低速段速度", "中转台", 0.1, 500.0, UnitHelper.speed)]
        public double IPTSlowTravelSpeedAfterBonding { get; set; } = 50;

        /// <summary>
        /// 中转台放片延时
        /// </summary>
        [TreeProgramListArgs("放片延时", "中转台", 0, 9999, UnitHelper.ms)]
        public int IPTPlacementDelay { get; set; } = 40;

        /// <summary>
        /// 吸嘴关真空延时
        /// </summary>
        [TreeProgramListArgs("吸嘴关真空延时", "中转台", 0, 9999, UnitHelper.ms)]
        public int NozzleVacuumOffDelayDuringPlaceOnIPT { get; set; } = 20;

        /// <summary>
        /// 中转台放片吸嘴吹气时间
        /// </summary>
        [TreeProgramListArgs("吸嘴吹气时间", "中转台", 0, 9999, UnitHelper.ms)]
        public int NozzleBlowingDelayDuringPlaceOnIPT { get; set; } = 20;

        /// <summary>
        /// 弱吹比例
        /// </summary>
        [TreeProgramListArgs("放片时吸嘴吹气比例", "中转台", 0.1, 200000.0)]
        public int IPTWeakBlowProportion { get; set; } = 5000;

        /// <summary>
        /// 中转台取片力控模式
        /// </summary>
        [TreeProgramListArgs("取片力控模式", "中转台")]
        public ForceModeEnum IPTPickupForceMode { get; set; } = ForceModeEnum.Force;

        /// <summary>
        /// 中转台取片力控-g
        /// </summary>
        [TreeProgramListArgs("中转台取片力值", "中转台", 9, 1000, UnitHelper.force)]
        public double IPTPickupForce { get; set; } = 50;

        /// <summary>
        /// 是否激活中转台取片前慢速下降
        /// </summary>
        [TreeProgramListArgs("是否开启中转台取片前慢速下降", "中转台")]
        public bool IsActivateSlowTravelBeforePickOnIPT { get; set; } = true;

        /// <summary>
        /// 中转台取片前低速段距离
        /// </summary>
        [TreeProgramListArgs("中转台取片前低速段距离", "中转台", 0.1, 20.0, UnitHelper.mm)]
        public double IPTSlowTravelDistanceBeforePickup { get; set; } = 2;

        /// <summary>
        /// 中转台取片前低速段速度
        /// </summary>
        [TreeProgramListArgs("中转台取片前低速段速度", "中转台", 0.1, 500.0, UnitHelper.speed)]
        public double IPTSlowTravelSpeedBeforePickup { get; set; } = 50;

        /// <summary>
        /// 是否激活中转台取片后慢速上升
        /// </summary>
        [TreeProgramListArgs("是否开启中转台取片后慢速上升", "中转台")]
        public bool IsActivateSlowTravelAfterPickOnIPT { get; set; } = true;

        /// <summary>
        /// 中转台取片后低速段距离
        /// </summary>
        [TreeProgramListArgs("中转台取片后低速段距离", "中转台", 0.1, 20.0, UnitHelper.mm)]
        public double IPTSlowTravelDistanceAfterPickup { get; set; } = 2;

        /// <summary>
        /// 中转台取片后低速段速度
        /// </summary>
        [TreeProgramListArgs("中转台取片后低速段速度", "中转台", 0.1, 500.0, UnitHelper.speed)]
        public double IPTSlowTravelSpeedAfterPickup { get; set; } = 50;

        /// <summary>
        /// 中转台取片延时
        /// </summary>
        [TreeProgramListArgs("中转台取片延时", "中转台", 0, 9999, UnitHelper.ms)]
        public int IPTPickupDelay { get; set; } = 20;

        /// <summary>
        /// 中转台取片硬补偿
        /// </summary>
        //[TreeProgramListArgs("中转台取片硬补偿", "中转台", UnitHelper.mm)]
        //public AKRSPoint3D IPTPickupOffset { get; set; } = new AKRSPoint3D();

        ///// <summary>
        ///// 中转台取片平台关真空时间(跟工艺沟通后取消)
        ///// </summary>
        //[TreeProgramListArgs("平台关真空时间", "中转台", UnitHelper.ms)]
        //public int IPTVacuumOffDelay { get; set; } = 100;

        ///// <summary>
        ///// 中转台取片吸嘴吸真空真空时间(跟工艺沟通后取消)
        ///// </summary>
        //[TreeProgramListArgs("吸嘴吸真空时间", "中转台", UnitHelper.ms)]
        //public int IPTVacuumBuildUpDelay { get; set; } = 100;

        /// <summary>
        /// 中转台中心位置硬补偿
        /// </summary>
        [TreeProgramListArgs("中转台中心位置硬补偿", "中转台", UnitHelper.mm)]
        public AKRSPoint3D IPTCenterOffset { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 中转台放片吸嘴吹气时间
        /// </summary>
        [TreeProgramListArgs("中转台吹气时间", "中转台", 0, 9999, UnitHelper.ms)]
        public int IPTBlowingDelayDuringPlaceOnIPT { get; set; } = 20;

        /// <summary>
        /// 下视角度
        /// </summary>
        [TreeProgramListArgs("下视角度", "上/下视", UnitHelper.degree)]
        public double IPTDownLookAngle { get; set; }

        /// <summary>
        /// 下视角度
        /// </summary>
        [TreeProgramListArgs("下视示教时视觉角度", "上/下视", UnitHelper.degree)]
        public double IPTDownLookVisionAngle { get; set; }

        /// <summary>
        /// 下视芯片示教中心
        /// </summary>
        [TreeProgramListArgs("下视芯片示教中心", "上/下视")]
        public AKRSPoint3D IPTComponentCenter { get; set; }

        /// <summary>
        /// 下视芯片示教参考点
        /// </summary>
        [TreeProgramListArgs("下视芯片示教参考点", "上/下视")]
        public AKRSPoint3D IPTComponentReference { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 上视定位失败自动跳过次数
        /// </summary>
        [TreeProgramListArgs("上视定位失败自动跳过次数", "上/下视")]
        public int UpLookAutoSkipTimes { get; set; } = 0;

        /// <summary>
        /// 是否开启漏晶检测
        /// </summary>
        [TreeProgramListArgs("漏晶/回带检测", "芯片载具公共属性")]
        public bool IsActiveComponentDetection { get; set; } = true;

        /// <summary>
        /// 翻转工具拾取延时
        /// </summary>
        [TreeProgramListArgs("翻转工具拾取延时 ", "翻转台", 0.1, 1000.0, UnitHelper.ms)]
        public int FlipToolPickupDelay { get; set; } = 20;

        /// <summary>
        /// 翻转工具拾取延时
        /// </summary>
        [TreeProgramListArgs("翻转工具吸真空延时 ", "翻转台", 0.1, 1000.0, UnitHelper.ms)]
        public int FlipToolVacuumAtEndOfProcess { get; set; } = 20;

        /// <summary>
        /// 是否使用翻转台
        /// </summary>
        [TreeProgramListArgs("使用翻转台 ", "翻转台")]
        public bool IsUseFlipTable { get; set; } = false;

        /// <summary>
        /// 在翻转过程中检测芯片
        /// </summary>
        [TreeProgramListArgs("在翻转过程中检测芯片(需要开启取料后慢速上抬) ", "翻转台")]
        public bool IsCheckComponentDuringFlip { get; set; } = false;

        /// <summary>
        /// 是否激活翻转台取料前慢速下降
        /// </summary>
        [TreeProgramListArgs("是否开启翻转台取料前慢速下降", "翻转台")]
        public bool IsActivateSlowTravelBeforeFlip { get; set; } = false;

        /// <summary>
        /// 是否激活翻转台取料后慢速上抬
        /// </summary>
        [TreeProgramListArgs("是否开启翻转台取料后慢速上抬", "翻转台")]
        public bool IsActivateSlowTravelAfterFlip { get; set; } = false;

        /// <summary>
        /// 翻转台取料前慢速下降距离
        /// </summary>
        [TreeProgramListArgs("翻转工具取料前慢速下降二段速距离", "翻转台", 0.1, 20.0, UnitHelper.mm)]
        public double SlowTravelDistanceBeforeFlip { get; set; } = 2;

        /// <summary>
        /// 翻转台取料前慢速下降速度
        /// </summary>
        [TreeProgramListArgs("翻转工具取料前慢速下降二段速速度", "翻转台", 0.1, 500.0, UnitHelper.speed)]
        public double SlowTravelSpeedBeforeFlip { get; set; } = 50;

        /// <summary>
        /// 翻转台取料后慢速上抬距离
        /// </summary>
        [TreeProgramListArgs("翻转工具取料后慢速上抬二段速距离", "翻转台", 0.1, 20.0, UnitHelper.mm)]
        public double SlowTravelDistanceAfterFlip { get; set; } = 2;

        /// <summary>
        /// 翻转台取料后慢速上抬速度
        /// </summary>
        [TreeProgramListArgs("翻转工具取料后慢速上抬二段速速度", "翻转台", 0.1, 500.0, UnitHelper.speed)]
        public double SlowTravelSpeedAfterFlip { get; set; } = 50;

        /// <summary>
        /// 翻转台取片硬补偿
        /// </summary>
        [TreeProgramListArgs("翻转工具取料硬补偿", "翻转台", 0, 30.0, UnitHelper.mm)]
        public double FlipArmOverTravelDistanceWafer { get; set; } = 0;

        /// <summary>
        /// 蘸胶模式
        /// </summary>
        [TreeProgramListArgs("蘸胶模式", "蘸胶")]
        public DipModeEnum DipMode = DipModeEnum.Off;

        /// <summary>
        /// 是否激活蘸胶前慢速下降
        /// </summary>
        [TreeProgramListArgs("是否激活蘸胶前慢速下降", "蘸胶")]
        public bool IsActivateSlowTravelBeforeDip { get; set; } = true;

        /// <summary>
        /// 蘸胶前低速段距离
        /// </summary>
        [TreeProgramListArgs("蘸胶前低速段距离", "蘸胶", 0.1, 20.0, UnitHelper.mm)]
        public double SlowTravelDistanceBeforeDipFlux { get; set; } = 2;

        /// <summary>
        /// 蘸胶前低速段速度
        /// </summary>
        [TreeProgramListArgs("蘸胶前低速段速度", "蘸胶", 0.1, 500.0, UnitHelper.speed)]
        public double SlowTravelSpeedBeforeDipFlux { get; set; } = 50;

        /// <summary>
        /// 是否激活蘸胶后慢速上升
        /// </summary>
        [TreeProgramListArgs("是否激活蘸胶后慢速上升", "蘸胶")]
        public bool IsActivateSlowTravelAfterDip { get; set; } = true;

        /// <summary>
        /// 蘸胶后低速段距离
        /// </summary>
        [TreeProgramListArgs("蘸胶后低速段距离", "蘸胶", 0.1, 20.0, UnitHelper.mm)]
        public double SlowTravelDistanceAfterDipFlux { get; set; } = 2;

        /// <summary>
        /// 蘸胶后低速段速度
        /// </summary>
        [TreeProgramListArgs("蘸胶后低速段速度", "蘸胶", 0.1, 500.0, UnitHelper.speed)]
        public double SlowTravelSpeedAfterDipFlux { get; set; } = 50;

        /// <summary>
        /// 蘸胶力控模式
        /// </summary>
        [TreeProgramListArgs("蘸胶力控模式", "蘸胶")]
        public ForceModeEnum DipFluxForceMode { get; set; } = ForceModeEnum.Distance;

        /// <summary>
        /// 蘸胶力度
        /// </summary>
        [TreeProgramListArgs("蘸胶力度", "蘸胶", 9, 1000, UnitHelper.force)]
        public double DipFluxForce { get; set; } = 50;

        /// <summary>
        /// 蘸胶延时
        /// </summary>
        [TreeProgramListArgs("蘸胶延时 ", "蘸胶", 0.1, 100000.0, UnitHelper.ms)]
        public int DipDelay { get; set; } = 100;

        /// <summary>
        /// 蘸胶位置补偿
        /// </summary>
        [TreeProgramListArgs("蘸胶位置补偿 ", "蘸胶", -100, 100.0, UnitHelper.ms)]
        public double DipDistance { get; set; } = 0;

        /// <summary>
        /// 是否在基板上点胶印
        /// </summary>
        [TreeProgramListArgs("在基板上点胶印 ", "蘸胶")]
        public bool IsDipOnTU { get; set; } = false;

        /// <summary>
        /// 隔多少个焊点点一次胶印
        /// </summary>
        [TreeProgramListArgs("隔多少个焊点点一次胶印 ", "蘸胶", 0, 999)]
        public int DipOnTUAfterBondingNum { get; set; } = 10;

        /// <summary>
        /// 允许的焊后胶量最小值
        /// </summary>
        [TreeProgramListArgs("允许的胶量最小值", "蘸胶", true)]
        public double TolerantFluxMin { get; set; } = 0.8;

        /// <summary>
        /// 允许的焊后胶量最大值
        /// </summary>
        [TreeProgramListArgs("允许的胶量最大值", "蘸胶", true)]
        public double TolerantFluxMax { get; set; } = 1.2;

        /// <summary>
        /// X方向的阈值
        /// </summary>
        [TreeProgramListArgs("X方向的阈值", "蘸胶", true)]
        public double FluxLimitX { get; set; } = 0.1;

        /// <summary>
        /// Y方向的阈值
        /// </summary>
        [TreeProgramListArgs("Y方向的阈值", "蘸胶", true)]
        public double FluxLimitY { get; set; } = 0.1;

        /// <summary>
        /// 角度阈值
        /// </summary>
        [TreeProgramListArgs("角度阈值", "蘸胶", true)]
        public double FluxLimitAngle { get; set; } = 0.5;

        /// <summary>
        /// 蘸胶标准距离偏差
        /// </summary>
        [TreeProgramListArgs("标准距离X", "蘸胶", true)]
        public double FluxDistanceX { get; set; }

        /// <summary>
        /// 蘸胶标准距离偏差
        /// </summary>
        [TreeProgramListArgs("标准距离Y", "蘸胶", true)]
        public double FluxDistanceY { get; set; }

        /// <summary>
        /// 焊后标准距离偏差
        /// </summary>
        [TreeProgramListArgs("标准角度", "蘸胶", true)]
        public double FluxDistanceAngle { get; set; }

        /// <summary>
        /// 基板上蘸胶数量
        /// </summary>
        public double DipOnTUFluxNum { get; set; }

        /// <summary>
        /// 焊后检测点胶示教胶量
        /// </summary>
        public List<double> DipOnTUFluxArea { get; set; }

        /// <summary>
        /// 基板上蘸胶示教胶心
        /// </summary>
        public List<double> DipOnTUFluxCenterX { get; set; }

        /// <summary>
        /// 基板上蘸胶示教胶心
        /// </summary>
        public List<double> DipOnTUFluxCenterY { get; set; }

        /// <summary>
        /// 蘸胶印子模板
        /// 芯片名+DipFluxMatch
        /// </summary>
        public string DipPRName { get; set; } = default;

        /// <summary>
        /// 蘸胶印子拍照位（偏移量）
        /// </summary>
        public AKRSPoint3D DipVisionPos { get; set; } = new AKRSPoint3D();

        #region 统计页面参数

        /// <summary>
        /// 定位报警数量（墨点、空晶）
        /// </summary>
        public int CountOfPositionError { get; set; } = 0;

        /// <summary>
        /// 真空报警数量
        /// </summary>
        public int CountOfVacuumError { get; set; } = 0;

        /// <summary>
        /// 墨点数量
        /// </summary>
        public int CountOfInkDot { get; set; } = 0;

        /// <summary>
        /// 被辨识数量--包括正常和墨点
        /// </summary>
        public int CountOfTotal { get; set; } = 0;

        /// <summary>
        /// 被抛掉的芯片
        /// </summary>
        public int CountOfReject { get; set; } = 0;

        /// <summary>
        /// 顶针顶起的芯片个数
        /// </summary>
        public int CountOfUseable { get; set; } = 0;

        /// <summary>
        ///  是否使用力控
        /// </summary>
        public bool IsUseForceControl => this.JudgeIsUseForceControl();


        #endregion

        #region 方法

        /// <summary>
        /// AddCountOfPositionError
        /// </summary>
        public void AddCountOfPositionError()
        {
            this.CountOfPositionError++;
        }

        /// <summary>
        /// AddCountOfInkDot
        /// </summary>
        public void AddCountOfInkDot()
        {
            this.CountOfInkDot++;
        }

        /// <summary>
        /// AddCountOfVacuumError
        /// </summary>
        public void AddCountOfVacuumError()
        {
            this.CountOfVacuumError++;
        }

        /// <summary>
        /// AddCountOfTotal
        /// </summary>
        public void AddCountOfTotal()
        {
            this.CountOfTotal++;
        }

        /// <summary>
        /// AddCountOfReject
        /// </summary>
        public void AddCountOfReject()
        {
            this.CountOfReject++;
        }

        /// <summary>
        /// ClearCount
        /// </summary>
        public void ClearCount()
        {
            this.CountOfPositionError = 0;
            this.CountOfInkDot = 0;
            this.CountOfTotal = 0;
            this.CountOfReject = 0;
        }

        /// <summary>
        /// AddCountOfUseable
        /// </summary>
        public void AddCountOfUseable()
        {
            this.CountOfUseable++;
        }

        /// <summary>
        /// GetDieMatchName
        /// </summary>
        /// <returns>result</returns>
        private string GetDieMatchName()
        {
            return this.Name + "DieMatch";
        }

        /// <summary>
        /// GetDieMatchNameP2
        /// </summary>
        /// <returns>result</returns>
        private string GetDieMatchNameP2()
        {
            return this.Name + "DieMatchP2";
        }

        /// <summary>
        /// GetDieBlobName
        /// </summary>
        /// <returns>result</returns>
        private string GetDieBlobName()
        {
            return this.Name + "DieBlob";
        }

        /// <summary>
        ///  判断是否使用力控
        /// </summary>
        /// <returns>结果</returns>
        private bool JudgeIsUseForceControl()
        {
            if (this.PickupForceMode == ForceModeEnum.Force || this.BondingForceMode == ForceModeEnum.Force)
            {
                return true;
            }

            if (this.AdjustCamera == CameraTypeEnum.BondCamera)
            {
                if (this.IPTPickupForceMode != ForceModeEnum.Force || this.IPTPlacementForceMode == ForceModeEnum.Force)
                {
                    return true;
                }
            }

            if (this.DipMode != DipModeEnum.Off)
            {
                if (this.DipFluxForceMode == ForceModeEnum.Force)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #endregion

        #region start示教

        /// <summary>
        /// BaseCarrierConfig
        /// </summary>
        public BaseCarrierConfig()
        {
            if (this.CarrierType != CarrierTypeEnum.Wafer || this.CarrierType != CarrierTypeEnum.Waffle || this.CarrierType != CarrierTypeEnum.StaticWaffle)
            {
                this.ComponentGeometry = new AssistantState() { Name = "Component geometry", State = AssistantStateEnum.ForBidden };
                this.ComponentWaferMap = new AssistantState() { Name = "Component wafer map", State = AssistantStateEnum.ForBidden };
                this.InkDot = new AssistantState() { Name = "Ink dot", State = AssistantStateEnum.ForBidden };
                this.WaferEdge = new AssistantState() { Name = "Wafer edge", State = AssistantStateEnum.ForBidden };
                this.ReferenceDie = new AssistantState() { Name = "Reference die", State = AssistantStateEnum.ForBidden };
                this.ComponentTransportUnitGeometry = new AssistantState() { Name = "Component transportUnit geometry", State = AssistantStateEnum.ForBidden };
                this.BadComponentTable = new AssistantState() { Name = "bad-component table", State = AssistantStateEnum.ForBidden };
                this.ComponentTransportUnitAdjust = new AssistantState() { Name = "Component transportUnit adjust", State = AssistantStateEnum.ForBidden };
                this.ComponentPickup = new AssistantState() { Name = "Component pickup", State = AssistantStateEnum.ForBidden };
                this.ComponentAccuracyMode = new AssistantState() { Name = "Component accuracy mode", State = AssistantStateEnum.ForBidden };
                this.ComponentPlacement = new AssistantState() { Name = "Component placement", State = AssistantStateEnum.ForBidden };
                this.ComponentWaferMap = new AssistantState() { Name = "Flip to wafer", State = AssistantStateEnum.ForBidden };
            }
        }

        /// <summary>
        /// ComponentGeometry
        /// </summary>
        public AssistantState ComponentGeometry { get; set; }

        /// <summary>
        /// ComponentWaferMap
        /// </summary>
        public AssistantState ComponentWaferMap { get; set; }

        /// <summary>
        /// InkDot
        /// </summary>
        public AssistantState InkDot { get; set; }

        /// <summary>
        /// WaferEdge
        /// </summary>
        public AssistantState WaferEdge { get; set; }

        /// <summary>
        /// ReferenceDie
        /// </summary>
        public AssistantState ReferenceDie { get; set; }

        /// <summary>
        /// ComponentTransportUnitGeometry
        /// </summary>
        public AssistantState ComponentTransportUnitGeometry { get; set; }

        /// <summary>
        /// BadComponentTable
        /// </summary>
        public AssistantState BadComponentTable { get; set; }

        /// <summary>
        /// ComponentTransportUnitAdjust
        /// </summary>
        public AssistantState ComponentTransportUnitAdjust { get; set; }

        /// <summary>
        /// ComponentPickup
        /// </summary>
        public AssistantState ComponentPickup { get; set; }

        /// <summary>
        /// ComponentAccuracyMode
        /// </summary>
        public AssistantState ComponentAccuracyMode { get; set; }

        /// <summary>
        /// ComponentPlacement
        /// </summary>
        public AssistantState ComponentPlacement { get; set; }

        /// <summary>
        /// FlipToWafer
        /// </summary>
        public AssistantState FlipToWafer { get; set; }

        /// <summary>
        /// 获取芯片所有示教的状态
        /// </summary>
        /// <returns>return</returns>
        public List<AssistantState> GetAssistantStates()
        {
            List<AssistantState> list = new List<AssistantState>();
            list.Add(this.ComponentGeometry);
            list.Add(this.FlipToWafer);
            list.Add(this.ComponentWaferMap);
            list.Add(this.InkDot);
            list.Add(this.WaferEdge);
            list.Add(this.ReferenceDie);
            list.Add(this.ComponentTransportUnitGeometry);
            list.Add(this.BadComponentTable);
            list.Add(this.ComponentTransportUnitAdjust);
            list.Add(this.ComponentPickup);
            list.Add(this.ComponentAccuracyMode);
            list.Add(this.ComponentPlacement);
            list.Add(this.FlipToWafer);
            return list;
        }

        /// <summary>
        /// 芯片是否示教完成
        /// </summary>
        [JsonIgnore]
        public bool IsAssistantSucceed => this.GetAssistantResult();

        /// <summary>
        /// 芯片是否示教完成
        /// </summary>
        /// <returns>return</returns>
        private bool GetAssistantResult()
        {
            List<AssistantState> list = this.GetAssistantStates();
            foreach (var item in list)
            {
                if (item == null)
                {
                    return false;
                }

                if (item.State == AssistantStateEnum.UnAble)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 初始化芯片示教状态
        /// </summary>
        public void ResetAssistantStates()
        {
            if (this.CarrierType == CarrierTypeEnum.Wafer)
            {
                this.ComponentGeometry = new AssistantState() { Name = "Component geometry", State = AssistantStateEnum.UnAble };

                if (this.IsMapping)
                {
                    this.ComponentWaferMap = new AssistantState() { Name = "Component wafer map", State = AssistantStateEnum.ForBidden };
                }
                else
                {
                    this.ComponentWaferMap = new AssistantState() { Name = "Component wafer map", State = AssistantStateEnum.ForBidden };
                }
                
                if (this.IsInkDotSearch)
                {
                    this.InkDot = new AssistantState() { Name = "Ink dot", State = AssistantStateEnum.UnAble };
                }
                else
                {
                    this.InkDot = new AssistantState() { Name = "Ink dot", State = AssistantStateEnum.ForBidden };
                }

                this.WaferEdge = new AssistantState() { Name = "Wafer edge", State = AssistantStateEnum.UnAble };
                this.ReferenceDie = new AssistantState() { Name = "Reference die", State = AssistantStateEnum.ForBidden };
                this.ComponentTransportUnitGeometry = new AssistantState() { Name = "Component transportUnit geometry", State = AssistantStateEnum.ForBidden };
                this.BadComponentTable = new AssistantState() { Name = "bad-component table", State = AssistantStateEnum.ForBidden };
                this.ComponentTransportUnitAdjust = new AssistantState() { Name = "Component transportUnit adjust", State = AssistantStateEnum.ForBidden };
                this.ComponentPickup = new AssistantState() { Name = "Component pickup", State = AssistantStateEnum.UnAble };
                if (this.AccuracyMode == AccuracyModeEnum.Off)
                {
                    this.ComponentAccuracyMode = new AssistantState() { Name = "Component accuracy mode", State = AssistantStateEnum.ForBidden };
                }
                else
                {
                    this.ComponentAccuracyMode = new AssistantState() { Name = "Component accuracy mode", State = AssistantStateEnum.UnAble };
                }

                this.ComponentPlacement = new AssistantState() { Name = "Component placement", State = AssistantStateEnum.UnAble };
            }
            else if (this.CarrierType == CarrierTypeEnum.Waffle || this.CarrierType == CarrierTypeEnum.StaticWaffle)
            {
                this.ComponentGeometry = new AssistantState() { Name = "Component geometry", State = AssistantStateEnum.UnAble };
                this.ComponentWaferMap = new AssistantState() { Name = "Component wafer map", State = AssistantStateEnum.ForBidden };
                this.InkDot = new AssistantState() { Name = "Ink dot", State = AssistantStateEnum.UnAble };
                this.WaferEdge = new AssistantState() { Name = "Wafer edge", State = AssistantStateEnum.ForBidden };
                this.ReferenceDie = new AssistantState() { Name = "Reference die", State = AssistantStateEnum.ForBidden };
                this.ComponentTransportUnitGeometry = new AssistantState() { Name = "Component transportUnit geometry", State = AssistantStateEnum.UnAble };
                this.BadComponentTable = new AssistantState() { Name = "bad-component table", State = AssistantStateEnum.ForBidden };
                this.ComponentTransportUnitAdjust = new AssistantState() { Name = "Component transportUnit adjust", State = AssistantStateEnum.ForBidden };
                this.ComponentPickup = new AssistantState() { Name = "Component pickup", State = AssistantStateEnum.UnAble };
                if (this.AccuracyMode == AccuracyModeEnum.Off)
                {
                    this.ComponentAccuracyMode = new AssistantState() { Name = "Component accuracy mode", State = AssistantStateEnum.ForBidden };
                }
                else
                {
                    this.ComponentAccuracyMode = new AssistantState() { Name = "Component accuracy mode", State = AssistantStateEnum.UnAble };
                }

                this.ComponentPlacement = new AssistantState() { Name = "Component placement", State = AssistantStateEnum.UnAble };
            }
            else
            {
                this.ComponentGeometry = new AssistantState() { Name = "Component geometry", State = AssistantStateEnum.ForBidden };
                this.ComponentWaferMap = new AssistantState() { Name = "Component wafer map", State = AssistantStateEnum.ForBidden };
                this.InkDot = new AssistantState() { Name = "Ink dot", State = AssistantStateEnum.ForBidden };
                this.WaferEdge = new AssistantState() { Name = "Wafer edge", State = AssistantStateEnum.ForBidden };
                this.ReferenceDie = new AssistantState() { Name = "Reference die", State = AssistantStateEnum.ForBidden };
                this.ComponentTransportUnitGeometry = new AssistantState() { Name = "Component transportUnit geometry", State = AssistantStateEnum.ForBidden };
                this.BadComponentTable = new AssistantState() { Name = "bad-component table", State = AssistantStateEnum.ForBidden };
                this.ComponentTransportUnitAdjust = new AssistantState() { Name = "Component transportUnit adjust", State = AssistantStateEnum.ForBidden };
                this.ComponentPickup = new AssistantState() { Name = "Component pickup", State = AssistantStateEnum.ForBidden };
                this.ComponentAccuracyMode = new AssistantState() { Name = "Component accuracy mode", State = AssistantStateEnum.ForBidden };
                this.ComponentPlacement = new AssistantState() { Name = "Component placement", State = AssistantStateEnum.ForBidden };
            }

            this.FlipToWafer = new AssistantState() { Name = "Flip to wafer", State = AssistantStateEnum.ForBidden };
        }

        /// <summary>
        /// 获取模板名称集合
        /// </summary>
        /// <returns>名称集合</returns>
        public List<string> GetPRNameList()
        {
            List<string> list = new List<string>();
            list.Add(DieMatchName);
            list.Add(DieMatchNameP2);
            list.Add(DieBlobName);
            list.Add(DieFrameName);

            list.Add(UpLookAdjustConfig.P1PRName);
            list.Add(UpLookAdjustConfig.P2PRName);

            list.Add(DownLookAdjustConfig.P1PRName);
            list.Add(DownLookAdjustConfig.P2PRName);

            list.Add(DipPRName);

            return list;
        }

        #endregion
    }
}
