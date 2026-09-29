#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2024/12/30 18:58:31
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
using AKRS.ZX2200.Infrastructure.Service;

namespace AKRS.ZX2200.SupportFeature.Statistics
{
    using System.Collections.Concurrent;
    using System.Drawing;
    using System.Reflection;
    using System.Threading;

    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Log;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using DevExpress.Xpo;
    using DevExpress.XtraBars;
    using log4net.Core;
    using SqlSugar;

    /// <summary>
    /// 描述：统计服务
    /// </summary>
    public class StatisticsService
    {
        /// <summary>
        /// 保存数据集合
        /// </summary>
        public static BlockingCollection<object> DbEntityBlockingCollection { get; set; } =
            new BlockingCollection<object>(500);


        /// <summary>
        /// 最后一次的信息
        /// </summary>
        private static string lastMessage;

        /// <summary>
        /// 数据库保存线程
        /// </summary>
        private static Thread thread;

        /// <summary>
        /// 最后操作时间
        /// </summary>
        public static DateTime LastOperaDateTime { get; set; }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="operate">操作员</param>
        public static void SaveEntity(string message, string operate)
        {
            if (message == lastMessage)
            {
                return;
            }

            LogHelper.Post(Level.Trace, message, LogCategory.Global);
            OperateLogEntity operateLogEntity = new OperateLogEntity(DateTime.Now, message, operate);

            StatisticsService.DbEntityBlockingCollection.Add(operateLogEntity);

            lastMessage = message;

            LastOperaDateTime = DateTime.Now;
        }

        /// <summary>
        /// 开启存图线程
        /// </summary>
        public static void StartSaveThread()
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(SaveEntityToDb)
                { Name = "数据库存储线程", IsBackground = true };
                thread.Start();
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public static void SaveEntityToDb()
        {
            while (true)
            {
                var item = DbEntityBlockingCollection.Take();

                if (item is OperateLogEntity operateLogEntity)
                {
                    DBService.Insert(operateLogEntity);
                }
                else if (item is DefectStatisticsEntity defectStatisticsEntity)
                {
                    DBService.Insert(defectStatisticsEntity);
                }
                else if (item is ParameterChangeLogEntity parameterChangeLogEntity)
                {
                    Type type = parameterChangeLogEntity.GetType();

                    var attribute = type.GetCustomAttribute<SugarTable>();

                    if (attribute != null)
                    {
                        DBService.Insert(parameterChangeLogEntity);
                    }
                }
                else if (item is BondPositionLogEntity bondPositionLogEntity)
                {
                    DBService.Insert(bondPositionLogEntity);
                }
            }
        }

        public int GetMaxMTBA()
        {
            string sqlstr =
                @"select isnull(MTBA, 0) as MaxMTBA from (select Max(datediff(mi,LastEndTime, StartTime)) as MTBA 
                    from( 
                          select StartTime, LAG(Endtime, 1,null) over(order by StartTime) as LastEndTime from (
                          select StartTime, HandleTime as EndTime from AlarmLog where Category not in ('上料仓报警','下料仓报警','Loader Warning','更换料片') 
                                 and StartTime between  DateAdd(hh, -12, getdate()) and  getdate() union all 
                              select LAG(Endtime, 1,null)  over(order by StartTime), StartTime from [dbo].[ProductTimeStatisticsEntity] 
                              where StartTime between  DateAdd(hh, -12, getdate()) and  getdate())t  where t.StartTime is not null and t.EndTime is not null)s ) M 
                              where MTBA > 0  and MTBA < 180";

            object o = DBService.GetScalar(sqlstr);

            if (o == null)
            {
                return 0;
            }

            return (int)DBService.GetScalar(sqlstr);
        }

        /// <summary>
        /// 获取报警的总秒数
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>报警的总秒数</returns>
        public int GetTotalAlarmSeconds(string beginTime, string endTime)
        {
            string sqlstr =
                @"select isnull(sum(datediff(ss, StartTime, HandleTime)), 0) as totalAlarmSec
                    from [dbo].[AlarmLog] where Category in ('焊后定位失败',
                    '上视定位报警',
                    '焊后结果超阈值',
                    '搜晶失败报警',
                    '更换料片',
                    '取片报警') and StartTime between '" + beginTime + "' and '" + endTime + @"'
                    and HandleTime is not null and StartTime is not null 
                    and HandleTime > StartTime ";

            return 0;

            return (int)DBService.GetScalar(sqlstr);
        }
    }
}
