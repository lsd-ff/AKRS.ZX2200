using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.ZX2200.Infrastructure.Models.BaseModels;
using AKRS.ZX2200.Infrastructure.Models.Path;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool
{
    /// <summary>
    /// 翻转工具库
    /// </summary>
    public class FlipToolRepository : BaseSettingRepository<FlipTool>
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static FlipToolRepository instance;

        /// <summary>
        /// 存储文件名称
        /// </summary>
        [JsonIgnore]
        public static string FileName => ZX2200PathConfig.FlipToolRepository;

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
        private FlipToolRepository()
        {
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static FlipToolRepository GetInstance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new FlipToolRepository();
                }

                return instance;
            }
        }
    }
}
