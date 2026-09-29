using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using AKRS.ZX2200.TransportUnitSystem.Module.Config;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    using System.Drawing;
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;

    using TransportUnit = AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit;

    /// <summary>
    /// 相对固晶示教窗体
    /// </summary>
    public partial class FrmRelativeBondTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="singleBondPositionConfig">焊点</param>
        /// <param name="coordinateSystem">所在的坐标系</param>
        public FrmRelativeBondTeach(SingleBondPositionConfig singleBondPositionConfig, GeneralCoordinateSystem coordinateSystem)
        {
            this.bondPosition = singleBondPositionConfig;
            this.coordinateSystem = coordinateSystem;
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 坐标系
        /// </summary>
        private readonly GeneralCoordinateSystem coordinateSystem;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 点击Start传进来的焊点配置
        /// </summary>
        private SingleBondPositionConfig bondPosition;

        /// <summary>
        /// 界面配置集
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        ///  方向盘
        /// </summary>
        private UcGuideMove ucGuideMove1 = new UcGuideMove("FrmRelativeBondTeach");

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);
            this.PnlControl.Controls.Add(ucGuideMove1);

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 第一步：做模板
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"Step 1/1:将参考点移动到相机中心并编辑模板。",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                            },
                       doneAction: () =>
                       {
                           (bool success, AKRSPoint3D point) =
                               TUAssistantHelper.AssistantPR(this.bondPosition.LocateConfig.P1PRName);
                           if (success)
                           {
                               this.bondPosition.LocateConfig.P1VisionRelativePos = this.coordinateSystem.G0PosToSelf(point);;
                           }
                           else
                           {
                               AKRSXtraMessageBox.Show("视觉定位失败，请重新编辑模板！");
                               // this.stepIndex--;
                           }
                       }),
                };


            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiAjustPoint;

            // 绑定模组
            this.ucGuideMove1.ChangeModuleName("固晶模组", true);
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        private void SetUIControl(int stepIndex)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[stepIndex];

            this.LbDescription.Text = assistantConfig.Descritpion;
            this.BtDone.Visible = assistantConfig.IsShowDone;
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            DialogResult dialog = AKRSXtraMessageBox.Show(
                $"是否结束示教?",
                "Question",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// Done 按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.DoneAction();
            this.Save();
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            this.bondPosition.TeachBondingPosition.State = AssistantStateEnum.Able;
            ProductConfiguration.GetInstance().Save();
            this.DialogResult = DialogResult.OK;

            MachineConfigContext.GetInstance().SaveMachineConfig();

            TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.UpdateTransportUnit();
        }

        /// <summary>
        /// 制作PR
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnEditProgram_Click(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            // todo:加配方名
           // this.bondPosition.LocateConfig.P1PRName = MachineConfigContext.GetInstance().RecipeName + this.bondPosition.Name;

            TUAssistantHelper.EditPrInSystem2(this.bondPosition.LocateConfig.P1PRName, CameraTypeEnum.BondCamera);
        }

        /// <summary>
        /// 自动对焦
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAutoFocus_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.bondHeadController.AutoFocus(CameraTypeEnum.BondCamera);
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
    }
}