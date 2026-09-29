using System;

using DevExpress.Utils.Drawing;
using DevExpress.XtraSplashScreen;

namespace AKRS.ZX2200.Main.Controls.Ucmain
{
    /// <summary>
    /// Splash 等待画面
    /// </summary>
    public partial class SplashScreen1 : SplashScreen
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SplashScreen1()
        {
            this.InitializeComponent();
        }

        #region Overrides

        /// <summary>
        /// 进度控制
        /// </summary>
        /// <param name="cmd">指令</param>
        /// <param name="arg">参数</param>
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);

            SplashScreenCommand command = (SplashScreenCommand)cmd;
            string result = arg as string;

            if (command == SplashScreenCommand.SetProgress)
            {
                this.labelControl2.Text = result; 
                this.progressBarControl1.PerformStep();
                this.progressBarControl1.Update();
            }
            else if (command == SplashScreenCommand.InitView)
            {
                this.labelControl2.Text = result;
                this.marqueeProgressBarControl1.Properties.MarqueeAnimationSpeed = 50;
                this.marqueeProgressBarControl1.Properties.ProgressAnimationMode = ProgressAnimationMode.PingPong;

                this.progressBarControl1.Properties.Maximum = 100;
                this.progressBarControl1.Properties.Minimum = 0;
                this.progressBarControl1.Properties.PercentView = true;
                this.progressBarControl1.Properties.Step = 1;
            }
        }

        #endregion

        /// <summary>
        /// 命令枚举
        /// </summary>
        public enum SplashScreenCommand
        {
            /// <summary>
            /// SetProgress
            /// </summary>
            SetProgress,

            /// <summary>
            /// InitView
            /// </summary>
            InitView
        }
    }
}