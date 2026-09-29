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
using VMControls.Winform.Release;

namespace AKRS.Galaxy2.PR.Controls
{
    /// <summary>
    ///  PR 结果显示
    /// </summary>
    public partial class UcVmShowPRResult : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>

        public UcVmShowPRResult()
        {
            InitializeComponent();
        }



        public void Show(PRResultShowModel prShowModel)
        {
            //this.vmRenderControl.ClearDisplayView();
            //MatchResult matchResult = prShowModel.PRResults[0] as MatchResult;
            //if (matchResult != null)
            //{
            //    this.vmRenderControl.ImageSource = prShowModel.PRImage;

            //    // 绘制图形
            //    VMControls.WPF.TextEx renderCtrlText = new VMControls.WPF.TextEx();

            //    renderCtrlText.Content = "名称：" + prShowModel.Name + "\r\n" +
            //                              "耗时：" + prShowModel.Times + "ms \r\n" +
            //                              " X:" + matchResult.CenterX.ToString("0.000") + " \r\n " +
            //                              " Y:" + matchResult.CenterY.ToString("0.000") + " \r\n " +
            //                              "Angle: " + matchResult.Angle.ToString("0.000") + " \r\n " +
            //                              "分值: " + matchResult.Score.ToString("0.00") + " \r\n " +
            //                              "时间：" + System.DateTime.Now.ToString("HH:mm:ss");
            //    vmRenderControl.DrawShape(renderCtrlText);
            //}

        }
    }
}
