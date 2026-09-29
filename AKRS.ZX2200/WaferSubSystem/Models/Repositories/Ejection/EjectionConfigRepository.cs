using AKRS.ZX2200.Models;

using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection
{
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;
    using System.Collections.Generic;
    using DevExpress.Utils.Extensions;

    /// <summary>
    /// 顶针库
    /// </summary>
    public class EjectionConfigRepository : BaseSettingRepository<EjectionConfig>
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static EjectionConfigRepository instance;

        /// <summary>
        /// 存储文件名称
        /// </summary>
        [JsonIgnore]
        public static string FileName => ZX2200PathConfig.EjectionRepository;

        /// <summary>
        /// 存储文件名称
        /// </summary>
        /// <returns>存储文件名称</returns>
        protected override string GetFileName()
        {
            return FileName;
        }

        /// <summary>
        /// 私有化
        /// </summary>
        private EjectionConfigRepository()
        {
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static EjectionConfigRepository GetInstance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new EjectionConfigRepository();
                }

                return instance;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dsSetting">数据对象</param>
        public override void Remove(EjectionConfig dsSetting)
        {
            this.BaseDsSettingList.Remove(dsSetting);
            PREntity.DeletePREntity(dsSetting.Name + "EjectMatch");

            VisionEntityRepository.GetInstance().PRVisionList.Remove(it => it.GetName() == dsSetting.Name + "EjectMatch");
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dsSetting">数据对象</param>
        public override void Add(EjectionConfig dsSetting)
        {
            List<string> prNames = new List<string>();

            // 在PR库里面找到所有包涵旧配方的名称
            List<BaseVisionEntity> baseVisionEntities = VisionEntityRepository.GetInstance().PRVisionList;

            List<string> oldPRNameList = new List<string>();

            oldPRNameList.Add(dsSetting.CopyName + "EjectMatch");

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
