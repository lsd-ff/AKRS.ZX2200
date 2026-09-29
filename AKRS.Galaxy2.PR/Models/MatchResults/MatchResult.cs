#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/4 20:12:41
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
using VM.PlatformSDKCS;

namespace AKRS.Galaxy2.PR.Models.MatchResults
{
    /// <summary>
    /// 描述：
    /// </summary>
    [Serializable]
    public class MatchResult : BaseAlgResult
    {
        /// <summary>
        /// 结果相加
        /// </summary>
        /// <param name="matchResult1">定位结果1</param>
        /// <param name="matchResult2">定位结果2</param>
        /// <returns>结果</returns>
        public static MatchResult operator +(MatchResult matchResult1, MatchResult matchResult2)
        {
            return new MatchResult(
                matchResult1.CenterX + matchResult2.CenterX,
                matchResult1.CenterY + matchResult2.CenterY,
                matchResult1.Angle + matchResult2.Angle);
        }

        /// <summary>
        /// 结果相减
        /// </summary>
        /// <param name="matchResult1">定位结果1</param>
        /// <param name="matchResult2">定位结果2</param>
        /// <returns>结果</returns>
        public static MatchResult operator -(MatchResult matchResult1, MatchResult matchResult2)
        {
            return new MatchResult(
                matchResult1.CenterX - matchResult2.CenterX,
                matchResult1.CenterY - matchResult2.CenterY,
                matchResult1.Angle - matchResult2.Angle);
        }

        /// <summary>
        /// 结果相减
        /// </summary>
        /// <param name="matchResult1">定位结果1</param>
        /// <param name="number">除数</param>
        /// <returns>结果</returns>
        public static MatchResult operator /(MatchResult matchResult1, double number)
        {
            return new MatchResult(
                matchResult1.CenterX / number,
                matchResult1.CenterY / number,
                matchResult1.Angle / number);
        }

        /// <summary>
        /// 无参构造
        /// 默认是相机中心
        /// </summary>
        public MatchResult()
        {
            this.CenterX = 1224;
            this.CenterY = 1024;
            this.Angle = 0;
        }

        public MatchResult(double centerX, double centerY, double angle)
        {
            this.CenterX = centerX;
            this.CenterY = centerY;
            this.Angle = angle;
        }

        /// <summary>
        /// 模板搜索X结果
        /// </summary>
        public double CenterX { get; set; }

        /// <summary>
        /// 模板搜索Y结果
        /// </summary>
        public double CenterY { get; set; }

        /// <summary>
        /// 模板搜索角度(角度制)
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// 模板搜索得到的分数
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// 矩形高
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// 矩形宽
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// 是否有缺陷
        /// </summary>
        public bool IsBreak { get; set; }

        /// <summary>
        /// 缺陷数量
        /// </summary>
        public int BreakNum { get; set; }
    }
}
