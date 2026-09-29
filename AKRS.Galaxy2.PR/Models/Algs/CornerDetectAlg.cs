using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.PR.Models.MatchResults;
using ImageSourceModuleCs;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VM.Core;
using VM.PlatformSDKCS;
using IMVSBlobFindModuCs;
using HalconDotNet;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using AKRS.Galaxy2.PR.Models.Services;
using GeometryCreateCs;

namespace AKRS.Galaxy2.PR.Models.Algs
{
    /// <summary>
    /// 缺角检测方法,输出图像为Blob框、Blob中心，输出文字为检测结果
    /// </summary>
    [Serializable]
    public class CornerDetectAlg : BaseAlg
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">算法流程名称</param>
        public CornerDetectAlg(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 执行缺角检测流程，获取结果
        /// </summary>
        /// <returns></returns>
        public override bool FindModel()
        {
            try
            {
                if (this.VmProcedureName == null)
                {
                    this.VmProcedureName = this.GetVmProcedure().FullName;
                }
                this.VmProcedure = (VmProcedure)VmSolution.Instance[this.VmProcedureName];

                IMVSBlobFindModuTool iMVSBlobFindModuTool = (IMVSBlobFindModuTool)VmSolution.Instance[$"{this.VmProcedureName}.BLOB分析1"];
                if (iMVSBlobFindModuTool == null)
                {
                    return false;
                }
                int[] Threshold = { iMVSBlobFindModuTool.ModuParams.LowThreshold };
                this.ProcedureParam.SetInputInt("Threshold", Threshold);

                int[] Area = { iMVSBlobFindModuTool.ModuParams.MinArea };
                this.ProcedureParam.SetInputInt("Area", Area);
         
                this.MatchResults = new List<BaseAlgResult>();
                this.VmProcedure.Run();
                ImageBaseData saveImageResult = SaveImageTool.ModuResult.OutputImage;
                ProcedureResult = this.VmProcedure.ModuResult;

                FloatDataArray Centerx = ProcedureResult.GetOutputFloat("ResX");
                float[] xResults = Centerx.pFloatVal == null ? null : Centerx.pFloatVal;

                FloatDataArray Centery = ProcedureResult.GetOutputFloat("ResY");
                float[] yResults = Centery.pFloatVal == null ? null : Centery.pFloatVal;

                StringDataArray Angle = ProcedureResult.GetOutputString("ResString");
                StringData[] sResults = Angle.astStringVal == null ? null : Angle.astStringVal;


                #region Bitmap
                //GeometryCreateTool geometryCreateTool = (GeometryCreateTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.几何创建1"];
                //GeometryCreateResult GP = geometryCreateTool.ModuResult;

                //HObject regionZoom, imageResult1, imageResult2, imageResult3, imageResult;
                //HOperatorSet.GenEmptyObj(out regionZoom);
                //HOperatorSet.GenEmptyObj(out imageResult1);
                //HOperatorSet.GenEmptyObj(out imageResult2);
                //HOperatorSet.GenEmptyObj(out imageResult3);
                //HOperatorSet.GenEmptyObj(out imageResult);
                //Bitmap bitmap, bitmapResult;
                //Graphics graph;

                //if (xResults != null&& xResults.Count() != 0)
                //{
                //    #region 绘制图像
                //    HObject recRegion;
                //    HOperatorSet.GenEmptyObj(out recRegion);

                //    HTuple a1 = new HTuple(), r1 = new HTuple(), c1 = new HTuple();
                //    StringBuilder stringBuilder = new StringBuilder();

                //    List<RectBox> box = GP.ListOutputResultBox;

                //    for (int i = 0; i < GP.BoxNum; i++)
                //    {

                //        RectBox rectBox = box[i];
                //        HTuple angle = rectBox.Angle;
                //        HOperatorSet.GenRectangle2(out recRegion, rectBox.CenterPoint.Y, rectBox.CenterPoint.X, -angle.TupleRad(), rectBox.BoxWidth / 2, rectBox.BoxHeight / 2);
                //        HOperatorSet.ZoomRegion(recRegion, out recRegion, 0.25, 0.25);
                //        HOperatorSet.AreaCenter(recRegion, out a1, out r1, out c1);
                //        HOperatorSet.ConcatObj(regionZoom, recRegion, out regionZoom);

                //    }
                //    recRegion.Dispose();
                //    stringBuilder.Append("检测结果: true" + "\n");

                //    // 将结果画在三通道压缩图像上并传出
                //    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult1, 0, "margin");
                //    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult2, 255, "margin");
                //    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult3, 0, "margin");
                //    HOperatorSet.Compose3(imageResult1, imageResult2, imageResult3, out imageResult);

                //    // 绘制文字
                //    bitmap = ImageHelp.HObjectToBitmap(imageResult);
                //    bitmapResult = new Bitmap(bitmap);
                //    graph = Graphics.FromImage(bitmapResult);
                //    graph.DrawString(stringBuilder.ToString(), new Font("宋体", 10), new SolidBrush(Color.Red), new Point(0, 0));
                //    bitmap.Dispose();
                ////    bitmapResult.Save("D:\\14.bmp");
                //    #endregion
                //    for (int i = 0; i < xResults.Length; i++)
                //    {
                //        this.MatchResults.Add(new MatchResult()
                //        {
                //            CenterX = xResults[i],
                //            CenterY = yResults[i],
                //            IsSuccess = true,
                //            OutPutImg1 = bitmapResult,
                //        });
                //    }
                //    return true;
                //}
                //else
                //{
                //    #region 绘制检测区域
                //    string MarkedWord = "检测结果: false";
                //    HOperatorSet.Compose3(this.SourceImg, this.SourceImg, this.SourceImg, out imageResult);
                //    bitmap = ImageHelp.HObjectToBitmap(imageResult);
                //    bitmapResult = new Bitmap(bitmap);
                //    graph = Graphics.FromImage(bitmapResult);
                //    graph.DrawString(MarkedWord, new Font("宋体", 10), new SolidBrush(Color.Green), new Point(0, 0));
                //    bitmap.Dispose();
                //    #endregion

                //    this.MatchResults.Add(new MatchResult()
                //    {
                //        IsSuccess = false,
                //        OutPutImg1 = bitmapResult,
                //        MarkedWords = MarkedWord,
                //    });
                //}
                //graph.Dispose();
                ////bitmapResult.Save("D:\\15.bmp");
                //regionZoom.Dispose();
                //imageResult1.Dispose();
                //imageResult2.Dispose();
                //imageResult3.Dispose();
                //imageResult.Dispose(); 
                #endregion

                #region ImageBaseData
                if (sResults != null&&sResults[0].strValue.ToString() == "true")
                {
                    for (int i = 0; i < xResults.Length; i++)
                    {
                        this.MatchResults.Add(new MatchResult()
                        {
                            CenterX = xResults[i],
                            CenterY = yResults[i],
                            IsSuccess = true,
                            OutPutImg = saveImageResult,
                        });
                    }
                }
                else
                {
                    this.MatchResults.Add(new MatchResult()
                    {
                        IsSuccess = false,
                        OutPutImg = saveImageResult,
                    });
                } 
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"{this.Name}: CornerDetectAlg 异常：{ex.Message} ", ex, Log.LogCategory.PR);
                return false;
            }
        }

        

    }
}
