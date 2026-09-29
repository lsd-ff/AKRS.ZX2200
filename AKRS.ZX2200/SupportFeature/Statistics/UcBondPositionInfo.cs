using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.SupportFeature.Statistics
{
    /// <summary>
    /// 生产的焊点信息展示控件
    /// </summary>
    public partial class UcBondPositionInfo : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 焊点信息日志实体列表
        /// </summary>
        private List<BondPositionLogEntity> bondPositionLogEntities = new List<BondPositionLogEntity>();

        /// <summary>
        /// 生产的焊点信息展示控件
        /// </summary>
        public UcBondPositionInfo()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void UcBondPositionInfo_Load(object sender, EventArgs e)
        {
            this.DeQurtyStartTime.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            this.DeQurtyEndTime.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);

            // 清除已有列
            this.gridView1.Columns.Clear();

            // 关闭列自动宽度，启用横向滚动条以便列过多时可以水平滚动
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            var type = typeof(BondPositionLogEntity);
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                // 使用 GridView 的 AddField 创建列并设置属性
                var col = this.gridView1.Columns.AddField(prop.Name);
                // 明确绑定 FieldName 为属性名（确保列与数据源属性绑定）
                col.FieldName = prop.Name;
                // 获取 DescriptionAttribute
                var descAttr = prop.GetCustomAttribute<DescriptionAttribute>();
                col.Caption = descAttr != null && !string.IsNullOrWhiteSpace(descAttr.Description)
                    ? descAttr.Description
                    : prop.Name;
                col.Visible = true;
                col.VisibleIndex = this.gridView1.Columns.Count - 1;
            }

            // 绑定数据源
            this.gridControl1.DataSource = this.bondPositionLogEntities;

            // 根据列标题和内容自适应列宽
            // 注意：如果数据在后续才加载，需在数据加载完成后再次调用 BestFitColumns()
            this.gridView1.BestFitColumns();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtQuery_Click(object sender, EventArgs e)
        {
            this.bondPositionLogEntities = StatisticsDomain.GetInstance()
                .QueryBondPositionLog(this.DeQurtyStartTime.DateTimeOffset.DateTime, this.DeQurtyEndTime.DateTimeOffset.DateTime);

            if (!string.IsNullOrEmpty(this.TxName.Text.Trim()))
            {
                this.bondPositionLogEntities = this.bondPositionLogEntities.FindAll(it => it.BondPositionName == this.TxName.Text.Trim());
            }

            this.gridControl1.DataSource = this.bondPositionLogEntities;

            this.gridControl1.RefreshDataSource();
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtExport_Click(object sender, EventArgs e)
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
                    this.gridControl1.ExportToXls(fileDialog.FileName);
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
