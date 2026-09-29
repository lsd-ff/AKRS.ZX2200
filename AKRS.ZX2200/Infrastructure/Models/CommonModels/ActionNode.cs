namespace AKRS.ZX2200.Infrastructure.Models.CommonModels
{
    using System;

    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.ZX2200.Models;

    using Newtonsoft.Json;

    /// <summary>
    /// 动作节点
    /// </summary>
    [Serializable]
    public abstract class ActionNode
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        protected ActionNode()
        {
        }

        /// <summary>
        /// 动作节点名字
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 动作层级
        /// </summary>
        public ActionLevelEnum ActionLevel { get; set; }

        /// <summary>
        /// 排序使用
        /// </summary>
        public ActionTypeEnum ActionSortType { get; set; }

        /// <summary>
        /// 动作所在的系统
        /// </summary>
        public ActionNodeSystemEnum System { get; set; }

        /// <summary>
        /// 动作开始事件
        /// </summary>
        [JsonIgnore]
        public Action WorkStart { get; set; }

        /// <summary>
        /// 运行过程中的信息
        /// </summary>
        public ActionNodeInformation Info { get; set; }

        /// <summary>
        /// 是否去完成这个动作
        /// 这里面主要包涵去完成这个动作的条件
        /// </summary>
        /// <returns>结果</returns>
        public virtual bool IsDoWork()
        {
            return true;
        }

        /// <summary>
        /// 是否去完成这个动作
        /// 这里面主要包涵对空指针已经逻辑完全有问题的判断
        /// </summary>
        /// <returns>结果</returns>
        public virtual bool IsAlarm()
        {
            return true;
        }

        /// <summary>
        /// 动作停止事件
        /// </summary>
        [JsonIgnore]
        public Action WorkStop { get; set; }

        /// <summary>
        /// 动作节点状态
        /// </summary>
        [JsonIgnore]
        public RunStateEnum State { get; set; } = RunStateEnum.Idle;

        /// <summary>
        /// 是否关闭气缸
        /// </summary>
        public bool IsCloseCylinder { get; set; } = false;

        /// <summary>
        /// 动作内容
        /// </summary>
        public ActionNodeStyle Style { get; set; } = ActionNodeStyle.Other;

        /// <summary>
        /// 是否为拍照动作
        /// </summary>
        public bool IsVisionAction { get; set; } = false;

        /// <summary>
        /// 执行节点动作
        /// </summary>
        /// <returns>执行结果</returns>
        public virtual ExcuteResult DoWork()
        {
            this.WorkStart?.Invoke();
            this.State = RunStateEnum.Running;
            try
            {
                return ExcuteResult.Success;
            }
            finally
            {
                this.State = RunStateEnum.Stop;
                this.WorkStop?.Invoke();
            }
        }
    }

    /// <summary>
    /// 动作的层级
    /// </summary>
    public enum ActionLevelEnum
    {
        /// <summary>
        /// 载具
        /// </summary>
        Carrier,

        /// <summary>
        /// 基板
        /// </summary>
        Substrate,

        /// <summary>
        /// 模组
        /// </summary>
        Module,

        /// <summary>
        /// 焊点
        /// </summary>
        BondPosition,

        /// <summary>
        /// 其他
        /// </summary>
        Other
    }

    /// <summary>
    /// 执行这个动作的系统
    /// </summary>
    public enum ActionNodeSystemEnum
    {
        /// <summary>
        /// 系统1
        /// </summary>
        System1,

        /// <summary>
        /// 系统2
        /// </summary>
        System2,
    }

    /// <summary>
    /// 动作类型
    /// </summary>
    public enum ActionNodeStyle
    {
        /// <summary>
        /// 拍照
        /// </summary>
        Vision,

        /// <summary>
        /// 测高
        /// </summary>
        MeasureHeight,

        /// <summary>
        /// 其他
        /// </summary>
        Other
    }
}
