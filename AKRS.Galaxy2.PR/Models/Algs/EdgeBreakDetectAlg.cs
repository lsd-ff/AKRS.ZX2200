using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Models.Services;
using GeometryCreateCs;
using HalconDotNet;
using IMVSEdgeFlawInspModuCs;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using VM.Core;

namespace AKRS.Galaxy2.PR.Models.Algs
{
    /// <summary>
    /// 崩边检测方法
    /// </summary>
    [Serializable]
    public class EdgeBreakDetectAlg : BaseAlg
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">算法流程名称</param>
        public EdgeBreakDetectAlg(string name)
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
                this.MatchResults = new List<BaseAlgResult>();

                if (this.VmProcedureName == null)
                {
                    this.VmProcedureName = this.GetVmProcedure().FullName;
                }
                this.VmProcedure = (VmProcedure)VmSolution.Instance[this.VmProcedureName];

                this.VmProcedure.Run();


                ProcedureResult = this.VmProcedure.ModuResult;
                int[] num = ProcedureResult.GetOutputInt("DetectNum").pIntVal;

                #region 绘制图像
                IMVSEdgeFlawInspModuCs.IMVSEdgeFlawInspModuTool iMVSEdgeFlawInspModuTool = (IMVSEdgeFlawInspModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.边缘模型缺陷检测1"];
                if (iMVSEdgeFlawInspModuTool == null)
                {
                    return false;
                }
                HObject outPutImage = null,
                regionZoom = null;
                Bitmap bitmap, bitmapResult;
                Graphics graph;
                StringBuilder stringBuilder = new StringBuilder();

                HOperatorSet.GenEmptyObj(out outPutImage);

                for (int i = 0; i < iMVSEdgeFlawInspModuTool.ModuResult.FlawNum; i++)
                {
                    HTuple angle = iMVSEdgeFlawInspModuTool.ModuResult.DefectBox[i].Angle;
                    HTuple centerX = iMVSEdgeFlawInspModuTool.ModuResult.DefectBox[i].CenterPoint.Y;
                    HTuple centerY = iMVSEdgeFlawInspModuTool.ModuResult.DefectBox[i].CenterPoint.Y;
                    HTuple boxWidth = iMVSEdgeFlawInspModuTool.ModuResult.DefectBox[i].BoxWidth;
                    HTuple boxHeight = iMVSEdgeFlawInspModuTool.ModuResult.DefectBox[i].BoxHeight;

                    HOperatorSet.GenRectangle2(out HObject recRegion, centerX, centerY, -angle.TupleRad(), boxWidth / 2, boxHeight / 2);
                    HOperatorSet.ZoomRegion(recRegion, out recRegion, 0.25, 0.25);
                    HOperatorSet.AreaCenter(recRegion, out HTuple a1, out HTuple r1, out HTuple c1);
                    HOperatorSet.ConcatObj(regionZoom, recRegion, out regionZoom);
                }
                stringBuilder.Append("缺陷数量:" + iMVSEdgeFlawInspModuTool.ModuResult.FlawNum + "\n");

                #endregion


                if (num != null && num[0] != 0)
                {
                    // 将结果画在三通道压缩图像上并传出
                    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out HObject imageResult1, 0, "margin");
                    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out HObject imageResult2, 255, "margin");
                    HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out HObject imageResult3, 0, "margin");
                    HOperatorSet.Compose3(imageResult1, imageResult2, imageResult3, out HObject imageResult);

                    // 绘制文字
                    bitmap = ImageHelp.HObjectToBitmap(imageResult);
                    bitmapResult = new Bitmap(bitmap);
                    graph = Graphics.FromImage(bitmapResult);
                    graph.DrawString(stringBuilder.ToString(), new Font("宋体", 15), new SolidBrush(Color.LightGreen), new Point(0, 0));
                    this.MatchResults.Add(new MatchResult()
                    {
                        BreakNum = num[0],
                        IsSuccess = true,
                        IsBreak = true, 
                        OutPutImg1 = bitmapResult,
                    });
                }
                else
                {
                    HOperatorSet.Compose3(this.SourceImg, this.SourceImg, this.SourceImg, out HObject imageResult);

                    // 绘制文字
                    bitmap = ImageHelp.HObjectToBitmap(imageResult);
                    bitmapResult = new Bitmap(bitmap);
                    graph = Graphics.FromImage(bitmapResult);
                    graph.DrawString(stringBuilder.ToString(), new Font("宋体", 15), new SolidBrush(Color.LightGreen), new Point(0, 0));
                    this.MatchResults.Add(new MatchResult()
                    {
                        BreakNum = 0,
                        IsSuccess = true,
                        IsBreak = false,
                        OutPutImg1 = bitmapResult,
                    });
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"{this.Name}: EdgeBreakDetectAlg 异常：{ex.Message} ", ex, Log.LogCategory.PR);
                return false;
            }
        }
    }
}
