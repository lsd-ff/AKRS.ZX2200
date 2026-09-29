using System;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using AKRS.ZX2200.TransportUnitSystem.Module.Config;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    using System.Drawing;
    using AKRS.ZX2200.BondSystem.Services;
    using AKRS.ZX2200.Controls;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using TransportUnit = AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit;

    /// <summary>
    /// 焊点测高示教窗体
    /// </summary>
    public partial class FrmBondPositionHeightTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="singleBondPositionConfig">焊点</param>
        public FrmBondPositionHeightTeach(SingleBondPositionConfig singleBondPositionConfig)
        {
            this.bondPosition = singleBondPositionConfig;
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 当前载具
        /// </summary>
        private TransportUnit transportUnit => TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 点击Teach传进来的焊点对象
        /// </summary>
        private SingleBondPositionConfig bondPosition;

        /// <summary>
        ///  方向盘
        /// </summary>
        private UcGuideMove ucGuideMove1 = new UcGuideMove("FrmBondPositionHeightTeach");

        /// <summary>
        /// 控件初始化
        /// </summary>
        private void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);

            this.LbDescription.Text = @"将touchdown移动到被测点表面正上方5mm处，点击DONE开始测高";

            // 设置高亮
            this.TileBar.SelectedItem = this.TbiMeasureHeight;

            this.PnlControl.Controls.Add(ucGuideMove1);

            // 方向盘设置模组名称
            this.ucGuideMove1.ChangeModuleName("固晶模组", true);
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
        /// 完成按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            if (!this.system2Controller.ChangeTouchDownAssistance())
            {
                return;
            }
            
            // 执行测高
            (ExcuteResult Ret, double HeightValue) res = this.bondHeadController.MeasureHeight(
    BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
    HeightMeasurementFunctionEnum.WithTDSensor);

            if (res.Ret == ExcuteResult.Success)
            {
                // 获取基岛
                Module module = this.transportUnit.GetModule(
                    this.bondPosition.TeachSubstrateNum,
                    this.bondPosition.TeachModuleNum);

                // 在G0中的高度
                double heightInG0 = this.bondModuleController.ConvertMachineToG0Pos(res.HeightValue);

                // 相对基岛的高度
                AKRSPoint3D heightInModule =
                    module.CoordinateSystem.G0PosToSelf(new AKRSPoint3D(0, 0, heightInG0));

                // 高度差
                double distanceHeight = this.bondPosition.ElementCoordinate.Point.Z - heightInModule.Z;

                this.bondPosition.LocateConfig.VisionPosOffSet(new AKRSPoint3D(0, 0, distanceHeight));

                // 记录测高结果,保存的是焊点高度相对基岛的差值
                this.bondPosition.ElementCoordinate.Point.Z = heightInModule.Z;

                // 保存当前载具
                ProductConfiguration.GetInstance().Save();

                this.DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// 自动对焦
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAutoFocus_Click(object sender, EventArgs e)
        {
            this.bondHeadController.AutoFocusAssistance(CameraTypeEnum.BondCamera, sender, this);
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmBondPositionHeightTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.system2Controller.PutbackNozzleAssitance();
        }
    }
}