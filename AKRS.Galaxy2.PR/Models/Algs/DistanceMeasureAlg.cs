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
using IMVSHPFeatureMatchModuCs;
using IMVSCaliperCornerModuCs;

namespace AKRS.Galaxy2.PR.Models.Algs
{
    /// <summary>
    /// 距离测量方法
    /// </summary>
    [Serializable]
    public class DistanceMeasure : BaseAlg
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        public DistanceMeasure(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 执行距离测量流程，获取结果
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

                IMVSHPFeatureMatchModuTool HPFeatureMatchModuTool =
               (IMVSHPFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.高精度匹配1"];
                if (HPFeatureMatchModuTool == null)
                {
                    return false;
                }
                HPFeatureMatchResult HP = HPFeatureMatchModuTool.ModuResult;

                IMVSHPFeatureMatchModuTool HPFeatureMatchModuTool2 =
                (IMVSHPFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.高精度匹配2"];
                HPFeatureMatchResult HP2 = HPFeatureMatchModuTool2.ModuResult;

                float[] xResults = new float[2];
                float[] yResults = new float[2];
                float[] angleResults = new float[2];
                float[] scoreResults = new float[2];
                if (HP.ModuStatus == 1&& HP2.ModuStatus==1)
                {

                    FloatDataArray Centerx1=ProcedureResult.GetOutputFloat("1ReasultX");
                    FloatDataArray Centery1 = ProcedureResult.GetOutputFloat("1ReasultY");
                    FloatDataArray Angle1 = ProcedureResult.GetOutputFloat("1ReasultA");
                    FloatDataArray Score1 = ProcedureResult.GetOutputFloat("1ReasultS");
                    FloatDataArray Centerx2 = ProcedureResult.GetOutputFloat("2ReasultX");
                    FloatDataArray Centery2 = ProcedureResult.GetOutputFloat("2ReasultY");
                    FloatDataArray Angle2 = ProcedureResult.GetOutputFloat("2ReasultA");
                    FloatDataArray Score2 = ProcedureResult.GetOutputFloat("2ReasultS");

                    if (Centerx1.nValueNum > 0) 
                    { 
                        xResults[0] = Centerx1.pFloatVal[0]; 
                    }
                    if (Centery1.nValueNum > 0) 
                    {
                        yResults[0] = Centery1.pFloatVal[0]; 
                    }
                    if (Angle1.nValueNum > 0) 
                    { 
                        angleResults[0] = Angle1.pFloatVal[0]; 
                    }
                    if (Score1.nValueNum > 0) 
                    { 
                        scoreResults[0] = Score1.pFloatVal[0]; 
                    }
                    if (Centerx2.nValueNum > 0) 
                    { 
                        xResults[1] = Centerx2.pFloatVal[0]; 
                    }
                    if (Centery2.nValueNum > 0) 
                    { 
                        yResults[1] = Centery2.pFloatVal[0]; 
                    }
                    if (Angle2.nValueNum > 0)
                    { 
                        angleResults[1] = Angle2.pFloatVal[0];
                    }
                    if (Score2.nValueNum > 0) 
                    { 
                        scoreResults[1] = Score2.pFloatVal[0]; 
                    }
                }

                #region Bitmap
                HObject regionZoom, imageResult1, imageResult2, imageResult3, imageResult;
                HOperatorSet.GenEmptyObj(out regionZoom);
                HOperatorSet.GenEmptyObj(out imageResult1);
                HOperatorSet.GenEmptyObj(out imageResult2);
                HOperatorSet.GenEmptyObj(out imageResult3);
                HOperatorSet.GenEmptyObj(out imageResult);
                Bitmap bitmap, bitmapResult;
                Graphics graph;

                if (HP.ModuStatus == 1 && HP2.ModuStatus == 1)
                {
                    #region 绘制图像
                    HObject regionline1, regionline2, region, recRegion;
                    HOperatorSet.GenEmptyObj(out regionline1);
                    HOperatorSet.GenEmptyObj(out regionline2);
                    HOperatorSet.GenEmptyObj(out region);
                    HOperatorSet.GenEmptyObj(out recRegion);
                    HTuple a1 = new HTuple(), r1 = new HTuple(), c1 = new HTuple();
                    StringBuilder stringBuilder = new StringBuilder();

                    for (int i = 0; i < HP.MatchRect.Count; i++)
                    {
                        HTuple angle = HP.MatchRect[i].Angle;
                        HOperatorSet.GenRectangle2(out recRegion, HP.MatchRect[i].CenterPoint.Y, HP.MatchRect[i].CenterPoint.X, -angle.TupleRad(), HP.MatchRect[i].BoxWidth / 2, HP.MatchRect[i].BoxHeight / 2);
                        HOperatorSet.ZoomRegion(recRegion, out recRegion, 0.25, 0.25);
                        r1 = HP.MatchRect[i].CenterPoint.Y / 4;
                        c1 = HP.MatchRect[i].CenterPoint.X / 4;

                        // 绘制中心线        
                        HOperatorSet.GenRegionLine(out regionline1, r1 - 5, c1, r1 + 5, c1);
                        HOperatorSet.GenRegionLine(out regionline2, r1, c1 - 5, r1, c1 + 5);
                        HTuple HomMat2D2 = new HTuple();
                        HOperatorSet.VectorAngleToRigid(r1, c1, 0, r1, c1, -angle.TupleRad(), out HomMat2D2);
                        HOperatorSet.AffineTransRegion(regionline1, out regionline1, HomMat2D2, "nearest_neighbor");
                        HOperatorSet.ConcatObj(regionZoom, recRegion, out regionZoom);
                        HOperatorSet.ConcatObj(regionZoom, regionline2, out regionZoom);
                        HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);
                        stringBuilder.Append("匹配框1中心:(" + HP.MatchRect[i].CenterPoint.X + "," + HP.MatchRect[i].CenterPoint.Y + ")" + "\n");
                        stringBuilder.Append("角度1:" + HP.MatchRect[i].Angle + ",分数1:" + HP.MatchScore[i] + "\n");
                        regionline1.Dispose();
                        regionline2.Dispose();
                    }

                    for (int i = 0; i < HP2.MatchRect.Count; i++)
                    {
                        HTuple angle = HP2.MatchRect[i].Angle;
                        HOperatorSet.GenRectangle2(out recRegion, HP2.MatchRect[i].CenterPoint.Y, HP2.MatchRect[i].CenterPoint.X, -angle.TupleRad(), HP2.MatchRect[i].BoxWidth / 2, HP2.MatchRect[i].BoxHeight / 2);
                        HOperatorSet.ZoomRegion(recRegion, out recRegion, 0.25, 0.25);
                        r1 = HP2.MatchRect[i].CenterPoint.Y / 4;
                        c1 = HP2.MatchRect[i].CenterPoint.X / 4;

                        // 绘制中心线        
                        HOperatorSet.GenRegionLine(out regionline1, r1 - 5, c1, r1 + 5, c1);
                        HOperatorSet.GenRegionLine(out regionline2, r1, c1 - 5, r1, c1 + 5);
                        HTuple HomMat2D2 = new HTuple();
                        HOperatorSet.VectorAngleToRigid(r1, c1, 0, r1, c1, -angle.TupleRad(), out HomMat2D2);
                        HOperatorSet.AffineTransRegion(regionline1, out regionline1, HomMat2D2, "nearest_neighbor");
                        HOperatorSet.ConcatObj(regionZoom, recRegion, out regionZoom);
                        HOperatorSet.ConcatObj(regionZoom, regionline2, out regionZoom);
                        
                        HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);
                        stringBuilder.Append("匹配框2中心:(" + HP2.MatchRect[i].CenterPoint.X + "," + HP2.MatchRect[i].CenterPoint.Y + ")" + "\n");
                        stringBuilder.Append("角度2:" + HP2.MatchRect[i].Angle + ",分数2:" + HP2.MatchScore[i] + "\n");
                    }

                    // 轮廓点

                    var outlinePoints = HP.MatchOutline.MatchOutlinePoints;
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

                    var outlinePoints1 = HP2.MatchOutline.MatchOutlinePoints;
                    List<HTuple> xList1 = new List<HTuple>();
                    List<HTuple> yList1 = new List<HTuple>();

                    for (int i = 0; i < outlinePoints1.Count; i += 8)
                    {
                        xList1.Add((HTuple)outlinePoints1[i].MatchOutlineX / 4);
                        yList1.Add((HTuple)outlinePoints1[i].MatchOutlineY / 4);
                    }

                    HTuple PointX2 = new HTuple(xList1.ToArray());
                    HTuple PointY2= new HTuple(yList1.ToArray());


                    HOperatorSet.GenRegionPoints(out region, PointY2, PointX2);
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
                    StringBuilder stringBuilder = new StringBuilder();
                    #region 绘制检测区域
                    if (HP.ModuStatus == 0)
                    {
                        stringBuilder.Append("模板 - " + this.Name + " 高精度定位模块1失败。" + "\n");
                        HTuple angle = HP.ROI.Angle;
                        HOperatorSet.GenRectangle2(out regionZoom, HP.ROI.CenterPoint.Y, HP.ROI.CenterPoint.X, -angle.TupleRad(), HP.ROI.BoxWidth / 2, HP.ROI.BoxHeight / 2);
                    }
                    if (HP2.ModuStatus == 0) 
                    {
                        HObject Roi;
                        HOperatorSet.GenEmptyObj(out Roi);
                        stringBuilder.Append("模板 - " + this.Name + " 高精度定位模块2失败。" + "\n");
                        HTuple angle = HP2.ROI.Angle;
                        Roi.Dispose();
                        HOperatorSet.GenRectangle2(out Roi, HP2.ROI.CenterPoint.Y, HP2.ROI.CenterPoint.X, -angle.TupleRad(), HP2.ROI.BoxWidth / 2, HP2.ROI.BoxHeight / 2);
                        HOperatorSet.ConcatObj(regionZoom, Roi, out regionZoom);
                        Roi.Dispose();
                    }
                    HOperatorSet.ZoomRegion(regionZoom, out regionZoom, 0.25, 0.25);
                    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult1, 255, "margin");
                    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult2, 0, "margin");
                    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult3, 0, "margin");
                    HOperatorSet.Compose3(imageResult1, imageResult2, imageResult3, out imageResult);
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
                #endregion

                return false;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"{this.Name}: DistanceMeasureAlg 异常：{ex.Message} ", ex, Log.LogCategory.PR);
                return false;
            }
        }
    }
}
