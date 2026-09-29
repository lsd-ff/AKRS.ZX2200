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
using IMVSLineFindModuCs;
using IMVSRectFindModuCs;

namespace AKRS.Galaxy2.PR.Models.Algs
{

    /// <summary>
    /// 矩形检测方法，输出图像为矩形、中心，输出文字为中心坐标、直线角度
    /// </summary>
    [Serializable]
    public class RectangleSecondDetectAlg : BaseAlg
    {
        /// <summary>
        /// 粗定位方式
        /// </summary>
        public int[] CrudePRType = new int[1];

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        public RectangleSecondDetectAlg(string name)
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

                //FloatDataArray Height = ProcedureResult.GetOutputFloat("1ResultHeight");
                //float[] heightResults = Height.pFloatVal == null ? null : Height.pFloatVal;               
                
                //FloatDataArray Width = ProcedureResult.GetOutputFloat("1ResultWidth");
                //float[] widthResults = Width.pFloatVal == null ? null : Width.pFloatVal;

                #region Bitmap
                IMVSRectFindModuTool RectFindModuTool = (IMVSRectFindModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.矩形检测1"];
                if (RectFindModuTool == null)
                {
                    return false;
                }
                RectFindResult QuadrangleRes = RectFindModuTool.ModuResult;

                IMVSLineFindModuTool LineFindModuTool = (IMVSLineFindModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.直线查找1"];

                LineFindResult LineRes = LineFindModuTool.ModuResult;

                HObject regionZoom, imageResult1, imageResult2, imageResult3, imageResult;
                HOperatorSet.GenEmptyObj(out regionZoom);
                HOperatorSet.GenEmptyObj(out imageResult1);
                HOperatorSet.GenEmptyObj(out imageResult2);
                HOperatorSet.GenEmptyObj(out imageResult3);
                HOperatorSet.GenEmptyObj(out imageResult);
                Bitmap bitmap, bitmapResult;
                Graphics graph;

                if (xResults != null && xResults.Count() != 0 && angleResults != null && angleResults.Count() != 0)
                {
                    
                    StringBuilder stringBuilder = new StringBuilder();

                    switch (CrudePRType[0])
                    {
                        case 0:
                            IMVSFastFeatureMatchModuTool iMVSFastFeatureMatchModuTool = (IMVSFastFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.快速匹配1"];
                            FastFeatureMatchResult FastRes = iMVSFastFeatureMatchModuTool.ModuResult;
                            stringBuilder.Append("快速匹配框中心:(" + FastRes.MatchRect[0].CenterPoint.X + "," + FastRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                            stringBuilder.Append("角度:" + FastRes.MatchRect[0].Angle + ",分数:" + FastRes.MatchScore[0] + "\n");
                            break;
                        case 1:
                            IMVSHPFeatureMatchModuTool iMVSHPFeatureMatchModuTool = (IMVSHPFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.高精度匹配1"];
                            HPFeatureMatchResult HPRes = iMVSHPFeatureMatchModuTool.ModuResult;
                            stringBuilder.Append("高精度匹配框中心:(" + HPRes.MatchRect[0].CenterPoint.X + "," + HPRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
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
                    HObject regionline1, regionline2, line1, recRegion;
                    HOperatorSet.GenEmptyObj(out regionline1);
                    HOperatorSet.GenEmptyObj(out regionline2);
                    HOperatorSet.GenEmptyObj(out line1);
                    HOperatorSet.GenEmptyObj(out recRegion);

                    HTuple a1 = new HTuple(), r1 = new HTuple(), c1 = new HTuple();

                    HTuple angle = QuadrangleRes.RectBox.Angle;
                    HOperatorSet.GenRectangle2(out recRegion, QuadrangleRes.RectBox.CenterPoint.Y, QuadrangleRes.RectBox.CenterPoint.X, -angle.TupleRad(), QuadrangleRes.RectBox.BoxWidth / 2, QuadrangleRes.RectBox.BoxHeight / 2);
                    HOperatorSet.ZoomRegion(recRegion, out recRegion, 0.25, 0.25);
                    HOperatorSet.ConcatObj(regionZoom, recRegion, out regionZoom);

                    // 绘制线
                    HOperatorSet.GenRegionLine(out line1, LineRes.OutputLine.StartPoint.Y / 4, LineRes.OutputLine.StartPoint.X / 4, LineRes.OutputLine.EndPoint.Y / 4, LineRes.OutputLine.EndPoint.X / 4);
                    HOperatorSet.ConcatObj(regionZoom, line1, out regionZoom);
                    line1.Dispose();

                    // 输出矩形检测的长宽,X---高，Y----宽
                    if (IsUseRecLength)
                    {
                        xResults = new float[] { QuadrangleRes.RectBox.BoxHeight }; 

                        yResults = new float[] { QuadrangleRes.RectBox.BoxWidth };
                        
                        stringBuilder.Append("矩形高:" + QuadrangleRes.RectBox.BoxHeight + ", 矩形宽:" + QuadrangleRes.RectBox.BoxWidth + "\n");
                    }
                    else
                    {
                        stringBuilder.Append("矩形中心点:(" + QuadrangleRes.RectBox.CenterPoint.X + "," + QuadrangleRes.RectBox.CenterPoint.Y + ")" + "\n");
                    }
                    
                    // 输出矩形检测的长宽,X---高，Y----宽
                    if (IsUseRecLength)
                    {
                        // heightResults = new float[] { QuadrangleRes.RectBox.BoxHeight };

                        // widthResults = new float[] { QuadrangleRes.RectBox.BoxWidth };
                        
                        stringBuilder.Append("矩形高:" + QuadrangleRes.RectBox.BoxHeight + ", 矩形宽:" + QuadrangleRes.RectBox.BoxWidth + "\n");
                    }
                    else
                    {
                        stringBuilder.Append("矩形中心点:(" + QuadrangleRes.RectBox.CenterPoint.X + "," + QuadrangleRes.RectBox.CenterPoint.Y + ")" + "\n");
                    }
                    
                    stringBuilder.Append("直线角度:" + LineRes.OutputLine.Angle + "\n");
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
                            Height = QuadrangleRes.RectBox.BoxHeight,
                            Width = QuadrangleRes.RectBox.BoxWidth,
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
                    #region 绘制检测区域
                    HObject ROI1, ROI2, ROI3;
                    HOperatorSet.GenEmptyObj(out ROI1);
                    HOperatorSet.GenEmptyObj(out ROI2);
                    HOperatorSet.GenEmptyObj(out ROI3);

                    if (iMVSFixtureModuTool.ModuResult.ModuStatus == 0)
                    {
                        MarkedWord = "模板 - " + this.Name + " 粗定位模块失败。"; 
                        switch (CrudePRType[0])
                        {
                            case 0:
                                IMVSFastFeatureMatchModuTool iMVSFastFeatureMatchModuTool = (IMVSFastFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.快速匹配1"];
                                FastFeatureMatchResult FastRes = iMVSFastFeatureMatchModuTool.ModuResult;
                                HTuple angle3 = FastRes.ROI.Angle;
                                HOperatorSet.GenRectangle2(out ROI3, FastRes.ROI.CenterPoint.Y, FastRes.ROI.CenterPoint.X, -angle3.TupleRad(), FastRes.ROI.BoxWidth / 2, FastRes.ROI.BoxHeight / 2);
                                break;
                            case 1:
                                IMVSHPFeatureMatchModuTool iMVSHPFeatureMatchModuTool = (IMVSHPFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.高精度匹配1"];
                                HPFeatureMatchResult HPRes = iMVSHPFeatureMatchModuTool.ModuResult;
                                HTuple angle4 = HPRes.ROI.Angle;
                                HOperatorSet.GenRectangle2(out ROI3, HPRes.ROI.CenterPoint.Y, HPRes.ROI.CenterPoint.X, -angle4.TupleRad(), HPRes.ROI.BoxWidth / 2, HPRes.ROI.BoxHeight / 2);

                                break;
                            case 2:
                                IMVSGrayMatchModuVATool iMVSGrayMatchModuVATool = (IMVSGrayMatchModuVATool)VmSolution.Instance[$"{this.VmProcedure.FullName}.灰度匹配1"];
                                GrayMatchVAResult GrayRes = iMVSGrayMatchModuVATool.ModuResult;
                                HTuple angle5 = GrayRes.ROI.Angle;
                                HOperatorSet.GenRectangle2(out ROI3, GrayRes.ROI.CenterPoint.Y, GrayRes.ROI.CenterPoint.X, -angle5.TupleRad(), GrayRes.ROI.BoxWidth / 2, GrayRes.ROI.BoxHeight / 2);
                                break;
                            case 3:
                                IMVSContourMatchModuTool iMVSContourMatchModuTool = (IMVSContourMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.轮廓匹配1"];
                                ContourMatchResult ContourRes = iMVSContourMatchModuTool.ModuResult;
                                HTuple angle6 = ContourRes.ROI.Angle;
                                HOperatorSet.GenRectangle2(out ROI3, ContourRes.ROI.CenterPoint.Y, ContourRes.ROI.CenterPoint.X, -angle6.TupleRad(), ContourRes.ROI.BoxWidth / 2, ContourRes.ROI.BoxHeight / 2);
                                break;
                        }
                        HOperatorSet.ConcatObj(regionZoom, ROI3, out regionZoom);
                    }
                    else
                    {
                        if (QuadrangleRes.ModuStatus == 0)
                        {
                            MarkedWord = "模板 - " + this.Name + " 矩形检测模块失败。";
                            HTuple angle1 = QuadrangleRes.ROI.Angle;
                            HOperatorSet.GenRectangle2(out ROI1, QuadrangleRes.ROI.CenterPoint.Y, QuadrangleRes.ROI.CenterPoint.X, -angle1.TupleRad(), QuadrangleRes.ROI.BoxWidth / 2, QuadrangleRes.ROI.BoxHeight / 2);
                            HOperatorSet.ConcatObj(regionZoom, ROI1, out regionZoom);
                        }

                        if (LineRes.ModuStatus == 0)
                        {
                            MarkedWord = "模板 - " + this.Name + " 线检测模块失败。";
                            HTuple angle2 = LineRes.ROI.Angle;
                            HOperatorSet.GenRectangle2(out ROI2, LineRes.ROI.CenterPoint.Y, LineRes.ROI.CenterPoint.X, -angle2.TupleRad(), LineRes.ROI.BoxWidth / 2, LineRes.ROI.BoxHeight / 2);
                            HOperatorSet.ConcatObj(regionZoom, ROI2, out regionZoom);
                        }
                    }

                    ROI1.Dispose();
                    ROI2.Dispose();
                    ROI3.Dispose();

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
                LogHelper.Post(Level.Error, $"{this.Name}: RectangleSecondDetectAlg 异常：{ex.Message} ", ex, Log.LogCategory.PR);
                return false;
            }
        }
    }
}
