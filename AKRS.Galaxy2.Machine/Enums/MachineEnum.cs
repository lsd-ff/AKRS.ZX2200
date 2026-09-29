using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.Galaxy2.Machine.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// 设备状态枚举
    /// </summary>
    public enum MachineStateEnum
    {
        /// <summary>
        /// 停止(设备停止标记)
        /// </summary>
        [Description("停止")]
        Stop = 0,

        /// <summary>
        /// 暂停(设备报警标志)
        /// </summary>
        [Description("暂停")]
        Pause,

        /// <summary>
        /// 工作中(设备工作中 标志)
        /// </summary>
        [Description("工作中")]
        Working,

        /// <summary>
        /// 手动操作(人工操作标志)
        /// </summary>
        [Description("手动操作")]
        Operate,

        /// <summary>
        /// 示教中
        /// </summary>
        [Description("示教中")]
        Teaching,

        /// <summary>
        /// 准备中
        /// </summary>
        [Description("准备中")]
        Preparing,
    }

    /// <summary>
    /// 工作模式
    /// </summary>
    public enum MachineWorkModeEnum
    {
        /// <summary>
        /// 正常工作模式
        /// </summary>
        [Description("正常模式")]
        NormalWork = 0,

        /// <summary>
        /// 空跑模式
        /// </summary>
        [Description("空跑模式(不带载具)")]
        DryCycle = 1,

        /// <summary>
        /// 离线模式
        /// </summary>
        [Description("离线模式")]
        OffLineWork = 2,

        /// <summary>
        /// 调试模式
        /// </summary>
        [Description("调试模式")]
        DeBugWork = 3,

        /// <summary>
        /// 调试模式
        /// </summary>
        [Description("单颗循环模式")]
        CompensateWork = 4
    }

    /// <summary>
    /// 当前选择的系统
    /// </summary>
    public enum CurrentMachineSystemEnum
    {
        /// <summary>
        /// 系统1
        /// </summary>
        System1,

        /// <summary>
        /// 系统2
        /// </summary>
        System2
    }

    /// <summary>
    /// 信号状态枚举
    /// </summary>
    public enum SignalStateEnum
    {
        /// <summary>
        /// 置位
        /// </summary>
        [Description("Set")]
        Set,

        /// <summary>
        /// 复位
        /// </summary>
        [Description("复位")]
        ReSet = 2,
    }

    /// <summary>
    /// 上料配置枚举
    /// </summary>
    public enum LoadConfigurationEnum
    {
        /// <summary>
        /// 皮带，1号机
        /// </summary>
        [Description("皮带")]
        Belt,

        /// <summary>
        /// 料仓，2号机
        /// </summary>
        [Description("料仓")]
        LoaderBin,

        /// <summary>
        /// 联机，3号机
        /// </summary>
        [Description("联机")]
        Online,

        /// <summary>
        /// 只和上料机联机，3号机
        /// </summary>
        [Description("只和上料机联机")]
        OnlineWithAutoLoader,

        /// <summary>
        /// 只和下料机联机，3号机
        /// </summary>
        [Description("只和下料机联机")]
        OnlineWithAutoUnloader,

        /// <summary>
        /// 无上下料
        /// </summary>
        [Description("无上下料")]
        None,
    }

    /// <summary>
    /// 轴运动模式枚举
    /// </summary>
    public enum AxisMoveModeEnum
    {
        /// <summary>
        /// 插补运动
        /// </summary>
        [Description("插补运动")]
        InterpolationMove = 0,

        /// <summary>
        /// 绝对运动
        /// </summary>
        [Description("绝对运动")]
        AbsoluteMove = 1,
    }
}
