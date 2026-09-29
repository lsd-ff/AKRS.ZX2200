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
using AKRS.Galaxy2.PR.Models.Services;
using HalconDotNet;
using IMVSHPFeatureMatchModuCs;

namespace AKRSAKRS.Galaxy2.PR.Models.Algs
{
    /// <summary>
    /// 高精度匹配方法，输出图像为匹配框、匹配框中心和轮廓点，输出文字为匹配框中心坐标
    /// </summary>
    [Serializable]
    public class HighPreModelAlg : BaseAlg
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">算法流程名称</param>
        public HighPreModelAlg(string name)
        {
           this.Name = name;
        }

        /// <summary>
        /// 执行高精度匹配流程，获取结果
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
                this.MatchResults = new List<BaseAlgResult>();
                this.VmProcedure.Run();
                ProcedureResult = this.VmProcedure.ModuResult;
                IMVSHPFeatureMatchModuTool HPFeatureMatchModuTool =
                (IMVSHPFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.高精度匹配3"];
                if (HPFeatureMatchModuTool == null)
                {
                    return false;
                }
                HPFeatureMatchResult HP = HPFeatureMatchModuTool.ModuResult;
                float[] xResults=null;
                float[] yResults=null;
                float[] angleResults = null;
                float[] scoreResults=null;
                if (HP.ModuStatus == 1)
                {
                    FloatDataArray Centerx = ProcedureResult.GetOutputFloat("ReasultX");
                    xResults = Centerx.pFloatVal == null ? null : Centerx.pFloatVal;

                    FloatDataArray Centery = ProcedureResult.GetOutputFloat("ReasultY");
                    yResults = Centery.pFloatVal == null ? null : Centery.pFloatVal;

                    FloatDataArray Angle = ProcedureResult.GetOutputFloat("ReasultA");
                    angleResults = Angle.pFloatVal == null ? null : Angle.pFloatVal;

                    FloatDataArray Score = ProcedureResult.GetOutputFloat("ReasultS");
                    scoreResults = Score.pFloatVal == null ? null : Score.pFloatVal;

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

                if (xResults != null && xResults.Count() != 0)
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
                        HOperatorSet.ConcatObj(regionline1, regionline2, out regionline1);
                        HTuple HomMat2D2 = new HTuple();
                        HOperatorSet.VectorAngleToRigid(r1, c1, 0, r1, c1, -angle.TupleRad(), out HomMat2D2);
                        HOperatorSet.AffineTransRegion(regionline1, out regionline1, HomMat2D2, "nearest_neighbor");
                        HOperatorSet.ConcatObj(regionZoom, recRegion, out regionZoom);

                        // 模板框与中心线
                        HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);
                        stringBuilder.Append("匹配框中心:(" + HP.MatchRect[i].CenterPoint.X + "," + HP.MatchRect[i].CenterPoint.Y + ")"  + "\n");
                        stringBuilder.Append( "角度:" + HP.MatchRect[i].Angle + ",分数:" + HP.MatchScore[i] + "\n");
                    }
                    recRegion.Dispose();

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
                   
                    string MarkedWord = "模板 - " + this.Name + " 高精度定位模块失败。";

                    #region 绘制检测区域
                    HTuple angle = HP.ROI.Angle;
                    HOperatorSet.GenRectangle2(out regionZoom, HP.ROI.CenterPoint.Y, HP.ROI.CenterPoint.X, -angle.TupleRad(), HP.ROI.BoxWidth / 2, HP.ROI.BoxHeight / 2);
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
                LogHelper.Post(Level.Error, $"{this.Name}: HighPreModelAlg 异常：{ex.Message} ", ex, LogCategory.PR);
                return false;
            }
        }
    }
}
