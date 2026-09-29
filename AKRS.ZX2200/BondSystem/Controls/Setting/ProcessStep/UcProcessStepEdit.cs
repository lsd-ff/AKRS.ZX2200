namespace AKRS.ZX2200.BondSystem.Controls.Setting.ProcessStep
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.Infrastructure.Interface;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Product.ProcessStep;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;

    using DevExpress.XtraEditors;

    /// <summary>
    /// 步骤示教界面
    /// </summary>
    public partial class UcProcessStepEdit : DevExpress.XtraEditors.XtraUserControl, IProgrammingControl
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public UcProcessStepEdit()
        {
            this.InitializeComponent();
            this.Init();
        }

        /// <summary>
        /// 当前选中的
        /// </summary>
        public ProcessStep SelectProcessStep { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            this.LsBondPosition.DrawItem += this.DrawColor;
            this.LsComponet.DrawItem += this.DrawColor;
            this.LsEpoxyApplication.DrawItem += this.DrawColor;
            this.LsPostBondInspection.DrawItem += this.DrawColor;

            this.LsBondPosition.Items.Clear();
            this.LsComponet.Items.Clear();
            this.LsEpoxyApplication.Items.Clear();
            this.LsPostBondInspection.Items.Clear();

            this.LsEpoxyApplication.Items.Add("Null");

            foreach (string epoxyApplication in System1Domain.GetInstance().System1Program.EpoxyApplicationProgram.EpoxyApplicationNames)
            {
                this.LsEpoxyApplication.Items.Add(epoxyApplication);
            }


            if (WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig == null)
            {
                // 这种情况不会出现，外部加流程防呆！
            }
            else
            {
                this.LsComponet.Items.Add("Null");

                foreach (BaseCarrierConfig carrier in WaferSystemProgram.GetInstance().GetCarriers())
                {
                    this.LsComponet.Items.Add(carrier.Name);
                }
            }

            this.LsBondPosition.Items.Add("Null");
            if (ProductConfiguration.GetInstance()?.BondPositionConfig?.SingleBpPositionConfigList
                != null)
            {
                foreach (SingleBondPositionConfig bondPosition in ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList)
                {
                    this.LsBondPosition.Items.Add(bondPosition.Name);
                }
            }

            this.LsPostBondInspection.Items.Add("Null");
            if (PostBondInspectionRepository.GetInstance().BaseDsSettingList != null) 
            {
                foreach (PostBondInspection postBondInspection in BondProgram.GetInstance().PostBondProgram.PostBondInspections)
                {
                    this.LsPostBondInspection.Items.Add(postBondInspection.Name);
                }
            } 

            List<ProcessStep>
                processStepList = ProcessStepProgram.GetInstance().ProcessStepList; //.OrderBy(a => a.Order).ToList();

            this.GcProcessStep.DataSource = processStepList;
            this.GcProcessStep.RefreshDataSource();

            this.LueActionNodeSortMode.Properties.DataSource =
                EnumHelper.ConvertEnumToNameDisplayDto<ActionNodesSortModeEnum>();

            this.RefreshControl();
        }

        /// <summary>
        /// 改变选中项背景色
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void DrawColor(object sender, ListBoxDrawItemEventArgs e)
        {
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Appearance.ForeColor = Color.Black;
                e.Appearance.BackColor = Color.Yellow;
            }
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void RefreshControl()
        {
            // 获取目前选中的名称
            this.SelectProcessStep = this.GvProcessStep.GetFocusedRow() as ProcessStep;

            if (this.CmbSystem.Text == @"System1")
            {
                this.LueActionNodeSortMode.EditValue = ProcessStepProgram.GetInstance().ActionNodesSortModeInS1;

                if (this.SelectProcessStep != null)
                {
                    this.ChkEnable.Checked = this.SelectProcessStep.IsEnableInSystem1;

                    this.ChangeSelect(this.SelectProcessStep.PostBondInspectionNameInS1, this.LsPostBondInspection);
                }
            }
            else
            {
                this.LueActionNodeSortMode.EditValue = ProcessStepProgram.GetInstance().ActionNodesSortModeInS2;
                if (this.SelectProcessStep != null)
                {
                    this.ChkEnable.Checked = this.SelectProcessStep.IsEnableInSystem2;
                    this.ChangeSelect(this.SelectProcessStep.PostBondInspectionNameInS2, this.LsPostBondInspection);
                }
            }

            if (this.SelectProcessStep != null)
            {
                this.ChangeSelect(this.SelectProcessStep.EpoxyApplicationNameInS1, this.LsEpoxyApplication);
            }
        }

        /// <summary>
        /// 选择发生了改变
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void GvProcessStep_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            // 获取目前选中的名称
            this.SelectProcessStep = this.GvProcessStep.GetFocusedRow() as ProcessStep;

            if (this.SelectProcessStep == null)
            {
                return;
            }

            this.ChangeSelect(this.SelectProcessStep.BondPositionName, this.LsBondPosition);
            this.ChangeSelect(this.SelectProcessStep.ComponentName, this.LsComponet);
            if (this.CmbSystem.Text == @"System1")
            {
                this.ChangeSelect(this.SelectProcessStep.PostBondInspectionNameInS1, this.LsPostBondInspection);
                this.ChangeSelect(this.SelectProcessStep.EpoxyApplicationNameInS1, this.LsEpoxyApplication);
                this.ChangeSelect(this.SelectProcessStep.PostBondInspectionNameInS1, this.LsPostBondInspection);
            }
            else
            {
                this.ChangeSelect(this.SelectProcessStep.PostBondInspectionNameInS2, this.LsPostBondInspection);
                this.ChangeSelect(this.SelectProcessStep.EpoxyApplicationNameInS2, this.LsEpoxyApplication);
                this.ChangeSelect(this.SelectProcessStep.PostBondInspectionNameInS2, this.LsPostBondInspection);
            }

            this.RefreshControl();
        }

        /// <summary>
        /// 改变选中
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="listBoxControl">窗体</param>
        private void ChangeSelect(string name, ListBoxControl listBoxControl)
        {
            if (name == null)
            {
                listBoxControl.SelectedItem = "Null";
            }

            listBoxControl.SelectedItem = name;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtAdd_Click(object sender, EventArgs e)
        {
            FrmCreateProcessStep frmCreateProcessStep = new FrmCreateProcessStep();

            frmCreateProcessStep.ShowDialog();

            // 刷新数据
            this.Init();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtDelete_Click(object sender, EventArgs e)
        {
            this.SelectProcessStep = this.GvProcessStep.GetFocusedRow() as ProcessStep;

            ProcessStepProgram.GetInstance().Remove(this.SelectProcessStep);
            ProcessStepProgram.GetInstance().Save();

            this.Init();
        }

        /// <summary>
        /// 改变画胶界面
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void LsEpoxyApplication_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.LsEpoxyApplication.SelectedItem == null)
            {
                return;
            }

            if (this.SelectProcessStep == null)
            {
                return;
            }

            if (this.CmbSystem.Text == @"System1")
            {
                this.SelectProcessStep.EpoxyApplicationNameInS1 = (string)this.LsEpoxyApplication.SelectedItem;
            }
            else
            {
                this.SelectProcessStep.EpoxyApplicationNameInS2 = (string)this.LsEpoxyApplication.SelectedItem;
            }
        }

        /// <summary>
        /// 改变BondPosition
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void LsBondPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.LsBondPosition.SelectedItem == null)
            {
                return;
            }

            if (this.SelectProcessStep == null)
            {
                return;
            }

            this.SelectProcessStep.BondPositionName = (string)this.LsBondPosition.SelectedItem;

            //// 保存数据
            //ProcessStepProgram.GetInstance().Save();
        }

        /// <summary>
        /// 改变芯片
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void LsComponet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.LsComponet.SelectedItem == null)
            {
                return;
            }

            if (this.SelectProcessStep == null)
            {
                return;
            }

            this.SelectProcessStep.ComponentName = (string)this.LsComponet.SelectedItem;

            //// 保存数据
            //ProcessStepProgram.GetInstance().Save();
        }

        /// <summary>
        /// 上升
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtUp_Click(object sender, EventArgs e)
        {
            //CommonHelper.UpOrDown<ProcessStep>(Direction.Up, this.GvProcessStep);

            CommonHelper.UpOrDown(Direction.Up, this.GvProcessStep);
            ProcessStepProgram.GetInstance().Save();
            this.GvProcessStep_FocusedRowChanged(null, null);
            this.GvProcessStep.RefreshData();
        }

        /// <summary>
        /// 改变芯片
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtDown_Click(object sender, EventArgs e)
        {
            // CommonHelper.UpOrDown<ProcessStep>(Direction.Down, this.GvProcessStep);

           CommonHelper.UpOrDown(Direction.Down, this.GvProcessStep);
            ProcessStepProgram.GetInstance().Save();
            this.GvProcessStep_FocusedRowChanged(null, null);
            this.GvProcessStep.RefreshData();
        }

        /// <summary>
        /// 保存现有参数
        /// </summary>
        public void Confirm()
        {
            this.SelectProcessStep = this.GvProcessStep.GetFocusedRow() as ProcessStep;

            List<ProcessStep> processSteps = ProcessStepProgram.GetInstance().ProcessStepList;

            foreach (ProcessStep preProcessStep in processSteps)
            {
                if (preProcessStep.BondPositionName == null || preProcessStep.BondPositionName == "Null")
                {
                    XtraMessageBox.Show($"流程步：{preProcessStep.Name} 焊点名为空！保存失败！");
                    return;
                }

                if (preProcessStep.ComponentName == null || preProcessStep.ComponentName == "Null")
                {
                    if (preProcessStep.IsEnableInSystem2)
                    {
                        XtraMessageBox.Show($"流程步：{preProcessStep.Name} 芯片名为空！保存失败！");
                        return;
                    }
                }

                if (MachineConfiguration.GetInstance().IsSystem1Configrated)
                {
                    if (preProcessStep.EpoxyApplicationNameInS1 == null || preProcessStep.EpoxyApplicationNameInS1 == "Null")
                    {
                        XtraMessageBox.Show($"流程步：{preProcessStep.Name} 点胶图形名称为空！保存失败！");
                        return;
                    }
                }
            }

            if (this.CmbSystem.Text == @"System1")
            {
                 ProcessStepProgram.GetInstance().ActionNodesSortModeInS1 = (ActionNodesSortModeEnum)this.LueActionNodeSortMode.EditValue;

                //// 默认为高UPH模式
                //ProcessStepProgram.GetInstance().ActionNodesSortModeInS1 = ActionNodesSortModeEnum.HighUPH;

                //ProcessStepProgram.GetInstance().ActionNodesSortModeInS1 = 

                ProcessStepProgram.GetInstance().S1ProcessingStrategy = ProcessingStrategyEnum.ModulesBeforeSteps;

                if (this.SelectProcessStep != null)
                {
                    this.SelectProcessStep.IsEnableInSystem1 = this.ChkEnable.Checked;
                }
            }
            else
            {
                ProcessStepProgram.GetInstance().ActionNodesSortModeInS2 = (ActionNodesSortModeEnum)this.LueActionNodeSortMode.EditValue;

                //// 默认为高UPH模式
                //ProcessStepProgram.GetInstance().ActionNodesSortModeInS2 = ActionNodesSortModeEnum.HighUPH;
                ProcessStepProgram.GetInstance().S2ProcessingStrategy = ProcessingStrategyEnum.ModulesBeforeSteps;

                if (this.SelectProcessStep != null)
                {
                    this.SelectProcessStep.IsEnableInSystem2 = this.ChkEnable.Checked;
                }
            }

            this.SelectProcessStep.EpoxyApplicationNameInS1 = (string)this.LsEpoxyApplication.SelectedItem;

            // 保存数据
            ProcessStepProgram.GetInstance().Save();
        }

        /// <summary>
        /// 下拉框选项改变事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void CmbSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RefreshControl();
        }

        /// <summary>
        /// Enable勾选改变事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void ChkEnable_CheckedChanged(object sender, EventArgs e)
        {
            this.SelectProcessStep = this.GvProcessStep.GetFocusedRow() as ProcessStep;

            if (this.CmbSystem.Text == @"System1")
            {
                if (this.SelectProcessStep != null)
                {
                    this.SelectProcessStep.IsEnableInSystem1 = this.ChkEnable.Checked;
                }
            }
            else
            {
                if (this.SelectProcessStep != null)
                {
                    this.SelectProcessStep.IsEnableInSystem2 = this.ChkEnable.Checked;
                }
            }
        }

        /// <summary>
        /// 选择改变事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void LsPostBondInspection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.LsPostBondInspection.SelectedItem == null)
            {
                return;
            }

            if (this.SelectProcessStep == null)
            {
                return;
            }

            if (this.CmbSystem.Text == @"System1")
            {
                this.SelectProcessStep.PostBondInspectionNameInS1 = (string)this.LsPostBondInspection.SelectedItem;
            }
            else
            {
                this.SelectProcessStep.PostBondInspectionNameInS2 = (string)this.LsPostBondInspection.SelectedItem;
            }
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void UcProcessStepEdit_Load(object sender, EventArgs e)
        {
            if (!MachineConfiguration.GetInstance().IsSystem1Configrated)
            {
                this.groupControl7.Visible = false;
                this.groupControl4.Visible = false;
            }
        }
    }
}
