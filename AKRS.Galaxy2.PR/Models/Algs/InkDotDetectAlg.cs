using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Models.Services;
using HalconDotNet;
using ImageSourceModuleCs;
using IMVSBlobFindModuCs;
using IMVSContourMatchModuCs;
using IMVSFastFeatureMatchModuCs;
using IMVSFixtureModuCs;
using IMVSGrayMatchModuVACs;
using IMVSHPFeatureMatchModuCs;
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

namespace AKRS.Galaxy2.PR.Models.Algs
{
    /// <summary>
    /// 墨点检测方法，输出图像为中心和轮廓点，输出文字为中心坐标和面积
    /// </summary>
    [Serializable]
    public class InkDotDetectAlg : BaseAlg
    {
        /// <summary>
        /// 是否采用粗定位
        /// </summary>
        public int[] IsUseCrudePR = new int[1];

        /// <summary>
        /// 粗定位方式
        /// </summary>
        public int[] CrudePRType = new int[1];

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">算法流程名称</param>
        public InkDotDetectAlg(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 执行墨点检测流程，获取结果
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
                this.MatchResults = new List<BaseAlgResult>();
                IsUseCrudePR[0] = Convert.ToInt32(IsUseCrudeLocate);
                this.ProcedureParam.SetInputInt("IsFirPR", IsUseCrudePR);

                #region Bitmap
                IMVSBlobFindModuTool BlobFindModuTool;
                if (IsUseCrudePR[0] == 1)
                {
                    CrudePRType[0] = (int)CrudeLocateType;
                    this.ProcedureParam.SetInputInt("FirPRType", CrudePRType);
                    BlobFindModuTool = (IMVSBlobFindModuTool)VmSolution.Instance[$"{this.GetVmProcedure().FullName}.BLOB分析1"];
                }
                else
                {
                    BlobFindModuTool = (IMVSBlobFindModuTool)VmSolution.Instance[$"{this.GetVmProcedure().FullName}.BLOB分析2"];
                }
                if (BlobFindModuTool == null)
                {
                    return false;
                }
                this.VmProcedure.Run();
                ProcedureResult = this.VmProcedure.ModuResult;
                BlobFindResult blobRes = BlobFindModuTool.ModuResult;
                float[] xResults;
                float[] yResults;
                float[] areaResults;
                int[] Result;
                int[] NumResults;

                if (IsUseCrudeLocate)
                {
                    FloatDataArray Centerx = ProcedureResult.GetOutputFloat("1BlobCenterX");
                    xResults = Centerx.pFloatVal == null ? null : Centerx.pFloatVal;

                    FloatDataArray Centery = ProcedureResult.GetOutputFloat("1BlobCenterY");
                    yResults = Centery.pFloatVal == null ? null : Centery.pFloatVal;

                    FloatDataArray Area = ProcedureResult.GetOutputFloat("1BlobArea");
                    areaResults = Area.pFloatVal == null ? null : Area.pFloatVal;

                    IntDataArray Reasult = ProcedureResult.GetOutputInt("1BlobResult");
                    Result = Reasult.pIntVal == null ? null : Reasult.pIntVal;

                    IntDataArray Num = ProcedureResult.GetOutputInt("1BlobNum");
                    NumResults = Num.pIntVal == null ? null : Num.pIntVal;
                }
                else
                {
                    FloatDataArray Centerx = ProcedureResult.GetOutputFloat("2BlobCenterX");
                    xResults = Centerx.pFloatVal == null ? null : Centerx.pFloatVal;

                    FloatDataArray Centery = ProcedureResult.GetOutputFloat("2BlobCenterY");
                    yResults = Centery.pFloatVal == null ? null : Centery.pFloatVal;

                    FloatDataArray Area = ProcedureResult.GetOutputFloat("2BlobArea");
                    areaResults = Area.pFloatVal == null ? null : Area.pFloatVal;

                    IntDataArray Reasult = ProcedureResult.GetOutputInt("2BlobResult");
                    Result = Reasult.pIntVal == null ? null : Reasult.pIntVal;

                    IntDataArray Num = ProcedureResult.GetOutputInt("2BlobNum");
                    NumResults = Num.pIntVal == null ? null : Num.pIntVal;
                }

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
                    if (IsUseCrudePR[0] == 1)
                    {
                        switch (CrudePRType[0])
                        {
                            case 0:
                                IMVSFastFeatureMatchModuTool iMVSFastFeatureMatchModuTool = (IMVSFastFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.快速匹配1"];
                                FastFeatureMatchResult FastRes = iMVSFastFeatureMatchModuTool.ModuResult;
                                stringBuilder.Append("粗定位匹配框中心:(" + FastRes.MatchRect[0].CenterPoint.X + "," + FastRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                                stringBuilder.Append("角度:" + FastRes.MatchRect[0].Angle + ",分数:" + FastRes.MatchScore[0] + "\n");
                                break;
                            case 1:
                                IMVSHPFeatureMatchModuTool iMVSHPFeatureMatchModuTool = (IMVSHPFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.高精度匹配1"];
                                HPFeatureMatchResult HPRes = iMVSHPFeatureMatchModuTool.ModuResult;
                                stringBuilder.Append("粗定位匹配框中心:(" + HPRes.MatchRect[0].CenterPoint.X + "," + HPRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                                stringBuilder.Append( "角度:" + HPRes.MatchRect[0].Angle + ",分数:" + HPRes.MatchScore[0] + "\n");
                                break;
                            case 2:
                                IMVSGrayMatchModuVATool iMVSGrayMatchModuVATool = (IMVSGrayMatchModuVATool)VmSolution.Instance[$"{this.VmProcedure.FullName}.灰度匹配1"];
                                GrayMatchVAResult GrayRes = iMVSGrayMatchModuVATool.ModuResult;
                                stringBuilder.Append("粗定位匹配框中心:(" + GrayRes.MatchRect[0].CenterPoint.X + "," + GrayRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                                stringBuilder.Append("角度:" + GrayRes.MatchRect[0].Angle + ",分数:" + GrayRes.MatchScore[0] + "\n");
                                break;
                            case 3:
                                IMVSContourMatchModuTool iMVSContourMatchModuTool = (IMVSContourMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.轮廓匹配1"];
                                ContourMatchResult ContourRes = iMVSContourMatchModuTool.ModuResult;
                                stringBuilder.Append("粗定位匹配框中心:(" + ContourRes.MatchRect[0].CenterPoint.X + "," + ContourRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                                stringBuilder.Append("角度:" + ContourRes.MatchRect[0].Angle + ",分数:" + ContourRes.MatchScore[0] + "\n");
                                break;
                        }
                    }
                    #region 绘制图像
                    HObject regionline1, regionline2, region;
                    HOperatorSet.GenEmptyObj(out regionline1);
                    HOperatorSet.GenEmptyObj(out regionline2);
                    HOperatorSet.GenEmptyObj(out region);

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

                        HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);
                        stringBuilder.Append("质心点:(" + blobRes.CentroidPoint[i].X + "," + blobRes.CentroidPoint[i].Y + ")"+ "\n");
                        stringBuilder.Append("面积:" + blobRes.Area[i] + "\n");
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

                    for (int i = 0; i < xResults.Length; i++)
                    {
                        this.MatchResults.Add(new BlobResult()
                        {
                            CenterX = xResults[i],
                            CenterY = yResults[i],
                            Area = areaResults[i],
                            IsSuccess = true,
                            OutPutImg1 = bitmapResult,
                            Num = NumResults[0]
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
                    if (IsUseCrudePR[0] == 1)
                    {
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

                            HTuple angle3 = MatchRectROI.Angle;
                            HObject ROImodel;
                            HOperatorSet.GenEmptyObj(out ROImodel);
                            ROImodel.Dispose();

                            HOperatorSet.GenRectangle2(out ROImodel, MatchRectROI.CenterPoint.Y, MatchRectROI.CenterPoint.X, -angle3.TupleRad(), MatchRectROI.BoxWidth / 2, MatchRectROI.BoxHeight / 2);
                            HOperatorSet.ZoomRegion(ROImodel, out regionZoom, 0.25, 0.25);
                            ROImodel.Dispose();

                        }
                        else
                        {
                            MarkedWord = "模板 - " + this.Name + " 墨点检测模块失败。";
                        }
                    }
                    else
                    {
                        MarkedWord = "模板 - " + this.Name + " 墨点检测模块失败。";
                    }
                    #region 绘制检测区域
                    HTuple angle = new HTuple();
                    for (int j = 0; j < blobRes.ROI.Count; j++)
                    {
                        HObject tempRegion, zoomRegion;
                        HOperatorSet.GenEmptyObj(out tempRegion);
                        HOperatorSet.GenEmptyObj(out zoomRegion);
                        angle = blobRes.ROI[j].Angle;

                        HOperatorSet.GenRectangle2(out tempRegion, blobRes.ROI[j].CenterPoint.Y, blobRes.ROI[j].CenterPoint.X, -angle.TupleRad(), blobRes.ROI[j].BoxWidth / 2, blobRes.ROI[j].BoxHeight / 2);
                        
                        HOperatorSet.ZoomRegion(tempRegion, out zoomRegion, 0.25, 0.25);
                        HOperatorSet.ConcatObj(regionZoom, zoomRegion, out regionZoom);
                        tempRegion.Dispose();
                        zoomRegion.Dispose();
                    }


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

                    this.MatchResults.Add(new BlobResult()
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
                LogHelper.Post(Level.Error, $"{this.Name}: InkDotDetectAlg 异常：{ex.Message} ", ex, Log.LogCategory.PR);
                return false;
            }
        }
    }
}
