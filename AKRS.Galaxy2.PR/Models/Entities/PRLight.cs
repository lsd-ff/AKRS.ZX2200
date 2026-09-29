#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/11 18:17:43
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
using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using Newtonsoft.Json;

namespace AKRS.Galaxy2.PR.Models.Entities
{
    /// <summary>
    /// 描述：
    /// </summary>
    /// <summary>
    /// PR 里的光源
    /// 是一个值对象 用来存储PR里光源的信息
    /// </summary>
    public class PRLight
    {
        /// <summary>
        /// 是否使用
        /// </summary>
        public bool IsUse { get; set; } = true;

        /// <summary>
        /// 光源亮度
        /// </summary>
        public int LightIntensity { get; set; } = 0;

        /// <summary>
        /// 光源ID
        /// </summary>
        public string LightName { get; set; }

        /// <summary>
        /// 光源缓存
        /// </summary>
        [JsonIgnore]
        private Light light;

        /// <summary>
        /// 硬件
        /// </summary>
        [JsonIgnore]
        public Light Light
        {
            get
            {
                if (this.light == null || this.light.HardwareName != LightName)
                {
                    this.light = HardwareRepositoryService.GetHardware<Light>(LightName);
                }

                return this.light;
            }

            set => this.light = value;
        }
    }
}
