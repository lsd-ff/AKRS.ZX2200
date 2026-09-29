using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using VM.Core;
using BranchModule_STDCs;
using ImageSourceModuleCs;
using IMVSFastFeatureMatchModuCs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using VM.PlatformSDKCS;
using AKRS.Galaxy2.Log;
using log4net.Core;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Models.CommonModels;
using IMVSFixtureModuCs;
using System.Diagnostics;
using AKRS.Galaxy2.PR.Models.Services;
using HalconDotNet;
using IMVSContourMatchModuCs;
using IMVSGrayMatchModuVACs;
using IMVSHPFeatureMatchModuCs;
using IMVSQuadrangleFindModuCs;
using ShellModuleCs;

namespace AKRS.Galaxy2.PR.Models.Algs
{


    /// <summary>
    /// 矩形检测方法，输出图像为四条边、对角线，输出文字为匹配框中心坐标、矩形对角线交点
    /// </summary>
    [Serializable]
    public class RectangleDetectAlg : BaseAlg
    {
        /// <summary>
        /// 粗定位方式
        /// </summary>
        public int[] CrudePRType = new int[1];

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        public RectangleDetectAlg(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 执行矩形检测流程，获取结果
        /// </summary>
        /// <returns>结果</returns>
        public override bool FindModel()
        {
            try
            {
                if (this.VmProcedureName == null)
                {
                    this.VmProcedureName = this.GetVmProcedure().FullName;
                }
                this.VmProcedure = (VmProcedure)VmSolution.Instance[this.VmProcedureName];
                this.MatchResults = new List<BaseAlgResult>();

                CrudePRType[0] = (int)CrudeLocateType;
                this.ProcedureParam.SetInputInt("FirPRType", CrudePRType);

                this.VmProcedure.Run();
                ProcedureResult = this.VmProcedure.ModuResult;

                FloatDataArray Centerx = ProcedureResult.GetOutputFloat("1ReasultX");
                float[] xResults = Centerx.pFloatVal == null ? null : Centerx.pFloatVal;

                FloatDataArray Centery = ProcedureResult.GetOutputFloat("1ReasultY");
                float[] yResults = Centery.pFloatVal == null ? null : Centery.pFloatVal;


                FloatDataArray Angle = ProcedureResult.GetOutputFloat("1ReasultA");
                float[] angleResults = Angle.pFloatVal == null ? null : Angle.pFloatVal;
                
                #region Bitmap
                IMVSQuadrangleFindModuTool QuadrangleFindModuTool = (IMVSQuadrangleFindModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.四边形查找1"];
                if (QuadrangleFindModuTool == null)
                {
                    return false;
                }
                QuadrangleFindResult QuadrangleRes = QuadrangleFindModuTool.ModuResult;

                HObject regionZoom, imageResult1, imageResult2, imageResult3, imageResult;
                HOperatorSet.GenEmptyObj(out regionZoom);
                HOperatorSet.GenEmptyObj(out imageResult1);
                HOperatorSet.GenEmptyObj(out imageResult2);
                HOperatorSet.GenEmptyObj(out imageResult3);
                HOperatorSet.GenEmptyObj(out imageResult);
                Bitmap bitmap, bitmapResult;
                Graphics graph;

                if (xResults != null && xResults.Count() != 0)
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    switch (CrudePRType[0])
                    {
                        case 0:
                            IMVSFastFeatureMatchModuTool iMVSFastFeatureMatchModuTool = (IMVSFastFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.快速匹配1"];
                            FastFeatureMatchResult FastRes = iMVSFastFeatureMatchModuTool.ModuResult;
                            stringBuilder.Append("快速匹配框中心:(" + FastRes.MatchRect[0].CenterPoint.X + "," + FastRes.MatchRect[0].CenterPoint.Y + ")" +  "\n");
                            stringBuilder.Append("角度:" + FastRes.MatchRect[0].Angle + ",分数:" + FastRes.MatchScore[0] + "\n");
                            break;
                        case 1:
                            IMVSHPFeatureMatchModuTool iMVSHPFeatureMatchModuTool = (IMVSHPFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.高精度匹配1"];
                            HPFeatureMatchResult HPRes = iMVSHPFeatureMatchModuTool.ModuResult;
                            stringBuilder.Append("高精度匹配框中心:(" + HPRes.MatchRect[0].CenterPoint.X + "," + HPRes.MatchRect[0].CenterPoint.Y  +")" + "\n");
                            stringBuilder.Append("角度:" + HPRes.MatchRect[0].Angle + ",分数:" + HPRes.MatchScore[0] + "\n");
                            break;
                        case 2:
                            IMVSGrayMatchModuVATool iMVSGrayMatchModuVATool = (IMVSGrayMatchModuVATool)VmSolution.Instance[$"{this.VmProcedure.FullName}.灰度匹配1"];
                            GrayMatchVAResult GrayRes = iMVSGrayMatchModuVATool.ModuResult;
                            stringBuilder.Append("灰度匹配框中心:(" + GrayRes.MatchRect[0].CenterPoint.X + "," + GrayRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                            stringBuilder.Append("角度:" + GrayRes.MatchRect[0].Angle + ",分数:" + GrayRes.MatchScore[0] + "\n");
                            break;
                        case 3:
                            IMVSContourMatchModuTool iMVSContourMatchModuTool = (IMVSContourMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.轮廓匹配1"];
                            ContourMatchResult ContourRes = iMVSContourMatchModuTool.ModuResult;
                            stringBuilder.Append("轮廓匹配框中心:(" + ContourRes.MatchRect[0].CenterPoint.X + "," + ContourRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                            stringBuilder.Append("角度:" + ContourRes.MatchRect[0].Angle + ",分数:" + ContourRes.MatchScore[0] + "\n");
                            break;
                    }


                    #region 绘制图像
                    HObject regionline1, regionline2, line1, line2, line3, line4, recRegion;
                    HOperatorSet.GenEmptyObj(out regionline1);
                    HOperatorSet.GenEmptyObj(out regionline2);
                    HOperatorSet.GenEmptyObj(out line1);
                    HOperatorSet.GenEmptyObj(out line2);
                    HOperatorSet.GenEmptyObj(out line3);
                    HOperatorSet.GenEmptyObj(out line4);
                    HOperatorSet.GenEmptyObj(out recRegion);

                    HTuple a1 = new HTuple(), r1 = new HTuple(), c1 = new HTuple();

                    // 绘制线
                    HOperatorSet.GenRegionLine(out line1, QuadrangleRes.EdgeLine1.StartPoint.Y / 4, QuadrangleRes.EdgeLine1.StartPoint.X / 4, QuadrangleRes.EdgeLine1.EndPoint.Y / 4, QuadrangleRes.EdgeLine1.EndPoint.X / 4);
                    HOperatorSet.ConcatObj(regionZoom, line1, out regionZoom);
                    HOperatorSet.GenRegionLine(out line2, QuadrangleRes.EdgeLine2.StartPoint.Y / 4, QuadrangleRes.EdgeLine2.StartPoint.X / 4, QuadrangleRes.EdgeLine2.EndPoint.Y / 4, QuadrangleRes.EdgeLine2.EndPoint.X / 4);
                    HOperatorSet.ConcatObj(regionZoom, line2, out regionZoom);
                    HOperatorSet.GenRegionLine(out line3, QuadrangleRes.EdgeLine3.StartPoint.Y / 4-1, QuadrangleRes.EdgeLine3.StartPoint.X / 4, QuadrangleRes.EdgeLine3.EndPoint.Y / 4-1, QuadrangleRes.EdgeLine3.EndPoint.X / 4);
                    HOperatorSet.ConcatObj(regionZoom, line3, out regionZoom);
                    HOperatorSet.GenRegionLine(out line4, QuadrangleRes.EdgeLine4.StartPoint.Y / 4, QuadrangleRes.EdgeLine4.StartPoint.X / 4, QuadrangleRes.EdgeLine4.EndPoint.Y / 4, QuadrangleRes.EdgeLine4.EndPoint.X / 4);
                    HOperatorSet.ConcatObj(regionZoom, line4, out regionZoom);
                    line1.Dispose();
                    line2.Dispose();
                    line3.Dispose();
                    line4.Dispose();


                    // 绘制中点
                    r1 = QuadrangleRes.CentralPoint.Y / 4;
                    c1 = QuadrangleRes.CentralPoint.X / 4;
                    HOperatorSet.GenRegionLine(out regionline1, r1 - 5, c1, r1 + 5, c1);
                    HOperatorSet.GenRegionLine(out regionline2, r1, c1 - 5, r1, c1 + 5);
                    HOperatorSet.ConcatObj(regionZoom, regionline2, out regionZoom);
                    HOperatorSet.ConcatObj(regionZoom, recRegion, out regionZoom);
                    HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);

                    stringBuilder.Append("四边形中心点:(" + QuadrangleRes.CentralPoint.X + "," + QuadrangleRes.CentralPoint.Y + ")"  + "\n");
                    recRegion.Dispose();

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

                    #endregion
                    for (int i = 0; i < xResults.Length; i++)
                    {
                        this.MatchResults.Add(new MatchResult()
                        {
                            CenterX = xResults[i],
                            CenterY = yResults[i],
                            Angle = angleResults[i],
                            IsSuccess = true,
                            OutPutImg1 = bitmapResult,
                        });
                    }
                    graph.Dispose();
                    regionZoom.Dispose();
                    imageResult1.Dispose();
                    imageResult2.Dispose();
                    imageResult3.Dispose();
                    imageResult.Dispose();
                    return true;
                }
                else
                {
                    string MarkedWord = "";

                    IMVSFixtureModuTool iMVSFixtureModuTool = (IMVSFixtureModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.位置修正1"];

                    if (iMVSFixtureModuTool.ModuResult.ModuStatus == 0)
                    {
                        MarkedWord = "模板 - " + this.Name + " 粗定位模块失败。";
                        RectBox MatchRectROI = new RectBox();
                        switch (CrudePRType[0])
                        {
                            case 0:
                                IMVSFastFeatureMatchModuTool iMVSFastFeatureMatchModuTool = (IMVSFastFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.快速匹配1"];
                                FastFeatureMatchResult FastRes = iMVSFastFeatureMatchModuTool.ModuResult;
                                MatchRectROI = FastRes.ROI;
                                break;
                            case 1:
                                IMVSHPFeatureMatchModuTool iMVSHPFeatureMatchModuTool = (IMVSHPFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.高精度匹配1"];
                                HPFeatureMatchResult HPRes = iMVSHPFeatureMatchModuTool.ModuResult;
                                MatchRectROI = HPRes.ROI;
                                break;
                            case 2:
                                IMVSGrayMatchModuVATool iMVSGrayMatchModuVATool = (IMVSGrayMatchModuVATool)VmSolution.Instance[$"{this.VmProcedure.FullName}.灰度匹配1"];
                                GrayMatchVAResult GrayRes = iMVSGrayMatchModuVATool.ModuResult;
                                MatchRectROI = GrayRes.ROI;
                                break;
                            case 3:
                                IMVSContourMatchModuTool iMVSContourMatchModuTool = (IMVSContourMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.轮廓匹配1"];
                                ContourMatchResult ContourRes = iMVSContourMatchModuTool.ModuResult;
                                MatchRectROI = ContourRes.ROI;
                                break;
                        }
                        HTuple angle5 = MatchRectROI.Angle;
                        HOperatorSet.GenRectangle2(out regionZoom, MatchRectROI.CenterPoint.Y, MatchRectROI.CenterPoint.X, -angle5.TupleRad(), MatchRectROI.BoxWidth / 2, MatchRectROI.BoxHeight / 2);
                    }
                    else
                    {
                        MarkedWord = "模板 - " + this.Name + " 矩形检测模块失败。";
                    }

                    #region 绘制检测区域
                    HObject ROI1, ROI2, ROI3, ROI4;
                    HOperatorSet.GenEmptyObj(out ROI1);
                    HOperatorSet.GenEmptyObj(out ROI2);
                    HOperatorSet.GenEmptyObj(out ROI3);
                    HOperatorSet.GenEmptyObj(out ROI4);

                    HTuple angle1 = QuadrangleRes.ROI1.Angle;
                    HOperatorSet.GenRectangle2(out ROI1, QuadrangleRes.ROI1.CenterPoint.Y, QuadrangleRes.ROI1.CenterPoint.X, -angle1.TupleRad(), QuadrangleRes.ROI1.BoxWidth / 2, QuadrangleRes.ROI1.BoxHeight / 2);
                    HOperatorSet.ConcatObj(regionZoom, ROI1, out regionZoom);
                    HTuple angle2 = QuadrangleRes.ROI2.Angle;
                    HOperatorSet.GenRectangle2(out ROI2, QuadrangleRes.ROI2.CenterPoint.Y, QuadrangleRes.ROI2.CenterPoint.X, -angle2.TupleRad(), QuadrangleRes.ROI2.BoxWidth / 2, QuadrangleRes.ROI2.BoxHeight / 2);
                    HOperatorSet.ConcatObj(regionZoom, ROI2, out regionZoom);
                    HTuple angle3 = QuadrangleRes.ROI3.Angle;
                    HOperatorSet.GenRectangle2(out ROI3, QuadrangleRes.ROI3.CenterPoint.Y, QuadrangleRes.ROI3.CenterPoint.X, -angle3.TupleRad(), QuadrangleRes.ROI3.BoxWidth / 2, QuadrangleRes.ROI3.BoxHeight / 2);
                    HOperatorSet.ConcatObj(regionZoom, ROI3, out regionZoom);
                    HTuple angle4 = QuadrangleRes.ROI4.Angle;
                    HOperatorSet.GenRectangle2(out ROI4, QuadrangleRes.ROI4.CenterPoint.Y, QuadrangleRes.ROI4.CenterPoint.X, -angle4.TupleRad(), QuadrangleRes.ROI4.BoxWidth / 2, QuadrangleRes.ROI4.BoxHeight / 2);
                    HOperatorSet.ConcatObj(regionZoom, ROI4, out regionZoom);
                    ROI1.Dispose();
                    ROI2.Dispose();
                    ROI3.Dispose();
                    ROI4.Dispose();

                    HOperatorSet.ZoomRegion(regionZoom, out regionZoom, 0.25, 0.25);
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

                return false;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"{this.Name}: RectangleDetectAlg 异常：{ex.Message} ", ex, Log.LogCategory.PR);
                return false;
            }
        }
    }
}
