namespace AKRS.ZX2200.Infrastructure.Service
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using AKRS.Galaxy2.Component.Simple.MessageBox;
    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.Galaxy2.UserManager.Models;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using AKRS.ZX2200.TransportUnitSystem.Service;
    using ch.etel.edi.dsa.v40;
    using DevExpress.DataAccess.Native;
    using DevExpress.Utils;

    using LanguageExt;
    using LanguageExt.Pipes;
    using log4net.Core;
    using MathNet.Numerics;

    using PostSharp.Aspects.Advices;

    /// <summary>
    /// 视觉服务类
    /// </summary>
    public static class VisionService
    {
        /// <summary>
        /// 锁
        /// </summary>
        public static readonly object lockObj = new object();

        /// <summary>
        /// 存图线程
        /// </summary>
        private static Thread thread;

        /// <summary>
        /// 拍照定位结果
        /// </summary>
        /// <param name="pRName">pR名称</param>
        /// <param name="prHardware">重试次数</param>
        /// <param name="systemName">系统名称</param>
        /// <param name="bitmapName">图片的名称</param>
        /// <param name="setLight">设置光源</param>
        /// <returns>结果</returns>
        public static BaseAlgResult Vision(string pRName, PRHardware prHardware, string systemName = "Other", string bitmapName = "", bool setLight = true)
        {
            // 寻找Pr模板
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(pRName);

            // 如果没有找到，直接报警
            if (pREntity == null)
            {
                AKRSMessageBoxExt.Show(
                    $"未找到PR： {pRName}\r\n",
                    "报警",
                    new string[] { "终止" },
                    new DialogResult[] { DialogResult.Abort },
                    AlarmLevel.SecondLevel);
                return null;
            }

            lock (pREntity)
            {
                //if (!MachineSoftwareConfiguration.GetInstance().System1And2UseSamePr)
                //{
                //    if (pREntity.CameraName != "点胶相机")
                //    {
                //        // 因为系统1和系统2用的是同一份模板，所以定位失败的时候会造成资源冲突，所以点胶这里复制一份模板再定位
                //        if (systemName == "Dispense")
                //        {
                //            pREntity = DispenseRunTimeProvider.GetDispensePREntity(pREntity);
                //        }
                //    }
                //}

                Stopwatch sp = Stopwatch.StartNew();

                ExcuteResult result = pREntity.DoWork(prHardware,setLight, true);
                sp.Stop();

                LogHelper.Post(Level.Info, $" {pRName}定位完成，定位用时{sp.ElapsedMilliseconds}ms", LogCategory.PR);

                if (result == ExcuteResult.Fail)
                {
                    // 定位失败不存图，直接返回空
                    return null;
                }

                //string tuName = systemName == "Dispense"
                //                    ? TransportProgram.GetInstance().DispenseSubSectionProgram.TransportUnit?.Name
                //                    : TransportProgram.GetInstance().BondSubSectionProgram.TransportUnit?.Name;

                //SaveVisionImage(
                //    pREntity.AlgResult.OutPutImg1,
                //    new List<string>() { systemName, tuName, pREntity.GetName() },
                //    bitmapName,
                //    pREntity.OriginalBmp);

                if (result == ExcuteResult.Success)
                {
                    if (pREntity.AlgResult is MatchResult)
                    {
                        MatchResult matchResult = (MatchResult)pREntity.AlgResult;

                        return new MatchResult(matchResult.CenterX, matchResult.CenterY, matchResult.Angle);
                    }

                    return pREntity.AlgResult;
                }

                return null;
            }
           
        }

        /// <summary>
        /// 拍照定位结果
        /// </summary>
        /// <param name="pRName">pR名称</param>
        /// <param name="prHardware">重试次数</param>
        /// <param name="systemName">系统名称</param>
        /// <param name="bitmapName">图片的名称</param>
        /// <param name="setLight">设置光源</param>
        /// <returns>结果</returns>
        public static List<BaseAlgResult> VisionDefect(string pRName, PRHardware prHardware, string systemName = "Other", string bitmapName = "", bool setLight = true)
        {
            // 寻找Pr模板
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(pRName);

            pREntity.IsDefectPR = true;

            // 如果没有找到，直接报警
            if (pREntity == null)
            {
                AKRSMessageBoxExt.Show(
                    $"Not find Program Named {pRName}\r\n",
                    "Alarm",
                    new string[] { "Abort" },
                    new DialogResult[] { DialogResult.Abort },
                    AlarmLevel.SecondLevel);
                return null;
            }

            lock (pREntity)
            {
                Stopwatch sp = Stopwatch.StartNew();

                ExcuteResult result = pREntity.DoWork(prHardware,setLight, true);

                sp.Stop();

                LogHelper.Post(Level.Info, $" {pRName}定位完成，定位用时{sp.ElapsedMilliseconds}ms", LogCategory.PR);

                //string tuName = systemName == "Dispense"
                //                    ? TransportProgram.GetInstance().DispenseSubSectionProgram.TransportUnit?.Name
                //                    : TransportProgram.GetInstance().BondSubSectionProgram.TransportUnit?.Name;

                //SaveVisionImage(
                //    pREntity.AlgResult.OutPutImg1,
                //    new List<string>() { systemName, tuName, pREntity.GetName() },
                //    bitmapName,
                //    pREntity.OriginalBmp);

                if (result == ExcuteResult.Success)
                {
                    return pREntity.AlgResults;
                }

                return null;
            }
        }

        /// <summary>
        /// 保存视觉图片
        /// </summary>
        /// <param name="bitmap">定位结果图片</param>
        /// <param name="filePath">地址</param>
        /// <param name="bitmapName">定位图片名称</param>
        /// <param name="bitmap2">定位原图</param>
        public static void SaveVisionImage(Bitmap bitmap, List<string> filePath,string bitmapName)
        {
            try
            {
                string allPath = "";

                // 创建文件夹
                foreach (string path in filePath)
                {
                    if (path == string.Empty)
                    {
                        continue;
                    }

                    allPath = Path.Combine(allPath, path);
                    if (!Directory.Exists(allPath))
                    {
                        Directory.CreateDirectory(allPath);
                    }
                }

                // 非法字符剔除
                foreach (char rInvalidChar in System.IO.Path.GetInvalidPathChars())
                {
                    if (allPath.Contains(rInvalidChar.ToString()))
                    {
                        allPath = allPath.Replace(rInvalidChar.ToString(), string.Empty);
                    }
                }

                // 考虑磁盘是否够用
                BitmapBlockingCollection.Add(((Bitmap)bitmap, allPath + "//" + bitmapName));
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障！", ex, LogCategory.PR);
            }
        }

        /// <summary>
        /// 距离检查
        /// </summary>
        /// <param name="baseMatter">实体对象</param>
        /// <returns>结果</returns>
        public static DialogResult DistanceCheck(BaseMatter baseMatter)
        {
            if (!baseMatter.Config.LocateConfig.DistanceCheck)
            {
                return DialogResult.OK;
            }

            double p1XDistance = baseMatter.Config.LocateConfig.P1VisionRelativePos.X
                                 - baseMatter.BaseInfo.LocateResultInfo.P1Info.ResultPoint3D.X;

            double p1YDistance = baseMatter.Config.LocateConfig.P1VisionRelativePos.Y
                                 - baseMatter.BaseInfo.LocateResultInfo.P1Info.ResultPoint3D.Y;

            double p2XDistance = baseMatter.Config.LocateConfig.P2VisionRelativePos.X
                                 - baseMatter.BaseInfo.LocateResultInfo.P2Info.ResultPoint3D.X;

            double p2YDistance = baseMatter.Config.LocateConfig.P2VisionRelativePos.Y
                                 - baseMatter.BaseInfo.LocateResultInfo.P2Info.ResultPoint3D.Y;

            double xDistance = Math.Abs(p1XDistance - p2XDistance);

            double yDistance = Math.Abs(p1YDistance - p2YDistance);

            double oldDistance = Math.Sqrt(
                (Math.Pow(
                     baseMatter.Config.LocateConfig.P1VisionRelativePos.X
                     - baseMatter.Config.LocateConfig.P2VisionRelativePos.X,
                     2) + Math.Pow(
                     baseMatter.Config.LocateConfig.P1VisionRelativePos.Y
                     - baseMatter.Config.LocateConfig.P2VisionRelativePos.Y,
                     2)));

            double newDistance = Math.Sqrt(
                (Math.Pow(
                     baseMatter.BaseInfo.LocateResultInfo.P1Info.ResultPoint3D.X
                     - baseMatter.BaseInfo.LocateResultInfo.P2Info.ResultPoint3D.X,
                     2) + Math.Pow(
                     baseMatter.BaseInfo.LocateResultInfo.P1Info.ResultPoint3D.Y
                     - baseMatter.BaseInfo.LocateResultInfo.P2Info.ResultPoint3D.Y,
                     2)));


            if (Math.Abs(oldDistance - newDistance) > baseMatter.Config.LocateConfig.DistanceTolerance)
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"{baseMatter.Name} 定位距离容差过大,标准：{oldDistance},现在：{newDistance},请选择怎么处理",
                    "报警",
                    new[] { "手动对点", "忽略", "跳过", "停止" },
                    new[] { DialogResult.Retry, DialogResult.Ignore, DialogResult.None, DialogResult.Abort });

                return dialogResult;
            }

            baseMatter.BaseInfo.LocateResultInfo.OldTwoPointDistance = oldDistance;

            baseMatter.BaseInfo.LocateResultInfo.TwoPointDistance = newDistance;

            return DialogResult.OK;
        }

        /// <summary>
        /// 存图缓存
        /// </summary>
        public static BlockingCollection<(Bitmap, string)> BitmapBlockingCollection { get; set; } =
            new BlockingCollection<(Bitmap, string)>();

        /// <summary>
        /// 开启存图线程
        /// </summary>
        public static void StartSaveThread()
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(
                                  () =>
                                  {
                                      SaveBitmap();
                                  })
                { Name = "存图线程", IsBackground = true };
                thread.Start();
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public static void SaveBitmap()
        {
            while (true)
            {
                var item = BitmapBlockingCollection.Take();

                if (item.Item1 != null)
                {
                    item.Item1.Save(item.Item2, System.Drawing.Imaging.ImageFormat.Bmp);

                    item.Item1.Dispose();
                }
            }
        }

        /// <summary>
        /// 实体对象的集合
        /// </summary>
        public static BlockingCollection<BaseMatter> BaseMatters { get; set; } = new BlockingCollection<BaseMatter>();

        /// <summary>
        /// 定位失败的对象
        /// </summary>
        public static BlockingCollection<BaseMatter> VisionFailBaseMatters { get; set; } = new BlockingCollection<BaseMatter>();

        /// <summary>
        /// 实体处理
        /// </summary>
        /// <param name="baseMatter">实体对象</param>
        public static void AddMatterVision(BaseMatter baseMatter)
        {
            BaseMatters.Add(baseMatter);
        }
    }
}
