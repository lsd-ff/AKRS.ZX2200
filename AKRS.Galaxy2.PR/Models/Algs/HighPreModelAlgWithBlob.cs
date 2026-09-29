using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.PR.Models.Algs;
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
using AKRS.Galaxy2.PR.Models.MatchResults;
using VM.Core;
using VM.PlatformSDKCS;
using IMVSFixtureModuCs;
using IMVSHPFeatureMatchModuCs;
using VMControls.Interface;
using AKRS.Galaxy2.PR.Models.Services;
using HalconDotNet;
using IMVSBlobFindModuCs;

namespace AKRSAKRS.Galaxy2.PR.Models.Algs
{
    /// <summary>
    /// 带墨点检测的高精度匹配方法，输出定位框和Blob区域、中心点
    /// </summary>
    [Serializable]
    public class HighPreModelAlgWithBlob : BaseAlg
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">算法流程名称</param>
        public HighPreModelAlgWithBlob(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 执行带墨点检测的高精度匹配流程，获取结果
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
                
                this.VmProcedure.Run();
                ProcedureResult = this.VmProcedure.ModuResult;

                FloatDataArray Centerx = ProcedureResult.GetOutputFloat("ReasultX");
                float[] xResults = Centerx.pFloatVal == null ? null : Centerx.pFloatVal;

                FloatDataArray Centery = ProcedureResult.GetOutputFloat("ReasultY");
                float[] yResults = Centery.pFloatVal == null ? null : Centery.pFloatVal;

                FloatDataArray Angle = ProcedureResult.GetOutputFloat("ReasultA");
                float[] angleResults = Angle.pFloatVal == null ? null : Angle.pFloatVal;

                FloatDataArray Score = ProcedureResult.GetOutputFloat("ReasultS");
                float[] scoreResults = Score.pFloatVal == null ? null : Score.pFloatVal;


                #region Bitmap
                IMVSHPFeatureMatchModuTool HPFeatureMatchModuTool = (IMVSHPFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.高精度匹配1"];
                if (HPFeatureMatchModuTool == null)
                {
                    return false;
                }
                HPFeatureMatchResult HP = HPFeatureMatchModuTool.ModuResult;

                IMVSBlobFindModuTool BlobFindModuTool = (IMVSBlobFindModuTool)VmSolution.Instance[$"{this.GetVmProcedure().FullName}.BLOB分析1"];
                BlobFindResult blobRes = BlobFindModuTool.ModuResult;

                HObject regionZoom, imageResult1, imageResult2, imageResult3, imageResult;
                HOperatorSet.GenEmptyObj(out regionZoom);
                HOperatorSet.GenEmptyObj(out imageResult1);
                HOperatorSet.GenEmptyObj(out imageResult2);
                HOperatorSet.GenEmptyObj(out imageResult3);
                HOperatorSet.GenEmptyObj(out imageResult);
                Bitmap bitmap, bitmapResult;
                Graphics graph;

                if (HP.ModuStatus == 1  && xResults!=null&&xResults.Count()!=0)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("匹配框中心:(" + HP.MatchRect[0].CenterPoint.X + "," + HP.MatchRect[0].CenterPoint.Y + ")"  + "\n");
                    stringBuilder.Append( "角度:" + HP.MatchRect[0].Angle + ",分数:" + HP.MatchScore[0] + "\n");
                    #region 绘制图像
                    HObject regionline1, regionline2, region, recRegion;
                    HOperatorSet.GenEmptyObj(out regionline1);
                    HOperatorSet.GenEmptyObj(out regionline2);
                    HOperatorSet.GenEmptyObj(out region);
                    HOperatorSet.GenEmptyObj(out recRegion);

                    HTuple angle = HP.MatchRect[0].Angle;
                    HOperatorSet.GenRectangle2(out recRegion, HP.MatchRect[0].CenterPoint.Y, HP.MatchRect[0].CenterPoint.X, -angle.TupleRad(), HP.MatchRect[0].BoxWidth / 2, HP.MatchRect[0].BoxHeight / 2);
                    HOperatorSet.ZoomRegion(recRegion, out regionZoom, 0.25, 0.25);
                    recRegion.Dispose();
                    if (blobRes.ModuStatus == 0)
                    {
                        // 绘制中心线
                        HTuple a1 = new HTuple(), r1 = new HTuple(), c1 = new HTuple();
                        for (int i = 0; i < blobRes.CentroidPoint.Count; i++)
                        {
                            r1 = blobRes.CentroidPoint[i].Y / 4;
                            c1 = blobRes.CentroidPoint[i].X / 4;
                            a1 = blobRes.Area[i];

                            HOperatorSet.GenRegionLine(out regionline1, r1 - 5, c1, r1 + 5, c1);
                            HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);
                            HOperatorSet.GenRegionLine(out regionline2, r1, c1 - 5, r1, c1 + 5);
                            HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);
                            HOperatorSet.ConcatObj(regionZoom, regionline2, out regionZoom);

                            stringBuilder.Append("质心点:(" + blobRes.CentroidPoint[i].X + "," + blobRes.CentroidPoint[i].Y + "),面积:" + blobRes.Area[i] + "\n");
                        }

                        // 轮廓点

                        var outlinePoints = blobRes.ContourInfo.MatchOutlinePoints;
                        List<HTuple> xList = new List<HTuple>();
                        List<HTuple> yList = new List<HTuple>();

                        for (int i = 0; i < outlinePoints.Count; i += 4)
                        {
                            xList.Add((HTuple)outlinePoints[i].MatchOutlineX / 4);
                            yList.Add((HTuple)outlinePoints[i].MatchOutlineY / 4);
                        }

                        HTuple PointX = new HTuple(xList.ToArray());
                        HTuple PointY = new HTuple(yList.ToArray());


                     
                        HOperatorSet.GenRegionPoints(out region, PointY, PointX);
                        HOperatorSet.ConcatObj(regionZoom, region, out regionZoom);

                    }
                    // 将结果画在三通道压缩图像上并传出
                    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult1, 0, "margin");
                    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult2, 255, "margin");
                    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult3, 0, "margin");
                    HOperatorSet.Compose3(imageResult1, imageResult2, imageResult3, out imageResult);

                    // 绘制文字
                    bitmap = ImageHelp.HObjectToBitmap(imageResult);
                    bitmapResult = new Bitmap(bitmap);
                    graph = Graphics.FromImage(bitmapResult);
                    graph.DrawString(stringBuilder.ToString(), new Font("宋体", 15), new SolidBrush(Color.LightGreen), new Point(0, 0));

                    bitmap.Dispose();
                    regionline1.Dispose();
                    regionline2.Dispose();
                    region.Dispose();
                    #endregion

                    if (blobRes.ModuStatus == 0)
                    {
                        for (int i = 0; i < xResults.Length; i++)
                        {
                            this.MatchResults.Add(new MatchResult()
                            {
                                CenterX = xResults[i],
                                CenterY = yResults[i],
                                Angle = angleResults[i],
                                Score = scoreResults[i],
                                IsSuccess = false,
                                OutPutImg1 = bitmapResult,
                            });
                        }
                    }
                    else
                    {
                        this.MatchResults = new List<BaseAlgResult>();
                        for (int i = 0; i < xResults.Length; i++)
                        {
                            this.MatchResults.Add(new MatchResult()
                            {
                                CenterX = xResults[i],
                                CenterY = yResults[i],
                                Angle = angleResults[i],
                                Score = scoreResults[i],
                                IsSuccess = true,
                                OutPutImg1 = bitmapResult,
                            });
                        }
                    }
                    
                }
                else
                {
                    string MarkedWord = "";
                    HObject tempRegion, zoomRegion;
                    HOperatorSet.GenEmptyObj(out tempRegion);
                    HOperatorSet.GenEmptyObj(out zoomRegion);
                    if (HP.ModuStatus == 0)
                    {
                        MarkedWord = "模板 - " + this.Name + " 粗定位模块失败。";
                        HTuple angle1 = new HTuple();
                        angle1 = HP.ROI.Angle;
                        HOperatorSet.GenRectangle2(out tempRegion, HP.ROI.CenterPoint.Y, HP.ROI.CenterPoint.X, -angle1.TupleRad(), HP.ROI.BoxWidth / 2, HP.ROI.BoxHeight / 2);
                        HOperatorSet.ZoomRegion(tempRegion, out zoomRegion, 0.25, 0.25);
                        HOperatorSet.ConcatObj(regionZoom, zoomRegion, out regionZoom);
                    }
                    else
                    {
                        MarkedWord = "模板 - " + this.Name + " Blob检测模块失败。";
                    }
                    #region 绘制检测区域
                    HTuple angle = new HTuple();
                    for (int j = 0; j < blobRes.ROI.Count; j++)
                    {
                        HOperatorSet.GenEmptyObj(out tempRegion);
                        HOperatorSet.GenEmptyObj(out zoomRegion);
                        angle = blobRes.ROI[j].Angle;

                        HOperatorSet.GenRectangle2(out tempRegion, blobRes.ROI[j].CenterPoint.Y, blobRes.ROI[j].CenterPoint.X, -angle.TupleRad(), blobRes.ROI[j].BoxWidth / 2, blobRes.ROI[j].BoxHeight / 2);
                        HOperatorSet.ZoomRegion(tempRegion, out zoomRegion, 0.25, 0.25);
                        HOperatorSet.ConcatObj(regionZoom, zoomRegion, out regionZoom);

                    }
                    tempRegion.Dispose();
                    zoomRegion.Dispose();

                    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult1, 255, "margin");
                    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult2, 0, "margin");
                    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult3, 0, "margin");
                    HOperatorSet.Compose3(imageResult1, imageResult2, imageResult3, out imageResult);
                    bitmap = ImageHelp.HObjectToBitmap(imageResult);
                    bitmapResult = new Bitmap(bitmap);
                    graph = Graphics.FromImage(bitmapResult);
                    graph.DrawString(MarkedWord, new Font("宋体", 15), new SolidBrush(Color.Red), new Point(0, 0));
                    bitmap.Dispose();
                    #endregion

                    this.MatchResults.Add(new MatchResult()
                    {
                        IsSuccess = false,
                        OutPutImg1 = bitmapResult,
                        MarkedWords = MarkedWord,
                    });
                }
                graph.Dispose();
                regionZoom.Dispose();
                imageResult1.Dispose();
                imageResult2.Dispose();
                imageResult3.Dispose();
                imageResult.Dispose();
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"{this.Name}: HighPreModelAlgWithBlob异常：{ex.Message} ", ex, LogCategory.PR);
                return false;
            }

            
        }
    }
}
