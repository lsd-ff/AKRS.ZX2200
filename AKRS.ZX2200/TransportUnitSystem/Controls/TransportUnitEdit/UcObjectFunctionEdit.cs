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

namespace AKRS.ZX2200.TransportUnitSystem.Controls.TransportUnitEdit
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Controls.Assistant;
    using AKRS.ZX2200.BondSystem.Controls.Setting.PostBond;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.DispenseSystem.Controls.Setting.EpoxyApplication;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Product;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Tool;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using DevExpress.CodeParser;
    using DevExpress.XtraLayout.Customization;
    using System.IO;
    using static AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool.FrmMultipleHeightMeasurementType;

    /// <summary>
    /// 编辑功能
    /// </summary>
    public partial class UcObjectFunctionEdit : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 功能配置
        /// </summary>
        private OppositeSexConfig baseConfig;

        /// <summary>
        /// 配置
        /// </summary>
        private OppositeSex oppositeSex;

        /// <summary>
        /// 是否在加载
        /// </summary>
        private bool isLoad = false;

        /// <summary>
        /// 刷新
        /// </summary>
        private readonly Action refreshAction;

        /// <summary>
        /// 返回执行
        /// </summary>
        private readonly Action backAction;

        /// <summary>
        /// 功能编辑
        /// </summary>
        /// <param name="refreshAction">刷新</param>
        public UcObjectFunctionEdit(Action refreshAction,Action BackAction)
        {
            this.InitializeComponent();
            this.refreshAction = refreshAction;
            this.backAction = BackAction;
        }

        /// <summary>
        /// 配置对象
        /// </summary>
        /// <param name="oppositeSexConfig">对象</param>
        public void Init(OppositeSex oppositeSexConfig)
        {
            this.baseConfig = oppositeSexConfig.Config;
            this.oppositeSex = oppositeSexConfig;
            this.isLoad = true;
            this.ReLoad();
            this.isLoad = false;
        }

        /// <summary>
        /// 重新加载
        /// </summary>
        private void ReLoad()
        {
            if (this.oppositeSex == null)
            {
                this.Enabled = false;
                return;
            }

            this.Enabled = true;
            this.TxName.Text = this.oppositeSex.Name;
            this.SpOffsetX.EditValue = this.oppositeSex.ElementCoordinate.Point.X;
            this.SpOffsetY.EditValue = this.oppositeSex.ElementCoordinate.Point.Y;
            this.SpOffsetZ.EditValue = this.oppositeSex.ElementCoordinate.Point.Z;
            this.SpOffsetAngle.EditValue = this.oppositeSex.ElementCoordinate.Degree * 180.0 / Math.PI;
            this.SpSizeX.EditValue = this.oppositeSex.SizeX;
            this.SpSizeY.EditValue = this.oppositeSex.SizeY;

            foreach (var item in this.groupControl1.Controls)
            {
                if (item is SpinEdit)
                {
                    SpinEdit spin = (SpinEdit)item;
                    spin.Enabled = false;
                }
            }

            this.BtEdit.Text = "编辑";

            this.LueAdjustType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<AdjustTypeEnum>();
            this.LueAdjustType.EditValue = this.oppositeSex.Config.LocateConfig.AdjustType;
            bool isBondPosition = this.oppositeSex.MatterTypeEnum == EntityTypeEnum.BondPosition;
            this.groupControl8.Enabled = isBondPosition;
            this.groupControl9.Enabled = isBondPosition;
            this.groupControl10.Enabled = isBondPosition;
            this.groupControl6.Enabled = isBondPosition;
            this.groupControl2.Text = $"配置:{this.baseConfig?.Name}";

            #region 注入数据

            this.CmbDispense.Properties.Items.Clear();
            foreach (string epoxyApplication in System1Domain.GetInstance().System1Program.EpoxyApplicationProgram.EpoxyApplicationNames)
            {
                this.CmbDispense.Properties.Items.Add(epoxyApplication);
            }

            this.CmbDispense.SelectedItem = null;

            this.CmbBond.Properties.Items.Clear();
            foreach (BaseCarrierConfig carrier in WaferSystemProgram.GetInstance().GetCarriers())
            {
                this.CmbBond.Properties.Items.Add(carrier.Name);
            }

            this.CmbBond.SelectedItem = null;

            this.CmbEpoxyCheck.Properties.Items.Clear();
            this.CmbAfterBond.Properties.Items.Clear();
            foreach (PostBondInspection postBondInspection in BondProgram.GetInstance().PostBondProgram.PostBondInspections)
            {
                if (postBondInspection.ApplicationSystem == PostBondApplicationSystemEnum.EpoxyCheck)
                {
                    this.CmbEpoxyCheck.Properties.Items.Add(postBondInspection.Name);
                }
                else
                {
                    this.CmbAfterBond.Properties.Items.Add(postBondInspection.Name);
                }
            }

            this.CmbEpoxyCheck.SelectedItem = null;
            this.CmbAfterBond.SelectedItem = null;

            #endregion

            #region 选择数据

            this.CmbDispense.SelectedItem = this.baseConfig.DispenseName;
            this.CmbEpoxyCheck.SelectedItem = this.baseConfig.EpoxyCheckName;
            this.CmbBond.SelectedItem = this.baseConfig.BondComponentName;
            this.CmbAfterBond.SelectedItem = this.baseConfig.AfterBondName;

            this.ChkDispenseOpen.Checked = this.baseConfig.Dispense;
            this.ChkEpoxyCheckOpen.Checked = this.baseConfig.EpoxyCheck;
            this.ChkBond.Checked = this.baseConfig.BondComponent;
            this.ChkAfterBondOpen.Checked = this.baseConfig.AfterBond;

            this.BtAssistantVision.Image = this.baseConfig.IsPrAssistant
                                               ? this.imageCollection1.Images[0]
                                               : this.imageCollection1.Images[1];
            this.BtAssistantMeasureHeight.Image = this.baseConfig.IsMeasureHeightAssistant
                                                      ? this.imageCollection1.Images[0]
                                                      : this.imageCollection1.Images[1];
            this.BtAssistantEpoxyCheck.Image = this.baseConfig.IsEpoxyCheckAssistant
                                                   ? this.imageCollection1.Images[0]
                                                   : this.imageCollection1.Images[1];
            this.BtAssistanAfterBond.Image = this.baseConfig.IsAfterBondAssistant
                                                 ? this.imageCollection1.Images[0]
                                                 : this.imageCollection1.Images[1];

            this.ChkMeasureHeightSystem1.Checked = this.baseConfig.MeasureHeightInSystem1;
            this.ChkMeasureHeightSystem2.Checked = this.baseConfig.MeasureHeightInSystem2;

            this.BtAssistantSizeAndCenter.Image = this.oppositeSex.AssistantPos
                                                     ? this.imageCollection1.Images[0]
                                                     : this.imageCollection1.Images[1];

            #endregion
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtEdit_Click(object sender, EventArgs e)
        {
            if (this.BtEdit.Text == "编辑")
            {
                this.TxName.Enabled = true;
                foreach (var item in this.groupControl1.Controls)
                {
                    if (item is SpinEdit)
                    {
                        SpinEdit spin = (SpinEdit)item;
                        spin.Enabled = true;
                    }

                    if (this.oppositeSex.MatterTypeEnum == EntityTypeEnum.TransportUnit)
                    {
                        this.TxName.Enabled = false;
                    }
                }

                this.BtEdit.Text = "保存";
            }
            else
            {
                this.SaveOppositeSexConfig();
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        private void SaveOppositeSexConfig()
        {
            foreach (char rInvalidChar in Path.GetInvalidFileNameChars())
            {
                if (this.TxName.Text.Contains(rInvalidChar.ToString()))
                {
                    AKRSXtraMessageBox.Show($"焊点名存在非法字符: {rInvalidChar.ToString()}！", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (this.oppositeSex.Name != this.TxName.Text)
            {
                this.Enabled = false;
                this.label1.Visible = true;
                List<OppositeSex> oppositeSex = ProductDomain.GetInstance().ProductConfig.OppositeSexConfiguration
                    .GetOppositeSexConfigs().FindAll(it => it.ConfigName == this.oppositeSex.ConfigName);
                OppositeSexConfig oppositeSexConfig = this.oppositeSex.Config;

                string oldName = oppositeSexConfig.LocateConfig.Name;

                foreach (OppositeSex opposite in oppositeSex)
                {
                    opposite.ConfigName = this.TxName.Text;
                }

                this.oppositeSex.Name = this.TxName.Text;
                this.oppositeSex.ConfigName = this.TxName.Text;
                oppositeSexConfig.Name = this.TxName.Text;
                oppositeSexConfig.LocateConfig.Name = this.TxName.Text;

                List<string> list = new List<string>() { "Mark1", "Mark2", "Mark3", "Mark4" };

                foreach (string mark in list)
                {
                    PREntity pREntity = PREntity.CopyPREntity(
                        MachineConfigContext.GetInstance().RecipeName + oldName + mark,
                        MachineConfigContext.GetInstance().RecipeName + this.TxName.Text + mark);
                    
                    if (pREntity != null)
                    {
                        PREntity.DeletePREntity(MachineConfigContext.GetInstance().RecipeName + oldName + mark);
                        VisionEntityRepository.GetInstance().PRVisionList.RemoveAll(it => it.GetName() == MachineConfigContext.GetInstance().RecipeName + oldName + mark);
                    }
                    

                }

                VisionEntityRepository.GetInstance().Save();

                this.Enabled = true;
                this.label1.Visible = false;
            }
            
            this.oppositeSex.ElementCoordinate = new ElementCoordinate(
                    (double)this.SpOffsetAngle.Value / 180 * Math.PI,
                new AKRSPoint3D(
                    (double)this.SpOffsetX.Value,
                    (double)this.SpOffsetY.Value,
                    (double)this.SpOffsetZ.Value));

            this.oppositeSex.SizeX = (double)this.SpSizeX.Value;
            this.oppositeSex.SizeY = (double)this.SpSizeY.Value;

            this.BtEdit.Text = "编辑";

            this.TxName.Enabled = false;
            foreach (var item in this.groupControl1.Controls)
            {
                if (item is SpinEdit)
                {
                    SpinEdit spin = (SpinEdit)item;
                    spin.Enabled = false;
                }
            }

            this.refreshAction();
        }

        #region 示教

        /// <summary>
        /// 示教视觉
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAssistantVision_Click(object sender, EventArgs e)
        {
            TransportUnit transportUnit = TUAssistantHelper.JudgeTuExist();
            if (transportUnit == null)
            {
                return;
            }

            if (this.oppositeSex.MatterTypeEnum != EntityTypeEnum.TransportUnit)
            {
                OppositeSex parentOppositeSex = ProductDomain.GetInstance().ProductConfig.OppositeSexConfiguration
                 .GetParentOppositeSex(this.oppositeSex);

                BaseMatter ParentBaseMatter = transportUnit.GetBaseMatterByOppositeSex(parentOppositeSex);

                this.VisionBaseMatter(transportUnit, false);

                if (!ParentBaseMatter.BaseInfo.IsVisioned)
                {
                    return;
                }
            }

            BaseMatter baseMatter = transportUnit.GetBaseMatterByOppositeSex(this.oppositeSex);

            AKRSPoint3D point3D = baseMatter.CoordinateSystem.SelfPosToG0(new AKRSPoint3D());

            System2Domain.GetInstance().BondModuleController.CameraMoveToG0Pos(point3D);

            FrmEditPRs frmEditPRsSu = new FrmEditPRs(
                this.oppositeSex.Config.LocateConfig,
                this.oppositeSex.Config.Name,
                baseMatter.CoordinateSystem);

            frmEditPRsSu.ShowDialog();
            if (frmEditPRsSu.DialogResult != DialogResult.OK)
            {
                frmEditPRsSu.Dispose();
                return;
            }

            frmEditPRsSu.Dispose();
            this.baseConfig.IsPrAssistant = true;
            this.BtAssistantVision.Image = this.imageCollection1.Images[0];
            this.refreshAction();
        }

        /// <summary>
        /// 示教测高
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAssistantMeasureHeight_Click(object sender, EventArgs e)
        {
            TransportUnit transportUnit = TUAssistantHelper.JudgeTuExist();
            if (transportUnit == null)
            {
                return;
            }

            BaseMatter baseMatter = transportUnit.GetBaseMatterByOppositeSex(this.oppositeSex);
            System2Domain.GetInstance().System2ObjectVision(baseMatter, false);
            if (!baseMatter.BaseInfo.IsVisioned)
            {
                return;
            }

            AKRSPoint3D point3D = baseMatter.CoordinateSystem.SelfPosToG0(new AKRSPoint3D());

            if (!System2Domain.GetInstance().System2Controller.ChangeTouchDownAssistance())
            {
                return;
            }

            System2Domain.GetInstance().BondModuleController.CameraMoveToG0Pos(point3D);

            FrmMeasureHeight frmMeasureHeight = new FrmMeasureHeight(
                this.oppositeSex.Name,
                this.baseConfig,
                baseMatter.CoordinateSystem);

            frmMeasureHeight.ShowDialog();
            if (frmMeasureHeight.DialogResult != DialogResult.OK)
            {
                frmMeasureHeight.Dispose();
                return;
            }

            frmMeasureHeight.Dispose();
            
            this.baseConfig.IsMeasureHeightAssistant = true;
            this.BtAssistantMeasureHeight.Image = this.imageCollection1.Images[0];
        }

        /// <summary>
        /// 示教胶检
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAssistantEpoxyCheck_Click(object sender, EventArgs e)
        {
            this.baseConfig.EpoxyCheckName = this.CmbEpoxyCheck.SelectedItem?.ToString();
            ProductConfiguration.GetInstance().Save();
            if (string.IsNullOrEmpty(this.baseConfig.EpoxyCheckName))
            {
                AKRSXtraMessageBox.Show("请先选择焊后名称");
                return;
            }

            TransportUnit transportUnit = TUAssistantHelper.JudgeTuExist();
            if (transportUnit == null)
            {
                return;
            }

            this.MoveToSelectCenter();

            BondPosition bondPosition = (BondPosition)transportUnit.GetBaseMatterByOppositeSex(this.oppositeSex);

            PostBondInspection postBondInspection =
                (PostBondInspection)PostBondInspectionRepository.GetInstance().Find(this.baseConfig.EpoxyCheckName);

            FrmAssistantDefect frmAssistant = new FrmAssistantDefect(postBondInspection, bondPosition);

            if (frmAssistant.ShowDialog() == DialogResult.OK)
            {
                postBondInspection.PostBond.State = AssistantStateEnum.Able;
            }

            frmAssistant.Dispose();
            this.BtAssistantEpoxyCheck.Image = this.imageCollection1.Images[0];
            this.refreshAction();
        }

        /// <summary>
        /// 示教焊后
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAssistantAfterBond_Click(object sender, EventArgs e)
        {
            this.baseConfig.AfterBondName = this.CmbAfterBond.SelectedItem?.ToString();
            ProductConfiguration.GetInstance().Save();
            if (string.IsNullOrEmpty(this.baseConfig.AfterBondName))
            {
                AKRSXtraMessageBox.Show("请先选择焊后名称");
                return;
            }

            TransportUnit transportUnit = TUAssistantHelper.JudgeTuExist();
            if (transportUnit == null)
            {
                return;
            }

            this.MoveToSelectCenter();

            BondPosition bondPosition = (BondPosition)transportUnit.GetBaseMatterByOppositeSex(this.oppositeSex);

            PostBondInspection postBondInspection =
                (PostBondInspection)PostBondInspectionRepository.GetInstance().Find(this.baseConfig.AfterBondName);

            FrmAssistantDefect frmAssistant = new FrmAssistantDefect(postBondInspection, bondPosition);

            if (frmAssistant.ShowDialog() == DialogResult.OK)
            {
                postBondInspection.PostBond.State = AssistantStateEnum.Able;
            }

            frmAssistant.Dispose();
            this.BtAssistanAfterBond.Image = this.imageCollection1.Images[0];
            this.refreshAction();
        }

        #endregion

        /// <summary>
        /// 定位使用
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtLocateUseInfo_Click(object sender, EventArgs e)
        {
            FrmLocateUseInfo frmLocate = new FrmLocateUseInfo(this.oppositeSex.Config.LocateConfig);
            frmLocate.ShowDialog();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void UcObjectFunctionEdit_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void LueAdjustType_EditValueChanged(object sender, EventArgs e)
        {
            if ((AdjustTypeEnum)this.LueAdjustType.EditValue == AdjustTypeEnum.None)
            {
                this.BtLocateUseInfo.Enabled = false;
                this.BtAssistantVision.Enabled = false;
                this.baseConfig.IsPrAssistant = true;
                this.BtAssistantVision.Image = this.imageCollection1.Images[0];
            }
            else
            {
                this.BtLocateUseInfo.Enabled = true;
                this.BtAssistantVision.Enabled = true;

                if (!this.isLoad)
                {
                    this.baseConfig.IsPrAssistant = false;
                    this.BtAssistantVision.Image = this.imageCollection1.Images[1];
                }
            }
            this.refreshAction();

            this.oppositeSex.Config.LocateConfig.AdjustType = (AdjustTypeEnum)this.LueAdjustType.EditValue;
        }

        /// <summary>
        /// 系统1示教
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkMeasureHeightSystem1_CheckedChanged(object sender, EventArgs e)
        {
            this.oppositeSex.Config.MeasureHeightInSystem1 = this.ChkMeasureHeightSystem1.Checked;

            if (this.ChkMeasureHeightSystem1.Checked || this.ChkMeasureHeightSystem2.Checked)
            {
                this.BtAssistantMeasureHeight.Enabled = true;
            }
            else
            {
                this.BtAssistantMeasureHeight.Enabled = false;
            }
        }

        /// <summary>
        /// 系统2示教
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkMeasureHeightSystem2_CheckedChanged(object sender, EventArgs e)
        {
            this.oppositeSex.Config.MeasureHeightInSystem2 = this.ChkMeasureHeightSystem2.Checked;

            if (this.ChkMeasureHeightSystem1.Checked || this.ChkMeasureHeightSystem2.Checked)
            {
                this.BtAssistantMeasureHeight.Enabled = true;
            }
            else
            {
                this.BtAssistantMeasureHeight.Enabled = false;
            }
        }
        
        /// <summary>
        /// 新建胶量检测
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtNewCreateEpoxyCheck_Click(object sender, EventArgs e)
        {
            string name = XtraInputBox.Show("请输入名称", "名称", $"{MachineConfigContext.GetInstance().RecipeName + this.baseConfig.Name}" + "胶量检测");

            if (!string.IsNullOrEmpty(name))
            {
                // 移除原来的对象
                PostBondInspectionRepository.GetInstance().BaseDsSettingList.RemoveAll(
                    it => it.Name == name || it.Name == this.baseConfig.EpoxyCheckName);
                BondProgram.GetInstance().PostBondProgram.PostBondInspectionLists.RemoveAll(
                    it => it == name || it == this.baseConfig.EpoxyCheckName);

                // 创建新的对象
                PostBondInspection postBondInspection =
                    new PostBondInspection(name) { ApplicationSystem = PostBondApplicationSystemEnum.EpoxyCheck };
                PostBondInspectionRepository.GetInstance().BaseDsSettingList.Add(postBondInspection);
                BondProgram.GetInstance().PostBondProgram.PostBondInspectionLists.Add(postBondInspection.Name);
                if (!this.CmbEpoxyCheck.Properties.Items.Contains(postBondInspection.Name))
                {
                    this.CmbEpoxyCheck.Properties.Items.Add(postBondInspection.Name);
                }

                this.CmbEpoxyCheck.SelectedItem = postBondInspection.Name;
            }
            else
            {
                AKRSXtraMessageBox.Show("名字为空，请重新输入");
            }

            BondProgram.GetInstance().Save();
            PostBondInspectionRepository.GetInstance().Save();
            this.Save();
            this.BtAssistantEpoxyCheck.Image = this.baseConfig.IsEpoxyCheckAssistant
                                                   ? this.imageCollection1.Images[0]
                                                   : this.imageCollection1.Images[1];
            this.refreshAction();
        }

        /// <summary>
        /// 新建焊后检测
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtNewCreateAfterBond_Click(object sender, EventArgs e)
        {
            string name = XtraInputBox.Show("请输入名称", "名称", $"{MachineConfigContext.GetInstance().RecipeName + this.baseConfig.Name}" + "焊后检测");

            if (string.IsNullOrEmpty(name))
            {
                AKRSXtraMessageBox.Show("名字为空，请重新输入");
                return;
            }


            // 移除原来的对象
            PostBondInspectionRepository.GetInstance().BaseDsSettingList.RemoveAll(
                it => it.Name == name || it.Name == this.baseConfig.AfterBondName);
            BondProgram.GetInstance().PostBondProgram.PostBondInspectionLists.RemoveAll(
                it => it == name || it == this.baseConfig.AfterBondName);

            // 创建新的对象
            PostBondInspection postBondInspection =
                new PostBondInspection(name) { ApplicationSystem = PostBondApplicationSystemEnum.AfterBondCheck };
            PostBondInspectionRepository.GetInstance().BaseDsSettingList.Add(postBondInspection);
            BondProgram.GetInstance().PostBondProgram.PostBondInspectionLists.Add(postBondInspection.Name);

            if (!this.CmbAfterBond.Properties.Items.Contains(postBondInspection.Name))
            {
                this.CmbAfterBond.Properties.Items.Add(postBondInspection.Name);
            }
            
            this.CmbAfterBond.SelectedItem = postBondInspection.Name;

            BondProgram.GetInstance().Save();
            PostBondInspectionRepository.GetInstance().Save();
            this.Save();

            this.BtAssistanAfterBond.Image = this.baseConfig.IsAfterBondAssistant
                                                   ? this.imageCollection1.Images[0]
                                                   : this.imageCollection1.Images[1];
            this.refreshAction();
        }

        /// <summary>
        /// 示教形状
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAssistantSize_Click(object sender, EventArgs e)
        {
            TransportUnit transportUnit = TUAssistantHelper.JudgeTuExist();
            if (transportUnit == null)
            {
                return;
            }

            if (!System2Domain.GetInstance().System2Controller.ChangeTouchDownAssistance())
            {
                return;
            }

            OppositeSex parentOppositeSex = ProductDomain.GetInstance().ProductConfig.OppositeSexConfiguration
               .GetParentOppositeSex(this.oppositeSex);

            BaseMatter baseMatter = null;

            if (this.oppositeSex.MatterTypeEnum != EntityTypeEnum.TransportUnit)
            {
                baseMatter = transportUnit.GetBaseMatterByOppositeSex(parentOppositeSex);

                this.VisionBaseMatter(transportUnit, false);
                if (!baseMatter.BaseInfo.IsVisioned)
                {
                    return;
                }

                AKRSPoint3D point3D = baseMatter.CoordinateSystem.SelfPosToG0(new AKRSPoint3D());

                System2Domain.GetInstance().BondModuleController.CameraMoveToG0Pos(point3D);
            }

            FrmAssistantSizeAndCenter frmAssistantSizeAndCenter = new FrmAssistantSizeAndCenter(this.oppositeSex.Name, this.oppositeSex.MatterTypeEnum == EntityTypeEnum.TransportUnit);
            if (frmAssistantSizeAndCenter.ShowDialog() != DialogResult.OK)
            {
                frmAssistantSizeAndCenter.Dispose();
                return;
            }

            frmAssistantSizeAndCenter.Dispose();

            this.oppositeSex.SizeX =
                frmAssistantSizeAndCenter.RightUpPoint3D.X - frmAssistantSizeAndCenter.LeftDownPoint3D.X;
            this.oppositeSex.SizeY =
                frmAssistantSizeAndCenter.RightUpPoint3D.Y - frmAssistantSizeAndCenter.LeftDownPoint3D.Y;
            AKRSPoint3D centerPoint =
                (frmAssistantSizeAndCenter.RightUpPoint3D + frmAssistantSizeAndCenter.LeftDownPoint3D) / 2.0;

            GeneralCoordinateSystem generalCoordinateSystem = null;

            if (this.oppositeSex.MatterTypeEnum != EntityTypeEnum.TransportUnit)
            {
                 generalCoordinateSystem = (GeneralCoordinateSystem)baseMatter.CoordinateSystem;
            }
            else
            {
                generalCoordinateSystem = (GeneralCoordinateSystem)MachineCoordinateSystem
               .GetInstance().CoordinateSystems.Find(it => it.Name == "TransportCoordinateSystem");
            }

            

            // 转到自己的坐标系
            this.oppositeSex.ElementCoordinate.Point =
               generalCoordinateSystem.G0PosToSelf(centerPoint);
            
            if (this.oppositeSex.MatterTypeEnum == EntityTypeEnum.TransportUnit)
            {
                FrmEditSafeHeight frmEditSafeHeight = new FrmEditSafeHeight();
                if (frmEditSafeHeight.ShowDialog() == DialogResult.OK)
                {
                    ProductDomain.GetInstance().ProductConfig.TransportUnitConfig.SafeHeight =
                        frmEditSafeHeight.SafeHeight;
                }
            }

            this.oppositeSex.AssistantPos = true;
            this.BtAssistantSizeAndCenter.Image = this.imageCollection1.Images[0];
            this.refreshAction();
            ProductConfiguration.GetInstance().Save();
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSave_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            this.baseConfig.DispenseName = this.CmbDispense.SelectedItem?.ToString();
            this.baseConfig.EpoxyCheckName = this.CmbEpoxyCheck.SelectedItem?.ToString();
            this.baseConfig.BondComponentName = this.CmbBond.SelectedItem?.ToString();
            this.baseConfig.AfterBondName = this.CmbAfterBond.SelectedItem?.ToString();

            this.baseConfig.Dispense = this.ChkDispenseOpen.Checked;
            this.baseConfig.EpoxyCheck = this.ChkEpoxyCheckOpen.Checked;
            this.baseConfig.BondComponent = this.ChkBond.Checked;
            this.baseConfig.AfterBond = this.ChkAfterBondOpen.Checked;

            this.baseConfig.MeasureHeightInSystem1 = this.ChkMeasureHeightSystem1.Checked;
            this.baseConfig.MeasureHeightInSystem2 = this.ChkMeasureHeightSystem2.Checked;

            if ((this.baseConfig.MeasureHeightInSystem1 || this.baseConfig.MeasureHeightInSystem2) && this.baseConfig.HeightMeasurementPoints.Count == 0)
            {
                this.baseConfig.HeightMeasurementPoints.Add(new AKRSPoint3D());
            }

            this.refreshAction();
            ProductConfiguration.GetInstance().Save();
        }

        #region 开启/关闭功能

        /// <summary>
        /// 点胶开启
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkDispenseOpen_CheckedChanged(object sender, EventArgs e)
        {
            this.CmbDispense.Enabled = this.ChkDispenseOpen.Checked;
        }

        /// <summary>
        /// 胶量检测
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkEpoxyCheckOpen_CheckedChanged(object sender, EventArgs e)
        {
            this.CmbEpoxyCheck.Enabled = this.ChkEpoxyCheckOpen.Checked;
        }

        /// <summary>
        /// 芯片是否开启
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkBond_CheckedChanged(object sender, EventArgs e)
        {
            this.CmbBond.Enabled = this.ChkBond.Checked;
        }

        /// <summary>
        /// 焊后是否执行
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkAfterBondOpen_CheckedChanged(object sender, EventArgs e)
        {
            this.CmbAfterBond.Enabled = this.ChkAfterBondOpen.Checked;
        }

        #endregion

        /// <summary>
        /// 移动到参考点
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtMoveToCenter_Click(object sender, EventArgs e)
        {
            this.MoveToSelectCenter();
        }

        /// <summary>
        /// 移动到选中的中心
        /// </summary>
        private void MoveToSelectCenter()
        {
            if (!this.oppositeSex.AssistantPos)
            {
                AKRSXtraMessageBox.Show("未示教点位，无法移动");
                return;
            }

            TransportUnit transportUnit = TUAssistantHelper.JudgeTuExist();
            if (transportUnit == null)
            {
                return;
            }

            BaseMatter baseMatter = transportUnit.GetBaseMatterByOppositeSex(this.oppositeSex);

            this.VisionBaseMatter(transportUnit, true);

            if (!baseMatter.BaseInfo.IsVisioned)
            {
                return;
            }

            AKRSPoint3D point3D = baseMatter.CoordinateSystem.SelfPosToG0(new AKRSPoint3D());

            System2Domain.GetInstance().BondModuleController.CameraMoveToG0Pos(point3D);
        }

        /// <summary>
        /// 新建点胶
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtNewCreateDispense_Click(object sender, EventArgs e)
        {
            string name = XtraInputBox.Show("请输入名称", "名称", $"{MachineConfigContext.GetInstance().RecipeName + this.baseConfig.Name}" + "胶型");

            if (!string.IsNullOrEmpty(name))
            {
                // 移除原来的对象
                EpoxyApplicationRepository.GetInstance().BaseDsSettingList.RemoveAll(
                    it => it.Name == name || it.Name == this.baseConfig.EpoxyCheckName);
                System1Domain.GetInstance().System1Program.EpoxyApplicationProgram.EpoxyApplicationNames.RemoveAll(
                    it => it == name || it == this.baseConfig.EpoxyCheckName);

                // 创建新的对象
                EpoxyApplication epoxyApplication = new EpoxyApplication() { Name = name };
                EpoxyApplicationRepository.GetInstance().BaseDsSettingList.Add(epoxyApplication);
                System1Domain.GetInstance().System1Program.EpoxyApplicationProgram.EpoxyApplicationNames.Add(epoxyApplication.Name);
                this.CmbDispense.Properties.Items.Add(epoxyApplication.Name);
                this.CmbDispense.SelectedItem = epoxyApplication.Name;
            }
            else
            {
                AKRSXtraMessageBox.Show("名字为空，请重新输入");
            }

            System1Domain.GetInstance().System1Program.Save();
            EpoxyApplicationRepository.GetInstance().Save();
            this.Save();
        }

        /// <summary>
        /// 编辑点胶
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtEditEpoxy_Click(object sender, EventArgs e)
        {
            this.Save();
            EpoxyApplication epoxyApplication = System1Domain.GetInstance().System1Program.EpoxyApplicationProgram
                .EpoxyApplications.Find(it => it.Name == this.baseConfig.DispenseName);

            OppositeSex module = ProductDomain.GetInstance().ProductConfig.OppositeSexConfiguration
                .GetParentOppositeSex(this.oppositeSex);

            OppositeSex substrate = ProductDomain.GetInstance().ProductConfig.OppositeSexConfiguration
                .GetParentOppositeSex(module);

            Form from = new Form() { Size = new Size(500, 500) };
            MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System1;
            from.StartPosition = FormStartPosition.CenterParent;
            from.Text = "胶形编辑";
            from.Size = new Size(1259, 920);
            from.Controls.Add(
                new UcEpoxyApplication(epoxyApplication)
                    {
                        Dock = DockStyle.Fill,
                        SelectSubstrate = substrate.Name,
                        SelectModule = module.Name,
                        SelectBondPosition = this.oppositeSex.Name
                    });
            from.ShowDialog();
            MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System2;
        }

        /// <summary>
        /// 胶量检测发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CmbEpoxyCheck_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.isLoad)
            {
                return;
            }

            string name = (string)this.CmbEpoxyCheck.SelectedItem;
            if (string.IsNullOrEmpty(name) || !this.ChkEpoxyCheckOpen.Checked)
            {
                return;
            }

            this.baseConfig.EpoxyCheckName = name;

            this.BtAssistantEpoxyCheck.Image = this.baseConfig.IsEpoxyCheckAssistant
                                                 ? this.imageCollection1.Images[0]
                                                 : this.imageCollection1.Images[1];
            this.Save();
        }

        /// <summary>
        /// 焊后数据发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CmbAfterBond_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.isLoad)
            {
                return;
            }

            string name = (string)this.CmbAfterBond.SelectedItem;
            if (string.IsNullOrEmpty(name) || !this.ChkAfterBondOpen.Checked)
            {
                return;
            }

            this.baseConfig.AfterBondName = name;

            this.BtAssistanAfterBond.Image = this.baseConfig.IsAfterBondAssistant
                                                 ? this.imageCollection1.Images[0]
                                                 : this.imageCollection1.Images[1];
            this.Save();
        }

        /// <summary>
        /// 测高
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAssistanceHeight_Click(object sender, EventArgs e)
        {
            TransportUnit transportUnit = TUAssistantHelper.JudgeTuExist();
            if (transportUnit == null)
            {
                return;
            }

            if (!this.oppositeSex.AssistantPos)
            {
                AKRSXtraMessageBox.Show("请先示教位置");
                return;
            }

            // 更换touchdown
            if (!System2Domain.GetInstance().System2Controller.ChangeTouchDownAssistance())
            {
                return;
            }

            BaseMatter baseMatter = transportUnit.GetBaseMatterByOppositeSex(this.oppositeSex);

            this.VisionBaseMatter(transportUnit, true);

            if (!baseMatter.BaseInfo.IsVisioned)
            {
                return;
            }

            AKRSPoint3D point3D = baseMatter.CoordinateSystem.SelfPosToG0(new AKRSPoint3D());

            System2Domain.GetInstance().BondModuleController.CameraMoveToG0Pos(point3D);

            FrmAssistantHeight frmAssistantHeight = new FrmAssistantHeight(this.oppositeSex.Name);
            DialogResult dialogResult = frmAssistantHeight.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                GeneralCoordinateSystem generalCoordinateSystem = (GeneralCoordinateSystem)baseMatter.CoordinateSystem.UpperCoordinateSystem;

                double heightOb = generalCoordinateSystem.G0PosToSelf(new AKRSPoint3D(0, 0, frmAssistantHeight.ObHeight)).Z;

                // 转到自己的坐标系
                this.oppositeSex.ElementCoordinate.Point.Z = heightOb;

                this.ReLoad();

                ProductDomain.GetInstance().Save();
            }
            else
            {
                AKRSXtraMessageBox.Show("测高失败，请重试");
            }
        }

        private void VisionBaseMatter(TransportUnit transportUnit, bool visionSelf)
        {
            List<BaseMatter> baseMatters = new List<BaseMatter>();

            OppositeSex parentOppositeSex = this.oppositeSex;

            if (visionSelf)
            {
                BaseMatter baseMatter = transportUnit.GetBaseMatterByOppositeSex(parentOppositeSex);
                baseMatters.Add(baseMatter);
            }

            for (int i = 0; i < 4; i++)
            {
                parentOppositeSex = ProductDomain.GetInstance().ProductConfig.OppositeSexConfiguration
                   .GetParentOppositeSex(parentOppositeSex);

                if (parentOppositeSex == null)
                {
                    break;
                }

                BaseMatter baseMatter = transportUnit.GetBaseMatterByOppositeSex(parentOppositeSex);
                if (parentOppositeSex != null)
                {
                    baseMatters.Insert(0, baseMatter);
                }
            }

            foreach (BaseMatter baseMatter in baseMatters)
            {
                System2Domain.GetInstance().System2ObjectVision(baseMatter, false);
            }
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtBack_Click(object sender, EventArgs e)
        {
            this.backAction();
        }
    }
}
