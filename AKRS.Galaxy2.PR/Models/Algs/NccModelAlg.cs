using AKRS.Galaxy2.Log;
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
using IMVSGrayMatchModuVACs;

namespace AKRS.Galaxy2.PR.Models.Algs
{
    /// <summary>
    /// 灰度匹配方法,输出图像为匹配框、匹配框中心，输出文字为匹配框中心坐标
    /// </summary>
    [Serializable]
    public class NccModelAlg : BaseAlg
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">算法流程名称</param>
        public NccModelAlg(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 执行灰度匹配流程，获取结果
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


                FloatDataArray Centerx = ProcedureResult.GetOutputFloat("ReasultX");
                float[] xResults = Centerx.pFloatVal == null ? null : Centerx.pFloatVal;

                FloatDataArray Centery = ProcedureResult.GetOutputFloat("ReasultY");
                float[] yResults = Centery.pFloatVal == null ? null : Centery.pFloatVal;

                FloatDataArray Angle = ProcedureResult.GetOutputFloat("ReasultA");
                float[] angleResults = Angle.pFloatVal == null ? null : Angle.pFloatVal;

                FloatDataArray Score = ProcedureResult.GetOutputFloat("ReasultS");
                float[] scoreResults = Score.pFloatVal == null ? null : Score.pFloatVal;

                #region Bitmap
                IMVSGrayMatchModuVATool GrayMatchModuVATool =
                       (IMVSGrayMatchModuVATool)VmSolution.Instance[$"{this.VmProcedure.FullName}.灰度匹配1"];
                if (GrayMatchModuVATool == null)
                {
                    return false;
                }
                GrayMatchVAResult GrayRes = GrayMatchModuVATool.ModuResult;

                HObject regionZoom, imageResult1, imageResult2, imageResult3, imageResult;
                HOperatorSet.GenEmptyObj(out regionZoom);
                HOperatorSet.GenEmptyObj(out imageResult1);
                HOperatorSet.GenEmptyObj(out imageResult2);
                HOperatorSet.GenEmptyObj(out imageResult3);
                HOperatorSet.GenEmptyObj(out imageResult);
                Bitmap bitmap, bitmapResult;
                Graphics graph;

                if (xResults != null&& GrayRes.ModuStatus==1&& xResults.Count() != 0)
                {
                    #region 绘制图像
                    HObject regionline1, regionline2, recRegion;
                    HOperatorSet.GenEmptyObj(out regionline1);
                    HOperatorSet.GenEmptyObj(out regionline2);
                    HOperatorSet.GenEmptyObj(out recRegion);

                    HTuple a1 = new HTuple(), r1 = new HTuple(), c1 = new HTuple();
                    StringBuilder stringBuilder = new StringBuilder();

                    for (int i = 0; i < GrayRes.MatchRect.Count; i++)
                    {
                        HTuple angle = GrayRes.MatchRect[i].Angle;
                        HOperatorSet.GenRectangle2(out recRegion, GrayRes.MatchRect[i].CenterPoint.Y, GrayRes.MatchRect[i].CenterPoint.X, -angle.TupleRad(), GrayRes.MatchRect[i].BoxWidth / 2, GrayRes.MatchRect[i].BoxHeight / 2);

                        HOperatorSet.ZoomRegion(recRegion, out recRegion, 0.25, 0.25);
                        r1 = GrayRes.MatchRect[i].CenterPoint.Y / 4;
                        c1 = GrayRes.MatchRect[i].CenterPoint.X / 4;

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
                        stringBuilder.Append("匹配框中心:(" + GrayRes.MatchRect[i].CenterPoint.X + "," + GrayRes.MatchRect[i].CenterPoint.Y + ")" + "\n");
                        stringBuilder.Append( "角度:" + GrayRes.MatchRect[i].Angle + ",分数:" + GrayRes.MatchScore[i] + "\n");
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
                    string MarkedWord = "模板 - " + this.Name + " 灰度定位模块失败。";
                    #region 绘制检测区域
                    HTuple angle = GrayRes.ROI.Angle;
                    HOperatorSet.GenRectangle2(out regionZoom, GrayRes.ROI.CenterPoint.Y, GrayRes.ROI.CenterPoint.X, -angle.TupleRad(), GrayRes.ROI.BoxWidth / 2, GrayRes.ROI.BoxHeight / 2);
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
                LogHelper.Post(Level.Error, $"{this.Name}: NccModelAlg 异常：{ex.Message} ", ex, Log.LogCategory.PR);
                return false;
            }
        }
    }
}
