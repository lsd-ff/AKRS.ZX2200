namespace AKRS.ZX2200.Infrastructure.Models.BaseModels
{
    using System;

    /// <summary>
    /// 材料的抽象类
    /// </summary>
    public abstract class BaseMaterial
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Id { get; set; }

        /// <summary>
        /// 耗材的创造时间
        /// </summary>
        public DateTime CreateTime;

        /// <summary>
        /// 工具的最后使用时间
        /// </summary>
        public DateTime LastUsedTime;

        /// <summary>
        /// 工具的使用次数
        /// </summary>
        public int UsedTimes;

        /// <summary>
        /// 工具一共使用的总时长,单位为ms
        /// </summary>
        public int UsedTime;

        /// <summary>
        /// 是不是耗材
        /// </summary>
        public bool Consumables;


        /// <summary>
        /// 是否开始使用，针对于某些开封之后才开始计时的
        /// </summary>
        public bool StartUsing;

        /// <summary>
        /// 初始化
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// 使用完之后更新
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// 是否能够使用
        /// </summary>
        /// <returns></returns>
        public abstract bool CanUse();
    }
}
