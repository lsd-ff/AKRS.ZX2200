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

using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
using AKRS.ZX2200.BondSystem.Modules;
using ch.etel.edi.dsa.v40;

namespace AKRS.ZX2200.Main.Controls.Ucmain.MainControls
{
    using AKRS.ZX2200.CalibSystem.GlobalCalibration;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.Main.Machine.Control;
    using DevExpress.XtraBars;

    /// <summary>
    /// 描述: Machine
    /// </summary>
    public partial class UcMainSystem
    {
        /// <summary>
        /// 设备硬件配置
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void barBtnHardwareConfig_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmMachineHardWareConfig frmMachineHardWareConfig = new FrmMachineHardWareConfig();
            frmMachineHardWareConfig.ShowDialog();
            frmMachineHardWareConfig.Dispose();
        }

        /// <summary>
        /// 设备硬件列表
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void barBtnHardWareList_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmHardWareCheck frmHardWareCheck = new FrmHardWareCheck();
            frmHardWareCheck.ShowDialog();
            frmHardWareCheck.Dispose();
        }
    }
}
