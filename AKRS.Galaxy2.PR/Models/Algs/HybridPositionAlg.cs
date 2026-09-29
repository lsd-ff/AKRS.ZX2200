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
    /// 组合定位方法，输出图像为匹配框、中心、两直线，输出文字为匹配中心、直线平均角度
    /// </summary>
    [Serializable]
    public class HybridPositionAlg : BaseAlg
    {


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        public HybridPositionAlg(string name)
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


                this.VmProcedure.Run();

                ProcedureResult = this.VmProcedure.ModuResult;

                FloatDataArray Centerx = ProcedureResult.GetOutputFloat("1ReasultX");
                float[] xResults = Centerx.pFloatVal == null ? null : Centerx.pFloatVal;

                FloatDataArray Centery = ProcedureResult.GetOutputFloat("1ReasultY");
                float[] yResults = Centery.pFloatVal == null ? null : Centery.pFloatVal;

                FloatDataArray Angle = ProcedureResult.GetOutputFloat("1ReasultA");
                float[] angleResults = Angle.pFloatVal == null ? null : Angle.pFloatVal;

                #region Bitmap

                IMVSLineFindModuTool LineFindModuTool = (IMVSLineFindModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.直线查找1"];
                if (LineFindModuTool == null)
                {
                    return false;
                }
                LineFindResult LineRes = LineFindModuTool.ModuResult;

                IMVSLineFindModuTool LineFindModuTool2 = (IMVSLineFindModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.直线查找2"];

                LineFindResult LineRes2 = LineFindModuTool2.ModuResult;

                IMVSContourMatchModuTool iMVSContourMatchModuTool = (IMVSContourMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.轮廓匹配1"];

                ContourMatchResult ContourRes = iMVSContourMatchModuTool.ModuResult;

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

                    stringBuilder.Append("轮廓定位匹配框中心:(" + ContourRes.MatchRect[0].CenterPoint.X + "," + ContourRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                    stringBuilder.Append("角度:" + ContourRes.MatchRect[0].Angle + ",分数:" + ContourRes.MatchScore[0] + "\n");

                    #region 绘制图像
                    HObject regionline1, regionline2, line1, recRegion;
                    HOperatorSet.GenEmptyObj(out regionline1);
                    HOperatorSet.GenEmptyObj(out regionline2);
                    HOperatorSet.GenEmptyObj(out line1);
                    HOperatorSet.GenEmptyObj(out recRegion);

                    HTuple a1 = new HTuple(), r1 = new HTuple(), c1 = new HTuple();
                    HTuple angle = ContourRes.MatchRect[0].Angle;
                    HOperatorSet.GenRectangle2(out recRegion, ContourRes.MatchRect[0].CenterPoint.Y, ContourRes.MatchRect[0].CenterPoint.X, -angle.TupleRad(), ContourRes.MatchRect[0].BoxWidth / 2, ContourRes.MatchRect[0].BoxHeight / 2);
                    HOperatorSet.ZoomRegion(recRegion, out recRegion, 0.25, 0.25);
                    r1 = ContourRes.MatchRect[0].CenterPoint.Y / 4;
                    c1 = ContourRes.MatchRect[0].CenterPoint.X / 4;

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


                    // 绘制线
                    HOperatorSet.GenRegionLine(out line1, LineRes.OutputLine.StartPoint.Y / 4, LineRes.OutputLine.StartPoint.X / 4, LineRes.OutputLine.EndPoint.Y / 4, LineRes.OutputLine.EndPoint.X / 4);
                    HOperatorSet.ConcatObj(regionZoom, line1, out regionZoom);
                    line1.Dispose();

                    HOperatorSet.GenRegionLine(out recRegion, LineRes2.OutputLine.StartPoint.Y / 4, LineRes2.OutputLine.StartPoint.X / 4, LineRes2.OutputLine.EndPoint.Y / 4, LineRes2.OutputLine.EndPoint.X / 4);
                    HOperatorSet.ConcatObj(regionZoom, recRegion, out regionZoom);
                    recRegion.Dispose();

                    stringBuilder.Append("直线平均角度:" + angleResults[0] + "\n");

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
                    #region 绘制检测区域
                    HObject ROI1, ROI2, ROI3;
                    HOperatorSet.GenEmptyObj(out ROI1);
                    HOperatorSet.GenEmptyObj(out ROI2);
                    HOperatorSet.GenEmptyObj(out ROI3);

                    if (iMVSFixtureModuTool.ModuResult.ModuStatus == 0)
                    {
                        MarkedWord = "模板 - " + this.Name + " 粗定位模块失败。";

                        ContourRes = iMVSContourMatchModuTool.ModuResult;
                        HTuple angle6 = ContourRes.ROI.Angle;
                        HOperatorSet.GenRectangle2(out ROI3, ContourRes.ROI.CenterPoint.Y, ContourRes.ROI.CenterPoint.X, -angle6.TupleRad(), ContourRes.ROI.BoxWidth / 2, ContourRes.ROI.BoxHeight / 2);
                        HOperatorSet.ConcatObj(regionZoom, ROI3, out regionZoom);
                    }
                    else
                    {

                        if (LineRes.ModuStatus == 0)
                        {
                            MarkedWord = "模板 - " + this.Name + " 线1检测模块失败。";
                            HTuple angle2 = LineRes.ROI.Angle;
                            HOperatorSet.GenRectangle2(out ROI2, LineRes.ROI.CenterPoint.Y, LineRes.ROI.CenterPoint.X, -angle2.TupleRad(), LineRes.ROI.BoxWidth / 2, LineRes.ROI.BoxHeight / 2);
                            HOperatorSet.ConcatObj(regionZoom, ROI2, out regionZoom);
                        }

                        if (LineRes2.ModuStatus == 0)
                        {
                            MarkedWord = "模板 - " + this.Name + " 线2检测模块失败。";
                            HTuple angle2 = LineRes2.ROI.Angle;
                            HOperatorSet.GenRectangle2(out ROI1, LineRes2.ROI.CenterPoint.Y, LineRes2.ROI.CenterPoint.X, -angle2.TupleRad(), LineRes2.ROI.BoxWidth / 2, LineRes2.ROI.BoxHeight / 2);
                            HOperatorSet.ConcatObj(regionZoom, ROI1, out regionZoom);
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
                LogHelper.Post(Level.Error, $"{this.Name}: HybridPositionAlg 异常：{ex.Message} ", ex, Log.LogCategory.PR);
                return false;
            }
        }
    }
}
