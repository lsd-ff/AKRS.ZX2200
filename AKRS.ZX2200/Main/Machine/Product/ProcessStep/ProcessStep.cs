namespace AKRS.ZX2200.Main.Machine.Product.ProcessStep
{
    using System;

    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;

    /// <summary>
    /// 点胶Bond动作步骤存储类
    /// </summary>
    [Serializable]
    public class ProcessStep : BaseOrderDsSetting
    {
        /// <summary>
        /// 焊点名称
        /// </summary>
        public string BondPositionName { get; set; }

        /// <summary>
        /// 芯片名称
        /// </summary>
        public string ComponentName { get; set; }

        /// <summary>
        /// 系统1焊后检测
        /// </summary>
        public string PostBondInspectionNameInS1 { get; set; }

        /// <summary>
        /// 系统2焊后检测
        /// </summary>
        public string PostBondInspectionNameInS2 { get; set; }

        /// <summary>
        /// 系统1点胶图形名称
        /// </summary>
        public string EpoxyApplicationNameInS1 { get; set; }

        /// <summary>
        /// 系统2点胶图形名称
        /// </summary>
        public string EpoxyApplicationNameInS2 { get; set; }

        /// <summary>
        /// 在系统1的运行时机，暂时没用
        /// </summary>
        public RunThisStepEnum RunThisStepInSystem1 { get; set; }

        /// <summary>
        /// 在系统2的运行时机，暂时没用
        /// </summary>
        public RunThisStepEnum RunThisStepInSystem2 { get; set; }

        /// <summary>
        /// 是否在系统1启用
        /// </summary>
        public bool IsEnableInSystem1 { get; set; } = true;

        /// <summary>
        /// 是否在系统2启用
        /// </summary>
        public bool IsEnableInSystem2 { get; set; } = true;
    }
}
