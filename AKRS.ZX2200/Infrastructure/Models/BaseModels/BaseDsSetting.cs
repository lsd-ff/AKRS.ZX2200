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
    using System.Collections.Generic;

    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.ZX2200.Infrastructure.AOP.Module;
    using PropertyChanged;

    /// <summary>
    /// 描述：数据集设置项基类
    /// </summary>
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class BaseDsSetting : PropertyChangeAop
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 复制的名称
        /// </summary>
        public string CopyName { get; set; }

        /// <summary>
        /// 编辑状态
        /// </summary>
        public EditStateEnum EditState { get; set; } = EditStateEnum.NewCreate;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; } = true;

        /// <summary>
        /// 是否系统自带的工具
        /// </summary>
        public bool IsSystemConfiguration { get; set; } = false;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = System.DateTime.Now;

        /// <summary>
        /// 所属RecipeID 列表
        /// </summary>
        public List<Guid> RecipeIDList { get; set; } = new List<Guid>();

        /// <summary>
        /// 添加到所属Recipe里
        /// </summary>
        public void AddBelongRecipeIds()
        {
            Guid recipeID = MachineConfigContext.GetInstance().RecipeID;
            if (this.RecipeIDList.Contains(recipeID) == false)
            {
                this.RecipeIDList.Add(recipeID);
            }
        }

        /// <summary>
        /// 从所属Recipe里删除
        /// </summary>
        public void RemoveFromBelongRecipeIds()
        {
            Guid recipeID = MachineConfigContext.GetInstance().RecipeID;
            if (this.RecipeIDList.Contains(recipeID))
            {
                this.RecipeIDList.Remove(recipeID);
            }
        }
    }

    /// <summary>
    /// 编辑状态枚举
    /// </summary>
    public enum EditStateEnum
    {
        /// <summary>
        /// 新建
        /// </summary>
        NewCreate = 0,

        /// <summary>
        /// 未完成
        /// </summary>
        NotYetFinished = 1,

        /// <summary>
        /// 外部修改
        /// </summary>
        ChangedOutside = 2,

        /// <summary>
        /// 完成
        /// </summary>
        Completely = 3
    }
}
