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
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;

namespace AKRS.ZX2200.CalibSystem.GlobalCalibration;

/// <summary>
/// 补偿区域映射表窗体
/// </summary>
public partial class FrmCompensationRegionView : XtraForm
{
    private RegionCompensator region;

    public FrmCompensationRegionView(RegionCompensator region)
    {
        this.InitializeComponent();
        this.region = region;
    }

    private void FrmCompensationRegionView_Load(object sender, EventArgs e)
    {
        this.Text = $"补偿区域映射表 - {this.region.RegionName}";
        var compensationMap = this.region.Region.CompensationMap
            .Select(pair => new { AxisPos = pair.Key.ToString(), Compensation = pair.Value.ToString() }).ToList();
        this.GcCompensationRegion.DataSource = compensationMap;

    }
}