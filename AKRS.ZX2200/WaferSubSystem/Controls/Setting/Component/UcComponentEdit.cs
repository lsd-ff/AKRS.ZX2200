using System;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Setting.Component
{
    using System.Collections.Generic;
    using System.Drawing;

    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.Controls.ToolControls.Programming;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Interface;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using DevExpress.DataAccess.Native.DataFederation.QueryBuilder;
    using DevExpress.XtraBars.Ribbon.Drawing;
    using DevExpress.XtraEditors;
    using System.Drawing;

    using AKRS.ZX2200.Infrastructure.Service;

    using UcEditProductProgramming = AKRS.ZX2200.Infrastructure.Controls.Feature.EditProgram.UcEditProductProgram.UcEditProductProgramming;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.EjectionsTeach;
    using AKRS.WM;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using System.Linq;

    using AKRS.ZX2200.TransportUnitSystem.Model;

    using DevExpress.XtraEditors.Controls;

    using SearchMode = AKRS.ZX2200.WaferSubSystem.Models.Enums.SearchMode;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Tool;

    /// <summary>
    /// UcComponentEdit
    /// </summary>
    public partial class UcComponentEdit : DevExpress.XtraEditors.XtraUserControl, IProgrammingControl
    {
        /// <summary>
        /// 提示窗口
        /// </summary>
        private DialogResult res;

        /// <summary>
        /// 是否是初始化
        /// </summary>
        private bool isIni = true;

        /// <summary>
        /// 界面是否是初始化
        /// </summary>
        private bool isControlIni = false;


        /// <summary>
        /// 是否提示信号
        /// </summary>
        private bool isPositionSearchMessage;

        /// <summary>
        /// 上晶圆程式
        /// </summary>
        private WaferSystemDomain WaferSystemDomain => WaferSystemDomain.GetInstance() ?? new WaferSystemDomain();

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// 吸嘴架控制器
        /// </summary>
        private NozzleShelfController nozzleShelfController = new NozzleShelfController();

        /// <summary>
        /// 芯片载具
        /// </summary>
        private BaseCarrierConfig carrierConfig;

        public static Action<double> SetLcThicknessAction;

        private void SetLcThickness(double height)
        {
            this.LcThickness.Text = $"芯片 + 蓝膜总厚度：{height}mm";
            this.SpComponentThickness.Text = this.carrierConfig.ComponentThickness.ToString();
            this.SpCarrierThickness.Text = this.carrierConfig.CarrierThickness.ToString();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="carrierConfig">carrier</param>
        public UcComponentEdit(BaseCarrierConfig carrierConfig)
        {
            this.InitializeComponent();
            this.Disposed += (s, e) =>
                {
                    if (this.timer != null)
                    {
                        this.timer.Tick -= this.Timer1_Tick;
                        this.timer.Dispose();
                    }
                };

            this.carrierConfig = carrierConfig;

            if (this.carrierConfig is CarrierWithWaferConfig wt)
            {
                this.CeIsUseCarrierHeightMeasure.Visible = this.BtnMeasurementComponentAndCarrierThickness.Visible = true;
                this.LcThickness.Text = $"芯片 + 蓝膜总厚度：{wt.ComponentAndCarrierThickness}mm";
            }
            else
            {
                this.CeIsUseCarrierHeightMeasure.Visible = this.BtnMeasurementComponentAndCarrierThickness.Visible = false;
            }

            CeIsUseCarrierHeightMeasure_CheckedChanged(null, null);

            SetLcThicknessAction += this.SetLcThickness;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            // 绑定枚举
            this.EnumBinding();

            // 绑定顶针
            this.CmbEjectionName.Properties.Items.Clear();
            if (WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig != null)
            {
                foreach (var item in WaferSystemProgram.GetInstance().GetDistinctEjectionSettings())
                {
                    this.CmbEjectionName.Properties.Items.Add(item.Name);
                }
            }

            // 绑定吸嘴
            this.CmbNozzleName.Properties.Items.Clear();
            foreach (var item in System2Domain.GetInstance().BondProgram.NozzleShelfProgram.GetNozzleList())
            {
                this.CmbNozzleName.Properties.Items.Add(item.Name);
            }

            // 新增防呆
            if (this.nozzleShelfController.IsConfiguredOnToolBank(this.carrierConfig.NozzleName) == false)
            {
                this.carrierConfig.NozzleName = string.Empty;
            }

            // 获取当前选中载具及其他信息并对界面控件赋值
            this.GetDataAndAssign();

            this.SpUplookVisionDelay.Value = (decimal)this.carrierConfig.AdjustVisionDelay;

            this.SpWeakBlowProportion.Value = (int)this.carrierConfig.WeakBlowProportion;
            this.CeIsComponentDetection.Checked = this.carrierConfig.IsActiveComponentDetection;


            this.CeIsFlip.Enabled = (this.carrierConfig.CarrierType != CarrierTypeEnum.StaticWaffle)
                                    && MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated;

            this.ChkIsActiveSlowDownBeforeDip.Checked = this.carrierConfig.IsActivateSlowTravelBeforeDip;
            this.ChkIsActiveSlowDownAfterDip.Checked = this.carrierConfig.IsActivateSlowTravelAfterDip;
            this.SpSlowTravleDistanceBeforeDip.Value = (decimal)this.carrierConfig.SlowTravelDistanceBeforeDipFlux;
            this.SpSlowTravleDistanceAfterDip.Value = (decimal)this.carrierConfig.SlowTravelDistanceAfterDipFlux;
            this.SpSlowTravleSpeedBeforeDip.Value = (decimal)this.carrierConfig.SlowTravelSpeedBeforeDipFlux;
            this.SpSlowTravleSpeedAfterDip.Value = (decimal)this.carrierConfig.SlowTravelSpeedAfterDipFlux;

            if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured == false)
            {
                this.gCDipMode.Enabled = false;
                this.LueDipMode.EditValue = DipModeEnum.Off;
            }

            this.gCIPTType.Enabled = this.carrierConfig.AdjustCamera == CameraTypeEnum.BondCamera;

            this.SpDipDelay.Value = this.carrierConfig.DipDelay;
            this.SpDipDistance.Value = (decimal)this.carrierConfig.DipDistance;

            this.ChkIsVisionCode.Checked = this.carrierConfig.IsRecognizeQRCode;
            this.ChkIsReferencePoint.Checked = this.carrierConfig.IsConfirmReferencePoint;

            isControlIni = true;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            this.RefreshOptionsItemsCheckState();
        }

        #region 同步块

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpMaxNumberOfComponentsWithoutSearch_EditValueChanged(object sender, EventArgs e)
        {
            this.TbMaxNumberOfComponentsWithoutSearch.EditValue = this.SpMaxNumberOfComponentsWithoutSearch.EditValue;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TbMaxNumberOfComponentsWithoutSearch_EditValueChanged(object sender, EventArgs e)
        {
            this.SpMaxNumberOfComponentsWithoutSearch.EditValue = this.TbMaxNumberOfComponentsWithoutSearch.EditValue;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpIntermediateSteps_EditValueChanged(object sender, EventArgs e)
        {
            this.TbIntermediateSteps.EditValue = this.SpIntermediateSteps.EditValue;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TbIntermediateSteps_EditValueChanged(object sender, EventArgs e)
        {
            this.SpIntermediateSteps.EditValue = this.TbIntermediateSteps.EditValue;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpStepDelay_EditValueChanged(object sender, EventArgs e)
        {
            this.TbStepDelay.EditValue = this.SpStepDelay.EditValue;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TbStepDelay_EditValueChanged(object sender, EventArgs e)
        {
            this.SpStepDelay.EditValue = this.TbStepDelay.EditValue;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpRotaryModeCount_EditValueChanged(object sender, EventArgs e)
        {
            this.TbRotaryModeCount.EditValue = this.SpRotaryModeCount.EditValue;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TbRotaryModeCount_EditValueChanged(object sender, EventArgs e)
        {
            this.SpRotaryModeCount.EditValue = this.TbRotaryModeCount.EditValue;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void LuePositionSearch_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isPositionSearchMessage || this.isIni)
            {
                this.isIni = false;
            }
            else
            {
                this.isPositionSearchMessage = true;
                this.res = AKRSMessageBoxExt.Show("Question 2.3640:\r\n" + "改变位置搜索\r\n" + "确定想要改变芯片的位置搜索吗?\r\n" + "改变之后必须重新示教.", "Prompt", new string[] { "Yes", "No" }, new DialogResult[] { DialogResult.Yes, DialogResult.No });
                switch (this.res)
                {
                    case DialogResult.Yes:
                        break;
                    case DialogResult.No:
                        if ((PositionSearchModeEnum)this.LuePositionSearchMode.EditValue == PositionSearchModeEnum.MultipleSearch)
                        {
                            this.LuePositionSearchMode.EditValue = PositionSearchModeEnum.StandardSearch;
                        }
                        else
                        {
                            this.LuePositionSearchMode.EditValue = PositionSearchModeEnum.MultipleSearch;
                        }

                        break;
                }

                this.isPositionSearchMessage = false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 枚举绑定
        /// </summary>
        private void EnumBinding()
        {
            // AccuracyModeEnum
            this.LueAccuracyMode.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<AccuracyModeEnum>();
            this.LueAccuracyMode.EditValue = AccuracyModeEnum.Off;

            // CarrierShapeEnum
            this.LueCarrierShape.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<CarrierShapeTypeEnum>();
            this.LueCarrierShape.EditValue = CarrierShapeTypeEnum.Circular;

            // CarrierIDReaderModeEnum
            this.LueCarrierIDReaderMode.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<CarrierIDReaderModeEnum>();
            this.LueCarrierIDReaderMode.EditValue = CarrierIDReaderModeEnum.None;

            // PositionSearchModeEnum
            this.LuePositionSearchMode.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<PositionSearchModeEnum>();

            // AccuracySearchModeEnum
            this.LueAccuracySearch.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<PositionSearchModeEnum>();
            this.LueAccuracySearch.EditValue = PositionSearchModeEnum.StandardSearch;

            // CarrierGeoSetModeEnum
            this.LueCarrierGeoSetMode.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<CarrierGeoSetModeEnum>();
            this.LueCarrierGeoSetMode.EditValue = CarrierGeoSetModeEnum.Standard;

            // CarrierAdjustModeEnum
            this.LueCarrierAdjustMode.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<CarrierAdjustModeEnum>();
            this.LueCarrierAdjustMode.EditValue = CarrierAdjustModeEnum.Point1Adjust;

            // PointArrangementModeEnum
            //this.LuePointArrangementMode.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<PointArrangementModeEnum>();
            //this.LuePointArrangementMode.EditValue = PointArrangementModeEnum.Any;

            // 中转台类型
            this.LueIPTType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<IPTTypeEnum>();
            this.LueIPTType.EditValue = IPTTypeEnum.LeftIPT;

            // 纠偏相机
            // 使用 Dictionary 存储枚举值和显示文本
            var dict = new Dictionary<CameraTypeEnum, string>
                           {
                               { CameraTypeEnum.BondCamera,CameraTypeEnum.BondCamera.GetDescription()},
                               { CameraTypeEnum.UpLookCamera, CameraTypeEnum.UpLookCamera.GetDescription()}
                           };
            this.LueAjustCameraType.Properties.DataSource = dict.ToList();
            this.LueAjustCameraType.Properties.DisplayMember = "Value";
            this.LueAjustCameraType.Properties.ValueMember = "Key";

            this.LueAjustCameraType.Properties.Columns.Clear();
            this.LueAjustCameraType.Properties.Columns.Add(new LookUpColumnInfo("Value", ""));
            this.LueAjustCameraType.EditValue = dict[CameraTypeEnum.UpLookCamera];

            // 搜晶相机
            this.LueSearchCamera.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<SearchCameraEnum>();
            this.LueSearchCamera.EditValue = SearchCameraEnum.WaferCamera;

            // CarrierChangeTypeEnum
            this.LueTabletChangeType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<TabletChangeTypeEnum>();
            this.LueTabletChangeType.EditValue = TabletChangeTypeEnum.ManualChange;

            // AutoRunWithDieModeEnum
            this.LueAutoRunWithDieMode.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<AutoRunWithDieModeEnum>();
            this.LueAutoRunWithDieMode.EditValue = AutoRunWithDieModeEnum.Manual;

            // RotationSearchRangeEnum
            this.LueRotationSearchRange.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<RotationSearchRangeEnum>();
            this.LueRotationSearchRange.EditValue = RotationSearchRangeEnum.All;

            // RotaryPositionDeterminationModeEnum
            this.LueRotaryPositionDetermination.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<RotaryPositionDeterminationModeEnum>();
            this.LueRotaryPositionDetermination.EditValue = RotaryPositionDeterminationModeEnum.None;

            // RotaryModeSequenceEnum
            this.LueRotaryModeSequence.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<RotaryModeSequenceEnum>();
            this.LueRotaryModeSequence.EditValue = RotaryModeSequenceEnum.Manual;

            // IsOpenEnum
            this.LueMoveWithWaferTable.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<IsOpenEnum>();
            this.LueMoveWithWaferTable.EditValue = IsOpenEnum.Off;

            // LueDipMode
            this.LueDipMode.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<DipModeEnum>();
            this.LueDipMode.EditValue = DipModeEnum.Off;
        }

        /// <summary>
        /// 刷新操作选项的选中状态
        /// </summary>
        private void RefreshOptionsItemsCheckState()
        {
            if (this.CeIsPositionSearch.CheckState == CheckState.Unchecked)
            {
                this.CeIsInkDotSearch.Enabled = false;
                this.CeIsInkDotSearch.CheckState = CheckState.Unchecked;
                this.CeIsComponentAdjust.Enabled = false;
                this.CeIsComponentAdjust.CheckState = CheckState.Unchecked;
                this.CeIsReferenceDie.Enabled = false;
                this.CeIsReferenceDie.CheckState = CheckState.Unchecked;
                this.CeIsMapping.Enabled = false;
                this.CeIsMapping.CheckState = CheckState.Unchecked;
                this.CeIsGraphicalSyn.Enabled = false;
                this.CeIsGraphicalSyn.CheckState = CheckState.Unchecked;
                this.CeIsBadChipTable.Enabled = false;
                this.CeIsBadChipTable.CheckState = CheckState.Unchecked;
                this.CeIsCarryOutSCT.Enabled = false;
                this.CeIsCarryOutSCT.CheckState = CheckState.Unchecked;
                this.CeIsBondWithProfile.Enabled = false;
                this.CeIsBondWithProfile.CheckState = CheckState.Unchecked;
                this.CeIsRotationSearch.Enabled = false;
                this.CeIsRotationSearch.CheckState = CheckState.Unchecked;
                this.CeIsNineSearch.Enabled = false;
                this.CeIsNineSearch.CheckState = CheckState.Unchecked;
                this.CeIsOneSnap.Enabled = false;
                this.CeIsOneSnap.CheckState = CheckState.Unchecked;
                this.GcPositionSearch.Enabled = false;
                //this.GcAccuracyWithCamera.Enabled = false;
                return;
            }

            // Position search
            if (this.CeIsPositionSearch.CheckState == CheckState.Checked)
            {
                this.CeIsInkDotSearch.Enabled = true;
                this.CeIsMapping.Enabled = true;

                // CarrierType
                if (this.LcCarrierType.Text == CarrierTypeEnum.Wafer.GetDescription())
                {
                    this.CeIsComponentAdjust.Enabled = false;
                    //this.CeIsMapping.Enabled = true;
                    this.CeIsNineSearch.Enabled = true;
                    this.GcEjectionName.Enabled = true;
                    this.GcCarrierShape.Enabled = true;
                    this.GcTestComponents.Enabled = true;
                    this.GcQualityForIntermediateHandling.Enabled = true;
                    this.GcStartDieVerificationMethods.Enabled = true;
                    this.GcNeedleIntermediateSteps.Enabled = true;
                    this.GcNeedleMode.Enabled = true;
                    this.GcRotaryPositionDetermination.Enabled = true;
                }
                else if (this.LcCarrierType.Text == CarrierTypeEnum.Waffle.GetDescription())
                {
                    this.CeIsComponentAdjust.Enabled = false;
                    this.CeIsComponentAdjust.CheckState = CheckState.Unchecked;
                    //this.CeIsMapping.Enabled = false;
                    //this.CeIsMapping.CheckState = CheckState.Unchecked;
                    this.CeIsNineSearch.Enabled = false;
                    this.CeIsNineSearch.CheckState = CheckState.Unchecked;
                    this.GcEjectionName.Enabled = false;
                    this.GcCarrierShape.Enabled = false;
                    this.GcTestComponents.Enabled = false;
                    this.GcQualityForIntermediateHandling.Enabled = false;
                    this.GcStartDieVerificationMethods.Enabled = false;
                    this.GcNeedleIntermediateSteps.Enabled = false;
                    this.GcNeedleMode.Enabled = false;
                    this.GcRotaryPositionDetermination.Enabled = false;
                }

                this.CeIsReferenceDie.Enabled = false;
                this.CeIsGraphicalSyn.Enabled = false;
                this.CeIsBadChipTable.Enabled = false;
                this.CeIsCarryOutSCT.Enabled = false;
                this.CeIsBondWithProfile.Enabled = false;
                this.CeIsRotationSearch.Enabled = false;
                this.CeIsOneSnap.Enabled = true;
                this.GcPositionSearch.Enabled = true;
                //this.GcAccuracyWithCamera.Enabled = false;
            }
            else
            {
                // CarrierType
                if (this.LcCarrierType.Text == CarrierTypeEnum.Wafer.GetDescription())
                {
                    this.CeIsInkDotSearch.Enabled = true;
                    this.CeIsComponentAdjust.Enabled = false;
                    this.GcEjectionName.Enabled = true;
                    this.GcCarrierShape.Enabled = true;
                    this.GcTestComponents.Enabled = true;
                    this.GcQualityForIntermediateHandling.Enabled = true;
                    this.GcStartDieVerificationMethods.Enabled = true;
                    this.GcNeedleIntermediateSteps.Enabled = true;
                    this.GcNeedleMode.Enabled = true;
                    this.GcRotaryPositionDetermination.Enabled = true;
                }
                else if (this.LcCarrierType.Text == CarrierTypeEnum.Waffle.GetDescription())
                {
                    this.CeIsInkDotSearch.Enabled = false;
                    this.CeIsInkDotSearch.CheckState = CheckState.Unchecked;
                    this.CeIsComponentAdjust.Enabled = false;
                    this.CeIsComponentAdjust.CheckState = CheckState.Unchecked;
                    this.GcEjectionName.Enabled = false;
                    this.GcCarrierShape.Enabled = false;
                    this.GcTestComponents.Enabled = false;
                    this.GcQualityForIntermediateHandling.Enabled = false;
                    this.GcStartDieVerificationMethods.Enabled = false;
                    this.GcNeedleIntermediateSteps.Enabled = false;
                    this.GcNeedleMode.Enabled = false;
                    this.GcRotaryPositionDetermination.Enabled = false;
                }

                this.CeIsInkDotSearch.Enabled = false;
                this.CeIsInkDotSearch.CheckState = CheckState.Unchecked;
                this.CeIsComponentAdjust.Enabled = false;
                this.CeIsComponentAdjust.CheckState = CheckState.Unchecked;
                this.CeIsReferenceDie.Enabled = false;
                this.CeIsReferenceDie.CheckState = CheckState.Unchecked;
                this.CeIsMapping.Enabled = false;
                this.CeIsMapping.CheckState = CheckState.Unchecked;
                this.CeIsGraphicalSyn.Enabled = false;
                this.CeIsGraphicalSyn.CheckState = CheckState.Unchecked;
                this.CeIsBadChipTable.Enabled = false;
                this.CeIsBadChipTable.CheckState = CheckState.Unchecked;
                this.CeIsCarryOutSCT.Enabled = false;
                this.CeIsCarryOutSCT.CheckState = CheckState.Unchecked;
                this.CeIsBondWithProfile.Enabled = false;
                this.CeIsBondWithProfile.CheckState = CheckState.Unchecked;
                this.CeIsRotationSearch.Enabled = false;
                this.CeIsRotationSearch.CheckState = CheckState.Unchecked;
                this.CeIsNineSearch.Enabled = false;
                this.CeIsNineSearch.CheckState = CheckState.Unchecked;
                this.CeIsOneSnap.Enabled = false;
                this.CeIsOneSnap.CheckState = CheckState.Unchecked;
                this.GcPositionSearch.Enabled = false;
                //this.GcAccuracyWithCamera.Enabled = false;
            }

            // GcAccuracySearch
            if ((AccuracyModeEnum)this.LueAccuracyMode.EditValue == AccuracyModeEnum.Configurable)
            {
                this.GcAccuracyWithCamera.Enabled = true;
                this.GcAccuracySearch.Enabled = true;
            }
            else
            {
                if ((AccuracyModeEnum)this.LueAccuracyMode.EditValue == AccuracyModeEnum.OutsideEdgeSearch)
                {
                    this.GcAccuracyWithCamera.Enabled = true;
                }
                else
                {
                    this.GcAccuracyWithCamera.Enabled = false;
                }

                this.GcAccuracySearch.Enabled = false;
            }

            // Ink-dot search
            if (this.CeIsInkDotSearch.CheckState == CheckState.Checked)
            {
                this.GcInkDotSearch.Enabled = true;
            }
            else
            {
                this.GcInkDotSearch.Enabled = false;
            }

            // Component-transportUnit adjust
            if (this.CeIsComponentAdjust.CheckState == CheckState.Checked)
            {
                this.GcCarrierAdjust.Enabled = true;
                if (this.LueCarrierAdjustMode.ItemIndex > 0)
                {
                    this.CeIsDifferentSearches.Enabled = true;
                }
                else
                {
                    this.CeIsDifferentSearches.Enabled = false;
                    this.CeIsDifferentSearches.CheckState = CheckState.Unchecked;
                }
            }
            else
            {
                this.LueCarrierAdjustMode.EditValue = CarrierAdjustModeEnum.Point1Adjust;
                this.CeIsDifferentSearches.CheckState = CheckState.Unchecked;
                this.CeIsAfterEachChange.CheckState = CheckState.Unchecked;
                this.GcCarrierAdjust.Enabled = false;
            }

            // Reference die
            if (this.CeIsReferenceDie.CheckState == CheckState.Checked)
            {
                this.CeIsUseReferenceDies.Enabled = true;
                this.CeIsReferenceDieAsAlignmentDie.Enabled = true;
            }
            else
            {
                this.CeIsUseReferenceDies.Enabled = false;
                this.CeIsUseReferenceDies.CheckState = CheckState.Unchecked;
                this.CeIsReferenceDieAsAlignmentDie.Enabled = false;
                this.CeIsReferenceDieAsAlignmentDie.CheckState = CheckState.Unchecked;
            }

            // Wafer mapping
            if (this.CeIsMapping.CheckState == CheckState.Checked)
            {
                //this.CeIsReferenceDie.Enabled = true;
                //this.CeIsBadChipTable.Enabled = false;
                //this.CeIsBadChipTable.CheckState = CheckState.Unchecked;
                this.CeIsNineSearch.Enabled = false;
                this.CeIsNineSearch.CheckState = CheckState.Unchecked;
                this.GcCarrierGeoSetupMethod.Enabled = false;
                this.CeIsInverseWaferMapProcessing.Enabled = true;
            }
            else
            {
                //this.CeIsReferenceDie.Enabled = false;
                //this.CeIsReferenceDie.CheckState = CheckState.Unchecked;
                //this.CeIsBadChipTable.Enabled = true;
                if (this.LcCarrierType.Text == CarrierTypeEnum.Wafer.GetDescription())
                {
                    this.CeIsNineSearch.Enabled = true;
                }
                else
                {
                    this.CeIsNineSearch.Enabled = false;
                }

                this.GcCarrierGeoSetupMethod.Enabled = true;
                this.CeIsInverseWaferMapProcessing.Enabled = false;
                this.CeIsInverseWaferMapProcessing.CheckState = CheckState.Unchecked;
            }

            //// Bad-chip table
            //if (this.CeIsBadChipTable.CheckState == CheckState.Checked)
            //{
            //    if (this.LcCarrierType.Text == CarrierTypeEnum.Wafer.GetDescription())
            //    {
            //        this.CeIsWaferMapping.Enabled = false;
            //        this.CeIsWaferMapping.CheckState = CheckState.Unchecked;
            //    }
            //}
            //else if (this.CeIsPositionSearch.CheckState != CheckState.Unchecked)
            //{
            //    if (this.LcCarrierType.Text == CarrierTypeEnum.Wafer.GetDescription())
            //    {
            //        this.CeIsWaferMapping.Enabled = true;
            //    }
            //}

            // Rotation search
            if (this.CeIsRotationSearch.CheckState == CheckState.Checked)
            {
                this.GcRotationSearch.Enabled = true;
            }
            else
            {
                this.GcRotationSearch.Enabled = false;
            }

            // Nine search
            if (this.CeIsNineSearch.CheckState == CheckState.Checked)
            {
                this.CeIsMapping.Enabled = false;
                this.CeIsInkDotSearch.Enabled = false;
                this.CeIsInkDotSearch.CheckState = CheckState.Unchecked;
                this.GcPositionSearch.Enabled = false;
            }
            else
            {
                if (this.LcCarrierType.Text == CarrierTypeEnum.Wafer.GetDescription())
                {
                    this.CeIsMapping.Enabled = true;
                    this.CeIsInkDotSearch.Enabled = true;
                }
            }

            // GcPositionSearch
            if (this.GcPositionSearch.Enabled == true)
            {
                if ((PositionSearchModeEnum)this.LuePositionSearchMode.EditValue == PositionSearchModeEnum.StandardSearch)
                {
                    this.CePositionSearch2.Enabled = true;
                }
                else
                {
                    this.CePositionSearch2.Enabled = false;
                    this.CePositionSearch2.CheckState = CheckState.Unchecked;
                }

                if ((PositionSearchModeEnum)this.LuePositionSearchMode.EditValue == PositionSearchModeEnum.MultipleSearch)
                {
                    this.BtnPositionSearchConfigure.Enabled = true;
                }
                else
                {
                    this.BtnPositionSearchConfigure.Enabled = false;
                }

                if (this.CePositionSearch2.CheckState == CheckState.Checked)
                {
                    this.LuePositionSearchMode.Enabled = false;
                }
                else
                {
                    this.LuePositionSearchMode.Enabled = true;
                }
            }

            // GcTestComponents
            if (this.GcTestComponents.Enabled == true)
            {
                if (this.CeIsAfterRows.CheckState == CheckState.Checked)
                {
                    this.SpAfterNumberOfRows.Enabled = true;
                }
                else
                {
                    this.SpAfterNumberOfRows.Enabled = false;
                }

                if (this.CeIsAfterColumns.CheckState == CheckState.Checked)
                {
                    this.SpAfterNumberOfColumns.Enabled = true;
                }
                else
                {
                    this.SpAfterNumberOfColumns.Enabled = false;
                }
            }

            // ClbNeedleMode
            if (this.GcNeedleMode.Enabled)
            {
                if (this.CeIsSynchronousEjection.CheckState == CheckState.Checked)
                {
                    this.CeIsMultiPinEjectProcess.Enabled = false;
                }
                else if (this.CeIsMultiPinEjectProcess.CheckState == CheckState.Checked)
                {
                    this.CeIsSynchronousEjection.Enabled = false;
                }
                else
                {
                    this.CeIsSynchronousEjection.Enabled = true;
                    this.CeIsMultiPinEjectProcess.Enabled = true;
                }
            }
            else
            {
                this.CeIsSynchronousEjection.CheckState = CheckState.Unchecked;
                this.CeIsMultiPinEjectProcess.CheckState = CheckState.Unchecked;
            }

            // GcRotaryPositionDetermination
            if (this.GcRotaryPositionDetermination.Enabled == true)
            {
                if (this.LueRotaryPositionDetermination.ItemIndex < 3)
                {
                    this.GcBondingHeadAngleFromPositionSearch.Enabled = true;
                }
                else
                {
                    this.GcBondingHeadAngleFromPositionSearch.Enabled = false;
                }

                if ((RotaryPositionDeterminationModeEnum)this.LueRotaryPositionDetermination.EditValue == RotaryPositionDeterminationModeEnum.RotaryMode)
                {
                    this.GcRotaryModeSequence.Enabled = true;
                    this.GcRotaryModeCounter.Enabled = true;
                }
                else
                {
                    this.GcRotaryModeSequence.Enabled = false;
                    this.GcRotaryModeCounter.Enabled = false;
                }
            }
        }

        /// <summary>
        /// GetSearchMode
        /// </summary>
        /// <returns>result</returns>
        private SearchMode GetSearchMode()
        {
            if (this.carrierConfig is CarrierWithWaferConfig)
            {
                if (this.CeIsMapping.Checked)
                {
                    return SearchMode.WaferMap;
                }
                else if (this.CeIsNineSearch.Checked)
                {
                    return SearchMode.Nine;
                }
                else
                {
                    return SearchMode.Single;
                }
            }
            else if (this.carrierConfig is CarrierWithWaffleConfig)
            {
                return SearchMode.Box;
            }
            else
            {
                throw new Exception("不存在该搜索模式!");
            }
        }

        /// <summary>
        /// 获取当前载具及其余信息并赋值
        /// </summary>
        private void GetDataAndAssign()
        {
            this.GcEjectionName.Enabled = this.carrierConfig is CarrierWithWaferConfig;

            this.LcCarrierType.Text = EnumHelper.GetDescription(this.carrierConfig.CarrierType);
            this.SpComponentThickness.EditValue = this.carrierConfig.ComponentThickness;
            this.SpCarrierThickness.EditValue = this.carrierConfig.CarrierThickness;
            this.SpRotaryPosition.EditValue = this.carrierConfig.RotaryPosition;
            this.CmbNozzleName.EditValue = this.carrierConfig.NozzleName;
            this.LueAccuracyMode.EditValue = this.carrierConfig.AccuracyMode;
            this.CeTwoPointAdjust.Checked = this.carrierConfig.IsTwoPointAdjust;
            this.CePositionSearch2.Checked = this.carrierConfig.IsTwoPointSearch; 
            this.CeIsPositionSearch.CheckState = this.carrierConfig.IsPositionSearch ? CheckState.Checked : CheckState.Unchecked;
            this.CeIsInkDotSearch.CheckState = this.carrierConfig.IsInkDotSearch ? CheckState.Checked : CheckState.Unchecked;
            this.CeIsMapping.CheckState = this.carrierConfig.IsMapping ? CheckState.Checked : CheckState.Unchecked;
            this.CeIsNineSearch.CheckState = this.carrierConfig.SearchMode == SearchMode.Nine ? CheckState.Checked : CheckState.Unchecked;
            this.CeIsComponentDetection.CheckState =
                this.carrierConfig.IsActiveComponentDetection ? CheckState.Checked : CheckState.Unchecked;

            this.LuePositionSearchMode.EditValue = this.carrierConfig.PositionSearchMode;
            this.LueTabletChangeType.EditValue = this.carrierConfig.TabletChangeType;
            this.LueAutoRunWithDieMode.EditValue = this.carrierConfig.AutoRunWithDieMode;
            this.LueAjustCameraType.EditValue = this.carrierConfig.AdjustCamera;
            this.LueSearchCamera.EditValue = this.carrierConfig.SearchCamera;
            this.LueIPTType.EditValue = this.carrierConfig.IPTType;

            this.SpNumberOfInkDot.EditValue = this.carrierConfig.NumberOfInkDot;
            this.SpMaxNumberOfComponentsWithoutSearch.EditValue = this.carrierConfig.BlankDieAutoSkipMaxTimes;
            this.CeIsFlip.Checked = this.carrierConfig.IsUseFlipTable;

            if (this.carrierConfig is CarrierWithWaferConfig waferCarrier)
            {
                this.CmbEjectionName.SelectedItem = waferCarrier.EjectionName;
                this.LueCarrierShape.EditValue = waferCarrier.CarrierShapeType;
                this.SpAfterNumberOfRows.EditValue = waferCarrier.AfterNumberOfRows;
                this.SpAfterNumberOfColumns.EditValue = waferCarrier.AfterNumberOfColumns;
            }
            else if (this.carrierConfig is CarrierWithWaffleConfig waffleCarrier)
            {
            }


            this.LueDipMode.EditValue = this.carrierConfig.DipMode;
        }

        /// <summary>
        /// 保存当前载具及其余信息
        /// </summary>
        private void SaveAllData()
        {
            this.carrierConfig.ComponentThickness = (double)this.SpComponentThickness.Value;
            this.carrierConfig.CarrierThickness = (double)this.SpCarrierThickness.Value;
            this.carrierConfig.RotaryPosition = (double)this.SpRotaryPosition.Value;
            this.carrierConfig.IsMapping = this.CeIsMapping.Checked;
            this.carrierConfig.SearchMode = this.GetSearchMode();

            this.carrierConfig.NozzleName = this.CmbNozzleName.Text;
            this.carrierConfig.AccuracyMode = (AccuracyModeEnum)this.LueAccuracyMode.EditValue;
            if (this.carrierConfig.AccuracyMode == AccuracyModeEnum.Off)
            {
                this.carrierConfig.ComponentAccuracyMode.State = AssistantStateEnum.ForBidden;
            }
            else
            {
                if (this.carrierConfig.ComponentAccuracyMode.State != AssistantStateEnum.Able)
                {
                    this.carrierConfig.ComponentAccuracyMode.State = AssistantStateEnum.UnAble;
                }
            }

            this.carrierConfig.AdjustCamera = (CameraTypeEnum)this.LueAjustCameraType.EditValue;
            this.carrierConfig.SearchCamera = (SearchCameraEnum)this.LueSearchCamera.EditValue;
            this.carrierConfig.IsTwoPointSearch = this.CePositionSearch2.Checked; 
            this.carrierConfig.IsTwoPointAdjust = this.CeTwoPointAdjust.Checked;
            this.carrierConfig.IsPositionSearch = this.CeIsPositionSearch.Checked;
            this.carrierConfig.IsInkDotSearch = this.CeIsInkDotSearch.Checked;
            if (!this.carrierConfig.IsInkDotSearch)
            {
                this.carrierConfig.InkDot.State = AssistantStateEnum.ForBidden;
            }
            else
            {
                if (this.carrierConfig.InkDot.State != AssistantStateEnum.Able)
                {
                    this.carrierConfig.InkDot.State = AssistantStateEnum.UnAble;
                }
            }

            this.carrierConfig.IsActiveComponentDetection = this.CeIsComponentDetection.Checked;
            this.carrierConfig.NumberOfInkDot = (int)this.SpNumberOfInkDot.Value;
            this.carrierConfig.BlankDieAutoSkipMaxTimes = (int)this.SpMaxNumberOfComponentsWithoutSearch.Value;

            this.carrierConfig.PositionSearchMode = (PositionSearchModeEnum)this.LuePositionSearchMode.EditValue;
            this.carrierConfig.TabletChangeType = (TabletChangeTypeEnum)this.LueTabletChangeType.EditValue;
            this.carrierConfig.AutoRunWithDieMode = (AutoRunWithDieModeEnum)this.LueAutoRunWithDieMode.EditValue;

            this.carrierConfig.AdjustVisionDelay = (int)this.SpUplookVisionDelay.Value;

            this.carrierConfig.WeakBlowProportion = (int)this.SpWeakBlowProportion.Value;
            this.carrierConfig.IsActiveComponentDetection = this.CeIsComponentDetection.Checked;

            this.carrierConfig.IsUseFlipTable = this.CeIsFlip.Checked;

            if (this.carrierConfig is CarrierWithWaferConfig waferCarrier)
            {
                waferCarrier.EjectionName = this.CmbEjectionName.Text;
                waferCarrier.CarrierShapeType = (CarrierShapeTypeEnum)this.LueCarrierShape.EditValue;
                waferCarrier.AfterNumberOfRows = (double)this.SpAfterNumberOfRows.Value;
                waferCarrier.AfterNumberOfColumns = (double)this.SpAfterNumberOfColumns.Value;
            }
            else if (this.carrierConfig is CarrierWithWaffleConfig waffleCarrier)
            {
            }

            this.carrierConfig.IsActivateSlowTravelBeforeDip = this.ChkIsActiveSlowDownBeforeDip.Checked;
            this.carrierConfig.IsActivateSlowTravelAfterDip = this.ChkIsActiveSlowDownAfterDip.Checked;
            this.carrierConfig.SlowTravelDistanceBeforeDipFlux = (double)this.SpSlowTravleDistanceBeforeDip.Value;
            this.carrierConfig.SlowTravelDistanceAfterDipFlux = (double)this.SpSlowTravleDistanceAfterDip.Value;
            this.carrierConfig.SlowTravelSpeedBeforeDipFlux = (double)this.SpSlowTravleSpeedBeforeDip.Value;
            this.carrierConfig.SlowTravelSpeedAfterDipFlux = (double)this.SpSlowTravleSpeedAfterDip.Value;

            this.carrierConfig.DipMode = (DipModeEnum)this.LueDipMode.EditValue;

            this.carrierConfig.DipDelay = (int)this.SpDipDelay.Value;

            this.carrierConfig.DipDistance = (double)this.SpDipDistance.Value;

            if (!this.carrierConfig.IsUseFlipTable)
            {
                this.carrierConfig.FlipToWafer.State = AssistantStateEnum.ForBidden;
            }
            else
            {
                if (this.carrierConfig.FlipToWafer.State != AssistantStateEnum.Able)
                {
                    this.carrierConfig.FlipToWafer.State = AssistantStateEnum.UnAble;
                }
            }

            this.carrierConfig.IPTType = (IPTTypeEnum)this.LueIPTType.EditValue;

            this.carrierConfig.IsConfirmReferencePoint = this.ChkIsReferencePoint.Checked;
            this.carrierConfig.IsRecognizeQRCode = this.ChkIsVisionCode.Checked;

            // 芯片检查
            this.ComponentCheck();

            CarrierConfigRepository.GetInstance().Save();
        }

        #endregion

        /// <summary>
        /// 芯片检查
        /// </summary>
        private void ComponentCheck()
        {
            if (string.IsNullOrEmpty(this.carrierConfig.NozzleName))
            {
                AKRSXtraMessageBox.Show($"芯片{this.carrierConfig.Name}未选择吸嘴，请选择吸嘴！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            if (this.nozzleShelfController.IsConfiguredOnToolBank(this.carrierConfig.NozzleName) == false)
            {
                this.carrierConfig.NozzleName = string.Empty;

                AKRSXtraMessageBox.Show($"芯片{this.carrierConfig.Name}对应的吸嘴:{this.carrierConfig.NozzleName}未配置到吸嘴架，请重新选择吸嘴！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            if (this.carrierConfig.AdjustCamera == CameraTypeEnum.BondCamera)
            {
                if (this.carrierConfig.IPTType == IPTTypeEnum.LeftIPT
                  && !MachineHardwareConfiguration.GetInstance().IsIPTConfigrated)
                {
                    AKRSXtraMessageBox.Show($"左中转台未配置，请重新选择中转台类型！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                if (this.carrierConfig.IPTType == IPTTypeEnum.RightIPT
                    && !MachineHardwareConfiguration.GetInstance().IsRightIPTConfigrated)
                {
                    AKRSXtraMessageBox.Show($"右中转台未配置，请重新选择中转台！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }
          
        }

        /// <summary>
        /// 确定
        /// </summary>
        public void Confirm()
        {
            this.SaveAllData();
        }

        /// <summary>
        /// LueAccuracyMode_EditValueChanged
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void LueAccuracyMode_EditValueChanged(object sender, EventArgs e)
        {
            if (!this.isIni)
            {
                this.SaveAllData();
                UcEditProductProgramming.RefreshILstAssistantStepWithComponentsAction(this.carrierConfig);
            }
        }

        /// <summary>
        /// UcComponentEdit_Load
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void UcComponentEdit_Load(object sender, EventArgs e)
        {
            this.InitControl();
            this.CeIsUseCarrierHeightMeasure.Checked = WaferSubDevicePara.GetInstance().WaferTableDevicePara.IsUseCarrierHeightMeasure;
            this.timer.Enabled = true;
        }

        /// <summary>
        /// CeIsInkDotSearch_CheckedChanged
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void CeIsInkDotSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.isIni)
            {
                this.SaveAllData();
                UcEditProductProgramming.RefreshILstAssistantStepWithComponentsAction(this.carrierConfig);
            }
        }

        /// <summary>
        /// 弱吹
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnBlow_Click(object sender, EventArgs e)
        {
            if (!this.bondHeadController.GetNozzleBlowState())
            {
                // 先关真空电磁阀
                this.bondHeadController.CloseToolVaccum();

                // 开吹气
                this.bondHeadController.OpenToolBlowEle((int)this.SpWeakBlowProportion.Value);
                this.BtnBlow.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                this.bondHeadController.CloseToolBlowEle();
                this.BtnBlow.Appearance.BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// ESUpOrDown按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnESUpOrDown_Click(object sender, EventArgs e)
        {
            if (this.carrierConfig is CarrierWithWaferConfig wt)
            {
                SimpleButton btn = sender as SimpleButton;
                try
                {
                    btn.Enabled = false;
                    if (btn.Appearance.BackColor == Color.Yellow)
                    {
                        WaferSubController.GetInstance().EjectController.ReturnEjection();
                        btn.Appearance.BackColor = default;
                    }
                    else
                    {
                        if (WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig.EjectionBankSlots.Exists(item => item.Name == wt.EjectionName) && wt.EjectionName != string.Empty)
                        {
                            EjectionBankSlotConfig tarBankSlotConfig = (EjectionBankSlotConfig)WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig.EjectionBankSlots.Find(item => item.Name == wt.EjectionName);
                            Block.GetInstance().SetCurrentCarrier(wt);
                            WaferSubController.GetInstance().EjectController.ChangeEjection(tarBankSlotConfig.Index);
                            WaferSubController.GetInstance().EjectController.MoveEjectToLiftPosition();
                            btn.Appearance.BackColor = Color.Yellow;
                        }
                        else
                        {
                            //throw new Exception($"This Ejection does not exist in the current EjectionBank!");
                            throw new Exception($"该芯片使用的顶针在当前顶针架中不存在!");
                        }
                    }
                }
                catch (Exception exception)
                {
                    AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    btn.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 勾选框改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ChkIsActiveSlowDownBeforeDip_CheckedChanged(object sender, EventArgs e)
        {
            this.SpSlowTravleDistanceBeforeDip.Enabled = this.ChkIsActiveSlowDownBeforeDip.Checked;
            this.SpSlowTravleSpeedBeforeDip.Enabled = this.ChkIsActiveSlowDownBeforeDip.Checked;
        }

        /// <summary>
        /// 勾选框改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ChkIsActiveSlowDownAfterDip_CheckedChanged(object sender, EventArgs e)
        {
            this.SpSlowTravleDistanceAfterDip.Enabled = this.ChkIsActiveSlowDownAfterDip.Checked;
            this.SpSlowTravleSpeedAfterDip.Enabled = this.ChkIsActiveSlowDownAfterDip.Checked;
        }

        /// <summary>
        /// 下拉框改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void LueDipMode_EditValueChanged(object sender, EventArgs e)
        {
            this.gCDip.Enabled = this.LueDipMode.Text != DipModeEnum.Off.GetDescription();
        }

        /// <summary>
        /// system2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 获取芯片所用的顶针的配置
        /// </summary>
        /// <returns>return</returns>
        private EjectionConfig GetCurrentComponentEjectionConfig()
        {
            return (EjectionConfig)EjectionConfigRepository.GetInstance().Find(this.CmbEjectionName.Text);
        }

        /// <summary>
        /// 测厚
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMeasurementComponentAndCarrierThickness_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.CmbEjectionName.Text))
            {
                AKRSXtraMessageBox.Show($"芯片所使用的顶针为空，请选择顶针.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                if (this.system2Controller.ChangeTouchDownAssistance()/*this.system2Controller.ChangeNozzleAssistance(((CarrierWithWaferConfig)this.carrierConfig).NozzleName)*/)
                {
                    ControlService.ShowDialogForm<FrmCarrierHeightMeasureTeach>((CarrierWithWaferConfig)this.carrierConfig);
                    System2Domain.GetInstance().BondModuleController.MoveToSafePos();

                    //System2Domain.GetInstance().BondModuleController.MoveToPickupPos();

                    //XtraMessageBox.Show("即将进行芯片与蓝膜总厚度测高,请将晶圆台移动到芯片处！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //ControlService.ShowDialogForm<FrmHeightMeasurementTeach>((CarrierWithWaferConfig)this.carrierConfig, false);

                    //XtraMessageBox.Show("即将进行蓝膜厚度测高,请将晶圆台移动到蓝膜处！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //ControlService.ShowDialogForm<FrmHeightMeasurementTeach>((CarrierWithWaferConfig)this.carrierConfig, true);

                    //System2Domain.GetInstance().BondModuleController.MoveToSafePos();
                }
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// 外框pr编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            WaferSubController.GetInstance().WaferTableController.EditPr(carrierConfig.DieFrameName);
        }

        /// <summary>
        /// 翻转台勾选框改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CeIsFlip_CheckedChanged(object sender, EventArgs e)
        {
            if (this.isControlIni)
            {
                this.SaveAllData();
                UcEditProductProgramming.RefreshILstAssistantStepWithComponentsAction(this.carrierConfig);
            }
        }

        private void CeIsUseCarrierHeightMeasure_CheckedChanged(object sender, EventArgs e)
        {
            WaferSubDevicePara.GetInstance().WaferTableDevicePara.IsUseCarrierHeightMeasure = this.BtnMeasurementComponentAndCarrierThickness.Enabled = this.CeIsUseCarrierHeightMeasure.Checked;
            WaferSubDevicePara.GetInstance().Save();
        }

        /// <summary>
        /// 搜晶相机改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void LueSearchCamera_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isControlIni)
            {
                this.res = AKRSMessageBoxExt.Show("Question 2.3640:\r\n" + "改变搜晶相机\r\n" + "确定想要改变搜晶相机吗?\r\n" + "改变之后必须重新示教.", "Prompt", new string[] { "Yes", "No" }, new DialogResult[] { DialogResult.Yes, DialogResult.No });

                switch (this.res)
                {
                    case DialogResult.Yes:
                        this.carrierConfig.ComponentGeometry.State = AssistantStateEnum.UnAble;
                        this.SaveAllData();
                        UcEditProductProgramming.RefreshILstAssistantStepWithComponentsAction(this.carrierConfig);
                        break;
                    case DialogResult.No:
                        break;
                }
            }
        }

        /// <summary>
        ///  下拉框改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void LueAjustCameraType_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isControlIni)
            {
                this.SaveAllData();
                this.gCIPTType.Enabled = this.carrierConfig.AdjustCamera == CameraTypeEnum.BondCamera;
            }
        }

        /// <summary>
        /// 编辑搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtEditSearch_Click(object sender, EventArgs e)
        {
            // 保存数据
            this.SaveAllData();

            if (this.carrierConfig.AccuracyMode == AccuracyModeEnum.Off)
            {
                AKRSXtraMessageBox.Show("请先选择矫正模式");
                return;
            }

            // 定位配置
            AdjustConfig adjustConfig = this.carrierConfig.AdjustCamera == CameraTypeEnum.UpLookCamera ?
                                            this.carrierConfig.UpLookAdjustConfig : this.carrierConfig.DownLookAdjustConfig;

            // 定位配置
            AdjustTypeEnum adjustTypeEnum = this.carrierConfig.IsTwoPointAdjust ? AdjustTypeEnum.TwoPoints : AdjustTypeEnum.OnePoint;
            FrmLocateUseInfo frmLocateUseInfo = new FrmLocateUseInfo(adjustConfig, adjustTypeEnum);
            frmLocateUseInfo.ShowDialog();

            // 保存数据
            this.SaveAllData();
        }

        /// <summary>
        /// 两点定位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CeTwoPointAdjust_CheckedChanged(object sender, EventArgs e)
        {
            // 保存数据
            this.SaveAllData();
        }

        // 确保在控件从父窗体移除时停止Timer
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (this.Parent == null)
            {
                StopTimer();
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            StopTimer();
            base.OnHandleDestroyed(e);
        }

        public void StopTimer()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
                timer = null;
            }
        }

        /// <summary>
        /// 配置搜索界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            AdjustTypeEnum adjustTypeEnum = !this.carrierConfig.IsTwoPointAdjust ? AdjustTypeEnum.OnePoint : AdjustTypeEnum.TwoPoints;

            if (this.carrierConfig.AdjustCamera == CameraTypeEnum.BondCamera)
            {
                FrmLocateUseInfo frmLocate = new FrmLocateUseInfo(this.carrierConfig.DownLookAdjustConfig, adjustTypeEnum);
                frmLocate.TopMost = true;
                System.Windows.Forms.DialogResult dialogResult = frmLocate.ShowDialog();
            }
            else
            {
                FrmLocateUseInfo frmLocate = new FrmLocateUseInfo(this.carrierConfig.UpLookAdjustConfig, adjustTypeEnum);
                frmLocate.TopMost = true;
                System.Windows.Forms.DialogResult dialogResult = frmLocate.ShowDialog();
            }

            this.SaveAllData();
        }
    }
}
