using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Models
{
    using AKRS.ZX2200.Infrastructure.Interface;

    /// <summary>
    /// 工具抽象类
    /// </summary>
    public abstract class BaseTool : ITool
    {
        /// <summary>
        /// 工具的名字
        /// </summary>
        public string Name;

        /// <summary>
        /// 工具的创造时间
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
        /// 工具一共使用的总时长
        /// </summary>
        public DateTime UsedTime;

        /// <summary>
        /// 检查工具是否能够正常工作
        /// 如果不能使用，让使用者去判断
        /// </summary>
        /// <returns></returns>
        public abstract bool CanWorkProperly();

        /// <summary>
        /// 初始化工具
        /// </summary>
        /// <returns></returns>
        public abstract void Init();
    }
}
