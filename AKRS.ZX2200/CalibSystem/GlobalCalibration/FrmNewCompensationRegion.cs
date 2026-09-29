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
using AKRS.Galaxy2.Infrastructure.ControlServices;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Infrastructure.Controls.Currency;

namespace AKRS.ZX2200.CalibSystem.GlobalCalibration;

/// <summary>
/// 新建补偿区域窗体
/// </summary>
public partial class FrmNewCompensationRegion : XtraForm
{
    /// <summary>
    /// 补偿器
    /// </summary>
    private readonly BilinearCompensator compensator;

    /// <summary>
    /// 区域名称
    /// </summary>
    public string RegionName { get; set; } = "新区域";

    /// <summary>
    /// 优先级
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 构造函数
    /// </summary>
    public FrmNewCompensationRegion(BilinearCompensator compensator)
    {
        this.InitializeComponent();
        this.compensator = compensator;
    }

    /// <summary>
    /// 确认按钮点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnConfirm_Click(object sender, EventArgs e)
    {
        if (this.compensator.Regions.Exists(r=>r.RegionName == this.RegionName))
        {
            AKRSXtraMessageBox.Show($"当前已存在名称为[{this.RegionName}]的区域，请重新输入", "提示", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            this.TxtRegionName.SelectAll();
            this.TxtRegionName.Focus();
            return;
        }

        this.DialogResult = DialogResult.OK;
    }

    /// <summary>
    /// 窗体加载事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void FrmNewCompensationRegion_Load(object sender, EventArgs e)
    {
        this.TxtRegionName.Binding(this, nameof(this.RegionName));
        this.SpPriority.Binding(this, nameof(this.Priority));
    }
}