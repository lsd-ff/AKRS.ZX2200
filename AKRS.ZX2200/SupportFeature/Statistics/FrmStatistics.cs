using System;

namespace AKRS.ZX2200.Product.Statistics
{
    using AKRS.Galaxy2.Component.Simple.MessageBox;
    using AKRS.Galaxy2.MachineSupport;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.Infrastructure.Controls.Common;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.SupportFeature.Compensate.TemperatureCompensate;
    using AKRS.ZX2200.SupportFeature.Statistics;
    using DevExpress.ExpressApp.Utils;
    using DevExpress.Office.Utils;
    using DevExpress.XtraEditors;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;

    /// <summary>
    /// 统计
    /// </summary>
    public partial class FrmStatistics : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 统计服务
        /// </summary>
        private StatisticsService statisticsService = new StatisticsService();

        /// <summary>
        /// 统计
        /// </summary>
        public FrmStatistics()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 统计
        /// </summary>
        public StatisticsDomain StatisticsDomain => StatisticsDomain.GetInstance();

        /// <summary>
        /// 统计加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmStatistics_Load(object sender, EventArgs e)
        {
            this.RefreshData();
            this.xtraTabPage5.Controls.Add(new UcCapacityStatistics() { Dock = DockStyle.Fill });
            this.xtraTabPage6.Controls.Add(new UcDefectStatistics() { Dock = DockStyle.Fill });
            this.TpAlarm.Controls.Add(new UcAlarmHistory() { Dock = DockStyle.Fill });
            this.TpTemData.Controls.Add(new UcTemperatureCompensation() { Dock = DockStyle.Fill });
            this.xtraTabPage2.Controls.Add(new UcOperateLogStatistics() { Dock = DockStyle.Fill });
            this.xtraTabPage4.Controls.Add(new UcParameterChangeLogStatistics() { Dock = DockStyle.Fill });
            this.xtraTabPage7.Controls.Add(new UcBondPositionInfo() { Dock = DockStyle.Fill });

            this.DoStartTime.EditValue = DateTime.Now.AddDays(-1);
            this.DoEndTime.EditValue = DateTime.Now;
            this.QueryData();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshData()
        {
            this.LbCurrentRecipe1.Text = this.StatisticsDomain.CurrentRecipeName;
            this.LbCurrentRecipe2.Text = this.StatisticsDomain.CurrentRecipeName;
            // this.labelControl17.Text = this.StatisticsDomain.CurrentRecipeName;
            this.labelControl17.Text = "未知";
            this.labelControl48.Text = "未知";
            this.LbProductionTime.Text = "未知";

            this.LbDispenseNumber.Text = this.StatisticsDomain.AllDispensedNumber.ToString();
            this.LbDispnseSingleCycle.Text = this.StatisticsDomain.DispenseSingleCycle.ToString("F3");

            this.LbBondNumber.Text = this.StatisticsDomain.AllBondedNumber.ToString();
            this.LbBondSingle.Text = this.StatisticsDomain.BondSingleCycle.ToString("F3");
            this.LbDispenseS2Cycle.Text = this.StatisticsDomain.S2DispenseSingleCycle.ToString("F3");


            this.LbAbandonCount.Text = this.StatisticsDomain.AbandonNumber.ToString();
            this.LbPickUpFailedCount.Text = this.StatisticsDomain.PickUpFailedNumber.ToString();
            this.LbAdjustFailedCount.Text = this.StatisticsDomain.AdjustFailedCount.ToString();
            this.LbBondFailedCount.Text = this.StatisticsDomain.BondFailedCount.ToString();
            this.LbPostBondFailedCount.Text = this.StatisticsDomain.PostBondFailedCount.ToString();

            this.LbUphEpoxy.Text = this.StatisticsDomain.DispenseUph.ToString("F0");
            this.LbUphBond.Text = this.StatisticsDomain.BondUph.ToString("F0");
        }

        /// <summary>
        /// 扫描
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void StatisticsTimer_Tick(object sender, EventArgs e)
        {
            this.RefreshData();
        }

        /// <summary>
        /// 初始化系统1
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtInitDispense_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = AKRSXtraMessageBox.Show("是否确定重置，重置则统计数据归零", "提示", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                return;
            }

            this.StatisticsDomain.DispenseSingleCycle = 0;
            this.StatisticsDomain.AllDispensedNumber = 0;
            this.RefreshData();
        }

        /// <summary>
        /// 初始化系统2
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtInitBod_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = AKRSXtraMessageBox.Show("是否确定重置，重置则统计数据归零", "提示", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                return;
            }

            this.StatisticsDomain.AllBondedNumber = 0;
            this.StatisticsDomain.BondSingleCycle = 0;
            this.StatisticsDomain.S2DispenseSingleCycle = 0;
            this.StatisticsDomain.AbandonNumber = 0;
            this.StatisticsDomain.PickUpFailedNumber = 0;
            this.StatisticsDomain.AdjustFailedCount = 0;
            this.StatisticsDomain.BondFailedCount = 0;
            this.StatisticsDomain.PostBondFailedCount = 0;
            this.RefreshData();
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmStatistics_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatisticsTimer.Stop();
            this.StatisticsTimer.Tick -= this.StatisticsTimer_Tick;
            this.StatisticsTimer.Dispose();
            this.StatisticsTimer = null;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtQuery_Click(object sender, EventArgs e)
        {
            this.QueryData();
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        private void QueryData()
        {
            try
            {
                DateTime dateTime = DateTime.Now;

                // 开始时间必须小于结束时间
                if (this.DoStartTime.DateTimeOffset.DateTime > this.DoEndTime.DateTimeOffset.DateTime)
                {
                    AKRSXtraMessageBox.Show("开始时间必须小于结束，请重新输入");
                }

                List<ParameterChangeLogEntity> parameterChangeLogEntities = StatisticsDomain.QueryParameterChangeLog(this.DoStartTime.DateTimeOffset.DateTime, this.DoEndTime.DateTimeOffset.DateTime).OrderBy(it => it.DateTime).ToList();
                List<AlarmLogEntity> AlarmLogEntities = StatisticsDomain.QueryAlarmLog(this.DoStartTime.DateTimeOffset.DateTime, this.DoEndTime.DateTimeOffset.DateTime).OrderBy(it => it.StartTime).ToList();
                List<BondPositionLogEntity> bondPositionLogEntities = StatisticsDomain.QueryBondPositionLog(this.DoStartTime.DateTimeOffset.DateTime, this.DoEndTime.DateTimeOffset.DateTime).OrderBy(it => it.DateTime).ToList();

                #region 自动运行生产时间

                // 计算自动生时间
                List<ParameterChangeLogEntity> MachineState = parameterChangeLogEntities.FindAll(it => it.ParameterName == "设备状态").OrderBy(it => it.DateTime).ToList();

                double productTime = 0;
                DateTime productDateTime = DateTime.Now;
                for (int i = 0; i < MachineState.Count; i++)
                {
                    if (MachineState[i].Message == "设备状态 : 状态值由  停止 ===>  工作中")
                    {
                        productDateTime = MachineState[i].DateTime;

                        if (i == MachineState.Count - 1 && !Machine.GetInstance().IsStop())
                        {
                            // 如果是最后一次的话，计算到显示的时间
                            productTime += (DateTime.Now - productDateTime).TotalHours;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (MachineState[i].Message == "设备状态 : 状态值由  工作中 ===>  停止")
                    {
                        productTime += (MachineState[i].DateTime - productDateTime).TotalHours;
                    }
                }

                this.labelControl58.Text = productTime.ToString("F2") + " 小时";

                #endregion

                #region 最长无报警时间

                if (AlarmLogEntities != null)
                {
                    if (AlarmLogEntities.Count >= 2)
                    {
                        List<double> times = new List<double>();

                        // 计算相邻报警之间的最大时间间隔
                        for (int i = 0; i < AlarmLogEntities.Count - 1; i++)
                        {
                            times.Add((AlarmLogEntities[i + 1].StartTime - AlarmLogEntities[i].StartTime).TotalHours);
                        }

                        this.LbMTBA.Text = times.Max().ToString("F2") + " 小时";

                    }

                    else if (AlarmLogEntities.Count == 1)
                    {
                        double beforeHours = (AlarmLogEntities[0].StartTime - this.DoStartTime.DateTimeOffset.DateTime).TotalHours;
                        double after = (this.DoEndTime.DateTimeOffset.DateTime - AlarmLogEntities[0].StartTime).TotalHours;

                        this.LbMTBA.Text = Math.Max(beforeHours, after).ToString("F2") + " 小时";
                    }
                    else
                    {
                        this.LbMTBA.Text = (this.DoEndTime.DateTimeOffset.DateTime - this.DoStartTime.DateTimeOffset.DateTime).TotalHours.ToString("F2") + " 小时";
                    }
                }
               

                #endregion

                #region 一小时最高生产数量 未开发


                // 计算任意 1 小时内的最高产量
                int maxPerHour = 0;
                if (bondPositionLogEntities != null && bondPositionLogEntities.Count > 0)
                {
                    int left = 0;
                    for (int right = 0; right < bondPositionLogEntities.Count; right++)
                    {
                        // 保证窗口时间跨度不超过 1 小时（含边界）
                        while (left <= right && (bondPositionLogEntities[right].DateTime - bondPositionLogEntities[left].DateTime).TotalHours > 1.0)
                        {
                            left++;
                        }

                        int windowCount = right - left + 1;
                        if (windowCount > maxPerHour)
                        {
                            maxPerHour = windowCount;
                        }
                    }
                }

                // 结果用法示例：显示到界面或记录日志
                this.labelControl20.Text = maxPerHour.ToString() + " 颗"; // 替换为实际的 label 控件

                #endregion

                #region 总报警次数

                this.labelControl22.Text = AlarmLogEntities.Count.ToString() + " 次";

                #endregion

                #region 解决报警平均时间

                this.labelControl59.Text = AlarmLogEntities.Count == 0 ? "0" : AlarmLogEntities.Average(it => (it.HandleTime - it.StartTime).TotalSeconds).ToString("F2") + " 秒";

                #endregion

                #region 参数更改次数最多名称

                this.labelControl30.Text = parameterChangeLogEntities.GroupBy(it => it.ParameterName).OrderByDescending(it => it.Count()).FirstOrDefault()?.Key ?? "--";

                #endregion

                #region 最多报警类型

                this.labelControl24.Text = AlarmLogEntities.GroupBy(it => it.Category).OrderByDescending(it => it.Count()).FirstOrDefault()?.Key ?? "--";

                #endregion

                #region 胶量检测失败个数

                this.labelControl31.Text = AlarmLogEntities.FindAll(it => it.Message == "胶量检测失败").Count.ToString();

                #endregion

                #region 焊后检测失败个数

                this.labelControl33.Text = AlarmLogEntities.FindAll(it => it.Category == "焊后定位失败").Count.ToString();

                #endregion

                #region 基板识别失败次数

                this.labelControl36.Text = AlarmLogEntities.FindAll(it => it.Category == "基板识别失败").Count.ToString();

                #endregion

                #region 胶厚检测失败个数

                this.labelControl49.Text = AlarmLogEntities.FindAll(it => it.Category == "胶厚检测失败").Count.ToString();

                #endregion

                #region 框架二维码识别失败个数

                this.labelControl51.Text = AlarmLogEntities.FindAll(it => it.Category == "框架二维码识别失败").Count.ToString();

                #endregion

                #region 上视识别失败个数

                this.labelControl55.Text = AlarmLogEntities.FindAll(it => it.Category == "上视识别失败").Count.ToString();

                #endregion


                #region 生产框架总数

                this.labelControl42.Text = bondPositionLogEntities.Select(it => it.TransportUnitName).Distinct().Count().ToString();

                #endregion

                #region 生产基板总个数

                // 计算基板总个数 = 生产日志中基板名称和框架名称的不相同的总数（去重）

                this.labelControl18.Text = bondPositionLogEntities.Select(it => new { it.TransportUnitName, it.SubstrateName }).Distinct().Count().ToString();

                #endregion

                #region 生产基岛总个数

                // 计算基岛总个数 = 生产日志中基岛名称和基板名称和框架名称的不相同的总数（去重）

                this.labelControl43.Text = bondPositionLogEntities.Select(it => new
                {
                    it.TransportUnitName,
                    it.SubstrateName,
                    it.ModuleName
                }).Distinct().Count().ToString();

                #endregion

                #region 生产焊点个数

                this.labelControl44.Text = bondPositionLogEntities.Count.ToString();

                #endregion
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show("查询失败");
            }
        }
    }
}