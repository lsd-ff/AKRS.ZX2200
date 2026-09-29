#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/18 16:51:08
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

namespace AKRS.ZX2200.Infrastructure.Models.BaseModels
{
    using System;

    /// <summary>
    /// 描述：带排序的数据集设置项基类
    /// </summary>
    [Serializable]
    public abstract class BaseOrderDsSetting : BaseDsSetting, IComparable<BaseOrderDsSetting>
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="other">其他实体</param>
        /// <returns>比较结果</returns>
        public int CompareTo(BaseOrderDsSetting other)
        {
            if (this.Order > other.Order)
            {
                return 1;
            }
            else if (this.Order == other.Order)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }
}
