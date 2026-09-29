using System.Collections.Generic;
using System.Drawing;
using AKRS.Galaxy2.PR.Models.MatchResults;
using VM.PlatformSDKCS;

namespace AKRS.Galaxy2.PR.Models.CommonModels
{
    /// <summary>
    /// 结果显示模型
    /// </summary>
    public class PRResultShowModel
    {
        /// <summary>
        /// PR 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// PR图片
        /// </summary>
        public Bitmap PRImage { get; set; }

        /// <summary>
        /// PR结果
        /// </summary>
        public List<BaseAlgResult> PRResults { get; set; }

        /// <summary>
        /// 耗时
        /// </summary>
        public long Times { get; set; }

        /// <summary>
        /// 相机名称
        /// </summary>
        public string CameraName { get; set; }

        /// <summary>
        /// 算法类型
        /// </summary>
        public AlgFlowTypeEnum AlgFlowType { get; set; }
    }
}
