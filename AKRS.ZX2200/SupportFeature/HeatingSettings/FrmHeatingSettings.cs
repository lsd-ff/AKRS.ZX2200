using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.TransportSystem.Controllers;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportSystem.Models.Programs;
using AKRS.ZX2200.TransportSystem.Modules;
using DevExpress.XtraEditors;
using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.Infrastructure.HeatingSettings
{
    public partial class FrmHeatingSettings : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 固晶载具
        /// </summary>
        private BondSubSectionController BondSubSectionController => TransportDomain.GetInstance().TransportController.BondSubSectionController;

        /// <summary>
        /// 加热设置
        /// </summary>
        public FrmHeatingSettings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置温度
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtHeatingSet_Click(object sender, EventArgs e)
        {
            this.BondSubSectionController.bondSubSectionProgram.HeaterTemperature = (int)this.SpHeaterTemperature.Value;
            this.BondSubSectionController.SetHeaterTemperature((int)this.SpHeaterTemperature.Value);

            Machine.GetInstance().InitTemperatureInfo();

            TransportProgram.GetInstance().Save();
            AKRSXtraMessageBox.Show("设置成功");
        }

        /// <summary>
        /// 设置报警温度
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAlarmHeatingSet_Click(object sender, EventArgs e)
        {
            this.BondSubSectionController.bondSubSectionProgram.AlarmTemperature = (int)this.SpAlarmTemperature.Value;
            TransportProgram.GetInstance().Save();
            AKRSXtraMessageBox.Show("设置成功");
        }

        /// <summary>
        /// 定时器
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            this.labelControl3.Text = this.BondSubSectionController.GetHeaterTemperature().ToString();
            this.TsIsOpenHeater.IsOn = this.BondSubSectionController.IsHeaterOpen();
            if (!Machine.GetInstance().HeatingComplete)
            {
                this.labelControl13.Text = "温度未达到设定温度，未开始预热";
            }
            else if (Machine.GetInstance().PreheatingComplete)
            {
                this.labelControl13.Text = "预热完成";
            }
            else
            {
                this.labelControl13.Text = (this.BondSubSectionController.bondSubSectionProgram.PreheatTime - (DateTime.Now - Machine.GetInstance().HeatingCompleteTime).Minutes).ToString() + "分钟";
            }
        }

        /// <summary>
        /// 打开加热
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void TsIsOpenHeater_Toggled(object sender, EventArgs e)
        {
            if (this.TsIsOpenHeater.IsOn)
            {
                Machine.GetInstance().InitTemperatureInfo();
                this.BondSubSectionController.OpenHeater();
            }
            else
            {
                this.BondSubSectionController.CLoseHeater();
            }
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmHeatingSettings_Load(object sender, EventArgs e)
        {
            this.TsIsOpenHeater.IsOn = this.BondSubSectionController.IsHeaterOpen();
            this.SpHeaterTemperature.EditValue = this.BondSubSectionController.bondSubSectionProgram.HeaterTemperature;
            this.SpAlarmTemperature.EditValue = this.BondSubSectionController.bondSubSectionProgram.AlarmTemperature;
            this.SpCompensateValue.EditValue = this.BondSubSectionController.GetCompensateValue();
            this.SpCompensateSlope.EditValue = this.BondSubSectionController.GetCompensateSlope();
            this.SpPreheatTime.EditValue = this.BondSubSectionController.bondSubSectionProgram.PreheatTime;
            this.TsIsOpenHeater.Toggled += new System.EventHandler(this.TsIsOpenHeater_Toggled);

            this.labelControl16.Text = "例：设定100°，实测120°，补偿20。\r\n 设定100°，实测80°，补偿-20。";
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmHeatingSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Timer.Tick -= new System.EventHandler(this.Timer_Tick);
            this.Timer.Enabled = false;
            this.Timer.Dispose();
        }

        /// <summary>
        /// 设置温度补偿
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSetCompensateValue_Click(object sender, EventArgs e)
        {
            this.BondSubSectionController.bondSubSectionProgram.CompensateTemperature = (double)this.SpCompensateValue.Value;
            this.BondSubSectionController.SetCompensateValue(this.BondSubSectionController.bondSubSectionProgram.CompensateTemperature);
            TransportProgram.GetInstance().Save();
            AKRSXtraMessageBox.Show("设置成功");
        }

        /// <summary>
        /// 设置温度斜率
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSetCompensateSlope_Click(object sender, EventArgs e)
        {
            this.BondSubSectionController.bondSubSectionProgram.CompensateSlope = (double)this.SpCompensateSlope.Value;
            this.BondSubSectionController.SetCompensateSlope(this.BondSubSectionController.bondSubSectionProgram.CompensateSlope);
            TransportProgram.GetInstance().Save();
            AKRSXtraMessageBox.Show("设置成功");
        }

        /// <summary>
        /// 设置预热时间
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtPreheatTime_Click(object sender, EventArgs e)
        {
            this.BondSubSectionController.bondSubSectionProgram.PreheatTime = (int)this.SpPreheatTime.Value;
            TransportProgram.GetInstance().Save();
            AKRSXtraMessageBox.Show("设置成功");
        }


        /// <summary>
        /// 数值改变时事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void SpHeaterTemperature_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            string valueString = e.NewValue.ToString().Replace(".", "");

           bool success =  int.TryParse(valueString, out int value);

            if (success && value > 200)
            {
                this.BondSubSectionController.CLoseHeater();
                XtraMessageBox.Show("加热数值超过200，请重新输入");
                e.Cancel = true;
            }
        }
    }
}
