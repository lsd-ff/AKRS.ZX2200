using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.ZX2200.Controls.ToolControls.Programming;
using DevExpress.XtraEditors;
using System;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Setting
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Controls.Feature.EditProgram.UcEditProductProgram;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Tool;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;

    using DevExpress.Utils.CommonDialogs.Internal;

    /// <summary>
    /// 载具
    /// </summary>
    public partial class UcTransportUnit : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// transport unit
        /// </summary>
        private TransportUnitConfig transportUnitConfig;

        /// <summary>
        /// 是不是第一次加载
        /// </summary>
        private bool isFirstLoad = true;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcTransportUnit()
        {
            this.InitializeComponent();

            this.Disposed += (s, e) =>
            {
                this.timer1.Stop();
                this.timer1.Tick -= Timer1_Tick;
                this.timer1.Dispose();
                this.timer1 = null;
            };
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitControl()
        {
            if (this.transportUnitConfig == null)
            {
                return;
            }

            this.LueIdentification.EditValue = this.transportUnitConfig.IdentityConfig.Identification;
            //this.ChkMapping.Checked = this.transportUnitConfig.TuMappingEnable;
            //this.LuTumapping.EditValue = this.transportUnitConfig.TuMapping;
            this.ChkAllRotation.Checked = this.transportUnitConfig.AssistantAllRotation;
            this.ChkAllHeight.Checked = this.transportUnitConfig.AssistantAllHeight;
            this.ChkAllForce.Checked = this.transportUnitConfig.AssistantAllForce;
            this.BtMeasureHeight1.Checked = this.transportUnitConfig.MeasureHeightInSystem1;
            this.BtMeasureHeight2.Checked = this.transportUnitConfig.MeasureHeightInSystem2;

            this.LueSubstrateProcessing.EditValue = this.transportUnitConfig.SubstrateProcessing;

            this.LueAdjustType.EditValue = this.transportUnitConfig.LocateConfig.AdjustType;
            this.LueSearchType.EditValue = this.transportUnitConfig.LocateConfig.SearchType;
            this.ChkDistanceCheck.Checked = this.transportUnitConfig.LocateConfig.DistanceCheck;
            this.SpSpacingTolerance.Value = (decimal)this.transportUnitConfig.LocateConfig.DistanceTolerance;
            this.ChkIsSymmetrical.Checked = this.transportUnitConfig.LocateConfig.IsSymmetric;
            this.ChkIsCommonDataset.Checked = this.transportUnitConfig.LocateConfig.CommonDataSet;

            this.LueLeftAdjustType.EditValue = this.transportUnitConfig.LeftLocateConfig.AdjustType;
            this.LueLeftSearchType.EditValue = this.transportUnitConfig.LeftLocateConfig.SearchType;
            this.ChkLeftDistanceCheck.Checked = this.transportUnitConfig.LeftLocateConfig.DistanceCheck;
            this.SpLeftSpacingTolerance.Value = (decimal)this.transportUnitConfig.LeftLocateConfig.DistanceTolerance;
            this.ChkLeftIsSymmetrical.Checked = this.transportUnitConfig.LeftLocateConfig.IsSymmetric;
            this.ChkLeftIsCommonDataset.Checked = this.transportUnitConfig.LeftLocateConfig.CommonDataSet;

            this.LueRightAdjustType.EditValue = this.transportUnitConfig.LocateConfig.AdjustType;
            this.LueRightSearchType.EditValue = this.transportUnitConfig.LocateConfig.SearchType;
            this.ChkRightDistanceCheck.Checked = this.transportUnitConfig.LocateConfig.DistanceCheck;
            this.SpRightSpacingTolerance.Value = (decimal)this.transportUnitConfig.LocateConfig.DistanceTolerance;
            this.ChkRightIsSymmetrical.Checked = this.transportUnitConfig.LocateConfig.IsSymmetric;
            this.ChkRightIsCommonDataset.Checked = this.transportUnitConfig.LocateConfig.CommonDataSet;

            this.LbTransportUnitName.Text = MachineConfigContext.GetInstance().RecipeName;

            this.isFirstLoad = false;

            this.BtMeasureHeight1.Enabled = MachineHardwareConfiguration.GetInstance().IsSystem1Configrated;
            this.BtMeasureHeight2.Enabled = MachineHardwareConfiguration.GetInstance().IsSystem2ConfigLaserMh;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            this.transportUnitConfig.IdentityConfig.Identification = (IdentificationEnum)this.LueIdentification.EditValue;
            //this.transportUnitConfig.TuMappingEnable = this.ChkMapping.Checked;
            //this.transportUnitConfig.TuMapping = (TuMappingEnum)this.LuTumapping.EditValue;
            this.transportUnitConfig.AssistantAllRotation = this.ChkAllRotation.Checked;
            this.transportUnitConfig.AssistantAllHeight = this.ChkAllHeight.Checked;
            this.transportUnitConfig.AssistantAllForce = this.ChkAllForce.Checked;

            this.transportUnitConfig.MeasureHeightInSystem1 = this.BtMeasureHeight1.Checked;
            this.transportUnitConfig.MeasureHeightInSystem2 = this.BtMeasureHeight2.Checked;

            this.transportUnitConfig.SubstrateProcessing =
                (SubstrateProcessingEnum)this.LueSubstrateProcessing.EditValue;

            this.transportUnitConfig.LocateConfig.AdjustType = (AdjustTypeEnum)this.LueAdjustType.EditValue;
            this.transportUnitConfig.LocateConfig.SearchType = (SearchTypeEnum)this.LueSearchType.EditValue;
            this.transportUnitConfig.LocateConfig.DistanceCheck = this.ChkDistanceCheck.Checked;
            this.transportUnitConfig.LocateConfig.DistanceTolerance = (double)this.SpSpacingTolerance.Value;
            this.transportUnitConfig.LocateConfig.IsSymmetric = this.ChkIsSymmetrical.Checked;
            this.transportUnitConfig.LocateConfig.CommonDataSet = this.ChkIsCommonDataset.Checked;

            this.transportUnitConfig.LeftLocateConfig.AdjustType = (AdjustTypeEnum)this.LueLeftAdjustType.EditValue;
            this.transportUnitConfig.LeftLocateConfig.SearchType = (SearchTypeEnum)this.LueLeftSearchType.EditValue;
            this.transportUnitConfig.LeftLocateConfig.DistanceCheck = this.ChkLeftDistanceCheck.Checked;
            this.transportUnitConfig.LeftLocateConfig.DistanceTolerance = (double)this.SpLeftSpacingTolerance.Value;
            this.transportUnitConfig.LeftLocateConfig.IsSymmetric = this.ChkLeftIsSymmetrical.Checked;
            this.transportUnitConfig.LeftLocateConfig.CommonDataSet = this.ChkLeftIsCommonDataset.Checked;

            this.transportUnitConfig.RightLocateConfig.AdjustType = (AdjustTypeEnum)this.LueRightAdjustType.EditValue;
            this.transportUnitConfig.RightLocateConfig.SearchType = (SearchTypeEnum)this.LueRightSearchType.EditValue;
            this.transportUnitConfig.RightLocateConfig.DistanceCheck = this.ChkRightDistanceCheck.Checked;
            this.transportUnitConfig.RightLocateConfig.DistanceTolerance = (double)this.SpRightSpacingTolerance.Value;
            this.transportUnitConfig.RightLocateConfig.IsSymmetric = this.ChkRightIsSymmetrical.Checked;
            this.transportUnitConfig.RightLocateConfig.CommonDataSet = this.ChkRightIsCommonDataset.Checked;
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
                this.transportUnitConfig.TUAdjust.State = AssistantStateEnum.ForBidden;
            }
            else if ((AdjustTypeEnum)this.LueAdjustType.EditValue == AdjustTypeEnum.OnePoint)
            {
                this.LueSearchType.Enabled = true;
                this.ChkDistanceCheck.Enabled = false;
                this.ChkIsCommonDataset.Enabled = false;
                this.ChkIsSymmetrical.Enabled = false;
                this.SpSpacingTolerance.Enabled = false;
                this.transportUnitConfig.TUAdjust.State = AssistantStateEnum.UnAble;
            }
            else if ((AdjustTypeEnum)this.LueAdjustType.EditValue == AdjustTypeEnum.TwoPoints)
            {
                this.LueSearchType.Enabled = true;
                this.ChkDistanceCheck.Enabled = true;
                this.ChkIsCommonDataset.Enabled = true;
                this.ChkIsSymmetrical.Enabled = true;
                this.SpSpacingTolerance.Enabled = false;
                this.transportUnitConfig.TUAdjust.State = AssistantStateEnum.UnAble;
            }
            else if ((AdjustTypeEnum)this.LueAdjustType.EditValue == AdjustTypeEnum.ThreePoints)
            {
                this.LueSearchType.Enabled = true;
                this.ChkDistanceCheck.Enabled = false;
                this.ChkIsCommonDataset.Enabled = true;
                this.ChkIsSymmetrical.Enabled = false;
                this.SpSpacingTolerance.Enabled = false;
                this.transportUnitConfig.TUAdjust.State = AssistantStateEnum.UnAble;
            }

            this.transportUnitConfig.LocateConfig.AdjustType = (AdjustTypeEnum)this.LueAdjustType.EditValue;
            UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(this.transportUnitConfig);
            this.Save();
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
        /// 识别设置
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void LueIdentification_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isFirstLoad)
            {
                return;
            }

            if ((IdentificationEnum)this.LueIdentification.EditValue == IdentificationEnum.Off)
            {
                this.transportUnitConfig.TUIdentification.State = AssistantStateEnum.ForBidden;
            }
            else
            {
                this.transportUnitConfig.TUIdentification.State = AssistantStateEnum.UnAble;
            }

            UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(this.transportUnitConfig);
            this.Save();
        }

        ///// <summary>
        ///// 改变TUMapping
        ///// </summary>
        ///// <param name="sender">事件</param>
        ///// <param name="e">参数</param>
        //private void ChkMapping_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (this.ChkMapping.Checked)
        //    {
        //        this.LuTumapping.Enabled = true;
        //    }
        //    else
        //    {
        //        this.LuTumapping.Enabled = false;
        //    }
        //}

        /// <summary>
        /// 选择对称的
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void ChkIsSymmetrical_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkIsSymmetrical.Checked)
            {
                this.ChkIsCommonDataset.Checked = false;
            }
        }

        /// <summary>
        /// 选择普通的
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void ChkIsCommonDataSet_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkIsCommonDataset.Checked)
            {
                this.ChkIsSymmetrical.Checked = false;
            }
        }

        /// <summary>
        /// 选择分开的
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void LueSubstrateProcessing_EditValueChanged(object sender, EventArgs e)
        {
            //if ((SubstrateProcessingEnum)this.LueSubstrateProcessing.EditValue == SubstrateProcessingEnum.Divide)
            //{
            //    this.groupControl3.Enabled = false;
            //    this.groupControl6.Enabled = false;
            //    this.groupControl2.Enabled = false;
            //    this.LueAdjustType.EditValue = AdjustTypeEnum.None;
            //}
            //else
            //{
            //    this.groupControl3.Enabled = false;
            //    this.groupControl6.Enabled = false;
            //    this.groupControl2.Enabled = true;
            //    this.LueLeftAdjustType.EditValue = AdjustTypeEnum.None;
            //    this.LueRightAdjustType.EditValue = AdjustTypeEnum.None;
            //}
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void UcTransportUnit_Load(object sender, EventArgs e)
        {
            this.transportUnitConfig = ProductConfiguration.GetInstance().TransportUnitConfig;

            this.LueAdjustType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<AdjustTypeEnum>();
            this.LueSearchType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<SearchTypeEnum>();
            this.LueIdentification.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<IdentificationEnum>();
            //this.LuTumapping.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<TuMappingEnum>();
            this.LueSubstrateProcessing.Properties.DataSource =
                EnumHelper.ConvertEnumToNameDisplayDto<SubstrateProcessingEnum>();

            this.LueLeftAdjustType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<AdjustTypeEnum>();
            this.LueRightAdjustType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<AdjustTypeEnum>();
            this.LueLeftSearchType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<SearchTypeEnum>();
            this.LueRightSearchType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<SearchTypeEnum>();
            //this.LuTumapping.Enabled = false;

            // 设置初始值
            this.LueAdjustType.EditValue = AdjustTypeEnum.None;
            this.LueLeftAdjustType.EditValue = AdjustTypeEnum.None;
            this.LueRightAdjustType.EditValue = AdjustTypeEnum.None;
            this.LueSearchType.EditValue = SearchTypeEnum.StandardSearch;
            this.LueLeftSearchType.EditValue = SearchTypeEnum.StandardSearch;
            this.LueRightSearchType.EditValue = SearchTypeEnum.StandardSearch;
            this.LueIdentification.EditValue = IdentificationEnum.Off;
            //this.LuTumapping.EditValue = TuMappingEnum.Substrate;
            this.LueSubstrateProcessing.EditValue = SubstrateProcessingEnum.Complete;
            this.InitControl();
        }

        /// <summary>
        /// 扫描事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            UcTransportUnitFrame ucTransportUnitFrame = (UcTransportUnitFrame)this.Parent.Parent.Parent;

            if (ucTransportUnitFrame.GetSubstrateProcessingEnable())
            {
                this.LueSubstrateProcessing.Enabled = true;
            }
            else
            {
                this.LueSubstrateProcessing.Enabled = false;
                this.LueSubstrateProcessing.EditValue = SubstrateProcessingEnum.Complete;
            }
        }

        /// <summary>
        /// 坐标定位方式发生改变
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void LueLeftAdjustType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((AdjustTypeEnum)this.LueLeftAdjustType.EditValue == AdjustTypeEnum.None)
            {
                this.LueLeftSearchType.Enabled = false;
                this.ChkLeftDistanceCheck.Enabled = false;
                this.ChkLeftIsCommonDataset.Enabled = false;
                this.ChkLeftIsSymmetrical.Enabled = false;
                this.SpLeftSpacingTolerance.Enabled = false;
            }
            else if ((AdjustTypeEnum)this.LueLeftAdjustType.EditValue == AdjustTypeEnum.OnePoint)
            {
                this.LueLeftSearchType.Enabled = true;
                this.ChkLeftDistanceCheck.Enabled = false;
                this.ChkLeftIsCommonDataset.Enabled = false;
                this.ChkLeftIsSymmetrical.Enabled = false;
                this.SpLeftSpacingTolerance.Enabled = false;
            }
            else if ((AdjustTypeEnum)this.LueLeftAdjustType.EditValue == AdjustTypeEnum.TwoPoints)
            {
                this.LueLeftSearchType.Enabled = true;
                this.ChkLeftDistanceCheck.Enabled = true;
                this.ChkLeftIsCommonDataset.Enabled = true;
                this.ChkLeftIsSymmetrical.Enabled = true;
                this.SpLeftSpacingTolerance.Enabled = false;
            }
            else if ((AdjustTypeEnum)this.LueLeftAdjustType.EditValue == AdjustTypeEnum.ThreePoints)
            {
                this.LueLeftSearchType.Enabled = true;
                this.ChkLeftDistanceCheck.Enabled = false;
                this.ChkLeftIsCommonDataset.Enabled = true;
                this.ChkLeftIsSymmetrical.Enabled = false;
                this.SpLeftSpacingTolerance.Enabled = false;
            }
        }

        /// <summary>
        /// 左边距离检查
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void ChkLeftDistanceCheck_EditValueChanged(object sender, EventArgs e)
        {
            if (this.ChkLeftDistanceCheck.Checked)
            {
                this.SpLeftSpacingTolerance.Enabled = true;
            }
            else
            {
                this.SpLeftSpacingTolerance.Enabled = false;
            }
        }

        /// <summary>
        /// 坐标定位方式发生改变
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void LueRightAdjustType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((AdjustTypeEnum)this.LueRightAdjustType.EditValue == AdjustTypeEnum.None)
            {
                this.LueRightSearchType.Enabled = false;
                this.ChkRightDistanceCheck.Enabled = false;
                this.ChkRightIsCommonDataset.Enabled = false;
                this.ChkRightIsSymmetrical.Enabled = false;
                this.SpRightSpacingTolerance.Enabled = false;
            }
            else if ((AdjustTypeEnum)this.LueRightAdjustType.EditValue == AdjustTypeEnum.OnePoint)
            {
                this.LueRightSearchType.Enabled = true;
                this.ChkRightDistanceCheck.Enabled = false;
                this.ChkRightIsCommonDataset.Enabled = false;
                this.ChkRightIsSymmetrical.Enabled = false;
                this.SpRightSpacingTolerance.Enabled = false;
            }
            else if ((AdjustTypeEnum)this.LueRightAdjustType.EditValue == AdjustTypeEnum.TwoPoints)
            {
                this.LueRightSearchType.Enabled = true;
                this.ChkRightDistanceCheck.Enabled = true;
                this.ChkRightIsCommonDataset.Enabled = true;
                this.ChkRightIsSymmetrical.Enabled = true;
                this.SpRightSpacingTolerance.Enabled = false;
            }
            else if ((AdjustTypeEnum)this.LueRightAdjustType.EditValue == AdjustTypeEnum.ThreePoints)
            {
                this.LueRightSearchType.Enabled = true;
                this.ChkRightDistanceCheck.Enabled = false;
                this.ChkRightIsCommonDataset.Enabled = true;
                this.ChkRightIsSymmetrical.Enabled = false;
                this.SpRightSpacingTolerance.Enabled = false;
            }
        }

        /// <summary>
        /// 右边距离检查
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void ChkRightDistanceCheck_EditValueChanged(object sender, EventArgs e)
        {
            if (this.ChkRightDistanceCheck.Checked)
            {
                this.SpRightSpacingTolerance.Enabled = true;
            }
            else
            {
                this.SpRightSpacingTolerance.Enabled = false;
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
            FrmLocateUseInfo frmLocate = new FrmLocateUseInfo(this.transportUnitConfig.LocateConfig);
            frmLocate.TopMost = true;
            System.Windows.Forms.DialogResult dialogResult = frmLocate.ShowDialog();
        }


        /// <summary>
        /// 左边定位的使用信息配置
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtLocateUseInfoLeft_Click(object sender, EventArgs e)
        {
            this.Save();
            FrmLocateUseInfo frmLocate = new FrmLocateUseInfo(this.transportUnitConfig.LeftLocateConfig);
            frmLocate.ShowDialog();
        }

        /// <summary>
        /// 右边定位的使用信息配置
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtLocateUseInfoRight_Click(object sender, EventArgs e)
        {
            this.Save();
            FrmLocateUseInfo frmLocate = new FrmLocateUseInfo(this.transportUnitConfig.RightLocateConfig);
            frmLocate.ShowDialog();
        }

        /// <summary>
        /// 系统1测高
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">封装参数</param>
        private void BtMeasureHeight1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.isFirstLoad)
            {
                return;
            }

            if (!this.BtMeasureHeight1.Checked && !this.BtMeasureHeight2.Checked)
            {
                this.transportUnitConfig.TUHeightMeasurement.State = AssistantStateEnum.ForBidden;
                UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(this.transportUnitConfig);
            }
            else
            {
                this.transportUnitConfig.TUHeightMeasurement.State = AssistantStateEnum.Able;

                this.transportUnitConfig.TUHeightMeasurement.State = AssistantStateEnum.UnAble;
                UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(this.transportUnitConfig);
            }

            this.transportUnitConfig.MeasureHeightInSystem1 = this.BtMeasureHeight1.Checked;
            this.Save();
        }

        /// <summary>
        /// 系统2测高
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">封装参数</param>
        private void BtMeasureHeight2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.isFirstLoad)
            {
                return;
            }

            if (!this.BtMeasureHeight1.Checked && !this.BtMeasureHeight2.Checked)
            {
                this.transportUnitConfig.TUHeightMeasurement.State = AssistantStateEnum.ForBidden;
                UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(this.transportUnitConfig);
            }
            else
            {
                this.transportUnitConfig.TUHeightMeasurement.State = AssistantStateEnum.UnAble;
                UcEditProductProgramming.RefreshILstAssistantStepWithTuAction(this.transportUnitConfig);
            }

            this.transportUnitConfig.MeasureHeightInSystem1 = this.BtMeasureHeight2.Checked;
            this.Save();
        }

        /// <summary>
        /// 修改测高位置
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtChangeMeasureHeightPoint_Click(object sender, EventArgs e)
        {
            if (this.transportUnitConfig.HeightMeasurementPoints.Count == 0)
            {
                AKRSXtraMessageBox.Show("请先在配置界面开启测高并示教点位！");
                return;
            }

            FrmMeasureHeightPoints frmMeasureHeightPoints = new FrmMeasureHeightPoints(this.transportUnitConfig);
            frmMeasureHeightPoints.ShowDialog();
            this.Save();
        }
    }
}
