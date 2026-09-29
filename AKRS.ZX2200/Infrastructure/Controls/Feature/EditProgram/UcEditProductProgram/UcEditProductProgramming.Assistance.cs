namespace AKRS.ZX2200.Infrastructure.Controls.Feature.EditProgram.UcEditProductProgram
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Controls.Assistant;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
    using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.DispenseSystem.Controls.Assistant;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.FlipTeach;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool;
    using DevExpress.XtraEditors;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using TransportUnit = AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit;

    /// <summary>
    /// 这个类为示教的分布类，
    /// 里面主要包括示教的方法
    /// </summary>
    public partial class UcEditProductProgramming
    {
        /// <summary>
        /// 载台上的载具
        /// </summary>
        public TransportUnit TransportUnit => TUAssistantHelper.JudgeTuExist();

        /// <summary>
        /// 载具定位配置
        /// </summary>
        public ProductConfiguration ProductConfiguration => ProductConfiguration.GetInstance();

        /// <summary>
        /// Tu的配置对象
        /// </summary>
        private TransportUnitConfig TransportUnitConfig => ProductConfiguration.GetInstance().TransportUnitConfig;

        /// <summary>
        /// 当前选中的焊点配置对象
        /// </summary>
        private SingleBondPositionConfig CurrentBondPositionConfig =>
            (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as SingleBondPositionConfig;

        /// <summary>
        /// system2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// Tu定位示教
        /// </summary>
        public void AssistanceTuAdjust()
        {
            if (this.TransportUnit == null)
            {
                return;
            }

            TransportUnitConfig transportUnitConfig = ProductConfiguration.GetInstance().TransportUnitConfig;

            // 完整的情况下直接进行PR定位
            if (transportUnitConfig.SubstrateProcessing
                == SubstrateProcessingEnum.Complete)
            {
                // 拉角度
                FrmAdjustDegree frmAdjustDegree = new FrmAdjustDegree(
                    EntityTypeEnum.Substrate.ToString(),
                    this.TransportUnit.CoordinateSystem);
                if (frmAdjustDegree.ShowDialog() != DialogResult.OK)
                {
                    frmAdjustDegree.Dispose();
                    return;
                }

                // 角度保存
                transportUnitConfig.ElementCoordinate.Degree = frmAdjustDegree.RealDegree;

                frmAdjustDegree.Dispose();

                ProductConfiguration.GetInstance().Save();

                FrmEditPRs frmEditPRsTu = new FrmEditPRs(
                    transportUnitConfig.LocateConfig,
                    EntityTypeEnum.TransportUnit.ToString(),
                    this.TransportUnit.CoordinateSystem);
                if (frmEditPRsTu.ShowDialog() == DialogResult.OK)
                {
                    frmEditPRsTu.Dispose();
                    this.RefreshILstAssistantStepWithTransportUnit(transportUnitConfig);
                    this.Start();
                }

                frmEditPRsTu.Dispose();
            }
            else
            {
                if (ProductConfiguration.GetInstance().TransportUnitConfig.SubstrateProcessing == SubstrateProcessingEnum.Divide)
                {
                    // 拉角度
                    FrmAdjustDegree frmAdjustDegree = new FrmAdjustDegree(
                        EntityTypeEnum.Substrate.ToString(),
                        this.TransportUnit.CoordinateSystem);
                    if (frmAdjustDegree.ShowDialog() != DialogResult.OK)
                    {
                        frmAdjustDegree.Dispose();
                        return;
                    }

                    // 角度保存
                    transportUnitConfig.ElementCoordinate.Degree = this.TransportUnit.CoordinateSystem.Degree;

                    frmAdjustDegree.Dispose();

                    ProductConfiguration.GetInstance().Save();
                    
                    FrmEditPRs frmEditPRsTuLeft = new FrmEditPRs(
                        transportUnitConfig.LocateConfig,
                        EntityTypeEnum.TransportUnit.ToString() + "Left",
                        this.TransportUnit.CoordinateSystem);

                    if (frmEditPRsTuLeft.ShowDialog() != DialogResult.OK)
                    {
                        frmEditPRsTuLeft.Dispose();
                        return;
                    }

                    frmEditPRsTuLeft.Dispose();

                    FrmEditPRs frmEditPRsTuRight = new FrmEditPRs(
                        transportUnitConfig.LocateConfig,
                        EntityTypeEnum.TransportUnit.ToString() + "Right",
                        this.TransportUnit.CoordinateSystem);

                    if (frmEditPRsTuRight.ShowDialog() == DialogResult.OK)
                    {
                        this.RefreshILstAssistantStepWithTransportUnit(transportUnitConfig);
                        this.Start();
                    }

                    frmEditPRsTuRight.Dispose();
                }
            }
        }

        /// <summary>
        /// Sub定位
        /// </summary>
        public void AssistanceSubAdjust()
        {
            if (this.TransportUnit == null)
            {
                return;
            }

            TransportUnitConfig transportUnitConfig = ProductConfiguration.GetInstance().TransportUnitConfig;

            // 选择sub
            FrmModuleSelect frmModuleSelectSub = new FrmModuleSelect(EntityTypeEnum.Substrate);
            frmModuleSelectSub.IsVisionSelf = false;
            if (frmModuleSelectSub.ShowDialog() != DialogResult.OK)
            {
                frmModuleSelectSub.Dispose();
                return;
            }

            frmModuleSelectSub.Dispose();

            if (this.ProductConfiguration.TransportUnitConfig.AssistantAllRotation)
            {
                // 拉角度
                FrmAdjustDegree frmAdjustDegree = new FrmAdjustDegree(
                    EntityTypeEnum.Substrate.ToString(),
                    this.TransportUnit.CoordinateSystem);
                if (frmAdjustDegree.ShowDialog() != DialogResult.OK)
                {
                    frmModuleSelectSub.Dispose();
                    return;
                }

                frmAdjustDegree.Dispose();

                frmModuleSelectSub.CurrentSubstrate.CoordinateSystem.Degree = frmAdjustDegree.RealDegree;
            }
            
            FrmEditPRs frmEditPRsSu = new FrmEditPRs(
                ProductConfiguration.GetInstance().SubstrateConfig.LocateConfig,
                EntityTypeEnum.Substrate.ToString(),
                frmModuleSelectSub.CurrentSubstrate.CoordinateSystem);

            if (frmEditPRsSu.ShowDialog() == DialogResult.OK)
            {
                // 当前的所有基板都默认成当前角度
                foreach (ElementCoordinate generalCoordinate in ProductConfiguration.GetInstance().SubstrateConfig.ElementCoordinates)
                {
                    generalCoordinate.Degree = frmModuleSelectSub.CurrentSubstrate.CoordinateSystem.Degree;
                }

                this.RefreshILstAssistantStepWithTransportUnit(transportUnitConfig);
                this.Start();
            }

            frmEditPRsSu.Dispose();
        }

        /// <summary>
        /// Module定位
        /// </summary>
        public void AssistanceModuleAdjust()
        {
            if (this.TransportUnit == null)
            {
                return;
            }

            TransportUnitConfig transportUnitConfig = ProductConfiguration.GetInstance().TransportUnitConfig;

            // 选择module
            FrmModuleSelect frmModuleSelectModule = new FrmModuleSelect(EntityTypeEnum.Module);
            frmModuleSelectModule.IsVisionSelf = false;
            if (frmModuleSelectModule.ShowDialog() != DialogResult.OK)
            {
                frmModuleSelectModule.Dispose();
                return;
            }

            frmModuleSelectModule.Dispose();

            if (this.ProductConfiguration.TransportUnitConfig.AssistantAllRotation)
            {
                // 拉角度
                FrmAdjustDegree frmAdjustDegree = new FrmAdjustDegree(
                    EntityTypeEnum.Module.ToString(),
                    frmModuleSelectModule.CurrentSubstrate.CoordinateSystem);
                if (frmAdjustDegree.ShowDialog() != DialogResult.OK)
                {
                    frmAdjustDegree.Dispose();
                    return;
                }

                frmModuleSelectModule.CurrentModule.CoordinateSystem.Degree = frmAdjustDegree.RealDegree;

                frmAdjustDegree.Dispose();
            }

            FrmEditPRs frmEditPRsSu = new FrmEditPRs(
                ProductConfiguration.GetInstance().ModuleConfig.LocateConfig,
                EntityTypeEnum.Module.ToString(),
                frmModuleSelectModule.CurrentModule.CoordinateSystem);

            if (frmEditPRsSu.ShowDialog() == DialogResult.OK)
            {
                frmEditPRsSu.Dispose();
                foreach (ElementCoordinate elementCoordinate in this.ProductConfiguration.ModuleConfig.ElementCoordinates)
                {
                    elementCoordinate.Degree = frmModuleSelectModule.CurrentModule.CoordinateSystem.Degree;
                }

                this.RefreshILstAssistantStepWithTransportUnit(transportUnitConfig);
                this.Start();
            }

            frmEditPRsSu.Dispose();
        }

        /// <summary>
        /// 焊点定位配置
        /// </summary>
        public void AssistanceBondPositionAdjust()
        {
            if (this.TransportUnit == null)
            {
                return;
            }

            SingleBondPositionConfig singleBondPositionConfig =
                (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as SingleBondPositionConfig;

            if (singleBondPositionConfig == null)
            {
                return;
            }

            // 选择module
            FrmModuleSelect frmModuleSelectModule = new FrmModuleSelect(
                this.CurrentBondPositionConfig,
                TeachFormEnum.BondPositionAdjust);
            if (frmModuleSelectModule.ShowDialog() != DialogResult.OK)
            {
                frmModuleSelectModule.Dispose();
                return;
            }

            frmModuleSelectModule.Dispose();

            FrmEditPRs frmEditPRsSu = new FrmEditPRs(
                this.CurrentBondPositionConfig.LocateConfig,
                this.CurrentBondPositionConfig.Name,
                frmModuleSelectModule.CurrentBondPosition.CoordinateSystem);

            if (frmEditPRsSu.ShowDialog() == DialogResult.OK)
            {
                frmEditPRsSu.Dispose();
                singleBondPositionConfig.BondPositionAdjust.State = AssistantStateEnum.Able;
                ProductConfiguration.GetInstance().Save();

                this.RefreshILstAssistantStepWithBondPosition(singleBondPositionConfig);
                this.RefreshNode();
                this.UnFocusedWithILstAssistantStep();
            }

            frmEditPRsSu.Dispose();
        }

        /// <summary>
        /// TuID识别示教
        /// </summary>
        public void AssistanceTuIdentity()
        {
            if (this.TransportUnit == null)
            {
                return;
            }

            if (this.TransportUnit.TransportUnitConfig.IdentityConfig.Identification == IdentificationEnum.Off)
            {
                AKRSXtraMessageBox.Show("请先开启框架ID识别");
                return;
            }

            FrmAssistanceIdentity frmEditPRsTu = new FrmAssistanceIdentity(
                    this.TransportUnit.TransportUnitConfig.IdentityConfig,
                    EntityTypeEnum.TransportUnit.ToString(),
                    this.TransportUnit.CoordinateSystem);
            if (frmEditPRsTu.ShowDialog() == DialogResult.OK)
            {
                frmEditPRsTu.Dispose();
                this.RefreshILstAssistantStepWithTransportUnit(this.TransportUnit.TransportUnitConfig);
                this.Start();
            }

            frmEditPRsTu.Dispose();
        }

        /// <summary>
        /// Sub识别示教
        /// </summary>
        public void AssistanceSubIdentity()
        {
            if (this.TransportUnit == null)
            {
                return;
            }

            if (ProductConfiguration.GetInstance().SubstrateConfig.IdentityConfig.Identification == IdentificationEnum.Off)
            {
                AKRSXtraMessageBox.Show("请先开启基板ID识别");
                return;
            }

            TransportUnitConfig transportUnitConfig = ProductConfiguration.GetInstance().TransportUnitConfig;

            // 选择sub
            FrmModuleSelect frmModuleSelectSub = new FrmModuleSelect(EntityTypeEnum.Substrate);
            if (frmModuleSelectSub.ShowDialog() != DialogResult.OK)
            {
                frmModuleSelectSub.Dispose();
                return;
            }

            frmModuleSelectSub.Dispose();

            FrmAssistanceIdentity frmEditPRsSu = new FrmAssistanceIdentity(
                ProductConfiguration.GetInstance().SubstrateConfig.IdentityConfig,
                EntityTypeEnum.Substrate.ToString(),
                frmModuleSelectSub.CurrentSubstrate.CoordinateSystem);

            if (frmEditPRsSu.ShowDialog() == DialogResult.OK)
            {
                this.RefreshILstAssistantStepWithTransportUnit(transportUnitConfig);
                this.Start();
            }

            frmEditPRsSu.Dispose();
        }

        /// <summary>
        /// Module识别示教
        /// </summary>
        public void AssistanceModuleIdentity()
        {
            if (this.TransportUnit == null)
            {
                return;
            }

            if (ProductConfiguration.GetInstance().ModuleConfig.IdentityConfig.Identification == IdentificationEnum.Off)
            {
                AKRSXtraMessageBox.Show("请先开启基岛ID识别");
                return;
            }

            TransportUnitConfig transportUnitConfig = ProductConfiguration.GetInstance().TransportUnitConfig;

            // 选择module
            FrmModuleSelect frmModuleSelectModule = new FrmModuleSelect(EntityTypeEnum.Module);
            if (frmModuleSelectModule.ShowDialog() != DialogResult.OK)
            {
                frmModuleSelectModule.Dispose();
                return;
            }

            frmModuleSelectModule.Dispose();

            FrmAssistanceIdentity frmEditPRsSu = new FrmAssistanceIdentity(
                ProductConfiguration.GetInstance().ModuleConfig.IdentityConfig,
                EntityTypeEnum.Module.ToString(),
                frmModuleSelectModule.CurrentModule.CoordinateSystem);

            if (frmEditPRsSu.ShowDialog() == DialogResult.OK)
            {
                frmEditPRsSu.Dispose();
               
                this.RefreshILstAssistantStepWithTransportUnit(transportUnitConfig);
                this.Start();
            }

            frmEditPRsSu.Dispose();
        }

        /// <summary>
        /// 焊点识别示教
        /// </summary>
        public void AssistanceBondPositionIdentity()
        {
            if (this.TransportUnit == null)
            {
                return;
            }

            SingleBondPositionConfig singleBondPositionConfig =
                (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as SingleBondPositionConfig;

            if (singleBondPositionConfig.IdentityConfig.Identification == IdentificationEnum.Off)
            {
                AKRSXtraMessageBox.Show("请先开启焊点ID识别");
                return;
            }

            if (singleBondPositionConfig == null)
            {
                return;
            }

            // 选择module
            FrmModuleSelect frmModuleSelectModule = new FrmModuleSelect(
                this.CurrentBondPositionConfig,
                TeachFormEnum.BondPositionAdjust);
            if (frmModuleSelectModule.ShowDialog() != DialogResult.OK)
            {
                frmModuleSelectModule.Dispose();
                return;
            }

            frmModuleSelectModule.Dispose();

            FrmAssistanceIdentity frmEditPRsSu = new FrmAssistanceIdentity(
                this.CurrentBondPositionConfig.IdentityConfig,
                this.CurrentBondPositionConfig.Name,
                frmModuleSelectModule.CurrentBondPosition.CoordinateSystem);

            if (frmEditPRsSu.ShowDialog() == DialogResult.OK)
            {
                frmEditPRsSu.Dispose();
                singleBondPositionConfig.BondPositionAdjust.State = AssistantStateEnum.Able;
                ProductConfiguration.GetInstance().Save();

                this.RefreshILstAssistantStepWithBondPosition(singleBondPositionConfig);
                this.RefreshNode();
                this.UnFocusedWithILstAssistantStep();
            }

            frmEditPRsSu.Dispose();
        }

        /// <summary>
        /// 焊点定位配置
        /// </summary>
        public void AssistanceBondPositionMeasureHeight()
        {
            if (this.TransportUnit == null)
            {
                return;
            }

            SingleBondPositionConfig singleBondPositionConfig =
                (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as SingleBondPositionConfig;
            if (singleBondPositionConfig == null)
            {
                return;
            }

            // 判断是否需要测高
            if (!singleBondPositionConfig.MeasureHeightInSystem1 && !singleBondPositionConfig.MeasureHeightInSystem2)
            {
                AKRSXtraMessageBox.Show("请先开启测高");
                return;
            }

            if (!this.system2Controller.ChangeTouchDownAssistance())
            {
                return;
            }

            FrmModuleSelect frmBondPositionSelect = new FrmModuleSelect(
                singleBondPositionConfig,
                TeachFormEnum.BondPositionMeasureHeight);
            if (frmBondPositionSelect.ShowDialog() != DialogResult.OK)
            {
                frmBondPositionSelect.Dispose();
                return;
            }

            FrmMeasureHeight frmMeasureHeight = new FrmMeasureHeight(
                singleBondPositionConfig.Name,
                singleBondPositionConfig,
                frmBondPositionSelect.CurrentBondPosition.CoordinateSystem);

            if (frmMeasureHeight.ShowDialog() == DialogResult.OK)
            {
                frmMeasureHeight.Dispose();
                frmBondPositionSelect.Dispose();
                this.RefreshILstAssistantStepWithBondPosition(singleBondPositionConfig);
                this.RefreshNode();
                this.UnFocusedWithILstAssistantStep();
            }

            frmMeasureHeight.Dispose();
            frmBondPositionSelect.Dispose();
        }

        /// <summary>
        /// Tu测高
        /// </summary>
        public void AssistanceMeasureHeightTu()
        {
            if (this.TransportUnit == null)
            {
                return;
            }

            TransportUnitConfig transportUnitConfig = ProductConfiguration.GetInstance().TransportUnitConfig;

            if (!this.system2Controller.ChangeTouchDownAssistance())
            {
                return;
            }

            FrmMeasureHeight frmMeasureHeight = new FrmMeasureHeight(
                EntityTypeEnum.TransportUnit.ToString(),
                this.TransportUnit.TransportUnitConfig,
                this.TransportUnit.CoordinateSystem);

            if (frmMeasureHeight.ShowDialog() == DialogResult.OK)
            {
                frmMeasureHeight.Dispose();
                this.RefreshILstAssistantStepWithTransportUnit(transportUnitConfig);
                this.Start();
            }

            frmMeasureHeight.Dispose();
        }

        /// <summary>
        /// Sub测高
        /// </summary>
        public void AssistanceMeasureHeightSu()
        {
            if (this.TransportUnit == null)
            {
                return;
            }

            if (!this.system2Controller.ChangeTouchDownAssistance())
            {
                return;
            }

            FrmModuleSelect frmModuleSelect = new FrmModuleSelect(EntityTypeEnum.Substrate);
            if (frmModuleSelect.ShowDialog() != DialogResult.OK)
            {
                frmModuleSelect.Dispose();
                return;
            }

            FrmMeasureHeight frmMeasureHeight = new FrmMeasureHeight(
                EntityTypeEnum.Substrate.ToString(),
                ProductConfiguration.GetInstance().SubstrateConfig,
                frmModuleSelect.CurrentSubstrate.CoordinateSystem);
            if (frmMeasureHeight.ShowDialog() == DialogResult.OK)
            {
                frmMeasureHeight.Dispose();
                frmModuleSelect.Dispose();
                this.RefreshILstAssistantStepWithTransportUnit(this.TransportUnitConfig);
                this.Start();
            }

            frmMeasureHeight.Dispose();
            frmModuleSelect.Dispose();
        }

        /// <summary>
        /// Module测高
        /// </summary>
        public void AssistanceMeasureHeightModule()
        {
            // 判断是否需要测高
            if (!ProductConfiguration.GetInstance().ModuleConfig.MeasureHeightInSystem1 && !ProductConfiguration.GetInstance().ModuleConfig.MeasureHeightInSystem2)
            {
                AKRSXtraMessageBox.Show("请先开启测高");
                return;
            }

            if (!this.system2Controller.ChangeTouchDownAssistance())
            {
                return;
            }

            FrmModuleSelect frmModuleSelect = new FrmModuleSelect(EntityTypeEnum.Module);
            if (frmModuleSelect.ShowDialog() != DialogResult.OK)
            {
                frmModuleSelect.Dispose();
                return;
            }

            FrmMeasureHeight frmMeasureHeight = new FrmMeasureHeight(
                EntityTypeEnum.Module.ToString(),
                ProductConfiguration.GetInstance().ModuleConfig,
                frmModuleSelect.CurrentModule.CoordinateSystem);

            if (frmMeasureHeight.ShowDialog() == DialogResult.OK)
            {
                frmMeasureHeight.Dispose();
                frmModuleSelect.Dispose();
                this.RefreshILstAssistantStepWithTransportUnit(this.TransportUnitConfig);
                this.Start();
            }

            frmMeasureHeight.Dispose();
            frmModuleSelect.Dispose();
        }
        
        /// <summary>
        /// 焊点示教
        /// </summary>
        public void AssistanceTeachBondingPosition()
        {
            if (System2Domain.GetInstance().TransportUnit == null)
            {
                AKRSXtraMessageBox.Show(
                    "系统2没有找到基板",
                    "警告",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            SingleBondPositionConfig singleBondPositionConfig =
                       (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as SingleBondPositionConfig;

            if (singleBondPositionConfig == null)
            {
                return;
            }

            if (singleBondPositionConfig.RelativeBonding == RelativeBondingEnum.Off)
            {
                FrmModuleSelect frmModuleSelect = new FrmModuleSelect(EntityTypeEnum.Module);
                frmModuleSelect.SetModuleIndex(
                    singleBondPositionConfig.TeachSubstrateNum,
                    singleBondPositionConfig.TeachModuleNum);

                frmModuleSelect.MoveToPos();

                DialogResult dialogResult = frmModuleSelect.ShowDialog();
                frmModuleSelect.IsVisionSelf = false;
                if (dialogResult == DialogResult.OK)
                {
                    singleBondPositionConfig.ElementCoordinate.Degree = 0;
                    
                    if (singleBondPositionConfig.AssistanceAngle)
                    {
                        // 拉角度
                        FrmAdjustDegree frmAdjustDegree = new FrmAdjustDegree(
                            singleBondPositionConfig.Name,
                            frmModuleSelect.CurrentModule.CoordinateSystem);

                        DialogResult dialogResult2 = frmAdjustDegree.ShowDialog();

                        if (dialogResult2 != DialogResult.OK)
                        {
                            frmAdjustDegree.Dispose();  
                            return;
                        }

                        singleBondPositionConfig.ElementCoordinate.Degree = frmAdjustDegree.RealDegree;
                        frmAdjustDegree.Dispose();
                    }
                    
                    singleBondPositionConfig.TeachSubstrateNum = frmModuleSelect.CurrentSubstrate.Index;
                    singleBondPositionConfig.TeachModuleNum = frmModuleSelect.CurrentModule.Index;
                    FrmTeachBondingPosition frmTeachBondingPosition = new FrmTeachBondingPosition(
                        singleBondPositionConfig,
                        frmModuleSelect.CurrentModule.CoordinateSystem);
                    frmTeachBondingPosition.ShowDialog();

                    this.RefreshILstAssistantStepWithBondPosition(singleBondPositionConfig);
                    if (frmTeachBondingPosition.DialogResult == DialogResult.OK)
                    {
                        this.Start();
                    }

                    frmTeachBondingPosition.Dispose();
                }
            }
            else
            {
                FrmModuleSelect frmModuleSelect = new FrmModuleSelect(singleBondPositionConfig, TeachFormEnum.TeachBondingPosition);

                frmModuleSelect.SetModuleIndex(
                    singleBondPositionConfig.TeachSubstrateNum,
                    singleBondPositionConfig.TeachModuleNum);

                frmModuleSelect.MoveToPos();

                frmModuleSelect.ShowDialog();

                FrmRelativeBondTeach frmRelativeBondTeach = new FrmRelativeBondTeach(
                    singleBondPositionConfig,
                    frmModuleSelect.CurrentModule.CoordinateSystem);
                frmRelativeBondTeach.ShowDialog();

                this.RefreshILstAssistantStepWithBondPosition(singleBondPositionConfig);
                if (frmRelativeBondTeach.DialogResult == DialogResult.OK)
                {
                    this.Start();
                }
                frmRelativeBondTeach.Dispose();
                frmModuleSelect.Dispose();
            }
        }

        /// <summary>
        /// 移动到焊点
        /// </summary>
        public void AssistanceMoveToBondingPosition()
        {
            SingleBondPositionConfig singleBondPositionConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as SingleBondPositionConfig;
            FrmModuleSelect frmModuleSelect2 = new FrmModuleSelect(singleBondPositionConfig, TeachFormEnum.MoveToBondingPosition);


            if (singleBondPositionConfig == null)
            {
                return;
            }

            frmModuleSelect2.SetModuleIndex(
                singleBondPositionConfig.TeachSubstrateNum,
                singleBondPositionConfig.TeachModuleNum);

            frmModuleSelect2.MoveToPos();

            frmModuleSelect2.ShowDialog();

            if (frmModuleSelect2.DialogResult == DialogResult.OK)
            {
                frmModuleSelect2.Save();

                this.RefreshILstAssistantStepWithBondPosition(singleBondPositionConfig);
                this.Start();
            }

            frmModuleSelect2.Dispose();
        }

        /// <summary>
        /// 焊后检测
        /// </summary>
        public void AssistancePostBond()
        {
            PostBondInspection postBondInspection = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as PostBondInspection;

            if (postBondInspection == null)
            {
                return;
            }

            // 示教BLT
            if (postBondInspection.ApplicationSystem == BondSystem.Models.Enums.PostBondApplicationSystemEnum.BLTMeasure)
            {
                this.AssistanceBLTCheck();
                return;
            }

            // 预点胶检测
            if (postBondInspection.IsPreDispenseCheck)
            {
                FrmPreDispenseSelect frmPreDispenseSelect = new FrmPreDispenseSelect();
                DialogResult dialog = frmPreDispenseSelect.ShowDialog();
                if (dialog == DialogResult.Cancel)
                {
                    return;
                }

                FrmAssistantDefect frmAssistant = new FrmAssistantDefect(postBondInspection, frmPreDispenseSelect.Point3D);
                if (frmAssistant.ShowDialog() == DialogResult.OK)
                {
                    postBondInspection.PostBond.State = AssistantStateEnum.Able;
                    this.RefreshILstAssistantStepWithPostBond(postBondInspection);
                    this.RefreshNode();
                    this.UnFocusedWithILstAssistantStep();
                }

                frmAssistant.Dispose();
            }
            else
            {
                // 判断载台上有没有产品
                TransportUnit transportUnit = TUAssistantHelper.JudgeTuExist();
                if (transportUnit == null)
                {
                    return;
                }

                FrmModuleSelect frmModuleSelect3 = new FrmModuleSelect(EntityTypeEnum.BondPosition);
                DialogResult dialog = frmModuleSelect3.ShowDialog();

                if (dialog == DialogResult.Cancel)
                {
                    frmModuleSelect3.Dispose();
                    return;
                }

                // 当前焊点
                BondPosition bondPosition = frmModuleSelect3.CurrentBondPosition;

                FrmAssistantDefect frmAssistant = new FrmAssistantDefect(postBondInspection, bondPosition);

                if (frmAssistant.ShowDialog() == DialogResult.OK)
                {
                    postBondInspection.PostBond.State = AssistantStateEnum.Able;
                    this.RefreshILstAssistantStepWithPostBond(postBondInspection);
                    this.RefreshNode();
                    this.UnFocusedWithILstAssistantStep();
                }

                frmAssistant.Dispose();
            }
        }

        /// <summary>
        /// 背崩检测
        /// </summary>
        public void AssistanceBacksideCrackDetection()
        {
            PostBondInspection postBondInspection = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as PostBondInspection;

            if (postBondInspection == null)
            {
                return;
            }

            FrmBackSideCrackDetectionTeach frmAssistant = new FrmBackSideCrackDetectionTeach(postBondInspection);

            if (frmAssistant.ShowDialog() == DialogResult.OK)
            {
                postBondInspection.BacksideCrackDetection.State = AssistantStateEnum.Able;
                this.RefreshILstAssistantStepWithPostBond(postBondInspection);
                this.RefreshNode();
                this.UnFocusedWithILstAssistantStep();
            }

            frmAssistant.Dispose();
        }

        /// <summary>
        /// 翻转工具示教
        /// </summary>
        public void AssistanceFlipTool()
        {
            FlipTool flipTool = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as FlipTool;

            FrmFlipToolTeach frmFlipToolTeach = new FrmFlipToolTeach(flipTool);
            if (frmFlipToolTeach.IsShowDialog())
            {
                frmFlipToolTeach.ShowDialog();

                this.RefreshILstAssistantStepWithFlipTool(flipTool);
                if (frmFlipToolTeach.DialogResult == DialogResult.OK)
                {
                    this.Start();
                }

                frmFlipToolTeach.Dispose();
            }

            // 刷新Config 界面
            (this.selectedControl as UcConfiguration2)?.RefreshControl();

            // 刷新子节点
            this.RefreshNode();
        }

        /// <summary>
        /// 吸嘴测高
        /// </summary>
        public void AssistanceToolHeight()
        {
            NozzleShelfSlot nozzleShelfSlot = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as NozzleShelfSlot;

            Nozzle nozzle = (Nozzle)NozzleRepository.GetInstance().Find(nozzleShelfSlot.NozzleName);

            if (!this.system2Controller.ChangeNozzleAssistance(nozzleShelfSlot.NozzleName))
            {
               return;
            }

            if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                AKRSXtraMessageBox.Show("请拿走BMC矫正台上的预点胶板");
            }
            
            FrmToolHeightTeach frmToolHeightTeach = new FrmToolHeightTeach(nozzle);
            frmToolHeightTeach.ShowDialog();

            this.RefreshILstAssistantStepWithNozzle(nozzleShelfSlot);
            if (frmToolHeightTeach.DialogResult == DialogResult.OK)
            {
                this.Start();
            }

            if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                AKRSXtraMessageBox.Show("请放回BMC矫正台上的预点胶板");
            }

            frmToolHeightTeach.Dispose();

            // 还没示教完就不归还吸嘴
            if (nozzle.IsAssistantSucceed == false && frmToolHeightTeach.DialogResult == DialogResult.OK)
            {
                return;
            }

            this.system2Controller.PutbackNozzleAssitance();
        }

        /// <summary>
        /// 吸嘴标定
        /// </summary>
        public void AssistanceToolAlignment()
        {
            NozzleShelfSlot nozzleShelfSlot = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as NozzleShelfSlot;

            Nozzle nozzle = (Nozzle)NozzleRepository.GetInstance().Find(nozzleShelfSlot.NozzleName);

            if (!this.system2Controller.ChangeNozzleAssistance(nozzleShelfSlot.NozzleName))
            {
                return;
            }

            FrmToolAlignmentTeach frmToolAlignmentTeach = new FrmToolAlignmentTeach(nozzle);
            frmToolAlignmentTeach.ShowDialog();

            this.RefreshILstAssistantStepWithNozzle(nozzleShelfSlot);
            if (frmToolAlignmentTeach.DialogResult == DialogResult.OK)
            {
                this.Start();
            }

            frmToolAlignmentTeach.Dispose();

            // 还没示教完就不归还吸嘴
            if (nozzle.IsAssistantSucceed == false && frmToolAlignmentTeach.DialogResult == DialogResult.OK) 
            {
               return;
            }

            //this.system2Controller.PutbackNozzleAssitance();
        }

        /// <summary>
        /// 吸嘴尺寸
        /// </summary>
        public void AssistanceToolGeometry()
        {
            NozzleShelfSlot nozzleShelfSlot = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as NozzleShelfSlot;

            Nozzle nozzle = (Nozzle)NozzleRepository.GetInstance().Find(nozzleShelfSlot.NozzleName);

            if (!this.system2Controller.ChangeNozzleAssistance(nozzleShelfSlot.NozzleName))
            {
                return;
            }

            FrmToolGeometryTeach frmToolGeometryTeach = new FrmToolGeometryTeach(nozzle);
            frmToolGeometryTeach.ShowDialog();

            this.RefreshILstAssistantStepWithNozzle(nozzleShelfSlot);
            if (frmToolGeometryTeach.DialogResult == DialogResult.OK)
            {
                this.RefreshNode();
                this.UnFocusedWithILstAssistantStep();
            }

            frmToolGeometryTeach.Dispose();

            this.system2Controller.PutbackNozzleAssitance();
        }

        /// <summary>
        /// BLT检测
        /// </summary>
        public void AssistanceBLTCheck()
        {
            PostBondInspection postBondInspection = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as PostBondInspection;

            if (postBondInspection == null)
            {
                return;
            }

            // 判断载台上有没有产品
            TransportUnit transportUnit = TUAssistantHelper.JudgeTuExist();
            if (transportUnit == null)
            {
                return;
            }

            FrmModuleSelect frmModuleSelect = new FrmModuleSelect(EntityTypeEnum.BondPosition);
            DialogResult dialog = frmModuleSelect.ShowDialog();

            if (dialog == DialogResult.Cancel)
            {
                frmModuleSelect.Dispose();
                return;
            }

            // 当前焊点
            BondPosition bondPosition = frmModuleSelect.CurrentBondPosition;

            FrmAssistantBLT frmAssistant1 = new FrmAssistantBLT("芯片周围");

            List<double> height1 = new List<double>();

            if (frmAssistant1.ShowDialog() == DialogResult.OK)
            {
                // 测高
                height1 = this.system2Controller.LaserMeasureHeight(frmAssistant1.PosList);
            }
            else
            {
                return;
            }

            System2Domain.GetInstance().S2DispenseController.CloseDispenseHeightMeasurementCylinder();

            System2Domain.GetInstance().BondModuleController.CameraMoveToG0Pos(bondPosition.CoordinateSystem.SelfPosToG0(new Galaxy2.Infrastructure.CommonModel.AKRSPoint3D()));

            FrmAssistantBLT frmAssistant2 = new FrmAssistantBLT("芯片表面");

            System2Domain.GetInstance().SaveBLTHardwareParameter(postBondInspection);

            if (frmAssistant2.ShowDialog() == DialogResult.OK)
            {
                // 测高
                List<double> height2 = this.system2Controller.LaserMeasureHeight(frmAssistant2.PosList);

                // 保存
                postBondInspection.BLTStandard = height2.Average() - height1.Average();
                postBondInspection.ComponentAroundMeasureHeightPosList = frmAssistant1.PosList
                    .Select(it => bondPosition.CoordinateSystem.G0PosToSelf(it)).ToList();

                postBondInspection.ComponentSurfaceMeasureHeightPosList = frmAssistant2.PosList
                    .Select(it => bondPosition.CoordinateSystem.G0PosToSelf(it)).ToList();

                postBondInspection.PostBond.State = AssistantStateEnum.Able;
            }
            else
            {
                return;
            }

            frmAssistant1?.Dispose();
            frmAssistant2?.Dispose();

            PostBondInspectionRepository.GetInstance().Save();
            BondProgram.GetInstance().Save();   
        }
    }
}
