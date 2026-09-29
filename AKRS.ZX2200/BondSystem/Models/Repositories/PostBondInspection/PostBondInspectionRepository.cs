using System.Collections.Generic;
using AKRS.ZX2200.Models;

using Newtonsoft.Json;

namespace AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection
{
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;
    using AKRS.ZX2200.TransportUnitSystem;
    using DevExpress.Utils.Extensions;
    using DevExpress.XtraLayout.Customization;

    /// <summary>
    /// 焊后检测库
    /// </summary>
    public class PostBondInspectionRepository : BaseSettingRepository<PostBondInspection>
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static PostBondInspectionRepository instance;

        /// <summary>
        /// 存储文件名称
        /// </summary>
        [JsonIgnore]
        public static string FileName => ZX2200PathConfig.PostBondInspectionRepositoryFilePath;

        /// <summary>
        /// 存储文件名称
        /// </summary>
        /// <returns>文件名称</returns>
        protected override string GetFileName()
        {
            return FileName;
        }

        /// <summary>
        /// 获取所有焊后检测的pr模板
        /// </summary>
        /// <returns>result</returns>
        public List<string> GetPostBondPREntityNameList()
        {
            List<string> list = new List<string>();
            foreach (var item in this.GetDataSourceByCurrentRecipe())
            {
                list.Add(item.Name + "1");
                list.Add(item.Name + "r1");
            }

            return list;
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static PostBondInspectionRepository GetInstance()
        {
            lock (BaseSettingRepository<PostBondInspection>.locker) 
            {
                if (instance == null)
                {
                    instance = new PostBondInspectionRepository();
                }

                return instance;
            }
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>对象</returns>
        public PostBondInspection GetPostBondInspection(string name)
        {
            return (PostBondInspection)Find(name);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dsSetting">数据对象</param>
        public override void Remove(PostBondInspection dsSetting)
        {
            this.BaseDsSettingList.Remove(dsSetting);
            PREntity.DeletePREntity(dsSetting.Name + "1");
            PREntity.DeletePREntity(dsSetting.Name + "2");
            PREntity.DeletePREntity(dsSetting.Name + "Refer" + "1");
            PREntity.DeletePREntity(dsSetting.Name + "Refer" + "2");

            VisionEntityRepository.GetInstance().PRVisionList.Remove(it => it.GetName() == dsSetting.Name + "1");
            VisionEntityRepository.GetInstance().PRVisionList.Remove(it => it.GetName() == dsSetting.Name + "2");
            VisionEntityRepository.GetInstance().PRVisionList
                .Remove(it => it.GetName() == dsSetting.Name + "Refer" + "1");
            VisionEntityRepository.GetInstance().PRVisionList
                .Remove(it => it.GetName() == dsSetting.Name + "Refer" + "2");
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dsSetting">数据对象</param>
        public override void Add(PostBondInspection dsSetting)
        {
            List<string> prNames = new List<string>();

            // 在PR库里面找到所有包涵旧配方的名称
            List<BaseVisionEntity> baseVisionEntities = VisionEntityRepository.GetInstance().PRVisionList;

            List<string> oldPRNameList = new List<string>();

            oldPRNameList.Add(dsSetting.CopyName + "1");
            oldPRNameList.Add(dsSetting.CopyName + "2");
            oldPRNameList.Add(dsSetting.CopyName + "Refer" + "1");
            oldPRNameList.Add(dsSetting.CopyName + "Refer" + "2");

            foreach (BaseVisionEntity baseVisionEntity in baseVisionEntities)
            {
                foreach (var name in oldPRNameList)
                {
                    if (baseVisionEntity.GetName() == name)
                    {
                        prNames.Add(baseVisionEntity.GetName());
                    }
                }
            }

            // 复制PR
            foreach (string prName in prNames)
            {
                string newPrName = prName.Replace(dsSetting.CopyName, dsSetting.Name);

                PREntity.CopyPREntity(prName, newPrName);
            }

            VisionEntityRepository.GetInstance().Save();

            this.BaseDsSettingList.Add(dsSetting);
        }
    }
}
