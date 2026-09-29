using AKRS.ZX2200.Models;

using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer
{
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using System.Collections.Generic;
    using DevExpress.Utils.Extensions;

    /// <summary>
    /// 芯片数据集
    /// </summary>
    public class CarrierConfigRepository : BaseSettingRepository<BaseCarrierConfig>
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static CarrierConfigRepository instance;

        /// <summary>
        /// 存储文件名称
        /// </summary>
        [JsonIgnore]
        public static string FileName => ZX2200PathConfig.CarrierRepository;

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
        private CarrierConfigRepository()
        {
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static CarrierConfigRepository GetInstance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new CarrierConfigRepository();
                }

                return instance;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dsSetting">数据对象</param>
        public override void Remove(BaseCarrierConfig dsSetting)
        {
            this.BaseDsSettingList.Remove(dsSetting);
            PREntity.DeletePREntity(dsSetting.Name + "DieMatch");
            //PREntity.DeletePREntity(dsSetting.Name + "DieMatchP2");
            PREntity.DeletePREntity(dsSetting.Name + "DieBlob");
            PREntity.DeletePREntity(dsSetting.Name + "DieFrame");
            PREntity.DeletePREntity(dsSetting.Name + "DownLookMatch1");
            PREntity.DeletePREntity(dsSetting.Name + "DownLookMatch2");
            PREntity.DeletePREntity(dsSetting.Name + "UpLookMatch1");
            PREntity.DeletePREntity(dsSetting.Name + "UpLookMatch2");

            VisionEntityRepository.GetInstance().PRVisionList.Remove(it => it.GetName() == dsSetting.Name + "DieMatch");
            VisionEntityRepository.GetInstance().PRVisionList.Remove(it => it.GetName() == dsSetting.Name + "DieMatchP2");
            VisionEntityRepository.GetInstance().PRVisionList.Remove(it => it.GetName() == dsSetting.Name + "DieBlob");
            VisionEntityRepository.GetInstance().PRVisionList.Remove(it => it.GetName() == dsSetting.Name + "DieFrame");
            VisionEntityRepository.GetInstance().PRVisionList.Remove(it => it.GetName() == dsSetting.Name + "DownLookMatch1");
            VisionEntityRepository.GetInstance().PRVisionList.Remove(it => it.GetName() == dsSetting.Name + "DownLookMatch2");
            VisionEntityRepository.GetInstance().PRVisionList.Remove(it => it.GetName() == dsSetting.Name + "UpLookMatch1");
            VisionEntityRepository.GetInstance().PRVisionList.Remove(it => it.GetName() == dsSetting.Name + "UpLookMatch2");

            VisionEntityRepository.GetInstance().PRVisionList.Remove(it => it.GetName() == dsSetting.Name + "DipFluxMatch");
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dsSetting">数据对象</param>
        public override void Add(BaseCarrierConfig dsSetting)
        {
            List<string> prNames = new List<string>();

            // 在PR库里面找到所有包涵旧配方的名称
            List<BaseVisionEntity> baseVisionEntities = VisionEntityRepository.GetInstance().PRVisionList;

            List<string> oldPRNameList = new List<string>();

            oldPRNameList.Add(dsSetting.CopyName + "DieMatch");
            oldPRNameList.Add(dsSetting.CopyName + "DieMatchP2");
            oldPRNameList.Add(dsSetting.CopyName + "DieBlob");
            oldPRNameList.Add(dsSetting.CopyName + "DieFrame");
            oldPRNameList.Add(dsSetting.CopyName + "DownLookMatch1");
            oldPRNameList.Add(dsSetting.CopyName + "DownLookMatch2");
            oldPRNameList.Add(dsSetting.CopyName + "UpLookMatch1");
            oldPRNameList.Add(dsSetting.CopyName + "UpLookMatch2");
            oldPRNameList.Add(dsSetting.CopyName + "DipFluxMatch");

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
