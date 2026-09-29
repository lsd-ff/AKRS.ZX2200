using AKRS.ZX2200.Infrastructure.Controls.Currency;
using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.Experiment.AccuracyExperiment
{
    /// <summary>
    /// 精度结果
    /// </summary>
    public partial class FrmAccuracyExperimentResult : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 精度结果
        /// </summary>
        public FrmAccuracyExperimentResult()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 数据表
        /// </summary>
        private readonly DataTable dataTable;

        /// <summary>
        /// 精度结果
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="name">名称</param>
        /// <param name="standard">结果</param>
        public FrmAccuracyExperimentResult(List<List<double>> data, List<string> name, List<double> standard)
        {
            this.InitializeComponent();

            this.dataTable = new DataTable("精度数据");
            for (int i = 0; i < name.Count; i++)
            {
                this.dataTable.Columns.Add(name[i]);
            }

            for (int i = 0; i < data.Count; i++)
            {
                object[] ob = new object[data[i].Count];
                for (int j = 0; j < data[i].Count; j++)
                {
                    ob[j] = data[i][j];
                }

                this.dataTable.Rows.Add(ob);
            }

            this.gridControl1.DataSource = this.dataTable;

            this.labelControl1.Text = string.Empty;

            for (int i = 0; i < standard.Count; i++)
            {
                double value = this.GetMax(name[i]) - this.GetMin(name[i]);

                this.labelControl1.Text += $"{name[i]}: 极差为{value}。  标准为{standard[i]}" + "\n";
            }
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>结果</returns>
        private double GetMax(string name)
        {
            double maxValue = double.MinValue;
            foreach (DataRow row in this.dataTable.Rows)
            {
                object value = row[name];
                if (double.TryParse(value.ToString(), out double currentValue))
                {
                    if (currentValue > maxValue)
                    {
                        maxValue = currentValue;
                    }
                }
            }

            return maxValue;
        }

        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>结果</returns>
        private double GetMin(string name)
        {
            double minValue = double.MaxValue;
            foreach (DataRow row in this.dataTable.Rows)
            {
                object value = row[name];
                if (double.TryParse(value.ToString(), out double currentValue))
                {
                    if (currentValue < minValue)
                    {
                        minValue = currentValue;
                    }
                }
            }

            return minValue;
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtExportData_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "导出Excel";
            fileDialog.Filter = "Excel文件(*.xls)|*.xlsx";
            DialogResult dialogResult = fileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                try
                {
                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                    this.gridView1.ExportToXls(fileDialog.FileName);
                    AKRSXtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("正由另一进程使用"))
                    {
                        AKRSXtraMessageBox.Show("数据导出失败！文件正由另一个程序占用！", "提示");
                    }
                    else
                    {
                       AKRSXtraMessageBox.Show("数据导出失败", "提示");
                    }
                }
            }
        }
    }
}