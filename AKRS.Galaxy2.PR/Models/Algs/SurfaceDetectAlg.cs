using AKRS.Galaxy2.Log;
using ImageSourceModuleCs;
using log4net.Core;
using System;
using System.IO;
using VM.Core;

namespace AKRS.Galaxy2.PR.Models.Algs
{
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using System.Collections.Generic;

    using GeometryCreateCs;

    using HalconDotNet;
    using VM.PlatformSDKCS;
    using AKRS.Galaxy2.PR.Models.Services;
    using System.Drawing;
    using System.Text;

    /// <summary>
    /// 缺陷检测
    /// </summary>
    [Serializable]
    public class SurfaceDetectAlg : BaseAlg
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">算法流程名称</param>
        public SurfaceDetectAlg(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 执行缺陷检测流程，获取结果
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

                int[] threshold = new int[] { Threshold };
                int[] area = new int[] { Area };

                this.ProcedureParam = this.VmProcedure.ModuParams;
                this.ProcedureParam.SetInputInt("Threshold", threshold);
                this.ProcedureParam.SetInputInt("Area", area);

                string CrudeModuleName = $"{this.GetVmProcedure().FullName}." + "图像源2";

                ImageSourceModuleTool imageSourceModuleTool = (ImageSourceModuleTool)VmSolution.Instance[CrudeModuleName];
                if (imageSourceModuleTool == null)
                {
                    return false;
                }
                if (File.Exists(this.AlgSavePath + this.Name + ".bmp"))
                {
                    imageSourceModuleTool.SetImagePath(this.AlgSavePath + this.Name + ".bmp");
                }

                this.VmProcedure.Run();

                ProcedureResult = this.VmProcedure.ModuResult;
                int[] Result;
                Result = ProcedureResult.GetOutputInt("out").pIntVal;

                #region 绘制图像
                GeometryCreateTool geometryCreateTool = (GeometryCreateTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.几何创建2"];

                HObject outPutImage = null,
                regionZoom = null;
                Bitmap bitmap, bitmapResult;
                Graphics graph;
                StringBuilder stringBuilder = new StringBuilder();

                HOperatorSet.GenEmptyObj(out outPutImage);

                for (int i = 0; i < geometryCreateTool.ModuResult.BoxNum; i++)
                {
                    HTuple angle = geometryCreateTool.ModuResult.ListOutputResultBox[i].Angle;
                    HTuple centerX = geometryCreateTool.ModuResult.ListOutputResultBox[i].CenterPoint.Y;
                    HTuple centerY = geometryCreateTool.ModuResult.ListOutputResultBox[i].CenterPoint.Y;
                    HTuple boxWidth = geometryCreateTool.ModuResult.ListOutputResultBox[i].BoxWidth;
                    HTuple boxHeight = geometryCreateTool.ModuResult.ListOutputResultBox[i].BoxHeight;

                    HOperatorSet.GenRectangle2(out HObject recRegion, centerX, centerY, -angle.TupleRad(), boxWidth / 2, boxHeight / 2);
                    HOperatorSet.ZoomRegion(recRegion, out recRegion, 0.25, 0.25);
                    HOperatorSet.AreaCenter(recRegion, out HTuple a1, out HTuple r1, out HTuple c1);
                    HOperatorSet.ConcatObj(regionZoom, recRegion, out regionZoom);
                }
                stringBuilder.Append("缺陷数量:" + geometryCreateTool.ModuResult.BoxNum + "\n");

                // 将结果画在三通道压缩图像上并传出
                HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out HObject imageResult1, 0, "margin");
                HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out HObject imageResult2, 255, "margin");
                HOperatorSet.PaintRegion(regionZoom, this.SourceImg, out HObject  imageResult3, 0, "margin");
                HOperatorSet.Compose3(imageResult1, imageResult2, imageResult3, out HObject imageResult);

                // 绘制文字
                bitmap = ImageHelp.HObjectToBitmap(imageResult);
                bitmapResult = new Bitmap(bitmap);
                graph = Graphics.FromImage(bitmapResult);
                graph.DrawString(stringBuilder.ToString(), new Font("宋体", 15), new SolidBrush(Color.LightGreen), new Point(0, 0));

                #endregion
                if (Result != null && Result[0] != 0)
                {

                    this.MatchResults.Add(new MatchResult()
                                              {
                                                  IsSuccess = true,
                                                  IsBreak = true,
                                                  BreakNum = Result[0],
                                                  OutPutImg1 = bitmapResult,
                    });
                }
                else
                {
                    this.MatchResults.Add(new MatchResult()
                                              {
                                                  IsSuccess = true,
                                                  IsBreak = false,
                                                  BreakNum = 0,
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
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"{this.Name}: SurfaceDetectAlg：{ex.Message} 异常", ex, Log.LogCategory.PR);
                return false;
            }
        }

    }
}
