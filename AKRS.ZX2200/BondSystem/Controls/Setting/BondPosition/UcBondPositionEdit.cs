using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Controls.Assistant;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.Controls.ToolControls.Programming;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Config;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;
using static AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool.FrmMultipleHeightMeasurementType;

namespace AKRS.ZX2200.BondSystem.Controls.Setting.BondPosition
{
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Common;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Controls.Feature.EditProgram.UcEditProductProgram;
    using AKRS.ZX2200.Infrastructure.Interface;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Setting;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Tool;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    /// <summary>
    /// 焊点编辑界面
    /// </summary>
    public partial class UcBondPositionEdit : DevExpress.XtraEditors.XtraUserControl, IProgrammingControl
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="singleBondPositionConfig">传入的焊点对象</param>
        public UcBondPositionEdit(SingleBondPositionConfig singleBondPositionConfig)
        {
            this.bondPositionConfigure = singleBondPositionConfig;
            this.InitializeComponent();
            this.InitControl();
            this.LueAdjustType.EditValueChanged += new System.EventHandler(this.LueAdjustType_EditValueChanged);
            this.ChkMeasureHeightInSystem2.CheckedChanged += new System.EventHandler(this.ChkMeasureHeightInSystem2_CheckedChanged);
            this.ChkMeasureHeightInSystem1.CheckedChanged += new System.EventHandler(this.ChkMeasureHeightInSystem1_CheckedChanged);
            this.LueIdentification.EditValueChanged += new System.EventHandler(this.LueIdentification_EditValueChanged);
        }

        /// <summary>
        /// 编辑的焊点对象
        /// </summary>
        private SingleBondPositionConfig bondPositionConfigure;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// 吸嘴架控制器
        /// </summary>
        private NozzleShelfController nozzleShelfController = new NozzleShelfController();

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 是否初始化
        /// </summary>
        private bool isInit = false;

        /// <summary>
        /// 焊点高度示教按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnHeightTeach_Click(object sender, EventArgs e)
        {
            // 获取载台上的TU对象
            if (TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit == null)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"系统2载具为空，请先示教载具!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // 自动换Touchdown
            bool ret = this.system2Controller.ChangeTouchDownAssistance();
            if (!ret)
            {
                return;
            }

            // 判断测高方式
            FrmChooseMeasureHeightType frm = new FrmChooseMeasureHeightType();

            if (frm.ShowDialog() == DialogResult.Cancel)
            {
                frm.DialogResult = DialogResult.Cancel;
                return;
            }

            // TouchDown自动移动焊点测高，相机就是手动移动
            if (frm.Type == MultipleHeightMeasurementType.TouchDown)
            {
                // 移动到焊点示教位
                AKRSPoint3D teachPos = System2TUService.GetBondPositionTeachPos(
                    (int)this.SpSubstrateNum.Value,
                    (int)this.SpModuleNum.Value,
                    this.bondPositionConfigure.Name);

                // TouchDown与旋转中心偏移量
                AKRSPoint2D offset = CalibrateRunPara.GetInstance().BondCenterToTouchDownOffset;

                // 测高位置在示教位上方5mm
                AKRSPoint3D pos = teachPos + new AKRSPoint3D(-offset.X, -offset.Y, 5);

              this.bondModuleController.MoveSafeBondXYZ(pos);
            }

            FrmBondPositionHeightTeach frmBondPositionHeightTeach = new FrmBondPositionHeightTeach(this.bondPositionConfigure);
            frmBondPositionHeightTeach.ShowDialog();

            frmBondPositionHeightTeach.Dispose();

            // 刷新界面参数
            this.InitControl();

            // 自动归还Touchdown
            this.system2Controller.PutbackNozzleAssitance();
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        public void InitControl()
        {
            #region  控件绑定数据源

            //this.lBComponent.Items.Clear();

            //// 遍历芯片库,应该不是遍历所有而是筛选出当前配方里面的芯片！
            //foreach (BaseCarrierConfig item in CarrierConfigRepository.GetInstance().BaseDsSettingList)
            //{
            //    this.lBComponent.Items.Add(item.Name);
            //}

            this.LueAdjustType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<AdjustTypeEnum>();
            this.LueSearchType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<SearchTypeEnum>();
            this.LueIdentification.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<IdentificationEnum>();
            //LueRelativeBonding.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<RelativeBondingEnum>();
            //this.CmbRelativeBondingTwoSearch.Properties.Items.Clear();
            // List<string> list = Enum.GetValues(typeof(SearchTypeEnum)).Cast<SearchTypeEnum>().Select(x => x.ToString()).ToList();
            //this.CmbRelativeBondingTwoSearch.Properties.Items.AddRange(list);

            #endregion

            #region 参数赋值

            this.LueIdentification.EditValue = this.bondPositionConfigure.IdentityConfig.Identification;

            this.SpHeight.Value = (decimal)this.bondPositionConfigure.ElementCoordinate.Point.Z;
            this.SpRotaryPosition.Value = (decimal)this.bondPositionConfigure.RotaryPosition;
            //this.lBComponent.SelectedValue = this.bondPositionConfigure.ComponentName;
            this.ChkRelationship.Checked = this.bondPositionConfigure.RelativeBondPosition;
            this.ChkDistanceCheck.Checked = this.bondPositionConfigure.LocateConfig.DistanceCheck;
            this.SpSpacingTolerance.Value = (decimal)this.bondPositionConfigure.LocateConfig.DistanceTolerance;

            this.SpBondOffsetX.Value = (decimal)this.bondPositionConfigure.BondPosOffset.X;
            this.SpBondOffsetY.Value = (decimal)this.bondPositionConfigure.BondPosOffset.Y;
            this.SpBondOffsetZ.Value = (decimal)this.bondPositionConfigure.BondPosOffset.Z;

            // 定位参数赋值
            this.LueAdjustType.EditValue = this.bondPositionConfigure.LocateConfig.AdjustType;
            this.LueSearchType.EditValue = this.bondPositionConfigure.LocateConfig.SearchType;

            this.ChkMeasureHeightInSystem1.Checked = this.bondPositionConfigure.MeasureHeightInSystem1;
            this.ChkMeasureHeightInSystem2.Checked = this.bondPositionConfigure.MeasureHeightInSystem2;

            this.ChkAssistanceAngle.Checked = this.bondPositionConfigure.AssistanceAngle;

            this.ChkMeasureHeightInSystem1.Enabled = MachineHardwareConfiguration.GetInstance().IsSystem1Configrated;
            this.ChkMeasureHeightInSystem2.Enabled = MachineHardwareConfiguration.GetInstance().IsSystem2ConfigLaserMh;

            #endregion

            #region 界面防呆

            //this.ChkCreateSteps.Checked = false;
            //this.CmbRelativeBondingTwoSearch.Enabled = false;
            //this.BtnConfiguration.Enabled = false;
            this.BtnHeightTeach.Enabled = true;

            this.SpModuleNum.Properties.MinValue = 1;
            this.SpSubstrateNum.Properties.MinValue = 1;

            this.SpSubstrateNum.Properties.MaxValue = ProductConfiguration.GetInstance().SubstrateConfig.ColumnCount * ProductConfiguration.GetInstance().SubstrateConfig.RowCount;
            this.SpModuleNum.Properties.MaxValue = ProductConfiguration.GetInstance().ModuleConfig.ColumnCount * ProductConfiguration.GetInstance().ModuleConfig.RowCount;

            if (this.bondPositionConfigure.LocateConfig.AdjustType == AdjustTypeEnum.None)
            {
                this.bondPositionConfigure.BondPositionAdjust.State = AssistantStateEnum.ForBidden;
            }

            //if (!BondConfiguration.GetInstance().IsActivateRelativeBond)
            //{
            //    this.groupControl6.Enabled = false;
            //    this.groupControl6.Visible = false;
            //}
            //else
            //{
            //    this.groupControl6.Enabled = true;
            //    this.groupControl6.Visible = true;
            //}

            //if (this.ChkCreateSteps.Checked == true)
            //{
            //    this.gCComponent.Enabled = true;
            //    this.gCEpoxyApplication.Enabled = true;
            //}
            //else
            //{
            //    this.gCComponent.Enabled = false;
            //    this.gCEpoxyApplication.Enabled = false;
            //}

            //if (this.CmbRelativeBondingTwoSearch.Text == @"MultipleSearch")
            //{
            //    this.BtnConfiguration.Enabled = true;
            //}
            //else
            //{
            //    this.BtnConfiguration.Enabled = false;
            //}

            //if (this.LueRelativeBonding.Text == @"TwoSearch")
            //{
            //    this.CmbRelativeBondingTwoSearch.Enabled = true;
            //}
            //else
            //{
            //    this.CmbRelativeBondingTwoSearch.Enabled = false;
            //}
            #endregion

            UcEditProductProgramming.RefreshILstAssistantStepWithBpAction(this.bondPositionConfigure);
            this.ChkMeasureHeightInSystem2.Enabled = MachineHardwareConfiguration.GetInstance().IsSystem2ConfigLaserMh;
            isInit = true;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            // 保存参数
            this.bondPositionConfigure.RotaryPosition = (double)this.SpRotaryPosition.Value;
            this.bondPositionConfigure.TeachModuleNum = (int)this.SpModuleNum.Value;
            this.bondPositionConfigure.TeachSubstrateNum = (int)this.SpSubstrateNum.Value;

            //this.bondPositionConfigure.BondPosOffset.X = (double)this.SpBondOffsetX.Value;
            //this.bondPositionConfigure.BondPosOffset.Y = (double)this.SpBondOffsetY.Value;
            //this.bondPositionConfigure.BondPosOffset.Z = (double)this.SpBondOffsetZ.Value;

            this.bondPositionConfigure.BondPosOffset = new AKRSPoint3D(
                (double)this.SpBondOffsetX.Value,
                (double)this.SpBondOffsetY.Value,
                (double)this.SpBondOffsetZ.Value);

            this.bondPositionConfigure.LocateConfig.AdjustType = (AdjustTypeEnum)this.LueAdjustType.EditValue;
            this.bondPositionConfigure.LocateConfig.SearchType = (SearchTypeEnum)this.LueSearchType.EditValue;
            this.bondPositionConfigure.LocateConfig.DistanceCheck = this.ChkDistanceCheck.Checked;
            this.bondPositionConfigure.LocateConfig.DistanceTolerance = (double)this.SpSpacingTolerance.Value;
            this.bondPositionConfigure.RelativeBondPosition = this.ChkRelationship.Checked;

            this.bondPositionConfigure.MeasureHeightInSystem1 = this.ChkMeasureHeightInSystem1.Checked;
            this.bondPositionConfigure.MeasureHeightInSystem2 = this.ChkMeasureHeightInSystem2.Checked;

            if (this.bondPositionConfigure.LocateConfig.AdjustType == AdjustTypeEnum.None)
            {
                this.bondPositionConfigure.BondPositionAdjust.State = AssistantStateEnum.ForBidden;
            }

            this.bondPositionConfigure.ElementCoordinate.Point.Z = (double)this.SpHeight.Value;

            this.bondPositionConfigure.AssistanceAngle = this.ChkAssistanceAngle.Checked;

            this.bondPositionConfigure.IdentityConfig.Identification = (IdentificationEnum)this.LueIdentification.EditValue;

            // 保存当前载具
            ProductConfiguration.GetInstance().Save();
        }

        /// <summary>
        /// 确定
        /// </summary>
        public void Confirm()
        {
            this.Save();
        }

        /// <summary>
        /// 定位方式发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void LueAdjustType_EditValueChanged(object sender, EventArgs e)
        {
            if ((AdjustTypeEnum)this.LueAdjustType.EditValue == AdjustTypeEnum.None)
            {
                this.LueSearchType.Enabled = false;
                this.ChkDistanceCheck.Enabled = false;
                this.ChkRelationship.Enabled = false;
                this.SpSpacingTolerance.Enabled = false;
                this.bondPositionConfigure.BondPositionAdjust.State = AssistantStateEnum.ForBidden;
            }
            else if ((AdjustTypeEnum)this.LueAdjustType.EditValue == AdjustTypeEnum.OnePoint)
            {
                this.LueSearchType.Enabled = true;
                this.ChkDistanceCheck.Enabled = false;
                this.ChkRelationship.Enabled = true;
                this.SpSpacingTolerance.Enabled = false;
                this.bondPositionConfigure.BondPositionAdjust.State = AssistantStateEnum.UnAble;
            }
            else if ((AdjustTypeEnum)this.LueAdjustType.EditValue == AdjustTypeEnum.TwoPoints)
            {
                this.LueSearchType.Enabled = true;
                this.ChkDistanceCheck.Enabled = true;
                this.ChkRelationship.Enabled = true;
                this.SpSpacingTolerance.Enabled = false;
                this.bondPositionConfigure.BondPositionAdjust.State = AssistantStateEnum.UnAble;
            }
            else if ((AdjustTypeEnum)this.LueAdjustType.EditValue == AdjustTypeEnum.ThreePoints)
            {
                this.LueSearchType.Enabled = true;
                this.ChkDistanceCheck.Enabled = false;
                this.ChkRelationship.Enabled = true;
                this.SpSpacingTolerance.Enabled = false;
                this.bondPositionConfigure.BondPositionAdjust.State = AssistantStateEnum.UnAble;
            }

            this.bondPositionConfigure.LocateConfig.AdjustType = (AdjustTypeEnum)this.LueAdjustType.EditValue;
            this.Save();
            UcEditProductProgramming.RefreshILstAssistantStepWithBpAction(this.bondPositionConfigure);
        }

        /// <summary>
        /// 示教测高
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkMeasureHeightInSystem1_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.ChkMeasureHeightInSystem1.Checked && !this.ChkMeasureHeightInSystem2.Checked)
            {
                this.bondPositionConfigure.BondPositionMeasureHeight.State = AssistantStateEnum.ForBidden;
                UcEditProductProgramming.RefreshILstAssistantStepWithBpAction(this.bondPositionConfigure);
            }
            else
            {
                this.bondPositionConfigure.BondPositionMeasureHeight.State = AssistantStateEnum.UnAble;
                UcEditProductProgramming.RefreshILstAssistantStepWithBpAction(this.bondPositionConfigure);
            }

            this.bondPositionConfigure.MeasureHeightInSystem1 = this.ChkMeasureHeightInSystem1.Checked;     
            this.Save();
        }

        /// <summary>
        /// 示教测高
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkMeasureHeightInSystem2_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.ChkMeasureHeightInSystem1.Checked && !this.ChkMeasureHeightInSystem2.Checked)
            {
                this.bondPositionConfigure.BondPositionMeasureHeight.State = AssistantStateEnum.ForBidden;
                UcEditProductProgramming.RefreshILstAssistantStepWithBpAction(this.bondPositionConfigure);
            }
            else
            {
                this.bondPositionConfigure.BondPositionMeasureHeight.State = AssistantStateEnum.Able;
                UcEditProductProgramming.RefreshILstAssistantStepWithBpAction(this.bondPositionConfigure);
            }

            this.bondPositionConfigure.MeasureHeightInSystem2 = this.ChkMeasureHeightInSystem2.Checked;
            this.Save();
        }

        /// <summary>
        /// 勾选框改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void LueAdjustType_EditValueChanged_1(object sender, EventArgs e)
        {
            if (this.isInit)
            {
                this.Save();
                UcEditProductProgramming.RefreshILstAssistantStepWithBpAction(this.bondPositionConfigure);
            }
        }

        /// <summary>
        /// 配置按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtLocateUseInfo_Click(object sender, EventArgs e)
        {
            FrmLocateUseInfo frmLocateUseInfo = new FrmLocateUseInfo(this.bondPositionConfigure.LocateConfig);
            frmLocateUseInfo.ShowDialog();

            frmLocateUseInfo.Dispose();
        }

        /// <summary>
        /// 是否示教角度
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkAssistanceAngle_CheckedChanged(object sender, EventArgs e)
        {
            this.bondPositionConfigure.AssistanceAngle = this.ChkAssistanceAngle.Checked;
        }

        /// <summary>
        /// 修改测高点位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtEditMeasureHeightPos_Click(object sender, EventArgs e)
        {
            FrmMeasureHeightPoints frmMeasureHeightPoints = new FrmMeasureHeightPoints(this.bondPositionConfigure);
            frmMeasureHeightPoints.ShowDialog();
            this.Save();
        }

        /// <summary>
        /// 身份识别
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void LueIdentification_EditValueChanged(object sender, EventArgs e)
        {
            if ((IdentificationEnum)this.LueIdentification.EditValue == IdentificationEnum.Off)
            {
                this.bondPositionConfigure.BondPositionIdentity.State = AssistantStateEnum.ForBidden;
            }
            else
            {
                this.bondPositionConfigure.BondPositionIdentity.State = AssistantStateEnum.UnAble;
            }

            UcEditProductProgramming.RefreshILstAssistantStepWithBpAction(this.bondPositionConfigure);
            this.Save();
        }
    }
}
