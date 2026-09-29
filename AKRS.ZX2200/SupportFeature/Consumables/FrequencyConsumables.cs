namespace AKRS.ZX2200.SupportFeature.Consumables
{
    using System;

    /// <summary>
    /// 使用频率耗材
    /// </summary>
    [Serializable]
    public class FrequencyConsumables : BaseConsumable
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        public FrequencyConsumables(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 总共使用的次数
        /// </summary>
        public int TotalUseTimes { get; set; } = 10000;

        /// <summary>
        /// 当前使用次数
        /// </summary>
        public int CurrentUseTimes { get; set; } = 0;

        /// <summary>
        /// 使用多少次之后提醒
        /// </summary>
        public int RemindUserTimes { get; set; } = 90;

        /// <summary>
        /// 使用次数清空
        /// </summary>
        public override void Clear()
        {
            this.IsReminded = false;
            this.CurrentUseTimes = 0;
        }

        /// <summary>
        /// 是否达到使用次数
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsExpired()
        {
            if (!this.Enable)
            {
                return false;
            }

            if (this.CurrentUseTimes > this.TotalUseTimes)
            {
                return true;
            }
            else
            {
                return false;
            }
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

            if ((this.CurrentUseTimes / this.TotalUseTimes) > (this.RemindUserTimes / 100))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
