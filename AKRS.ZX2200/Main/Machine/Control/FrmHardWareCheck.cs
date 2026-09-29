using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.TransportSystem.Models;
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

namespace AKRS.ZX2200.Main.Machine.Control
{
    using AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Models.BondLevelMeasure;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using DevExpress.XtraExport.Helpers;
    using DevExpress.XtraGrid.Views.Grid;

    /// <summary>
    /// 硬件检测
    /// </summary>
    public partial class FrmHardWareCheck : XtraForm
    {
        /// <summary>
        /// 硬件检查
        /// </summary>
        public FrmHardWareCheck()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 配置文件的集合
        /// </summary>
        private List<HardWareCheck> hardWareChecks = new List<HardWareCheck>();

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void HardWareCheck_Load(object sender, EventArgs e)
        {
            this.Init();
            this.gridControl1.DataSource = hardWareChecks;

            // 设置选中行不覆盖背景色
            this.GvHardWareCheck.Appearance.SelectedRow.BackColor = Color.Transparent;
            this.GvHardWareCheck.Appearance.SelectedRow.BackColor2 = Color.Transparent;
            this.GvHardWareCheck.Appearance.SelectedRow.ForeColor = Color.Black; // 保持文字颜色
        }

        /// <summary>
        /// 初始化硬件配置
        /// </summary>
        private void Init()
        {
            List<string> list = new List<string>();

            // 点胶
            list.AddRange(System1Domain.GetInstance().DispenseController.GetHardWareNames());

            // 固精
            list.AddRange(System2Domain.GetInstance().System2Controller.GetHardWareNames());

            // 设备
            list.AddRange(MachineSupport.Machine.GetInstance().GetHardWareNames());

            // 流道
            list.AddRange(TransportDomain.GetInstance().TransportController.GetHardWareNames());

            // 晶圆
            list.AddRange(WaferSubController.GetInstance().GetHardWareNames());

            foreach (string name in list)
            {
                this.hardWareChecks.Add(new HardWareCheck(name));
            }
        }

        /// <summary>
        /// 绘制行
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void GvHardWareCheck_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.RowHandle >= 0)
            {
                // 获取当前行的数据
                HardWareCheck hardWareCheck = this.GvHardWareCheck.GetRow(e.RowHandle) as HardWareCheck;

                if (hardWareCheck != null)
                {
                    if (hardWareCheck.IsInit == false || hardWareCheck.IsExist == false) 
                    {
                        e.Appearance.BackColor = Color.Yellow;
                    }
                }
            }
        }
    }
}
