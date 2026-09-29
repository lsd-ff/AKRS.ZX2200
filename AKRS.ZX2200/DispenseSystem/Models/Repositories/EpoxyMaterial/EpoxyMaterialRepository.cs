using AKRS.Galaxy2.MachineSupport;
using AKRS.ZX2200.Models;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.DispenseSystem.Models.Repositories.EpoxyMaterial
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    /// <summary>
    /// 胶水类型的仓库
    /// </summary>
    public class EpoxyMaterialRepository : BaseSettingRepository<EpoxyMaterial>
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static EpoxyMaterialRepository instance;

        /// <summary>
        /// 存储文件名称
        /// </summary>
        [JsonIgnore]
        public static string FileName => ZX2200PathConfig.EpoxyMaterialRepositoryPath;

        /// <summary>
        /// 获取文件存储名称
        /// </summary>
        /// <returns>名称</returns>
        protected override string GetFileName()
        {
            return FileName;
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static EpoxyMaterialRepository GetInstance()
        {
            
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new EpoxyMaterialRepository();
                }
                return instance;
            }
        }
    }
}
