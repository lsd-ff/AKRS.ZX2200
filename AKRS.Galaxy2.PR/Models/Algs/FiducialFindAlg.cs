using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Models.Services;
using HalconDotNet;
using IMVSContourMatchModuCs;
using IMVSFastFeatureMatchModuCs;
using IMVSFixtureModuCs;
using IMVSGrayMatchModuVACs;
using IMVSHPFeatureMatchModuCs;
using IMVSMarkFindModuCs;
using LanguageExt.Common;
using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Core;
using VM.PlatformSDKCS;

namespace AKRS.Galaxy2.PR.Models.Algs
{
    /// <summary>
    /// 基准查找方法,输出图像为匹配框、匹配框中心和轮廓点，输出文字为匹配框中心坐标
    /// </summary>
    [Serializable]
    public class FiducialFindAlg : BaseAlg
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
        public FiducialFindAlg(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 执行基准查找流程，获取结果
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

                #region Bitmap
                IMVSMarkFindModuTool MarkFindModuTool;
                if (IsUseCrudePR[0] == 1)
                {
                    CrudePRType[0] = (int)CrudeLocateType;
                    this.ProcedureParam.SetInputInt("FirPRType", CrudePRType);
                    MarkFindModuTool = (IMVSMarkFindModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.图形定位1"];
                }
                else
                {
                    MarkFindModuTool = (IMVSMarkFindModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.图形定位2"];
                }
                if (MarkFindModuTool == null)
                {
                    return false;
                }
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

                MarkFindResult MarkRes = MarkFindModuTool.ModuResult;

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
                                stringBuilder.Append("粗定位匹配框中心:(" + FastRes.MatchRect[0].CenterPoint.X + "," + FastRes.MatchRect[0].CenterPoint.Y + ")" +  "\n");
                                stringBuilder.Append("角度:" + FastRes.MatchRect[0].Angle + ",分数:" + FastRes.MatchScore[0] + "\n");
                                break;
                            case 1:
                                IMVSHPFeatureMatchModuTool iMVSHPFeatureMatchModuTool = (IMVSHPFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.高精度匹配1"];
                                HPFeatureMatchResult HPRes = iMVSHPFeatureMatchModuTool.ModuResult;
                                stringBuilder.Append("粗定位匹配框中心:(" + HPRes.MatchRect[0].CenterPoint.X + "," + HPRes.MatchRect[0].CenterPoint.Y + ")" +  "\n");
                                stringBuilder.Append( "角度:" + HPRes.MatchRect[0].Angle + ",分数:" + HPRes.MatchScore[0] + "\n");
                                break;
                            case 2:
                                IMVSGrayMatchModuVATool iMVSGrayMatchModuVATool = (IMVSGrayMatchModuVATool)VmSolution.Instance[$"{this.VmProcedure.FullName}.灰度匹配1"];
                                GrayMatchVAResult GrayRes = iMVSGrayMatchModuVATool.ModuResult;
                                stringBuilder.Append("粗定位匹配框中心:(" + GrayRes.MatchRect[0].CenterPoint.X + "," + GrayRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                                stringBuilder.Append( "角度:" + GrayRes.MatchRect[0].Angle + ",分数:" + GrayRes.MatchScore[0] + "\n");
                                break;
                            case 3:
                                IMVSContourMatchModuTool iMVSContourMatchModuTool = (IMVSContourMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.轮廓匹配1"];
                                ContourMatchResult ContourRes = iMVSContourMatchModuTool.ModuResult;
                                stringBuilder.Append("粗定位匹配框中心:(" + ContourRes.MatchRect[0].CenterPoint.X + "," + ContourRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                                stringBuilder.Append( "角度:" + ContourRes.MatchRect[0].Angle + ",分数:" + ContourRes.MatchScore[0] + "\n");
                                break;
                        }
                    }

                    #region 绘制图像
                    HObject regionline1, regionline2, region, recRegion;
                    HOperatorSet.GenEmptyObj(out regionline1);
                    HOperatorSet.GenEmptyObj(out regionline2);
                    HOperatorSet.GenEmptyObj(out region);
                    HOperatorSet.GenEmptyObj(out recRegion);

                    HTuple a1 = new HTuple(), r1 = new HTuple(), c1 = new HTuple();

                    for (int i = 0; i < MarkRes.MatchRect.Count; i++)
                    {
                        HTuple angle = MarkRes.MatchRect[i].Angle;
                        HOperatorSet.GenRectangle2(out recRegion, MarkRes.MatchRect[i].CenterPoint.Y, MarkRes.MatchRect[i].CenterPoint.X, -angle.TupleRad(), MarkRes.MatchRect[i].BoxWidth / 2, MarkRes.MatchRect[i].BoxHeight / 2);
                        HOperatorSet.ZoomRegion(recRegion, out recRegion, 0.25, 0.25);
                        r1 = MarkRes.MatchRect[i].CenterPoint.Y / 4;
                        c1 = MarkRes.MatchRect[i].CenterPoint.X / 4;

                        // 绘制中心线        
                        HOperatorSet.GenRegionLine(out regionline1, r1 - 5, c1, r1 + 5, c1);
                        HOperatorSet.GenRegionLine(out regionline2, r1, c1 - 5, r1, c1 + 5);
                        HOperatorSet.ConcatObj(regionline1, regionline2, out regionline1);
                        HTuple HomMat2D2 = new HTuple();
                        HOperatorSet.VectorAngleToRigid(r1, c1, 0, r1, c1, -angle.TupleRad(), out HomMat2D2);
                        HOperatorSet.AffineTransRegion(regionline1, out regionline1, HomMat2D2, "nearest_neighbor");
                        HOperatorSet.ConcatObj(regionZoom, recRegion, out regionZoom);

                        // 模板框与中心线
                        HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);
                        stringBuilder.Append("匹配框中心:(" + MarkRes.MatchRect[i].CenterPoint.X + "," + MarkRes.MatchRect[i].CenterPoint.Y + ")" + "\n");
                        stringBuilder.Append( "角度:" + MarkRes.MatchRect[i].Angle + ",分数:" + MarkRes.MatchScore[i] + "\n");
                    }
                    recRegion.Dispose();
                    // 轮廓点
                    var outlinePoints = MarkRes.MatchOutline.MatchOutlinePoints;
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

                            HTuple angle1 = MatchRectROI.Angle;
                            HOperatorSet.GenRectangle2(out regionZoom, MatchRectROI.CenterPoint.Y, MatchRectROI.CenterPoint.X, -angle1.TupleRad(), MatchRectROI.BoxWidth / 2, MatchRectROI.BoxHeight / 2);
                        }
                        else
                        {
                            MarkedWord = "模板 - " + this.Name + " 基准查找模块失败。";
                        }
                    }
                    else
                    {
                        MarkedWord = "模板 - " + this.Name + " 基准查找模块失败。";
                    }
                    #region 绘制检测区域
                    HTuple angle = MarkRes.ROI.Angle;
                    HObject ROImodel;
                    HOperatorSet.GenEmptyObj(out ROImodel);
                    ROImodel.Dispose(); 
                    HOperatorSet.GenRectangle2(out ROImodel, MarkRes.ROI.CenterPoint.Y, MarkRes.ROI.CenterPoint.X, -angle.TupleRad(), MarkRes.ROI.BoxWidth / 2, MarkRes.ROI.BoxHeight / 2);
                    HOperatorSet.ConcatObj( regionZoom, ROImodel, out regionZoom);
                    ROImodel.Dispose();
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
                LogHelper.Post(Level.Error, $"{this.Name}: FiducialFindAlg 异常：{ex.Message} ", ex, Log.LogCategory.PR);
                return false;
            }
        }
    }
}
