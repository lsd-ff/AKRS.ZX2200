using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.TransportUnitSystem;
using AKRS.ZX2200.TransportUnitSystem.Module.Config;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AKRS.ZX2200.Main.Machine.Process
{
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Interface;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Product;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using DevExpress.Utils;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraGrid.Views.Grid.ViewInfo;
    using MathNet.Numerics.Distributions;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// 步骤
    /// </summary>
    public partial class UcProcessStep : DevExpress.XtraEditors.XtraUserControl, IProgrammingControl
    {
        /// <summary>
        /// 步骤程式
        /// </summary>
        private ProcessDomain ProcessDomain => ProcessDomain.GetInstance();

        /// <summary>
        /// 当前选择的步骤
        /// </summary>
        private SingleProcessStep CurrentSingleProcessStep => this.gridView1.GetFocusedRow() as SingleProcessStep;

        /// <summary>
        /// 当前步骤集合
        /// </summary>
        private List<SingleProcessStep> SingleProcessSteps => this.GetSingleProcessSteps();

        /// <summary>
        /// 获取当前的系统
        /// </summary>
        private SystemProcess CurrentSystemProcess => this.GetCurrentSystemProcess();

        /// <summary>
        /// 是否在加载
        /// </summary>
        private bool isLoad = true;

        /// <summary>
        /// 制程步骤
        /// </summary>
        public UcProcessStep()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void UcProcessStep_Load(object sender, EventArgs e)
        {
            this.Init();

            this.LsBondPosition.DrawItem += this.DrawColor;
            this.LsComponentName.DrawItem += this.DrawColor;
            this.LsPostBondInspection.DrawItem += this.DrawColor;
            this.LsEpoxyApplication.DrawItem += this.DrawColor;
            this.gridView1.Appearance.FocusedRow.BackColor = Color.GreenYellow;
            this.gridView1.Appearance.FocusedRow.ForeColor = Color.Red;
            this.isLoad = false;
            this.gridView1.MouseDown += this.GridView1_MouseDown;
            this.BtAutoGenerate.Visible = ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex;

            this.BtOpenCloseDefect.Appearance.BackColor = Color.Green;
            this.BtOpenCloseDefect.Text = "打开所有检测";

            foreach (SingleProcessStep singleProcessStep in ProcessDomain.S1SystemProcess.ProcessSteps)
            {
                if (singleProcessStep.DefectName != "Null" && singleProcessStep.IsEnable)
                {
                    this.BtOpenCloseDefect.Appearance.BackColor = SystemColors.Window;
                    this.BtOpenCloseDefect.Text = "关闭所有检测";
                }
            }

            foreach (SingleProcessStep singleProcessStep in ProcessDomain.S2SystemProcess.ProcessSteps)
            {
                if (singleProcessStep.DefectName != "Null" && singleProcessStep.IsEnable)
                {
                    this.BtOpenCloseDefect.Appearance.BackColor = SystemColors.Window;
                    this.BtOpenCloseDefect.Text = "关闭所有检测";
                }
            }
        }

        /// <summary>
        /// 双击选中之后才能更改
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void GridView1_MouseDown(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;

            if (view == null)
            {
                return;
            }

            GridHitInfo hitInfo = view.CalcHitInfo(e.Location);

            if (hitInfo.Column?.AbsoluteIndex == 2)
            {
                return;
            }

            if (hitInfo.InRowCell && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (e.Clicks == 1)
                {
                    view.FocusedColumn = hitInfo.Column;
                    view.FocusedRowHandle = hitInfo.RowHandle;
                }
                else if (e.Clicks == 2)
                {
                    view.ShowEditor();
                }
                
                DXMouseEventArgs.GetMouseArgs(e).Handled = true;
            }
        }

        /// <summary>
        /// 选中项变颜色
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        public void DrawColor(object sender, DevExpress.XtraEditors.ListBoxDrawItemEventArgs e)
        {
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Appearance.BackColor = Color.GreenYellow;
                e.Appearance.ForeColor = Color.Red;
            }
        }
        
        /// <summary>
        /// 获取当前的步骤
        /// </summary>
        /// <returns>结果</returns>
        private SystemProcess GetCurrentSystemProcess()
        {
            if ((ProcessSystemEnum)this.LueSystem.EditValue == ProcessSystemEnum.System1)
            {
                return ProcessDomain.S1SystemProcess;
            }
            else
            {
                return ProcessDomain.S2SystemProcess;
            }
        }

        /// <summary>
        /// 获取当前的步骤集合
        /// </summary>
        /// <returns>集合</returns>
        private List<SingleProcessStep> GetSingleProcessSteps()
        {
            if (this.LueSystem.EditValue == null)
            {
                return null;
            }

            return this.CurrentSystemProcess.ProcessSteps;
        }

        /// <summary>
        /// 初始化系统选择
        /// </summary>
        private void InitSystemChoose()
        {
            if (!MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                this.LueSystem.EditValue = ProcessSystemEnum.System2;
                this.LueSystem.Enabled = false;
            }
            else
            {
                this.LueSystem.EditValue = ProcessSystemEnum.System1;
                this.LueSystem.Enabled = true;
            }
        }


        /// <summary>
        /// 初始化图形
        /// </summary>
        private void InitEpoxyApplication()
        {
            this.LsEpoxyApplication.Items.Clear();
            this.LsEpoxyApplication.Items.Add("Null");
            if ((ProcessSystemEnum)this.LueSystem.EditValue == ProcessSystemEnum.System2 && MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                foreach (string epoxyApplication in System2Domain.GetInstance().BondProgram.EpoxyApplicationProgram.EpoxyApplicationNames)
                {
                    this.LsEpoxyApplication.Items.Add(epoxyApplication);
                }
            }
            else if ((ProcessSystemEnum)this.LueSystem.EditValue == ProcessSystemEnum.System2 && MachineHardwareConfiguration.GetInstance().IsSystem2ConfigPrintingTool)
            {
                foreach (EpoxyApplication epoxyApplication in System2Domain.GetInstance().BondProgram.EpoxyApplicationProgram.EpoxyApplications)
                {
                    if (epoxyApplication.EpoxyApplicationStrategy == DispenseSystem.Models.Enums.EpoxyApplicationTypeEnum.Printting)
                    {
                        this.LsEpoxyApplication.Items.Add(epoxyApplication.Name);
                    }
                }

                foreach (EpoxyApplication epoxyApplication in System1Domain.GetInstance().System1Program.EpoxyApplicationProgram.EpoxyApplications)
                {
                    if (epoxyApplication.EpoxyApplicationStrategy == DispenseSystem.Models.Enums.EpoxyApplicationTypeEnum.Printting)
                    {
                        this.LsEpoxyApplication.Items.Add(epoxyApplication.Name);
                    }
                }
            }
            else if ((ProcessSystemEnum)this.LueSystem.EditValue == ProcessSystemEnum.System1)
            {
                foreach (string epoxyApplication in System1Domain.GetInstance().System1Program.EpoxyApplicationProgram.EpoxyApplicationNames)
                {
                    this.LsEpoxyApplication.Items.Add(epoxyApplication);
                }
            }
        }

        /// <summary>
        /// 初始化焊点
        /// </summary>
        private void InitPostBondInspection()
        {
            this.LsPostBondInspection.Items.Add("Null");
            if (PostBondInspectionRepository.GetInstance().BaseDsSettingList != null)
            {
                foreach (PostBondInspection postBondInspection in BondProgram.GetInstance().PostBondProgram.PostBondInspections)
                {
                    this.LsPostBondInspection.Items.Add(postBondInspection.Name);
                }
            }
        }

        /// <summary>
        /// 初始化芯片
        /// </summary>
        private void InitComponent()
        {
            this.LsComponentName.Items.Add("Null");
            if (WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig != null)
            {
                foreach (BaseCarrierConfig carrier in WaferSystemProgram.GetInstance().GetCarriers())
                {
                    this.LsComponentName.Items.Add(carrier.Name);
                }
            }
        }

        /// <summary>
        /// 初始化焊点
        /// </summary>
        private void InitBondPosition()
        {
            this.LsBondPosition.Items.Add("Null");

            if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
            {
                foreach (OppositeSexConfig oppositeSexConfig in ProductDomain.GetInstance().ProductConfig.OppositeSexConfiguration.BaseConfigs)
                {
                    if (oppositeSexConfig.EntityType != EntityTypeEnum.BondPosition)
                    {
                        continue;
                    }

                    if (!this.LsBondPosition.Items.Contains(oppositeSexConfig.Name))
                    {
                        this.LsBondPosition.Items.Add(oppositeSexConfig.Name);
                    }
                }
            }
            else
            {
                if (ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList
                    != null)
                {
                    foreach (SingleBondPositionConfig bondPosition in ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList)
                    {
                        this.LsBondPosition.Items.Add(bondPosition.Name);
                    }
                }
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            this.LueSystem.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<ProcessSystemEnum>();
            this.LueWorkMode.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<WorkProcessEnum>();
            this.LueWorkModeFirst.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<WorkProcessModuleEnum>();

            this.LueWorkMode.EditValue = this.CurrentSystemProcess.WorkProcess;
            this.LueWorkModeFirst.EditValue = this.CurrentSystemProcess.WorkProcessModule;
            this.SpCycleNumber.EditValue = this.CurrentSystemProcess.WorkCycleNumber;
            this.ChkWorkByNumber.EditValue = this.CurrentSystemProcess.IsWorkCycleNumber;
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            this.InitSystemChoose();

            this.InitEpoxyApplication();

            this.InitPostBondInspection();

            this.InitComponent();
            
            this.InitBondPosition();

           this.InitData();

            this.RefreshData();

            this.LsBondPosition.SelectedIndexChanged += new System.EventHandler(this.LsBondPosition_SelectedIndexChanged);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            this.InitEpoxyApplication();

            this.ProcessDomain.S1SystemProcess.ProcessSteps.Sort((x, y) => x.Index.CompareTo(y.Index));
            this.ProcessDomain.S2SystemProcess.ProcessSteps.Sort((x, y) => x.Index.CompareTo(y.Index));
            this.GcProcessStep.DataSource = null;

            this.GcProcessStep.DataSource = this.SingleProcessSteps;
            
            this.LsComponentName.Enabled = (ProcessSystemEnum)this.LueSystem.EditValue == ProcessSystemEnum.System2;

            this.ProcessDomain.Save();
        }

        /// <summary>
        /// 添加步骤
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAdd_Click(object sender, EventArgs e)
        {
            string stepName = "步骤1";

            if ((ProcessSystemEnum)this.LueSystem.EditValue == ProcessSystemEnum.System1)
            {
                for (int i = 0; i < ProcessDomain.S1SystemProcess.ProcessSteps.Count + 1; i++)
                {
                    if (ProcessDomain.S1SystemProcess.ProcessSteps.Find(it => it.ProcessStepName == $"S1-步骤{i + 1}") == null)
                    {
                        stepName = $"S1-步骤{i + 1}";
                    }
                }
            }
            else
            {
                for (int i = 0; i < ProcessDomain.S2SystemProcess.ProcessSteps.Count + 1; i++)
                {
                    if (ProcessDomain.S2SystemProcess.ProcessSteps.Find(it => it.ProcessStepName == $"S2-步骤{i + 1}") == null)
                    {
                        stepName = $"S2-步骤{i + 1}";
                    }
                }
            }

            // 名称
            string newSingleStepName = XtraInputBox.Show("请输入步骤的名称", "步骤名称", stepName);

            if (newSingleStepName == string.Empty)
            {
                AKRSXtraMessageBox.Show("输入名称有误");
                return;
            }

            if (ProcessDomain.S2SystemProcess.ProcessSteps.Find(it => it.ProcessStepName == newSingleStepName) != null
                || ProcessDomain.S2SystemProcess.ProcessSteps.Find(it => it.ProcessStepName == newSingleStepName) != null)
            {
                AKRSXtraMessageBox.Show($"名称为:{newSingleStepName} 的步骤已经存在，请重新输入");
                return;
            }

            this.SingleProcessSteps.Add(
                new SingleProcessStep()
                    {
                        ProcessStepName = newSingleStepName,
                        Index = this.SingleProcessSteps.Count + 1
                    });

            this.RefreshData();
        }

        /// <summary>
        /// 上升
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtUp_Click(object sender, EventArgs e)
        {
            SingleProcessStep singleProcessStep = this.gridView1.GetFocusedRow() as SingleProcessStep;

            if (singleProcessStep == null)
            {
                AKRSXtraMessageBox.Show("请先选择步骤");
                return;
            }

            if (singleProcessStep.Index <= 1)
            {
                AKRSXtraMessageBox.Show("当前步骤为第一步，无法继续上升");
                return;
            }

            int currentIndex = singleProcessStep.Index;

            SingleProcessStep singleProcessStepNext = this.SingleProcessSteps.FirstOrDefault(x => x.Index == currentIndex - 1);

            if (singleProcessStepNext != null)
            {
                singleProcessStepNext.Index += 1;
            }

            singleProcessStep.Index--;
            this.RefreshData();
            this.gridView1.FocusedRowHandle = singleProcessStep.Index - 1;
        }

        /// <summary>
        /// 下降
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtDown_Click(object sender, EventArgs e)
        {
            SingleProcessStep singleProcessStep = this.gridView1.GetFocusedRow() as SingleProcessStep;

            if (singleProcessStep == null)
            {
                AKRSXtraMessageBox.Show("请先选择步骤");
                return;
            }

            if (singleProcessStep.Index == this.SingleProcessSteps.Count)
            {
                AKRSXtraMessageBox.Show("当前步骤为最后一个，无法继续下降");
                return;
            }

            int currentIndex = singleProcessStep.Index;

            SingleProcessStep singleProcessStepNext = this.SingleProcessSteps.FirstOrDefault(x => x.Index == currentIndex + 1);

            if (singleProcessStepNext != null)
            {
                singleProcessStepNext.Index -= 1;
            }

            singleProcessStep.Index++;
            this.RefreshData();
            this.gridView1.FocusedRowHandle = singleProcessStep.Index - 1;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtDelete_Click(object sender, EventArgs e)
        {
            SingleProcessStep singleProcessStep = this.gridView1.GetFocusedRow() as SingleProcessStep;

            if (singleProcessStep == null)
            {
                AKRSXtraMessageBox.Show("请先选择步骤");
                return;
            }

            this.SingleProcessSteps.Remove(singleProcessStep);

            this.gridView1.SelectRow(singleProcessStep.Index - 1);

            foreach (SingleProcessStep singleProcess in this.SingleProcessSteps)
            {
                if (singleProcess.Index > singleProcessStep.Index)
                {
                    singleProcess.Index--;
                }
            }

            this.RefreshData();
        }

        /// <summary>
        /// 选择事件发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void LsBondPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.LsBondPosition.SelectedItem == null || this.CurrentSingleProcessStep == null)
            {
                return;
            }

            this.CurrentSingleProcessStep.BondPositionName = this.LsBondPosition.SelectedItem.ToString();
            ProcessDomain.Save();
        }

        /// <summary>
        /// 选择事件发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void LsComponentName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.LsComponentName.SelectedItem == null || 
                this.CurrentSingleProcessStep == null
                || (string)this.LsComponentName.SelectedItem == "Null")
            {
                return;
            }

            this.CurrentSingleProcessStep.ComponentName = this.LsComponentName.SelectedItem.ToString();
            this.CurrentSingleProcessStep.DefectName = "Null";
            this.CurrentSingleProcessStep.EpoxyNameApplicationName = "Null";
            this.LsEpoxyApplication.SelectedItem = "Null";
            this.LsPostBondInspection.SelectedItem = "Null";
            ProcessDomain.Save();
        }

        /// <summary>
        /// 选择事件发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void LsPostBondInspection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.LsPostBondInspection.SelectedItem == null 
                || this.CurrentSingleProcessStep == null
                || (string)this.LsPostBondInspection.SelectedItem == "Null")
            {
                return;
            }

            this.CurrentSingleProcessStep.DefectName = this.LsPostBondInspection.SelectedItem.ToString();
            this.CurrentSingleProcessStep.ComponentName = "Null";
            this.CurrentSingleProcessStep.EpoxyNameApplicationName = "Null";

            this.LsEpoxyApplication.SelectedItem = "Null";
            this.LsComponentName.SelectedItem = "Null";
            ProcessDomain.Save();
        }

        /// <summary>
        /// 选择事件发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void LsEpoxyApplication_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.LsEpoxyApplication.SelectedItem == null 
                || this.CurrentSingleProcessStep == null
                || (string)this.LsEpoxyApplication.SelectedItem == "Null")
            {
                return;
            }

            this.CurrentSingleProcessStep.EpoxyNameApplicationName = this.LsEpoxyApplication.SelectedItem.ToString();
            this.CurrentSingleProcessStep.ComponentName = "Null";
            this.CurrentSingleProcessStep.DefectName = "Null";
            this.LsComponentName.SelectedItem = "Null";
            this.LsPostBondInspection.SelectedItem = "Null";
            ProcessDomain.Save();
        }

        /// <summary>
        /// 选择改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void GcProcessStep_FocusedViewChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (this.CurrentSingleProcessStep != null)
            {
                this.LsEpoxyApplication.SelectedItem = "Null";
                this.LsComponentName.SelectedItem = "Null";
                this.LsPostBondInspection.SelectedItem = "Null";

                this.LsBondPosition.SelectedItem = this.CurrentSingleProcessStep.BondPositionName;
                this.LsComponentName.SelectedItem = this.CurrentSingleProcessStep.ComponentName;
                this.LsPostBondInspection.SelectedItem = this.CurrentSingleProcessStep.DefectName;
                this.LsEpoxyApplication.SelectedItem = this.CurrentSingleProcessStep.EpoxyNameApplicationName;
            }
        }

        /// <summary>
        /// 系统发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void LueSystem_EditValueChanged(object sender, EventArgs e)
        {
            if (this.LueSystem.EditValue == null)
            {
                return;
            }

            this.LueWorkMode.EditValue = this.CurrentSystemProcess.WorkProcess;

            this.LueWorkModeFirst.EditValue = this.CurrentSystemProcess.WorkProcessModule;

            this.SpCycleNumber.EditValue = this.CurrentSystemProcess.WorkCycleNumber;

            this.ChkWorkByNumber.Checked = this.CurrentSystemProcess.IsWorkCycleNumber;

            this.RefreshData();
        }

        /// <summary>
        /// 系统发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void LueWorkMode_EditValueChanged(object sender, EventArgs e)
        {
            if (this.LueWorkMode.EditValue == null || this.isLoad)
            {
                return;
            }

            this.CurrentSystemProcess.WorkProcess = (WorkProcessEnum)this.LueWorkMode.EditValue;
            
            this.ProcessDomain.Save();

            this.IsEnableToEditCycleNumber();
        }
        
        /// <summary>
        /// 保存
        /// </summary>
        public void Confirm()
        {
            if (!ProcessDomain.CheckIsReady())
            {
                throw new Exception("步骤保存失败");
            }

            this.ProcessDomain.Save();
        }

        /// <summary>
        /// 模式选择改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void LueWorkModeFirst_EditValueChanged(object sender, EventArgs e)
        {
            if (this.LueWorkModeFirst.EditValue == null || this.isLoad)
            {
                return;
            }

            this.CurrentSystemProcess.WorkProcessModule = (WorkProcessModuleEnum)this.LueWorkModeFirst.EditValue;
        }

        /// <summary>
        /// 选择发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkWorkByNumber_CheckedChanged(object sender, EventArgs e)
        {
            this.CurrentSystemProcess.IsWorkCycleNumber = this.ChkWorkByNumber.Checked;

            ProcessDomain.GetInstance().Save();

            this.IsEnableToEditCycleNumber();
        }


        /// <summary>
        /// 是否可以编辑个数
        /// </summary>
        private void IsEnableToEditCycleNumber()
        {
            if ((WorkProcessEnum)this.LueWorkMode.EditValue == WorkProcessEnum.TransportFirst
                && ProductDomain.GetInstance().ProductConfig.SubstrateConfig.IsMultiple
                && this.ChkWorkByNumber.Checked)
            {
                this.SpCycleNumber.Enabled = true;
                this.SpCycleNumber.Properties.MaxValue =
                    ProductDomain.GetInstance().ProductConfig.SubstrateConfig.Count;
            }
            else if ((WorkProcessEnum)this.LueWorkMode.EditValue == WorkProcessEnum.SubstrateFirst
                    && ProductDomain.GetInstance().ProductConfig.ModuleConfig.IsMultiple
                    && this.ChkWorkByNumber.Checked)
            {
                this.SpCycleNumber.Enabled = true;
                this.SpCycleNumber.Properties.MaxValue =
                    ProductDomain.GetInstance().ProductConfig.ModuleConfig.Count;
            }
            else
            {
                this.SpCycleNumber.Enabled = false;
            }
        }

        /// <summary>
        /// 个数发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void SpCycleNumber_EditValueChanged(object sender, EventArgs e)
        {
            this.CurrentSystemProcess.WorkCycleNumber = (int)this.SpCycleNumber.Value;
            ProcessDomain.Save();
        }

        /// <summary>
        /// 点击按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void RepositoryItemCheckEdit2_Click(object sender, EventArgs e)
        {
            ProcessDomain.Save();
        }

        /// <summary>
        /// 自动生成步骤
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAutoGenerate_Click(object sender, EventArgs e)
        {
            ProcessDomain.AutoGenerateProcessStep();
            this.RefreshData();
        }

        /// <summary>
        /// 开启所有焊后
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtOpenCloseDefect_Click(object sender, EventArgs e)
        {
            if (this.BtOpenCloseDefect.Text == "打开所有检测")
            {
                this.BtOpenCloseDefect.Appearance.BackColor = SystemColors.Window;
                this.BtOpenCloseDefect.Text = "关闭所有检测";
                foreach (SingleProcessStep singleProcessStep in ProcessDomain.S1SystemProcess.ProcessSteps)
                {
                    if (singleProcessStep.DefectName != "Null")
                    {
                        singleProcessStep.IsEnable = true;
                    }
                }

                foreach (SingleProcessStep singleProcessStep in ProcessDomain.S2SystemProcess.ProcessSteps)
                {
                    if (singleProcessStep.DefectName != "Null")
                    {
                        singleProcessStep.IsEnable = true;
                    }
                }
            }
            else
            {
                foreach (SingleProcessStep singleProcessStep in ProcessDomain.S1SystemProcess.ProcessSteps)
                {
                    if (singleProcessStep.DefectName != "Null")
                    {
                        singleProcessStep.IsEnable = false;
                    }
                }

                foreach (SingleProcessStep singleProcessStep in ProcessDomain.S2SystemProcess.ProcessSteps)
                {
                    if (singleProcessStep.DefectName != "Null")
                    {
                        singleProcessStep.IsEnable = false;
                    }
                }

                this.BtOpenCloseDefect.Appearance.BackColor = Color.Green;
                this.BtOpenCloseDefect.Text = "打开所有检测";
            }

            this.RefreshData();
        }
    }
}
