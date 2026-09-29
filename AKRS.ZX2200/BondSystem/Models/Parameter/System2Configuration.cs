using System;
using System.ComponentModel;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Infrastructure.AOP.Module;
using AKRS.ZX2200.Infrastructure.Models.Enums;
using AKRS.ZX2200.Infrastructure.Models.Path;
using AKRS.ZX2200.SupportFeature.Parameters;
using AKRS.ZX2200.SupportFeature.Parameters.Model;
using PropertyChanged;

namespace AKRS.ZX2200.BondSystem.Models.Parameter
{
    /// <summary>
    ///  系统2配置参数
    /// </summary>
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class System2Configuration : Singleton<System2Configuration>, INotifyPropertyChanged
    {
        /// <summary>
        /// 定位失败重试次数
        /// </summary>
        [TreeProgramListArgs("定位失败重试次数", (string)null, 0, 1000000)]
        public double VisionRetryTimesLimit { get; set; } = 2;

        /// <summary>
        /// 是否检测焊头上有无吸嘴
        /// </summary>
        [TreeProgramListArgs("是否检测焊头上有无吸嘴", (string)null)]
        public bool IsActiveToolDetection { get; set; } = true;

        /// <summary>
        /// 焊头吸附是否读真空模拟量
        /// </summary>
        [TreeProgramListArgs("焊头吸附是否读真空模拟量", (string)null, null, RoleEnum.Admin)]
        public bool IsBondheadVacuumUseAnalogue { get; set; } = false;

        /// <summary>
        /// 焊头吸附模拟量阈值
        /// </summary>
        [TreeProgramListArgs("焊头吸附模拟量阈值", (string)null, 0, 100000000)]
        public int BondheadVacuumAnalogueLimit { get; set; } = 10000;

        /// <summary>
        /// 上视是否二次定位
        /// </summary>
        public bool IsUpLookAjustTwice { get; set; } = false;

        /// <summary>
        /// 是否开启测高
        /// </summary>
        [TreeProgramListArgs("是否开启测高", (string)null)]
        public bool IsActiveHeightMeasurement { get; set; } = false;

        ///// <summary>
        ///// 是否开启图片保存
        ///// </summary>
        //[TreeProgramListArgs("图片保存", (string)null)]
        //public bool IsActiveSaveImg { get; set; } = true;

        /// <summary>
        /// 是否开启自检
        /// </summary>
        [TreeProgramListArgs("自检", (string)null)]
        public bool IsActiveSelfCheck { get; set; } = true;

        /// <summary>
        /// 是否开启上视验证
        /// </summary>
        [TreeProgramListArgs("上视验证", (string)null)]
        public bool IsActiveUpLookTest { get; set; } = true;

        /// <summary>
        /// 启用吸嘴架
        /// </summary>
        [TreeProgramListArgs("启用吸嘴架", (string)null)]
        public bool IsToolBankEnable { get; set; } = true;

        #region 温漂补偿

        /// <summary>
        ///  开启上视温度漂移补偿
        /// </summary>
        [TreeProgramListArgs("开启上视温度漂移补偿", "上视矫正温漂补偿")]
        public bool IsActiveDriftCompensate { get; set; } = false;

        /// <summary>
        ///  温度漂移Mark个数
        /// </summary>
        [TreeProgramListArgs("温度漂移Mark个数", "上视矫正温漂补偿", min: 1, max: 2, "个")]
        public int DriftCompensateMarkNumber { get; set; } = 1;

        /// <summary>
        /// 温度漂移补偿间隔时间(分钟)
        /// </summary>
        [TreeProgramListArgs("温度漂移补偿间隔时间", "上视矫正温漂补偿", "分钟")]
        public int DriftCompensateIntervalTime { get; set; } = 10;

        /// <summary>
        ///  开启取片温度漂移补偿
        /// </summary>
        [TreeProgramListArgs("开启取片温度漂移补偿", "取片矫正温漂补偿")]
        public bool IsActivePickUpCompensate { get; set; } = false;

        /// <summary>
        ///  温度漂移Mark个数
        /// </summary>
        [TreeProgramListArgs("温度漂移Mark个数", "取片矫正温漂补偿", min: 1, max: 2, "个")]
        public int PickUpCompensateMarkNumber { get; set; } = 1;

        /// <summary>
        /// 温度漂移补偿间隔时间(分钟)
        /// </summary>
        [TreeProgramListArgs("温度漂移补偿间隔时间", "取片矫正温漂补偿", "分钟")]
        public int PickUpCompensateIntervalTime { get; set; } = 10;

        /// <summary>
        ///  开启取片温度漂移补偿
        /// </summary>
        [TreeProgramListArgs("轨道标定尺子实时矫正", "轨道标定尺子实时矫正")]
        public bool IsRealTimeCompensate { get; set; } = false;

        /// <summary>
        /// 温度漂移补偿间隔时间(分钟)
        /// </summary>
        [TreeProgramListArgs("轨道标定尺子实时矫正间隔时间", "轨道标定尺子实时矫正", "分钟")]
        public int IsRealTimeCompensateTime { get; set; } = 10;

        #endregion

        /// <summary>
        /// 开启换吸嘴时吹气
        /// </summary>
        [TreeProgramListArgs("换吸嘴时吹气", (string)null)]
        public bool IsActiveBlowDuringChangeNozzle { get; set; } = false;

        /// <summary>
        /// 是否打印取片力值
        /// </summary>
        [TreeProgramListArgs("是否开启取片力值打印", (string)null)]
        public bool IsActiveSavePickupForce { get; set; } = false;

        /// <summary>
        /// 是否开启顶针顶起过程LVDT值打印
        /// </summary>
        [TreeProgramListArgs("是否开启顶针顶起过程LVDT值打印", (string)null)]
        public bool IsActiveSaveLVDTValue { get; set; } = false;

        /// <summary>
        /// 是否开启顶针顶起过程LVDT值打印
        /// </summary>
        [TreeProgramListArgs("是否顶针顶起过程LVDT报警", (string)null)]
        public bool IsActiveLVDTWarning { get; set; } = false;

        /// <summary>
        /// 是否开启顶针顶起过程LVDT值打印
        /// </summary>
        [TreeProgramListArgs("是否顶针顶起过程LVDT报警阀值", (string)null)]
        public double LVDTWarningLimit { get; set; } = 20000;

        /// <summary>
        /// 焊头力自动补偿
        /// 自研焊头，4号机专用
        /// </summary>
        //[TreeProgramListArgs("焊头力自动补偿", (string)null, null, RoleEnum.Admin)]
        public bool IsActiveBondForceAutoCompensate { get; set; } = false;

        /// <summary>
        /// 下视相机拍照前延迟
        /// </summary>
        [TreeProgramListArgs("下视相机拍照前延迟", (string)null)]
        public int DownLookVisionDelay { get; set; } = 500;

        /// <summary>
        /// 视觉处理后延迟
        /// </summary>
        public int VisionResultDelay { get; set; }

        /// <summary>
        /// 上视快速定位，一次定位
        /// </summary>
        [TreeProgramListArgs("上视快速定位", (string)null)]
        public bool UpLookQuickPositioning { get; set; } = true;

        /// <summary>
        /// 上视提前旋转
        /// </summary>
        [TreeProgramListArgs("上视提前旋转", (string)null)]
        public bool UpLookSpinInAdvance { get; set; } = true;

        /// <summary>
        /// 提前换吸嘴
        /// </summary>
        [TreeProgramListArgs("提前换吸嘴", (string)null)]
        public bool IsChangeNozzleAdvance { get; set; } = true;

        /// <summary>
        /// 换静态华夫盒时自动移到避让位
        /// </summary>
        [TreeProgramListArgs("换静态华夫盒时自动移到避让位", (string)null)]
        public bool IsAutoMoveToChangeStaticWaffleAvoidancePos { get; set; } = false;

        /// <summary>
        /// 保存顶针顶起收回数据
        /// </summary>
        [TreeProgramListArgs("保存顶针顶起收回数据", (string)null)]
        public bool IsSaveEjectionTimeData { get; set; } = false;

        /// <summary>
        /// 力控下压前焊头压力清零
        /// </summary>
        [TreeProgramListArgs("力控下压前焊头压力清零", (string)null, null, RoleEnum.Admin)]
        public bool IsZeroBondForceBeforeDown { get; set; } = true;

        /// <summary>
        /// 固晶前焊头压力表清零
        /// </summary>
        //[TreeProgramListArgs("固晶前焊头压力表清零", (string)null, null, RoleEnum.Admin)]
        public bool IsResetBondForceBeforeBonding { get; set; } = false;

        /// <summary>
        /// 换吸嘴失败重试次数
        /// </summary>
        [TreeProgramListArgs("换吸嘴失败重试次数", (string)null, 0, 100, "", RoleEnum.Admin)]
        public double ChangeNozzleRetryTimesLimit { get; set; } = 2;

        /// <summary>
        /// 换吸嘴失败重试功能
        /// </summary>
        [TreeProgramListArgs("换吸嘴失败重试功能", (string)null, null, RoleEnum.Admin)]
        public bool IsRetryAfterChangeNozzleFailed { get; set; } = false;

        /// <summary>
        /// 是否开启贴片上限报警
        /// </summary>
        [TreeProgramListArgs("是否开启贴片上限报警")]
        public bool IsActivateBondLimitWaring { get; set; } = false;

        /// <summary>
        /// 贴片颗数上限
        /// </summary>
        [TreeProgramListArgs("贴片颗数上限")]
        public int BondWarningLimit { get; set; } = 10000;

        ///// <summary>
        ///// 焊头力自动补偿
        ///// 6号机专用
        ///// </summary>
        //[TreeProgramListArgs("是否保存二维补偿数据", (string)null, null, RoleEnum.Admin)]
        //public bool IsActiveSaveGlobalCalibrationOffset { get; set; } = false;

        /// <summary>
        /// 打印芯片取贴片力值
        /// 6号机专用
        /// </summary>
        [TreeProgramListArgs("是否打印芯片取贴片力值", (string)null, null, RoleEnum.Admin)]
        public bool IsExportComponentForceData { get; set; } = false;


        /// <summary>
        /// 是否打印胶量检测数据
        /// </summary>
        [TreeProgramListArgs("是否打印胶量检测数据", (string)null, null, RoleEnum.Admin)]
        public bool IsExportPostBondEpoxyCheckData { get; set; } = false;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static System2Configuration()
        {
            FilePath = ZX2200PathConfig.BondRecipeParaPath;
        }

        /// <summary>
        /// 属性更改事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性更改记录
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="before">开始</param>
        /// <param name="after">结束</param>
        protected virtual void OnPropertyChanged(string propertyName, object before, object after)
        {
            PropertyChangeAop.OnPropertyChangedEvent(this, propertyName, before, after);
        }
    }

    /// <summary>
    /// 温漂拍照的时间间隔
    /// </summary>
    public enum DriftCompensateTypeEnum
    {
        /// <summary>
        /// 根据排序自动选择
        /// </summary>
        [Description("根据工作步骤自动选择")]
        AutoByPrecess,

        /// <summary>
        /// 根据时间
        /// </summary>
        [Description("根据设定时间")]
        TimeCycle,
    }
}
