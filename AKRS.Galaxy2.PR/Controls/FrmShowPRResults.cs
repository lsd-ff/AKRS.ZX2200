using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.Entities;
using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors;
using log4net.Core;
using static DevExpress.XtraEditors.TextEdit;

namespace AKRS.Galaxy2.PR.Controls
{
    /// <summary>
    /// PR 结果显示窗体
    /// </summary>
    public partial class FrmShowPRResults : DevExpress.XtraEditors.XtraForm
    {
        public static BroadcastBlock<PRResultShowModel> PrResultImageBlock = new BroadcastBlock<PRResultShowModel>(prResult => prResult);

        /// <summary>
        /// 进程锁
        /// </summary>
        private static object objlock = new object();

        /// <summary>
        /// 数据流释放对象
        /// </summary>
        private IDisposable showPRReusltActionBlockDispose;

        UcShowPRResult [] showPRControlArray = new UcShowPRResult[16];

        UcShowPRResult[] showPRControl2X1Array = new UcShowPRResult[4];

        UcShowPRResult[] showPRControlArrayTemp;

        int currentIndex = 0;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmShowPRResults()
        {
            InitializeComponent();
            showPRControlArray[0] = this.ucShowPRResult1; showPRControlArray[1] = this.ucShowPRResult2; showPRControlArray[2] = this.ucShowPRResult3; showPRControlArray[3] = this.ucShowPRResult4;
            showPRControlArray[4] = this.ucShowPRResult5; showPRControlArray[5] = this.ucShowPRResult6; showPRControlArray[6] = this.ucShowPRResult7; showPRControlArray[7] = this.ucShowPRResult8;
            showPRControlArray[8] = this.ucShowPRResult9; showPRControlArray[9] = this.ucShowPRResult10; showPRControlArray[10] = this.ucShowPRResult11; showPRControlArray[11] = this.ucShowPRResult12;
            showPRControlArray[12] = this.ucShowPRResult13; showPRControlArray[13] = this.ucShowPRResult14; showPRControlArray[14] = this.ucShowPRResult15; showPRControlArray[15] = this.ucShowPRResult16;

            showPRControl2X1Array[0] = this.ucShowPRResult1; showPRControl2X1Array[1] = this.ucShowPRResult3;
            showPRControl2X1Array[2] = this.ucShowPRResult9; showPRControl2X1Array[3] = this.ucShowPRResult11;

            this.showPRControlArrayTemp = this.showPRControlArray;
        }

        /// <summary>
        /// 窗体显示时
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmPRResults_Shown(object sender, EventArgs e)
        {
            // 构造日志流
            ActionBlock<PRResultShowModel> showPRReusltActionBlock = new ActionBlock<PRResultShowModel>(prShowModel =>
            {
                try
                {
                    if (prShowModel == null)
                    {
                        return;
                    }

                    UcShowPRResult showPRControl;

                    lock (objlock)
                    {
                        showPRControl = showPRControlArrayTemp[currentIndex];
                        currentIndex++;

                        if (currentIndex >= showPRControlArrayTemp.Length)
                        {
                            currentIndex = 0;
                        }
                    }

                    //UcShowPRResult showPRControl =  showPRControlArrayTemp.FirstOrDefault(c => c.Title == prShowModel.Name);

                    //// 如果不存在
                    //if (showPRControl == null)
                    //{
                    //    lock (objlock)
                    //    {
                    //        showPRControl = showPRControlArrayTemp[currentIndex];
                    //        currentIndex++;
                    //        if (currentIndex >= showPRControlArrayTemp.Length)
                    //        {
                    //            currentIndex = 0;
                    //        }
                    //    }
                    //}

                    showPRControl.BeginInvoke
                    (
                        new Action(() =>
                        {
                            try
                            {
                                showPRControl.Show(prShowModel);                              
                            }
                            catch 
                            { 

                            }
                        })
                    );
                }
                catch (Exception ex)
                {
                    LogHelper.Post(Level.Error, $"推送图片到主影像区异常：{ex.Message}", ex, Log.LogCategory.PR);
                } 
            });

            // 链接日志流
            this.showPRReusltActionBlockDispose = FrmShowPRResults.PrResultImageBlock.LinkTo(showPRReusltActionBlock);
        }

        /// <summary>
        /// 窗体关闭时
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmPRResults_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.showPRReusltActionBlockDispose.Dispose();  
        }

        /// <summary>
        /// 2X1 布局
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBt2X1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            showPRControlArray.ForEach(c =>
            {
                c.Visible = false;
            });

            showPRControl2X1Array.ForEach(c => 
            {
                c.Visible = true;
                this.tablePanel1.SetRowSpan(c, 2);
                this.tablePanel1.SetColumnSpan(c, 2);
            });

            currentIndex = 0;
            this.showPRControlArrayTemp = showPRControl2X1Array;
        }

        /// <summary>
        /// 4X1 布局
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBt4X1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            showPRControlArray.ForEach(c =>
            {
                c.Visible = true;
                this.tablePanel1.SetRowSpan(c, 1);
                this.tablePanel1.SetColumnSpan(c, 1);
            });

            currentIndex = 0;
            this.showPRControlArrayTemp = showPRControlArray;
        }
    }
}