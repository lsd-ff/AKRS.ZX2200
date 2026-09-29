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

namespace AKRS.ZX2200.CalibSystem.TestControls
{
    using System.Threading;

    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.CalibSystem.Test;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    public partial class FrmSetting : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 当前选择相机类型
        /// </summary>
        private CameraType curCameraTypeEnum;

        /// <summary>
        /// 循环次数
        /// </summary>
        private int cycles;

        /// <summary>
        /// 定位间隔时间
        /// </summary>
        private int interval;

        public FrmSetting()
        {
            InitializeComponent();
        }

        public FrmSetting(CameraType cameraType)
        {
            InitializeComponent();
            this.curCameraTypeEnum = cameraType;
        }

        /// <summary>
        /// 开始定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (this.txtCycles.Text == null)
            {
                AKRSXtraMessageBox.Show("Please input cycles！");
            }
            else if (this.txtInterval.Text == null)
            {
                AKRSXtraMessageBox.Show("Please input interval!");
            }
            else
            {
                this.cycles = Convert.ToInt32(this.txtCycles.Text);
                this.interval = Convert.ToInt32(this.txtInterval.Text);
            }

            if (this.curCameraTypeEnum == CameraType.BondCamera)
            {
                TestAlg.GetInstance().OnlyOnePointBond(this.cycles, this.interval);
            }
            else
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    "System2: Warning: \n"
                    + "Remove touchdown tool and Attach BMC tool holder to the bonding head.\r\n",
                    "Alarm",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Error);

                if (dialog == DialogResult.OK)
                {
                    // 三小时后再拍, 充分冷却后
                    //Thread.Sleep(7200000);

                    // Task.Run(() => TestAlg.GetInstance().OnlyOnePointUpLook(this.cycles, this.interval));

                    // 频闪拍摄 长延时 
                    Task t1 = Task.Run(() => TestAlg.GetInstance().OnlyOnePointUpLook(500, 5000, true));
                    t1.Wait();

                    // 常亮拍摄 1000 组  测相光源对相机的影响
                    Task t2 = Task.Run(() => TestAlg.GetInstance().OnlyOnePointUpLook(500, 5000, false));
                    t2.Wait();

                    // 频闪拍摄  短延时 主要测相机升温
                    Task t3 = Task.Run(() => TestAlg.GetInstance().OnlyOnePointUpLook(10000, 50, true));
                    t3.Wait();

                    Thread.Sleep(8000000);

                    // 频闪拍摄  短延时 主要测相机升温
                    Task t4 = Task.Run(() => TestAlg.GetInstance().OnlyOnePointUpLook(15000, 50, true));
                    t4.Wait();

                    // 常亮拍摄 1000 组 长延时  测相光源对相机的影响
                    Task t5 = Task.Run(() => TestAlg.GetInstance().OnlyOnePointUpLook(200, 5000, false));
                    t5.Wait();

                    AKRSXtraMessageBox.Show("测试完成");
                }
            }
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}