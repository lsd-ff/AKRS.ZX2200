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
using HalconDotNet;
using IMVSCircleFindModuCs;
using IMVSContourMatchModuCs;
using IMVSGrayMatchModuVACs;
using IMVSHPFeatureMatchModuCs;
using ShellModuleCs;

namespace AKRS.Galaxy2.PR.Models.Algs
{
    /// <summary>
    /// 圆查找方法，输出图像为圆和圆心，输出文字为圆心坐标和粗定位结果
    /// </summary>
    [Serializable]
    public class CircleFindAlg : BaseAlg
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
        public CircleFindAlg(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 执行圆查找流程，获取结果
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
                IMVSCircleFindModuTool CircleFindModuTool;
                if (IsUseCrudePR[0] == 1)
                {
                    CrudePRType[0] = (int)CrudeLocateType;
                    this.ProcedureParam.SetInputInt("FirPRType", CrudePRType);
                    CircleFindModuTool = (IMVSCircleFindModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.圆查找1"];
                }
                else
                {
                    CircleFindModuTool = (IMVSCircleFindModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.圆查找2"];
                }
                if (CircleFindModuTool == null)
                {
                    return false;
                }
                this.VmProcedure.Run();
                ProcedureResult = this.VmProcedure.ModuResult;
                CircleFindResult CircleRes = CircleFindModuTool.ModuResult;
                float[] xResults;
                float[] yResults;
                float[] angleResults;
                 if (IsOutputMidpoint&& CircleRes.EdgePointNum >= 1)
                {
                    int num1=CircleRes.EdgePointNum;
                    VM.PlatformSDKCS.PointF pp = CircleRes.CircleCoutourPoint[(int)num1 / 2];
                    xResults = new float[1];
                    xResults[0]= pp.X;
                    
                    yResults=new float[1];
                    yResults[0] = pp.Y;
                }
                else
                {
                    FloatDataArray Centerx = ProcedureResult.GetOutputFloat("ReasultX");
                    xResults = Centerx.pFloatVal == null ? null : Centerx.pFloatVal;

                    FloatDataArray Centery = ProcedureResult.GetOutputFloat("ReasultY");
                    yResults = Centery.pFloatVal == null ? null : Centery.pFloatVal;
                }

                if (IsUseCrudePR[0] == 1)
                {
                    ShellModuleTool shellModuleTool = (ShellModuleTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.脚本1"];
                    ShellResult shellResult = shellModuleTool.ModuResult;
                    FloatDataArray Anlge = shellResult.GetOutputFloat("outA");
                    angleResults = Anlge.pFloatVal == null ? null : Anlge.pFloatVal;
                }
                else
                {
                    angleResults = new float[1];
                    angleResults[0] = 0;
                }

                HObject regionZoom, imageResult1, imageResult2, imageResult3, imageResult;
                HOperatorSet.GenEmptyObj(out regionZoom);
                HOperatorSet.GenEmptyObj(out imageResult1);
                HOperatorSet.GenEmptyObj(out imageResult2);
                HOperatorSet.GenEmptyObj(out imageResult3);
                HOperatorSet.GenEmptyObj(out imageResult);
                Bitmap bitmap, bitmapResult;
                Graphics graph;

                if (xResults != null && xResults.Count() != 0&& angleResults!=null)
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
                                stringBuilder.Append("粗定位匹配框中心:(" + HPRes.MatchRect[0].CenterPoint.X + "," + HPRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
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
                    HObject regionline1, regionline2, region;
                    HOperatorSet.GenEmptyObj(out regionline1);
                    HOperatorSet.GenEmptyObj(out regionline2);
                    HOperatorSet.GenEmptyObj(out region);
                    HTuple r1 = new HTuple(), c1 = new HTuple();

                    if (IsOutputMidpoint)
                    {
                        r1 = yResults[0] / 4;
                        c1 = xResults[0] / 4;
                    }
                    else
                    {
                        r1 = CircleRes.OutputCircle.CenterPoint.Y / 4;
                        c1 = CircleRes.OutputCircle.CenterPoint.X / 4;
                    }

                    // 绘制中心线        
                    HOperatorSet.GenRegionLine(out regionline1, r1 - 5, c1, r1 + 5, c1);
                    HOperatorSet.GenRegionLine(out regionline2, r1, c1 - 5, r1, c1 + 5);
                    HOperatorSet.ConcatObj(regionZoom, regionline2, out regionZoom);
                    HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);

                    // 圆
                    stringBuilder.Append("圆心:(" + CircleRes.OutputCircle.CenterPoint.X + "," + CircleRes.OutputCircle.CenterPoint.Y + ")\n");
                    HOperatorSet.GenCircle(out region, CircleRes.OutputCircle.CenterPoint.Y, CircleRes.OutputCircle.CenterPoint.X, CircleRes.OutputCircle.Radius);
                    HOperatorSet.ZoomRegion(region, out region, 0.25, 0.25);
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
                            MarkedWord = "模板 - " + this.Name + " 圆搜索模块失败。";
                        }
                    }
                    else
                    {
                        MarkedWord = "模块 - " + this.Name + " 圆搜索模板失败。";
                    }
                    #region 绘制检测区域
                    HObject region;
                    HOperatorSet.GenEmptyObj(out region);
                    HTuple angle = CircleRes.ROI.Angle;
                    region.Dispose();
                    HOperatorSet.GenRectangle2(out region, CircleRes.ROI.CenterPoint.Y, CircleRes.ROI.CenterPoint.X, -angle.TupleRad(), CircleRes.ROI.BoxWidth / 2, CircleRes.ROI.BoxHeight / 2);
                    HOperatorSet.ConcatObj(regionZoom, region, out regionZoom);
                    HOperatorSet.ZoomRegion(regionZoom, out regionZoom, 0.25, 0.25);
                    region.Dispose();
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
                LogHelper.Post(Level.Error, $"{this.Name}: CircleFindAlg 异常：{ex.Message} ", ex, Log.LogCategory.PR);
                return false;
            }
        }
    }
}
