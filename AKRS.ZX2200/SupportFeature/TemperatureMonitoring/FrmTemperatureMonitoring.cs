using AKRS.Galaxy2.Component.Simple.MessageBox;
using AKRS.Galaxy2.LogicHardware.HardWares.TemperatureControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Utils;
using DevExpress.Charts.Native;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.SupportFeature.TemperatureMonitoring
{
    /// <summary>
    /// 温度监控
    /// </summary>
    public partial class FrmTemperatureMonitoring : XtraForm
    {

        /// <summary>
        /// 热电偶温度数据集合1
        /// </summary>
        private List<List<TemperatureData>> temperatureData = new List<List<TemperatureData>>();

        /// <summary>
        /// 温度监控加热器集合
        /// </summary>
        private List<Heater> heater = new List<Heater>();

        /// <summary>
        /// 热电偶名称集合
        /// </summary>
        private List<string> heaterName = new List<string>() { "焊头背板热电偶", "X右3热电偶", "X右2热电偶", "X右1热电偶"/*, "加热器5"*/ };

        /// <summary>
        /// 是否开始采集
        /// </summary>
        private bool isStart => this.BtStart.Text == "停止采集";
        
        private Task task;

        /// <summary>
        /// 温度监控
        /// </summary>
        public FrmTemperatureMonitoring()
        {
            this.InitializeComponent();
        }

        public int time = 0;

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmTemperatureMonitoring_Load(object sender, EventArgs e)
        {
            foreach (string name in heaterName)
            {
                Heater heater = HardwareRepositoryService.GetHardware<Heater>(name);

                if (heater != null)
                {
                    this.heater.Add(heater);
                    this.temperatureData.Add(new List<TemperatureData>());
                }
            }
            
            //// 设置X轴的坐标为毫秒
            //XYDiagram diagram = (XYDiagram)this.chartControl1.Diagram;
            //diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Minute;

            this.task = new Task(() => { this.GetTemperatureData(); });
            this.task.Start();
        }

        /// <summary>
        /// 刷新时间
        /// </summary>
        private void ReFresheData()
        {
            if (!this.IsDisposed)
            {
                //this.chartControl1.Series.Clear();

                //for (int i = 0; i < this.heater.Count; i++)
                //{
                //    DevExpress.XtraCharts.Series series = new Series("温度", ViewType.Line);
                //    for (int j = 0; j < this.temperatureData[i].Count; j++)
                //    {
                //        series.Points.Add(new SeriesPoint(/*this.temperatureData[i][j].Timestamp*/j, this.temperatureData[i][j].Temperature));
                //    }
                //    this.chartControl1.Series.Add(series);
                //}

                //this.Invoke(this.chartControl1.Refresh);
            }

        }

        private BondModule bondModule = new BondModule();

        private BondHead bondHead = new BondHead();

        /// <summary>
        /// 定时器
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void GetTemperatureData()
        {
            while (true)
            {
                Thread.Sleep((int)this.SpTimes.Value);

                if (!this.isStart)
                {
                    continue;
                }

                List<TemperatureData> listData = new List<TemperatureData>();

                DateTime Timestamp = DateTime.Now;

                

                // 采集数据
                for (int i = 0; i < this.heater.Count; i++)
                {
                    TemperatureData data = new TemperatureData()
                    {
                        TemperatureName = this.heater[i].HardwareName,
                        Timestamp = Timestamp,
                        Temperature = this.heater[i].GetTemperature()
                    };

                    Thread.Sleep(100);

                    listData.Add(data);

                    if (this.temperatureData[i].Count > 100)
                    {
                        this.temperatureData[i].RemoveAt(0);
                    }

                    this.temperatureData[i].Add(data);
                }

                listData.Add(new TemperatureData() { 
                    TemperatureName = bondModule.BondAxisX.HardwareName,
                    Timestamp = Timestamp,
                    Temperature = bondModule.BondAxisX.ReadTemperature()
                });

                listData.Add(new TemperatureData()
                {
                    TemperatureName = bondModule.BondAxisY.HardwareName,
                    Timestamp = Timestamp,
                    Temperature = bondModule.BondAxisY.ReadTemperature()
                });

                listData.Add(new TemperatureData()
                {
                    TemperatureName = bondHead.AxisZ.HardwareName,
                    Timestamp = Timestamp,
                    Temperature = bondHead.AxisZ.ReadTemperature()
                });

                if (this.IsDisposed)
                {
                    return;
                }

                // 刷新图表
                this.ReFresheData();

                // 保存数据
                this.SaveData(listData);
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="listData">结果</param>
        private void SaveData(List<TemperatureData> listData)
        {
            try
            {
                // 组织数据
                List<List<object>> data = new List<List<object>>();

                List<object> list = new List<object>();
                for (int i = 0; i < listData.Count; i++)
                {
                    // list.Add(listData[i].TemperatureName);
                    list.Add(listData[i].Timestamp);
                    list.Add(listData[i].Temperature);
                }

                data.Add(list);
                string path = "D://热电偶检测数据.xlsx";

                if (!File.Exists(path))
                {
                    File.Create(path);
                }

                FileHelper.AppendExcelData(path, data);
            }
            catch (Exception ex)
            {
            
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmTemperatureMonitoring_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        /// <summary>
        /// 开始采集
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtStart_Click(object sender, EventArgs e)
        {
            if (this.BtStart.Text == "开始采集")
            {
                this.BtStart.Text = "停止采集";
            }
            else
            {
                this.BtStart.Text = "开始采集";
            }
        }

        /// <summary>
        /// 更改采集频率
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void SpTimes_EditValueChanged(object sender, EventArgs e)
        {
          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmTemperatureMonitoring_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.isStart)
            {
                AKRSXtraMessageBox.Show("请先关闭采集数据");
                e.Cancel = true;
                return;
            }
        }
    }

    /// <summary>
    /// 温度数据
    /// </summary>
    public class TemperatureData
    {
        /// <summary>
        /// 热电偶的名称
        /// </summary>
        public string TemperatureName { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 温度值
        /// </summary>
        public double Temperature { get; set; }
    }
}
