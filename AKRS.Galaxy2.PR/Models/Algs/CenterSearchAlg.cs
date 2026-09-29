using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Models.Services;
using HalconDotNet;
using IMVSBlobFindModuCs;
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
    /// 中心查找方法,，输出图像为质心和轮廓点，输出文字为质心坐标
    /// </summary>
    [Serializable]
    public class CenterSearchAlg : BaseAlg
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        public CenterSearchAlg(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 执行中心查找流程，获取结果
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

                FloatDataArray Centerx = ProcedureResult.GetOutputFloat("ReasultX");
                float[] xResults = Centerx.pFloatVal == null ? null : Centerx.pFloatVal;

                FloatDataArray Centery = ProcedureResult.GetOutputFloat("ReasultY");
                float[] yResults = Centery.pFloatVal == null ? null : Centery.pFloatVal;


                #region Bitmap
                IMVSBlobFindModuTool BlobFindModuTool = (IMVSBlobFindModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.BLOB分析1"];
                if (BlobFindModuTool == null) 
                {
                    return false;
                }
                BlobFindResult BlobRes = BlobFindModuTool.ModuResult;

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
                    HObject regionline1, regionline2, region;
                    HOperatorSet.GenEmptyObj(out regionline1);
                    HOperatorSet.GenEmptyObj(out regionline2);
                    HOperatorSet.GenEmptyObj(out region);


                    HTuple a1 = new HTuple(), r1 = new HTuple(), c1 = new HTuple();
                    StringBuilder stringBuilder = new StringBuilder();
                    // 质心点
                    for (int i = 0; i < BlobRes.CentroidPoint.Count; i++)
                    {
                        r1 = BlobRes.CentroidPoint[i].Y / 4;
                        c1 = BlobRes.CentroidPoint[i].X / 4;

                        // 绘制中心线        
                        HOperatorSet.GenRegionLine(out regionline1, r1 - 5, c1, r1 + 5, c1);
                        HOperatorSet.GenRegionLine(out regionline2, r1, c1 - 5, r1, c1 + 5);
                        HOperatorSet.ConcatObj(regionline1, regionline2, out regionline1);
                        HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);
                        stringBuilder.Append("质心点:(" + BlobRes.CentroidPoint[i].X + "," + BlobRes.CentroidPoint[i].Y + ")\n");
                    }

                    // 轮廓点
                    var outlinePoints = BlobRes.ContourInfo.MatchOutlinePoints;
                    List<HTuple> xList = new List<HTuple>();
                    List<HTuple> yList = new List<HTuple>();

                    for (int i = 0; i < outlinePoints.Count; i += 3)
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
                    string MarkedWord = "模板- " + this.Name + " 在中心搜索模块失败。";
                    #region 绘制检测区域
                    HObject recRegion;
                    HOperatorSet.GenEmptyObj(out recRegion);

                    for (int i = 0; i < BlobRes.ROI.Count; i++)
                    {
                        HTuple angle = BlobRes.ROI[i].Angle;
                        HOperatorSet.GenRectangle2(out recRegion, BlobRes.ROI[i].CenterPoint.Y, BlobRes.ROI[i].CenterPoint.X, -angle.TupleRad(), BlobRes.ROI[i].BoxWidth / 2, BlobRes.ROI[i].BoxHeight / 2);
                        HOperatorSet.ConcatObj(regionZoom, recRegion, out regionZoom);
                    }
                    recRegion.Dispose();
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
                LogHelper.Post(Level.Error, $"{this.Name}: CenterSearchAlg 异常：{ex.Message} ", ex, Log.LogCategory.PR);
                return false;
            }
        }
    }
}
