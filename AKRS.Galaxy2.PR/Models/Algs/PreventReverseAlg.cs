using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.PR.Models.MatchResults;
using ImageSourceModuleCs;
using IMVSFixtureModuCs;
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
using IMVSHPFeatureMatchModuCs;
using HalconDotNet;
using IMVSContourMatchModuCs;
using IMVSFastFeatureMatchModuCs;
using IMVSGrayMatchModuVACs;
using AKRS.Galaxy2.PR.Models.Services;

namespace AKRS.Galaxy2.PR.Models.Algs
{
    /// <summary>
    /// 防反检测方法
    /// </summary>
    [Serializable]
    public class PreventReverseAlg : BaseAlg
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
        /// 精定位方式
        /// </summary>
        public int[] ExactPRType = new int[1];

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">算法流程名称</param>
        public PreventReverseAlg(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 执行防反检测流程，获取结果
        /// </summary>
        /// <returns>是否成功</returns>
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
                CrudePRType[0] = (int)CrudeLocateType;
                this.ProcedureParam.SetInputInt("FirPRType", CrudePRType);
                ExactPRType[0] = (int)ExactLocateType;
                this.ProcedureParam.SetInputInt("PRType", ExactPRType);

                #region Bitmap
                this.VmProcedure.Run();
                ProcedureResult = this.VmProcedure.ModuResult;
                StringData[] sResults;
                if (IsUseCrudePR[0] == 1)
                {
                    StringDataArray Centerx = ProcedureResult.GetOutputString("Res1");
                    sResults = Centerx.astStringVal == null ? null : Centerx.astStringVal;

                }
                else
                {
                    StringDataArray Centerx = ProcedureResult.GetOutputString("Res2");
                    sResults = Centerx.astStringVal == null ? null : Centerx.astStringVal;
                }

                HObject regionZoom, imageResult1, imageResult2, imageResult3, imageResult;
                HOperatorSet.GenEmptyObj(out regionZoom);
                HOperatorSet.GenEmptyObj(out imageResult1);
                HOperatorSet.GenEmptyObj(out imageResult2);
                HOperatorSet.GenEmptyObj(out imageResult3);
                HOperatorSet.GenEmptyObj(out imageResult);
                Bitmap bitmap, bitmapResult;
                Graphics graph;


                if (sResults != null && sResults.Count() != 0 && sResults[0].strValue.ToString() == "true")
                {
                    // 正确
                    StringBuilder stringBuilder = new StringBuilder();
                    List<RectBox> MatchRectROI = new List<RectBox>();
                    if (IsUseCrudePR[0] == 1)
                    {
                        switch (ExactPRType[0])
                        {
                            case 0:
                                IMVSFastFeatureMatchModuTool iMVSFastFeatureMatchModuTool = (IMVSFastFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.快速匹配2"];
                                if (iMVSFastFeatureMatchModuTool == null)
                                {
                                    return false;
                                }
                                FastFeatureMatchResult FastRes = iMVSFastFeatureMatchModuTool.ModuResult;
                                MatchRectROI = FastRes.MatchRect;
                                stringBuilder.Append("匹配框中心:(" + FastRes.MatchRect[0].CenterPoint.X + "," + FastRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                                stringBuilder.Append("角度:" + FastRes.MatchRect[0].Angle + ",分数:" + FastRes.MatchScore[0] + "\n");
                                break;
                            case 1:
                                IMVSHPFeatureMatchModuTool iMVSHPFeatureMatchModuTool = (IMVSHPFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.高精度匹配2"];
                                if (iMVSHPFeatureMatchModuTool == null)
                                {
                                    return false;
                                }
                                HPFeatureMatchResult HPRes = iMVSHPFeatureMatchModuTool.ModuResult;
                                MatchRectROI = HPRes.MatchRect;
                                stringBuilder.Append("匹配框中心:(" + HPRes.MatchRect[0].CenterPoint.X + "," + HPRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                                stringBuilder.Append("角度:" + HPRes.MatchRect[0].Angle + ",分数:" + HPRes.MatchScore[0] + "\n");
                                break;
                            case 2:
                                IMVSGrayMatchModuVATool iMVSGrayMatchModuVATool = (IMVSGrayMatchModuVATool)VmSolution.Instance[$"{this.VmProcedure.FullName}.灰度匹配2"];
                                if (iMVSGrayMatchModuVATool == null)
                                {
                                    return false;
                                }
                                GrayMatchVAResult GrayRes = iMVSGrayMatchModuVATool.ModuResult;
                                MatchRectROI = GrayRes.MatchRect;
                                stringBuilder.Append("匹配框中心:(" + GrayRes.MatchRect[0].CenterPoint.X + "," + GrayRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                                stringBuilder.Append("角度:" + GrayRes.MatchRect[0].Angle + ",分数:" + GrayRes.MatchScore[0] + "\n");
                                break;
                            case 3:
                                IMVSContourMatchModuTool iMVSContourMatchModuTool = (IMVSContourMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.轮廓匹配2"];
                                if (iMVSContourMatchModuTool == null)
                                {
                                    return false;
                                }
                                ContourMatchResult ContourRes = iMVSContourMatchModuTool.ModuResult;
                                MatchRectROI = ContourRes.MatchRect;
                                stringBuilder.Append("匹配框中心:(" + ContourRes.MatchRect[0].CenterPoint.X + "," + ContourRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                                stringBuilder.Append("角度:" + ContourRes.MatchRect[0].Angle + ",分数:" + ContourRes.MatchScore[0] + "\n");
                                break;
                        }
                    }
                    else
                    {
                        switch (ExactPRType[0])
                        {
                            case 0:
                                IMVSFastFeatureMatchModuTool iMVSFastFeatureMatchModuTool = (IMVSFastFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.快速匹配3"];
                                if (iMVSFastFeatureMatchModuTool == null)
                                {
                                    return false;
                                }
                                FastFeatureMatchResult FastRes = iMVSFastFeatureMatchModuTool.ModuResult;
                                MatchRectROI = FastRes.MatchRect;
                                stringBuilder.Append("匹配框中心:(" + FastRes.MatchRect[0].CenterPoint.X + "," + FastRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                                stringBuilder.Append("角度:" + FastRes.MatchRect[0].Angle + ",分数:" + FastRes.MatchScore[0] + "\n");
                                break;
                            case 1:
                                IMVSHPFeatureMatchModuTool iMVSHPFeatureMatchModuTool = (IMVSHPFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.高精度匹配3"];
                                if (iMVSHPFeatureMatchModuTool == null)
                                {
                                    return false;
                                }
                                HPFeatureMatchResult HPRes = iMVSHPFeatureMatchModuTool.ModuResult;
                                MatchRectROI = HPRes.MatchRect;
                                stringBuilder.Append("匹配框中心:(" + HPRes.MatchRect[0].CenterPoint.X + "," + HPRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                                stringBuilder.Append("角度:" + HPRes.MatchRect[0].Angle + ",分数:" + HPRes.MatchScore[0] + "\n");
                                break;
                            case 2:
                                IMVSGrayMatchModuVATool iMVSGrayMatchModuVATool = (IMVSGrayMatchModuVATool)VmSolution.Instance[$"{this.VmProcedure.FullName}.灰度匹配3"];
                                if (iMVSGrayMatchModuVATool == null)
                                {
                                    return false;
                                }
                                GrayMatchVAResult GrayRes = iMVSGrayMatchModuVATool.ModuResult;
                                MatchRectROI = GrayRes.MatchRect;
                                stringBuilder.Append("匹配框中心:(" + GrayRes.MatchRect[0].CenterPoint.X + "," + GrayRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                                stringBuilder.Append("角度:" + GrayRes.MatchRect[0].Angle + ",分数:" + GrayRes.MatchScore[0] + "\n");
                                break;
                            case 3:
                                IMVSContourMatchModuTool iMVSContourMatchModuTool = (IMVSContourMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.轮廓匹配3"];
                                if (iMVSContourMatchModuTool == null)
                                {
                                    return false;
                                }
                                ContourMatchResult ContourRes = iMVSContourMatchModuTool.ModuResult;
                                MatchRectROI = ContourRes.MatchRect;
                                stringBuilder.Append("匹配框中心:(" + ContourRes.MatchRect[0].CenterPoint.X + "," + ContourRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                                stringBuilder.Append("角度:" + ContourRes.MatchRect[0].Angle + ",分数:" + ContourRes.MatchScore[0] + "\n");
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

                    for (int i = 0; i < MatchRectROI.Count; i++)
                    {
                        HTuple angle = MatchRectROI[i].Angle;
                        HOperatorSet.GenRectangle2(out recRegion, MatchRectROI[i].CenterPoint.Y, MatchRectROI[i].CenterPoint.X, -angle.TupleRad(), MatchRectROI[i].BoxWidth / 2, MatchRectROI[i].BoxHeight / 2);
                        HOperatorSet.ZoomRegion(recRegion, out recRegion, 0.25, 0.25);
                        r1 = MatchRectROI[i].CenterPoint.Y / 4;
                        c1 = MatchRectROI[i].CenterPoint.X / 4;

                        // 绘制中心线        
                        HOperatorSet.GenRegionLine(out regionline1, r1 - 10, c1, r1 + 10, c1);
                        HOperatorSet.GenRegionLine(out regionline2, r1, c1 - 10, r1, c1 + 10);
                        HOperatorSet.ConcatObj(regionline1, regionline2, out regionline1);
                        HTuple HomMat2D2 = new HTuple();
                        HOperatorSet.VectorAngleToRigid(r1, c1, 0, r1, c1, -angle.TupleRad(), out HomMat2D2);
                        HOperatorSet.AffineTransRegion(regionline1, out regionline1, HomMat2D2, "nearest_neighbor");
                        HOperatorSet.ConcatObj(regionZoom, recRegion, out regionZoom);
                        HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);
                    }
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
                    graph.DrawString(stringBuilder.ToString(), new Font("宋体", 15), new SolidBrush(Color.Green), new Point(0, 0));
                    bitmap.Dispose();
                    graph.Dispose();
                    regionline1.Dispose();
                    regionline2.Dispose();
                    region.Dispose();
                    #endregion
                    for (int i = 0; i < sResults.Length; i++)
                    {
                        this.MatchResults.Add(new MatchResult()
                        {
                            IsSuccess = true,
                            OutPutImg1 = bitmapResult,
                        });
                    }

                }
                else if (sResults[0].strValue.ToString() == "false")
                {
                    // 错误
                    string MarkedWord = "该物体和模板不匹配。";
                    HOperatorSet.GenEmptyObj(out imageResult);
                    imageResult.Dispose();
                    HOperatorSet.Compose3(this.SourceImg, this.SourceImg, this.SourceImg, out imageResult);

                    // 绘制文字
                    bitmap = ImageHelp.HObjectToBitmap(imageResult);
                    bitmapResult = new Bitmap(bitmap);
                    graph = Graphics.FromImage(bitmapResult);
                    graph.DrawString(MarkedWord, new Font("宋体", 15), new SolidBrush(Color.LightGreen), new Point(0, 0));
                    bitmap.Dispose();
                    graph.Dispose();
                    this.MatchResults.Add(new MatchResult()
                    {
                        IsSuccess = false,
                        OutPutImg1 = bitmapResult,
                        MarkedWords = MarkedWord,
                    });
                   
                }
                else
                {
                    RectBox MatchRectROI = new RectBox();
                    // 异常
                    string MarkedWord = "";
                    if (IsUseCrudePR[0] == 1)
                    {
                        IMVSFixtureModuTool iMVSFixtureModuTool = (IMVSFixtureModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.位置修正1"];
                        if(iMVSFixtureModuTool==null)
                        {
                            return false;
                        }
                        if (iMVSFixtureModuTool.ModuResult.ModuStatus == 0)
                        {
                            MarkedWord = "模板 - " + this.Name + " 粗定位模块失败。";
                        }

                        switch (ExactPRType[0])
                        {
                            case 0:
                                IMVSFastFeatureMatchModuTool iMVSFastFeatureMatchModuTool = (IMVSFastFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.快速匹配2"];
                                FastFeatureMatchResult FastRes = iMVSFastFeatureMatchModuTool.ModuResult;
                                MatchRectROI = FastRes.ROI;
                                break;
                            case 1:
                                IMVSHPFeatureMatchModuTool iMVSHPFeatureMatchModuTool = (IMVSHPFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.高精度匹配2"];
                                HPFeatureMatchResult HPRes = iMVSHPFeatureMatchModuTool.ModuResult;
                                MatchRectROI = HPRes.ROI;
                                break;
                            case 2:
                                IMVSGrayMatchModuVATool iMVSGrayMatchModuVATool = (IMVSGrayMatchModuVATool)VmSolution.Instance[$"{this.VmProcedure.FullName}.灰度匹配2"];
                                GrayMatchVAResult GrayRes = iMVSGrayMatchModuVATool.ModuResult;
                                MatchRectROI = GrayRes.ROI;
                                break;
                            case 3:
                                IMVSContourMatchModuTool iMVSContourMatchModuTool = (IMVSContourMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.轮廓匹配2"];
                                ContourMatchResult ContourRes = iMVSContourMatchModuTool.ModuResult;
                                MatchRectROI = ContourRes.ROI;
                                break;
                        }

                        #region 绘制检测区域
                        HTuple angle = MatchRectROI.Angle;
                        HOperatorSet.GenRectangle2(out regionZoom, MatchRectROI.CenterPoint.Y, MatchRectROI.CenterPoint.X, -angle.TupleRad(), MatchRectROI.BoxWidth / 2, MatchRectROI.BoxHeight / 2);
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
                        graph.Dispose();
                        #endregion
                    }
                    else
                    {
                        HOperatorSet.GenEmptyObj(out imageResult);
                        imageResult.Dispose();
                        HOperatorSet.Compose3(this.SourceImg, this.SourceImg, this.SourceImg, out imageResult);

                        // 绘制文字
                        bitmap = ImageHelp.HObjectToBitmap(imageResult);
                        bitmapResult = new Bitmap(bitmap);
                        graph = Graphics.FromImage(bitmapResult);
                        graph.DrawString("该物体和模板不匹配", new Font("宋体", 15), new SolidBrush(Color.LightGreen), new Point(0, 0));
                        bitmap.Dispose();
                        graph.Dispose();
                    }

                    this.MatchResults.Add(new MatchResult()
                    {
                        IsSuccess = false,
                        OutPutImg1 = bitmapResult,
                        MarkedWords = MarkedWord,
                    });
                }
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
                LogHelper.Post(Level.Error, $"{this.Name}: PreventReverseAlg 异常：{ex.Message} ", ex, Log.LogCategory.PR);
                return false;
            }
        }
        
        
    }
}
