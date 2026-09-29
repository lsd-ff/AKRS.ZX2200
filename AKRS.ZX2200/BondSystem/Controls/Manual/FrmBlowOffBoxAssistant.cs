using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    using AKRS.ZX2200.Main.Machine.MachineSupport;

    using SqlSugar.Extensions;

    /// <summary>
    /// 抛料盒示教
    /// </summary>
    public partial class FrmBlowOffBoxAssistant : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmBlowOffBoxAssistant()
        {
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 固晶模组
        /// </summary>
        private BondModule bondModule => System2Module.GetInstance().BondModule;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        ///  方向盘
        /// </summary>
        private UcGuideMove ucGuideMove1 = new UcGuideMove("FrmBlowOffBoxAssistant");

        /// <summary>
        /// 控件初始化
        /// </summary>
        private void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBar);

            this.LbDescription.Text = MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured? @"移动到抛料盒位置。（注意：上视拍照位到抛料盒走直线，路径不能与刮胶盘干涉！）" : @"移动到抛料盒位置。";

            // 设置高亮
            this.TileBar.SelectedItem = this.TbiDeterminePosition;

            this.PnlControl.AddControl(ucGuideMove1);

            // 方向盘设置模组名称
            ucGuideMove1.ChangeModuleName("固晶模组", true);
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
                this.Close();
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
                this.DialogResult = DialogResult.OK;
                return;
            }

            // 获取位置、转到G0
            AKRSPoint3D point = this.bondModuleController.Get3DRealPosition();
            AKRSPoint3D posInG0 = bondModule.ConvertMachineToG0Pos(point);

            // 保存
            BondDevicePara.GetInstance().BondHeadParam.ThrowPos = posInG0;
            BondDevicePara.GetInstance().Save();

            this.DialogResult = DialogResult.OK;
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
        /// 吹气
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnBlow_Click(object sender, EventArgs e)
        {
            if (this.BtnBlow.Appearance.BackColor == Color.Transparent)
            {
                bondHeadController.SetBlowProportion(BondDevicePara.GetInstance().BondHeadParam.ThrowBlowProportion);
                Thread.Sleep(50);

                // 打开焊头吹气
                bondHeadController.OpenToolBlowEle();
                this.BtnBlow.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                // 关焊头吹气
                bondHeadController.CloseToolBlowEle();
                this.BtnBlow.Appearance.BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmBlowOffBoxAssistant_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ucGuideMove1?.Dispose();
        }
    }
}