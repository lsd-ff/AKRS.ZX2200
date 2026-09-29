#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/18 15:51:01
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.Bondinsert;
using Newtonsoft.Json;

namespace AKRS.ZX2200.TransportSystem.Models.DatasetModels.TansportBelt
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    /// <summary>
    /// 描述： TransportBelt 皮带设置数据集（库）
    /// </summary>
    public class TransportBeltSettingRepository : BaseSettingRepository<TransportBeltSetting>
    { 
        /// <summary>
       /// 单例
       /// </summary>
        private static TransportBeltSettingRepository instance;

        /// <summary>
        /// 存储文件名称
        /// </summary>
        [JsonIgnore]
        public static string FileName => ZX2200PathConfig.TransportBeltSettingRepositoryFilePath;

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
        private TransportBeltSettingRepository()
        {
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static TransportBeltSettingRepository GetInstance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new TransportBeltSettingRepository();
                }

                return instance;
            }
        }
    }
}
