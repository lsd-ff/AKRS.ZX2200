#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/11/29 15:38:41
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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Log;
using log4net.Core;

namespace AKRS.ZX2200.TransportSystem.Models
{
    /// <summary>
    /// 描述：运行时环境  不存储
    /// </summary>
    public static class TransportProvider 
    {
        /// <summary>
        /// 持续上料  true:持续上料  false：不再上料
        /// </summary>
        public static bool ContinuousFeeding { get; set; } = true;

        /// <summary>
        /// 准备一片料
        /// </summary>
        public static bool PrepareFeeding { get; set; } = true;

        /// <summary>
        /// 流道计时器
        /// </summary>
        public static Stopwatch TransportSystemStopwatch { get; set; } = new Stopwatch();


        /// <summary>
        /// 时间记录
        /// </summary>
        /// <param name="item">动作信息</param>
        /// <param name="subItem">动作信息</param>
        public static void RecordTime(string item, string subItem)
        {
            LogHelper.Post(
                Level.Info,
                $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.fff")}: {item} - {subItem} 耗时：{TransportSystemStopwatch.ElapsedMilliseconds} ms",
                LogCategory.Transport,
                ViewType.InFileAndUI);

            TransportSystemStopwatch.Restart();
        }
    }
}
