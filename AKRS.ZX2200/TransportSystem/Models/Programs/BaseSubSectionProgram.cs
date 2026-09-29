#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/15 13:10:03
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AKRS.ZX2200.TransportSystem.Models.Programs
{
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    /// <summary>
    /// 描述：程式基类
    /// </summary>
    public abstract class BaseSubSectionProgram
    {
        /// <summary>
        /// 流道载台的状态  有料、无料
        /// </summary>
        [JsonIgnore]
        public SubSectionStateEnum SubSectionState { get; set; }

        /// <summary>
        /// 流道载台的状态 传输中、报警, 准备状态
        /// </summary>
        [JsonIgnore]
        public SubSectionStateTransferEnum SubSectionTransferState { get; set; } = SubSectionStateTransferEnum.Ready;

        /// <summary>
        /// 载具
        /// 不保存在本地，不序列化，用补料模式
        /// </summary>
        [JsonIgnore]
        public TransportUnit TransportUnit { get; set; }

        /// <summary>
        /// 重新构建TU对象，一般是TU对象结构发生改变之后会做的事情
        /// 这个主要是为了对TU对象的保护
        /// 外界只能通过流道的方法对TU进行操作
        /// </summary>
        public void UpdateTransportUnit()
        {
            this.TransportUnit = new TransportUnit("示教");
        }

        /// <summary>
        /// 刷新坐标系
        /// 一般是反序列化以及载台之间传递会用到此方法
        /// </summary>
        public void RefreshTransportUnit()
        {
            this.TransportUnit.Refresh();
        }
    }

    /// <summary>
    /// 流道载台的状态
    /// </summary>
    public enum SubSectionStateEnum
    {
        /// <summary>
        /// 无料
        /// </summary>
        [Description("无料")]
        NoMaterial,

        /// <summary>
        /// 有料
        /// </summary>
        [Description("有料")]
        HasMaterial
    }

    /// <summary>
    /// 流道载台的传送状态
    /// </summary>
    public enum SubSectionStateTransferEnum
    {
        /// <summary>
        /// 传料中
        /// </summary>
        [Description("传料中")]
        Transfering,

        /// <summary>
        /// 报警
        /// </summary>
        [Description("报警")]
        Alarm,

        /// <summary>
        /// 准备状态
        /// </summary>
        [Description("准备")]
        Ready
    }
}
