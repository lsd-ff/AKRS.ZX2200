using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Models.Services;
using HalconDotNet;
using ImageSourceModuleCs;
using IMVSFixtureModuCs;
using log4net.Core;
using Newtonsoft.Json;
using SaveImageCs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using VM.Core;
using VM.PlatformSDKCS;

namespace AKRS.Galaxy2.PR.Models.Algs
{
    /// <summary>
    /// 算法基类  -- 算法流程
    /// </summary>
    [Serializable]
    public abstract class BaseAlg
    {
        // 放原始PR模型的文件夹
        public string SourcePR = Application.StartupPath+ "\\AlgFlowTemplates\\";

        /// <summary>
        /// 锁
        /// </summary>
        [JsonIgnore]
        private static object locker = new object();

        /// <summary>
        /// 算法名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 算法类型
        /// </summary>
        public AlgFlowTypeEnum AlgFlowType { get; set; }

        /// <summary>
        /// 算法归属
        /// </summary>
        public AlgBeLongEnum AlgBeLong { get; set; }

        /// <summary>
        /// 算法保存路径
        /// </summary>
        [JsonIgnore]
        public string AlgSavePath { get; set; }

        /// <summary>
        /// 启用粗定位
        /// </summary>
        public bool IsUseCrudeLocate { get; set; } = false;

        /// <summary>
        /// 启用使用粗定位角度
        /// </summary>
        public bool IsUseCrudeLocateAngle { get; set; } = false;

        /// <summary>
        /// 胶检输出总和
        /// </summary>
        public bool IsOutputTotal { get; set; } = false;

        /// <summary>
        /// 圆检测输出圆弧中点
        /// </summary>
        public bool IsOutputMidpoint { get; set; } = false;

        /// <summary>
        /// 缺陷检测阈值
        /// </summary>
        public int Threshold { get; set; } = 100;

        /// <summary>
        /// 缺陷检测筛选面积
        /// </summary>
        public int Area { get; set; } = 10;

        /// <summary>
        /// 矩形检测是否输出长宽尺寸
        /// </summary>
        public bool IsUseRecLength { get; set; } = false;


        /// <summary>
        /// 粗定位方式
        /// </summary>
        public LocateTypeEnum CrudeLocateType { get; set; } = LocateTypeEnum.FastModel;

        /// <summary>
        /// 精定位方式
        /// </summary>
        public LocateTypeEnum ExactLocateType { get; set; } = LocateTypeEnum.FastModel;

        /// <summary>
        /// 基准点坐标
        /// </summary>
        public AKRSPoint2D ReferencePoint { get; set; } = new AKRSPoint2D(0, 0);

        /// <summary>
        /// 流程对象
        /// </summary>
        [JsonIgnore]
        internal VmProcedure VmProcedure;

        /// <summary>
        /// 流程名称
        /// </summary>
        public string VmProcedureName { get; set; }

        /// <summary>
        /// 流程参数对象，设置输入参数
        /// </summary>
        [JsonIgnore]
        internal ProcedureParam ProcedureParam;

        /// <summary>
        /// 流程结果对象
        /// </summary>
        [JsonIgnore]
        internal ProcedureResult ProcedureResult;

        /// <summary>
        /// 流程图像源
        /// </summary>
        [JsonIgnore]
        internal ImageSourceModuleTool ImageSourceModuleTool;

        /// <summary>
        /// 流程输出图像对象
        /// </summary>
        [JsonIgnore]
        internal SaveImageTool SaveImageTool;

        /// <summary>
        /// 输入图像ImageBaseData
        /// </summary>      
        [JsonIgnore]
        internal ImageBaseData ImageBaseData;


        /// <summary>
        /// 对称模板输出结果X
        /// </summary>
        public double SymmetricCenterX;

        /// <summary>
        /// 对称模板输出结果Y
        /// </summary>
        public double SymmetricCenterY;

        /// <summary>
        /// 匹配结果
        /// </summary>
        [JsonIgnore]
        public List<BaseAlgResult> MatchResults { get; set; } = new List<BaseAlgResult>();


        /// <summary>
        /// 传入检测图像
        /// </summary>
        [JsonIgnore]
        public HObject SourceImg = null;

        /// <summary>
        /// 传入位置修正参数
        /// </summary>
        /// <param name="Runx">定位中心x</param>
        /// <param name="Runy">定位中心y</param>
        /// <param name="RunAngle">定位角度</param>
        public void SetFix(float Runx, float Runy, float RunAngle)
        {
            try
            {
                VM.PlatformSDKCS.PointF pointF = new VM.PlatformSDKCS.PointF(Runx, Runy);

                List<VM.PlatformSDKCS.PointF> origin = new List<VM.PlatformSDKCS.PointF> { pointF };

                List<float> angle = new List<float> { RunAngle };

                string ModuleName = $"{this.GetVmProcedure().FullName}.位置修正2";
                IMVSFixtureModuTool iMVSFixtureModuTool = (IMVSFixtureModuTool)VmSolution.Instance[ModuleName];

                iMVSFixtureModuTool.ModuParams.Origin = origin;

                List<float> scalex = new List<float> { 1 };
                List<float> scaley = new List<float> { 1 };
                iMVSFixtureModuTool.ModuParams.InAngle = angle;
                iMVSFixtureModuTool.ModuParams.MatchScaleX = scalex;
                iMVSFixtureModuTool.ModuParams.MatchScaleY = scaley;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"初始化位置修正信息异常", ex, LogCategory.PR);
            }
        }

        /// <summary>
        /// 保存流程和变量
        /// </summary>
        /// <param name="passWord">密码</param>
        public bool SaveProcedure(string passWord)
        {
            try
            {
                if (this.VmProcedure != null)
                {

                    string Filepath = this.GetPRSavePath() + this.Name + ".prc";

                    this.VmProcedure.SaveAs(Filepath, "");

                    this.VmProcedureName = this.VmProcedure.FullName;

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"保存检测流程异常", ex, LogCategory.PR);

                return false;
            }
        }

        /// <summary>
        /// 模板搜索
        /// </summary>
        /// <returns>是否成功</returns>
        public abstract bool FindModel();

        /// <summary>
        /// 外部传入检测图像
        /// </summary>
        /// <param name="bitmap">图片</param>
        public void SetImage(Bitmap bitmap)
        {
            try
            {
                if(bitmap != null)
                {
                    this.ImageBaseData = ImageHelp.BitmapToImageBaseData(bitmap);

                    if (this.VmProcedureName == null || this.VmProcedure == null || (VmProcedure)VmSolution.Instance[this.VmProcedure.FullName] == null)
                    {
                        this.GetVmProcedure();
                    }
                    if (this.ImageSourceModuleTool != null)
                    {
                        this.ImageSourceModuleTool.SetImageData(this.ImageBaseData);
                    }
                    

                    HObject image;
                    HOperatorSet.GenEmptyObj(out image);
                    Rectangle imgRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    BitmapData bitData = bitmap.LockBits(imgRect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
                    image.Dispose();
                    HOperatorSet.GenImage1(out image, "byte", bitmap.Width, bitmap.Height, bitData.Scan0);
                    bitmap.UnlockBits(bitData);
                    HOperatorSet.ZoomImageFactor(image, out this.SourceImg, 0.25, 0.25, "constant");
                    bitmap.Dispose();
                    image.Dispose();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"设置输入图像异常", ex, LogCategory.PR);
            }
        }

        /// <summary>
        /// 初始化流程
        /// </summary>
        public void InitVmProcedure()
        {
            try
            {
                lock (locker)
                {     
                    string Directorypath = this.GetPRSavePath() + this.Name;
                    if (File.Exists(Directorypath + ".prc"))
                    {
                        this.VmProcedure = VmProcedure.Load(Directorypath + ".prc", "");
                    }
                    else
                    {
                        this.VmProcedure = VmProcedure.Load(SourcePR + this.AlgFlowType.GetDescription() + ".prc", "");
                    }
      
                    this.VmProcedureName = this.VmProcedure.FullName;
                    this.ProcedureParam = this.VmProcedure.ModuParams;
                    this.ImageSourceModuleTool = VmSolution.Instance[this.VmProcedure.FullName + ".图像源1"] as ImageSourceModuleTool;
                    this.SaveImageTool = (SaveImageTool)VmSolution.Instance[this.VmProcedure.FullName + ".输出图像1"]; 
                }
            }
            catch (Exception ex)
            {

                LogHelper.Post(Level.Error, $"初始化流程异常", ex, LogCategory.PR);

            }
        }

        /// <summary>
        /// 获取保存路径
        /// </summary>
        /// <returns></returns>
        public string GetPRSavePath()
        {
            try
            {
                if (this.AlgBeLong == AlgBeLongEnum.Calibration)
                {
                    AlgSavePath = PathConfig.DeviceDirPath + "\\设备标定PR\\";

                }
                else
                {
                    AlgSavePath = PathConfig.DeviceDirPath + "\\所有产品PR\\";
                }
                if (!Directory.Exists(AlgSavePath))
                {
                    Directory.CreateDirectory(AlgSavePath);
                }
                return AlgSavePath;


            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"获取PR保存路径失败", ex, LogCategory.PR);
                return null;
            }
        }

        /// <summary>
        /// 获取流程
        /// </summary>
        /// <returns></returns>
        internal VmProcedure GetVmProcedure()
        {
            try
            {
                if (this.VmProcedureName == null || this.VmProcedure == null || (VmProcedure)VmSolution.Instance[this.VmProcedure.FullName] == null)
                {
                    this.InitVmProcedure();
                }
                return this.VmProcedure;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"获取流程异常", ex, LogCategory.PR);
                return null;
            }
        }

    }
}
