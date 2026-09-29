using ImageSourceModuleCs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VM.Core;
using VM.PlatformSDKCS;
using HalconDotNet;

namespace AKRS.Galaxy2.PR.Models.Services
{
    /// <summary>
    /// 图像格式转换工具
    /// </summary>
    public class ImageHelp
    {             
        /// <summary>
        /// Bitmap 转ImageBaseData_V2
        /// </summary>
        /// <param name="bmpInputImg"></param>
        /// <returns></returns>
        public static ImageBaseData_V2 BitmapToImageBaseDataV2(Bitmap bmpInputImg)
        {
            ImageBaseData_V2 imageBaseDataV2 = new ImageBaseData_V2();

            System.Drawing.Imaging.PixelFormat bitPixelFormat = bmpInputImg.PixelFormat;
            BitmapData bmData = bmpInputImg.LockBits(new Rectangle(0, 0, bmpInputImg.Width, bmpInputImg.Height), ImageLockMode.ReadOnly, bitPixelFormat);//锁定
            if (bitPixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            {
                Int32 bitmapDataSize = bmData.Stride * bmData.Height;//bitmap 图像缓存长度
                int offset = bmData.Stride - bmData.Width;
                Int32 ImageBaseDataSize = bmData.Width * bmData.Height;//imageBaseDataV2 图像真正的缓存长度
                byte[]  BitImageBufferBytes = new byte[bitmapDataSize];
                byte[]  ImageBaseDataBufferBytes = new byte[ImageBaseDataSize];
                Marshal.Copy(bmData.Scan0,  BitImageBufferBytes, 0, bitmapDataSize);
                int bitmapIndex = 0;
                int ImageBaseDataIndex = 0;

                for (int i = 0; i < bmData.Height; i++)
                {
                    for (int j = 0; j < bmData.Width; j++)
                    {
                         ImageBaseDataBufferBytes[ImageBaseDataIndex++] =  BitImageBufferBytes[bitmapIndex++];
                    }
                    bitmapIndex += offset;//删除冗余数据
                }
                IntPtr  ImageBaseDataIntptr = Marshal.AllocHGlobal(ImageBaseDataSize);
                Marshal.Copy( ImageBaseDataBufferBytes, 0,
                ImageBaseDataIntptr, ImageBaseDataSize);
                imageBaseDataV2 = new ImageBaseData_V2( ImageBaseDataIntptr, (uint)ImageBaseDataSize,
               bmData.Width, bmData.Height, VMPixelFormat.VM_PIXEL_MONO_08);
                Marshal.FreeHGlobal( ImageBaseDataIntptr);
            }
            else if (bitPixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            {

                Int32 bitmapDataSize = bmData.Stride * bmData.Height;//bitmap 图像缓存长度
                int offset = bmData.Stride - bmData.Width * 3;
                Int32 ImageBaseDataSize = bmData.Width * bmData.Height * 3;//imageBaseDataV2 图像真正的缓存长度
                byte[]  BitImageBufferBytes = new byte[bitmapDataSize];
                byte[]  ImageBaseDataBufferBytes = new byte[ImageBaseDataSize];
                Marshal.Copy(bmData.Scan0,  BitImageBufferBytes, 0, bitmapDataSize);
                int bitmapIndex = 0;
                int ImageBaseDataIndex = 0;
                for (int i = 0; i < bmData.Height; i++)
                {
                    for (int j = 0; j < bmData.Width; j++)
                    {
                         ImageBaseDataBufferBytes[ImageBaseDataIndex++] =  BitImageBufferBytes[bitmapIndex + 2];//bitmap 为 BGR， imageBaseDataV2 为 RGB
                         ImageBaseDataBufferBytes[ImageBaseDataIndex++] =  BitImageBufferBytes[bitmapIndex + 1];

                         ImageBaseDataBufferBytes[ImageBaseDataIndex++] =  BitImageBufferBytes[bitmapIndex];
                        bitmapIndex += 3;
                    }
                    bitmapIndex += offset;
                }
                IntPtr  ImageBaseDataIntptr = Marshal.AllocHGlobal(ImageBaseDataSize);
                Marshal.Copy( ImageBaseDataBufferBytes, 0,  ImageBaseDataIntptr, ImageBaseDataSize);
                imageBaseDataV2 = new ImageBaseData_V2( ImageBaseDataIntptr, (uint)ImageBaseDataSize,
               bmData.Width, bmData.Height, VMPixelFormat.VM_PIXEL_RGB24_C3);
                Marshal.FreeHGlobal( ImageBaseDataIntptr);
            }
            bmpInputImg.UnlockBits(bmData); // 解除锁定
            return imageBaseDataV2;
        }

        /// <summary>
        /// Bitmap转ImageBaseData
        /// </summary>
        /// <param name="bmpInputImg"></param>
        /// <returns></returns>
        public static ImageBaseData BitmapToImageBaseData(Bitmap bmpInputImg)
        {
            ImageBaseData imageBaseData = new ImageBaseData();
            System.Drawing.Imaging.PixelFormat bitPixelFormat =bmpInputImg.PixelFormat;
            BitmapData bmData = bmpInputImg.LockBits(new Rectangle(0, 0, bmpInputImg.Width, bmpInputImg.Height), ImageLockMode.ReadOnly, bitPixelFormat);//锁定
            if (bitPixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            {
                Int32 ImageBaseDataSize = bmData.Width * bmData.Height;
                imageBaseData = new ImageBaseData(bmData.Scan0, (uint)ImageBaseDataSize, bmData.Width, bmData.Height, VMPixelFormat.VM_PIXEL_MONO_08);
            }
            else
            {
                Int32 ImageBaseDataSize = bmData.Width * bmData.Height * 3;
                imageBaseData = new ImageBaseData(bmData.Scan0, (uint)ImageBaseDataSize, bmData.Width, bmData.Height, VMPixelFormat.VM_PIXEL_RGB24_C3);
            }

            bmpInputImg.UnlockBits(bmData);
            return imageBaseData;

            #region 另一种转换方式
            // ImageBaseData imageBaseData = new ImageBaseData(); System.Drawing.Imaging.PixelFormat bitPixelFormat =
            //bmpInputImg.PixelFormat;
            // BitmapData bmData = bmpInputImg.LockBits(new Rectangle(0, 0, bmpInputImg.Width, bmpInputImg.Height), ImageLockMode.ReadOnly, bitPixelFormat);//锁定
            // if (bitPixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            // {
            //     Int32 bitmapDataSize = bmData.Stride * bmData.Height;//bitmap 图像缓存长度
            //     int offset = bmData.Stride - bmData.Width;
            //     Int32 ImageBaseDataSize = bmData.Width * bmData.Height;
            //     byte[]  BitImageBufferBytes = new byte[bitmapDataSize];
            //     byte[]  ImageBaseDataBufferBytes = new byte[ImageBaseDataSize];
            //     Marshal.Copy(bmData.Scan0,  BitImageBufferBytes, 0, bitmapDataSize);

            //     int bitmapIndex = 0;
            //     int ImageBaseDataIndex = 0;
            //     for (int i = 0; i < bmData.Height; i++)
            //     {
            //         for (int j = 0; j < bmData.Width; j++)
            //         {
            //              ImageBaseDataBufferBytes[ImageBaseDataIndex++] =  BitImageBufferBytes[bitmapIndex++];
            //         }
            //         bitmapIndex += offset;
            //     }
            //     imageBaseData = new ImageBaseData( ImageBaseDataBufferBytes, (uint)ImageBaseDataSize, bmData.Width, bmData.Height, (int)VMPixelFormat.VM_PIXEL_MONO_08);
            // }
            // else if (bitPixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            // {
            //     Int32 bitmapDataSize = bmData.Stride * bmData.Height;//bitmap 图像缓存长度

            //     int offset = bmData.Stride - bmData.Width * 3;
            //     Int32 ImageBaseDataSize = bmData.Width * bmData.Height * 3;
            //     byte[]  BitImageBufferBytes = new byte[bitmapDataSize];
            //     byte[]  ImageBaseDataBufferBytes = new byte[ImageBaseDataSize];
            //     Marshal.Copy(bmData.Scan0,  BitImageBufferBytes, 0, bitmapDataSize);
            //     int bitmapIndex = 0;
            //     int ImageBaseDataIndex = 0;
            //     for (int i = 0; i < bmData.Height; i++)
            //     {
            //         for (int j = 0; j < bmData.Width; j++)
            //         {
            //              ImageBaseDataBufferBytes[ImageBaseDataIndex++] =  BitImageBufferBytes[bitmapIndex + 2];
            //              ImageBaseDataBufferBytes[ImageBaseDataIndex++] =  BitImageBufferBytes[bitmapIndex + 1];
            //              ImageBaseDataBufferBytes[ImageBaseDataIndex++] =  BitImageBufferBytes[bitmapIndex];
            //             bitmapIndex += 3;
            //         }
            //         bitmapIndex += offset;
            //     }
            //     imageBaseData = new ImageBaseData( ImageBaseDataBufferBytes, (uint)ImageBaseDataSize, bmData.Width, bmData.Height, (int)VMPixelFormat.VM_PIXEL_RGB24_C3);
            // }
            // bmpInputImg.UnlockBits(bmData); 
            // return imageBaseData; 
            #endregion
        }

        /// <summary>
        /// ImageBaseData_V2转Bitmap
        /// </summary>
        /// <param name="imageBaseDataV2"></param>
        /// <returns></returns>
        public static Bitmap ImageBaseDataV2ToBitmap(ImageBaseData_V2 imageBaseDataV2)
        {
            Bitmap bmpInputImg = null;
            byte[] buffer = new byte[imageBaseDataV2.DataLen];
            Marshal.Copy(imageBaseDataV2.ImageData, buffer, 0, buffer.Length);
            if (VMPixelFormat.VM_PIXEL_MONO_08 == imageBaseDataV2.Pixelformat)
            {
                Int32 imageWidth = Convert.ToInt32(imageBaseDataV2.Width);
                Int32 imageHeight = Convert.ToInt32(imageBaseDataV2.Height);
                System.Drawing.Imaging.PixelFormat bitMaPixelFormat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
                bmpInputImg = new Bitmap(imageWidth, imageHeight, bitMaPixelFormat);

                int offset = imageWidth % 4 != 0 ? (4 - imageWidth % 4) : 0;//添 加冗余位，变成 4 的倍数
                int strid = imageWidth + offset;
                int bitmapBytesLenth = strid * imageHeight;
                byte[] bitmapDataBytes = new byte[bitmapBytesLenth];
                for (int i = 0; i < imageHeight; i++)
                {
                    for (int j = 0; j < strid; j++)
                    {
                        int bitIndex = i * strid + j;
                        int mvdIndex = i * imageWidth + j;
                        if (j >= imageWidth)
                        {
                            bitmapDataBytes[bitIndex] = 0;//冗余位填充 0
                        }
                        else
                        {
                            bitmapDataBytes[bitIndex] = buffer[mvdIndex];
                        }
                    }
                }

                BitmapData bitmapData = bmpInputImg.LockBits(new Rectangle(0, 0, imageWidth, imageHeight), ImageLockMode.WriteOnly, bitMaPixelFormat);
                IntPtr imageBufferPtr = bitmapData.Scan0;
                Marshal.Copy(bitmapDataBytes, 0, imageBufferPtr, bitmapBytesLenth);
                bmpInputImg.UnlockBits(bitmapData);
                var colorPalettes = bmpInputImg.Palette;
                for (int j = 0; j < 256; j++)
                {
                    colorPalettes.Entries[j] = Color.FromArgb(j, j, j);
                }
                bmpInputImg.Palette = colorPalettes;
            }
            else if (VMPixelFormat.VM_PIXEL_RGB24_C3 == imageBaseDataV2.Pixelformat)
            {
                Int32 imageWidth = Convert.ToInt32(imageBaseDataV2.Width);
                Int32 imageHeight = Convert.ToInt32(imageBaseDataV2.Height);
                System.Drawing.Imaging.PixelFormat bitMaPixelFormat = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
                bmpInputImg = new Bitmap(imageWidth, imageHeight, bitMaPixelFormat);
                int offset = imageWidth % 4 != 0 ? (4 - (imageWidth * 3) % 4) : 0;//添加冗余位，变成 4 的倍数
                int strid = imageWidth * 3 + offset;
                int bitmapBytesLenth = strid * imageHeight;
                byte[] bitmapDataBytes = new byte[bitmapBytesLenth];
                for (int i = 0; i < imageHeight; i++)
                {
                    for (int j = 0; j < imageWidth; j++)
                    {
                        int mvdIndex = i * imageWidth * 3 + j * 3;
                        int bitIndex = i * strid + j * 3;
                        bitmapDataBytes[bitIndex] = buffer[mvdIndex + 2];
                        bitmapDataBytes[bitIndex + 1] = buffer[mvdIndex + 1];
                        bitmapDataBytes[bitIndex + 2] = buffer[mvdIndex];
                    }
                    for (int k = 0; k < offset; k++)
                    {
                        bitmapDataBytes[i * strid + imageWidth * 3 + k] = 0;
                    }
                }
                BitmapData bitmapData = bmpInputImg.LockBits(new Rectangle(0, 0, imageWidth, imageHeight), ImageLockMode.WriteOnly, bitMaPixelFormat);
                IntPtr imageBufferPtr = bitmapData.Scan0;
                Marshal.Copy(bitmapDataBytes, 0, imageBufferPtr, bitmapBytesLenth);
                bmpInputImg.UnlockBits(bitmapData);
            }
            return bmpInputImg;
        }


        /// <summary>
        /// Hobject转Bitmap
        /// </summary>
        /// <param name="image"></param>
        /// <param name="res"></param>
        public static Bitmap HObjectToBitmap(HObject image)
        {
            HOperatorSet.GetImageSize(image, out HTuple Width, out HTuple Height);
            HOperatorSet.InterleaveChannels(image, out HObject interImage, "argb", "match", 255);
            HOperatorSet.GetImagePointer1(interImage, out HTuple pointer, out HTuple type, out HTuple w1, out HTuple h1);
            IntPtr scan = pointer;
            Bitmap bitmap = new Bitmap(w1 / 4, h1, w1, PixelFormat.Format32bppArgb, scan);
            Bitmap bitmap2 = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bitmap2);
            g.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
            bitmap.Dispose();
            return bitmap2;
        }

        public static HObject BitmapToImg(Bitmap bitmap)
        {
            HOperatorSet.GenEmptyObj(out HObject image);
            try
            {
                Rectangle imgRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                BitmapData bitData = bitmap.LockBits(imgRect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
                image.Dispose();
                HOperatorSet.GenImage1(out image, "byte", bitmap.Width, bitmap.Height, bitData.Scan0);
                bitmap.UnlockBits(bitData);
                return image;
            }
            catch (Exception)
            {
                return image = null;
            }
        }
    }
}
