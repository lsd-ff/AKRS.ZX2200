using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.PR.Models.CommonModels;
using DevExpress.XtraEditors;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Forms;
using System.Windows.Threading;

namespace AKRS.Galaxy2.PR.Controls
{
    /// <summary>
    /// 结果显示类
    /// </summary>
    public partial class FrmVmVisionResult : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public FrmVmVisionResult()
        {
            this.InitializeComponent();
            this.ucVmResultDispense.CameraName = "点胶相机";
            this.ucVmResultBond.CameraName = "BOND相机";
            this.ucVmResultUpLook.CameraName = "上视相机";
            this.ucVmResultComponent.CameraName = "晶圆相机";
        }

        /// <summary>
        /// 是否已经连接了BroadcastBlock和ActionBlock，防止重复连接
        /// </summary>
        public static bool IsBroadcastBlockConnected = false;

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

        /// <summary>
        /// 窗体显示的时候
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmVmVisionResult_Shown(object sender, EventArgs e)
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

                    lock (objlock)
                    {
                        UcVmResultShow ucVmResultShow = null;
                        if (this.ucVmResultDispense.CameraName == prShowModel.CameraName || prShowModel.CameraName == "固晶补偿相机")
                        {
                            ucVmResultShow = this.ucVmResultDispense;
                        }
                        else if (this.ucVmResultBond.CameraName == prShowModel.CameraName)
                        {
                            ucVmResultShow = this.ucVmResultBond;
                        }
                        else if (this.ucVmResultUpLook.CameraName == prShowModel.CameraName)
                        {
                            ucVmResultShow = this.ucVmResultUpLook;
                        }
                        else if (this.ucVmResultComponent.CameraName == prShowModel.CameraName)
                        {
                            ucVmResultShow = this.ucVmResultComponent;
                        }
                        else
                        {
                          //  XtraMessageBox.Show($"Do not find camera name {prShowModel.Name}");
                            return;
                        }

                        ucVmResultShow.BeginInvoke(
                            new Action(
                                () =>
                                    {
                                        try
                                        {
                                            ucVmResultShow.ShowResult(prShowModel);
                                        }
                                        catch (Exception exception)
                                        {
                                            LogHelper.Post(Level.Error, exception.ToString(), LogCategory.PR);
                                        }
                                    }));
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Post(Level.Error, $"推送图片到主影像区异常：{ex.Message}", ex, Log.LogCategory.PR);
                }
            });

            // 将broadcast block的输出连接到actionblock
            // 链接日志流
            this.ShowPRReusltActionBlockDispose = FrmVmVisionResult.PrResultImageBlock.LinkTo(showPRReusltActionBlock);
            IsBroadcastBlockConnected = true;
        }

        /// <summary>
        /// 关闭界面时发生
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmVmVisionResult_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsBroadcastBlockConnected = false;
            this.Dispose();
            this.ucVmResultDispense.ImagesDispose();
            this.ucVmResultBond.ImagesDispose();
            this.ucVmResultUpLook.ImagesDispose();
            this.ucVmResultComponent.ImagesDispose();
        }

        /// <summary>
        /// 阻止关闭，变成隐藏
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmVmVisionResult_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
            //this.Hide();
        }
    }
}