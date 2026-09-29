#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/10/12 13:11:10
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
    using System.Collections.Generic;

    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.MachineSupport.Config;

    using Newtonsoft.Json;

    /// <summary>
    /// 描述：抽象的基础数据集
    /// </summary>
    /// <typeparam name="T">数据泛型</typeparam>
    public abstract class BaseSettingRepository<T> where T : BaseDsSetting
    {
        /// <summary>
        /// 单例锁
        /// </summary>
        [JsonIgnore]
        protected static volatile object locker = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        protected BaseSettingRepository()
        {
            this.Load();
        }

        /// <summary>
        /// BondinsertSetting列表
        /// </summary>
        public List<T> BaseDsSettingList { get; set; }

        /// <summary>
        /// 存储文件名称
        /// </summary>
        /// <returns>存储文件名称</returns>
        protected abstract string GetFileName();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dsSetting">数据对象</param>
        public virtual void Add(T dsSetting)
        {
            this.BaseDsSettingList.Add(dsSetting);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dsSetting">数据对象</param>
        public virtual void Remove(T dsSetting)
        {
            this.BaseDsSettingList.Remove(dsSetting);
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            JsonFormatHelper<T>.SaveGenericList(this.BaseDsSettingList, this.GetFileName());
        }

        /// <summary>
        /// 加载
        /// </summary>
        public void Load()
        {
           this.BaseDsSettingList = JsonFormatHelper<T>.ReadGenericList(this.GetFileName())?? new List<T>();
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="name">数据配置实体名称</param>
        /// <returns>数据配置实体</returns>
        public BaseDsSetting Find(string name)
        {
            BaseDsSetting dsSetting = this.BaseDsSettingList.Find(bi => bi.Name == name);

            return dsSetting;
        }

        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="isIncludeAll">包含全部</param>
        /// <returns>数据集</returns>
        public List<T> Filter(string name, bool isIncludeAll)
        {
            return this.BaseDsSettingList.FindAll(
                a => (string.IsNullOrWhiteSpace(name) || a.Name.Contains(name)) 
                    && (a.IsEnable == true || isIncludeAll == true));
        }
  
        /// <summary>
        /// 获取数据源 根据当前的Recipe
        /// </summary>
        /// <returns>数据集</returns>
        public List<T> GetDataSourceByCurrentRecipe()
        {
           return this.BaseDsSettingList.FindAll(a => a.RecipeIDList.Contains(MachineConfigContext.GetInstance().RecipeID));
        }

        /// <summary>
        /// 检查是否存在config
        /// </summary>
        /// <param name="name">config名称</param>
        /// <returns>result</returns>
        public bool IsExists(string name)
        {
            return this.BaseDsSettingList.Exists(t => t.Name == name);
        }
    }
}
