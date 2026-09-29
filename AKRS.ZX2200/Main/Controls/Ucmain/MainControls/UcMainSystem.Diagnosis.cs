#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/11/30 12:34:30
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>

namespace AKRS.ZX2200.Main.Controls.Ucmain.MainControls
{
    using AKRS.ZX2200.BondSystem.Controls.Experiment;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.CalibSystem.Test;
    using AKRS.ZX2200.Experiment.RepeatPositionAccuracy;
    using AKRS.ZX2200.Experiment.Test;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using DevExpress.XtraBars;
    using DevExpress.XtraEditors;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// 描述: Diagnosis
    /// </summary>
    public partial class UcMainSystem
    {
        /// <summary>
        ///  信号
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarBtSignalDisplay_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmSignalDisplay frmSingnal = new FrmSignalDisplay();
            frmSingnal.ShowDialog();
        }

        /// <summary>
        /// BarBtBMCTest
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarBtBMCTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmBMCTest frmBmcTest = new FrmBMCTest();
            frmBmcTest.ShowDialog();
        }

        /// <summary>
        /// 上视模拟贴片实验
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarBtUplookSimulateBondTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmUpLookSimulateBondTest frmUpLookSimulateBondTest = new FrmUpLookSimulateBondTest();
            frmUpLookSimulateBondTest.ShowDialog();
        }

        /// <summary>
        ///  上视mark点拍照
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarBtUpLookMarkTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmUpLookMarkTest frmUpLookMarkTest = new FrmUpLookMarkTest();
            frmUpLookMarkTest.ShowDialog();
        }

        /// <summary>
        ///  震动测试
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BarBtVibrationTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            BtStop frmVibrationTest = new BtStop();
            frmVibrationTest.Show();
        }

        /// <summary>
        /// 点胶相机单步标定
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnDispenseCamera_ItemClick(object sender, ItemClickEventArgs e)
        {
            Task.Run(CalibrateTask.GetInstance().OnlyDispenseCameraCalib);
        }

        /// <summary>
        /// Bond相机单步标定
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnBondCamera_ItemClick(object sender, ItemClickEventArgs e)
        {
            Task.Run(CalibrateTask.GetInstance().OnlyBondCameraCalib);
        }

        /// <summary>
        /// Bond旋转中心到相机偏移单步标定
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnBondToCamera_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult dialog = AKRSXtraMessageBox.Show(
                "System2: \n"
                + "Remove touchdown tool and Attach BMC tool holder to the bonding head.\r\n",
                "Prompt",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information);
            if (dialog == DialogResult.OK)
            {
                Task.Run(CalibrateTask.GetInstance().OnlyBondToCameraOffsetCaib);
            }
        }

        /// <summary>
        /// 上视相机及上视旋转中心单步标定
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnUpLookCamera_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult dialog = AKRSXtraMessageBox.Show(
                "System2: \n"
                + "Remove touchdown tool and Attach BMC tool holder to the bonding head.\r\n",
                "Prompt",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information);
            if (dialog == DialogResult.OK)
            {
                Task.Run(CalibrateTask.GetInstance().OnlyUpLookCameraCalib);
            }
        }

        /// <summary>
        /// 晶圆相机单步标定
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnWaferCamera_ItemClick(object sender, ItemClickEventArgs e)
        {
            Task.Run(CalibrateTask.GetInstance().OnlyWaferCameraCaib);
        }

        /// <summary>
        /// 测试三点一线准确性
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnBondToUpLook_ItemClick(object sender, ItemClickEventArgs e)
        {
            Task.Run(TestAlg.GetInstance().TestBondToCamOffsetAccuracy);
        }

        /// <summary>
        /// 测试旋转中心温漂点关系性
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnRotateCenterAndTemp_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmRotateCenterAccuracyTwo frmRotateCenterAccuracyTwo = new FrmRotateCenterAccuracyTwo();
            frmRotateCenterAccuracyTwo.Show();
        }

        /// <summary>
        /// 测试移动准确度
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnMoveAccuracy_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmTestMoveAccuracy frmTestMoveAccuracy = new FrmTestMoveAccuracy();
            frmTestMoveAccuracy.Show();
        }

        /// <summary>
        /// 焊头稳定性测试
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BarBtBondHeadTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmBondheadTest frmBondheadTest = new FrmBondheadTest();
            frmBondheadTest.Show();
        }
    }
}
