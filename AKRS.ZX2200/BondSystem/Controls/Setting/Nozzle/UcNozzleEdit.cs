using System;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.Controls.ToolControls.Programming;

namespace AKRS.ZX2200.BondSystem.Controls.Setting.Nozzle
{
    using System.Threading;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.Infrastructure.Interface;

    using Nozzle = AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle.Nozzle;

    /// <summary>
    /// 吸嘴编辑界面
    /// </summary>
    public partial class UcNozzleEdit : DevExpress.XtraEditors.XtraUserControl, IProgrammingControl
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="nozzle">传入的吸嘴对象</param>
        public UcNozzleEdit(Nozzle nozzle)
        {
            this.nozzle = nozzle;
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 编辑的吸嘴对象
        /// </summary>
        private Nozzle nozzle;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// 刷新下拉框
        /// </summary>
        private void RefreshCmbItems()
        {
            LueNozzleHeightMeasurementFunction.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<HeightMeasurementFunctionEnum>();

            LueNozzleShape.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<NozzleShapeEnum>();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            this.LbSlotIdentification.Text = this.nozzle.SlotIdentification.ToString();
            this.LueNozzleHeightMeasurementFunction.EditValue = this.nozzle.NozzleHeightMeasurementFunction;
            this.LueNozzleShape.EditValue = this.nozzle.NozzleShape;

            this.RgZDeterminationMethod.SelectedIndex = (int)this.nozzle.ZDeterminationMethod;
            this.RgXYDeterminationMethod.SelectedIndex = (int)this.nozzle.XYDeterminationMethod;

           this.SpVacuumCheckAfterPickUp.Value = this.nozzle.AfterPickupVacuumCheckValue;
           this.SpVacummCheckDelay.Value = this.nozzle.AfterPickupVacuumCheckDelay;

            this.SpVacuumCheckValueAfterBonding.Value = this.nozzle.AfterBondingVacuumCheckValue;
            this.SpVacuumCheckDelayAfterBonding.Value = this.nozzle.AfterBondingVacuumCheckDelay;

            this.RefreshCmbItems();
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            this.nozzle.ZDeterminationMethod = (ZDeterminationMethodEnum)this.RgZDeterminationMethod.SelectedIndex;
            this.nozzle.XYDeterminationMethod = (XYDeterminationMethodEnum)this.RgZDeterminationMethod.SelectedIndex;

            this.nozzle.NozzleShape = (NozzleShapeEnum)this.LueNozzleShape.EditValue;
            this.nozzle.NozzleHeightMeasurementFunction = (HeightMeasurementFunctionEnum)this.LueNozzleHeightMeasurementFunction.EditValue;

            this.nozzle.AfterPickupVacuumCheckValue = (int)this.SpVacuumCheckAfterPickUp.Value;
            this.nozzle.AfterPickupVacuumCheckDelay = (int)this.SpVacummCheckDelay.Value;

            this.nozzle.AfterBondingVacuumCheckValue = (int)this.SpVacuumCheckValueAfterBonding.Value;
            this.nozzle.AfterBondingVacuumCheckDelay = (int)this.SpVacuumCheckDelayAfterBonding.Value;

            NozzleRepository.GetInstance().Save();
        }

        /// <summary>
        /// 确定
        /// </summary>
        public void Confirm()
        {
            this.Save();
        }

        /// <summary>
        /// 获取当前真空模拟量
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtGetCurrentVacuumValue_Click(object sender, EventArgs e)
        {
            this.bondHeadController.OpenToolVaccum();
            Thread.Sleep(1000);
            double value = this.bondHeadController.ReadVacuumValue();
            this.bondHeadController.CloseToolVaccum();
            this.SpVacuumCheckAfterPickUp.EditValue = value;
            this.Confirm();
        }

        /// <summary>
        /// 获取当前真空模拟量
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtGetAfterBondingVacuumValue_Click(object sender, EventArgs e)
        {
            this.bondHeadController.OpenToolVaccum();
            Thread.Sleep(1000);
            double value = this.bondHeadController.ReadVacuumValue();
            this.bondHeadController.CloseToolVaccum();
            this.SpVacuumCheckValueAfterBonding.EditValue = value;
            this.Confirm();
        }
    }
}
