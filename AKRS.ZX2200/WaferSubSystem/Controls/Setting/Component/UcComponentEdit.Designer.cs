namespace AKRS.ZX2200.WaferSubSystem.Controls.Setting.Component
{
    partial class UcComponentEdit
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (disposing)
            {
                StopTimer();
            }

            base.Dispose(disposing);
        }

        #region Wafer Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.TpProgramming = new DevExpress.XtraTab.XtraTabPage();
            this.gCDipMode = new DevExpress.XtraEditors.GroupControl();
            this.LueDipMode = new DevExpress.XtraEditors.LookUpEdit();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.BtnBlow = new DevExpress.XtraEditors.SimpleButton();
            this.SpWeakBlowProportion = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.SpRotaryPosition = new DevExpress.XtraEditors.SpinEdit();
            this.SpCarrierThickness = new DevExpress.XtraEditors.SpinEdit();
            this.SpComponentThickness = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
            this.CeIsFlip = new DevExpress.XtraEditors.CheckEdit();
            this.CeIsComponentDetection = new DevExpress.XtraEditors.CheckEdit();
            this.CeIsNineSearch = new DevExpress.XtraEditors.CheckEdit();
            this.CeIsRotationSearch = new DevExpress.XtraEditors.CheckEdit();
            this.CeIsPositionSearch = new DevExpress.XtraEditors.CheckEdit();
            this.CeIsBondWithProfile = new DevExpress.XtraEditors.CheckEdit();
            this.CeIsInkDotSearch = new DevExpress.XtraEditors.CheckEdit();
            this.CeIsMapping = new DevExpress.XtraEditors.CheckEdit();
            this.CeIsGraphicalSyn = new DevExpress.XtraEditors.CheckEdit();
            this.CeIsCarryOutSCT = new DevExpress.XtraEditors.CheckEdit();
            this.CeIsComponentAdjust = new DevExpress.XtraEditors.CheckEdit();
            this.CeIsReferenceDie = new DevExpress.XtraEditors.CheckEdit();
            this.CeIsBadChipTable = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl12 = new DevExpress.XtraEditors.GroupControl();
            this.LueCarrierIDReaderMode = new DevExpress.XtraEditors.LookUpEdit();
            this.groupControl11 = new DevExpress.XtraEditors.GroupControl();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.GcCarrierShape = new DevExpress.XtraEditors.GroupControl();
            this.LueCarrierShape = new DevExpress.XtraEditors.LookUpEdit();
            this.GcCarrierGeoSetupMethod = new DevExpress.XtraEditors.GroupControl();
            this.LueCarrierGeoSetMode = new DevExpress.XtraEditors.LookUpEdit();
            this.groupControl6 = new DevExpress.XtraEditors.GroupControl();
            this.CeIsOneSnap = new DevExpress.XtraEditors.CheckEdit();
            this.CeIsWithOutWaferTableMovement = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.CmbNozzleName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.GcEjectionName = new DevExpress.XtraEditors.GroupControl();
            this.BtnESUpOrDown = new DevExpress.XtraEditors.SimpleButton();
            this.CmbEjectionName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.groupControl5 = new DevExpress.XtraEditors.GroupControl();
            this.LueAccuracyMode = new DevExpress.XtraEditors.LookUpEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.CeIsUseCarrierHeightMeasure = new DevExpress.XtraEditors.CheckEdit();
            this.LcThickness = new DevExpress.XtraEditors.LabelControl();
            this.BtnMeasurementComponentAndCarrierThickness = new DevExpress.XtraEditors.SimpleButton();
            this.LcCarrierType = new DevExpress.XtraEditors.LabelControl();
            this.TpDetails = new DevExpress.XtraTab.XtraTabPage();
            this.gCIPTType = new DevExpress.XtraEditors.GroupControl();
            this.LueIPTType = new DevExpress.XtraEditors.LookUpEdit();
            this.CeIsInverseWaferMapProcessing = new DevExpress.XtraEditors.CheckEdit();
            this.CeIsReferenceDieAsAlignmentDie = new DevExpress.XtraEditors.CheckEdit();
            this.GcStartDieVerificationMethods = new DevExpress.XtraEditors.GroupControl();
            this.CeIsFirstDieIntegrityCheck = new DevExpress.XtraEditors.CheckEdit();
            this.CeIs8DieCheck = new DevExpress.XtraEditors.CheckEdit();
            this.CeIsEdgeDieCheck = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit3 = new DevExpress.XtraEditors.CheckEdit();
            this.GcRotationSearch = new DevExpress.XtraEditors.GroupControl();
            this.groupControl17 = new DevExpress.XtraEditors.GroupControl();
            this.LueRotationSearchRange = new DevExpress.XtraEditors.LookUpEdit();
            this.GcQualityForIntermediateHandling = new DevExpress.XtraEditors.GroupControl();
            this.CeIsErrorMessageOnBadSearch = new DevExpress.XtraEditors.CheckEdit();
            this.CeIsBad = new DevExpress.XtraEditors.CheckEdit();
            this.CeIsUseReferenceDies = new DevExpress.XtraEditors.CheckEdit();
            this.CeIsDoNotPick = new DevExpress.XtraEditors.CheckEdit();
            this.GcAccuracyWithCamera = new DevExpress.XtraEditors.GroupControl();
            this.labelControl21 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl20 = new DevExpress.XtraEditors.LabelControl();
            this.SpUplookVisionDelay = new DevExpress.XtraEditors.SpinEdit();
            this.LueAjustCameraType = new DevExpress.XtraEditors.LookUpEdit();
            this.groupControl19 = new DevExpress.XtraEditors.GroupControl();
            this.LueTabletChangeType = new DevExpress.XtraEditors.LookUpEdit();
            this.GcCarrierAdjust = new DevExpress.XtraEditors.GroupControl();
            this.CeIsAfterEachChange = new DevExpress.XtraEditors.CheckEdit();
            this.LueCarrierAdjustMode = new DevExpress.XtraEditors.LookUpEdit();
            this.CeIsDifferentSearches = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl24 = new DevExpress.XtraEditors.GroupControl();
            this.LueSearchCamera = new DevExpress.XtraEditors.LookUpEdit();
            this.groupControl25 = new DevExpress.XtraEditors.GroupControl();
            this.LueAutoRunWithDieMode = new DevExpress.XtraEditors.LookUpEdit();
            this.GcInkDotSearch = new DevExpress.XtraEditors.GroupControl();
            this.groupControl9 = new DevExpress.XtraEditors.GroupControl();
            this.SpNumberOfInkDot = new DevExpress.XtraEditors.SpinEdit();
            this.groupControl29 = new DevExpress.XtraEditors.GroupControl();
            this.TbMaxNumberOfComponentsWithoutSearch = new DevExpress.XtraEditors.ZoomTrackBarControl();
            this.SpMaxNumberOfComponentsWithoutSearch = new DevExpress.XtraEditors.SpinEdit();
            this.GcTestComponents = new DevExpress.XtraEditors.GroupControl();
            this.CeIsAfterColumns = new DevExpress.XtraEditors.CheckEdit();
            this.SpAfterNumberOfColumns = new DevExpress.XtraEditors.SpinEdit();
            this.CeIsAfterRows = new DevExpress.XtraEditors.CheckEdit();
            this.SpAfterNumberOfRows = new DevExpress.XtraEditors.SpinEdit();
            this.GcAccuracySearch = new DevExpress.XtraEditors.GroupControl();
            this.ChkIsVisionCode = new DevExpress.XtraEditors.CheckEdit();
            this.ChkIsReferencePoint = new DevExpress.XtraEditors.CheckEdit();
            this.LueAccuracySearch = new DevExpress.XtraEditors.LookUpEdit();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.CeTwoPointAdjust = new DevExpress.XtraEditors.CheckEdit();
            this.GcPositionSearch = new DevExpress.XtraEditors.GroupControl();
            this.LuePositionSearchMode = new DevExpress.XtraEditors.LookUpEdit();
            this.BtnPositionSearchConfigure = new DevExpress.XtraEditors.SimpleButton();
            this.CePositionSearch2 = new DevExpress.XtraEditors.CheckEdit();
            this.TpOptimization = new DevExpress.XtraTab.XtraTabPage();
            this.gCDip = new DevExpress.XtraEditors.GroupControl();
            this.labelControl32 = new DevExpress.XtraEditors.LabelControl();
            this.SpDipDistance = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl33 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl30 = new DevExpress.XtraEditors.LabelControl();
            this.SpDipDelay = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl31 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl13 = new DevExpress.XtraEditors.GroupControl();
            this.SpSlowTravleSpeedBeforeDip = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl25 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl22 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl23 = new DevExpress.XtraEditors.LabelControl();
            this.SpSlowTravleDistanceBeforeDip = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl24 = new DevExpress.XtraEditors.LabelControl();
            this.ChkIsActiveSlowDownBeforeDip = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl14 = new DevExpress.XtraEditors.GroupControl();
            this.SpSlowTravleSpeedAfterDip = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl26 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl27 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl28 = new DevExpress.XtraEditors.LabelControl();
            this.SpSlowTravleDistanceAfterDip = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl29 = new DevExpress.XtraEditors.LabelControl();
            this.ChkIsActiveSlowDownAfterDip = new DevExpress.XtraEditors.CheckEdit();
            this.GcNeedleMode = new DevExpress.XtraEditors.GroupControl();
            this.CeIsMultiPinEjectProcess = new DevExpress.XtraEditors.CheckEdit();
            this.CeIsSynchronousEjection = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl27 = new DevExpress.XtraEditors.GroupControl();
            this.LueMoveWithWaferTable = new DevExpress.XtraEditors.LookUpEdit();
            this.GcNeedleIntermediateSteps = new DevExpress.XtraEditors.GroupControl();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.TbStepDelay = new DevExpress.XtraEditors.ZoomTrackBarControl();
            this.spinEdit8 = new DevExpress.XtraEditors.SpinEdit();
            this.SpStepDelay = new DevExpress.XtraEditors.SpinEdit();
            this.TbIntermediateSteps = new DevExpress.XtraEditors.ZoomTrackBarControl();
            this.SpIntermediateSteps = new DevExpress.XtraEditors.SpinEdit();
            this.groupControl39 = new DevExpress.XtraEditors.GroupControl();
            this.spinEdit13 = new DevExpress.XtraEditors.SpinEdit();
            this.spinEdit12 = new DevExpress.XtraEditors.SpinEdit();
            this.spinEdit11 = new DevExpress.XtraEditors.SpinEdit();
            this.spinEdit10 = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl19 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl18 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl16 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.GcRotaryPositionDetermination = new DevExpress.XtraEditors.GroupControl();
            this.GcRotaryModeCounter = new DevExpress.XtraEditors.GroupControl();
            this.TbRotaryModeCount = new DevExpress.XtraEditors.ZoomTrackBarControl();
            this.SpRotaryModeCount = new DevExpress.XtraEditors.SpinEdit();
            this.GcRotaryModeSequence = new DevExpress.XtraEditors.GroupControl();
            this.LueRotaryModeSequence = new DevExpress.XtraEditors.LookUpEdit();
            this.groupControl21 = new DevExpress.XtraEditors.GroupControl();
            this.LueRotaryPositionDetermination = new DevExpress.XtraEditors.LookUpEdit();
            this.GcBondingHeadAngleFromPositionSearch = new DevExpress.XtraEditors.GroupControl();
            this.CeBondingHeadAngleFromPositionSearch = new DevExpress.XtraEditors.CheckEdit();
            this.timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.TpProgramming.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gCDipMode)).BeginInit();
            this.gCDipMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueDipMode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpWeakBlowProportion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpRotaryPosition.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpCarrierThickness.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpComponentThickness.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
            this.groupControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsFlip.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsComponentDetection.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsNineSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsRotationSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsPositionSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsBondWithProfile.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsInkDotSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsMapping.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsGraphicalSyn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsCarryOutSCT.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsComponentAdjust.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsReferenceDie.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsBadChipTable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl12)).BeginInit();
            this.groupControl12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueCarrierIDReaderMode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl11)).BeginInit();
            this.groupControl11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcCarrierShape)).BeginInit();
            this.GcCarrierShape.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueCarrierShape.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcCarrierGeoSetupMethod)).BeginInit();
            this.GcCarrierGeoSetupMethod.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueCarrierGeoSetMode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).BeginInit();
            this.groupControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsOneSnap.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsWithOutWaferTableMovement.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbNozzleName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcEjectionName)).BeginInit();
            this.GcEjectionName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbEjectionName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).BeginInit();
            this.groupControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueAccuracyMode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsUseCarrierHeightMeasure.Properties)).BeginInit();
            this.TpDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gCIPTType)).BeginInit();
            this.gCIPTType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueIPTType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsInverseWaferMapProcessing.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsReferenceDieAsAlignmentDie.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcStartDieVerificationMethods)).BeginInit();
            this.GcStartDieVerificationMethods.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsFirstDieIntegrityCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIs8DieCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsEdgeDieCheck.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcRotationSearch)).BeginInit();
            this.GcRotationSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl17)).BeginInit();
            this.groupControl17.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueRotationSearchRange.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcQualityForIntermediateHandling)).BeginInit();
            this.GcQualityForIntermediateHandling.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsErrorMessageOnBadSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsBad.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsUseReferenceDies.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsDoNotPick.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcAccuracyWithCamera)).BeginInit();
            this.GcAccuracyWithCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpUplookVisionDelay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueAjustCameraType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl19)).BeginInit();
            this.groupControl19.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueTabletChangeType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcCarrierAdjust)).BeginInit();
            this.GcCarrierAdjust.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsAfterEachChange.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueCarrierAdjustMode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsDifferentSearches.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl24)).BeginInit();
            this.groupControl24.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueSearchCamera.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl25)).BeginInit();
            this.groupControl25.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueAutoRunWithDieMode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcInkDotSearch)).BeginInit();
            this.GcInkDotSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl9)).BeginInit();
            this.groupControl9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpNumberOfInkDot.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl29)).BeginInit();
            this.groupControl29.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbMaxNumberOfComponentsWithoutSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbMaxNumberOfComponentsWithoutSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpMaxNumberOfComponentsWithoutSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcTestComponents)).BeginInit();
            this.GcTestComponents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsAfterColumns.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpAfterNumberOfColumns.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsAfterRows.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpAfterNumberOfRows.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcAccuracySearch)).BeginInit();
            this.GcAccuracySearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChkIsVisionCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkIsReferencePoint.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueAccuracySearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeTwoPointAdjust.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcPositionSearch)).BeginInit();
            this.GcPositionSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LuePositionSearchMode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CePositionSearch2.Properties)).BeginInit();
            this.TpOptimization.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gCDip)).BeginInit();
            this.gCDip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpDipDistance.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpDipDelay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl13)).BeginInit();
            this.groupControl13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlowTravleSpeedBeforeDip.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlowTravleDistanceBeforeDip.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkIsActiveSlowDownBeforeDip.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl14)).BeginInit();
            this.groupControl14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlowTravleSpeedAfterDip.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlowTravleDistanceAfterDip.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkIsActiveSlowDownAfterDip.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcNeedleMode)).BeginInit();
            this.GcNeedleMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsMultiPinEjectProcess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsSynchronousEjection.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl27)).BeginInit();
            this.groupControl27.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueMoveWithWaferTable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcNeedleIntermediateSteps)).BeginInit();
            this.GcNeedleIntermediateSteps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbStepDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbStepDelay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit8.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpStepDelay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbIntermediateSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbIntermediateSteps.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpIntermediateSteps.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl39)).BeginInit();
            this.groupControl39.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit13.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit12.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit11.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit10.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcRotaryPositionDetermination)).BeginInit();
            this.GcRotaryPositionDetermination.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GcRotaryModeCounter)).BeginInit();
            this.GcRotaryModeCounter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbRotaryModeCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbRotaryModeCount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpRotaryModeCount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcRotaryModeSequence)).BeginInit();
            this.GcRotaryModeSequence.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueRotaryModeSequence.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl21)).BeginInit();
            this.groupControl21.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueRotaryPositionDetermination.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcBondingHeadAngleFromPositionSearch)).BeginInit();
            this.GcBondingHeadAngleFromPositionSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CeBondingHeadAngleFromPositionSearch.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.TpProgramming;
            this.xtraTabControl1.Size = new System.Drawing.Size(1238, 668);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.TpProgramming,
            this.TpDetails,
            this.TpOptimization});
            // 
            // TpProgramming
            // 
            this.TpProgramming.Controls.Add(this.gCDipMode);
            this.TpProgramming.Controls.Add(this.groupControl2);
            this.TpProgramming.Controls.Add(this.labelControl6);
            this.TpProgramming.Controls.Add(this.labelControl5);
            this.TpProgramming.Controls.Add(this.labelControl4);
            this.TpProgramming.Controls.Add(this.SpRotaryPosition);
            this.TpProgramming.Controls.Add(this.SpCarrierThickness);
            this.TpProgramming.Controls.Add(this.SpComponentThickness);
            this.TpProgramming.Controls.Add(this.labelControl3);
            this.TpProgramming.Controls.Add(this.labelControl2);
            this.TpProgramming.Controls.Add(this.labelControl1);
            this.TpProgramming.Controls.Add(this.groupControl4);
            this.TpProgramming.Controls.Add(this.groupControl12);
            this.TpProgramming.Controls.Add(this.groupControl11);
            this.TpProgramming.Controls.Add(this.GcCarrierShape);
            this.TpProgramming.Controls.Add(this.GcCarrierGeoSetupMethod);
            this.TpProgramming.Controls.Add(this.groupControl6);
            this.TpProgramming.Controls.Add(this.groupControl3);
            this.TpProgramming.Controls.Add(this.GcEjectionName);
            this.TpProgramming.Controls.Add(this.groupControl5);
            this.TpProgramming.Controls.Add(this.groupControl1);
            this.TpProgramming.Name = "TpProgramming";
            this.TpProgramming.Size = new System.Drawing.Size(1236, 642);
            this.TpProgramming.Text = "设计";
            // 
            // gCDipMode
            // 
            this.gCDipMode.Controls.Add(this.LueDipMode);
            this.gCDipMode.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.gCDipMode.Location = new System.Drawing.Point(666, 502);
            this.gCDipMode.Name = "gCDipMode";
            this.gCDipMode.Size = new System.Drawing.Size(313, 84);
            this.gCDipMode.TabIndex = 43;
            this.gCDipMode.Text = "蘸胶";
            // 
            // LueDipMode
            // 
            this.LueDipMode.Location = new System.Drawing.Point(15, 40);
            this.LueDipMode.Name = "LueDipMode";
            this.LueDipMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueDipMode.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "")});
            this.LueDipMode.Properties.DisplayMember = "Display";
            this.LueDipMode.Properties.NullText = "";
            this.LueDipMode.Properties.ValueMember = "Value";
            this.LueDipMode.Size = new System.Drawing.Size(280, 20);
            this.LueDipMode.TabIndex = 42;
            this.LueDipMode.EditValueChanged += new System.EventHandler(this.LueDipMode_EditValueChanged);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.BtnBlow);
            this.groupControl2.Controls.Add(this.SpWeakBlowProportion);
            this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl2.Location = new System.Drawing.Point(3, 409);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(349, 73);
            this.groupControl2.TabIndex = 44;
            this.groupControl2.Text = "吹气比例";
            // 
            // BtnBlow
            // 
            this.BtnBlow.Location = new System.Drawing.Point(224, 26);
            this.BtnBlow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnBlow.Name = "BtnBlow";
            this.BtnBlow.Size = new System.Drawing.Size(116, 29);
            this.BtnBlow.TabIndex = 53;
            this.BtnBlow.Text = "吹气 开/关";
            this.BtnBlow.Click += new System.EventHandler(this.BtnBlow_Click);
            // 
            // SpWeakBlowProportion
            // 
            this.SpWeakBlowProportion.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpWeakBlowProportion.Location = new System.Drawing.Point(16, 31);
            this.SpWeakBlowProportion.Name = "SpWeakBlowProportion";
            this.SpWeakBlowProportion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpWeakBlowProportion.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.SpWeakBlowProportion.Properties.IsFloatValue = false;
            this.SpWeakBlowProportion.Properties.MaxValue = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.SpWeakBlowProportion.Size = new System.Drawing.Size(203, 20);
            this.SpWeakBlowProportion.TabIndex = 52;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(320, 192);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(12, 14);
            this.labelControl6.TabIndex = 40;
            this.labelControl6.Text = "。";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(320, 169);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(20, 14);
            this.labelControl5.TabIndex = 40;
            this.labelControl5.Text = "mm";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(320, 138);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(20, 14);
            this.labelControl4.TabIndex = 40;
            this.labelControl4.Text = "mm";
            // 
            // SpRotaryPosition
            // 
            this.SpRotaryPosition.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpRotaryPosition.Location = new System.Drawing.Point(174, 196);
            this.SpRotaryPosition.Name = "SpRotaryPosition";
            this.SpRotaryPosition.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpRotaryPosition.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.SpRotaryPosition.Properties.MaxValue = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.SpRotaryPosition.Properties.MinValue = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.SpRotaryPosition.Size = new System.Drawing.Size(140, 20);
            this.SpRotaryPosition.TabIndex = 39;
            // 
            // SpCarrierThickness
            // 
            this.SpCarrierThickness.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpCarrierThickness.Location = new System.Drawing.Point(217, 166);
            this.SpCarrierThickness.Name = "SpCarrierThickness";
            this.SpCarrierThickness.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpCarrierThickness.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.SpCarrierThickness.Properties.MaxValue = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.SpCarrierThickness.Size = new System.Drawing.Size(97, 20);
            this.SpCarrierThickness.TabIndex = 39;
            // 
            // SpComponentThickness
            // 
            this.SpComponentThickness.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpComponentThickness.Location = new System.Drawing.Point(144, 135);
            this.SpComponentThickness.Name = "SpComponentThickness";
            this.SpComponentThickness.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpComponentThickness.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.SpComponentThickness.Properties.MaxValue = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.SpComponentThickness.Size = new System.Drawing.Size(170, 20);
            this.SpComponentThickness.TabIndex = 39;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(2, 199);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(72, 14);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "芯片旋转位置";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(3, 169);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(72, 14);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "芯片载具厚度";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(3, 138);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "芯片厚度";
            // 
            // groupControl4
            // 
            this.groupControl4.Controls.Add(this.CeIsFlip);
            this.groupControl4.Controls.Add(this.CeIsComponentDetection);
            this.groupControl4.Controls.Add(this.CeIsNineSearch);
            this.groupControl4.Controls.Add(this.CeIsRotationSearch);
            this.groupControl4.Controls.Add(this.CeIsPositionSearch);
            this.groupControl4.Controls.Add(this.CeIsBondWithProfile);
            this.groupControl4.Controls.Add(this.CeIsInkDotSearch);
            this.groupControl4.Controls.Add(this.CeIsMapping);
            this.groupControl4.Controls.Add(this.CeIsGraphicalSyn);
            this.groupControl4.Controls.Add(this.CeIsCarryOutSCT);
            this.groupControl4.Controls.Add(this.CeIsComponentAdjust);
            this.groupControl4.Controls.Add(this.CeIsReferenceDie);
            this.groupControl4.Controls.Add(this.CeIsBadChipTable);
            this.groupControl4.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl4.Location = new System.Drawing.Point(358, 3);
            this.groupControl4.Name = "groupControl4";
            this.groupControl4.Size = new System.Drawing.Size(302, 400);
            this.groupControl4.TabIndex = 0;
            this.groupControl4.Text = "选项";
            // 
            // CeIsFlip
            // 
            this.CeIsFlip.Enabled = false;
            this.CeIsFlip.Location = new System.Drawing.Point(17, 230);
            this.CeIsFlip.Name = "CeIsFlip";
            this.CeIsFlip.Properties.Caption = "使用翻转台";
            this.CeIsFlip.Size = new System.Drawing.Size(242, 20);
            this.CeIsFlip.TabIndex = 43;
            this.CeIsFlip.CheckedChanged += new System.EventHandler(this.CeIsFlip_CheckedChanged);
            // 
            // CeIsComponentDetection
            // 
            this.CeIsComponentDetection.Location = new System.Drawing.Point(17, 375);
            this.CeIsComponentDetection.Name = "CeIsComponentDetection";
            this.CeIsComponentDetection.Properties.Caption = "漏晶检测";
            this.CeIsComponentDetection.Size = new System.Drawing.Size(242, 20);
            this.CeIsComponentDetection.TabIndex = 42;
            // 
            // CeIsNineSearch
            // 
            this.CeIsNineSearch.Location = new System.Drawing.Point(17, 348);
            this.CeIsNineSearch.Name = "CeIsNineSearch";
            this.CeIsNineSearch.Properties.Caption = "九点搜索";
            this.CeIsNineSearch.Size = new System.Drawing.Size(242, 20);
            this.CeIsNineSearch.TabIndex = 41;
            // 
            // CeIsRotationSearch
            // 
            this.CeIsRotationSearch.Enabled = false;
            this.CeIsRotationSearch.Location = new System.Drawing.Point(17, 319);
            this.CeIsRotationSearch.Name = "CeIsRotationSearch";
            this.CeIsRotationSearch.Properties.Caption = "旋转搜索";
            this.CeIsRotationSearch.Size = new System.Drawing.Size(242, 20);
            this.CeIsRotationSearch.TabIndex = 41;
            // 
            // CeIsPositionSearch
            // 
            this.CeIsPositionSearch.Location = new System.Drawing.Point(17, 25);
            this.CeIsPositionSearch.Name = "CeIsPositionSearch";
            this.CeIsPositionSearch.Properties.Caption = "视觉定位";
            this.CeIsPositionSearch.Size = new System.Drawing.Size(242, 20);
            this.CeIsPositionSearch.TabIndex = 41;
            // 
            // CeIsBondWithProfile
            // 
            this.CeIsBondWithProfile.Enabled = false;
            this.CeIsBondWithProfile.Location = new System.Drawing.Point(17, 289);
            this.CeIsBondWithProfile.Name = "CeIsBondWithProfile";
            this.CeIsBondWithProfile.Properties.Caption = "Bond配置";
            this.CeIsBondWithProfile.Size = new System.Drawing.Size(242, 20);
            this.CeIsBondWithProfile.TabIndex = 41;
            // 
            // CeIsInkDotSearch
            // 
            this.CeIsInkDotSearch.Location = new System.Drawing.Point(17, 55);
            this.CeIsInkDotSearch.Name = "CeIsInkDotSearch";
            this.CeIsInkDotSearch.Properties.Caption = "墨点搜索";
            this.CeIsInkDotSearch.Size = new System.Drawing.Size(242, 20);
            this.CeIsInkDotSearch.TabIndex = 41;
            this.CeIsInkDotSearch.CheckedChanged += new System.EventHandler(this.CeIsInkDotSearch_CheckedChanged);
            // 
            // CeIsMapping
            // 
            this.CeIsMapping.Location = new System.Drawing.Point(17, 145);
            this.CeIsMapping.Name = "CeIsMapping";
            this.CeIsMapping.Properties.Caption = "晶圆图";
            this.CeIsMapping.Size = new System.Drawing.Size(242, 20);
            this.CeIsMapping.TabIndex = 41;
            // 
            // CeIsGraphicalSyn
            // 
            this.CeIsGraphicalSyn.Enabled = false;
            this.CeIsGraphicalSyn.Location = new System.Drawing.Point(17, 175);
            this.CeIsGraphicalSyn.Name = "CeIsGraphicalSyn";
            this.CeIsGraphicalSyn.Properties.Caption = "绘制同步";
            this.CeIsGraphicalSyn.Size = new System.Drawing.Size(242, 20);
            this.CeIsGraphicalSyn.TabIndex = 41;
            // 
            // CeIsCarryOutSCT
            // 
            this.CeIsCarryOutSCT.Enabled = false;
            this.CeIsCarryOutSCT.Location = new System.Drawing.Point(17, 258);
            this.CeIsCarryOutSCT.Name = "CeIsCarryOutSCT";
            this.CeIsCarryOutSCT.Properties.Caption = "Carry out SCT";
            this.CeIsCarryOutSCT.Size = new System.Drawing.Size(242, 20);
            this.CeIsCarryOutSCT.TabIndex = 41;
            // 
            // CeIsComponentAdjust
            // 
            this.CeIsComponentAdjust.Enabled = false;
            this.CeIsComponentAdjust.Location = new System.Drawing.Point(17, 86);
            this.CeIsComponentAdjust.Name = "CeIsComponentAdjust";
            this.CeIsComponentAdjust.Properties.Caption = "芯片载具调整";
            this.CeIsComponentAdjust.Size = new System.Drawing.Size(242, 20);
            this.CeIsComponentAdjust.TabIndex = 41;
            // 
            // CeIsReferenceDie
            // 
            this.CeIsReferenceDie.Enabled = false;
            this.CeIsReferenceDie.Location = new System.Drawing.Point(17, 115);
            this.CeIsReferenceDie.Name = "CeIsReferenceDie";
            this.CeIsReferenceDie.Properties.Caption = "参考点";
            this.CeIsReferenceDie.Size = new System.Drawing.Size(242, 20);
            this.CeIsReferenceDie.TabIndex = 41;
            // 
            // CeIsBadChipTable
            // 
            this.CeIsBadChipTable.Enabled = false;
            this.CeIsBadChipTable.Location = new System.Drawing.Point(17, 205);
            this.CeIsBadChipTable.Name = "CeIsBadChipTable";
            this.CeIsBadChipTable.Properties.Caption = "坏芯片表";
            this.CeIsBadChipTable.Size = new System.Drawing.Size(242, 20);
            this.CeIsBadChipTable.TabIndex = 41;
            // 
            // groupControl12
            // 
            this.groupControl12.Controls.Add(this.LueCarrierIDReaderMode);
            this.groupControl12.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl12.Location = new System.Drawing.Point(666, 398);
            this.groupControl12.Name = "groupControl12";
            this.groupControl12.Size = new System.Drawing.Size(313, 84);
            this.groupControl12.TabIndex = 0;
            this.groupControl12.Text = "芯片载具ID输入";
            // 
            // LueCarrierIDReaderMode
            // 
            this.LueCarrierIDReaderMode.Location = new System.Drawing.Point(15, 42);
            this.LueCarrierIDReaderMode.Name = "LueCarrierIDReaderMode";
            this.LueCarrierIDReaderMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueCarrierIDReaderMode.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "")});
            this.LueCarrierIDReaderMode.Properties.DisplayMember = "Display";
            this.LueCarrierIDReaderMode.Properties.NullText = "";
            this.LueCarrierIDReaderMode.Properties.ValueMember = "Value";
            this.LueCarrierIDReaderMode.Size = new System.Drawing.Size(280, 20);
            this.LueCarrierIDReaderMode.TabIndex = 42;
            // 
            // groupControl11
            // 
            this.groupControl11.Controls.Add(this.checkEdit1);
            this.groupControl11.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl11.Location = new System.Drawing.Point(666, 277);
            this.groupControl11.Name = "groupControl11";
            this.groupControl11.Size = new System.Drawing.Size(313, 84);
            this.groupControl11.TabIndex = 0;
            this.groupControl11.Text = "芯片关吹气选项";
            // 
            // checkEdit1
            // 
            this.checkEdit1.Location = new System.Drawing.Point(15, 39);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "回零后不关吹气";
            this.checkEdit1.Size = new System.Drawing.Size(280, 20);
            this.checkEdit1.TabIndex = 0;
            // 
            // GcCarrierShape
            // 
            this.GcCarrierShape.Controls.Add(this.LueCarrierShape);
            this.GcCarrierShape.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcCarrierShape.Location = new System.Drawing.Point(666, 187);
            this.GcCarrierShape.Name = "GcCarrierShape";
            this.GcCarrierShape.Size = new System.Drawing.Size(313, 84);
            this.GcCarrierShape.TabIndex = 0;
            this.GcCarrierShape.Text = "芯片载具形状";
            // 
            // LueCarrierShape
            // 
            this.LueCarrierShape.Location = new System.Drawing.Point(15, 42);
            this.LueCarrierShape.Name = "LueCarrierShape";
            this.LueCarrierShape.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueCarrierShape.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "")});
            this.LueCarrierShape.Properties.DisplayMember = "Display";
            this.LueCarrierShape.Properties.NullText = "";
            this.LueCarrierShape.Properties.ValueMember = "Value";
            this.LueCarrierShape.Size = new System.Drawing.Size(280, 20);
            this.LueCarrierShape.TabIndex = 42;
            // 
            // GcCarrierGeoSetupMethod
            // 
            this.GcCarrierGeoSetupMethod.Controls.Add(this.LueCarrierGeoSetMode);
            this.GcCarrierGeoSetupMethod.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcCarrierGeoSetupMethod.Location = new System.Drawing.Point(666, 97);
            this.GcCarrierGeoSetupMethod.Name = "GcCarrierGeoSetupMethod";
            this.GcCarrierGeoSetupMethod.Size = new System.Drawing.Size(313, 84);
            this.GcCarrierGeoSetupMethod.TabIndex = 0;
            this.GcCarrierGeoSetupMethod.Text = "芯片载具几何设置方法";
            // 
            // LueCarrierGeoSetMode
            // 
            this.LueCarrierGeoSetMode.Location = new System.Drawing.Point(15, 42);
            this.LueCarrierGeoSetMode.Name = "LueCarrierGeoSetMode";
            this.LueCarrierGeoSetMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueCarrierGeoSetMode.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "")});
            this.LueCarrierGeoSetMode.Properties.DisplayMember = "Display";
            this.LueCarrierGeoSetMode.Properties.NullText = "";
            this.LueCarrierGeoSetMode.Properties.ValueMember = "Value";
            this.LueCarrierGeoSetMode.Size = new System.Drawing.Size(280, 20);
            this.LueCarrierGeoSetMode.TabIndex = 42;
            // 
            // groupControl6
            // 
            this.groupControl6.Controls.Add(this.CeIsOneSnap);
            this.groupControl6.Controls.Add(this.CeIsWithOutWaferTableMovement);
            this.groupControl6.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl6.Location = new System.Drawing.Point(666, 3);
            this.groupControl6.Name = "groupControl6";
            this.groupControl6.Size = new System.Drawing.Size(313, 88);
            this.groupControl6.TabIndex = 0;
            this.groupControl6.Text = "芯片示教选项";
            // 
            // CeIsOneSnap
            // 
            this.CeIsOneSnap.Location = new System.Drawing.Point(15, 55);
            this.CeIsOneSnap.Name = "CeIsOneSnap";
            this.CeIsOneSnap.Properties.Caption = "一次移动";
            this.CeIsOneSnap.Size = new System.Drawing.Size(218, 20);
            this.CeIsOneSnap.TabIndex = 41;
            // 
            // CeIsWithOutWaferTableMovement
            // 
            this.CeIsWithOutWaferTableMovement.Location = new System.Drawing.Point(15, 29);
            this.CeIsWithOutWaferTableMovement.Name = "CeIsWithOutWaferTableMovement";
            this.CeIsWithOutWaferTableMovement.Properties.Caption = "不带晶圆台移动";
            this.CeIsWithOutWaferTableMovement.Size = new System.Drawing.Size(218, 20);
            this.CeIsWithOutWaferTableMovement.TabIndex = 41;
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.CmbNozzleName);
            this.groupControl3.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl3.Location = new System.Drawing.Point(3, 324);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(349, 79);
            this.groupControl3.TabIndex = 0;
            this.groupControl3.Text = "吸嘴";
            // 
            // CmbNozzleName
            // 
            this.CmbNozzleName.Location = new System.Drawing.Point(16, 38);
            this.CmbNozzleName.Name = "CmbNozzleName";
            this.CmbNozzleName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbNozzleName.Size = new System.Drawing.Size(295, 20);
            this.CmbNozzleName.TabIndex = 41;
            // 
            // GcEjectionName
            // 
            this.GcEjectionName.Controls.Add(this.BtnESUpOrDown);
            this.GcEjectionName.Controls.Add(this.CmbEjectionName);
            this.GcEjectionName.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcEjectionName.Location = new System.Drawing.Point(3, 233);
            this.GcEjectionName.Name = "GcEjectionName";
            this.GcEjectionName.Size = new System.Drawing.Size(349, 85);
            this.GcEjectionName.TabIndex = 0;
            this.GcEjectionName.Text = "顶针";
            // 
            // BtnESUpOrDown
            // 
            this.BtnESUpOrDown.Location = new System.Drawing.Point(224, 34);
            this.BtnESUpOrDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnESUpOrDown.Name = "BtnESUpOrDown";
            this.BtnESUpOrDown.Size = new System.Drawing.Size(116, 29);
            this.BtnESUpOrDown.TabIndex = 54;
            this.BtnESUpOrDown.Text = "顶针 上/下";
            this.BtnESUpOrDown.Click += new System.EventHandler(this.BtnESUpOrDown_Click);
            // 
            // CmbEjectionName
            // 
            this.CmbEjectionName.Location = new System.Drawing.Point(16, 38);
            this.CmbEjectionName.Name = "CmbEjectionName";
            this.CmbEjectionName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbEjectionName.Size = new System.Drawing.Size(203, 20);
            this.CmbEjectionName.TabIndex = 41;
            // 
            // groupControl5
            // 
            this.groupControl5.Controls.Add(this.LueAccuracyMode);
            this.groupControl5.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl5.Location = new System.Drawing.Point(358, 409);
            this.groupControl5.Name = "groupControl5";
            this.groupControl5.Size = new System.Drawing.Size(302, 73);
            this.groupControl5.TabIndex = 0;
            this.groupControl5.Text = "精度模式";
            // 
            // LueAccuracyMode
            // 
            this.LueAccuracyMode.Location = new System.Drawing.Point(17, 36);
            this.LueAccuracyMode.Name = "LueAccuracyMode";
            this.LueAccuracyMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueAccuracyMode.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "")});
            this.LueAccuracyMode.Properties.DisplayMember = "Display";
            this.LueAccuracyMode.Properties.NullText = "";
            this.LueAccuracyMode.Properties.ValueMember = "Value";
            this.LueAccuracyMode.Size = new System.Drawing.Size(264, 20);
            this.LueAccuracyMode.TabIndex = 43;
            this.LueAccuracyMode.EditValueChanged += new System.EventHandler(this.LueAccuracyMode_EditValueChanged);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.CeIsUseCarrierHeightMeasure);
            this.groupControl1.Controls.Add(this.LcThickness);
            this.groupControl1.Controls.Add(this.BtnMeasurementComponentAndCarrierThickness);
            this.groupControl1.Controls.Add(this.LcCarrierType);
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(3, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(349, 112);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "芯片载具类型";
            // 
            // CeIsUseCarrierHeightMeasure
            // 
            this.CeIsUseCarrierHeightMeasure.Location = new System.Drawing.Point(236, 33);
            this.CeIsUseCarrierHeightMeasure.Name = "CeIsUseCarrierHeightMeasure";
            this.CeIsUseCarrierHeightMeasure.Properties.Caption = "是否测厚";
            this.CeIsUseCarrierHeightMeasure.Size = new System.Drawing.Size(75, 20);
            this.CeIsUseCarrierHeightMeasure.TabIndex = 5;
            this.CeIsUseCarrierHeightMeasure.CheckedChanged += new System.EventHandler(this.CeIsUseCarrierHeightMeasure_CheckedChanged);
            // 
            // LcThickness
            // 
            this.LcThickness.Location = new System.Drawing.Point(16, 74);
            this.LcThickness.Name = "LcThickness";
            this.LcThickness.Size = new System.Drawing.Size(124, 14);
            this.LcThickness.TabIndex = 4;
            this.LcThickness.Text = "芯片+蓝膜总厚度：mm";
            // 
            // BtnMeasurementComponentAndCarrierThickness
            // 
            this.BtnMeasurementComponentAndCarrierThickness.Location = new System.Drawing.Point(236, 70);
            this.BtnMeasurementComponentAndCarrierThickness.Name = "BtnMeasurementComponentAndCarrierThickness";
            this.BtnMeasurementComponentAndCarrierThickness.Size = new System.Drawing.Size(75, 23);
            this.BtnMeasurementComponentAndCarrierThickness.TabIndex = 3;
            this.BtnMeasurementComponentAndCarrierThickness.Text = "测厚";
            this.BtnMeasurementComponentAndCarrierThickness.Click += new System.EventHandler(this.BtnMeasurementComponentAndCarrierThickness_Click);
            // 
            // LcCarrierType
            // 
            this.LcCarrierType.Location = new System.Drawing.Point(16, 36);
            this.LcCarrierType.Name = "LcCarrierType";
            this.LcCarrierType.Size = new System.Drawing.Size(48, 14);
            this.LcCarrierType.TabIndex = 2;
            this.LcCarrierType.Text = "芯片类型";
            // 
            // TpDetails
            // 
            this.TpDetails.Controls.Add(this.gCIPTType);
            this.TpDetails.Controls.Add(this.CeIsInverseWaferMapProcessing);
            this.TpDetails.Controls.Add(this.CeIsReferenceDieAsAlignmentDie);
            this.TpDetails.Controls.Add(this.GcStartDieVerificationMethods);
            this.TpDetails.Controls.Add(this.checkEdit3);
            this.TpDetails.Controls.Add(this.GcRotationSearch);
            this.TpDetails.Controls.Add(this.GcQualityForIntermediateHandling);
            this.TpDetails.Controls.Add(this.GcAccuracyWithCamera);
            this.TpDetails.Controls.Add(this.groupControl19);
            this.TpDetails.Controls.Add(this.GcCarrierAdjust);
            this.TpDetails.Controls.Add(this.groupControl24);
            this.TpDetails.Controls.Add(this.groupControl25);
            this.TpDetails.Controls.Add(this.GcInkDotSearch);
            this.TpDetails.Controls.Add(this.groupControl29);
            this.TpDetails.Controls.Add(this.GcTestComponents);
            this.TpDetails.Controls.Add(this.GcAccuracySearch);
            this.TpDetails.Controls.Add(this.GcPositionSearch);
            this.TpDetails.Name = "TpDetails";
            this.TpDetails.Size = new System.Drawing.Size(1236, 642);
            this.TpDetails.Text = "详细";
            // 
            // gCIPTType
            // 
            this.gCIPTType.Controls.Add(this.LueIPTType);
            this.gCIPTType.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.gCIPTType.Location = new System.Drawing.Point(419, 107);
            this.gCIPTType.Name = "gCIPTType";
            this.gCIPTType.Size = new System.Drawing.Size(411, 79);
            this.gCIPTType.TabIndex = 50;
            this.gCIPTType.Text = "中转台类型";
            // 
            // LueIPTType
            // 
            this.LueIPTType.Location = new System.Drawing.Point(39, 37);
            this.LueIPTType.Name = "LueIPTType";
            this.LueIPTType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueIPTType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "")});
            this.LueIPTType.Properties.DisplayMember = "Display";
            this.LueIPTType.Properties.NullText = "";
            this.LueIPTType.Properties.ValueMember = "Value";
            this.LueIPTType.Size = new System.Drawing.Size(327, 20);
            this.LueIPTType.TabIndex = 49;
            // 
            // CeIsInverseWaferMapProcessing
            // 
            this.CeIsInverseWaferMapProcessing.Location = new System.Drawing.Point(836, 483);
            this.CeIsInverseWaferMapProcessing.Name = "CeIsInverseWaferMapProcessing";
            this.CeIsInverseWaferMapProcessing.Properties.Caption = "逆晶圆图处理";
            this.CeIsInverseWaferMapProcessing.Size = new System.Drawing.Size(251, 20);
            this.CeIsInverseWaferMapProcessing.TabIndex = 45;
            // 
            // CeIsReferenceDieAsAlignmentDie
            // 
            this.CeIsReferenceDieAsAlignmentDie.Location = new System.Drawing.Point(836, 457);
            this.CeIsReferenceDieAsAlignmentDie.Name = "CeIsReferenceDieAsAlignmentDie";
            this.CeIsReferenceDieAsAlignmentDie.Properties.Caption = "参考点校准";
            this.CeIsReferenceDieAsAlignmentDie.Size = new System.Drawing.Size(251, 20);
            this.CeIsReferenceDieAsAlignmentDie.TabIndex = 45;
            // 
            // GcStartDieVerificationMethods
            // 
            this.GcStartDieVerificationMethods.Controls.Add(this.CeIsFirstDieIntegrityCheck);
            this.GcStartDieVerificationMethods.Controls.Add(this.CeIs8DieCheck);
            this.GcStartDieVerificationMethods.Controls.Add(this.CeIsEdgeDieCheck);
            this.GcStartDieVerificationMethods.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcStartDieVerificationMethods.Location = new System.Drawing.Point(836, 157);
            this.GcStartDieVerificationMethods.Name = "GcStartDieVerificationMethods";
            this.GcStartDieVerificationMethods.Size = new System.Drawing.Size(379, 122);
            this.GcStartDieVerificationMethods.TabIndex = 2;
            this.GcStartDieVerificationMethods.Text = "启动芯片校验方法";
            // 
            // CeIsFirstDieIntegrityCheck
            // 
            this.CeIsFirstDieIntegrityCheck.Location = new System.Drawing.Point(15, 86);
            this.CeIsFirstDieIntegrityCheck.Name = "CeIsFirstDieIntegrityCheck";
            this.CeIsFirstDieIntegrityCheck.Properties.Caption = "首芯片检查";
            this.CeIsFirstDieIntegrityCheck.Size = new System.Drawing.Size(247, 20);
            this.CeIsFirstDieIntegrityCheck.TabIndex = 46;
            // 
            // CeIs8DieCheck
            // 
            this.CeIs8DieCheck.Location = new System.Drawing.Point(15, 34);
            this.CeIs8DieCheck.Name = "CeIs8DieCheck";
            this.CeIs8DieCheck.Properties.Caption = "8芯片检查";
            this.CeIs8DieCheck.Size = new System.Drawing.Size(247, 20);
            this.CeIs8DieCheck.TabIndex = 46;
            // 
            // CeIsEdgeDieCheck
            // 
            this.CeIsEdgeDieCheck.Location = new System.Drawing.Point(15, 60);
            this.CeIsEdgeDieCheck.Name = "CeIsEdgeDieCheck";
            this.CeIsEdgeDieCheck.Properties.Caption = "边缘芯片检查";
            this.CeIsEdgeDieCheck.Size = new System.Drawing.Size(247, 20);
            this.CeIsEdgeDieCheck.TabIndex = 46;
            // 
            // checkEdit3
            // 
            this.checkEdit3.Location = new System.Drawing.Point(3, 299);
            this.checkEdit3.Name = "checkEdit3";
            this.checkEdit3.Properties.Caption = "可选择的拾取位置";
            this.checkEdit3.Size = new System.Drawing.Size(206, 20);
            this.checkEdit3.TabIndex = 44;
            // 
            // GcRotationSearch
            // 
            this.GcRotationSearch.Controls.Add(this.groupControl17);
            this.GcRotationSearch.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcRotationSearch.Location = new System.Drawing.Point(2, 192);
            this.GcRotationSearch.Name = "GcRotationSearch";
            this.GcRotationSearch.Size = new System.Drawing.Size(410, 100);
            this.GcRotationSearch.TabIndex = 12;
            this.GcRotationSearch.Text = "旋转搜索";
            // 
            // groupControl17
            // 
            this.groupControl17.Controls.Add(this.LueRotationSearchRange);
            this.groupControl17.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl17.Location = new System.Drawing.Point(16, 26);
            this.groupControl17.Name = "groupControl17";
            this.groupControl17.Size = new System.Drawing.Size(378, 61);
            this.groupControl17.TabIndex = 1;
            this.groupControl17.Text = "可能的芯片旋转";
            // 
            // LueRotationSearchRange
            // 
            this.LueRotationSearchRange.Location = new System.Drawing.Point(16, 26);
            this.LueRotationSearchRange.Name = "LueRotationSearchRange";
            this.LueRotationSearchRange.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueRotationSearchRange.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "")});
            this.LueRotationSearchRange.Properties.DisplayMember = "Display";
            this.LueRotationSearchRange.Properties.NullText = "";
            this.LueRotationSearchRange.Properties.ValueMember = "Value";
            this.LueRotationSearchRange.Size = new System.Drawing.Size(348, 20);
            this.LueRotationSearchRange.TabIndex = 46;
            // 
            // GcQualityForIntermediateHandling
            // 
            this.GcQualityForIntermediateHandling.Controls.Add(this.CeIsErrorMessageOnBadSearch);
            this.GcQualityForIntermediateHandling.Controls.Add(this.CeIsBad);
            this.GcQualityForIntermediateHandling.Controls.Add(this.CeIsUseReferenceDies);
            this.GcQualityForIntermediateHandling.Controls.Add(this.CeIsDoNotPick);
            this.GcQualityForIntermediateHandling.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcQualityForIntermediateHandling.Location = new System.Drawing.Point(836, 3);
            this.GcQualityForIntermediateHandling.Name = "GcQualityForIntermediateHandling";
            this.GcQualityForIntermediateHandling.Size = new System.Drawing.Size(379, 148);
            this.GcQualityForIntermediateHandling.TabIndex = 6;
            this.GcQualityForIntermediateHandling.Text = "中间处理的芯片质量";
            // 
            // CeIsErrorMessageOnBadSearch
            // 
            this.CeIsErrorMessageOnBadSearch.Location = new System.Drawing.Point(15, 111);
            this.CeIsErrorMessageOnBadSearch.Name = "CeIsErrorMessageOnBadSearch";
            this.CeIsErrorMessageOnBadSearch.Properties.Caption = "坏芯片搜索的错误信息";
            this.CeIsErrorMessageOnBadSearch.Size = new System.Drawing.Size(264, 20);
            this.CeIsErrorMessageOnBadSearch.TabIndex = 46;
            // 
            // CeIsBad
            // 
            this.CeIsBad.Location = new System.Drawing.Point(15, 33);
            this.CeIsBad.Name = "CeIsBad";
            this.CeIsBad.Properties.Caption = "坏";
            this.CeIsBad.Size = new System.Drawing.Size(264, 20);
            this.CeIsBad.TabIndex = 46;
            // 
            // CeIsUseReferenceDies
            // 
            this.CeIsUseReferenceDies.Location = new System.Drawing.Point(15, 85);
            this.CeIsUseReferenceDies.Name = "CeIsUseReferenceDies";
            this.CeIsUseReferenceDies.Properties.Caption = "使用参考点";
            this.CeIsUseReferenceDies.Size = new System.Drawing.Size(264, 20);
            this.CeIsUseReferenceDies.TabIndex = 46;
            // 
            // CeIsDoNotPick
            // 
            this.CeIsDoNotPick.Location = new System.Drawing.Point(15, 59);
            this.CeIsDoNotPick.Name = "CeIsDoNotPick";
            this.CeIsDoNotPick.Properties.Caption = "不取";
            this.CeIsDoNotPick.Size = new System.Drawing.Size(264, 20);
            this.CeIsDoNotPick.TabIndex = 46;
            // 
            // GcAccuracyWithCamera
            // 
            this.GcAccuracyWithCamera.Controls.Add(this.labelControl21);
            this.GcAccuracyWithCamera.Controls.Add(this.labelControl20);
            this.GcAccuracyWithCamera.Controls.Add(this.SpUplookVisionDelay);
            this.GcAccuracyWithCamera.Controls.Add(this.LueAjustCameraType);
            this.GcAccuracyWithCamera.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcAccuracyWithCamera.Location = new System.Drawing.Point(419, 3);
            this.GcAccuracyWithCamera.Name = "GcAccuracyWithCamera";
            this.GcAccuracyWithCamera.Size = new System.Drawing.Size(411, 98);
            this.GcAccuracyWithCamera.TabIndex = 8;
            this.GcAccuracyWithCamera.Text = "纠偏相机";
            // 
            // labelControl21
            // 
            this.labelControl21.Location = new System.Drawing.Point(17, 68);
            this.labelControl21.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl21.Name = "labelControl21";
            this.labelControl21.Size = new System.Drawing.Size(48, 14);
            this.labelControl21.TabIndex = 51;
            this.labelControl21.Text = "拍照延时";
            // 
            // labelControl20
            // 
            this.labelControl20.Location = new System.Drawing.Point(349, 68);
            this.labelControl20.Name = "labelControl20";
            this.labelControl20.Size = new System.Drawing.Size(15, 14);
            this.labelControl20.TabIndex = 49;
            this.labelControl20.Text = "ms";
            // 
            // SpUplookVisionDelay
            // 
            this.SpUplookVisionDelay.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpUplookVisionDelay.Location = new System.Drawing.Point(110, 65);
            this.SpUplookVisionDelay.Name = "SpUplookVisionDelay";
            this.SpUplookVisionDelay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpUplookVisionDelay.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.SpUplookVisionDelay.Properties.MaxValue = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.SpUplookVisionDelay.Size = new System.Drawing.Size(221, 20);
            this.SpUplookVisionDelay.TabIndex = 50;
            // 
            // LueAjustCameraType
            // 
            this.LueAjustCameraType.Location = new System.Drawing.Point(17, 34);
            this.LueAjustCameraType.Name = "LueAjustCameraType";
            this.LueAjustCameraType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueAjustCameraType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "")});
            this.LueAjustCameraType.Properties.DisplayMember = "Display";
            this.LueAjustCameraType.Properties.NullText = "";
            this.LueAjustCameraType.Properties.ValueMember = "Value";
            this.LueAjustCameraType.Size = new System.Drawing.Size(364, 20);
            this.LueAjustCameraType.TabIndex = 48;
            this.LueAjustCameraType.EditValueChanged += new System.EventHandler(this.LueAjustCameraType_EditValueChanged);
            // 
            // groupControl19
            // 
            this.groupControl19.Controls.Add(this.LueTabletChangeType);
            this.groupControl19.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl19.Location = new System.Drawing.Point(419, 431);
            this.groupControl19.Name = "groupControl19";
            this.groupControl19.Size = new System.Drawing.Size(411, 71);
            this.groupControl19.TabIndex = 8;
            this.groupControl19.Text = "芯片载具更换类型";
            // 
            // LueTabletChangeType
            // 
            this.LueTabletChangeType.Location = new System.Drawing.Point(17, 34);
            this.LueTabletChangeType.Name = "LueTabletChangeType";
            this.LueTabletChangeType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueTabletChangeType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "")});
            this.LueTabletChangeType.Properties.DisplayMember = "Display";
            this.LueTabletChangeType.Properties.NullText = "";
            this.LueTabletChangeType.Properties.ValueMember = "Value";
            this.LueTabletChangeType.Size = new System.Drawing.Size(364, 20);
            this.LueTabletChangeType.TabIndex = 52;
            // 
            // GcCarrierAdjust
            // 
            this.GcCarrierAdjust.Controls.Add(this.CeIsAfterEachChange);
            this.GcCarrierAdjust.Controls.Add(this.LueCarrierAdjustMode);
            this.GcCarrierAdjust.Controls.Add(this.CeIsDifferentSearches);
            this.GcCarrierAdjust.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcCarrierAdjust.Location = new System.Drawing.Point(419, 301);
            this.GcCarrierAdjust.Name = "GcCarrierAdjust";
            this.GcCarrierAdjust.Size = new System.Drawing.Size(411, 126);
            this.GcCarrierAdjust.TabIndex = 8;
            this.GcCarrierAdjust.Text = "芯片载具调整";
            // 
            // CeIsAfterEachChange
            // 
            this.CeIsAfterEachChange.Location = new System.Drawing.Point(16, 97);
            this.CeIsAfterEachChange.Name = "CeIsAfterEachChange";
            this.CeIsAfterEachChange.Properties.Caption = "每次改动后";
            this.CeIsAfterEachChange.Size = new System.Drawing.Size(217, 20);
            this.CeIsAfterEachChange.TabIndex = 45;
            // 
            // LueCarrierAdjustMode
            // 
            this.LueCarrierAdjustMode.Location = new System.Drawing.Point(16, 34);
            this.LueCarrierAdjustMode.Name = "LueCarrierAdjustMode";
            this.LueCarrierAdjustMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueCarrierAdjustMode.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "")});
            this.LueCarrierAdjustMode.Properties.DisplayMember = "Display";
            this.LueCarrierAdjustMode.Properties.NullText = "";
            this.LueCarrierAdjustMode.Properties.ValueMember = "Value";
            this.LueCarrierAdjustMode.Size = new System.Drawing.Size(364, 20);
            this.LueCarrierAdjustMode.TabIndex = 51;
            // 
            // CeIsDifferentSearches
            // 
            this.CeIsDifferentSearches.Location = new System.Drawing.Point(16, 71);
            this.CeIsDifferentSearches.Name = "CeIsDifferentSearches";
            this.CeIsDifferentSearches.Properties.Caption = "不同的搜索";
            this.CeIsDifferentSearches.Size = new System.Drawing.Size(217, 20);
            this.CeIsDifferentSearches.TabIndex = 45;
            // 
            // groupControl24
            // 
            this.groupControl24.Controls.Add(this.LueSearchCamera);
            this.groupControl24.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl24.Location = new System.Drawing.Point(4, 3);
            this.groupControl24.Name = "groupControl24";
            this.groupControl24.Size = new System.Drawing.Size(410, 71);
            this.groupControl24.TabIndex = 8;
            this.groupControl24.Text = "搜晶相机";
            // 
            // LueSearchCamera
            // 
            this.LueSearchCamera.Location = new System.Drawing.Point(16, 34);
            this.LueSearchCamera.Name = "LueSearchCamera";
            this.LueSearchCamera.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueSearchCamera.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "")});
            this.LueSearchCamera.Properties.DisplayMember = "Display";
            this.LueSearchCamera.Properties.NullText = "";
            this.LueSearchCamera.Properties.ValueMember = "Value";
            this.LueSearchCamera.Size = new System.Drawing.Size(364, 20);
            this.LueSearchCamera.TabIndex = 52;
            this.LueSearchCamera.EditValueChanged += new System.EventHandler(this.LueSearchCamera_EditValueChanged);
            // 
            // groupControl25
            // 
            this.groupControl25.Controls.Add(this.LueAutoRunWithDieMode);
            this.groupControl25.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl25.Location = new System.Drawing.Point(836, 374);
            this.groupControl25.Name = "groupControl25";
            this.groupControl25.Size = new System.Drawing.Size(379, 72);
            this.groupControl25.TabIndex = 9;
            this.groupControl25.Text = "自动运行在启动/校准芯片";
            // 
            // LueAutoRunWithDieMode
            // 
            this.LueAutoRunWithDieMode.Location = new System.Drawing.Point(15, 34);
            this.LueAutoRunWithDieMode.Name = "LueAutoRunWithDieMode";
            this.LueAutoRunWithDieMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueAutoRunWithDieMode.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "")});
            this.LueAutoRunWithDieMode.Properties.DisplayMember = "Display";
            this.LueAutoRunWithDieMode.Properties.NullText = "";
            this.LueAutoRunWithDieMode.Properties.ValueMember = "Value";
            this.LueAutoRunWithDieMode.Size = new System.Drawing.Size(349, 20);
            this.LueAutoRunWithDieMode.TabIndex = 53;
            // 
            // GcInkDotSearch
            // 
            this.GcInkDotSearch.Controls.Add(this.groupControl9);
            this.GcInkDotSearch.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcInkDotSearch.Location = new System.Drawing.Point(3, 325);
            this.GcInkDotSearch.Name = "GcInkDotSearch";
            this.GcInkDotSearch.Size = new System.Drawing.Size(410, 100);
            this.GcInkDotSearch.TabIndex = 12;
            this.GcInkDotSearch.Text = "墨点搜索";
            // 
            // groupControl9
            // 
            this.groupControl9.Controls.Add(this.SpNumberOfInkDot);
            this.groupControl9.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl9.Location = new System.Drawing.Point(16, 26);
            this.groupControl9.Name = "groupControl9";
            this.groupControl9.Size = new System.Drawing.Size(378, 61);
            this.groupControl9.TabIndex = 1;
            this.groupControl9.Text = "墨点连续搜索数量";
            // 
            // SpNumberOfInkDot
            // 
            this.SpNumberOfInkDot.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpNumberOfInkDot.Location = new System.Drawing.Point(16, 27);
            this.SpNumberOfInkDot.Name = "SpNumberOfInkDot";
            this.SpNumberOfInkDot.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpNumberOfInkDot.Properties.IsFloatValue = false;
            this.SpNumberOfInkDot.Properties.MaxValue = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.SpNumberOfInkDot.Size = new System.Drawing.Size(349, 20);
            this.SpNumberOfInkDot.TabIndex = 0;
            // 
            // groupControl29
            // 
            this.groupControl29.Controls.Add(this.TbMaxNumberOfComponentsWithoutSearch);
            this.groupControl29.Controls.Add(this.SpMaxNumberOfComponentsWithoutSearch);
            this.groupControl29.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl29.Location = new System.Drawing.Point(836, 285);
            this.groupControl29.Name = "groupControl29";
            this.groupControl29.Size = new System.Drawing.Size(379, 83);
            this.groupControl29.TabIndex = 13;
            this.groupControl29.Text = "芯片搜索最大跳过次数";
            // 
            // TbMaxNumberOfComponentsWithoutSearch
            // 
            this.TbMaxNumberOfComponentsWithoutSearch.EditValue = 20;
            this.TbMaxNumberOfComponentsWithoutSearch.Location = new System.Drawing.Point(15, 56);
            this.TbMaxNumberOfComponentsWithoutSearch.Name = "TbMaxNumberOfComponentsWithoutSearch";
            this.TbMaxNumberOfComponentsWithoutSearch.Properties.Maximum = 10000;
            this.TbMaxNumberOfComponentsWithoutSearch.Size = new System.Drawing.Size(349, 16);
            this.TbMaxNumberOfComponentsWithoutSearch.TabIndex = 12;
            this.TbMaxNumberOfComponentsWithoutSearch.Value = 20;
            this.TbMaxNumberOfComponentsWithoutSearch.EditValueChanged += new System.EventHandler(this.TbMaxNumberOfComponentsWithoutSearch_EditValueChanged);
            // 
            // SpMaxNumberOfComponentsWithoutSearch
            // 
            this.SpMaxNumberOfComponentsWithoutSearch.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpMaxNumberOfComponentsWithoutSearch.Location = new System.Drawing.Point(15, 30);
            this.SpMaxNumberOfComponentsWithoutSearch.Name = "SpMaxNumberOfComponentsWithoutSearch";
            this.SpMaxNumberOfComponentsWithoutSearch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpMaxNumberOfComponentsWithoutSearch.Properties.IsFloatValue = false;
            this.SpMaxNumberOfComponentsWithoutSearch.Properties.MaxValue = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.SpMaxNumberOfComponentsWithoutSearch.Size = new System.Drawing.Size(349, 20);
            this.SpMaxNumberOfComponentsWithoutSearch.TabIndex = 0;
            this.SpMaxNumberOfComponentsWithoutSearch.EditValueChanged += new System.EventHandler(this.SpMaxNumberOfComponentsWithoutSearch_EditValueChanged);
            // 
            // GcTestComponents
            // 
            this.GcTestComponents.Controls.Add(this.CeIsAfterColumns);
            this.GcTestComponents.Controls.Add(this.SpAfterNumberOfColumns);
            this.GcTestComponents.Controls.Add(this.CeIsAfterRows);
            this.GcTestComponents.Controls.Add(this.SpAfterNumberOfRows);
            this.GcTestComponents.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcTestComponents.Location = new System.Drawing.Point(3, 431);
            this.GcTestComponents.Name = "GcTestComponents";
            this.GcTestComponents.Size = new System.Drawing.Size(410, 96);
            this.GcTestComponents.TabIndex = 15;
            this.GcTestComponents.Text = "测试芯片";
            // 
            // CeIsAfterColumns
            // 
            this.CeIsAfterColumns.Location = new System.Drawing.Point(16, 57);
            this.CeIsAfterColumns.Name = "CeIsAfterColumns";
            this.CeIsAfterColumns.Properties.Caption = "空隔列数";
            this.CeIsAfterColumns.Size = new System.Drawing.Size(181, 20);
            this.CeIsAfterColumns.TabIndex = 45;
            // 
            // SpAfterNumberOfColumns
            // 
            this.SpAfterNumberOfColumns.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpAfterNumberOfColumns.Location = new System.Drawing.Point(213, 57);
            this.SpAfterNumberOfColumns.Name = "SpAfterNumberOfColumns";
            this.SpAfterNumberOfColumns.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpAfterNumberOfColumns.Properties.IsFloatValue = false;
            this.SpAfterNumberOfColumns.Size = new System.Drawing.Size(167, 20);
            this.SpAfterNumberOfColumns.TabIndex = 0;
            // 
            // CeIsAfterRows
            // 
            this.CeIsAfterRows.Location = new System.Drawing.Point(16, 31);
            this.CeIsAfterRows.Name = "CeIsAfterRows";
            this.CeIsAfterRows.Properties.Caption = "空隔行数";
            this.CeIsAfterRows.Size = new System.Drawing.Size(181, 20);
            this.CeIsAfterRows.TabIndex = 45;
            // 
            // SpAfterNumberOfRows
            // 
            this.SpAfterNumberOfRows.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpAfterNumberOfRows.Location = new System.Drawing.Point(213, 31);
            this.SpAfterNumberOfRows.Name = "SpAfterNumberOfRows";
            this.SpAfterNumberOfRows.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpAfterNumberOfRows.Properties.IsFloatValue = false;
            this.SpAfterNumberOfRows.Size = new System.Drawing.Size(167, 20);
            this.SpAfterNumberOfRows.TabIndex = 0;
            // 
            // GcAccuracySearch
            // 
            this.GcAccuracySearch.Controls.Add(this.ChkIsVisionCode);
            this.GcAccuracySearch.Controls.Add(this.ChkIsReferencePoint);
            this.GcAccuracySearch.Controls.Add(this.LueAccuracySearch);
            this.GcAccuracySearch.Controls.Add(this.simpleButton2);
            this.GcAccuracySearch.Controls.Add(this.CeTwoPointAdjust);
            this.GcAccuracySearch.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcAccuracySearch.Location = new System.Drawing.Point(419, 192);
            this.GcAccuracySearch.Name = "GcAccuracySearch";
            this.GcAccuracySearch.Size = new System.Drawing.Size(411, 100);
            this.GcAccuracySearch.TabIndex = 16;
            this.GcAccuracySearch.Text = "纠偏";
            // 
            // ChkIsVisionCode
            // 
            this.ChkIsVisionCode.Location = new System.Drawing.Point(275, 68);
            this.ChkIsVisionCode.Name = "ChkIsVisionCode";
            this.ChkIsVisionCode.Properties.Caption = "识别背部二维码";
            this.ChkIsVisionCode.Size = new System.Drawing.Size(131, 20);
            this.ChkIsVisionCode.TabIndex = 51;
            // 
            // ChkIsReferencePoint
            // 
            this.ChkIsReferencePoint.Location = new System.Drawing.Point(122, 68);
            this.ChkIsReferencePoint.Name = "ChkIsReferencePoint";
            this.ChkIsReferencePoint.Properties.Caption = "寻找另外参考点";
            this.ChkIsReferencePoint.Size = new System.Drawing.Size(111, 20);
            this.ChkIsReferencePoint.TabIndex = 50;
            // 
            // LueAccuracySearch
            // 
            this.LueAccuracySearch.Location = new System.Drawing.Point(16, 37);
            this.LueAccuracySearch.Name = "LueAccuracySearch";
            this.LueAccuracySearch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueAccuracySearch.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "")});
            this.LueAccuracySearch.Properties.DisplayMember = "Display";
            this.LueAccuracySearch.Properties.NullText = "";
            this.LueAccuracySearch.Properties.ValueMember = "Value";
            this.LueAccuracySearch.Size = new System.Drawing.Size(206, 20);
            this.LueAccuracySearch.TabIndex = 49;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(238, 36);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(168, 23);
            this.simpleButton2.TabIndex = 45;
            this.simpleButton2.Text = "配置搜索";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // CeTwoPointAdjust
            // 
            this.CeTwoPointAdjust.Location = new System.Drawing.Point(17, 68);
            this.CeTwoPointAdjust.Name = "CeTwoPointAdjust";
            this.CeTwoPointAdjust.Properties.Caption = "两点定位";
            this.CeTwoPointAdjust.Size = new System.Drawing.Size(89, 20);
            this.CeTwoPointAdjust.TabIndex = 44;
            // 
            // GcPositionSearch
            // 
            this.GcPositionSearch.Controls.Add(this.LuePositionSearchMode);
            this.GcPositionSearch.Controls.Add(this.BtnPositionSearchConfigure);
            this.GcPositionSearch.Controls.Add(this.CePositionSearch2);
            this.GcPositionSearch.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcPositionSearch.Location = new System.Drawing.Point(3, 88);
            this.GcPositionSearch.Name = "GcPositionSearch";
            this.GcPositionSearch.Size = new System.Drawing.Size(411, 98);
            this.GcPositionSearch.TabIndex = 16;
            this.GcPositionSearch.Text = "视觉定位";
            // 
            // LuePositionSearchMode
            // 
            this.LuePositionSearchMode.Location = new System.Drawing.Point(17, 37);
            this.LuePositionSearchMode.Name = "LuePositionSearchMode";
            this.LuePositionSearchMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LuePositionSearchMode.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "")});
            this.LuePositionSearchMode.Properties.DisplayMember = "Display";
            this.LuePositionSearchMode.Properties.NullText = "";
            this.LuePositionSearchMode.Properties.ValueMember = "Value";
            this.LuePositionSearchMode.Size = new System.Drawing.Size(206, 20);
            this.LuePositionSearchMode.TabIndex = 45;
            this.LuePositionSearchMode.EditValueChanged += new System.EventHandler(this.LuePositionSearch_EditValueChanged);
            // 
            // BtnPositionSearchConfigure
            // 
            this.BtnPositionSearchConfigure.Location = new System.Drawing.Point(238, 36);
            this.BtnPositionSearchConfigure.Name = "BtnPositionSearchConfigure";
            this.BtnPositionSearchConfigure.Size = new System.Drawing.Size(168, 23);
            this.BtnPositionSearchConfigure.TabIndex = 45;
            this.BtnPositionSearchConfigure.Text = "配置搜索";
            // 
            // CePositionSearch2
            // 
            this.CePositionSearch2.Location = new System.Drawing.Point(17, 63);
            this.CePositionSearch2.Name = "CePositionSearch2";
            this.CePositionSearch2.Properties.Caption = "两点搜索";
            this.CePositionSearch2.Size = new System.Drawing.Size(206, 20);
            this.CePositionSearch2.TabIndex = 44;
            // 
            // TpOptimization
            // 
            this.TpOptimization.Controls.Add(this.gCDip);
            this.TpOptimization.Controls.Add(this.GcNeedleMode);
            this.TpOptimization.Controls.Add(this.groupControl27);
            this.TpOptimization.Controls.Add(this.GcNeedleIntermediateSteps);
            this.TpOptimization.Controls.Add(this.groupControl39);
            this.TpOptimization.Controls.Add(this.GcRotaryPositionDetermination);
            this.TpOptimization.Name = "TpOptimization";
            this.TpOptimization.Size = new System.Drawing.Size(1236, 642);
            this.TpOptimization.Text = "优化";
            // 
            // gCDip
            // 
            this.gCDip.Controls.Add(this.labelControl32);
            this.gCDip.Controls.Add(this.SpDipDistance);
            this.gCDip.Controls.Add(this.labelControl33);
            this.gCDip.Controls.Add(this.labelControl30);
            this.gCDip.Controls.Add(this.SpDipDelay);
            this.gCDip.Controls.Add(this.labelControl31);
            this.gCDip.Controls.Add(this.groupControl13);
            this.gCDip.Controls.Add(this.groupControl14);
            this.gCDip.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.gCDip.Location = new System.Drawing.Point(845, 3);
            this.gCDip.Name = "gCDip";
            this.gCDip.Size = new System.Drawing.Size(388, 436);
            this.gCDip.TabIndex = 15;
            this.gCDip.Text = "蘸胶";
            // 
            // labelControl32
            // 
            this.labelControl32.Location = new System.Drawing.Point(332, 398);
            this.labelControl32.Name = "labelControl32";
            this.labelControl32.Size = new System.Drawing.Size(20, 14);
            this.labelControl32.TabIndex = 52;
            this.labelControl32.Text = "mm";
            // 
            // SpDipDistance
            // 
            this.SpDipDistance.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpDipDistance.Location = new System.Drawing.Point(90, 397);
            this.SpDipDistance.Name = "SpDipDistance";
            this.SpDipDistance.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpDipDistance.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.SpDipDistance.Properties.MaskSettings.Set("mask", "");
            this.SpDipDistance.Properties.MaxValue = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.SpDipDistance.Size = new System.Drawing.Size(208, 20);
            this.SpDipDistance.TabIndex = 51;
            // 
            // labelControl33
            // 
            this.labelControl33.Location = new System.Drawing.Point(15, 398);
            this.labelControl33.Name = "labelControl33";
            this.labelControl33.Size = new System.Drawing.Size(48, 14);
            this.labelControl33.TabIndex = 50;
            this.labelControl33.Text = "距离补偿";
            // 
            // labelControl30
            // 
            this.labelControl30.Location = new System.Drawing.Point(332, 354);
            this.labelControl30.Name = "labelControl30";
            this.labelControl30.Size = new System.Drawing.Size(15, 14);
            this.labelControl30.TabIndex = 49;
            this.labelControl30.Text = "ms";
            // 
            // SpDipDelay
            // 
            this.SpDipDelay.EditValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.SpDipDelay.Location = new System.Drawing.Point(90, 352);
            this.SpDipDelay.Name = "SpDipDelay";
            this.SpDipDelay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpDipDelay.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.SpDipDelay.Properties.IsFloatValue = false;
            this.SpDipDelay.Properties.MaskSettings.Set("mask", "N00");
            this.SpDipDelay.Properties.MaxValue = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.SpDipDelay.Size = new System.Drawing.Size(208, 20);
            this.SpDipDelay.TabIndex = 48;
            // 
            // labelControl31
            // 
            this.labelControl31.Location = new System.Drawing.Point(15, 354);
            this.labelControl31.Name = "labelControl31";
            this.labelControl31.Size = new System.Drawing.Size(48, 14);
            this.labelControl31.TabIndex = 47;
            this.labelControl31.Text = "蘸胶延时";
            // 
            // groupControl13
            // 
            this.groupControl13.Controls.Add(this.SpSlowTravleSpeedBeforeDip);
            this.groupControl13.Controls.Add(this.labelControl25);
            this.groupControl13.Controls.Add(this.labelControl22);
            this.groupControl13.Controls.Add(this.labelControl23);
            this.groupControl13.Controls.Add(this.SpSlowTravleDistanceBeforeDip);
            this.groupControl13.Controls.Add(this.labelControl24);
            this.groupControl13.Controls.Add(this.ChkIsActiveSlowDownBeforeDip);
            this.groupControl13.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl13.Location = new System.Drawing.Point(15, 26);
            this.groupControl13.Name = "groupControl13";
            this.groupControl13.Size = new System.Drawing.Size(360, 127);
            this.groupControl13.TabIndex = 9;
            this.groupControl13.Text = "蘸胶前二段速";
            // 
            // SpSlowTravleSpeedBeforeDip
            // 
            this.SpSlowTravleSpeedBeforeDip.EditValue = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.SpSlowTravleSpeedBeforeDip.Location = new System.Drawing.Point(64, 89);
            this.SpSlowTravleSpeedBeforeDip.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SpSlowTravleSpeedBeforeDip.Name = "SpSlowTravleSpeedBeforeDip";
            this.SpSlowTravleSpeedBeforeDip.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpSlowTravleSpeedBeforeDip.Properties.MaxValue = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.SpSlowTravleSpeedBeforeDip.Properties.MinValue = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            this.SpSlowTravleSpeedBeforeDip.Size = new System.Drawing.Size(234, 20);
            this.SpSlowTravleSpeedBeforeDip.TabIndex = 53;
            // 
            // labelControl25
            // 
            this.labelControl25.Location = new System.Drawing.Point(318, 91);
            this.labelControl25.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl25.Name = "labelControl25";
            this.labelControl25.Size = new System.Drawing.Size(30, 14);
            this.labelControl25.TabIndex = 52;
            this.labelControl25.Text = "mm/s";
            // 
            // labelControl22
            // 
            this.labelControl22.Location = new System.Drawing.Point(15, 57);
            this.labelControl22.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl22.Name = "labelControl22";
            this.labelControl22.Size = new System.Drawing.Size(24, 14);
            this.labelControl22.TabIndex = 51;
            this.labelControl22.Text = "距离";
            // 
            // labelControl23
            // 
            this.labelControl23.Location = new System.Drawing.Point(319, 61);
            this.labelControl23.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl23.Name = "labelControl23";
            this.labelControl23.Size = new System.Drawing.Size(20, 14);
            this.labelControl23.TabIndex = 49;
            this.labelControl23.Text = "mm";
            // 
            // SpSlowTravleDistanceBeforeDip
            // 
            this.SpSlowTravleDistanceBeforeDip.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.SpSlowTravleDistanceBeforeDip.Location = new System.Drawing.Point(64, 57);
            this.SpSlowTravleDistanceBeforeDip.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SpSlowTravleDistanceBeforeDip.Name = "SpSlowTravleDistanceBeforeDip";
            this.SpSlowTravleDistanceBeforeDip.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpSlowTravleDistanceBeforeDip.Properties.MaxValue = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.SpSlowTravleDistanceBeforeDip.Properties.MinValue = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            this.SpSlowTravleDistanceBeforeDip.Size = new System.Drawing.Size(234, 20);
            this.SpSlowTravleDistanceBeforeDip.TabIndex = 50;
            // 
            // labelControl24
            // 
            this.labelControl24.Location = new System.Drawing.Point(15, 91);
            this.labelControl24.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl24.Name = "labelControl24";
            this.labelControl24.Size = new System.Drawing.Size(24, 14);
            this.labelControl24.TabIndex = 47;
            this.labelControl24.Text = "速度";
            // 
            // ChkIsActiveSlowDownBeforeDip
            // 
            this.ChkIsActiveSlowDownBeforeDip.Location = new System.Drawing.Point(16, 25);
            this.ChkIsActiveSlowDownBeforeDip.Name = "ChkIsActiveSlowDownBeforeDip";
            this.ChkIsActiveSlowDownBeforeDip.Properties.Caption = "开启";
            this.ChkIsActiveSlowDownBeforeDip.Size = new System.Drawing.Size(317, 20);
            this.ChkIsActiveSlowDownBeforeDip.TabIndex = 46;
            this.ChkIsActiveSlowDownBeforeDip.CheckedChanged += new System.EventHandler(this.ChkIsActiveSlowDownBeforeDip_CheckedChanged);
            // 
            // groupControl14
            // 
            this.groupControl14.Controls.Add(this.SpSlowTravleSpeedAfterDip);
            this.groupControl14.Controls.Add(this.labelControl26);
            this.groupControl14.Controls.Add(this.labelControl27);
            this.groupControl14.Controls.Add(this.labelControl28);
            this.groupControl14.Controls.Add(this.SpSlowTravleDistanceAfterDip);
            this.groupControl14.Controls.Add(this.labelControl29);
            this.groupControl14.Controls.Add(this.ChkIsActiveSlowDownAfterDip);
            this.groupControl14.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl14.Location = new System.Drawing.Point(15, 185);
            this.groupControl14.Name = "groupControl14";
            this.groupControl14.Size = new System.Drawing.Size(360, 140);
            this.groupControl14.TabIndex = 4;
            this.groupControl14.Text = "蘸胶后二段速";
            // 
            // SpSlowTravleSpeedAfterDip
            // 
            this.SpSlowTravleSpeedAfterDip.EditValue = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.SpSlowTravleSpeedAfterDip.Location = new System.Drawing.Point(64, 97);
            this.SpSlowTravleSpeedAfterDip.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SpSlowTravleSpeedAfterDip.Name = "SpSlowTravleSpeedAfterDip";
            this.SpSlowTravleSpeedAfterDip.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpSlowTravleSpeedAfterDip.Properties.MaxValue = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.SpSlowTravleSpeedAfterDip.Properties.MinValue = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            this.SpSlowTravleSpeedAfterDip.Size = new System.Drawing.Size(234, 20);
            this.SpSlowTravleSpeedAfterDip.TabIndex = 60;
            // 
            // labelControl26
            // 
            this.labelControl26.Location = new System.Drawing.Point(318, 100);
            this.labelControl26.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl26.Name = "labelControl26";
            this.labelControl26.Size = new System.Drawing.Size(30, 14);
            this.labelControl26.TabIndex = 59;
            this.labelControl26.Text = "mm/s";
            // 
            // labelControl27
            // 
            this.labelControl27.Location = new System.Drawing.Point(15, 65);
            this.labelControl27.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl27.Name = "labelControl27";
            this.labelControl27.Size = new System.Drawing.Size(24, 14);
            this.labelControl27.TabIndex = 58;
            this.labelControl27.Text = "距离";
            // 
            // labelControl28
            // 
            this.labelControl28.Location = new System.Drawing.Point(319, 70);
            this.labelControl28.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl28.Name = "labelControl28";
            this.labelControl28.Size = new System.Drawing.Size(20, 14);
            this.labelControl28.TabIndex = 56;
            this.labelControl28.Text = "mm";
            // 
            // SpSlowTravleDistanceAfterDip
            // 
            this.SpSlowTravleDistanceAfterDip.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.SpSlowTravleDistanceAfterDip.Location = new System.Drawing.Point(64, 65);
            this.SpSlowTravleDistanceAfterDip.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SpSlowTravleDistanceAfterDip.Name = "SpSlowTravleDistanceAfterDip";
            this.SpSlowTravleDistanceAfterDip.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpSlowTravleDistanceAfterDip.Properties.MaxValue = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.SpSlowTravleDistanceAfterDip.Properties.MinValue = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            this.SpSlowTravleDistanceAfterDip.Size = new System.Drawing.Size(234, 20);
            this.SpSlowTravleDistanceAfterDip.TabIndex = 57;
            // 
            // labelControl29
            // 
            this.labelControl29.Location = new System.Drawing.Point(15, 100);
            this.labelControl29.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl29.Name = "labelControl29";
            this.labelControl29.Size = new System.Drawing.Size(24, 14);
            this.labelControl29.TabIndex = 55;
            this.labelControl29.Text = "速度";
            // 
            // ChkIsActiveSlowDownAfterDip
            // 
            this.ChkIsActiveSlowDownAfterDip.Location = new System.Drawing.Point(16, 25);
            this.ChkIsActiveSlowDownAfterDip.Name = "ChkIsActiveSlowDownAfterDip";
            this.ChkIsActiveSlowDownAfterDip.Properties.Caption = "开启";
            this.ChkIsActiveSlowDownAfterDip.Size = new System.Drawing.Size(317, 20);
            this.ChkIsActiveSlowDownAfterDip.TabIndex = 47;
            this.ChkIsActiveSlowDownAfterDip.CheckedChanged += new System.EventHandler(this.ChkIsActiveSlowDownAfterDip_CheckedChanged);
            // 
            // GcNeedleMode
            // 
            this.GcNeedleMode.Controls.Add(this.CeIsMultiPinEjectProcess);
            this.GcNeedleMode.Controls.Add(this.CeIsSynchronousEjection);
            this.GcNeedleMode.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcNeedleMode.Location = new System.Drawing.Point(3, 188);
            this.GcNeedleMode.Name = "GcNeedleMode";
            this.GcNeedleMode.Size = new System.Drawing.Size(407, 100);
            this.GcNeedleMode.TabIndex = 20;
            this.GcNeedleMode.Text = "针模式";
            // 
            // CeIsMultiPinEjectProcess
            // 
            this.CeIsMultiPinEjectProcess.Location = new System.Drawing.Point(14, 60);
            this.CeIsMultiPinEjectProcess.Name = "CeIsMultiPinEjectProcess";
            this.CeIsMultiPinEjectProcess.Properties.Caption = "多次顶";
            this.CeIsMultiPinEjectProcess.Size = new System.Drawing.Size(216, 20);
            this.CeIsMultiPinEjectProcess.TabIndex = 19;
            // 
            // CeIsSynchronousEjection
            // 
            this.CeIsSynchronousEjection.Location = new System.Drawing.Point(14, 34);
            this.CeIsSynchronousEjection.Name = "CeIsSynchronousEjection";
            this.CeIsSynchronousEjection.Properties.Caption = "同步顶";
            this.CeIsSynchronousEjection.Size = new System.Drawing.Size(216, 20);
            this.CeIsSynchronousEjection.TabIndex = 19;
            // 
            // groupControl27
            // 
            this.groupControl27.Controls.Add(this.LueMoveWithWaferTable);
            this.groupControl27.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl27.Location = new System.Drawing.Point(430, 521);
            this.groupControl27.Name = "groupControl27";
            this.groupControl27.Size = new System.Drawing.Size(403, 71);
            this.groupControl27.TabIndex = 9;
            this.groupControl27.Text = "晶圆台移动";
            // 
            // LueMoveWithWaferTable
            // 
            this.LueMoveWithWaferTable.Location = new System.Drawing.Point(30, 34);
            this.LueMoveWithWaferTable.Name = "LueMoveWithWaferTable";
            this.LueMoveWithWaferTable.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueMoveWithWaferTable.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "")});
            this.LueMoveWithWaferTable.Properties.DisplayMember = "Display";
            this.LueMoveWithWaferTable.Properties.NullText = "";
            this.LueMoveWithWaferTable.Properties.ValueMember = "Value";
            this.LueMoveWithWaferTable.Size = new System.Drawing.Size(339, 20);
            this.LueMoveWithWaferTable.TabIndex = 43;
            // 
            // GcNeedleIntermediateSteps
            // 
            this.GcNeedleIntermediateSteps.Controls.Add(this.labelControl11);
            this.GcNeedleIntermediateSteps.Controls.Add(this.labelControl10);
            this.GcNeedleIntermediateSteps.Controls.Add(this.labelControl9);
            this.GcNeedleIntermediateSteps.Controls.Add(this.labelControl8);
            this.GcNeedleIntermediateSteps.Controls.Add(this.labelControl7);
            this.GcNeedleIntermediateSteps.Controls.Add(this.TbStepDelay);
            this.GcNeedleIntermediateSteps.Controls.Add(this.spinEdit8);
            this.GcNeedleIntermediateSteps.Controls.Add(this.SpStepDelay);
            this.GcNeedleIntermediateSteps.Controls.Add(this.TbIntermediateSteps);
            this.GcNeedleIntermediateSteps.Controls.Add(this.SpIntermediateSteps);
            this.GcNeedleIntermediateSteps.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcNeedleIntermediateSteps.Location = new System.Drawing.Point(3, 3);
            this.GcNeedleIntermediateSteps.Name = "GcNeedleIntermediateSteps";
            this.GcNeedleIntermediateSteps.Size = new System.Drawing.Size(407, 179);
            this.GcNeedleIntermediateSteps.TabIndex = 17;
            this.GcNeedleIntermediateSteps.Text = "顶起步数";
            // 
            // labelControl11
            // 
            this.labelControl11.Location = new System.Drawing.Point(369, 120);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(15, 14);
            this.labelControl11.TabIndex = 41;
            this.labelControl11.Text = "ms";
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(369, 87);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(20, 14);
            this.labelControl10.TabIndex = 41;
            this.labelControl10.Text = "mm";
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(14, 130);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(60, 14);
            this.labelControl9.TabIndex = 13;
            this.labelControl9.Text = "针单步延时";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(14, 87);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(48, 14);
            this.labelControl8.TabIndex = 13;
            this.labelControl8.Text = "顶起位置";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(14, 41);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(48, 14);
            this.labelControl7.TabIndex = 13;
            this.labelControl7.Text = "顶起步数";
            // 
            // TbStepDelay
            // 
            this.TbStepDelay.EditValue = 20;
            this.TbStepDelay.Location = new System.Drawing.Point(136, 143);
            this.TbStepDelay.Name = "TbStepDelay";
            this.TbStepDelay.Properties.Maximum = 60000;
            this.TbStepDelay.Size = new System.Drawing.Size(227, 16);
            this.TbStepDelay.TabIndex = 12;
            this.TbStepDelay.Value = 20;
            this.TbStepDelay.EditValueChanged += new System.EventHandler(this.TbStepDelay_EditValueChanged);
            // 
            // spinEdit8
            // 
            this.spinEdit8.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit8.Location = new System.Drawing.Point(201, 84);
            this.spinEdit8.Name = "spinEdit8";
            this.spinEdit8.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit8.Size = new System.Drawing.Size(162, 20);
            this.spinEdit8.TabIndex = 0;
            // 
            // SpStepDelay
            // 
            this.SpStepDelay.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpStepDelay.Location = new System.Drawing.Point(136, 117);
            this.SpStepDelay.Name = "SpStepDelay";
            this.SpStepDelay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpStepDelay.Properties.IsFloatValue = false;
            this.SpStepDelay.Properties.MaxValue = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.SpStepDelay.Size = new System.Drawing.Size(227, 20);
            this.SpStepDelay.TabIndex = 0;
            this.SpStepDelay.EditValueChanged += new System.EventHandler(this.SpStepDelay_EditValueChanged);
            // 
            // TbIntermediateSteps
            // 
            this.TbIntermediateSteps.EditValue = null;
            this.TbIntermediateSteps.Location = new System.Drawing.Point(183, 56);
            this.TbIntermediateSteps.Name = "TbIntermediateSteps";
            this.TbIntermediateSteps.Properties.Maximum = 100;
            this.TbIntermediateSteps.Size = new System.Drawing.Size(180, 16);
            this.TbIntermediateSteps.TabIndex = 12;
            this.TbIntermediateSteps.EditValueChanged += new System.EventHandler(this.TbIntermediateSteps_EditValueChanged);
            // 
            // SpIntermediateSteps
            // 
            this.SpIntermediateSteps.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpIntermediateSteps.Location = new System.Drawing.Point(183, 30);
            this.SpIntermediateSteps.Name = "SpIntermediateSteps";
            this.SpIntermediateSteps.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpIntermediateSteps.Properties.IsFloatValue = false;
            this.SpIntermediateSteps.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.SpIntermediateSteps.Size = new System.Drawing.Size(180, 20);
            this.SpIntermediateSteps.TabIndex = 0;
            this.SpIntermediateSteps.EditValueChanged += new System.EventHandler(this.SpIntermediateSteps_EditValueChanged);
            // 
            // groupControl39
            // 
            this.groupControl39.Controls.Add(this.spinEdit13);
            this.groupControl39.Controls.Add(this.spinEdit12);
            this.groupControl39.Controls.Add(this.spinEdit11);
            this.groupControl39.Controls.Add(this.spinEdit10);
            this.groupControl39.Controls.Add(this.labelControl15);
            this.groupControl39.Controls.Add(this.labelControl14);
            this.groupControl39.Controls.Add(this.labelControl13);
            this.groupControl39.Controls.Add(this.labelControl19);
            this.groupControl39.Controls.Add(this.labelControl18);
            this.groupControl39.Controls.Add(this.labelControl17);
            this.groupControl39.Controls.Add(this.labelControl16);
            this.groupControl39.Controls.Add(this.labelControl12);
            this.groupControl39.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl39.Location = new System.Drawing.Point(430, 357);
            this.groupControl39.Name = "groupControl39";
            this.groupControl39.Size = new System.Drawing.Size(403, 158);
            this.groupControl39.TabIndex = 7;
            this.groupControl39.Text = "速率";
            // 
            // spinEdit13
            // 
            this.spinEdit13.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit13.Location = new System.Drawing.Point(102, 120);
            this.spinEdit13.Name = "spinEdit13";
            this.spinEdit13.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit13.Properties.IsFloatValue = false;
            this.spinEdit13.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.spinEdit13.Size = new System.Drawing.Size(266, 20);
            this.spinEdit13.TabIndex = 0;
            // 
            // spinEdit12
            // 
            this.spinEdit12.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit12.Location = new System.Drawing.Point(102, 92);
            this.spinEdit12.Name = "spinEdit12";
            this.spinEdit12.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit12.Properties.IsFloatValue = false;
            this.spinEdit12.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.spinEdit12.Size = new System.Drawing.Size(266, 20);
            this.spinEdit12.TabIndex = 0;
            // 
            // spinEdit11
            // 
            this.spinEdit11.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit11.Location = new System.Drawing.Point(102, 64);
            this.spinEdit11.Name = "spinEdit11";
            this.spinEdit11.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit11.Properties.IsFloatValue = false;
            this.spinEdit11.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.spinEdit11.Size = new System.Drawing.Size(266, 20);
            this.spinEdit11.TabIndex = 0;
            // 
            // spinEdit10
            // 
            this.spinEdit10.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit10.Location = new System.Drawing.Point(102, 36);
            this.spinEdit10.Name = "spinEdit10";
            this.spinEdit10.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit10.Properties.IsFloatValue = false;
            this.spinEdit10.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.spinEdit10.Size = new System.Drawing.Size(266, 20);
            this.spinEdit10.TabIndex = 0;
            // 
            // labelControl15
            // 
            this.labelControl15.Location = new System.Drawing.Point(15, 123);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(36, 14);
            this.labelControl15.TabIndex = 13;
            this.labelControl15.Text = "晶圆台";
            // 
            // labelControl14
            // 
            this.labelControl14.Location = new System.Drawing.Point(15, 95);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(36, 14);
            this.labelControl14.TabIndex = 13;
            this.labelControl14.Text = "晶圆夹";
            // 
            // labelControl13
            // 
            this.labelControl13.Location = new System.Drawing.Point(15, 67);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(60, 14);
            this.labelControl13.TabIndex = 13;
            this.labelControl13.Text = "晶圆提升器";
            // 
            // labelControl19
            // 
            this.labelControl19.Location = new System.Drawing.Point(381, 124);
            this.labelControl19.Name = "labelControl19";
            this.labelControl19.Size = new System.Drawing.Size(12, 14);
            this.labelControl19.TabIndex = 13;
            this.labelControl19.Text = "%";
            // 
            // labelControl18
            // 
            this.labelControl18.Location = new System.Drawing.Point(382, 96);
            this.labelControl18.Name = "labelControl18";
            this.labelControl18.Size = new System.Drawing.Size(12, 14);
            this.labelControl18.TabIndex = 13;
            this.labelControl18.Text = "%";
            // 
            // labelControl17
            // 
            this.labelControl17.Location = new System.Drawing.Point(381, 68);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(12, 14);
            this.labelControl17.TabIndex = 13;
            this.labelControl17.Text = "%";
            // 
            // labelControl16
            // 
            this.labelControl16.Location = new System.Drawing.Point(381, 40);
            this.labelControl16.Name = "labelControl16";
            this.labelControl16.Size = new System.Drawing.Size(12, 14);
            this.labelControl16.TabIndex = 13;
            this.labelControl16.Text = "%";
            // 
            // labelControl12
            // 
            this.labelControl12.Location = new System.Drawing.Point(15, 40);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(24, 14);
            this.labelControl12.TabIndex = 13;
            this.labelControl12.Text = "晶圆";
            // 
            // GcRotaryPositionDetermination
            // 
            this.GcRotaryPositionDetermination.Controls.Add(this.GcRotaryModeCounter);
            this.GcRotaryPositionDetermination.Controls.Add(this.GcRotaryModeSequence);
            this.GcRotaryPositionDetermination.Controls.Add(this.groupControl21);
            this.GcRotaryPositionDetermination.Controls.Add(this.GcBondingHeadAngleFromPositionSearch);
            this.GcRotaryPositionDetermination.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcRotaryPositionDetermination.Location = new System.Drawing.Point(430, 3);
            this.GcRotaryPositionDetermination.Name = "GcRotaryPositionDetermination";
            this.GcRotaryPositionDetermination.Size = new System.Drawing.Size(403, 348);
            this.GcRotaryPositionDetermination.TabIndex = 12;
            this.GcRotaryPositionDetermination.Text = "旋转位置测定";
            // 
            // GcRotaryModeCounter
            // 
            this.GcRotaryModeCounter.Controls.Add(this.TbRotaryModeCount);
            this.GcRotaryModeCounter.Controls.Add(this.SpRotaryModeCount);
            this.GcRotaryModeCounter.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcRotaryModeCounter.Location = new System.Drawing.Point(15, 253);
            this.GcRotaryModeCounter.Name = "GcRotaryModeCounter";
            this.GcRotaryModeCounter.Size = new System.Drawing.Size(380, 83);
            this.GcRotaryModeCounter.TabIndex = 14;
            this.GcRotaryModeCounter.Text = "旋转模式计数";
            // 
            // TbRotaryModeCount
            // 
            this.TbRotaryModeCount.EditValue = null;
            this.TbRotaryModeCount.Location = new System.Drawing.Point(15, 56);
            this.TbRotaryModeCount.Name = "TbRotaryModeCount";
            this.TbRotaryModeCount.Properties.Maximum = 20;
            this.TbRotaryModeCount.Size = new System.Drawing.Size(354, 16);
            this.TbRotaryModeCount.TabIndex = 12;
            this.TbRotaryModeCount.EditValueChanged += new System.EventHandler(this.TbRotaryModeCount_EditValueChanged);
            // 
            // SpRotaryModeCount
            // 
            this.SpRotaryModeCount.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpRotaryModeCount.Location = new System.Drawing.Point(15, 30);
            this.SpRotaryModeCount.Name = "SpRotaryModeCount";
            this.SpRotaryModeCount.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpRotaryModeCount.Properties.IsFloatValue = false;
            this.SpRotaryModeCount.Properties.MaxValue = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.SpRotaryModeCount.Size = new System.Drawing.Size(354, 20);
            this.SpRotaryModeCount.TabIndex = 0;
            this.SpRotaryModeCount.EditValueChanged += new System.EventHandler(this.SpRotaryModeCount_EditValueChanged);
            // 
            // GcRotaryModeSequence
            // 
            this.GcRotaryModeSequence.Controls.Add(this.LueRotaryModeSequence);
            this.GcRotaryModeSequence.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcRotaryModeSequence.Location = new System.Drawing.Point(15, 176);
            this.GcRotaryModeSequence.Name = "GcRotaryModeSequence";
            this.GcRotaryModeSequence.Size = new System.Drawing.Size(379, 71);
            this.GcRotaryModeSequence.TabIndex = 9;
            this.GcRotaryModeSequence.Text = "旋转模式顺序";
            // 
            // LueRotaryModeSequence
            // 
            this.LueRotaryModeSequence.Location = new System.Drawing.Point(15, 34);
            this.LueRotaryModeSequence.Name = "LueRotaryModeSequence";
            this.LueRotaryModeSequence.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueRotaryModeSequence.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "")});
            this.LueRotaryModeSequence.Properties.DisplayMember = "Display";
            this.LueRotaryModeSequence.Properties.NullText = "";
            this.LueRotaryModeSequence.Properties.ValueMember = "Value";
            this.LueRotaryModeSequence.Size = new System.Drawing.Size(354, 20);
            this.LueRotaryModeSequence.TabIndex = 43;
            // 
            // groupControl21
            // 
            this.groupControl21.Controls.Add(this.LueRotaryPositionDetermination);
            this.groupControl21.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl21.Location = new System.Drawing.Point(15, 26);
            this.groupControl21.Name = "groupControl21";
            this.groupControl21.Size = new System.Drawing.Size(380, 71);
            this.groupControl21.TabIndex = 9;
            this.groupControl21.Text = "旋转定位";
            // 
            // LueRotaryPositionDetermination
            // 
            this.LueRotaryPositionDetermination.Location = new System.Drawing.Point(15, 34);
            this.LueRotaryPositionDetermination.Name = "LueRotaryPositionDetermination";
            this.LueRotaryPositionDetermination.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueRotaryPositionDetermination.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "")});
            this.LueRotaryPositionDetermination.Properties.DisplayMember = "Display";
            this.LueRotaryPositionDetermination.Properties.NullText = "";
            this.LueRotaryPositionDetermination.Properties.ValueMember = "Value";
            this.LueRotaryPositionDetermination.Size = new System.Drawing.Size(354, 20);
            this.LueRotaryPositionDetermination.TabIndex = 43;
            // 
            // GcBondingHeadAngleFromPositionSearch
            // 
            this.GcBondingHeadAngleFromPositionSearch.Controls.Add(this.CeBondingHeadAngleFromPositionSearch);
            this.GcBondingHeadAngleFromPositionSearch.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcBondingHeadAngleFromPositionSearch.Location = new System.Drawing.Point(15, 103);
            this.GcBondingHeadAngleFromPositionSearch.Name = "GcBondingHeadAngleFromPositionSearch";
            this.GcBondingHeadAngleFromPositionSearch.Size = new System.Drawing.Size(380, 67);
            this.GcBondingHeadAngleFromPositionSearch.TabIndex = 4;
            this.GcBondingHeadAngleFromPositionSearch.Text = "Bond头角度来源";
            // 
            // CeBondingHeadAngleFromPositionSearch
            // 
            this.CeBondingHeadAngleFromPositionSearch.Location = new System.Drawing.Point(16, 31);
            this.CeBondingHeadAngleFromPositionSearch.Name = "CeBondingHeadAngleFromPositionSearch";
            this.CeBondingHeadAngleFromPositionSearch.Properties.Caption = "来自定位识别";
            this.CeBondingHeadAngleFromPositionSearch.Size = new System.Drawing.Size(354, 20);
            this.CeBondingHeadAngleFromPositionSearch.TabIndex = 45;
            // 
            // timer
            // 
            this.timer.Tag = "UcComponentEdit";
            this.timer.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // UcComponentEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "UcComponentEdit";
            this.Size = new System.Drawing.Size(1238, 668);
            this.Load += new System.EventHandler(this.UcComponentEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.TpProgramming.ResumeLayout(false);
            this.TpProgramming.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gCDipMode)).EndInit();
            this.gCDipMode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueDipMode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SpWeakBlowProportion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpRotaryPosition.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpCarrierThickness.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpComponentThickness.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
            this.groupControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CeIsFlip.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsComponentDetection.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsNineSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsRotationSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsPositionSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsBondWithProfile.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsInkDotSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsMapping.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsGraphicalSyn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsCarryOutSCT.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsComponentAdjust.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsReferenceDie.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsBadChipTable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl12)).EndInit();
            this.groupControl12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueCarrierIDReaderMode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl11)).EndInit();
            this.groupControl11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcCarrierShape)).EndInit();
            this.GcCarrierShape.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueCarrierShape.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcCarrierGeoSetupMethod)).EndInit();
            this.GcCarrierGeoSetupMethod.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueCarrierGeoSetMode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).EndInit();
            this.groupControl6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CeIsOneSnap.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsWithOutWaferTableMovement.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CmbNozzleName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcEjectionName)).EndInit();
            this.GcEjectionName.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CmbEjectionName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).EndInit();
            this.groupControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueAccuracyMode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsUseCarrierHeightMeasure.Properties)).EndInit();
            this.TpDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gCIPTType)).EndInit();
            this.gCIPTType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueIPTType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsInverseWaferMapProcessing.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsReferenceDieAsAlignmentDie.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcStartDieVerificationMethods)).EndInit();
            this.GcStartDieVerificationMethods.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CeIsFirstDieIntegrityCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIs8DieCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsEdgeDieCheck.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcRotationSearch)).EndInit();
            this.GcRotationSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl17)).EndInit();
            this.groupControl17.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueRotationSearchRange.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcQualityForIntermediateHandling)).EndInit();
            this.GcQualityForIntermediateHandling.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CeIsErrorMessageOnBadSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsBad.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsUseReferenceDies.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsDoNotPick.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcAccuracyWithCamera)).EndInit();
            this.GcAccuracyWithCamera.ResumeLayout(false);
            this.GcAccuracyWithCamera.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpUplookVisionDelay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueAjustCameraType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl19)).EndInit();
            this.groupControl19.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueTabletChangeType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcCarrierAdjust)).EndInit();
            this.GcCarrierAdjust.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CeIsAfterEachChange.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueCarrierAdjustMode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsDifferentSearches.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl24)).EndInit();
            this.groupControl24.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueSearchCamera.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl25)).EndInit();
            this.groupControl25.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueAutoRunWithDieMode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcInkDotSearch)).EndInit();
            this.GcInkDotSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl9)).EndInit();
            this.groupControl9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SpNumberOfInkDot.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl29)).EndInit();
            this.groupControl29.ResumeLayout(false);
            this.groupControl29.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbMaxNumberOfComponentsWithoutSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbMaxNumberOfComponentsWithoutSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpMaxNumberOfComponentsWithoutSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcTestComponents)).EndInit();
            this.GcTestComponents.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CeIsAfterColumns.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpAfterNumberOfColumns.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsAfterRows.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpAfterNumberOfRows.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcAccuracySearch)).EndInit();
            this.GcAccuracySearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ChkIsVisionCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkIsReferencePoint.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueAccuracySearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeTwoPointAdjust.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcPositionSearch)).EndInit();
            this.GcPositionSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LuePositionSearchMode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CePositionSearch2.Properties)).EndInit();
            this.TpOptimization.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gCDip)).EndInit();
            this.gCDip.ResumeLayout(false);
            this.gCDip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpDipDistance.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpDipDelay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl13)).EndInit();
            this.groupControl13.ResumeLayout(false);
            this.groupControl13.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlowTravleSpeedBeforeDip.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlowTravleDistanceBeforeDip.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkIsActiveSlowDownBeforeDip.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl14)).EndInit();
            this.groupControl14.ResumeLayout(false);
            this.groupControl14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlowTravleSpeedAfterDip.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlowTravleDistanceAfterDip.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkIsActiveSlowDownAfterDip.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcNeedleMode)).EndInit();
            this.GcNeedleMode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CeIsMultiPinEjectProcess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsSynchronousEjection.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl27)).EndInit();
            this.groupControl27.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueMoveWithWaferTable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcNeedleIntermediateSteps)).EndInit();
            this.GcNeedleIntermediateSteps.ResumeLayout(false);
            this.GcNeedleIntermediateSteps.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbStepDelay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbStepDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit8.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpStepDelay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbIntermediateSteps.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbIntermediateSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpIntermediateSteps.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl39)).EndInit();
            this.groupControl39.ResumeLayout(false);
            this.groupControl39.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit13.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit12.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit11.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit10.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcRotaryPositionDetermination)).EndInit();
            this.GcRotaryPositionDetermination.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GcRotaryModeCounter)).EndInit();
            this.GcRotaryModeCounter.ResumeLayout(false);
            this.GcRotaryModeCounter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbRotaryModeCount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbRotaryModeCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpRotaryModeCount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcRotaryModeSequence)).EndInit();
            this.GcRotaryModeSequence.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueRotaryModeSequence.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl21)).EndInit();
            this.groupControl21.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueRotaryPositionDetermination.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcBondingHeadAngleFromPositionSearch)).EndInit();
            this.GcBondingHeadAngleFromPositionSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CeBondingHeadAngleFromPositionSearch.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage TpProgramming;
        private DevExpress.XtraTab.XtraTabPage TpDetails;
        private DevExpress.XtraTab.XtraTabPage TpOptimization;
        private DevExpress.XtraEditors.GroupControl groupControl4;
        private DevExpress.XtraEditors.GroupControl GcCarrierGeoSetupMethod;
        private DevExpress.XtraEditors.GroupControl groupControl6;
        private DevExpress.XtraEditors.GroupControl GcEjectionName;
        private DevExpress.XtraEditors.GroupControl groupControl5;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl17;
        private DevExpress.XtraEditors.GroupControl GcStartDieVerificationMethods;
        private DevExpress.XtraEditors.GroupControl GcQualityForIntermediateHandling;
        private DevExpress.XtraEditors.GroupControl groupControl24;
        private DevExpress.XtraEditors.GroupControl groupControl25;
        private DevExpress.XtraEditors.GroupControl GcRotationSearch;
        private DevExpress.XtraEditors.GroupControl groupControl29;
        private DevExpress.XtraEditors.GroupControl GcTestComponents;
        private DevExpress.XtraEditors.GroupControl GcPositionSearch;
        private DevExpress.XtraEditors.GroupControl GcBondingHeadAngleFromPositionSearch;
        private DevExpress.XtraEditors.GroupControl groupControl39;
        private DevExpress.XtraEditors.GroupControl GcRotaryPositionDetermination;
        private DevExpress.XtraEditors.LabelControl LcCarrierType;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SpinEdit SpComponentThickness;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SpinEdit SpRotaryPosition;
        private DevExpress.XtraEditors.SpinEdit SpCarrierThickness;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.ComboBoxEdit CmbNozzleName;
        private DevExpress.XtraEditors.ComboBoxEdit CmbEjectionName;
        private DevExpress.XtraEditors.GroupControl GcCarrierShape;
        private DevExpress.XtraEditors.GroupControl groupControl12;
        private DevExpress.XtraEditors.GroupControl groupControl11;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.CheckEdit CePositionSearch2;
        private DevExpress.XtraEditors.CheckEdit checkEdit3;
        private DevExpress.XtraEditors.GroupControl GcInkDotSearch;
        private DevExpress.XtraEditors.GroupControl groupControl9;
        private DevExpress.XtraEditors.SpinEdit SpMaxNumberOfComponentsWithoutSearch;
        private DevExpress.XtraEditors.GroupControl GcAccuracyWithCamera;
        private DevExpress.XtraEditors.SpinEdit SpAfterNumberOfColumns;
        private DevExpress.XtraEditors.SpinEdit SpAfterNumberOfRows;
        private DevExpress.XtraEditors.GroupControl GcAccuracySearch;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.CheckEdit CeTwoPointAdjust;
        private DevExpress.XtraEditors.GroupControl groupControl19;
        private DevExpress.XtraEditors.GroupControl GcCarrierAdjust;
        private DevExpress.XtraEditors.ZoomTrackBarControl TbMaxNumberOfComponentsWithoutSearch;
        private DevExpress.XtraEditors.GroupControl GcNeedleIntermediateSteps;
        private DevExpress.XtraEditors.ZoomTrackBarControl TbIntermediateSteps;
        private DevExpress.XtraEditors.SpinEdit SpIntermediateSteps;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.ZoomTrackBarControl TbStepDelay;
        private DevExpress.XtraEditors.SpinEdit spinEdit8;
        private DevExpress.XtraEditors.SpinEdit SpStepDelay;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.GroupControl groupControl21;
        private DevExpress.XtraEditors.GroupControl GcRotaryModeCounter;
        private DevExpress.XtraEditors.ZoomTrackBarControl TbRotaryModeCount;
        private DevExpress.XtraEditors.SpinEdit SpRotaryModeCount;
        private DevExpress.XtraEditors.GroupControl GcRotaryModeSequence;
        private DevExpress.XtraEditors.CheckEdit CeBondingHeadAngleFromPositionSearch;
        private DevExpress.XtraEditors.SpinEdit spinEdit10;
        private DevExpress.XtraEditors.LabelControl labelControl15;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.SpinEdit spinEdit13;
        private DevExpress.XtraEditors.SpinEdit spinEdit12;
        private DevExpress.XtraEditors.SpinEdit spinEdit11;
        private DevExpress.XtraEditors.LabelControl labelControl16;
        private DevExpress.XtraEditors.GroupControl groupControl27;
        private DevExpress.XtraEditors.LabelControl labelControl19;
        private DevExpress.XtraEditors.LabelControl labelControl18;
        private DevExpress.XtraEditors.LabelControl labelControl17;
        private System.Windows.Forms.Timer timer;
        private DevExpress.XtraEditors.SpinEdit SpNumberOfInkDot;
        private DevExpress.XtraEditors.LookUpEdit LueCarrierShape;
        private DevExpress.XtraEditors.LookUpEdit LueCarrierGeoSetMode;
        private DevExpress.XtraEditors.LookUpEdit LueCarrierIDReaderMode;
        private DevExpress.XtraEditors.LookUpEdit LueAccuracyMode;
        private DevExpress.XtraEditors.LookUpEdit LueAjustCameraType;
        private DevExpress.XtraEditors.LookUpEdit LueRotationSearchRange;
        private DevExpress.XtraEditors.LookUpEdit LueAccuracySearch;
        private DevExpress.XtraEditors.LookUpEdit LueCarrierAdjustMode;
        private DevExpress.XtraEditors.LookUpEdit LuePositionSearchMode;
        private DevExpress.XtraEditors.LookUpEdit LueAutoRunWithDieMode;
        private DevExpress.XtraEditors.LookUpEdit LueTabletChangeType;
        private DevExpress.XtraEditors.LookUpEdit LueMoveWithWaferTable;
        private DevExpress.XtraEditors.LookUpEdit LueRotaryModeSequence;
        private DevExpress.XtraEditors.LookUpEdit LueRotaryPositionDetermination;
        private DevExpress.XtraEditors.LookUpEdit LueSearchCamera;
        private DevExpress.XtraEditors.CheckEdit CeIsRotationSearch;
        private DevExpress.XtraEditors.CheckEdit CeIsBondWithProfile;
        private DevExpress.XtraEditors.CheckEdit CeIsMapping;
        private DevExpress.XtraEditors.CheckEdit CeIsCarryOutSCT;
        private DevExpress.XtraEditors.CheckEdit CeIsReferenceDie;
        private DevExpress.XtraEditors.CheckEdit CeIsBadChipTable;
        private DevExpress.XtraEditors.CheckEdit CeIsComponentAdjust;
        private DevExpress.XtraEditors.CheckEdit CeIsGraphicalSyn;
        private DevExpress.XtraEditors.CheckEdit CeIsInkDotSearch;
        private DevExpress.XtraEditors.CheckEdit CeIsPositionSearch;
        private DevExpress.XtraEditors.CheckEdit CeIsAfterColumns;
        private DevExpress.XtraEditors.CheckEdit CeIsAfterRows;
        private DevExpress.XtraEditors.CheckEdit CeIsMultiPinEjectProcess;
        private DevExpress.XtraEditors.CheckEdit CeIsSynchronousEjection;
        private DevExpress.XtraEditors.GroupControl GcNeedleMode;
        private DevExpress.XtraEditors.CheckEdit CeIsAfterEachChange;
        private DevExpress.XtraEditors.CheckEdit CeIsDifferentSearches;
        private DevExpress.XtraEditors.CheckEdit CeIsInverseWaferMapProcessing;
        private DevExpress.XtraEditors.CheckEdit CeIsReferenceDieAsAlignmentDie;
        private DevExpress.XtraEditors.CheckEdit CeIsErrorMessageOnBadSearch;
        private DevExpress.XtraEditors.CheckEdit CeIsBad;
        private DevExpress.XtraEditors.CheckEdit CeIsUseReferenceDies;
        private DevExpress.XtraEditors.CheckEdit CeIsDoNotPick;
        private DevExpress.XtraEditors.CheckEdit CeIsFirstDieIntegrityCheck;
        private DevExpress.XtraEditors.CheckEdit CeIs8DieCheck;
        private DevExpress.XtraEditors.CheckEdit CeIsEdgeDieCheck;
        private DevExpress.XtraEditors.CheckEdit CeIsOneSnap;
        private DevExpress.XtraEditors.CheckEdit CeIsWithOutWaferTableMovement;
        private DevExpress.XtraEditors.CheckEdit CeIsNineSearch;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.CheckEdit CeIsComponentDetection;
        private DevExpress.XtraEditors.LabelControl labelControl21;
        private DevExpress.XtraEditors.LabelControl labelControl20;
        private DevExpress.XtraEditors.SpinEdit SpUplookVisionDelay;
        private DevExpress.XtraEditors.SpinEdit SpWeakBlowProportion;
        private DevExpress.XtraEditors.SimpleButton BtnBlow;
        private DevExpress.XtraEditors.CheckEdit CeIsFlip;
        private DevExpress.XtraEditors.SimpleButton BtnESUpOrDown;
        private DevExpress.XtraEditors.GroupControl gCDip;
        private DevExpress.XtraEditors.GroupControl groupControl13;
        private DevExpress.XtraEditors.CheckEdit ChkIsActiveSlowDownBeforeDip;
        private DevExpress.XtraEditors.GroupControl groupControl14;
        private DevExpress.XtraEditors.CheckEdit ChkIsActiveSlowDownAfterDip;
        private DevExpress.XtraEditors.SpinEdit SpSlowTravleSpeedBeforeDip;
        private DevExpress.XtraEditors.LabelControl labelControl25;
        private DevExpress.XtraEditors.LabelControl labelControl22;
        private DevExpress.XtraEditors.LabelControl labelControl23;
        private DevExpress.XtraEditors.SpinEdit SpSlowTravleDistanceBeforeDip;
        private DevExpress.XtraEditors.LabelControl labelControl24;
        private DevExpress.XtraEditors.SpinEdit SpSlowTravleSpeedAfterDip;
        private DevExpress.XtraEditors.LabelControl labelControl26;
        private DevExpress.XtraEditors.LabelControl labelControl27;
        private DevExpress.XtraEditors.LabelControl labelControl28;
        private DevExpress.XtraEditors.SpinEdit SpSlowTravleDistanceAfterDip;
        private DevExpress.XtraEditors.LabelControl labelControl29;
        private DevExpress.XtraEditors.GroupControl gCDipMode;
        private DevExpress.XtraEditors.LookUpEdit LueDipMode;
        private DevExpress.XtraEditors.SimpleButton BtnMeasurementComponentAndCarrierThickness;
        private DevExpress.XtraEditors.LabelControl LcThickness;
        private DevExpress.XtraEditors.LabelControl labelControl32;
        private DevExpress.XtraEditors.SpinEdit SpDipDistance;
        private DevExpress.XtraEditors.LabelControl labelControl33;
        private DevExpress.XtraEditors.LabelControl labelControl30;
        private DevExpress.XtraEditors.SpinEdit SpDipDelay;
        private DevExpress.XtraEditors.LabelControl labelControl31;
        private DevExpress.XtraEditors.CheckEdit CeIsUseCarrierHeightMeasure;
        private DevExpress.XtraEditors.SimpleButton BtnPositionSearchConfigure;
        private DevExpress.XtraEditors.GroupControl gCIPTType;
        private DevExpress.XtraEditors.LookUpEdit LueIPTType;
        private DevExpress.XtraEditors.CheckEdit ChkIsVisionCode;
        private DevExpress.XtraEditors.CheckEdit ChkIsReferencePoint;
    }
}
