using AKRS.ZX2200.Infrastructure.Controls.Currency;
using DevExpress.XtraEditors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.SupportFeature.Statistics
{
    /// <summary>
    /// 测高数据
    /// </summary>
    public partial class FrmMeasureHeightData : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 测高对象
        /// </summary>
        public FrmMeasureHeightData()
        {
            this.InitializeComponent();
            this.gridControl1.DataSource = this.MeasureHeightData;
            AddMeasureHeightData += this.AddData;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        public static Action<MeasureHeightData> AddMeasureHeightData =
            new Action<MeasureHeightData>((MeasureHeightData measureHeightData) => { });

        /// <summary>
        /// 测高基板号
        /// </summary>
        private BindingList<MeasureHeightData> MeasureHeightData { get; set; } = new BindingList<MeasureHeightData>();


        private void AddData(MeasureHeightData measureHeightData)
        {
            this.Invoke(
                () =>
                {
                    if (!this.Created || this.Disposing || this.IsDisposed)
                    {
                        return;
                    }

                    this.MeasureHeightData.Add(measureHeightData);
                    });
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmMeasureHeightData_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "导出Excel";
            fileDialog.Filter = "Excel文件(*.xls)|*.xls";
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

            AddMeasureHeightData -= this.AddData;
            this.MeasureHeightData.Clear();
        }
    }

    /// <summary>
    /// 测高数据
    /// </summary>
    public class MeasureHeightData
    {
        // ToDo 这个应该放在数据库里面

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tuId">框架ID</param>
        /// <param name="substrateIndex">基板序号</param>
        /// <param name="moduleIndex">基岛序号</param>
        /// <param name="bondPositionName">焊点名称</param>
        /// <param name="height">高度</param>
        public MeasureHeightData(string tuId, int substrateIndex, int moduleIndex, string bondPositionName,double height)
        {
            this.TuId = tuId;
            this.SubstrateIndex = substrateIndex;
            this.ModuleIndex = moduleIndex;
            this.BondPositionName = bondPositionName;
            this.Height = height;
        }

        /// <summary>
        /// 框架ID
        /// </summary>
        public string TuId { get; set; }

        /// <summary>
        /// 基板序号
        /// </summary>
        public int SubstrateIndex { get; set; }

        /// <summary>
        /// 基岛序号
        /// </summary>
        public int ModuleIndex { get; set; }

        /// <summary>
        /// 焊点名称
        /// </summary>
        public string BondPositionName { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public double Height { get; set; }
}
}