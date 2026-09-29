using System;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.Infrastructure.AOP.Module;
using AKRS.ZX2200.Infrastructure.Models.Enums;
using AKRS.ZX2200.SupportFeature.Parameters;
using AKRS.ZX2200.SupportFeature.Parameters.Model;
using Newtonsoft.Json;

namespace AKRS.ZX2200.BondSystem.Models.DeviceParams
{
    using PropertyChanged;

    /// <summary>
    /// 焊头参数
    /// </summary>
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class BondHeadParam : PropertyChangeAop
    {
    /// <summary>
    /// Bond头上当前的吸嘴
    /// </summary>
    [TreeProgramListArgs("焊头当前吸嘴", "Bond模组")]
    public string CurrentNozzleName { get; set; } = string.Empty;

    /// <summary>
    /// 抛料位置,G0
    /// </summary>
    [TreeProgramListArgs("抛料位置（G0）", "Bond模组", UnitHelper.mm)]
    public AKRSPoint3D ThrowPos { get; set; } = new AKRSPoint3D();

    /// <summary>
    /// 模组安全位，轴坐标
    /// </summary>
    [TreeProgramListArgs("模组安全位（高于工作台/静态华夫盒，轴坐标）", "Bond模组", UnitHelper.mm, RoleEnum.Admin)]
    public AKRSPoint3D AxisSafePos { get; set; } = new AKRSPoint3D();

    /// <summary>
    /// 模组安全位，轴坐标
    /// </summary>
    [TreeProgramListArgs("T轴准备位", "Bond模组", UnitHelper.mm, RoleEnum.Admin)]
    public double TAxisPreparePos { get; set; } = 0;

        /// <summary>
        /// 换吸嘴安全位，G0
        /// </summary>
     [TreeProgramListArgs("换吸嘴安全位（高于吸嘴架，轴坐标）", "Bond模组", UnitHelper.mm, RoleEnum.Admin)]
    public AKRSPoint3D ChangeNozzleSafePos { get; set; } = new AKRSPoint3D();

    /// <summary>
    /// 上视安全高度,暂时没用
    /// </summary>
    public double UpLookSafeLevel { get; set; }

    /// <summary>
    /// 焊头和Bond相机的偏移，Z方向是相机看到的点到Touchdown的距离
    /// </summary>
    [JsonIgnore]
    [TreeProgramListArgs("焊头和Bond相机的偏移", "Bond模组", false, UnitHelper.mm)]
    public AKRSPoint3D HeadToCameraOffset => CalibrateRunPara.GetInstance().BondRotateCenterToCamOffset;

    /// <summary>
    /// 测高模拟量阈值
    /// 不接触时的模拟量加1000
    /// </summary>
    [TreeProgramListArgs("测高模拟量阈值", "Bond模组")]
    public int LvdtLimit { get; set; } = 17000;

    /// <summary>
    /// 抛料吹气比例
    /// </summary>
    [TreeProgramListArgs("抛料吹气比例", "Bond模组", 0, 100000000)]
    public int ThrowBlowProportion { get; set; } = 150;

    /// <summary>
    /// 抛料吹气比例
    /// </summary>
    [TreeProgramListArgs("抛料吹气延时", "Bond模组", 0, 100000000)]
    public int ThrowBlowDelay { get; set; } = 500;

        /// <summary>
        /// 换吸嘴吹气比例
        /// </summary>
        [TreeProgramListArgs("换吸嘴吹气比例", "Bond模组", 0, 100000000)]
    public int ChangeToolBlowProportion { get; set; } = 150;

    /// <summary>
    /// 换吸嘴吹气时间
    /// </summary>
    [TreeProgramListArgs("换吸嘴吹气时间", "Bond模组", 0, 100000000, UnitHelper.ms)]
    public int ChangeToolBlowDelay { get; set; } = 200;

    /// <summary>
    /// 温漂拍照位,轴坐标
    /// </summary>
    [TreeProgramListArgs("温漂拍照位1", "Bond模组", false, UnitHelper.mm)]
    [JsonIgnore]
    public AKRSPoint3D UpLookMarkVisionPos1 => CalibrateRunPara.GetInstance().UpLookMarkMachinePos;

    /// <summary>
    /// 温漂拍照位,轴坐标
    /// </summary>
    [TreeProgramListArgs("温漂拍照位2", "Bond模组", false, UnitHelper.mm)]
    [JsonIgnore]
    public AKRSPoint3D UpLookMarkVisionPos2 => CalibrateRunPara.GetInstance().UpLookMarkMachinePos2;

    /// <summary>
    /// 取片参考位1
    /// </summary>
    [TreeProgramListArgs("取片参考位1", "Bond模组", false, UnitHelper.mm)]
    [JsonIgnore]
    public AKRSPoint3D PickMarkVisionPos1 { get; set; } = new AKRSPoint3D();

    /// <summary>
    /// 取片参考位2
    /// </summary>
    [TreeProgramListArgs("取片参考位2", "Bond模组", false, UnitHelper.mm)]
    [JsonIgnore]
    public AKRSPoint3D PickMarkVisionPos2 { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 换静态华夫盒避让位，G0坐标
        /// </summary>
     [TreeProgramListArgs("换静态华夫盒避让位（G0）", "Bond模组", UnitHelper.mm)]
    public AKRSPoint3D ChangeStaticWaffleAvoidancePos { get; set; } = new AKRSPoint3D(211, -229, 121);

    /// <summary>
    /// 力控退出模式
    /// </summary>
    [TreeProgramListArgs("力控退出模式", "Bond模组", null, RoleEnum.Admin)]
    public int ForceResetMode { get; set; } = 0;

        /// <summary>
        /// 力控最小值
        /// </summary>
    [TreeProgramListArgs("力控最小值", "Bond模组", false, null, RoleEnum.Admin)]
    public double ForceControlMinVal { get; set; } = 0;

        /// <summary>
        /// 力控最大值
        /// </summary>
    [TreeProgramListArgs("力控最大值", "Bond模组", false, null, RoleEnum.Admin)]
    public double ForceControlMaxVal { get; set; } = 0;

    /// <summary>
    ///  吸嘴清洁位置(G0)
    /// </summary>
    [TreeProgramListArgs("吸嘴清洁左上位置(G0)", "Bond模组", false, null, RoleEnum.Admin)]
    public AKRSPoint3D NozzleCleanTableLeftTopPos { get; set; } = new AKRSPoint3D();

    /// <summary>
    ///  吸嘴清洁位置(G0)
    /// </summary>
    [TreeProgramListArgs("吸嘴清洁右下位置(G0)", "Bond模组", false, null, RoleEnum.Admin)]
    public AKRSPoint3D NozzleCleanTableRightBottomPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        ///  吸嘴清洁位置(G0)
        /// </summary>
        [TreeProgramListArgs("取片参考点Mark1位置(G0)", "Bond模组", false, null, RoleEnum.Admin)]
        public AKRSPoint3D PickUpMarkPos1 { get; set; }

        /// <summary>
        ///  吸嘴清洁位置(G0)
        /// </summary>
        [TreeProgramListArgs("取片参考点Mark2位置(G0)", "Bond模组", false, null, RoleEnum.Admin)]
        public AKRSPoint3D PickUpMarkPos2 { get; set; }

        /// <summary>
        /// 测高开始距离
        /// </summary>
        [TreeProgramListArgs("测高开始距离", "焊头测高")]
        public double MeasureHeightStartDistance { get; set; } = 5;

        /// <summary>
        /// 测高速度
        /// </summary>
        [TreeProgramListArgs("测高速度", "焊头测高")]
        public double MeasureHeightSpeed { get; set; } = 5;

        /// <summary>
        /// 测高结束距离
        /// </summary>
        [TreeProgramListArgs("测高结束距离", "焊头测高")]
        public double MeasureHeightEndDistance { get; set; } = 5;

        /// <summary>
        /// 参数是否为空
        /// </summary>
        /// <returns>结果</returns>
        public bool IsEmpty()
    {
        bool ret1 = this.ThrowPos.IsEmpty;
        bool ret2 = this.AxisSafePos.IsEmpty;
        bool ret3 = this.ChangeNozzleSafePos.IsEmpty;
        bool ret4 = this.HeadToCameraOffset.IsEmpty;
        bool ret5 = this.LvdtLimit == 0;
        bool ret6 = this.ThrowBlowProportion == 0;

        return ret1 || ret2 || ret3 || ret4 || ret5 || ret6;
    }
    }
}
