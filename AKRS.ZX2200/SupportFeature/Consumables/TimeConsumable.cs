namespace AKRS.ZX2200.SupportFeature.Consumables
{
    using System;

    /// <summary>
    /// 时间耗材
    /// </summary>
    [Serializable]
    public class TimeConsumable : BaseConsumable
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime DueDate => this.GetExpirationDate();

        /// <summary>
        /// 胶水使用时间
        /// 单位为分钟
        /// </summary>
        public int UseDateTime { get; set; } = 1440;

        /// <summary>
        /// 胶水提醒时间
        /// 百分比
        /// </summary>
        public int RemindTimeRate { get; set; } = 80;

        /// <summary>
        /// 获取到期时间
        /// </summary>
        /// <returns>时间</returns>
        private DateTime GetExpirationDate()
        {
            DateTime dateTime = this.StartDateTime;
            return dateTime.AddMinutes(this.UseDateTime);
        }

        /// <summary>
        /// 清空记录
        /// </summary>
        public override void Clear()
        {
            this.IsReminded = false;
            this.StartDateTime = DateTime.Now;
        }

        /// <summary>
        /// 是否到期
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsExpired()
        {
            if (!this.Enable)
            {
                return false;
            }

            if (DateTime.Now > this.DueDate)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 是否提醒
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsRemind()
        {
            if (!this.Enable)
            {
                return false;
            }

            DateTime dateTime = this.StartDateTime;
            if (DateTime.Now > dateTime.AddMinutes((int)(this.UseDateTime * this.RemindTimeRate / 100.0)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取剩余时间
        /// </summary>
        /// <returns>时间</returns>
        public int GetRemainingTime()
        {
            TimeSpan minuteSpan = new TimeSpan(this.DueDate.Ticks - DateTime.Now.Ticks);
            return (int)minuteSpan.TotalMinutes;
        }
    }
}
