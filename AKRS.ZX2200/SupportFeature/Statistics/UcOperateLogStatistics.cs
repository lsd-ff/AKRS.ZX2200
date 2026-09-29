using System;
using System.Collections.Generic;

namespace AKRS.ZX2200.SupportFeature.Statistics
{
    using AKRS.Galaxy2.UserManager.Bll;
    using AKRS.Galaxy2.UserManager.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using DevExpress.XtraEditors;
    using System.Windows.Forms;

    /// <summary>
    /// 操作记录统计
    /// </summary>
    public partial class UcOperateLogStatistics : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 操作记录统计
        /// </summary>
        public UcOperateLogStatistics()
        {
            this.InitializeComponent();
            this.gridView1.Columns[0].DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            List<OperateLogEntity> list = StatisticsDomain.GetInstance().QueryOperateLog(
                this.DoStartTime.DateTimeOffset.DateTime,
                this.DoEndTime.DateTimeOffset.DateTime);

            if (!string.IsNullOrEmpty(this.CbxOperate.SelectedItem?.ToString()))
            {
                list = list.FindAll(it => it.Technician == this.CbxOperate.SelectedItem.ToString());
            }

            this.gridControl1.DataSource = list;
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void UcOperateLogStatistics_Load(object sender, EventArgs e)
        {
            DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            this.DoStartTime.EditValue = dateTime;
            this.DoEndTime.EditValue = dateTime.AddDays(1);

            UserService userService = new UserService();
            List<User> list = userService.QueryAll();
            this.CbxOperate.Properties.Items.Add(string.Empty);
            foreach (User user in list)
            {
                this.CbxOperate.Properties.Items.Add(user.Name);
            }

            this.Init();
        }

        /// <summary>
        /// 开始时间发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void DoStartTime_EditValueChanged(object sender, EventArgs e)
        {
          // this.Init();
        }

        /// <summary>
        /// 结束时间发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void DoEndTime_EditValueChanged(object sender, EventArgs e)
        {
           // this.Init();
        }

        /// <summary>
        /// 操作人员发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CbxOperate_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.Init();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtQuery_Click(object sender, EventArgs e)
        {
            this.Init();
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
            fileDialog.Filter = "Excel文件(*.xlsx)|*.xlsx";
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
