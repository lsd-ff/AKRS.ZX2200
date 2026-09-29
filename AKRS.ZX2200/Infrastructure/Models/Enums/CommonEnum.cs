namespace AKRS.ZX2200.Infrastructure.Models.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// ProcessingStrategyEnum
    /// </summary>
    public enum ProcessingStrategyEnum
    {
        /// <summary>
        /// 先做一个基岛再做下一个基岛
        /// </summary>
        [Description("基岛优先")]
        ModulesBeforeSteps = 0,

        /// <summary>
        /// 先做一个基岛再做下一个基岛
        /// </summary>
        [Description("基板优先")]
        SubstrateBeforeSteps = 1,

        /// <summary>
        /// 贴完一种芯片再贴另一种
        /// </summary>
        [Description("步骤优先")]
        StepsFirst = 2,
    }

    /// <summary>
    /// 动作节点排序模式
    /// </summary>
    public enum ActionNodesSortModeEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常模式")]
        Normal = 0,

        /// <summary>
        /// 高uph
        /// </summary>
        [Description("高速模式")]
        HighUPH = 1,
    }

    /// <summary>
    /// 角色枚举
    /// </summary>
    public enum RoleEnum
    {
        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        Admin = 1,

        /// <summary>
        /// 工程师
        /// </summary>
        [Description("工程师")]
        Engineer = 2,

        /// <summary>
        /// 操作员
        /// </summary>
        [Description("操作员")]
        Operator = 3,
    }

    /// <summary>
    /// 示教状态枚举
    /// </summary>
    public enum AssistantStateEnum
    {
        /// <summary>
        /// 通过
        /// </summary>
        Able = 0,

        /// <summary>
        /// 未通过
        /// </summary>
        UnAble = 1,

        /// <summary>
        /// 禁止
        /// </summary>
        ForBidden = 2,
    }

    /// <summary>
    /// 语言枚举
    /// </summary>
    public enum LanguageEnum
    {
        /// <summary>
        /// 中文
        /// </summary>
        Chinese = 0,

        /// <summary>
        /// 英文
        /// </summary>
        English = 1,
    }

    /// <summary>
    /// 调试模式
    /// </summary>
    public enum DebugModelEnum
    {
        /// <summary>
        /// 暂停
        /// </summary>
        [Description("暂停模式")]
        PauseModel,

        /// <summary>
        /// 暂停
        /// </summary>
        [Description("延时模式")]
        DelayModel
    }
}
