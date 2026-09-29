using System;
using System.Collections.Generic;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.Galaxy2.PR.Models.Entities;
using LanguageExt;
using log4net.Core;
using Newtonsoft.Json;

namespace AKRS.Galaxy2.PR.Resipository
{
    using System.IO;
    using System.Linq;

    /// <summary>
    /// 视觉实体仓库
    /// </summary>
    public class VisionEntityRepository
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static VisionEntityRepository instance;

        /// <summary>
        /// 单例锁
        /// </summary>
        [JsonIgnore]
        private static volatile object locker = new object();

        /// <summary>
        /// 私有化
        /// </summary>
        private VisionEntityRepository()
        {
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static VisionEntityRepository GetInstance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = JsonFormatHelper<VisionEntityRepository>.ReadGenericObject(PathConfig.TemplateConfigRepositoryPath);
                    if(instance != null) 
                    {
                        for (int i = 0; i < instance.PRVisionList.Count; i++)
                        {
                            BaseVisionEntity p = instance.PRVisionList[i];
                        }
                    }                                       
                }
                if (instance == null)
                {
                    instance = new VisionEntityRepository(); 
                }
                return instance;
            }
        }

        /// <summary>
        /// PR列表
        /// </summary>
        public List<BaseVisionEntity> PRVisionList { get; set; } = new List<BaseVisionEntity>();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="visionEntity">PR实体</param>
        public void AddVisionEntity(BaseVisionEntity visionEntity)
        {
            BaseVisionEntity baseVisionEntity =
                this.PRVisionList.Find(item => item.GetName() == visionEntity.GetName());

            if (baseVisionEntity != null)
            {
                this.PRVisionList.Remove(baseVisionEntity);
            }

            this.PRVisionList.Add(visionEntity);
        }

        /// <summary>
        /// 去除重复的实体对象
        /// </summary>
        public void RemoveDuplicatesEntity()
        {
            List<BaseVisionEntity> rRVisionListNew =
                this.PRVisionList.GroupBy(it => it.GetName()).FirstOrDefault().ToList();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="visionEntity">PR模版配置</param>
        public void RemoveVisionEntity(BaseVisionEntity visionEntity)
        {
            this.PRVisionList.Remove(visionEntity);
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save() 
        {
            try
            {
                JsonFormatHelper<VisionEntityRepository>.SaveGenericObject(this, PathConfig.TemplateConfigRepositoryPath);

                if (File.Exists(PathConfig.TemplateConfigRepositoryPath))
                {
                    if (File.Exists(PathConfig.DeviceDirPath + "\\备份TemplateConfigRepository.json"))
                    {
                        File.Delete(PathConfig.DeviceDirPath + "\\备份TemplateConfigRepository.json");
                    }
                    File.Copy(PathConfig.TemplateConfigRepositoryPath, PathConfig.DeviceDirPath + "\\备份TemplateConfigRepository.json");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"PRJson异常：{ex.Message} ", ex, Log.LogCategory.PR);
            }
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="visionName">视觉实体名称</param>
        /// <param name="prSavePath">视觉实体保存路径</param>
        /// <returns>视觉实体</returns>
        public BaseVisionEntity Find(string visionName)
        {
            BaseVisionEntity visionEntity = this.PRVisionList.Find(pr => pr.GetName() == visionName );
            return visionEntity;
        }
    }
}
