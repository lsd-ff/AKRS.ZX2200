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

namespace AKRS.Galaxy2.PR.Controls
{
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using LanguageExt;
    using VM.PlatformSDKCS;

    /// <summary>
    /// 定位结果显示
    /// 刘江宪改
    /// </summary>
    public partial class UcVmResultShow : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 图片对象
        /// </summary>
        private ZoomablePictureBox zoomablePictureBox;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcVmResultShow()
        {
            this.InitializeComponent();
            this.zoomablePictureBox = new ZoomablePictureBox();
            this.zoomablePictureBox.Dock = DockStyle.Fill;
            this.Controls.Add(this.zoomablePictureBox);
            this.BtNext.BringToFront();
            this.BtBack.BringToFront();
            this.SizeChanged += UcVmResultShow_SizeChanged;
        }

        /// <summary>
        /// 展示图片，相机所在的名称
        /// </summary>
        public string CameraName
        {
            get
            {
                return this.cameraName;
            }
            set
            {
                this.cameraName = value;
            }
        }

        /// <summary>
        /// 相机名称
        /// </summary>
        private string cameraName;

        /// <summary>
        /// 图像数据源
        /// </summary>
        private readonly List<PRResultShowModel> listImages = new List<PRResultShowModel>();

        /// <summary>
        /// 当前所展示的图像
        /// </summary>
        private PRResultShowModel currentResult;

        /// <summary>
        /// 最大的缓存数量
        /// </summary>
        public int MaxImageCount { get; set; } = 50;

        /// <summary>
        /// 缓存数量
        /// </summary>
        public int ImageCount { get; set; } = 20;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cameraName">相机名称</param>
        public UcVmResultShow(string cameraName)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 展示图片
        /// </summary>
        /// <param name="result">需要展示展示图片</param>
        public void ShowResult(PRResultShowModel result)
        {
            // 在界面刷新时不变
            if (this.ChkRealTimeRefresh.Checked)
            {
                // 集合中加入元素
                this.listImages.Add(result);

                // 展示图片
                this.ShowImage(result);

                // 保持集合数量不变
                while (this.listImages.Count >= this.ImageCount)
                {
                    this.listImages[0].PRImage.Dispose();
                    this.listImages.RemoveAt(0);
                }
            }
            else 
            {
                result.PRImage.Dispose();
            }
        }


        /// <summary>
        /// 展示结果
        /// </summary>
        /// <param name="prShowModel">PR模型</param>
        public void ShowImage(PRResultShowModel prShowModel)
        {
            this.currentResult = prShowModel;

            try
            {
                if (this.currentResult?.PRImage != null)
                {
                    this.zoomablePictureBox.Image = prShowModel.PRImage;
                }
            }

            catch (Exception ex) 
            {
            
            }
        }

        /// <summary>
        /// 左右键控制图片
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void PeImage_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.ChkRealTimeRefresh.Checked)
            {
                return;
            }

            int index = this.listImages.IndexOf(this.currentResult);

            switch (e.KeyCode)
            {
                case Keys.Right:
                    if (index < this.listImages.Count - 1)
                    {
                        this.currentResult = this.listImages[index + 1];
                        this.ShowImage(this.currentResult);
                    }

                    break;

                case Keys.Left:
                    if (index > 0)
                    {
                        this.currentResult = this.listImages[index - 1];
                        this.ShowImage(this.currentResult);
                    }

                    break;
            }
        }

        /// <summary>
        /// 上一张
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtBack_Click(object sender, EventArgs e)
        {
            int index = this.listImages.IndexOf(this.currentResult);

            if (index > 0)
            {
                this.currentResult = this.listImages[index - 1];
                this.ShowImage(this.currentResult);
            }
        }

        /// <summary>
        /// 下一张
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtNext_Click(object sender, EventArgs e)
        {
            int index = this.listImages.IndexOf(this.currentResult);

            if (index < this.listImages.Count - 1)
            {
                this.currentResult = this.listImages[index + 1];
                this.ShowImage(this.currentResult);
            }
        }

        /// <summary>
        /// 结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcVmResultShow_SizeChanged(object sender, EventArgs e)
        {
            this.BtBack.Location = new Point(10, this.Height / 2);

            this.BtNext.Location = new Point(this.Width - this.BtNext.Width - 10, this.Height / 2);

            this.ShowImage(this.currentResult);
        }

        /// <summary>
        /// 实时刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkRealTimeRefresh_CheckedChanged(object sender, EventArgs e)
        {
            this.BtBack.Visible = !this.ChkRealTimeRefresh.Checked;
            this.BtNext.Visible = !this.ChkRealTimeRefresh.Checked;
        }

        /// <summary>
        /// 图片清理
        /// </summary>
        public void ImagesDispose()
        {
            try
            {
                foreach (var image in this.listImages) 
                {
                    image.PRImage.Dispose();
                }
            }
            catch (Exception)
            {
            }
        
        }
    }
}
