namespace AKRS.ZX2200.SupportFeature.Consumables
{
    using System;

    /// <summary>
    /// 基础耗材类
    /// </summary>
    [Serializable]
    public abstract class BaseConsumable
    {
        /// <summary>
        /// 耗材名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; } = false;

        /// <summary>
        /// 是否已经提醒过了
        /// </summary>
        public bool IsReminded { get; set; } = false;

        /// <summary>
        /// 耗材是否使用完成或者耗材是否过期
        /// </summary>
        /// <returns>结果</returns>
        public abstract bool IsExpired();

        /// <summary>
        /// 是否应该提醒
        /// </summary>
        /// <returns>结果</returns>
        public abstract bool IsRemind();

        /// <summary>
        /// 清空当前记录
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// 最近一次报警的时间
        /// 10分钟之内不会再报警
        /// </summary>
        public DateTime LastAlarmTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 距离上次报警是否有10分钟，如果超过10分钟就再次报警
        /// </summary>
        /// <returns>结果</returns>
        public bool IsEnableTimeToAlarm()
        {
            if (this.IsAlarmed)
            {
                return (DateTime.Now - this.LastAlarmTime).TotalMinutes > 10;
            }

            this.IsAlarmed = true;
            return true;
        }

        /// <summary>
        /// 是否已经报过警
        /// </summary>
        public bool IsAlarmed { get; set; } = false;
    }
}
