#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2024/11/15 16:02:42
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
using AKRS.Galaxy2.Log;
using log4net.Core;
using Newtonsoft.Json.Serialization;

namespace AKRS.ZX2200.Infrastructure.Models.CommonModels
{
    /// <summary>
    /// 描述：
    /// </summary>
    public class NLogTraceWriter : ITraceWriter
    {
        public TraceLevel LevelFilter
        {
            // trace all messages. nlog can handle filtering
            get { return TraceLevel.Verbose; }
        }

        public void Trace(TraceLevel level, string message, Exception ex)
        {
            LogHelper.Post(Level.Error, $"Json 序列化异常：{message}", ex, LogCategory.MainSoftWare);
        }
    }
}
