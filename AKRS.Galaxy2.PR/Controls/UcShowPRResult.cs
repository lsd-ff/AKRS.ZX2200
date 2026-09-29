using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.MatchResults;
using log4net.Core;
using VM.PlatformSDKCS;

namespace AKRS.Galaxy2.PR.Controls
{
    /// <summary>
    ///  PR 结果显示
    /// </summary>
    public partial class UcShowPRResult : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// PR 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcShowPRResult()
        {
            InitializeComponent();
        }

        public void Show(PRResultShowModel prShowModel)
        {
            this.PePRResultImage.Image?.Dispose();
            this.Title = prShowModel.Name;
            MatchResult matchResult = prShowModel.PRResults[0] as MatchResult;
            if (matchResult != null)
            {
                this.LbPRName.Text = prShowModel.Name;
                this.LbScore.Text = "分值: " + matchResult.Score.ToString("0.00");
                this.LbTime.Text = System.DateTime.Now.ToString("HH:mm:ss");
                this.LbX.Text = "X: " + matchResult.CenterX.ToString("0.000");
                this.LbY.Text = "Y: " + matchResult.CenterY.ToString("0.000");
                this.LbAngle.Text = "Angle: " + matchResult.Angle.ToString("0.000");
                this.PePRResultImage.Image = prShowModel.PRImage;
                this.LbTimes.Text = "耗时: " + prShowModel.Times + "ms";

            }
       
        }
    }
}
