using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.MachineSupport.Config;
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

    using UcEditProductProgramming = AKRS.ZX2200.Infrastructure.Controls.Feature.EditProgram.UcEditProductProgram.UcEditProductProgramming;

    /// <summary>
    /// 基板
    /// </summary>
    public partial class UcSubstrate : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// transport unit
        /// </summary>
        private readonly SubstrateConfig substrateConfig;

        /// <summary>
        /// Tu的配置对象
        /// </summary>
        private TransportUnitConfig TransportUnitConfig => ProductConfiguration.GetInstance().TransportUnitConfig;

        /// <summary>
        /// 是不是第一次加载
        /// </summary>
        private bool isFirstLoad = true;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcSubstrate()
        {
            this.InitializeComponent();

            this.substrateConfig = ProductConfiguration.GetInstance().SubstrateConfig;

            if (this.substrateConfig == null)
            {
                return;
            }

            this.InitControl();
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitControl()
        {
            this.LueAdjustType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<AdjustTypeEnum>();
            this.LueSearchType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<SearchTypeEnum>();
            this.LueSubstrateIdentification.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<IdentificationEnum>();
            this.LueSubstrateMultiplication.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<MultiplicationEnum>();
            this.LueSubstrateMultiplication.Enabled = false;

            this.LueAdjustType.EditValue = AdjustTypeEnum.None;
            this.LueSearchType.EditValue = SearchTypeEnum.StandardSearch;
            this.LueSubstrateIdentification.EditValue = IdentificationEnum.Off;
            this.LueSubstrateMultiplication.EditValue = MultiplicationEnum.Matrix;
            this.LoadData();
            this.isFirstLoad = false;
        }

        /// <summary>
        /// 加载参数
        /// </summary>
        private void LoadData()
        {
            this.ChkIsMultipleSubstrates.Checked = this.substrateConfig.IsMultiple;
            this.ChkIsSubstrateInkDot.Checked = this.substrateConfig.SubstrateInkDot;
            this.LueAdjustType.EditValue = this.substrateConfig.LocateConfig.AdjustType;
            this.LueSearchType.EditValue = this.substrateConfig.LocateConfig.SearchType;
            this.ChkDistanceCheck.Checked = this.substrateConfig.LocateConfig.DistanceCheck;
            this.SpSpacingTolerance.Value = (decimal)this.substrateConfig.LocateConfig.DistanceTolerance;
            this.ChkIsSymmetrical.Checked = this.substrateConfig.IsSymmetric;
            this.ChkIsCommonDataset.Checked = this.substrateConfig.CommonDataSet;
            this.LueSubstrateIdentification.EditValue = this.substrateConfig.IdentityConfig.Identification;
            this.ChkMapping.Checked = this.substrateConfig.MappingEnable;
            this.LueSubstrateMultiplication.EditValue = this.substrateConfig.Multiplication;

            this.ChkMeasureHeight1.Checked = this.substrateConfig.MeasureHeightInSystem1;
            this.ChkMeasureHeight2.Checked = this.substrateConfig.MeasureHeightInSystem2;

            this.ChkManualDistance.Checked = this.substrateConfig.IsAssistanceDistanceByInput;

            this.ChkMeasureHeight1.Enabled = MachineHardwareConfiguration.GetInstance().IsSystem1Configrated;
            this.ChkMeasureHeight2.Enabled = MachineHardwareConfiguration.GetInstance().IsSystem2ConfigLaserMh;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            this.substrateConfig.IsMultiple = this.ChkIsMultipleSubstrates.Checked;
            this.substrateConfig.SubstrateInkDot = this.ChkIsSubstrateInkDot.Checked;
            this.substrateConfig.LocateConfig.AdjustType = (AdjustTypeEnum)this.LueAdjustType.EditValue;
            this.substrateConfig.LocateConfig.SearchType = (SearchTypeEnum)this.LueSearchType.EditValue;
            this.substrateConfig.LocateConfig.DistanceCheck = this.ChkDistanceCheck.Checked;
            this.substrateConfig.LocateConfig.DistanceTolerance = (double)this.SpSpacingTolerance.Value;
            this.substrateConfig.IsSymmetric = this.ChkIsSymmetrical.Checked;
            this.substrateConfig.CommonDataSet = this.ChkIsCommonDataset.Checked;
            this.substrateConfig.IdentityConfig.Identification = (IdentificationEnum)this.LueSubstrateIdentification.EditValue;
            this.substrateConfig.MappingEnable = this.ChkMapping.Checked;
            this.substrateConfig.Multiplication =
                (MultiplicationEnum)this.LueSubstrateMultiplication.EditValue;

            this.substrateConfig.MeasureHeightInSystem1 = this.ChkMeasureHeight1.Checked;
            this.substrateConfig.MeasureHeightInSystem2 = this.ChkMeasureHeight2.Checked;

            this.substrateConfig.IsAssistanceDistanceByInput = this.ChkManualDistance.Checked;

        }

        /// <summary>
        /// 定位方式发生变化
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void LueAdjustType_EditValueChanged(object sender, EventArgs e)
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
                this.BtLocateUseInfo.Enabled = false;
                this.TransportUnitConfig.SubstrateAdjust.State = AssistantStateEnum.ForBidden;
            }
            else if ((AdjustTypeEnum)this.LueAdjustType.EditValue == AdjustTypeEnum.OnePoint)
            {
                this.LueSearchType.Enabled = true;
                this.ChkDistanceCheck.Enabled = false;
                this.ChkIsCommonDataset.Enabled = false;
                this.ChkIsSymmetrical.Enabled = false;
                this.SpSpacingTolerance.Enabled = false;
                this.ChkDistanceCheck.Checked = false;
                this.BtLocateUseInfo.Enabled = true;
                this.TransportUnitConfig.SubstrateAdjust.State = AssistantStateEnum.UnAble;
            }
            else if ((AdjustTypeEnum)this.LueAdjustType.EditValue == AdjustTypeEnum.TwoPoints)
            {
                this.LueSearchType.Enabled = true;
                this.ChkDistanceCheck.Enabled = true;
                this.ChkIsCommonDataset.Enabled = true;
                this.ChkIsSymmetrical.Enabled = true;
                this.SpSpacingTolerance.Enabled = false;
                this.ChkDistanceCheck.Checked = false;
                this.BtLocateUseInfo.Enabled = true;
                this.TransportUnitConfig.SubstrateAdjust.State = AssistantStateEnum.UnAble;
            }
            else if ((AdjustTypeEnum)this.LueAdjustType.EditValue == AdjustTypeEnum.ThreePoints)
            {
                this.LueSearchType.Enabled = true;
                this.ChkDistanceCheck.Enabled = false;
                this.ChkIsCommonDataset.Enabled = true;
                this.ChkIsSymmetrical.Enabled = false;
                this.SpSpacingTolerance.Enabled = false;
                this.ChkDistanceCheck.Checked = false;
                this.BtLocateUseInfo.Enabled = true;
                this.TransportUnitConfig.SubstrateAdjust.State = AssistantStateEnum.UnAble;
            }

            this.substrateConfig.LocateConfig.AdjustType = (AdjustTypeEnum)this.LueAdjustType.EditValue;
            this.Save();
            UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(TransportUnitConfig);
        }

        /// <summary>
        /// 距离检测
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void ChkDistanceCheck_CheckedChanged(object sender, EventArgs e)
        {
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
        /// 基板启用
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void ChkIsMultipleSubstrates_CheckedChanged(object sender, EventArgs e)
        {
            if (this.isFirstLoad)
            {
                return;
            }

            if (this.ChkIsMultipleSubstrates.Checked)
            {
                this.LueSubstrateMultiplication.Enabled = true;
                this.TransportUnitConfig.SubstratePosition.State = AssistantStateEnum.UnAble;
            }
            else
            {
                this.LueSubstrateMultiplication.Enabled = false;
                this.TransportUnitConfig.SubstratePosition.State = AssistantStateEnum.ForBidden;
            }

            this.substrateConfig.IsMultiple = this.LueSubstrateMultiplication.Enabled;
            UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(this.TransportUnitConfig);
            this.Save();
        }

        /// <summary>
        /// 事件改变
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void LueSubstrateIdentification_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isFirstLoad)
            {
                return;
            }

            if ((IdentificationEnum)this.LueSubstrateIdentification.EditValue == IdentificationEnum.Off)
            {
                this.TransportUnitConfig.SubstrateIdentification.State = AssistantStateEnum.ForBidden;
            }
            else
            {
                this.TransportUnitConfig.SubstrateIdentification.State = AssistantStateEnum.UnAble;
            }

            UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(this.TransportUnitConfig);
            this.Save();
        }

        /// <summary>
        /// 是不是异性基板
        /// </summary>
        /// <returns>结果</returns>
        public bool IsMultipleSubstrates()
        {
            return this.ChkIsMultipleSubstrates.Checked;
        }

        /// <summary>
        /// 定位的使用信息配置
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtLocateUseInfo_Click(object sender, EventArgs e)
        {
            this.Save();
            FrmLocateUseInfo frmLocate = new FrmLocateUseInfo(this.substrateConfig.LocateConfig);
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
                this.TransportUnitConfig.SubstrateHeightMeasurement.State = AssistantStateEnum.ForBidden;
                UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(this.TransportUnitConfig);
            }
            else
            {
                this.TransportUnitConfig.SubstrateHeightMeasurement.State = AssistantStateEnum.UnAble;
                UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(this.TransportUnitConfig);
            }
            
            this.substrateConfig.MeasureHeightInSystem1 = this.ChkMeasureHeight1.Checked;
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
                this.TransportUnitConfig.SubstrateHeightMeasurement.State = AssistantStateEnum.ForBidden;
                UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(this.TransportUnitConfig);
            }
            else
            {
                this.TransportUnitConfig.SubstrateHeightMeasurement.State = AssistantStateEnum.UnAble;
                UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(this.TransportUnitConfig);
            }

            this.substrateConfig.MeasureHeightInSystem2 = this.ChkMeasureHeight2.Checked;
            this.Save();
        }

        /// <summary>
        /// 修改测高点位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtChangeMeasureHeightPoint_Click(object sender, EventArgs e)
        {
            if (this.substrateConfig.HeightMeasurementPoints.Count == 0)
            {
                AKRSXtraMessageBox.Show("请先在配置界面开启测高并示教点位！");
                return;
            }

            FrmMeasureHeightPoints frmMeasureHeightPoints = new FrmMeasureHeightPoints(this.substrateConfig);
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
            this.substrateConfig.IsAssistanceDistanceByInput = this.ChkManualDistance.Checked;
        }

        private void UcSubstrate_Load(object sender, EventArgs e)
        {

        }
    }
}
