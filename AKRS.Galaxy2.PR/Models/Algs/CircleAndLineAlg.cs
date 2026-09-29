using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Models.Services;
using HalconDotNet;
using ImageSourceModuleCs;
using IMVSCircleFindModuCs;
using IMVSContourMatchModuCs;
using IMVSFastFeatureMatchModuCs;
using IMVSFixtureModuCs;
using IMVSGrayMatchModuVACs;
using IMVSHPFeatureMatchModuCs;
using IMVSLineFitModuCs;
using log4net.Core;
using SaveImageCs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VM.Core;
using VM.PlatformSDKCS;
using VMControls.WPF;


namespace AKRS.Galaxy2.PR.Models.Algs
{
    /// <summary>
    /// 圆心和角度检测方法,输出图像为圆心和线，输出文字为圆心、直线角度
    /// </summary>
    [Serializable]
    public class CircleAndLineAlg : BaseAlg
    {

        /// <summary>
        /// 粗定位方式
        /// </summary>
        public int[] CrudePRType = new int[1];

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        public CircleAndLineAlg(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 执行线检测流程，获取结果
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
                ImageBaseData saveImageResult = SaveImageTool.ModuResult.OutputImage;
                ProcedureResult = this.VmProcedure.ModuResult;

                FloatDataArray Centerx = ProcedureResult.GetOutputFloat("ReasultX");
                float[]  xResults = Centerx.pFloatVal == null ? null : Centerx.pFloatVal;

                FloatDataArray Centery = ProcedureResult.GetOutputFloat("ReasultY");
                float[]  yResults = Centery.pFloatVal == null ? null : Centery.pFloatVal;

                FloatDataArray Angle = ProcedureResult.GetOutputFloat("ReasultA");
                float[]  angleResults = Angle.pFloatVal == null ? null : Angle.pFloatVal;


                IMVSCircleFindModuTool CircleFindModuTool = (IMVSCircleFindModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.圆查找1"];
                CircleFindResult CircleRes = CircleFindModuTool.ModuResult;
                IMVSLineFitModuTool LineFindModuTool
                     = (IMVSLineFitModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.直线拟合1"];
                LineFitResult LineRes = LineFindModuTool.ModuResult;

                HObject regionZoom, imageResult1, imageResult2, imageResult3, imageResult;
                HOperatorSet.GenEmptyObj(out regionZoom);
                HOperatorSet.GenEmptyObj(out imageResult1);
                HOperatorSet.GenEmptyObj(out imageResult2);
                HOperatorSet.GenEmptyObj(out imageResult3);
                HOperatorSet.GenEmptyObj(out imageResult);
                Bitmap bitmap, bitmapResult;
                Graphics graph;

                if (xResults != null && xResults.Count() != 0&& angleResults!=null&& angleResults.Count()!=0)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    switch (CrudePRType[0])
                    {
                        case 0:
                            IMVSFastFeatureMatchModuTool iMVSFastFeatureMatchModuTool = (IMVSFastFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.快速匹配1"];
                            FastFeatureMatchResult FastRes = iMVSFastFeatureMatchModuTool.ModuResult;
                            break;

                        case 1:
                            IMVSHPFeatureMatchModuTool iMVSHPFeatureMatchModuTool = (IMVSHPFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.高精度匹配1"];
                            HPFeatureMatchResult HPRes = iMVSHPFeatureMatchModuTool.ModuResult;
                            break;

                        case 2:
                            IMVSGrayMatchModuVATool iMVSGrayMatchModuVATool = (IMVSGrayMatchModuVATool)VmSolution.Instance[$"{this.VmProcedure.FullName}.灰度匹配1"];
                            GrayMatchVAResult GrayRes = iMVSGrayMatchModuVATool.ModuResult;
                            break;

                        case 3:
                            IMVSContourMatchModuTool iMVSContourMatchModuTool = (IMVSContourMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.轮廓匹配1"];
                            ContourMatchResult ContourRes = iMVSContourMatchModuTool.ModuResult;
                            break;
                    }


                    #region 绘制图像
                    HObject regionline1, regionline2, region;
                    HOperatorSet.GenEmptyObj(out regionline1);
                    HOperatorSet.GenEmptyObj(out regionline2);
                    HOperatorSet.GenEmptyObj(out region);
                    HTuple r1 = new HTuple(), c1 = new HTuple();
                    r1 = CircleRes.OutputCircle.CenterPoint.Y / 4;
                    c1 = CircleRes.OutputCircle.CenterPoint.X / 4;

                    // 绘制中心线        
                    HOperatorSet.GenRegionLine(out regionline1, r1 - 5, c1, r1 + 5, c1);
                    HOperatorSet.GenRegionLine(out regionline2, r1, c1 - 5, r1, c1 + 5);
                    HOperatorSet.ConcatObj(regionZoom, regionline2, out regionZoom);
                    HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);

                    // 绘制直线
                    HOperatorSet.GenRegionLine(out regionline1, LineRes.OutputLine.StartPoint.Y / 4, LineRes.OutputLine.StartPoint.X / 4, LineRes.OutputLine.EndPoint.Y / 4, LineRes.OutputLine.EndPoint.X / 4);
                    HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);

                    // 圆
                    stringBuilder.Append("圆心1:(" + CircleRes.OutputCircle.CenterPoint.X + "," + CircleRes.OutputCircle.CenterPoint.Y + ")\n");
                    stringBuilder.Append("线角度:" + LineRes.LineAngle + "\n");
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

                        HTuple angle1 = MatchRectROI.Angle;
                        HOperatorSet.GenRectangle2(out regionZoom, MatchRectROI.CenterPoint.Y, MatchRectROI.CenterPoint.X, -angle1.TupleRad(), MatchRectROI.BoxWidth / 2, MatchRectROI.BoxHeight / 2);
                    }
                    else
                    {
                        #region 绘制检测区域

                        // 圆检测模块失败                
                        IMVSCircleFindModuTool CircleFindModuTool1 = (IMVSCircleFindModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.圆查找1"];
                        IMVSCircleFindModuTool CircleFindModuTool2 = (IMVSCircleFindModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.圆查找2"];
                        IMVSCircleFindModuTool CircleFindModuTool3 = (IMVSCircleFindModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.圆查找3"];
                        IMVSCircleFindModuTool CircleFindModuTool4 = (IMVSCircleFindModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.圆查找4"];
                        CircleRes = null;
                        if (CircleFindModuTool1.ModuResult.ModuStatus == 0)
                        {
                            MarkedWord = "模板 - " + this.Name + " 圆1搜索模块失败。";
                            CircleRes = CircleFindModuTool1.ModuResult;
                        }
                        else if (CircleFindModuTool2.ModuResult.ModuStatus == 0)
                        {
                            MarkedWord = "模板 - " + this.Name + " 圆2搜索模块失败。";
                            CircleRes = CircleFindModuTool2.ModuResult;
                        }
                        else if(CircleFindModuTool3.ModuResult.ModuStatus == 0)
                        {
                            MarkedWord = "模板 - " + this.Name + " 圆3搜索模块失败。";
                            CircleRes = CircleFindModuTool3.ModuResult;
                        }
                        else if(CircleFindModuTool4.ModuResult.ModuStatus == 0)
                        {
                            MarkedWord = "模板 - " + this.Name + " 圆4搜索模块失败。";
                            CircleRes = CircleFindModuTool4.ModuResult;
                        }
                        HObject region;
                        HOperatorSet.GenEmptyObj(out region);
                        if (CircleRes != null)
                        {
                            HTuple angle = CircleRes.ROI.Angle;
                            region.Dispose();
                            HOperatorSet.GenRectangle2(out region, CircleRes.ROI.CenterPoint.Y, CircleRes.ROI.CenterPoint.X, -angle.TupleRad(), CircleRes.ROI.BoxWidth / 2, CircleRes.ROI.BoxHeight / 2);
                            HOperatorSet.ConcatObj(regionZoom, region, out regionZoom);
                            region.Dispose();
                        }
                        else
                        {

                            // 直线查找模块失败  
                            MarkedWord = "模板 - " + this.Name + " 直线拟合模块失败。";                         
                        }                 
                    }
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
                return false;

                 
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"{this.Name}: CircleAndLineAlg 异常：{ex.Message} ", ex, Log.LogCategory.PR);
                return false;
            }
        }
    }
}
