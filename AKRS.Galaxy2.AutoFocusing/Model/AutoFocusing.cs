
using DevExpress.XtraEditors;
using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionDesigner;
using VisionDesigner.ImageSharpness;

namespace AKRS.Galaxy2.AutoFocusing
{
    using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    public class AutoFocusing
    {      

        public AutoFocusing() 
        { 

        }

        public double DoFocusing(string CameraName, Axis AxisZ, double Plimit, double Nlimit)
        {
            AKRSCamera Camera = HardwareRepositoryService.GetHardware<AKRSCamera>(CameraName);

            Bitmap bitmap = null;
            if (AxisZ != null)
            {
                (bool IsEnable, double N, double P) = AxisZ.GetSoftLimit();
                if (IsEnable)
                {
                    // 第一次步进距离
                    double FirOffset = (Plimit - Nlimit) / 5;
                    List<double> FirFocusRes = new List<double>();
                    List<double> FirPosition = new List<double>();
                    for (int Firindex = 0; Firindex < 6; Firindex++)
                    {
                        FirPosition.Add(Nlimit + Firindex * FirOffset);
                        AxisZ.AbsoluteMove(Nlimit + Firindex * FirOffset);

                        bitmap = Camera.SnapImage(false, false);
                        FirFocusRes.Add(this.CalSharpnessVm(bitmap));

                        if (Firindex != 0 && FirFocusRes[Firindex] < FirFocusRes[Firindex - 1])
                        {
                            break;
                        }
                    }

                    // 获取清晰度值最大的索引，移动到第一聚焦位置              
                    double FirStation = 0;
                    if(FirFocusRes.Count() < 3)
                    {
                        FirStation= FirPosition[FirPosition.Count - 1];
                    }
                    else
                    {
                        if (FirFocusRes[FirFocusRes.Count - 1] < FirFocusRes[FirFocusRes.Count - 3])
                        {
                            FirStation = FirPosition[FirPosition.Count - 2];
                        }
                        else
                        {
                            FirStation = FirPosition[FirPosition.Count - 1];
                        }
                    }
                    

                    // 第二次步进距离
                    double SecondOffset = (Plimit - Nlimit) / (5 * 5);
                    List<double> SecFocusRes = new List<double>();
                    List<double> SecPosition = new List<double>();
                    for (int Secindex = 0; Secindex < 6; Secindex++)
                    {
                        SecPosition.Add(FirStation - Secindex * SecondOffset);
                        AxisZ.AbsoluteMove(FirStation - Secindex * SecondOffset);

                        bitmap = Camera.SnapImage(false, false);
                        SecFocusRes.Add(this.CalSharpnessVm(bitmap));

                        if (Secindex != 0 && SecFocusRes[Secindex] < SecFocusRes[Secindex - 1])
                        {
                            break;
                        }
                    }

                    double[] ThirdFocusRes = new double[9];
                    double[] ThirdPosition = new double[9];

                    // 第三次步进距离
                    double ThirdOffset = (Plimit - Nlimit) / (5 * 5 * 5);
                    double SecStation = 0;
                    if (SecFocusRes[SecFocusRes.Count - 1] < SecFocusRes[SecFocusRes.Count - 3])
                    {
                        SecStation = SecPosition[SecFocusRes.Count - 2];
                        ThirdFocusRes[0] = SecFocusRes[SecFocusRes.Count - 2];
                    }
                    else
                    {
                        SecStation = SecPosition[SecFocusRes.Count - 1];
                        ThirdFocusRes[0] = SecFocusRes[SecFocusRes.Count - 1];
                    }
                    ThirdPosition[0] = SecStation;

                    for (int Thirindex = 0; Thirindex < 8; Thirindex++)
                    {
                        ThirdPosition[Thirindex+1] = SecStation-0.5* SecondOffset + Thirindex * ThirdOffset;
                        AxisZ.AbsoluteMove(SecStation - 0.5 * SecondOffset + Thirindex * ThirdOffset);

                        bitmap = Camera.SnapImage(false, false);
                        ThirdFocusRes[Thirindex+1] = this.CalSharpnessVm(bitmap);
                    }

                    double[] Parameters = Fit.Polynomial(ThirdPosition, ThirdFocusRes, 2);
                    if (Parameters[2] != 0)
                    {
                        double FocusPosition = -Parameters[1] / (2 * Parameters[2]);
                        AxisZ.AbsoluteMove(FocusPosition);
                        return FocusPosition;
                    }
                    else
                    {
                        XtraMessageBox.Show("拟合多项式失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return 0;
                    }
                }
                else
                {
                    XtraMessageBox.Show("Z轴无法使用!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return 0;
                }
            }
            else
            {
                XtraMessageBox.Show("Z轴没有配置!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return 0;
            }

        }

        private bool stopAdjusting = false;

        private double maxSharpnessScore = 0;

        private double stepSize1 = 1;

        public void DoFocusingNew(string CameraName, Axis AxisZ, double Plimit, double Nlimit)
        {
            AKRSCamera Camera = HardwareRepositoryService.GetHardware<AKRSCamera>(CameraName);
            int directionChangeCount = 0;
            AxisZ.AbsoluteMove(Plimit);
            while (!stopAdjusting)
            {
                // 获取当前图像
                Bitmap bitmap = Camera.SnapImage(false, false);

                // 计算清晰度得分
                double sharpnessScore = this.CalSharpnessVm(bitmap);

                // 根据清晰度得分调整阈值和轴位置
                AdjustThreshold(0);

                // 计算轴移动方向
                var direction = sharpnessScore >= this.maxSharpnessScore ? 1 : -1;

                // 移动轴
                AxisZ.RelativeMove(direction * stepSize1);

                // 判断是否停止调整
                if (sharpnessScore < maxSharpnessScore)
                {
                    directionChangeCount++;
                    if (directionChangeCount >= 2)
                    {
                        stopAdjusting = true;
                    }
                    else
                    {
                        stepSize1 /= 2; // 缩小步长
                    }
                }
                else
                {
                    directionChangeCount = 0;
                    maxSharpnessScore = sharpnessScore;
                    stepSize1 = 1; // 恢复初始步长
                }

                if (AxisZ.GetRealPosition() <= Nlimit)
                {
                    stopAdjusting = true;
                }
            }
        }

        public void AdjustThreshold(double threshold)
        {
        }

        /// <summary>
        /// 自动对焦
        /// </summary>
        /// <param name="camera">相机</param>
        /// <param name="axisZ">Z轴</param>
        /// <param name="pLimit">最大位置（结束位置）</param>
        /// <param name="nLimit">最小位置（开始位置）</param>
        public void AutoFocus(AKRSCamera camera, Axis axisZ, double pLimit, double nLimit)
        {
            int T = 0;
   

            List<double> posList = new List<double>();
            List<double> sharpnessList = new List<double>();

            axisZ.AbsoluteMove(nLimit);
            double distance = (pLimit - nLimit) / 5;

            double pulse = distance / 5;
            while (true)
            {
                

                axisZ.RelativeMove(distance);
                // 获取当前图像
                Bitmap bitmap = camera.SnapImage(false, false);
                Rectangle Roi = new Rectangle();     // 需要对焦的区域
                // 计算清晰度得分
                double currentArticulation = this.CalSharpnessVm(bitmap);
                posList.Add(axisZ.GetRealPosition());
                sharpnessList.Add(currentArticulation);

                if((axisZ.GetRealPosition() + distance) > pLimit)
                {
                    break;
                }
            }

            double max = sharpnessList[0];
            int index = 0;
            for (int i = 0; i < sharpnessList.Count; i++)
            {
                if (sharpnessList[i] >= max)
                {
                    max = sharpnessList[i];
                    index = i;
                }
            }

            axisZ.AbsoluteMove(posList[index]);

            label:
            T++;
            List<double> focusL = new List<double>();
            int Round = 6;
            while (true)
            {
                // 获取当前图像
                Bitmap bitmap = camera.SnapImage(false, false);
                Rectangle Roi = new Rectangle();     // 需要对焦的区域
                // 计算清晰度得分
                double currentArticulation = this.CalSharpnessVm(bitmap);

                focusL.Add(currentArticulation);
                if (focusL.Count == 1 ) // 这里是第一次运动
                {
                    axisZ.RelativeMove(pulse); 
                    continue;
                }
                // 前两张确认调焦方向
                if (focusL.Count == 2)
                {
                    if (currentArticulation > focusL[focusL.Count - 2]) // 判断是否跑对方向
                    {
                        pulse *= 1; // 对的方向
                        axisZ.RelativeMove(pulse);
                    }
                    else
                    {
                        pulse *= -1; // 不对的方向我们相反就可以了
                        axisZ.RelativeMove(pulse * 1);   // 这里不是跑回原来的位置，而是更过去一些，节约时间
                    }

                    continue;
                }

                // 清晰度大于前一张图
                if (currentArticulation > focusL[focusL.Count - 2])
                {
                    axisZ.RelativeMove(pulse);
                }
                else
                {

                    // 返回峰值，跳出循环
                    if (T == Round)
                    {
                        break; // 两轮结束了，跳出循环
                    }
                    pulse *= -0.5;
                    goto label;

                }
            }
        }


        // 首次步进距离
        private double stepSize = 4;

        // 轴方向
        private int direction = 1;


        public void DoFocusingNew2(string CameraName, Axis AxisZ, double Plimit, double Nlimit)
        {

            // 结束的判断是？当前的清晰度值-最大清晰度值 < 1

            AKRSCamera Camera = HardwareRepositoryService.GetHardware<AKRSCamera>(CameraName);

            AxisZ.AbsoluteMove(Plimit);

            while (!stopAdjusting)
            {
                // 获取当前图像
                Bitmap bitmap = Camera.SnapImage(false, false);

                // 计算清晰度得分
                double sharpnessScore = this.CalSharpnessVm(bitmap);

                // 判断是否停止调整
                if (sharpnessScore < this.maxSharpnessScore)
                {

                    stepSize /= 3; // 缩小步长
                    direction = -direction;
                }

                if (sharpnessScore-this.maxSharpnessScore<1)
                {
                    AxisZ.RelativeMove(-direction * stepSize * 3);
                    stopAdjusting = true;
                    break;
                }
                this.maxSharpnessScore = sharpnessScore;

                // 移动轴
               
                AxisZ.RelativeMove(direction * stepSize);

                if (AxisZ.GetRealPosition() <= Nlimit)
                {
                    stopAdjusting = true;
                    break;
                }
            }

        }




        private double CalSharpnessVm(Bitmap inputBitmap)
        {
            CImageSharpnessTool imageSharpnessTool = null;
            CMvdImage inputImage = null;

                // 创建图像清晰度评估算子实例
                imageSharpnessTool = new VisionDesigner.ImageSharpness.CImageSharpnessTool();

                // Set input image
                inputImage = new CMvdImage();
                inputImage = this.BitmapToCMvdImage(inputBitmap);

                imageSharpnessTool.InputImage = inputImage;

                imageSharpnessTool.SetRunParam("SharpnessMode", "SquaredGrad");

                imageSharpnessTool.Run();

                // 获取处理结果
                var Result = imageSharpnessTool.Result;

                return Result.Sharpness;
        }


        public CMvdImage BitmapToCMvdImage(Bitmap bmpInputImg)
        {
            CMvdImage cMvdImage = new CMvdImage();
            System.Drawing.Imaging.PixelFormat bitPixelFormat = bmpInputImg.PixelFormat;
            BitmapData bmData = bmpInputImg.LockBits(new Rectangle(0, 0, bmpInputImg.Width, bmpInputImg.Height), ImageLockMode.ReadOnly, bitPixelFormat);//锁定

            if (bitPixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            {
                Int32 bitmapDataSize = bmData.Stride * bmData.Height;//bitmap图像缓存长度
                int offset = bmData.Stride - bmData.Width;
                Int32 ImageBaseDataSize = bmData.Width * bmData.Height;
                byte[] _BitImageBufferBytes = new byte[bitmapDataSize];
                byte[] _ImageBaseDataBufferBytes = new byte[ImageBaseDataSize];
                Marshal.Copy(bmData.Scan0, _BitImageBufferBytes, 0, bitmapDataSize);
                int bitmapIndex = 0;
                int ImageBaseDataIndex = 0;
                for (int i = 0; i < bmData.Height; i++)
                {
                    for (int j = 0; j < bmData.Width; j++)
                    {
                        _ImageBaseDataBufferBytes[ImageBaseDataIndex++] = _BitImageBufferBytes[bitmapIndex++];
                    }
                    bitmapIndex += offset;
                }
                MVD_IMAGE_DATA_INFO stImageData = new MVD_IMAGE_DATA_INFO();
                stImageData.stDataChannel[0].nRowStep = (uint)bmData.Width;
                stImageData.stDataChannel[0].nLen = (uint)ImageBaseDataSize;
                stImageData.stDataChannel[0].nSize = (uint)ImageBaseDataSize;
                stImageData.stDataChannel[0].arrDataBytes = _ImageBaseDataBufferBytes;
                cMvdImage.InitImage((uint)bmData.Width, (uint)bmData.Height, MVD_PIXEL_FORMAT.MVD_PIXEL_MONO_08, stImageData);
            }
            else if (bitPixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            {
                Int32 bitmapDataSize = bmData.Stride * bmData.Height;//bitmap图像缓存长度
                int offset = bmData.Stride - bmData.Width * 3;
                Int32 ImageBaseDataSize = bmData.Width * bmData.Height * 3;
                byte[] _BitImageBufferBytes = new byte[bitmapDataSize];
                byte[] _ImageBaseDataBufferBytes = new byte[ImageBaseDataSize];
                Marshal.Copy(bmData.Scan0, _BitImageBufferBytes, 0, bitmapDataSize);
                int bitmapIndex = 0;
                int ImageBaseDataIndex = 0;
                for (int i = 0; i < bmData.Height; i++)
                {
                    for (int j = 0; j < bmData.Width; j++)
                    {
                        _ImageBaseDataBufferBytes[ImageBaseDataIndex++] = _BitImageBufferBytes[bitmapIndex + 2];
                        _ImageBaseDataBufferBytes[ImageBaseDataIndex++] = _BitImageBufferBytes[bitmapIndex + 1];
                        _ImageBaseDataBufferBytes[ImageBaseDataIndex++] = _BitImageBufferBytes[bitmapIndex];
                        bitmapIndex += 3;
                    }
                    bitmapIndex += offset;
                }
                MVD_IMAGE_DATA_INFO stImageData = new MVD_IMAGE_DATA_INFO();
                stImageData.stDataChannel[0].nRowStep = (uint)bmData.Width * 3;
                stImageData.stDataChannel[0].nLen = (uint)ImageBaseDataSize;
                stImageData.stDataChannel[0].nSize = (uint)ImageBaseDataSize;
                stImageData.stDataChannel[0].arrDataBytes = _ImageBaseDataBufferBytes;
                cMvdImage.InitImage((uint)bmData.Width, (uint)bmData.Height, MVD_PIXEL_FORMAT.MVD_PIXEL_RGB_RGB24_C3, stImageData);
            }
            bmpInputImg.UnlockBits(bmData);  // 解除锁定
            return cMvdImage;
        }

        private static void ConvertBitmap2MVDImage(Bitmap cBitmapImg, CMvdImage cMvdImg)
        {
            // 参数合法性判断
            if (null == cBitmapImg || null == cMvdImg)
            {
                throw new MvdException(MVD_MODULE_TYPE.MVD_MODUL_APP, VisionDesigner.MVD_ERROR_CODE.MVD_E_PARAMETER_ILLEGAL);
            }

            // 判断像素格式
            if (PixelFormat.Format8bppIndexed != cBitmapImg.PixelFormat && PixelFormat.Format24bppRgb != cBitmapImg.PixelFormat)
            {
                throw new MvdException(MVD_MODULE_TYPE.MVD_MODUL_APP, VisionDesigner.MVD_ERROR_CODE.MVD_E_SUPPORT);
            }

            Int32 nImageWidth = cBitmapImg.Width;
            Int32 nImageHeight = cBitmapImg.Height;
            Int32 nChannelNum = 0;
            BitmapData bitmapData = null;

            try
            {
                // 获取图像信息
                if (PixelFormat.Format8bppIndexed == cBitmapImg.PixelFormat) // 灰度图
                {
                    bitmapData = cBitmapImg.LockBits(new Rectangle(0, 0, nImageWidth, nImageHeight)
                                                                    , ImageLockMode.ReadOnly
                                                                    , PixelFormat.Format8bppIndexed);
                    cMvdImg.InitImage(Convert.ToUInt32(nImageWidth), Convert.ToUInt32(nImageHeight), MVD_PIXEL_FORMAT.MVD_PIXEL_MONO_08);
                    nChannelNum = 1;
                }
                else if (PixelFormat.Format24bppRgb == cBitmapImg.PixelFormat) // 彩色图
                {
                    bitmapData = cBitmapImg.LockBits(new Rectangle(0, 0, nImageWidth, nImageHeight)
                                                                , ImageLockMode.ReadOnly
                                                                , PixelFormat.Format24bppRgb);
                    cMvdImg.InitImage(Convert.ToUInt32(nImageWidth), Convert.ToUInt32(nImageHeight), MVD_PIXEL_FORMAT.MVD_PIXEL_RGB_RGB24_C3);
                    nChannelNum = 3;
                }

                // 考虑图像是否4字节对齐，bitmap要求4字节对齐，而mvdimage不要求对齐
                if (0 == nImageWidth % 4) // 4字节对齐时，直接拷贝
                {
                    Marshal.Copy(bitmapData.Scan0, cMvdImg.GetImageData().stDataChannel[0].arrDataBytes, 0, nImageWidth * nImageHeight * nChannelNum);
                }
                else // 按步长逐行拷贝
                {
                    // 每行实际占用字节数
                    Int32 nRowPixelByteNum = nImageWidth * nChannelNum + 4 - (nImageWidth * nChannelNum % 4);
                    // 每行首字节首地址
                    IntPtr bitmapDataRowPos = IntPtr.Zero;
                    for (int i = 0; i < nImageHeight; i++)
                    {
                        // 获取每行第一个像素值的首地址
                        bitmapDataRowPos = new IntPtr(bitmapData.Scan0.ToInt64() + nRowPixelByteNum * i);
                        Marshal.Copy(bitmapDataRowPos, cMvdImg.GetImageData().stDataChannel[0].arrDataBytes, i * nImageWidth * nChannelNum, nImageWidth * nChannelNum);
                    }
                }

                // bitmap彩色图按BGR存储，而MVDimg按RGB存储，改变存储顺序
                // 交换R和B
                if (PixelFormat.Format24bppRgb == cBitmapImg.PixelFormat)
                {
                    byte bTemp;
                    byte[] bMvdImgData = cMvdImg.GetImageData().stDataChannel[0].arrDataBytes;
                    for (int i = 0; i < nImageWidth * nImageHeight; i++)
                    {
                        bTemp = bMvdImgData[3 * i];
                        bMvdImgData[3 * i] = bMvdImgData[3 * i + 2];
                        bMvdImgData[3 * i + 2] = bTemp;
                    }
                }
            }
            finally
            {
                cBitmapImg.UnlockBits(bitmapData);
            }
        }
    }
}
