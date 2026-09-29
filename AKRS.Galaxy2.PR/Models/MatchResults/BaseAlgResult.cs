#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/4 20:11:22
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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PlatformSDKCS;

namespace AKRS.Galaxy2.PR.Models.MatchResults
{
    using Newtonsoft.Json;

    /// <summary>
    /// 描述：算法结果基类
    /// </summary>
    [Serializable]
    public abstract class BaseAlgResult
    {
        /// <summary>
        /// 算法是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 算法输出图像
        /// </summary>
        [JsonIgnore]
        public ImageBaseData OutPutImg { get; set; }
        /// <summary>
        /// 算法输出图像
        /// </summary>
        [JsonIgnore]
        public Bitmap OutPutImg1 { get; set; }

        /// <summary>
        /// PR提示语
        /// </summary>
        public string MarkedWords { get; set; }
    }
}
