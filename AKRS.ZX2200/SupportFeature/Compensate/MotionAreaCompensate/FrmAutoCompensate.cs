using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.Main.Machine.Product;
using AKRS.ZX2200.SupportFeature.Statistics;
using AuthenticatorBG_x86;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AKRS.ZX2200.SupportFeature.Compensate.MotionAreaCompensate
{
    /// <summary>
    /// 采集状态枚举
    /// </summary>
    public enum CollectState
    {
        Idle,        // 待机就绪
        Collecting,  // 采集中
        Calculated,  // 计算完成待启用
        Enabled      // 补偿已生效
    }

    public partial class FrmAutoCompensate : XtraForm
    {
        #region 业务字段
        private CompensateDataService _dataService;
        private int _rowCount;
        private int _colCount;
        private int _pointPerRound;

        private CollectState _currentState;
        private DateTime _collectStartTime;
        private int _targetRounds;
        private int _finishedRounds;
        private int _validRounds;
        private int _invalidRounds;

        private readonly HashSet<string> _processedRoundKeys = new HashSet<string>();
        private readonly List<List<DefectStatisticsEntity>> _roundDataList = new List<List<DefectStatisticsEntity>>();
        private double[,] _calculatedX;
        private double[,] _calculatedY;
        #endregion

        public FrmAutoCompensate()
        {
            InitializeComponent();
        }

        #region 窗体事件
        private void FrmAutoCompensate_Load(object sender, EventArgs e)
        {
            InitBusiness();
        }

        private void FrmAutoCompensate_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_currentState == CollectState.Collecting)
            {
                if (XtraMessageBox.Show("采集中，确定要关闭吗？", "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
                _pollTimer.Stop();
               // MachineRunner.GetInstance().Stop();
            }
        }
        #endregion

        #region 业务初始化
        private void InitBusiness()
        {
            var config = ProductDomain.GetInstance().ProductConfig.SubstrateConfig;
            _rowCount = config.RowCount;
            _colCount = config.ColumnCount;
            _pointPerRound = _rowCount * _colCount;

            // 直接实例化，内部自动复用项目统计查询通道
            _dataService = new CompensateDataService();

            UpdateCollectState(CollectState.Idle);
        }
        #endregion

        #region 按钮事件
        /// <summary>
        /// 开始采集
        /// </summary>
        private void BtnStartCollect_Click(object sender, EventArgs e)
        {
            if (_currentState == CollectState.Collecting) return;

            if (XtraMessageBox.Show(
                "请确认设备已进入稳定运行状态\n是否开始采集并计算补偿？",
                "确认启动",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            // 重置所有数据
            _targetRounds = (int)spinCollectRounds.Value;
            _finishedRounds = 0;
            _validRounds = 0;
            _invalidRounds = 0;
            _processedRoundKeys.Clear();
            _roundDataList.Clear();
            _calculatedX = null;
            _calculatedY = null;
            _collectStartTime = DateTime.Now;

            gcCompensate.DataSource = null;
            UpdateCollectState(CollectState.Collecting);

            // 启动设备
            Machine.GetInstance().Start();

            // 启动轮询
            _pollTimer.Start();
        }

        /// <summary>
        /// 停止采集
        /// </summary>
        private void BtnStopCollect_Click(object sender, EventArgs e)
        {
            if (_currentState != CollectState.Collecting) return;

            _pollTimer.Stop();
            Machine.GetInstance().Stop();
            UpdateCollectState(CollectState.Idle);

            XtraMessageBox.Show(
                $"采集已手动停止\n共完成 {_finishedRounds} 轮，有效 {_validRounds} 轮，废轮 {_invalidRounds} 轮",
                "已停止",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// 启用补偿
        /// </summary>
        private void BtnEnable_Click(object sender, EventArgs e)
        {
            if (_calculatedX == null || _calculatedY == null)
            {
                XtraMessageBox.Show("请先完成采集计算后再启用", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var inst = TransportUnitCompensate.GetInstance();
            inst.TransportUnitCompensateX = _calculatedX;
            inst.TransportUnitCompensateY = _calculatedY;
            inst.Save();

            UpdateCollectState(CollectState.Enabled);
            XtraMessageBox.Show("补偿已启用并生效", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 轮询核心逻辑
        private void PollTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                _pollTimer.Stop();

                // 查询分界点之后的所有轮次
                var roundGroups = _dataService.GetRoundsAfterTime(_collectStartTime);
                _finishedRounds = roundGroups.Count;

                // 处理新增轮次
                foreach (var group in roundGroups)
                {
                    string roundKey = $"{group.TuIndex}_{group.SubIndex}_{group.RoundTime:yyyyMMddHHmmss}";
                    if (_processedRoundKeys.Contains(roundKey))
                        continue;

                    _processedRoundKeys.Add(roundKey);

                    if (group.PointCount == _pointPerRound)
                    {
                        _roundDataList.Add(group.DataList);
                        _validRounds++;
                    }
                    else
                    {
                        _invalidRounds++;
                    }
                }

                // 更新界面
                UpdateProgressUI();

                // 达到目标轮数则完成
                if (_validRounds >= _targetRounds)
                {
                    FinishCollectAndCalculate();
                    return;
                }

                // 防无限补跑：总轮数超过目标2倍则停止
                if (_finishedRounds >= _targetRounds * 2)
                {
                    _pollTimer.Stop();
                    Machine.GetInstance().Stop();
                    UpdateCollectState(CollectState.Idle);
                    XtraMessageBox.Show("废轮过多，已自动停止，请检查设备状态", "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _pollTimer.Start();
            }
            catch (Exception ex)
            {
                _pollTimer.Stop();
                Machine.GetInstance().Stop();
                UpdateCollectState(CollectState.Idle);
                XtraMessageBox.Show($"采集巡检异常：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 采集完成 执行计算
        /// </summary>
        private void FinishCollectAndCalculate()
        {
            try
            {
                _pollTimer.Stop();
                Machine.GetInstance().Stop();

                // 构建[位置, 轮次]矩阵
                int pointCount = _rowCount * _colCount;
                double[,] matX = new double[pointCount, _targetRounds];
                double[,] matY = new double[pointCount, _targetRounds];

                for (int r = 0; r < _targetRounds; r++)
                {
                    var roundData = _roundDataList[r];
                    for (int p = 0; p < pointCount; p++)
                    {
                        matX[p, r] = roundData[p].OffsetX / 1000.0;
                        matY[p, r] = roundData[p].OffsetY / 1000.0;
                    }
                }

                // 执行鲁棒计算
                double alpha = 0.85;
                _calculatedX = CompensateCalculator.Calculate(matX, _rowCount, _colCount, alpha);
                _calculatedY = CompensateCalculator.Calculate(matY, _rowCount, _colCount, alpha);

                // 预览结果
                PreviewCompensateData();
                lblLastCalcTime.Text = $"上次计算时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}";

                // 自动启用判断
                if (chkAutoEnable.Checked)
                {
                    var inst = TransportUnitCompensate.GetInstance();
                    inst.TransportUnitCompensateX = _calculatedX;
                    inst.TransportUnitCompensateY = _calculatedY;
                    inst.Save();
                    UpdateCollectState(CollectState.Enabled);
                    XtraMessageBox.Show($"采集完成！共获得 {_validRounds} 轮有效数据\n补偿已自动启用", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    UpdateCollectState(CollectState.Calculated);
                    XtraMessageBox.Show($"采集完成！共获得 {_validRounds} 轮有效数据\n请确认后点击【启用补偿】", "计算完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                UpdateCollectState(CollectState.Idle);
                XtraMessageBox.Show($"计算失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region UI更新方法
        private void UpdateCollectState(CollectState state)
        {
            _currentState = state;

            switch (state)
            {
                case CollectState.Idle:
                    lblStateText.Text = "状态：待机就绪";
                    lblStateText.ForeColor = Color.Gray;
                    btnStartCollect.Enabled = true;
                    btnStopCollect.Enabled = false;
                    btnEnable.Enabled = false;
                    spinCollectRounds.Enabled = true;
                    //spinAlpha.Enabled = true;
                    progressBar.Value = 0;
                    break;

                case CollectState.Collecting:
                    lblStateText.Text = "状态：采集中...";
                    lblStateText.ForeColor = Color.RoyalBlue;
                    btnStartCollect.Enabled = false;
                    btnStopCollect.Enabled = true;
                    btnEnable.Enabled = false;
                    spinCollectRounds.Enabled = false;
                    //spinAlpha.Enabled = false;
                    break;

                case CollectState.Calculated:
                    lblStateText.Text = "状态：计算完成，待启用";
                    lblStateText.ForeColor = Color.DarkOrange;
                    btnStartCollect.Enabled = true;
                    btnStopCollect.Enabled = false;
                    btnEnable.Enabled = true;
                    spinCollectRounds.Enabled = true;
                    //spinAlpha.Enabled = true;
                    progressBar.Value = 100;
                    break;

                case CollectState.Enabled:
                    lblStateText.Text = "状态：补偿已生效";
                    lblStateText.ForeColor = Color.SeaGreen;
                    btnStartCollect.Enabled = true;
                    btnStopCollect.Enabled = false;
                    btnEnable.Enabled = false;
                    spinCollectRounds.Enabled = true;
                    //spinAlpha.Enabled = true;
                    progressBar.Value = 100;
                    break;
            }
        }

        private void UpdateProgressUI()
        {
            lblRoundInfo.Text = $"已完成：{_finishedRounds}轮 | 有效：{_validRounds}轮 | 废轮：{_invalidRounds}轮";
            int progress = Math.Min(100, (int)(_validRounds * 100.0 / _targetRounds));
            progressBar.Value = progress;
        }

        private void PreviewCompensateData()
        {
            List<AKRSPoint2D> list = new List<AKRSPoint2D>();
            for (int i = 0; i < _rowCount; i++)
                for (int j = 0; j < _colCount; j++)
                    list.Add(new AKRSPoint2D(_calculatedX[i, j], _calculatedY[i, j]));

            gcCompensate.DataSource = list;
            gvCompensate.BestFitColumns();
        }
        #endregion

        #region 表格行号
        private void GvCompensate_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
        #endregion

    }
}
