using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.Galaxy2.PR.Controls;
using AKRS.Galaxy2.PR.Models.Algs;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using DevExpress.XtraEditors;
using LanguageExt;
using LanguageExt.Common;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AKRS.Galaxy2.PR.Models.Entities
{
    /// <summary>
    /// PR实体
    /// </summary>
    [Serializable]
    public class PREntity : BaseVisionEntity
    {
        private static object lockObj = new object();

        /// <summary>
        /// 灯光锁
        /// </summary>
        private static object lockObLight = new object();

        /// <summary>
        /// 是否存图
        /// </summary>
        public static Func<bool> IsSaveLocateImage;

        /// <summary>
        /// 是否存图
        /// </summary>
        public static Action<Bitmap,List<string>,string> SaveLocateImageAction;

        /// <summary>
        /// 算法模板
        /// </summary>
        public BaseAlg Alg { get; set; }

        /// <summary>
        ///  是否被系统1复制
        /// </summary>
        public bool IsSystem1Copy { get; set; } = false;

        public Bitmap OriginalBmp { get; set; } = null;

        /// <summary>
        /// 是否为检测的PR，当为true时，执行PR时会有特殊处理
        /// </summary>
        public bool IsDefectPR { get; set; } = false;

        /// <summary>
        ///  构造函数
        /// </summary>
        public PREntity()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        public PREntity(string name)
        {
            this.name = name;

            this.Alg = AlgsFactory.Create(AlgFlowTypeEnum.XldModelAlg, name);
        }




        /// <summary>
        /// 提前设置光源
        /// </summary>
        /// <returns></returns>
        public void SetLight(PRHardware pRHardware = null, List<Light> bondLights = null, List<int> bondIntensities = null)
        {
            try
            {
                List<Light> lights = this.PRLightList.Where(a => a.IsUse).Select(a => a.Light).ToList();

                // 如果是手动出入的硬件，则根据硬件赋值
                if (pRHardware != null)
                {
                    lights.Clear();
                    foreach (string lightName in pRHardware.LightName) 
                    {
                        lights.Add(HardwareRepositoryService.GetHardware<Light>(lightName));
                    }
                }

                List<int> intensities = this.PRLightList.Where(a => a.IsUse).Select(a => a.LightIntensity).ToList();

                if (bondLights != null && bondIntensities != null)
                {
                    lights = bondLights;
                    intensities = bondIntensities;
                }

                lock (lockObLight)
                {
                    for (var i = 0; i < lights.Count; i++)
                    {
                        if (lights[i] != null)
                        {
                            lights[i].SetIntensity(intensities[i]);
                        }
                    }

                }

                AKRSCamera camera = this.Camera;

                if (pRHardware != null)
                {
                    camera = HardwareRepositoryService.GetHardware<AKRSCamera>(pRHardware.CameraName);
                }

                bool isExposeSuccess = camera.SetExposureTime(this.Exposure);
                
                bool isGainSuccess = camera.SetGain(this.Gain);

                bool isGammaSuccess = camera.SetGamma(this.Gamma / 10);

                if (!isExposeSuccess || !isGainSuccess || !isGammaSuccess) 
                {
                    throw new Exception("设置相机参数失败");
                }
            }
            catch (Exception)
            {
                throw new Exception("设置灯光和相机参数失败");
            }
        }

        /// <summary>
        /// 设置硬件
        /// </summary>
        /// <param name="pRHardware">硬件对象</param>
        public void SetHardware(PRHardware pRHardware)
        {
            this.CameraName = pRHardware.CameraName;
            this.AxisXName = pRHardware.AxisXName;
            this.AxisYName = pRHardware.AxisYName;
            this.AxisZName = pRHardware.AxisZName;
            for (int i = 0; i < pRHardware.LightName.Length(); i++)
            {

                if (this.PRLightList.Count() == 0 || this.PRLightList.Count() <= i)
                {
                    this.PRLightList.Add(new PRLight());
                }
                this.PRLightList[i].LightName = pRHardware.LightName[i];
            }
        }

        /// <summary>
        /// 查找定位
        /// </summary>
        /// <param name="bmp">传入图像</param>
        /// <returns>定位结果</returns>
        public ExcuteResult DoWork(Bitmap bmp)
        {
            // 执行PR
            lock (lockObj)
            {
                ExcuteResult prResult = ExcuteResult.Fail;

                Stopwatch sp = Stopwatch.StartNew();

                //Bitmap bmpClone = null;

                if (bmp == null)
                {
                    LogHelper.Post(Level.Error, $"{this.GetName()}中图像为空", LogCategory.PR);
                    this.Description = "图像为空";
                    return prResult;
                }

                //bmpClone = bmp /*(Bitmap)bmp.Clone()*/;
                OriginalBmp= bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), bmp.PixelFormat);
                Bitmap bitmapClone = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), bmp.PixelFormat);
                Stopwatch stopwatch = Stopwatch.StartNew();
                this.Alg.SetImage(bmp);
                bool IsSuccess = this.Alg.FindModel();
                AlgResults = this.Alg.MatchResults;
                LogHelper.Post(Level.Info, $"{this.GetName()} PR执行耗时：{stopwatch.ElapsedMilliseconds} ms", LogCategory.PR);

                if (IsSuccess)
                {
                    if (IsSaveImage)
                    {
                        Task.Run(() =>
                            {
                                try
                                {
                                    lock (bitmapClone)
                                    {
                                        // 文件夹路径
                                        string folderPath = @"D:\视觉定位成功图像\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\"
                                                            + this.name;

                                        // 检查文件夹是否存在
                                        if (!Directory.Exists(folderPath))
                                        {
                                            Directory.CreateDirectory(folderPath);
                                        }

                                        // 文件名
                                        string fileName = $"{DateTime.Now.ToString("yyMMddHHmmssfff")}.bmp";

                                        string fullPath = Path.Combine(folderPath, fileName); // 组合成完整路径

                                        // 保存图像到指定路径
                                        bitmapClone.Save(fullPath);
                                        bitmapClone.Dispose();
                                    }
                                }
                                catch
                                { }
                            });

                    }
                    prResult = ExcuteResult.Success;
                }
                else
                {
                    if (IsSaveImage)
                    {
                        Task.Run(() =>
                        {
                            try
                            {
                                lock (bitmapClone)
                                {
                                    // 文件夹路径
                                    string folderPath = @"D:\视觉定位失败图像\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\"
                                                        + this.name;

                                    // 检查文件夹是否存在
                                    if (!Directory.Exists(folderPath))
                                    {
                                        Directory.CreateDirectory(folderPath);
                                    }

                                    // 文件名
                                    string fileName = $"{DateTime.Now.ToString("yyMMddHHmmssfff")}.bmp";

                                    string fullPath = Path.Combine(folderPath, fileName); // 组合成完整路径

                                    // 保存图像到指定路径
                                    bitmapClone.Save(fullPath);
                                    bitmapClone.Dispose();
                                }
                            }
                            catch
                            { }
                        });

                    }
                    prResult = ExcuteResult.Fail;
                }
                if (bmp != null)
                {
                    bmp.Dispose();
                }

                #region 画好图像抛给主软件影像区

                if (prResult == ExcuteResult.Success)
                {
                    PRResultShowModel prShowModel = new PRResultShowModel();

                    prShowModel.Name = this.GetName();

                    Bitmap bitmap = (Bitmap)this.AlgResults[0].OutPutImg1;

                    prShowModel.PRImage = (Bitmap)bitmap.Clone();
                    prShowModel.PRResults = this.AlgResults;
                    prShowModel.Times = sp.ElapsedMilliseconds;
                    prShowModel.CameraName = Camera.HardwareName;
                    prShowModel.AlgFlowType = this.Alg.AlgFlowType;

                    if (this.IsDefectPR && FrmDefectResult.IsBroadcastBlockConnected)
                    {
                        FrmDefectResult.PrResultImageBlock.Post(prShowModel);
                    }
                    else if (FrmVmVisionResult.IsBroadcastBlockConnected)
                    {
                        FrmVmVisionResult.PrResultImageBlock.Post(prShowModel);
                    }
                    else
                    {
                        prShowModel.PRImage.Dispose();
                    }
                }

                return prResult;
            }


            #endregion


        }


        /// <summary>
        /// 执行PR
        /// </summary>
        /// <param name="isSetLight">是否设置灯光</param>
        /// <param name="postImage">是否抛图</param>
        /// <returns>执行结果</returns>
        public override ExcuteResult DoWork(PRHardware pRHardware = null,bool isSetLight = true, bool postImage = true)
        {
            lock (lockObj)
            {
                if (pRHardware != null)
                {
                    this.SetHardware(pRHardware);
                }

                ExcuteResult prResult = ExcuteResult.Fail;

                Bitmap bmpClone = null;

                Stopwatch sp = Stopwatch.StartNew();

                #region 设置光源
                if (isSetLight)
                {
                    Stopwatch sw = Stopwatch.StartNew();

                    List<Light> lights = this.PRLightList.Where(a => a.IsUse).Select(a => a.Light).ToList();

                    List<int> intensities = this.PRLightList.Where(a => a.IsUse).Select(a => a.LightIntensity).ToList();

                    lock (lockObj)
                    {
                        for (var i = 0; i < lights.Count; i++)
                        {
                            if (lights[i] != null)
                            {
                                lights[i].SetIntensity(intensities[i]);
                            }
                        }
                    }
                    Thread.Sleep(15);

                    if (!this.Camera.SetExposureTime(this.Exposure))
                    {
                        LogHelper.Post(Level.Error, $"{this.GetName()}的{Camera.HardwareName}设置曝光为{Exposure}出错,当前曝光:{Camera.GetExposureTime()}", LogCategory.PR);
                    }

                    if (!this.Camera.SetGain(this.Gain))
                    {
                        LogHelper.Post(Level.Error, $"{this.GetName()}的{Camera.HardwareName}设置增益为{Gain}出错,当前增益:{Camera.GetGain()}", LogCategory.PR);
                    }

                    if (!this.Camera.SetGamma(this.Gamma / 10))
                    {
                        LogHelper.Post(Level.Error, $"{this.GetName()}的{Camera.HardwareName}设置伽马为{Gamma}出错,当前伽马:{Camera.GetGamma()}", LogCategory.PR);
                    }

                    LogHelper.Post(Level.Info, $"{this.GetName()} 设置光源，曝光，增益，伽马耗时{sw.ElapsedMilliseconds} ms", LogCategory.PR);
                }
                #endregion

                this.Excute(Camera,postImage, ref bmpClone, ref prResult);

                if (bmpClone != null)
                {
                    bmpClone.Dispose();
                }
                #region 画好图像抛给主软件影像区

                if (prResult == ExcuteResult.Success)
                {
                    PRResultShowModel prShowModel = new PRResultShowModel();

                    prShowModel.Name = this.GetName();
                    prShowModel.PRImage = this.AlgResults[0].OutPutImg1;
                    prShowModel.PRResults = this.AlgResults;
                    prShowModel.Times = sp.ElapsedMilliseconds;
                    prShowModel.CameraName = Camera.HardwareName;
                    prShowModel.AlgFlowType = this.Alg.AlgFlowType;

                    if (postImage)
                    {
                        if (this.IsDefectPR && FrmDefectResult.IsBroadcastBlockConnected)
                        {
                            FrmDefectResult.PrResultImageBlock.Post(prShowModel);
                        }
                        else if (FrmVmVisionResult.IsBroadcastBlockConnected)
                        {
                            FrmVmVisionResult.PrResultImageBlock.Post(prShowModel);
                        }
                        else
                        {
                            prShowModel.PRImage.Dispose();
                        }
                    }
                }
                #endregion

                return prResult;
            }
        }

        /// <summary>
        /// 采图
        /// </summary>
        /// <param name="camera">相机对象</param>
        /// <returns>结果</returns>
        public Bitmap Photograph(string camera)
        {
            this.CameraName = camera;

            Bitmap bmp = this.Camera.SnapShot(false, SnapImageFormat.Format8bppIndexed);
            if (bmp == null)
            {
                throw new Exception("采图失败");
            }
            return bmp;
        }

        /// <summary>
        /// 执行PR
        /// </summary>
        /// <param name="camera">相机对象</param>
        /// <param name="bmpClone">PR产生的图片</param>
        /// <param name="result">PR结果</param>
        private void Excute(AKRSCamera camera,bool postImage, ref Bitmap bmpClone, ref ExcuteResult result)
        {
            result = ExcuteResult.None;

            // 采图
            Stopwatch stopwatch = Stopwatch.StartNew();

            using (Bitmap bmp = camera.SnapShot(false, SnapImageFormat.Format8bppIndexed))
            {
                LogHelper.Post(Level.Info, $"{this.GetName()} 采图耗时：{stopwatch.ElapsedMilliseconds} ms", LogCategory.PR);

                if (bmp == null)
                {
                    LogHelper.Post(Level.Error, $"{this.GetName()}中{camera.HardwareName}采图失败", LogCategory.PR);
                    this.Description = "采图失败";
                    return;
                }

                bmpClone = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), bmp.PixelFormat);
                // OriginalBmp = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), bmp.PixelFormat);
               
                #region 采图后光源亮度设为0
                if (IsFlash)
                {
                    Task closeLightTask = null;

                    if (this.IsFlash && camera.TriggerModel != ETriggerModel.HardWare)
                        closeLightTask = Task.Factory.StartNew(
                            (w) =>
                            {
                                CommonUtil.SetCurrentThreadName($"PR彩图后关闭光源 线程");

                                List<Light> lights = this.PRLightList.Where(a => a.IsUse).Select(a => a.Light).ToList();
                                for (int i = 0; i < lights.Count; i++)
                                {
                                    if (lights[i] != null)
                                    {
                                        lights[i].SetIntensity(0);
                                    }
                                }

                            },
                            new CancellationTokenSource().Token);
                }
                #endregion

                // 执行PR
                lock (this)
                {
                    stopwatch.Restart();

                    this.Alg.SetImage(bmpClone);
                    bool IsSuccess = this.Alg.FindModel();
                    AlgResults = this.Alg.MatchResults;

                    LogHelper.Post(Level.Info, $"{this.GetName()} PR执行耗时：{stopwatch.ElapsedMilliseconds} ms", LogCategory.PR);

                    try
                    {
                        // 结果赋值
                        if (IsSuccess)
                        {
                            // 保存原图
                            if (IsSaveImage)
                            {
                                List<string> pathList = new List<string>()
                            {
                               @"D:\视觉定位成功图像\",
                               DateTime.Now.ToString("yyyy-MM-dd"),
                               "定位原图",
                               this.name
                            };

                                Bitmap bitmap = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), bmp.PixelFormat);
                                SaveLocateImageAction(bitmap, pathList, $"{DateTime.Now.ToString("yyMMddHHmmssfff")}.bmp");
                            }

                            if (IsSaveLocateImage() && postImage)
                            {
                                List<string> pathList = new List<string>()
                            {
                               @"D:\视觉定位成功图像\",
                               DateTime.Now.ToString("yyyy-MM-dd"),
                               "定位结果图",
                               this.name
                            };

                                Bitmap bitmap = AlgResults[0].OutPutImg1.Clone(new Rectangle(0, 0, AlgResults[0].OutPutImg1.Width, AlgResults[0].OutPutImg1.Height), AlgResults[0].OutPutImg1.PixelFormat);
                                SaveLocateImageAction(bitmap, pathList, $"{DateTime.Now.ToString("yyMMddHHmmssfff")}.bmp");

                                //Task.Run(() =>
                                //{
                                //    try
                                //    {
                                //        lock (bitmap)
                                //        {
                                //            // 文件夹路径
                                //            string folderPath = @"D:\视觉定位成功图像\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\"
                                //                                + this.name;

                                //            // 检查文件夹是否存在
                                //            if (!Directory.Exists(folderPath))
                                //            {
                                //                Directory.CreateDirectory(folderPath);
                                //            }

                                //            // 文件名
                                //            string fileName = $"{DateTime.Now.ToString("yyMMddHHmmssfff")}.bmp";

                                //            string fullPath = Path.Combine(folderPath, fileName); // 组合成完整路径

                                //            // 保存图像到指定路径
                                //            bitmap.Save(fullPath);
                                //            bitmap.Dispose();
                                //        }
                                //    }
                                //    catch
                                //    { }
                                //});

                            }

                            result = ExcuteResult.Success;
                        }
                        else
                        {
                            if (IsSaveImage)
                            {
                                Bitmap bitmap = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), bmp.PixelFormat);

                                List<string> pathList = new List<string>()
                            {
                                @"D:\视觉定位失败图像\",
                               DateTime.Now.ToString("yyyy-MM-dd"),
                               this.name
                            };

                                SaveLocateImageAction(bitmap, pathList, $"{DateTime.Now.ToString("yyMMddHHmmssfff")}.bmp");

                                //Task.Run(() =>
                                //{                                
                                //    try
                                //    {
                                //        lock (bitmap)
                                //        {
                                //            // 文件夹路径
                                //            string folderPath = @"D:\视觉定位失败图像\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\"
                                //                                + this.name; 

                                //            // 检查文件夹是否存在
                                //            if (!Directory.Exists(folderPath))
                                //            {
                                //                Directory.CreateDirectory(folderPath);
                                //            }

                                //            // 文件名
                                //            string fileName = $"{DateTime.Now.ToString("yyMMddHHmmssfff")}.bmp";

                                //            string fullPath = Path.Combine(folderPath, fileName); // 组合成完整路径

                                //            // 保存图像到指定路径
                                //            bitmap.Save(fullPath);
                                //            bitmap.Dispose();
                                //        }
                                //    }
                                //    catch
                                //    { }
                                //});

                            }

                            result = ExcuteResult.Fail;

                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Post(Level.Info, $"{this.GetName()},存图失败,{ Thread.CurrentThread.Name},{ex.Message}", LogCategory.PR);
                    }
                }
            }
        }

        /// <summary>
        /// 改变PR文件路径
        /// </summary>
        public void ChangeFilePath(string oldName, string newName)
        {
            string oldConfigDir = this.Alg.GetPRSavePath() + oldName + ".prc";
            string newConfigDir = this.Alg.GetPRSavePath() + newName + ".prc";
            if (oldName != newName)
            {
                DirAndFileHelper.DeleteFile(oldConfigDir);
            }
        }

        /// <summary>
        /// 克隆一个此对象
        /// </summary>
        /// <param name="newName"></param>
        /// <returns></returns>
        public PREntity Clone(string newName)
        {
            try
            {
                PREntity res = new PREntity();
                string tmppath = Guid.NewGuid() + ".json";
                JsonFormatHelper<PREntity>.SaveGenericObject(this, tmppath);
                res = JsonFormatHelper<PREntity>.ReadGenericObject(tmppath);
                DirAndFileHelper.DeleteFile(tmppath);
                res.SetName(newName);
                return res;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 克隆一个此对象
        /// </summary>
        /// <param name="newName"></param>
        /// <returns></returns>
        //public PREntity CloneToMemory()
        //{
        //    try
        //    {
        //        PREntity prCopy = new PREntity();
        //        prCopy.Guid = Guid.NewGuid();
        //        prCopy.name = this.name;
        //        prCopy.AxisXName = this.AxisXName;
        //        prCopy.AxisYName = this.AxisYName;
        //        prCopy.AxisZName = this.AxisZName;

        //        prCopy.PRLightList = new List<PRLight>();

        //        for (int i = 0; i < this.PRLightList.Count; i++)
        //        {
        //            prCopy.PRLightList.Add(this.PRLightList[i]);
        //        }

        //        prCopy.IsSystem1Copy = false;
        //        prCopy.IsFlash = this.IsFlash;
        //        prCopy.Alg = this.Alg;

        //        //foreach (System.Reflection.PropertyInfo p in this.GetType().GetProperties())
        //        //{
        //        //    try {                                 
        //        //        if (p.GetCustomAttributes(false).Length > 0 
        //        //            && p.GetCustomAttributes(false).Select(a => a.ToString()).Contains("Newtonsoft.Json.JsonIgnoreAttribute"))
        //        //        {
        //        //            continue;
        //        //        }

        //        //        prCopy.GetType().GetProperty(p.Name).SetValue(prCopy, p.GetValue(this));

        //        //    }
        //        //    catch (Exception ex)
        //        //    {
        //        //        return null;
        //        //    }

        //        //}




        //        return prCopy;
        //    }
        //    catch(Exception ex)
        //    {                
        //        return null;
        //    }
        //}


        /// <summary>
        /// 复制PREntity
        /// </summary>
        /// <param name="SourcePREntityName">源PR名称</param>
        /// <param name="DestinationPREntityName">新PR名称</param>
        public static PREntity CopyPREntity(string SourcePREntityName, string DestinationPREntityName)
        {
            // 在视觉库里查找PR
            PREntity OriginPREntity = (PREntity)VisionEntityRepository.GetInstance().Find(SourcePREntityName);

            if (OriginPREntity == null)
            {
                // XtraMessageBox.Show("源PR文件不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            // 复制prc文件
            DirAndFileHelper.CopyFile(OriginPREntity.Alg.GetPRSavePath() + OriginPREntity.GetName() + ".prc", OriginPREntity.Alg.GetPRSavePath() + DestinationPREntityName + ".prc");
            PREntity CpoyPREntity = OriginPREntity.Clone(DestinationPREntityName);

            CpoyPREntity.Alg.Name = DestinationPREntityName;
            CpoyPREntity.Alg.InitVmProcedure();


            BaseVisionEntity CopyBaseVisionEntity = CpoyPREntity;
            CopyBaseVisionEntity.SetName(DestinationPREntityName);

            // 添加保存至视觉库
            VisionEntityRepository.GetInstance().AddVisionEntity(CopyBaseVisionEntity);

            return CpoyPREntity;
        }

        /// <summary>
        /// 删除PR
        /// </summary>
        /// <param name="PREntityName">PR名称</param>
        /// <returns></returns>
        public static void DeletePREntity(string PREntityName)
        {
            // 在视觉库里查找PR
            PREntity OriginPREntity = (PREntity)VisionEntityRepository.GetInstance().Find(PREntityName);

            if (OriginPREntity == null)
            {
                return;
            }

            // 删除prc文件
            string path = OriginPREntity.Alg.GetPRSavePath();
            string tempPath = Path.Combine(path, OriginPREntity.GetName() + ".prc"); 
     
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }

            // 从视觉库移除
            VisionEntityRepository.GetInstance().RemoveVisionEntity(OriginPREntity);
            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 获取当前算法类型
        /// </summary>
        /// <returns>算法类型枚举（AlgFlowTypeEnum）</returns>
        public AlgFlowTypeEnum GetAlgFlowType()
        {
            // 若Alg未初始化，返回默认类型（FastModelAlg）
            if (Alg == null)
            {
                LogHelper.Post(Level.Warn, $"{GetName()}的算法实例未初始化，返回默认算法类型", LogCategory.PR);
                return AlgFlowTypeEnum.FastModelAlg;
            }
            return Alg.AlgFlowType;
        }

        /// <summary>
        /// 设置算法类型（会重新初始化算法实例）
        /// </summary>
        /// <param name="algType">目标算法类型</param>
        /// <returns>是否设置成功</returns>
        public bool SetAlgFlowType(AlgFlowTypeEnum algType)
        {
            try
            {
                lock (lockObj) // 加锁确保线程安全
                {
                    // 若当前算法类型与目标一致，无需修改
                    if (Alg != null && Alg.AlgFlowType == algType)
                    {
                        LogHelper.Post(Level.Info, $"{GetName()}当前算法类型已为{algType}，无需修改", LogCategory.PR);
                        return true;
                    }

                    // 重新创建算法实例（使用新类型）
                    var newAlg = AlgsFactory.Create(algType, this.name);
                    newAlg.AlgFlowType = algType;
                    if (newAlg == null)
                    {
                        LogHelper.Post(Level.Error, $"创建算法类型{algType}失败，工厂返回空实例", LogCategory.PR);
                        return false;
                    }

                    // 替换算法实例并初始化
                    Alg = newAlg;

                    Alg.InitVmProcedure(); // 重新初始化流程

                    LogHelper.Post(Level.Info, $"{GetName()}算法类型已设置为{algType}", LogCategory.PR);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"{GetName()}设置算法类型{algType}失败", ex, LogCategory.PR);
                return false;
            }
        }
    }
}



