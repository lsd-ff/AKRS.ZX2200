namespace AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;

    using DevExpress.XtraEditors;

    using static FrmMultipleHeightMeasurementType;

    /// <summary>
    /// 多功能测高
    /// </summary>
    public partial class FrmMeasureHeightPoints : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 多功能测高
        /// </summary>
        public FrmMeasureHeightPoints()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 测高点位集合
        /// </summary>
        public List<AKRSPoint3D> MeasureHeightPoints { get; set; } = new List<AKRSPoint3D>();

        /// <summary>
        /// 结果集合
        /// </summary>
        public List<ResultValue> MeasureHeightResult { get; set; } = new List<ResultValue>();

        /// <summary>
        /// 结果集合
        /// </summary>
        public List<List<double>> ResultList { get; set; } = new List<List<double>>();

        /// <summary>
        /// 结果参数
        /// </summary>
        public List<List<double>> TestList { get; set; } = new List<List<double>>();

        /// <summary>
        /// 测高线程
        /// </summary>
        private Thread thread;

        /// <summary>
        /// 测高类型
        /// </summary>
        public MultipleHeightMeasurementType Type { get; set; }

        /// <summary>
        /// 是否需要刷新
        /// </summary>
        private bool needFresh;

        /// <summary>
        /// 执行测高动作
        /// </summary>
        private void DoWork()
        {
            this.TestList.Add(new List<double>());
            while (this.BtContinuousMeasurement.Checked)
            {
                this.Init();

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < this.MeasureHeightPoints.Count; j++)
                    {
                        // 移动到位置上方5mm，怕撞
                        TUAssistantHelper.MoveToPos(
                            new AKRSPoint3D(
                                this.MeasureHeightPoints[j].X,
                                this.MeasureHeightPoints[j].Y,
                                this.MeasureHeightPoints[j].Z + 3));

                        (ExcuteResult result, double height) result =
                            TUAssistantHelper.AssistantMeasureHeight(MultipleHeightMeasurementType.TouchDown, false);


                        if (result.result == ExcuteResult.Success)
                        {
                            this.ResultList[j].Add(result.height);
                            this.TestList[0].Add(result.height);
                        }
                        else
                        {
                            return;
                        }

                        this.ResultVision();

                        if (!this.BtContinuousMeasurement.Checked)
                        {
                            System1Domain.GetInstance().DispenseMeasureHeightController.CloseDispenseHeightMeasurementCylinder();
                            return;
                        }
                    }
                }


                System1Domain.GetInstance().DispenseMeasureHeightController.CloseDispenseHeightMeasurementCylinder();
                // 10组保存一下数据
                this.SaveData();
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        private void SaveData()
        {
            if (this.TestList.Count > 100)
            {
                string nane = DateTime.Now.ToFileTime().ToString();
                string path = "D:\\点胶测高针测试\\" + nane + ".xlsx";
                FileHelper.SaveDoubleExcel(this.TestList, path);
                this.TestList.Clear();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            this.MeasureHeightResult.Clear();
            for (int i = 0; i < this.MeasureHeightPoints.Count; i++)
            {
                this.MeasureHeightResult.Add(new ResultValue() { Point = i.ToString() });
            }

            this.ResultList.Clear();
            for (int i = 0; i < this.MeasureHeightPoints.Count; i++)
            {
                this.ResultList.Add(new List<double>());
            }
        }

        /// <summary>
        /// 结果显示
        /// </summary>
        public void ResultVision()
        {
            string value = String.Empty;

            for (int i = 0; i < this.ResultList.Count; i++)
            {
                for (int j = 0; j < this.ResultList[i].Count; j++)
                {
                    if (i == 0)
                    {
                        value = this.ResultList[i][j].ToString();
                    }
                    else
                    {
                        value = (this.ResultList[i][j] - this.ResultList[0][j]).ToString();
                    }

                    if (j == 0)
                    {
                        this.MeasureHeightResult[i].Result1 = value;
                    }
                    else if (j == 1)
                    {
                        this.MeasureHeightResult[i].Result2 = value;
                    }
                    else if (j == 2)
                    {
                        this.MeasureHeightResult[i].Result3 = value;
                    }
                    else if (j == 3)
                    {
                        this.MeasureHeightResult[i].Result4 = value;
                    }
                    else if (j == 4)
                    {
                        this.MeasureHeightResult[i].Result5 = value;
                    }
                    else if (j == 5)
                    {
                        this.MeasureHeightResult[i].Result6 = value;
                    }
                    else if (j == 6)
                    {
                        this.MeasureHeightResult[i].Result7 = value;
                    }
                    else if (j == 7)
                    {
                        this.MeasureHeightResult[i].Result8 = value;
                    }
                    else if (j == 8)
                    {
                        this.MeasureHeightResult[i].Result9 = value;
                    }
                    else if (j == 9)
                    {
                        this.MeasureHeightResult[i].Result10 = value;
                    }
                    else if (j == 10)
                    {
                        this.MeasureHeightResult[i].Result2 = value;
                    }
                }
            }
            
            Thread.Sleep(1000);
            this.needFresh = true;
        }


        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.needFresh)
            {
                // 结果展示
                this.GcNozzleShelf.DataSource = this.MeasureHeightResult;

                this.GcNozzleShelf.Refresh();

                this.GvNozzleShelf.RefreshData();

                this.needFresh = false;
            }

            if (this.thread == null || !this.thread.IsAlive)
            {
                this.BtContinuousMeasurement.Checked = false;
            }
        }

        /// <summary>
        /// 内除类，用于存储结果
        /// </summary>
        public class ResultValue
        {
            /// <summary>
            /// 序号
            /// </summary>
            public string Point { get; set; }

            /// <summary>
            /// 结果1
            /// </summary>
            public string Result1 { get; set; }

            /// <summary>
            /// 结果2
            /// </summary>
            public string Result2 { get; set; }

            /// <summary>
            /// 结果3
            /// </summary>
            public string Result3 { get; set; }

            /// <summary>
            /// 结果4
            /// </summary>
            public string Result4 { get; set; }

            /// <summary>
            /// 结果5
            /// </summary>
            public string Result5 { get; set; }

            /// <summary>
            /// 结果6
            /// </summary>
            public string Result6 { get; set; }

            /// <summary>
            /// 结果7
            /// </summary>
            public string Result7 { get; set; }

            /// <summary>
            /// 结果8
            /// </summary>
            public string Result8 { get; set; }

            /// <summary>
            /// 结果9
            /// </summary>
            public string Result9 { get; set; }

            /// <summary>
            /// 结果9
            /// </summary>
            public string Result10 { get; set; }
        }

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">方法</param>
        private void FrmMeasureHeightPoints_Load(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// 开始循环事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtContinuousMeasurement_CheckedChanged(object sender, EventArgs e)
        {
            if (this.BtContinuousMeasurement.Checked)
            {
                if (this.thread == null || !this.thread.IsAlive)
                {
                    this.thread = new Thread(this.DoWork)
                    {
                        IsBackground = true,
                        Name = "ContinuousMeasurement"
                    };
                    this.thread.Start();
                }
                else
                {
                    this.BtContinuousMeasurement.Checked = false;
                    AKRSXtraMessageBox.Show("Height measurement in progress, please wait");
                }
            }
        }

        private void FrmMeasureHeightPoints_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.thread != null && this.thread.IsAlive)
            {
                this.thread.Abort();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
           Sensor sendor =  HardwareRepositoryService.GetHardware<Sensor>("LVDT");
           //int value =  sendor.ReadTxPDO();
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMeasureHeightPoints_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Tick -= timer1_Tick;
            this.timer1.Dispose();
        }
    }
}