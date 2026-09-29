using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;

namespace AKRS.ZX2200.CalibSystem.Controls.Assistant
{
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Product;
    using AKRS.ZX2200.TransportSystem.Models;

    using static Infrastructure.Controls.Feature.MeasuringTool.FrmMultipleHeightMeasurementType;

    /// <summary>
    /// 轨道测试
    /// </summary>
    public partial class FrmTransportTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 轨道坐标系
        /// </summary>
        public FrmTransportTeach()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 方向盘
        /// </summary>
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// switch当前索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 系统2挡料气缸的位置
        /// </summary>
        private AKRSPoint3D system2StopPos;

        /// <summary>
        /// 系统2挡料气缸的高度
        /// </summary>
        private double system2StopHeight;

        /// <summary>
        /// 系统2挡料气缸的位置
        /// </summary>
        private AKRSPoint3D system1StopPos;

        /// <summary>
        /// 系统2挡料气缸的高度
        /// </summary>
        private double system1StopHeight;

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            this.assistantConfigList = new List<AssistantConfig>
                                           {
                                               // 寻找挡料气缸XY的位置
                                               new AssistantConfig(
                                                   index: 0,
                                                   descritpion: "Step 1/4:\r\n 移动Bond相机，使得相机十字线对准挡料气缸(或者载具的同一个Mark点)",
                                                   isShowTitle: true,
                                                   isShowBack: false,
                                                   isShowNext: true,
                                                   isShowDone: false,
                                                   backAction: () => { },
                                                   nextAction: () =>
                                                       {
                                                           this.TileBarTeach.SelectedItem = this.TbiRotaryPoint2;

                                                           this.system2StopPos = TUAssistantHelper.GetPosBySystem(null,MultipleHeightMeasurementType.SubstrateCamera);
                                                       },
                                                   doneAction: () => { }),

                                               // 测高
                                               new AssistantConfig(
                                                   index: 1,
                                                   descritpion: "Step 2/4:\r\n 移动Bond相机迪，使得相机十字线对准垫块上任一点",
                                                   isShowTitle: true,
                                                   isShowBack: true,
                                                   isShowNext: true,
                                                   isShowDone: false,
                                                   backAction: () =>
                                                       {
                                                           this.TileBarTeach.SelectedItem = this.TbiRotaryPoint1;
                                                       },
                                                   nextAction: () =>
                                                       {
                                                           (ExcuteResult result, double height) =
                                                               TUAssistantHelper.AssistantMeasureHeight(
                                                                   MultipleHeightMeasurementType.SubstrateCamera);

                                                           if (result != ExcuteResult.Success)
                                                           {
                                                               this.DialogResult = DialogResult.Abort;
                                                           }

                                                           this.system2StopHeight = height;

                                                           MachineStateModel.GetInstance().CurrentMachineSystem
                                                               = CurrentMachineSystemEnum.System1;

                                                           CommonHelper.ChangeUcMove(this.ucGuideMove, MachineStateModel.GetInstance().CurrentMachineSystem);
                                                       },
                                                   doneAction: () =>
                                                       {
                                                           this.system2StopPos.Z = this.system2StopHeight;
                                                           GeneralCoordinateSystem ts =
                                                               (GeneralCoordinateSystem)MachineCoordinateSystem
                                                                   .GetInstance().CreateCoordinateSystem(
                                                                       "TransportCoordinateSystem",
                                                                       "G0",
                                                                       true,
                                                                       CoordinateSystemTypeEnum.General);

                                                           ts.Init(this.system2StopPos, new AKRSPoint3D());
                                                           MachineCoordinateSystem.GetInstance().Save();
                                                           MachineCoordinateSystem.Refresh();
                                                       }),

                                               // 系统1寻找挡料气缸
                                               new AssistantConfig(
                                                   index: 2,
                                                   descritpion: "Step 3/4:\r\n 移动点胶相机，使得相机十字线对准挡料气缸(或者载具的同一个Mark点)",
                                                   isShowTitle: true,
                                                   isShowBack: true,
                                                   isShowNext: true,
                                                   isShowDone: false,
                                                   backAction: () =>
                                                       {
                                                           MachineStateModel.GetInstance().CurrentMachineSystem
                                                               = CurrentMachineSystemEnum.System2;

                                                           CommonHelper.ChangeUcMove(this.ucGuideMove, MachineStateModel.GetInstance().CurrentMachineSystem);
                                                           this.TileBarTeach.SelectedItem = this.TbiRotaryPoint2;
                                                       },
                                                   nextAction: () =>
                                                       {
                                                           this.system1StopPos = TUAssistantHelper.GetPosBySystem(null,MultipleHeightMeasurementType.SubstrateCamera);
                                                       },
                                                   doneAction: () => { }),

                                               // 系统2寻找基板
                                               new AssistantConfig(
                                                   index: 3,
                                                   descritpion: "Step 4/4:\r\n 移动点胶相机，使得相机十字线对准垫块上任一点",
                                                   isShowTitle: true,
                                                   isShowBack: true,
                                                   isShowNext: false,
                                                   isShowDone: true,
                                                   backAction: () =>
                                                       {
                                                           this.TileBarTeach.SelectedItem = this.TbiRotaryPoint3;
                                                       },
                                                   nextAction: () => { },
                                                   doneAction: () =>
                                                       {
                                                           (ExcuteResult result, double height) =
                                                               TUAssistantHelper.AssistantMeasureHeight(
                                                                   MultipleHeightMeasurementType.SubstrateCamera);

                                                           if (result != ExcuteResult.Success)
                                                           {
                                                               this.DialogResult = DialogResult.Abort;
                                                           }

                                                           this.system1StopHeight = height;

                                                           #region 计算

                                                           this.system2StopPos.Z = this.system2StopHeight;
                                                           this.system1StopPos.Z = this.system1StopHeight;

                                                           this.system1StopPos = System1Domain.GetInstance().DispenseController
                                                               .ConvertG0ToMachinePos(this.system1StopPos);

                                                            AKRSPoint3D point3D = MachineCoordinateSystem
                                                                   .GetInstance().TransportSystem.G0PosToSelf(new AKRSPoint3D());

                                                           GeneralCoordinateSystem ts =
                                                               (GeneralCoordinateSystem)MachineCoordinateSystem
                                                                   .GetInstance().CreateCoordinateSystem(
                                                                       "TransportCoordinateSystem",
                                                                       "G0",
                                                                       true,
                                                                       CoordinateSystemTypeEnum.General);

                                                           ts.Init(this.system2StopPos, new AKRSPoint3D());
                                                           MachineCoordinateSystem.GetInstance().Save();
                                                           MachineCoordinateSystem.Refresh();

                                                           AKRSPoint3D point3DDs = MachineCoordinateSystem
                                                                   .GetInstance().DispesenSystem.G0PosToSelf(new AKRSPoint3D());

                                                           GeneralCoordinateSystem ds =
                                                               (GeneralCoordinateSystem)MachineCoordinateSystem
                                                                   .GetInstance().CreateCoordinateSystem(
                                                                       "DispenseCoordinateSystem",
                                                                       "G0",
                                                                       true,
                                                                       CoordinateSystemTypeEnum.General);

                                                           ds.Init(this.system2StopPos, this.system1StopPos);
                                                           MachineCoordinateSystem.GetInstance().Save();
                                                           MachineCoordinateSystem.Refresh();

                                                           AKRSPoint3D point3DDs2 = MachineCoordinateSystem
                                                                   .GetInstance().DispesenSystem.G0PosToSelf(new AKRSPoint3D());

                                                           AKRSPoint3D point3D2 = MachineCoordinateSystem
                                                                   .GetInstance().TransportSystem.G0PosToSelf(new AKRSPoint3D());

                                                            AKRSPoint3D offsetDs = point3DDs2 - point3DDs;

                                                           AKRSPoint3D offset = point3D - point3D2;

                                                            DispenseDevicePara.GetInstance().OffSetPoint(offsetDs);

                                                           // 载具也偏移
                                                           ProductDomain.GetInstance().ProductConfig.TransportUnitConfig.ElementCoordinate.Point -= offset;
                                                           ProductDomain.GetInstance().Save();

                                                           #endregion

                                                           MachineStateModel.GetInstance().CurrentMachineSystem
                                                               = CurrentMachineSystemEnum.System2;

                                                           CommonHelper.ChangeUcMove(this.ucGuideMove, MachineStateModel.GetInstance().CurrentMachineSystem);

                                                           MachineCoordinateSystem.GetInstance().Save();
                                                       }),
                                           };

            this.ucGuideMove = new UcGuideMove("FrmTransportTeach");
            CommonHelper.ChangeUcMove(this.ucGuideMove, MachineStateModel.GetInstance().CurrentMachineSystem);
            this.PnlControl.Controls.Add(this.ucGuideMove);
            this.SetUiControl(0);

            // 首个步骤的Description
            this.LbDescription.Text = this.assistantConfigList[0].Descritpion;
            TUAssistantHelper.SetColor(this.TileBarTeach);
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        private void SetUiControl(int stepIndex)
        {
            this.TileBarTeach.SelectedItem = this.tileBarGroup2.Items[stepIndex];
            AssistantConfig assistantConfig = this.assistantConfigList[stepIndex];

            this.BtBack.Visible = assistantConfig.IsShowBack;
            this.BtNext.Visible = assistantConfig.IsShowNext;
            this.BtDone.Visible = assistantConfig.IsShowDone;
            this.LbDescription.Text = assistantConfig.Descritpion;
            this.LbDescription.Visible = assistantConfig.IsShowTitle;
        }

        /// <summary>
        /// Next
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtNext_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.NextAction();
            this.stepIndex++;
            this.SetUiControl(this.stepIndex);
        }

        /// <summary>
        /// 返回上一步
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtBack_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.BackAction();
            this.stepIndex--;
            this.SetUiControl(this.stepIndex);
        }

        /// <summary>
        /// cancel按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Done按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.DoneAction();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 自动聚焦
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAutoFocus_Click(object sender, EventArgs e)
        {
            TUAssistantHelper.AutoFocus(sender, this);
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmTransportTeach_Load(object sender, EventArgs e)
        {
            //DispenseDevicePara.GetInstance().OffSetPoint(new AKRSPoint3D(-74,0,0));
            MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System2;
            this.InitControl();
            this.SetUiControl(this.stepIndex);

            if (!MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                this.assistantConfigList[1].IsShowNext = false;
                this.assistantConfigList[2].IsShowDone = true;
                this.tileBarGroup2.Items.RemoveAt(2);
                this.tileBarGroup2.Items.RemoveAt(3);
            }
        }

        /// <summary>
        /// 清空载具
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtClearTu_Click(object sender, EventArgs e)
        {
            TransportDomain.GetInstance().TransportController.InitializeTS();
        }

        /// <summary>
        /// 搜索系统1载具
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSearch1Tu_Click(object sender, EventArgs e)
        {
            TransportDomain.GetInstance().TransportController.DispenseSubSectionController.SearchBelt();
        }

        /// <summary>
        /// 搜索系统2载具
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSearch2Tu_Click(object sender, EventArgs e)
        {
            TransportDomain.GetInstance().TransportController.BondSubSectionController.SearchBelt();
        }

        private void FrmTransportTeach_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ucGuideMove.Dispose();
        }
    }
}