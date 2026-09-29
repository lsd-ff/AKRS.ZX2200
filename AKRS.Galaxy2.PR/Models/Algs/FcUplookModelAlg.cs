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
using System.Diagnostics;
using AKRS.Galaxy2.PR.Models.Services;
using HalconDotNet;
using IMVSContourMatchModuCs;
using IMVSGrayMatchModuVACs;
using IMVSHPFeatureMatchModuCs;
using IMVSLineFindModuCs;
using IMVSRectFindModuCs;
using VMControls.Interface;
using IMVSBlobFindModuCs;
using IMVSEdgeFlawInspModuCs;

namespace AKRS.Galaxy2.PR.Models.Algs
{

    /// <summary>
    /// FC上视定位方法，输出图像为矩形、中心，输出文字为中心坐标、直线角度
    /// </summary>
    [Serializable]
    public class FcUplookModelAlg : BaseAlg
    {
        /// <summary>
        /// 粗定位方式
        /// </summary>
        public int[] CrudePRType = new int[1];

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        public FcUplookModelAlg(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 执行矩形检测流程，获取结果
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

                CrudePRType[0] = (int)CrudeLocateType;
                this.ProcedureParam.SetInputInt("FirPRType", CrudePRType);

               
                this.VmProcedure.Run();
              

                ProcedureResult = this.VmProcedure.ModuResult;

                FloatDataArray Centerx = ProcedureResult.GetOutputFloat("ReasultX");
                float[] xResults = Centerx.pFloatVal == null ? null : Centerx.pFloatVal;

                FloatDataArray Centery = ProcedureResult.GetOutputFloat("ReasultY");
                float[] yResults = Centery.pFloatVal == null ? null : Centery.pFloatVal;

                FloatDataArray Angle = ProcedureResult.GetOutputFloat("ReasultA");
                float[] angleResults = Angle.pFloatVal == null ? null : Angle.pFloatVal;

                #region Bitmap


                HObject regionZoom, imageResult1, imageResult2, imageResult3, imageResult;
                HOperatorSet.GenEmptyObj(out regionZoom);
                HOperatorSet.GenEmptyObj(out imageResult1);
                HOperatorSet.GenEmptyObj(out imageResult2);
                HOperatorSet.GenEmptyObj(out imageResult3);
                HOperatorSet.GenEmptyObj(out imageResult);
                Bitmap bitmap, bitmapResult;
                Graphics graph;

                if (xResults != null && xResults.Count() != 0 && angleResults != null && angleResults.Count() != 0)
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    HObject regionline1, regionline2, line1, recRegion, region;
                    HOperatorSet.GenEmptyObj(out regionline1);
                    HOperatorSet.GenEmptyObj(out regionline2);
                    HOperatorSet.GenEmptyObj(out line1);
                    HOperatorSet.GenEmptyObj(out recRegion);
                    HOperatorSet.GenEmptyObj(out region);

                    HTuple a1 = new HTuple(), r1 = new HTuple(), c1 = new HTuple();
                    HTuple angle = new HTuple(), HomMat2D2 = new HTuple();
                    HTuple PointX=new HTuple(), PointY=new HTuple();
                    List<HTuple> xList = new List<HTuple>();
                    List<HTuple> yList = new List<HTuple>();
                    List<MatchPoint> outlinePoints=new List<MatchPoint>();

                 
                    switch (CrudePRType[0])
                    {
                        case 0:
                            IMVSFastFeatureMatchModuTool iMVSFastFeatureMatchModuTool = (IMVSFastFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.快速匹配1"];
                            FastFeatureMatchResult FastRes = iMVSFastFeatureMatchModuTool.ModuResult;
                            stringBuilder.Append("快速匹配框中心:(" + FastRes.MatchRect[0].CenterPoint.X + "," + FastRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                            stringBuilder.Append("角度:" + FastRes.MatchRect[0].Angle + ",分数:" + FastRes.MatchScore[0] + "\n");

                            //  定位结果绘制
                            angle = FastRes.MatchRect[0].Angle;
                            HOperatorSet.GenRectangle2(out recRegion, FastRes.MatchRect[0].CenterPoint.Y, FastRes.MatchRect[0].CenterPoint.X, -angle.TupleRad(), FastRes.MatchRect[0].BoxWidth / 2, FastRes.MatchRect[0].BoxHeight / 2);
                            HOperatorSet.ZoomRegion(recRegion, out recRegion, 0.25, 0.25);
                            r1 = FastRes.MatchRect[0].CenterPoint.Y / 4;
                            c1 = FastRes.MatchRect[0].CenterPoint.X / 4;

                            // 绘制中心线        
                            HOperatorSet.GenRegionLine(out regionline1, r1 - 5, c1, r1 + 5, c1);
                            HOperatorSet.GenRegionLine(out regionline2, r1, c1 - 5, r1, c1 + 5);
                            HOperatorSet.ConcatObj(regionline1, regionline2, out regionline1);
                            HOperatorSet.VectorAngleToRigid(r1, c1, 0, r1, c1, -angle.TupleRad(), out HomMat2D2);
                            HOperatorSet.AffineTransRegion(regionline1, out regionline1, HomMat2D2, "nearest_neighbor");
                            HOperatorSet.ConcatObj(regionZoom, recRegion, out regionZoom);
                            HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);
                            recRegion.Dispose();

                            // 轮廓点

                            outlinePoints = FastRes.MatchOutline.MatchOutlinePoints;

                            for (int i = 0; i < outlinePoints.Count; i += 8)
                            {
                                xList.Add((HTuple)outlinePoints[i].MatchOutlineX / 4);
                                yList.Add((HTuple)outlinePoints[i].MatchOutlineY / 4);
                            }

                            PointX = new HTuple(xList.ToArray());
                            PointY = new HTuple(yList.ToArray());

                            HOperatorSet.GenRegionPoints(out region, PointY, PointX);
                            HOperatorSet.ConcatObj(regionZoom, region, out regionZoom);
                            break;
                        case 1:
                            IMVSHPFeatureMatchModuTool iMVSHPFeatureMatchModuTool = (IMVSHPFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.高精度匹配1"];
                            HPFeatureMatchResult HPRes = iMVSHPFeatureMatchModuTool.ModuResult;
                            stringBuilder.Append("高精度匹配框中心:(" + HPRes.MatchRect[0].CenterPoint.X + "," + HPRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                            stringBuilder.Append("角度:" + HPRes.MatchRect[0].Angle + ",分数:" + HPRes.MatchScore[0] + "\n");

                            //  定位结果绘制
                            angle = HPRes.MatchRect[0].Angle;
                            HOperatorSet.GenRectangle2(out recRegion, HPRes.MatchRect[0].CenterPoint.Y, HPRes.MatchRect[0].CenterPoint.X, -angle.TupleRad(), HPRes.MatchRect[0].BoxWidth / 2, HPRes.MatchRect[0].BoxHeight / 2);
                            HOperatorSet.ZoomRegion(recRegion, out recRegion, 0.25, 0.25);
                            r1 = HPRes.MatchRect[0].CenterPoint.Y / 4;
                            c1 = HPRes.MatchRect[0].CenterPoint.X / 4;

                            // 绘制中心线        
                            HOperatorSet.GenRegionLine(out regionline1, r1 - 5, c1, r1 + 5, c1);
                            HOperatorSet.GenRegionLine(out regionline2, r1, c1 - 5, r1, c1 + 5);
                            HOperatorSet.ConcatObj(regionline1, regionline2, out regionline1);
                            HOperatorSet.VectorAngleToRigid(r1, c1, 0, r1, c1, -angle.TupleRad(), out HomMat2D2);
                            HOperatorSet.AffineTransRegion(regionline1, out regionline1, HomMat2D2, "nearest_neighbor");
                            HOperatorSet.ConcatObj(regionZoom, recRegion, out regionZoom);
                            HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);
                            recRegion.Dispose();

                            // 轮廓点

                             outlinePoints = HPRes.MatchOutline.MatchOutlinePoints;
                           
                            for (int i = 0; i < outlinePoints.Count; i += 8)
                            {
                                xList.Add((HTuple)outlinePoints[i].MatchOutlineX / 4);
                                yList.Add((HTuple)outlinePoints[i].MatchOutlineY / 4);
                            }

                             PointX = new HTuple(xList.ToArray());
                             PointY = new HTuple(yList.ToArray());



                            HOperatorSet.GenRegionPoints(out region, PointY, PointX);
                            HOperatorSet.ConcatObj(regionZoom, region, out regionZoom);

                            break;
                        case 2:
                            IMVSGrayMatchModuVATool iMVSGrayMatchModuVATool = (IMVSGrayMatchModuVATool)VmSolution.Instance[$"{this.VmProcedure.FullName}.灰度匹配1"];
                            GrayMatchVAResult GrayRes = iMVSGrayMatchModuVATool.ModuResult;
                            stringBuilder.Append("灰度匹配框中心:(" + GrayRes.MatchRect[0].CenterPoint.X + "," + GrayRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                            stringBuilder.Append("角度:" + GrayRes.MatchRect[0].Angle + ",分数:" + GrayRes.MatchScore[0] + "\n");
                            angle = GrayRes.MatchRect[0].Angle;
                            HOperatorSet.GenRectangle2(out recRegion, GrayRes.MatchRect[0].CenterPoint.Y, GrayRes.MatchRect[0].CenterPoint.X, -angle.TupleRad(), GrayRes.MatchRect[0].BoxWidth / 2, GrayRes.MatchRect[0].BoxHeight / 2);
                            HOperatorSet.ZoomRegion(recRegion, out recRegion, 0.25, 0.25);
                            r1 = GrayRes.MatchRect[0].CenterPoint.Y / 4;
                            c1 = GrayRes.MatchRect[0].CenterPoint.X / 4;

                            // 绘制中心线        
                            HOperatorSet.GenRegionLine(out regionline1, r1 - 5, c1, r1 + 5, c1);
                            HOperatorSet.GenRegionLine(out regionline2, r1, c1 - 5, r1, c1 + 5);
                            HOperatorSet.ConcatObj(regionline1, regionline2, out regionline1);
                            HOperatorSet.VectorAngleToRigid(r1, c1, 0, r1, c1, -angle.TupleRad(), out HomMat2D2);
                            HOperatorSet.AffineTransRegion(regionline1, out regionline1, HomMat2D2, "nearest_neighbor");
                            HOperatorSet.ConcatObj(regionZoom, recRegion, out regionZoom);
                            HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);
                            recRegion.Dispose();
                            
                            break;
                        case 3:
                            IMVSContourMatchModuTool iMVSContourMatchModuTool = (IMVSContourMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.轮廓匹配1"];
                            ContourMatchResult ContourRes = iMVSContourMatchModuTool.ModuResult;
                            stringBuilder.Append("轮廓匹配框中心:(" + ContourRes.MatchRect[0].CenterPoint.X + "," + ContourRes.MatchRect[0].CenterPoint.Y + ")" + "\n");
                            stringBuilder.Append("角度:" + ContourRes.MatchRect[0].Angle + ",分数:" + ContourRes.MatchScore[0] + "\n");

                            //  定位结果绘制,模板框、轮廓点
                            angle = ContourRes.MatchRect[0].Angle;
                            HOperatorSet.GenRectangle2(out recRegion, ContourRes.MatchRect[0].CenterPoint.Y, ContourRes.MatchRect[0].CenterPoint.X, -angle.TupleRad(), ContourRes.MatchRect[0].BoxWidth / 2, ContourRes.MatchRect[0].BoxHeight / 2);
                            HOperatorSet.ZoomRegion(recRegion, out recRegion, 0.25, 0.25);
                            r1 = ContourRes.MatchRect[0].CenterPoint.Y / 4;
                            c1 = ContourRes.MatchRect[0].CenterPoint.X / 4;

                            // 绘制中心线        
                            HOperatorSet.GenRegionLine(out regionline1, r1 - 5, c1, r1 + 5, c1);
                            HOperatorSet.GenRegionLine(out regionline2, r1, c1 - 5, r1, c1 + 5);
                            HOperatorSet.ConcatObj(regionline1, regionline2, out regionline1);
                            HOperatorSet.VectorAngleToRigid(r1, c1, 0, r1, c1, -angle.TupleRad(), out HomMat2D2);
                            HOperatorSet.AffineTransRegion(regionline1, out regionline1, HomMat2D2, "nearest_neighbor");
                            HOperatorSet.ConcatObj(regionZoom, recRegion, out regionZoom);

                            HOperatorSet.ConcatObj(regionZoom, regionline1, out regionZoom);
                            recRegion.Dispose();

                            // 轮廓点
                            outlinePoints = ContourRes.MatchOutline.MatchOutlinePoints;

                            for (int i = 0; i < outlinePoints.Count; i += 8)
                            {
                                xList.Add((HTuple)outlinePoints[i].MatchOutlineX / 4);
                                yList.Add((HTuple)outlinePoints[i].MatchOutlineY / 4);
                            }

                            PointX = new HTuple(xList.ToArray());
                            PointY = new HTuple(yList.ToArray());

                            HOperatorSet.GenRegionPoints(out region, PointY, PointX);
                            HOperatorSet.ConcatObj(regionZoom, region, out regionZoom);
                            break;
                    }
                   


          
                    #region 绘制图像

                    // Blob结果
                    IMVSBlobFindModuTool BlobFindModuTool = (IMVSBlobFindModuTool)VmSolution.Instance[$"{this.GetVmProcedure().FullName}.BLOB分析1"];
                    BlobFindResult blobRes = BlobFindModuTool.ModuResult;

                    // Blob轮廓点
                    //HTuple Pointx = new HTuple((blobRes.ContourInfo.MatchOutlinePoints.Where((value, index) => index % 4 == 0).Select(p => ((HTuple)p.MatchOutlineX / 4)).ToArray()));
                    //HTuple Pointy = new HTuple((blobRes.ContourInfo.MatchOutlinePoints.Where((value, index) => index % 4 == 0).Select(p => (HTuple)p.MatchOutlineY / 4).ToArray()));
                    // 计算结果数组大小
                    int resultSize = (blobRes.ContourInfo.MatchOutlinePoints.Count + 3) / 4;
                    double[] pointXArray = new double[resultSize];
                    double[] pointYArray = new double[resultSize];

                    int resultIndex = 0;
                    // 同时处理X和Y坐标，只需一次循环
                    for (int i = 0; i < blobRes.ContourInfo.MatchOutlinePoints.Count && resultIndex < resultSize; i += 4)
                    {
                        var point = blobRes.ContourInfo.MatchOutlinePoints[i];
                        pointXArray[resultIndex] = point.MatchOutlineX / 4;
                        pointYArray[resultIndex] = point.MatchOutlineY / 4;
                        resultIndex++;
                    }

                    HTuple Pointx = new HTuple(pointXArray);
                    HTuple Pointy = new HTuple(pointYArray);
                    HOperatorSet.GenRegionPoints(out region, Pointy, Pointx);
                    HOperatorSet.ConcatObj(regionZoom, region, out regionZoom);
                    stringBuilder.Append("Blob个数:" + blobRes.BlobNum + "\n");

                    // 缺陷个数
                    IMVSEdgeFlawInspModuTool iMVSEdgeFlawInspModuTool = (IMVSEdgeFlawInspModuTool)VmSolution.Instance[$"{this.GetVmProcedure().FullName}.边缘模型缺陷检测1"];
                    EdgeFlawInspResult EdgeResult=iMVSEdgeFlawInspModuTool.ModuResult;
                    stringBuilder.Append("缺陷个数:" + EdgeResult.FlawNum + "\n");
                   

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
                    string MarkedWord = "";

                    IMVSFixtureModuTool iMVSFixtureModuTool = (IMVSFixtureModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.位置修正1"];
                    #region 绘制检测区域
                    HObject ROI3;
                    HOperatorSet.GenEmptyObj(out ROI3);

                    if (iMVSFixtureModuTool.ModuResult.ModuStatus == 0)
                    {
                        MarkedWord += "模板 - " + this.Name + " 粗定位模块失败。";
                        switch (CrudePRType[0])
                        {
                            case 0:
                                IMVSFastFeatureMatchModuTool iMVSFastFeatureMatchModuTool = (IMVSFastFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.快速匹配1"];
                                FastFeatureMatchResult FastRes = iMVSFastFeatureMatchModuTool.ModuResult;
                                HTuple angle3 = FastRes.ROI.Angle;
                                HOperatorSet.GenRectangle2(out ROI3, FastRes.ROI.CenterPoint.Y, FastRes.ROI.CenterPoint.X, -angle3.TupleRad(), FastRes.ROI.BoxWidth / 2, FastRes.ROI.BoxHeight / 2);
                                break;
                            case 1:
                                IMVSHPFeatureMatchModuTool iMVSHPFeatureMatchModuTool = (IMVSHPFeatureMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.高精度匹配1"];
                                HPFeatureMatchResult HPRes = iMVSHPFeatureMatchModuTool.ModuResult;
                                HTuple angle4 = HPRes.ROI.Angle;
                                HOperatorSet.GenRectangle2(out ROI3, HPRes.ROI.CenterPoint.Y, HPRes.ROI.CenterPoint.X, -angle4.TupleRad(), HPRes.ROI.BoxWidth / 2, HPRes.ROI.BoxHeight / 2);

                                break;
                            case 2:
                                IMVSGrayMatchModuVATool iMVSGrayMatchModuVATool = (IMVSGrayMatchModuVATool)VmSolution.Instance[$"{this.VmProcedure.FullName}.灰度匹配1"];
                                GrayMatchVAResult GrayRes = iMVSGrayMatchModuVATool.ModuResult;
                                HTuple angle5 = GrayRes.ROI.Angle;
                                HOperatorSet.GenRectangle2(out ROI3, GrayRes.ROI.CenterPoint.Y, GrayRes.ROI.CenterPoint.X, -angle5.TupleRad(), GrayRes.ROI.BoxWidth / 2, GrayRes.ROI.BoxHeight / 2);
                                break;
                            case 3:
                                IMVSContourMatchModuTool iMVSContourMatchModuTool = (IMVSContourMatchModuTool)VmSolution.Instance[$"{this.VmProcedure.FullName}.轮廓匹配1"];
                                ContourMatchResult ContourRes = iMVSContourMatchModuTool.ModuResult;
                                HTuple angle6 = ContourRes.ROI.Angle;
                                HOperatorSet.GenRectangle2(out ROI3, ContourRes.ROI.CenterPoint.Y, ContourRes.ROI.CenterPoint.X, -angle6.TupleRad(), ContourRes.ROI.BoxWidth / 2, ContourRes.ROI.BoxHeight / 2);
                                break;
                        }
                        HOperatorSet.ConcatObj(regionZoom, ROI3, out regionZoom);
                        HOperatorSet.ZoomRegion(regionZoom, out regionZoom, 0.25, 0.25);
                    }

                    // 判断Blob状态
                    IMVSBlobFindModuTool BlobFindModuTool = (IMVSBlobFindModuTool)VmSolution.Instance[$"{this.GetVmProcedure().FullName}.BLOB分析1"];
                    BlobFindResult blobRes = BlobFindModuTool.ModuResult;
                    if(blobRes.ModuStatus==0)
                    {
                        MarkedWord += "Blob模块失败。";
                        HObject region;
                        HOperatorSet.GenEmptyObj(out region);
                        if (blobRes.BlobNum!=0)
                        {

                            var outlinePoints = blobRes.ContourInfo.MatchOutlinePoints;
                            List<HTuple> xList = new List<HTuple>();
                            List<HTuple> yList = new List<HTuple>();

                            for (int i = 0; i < outlinePoints.Count; i += 4)
                            {
                                xList.Add((HTuple)outlinePoints[i].MatchOutlineX / 4);
                                yList.Add((HTuple)outlinePoints[i].MatchOutlineY / 4);
                            }

                            HTuple Pointx = new HTuple(xList.ToArray());
                            HTuple Pointy = new HTuple(yList.ToArray());

                            
                            HOperatorSet.GenRegionPoints(out region, Pointy, Pointx );
                            HOperatorSet.ConcatObj(regionZoom, region, out regionZoom);
                            region.Dispose();
                        }
                    }

                    // 判断边缘缺陷,绘制
                    IMVSEdgeFlawInspModuTool iMVSEdgeFlawInspModuTool = (IMVSEdgeFlawInspModuTool)VmSolution.Instance[$"{this.GetVmProcedure().FullName}.边缘模型缺陷检测1"];
                    EdgeFlawInspResult EdgeResult = iMVSEdgeFlawInspModuTool.ModuResult;
                    if(EdgeResult.ModuStatus==0)
                    {
                        MarkedWord+= "缺陷个数:" + EdgeResult.FlawNum;
                        HObject recRegion,region1;
                        HOperatorSet.GenEmptyObj(out recRegion);
                        HOperatorSet.GenEmptyObj(out region1);

                        for (int i = 0; i < EdgeResult.DefectBox.Count; i++)
                        {
                            HTuple angle1 = EdgeResult.DefectBox[i].Angle;
                            HOperatorSet.GenRectangle2(out recRegion, EdgeResult.DefectBox[i].CenterPoint.Y, EdgeResult.DefectBox[i].CenterPoint.X, -angle1.TupleRad(), EdgeResult.DefectBox[i].BoxWidth / 2, EdgeResult.DefectBox[i].BoxHeight / 2);
                            HOperatorSet.ZoomRegion(recRegion, out region1, 0.25, 0.25);
                            HOperatorSet.ConcatObj(regionZoom, region1, out regionZoom);
                            recRegion.Dispose();
                            region1.Dispose();
                        }
                    }
                  
                    ROI3.Dispose();
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
                LogHelper.Post(Level.Error, $"{this.Name}: FcUplookModelAlg 异常：{ex.Message} ", ex, Log.LogCategory.PR);
                return false;
            }
        }
    }
}
