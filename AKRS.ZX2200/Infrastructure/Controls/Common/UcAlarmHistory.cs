using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Infrastructure.Service;

namespace AKRS.ZX2200.Infrastructure.Controls.Common;

/// <summary>
/// 报警历史查询
/// </summary>
public partial class UcAlarmHistory : XtraUserControl
{
    /// <summary>
    /// constructor
    /// </summary>
    public UcAlarmHistory()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// load event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UcAlarmHistory_Load(object sender, EventArgs e)
    {
        DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        this.DoStartTime.EditValue = dateTime;
        this.DoEndTime.EditValue = dateTime.AddDays(1);

        this.Init();
    }

    private void Init()
    {
        DateTime startTime = this.DoStartTime.DateTimeOffset.DateTime;
        DateTime endTime = this.DoEndTime.DateTimeOffset.DateTime;

        List<AlarmLogEntity> list = DBService.Query<AlarmLogEntity>(
            it => it.StartTime >= startTime && it.StartTime <= endTime);

        this.gridControl1.DataSource = list;
        this.RefreshAlarmTypeCount(list);
    }

    private void RefreshAlarmTypeCount(List<AlarmLogEntity> list)
    {
        List<AlarmTypeCount> listAlarmType = new List<AlarmTypeCount>();

        for (int i = 0; i < list.Count; i++)
        {
            AlarmTypeCount alarmTypeCount = listAlarmType.Find(item => item.AlarmType == list[i].Category);

            if (alarmTypeCount == null)
            {
                alarmTypeCount = new AlarmTypeCount() { AlarmType = list[i].Category, AlarmCount = 0 };
                listAlarmType.Add(alarmTypeCount);
            }

            alarmTypeCount.AlarmCount++;
        }

        this.gridControl2.DataSource = listAlarmType;
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
        fileDialog.Filter = "Excel文件(*.xls)|*.xlsx";
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

public class AlarmTypeCount
{
    public string AlarmType { get; set; }

    public int AlarmCount { get; set; }
}
