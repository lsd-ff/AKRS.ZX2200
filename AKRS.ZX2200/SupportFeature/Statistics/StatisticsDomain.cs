namespace AKRS.ZX2200.SupportFeature.Statistics
{
    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.ZX2200.BondSystem.Controls.Manual;
    using AKRS.ZX2200.BondSystem.Controls.Setting.PostBond;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.SupportFeature.Compensate.DefectCompensate;
    using AKRS.ZX2200.SupportFeature.Compensate.TemperatureCompensate;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using Newtonsoft.Json;
    using PostSharp.Aspects.Advices;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;

    /// <summary>
    /// 生产统计类
    /// </summary>
    public class StatisticsDomain : Singleton<StatisticsDomain>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static StatisticsDomain()
        {
            StatisticsDomain.FilePath = ZX2200PathConfig.StatisticsDomainPath;
        }

        /// <summary>
        /// 白班开始时间
        /// </summary>
        public DateTime DayStart { get; set; }

        /// <summary>
        /// 当前程式的名称
        /// </summary>
        public string CurrentRecipeName => MachineConfigContext.GetInstance().RecipeName;

        /// <summary>
        /// 点胶的总数
        /// </summary>
        public int AllDispensedNumber { get; set; }

        /// <summary>
        /// 单颗点胶的周期
        /// </summary>
        public double DispenseSingleCycle { get; set; }

        #region 固晶统计

        /// <summary>
        /// 当前程式贴片的总数
        /// </summary>
        public int AllBondedNumber { get; set; }

        /// <summary>
        /// 单颗固晶的周期
        /// </summary>
        public double BondSingleCycle { get; set; }

        /// <summary>
        /// 单颗固晶的周期
        /// </summary>
        public double S2DispenseSingleCycle { get; set; }

        /// <summary>
        /// 抛的数量
        /// </summary>
        public int AbandonNumber { get; set; }

        /// <summary>
        /// 取料失败的次数
        /// </summary>
        public int PickUpFailedNumber { get; set; }

        /// <summary>
        /// 矫正失败的数量
        /// </summary>
        public int AdjustFailedCount { get; set; }

        /// <summary>
        /// 贴片失败的数量
        /// </summary>
        public int BondFailedCount { get; set; }

        /// <summary>
        /// 焊后失败的数量
        /// </summary>
        public int PostBondFailedCount { get; set; }

        #endregion

        /// <summary>
        /// BondUPH
        /// </summary>
        public double BondUph { get; set; } = 0;

        /// <summary>
        /// 点胶UPH
        /// </summary>
        public double DispenseUph { get; set; } = 0;

        #region 计算参数

        /// <summary>
        /// Bond单颗结束时间
        /// </summary>
        private DateTime BondEndTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Bond单颗结束时间
        /// </summary>
        private DateTime S2DispenseEndTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 点胶单颗结束结束时间
        /// </summary>
        private DateTime DispenseEndTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Bond开始贴片时间
        /// </summary>
        private DateTime BondStartTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 点胶开始的时间
        /// </summary>
        private DateTime DispenseStartTime { get; set; }

        /// <summary>
        /// 计算BondUPH的数量
        /// </summary>
        private int bondUphCount = 0;

        /// <summary>
        /// 计算BondUPH的数量
        /// </summary>
        private int dispenseUphCount = 0;

        #endregion

        /// <summary>
        /// 保存焊点信息并统计固晶周期
        /// </summary>
        /// <param name="bondPosition">焊点信息</param>
        /// <param name="component">芯片信息</param>
        public void StatisticsBondPositionInfo(BondPosition bondPosition, BaseCarrierConfig component)
        {
            System2RunTimeProvider.RecordTime("BondAction", "统计固精周期 开始");

            this.bondUphCount++;
            this.AllBondedNumber++;

            if (bondPosition == null || component == null)
            {
                return;
            }

            // 保存到数据库
            this.SaveDataToDb(new BondPositionLogEntity("", bondPosition.SubstrateNum.ToString(), bondPosition.ModuleNum.ToString(), bondPosition.Name)
            {
                ComponentName = component.Name,
                ComponentPickPressure = component.PickupForceMode == BondSystem.Models.Enums.ForceModeEnum.Force ? component.PickupForce : 0,
                ComponentBondPressure = component.BondingForceMode == BondSystem.Models.Enums.ForceModeEnum.Force ? component.BondingForce : 0,
                ComponentBeforePickSecondHeight = component.IsActivateSlowTravelBeforePickup ? component.SlowTravelDistanceBeforePickup : 0,
                ComponentBeforeBondSecondHeight = component.IsActivateSlowTravelBeforeBonding ? component.SlowTravelDistanceBeforeBonding : 0,
                ComponentBeforePickSecondSpeed = component.PickupForceMode == BondSystem.Models.Enums.ForceModeEnum.Distance ? component.SlowTravelSpeedBeforePickup : 0,
                ComponentBeforeBondSecondSpeed = component.BondingForceMode == BondSystem.Models.Enums.ForceModeEnum.Distance ? component.SlowTravelSpeedBeforeBonding : 0,
                ComponentPickDelay = component.PickupDelay,
                ComponentBondDelay = component.PlacementDelay,
                ComponentAfterPickSecondHeight = component.IsActivateSlowTravelAfterPickup ? component.SlowTravelDistanceAfterPickup : 0,
                ComponentAfterBondSecondHeight = component.IsActivateSlowTravelAfterBonding ? component.SlowTravelDistanceAfterBonding : 0,
                ComponentAfterPickSecondSpeed = component.PickupForceMode == BondSystem.Models.Enums.ForceModeEnum.Distance ? component.SlowTravelSpeedAfterPickup : 0,
                ComponentAfterBondSecondSpeed = component.BondingForceMode == BondSystem.Models.Enums.ForceModeEnum.Distance ? component.SlowTravelSpeedAfterBonding : 0,

                ComponentGlueDelay = component.DipDelay,
                ComponentGluePressure = component.DipFluxForce,
                ComponentBeforeGlueSecondSpeed = component.PickupForceMode == BondSystem.Models.Enums.ForceModeEnum.Distance ? component.SlowTravelSpeedBeforeDipFlux : 0,
                ComponentBeforeGlueSecondDistance = component.IsActivateSlowTravelBeforeDip ? component.SlowTravelDistanceBeforeDipFlux : 0,
                ComponentAfterGlueSecondSpeed = component.PickupForceMode == BondSystem.Models.Enums.ForceModeEnum.Distance ? component.SlowTravelSpeedBeforeDipFlux : 0,
                ComponentAfterGlueSecondDistance = component.IsActivateSlowTravelBeforeDip ? component.SlowTravelDistanceBeforeDipFlux : 0,
            });

            DateTime bonded = DateTime.Now;

            if ((bonded - this.BondEndTime).Seconds > 0 && (bonded - this.BondEndTime).Seconds < 10)
            {
                this.BondSingleCycle = (bonded - this.BondEndTime).TotalMilliseconds / 1000;
            }

            this.BondEndTime = DateTime.Now;

            System2RunTimeProvider.RecordTime("BondAction", "统计固精周期 结束");
        }

        /// <summary>
        /// 刷新固晶周期
        /// </summary>
        public void RefreshS2DispenseStatistics()
        {
            DateTime dispensed = DateTime.Now;

            if ((dispensed - this.S2DispenseEndTime).Seconds >= 0 && (dispensed - this.S2DispenseEndTime).Seconds < 10)
            {
                this.S2DispenseSingleCycle = (dispensed - this.S2DispenseEndTime).TotalMilliseconds / 1000;
            }

            this.S2DispenseEndTime = DateTime.Now;
        }

        /// <summary>
        /// 刷新点胶周期
        /// </summary>
        public void RefreshDispenseStatistics()
        {
            DateTime dispensed = DateTime.Now;

            this.AllDispensedNumber++;
            this.dispenseUphCount++;

            if ((dispensed - this.DispenseEndTime).Seconds > 0 && (dispensed - this.DispenseEndTime).Seconds < 10)
            {
                this.DispenseSingleCycle = (dispensed - this.DispenseEndTime).TotalMilliseconds / 1000;
            }

            this.DispenseEndTime = DateTime.Now;
        }

        /// <summary>
        /// 计算点胶UPH
        /// </summary>
        public void CalculateDispenseCycle()
        {
            // 如果是第一次进入统计，记录开始时间并返回
            if (this.dispenseUphCount == 0)
            {
                return;
            }

            double useSeconds = (DateTime.Now - this.DispenseStartTime).TotalSeconds;

            // 计算UPH
            if (useSeconds <= 0)
            {
                return;
            }

            double singleTime = this.dispenseUphCount / useSeconds;

            this.DispenseUph = (int)(singleTime * 3600);

            // 清空变量
            this.dispenseUphCount = 0;
            this.DispenseStartTime = DateTime.Now;
            this.Save();
        }

        /// <summary>
        /// 计算BondUPH
        /// </summary>
        public void CalculateBondCycle()
        {
            // 如果是第一次进入统计，记录开始时间并返回
            if (this.bondUphCount == 0)
            {
                return;
            }

            double useSeconds = (DateTime.Now - this.BondStartTime).TotalSeconds;

            // 计算UPH
            if (useSeconds <= 0)
            {
                return;
            }

            double singleTime = this.bondUphCount / useSeconds;

            this.BondUph = (int)(singleTime * 3600);

            // 清空变量
            this.bondUphCount = 0;
            this.BondStartTime = DateTime.Now;
            this.Save();
        }

        #region 数据查询

        /// <summary>
        /// 产能统计查询
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>结果</returns>
        public List<CapacityLogEntity> QueryCapacity(DateTime startTime, DateTime endTime)
        {
            return DBService.Query<CapacityLogEntity>(it => it.StartTime >= startTime && it.StartTime <= endTime);
        }

        /// <summary>
        /// 精度统计查询
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>结果</returns>
        public List<DefectStatisticsEntity> QueryDefect(string name, DateTime startTime, DateTime endTime)
        {
            return DBService.Query<DefectStatisticsEntity>(it => it.DateTime >= startTime && it.DateTime <= endTime && it.DefectName == name);
        }

        /// <summary>
        /// 精度统计查询
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>结果</returns>
        public List<ProductTimeStatisticsEntity> QueryProductTime(DateTime startTime, DateTime endTime)
        {
            return DBService.Query<ProductTimeStatisticsEntity>(
                it => it.StartTime >= startTime && it.EndTime <= endTime
                      || it.StartTime <= startTime && it.EndTime >= startTime
                      || it.StartTime <= endTime && it.EndTime >= endTime
                      || it.StartTime <= startTime && it.EndTime >= endTime);
        }

        /// <summary>
        /// 温飘查询
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>结果</returns>
        public List<TemperatureCompensationEntity> QueryTemperatureCompensationTime(DateTime startTime, DateTime endTime)
        {
            return DBService.Query<TemperatureCompensationEntity>(
                it => it.DateTime >= startTime && it.DateTime <= endTime);
        }

        /// <summary>
        /// 操作日志查询
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>结果</returns>
        public List<OperateLogEntity> QueryOperateLog(DateTime startTime, DateTime endTime)
        {
            return DBService.Query<OperateLogEntity>(it => it.DateTime >= startTime && it.DateTime <= endTime);
        }

        /// <summary>
        /// 报警日志查询
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>结果</returns>
        public List<AlarmLogEntity> QueryAlarmLog(DateTime startTime, DateTime endTime)
        {
            return DBService.Query<AlarmLogEntity>(it => it.StartTime >= startTime && it.StartTime <= endTime);
        }

        /// <summary>
        /// 焊点日志查询
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>结果</returns>
        public List<BondPositionLogEntity> QueryBondPositionLog(DateTime startTime, DateTime endTime)
        {
            return DBService.Query<BondPositionLogEntity>(it => it.DateTime >= startTime && it.DateTime <= endTime);
        }

        /// <summary>
        /// 参数修改日志查询
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>结果</returns>
        public List<ParameterChangeLogEntity> QueryParameterChangeLog(DateTime startTime, DateTime endTime)
        {
            return DBService.Query<ParameterChangeLogEntity>(it => it.DateTime >= startTime && it.DateTime <= endTime);
        }

        #endregion

        /// <summary>
        /// 增加一个检测结果
        /// </summary>
        /// <param name="defectStatisticsEntity">精度统计</param>
        public void AddDefectResult(DefectStatisticsEntity defectStatisticsEntity)
        {
            // 保存到数据库
            this.SaveDataToDb(defectStatisticsEntity);

            DefectCompensate.RePostBondInspection(defectStatisticsEntity);
        }

        /// <summary>
        /// 保存数据到数据库
        /// </summary>
        /// <param name="ob"></param>
        public void SaveDataToDb(object ob)
        {
            StatisticsService.DbEntityBlockingCollection.Add(ob);
        }
    }
}
