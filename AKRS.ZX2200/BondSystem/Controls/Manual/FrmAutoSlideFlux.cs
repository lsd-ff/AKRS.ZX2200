using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using AKRS.Galaxy2.Infrastructure;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models.Programs;

namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    using System.Threading;

    /// <summary>
    ///  自动刮胶线程
    /// </summary>
    public partial class FrmAutoSlideFlux : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmAutoSlideFlux()
        {
            InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        ///  刮胶程式
        /// </summary>
        private SlideFluxerProgram slideFluxerProgram => BondProgram.GetInstance().SlideFluxerProgram;

        /// <summary>
        ///  刮胶控制器
        /// </summary>
        private SlideFluxerController slideFluxerController = new SlideFluxerController();

        /// <summary>
        ///  刮胶控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// 是否暂停
        /// </summary>
        private bool isStop = false;


        /// <summary>
        /// 刮胶线程
        /// </summary>
        private Task slideTask;

        /// <summary>
        ///  预热助焊剂
        /// </summary>
        private void PreHeatFlux()
        {
            for (int i = 0; i < this.slideFluxerProgram.SlideFluxTimes; i++)
            {
                this.slideFluxerController.SlideFlux();
            }

            Stopwatch sp = Stopwatch.StartNew();


            while (true)
            {
                if (this.isStop)
                {
                    return;
                }

                if (sp.Elapsed.TotalMinutes > this.slideFluxerProgram.SlideFluxInterval)
                {
                    for (int i = 0; i < this.slideFluxerProgram.SlideFluxTimes; i++)
                    {
                        this.slideFluxerController.SlideFlux();

                        if (this.isStop)
                        {
                            return;
                        }
                    }

                    sp.Restart();
                }

                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            this.SpSlideInterval.Value = (decimal)this.slideFluxerProgram.SlideFluxInterval;
            this.SpSlideTime.Value = (decimal)this.slideFluxerProgram.SlideFluxTimes;
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            BondProgram.GetInstance().SlideFluxerProgram.SlideFluxInterval = (int)this.SpSlideInterval.Value;
            BondProgram.GetInstance().SlideFluxerProgram.SlideFluxTimes = (int)this.SpSlideTime.Value;
            BondProgram.GetInstance().Save();
        }

        /// <summary>
        /// 开始/停止按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            Save();

            if (this.slideTask == null || this.slideTask.IsCompleted == true)
            {
                LbTip.Visible = true;

                // 开始采集
                this.isStop = false;
                this.BtnStart.Text = @"停止";
                this.BtnStart.Appearance.BackColor = Color.Yellow; 
                this.slideTask = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("自动刮胶线程");
                            this.bondModuleController.MoveToSafePos();
                            this.PreHeatFlux();
                        });

                this.slideTask.ContinueWith(t =>
                    {
                        if (this.BtnStart.InvokeRequired)
                        {
                            this.BtnStart.Invoke(
                                new Action(
                                    () =>
                                        {
                                            this.BtnStart.Text = @"开始";
                                            LbTip.Visible = false;
                                        }));
                        }
                        else
                        {
                            this.BtnStart.Text = @"开始";
                            LbTip.Visible = false;
                        }
                    });
            }
            else
            {
                // 暂停采集
                this.isStop = true;
                this.BtnStart.Text = @"开始";
                LbTip.Visible = false;
                this.BtnStart.Appearance.BackColor = default; 
            }
        }

        private void FrmAutoSlideFlux_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (this.slideTask != null && this.slideTask.IsCompleted == false)
            {
                e.Cancel = true;
            }
        }
    }
}