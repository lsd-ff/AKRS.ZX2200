using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.MachineSupport.Config;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Setting
{
    using AKRS.ZX2200.Controls.ToolControls.Programming;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Product;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Tool;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using DevExpress.XtraEditors;
    using System;
    using UcEditProductProgramming = AKRS.ZX2200.Infrastructure.Controls.Feature.EditProgram.UcEditProductProgram.UcEditProductProgramming;

    /// <summary>
    /// 基板配置
    /// </summary>
    public partial class UcModule : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// Tu的配置对象
        /// </summary>
        private TransportUnitConfig TransportUnitConfig => ProductConfiguration.GetInstance().TransportUnitConfig;

        /// <summary>
        /// 基岛配置
        /// </summary>
        private ModuleConfig moduleConfig;

        /// <summary>
        /// 是不是第一次加载
        /// </summary>
        private bool isFirstLoad = true;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcModule()
        {
            this.InitializeComponent();

            this.moduleConfig = ProductConfiguration.GetInstance().ModuleConfig;
            this.LueAdjustType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<AdjustTypeEnum>();
            this.LueSearchType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<SearchTypeEnum>();
            this.LueModuleMultiplication.Properties.DataSource =
                EnumHelper.ConvertEnumToNameDisplayDto<MultiplicationEnum>();
            this.LueIdentification.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<IdentificationEnum>();

            this.LueModuleMultiplication.Enabled = false;

            this.InitControl();
            this.isFirstLoad = false;
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitControl()
        {
            if (ProductConfiguration.GetInstance() == null)
            {
                return;
            }

            this.ChkIsMultipleModules.Checked = this.moduleConfig.IsMultiple;
            this.ChkIsModuleInkDot.Checked = this.moduleConfig.IsModuleInkDot;
            this.LueAdjustType.EditValue = this.moduleConfig.LocateConfig.AdjustType;
            this.LueSearchType.EditValue = this.moduleConfig.LocateConfig.SearchType;


            this.ChkDistanceCheck.Checked = this.moduleConfig.LocateConfig.DistanceCheck;
            this.SpSpacingTolerance.Value = (decimal)this.moduleConfig.LocateConfig.DistanceTolerance;
            this.ChkIsSymmetrical.Checked = this.moduleConfig.IsSymmetric;
            this.ChkIsCommonDataset.Checked = this.moduleConfig.CommonDataSet;
            this.LueModuleMultiplication.EditValue = this.moduleConfig.Multiplication;

            this.ChkMeasureHeight1.Checked = this.moduleConfig.MeasureHeightInSystem1;
            this.ChkMeasureHeight2.Checked = this.moduleConfig.MeasureHeightInSystem2;
            this.ChkManualDistance.Checked = this.moduleConfig.IsAssistanceDistanceByInput;

            this.ChkMeasureHeight1.Enabled = MachineHardwareConfiguration.GetInstance().IsSystem1Configrated;
            this.ChkMeasureHeight2.Enabled = MachineHardwareConfiguration.GetInstance().IsSystem2ConfigLaserMh;

            this.LueIdentification.EditValue = this.moduleConfig.IdentityConfig.Identification;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            this.moduleConfig.IsMultiple = this.ChkIsMultipleModules.Checked;
            this.moduleConfig.IsModuleInkDot = this.ChkIsModuleInkDot.Checked;
            this.moduleConfig.LocateConfig.AdjustType = (AdjustTypeEnum)this.LueAdjustType.EditValue;
            this.moduleConfig.LocateConfig.SearchType = (SearchTypeEnum)this.LueSearchType.EditValue;
            this.moduleConfig.LocateConfig.DistanceCheck = this.ChkDistanceCheck.Checked;
            this.moduleConfig.LocateConfig.DistanceTolerance = (double)this.SpSpacingTolerance.Value;
            this.moduleConfig.IsSymmetric = this.ChkIsSymmetrical.Checked;
            this.moduleConfig.CommonDataSet = this.ChkIsCommonDataset.Checked;
            this.moduleConfig.Multiplication = (MultiplicationEnum)this.LueModuleMultiplication.EditValue;

            this.moduleConfig.MeasureHeightInSystem1 = this.ChkMeasureHeight1.Checked;
            this.moduleConfig.MeasureHeightInSystem2 = this.ChkMeasureHeight2.Checked;

            this.moduleConfig.IsAssistanceDistanceByInput = this.ChkManualDistance.Checked;

            this.moduleConfig.IdentityConfig.Identification = (IdentificationEnum)this.LueIdentification.EditValue;
        }

        /// <summary>
        /// 多基板设置
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void ChkIsMultipleModules_CheckedChanged(object sender, System.EventArgs e)
        {
            if (this.isFirstLoad)
            {
                return;
            }

            if (this.ChkIsMultipleModules.Checked)
            {
                this.LueModuleMultiplication.Enabled = true;
                TransportUnitConfig.ModulePosition.State = AssistantStateEnum.UnAble;
                this.moduleConfig.IsMultiple = true;
            }
            else
            {
                TransportUnitConfig.ModulePosition.State = AssistantStateEnum.ForBidden;
                this.LueModuleMultiplication.Enabled = false;
                this.moduleConfig.IsMultiple = false;
            }

            UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(TransportUnitConfig);
            this.Save();
        }

        /// <summary>
        /// 定位方式发生改变
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void LueAdjustType_EditValueChanged(object sender, System.EventArgs e)
        {
            if (this.isFirstLoad)
            {
                return;
            }

            if ((AdjustTypeEnum)this.LueAdjustType.EditValue == AdjustTypeEnum.None)
            {
                this.LueSearchType.Enabled = false;
                this.ChkDistanceCheck.Enabled = false;
                this.ChkIsCommonDataset.Enabled = false;
                this.ChkIsSymmetrical.Enabled = false;
                this.SpSpacingTolerance.Enabled = false;
                this.ChkDistanceCheck.Checked = false;
                this.TransportUnitConfig.ModuleAdjust.State = AssistantStateEnum.ForBidden;
            }
            else if ((AdjustTypeEnum)this.LueAdjustType.EditValue == AdjustTypeEnum.OnePoint)
            {
                this.LueSearchType.Enabled = true;
                this.ChkDistanceCheck.Enabled = false;
                this.ChkIsCommonDataset.Enabled = false;
                this.ChkIsSymmetrical.Enabled = false;
                this.SpSpacingTolerance.Enabled = false;
                this.ChkDistanceCheck.Checked = false;
                this.TransportUnitConfig.ModuleAdjust.State = AssistantStateEnum.UnAble;
            }
            else if ((AdjustTypeEnum)this.LueAdjustType.EditValue == AdjustTypeEnum.TwoPoints)
            {
                this.LueSearchType.Enabled = true;
                this.ChkDistanceCheck.Enabled = true;
                this.ChkIsCommonDataset.Enabled = true;
                this.ChkIsSymmetrical.Enabled = true;
                this.SpSpacingTolerance.Enabled = false;
                this.ChkDistanceCheck.Checked = false;
                this.TransportUnitConfig.ModuleAdjust.State = AssistantStateEnum.UnAble;
            }
            else if ((AdjustTypeEnum)this.LueAdjustType.EditValue == AdjustTypeEnum.ThreePoints)
            {
                this.LueSearchType.Enabled = true;
                this.ChkDistanceCheck.Enabled = false;
                this.ChkIsCommonDataset.Enabled = true;
                this.ChkIsSymmetrical.Enabled = false;
                this.SpSpacingTolerance.Enabled = false;
                this.ChkDistanceCheck.Checked = false;
                this.TransportUnitConfig.ModuleAdjust.State = AssistantStateEnum.UnAble;
            }

            this.moduleConfig.LocateConfig.AdjustType = (AdjustTypeEnum)this.LueAdjustType.EditValue;
            this.Save();
            UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(TransportUnitConfig);
        }

        /// <summary>
        /// 距离检查
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void ChkDistanceCheck_EditValueChanged(object sender, System.EventArgs e)
        {
            if (this.isFirstLoad)
            {
                return;
            }

            if (this.ChkDistanceCheck.Checked)
            {
                this.SpSpacingTolerance.Enabled = true;
            }
            else
            {
                this.SpSpacingTolerance.Enabled = false;
            }
        }

        /// <summary>
        /// 定位的使用信息配置
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtLocateUseInfo_Click(object sender, EventArgs e)
        {
            this.Save();
            FrmLocateUseInfo frmLocate = new FrmLocateUseInfo(this.moduleConfig.LocateConfig);
            frmLocate.ShowDialog();
        }

        /// <summary>
        /// 系统1测高
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkMeasureHeight1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.isFirstLoad)
            {
                return;
            }

            if (!this.ChkMeasureHeight1.Checked && !this.ChkMeasureHeight2.Checked)
            {
                this.TransportUnitConfig.ModuleHeightMeasurement.State = AssistantStateEnum.ForBidden;
                UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(this.TransportUnitConfig);
            }
            else
            {
                this.TransportUnitConfig.ModuleHeightMeasurement.State = AssistantStateEnum.UnAble;
                UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(this.TransportUnitConfig);
            }

            this.moduleConfig.MeasureHeightInSystem1 = this.ChkMeasureHeight1.Checked;
            this.Save();
        }

        /// <summary>
        /// 系统2测高
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkMeasureHeight2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.isFirstLoad)
            {
                return;
            }

            if (!this.ChkMeasureHeight1.Checked && !this.ChkMeasureHeight2.Checked)
            {
                this.TransportUnitConfig.ModuleHeightMeasurement.State = AssistantStateEnum.ForBidden;
                UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(this.TransportUnitConfig);
            }
            else
            {
                this.TransportUnitConfig.ModuleHeightMeasurement.State = AssistantStateEnum.UnAble;
                UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(this.TransportUnitConfig);
            }

            this.moduleConfig.MeasureHeightInSystem2 = this.ChkMeasureHeight2.Checked;
            this.Save();
        }

        /// <summary>
        /// 修改测高点位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtChangeMeasureHeightPoint_Click(object sender, EventArgs e)
        {
            if (this.moduleConfig.HeightMeasurementPoints.Count == 0)
            {
                AKRSXtraMessageBox.Show("请先在配置界面开启测高并示教点位！");
                return;
            }

            FrmMeasureHeightPoints frmMeasureHeightPoints = new FrmMeasureHeightPoints(this.moduleConfig);
            frmMeasureHeightPoints.ShowDialog();
            this.Save();
        }

        /// <summary>
        /// 手动输入间距
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkManualDistance_CheckedChanged(object sender, EventArgs e)
        {
            this.moduleConfig.IsAssistanceDistanceByInput = this.ChkManualDistance.Checked;
        }

        /// <summary>
        /// 身份识别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LueIdentification_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isFirstLoad)
            {
                return;
            }

            if ((IdentificationEnum)this.LueIdentification.EditValue == IdentificationEnum.Off)
            {
                this.TransportUnitConfig.ModuleIdentification.State = AssistantStateEnum.ForBidden;
            }
            else
            {
                this.TransportUnitConfig.ModuleIdentification.State = AssistantStateEnum.UnAble;
            }

            UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(this.TransportUnitConfig);
            this.Save();
        }
    }
}
