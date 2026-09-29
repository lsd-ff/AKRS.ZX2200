using AKRS.ZX2200.Controls.ToolControls.Programming;
using System;


namespace AKRS.ZX2200.DispenseSystem.Controls.Setting.Dispenser
{
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Dispenser;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.EpoxyMaterial;
    using AKRS.ZX2200.Infrastructure.Action;
    using AKRS.ZX2200.Infrastructure.Interface;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using System.Collections.Generic;

    /// <summary>
    /// 程式点教头设置
    /// </summary>
    public partial class UcDispenser : DevExpress.XtraEditors.XtraUserControl, IProgrammingControl
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public UcDispenser()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 实体对象
        /// </summary>
        private Dispenser Dispenser =>
           this.GetDispenser();

        /// <summary>
        /// 获取胶水材料
        /// </summary>
        private EpoxyMaterial EpoxyMaterial => this.GetEpoxyMaterial();

        /// <summary>
        /// 获取点胶头
        /// </summary>
        /// <returns>结果</returns>
        private Dispenser GetDispenser()
        {
            if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                return BondProgram.GetInstance().S2DispenserProgram.Dispenser;
            }
            else
            {
                return System1Domain.GetInstance().System1Program.DispenserProgram.Dispenser;
            }
        }

        /// <summary>
        /// 获取胶水
        /// </summary>
        /// <returns>结果</returns>
        private EpoxyMaterial GetEpoxyMaterial()
        {
            if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense
                && MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
            {
                return BondProgram.GetInstance().S2EpoxyMaterialProgram.EpoxyMaterial;
            }
            else
            {
                return System1Domain.GetInstance().System1Program.EpoxyMaterialProgram.EpoxyMaterial;
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void UcDispenser_Load(object sender, EventArgs e)
        {
            if (this.Dispenser == null)
            {
                this.Visible = false;
                return;
            }

            this.Init();
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        public void Init()
        {
            // 初始化下拉框的值,这样写不对，后续再改,显示的值应该是注释
            this.LueDispenserType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<DispenserTypeEnum>();
            this.LuePreDispenseModel.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<PreDispensingModeEnum>();
            this.LuePreDispenseTiming.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<PreDispensingTimingEnum>();

            if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                for (int i = 0; i < BondProgram.GetInstance().EpoxyApplicationProgram.EpoxyApplicationNames.Count; i++)
                {
                    this.CbEpoxyApplication.Properties.Items.Add(
                        BondProgram.GetInstance().EpoxyApplicationProgram.EpoxyApplicationNames[i]);
                }
            }
            else
            {
                for (int i = 0; i < System1Domain.GetInstance().System1Program.EpoxyApplicationProgram.EpoxyApplicationNames.Count; i++)
                {
                    this.CbEpoxyApplication.Properties.Items.Add(
                        System1Domain.GetInstance().System1Program.EpoxyApplicationProgram.EpoxyApplicationNames[i]);
                }
            }
           

            this.CbEpoxyApplication.SelectedItem = this.Dispenser.PreDispense.EpoxyApplicationName;

            // 加载参数
            this.LueDispenserType.EditValue = this.Dispenser.DispenserType;

            this.LuePreDispenseModel.EditValue = this.Dispenser.PreDispense.PreDispensingMode;

            this.LuePreDispenseTiming.EditValue = this.Dispenser.PreDispense.PreDispensingTiming;

            this.SpPreDispensingTimeIntervalMin.EditValue = this.Dispenser.PreDispense.PreDispensingTimeInterval / 60000;

            this.SpPreDispensingTimeIntervalS.EditValue =
                (this.Dispenser.PreDispense.PreDispensingTimeInterval % 60000) / 1000;

            this.SpPreDispensingOffsetX.EditValue = this.Dispenser.PreDispense.PreDispensingOffsetX;

            this.SpPreDispensingOffsetY.EditValue = this.Dispenser.PreDispense.PreDispensingOffsetY;

            this.SpRepeatPreDispensing.EditValue = this.Dispenser.PreDispense.RepeatPreDispensing;

            this.ChkEpoxySqueeze.EditValue = this.Dispenser.PreDispense.EnableEpoxySqueeze;

            this.SpEpoxySqueezeTime.EditValue = this.Dispenser.PreDispense.EpoxySqueezeTime;

            this.SpEpoxySqueezePressure.EditValue = this.Dispenser.PreDispense.EpoxySqueezePressure;

            this.spinEdit2.EditValue = DispenseDevicePara.GetInstance().DispenserPara.AssistanceOpenDispenseVacuum;

            List<PostBondInspection> postBondInspections = BondProgram.GetInstance().PostBondProgram.PostBondInspections.FindAll(it => it.IsPreDispenseCheck);

            this.CmbEpoxyDefect.Properties.Items.Add("Null");

            foreach (PostBondInspection postBondInspection in postBondInspections)
            {
                this.CmbEpoxyDefect.Properties.Items.Add(postBondInspection.Name);
            }

            this.CmbEpoxyDefect.Enabled = this.Dispenser.PreDispense.PreDispenseCheckName != "Null";

            this.CmbEpoxyDefect.SelectedItem = this.Dispenser.PreDispense.PreDispenseCheckName;

            this.ChkDispenserTimes.Checked = this.Dispenser.DispenseTimes.Enable;

            this.SpDispenseTimes.Value = this.Dispenser.DispenseTimes.TotalUseTimes;

            if (this.EpoxyMaterial != null)
            {
                this.ChkEpoxyDueTime.Checked = this.EpoxyMaterial.EpoxyMaterialConsumable.Enable;

                this.SpEpoxyDueTime.Value = this.EpoxyMaterial.EpoxyMaterialConsumable.UseDateTime;
            }
        }

        /// <summary>
        /// 保存现有参数
        /// </summary>
        public void Confirm()
        {
            try
            {
                this.Dispenser.DispenserType = (DispenserTypeEnum)this.LueDispenserType.EditValue;

                this.Dispenser.PreDispense.PreDispensingMode =
                    (PreDispensingModeEnum)this.LuePreDispenseModel.EditValue;

                this.Dispenser.PreDispense.PreDispensingTiming =
                    (PreDispensingTimingEnum)this.LuePreDispenseTiming.EditValue;

                this.Dispenser.PreDispense.PreDispensingTimeInterval =
                (int)this.SpPreDispensingTimeIntervalMin.Value * 60000
                + (int)this.SpPreDispensingTimeIntervalS.Value * 1000;

                this.Dispenser.PreDispense.PreDispensingOffsetX = (double)this.SpPreDispensingOffsetX.Value;

                this.Dispenser.PreDispense.PreDispensingOffsetY = (double)this.SpPreDispensingOffsetY.Value;

                this.Dispenser.PreDispense.RepeatPreDispensing = (int)this.SpRepeatPreDispensing.Value;

                if (this.CbEpoxyApplication.SelectedItem != null)
                {
                    this.Dispenser.PreDispense.EpoxyApplicationName = this.CbEpoxyApplication.SelectedItem.ToString();
                }

                this.Dispenser.PreDispense.EnableEpoxySqueeze = this.ChkEpoxySqueeze.Checked;

                this.Dispenser.PreDispense.EpoxySqueezeTime = (int)this.SpEpoxySqueezeTime.Value;

                this.Dispenser.PreDispense.EpoxySqueezePressure = (int)this.SpEpoxySqueezePressure.Value;

                this.Dispenser.PreDispense.PreDispenseCheckName = this.CmbEpoxyDefect.SelectedItem?.ToString();

                this.Dispenser.DispenseTimes.Enable = this.ChkDispenserTimes.Checked;

                this.Dispenser.DispenseTimes.TotalUseTimes = (int)this.SpDispenseTimes.Value;

                DispenseDevicePara.GetInstance().DispenserPara.AssistanceOpenDispenseVacuum = (int)this.spinEdit2.Value;

                if (this.EpoxyMaterial != null)
                {
                    this.EpoxyMaterial.EpoxyMaterialConsumable.Enable = this.ChkEpoxyDueTime.Checked;

                    this.EpoxyMaterial.EpoxyMaterialConsumable.UseDateTime = (int)this.SpEpoxyDueTime.Value;
                }
                
                // 赋值完成后保存
                DispenserRepository.GetInstance().Save();

                System1Domain.GetInstance().System1Program.Save();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        /// <summary>
        /// 刷新页面
        /// </summary>
        private void RefreshDataByModel()
        {
            if (this.LuePreDispenseModel.EditValue == null || this.LuePreDispenseTiming.EditValue == null)
            {
                return;
            }

            if ((PreDispensingModeEnum)this.LuePreDispenseModel.EditValue == PreDispensingModeEnum.Off
                && (PreDispensingTimingEnum)this.LuePreDispenseTiming.EditValue == PreDispensingTimingEnum.Off)
            {
                this.groupControl1.Enabled = false;
            }
            else if ((PreDispensingModeEnum)this.LuePreDispenseModel.EditValue == PreDispensingModeEnum.Off)
            {
                this.groupControl1.Enabled = true;
            }
            else
            {
                this.groupControl1.Enabled = true;
            }
        }


        /// <summary>
        /// 挤胶时间发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkEpoxySqueeze_CheckedChanged(object sender, EventArgs e)
        {
            this.SpEpoxySqueezeTime.Enabled = this.ChkEpoxySqueeze.Checked;
            this.SpEpoxySqueezePressure.Enabled = this.ChkEpoxySqueeze.Checked;
        }

        /// <summary>
        /// 选择发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void LuePreDispenseModel_EditValueChanged(object sender, EventArgs e)
        {
            this.RefreshDataByModel();
        }

        /// <summary>
        /// 选择发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void LuePreDispenseTiming_EditValueChanged(object sender, EventArgs e)
        {
            this.RefreshDataByModel();
        }

        /// <summary>
        /// 选择发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void LueDispenserType_EditValueChanged(object sender, EventArgs e)
        {
            this.groupControl12.Enabled =
                (DispenserTypeEnum)this.LueDispenserType.EditValue == DispenserTypeEnum.PrintingTool;

            this.groupControl7.Enabled =
                (DispenserTypeEnum)this.LueDispenserType.EditValue != DispenserTypeEnum.PrintingTool;
        }

        /// <summary>
        /// 点胶次数发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkDispenserTimes_CheckedChanged(object sender, EventArgs e)
        {
            this.SpDispenseTimes.Enabled = this.ChkDispenserTimes.Checked;
        }

        /// <summary>
        /// 胶水到期发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkEpoxyDueTime_CheckedChanged(object sender, EventArgs e)
        {
            this.SpEpoxyDueTime.Enabled = this.ChkEpoxyDueTime.Checked;
        }

        /// <summary>
        /// 胶型检测发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkEpoxyDefect_CheckedChanged(object sender, EventArgs e)
        {
            this.CmbEpoxyDefect.Enabled = this.ChkEpoxyDefect.Checked;
        }
    }
}
