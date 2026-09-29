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
using AKRS.Galaxy2.PR.Models.Services;
using GeometryCreateCs;
using HalconDotNet;
using GraphicsSetModuleCs;
using ShellModuleCs;
using DevExpress.XtraPrinting;
using IMVSContourMatchModuCs;
using IMVSGrayMatchModuVACs;
using IMVSHPFeatureMatchModuCs;
using IMVSBlobFindModuCs;
using IMVSEdgeFlawInspModuCs;

namespace AKRS.Galaxy2.PR.Models.Algs
{
    /// <summary>
    /// 胶量检测方法，输出轮廓点和质心点，文字
    /// </summary>
    [Serializable]
    public class EpoxyDetectAlg : BaseAlg
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
        /// <param name="name">名称</param>
        public EpoxyDetectAlg(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 执行胶量检测流程，获取结果
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
                IsUseCrudePR[0] = Convert.ToInt32(IsUseCrudeLocate);
                this.ProcedureParam.SetInputInt("IsFirPR", IsUseCrudePR);

                float Crudex = 0;
                float Crudey = 0;

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

                float[] xResults;
                float[] yResults;
                float[] areaResults;

                if (IsUseCrudeLocate)
                {
                    FloatDataArray Centerx = ProcedureResult.GetOutputFloat("1BlobCenterX");
                    xResults = Centerx.pFloatVal == null ? null : Centerx.pFloatVal;

                    FloatDataArray Centery = ProcedureResult.GetOutputFloat("1BlobCenterY");
                    yResults = Centery.pFloatVal == null ? null : Centery.pFloatVal;

                    FloatDataArray Area = ProcedureResult.GetOutputFloat("1BlobArea");
                    areaResults = Area.pFloatVal == null ? null : Area.pFloatVal;

                }
                else
                {
                    FloatDataArray Centerx = ProcedureResult.GetOutputFloat("2BlobCenterX");
                    xResults = Centerx.pFloatVal == null ? null : Centerx.pFloatVal;

                    FloatDataArray Centery = ProcedureResult.GetOutputFloat("2BlobCenterY");
                    yResults = Centery.pFloatVal == null ? null : Centery.pFloatVal;

                    FloatDataArray Area = ProcedureResult.GetOutputFloat("2BlobArea");
                    areaResults = Area.pFloatVal == null ? null : Area.pFloatVal;
                }


                BlobFindResult blobRes = BlobFindModuTool.ModuResult;
               
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
                    if (IsUseCrudeLocate)
                    {
                        switch (CrudePRType[0])
                        {
                            case 0:
                                IMVSFastFeatureMatchModuTool iMVSFastFeatureMatchModuTool = (IMVSFastFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.快速匹配1"];
                                FastFeatureMatchResult FastRes = iMVSFastFeatureMatchModuTool.ModuResult;
                                Crudex = FastRes.MatchRect[0].CenterPoint.X;
                                Crudey = FastRes.MatchRect[0].CenterPoint.Y;
                                break;
                            case 1:
                                IMVSHPFeatureMatchModuTool iMVSHPFeatureMatchModuTool = (IMVSHPFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.高精度匹配1"];
                                HPFeatureMatchResult HPRes = iMVSHPFeatureMatchModuTool.ModuResult;
                                Crudex = HPRes.MatchRect[0].CenterPoint.X;
                                Crudey = HPRes.MatchRect[0].CenterPoint.Y;
                                break;
                            case 2:
                                IMVSGrayMatchModuVATool iMVSGrayMatchModuVATool = (IMVSGrayMatchModuVATool)VmSolution.Instance[$"{this.VmProcedure.FullName}.灰度匹配1"];
                                GrayMatchVAResult GrayRes = iMVSGrayMatchModuVATool.ModuResult;
                                Crudex = GrayRes.MatchRect[0].CenterPoint.X;
                                Crudey = GrayRes.MatchRect[0].CenterPoint.Y;
                                break;
                            case 3:
                                IMVSContourMatchModuTool iMVSContourMatchModuTool = (IMVSContourMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.轮廓匹配1"];
                                ContourMatchResult ContourRes = iMVSContourMatchModuTool.ModuResult;
                                Crudex = ContourRes.MatchRect[0].CenterPoint.X;
                                Crudey = ContourRes.MatchRect[0].CenterPoint.Y;
                                break;
                        }
                        
                    }
                    #region 绘制图像
                    HObject regionline1, regionline2, region;
                    HOperatorSet.GenEmptyObj(out regionline1);
                    HOperatorSet.GenEmptyObj(out regionline2);
                    HOperatorSet.GenEmptyObj(out region);

                    HTuple a1 = new HTuple(), r1 = new HTuple(), c1 = new HTuple();
                    StringBuilder stringBuilder = new StringBuilder();
                    if (IsOutputTotal)
                    {
                        float x = 0;
                        float y = 0;
                        float a = 0;

                        for (int i = 0; i < xResults.Length; i++)
                        {
                            x += xResults[i];
                            y += yResults[i];
                            a += areaResults[i];
                        }
                        xResults = new float[1] { x / xResults.Length };
                        yResults = new float[1] { y / yResults.Length };
                        areaResults = new float[1] { a };
                    }
                    if (IsOutputTotal)
                    {
                        
                            r1 = yResults[0]/4;
                            c1 = xResults[0] / 4;

                            // 绘制中心线        
                            HOperatorSet.GenRegionLine(out regionline1, r1 - 5, c1, r1 + 5, c1);
                            HOperatorSet.GenRegionLine(out regionline2, r1, c1 - 5, r1, c1 + 5);
                            HOperatorSet.ConcatObj(regionZoom, regionline2, out regionZoom);                          
                            HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);

                        stringBuilder.Append("质心点:(" + xResults[0] + "," + yResults[0] + ")" + "\n");
                        stringBuilder.Append("面积:" + areaResults[0] + "\n");
                    }
                    else
                    {
                        for (int i = 0; i < xResults.Length; i++)
                        {
                            r1 = yResults[i] /4;
                            c1 = xResults[i] /4;

                            // 绘制中心线        
                            HOperatorSet.GenRegionLine(out regionline1, r1 - 5, c1, r1 + 5, c1);
                            HOperatorSet.GenRegionLine(out regionline2, r1, c1 - 5, r1, c1 + 5);
                            HOperatorSet.ConcatObj(regionline1, regionline2, out regionline1);
                            HTuple HomMat2D2 = new HTuple();
                            HOperatorSet.VectorAngleToRigid(r1, c1, 0, r1, c1, 0, out HomMat2D2);
                            HOperatorSet.AffineTransRegion(regionline1, out regionline1, HomMat2D2, "nearest_neighbor");
                            HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);
                            stringBuilder.Append("质心点:(" + xResults[i] + "," + yResults[i] + ")" + "\n");
                            stringBuilder.Append("面积:" + areaResults[i] + "\n");
           
                        }
                    }

                    // 轮廓点

                    var outlinePoints = blobRes.ContourInfo.MatchOutlinePoints;
                    List<HTuple> xList = new List<HTuple>();
                    List<HTuple> yList = new List<HTuple>();

                    for (int i = 0; i < outlinePoints.Count; i += 5)
                    {
                        xList.Add((HTuple)outlinePoints[i].MatchOutlineX / 4);
                        yList.Add((HTuple)outlinePoints[i].MatchOutlineY / 4);
                    }

                    HTuple PointX = new HTuple(xList.ToArray());
                    HTuple PointY = new HTuple(yList.ToArray());


                
                    HOperatorSet.GenRegionPoints(out region, PointY, PointX);
                    HOperatorSet.ConcatObj(regionZoom, region, out regionZoom); 

                    // 将结果画在三通道压缩图像上并传出
                    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult1, 255, "margin");
                    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult2, 0, "margin");
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
                        });
                    }

                    //if (IsUseCrudeLocate)
                    //{                        
                    //    for (int i = 0; i < xResults.Length; i++)
                    //    {
                    //        this.MatchResults.Add(new BlobResult()
                    //        {
                    //            CenterX = xResults[i] - Crudex,
                    //            CenterY = yResults[i] - Crudey,
                    //            Area = areaResults[i],
                    //            IsSuccess = true,
                    //            OutPutImg1 = bitmapResult,
                    //        });
                    //    }                       
                    //}
                    //else
                    //{
                    //    for (int i = 0; i < xResults.Length; i++)
                    //    {
                    //        this.MatchResults.Add(new BlobResult()
                    //        {
                    //            CenterX = xResults[i],
                    //            CenterY = yResults[i],
                    //            Area = areaResults[i],
                    //            IsSuccess = true,
                    //            OutPutImg1 = bitmapResult,
                    //        });
                    //    }
                    //}

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
                    string MarkedWord;
                    HObject regionline1, zoomRegion;
                    HOperatorSet.GenEmptyObj(out regionline1);
                    HOperatorSet.GenEmptyObj(out zoomRegion);
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
                            HObject ROImodel;
                            HOperatorSet.GenEmptyObj(out ROImodel);
                            HTuple angle3 = MatchRectROI.Angle;
                            ROImodel.Dispose();
                            HOperatorSet.GenRectangle2(out ROImodel, MatchRectROI.CenterPoint.Y, MatchRectROI.CenterPoint.X, -angle3.TupleRad(), MatchRectROI.BoxWidth / 2, MatchRectROI.BoxHeight / 2);
                            HOperatorSet.ZoomRegion(ROImodel, out regionZoom, 0.25, 0.25);
                            ROImodel.Dispose();
                        }
                        else
                        {
                            MarkedWord = "模板 - " + this.Name + " 胶量检测模块失败。";
                        }                        
                    }
                    else
                    {
                        MarkedWord = "模板 - " + this.Name + " 胶量检测模块失败。";
                    }

                    #region 绘制检测区域                  
                    for (int i = 0; i < blobRes.ROI.Count; i++)
                    {
                        HTuple angle = blobRes.ROI[i].Angle;
                        HOperatorSet.GenRectangle2(out regionline1, blobRes.ROI[i].CenterPoint.Y, blobRes.ROI[i].CenterPoint.X, -angle.TupleRad(), blobRes.ROI[i].BoxWidth / 2, blobRes.ROI[i].BoxHeight / 2);
                        HOperatorSet.ZoomRegion(regionline1, out zoomRegion, 0.25, 0.25);
                        HOperatorSet.ConcatObj(regionZoom, zoomRegion, out regionZoom);
                    }
                    regionline1.Dispose();
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

                return false;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"{this.Name}: EpoxyDetectAlg 异常：{ex.Message} ", ex, Log.LogCategory.PR);
                return false;
            }
        }
    }
}
