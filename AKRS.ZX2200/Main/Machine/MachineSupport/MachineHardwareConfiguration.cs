using System.ComponentModel;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.Infrastructure.AOP.Module;
using AKRS.ZX2200.Infrastructure.Models.Enums;
using AKRS.ZX2200.Infrastructure.Models.Path;
using AKRS.ZX2200.SupportFeature.Parameters.Model;
using PropertyChanged;


namespace AKRS.ZX2200.Main.Machine.MachineSupport
{
    /// <summary>
    /// 设备硬件配置
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class MachineHardwareConfiguration : Singleton<MachineHardwareConfiguration>, INotifyPropertyChanged
    {
        /// <summary>
        ///  上下料配置
        /// </summary>
        [TreeProgramListArgs("上下料配置", (string)null, null, RoleEnum.Admin)]
        public LoadConfigurationEnum LoadConfiguration { get; set; } = LoadConfigurationEnum.Belt;

        /// <summary>
        ///  是否配置静态华夫盒
        /// </summary>
        [TreeProgramListArgs("静态华夫盒配置（设置后需修改模组安全位）", (string)null, null, RoleEnum.Admin)]
        public bool IsStaticWaffleConfigrated { get; set; } = false;

        /// <summary>
        ///  下料载台是否继续运动
        /// </summary>
        [TreeProgramListArgs("下料载台是否继续运动", (string)null, null, RoleEnum.Admin)]
        public bool IsUnloadingSubSectionContinueMove { get; set; } = false;

        /// <summary>
        ///  T 轴单独驱动  不跟x y Z 一起插补
        /// </summary>
        [TreeProgramListArgs("T轴单独驱动", (string)null, null, RoleEnum.Admin)]
        public bool IsBondAxisTSingleMove { get; set; } = false;

        /// <summary>
        ///  系统2配置点胶头
        /// </summary>
        [TreeProgramListArgs("系统2配置点胶头", (string)null, null, RoleEnum.Admin)]
        public bool IsSystem2Dispense { get; set; } = false;

        /// <summary>
        ///  系统2配置点胶头
        /// </summary>
        [TreeProgramListArgs("系统2蘸胶盘是否配置", (string)null, null, RoleEnum.Admin)]
        public bool IsSystem2ConfigPrintingTool { get; set; } = false;

        /// <summary>
        /// 系统1是否配置
        /// </summary>
        [TreeProgramListArgs("系统1是否配置", "系统1", null, RoleEnum.Admin)]
        public bool IsSystem1Configrated { get; set; } = true;

        /// <summary>
        /// 系统1蘸胶头是否配置
        /// </summary>
        [TreeProgramListArgs("系统1蘸胶盘是否配置", "系统1", null, RoleEnum.Admin)]
        public bool IsSystem1ConfigPrintingTool { get; set; } = false;

        /// <summary>
        /// 点胶Z是否为固高
        /// </summary>
        [TreeProgramListArgs("点胶Z是否为固高", "系统1", null, RoleEnum.Admin)]
        public bool IsDispenseZGuGao { get; set; } = true;

        /// <summary>
        /// 系统1激光测高是否配置
        /// </summary>
        [TreeProgramListArgs("系统1激光测高是否配置", "系统1", null, RoleEnum.Admin)]
        public bool IsSystem1ConfigLaserMh { get; set; } = false;

        /// <summary>
        /// 左中转台是否配置
        /// </summary>
        [TreeProgramListArgs("左中转台是否配置", (string)null, null, RoleEnum.Admin)]
        public bool IsIPTConfigrated { get; set; } = true;

        /// <summary>
        /// 右中转台是否配置
        /// </summary>
        [TreeProgramListArgs("右中转台是否配置", (string)null, null, RoleEnum.Admin)]
        public bool IsRightIPTConfigrated { get; set; } = false;

        /// <summary>
        /// 翻转台是否配置
        /// </summary>
        [TreeProgramListArgs("翻转台是否配置", (string)null, null, RoleEnum.Admin)]
        public bool IsFlipModuleConfigrated { get; set; } = false;

        /// <summary>
        /// 安全门是否配置
        /// </summary>
        [TreeProgramListArgs("安全门是否配置", (string)null, null, RoleEnum.Admin)]
        public bool IsSafeDoorConfigured { get; set; } = false;

        /// <summary>
        /// 是否清空静态华夫盒记忆在切换程式后
        /// </summary>
        [TreeProgramListArgs("是否配置刮胶盘", (string)null, null, RoleEnum.Admin)]
        public bool IsSlideFluxerConfigured { get; set; } = false;

        /// <summary>
        /// 系统2光源配置，默认三色光
        /// </summary>
        [TreeProgramListArgs("光源配置", (string)null, null, RoleEnum.Admin)]
        public LightConfigEnum LightConfig { get; set; } = LightConfigEnum.TrichromaticLight;

        /// <summary>
        /// 传送系统是否配置
        /// </summary>
        [TreeProgramListArgs("传送系统是否配置", (string)null, null, RoleEnum.Admin)]
        public bool IsTransportConfigured { get; set; } = true;

        /// <summary>
        /// 轨道加热是否配置
        /// </summary>
        [TreeProgramListArgs("轨道加热是否配置", (string)null, null, RoleEnum.Admin)]
        public bool IsTransportHeatConfigured { get; set; } = false;

        /// <summary>
        /// 吸嘴清洁台是否配置
        /// </summary>
        [TreeProgramListArgs("吸嘴清洁台是否配置", (string)null, null, RoleEnum.Admin)]
        public bool IsNozzleCleanTableConfigured { get; set; } = false;

        /// <summary>
        /// 控制卡位置闭环Kp值
        /// </summary>
        [TreeProgramListArgs("控制卡位置闭环Kp值(修改后需重启软件)", (string)null, null, RoleEnum.Admin)]
        public double PositionClosedLoopKp { get; set; } = 1;

        /// <summary>
        /// 控制卡位置闭环Kp值
        /// </summary>
        [TreeProgramListArgs("控制卡位置闭环Kvff值(修改后需重启软件)", (string)null, null, RoleEnum.Admin)]
        public double PositionClosedLoopKvff { get; set; } = 15;

        /// <summary>
        /// 控制卡位置闭环Kp值
        /// </summary>
        [TreeProgramListArgs("控制卡位置闭环Kaff值(修改后需重启软件)", (string)null, null, RoleEnum.Admin)]
        public double PositionClosedLoopKaff { get; set; } = 50;

        /// <summary>
        /// 系统2是否配置激光测高
        /// </summary>
        [TreeProgramListArgs("系统2激光测高是否配置", (string)null, null, RoleEnum.Admin)]
        public bool IsSystem2ConfigLaserMh { get; set; } = false;
        
        /// <summary>
        /// 顶针系统是否配置
        /// </summary>
        [TreeProgramListArgs("顶针系统是否配置", (string)null, null, RoleEnum.Admin)]
        public bool IsEjectSystemConfigured { get; set; } = true;
        
        /// <summary>
        /// 摇杆是否配置
        /// </summary>
        [TreeProgramListArgs("摇杆是否配置", (string)null, null, RoleEnum.Admin)]
        public bool IsJoyStickConfigured { get; set; } = false;

        /// <summary>
        /// 轨迹球是否配置
        /// </summary>
        //[TreeProgramListArgs("轨迹球是否配置", (string)null, null, RoleEnum.Admin)]
        public bool IsTrackballConfigured { get; set; } = false;

        /// <summary>
        /// Magazine是否配置
        /// 如果为false 则屏蔽晶圆夹和Magazine
        /// </summary>
        [TreeProgramListArgs("Magazine是否配置", (string)null, null, RoleEnum.Admin)]
        public bool IsMagazineConfigured { get; set; } = true;

        /// <summary>
        /// 激光干涉尺是否配置
        /// </summary>
        [TreeProgramListArgs("激光干涉尺是否配置", (string)null, null, RoleEnum.Admin)]
        public bool IsLaserEncoderConfigured { get; set; } = false;

        /// <summary>
        /// bond回零实验点1
        /// </summary>
        public AKRSPoint3D BondGoHomeTestPoint1 { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// bond回零实验点2
        /// </summary>
        public AKRSPoint3D BondGoHomeTestPoint2 { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// bond回零实验点3
        /// </summary>
        public AKRSPoint3D BondGoHomeTestPoint3 { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static MachineHardwareConfiguration()
        {
            FilePath = ZX2200PathConfig.MachineHardwareConfigurationPath;
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
}
