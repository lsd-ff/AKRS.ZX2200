using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.DispenseSystem.Models.DeviceParams
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.Infrastructure.AOP.Module;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using PropertyChanged;
    using System.ComponentModel;

    /// <summary>
    /// 点胶模组参数
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class DispenseModulePara : PropertyChangeAop
    {
        /// <summary>
        /// 到位之后的拍照延迟
        /// </summary>
        [TreeProgramListArgs("拍照延迟", null, 0, 10000, "ms")]
        public int VisionDelay { get; set; } = 100;

        /// <summary>
        /// 气缸到位延迟
        /// </summary>
        [TreeProgramListArgs("气缸未到位检测时间", null, 0, 10000, "ms")]
        public int CylinderDelay { get; set; } = 500;

        /// <summary>
        /// 激光测高到位后工作延时
        /// </summary>
        [TreeProgramListArgs("激光测高到位后工作延时", null, 0, 10000, "ms")]
        public int LaserMhDelayTime { get; set; } = 200;

        /// <summary>
        /// 测高速度
        /// </summary>
        [TreeProgramListArgs("测高速度", null, 0, 500, "mm/s")]
        public int MeasureHeightSpeed { get; set; } = 20;

        /// <summary>
        /// 测高距离平面开始距离
        /// </summary>
        [TreeProgramListArgs("测高距离平面开始距离", null, 0, 10, "mm")]
        public double MeasureHeightDistance { get; set; } = 5;

        /// <summary>
        /// 缓慢下探的终点
        /// </summary>
        [TreeProgramListArgs("测高下探行程", null, 0, 10, "mm")]
        public double MeasureHeightDownDistance { get; set; } = 3;

        /// <summary>
        /// 点胶测高跨越高度
        /// </summary>
        [TreeProgramListArgs("点胶测高时跨越高度", null, 0, 20, "mm")]
        public double MeasureHeightCrossingHeights { get; set; } = 10;

        /// <summary>
        /// 测高报警距离
        /// </summary>
        [TreeProgramListArgs("测高距离误差报警阈值", null, 0, 5, "mm")]
        public double MeasureHeightAlarmDistance { get; set; } = 1;

        /// <summary>
        /// 系统1的安全位置
        /// </summary>
        [TreeProgramListArgs("系统1的安全位置(机械坐标)", (string)null)]
        public AKRSPoint3D SafePoint3D { get; set; } = new AKRSPoint3D(10, -10, 0);

        /// <summary>
        /// 蘸胶旋转速度
        /// </summary>
        [TreeProgramListArgs("蘸胶旋转速度", (string)null,"转/分钟")]
        public double PrintSpinSpeed { get; set; } = 10;

        /// <summary>
        /// 系统1点胶胶压补偿
        /// </summary>
        [TreeProgramListArgs("系统1点胶胶压补偿", null, -100, 100, "千帕")]
        public int GluePressCompensation { get; set; } = 0;

        /// <summary>
        /// 是否链接点胶器
        /// </summary>
        [TreeProgramListArgs("点胶器通讯", (string)null)]
        public bool IsLinkDispenser { get; set; } = true;

        /// <summary>
        /// 点胶器最大胶压
        /// </summary>
        [TreeProgramListArgs("点胶器最大胶压", null, 200, 500, "千帕")]
        public int DispenserMaxValue { get; set; } = 200;

        /// <summary>
        /// 点胶整体偏移量
        /// </summary>
        [TreeProgramListArgs("点胶整体偏移量(单位为mm)", (string)null)]
        public AKRSPoint3D DispenseOffset { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 第一次点胶延迟
        /// </summary>
        [TreeProgramListArgs("第一次点胶延迟", null, 0, 100000, "ms")]
        public int FisrtDispenseDelay { get; set; } = 0;
    }
}
