using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
using AKRS.Galaxy2.LogicHardware.Repository;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace AKRS.Galaxy2.PR.Controls
{
    public partial class FrmPRImage : DevExpress.XtraEditors.XtraForm
    {

        // halcon窗口ID
        private HTuple windowID;

        //鼠标按下时的行坐标
        private double rowDown;

        // 鼠标按下时的列坐标
        private double colDown;

        // 相机
        private AKRSCamera camera;

        // 实时图像
        private HObject currentImage;

        // 鼠标按下标志
        private bool isDown = false;

        // 采图结束标志位
        private bool isFinish = false;

        public FrmPRImage(string cameraName)
        {
            InitializeComponent();
            windowID = hWindowControl1.HalconWindow;
            this.hWindowControl1.HMouseWheel += this.My_HMouseWheel;
            camera = HardwareRepositoryService.GetHardware<AKRSCamera>(cameraName);                            
            Task.Run(RealTimeRefreshImg);
        }

        /// <summary>
        /// 获取像素坐标
        /// </summary>
        /// <returns></returns>
        public (double x, double y) GetPixelPos()
        {
            if (isDown)
            {
                return (this.colDown, this.rowDown);
            }
            else
            {
                XtraMessageBox.Show("请单击鼠标左键确定顶针位置！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (0, 0);
            }
        }


        /// <summary>
        /// 实时刷新图片
        /// </summary>
        private void RealTimeRefreshImg()
        {
            while (!isFinish)
            {
                Bitmap bitmap = this.camera.SnapShot(false, SnapImageFormat.Format8bppIndexed);
                if (bitmap != null)
                {
                    Bitmap bitmap1 = (Bitmap)bitmap.Clone();
                    HObject image;
                    HOperatorSet.GenEmptyObj(out image);
                    this.Bitmap2HImage(bitmap1, out image);
                    this.currentImage = image;
                    if(this.currentImage != null)
                    {
                        HOperatorSet.DispObj(this.currentImage, this.hWindowControl1.HalconWindow);
                    }                    
                }
                Thread.Sleep(200);
            }
        }


        /// <summary>
        /// 鼠标滚轮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void My_HMouseWheel(object sender, HalconDotNet.HMouseEventArgs e)
        {

            HalconDotNet.HTuple Zoom, Row, Col, Button = 0;
            HalconDotNet.HTuple Row0, Column0, Row00, Column00, Ht, Wt, r1, c1, r2, c2;
            if (e.Delta > 0)
            {
                Zoom = 1.5;
            }
            else
            {
                Zoom = 0.5;
            }

            HalconDotNet.HTuple WindowID = ((HalconDotNet.HWindowControl)sender).HalconWindow;
            HalconDotNet.HOperatorSet.SetDraw(WindowID, "margin");
            HalconDotNet.HOperatorSet.GetMposition(WindowID, out Row, out Col, out Button);
            HalconDotNet.HOperatorSet.GetPart(WindowID, out Row0, out Column0, out Row00, out Column00);
            Ht = Row00 - Row0;
            Wt = Column00 - Column0;
            if (Ht * Wt < 32000 * 32000 || Zoom == 1.5)
            {
                r1 = (Row0 + ((1 - (1.0 / Zoom)) * (Row - Row0)));
                c1 = (Column0 + ((1 - (1.0 / Zoom)) * (Col - Column0)));
                r2 = r1 + (Ht / Zoom);
                c2 = c1 + (Wt / Zoom);
                HalconDotNet.HOperatorSet.SetPart(WindowID, r1, c1, r2, c2);
                HalconDotNet.HOperatorSet.ClearWindow(WindowID);
                if (this.currentImage != null)
                {
                    HalconDotNet.HOperatorSet.DispObj(this.currentImage, WindowID);
                }
            }

        }

        /// <summary>
        /// Bitmap转Hobject
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="image"></param>
        public void Bitmap2HImage(Bitmap bmp, out HObject image)
        {
            try
            {
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                BitmapData bitmapData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
                HOperatorSet.GenImage1(out image, "byte", bmp.Width, bmp.Height, bitmapData.Scan0);
                bmp.UnlockBits(bitmapData);
            }
            catch
            {
                image = null;
            }
        }



        private void BtnOK_Click(object sender, EventArgs e)
        {
            isFinish = true;
            this.DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            isFinish = true;
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hWindowControl1_HMouseDown_1(object sender, HMouseEventArgs e)
        {
            isDown = true;
            HTuple Row, Column, Button;
            HOperatorSet.GetMposition(hWindowControl1.HalconWindow, out Row, out Column, out Button);
            rowDown = Row;    //鼠标按下时的行坐标
            colDown = Column; //鼠标按下时的列坐标
            this.txtX.Text = Column.ToString();
            this.txtY.Text = Row.ToString();
        }

        /// <summary>
        /// 鼠标上抬事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hWindowControl1_HMouseUp_1(object sender, HMouseEventArgs e)
        {

            HTuple row1, col1, row2, col2, Row, Column, Button;
            HOperatorSet.GetMposition(windowID, out Row, out Column, out Button);
            double RowMove = Row - rowDown;
            double ColMove = Column - colDown;
            HOperatorSet.GetPart(windowID, out row1, out col1, out row2, out col2);
            HOperatorSet.SetPart(windowID, row1 - RowMove, col1 - ColMove, row2 - RowMove, col2 - ColMove);
            HOperatorSet.ClearWindow(windowID);
            if (this.currentImage != null)
            {
                HOperatorSet.DispObj(this.currentImage, windowID);
            }
        }
    }
}
