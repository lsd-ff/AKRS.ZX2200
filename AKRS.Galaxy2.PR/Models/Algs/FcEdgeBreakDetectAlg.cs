using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Models.Services;
using HalconDotNet;
using ImageSourceModuleCs;
using IMVSEdgeFlawInspModuCs;
using LanguageExt;
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

namespace AKRS.Galaxy2.PR.Models.Algs
{
    /// <summary>
    /// Fc崩边检测方法
    /// </summary>
    [Serializable]
    public class FcEdgeBreakDetectAlg : BaseAlg
    {
    
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">算法流程名称</param>
        public FcEdgeBreakDetectAlg(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 执行崩边检测流程，获取结果
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
                
                int[] num = ProcedureResult.GetOutputInt("DefectNum").pIntVal;

                HObject regionZoom, imageResult1, imageResult2, imageResult3, imageResult;
                HOperatorSet.GenEmptyObj(out regionZoom);
                HOperatorSet.GenEmptyObj(out imageResult1);
                HOperatorSet.GenEmptyObj(out imageResult2);
                HOperatorSet.GenEmptyObj(out imageResult3);
                HOperatorSet.GenEmptyObj(out imageResult);
                Bitmap bitmap, bitmapResult;
                Graphics graph;

                IMVSEdgeFlawInspModuTool iMVSEdgeFlawInspModuTool = (IMVSEdgeFlawInspModuTool)VmSolution.Instance[$"{this.GetVmProcedure().FullName}.边缘模型缺陷检测1"];
                if (iMVSEdgeFlawInspModuTool == null)
                {
                    return false;
                }
                EdgeFlawInspResult EdgeResult = iMVSEdgeFlawInspModuTool.ModuResult;

                if (num != null && num.Count()!= 0 && num[0] != 0)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("缺陷个数:" + EdgeResult.DefectBox.Count + "\n");

                    // 有缺陷                                       
                    HObject regionline1, regionline2, region, recRegion;
                    HOperatorSet.GenEmptyObj(out regionline1);
                    HOperatorSet.GenEmptyObj(out regionline2);
                    HOperatorSet.GenEmptyObj(out region);
                    HOperatorSet.GenEmptyObj(out recRegion);

                    HTuple a1 = new HTuple(), r1 = new HTuple(), c1 = new HTuple();

                    for (int i = 0; i < EdgeResult.DefectBox.Count; i++)
                    {
                        HTuple angle = EdgeResult.DefectBox[i].Angle;
                        HOperatorSet.GenRectangle2(out recRegion, EdgeResult.DefectBox[i].CenterPoint.Y, EdgeResult.DefectBox[i].CenterPoint.X, -angle.TupleRad(), EdgeResult.DefectBox[i].BoxWidth / 2, EdgeResult.DefectBox[i].BoxHeight / 2);
                        HOperatorSet.ZoomRegion(recRegion, out recRegion, 0.25, 0.25);
                        HOperatorSet.ConcatObj(regionZoom, recRegion, out regionZoom);
                    }
                    recRegion.Dispose();
                    
                    // 将结果画在三通道压缩图像上并传出
                    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult1, 255, "margin");
                    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult2, 0, "margin");
                    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult3, 0, "margin");
                    HOperatorSet.Compose3(imageResult1, imageResult2, imageResult3, out imageResult);

                    // 绘制文字
                    bitmap = ImageHelp.HObjectToBitmap(imageResult);
                    bitmapResult = new Bitmap(bitmap);
                    graph = Graphics.FromImage(bitmapResult);
                    graph.DrawString(stringBuilder.ToString(), new Font("宋体", 15), new SolidBrush(Color.Red), new Point(0, 0));

                    bitmap.Dispose();
                    regionline1.Dispose();
                    regionline2.Dispose();
                    region.Dispose();
                    for (int i = 0; i < num.Length; i++)
                    {
                        this.MatchResults.Add(new MatchResult()
                        {
                            IsSuccess = true,
                            OutPutImg1 = bitmapResult, 
                        });
                    }
                }
                else
                {
                    string MarkedWord = "";
                    if (num != null && num.Count() != 0)
                    {
                        MarkedWord = "模板 - " + this.Name + " 背崩检测缺陷个数为0";
                    }
                    else
                    {
                        MarkedWord = "模板 - " + this.Name + " 背崩检测模块失败。";
                        HOperatorSet.Compose3(this.SourceImg, this.SourceImg, this.SourceImg, out imageResult);
                        bitmap = ImageHelp.HObjectToBitmap(imageResult);
                        bitmapResult = new Bitmap(bitmap);
                        graph = Graphics.FromImage(bitmapResult);
                        graph.DrawString(MarkedWord, new Font("宋体", 15), new SolidBrush(Color.Red), new Point(0, 0));
                        bitmap.Dispose();
                        this.MatchResults.Add(new MatchResult()
                        {
                            IsSuccess = false,
                            OutPutImg1 = bitmapResult,
                            MarkedWords = MarkedWord,
                        });
                    }
                    
                    #region 绘制检测区域
                    //HObject regionline1, regionline2, region, recRegion;
                    //HOperatorSet.GenEmptyObj(out regionline1);
                    //HOperatorSet.GenEmptyObj(out regionline2);
                    //HOperatorSet.GenEmptyObj(out region);
                    //HOperatorSet.GenEmptyObj(out recRegion);

                    //HTuple a1 = new HTuple(), r1 = new HTuple(), c1 = new HTuple();

                    //for (int i = 0; i < EdgeResult.CaliperBox.Count; i++)
                    //{
                    //    HTuple angle1 = EdgeResult.CaliperBox[i].Angle;
                    //    HOperatorSet.GenRectangle2(out recRegion, EdgeResult.CaliperBox[i].CenterPoint.Y, EdgeResult.CaliperBox[i].CenterPoint.X, -angle.TupleRad(), EdgeResult.CaliperBox[i].BoxWidth / 2, EdgeResult.CaliperBox[i].BoxHeight / 2);

                    //    HOperatorSet.ZoomRegion(recRegion, out recRegion, 0.25, 0.25);
                    //    r1 = EdgeResult.CaliperBox[i].CenterPoint.Y / 4;
                    //    c1 = EdgeResult.CaliperBox[i].CenterPoint.X / 4;

                    //    // 绘制中心线        
                    //    HOperatorSet.GenRegionLine(out regionline1, r1 - 5, c1, r1 + 5, c1);
                    //    HOperatorSet.GenRegionLine(out regionline2, r1, c1 - 5, r1, c1 + 5);
                    //    HOperatorSet.ConcatObj(regionline1, regionline2, out regionline1);
                    //    HTuple HomMat2D2 = new HTuple();
                    //    HOperatorSet.VectorAngleToRigid(r1, c1, 0, r1, c1, -angle1.TupleRad(), out HomMat2D2);
                    //    HOperatorSet.AffineTransRegion(regionline1, out regionline1, HomMat2D2, "nearest_neighbor");
                    //    HOperatorSet.ConcatObj(regionZoom, recRegion, out regionZoom);

                    //    // 模板框与中心线
                    //    HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);

                    //}
                    //recRegion.Dispose();
                  
                    //HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult1, 255, "margin");
                    //HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult2, 0, "margin");
                    //HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out imageResult3, 0, "margin");
                    HOperatorSet.Compose3(this.SourceImg, this.SourceImg, this.SourceImg, out imageResult);
                    bitmap = ImageHelp.HObjectToBitmap(imageResult);
                    bitmapResult = new Bitmap(bitmap);
                    graph = Graphics.FromImage(bitmapResult);
                    graph.DrawString(MarkedWord, new Font("宋体", 15), new SolidBrush(Color.Green), new Point(0, 0));
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

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"{this.Name}: FcEdgeBreakDetectAlg 异常：{ex.Message} ", ex, Log.LogCategory.PR);
                return false;
            }
        }
    }
}
