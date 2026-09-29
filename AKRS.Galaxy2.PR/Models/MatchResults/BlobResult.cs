using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.PR.Models.MatchResults;

namespace AKRS.Galaxy2.PR.Models.MatchResults
{
    /// <summary>
    /// 胶量、墨点检测结果
    /// </summary>
    public class BlobResult : BaseAlgResult
    {
        /// <summary>
        /// 检测X结果
        /// </summary>
        public double CenterX { get; set; }

        /// <summary>
        /// 检测Y结果
        /// </summary>
        public double CenterY { get; set; }

        /// <summary>
        /// 检测面积
        /// </summary>
        public double Area { get; set; }

        /// <summary>
        /// 检测总面积
        /// </summary>
        public double TotalArea { get; set; }

        /// <summary>
        /// 检测数量
        /// </summary>
        public int Num { get; set; }
    }
}
