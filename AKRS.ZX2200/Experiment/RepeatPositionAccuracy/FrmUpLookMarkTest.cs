namespace AKRS.ZX2200.Experiment.RepeatPositionAccuracy
{
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    using DevExpress.XtraEditors;

    /// <summary>
    /// 上视测试窗体
    /// </summary>
    public partial class FrmUpLookMarkTest : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmUpLookMarkTest()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 循环次数
        /// </summary>
        private int cycle;

        /// <summary>
        /// 距离
        /// </summary>
        private double distance;

        /// <summary>
        /// 定位延时
        /// </summary>
        private int visionDelay;

        /// <summary>
        /// Z轴是否上下移动
        /// </summary>
        private bool isActiveZMove;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            this.Save();

            this.BtnStart.BackColor = Color.Yellow;

            // 寻找Pr模板
            PREntity upLookMarkEntity =
                (PREntity)VisionEntityRepository.GetInstance().Find("上视温漂测试Mark");

            try
            {
                // 去温漂Mark点
                this.bondModuleController.MoveSafeBondXYZ(
                    BondDevicePara.GetInstance().BondHeadParam.UpLookMarkVisionPos1);

                // 重复定位
                for (int i = 0; i < this.cycle; i++)
                {
                    this.LbCycle.Text = (i + 1).ToString();

                    if (this.isActiveZMove)
                    {
                        // 去温漂Mark点
                        this.bondModuleController.MoveSafeBondXYZ(
                            BondDevicePara.GetInstance().BondHeadParam.UpLookMarkVisionPos1);
                    }

                RetryUplookMark:
                    // 定位
                    ExcuteResult testResult = upLookMarkEntity.DoWork();

                    MatchResult testMatchResult = (MatchResult)upLookMarkEntity.AlgResult;

                    if (testResult == ExcuteResult.Fail || testMatchResult == null)
                    {
                        DialogResult dialog = AKRSMessageBoxExt.Show(
                            $"Downlook  上视温漂测试Mark  P1 adjust failed!\r\nRetry:retry point  adjust\r\nAbort: Exit program\r\n Ignore: Ignore this failure.\r\n",
                            "Alarm",
                            new string[] { "Retry", "Abort" },
                            new DialogResult[] { DialogResult.Retry, DialogResult.Abort },
                            AlarmLevel.SecondLevel);

                        switch (dialog)
                        {
                            case DialogResult.Retry:

                                goto RetryUplookMark;

                            case DialogResult.Abort:

                                throw new Exception($"上视温漂测试Mark  adjust  failed!");
                        }
                    }

                    System2CommonService.SaveVisionResult(testMatchResult, "UplookMarkVisionRes");

                    // 保存结果图片
                    Bitmap bitmap =
                       upLookMarkEntity.AlgResult.OutPutImg1;
                    string path = "D:\\UpLookMarkTest\\" + DateTime.Now.ToString("ddhhmmssfff") + upLookMarkEntity.GetName() + ".bmp";
                    bitmap.Save(path);

                    Thread.Sleep(this.visionDelay);

                    //if (i == 5000)
                    //{
                    //    BondModule bondModule = new BondModule();
                    //    bondModule.BondAxisX.ServoOff();
                    //    bondModule.BondAxisY.ServoOff();
                    //    Thread.Sleep(3600);
                    //}

                    // this.bondHeadController.RelativeMoveZAxis(this.distance);
                }

                //this.bondModuleController.MoveToSafePos();

                this.BtnStart.BackColor = Color.Transparent;
            }
            catch (Exception exception)
            {
                // 提示
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"UpLook  mark  test  failed !\r\n" + exception.ToString(),
                    "Question",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            this.cycle = (int)this.SpCycles.Value;

            this.distance = (double)this.SpDistance.Value;

            this.visionDelay = (int)this.SpVisionDelay.Value;

            this.isActiveZMove = this.ChkIsActiveZMove.Checked;
        }
    }
}