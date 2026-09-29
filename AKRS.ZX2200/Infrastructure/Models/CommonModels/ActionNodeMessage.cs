using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Services
{
    /// <summary>
    /// 动作执行信息
    /// </summary>
    public class ActionNodeMessage
    {
        /// <summary>
        /// 动作信息
        /// </summary>
        /// <param name="actionNodeName">名称</param>
        /// <param name="substrate">基板</param>
        /// <param name="module">基岛</param>
        /// <param name="processStepName">动作里面的信息</param>
        public ActionNodeMessage(string actionNodeName, int substrate, int module, string processStepName)
        {
            this.ActionNodeName = actionNodeName;
            this.Substrate = substrate;
            this.Module = module;
            this.ProcessStepName = processStepName;
        }

        /// <summary>
        /// 动作信息
        /// </summary>
        public ActionNodeMessage()
        {
        }

        /// <summary>
        /// 动作名称
        /// </summary>
        public string ActionNodeName { get; set; }

        /// <summary>
        /// 基板号
        /// </summary>
        public int Substrate { get; set; }

        /// <summary>
        /// 基岛号
        /// </summary>
        public int Module { get; set; }

        /// <summary>
        /// 执行基岛所在的行
        /// </summary>
        public string ProcessStepName { get; set; }

        /// <summary>
        /// 是否已经完成
        /// </summary>
        public bool Finished { get; set; }
    }
}

/// <summary>
/// 动作执行单元
/// </summary>
public enum ActionTypeEnum
{
    /// <summary>
    /// 基岛
    /// </summary>
    Island,

    /// <summary>
    /// 载具
    /// </summary>
    TransportUnit,

    /// <summary>
    /// 基板
    /// </summary>
    Substrate,

    /// <summary>
    /// 焊点
    /// </summary>
    BondPosition
}
