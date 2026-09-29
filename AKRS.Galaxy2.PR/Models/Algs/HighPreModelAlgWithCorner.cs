using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.PR.Models.Algs;
using ImageSourceModuleCs;
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
using AKRS.Galaxy2.PR.Models.MatchResults;
using VM.Core;
using VM.PlatformSDKCS;
using IMVSFixtureModuCs;
using IMVSHPFeatureMatchModuCs;
using VMControls.Interface;
using AKRS.Galaxy2.PR.Models.Services;
using HalconDotNet;
using IMVSBlobFindModuCs;
using GraphicsSetModuleCs;
using IMVSGroupCs;
using IMVSContourMatchModuCs;
using IMVSEdgeFlawInspModuCs;

namespace AKRSAKRS.Galaxy2.PR.Models.Algs
{
    /// <summary>
    /// 带缺角检测的轮廓匹配方法，输出定位框和Blob区域、中心点
    /// </summary>
    [Serializable]
    public class HighPreModelAlgWithCorner : BaseAlg
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">算法流程名称</param>
        public HighPreModelAlgWithCorner(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 执行带缺角检测的轮廓匹配流程，获取结果
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

                this.VmProcedure.Run();
                ProcedureResult = this.VmProcedure.ModuResult;

                FloatDataArray Centerx = ProcedureResult.GetOutputFloat("ResX");
                float[] xResults = Centerx.pFloatVal == null ? null : Centerx.pFloatVal;

                FloatDataArray Centery = ProcedureResult.GetOutputFloat("ResY");
                float[] yResults = Centery.pFloatVal == null ? null : Centery.pFloatVal;

                FloatDataArray Angle = ProcedureResult.GetOutputFloat("ResA");
                float[] angleResults = Angle.pFloatVal == null ? null : Angle.pFloatVal;

                FloatDataArray Score = ProcedureResult.GetOutputFloat("ResS");
                float[] scoreResults = Score.pFloatVal == null ? null : Score.pFloatVal;


                #region Bitmap

                IMVSEdgeFlawInspModuTool iMVSEdgeFlawInspModuTool = (IMVSEdgeFlawInspModuTool)VmSolution.Instance[$"{this.GetVmProcedure().FullName}.边缘模型缺陷检测1"];
                if (iMVSEdgeFlawInspModuTool == null)
                {
                    return false;
                }
                EdgeFlawInspResult EdgeResult = iMVSEdgeFlawInspModuTool.ModuResult;

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

                // 定位到、缺角检测执行成功、没有缺角
                if (EdgeResult.ModuStatus==1&& EdgeResult .DefectBox.Count==0&& xResults != null&&xResults.Count()>0)
                {
                    List<RectBox> rectBoxes = new List<RectBox>();

                    // 定位匹配框
                    rectBoxes = ContourRes.MatchRect;
                    StringBuilder stringBuilder = new StringBuilder();
                    #region 绘制图像
                    HObject regionline1, regionline2, region, recRegion, zoomReg;
                    HOperatorSet.GenEmptyObj(out regionline1);
                    HOperatorSet.GenEmptyObj(out regionline2);
                    HOperatorSet.GenEmptyObj(out region);
                    HOperatorSet.GenEmptyObj(out recRegion);
                    HOperatorSet.GenEmptyObj(out zoomReg);

                    // 绘制中心线
                    HTuple r1 = new HTuple(), c1 = new HTuple();
                    for (int i = 0; i < rectBoxes.Count; i++)
                    {
                        r1 = rectBoxes[i].CenterPoint.Y / 4;
                        c1 = rectBoxes[i].CenterPoint.X / 4;

                        HOperatorSet.GenRegionLine(out regionline1, r1 - 5, c1, r1 + 5, c1);
                        HOperatorSet.GenRegionLine(out regionline2, r1, c1 - 5, r1, c1 + 5);
                        HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);
                        HOperatorSet.ConcatObj(regionZoom, regionline2, out regionZoom);

                        HTuple angle = rectBoxes[i].Angle;
                        HOperatorSet.GenRectangle2(out recRegion, rectBoxes[i].CenterPoint.Y, rectBoxes[i].CenterPoint.X, -angle.TupleRad(), rectBoxes[i].BoxWidth / 2, rectBoxes[i].BoxHeight / 2);
                        HOperatorSet.ZoomRegion(recRegion, out zoomReg, 0.25, 0.25);
                        HOperatorSet.ConcatObj(regionZoom, zoomReg, out regionZoom);

                        stringBuilder.Append("匹配框中心:(" + xResults[i] + "," + yResults[i] + ")" + "\n");
                        stringBuilder.Append("角度:" + angleResults[i] + ",分数:" + scoreResults[i] + "\n");
                    }

                    // 轮廓点

                    var outlinePoints = ContourRes.MatchOutline.MatchOutlinePoints;
                    List<HTuple> xList = new List<HTuple>();
                    List<HTuple> yList = new List<HTuple>();

                    for (int i = 0; i < outlinePoints.Count; i += 8)
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
                    recRegion.Dispose();
                    zoomReg.Dispose();
                    #endregion

                    this.MatchResults = new List<BaseAlgResult>();
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
                    // 未定位到,false
                    // 缺陷检测模块失败，false
                    StringBuilder stringBuilder = new StringBuilder();
                    HObject tempRegion, zoomRegion;
                    HOperatorSet.GenEmptyObj(out tempRegion);
                    HOperatorSet.GenEmptyObj(out zoomRegion);

                    // 定位失败
                    if (ContourRes.ModuStatus == 0)
                    {
                        stringBuilder.Append("模板 - " + this.Name + " 定位模块失败。" + "\n");
                        HTuple angle1 = new HTuple();
                        angle1 = ContourRes.ROI.Angle;
                        HOperatorSet.GenRectangle2(out tempRegion, ContourRes.ROI.CenterPoint.Y, ContourRes.ROI.CenterPoint.X, -angle1.TupleRad(), ContourRes.ROI.BoxWidth / 2, ContourRes.ROI.BoxHeight / 2);
                        HOperatorSet.ZoomRegion(tempRegion, out zoomRegion, 0.25, 0.25);
                        HOperatorSet.ConcatObj(regionZoom, zoomRegion, out regionZoom);
                    }
                    
                    if(EdgeResult.ModuStatus == 0)
                    {
                        //有缺陷
                        if (EdgeResult.DefectBox.Count >= 1)
                        {
                            stringBuilder.Append("模板 - " + this.Name + " 边缘存在"+ EdgeResult.DefectBox.Count + "个缺陷。" + "\n");
                            List<RectBox> rectBoxes = new List<RectBox>();
                            rectBoxes = EdgeResult.DefectBox;
                            for (int i = 0; i < rectBoxes.Count; i++)
                            {
                                HTuple angle = rectBoxes[i].Angle;
                                HOperatorSet.GenRectangle2(out tempRegion, rectBoxes[i].CenterPoint.Y, rectBoxes[i].CenterPoint.X, -angle.TupleRad(), rectBoxes[i].BoxWidth / 2, rectBoxes[i].BoxHeight / 2);
                                HOperatorSet.ZoomRegion(tempRegion, out zoomRegion, 0.25, 0.25);
                                HOperatorSet.ConcatObj(regionZoom, zoomRegion, out regionZoom);
                            }
                        }
                        else
                        {
                            // 缺陷检测模块失败
                            stringBuilder.Append("模板 - " + this.Name + " 边缘缺陷检测模块失败。" + "\n");
                            if (EdgeResult.CaliperBox.Count!=0)
                            {
                                for (int i = 0; i < EdgeResult.CaliperBox.Count; i++)
                                {
                                    HTuple angle1 = EdgeResult.CaliperBox[i].Angle;
                                    HOperatorSet.GenRectangle2(out tempRegion, EdgeResult.CaliperBox[i].CenterPoint.Y, EdgeResult.CaliperBox[i].CenterPoint.X, -angle1.TupleRad(), EdgeResult.CaliperBox[i].BoxWidth / 2, EdgeResult.CaliperBox[i].BoxHeight / 2);
                                    HOperatorSet.ZoomRegion(tempRegion, out zoomRegion, 0.25, 0.25);
                                    HOperatorSet.ConcatObj(regionZoom, zoomRegion, out regionZoom);
                                }
                            }
                                                   
                        }
                    }
                   
                    tempRegion.Dispose();
                    zoomRegion.Dispose();

                    if (regionZoom.CountObj() != 0) 
                    {
                        HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult1, 255, "margin");
                        HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult2, 0, "margin");
                        HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult3, 0, "margin");
                        HOperatorSet.Compose3(imageResult1, imageResult2, imageResult3, out imageResult);
                    }
                    else
                    {
                        HOperatorSet.Compose3(this.SourceImg, this.SourceImg, this.SourceImg, out imageResult);
                    }
                    
                    bitmap = ImageHelp.HObjectToBitmap(imageResult);
                    bitmapResult = new Bitmap(bitmap);
                    graph = Graphics.FromImage(bitmapResult);
                    graph.DrawString(stringBuilder.ToString(), new Font("宋体", 15), new SolidBrush(Color.Red), new Point(0, 0));
                    bitmap.Dispose();
                    #endregion
                    this.MatchResults.Add(new MatchResult()
                    {
                        IsSuccess = false,
                        OutPutImg1 = bitmapResult,
                        MarkedWords = stringBuilder.ToString(),
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
                LogHelper.Post(Level.Error, $"{this.Name}: HighPreModelAlgWithCorner异常：{ex.Message} ", ex, LogCategory.PR);
                return false;
            }

        }
    }
}
