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

namespace AKRS.Galaxy2.PR.Controls
{
    /// <summary>
    /// 焊后结果显示窗体
    /// </summary>
    public partial class FrmDefectResult : XtraForm
    {
        /// <summary>
        /// 焊后结果显示窗体
        /// </summary>
        public FrmDefectResult()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 是否已经连接了BroadcastBlock和ActionBlock，防止重复连接
        /// </summary>
        public static bool IsBroadcastBlockConnected = false;

        /// <summary>
        /// 胶量检测图片集合
        /// </summary>
        private List<PRResultShowModel> EpoxyDefectPRResultShowModels = new List<PRResultShowModel>();

        /// <summary>
        /// 控件集合
        /// </summary>
        private List<ZoomablePictureBox> EpoxyDefectZoomablePictureBoxes = new List<ZoomablePictureBox>();

        /// <summary>
        /// 控件集合
        /// </summary>
        private List<ZoomablePictureBox> PostbondZoomablePictureBoxes = new List<ZoomablePictureBox>();

        /// <summary>
        /// 焊后检测图片集合
        /// </summary>
        private List<PRResultShowModel> PostBondPRResultShowModels = new List<PRResultShowModel>();

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
        private void FrmDefectResult_Shown(object sender, EventArgs e)
        {
            this.EpoxyDefectZoomablePictureBoxes.Add(this.zoomablePictureBox1);
            this.EpoxyDefectZoomablePictureBoxes.Add(this.zoomablePictureBox2);
            this.EpoxyDefectZoomablePictureBoxes.Add(this.zoomablePictureBox3);
            this.EpoxyDefectZoomablePictureBoxes.Add(this.zoomablePictureBox4);

            this.PostbondZoomablePictureBoxes.Add(this.zoomablePictureBox6);
            this.PostbondZoomablePictureBoxes.Add(this.zoomablePictureBox7);
            this.PostbondZoomablePictureBoxes.Add(this.zoomablePictureBox8);
            this.PostbondZoomablePictureBoxes.Add(this.zoomablePictureBox9);



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
                        this.BeginInvoke(
                            new Action(
                                () =>
                                {
                                    try
                                    {
                                        this.InputImageToForm(prShowModel);
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
            this.ShowPRReusltActionBlockDispose = FrmDefectResult.PrResultImageBlock.LinkTo(showPRReusltActionBlock);

            IsBroadcastBlockConnected = true;
        }

        /// <summary>
        /// 显示图片到窗体
        /// </summary>
        /// <param name="prResult">结果</param>
        private void InputImageToForm(PRResultShowModel prResult)
        {
            if (prResult.AlgFlowType == AlgFlowTypeEnum.EpoxyDetectAlg)
            {
                if (this.EpoxyDefectPRResultShowModels.Count > 4)
                {
                    this.EpoxyDefectPRResultShowModels[0].PRImage.Dispose();
                    this.EpoxyDefectPRResultShowModels.RemoveAt(0);
                }

                this.EpoxyDefectPRResultShowModels.Add(prResult);

                for (int i = 0; i < this.EpoxyDefectPRResultShowModels.Count; i++)
                {
                    this.EpoxyDefectZoomablePictureBoxes[i].Image = this.EpoxyDefectPRResultShowModels[i].PRImage;
                }
            }
            else
            {
                if (this.PostBondPRResultShowModels.Count > 4)
                {
                    this.PostBondPRResultShowModels[0].PRImage.Dispose();
                    this.PostBondPRResultShowModels.RemoveAt(0);
                }

                this.PostBondPRResultShowModels.Add(prResult);

                for (int i = 0; i < this.PostbondZoomablePictureBoxes.Count; i++)
                {
                    this.PostbondZoomablePictureBoxes[i].Image = this.PostBondPRResultShowModels[i].PRImage;
                }
            }

        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmDefectResult_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                foreach (var item in this.EpoxyDefectPRResultShowModels)
                {
                    item.PRImage.Dispose();
                }

                this.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("关闭定位结果窗体失败" + ex.Message);
            }
            finally
            {
                IsBroadcastBlockConnected = false;
            }
        }
    }
}
