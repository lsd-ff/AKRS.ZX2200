using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Models.Services;
using HalconDotNet;
using IMVSBlobFindModuCs;
using IMVSCaliperCornerModuCs;
using IMVSContourMatchModuCs;
using IMVSFastFeatureMatchModuCs;
using IMVSFixtureModuCs;
using IMVSGrayMatchModuVACs;
using IMVSHPFeatureMatchModuCs;
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
    /// 交点查找方法，输出图像为两条边和交点十字，输出文字为交点坐标和粗定位结果
    /// </summary>
    [Serializable]
    public class CrossSearchAlg : BaseAlg
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
        public CrossSearchAlg(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 执行交点查找流程，获取结果
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
                IMVSCaliperCornerModuTool iMVSCaliperCornerModuTool;
                if (IsUseCrudePR[0] == 1)
                {
                    CrudePRType[0] = (int)CrudeLocateType;
                    this.ProcedureParam.SetInputInt("FirPRType", CrudePRType);
                    iMVSCaliperCornerModuTool = (IMVSCaliperCornerModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.边缘交点1"];
                }
                else
                {
                    iMVSCaliperCornerModuTool = (IMVSCaliperCornerModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.边缘交点2"];
                }
                if (iMVSCaliperCornerModuTool == null)
                {
                    return false;
                }
                this.VmProcedure.Run();
                ProcedureResult = this.VmProcedure.ModuResult;

                FloatDataArray Centerx = ProcedureResult.GetOutputFloat("ReasultX");
                float[] xResults = Centerx.pFloatVal == null ? null : Centerx.pFloatVal;

                FloatDataArray Centery = ProcedureResult.GetOutputFloat("ReasultY");
                float[] yResults = Centery.pFloatVal == null ? null : Centery.pFloatVal;

                FloatDataArray Radius = ProcedureResult.GetOutputFloat("ReasultA");
                float[] angleResults = Radius.pFloatVal == null ? null : Radius.pFloatVal;

                CaliperCornerResult CaliperRes = iMVSCaliperCornerModuTool.ModuResult;
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
                                stringBuilder.Append("粗定位匹配框中心:(" + FastRes.MatchRect[0].CenterPoint.X + "," + FastRes.MatchRect[0].CenterPoint.Y + ")"  + "\n");
                                stringBuilder.Append( "角度:" + FastRes.MatchRect[0].Angle + ",分数:" + FastRes.MatchScore[0] + "\n");
                                break;
                            case 1:
                                IMVSHPFeatureMatchModuTool iMVSHPFeatureMatchModuTool = (IMVSHPFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.高精度匹配1"];
                                HPFeatureMatchResult HPRes = iMVSHPFeatureMatchModuTool.ModuResult;
                                stringBuilder.Append("粗定位匹配框中心:(" + HPRes.MatchRect[0].CenterPoint.X + "," + HPRes.MatchRect[0].CenterPoint.Y + ")"  + "\n");
                                stringBuilder.Append("角度:" + HPRes.MatchRect[0].Angle + ",分数:" + HPRes.MatchScore[0] + "\n");
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
                                stringBuilder.Append("粗定位匹配框中心:(" + ContourRes.MatchRect[0].CenterPoint.X + "," + ContourRes.MatchRect[0].CenterPoint.Y + ")"+"\n");
                                stringBuilder.Append( "角度:" + ContourRes.MatchRect[0].Angle + ",分数:" + ContourRes.MatchScore[0] + "\n");
                                break;
                        }
                    }
                    #region 绘制图像
                    HObject centerline1, centerline2, region, line1, line2;
                    HOperatorSet.GenEmptyObj(out centerline1);
                    HOperatorSet.GenEmptyObj(out centerline2);
                    HOperatorSet.GenEmptyObj(out region);
                    HOperatorSet.GenEmptyObj(out line1);
                    HOperatorSet.GenEmptyObj(out line2);
                    HTuple r1 = new HTuple(), c1 = new HTuple();
                    HOperatorSet.GenRegionLine(out line1, CaliperRes.EdgeLine1.StartPoint.Y / 4, CaliperRes.EdgeLine1.StartPoint.X / 4, CaliperRes.EdgeLine1.EndPoint.Y / 4, CaliperRes.EdgeLine1.EndPoint.X / 4);
                    HOperatorSet.GenRegionLine(out line2, CaliperRes.EdgeLine2.StartPoint.Y / 4, CaliperRes.EdgeLine2.StartPoint.X / 4, CaliperRes.EdgeLine2.EndPoint.Y / 4, CaliperRes.EdgeLine2.EndPoint.X / 4);
                    HOperatorSet.ConcatObj(line1, line2, out region);
                    HOperatorSet.ConcatObj(regionZoom, region, out regionZoom);
                    r1 = CaliperRes.CaliperCorner.Y / 4;
                    c1 = CaliperRes.CaliperCorner.X / 4;

                    // 绘制中心线        
                    HOperatorSet.GenRegionLine(out centerline1, r1 - 5, c1, r1 + 5, c1);
                    HOperatorSet.GenRegionLine(out centerline2, r1, c1 - 5, r1, c1 + 5);
                    HOperatorSet.ConcatObj(regionZoom, centerline2, out regionZoom);
                    HOperatorSet.ConcatObj(regionZoom, centerline1, out regionZoom);
                    stringBuilder.Append("交点坐标:(" + CaliperRes.CaliperCorner.X + "," + CaliperRes.CaliperCorner.Y + ")\n");

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
                    centerline1.Dispose();
                    centerline2.Dispose();
                    region.Dispose();
                    line1.Dispose();
                    line2.Dispose();
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
                    if (IsUseCrudePR[0] == 1)
                    {
                        IMVSFixtureModuTool iMVSFixtureModuTool = (IMVSFixtureModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.位置修正2"];

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
                            HOperatorSet.GenRectangle2(out regionZoom, MatchRectROI.CenterPoint.Y, MatchRectROI.CenterPoint.X, -angle3.TupleRad(), MatchRectROI.BoxWidth / 2, MatchRectROI.BoxHeight / 2);
                        }
                        else
                        {
                            MarkedWord = "模板 - " + this.Name + " 交点查找模块失败";
                        }
                    }
                    else
                    {
                        MarkedWord = "模板 - " + this.Name + " 交点查找模块失败。";
                    }
                    #region 绘制检测区域

                    HObject ROI1, ROI2;
                    HOperatorSet.GenEmptyObj(out ROI1);
                    HOperatorSet.GenEmptyObj(out ROI2);
                    HTuple angle1 = CaliperRes.ROI1.Angle;
                    HOperatorSet.GenRectangle2(out ROI1, CaliperRes.ROI1.CenterPoint.Y, CaliperRes.ROI1.CenterPoint.X, -angle1.TupleRad(), CaliperRes.ROI1.BoxWidth / 2, CaliperRes.ROI1.BoxHeight / 2);
                    HTuple angle2 = CaliperRes.ROI2.Angle;
                    HOperatorSet.GenRectangle2(out ROI2, CaliperRes.ROI2.CenterPoint.Y, CaliperRes.ROI2.CenterPoint.X, -angle2.TupleRad(), CaliperRes.ROI2.BoxWidth / 2, CaliperRes.ROI2.BoxHeight / 2);
                    HOperatorSet.ConcatObj(regionZoom, ROI2, out regionZoom);
                    HOperatorSet.ConcatObj(regionZoom, ROI1, out regionZoom);
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
                    ROI1.Dispose();
                    ROI2.Dispose();
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
                LogHelper.Post(Level.Error, $"{this.Name}: CrossSearchAlg 异常：{ex.Message} ", ex, Log.LogCategory.PR);
                return false;
            }
        }
    }
}
