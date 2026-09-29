using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Models.Services;
using HalconDotNet;
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
    /// 清晰度评价方法
    /// </summary>
    [Serializable]
    public class FocusAlg : BaseAlg
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        public FocusAlg(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 执行清晰度计算流程，获取结果
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

                FloatDataArray Centerx = ProcedureResult.GetOutputFloat("FocusValue");
                float[] Results = Centerx.pFloatVal == null ? null : Centerx.pFloatVal;

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("清晰度值:" + Results[0]  + "\n");

                Bitmap bitmap, bitmapResult;
                Graphics graph;
                HObject imageResult;
                HOperatorSet.GenEmptyObj(out imageResult);
                imageResult.Dispose();
                HOperatorSet.Compose3(this.SourceImg, this.SourceImg, this.SourceImg, out imageResult);

                // 绘制文字
                bitmap = ImageHelp.HObjectToBitmap(imageResult);
                bitmapResult = new Bitmap(bitmap);
                graph = Graphics.FromImage(bitmapResult);
                graph.DrawString(stringBuilder.ToString(), new Font("宋体", 15), new SolidBrush(Color.LightGreen), new Point(0, 0));

                if (Results != null&&Results.Count()!=0)
                {
                    for (int i = 0; i < Results.Length; i++)
                    {
                        this.MatchResults.Add(new FocusResult()
                        {
                            FocusValue = Results[i],
                            OutPutImg1 = bitmapResult,
                        });
                    }
                    imageResult.Dispose();
                    graph.Dispose();
                    bitmap.Dispose();
                    return true;
                }
                else
                {
                    string MarkedWord = "";
                    MarkedWord = "模板 - " + this.Name + " 清晰度评价模块失败。";

                    bitmap = ImageHelp.HObjectToBitmap(imageResult);
                    bitmapResult = new Bitmap(bitmap);
                    graph = Graphics.FromImage(bitmapResult);
                    graph.DrawString(MarkedWord, new Font("宋体", 15), new SolidBrush(Color.Red), new Point(0, 0));

                    this.MatchResults.Add(new FocusResult()
                    {
                        IsSuccess = false,
                        OutPutImg1 = bitmapResult,
                        MarkedWords = MarkedWord,
                    });
                    bitmap.Dispose();
                    imageResult.Dispose();
                    graph.Dispose();
                }
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"{this.Name}:FocusAlg 异常：{ex.Message} ", ex, Log.LogCategory.PR);
                return false;
            }
        }
    }
}
