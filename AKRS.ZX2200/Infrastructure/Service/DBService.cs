using AKRS.AccessControl;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace AKRS.ZX2200.Infrastructure.Service;

using AKRS.Galaxy2.Log;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.SupportFeature.Statistics;
using DevExpress.XtraEditors;
using log4net.Core;
using System.Data.SqlClient;
using System.Management.Instrumentation;

/// <summary>
/// 数据库服务
/// </summary>
public class DBService
{
    /// <summary>
    /// 单例
    /// </summary>
    private static readonly Lazy<DBService> instance =
        new(() => new());

    /// <summary>
    /// 数据库客户端
    /// </summary>
    private SqlSugarClient db;

    // /// <summary>
    // /// 锁
    // /// </summary>
    // private static object locker = new object();

    /// <summary>
    /// 连接字符串
    /// </summary>
    private const string ConnectionString = //"Data Source = localhost;Initial Catalog = ZX2200;User Id = sa;Password = sa123456"; 
    "Server=localhost;Database=ZX2200;Trusted_Connection=True;";

    /// <summary>
    /// Constructor
    /// </summary>
    private DBService()
    {
        this.db = new(new ConnectionConfig()
        {
            ConnectionString = ConnectionString,
            DbType = DbType.SqlServer,
            IsAutoCloseConnection = true
        });

        // 初始化报警记录表
        this.CreateTableIfNotExists<AlarmLogEntity>();
        this.CreateTableIfNotExists<CapacityLogEntity>();
        this.CreateTableIfNotExists<DefectStatisticsEntity>();
        this.CreateTableIfNotExists<ProductTimeStatisticsEntity>();
        this.CreateTableIfNotExists<TemperatureCompensationEntity>();
        this.CreateTableIfNotExists<OperateLogEntity>();
        this.CreateTableIfNotExists<ParameterChangeLogEntity>();
        this.CreateTableIfNotExists<BondPositionLogEntity>();
        this.CleanInfo();
    }

    /// <summary>
    /// 单例
    /// </summary>
    public static DBService Instance => instance.Value;

    /// <summary>
    /// 数据库存储周期
    /// </summary>
    private int SqlSaveDue => MachineSoftwareConfiguration.GetInstance().SqlSaveDue;

    /// <summary>
    /// 写入
    /// </summary>
    /// <param name="entity">记录实体</param>
    public static void Insert<T>(T entity) where T : class, new()
    {
        try
        {
            using SqlSugarClient db = Instance.db.CopyNew();
            db.Insertable(entity).ExecuteCommand();
        }
        catch (Exception ex)
        {
            LogHelper.Post(Level.Info,
                    $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.f")}:数据库写入出现异常 + 类型{typeof(T)} + {ex.Message}"
                    , LogCategory.Global, ViewType.InFileAndUI);
        }
    }

    /// <summary>
    /// 条件查询
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static List<T> Query<T>(Expression<Func<T, bool>> condition)
    {
        try
        {
            using SqlSugarClient db = Instance.db.CopyNew();
            return db.Queryable<T>().Where(condition).ToList();
        }
        catch (Exception ex)
        {
            LogHelper.Post(Level.Info,
                      $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.f")}:数据库查询出现异常 + 类型{typeof(T)} + {ex.Message}"
                      , LogCategory.Global, ViewType.InFileAndUI);
            return new List<T>();
        }
    }

    /// <summary>
    /// 按表查询
    /// </summary>
    /// <returns></returns>
    public static List<T> Query<T>()
    {
        try
        {
            using SqlSugarClient db = Instance.db.CopyNew();
            return db.Queryable<T>().ToList();
            // return Instance.db.Queryable<T>().ToList();
        }
        catch (Exception ex)
        {
            LogHelper.Post(Level.Info,
                       $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.f")}:数据库查询出现异常 + 类型{typeof(T)} + {ex.Message}"
                       , LogCategory.Global, ViewType.InFileAndUI);
            return new List<T>();
        }
    }

    /// <summary>
    /// 检查表是否存在，如果不存在则自动创建
    /// </summary>
    private void CreateTableIfNotExists<T>()
    {
        try
        {
            this.db.CodeFirst.InitTables<T>();
        }
        catch (Exception ex)
        {
            LogHelper.Post(Level.Info,
                       $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.f")}:数据库建表出现异常 + 类型{typeof(T)} + {ex.Message}"
                       , LogCategory.Global, ViewType.InFileAndUI);
        }
    }

    /// <summary>
    /// 清理超过180天的报警信息
    /// </summary>
    private void CleanInfo()
    {
        this.ClearOldAlarms();
        this.ClearCapacityLogEntity();
        this.ClearDefectStatisticsEntity();
        this.ClearProductTimeStatisticsEntity();
        this.ClearTemperatureCompensationEntity();
        this.ClearOperateLogEntity();
        this.ClearParameterChangeLogEntity();
        this.ClearSingBondLogEntity();
    }

    /// <summary>
    /// 清楚焊点信息
    /// </summary>
    private void ClearSingBondLogEntity()
    {
        try
        {
            DateTime dateLimit = DateTime.Now.AddDays(-SqlSaveDue);

            int affectedRows = this.db.Deleteable<BondPositionLogEntity>()
                .Where(parameterChangeLogEntity => parameterChangeLogEntity.DateTime < dateLimit)
                .ExecuteCommand();
        }
        catch (Exception ex)
        {
            LogHelper.Post(Level.Info,
                      $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.f")}:数据库删除出现异常 + 类型{typeof(BondPositionLogEntity)} + {ex.Message}"
                      , LogCategory.Global, ViewType.InFileAndUI);
        }
    }

    /// <summary>
    /// 清除旧的参数变更日志
    /// </summary>
    private void ClearParameterChangeLogEntity()
    {
        try
        {
            DateTime dateLimit = DateTime.Now.AddDays(-SqlSaveDue);

            int affectedRows = this.db.Deleteable<ParameterChangeLogEntity>()
                .Where(parameterChangeLogEntity => parameterChangeLogEntity.DateTime < dateLimit)
                .ExecuteCommand();
        }
        catch (Exception ex)
        {
            LogHelper.Post(Level.Info,
                      $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.f")}:数据库删除出现异常 + 类型{typeof(ParameterChangeLogEntity)} + {ex.Message}"
                      , LogCategory.Global, ViewType.InFileAndUI);
        }
    }

    /// <summary>
    /// 清除旧的操作日志
    /// </summary>
    private void ClearOperateLogEntity()
    {
        try
        {
            DateTime dateLimit = DateTime.Now.AddDays(-SqlSaveDue);

            int affectedRows = this.db.Deleteable<OperateLogEntity>()
                .Where(operateLogEntity => operateLogEntity.DateTime < dateLimit)
                .ExecuteCommand();
        }
        catch (Exception ex)
        {
            LogHelper.Post(Level.Info,
                      $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.f")}:数据库删除出现异常 + 类型{typeof(OperateLogEntity)} + {ex.Message}"
                      , LogCategory.Global, ViewType.InFileAndUI);
        }
    }

    /// <summary>
    /// 清除就得温度补偿信息
    /// </summary>    
    private void ClearTemperatureCompensationEntity()
    {
        try
        {
            DateTime dateLimit = DateTime.Now.AddDays(-SqlSaveDue);

            int affectedRows = this.db.Deleteable<TemperatureCompensationEntity>()
                .Where(temperatureCompensationEntity => temperatureCompensationEntity.DateTime < dateLimit)
                .ExecuteCommand();
        }
        catch (Exception ex)
        {
            LogHelper.Post(Level.Info,
                       $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.f")}:数据库删除出现异常 + 类型{typeof(TemperatureCompensationEntity)} + {ex.Message}"
                       , LogCategory.Global, ViewType.InFileAndUI);
        }
    }

    /// <summary>
    /// 清理旧的制造信息信息
    /// </summary>
    private void ClearProductTimeStatisticsEntity()
    {
        try
        {
            DateTime dateLimit = DateTime.Now.AddDays(-SqlSaveDue);

            int affectedRows = this.db.Deleteable<ProductTimeStatisticsEntity>()
                .Where(capacityLogEntity => capacityLogEntity.StartTime < dateLimit)
                .ExecuteCommand();
        }
        catch (Exception ex)
        {
            LogHelper.Post(Level.Info,
                        $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.f")}:数据库删除出现异常 + 类型{typeof(ProductTimeStatisticsEntity)} + {ex.Message}"
                        , LogCategory.Global, ViewType.InFileAndUI);
        }
    }

    /// <summary>
    /// 清理旧的精度信息
    /// </summary>
    private void ClearDefectStatisticsEntity()
    {
        try
        {
            DateTime dateLimit = DateTime.Now.AddDays(-SqlSaveDue);

            int affectedRows = this.db.Deleteable<DefectStatisticsEntity>()
                .Where(capacityLogEntity => capacityLogEntity.DateTime < dateLimit)
                .ExecuteCommand();
        }
        catch (Exception ex)
        {
            LogHelper.Post(Level.Info,
                        $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.f")}:数据库删除出现异常 + 类型{typeof(DefectStatisticsEntity)} + {ex.Message}"
                        , LogCategory.Global, ViewType.InFileAndUI);
        }
    }

    /// <summary>
    /// 清理旧的产能信息
    /// </summary>
    private void ClearCapacityLogEntity()
    {
        try
        {
            DateTime dateLimit = DateTime.Now.AddDays(-SqlSaveDue);

            int affectedRows = this.db.Deleteable<CapacityLogEntity>()
                .Where(capacityLogEntity => capacityLogEntity.StartTime < dateLimit)
                .ExecuteCommand();
        }
        catch (Exception ex)
        {
            LogHelper.Post(Level.Info,
                         $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.f")}:数据库删除出现异常 + 类型{typeof(CapacityLogEntity)} + {ex.Message}"
                         , LogCategory.Global, ViewType.InFileAndUI);
        }
    }

    /// <summary>
    /// 清理旧的报警信息
    /// </summary>
    private void ClearOldAlarms()
    {
        try
        {
            DateTime dateLimit = DateTime.Now.AddDays(-SqlSaveDue);

            int affectedRows = this.db.Deleteable<AlarmLogEntity>()
                .Where(alarm => alarm.StartTime < dateLimit)
                .ExecuteCommand();

           // Console.WriteLine($"报警数据库删除了 {affectedRows} 条过期报警记录。");
        }
        catch (Exception ex)
        {
            LogHelper.Post(Level.Info,
                         $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.f")}:数据库删除出现异常 + 类型{typeof(AlarmLogEntity)} + {ex.Message}"
                         , LogCategory.Global, ViewType.InFileAndUI);
        }
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="conditionUpdate">更新内容</param>
    /// <param name="conditionUpFind">寻找条件</param>
    public static void UpDataData<T>(Expression<Func<T, bool>> conditionUpdate, Expression<Func<T, bool>> conditionUpFind) where T : class, new()
    {
        try
        {
            using SqlSugarClient db = Instance.db.CopyNew();
            db.Updateable(conditionUpdate).Where(conditionUpFind).ExecuteCommand();
        }
        catch (Exception ex)
        {
            LogHelper.Post(Level.Info,
                        $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.f")}:数据库更新出现异常 + 类型{typeof(T)} + {ex.Message}"
                        , LogCategory.Global, ViewType.InFileAndUI);
        }
    }

    /// <summary>
    /// 获取首行首列
    /// </summary>
    /// <param name="queryStr">sql语句</param>
    /// <returns>结果对象</returns>
    public static object GetScalar(string queryStr)
    {
        using SqlSugarClient db = Instance.db.CopyNew();
        return null;

        return db.Ado.GetScalar(queryStr);
    }
}