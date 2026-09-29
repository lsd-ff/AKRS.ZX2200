using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
using System;

namespace AKRS.ZX2200.BondSystem.Controls.Setting.PostBond
{
    using AKRS.ZX2200.BondSystem.Models.ActionNodes.BPActionNode;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Controls.Feature.EditProgram.UcEditProductProgram;
    using AKRS.ZX2200.Infrastructure.Interface;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using DevExpress.XtraEditors;

    using BondPosition = AKRS.ZX2200.TransportUnitSystem.Module.Matter.BondPosition;
    using Module = AKRS.ZX2200.TransportUnitSystem.Module.Matter.Module;
    using TransportUnit = AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit;

    /// <summary>
    /// 焊后检测编辑界面
    /// </summary>
    public partial class UcPostBondEdit : DevExpress.XtraEditors.XtraUserControl, IProgrammingControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="postBondInspection">传入的对象</param>
        public UcPostBondEdit(PostBondInspection postBondInspection)
        {
            this.postBondInspection = postBondInspection;
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 点Start传进来的对象
        /// </summary>
        private readonly PostBondInspection postBondInspection;

        private bool isInit = false;

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            #region  控件绑定数据源

            this.LueMeasurePointNumber.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<MeasurePointNumberEnum>();
            this.LueApplicationSystem.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<PostBondApplicationSystemEnum>();
            this.LueReference.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<MeasurePointNumberEnum>();

            #endregion

            this.LueMeasurePointNumber.EditValue = this.postBondInspection.MeasurePointNumber;
            this.LueReference.EditValue = this.postBondInspection.ReferencePointNumber;
            this.RgMode.SelectedIndex = (int)this.postBondInspection.PostBondInspectionMode;
            this.LueApplicationSystem.EditValue = this.postBondInspection.ApplicationSystem;


            this.SpLimitX.EditValue = this.postBondInspection.LimitX;
            this.SpLimitY.EditValue = this.postBondInspection.LimitY;
            this.SpLimitAngle.EditValue = this.postBondInspection.LimitAngle;

            this.SpTolerantEpoxyMax.EditValue = this.postBondInspection.TolerantEpoxyMax;
            this.SpTolerantEpoxyMin.EditValue = this.postBondInspection.TolerantEpoxyMin;

            this.SpMaxBLTOffset.EditValue = this.postBondInspection.MaxBLTOffset;
            this.SpMaxSurfaceSlope.EditValue = this.postBondInspection.MaxSurfaceSlope;

            this.SpFrequency.EditValue = this.postBondInspection.DefectFrequency;

            this.ChkDefectFrequencyOpen.Checked = this.postBondInspection.IsDefectFrequencyOpen;

            foreach (SingleBondPositionConfig singleBondPositionConfig in ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList)
            {
                this.CmbBondPositon.Properties.Items.Add(singleBondPositionConfig.Name);
            }

            if (this.CmbBondPositon.Properties.Items.Count > 0)
            {
                this.CmbBondPositon.SelectedIndex = 0;
            }

            this.ChkPreDispenseCheck.Checked = this.postBondInspection.IsPreDispenseCheck;

            this.ChkBackSideCrackDetection.Checked = this.postBondInspection.IsDetectBacksideCrack;

            foreach (EpoxyApplication epoxyApplication in System1Program.GetInstance().EpoxyApplicationProgram.EpoxyApplications)
            {
                this.CmbReDispenseName.Properties.Items.Add(epoxyApplication.Name);
            }

            foreach (EpoxyApplication epoxyApplication in BondProgram.GetInstance().EpoxyApplicationProgram.EpoxyApplications)
            {
                if (!System1Program.GetInstance().EpoxyApplicationProgram.EpoxyApplications.Contains(epoxyApplication))
                {
                    this.CmbReDispenseName.Properties.Items.Add(epoxyApplication.Name);
                }
            }

            this.CmbReDispenseName.Properties.Items.Add("Null");

            this.CmbReDispenseName.SelectedItem = this.postBondInspection.ReDispenseName;

            this.ChkIsCycle.Checked = this.postBondInspection.IsCircle;

            this.isInit = true;
        }

        /// <summary>
        /// 确定,保存
        /// </summary>
        public void Confirm()
        {
            this.postBondInspection.PostBondInspectionMode = (PostBondInspectionModeEnum)this.RgMode.SelectedIndex;
            this.postBondInspection.MeasurePointNumber = (MeasurePointNumberEnum)Enum.Parse(typeof(MeasurePointNumberEnum), this.LueMeasurePointNumber.EditValue.ToString());
            this.postBondInspection.ReferencePointNumber = (MeasurePointNumberEnum)Enum.Parse(typeof(MeasurePointNumberEnum), this.LueReference.EditValue.ToString());
            this.postBondInspection.ApplicationSystem = (PostBondApplicationSystemEnum)Enum.Parse(typeof(PostBondApplicationSystemEnum), this.LueApplicationSystem.EditValue.ToString());

            this.postBondInspection.LimitX = (double)this.SpLimitX.Value;

            this.postBondInspection.LimitY = (double)this.SpLimitY.Value;

            this.postBondInspection.LimitAngle = (double)this.SpLimitAngle.Value;

            this.postBondInspection.TolerantEpoxyMax = (double)this.SpTolerantEpoxyMax.Value;
            this.postBondInspection.TolerantEpoxyMin = (double)this.SpTolerantEpoxyMin.Value;

            this.postBondInspection.MaxBLTOffset = (double)this.SpMaxBLTOffset.Value;
            this.postBondInspection.MaxSurfaceSlope = (double)this.SpMaxSurfaceSlope.Value;

            this.postBondInspection.IsPreDispenseCheck = this.ChkPreDispenseCheck.Checked;

            this.postBondInspection.ReDispenseName = this.CmbReDispenseName.SelectedItem?.ToString();

            this.postBondInspection.IsDefectFrequencyOpen = this.ChkDefectFrequencyOpen.Checked;

            this.postBondInspection.DefectFrequency = (int)this.SpFrequency.Value;

            this.postBondInspection.IsDetectBacksideCrack = this.ChkBackSideCrackDetection.Checked;
            this.ChkIsCycle.Checked = this.postBondInspection.IsCircle;

            PostBondInspectionRepository.GetInstance().Save();
        }

        /// <summary>
        /// 执行定位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtPostBondDoWork_Click(object sender, EventArgs e)
        {
            if (!this.postBondInspection.IsAssistantSucceed)
            {
                AKRSXtraMessageBox.Show("请先完成示教");
                return;
            }

            TransportUnit transportUnit = TUAssistantHelper.JudgeTuExist();

            if (transportUnit == null)
            {
                return;
            }

            Substrate substrate = transportUnit.Substrates.Find(it => it.Index == this.SpSubStrate.Value);

            Module module = substrate.Modules.Find(it => it.Index == this.SpModule.Value);

            BondPosition bondPosition =
                module.BondPositions.Find(it => it.Name == (string)this.CmbBondPositon.SelectedItem);

            AfterBondCheckActionNode actionNode =
                System2Domain.GetInstance().BondActionNodeRepository.AfterBondCheckActionNode;

            if (!TUAssistantHelper.System2VisionByPos(bondPosition))
            {
                return;
            }

            actionNode.IsEditTest = true;
            actionNode.Substrate = substrate;
            actionNode.Module = module;
            actionNode.BondPosition = bondPosition;
            actionNode.PostBondInspection = this.postBondInspection;
            actionNode.DoWork();
        }

        /// <summary>
        /// 选择发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void LueApplicationSystem_EditValueChanged(object sender, EventArgs e)
        {
            if ((PostBondApplicationSystemEnum)Enum.Parse(typeof(PostBondApplicationSystemEnum), this.LueApplicationSystem.EditValue.ToString()) == PostBondApplicationSystemEnum.EpoxyCheck)
            {
                this.groupControl2.Enabled = false;
                this.LueMeasurePointNumber.Enabled = false;
                this.SpTolerantEpoxyMax.Enabled = true;
                this.SpTolerantEpoxyMin.Enabled = true;
                this.LueReference.Enabled = false;
                this.ChkPreDispenseCheck.Enabled = true;
                this.groupControl7.Enabled = true;

                this.ChkBackSideCrackDetection.Checked = false;
                this.ChkBackSideCrackDetection.Enabled = false;
                this.ChkIsCycle.Enabled = true;

                this.SpMaxBLTOffset.Enabled = false;
                this.SpMaxSurfaceSlope.Enabled = false;

                this.SpLimitX.Enabled = false;
                this.SpLimitY.Enabled = false;
                this.SpLimitAngle.Enabled = false;
            }
            else if ((PostBondApplicationSystemEnum)Enum.Parse(typeof(PostBondApplicationSystemEnum), this.LueApplicationSystem.EditValue.ToString()) == PostBondApplicationSystemEnum.AfterBondCheck)
            {
                this.groupControl2.Enabled = true;
                this.LueMeasurePointNumber.Enabled = true;
                this.SpTolerantEpoxyMax.Enabled = false;
                this.SpTolerantEpoxyMin.Enabled = false;
                this.ChkPreDispenseCheck.Enabled = false;
                this.groupControl7.Enabled = false;

                this.ChkBackSideCrackDetection.Enabled = true;
                this.ChkIsCycle.Enabled = false;

                this.SpMaxBLTOffset.Enabled = false;
                this.SpMaxSurfaceSlope.Enabled = false;

                this.SpLimitX.Enabled = true;
                this.SpLimitY.Enabled = true;
                this.SpLimitAngle.Enabled = true;
            }
            else
            {
                this.SpLimitX.Enabled = false;
                this.SpLimitY.Enabled = false;
                this.SpLimitAngle.Enabled = false;


                this.groupControl2.Enabled = false;
                this.LueMeasurePointNumber.Enabled = false;
                this.SpTolerantEpoxyMax.Enabled = false;
                this.SpTolerantEpoxyMin.Enabled = false;
                
                this.ChkPreDispenseCheck.Enabled = false;
                this.groupControl7.Enabled = false;

                this.ChkBackSideCrackDetection.Enabled = false;
                this.ChkIsCycle.Enabled = false;

                this.SpMaxBLTOffset.Enabled = true;
                this.SpMaxSurfaceSlope.Enabled = true;
            }
        }

        /// <summary>
        /// 模式选择发生了改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void RgMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LueReference.Enabled = this.RgMode.SelectedIndex != 0;
        }

        /// <summary>
        /// 启用选择发生了改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void RgMode_EnabledChanged(object sender, EventArgs e)
        {
            this.LueReference.Enabled = this.RgMode.SelectedIndex != 0;
        }

        /// <summary>
        /// 当选择发生了改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkPreDispenseCheck_CheckedChanged(object sender, EventArgs e)
        {
            this.groupControl7.Enabled = !this.ChkPreDispenseCheck.Checked;
            this.SpSubStrate.Enabled = !this.ChkPreDispenseCheck.Checked;
            this.SpModule.Enabled = !this.ChkPreDispenseCheck.Checked;
            this.CmbBondPositon.Enabled = !this.ChkPreDispenseCheck.Checked;

            this.ChkBackSideCrackDetection.Checked =
                false;

            if (this.ChkPreDispenseCheck.Checked)
            {
                this.groupControl7.Enabled = false;
                this.SpSubStrate.Enabled = false;
                this.SpModule.Enabled = false;
                this.CmbBondPositon.Enabled = false;
                this.BtPostBondDoWork.Enabled = false;
                this.BtDoWorkInPreDispense.Enabled = true;
            }
            else
            {
                this.groupControl7.Enabled = true;
                this.SpSubStrate.Enabled = true;
                this.SpModule.Enabled = true;
                this.CmbBondPositon.Enabled = true;
                this.BtPostBondDoWork.Enabled = true;
                this.BtDoWorkInPreDispense.Enabled = false;
            }
        }

        /// <summary>
        /// 执行预点胶
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtDoWorkInPreDispense_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 开启抽检
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkDefectFrequencyOpen_CheckedChanged(object sender, EventArgs e)
        {
            this.postBondInspection.IsDefectFrequencyOpen = this.ChkDefectFrequencyOpen.Checked;

            this.SpFrequency.Enabled = this.ChkDefectFrequencyOpen.Checked;
        }

        /// <summary>
        /// 勾选框改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkBackSideCrackDetection_CheckedChanged(object sender, EventArgs e)
        {
            this.RgMode.SelectedIndex = 0;
            this.LueReference.ItemIndex = 0;
            this.ChkPreDispenseCheck.Checked = false;


            this.RgMode.Enabled = !this.ChkBackSideCrackDetection.Checked;
            this.LueReference.Enabled = !this.ChkBackSideCrackDetection.Checked;

            if (this.isInit)
            {
                this.Confirm();

                if (this.postBondInspection.IsDetectBacksideCrack)
                {
                    this.postBondInspection.BacksideCrackDetection.State = AssistantStateEnum.UnAble;
                }
                else
                {
                    this.postBondInspection.BacksideCrackDetection.State = AssistantStateEnum.ForBidden;
                }

                // 编程界面刷新
                UcEditProductProgramming.RefreshILstAssistantStepWithPBAction(this.postBondInspection);
            }
        }

        /// <summary>
        /// 是否为圆形
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkIsCycle_CheckedChanged(object sender, EventArgs e)
        {
            this.postBondInspection.IsCircle = this.ChkIsCycle.Checked;
        }
    }
}
