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
using System.Windows.Threading;
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
    public partial class FrmShowPRResultsNew : DevExpress.XtraEditors.XtraForm
    {

        // 创建了一个BroadcastBlock，然后创建了ActionBlock用于处理消息。
        // 通过LinkTo方法，将broadcastblock的输出传递给actionblock。
        // 等待action block完成处理后，释放并释放所有资源。这个例子展示了如何使用Dataflow库进行简单的数据流操作。

        // BroadcastBlock将输入的消息广播到多个目标块
        public static BroadcastBlock<PRResultShowModel> PrResultImageBlock = new BroadcastBlock<PRResultShowModel>(prResult => prResult);

        /// <summary>
        /// 进程锁
        /// </summary>
        private static object objlock = new object();

        /// <summary>
        /// 数据流释放对象
        /// </summary>
        private IDisposable ShowPRReusltActionBlockDispose;

        UcVmShowPRResult[] WafershowPRControlArray = new UcVmShowPRResult[4];
        UcVmShowPRResult[] UplookShowPRControlArray = new UcVmShowPRResult[4];
        UcVmShowPRResult[] DispenseShowPRControlArray = new UcVmShowPRResult[4];
        UcVmShowPRResult[] BondShowPRControlArray = new UcVmShowPRResult[4];

        UcVmShowPRResult[] WaferShowPRControlArrayTemp;
        UcVmShowPRResult[] UplookShowPRControlArrayTemp;
        UcVmShowPRResult[] DispenseShowPRControlArrayTemp;
        UcVmShowPRResult[] BondShowPRControlArrayTemp;

        int WaferCurrentIndex = 0;
        int UplookCurrentIndex = 0;
        int DispenseCurrentIndex = 0;
        int BondCurrentIndex = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmShowPRResultsNew()
        {
            InitializeComponent();
            WafershowPRControlArray[0] = this.ucVmShowPRResult1;
            WafershowPRControlArray[1] = this.ucVmShowPRResult2;
            WafershowPRControlArray[2] = this.ucVmShowPRResult5;
            WafershowPRControlArray[3] = this.ucVmShowPRResult6;

            UplookShowPRControlArray[0] = this.ucVmShowPRResult3;
            UplookShowPRControlArray[1] = this.ucVmShowPRResult4;
            UplookShowPRControlArray[2] = this.ucVmShowPRResult7;
            UplookShowPRControlArray[3] = this.ucVmShowPRResult8;

            DispenseShowPRControlArray[0] = this.ucVmShowPRResult9;
            DispenseShowPRControlArray[1] = this.ucVmShowPRResult10;
            DispenseShowPRControlArray[2] = this.ucVmShowPRResult13;
            DispenseShowPRControlArray[3] = this.ucVmShowPRResult14;

            BondShowPRControlArray[0] = this.ucVmShowPRResult11;
            BondShowPRControlArray[1] = this.ucVmShowPRResult12;
            BondShowPRControlArray[2] = this.ucVmShowPRResult15;
            BondShowPRControlArray[3] = this.ucVmShowPRResult16;

            this.WaferShowPRControlArrayTemp = this.WafershowPRControlArray;

            this.UplookShowPRControlArrayTemp = this.UplookShowPRControlArray;

            this.DispenseShowPRControlArrayTemp = this.DispenseShowPRControlArray;

            this.BondShowPRControlArrayTemp = this.BondShowPRControlArray;
        }


        UcVmShowPRResult showPRControl = new UcVmShowPRResult();      

        /// <summary>
        /// 窗体显示时
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmShowPRResultsNew_Shown(object sender, EventArgs e)
        {
            // ActionBlock是一个处理消息并可选择执行执行动作的块，如开始异步操作或保存结果到数据库
            // 创建action block用于处理broadcast block的输出
            // 构造日志流
            ActionBlock<PRResultShowModel> showPRReusltActionBlock = new ActionBlock<PRResultShowModel>(prShowModel =>
            {
                try
                {
                    if (prShowModel == null)
                    {
                        return;
                    }

                    Dispatcher.CurrentDispatcher.BeginInvoke
                    (
                        new Action(delegate ()
                        {

                            showPRControl = new UcVmShowPRResult();

                        }));


                    lock (objlock)
                    {
                        switch (prShowModel.CameraName)
                        {
                            case "晶圆相机":
                                showPRControl = WaferShowPRControlArrayTemp[WaferCurrentIndex];
                                WaferCurrentIndex++;

                                if (WaferCurrentIndex >= WaferShowPRControlArrayTemp.Length)
                                {
                                    WaferCurrentIndex = 0;
                                }
                                break;
                            case "上视相机":
                                showPRControl = UplookShowPRControlArrayTemp[UplookCurrentIndex];
                                UplookCurrentIndex++;

                                if (UplookCurrentIndex >= UplookShowPRControlArrayTemp.Length)
                                {
                                    UplookCurrentIndex = 0;
                                }
                                break;
                            case "点胶相机":
                                showPRControl = DispenseShowPRControlArrayTemp[DispenseCurrentIndex];
                                DispenseCurrentIndex++;

                                if (DispenseCurrentIndex >= DispenseShowPRControlArrayTemp.Length)
                                {
                                    DispenseCurrentIndex = 0;
                                }
                                break;
                            case "BOND相机":
                                showPRControl = BondShowPRControlArrayTemp[BondCurrentIndex];
                                BondCurrentIndex++;

                                if (BondCurrentIndex >= BondShowPRControlArrayTemp.Length)
                                {
                                    BondCurrentIndex = 0;
                                }
                                break;
                        }
                    }

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

            // 将broadcast block的输出连接到actionblock
            // 链接日志流
            this.ShowPRReusltActionBlockDispose = FrmShowPRResultsNew.PrResultImageBlock.LinkTo(showPRReusltActionBlock);
        }

        /// <summary>
        /// 窗体关闭时
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmShowPRResultsNew_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ShowPRReusltActionBlockDispose.Dispose();
        }
    }
}