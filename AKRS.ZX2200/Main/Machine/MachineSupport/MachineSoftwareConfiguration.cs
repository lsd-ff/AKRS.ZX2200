using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.ZX2200.Infrastructure.AOP.Module;
using AKRS.ZX2200.Infrastructure.Models.Enums;
using AKRS.ZX2200.Infrastructure.Models.Path;
using AKRS.ZX2200.SupportFeature.Parameters;
using AKRS.ZX2200.SupportFeature.Parameters.Model;
using PropertyChanged;

namespace AKRS.ZX2200.Main.Machine.MachineSupport
{
    /// <summary>
    /// 设备软件配置
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class MachineSoftwareConfiguration : Singleton<MachineSoftwareConfiguration>, INotifyPropertyChanged
    {
        /// <summary>
        /// 当前软件版本号
        /// </summary>
        public string CurrentVersionNumber { get; set; } = "0.0.0.0";

        /// <summary>
        /// 上次软件版本号
        /// </summary>
        public string LastVersionNumber { get; set; } = "0.0.0.0";

        /// <summary>
        /// 最后一次软件更新时间
        /// </summary>
        public DateTime LastVersionUpdataTime { get; set; }

        /// <summary>
        /// 是否记录焊后无参考点
        /// </summary>
        [TreeProgramListArgs("是否记录工作日志")]
        public bool IsWorkLog { get; set; } = true;

        /// <summary>
        /// 是否清空静态华夫盒记忆在切换程式后
        /// </summary>
        [TreeProgramListArgs("是否清空静态华夫盒记忆在切换程式后")]
        public bool IsClearStaticAdapter { get; set; } = true;

        /// <summary>
        /// 文件存储期限
        /// </summary>
        [TreeProgramListArgs("图片等存储期限(设备自动工作前自动删除)")]
        public int FileSaveDue { get; set; } = 7;

        /// <summary>
        /// 数据库存储日期
        /// </summary>
        [TreeProgramListArgs("数据库存储日期(软件启动时自动删除)")]
        public int SqlSaveDue { get; set; } = 180;

        /// <summary>
        /// 启动之前是否检查所有是否复位
        /// </summary>
        [TreeProgramListArgs("软件启动轴自动复位")]
        public bool IsAxisResetBeforeWork { get; set; } = false;

        /// <summary>
        /// 机器启动前程式检查报警
        /// </summary>
        [TreeProgramListArgs("机器启动前程式检查报警")]
        public bool RecipeWarningBeforeMachineStart { get; set; } = false;

        /// <summary>
        /// 速度百分比
        /// </summary>
        [TreeProgramListArgs("机器速度百分比", null, 0.01, 1,"", RoleEnum.Admin)]
        public double MachineMoveSpeedPercentage { get; set; } = 1;

        /// <summary>
        /// 轴运动模式
        /// </summary>
        [TreeProgramListArgs("轴运动模式", (string)null, null, RoleEnum.Admin)]
        public AxisMoveModeEnum AxisMoveMode { get; set; } = AxisMoveModeEnum.InterpolationMove;

        /// <summary>
        /// 自动保存框架Mapping
        /// </summary>
        [TreeProgramListArgs("自动保存框架Mapping", "框架Mapping")]
        public bool AutoSaveMapping { get; set; } = false;

        /// <summary>
        /// 自动保存框架Mapping
        /// </summary>
        [TreeProgramListArgs("保存框架Mapping芯片信息", "框架Mapping")]
        public bool AutoSaveBondPositionComponentMapping { get; set; } = false;

        /// <summary>
        /// 自动退出登录
        /// </summary>
        [TreeProgramListArgs("自动退出登录","自动退出登录功能")]
        public bool AutoLogOut { get; set; } = false;

        /// <summary>
        /// 自动退出登录时间
        /// </summary>
        [TreeProgramListArgs("自动退出登录时间", "自动退出登录功能", 0, 1440, UnitName = "分钟")]
        public int AutoLogOutTime { get; set; } = 60;

        /// <summary>
        /// 权限管理
        /// </summary>
        [TreeProgramListArgs("权限管理")]
        public bool IsPermissionOpen { get; set; } = true;

        /// <summary>
        /// 基板拍照异步执行
        /// </summary>
        [TreeProgramListArgs("基板拍照异步执行")]
        public bool IsTuVisionAsync { get; set; } = false;

        /// <summary>
        /// 固晶补偿
        /// </summary>
        [TreeProgramListArgs("终极补偿是否开启", "补偿")]
        public bool IsBondCompensate { get; set; } = false;

        /// <summary>
        /// 2维补偿是否开启
        /// </summary>
        [TreeProgramListArgs("初始补偿是否开启", "补偿")]
        public bool Is2DCompensationOpen { get; set; } = true;

        /// <summary>
        /// 2维补偿是否开启
        /// </summary>
        [TreeProgramListArgs("系统1系统2共用视觉模板", "")]
        public bool System1And2UseSamePr { get; set; } = false;

        /// <summary>
        /// 屏蔽蜂鸣器
        /// </summary>
        [TreeProgramListArgs("屏蔽蜂鸣器、三色灯", (string)null, null, RoleEnum.Admin)]
        public bool IsBlockAlarmer { get; set; } = false;

        /// <summary>
        /// 是否存图
        /// </summary>
        [TreeProgramListArgs("是否存图", (string)null, null, RoleEnum.Admin)]
        public bool ISSaveLocateImage { get; set; } = true;

        /// <summary>
        /// 是否检测加密狗
        /// </summary>
        public bool IsDetectingEncryption { get; set; } = false;

        /// <summary>
        /// 是否提示加密到期
        /// </summary>
        public bool IsPromptEncryption { get; set; } = true;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static MachineSoftwareConfiguration()
        {
            FilePath = ZX2200PathConfig.MachineSoftwareConfigurationPath;
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

        /// <summary>
        ///  是否存图
        /// </summary>
        /// <returns>结果</returns>
        public bool SaveLoacteImage()
        {
            return this.ISSaveLocateImage;
        }
    }
}
