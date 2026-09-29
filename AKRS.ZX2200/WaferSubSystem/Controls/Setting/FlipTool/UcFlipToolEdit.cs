using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.Infrastructure.Interface;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Setting.FlipTool
{
    using System.Threading;

    using AKRS.ZX2200.WaferSubSystem.Controllers;

    using FlipTool = AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool.FlipTool;

    /// <summary>
    /// 翻转工具编辑页面
    /// </summary>
    public partial class UcFlipToolEdit : DevExpress.XtraEditors.XtraUserControl, IProgrammingControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="flipTool">翻转工具</param>
        public UcFlipToolEdit(FlipTool flipTool)
        {
            InitializeComponent();
            this.flipTool = flipTool;
            this.InitControl();
        }

        /// <summary>
        /// 翻转工具
        /// </summary>
        private FlipTool flipTool;

        /// <summary>
        ///  翻转台控制器
        /// </summary>
        private FlipTableController flipTableController = new FlipTableController();

        /// <summary>
        ///  吸嘴架程式
        /// </summary>
        private NozzleShelfProgram nozzleShelfProgram => BondProgram.GetInstance().NozzleShelfProgram;

        /// <summary>
        ///  焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// 界面初始化
        /// </summary>
        private void InitControl()
        {
            // 绑定吸嘴
            List<string> nozzleList = this.nozzleShelfProgram.GetNozzleNameList();
            this.CmbNozzle.Properties.Items.Clear();
            this.CmbNozzle.Properties.Items.AddRange(nozzleList);
            this.CmbNozzle.SelectedItem = flipTool.MeasureHeightNozzle;

            this.SpBlowProportion.EditValue = this.flipTool.BlowProportion;

            this.SpBlowDelay.EditValue = this.flipTool.BlowDelay;

            this.SpVacuumCheckVal.Value = this.flipTool.ComponentCheckVal;
            this.SpVacuumCheckDelay.Value = this.flipTool.ComponentCheckDelay;
        }

        /// <summary>
        /// 下拉框改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CmbNozzle_SelectedIndexChanged(object sender, EventArgs e)
        {
            flipTool.MeasureHeightNozzle = this.CmbNozzle.Text;
        }


        /// <summary>
        /// 确定
        /// </summary>
        public void Confirm()
        {
            this.flipTool.MeasureHeightNozzle = this.CmbNozzle.Text;
            this.flipTool.BlowProportion = (int)this.SpBlowProportion.Value;
            this.flipTool.BlowDelay = (int)this.SpBlowDelay.Value;

            this.flipTool.ComponentCheckVal = (int)this.SpVacuumCheckVal.Value;
            this.flipTool.ComponentCheckDelay = (int)this.SpVacuumCheckDelay.Value;

            FlipToolRepository.GetInstance().Save();
        }

        /// <summary>
        /// 吹气
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnBlow_Click(object sender, EventArgs e)
        {
            Confirm();

            if (!this.flipTableController.GetFlipTableBlowState())
            {
                // 先关真空电磁阀
                this.flipTableController.CloseFlipTableVacuum();

                // 开吹气
                this.flipTableController.SetBlowProportion(this.flipTool.BlowProportion);
                Thread.Sleep(50);
                this.flipTableController.OpenFlipModuleBlow();
                this.BtnBlow.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                this.flipTableController.CloseFlipModuleBlow();
                this.BtnBlow.Appearance.BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// 获取当前真空模拟量
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtGetCurrentVacuumValue_Click(object sender, EventArgs e)
        {
            this.flipTableController.OpenFlipTableVacuum();
            Thread.Sleep(1000);
            double value = this.flipTableController.ReadVacuumValue();
            this.flipTableController.CloseFlipTableVacuum();
            this.SpVacuumCheckVal.EditValue = value;
            this.Confirm();
        }
    }
}
