using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using VM.Core;
using BranchModule_STDCs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using VM.PlatformSDKCS;
using AKRS.Galaxy2.Log;
using log4net.Core;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Models.CommonModels;
using System.Diagnostics;
using AKRS.Galaxy2.PR.Models.Services;
using HalconDotNet;
using IMVS2dBcrModuCs;
using IMVSFixtureModuCs;

namespace AKRS.Galaxy2.PR.Models.Algs
{
    /// <summary>
    /// 二维码检测方法，输出图像为矩形，输出文字为条码字符
    /// </summary>
    [Serializable]
    public class QrCodeDetectAlg : BaseAlg
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        public QrCodeDetectAlg(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 执行二维码检测流程，获取结果
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

                StringDataArray Code = ProcedureResult.GetOutputString("CodeReasult");
                StringData[] codeResults = Code.astStringVal == null ? null : Code.astStringVal;

                #region Bitmap.
                IMVS2dBcrModuTool BcrModuTool = (IMVS2dBcrModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.二维码识别1"];
                if (BcrModuTool == null)
                {
                    return false;
                }
                Bcr2dResult BcrRes = BcrModuTool.ModuResult;

                HObject regionZoom, imageResult1, imageResult2, imageResult3, imageResult;
                HOperatorSet.GenEmptyObj(out regionZoom);
                HOperatorSet.GenEmptyObj(out imageResult1);
                HOperatorSet.GenEmptyObj(out imageResult2);
                HOperatorSet.GenEmptyObj(out imageResult3);
                HOperatorSet.GenEmptyObj(out imageResult);
                Bitmap bitmap, bitmapResult;
                Graphics graph;

                if (codeResults != null && codeResults.Count() != 0)
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    #region 绘制图像
                    HTuple angle5 = BcrRes.Rect[0].Angle;
                    HOperatorSet.GenRectangle2(out regionZoom, BcrRes.Rect[0].CenterPoint.Y, BcrRes.Rect[0].CenterPoint.X, -angle5.TupleRad(), BcrRes.Rect[0].BoxWidth / 2, BcrRes.Rect[0].BoxHeight / 2);
                    HOperatorSet.ZoomRegion(regionZoom, out regionZoom, 0.25, 0.25);
                    stringBuilder.Append("编码为:" + BcrRes.CodeStr[0].ToString() + "\n");

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
                    #endregion

                    this.MatchResults.Add(new CodeResult()
                    {
                        CodeValue = codeResults[0].ToString(),
                        IsSuccess = true,
                        OutPutImg1 = bitmapResult,
                    });
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
                    string MarkedWord = "模板 - " + this.Name + " 二维码识别失败。";
                    
                    #region 绘制检测区域
                    HTuple angle1 = BcrRes.ROI.Angle;
                    HOperatorSet.GenRectangle2(out regionZoom, BcrRes.ROI.CenterPoint.Y, BcrRes.ROI.CenterPoint.X, -angle1.TupleRad(), BcrRes.ROI.BoxWidth / 2, BcrRes.ROI.BoxHeight / 2);

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
                    this.MatchResults.Add(new CodeResult()
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
                LogHelper.Post(Level.Error, $"{this.Name}: QrCodeDetectAlg 异常：{ex.Message} ", ex, Log.LogCategory.PR);
                return false;
            }
        }
    }
}
