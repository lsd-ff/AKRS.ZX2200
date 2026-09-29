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

namespace AKRS.ZX2200.WaferSubSystem.Controls
{
    using AKRS.Galaxy2.Machine;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;

    [Obsolete]
    public partial class FrmDieDeal : DevExpress.XtraEditors.XtraForm
    {
        UcGuideMove ucGuideMove;

        public FrmDieDeal()
        {
            InitializeComponent();
        }

        private void FrmDieDeal_Load(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
                {
                    ucGuideMove = new UcGuideMove("晶圆台模组", "FrmDieDeal", true, CameraEnum.WaferCamera);
                    this.PnlControl.Controls.Add(ucGuideMove);
                    ucGuideMove.Dock = DockStyle.Fill;
                }));

            if (Block.GetInstance().GetSearchMode() == SearchMode.Single)
            {
                this.LbString.Text = $"提示：未检测到芯片, 将搜索下一颗芯片，当前方向为{Block.GetInstance().GetSingleSearchDirection()}, 点击 \r\n" + "Skip：跳过当前芯片\r\n" + "Retry：重新检测\r\n" + "Change: 提前更换料片\r\n" + "Abort: 停止";
            }
            else
            {
                this.LbString.Text = "提示：未检测到芯片, 将搜索下一颗芯片, 点击 \r\n" + "Skip：跳过当前芯片\r\n" + "Retry：重新检测\r\n" + "Change: 提前更换料片\r\n" + "Abort: 停止";
            }
            
            Machine.GetInstance().OpenAlarm();
        }

        private void BtnSkip_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Ignore;
        }

        private void BtnRetry_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
        }

        private void BtnChange_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void BtnAbort_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
        }

        private void FrmDieDeal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Machine.GetInstance().ResetAlarm();
            this.ucGuideMove.Dispose();
        }
    }
}