using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.WaferSubSystem.Models.DeviceParams
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.Infrastructure.AOP.Module;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using Newtonsoft.Json;

    using PropertyChanged;

    /// <summary>
    /// 顶针台模组设备参数
    /// </summary>
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class EjectDevicePara : PropertyChangeAop
    {
        #region Delay

        /// <summary>
        /// 顶针固定气缸动作后延时
        /// </summary>
        [TreeProgramListArgs("顶针固定气缸动作后延时", "延时", 0, 2000, UnitHelper.ms)]
        public int DelayFixedCycActionAfter { get; set; } = 10;

        /// <summary>
        /// 顶针开真空后延时
        /// </summary>
        [TreeProgramListArgs("顶针开真空后延时", "延时", 0, 2000, UnitHelper.ms)]
        public int DelayOpenEjectInhaleAfter { get; set; } = 10;

        ///// <summary>
        ///// 顶针台关真空前延时
        ///// </summary>
        //[TreeProgramListArgs("顶针台关真空前延时", "延时", 0, 2000, UnitHelper.ms)]
        //public int DelayCloseEjectInhaleBefore { get; set; } = 0;

        /// <summary>
        /// 顶针关真空后延时
        /// </summary>
        [TreeProgramListArgs("顶针关真空后延时", "延时", 0, 2000, UnitHelper.ms)]
        public int DelayCloseEjectInhaleAfter { get; set; } = 10;

        ///// <summary>
        ///// 顶针台开吹气后延时
        ///// </summary>
        //[TreeProgramListArgs("顶针台开吹气后延时", "延时", 0, 2000, UnitHelper.ms)]
        //public int DelayOpenEjectBlowAfter { get; set; } = 10;

        ///// <summary>
        ///// 顶针台关吹气前延时
        ///// </summary>
        //[TreeProgramListArgs("顶针台关吹气前延时", "延时", 0, 2000, UnitHelper.ms)]
        //public int DelayCloseEjectBlowBefore { get; set; } = 0;

        /// <summary>
        /// 顶针吹气保持时间
        /// </summary>
        [TreeProgramListArgs("顶针吹气保持时间", "延时", 0, 2000, UnitHelper.ms)]
        public int DelayEjectBlowHold { get; set; } = 10;

        /// <summary>
        /// 顶针关吹气后延时
        /// </summary>
        [TreeProgramListArgs("顶针关吹气后延时", "延时", 0, 2000, UnitHelper.ms)]
        public int DelayCloseEjectBlowAfter { get; set; } = 10;

        #endregion

        #region 多段顶参数

        /// <summary>
        /// 步数
        /// </summary>
        public int NeedleIntermediateSteps { get; set; } = 1;

        /// <summary>
        /// 步距
        /// </summary>
        public double NeedleIntermediatePosition { get; set; } = 1;

        /// <summary>
        /// 单步顶起后延时
        /// </summary>
        public int NeedleStepDelay { get; set; } = 1;

        #endregion

        #region 顶针台模组设备参数

        /// <summary>
        /// 当前槽位
        /// </summary>
        public EjectionBankSlotConfig CurrentSlotConfig { get; set; } = null;

        /// <summary>
        /// 顶针安全位置
        /// </summary>
        [TreeProgramListArgs("顶针安全位置", "顶针设备参数", UnitHelper.mm)]
        public AKRSPoint3D EjectSafePosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 槽位位置
        /// </summary>
        [TreeProgramListArgs("槽位位置", "顶针设备参数", UnitHelper.mm)]
        public AKRSPoint3D[] SlotPosition { get; set; } = new AKRSPoint3D[5];

        /// <summary>
        /// 安全位置
        /// </summary>
        [TreeProgramListArgs("顶针台安全位置", "顶针设备参数", UnitHelper.mm)]
        public AKRSPoint3D UpDownSafePosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 顶针座不剥离位置，弃用
        /// </summary>
        [TreeProgramListArgs("顶针座不剥离位置（G0）", "顶针设备参数", UnitHelper.mm)]
        public AKRSPoint3D EjectionInseparablePos { get; set; } = new AKRSPoint3D(0, 0, 55);

        /// <summary>
        /// 气缸动作位置
        /// </summary>
        [TreeProgramListArgs("固定气缸动作位置", "顶针设备参数", UnitHelper.mm)]
        public AKRSPoint3D CycActionPosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 顶针台mark位置
        /// </summary>
        [TreeProgramListArgs("顶针台mark位置", "顶针设备参数", UnitHelper.mm)]
        public AKRSPoint3D UpDownMarkPosition { get; set; } = new AKRSPoint3D();

        #endregion

        /// <summary>
        /// 是否使用顶针固定气缸
        /// </summary>
        [TreeProgramListArgs("是否使用顶针固定气缸", "")]
        public bool IsUseEjectFixedCylinder { get; set; } = false;

        /// <summary>
        /// 是否使用硬件编辑器顶针轴设置参数
        /// </summary>
        [TreeProgramListArgs("是否使用硬件编辑器顶针轴设置参数", "")]
        public bool IsUseEjectHardwarePara { get; set; } = false;

        /// <summary>
        /// 是否需要顶针回零
        /// </summary>
        [TreeProgramListArgs("是否需要顶针回零", "")]
        public bool IsNeedEjectZero { get; set; } = false;

        /// <summary>
        /// 是否需要顶针台回零
        /// </summary>
        [TreeProgramListArgs("是否需要顶针台回零", "")]
        public bool IsNeedEjectTableZero { get; set; } = false;

        /// <summary>
        /// 是否屏蔽顶针模组
        /// </summary>
        [TreeProgramListArgs("是否屏蔽顶针模组", "")]
        public bool IsShieldEjectModule { get; set; } = false;

        /// <summary>
        /// 是否使用晶圆相机示教顶针中心
        /// </summary>
        [TreeProgramListArgs("是否使用晶圆相机示教顶针中心", "")]
        public bool IsUseWaferCameraAssistantEjectCenter { get; set; } = true;
    }
}
