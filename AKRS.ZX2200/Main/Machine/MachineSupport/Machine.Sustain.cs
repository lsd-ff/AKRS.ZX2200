using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using System.Collections.Generic;

namespace AKRS.ZX2200.Main.Machine.MachineSupport
{
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.LogicHardware.Hardwares.LaserInterferometerEncoderControllers;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Services;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using DevExpress.XtraEditors;
    using log4net.Core;
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using VM.Core;
    using ZedGraph;

    /// <summary>
    /// 设备静态状态类
    /// </summary>
    public partial class Machine
    {
        /// <summary>
        /// Pr模板是否初始化成功
        /// </summary>
        public bool PrInitSuccess { get; set; } = false;

        /// <summary>
        /// 所有轴是否复位完成
        /// </summary>
        public bool AllAxisReset { get; set; } = false;

        /// <summary>
        /// 安全门是否被关闭
        /// </summary>
        public bool IsSafeDoorClose => this.IsSafeDoorClosed();

        /// <summary>
        /// 预热完成
        /// </summary>
        [JsonIgnore]
        public bool PreheatingComplete { get; set; } = false;

        /// <summary>
        /// 加热完成
        /// </summary>
        [JsonIgnore]
        public bool HeatingComplete { get; set; } = false;

        /// <summary>
        /// 加热完成时间
        /// </summary>
        public DateTime HeatingCompleteTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 设备传感器
        /// </summary>
        // private List<Sensor> mahcineSensors;

        /// <summary>
        /// 安全门是否关闭
        /// </summary>
        /// <returns>结果</returns>
        private bool IsSafeDoorClosed()
        {
            // 寻找安全门对象
            Sensor sensor = HardwareRepositoryService.GetHardware<Sensor>("门控");
            return sensor.GetInputValue();

            //return true;
        }

        /// <summary>
        /// 检查设备状态
        /// </summary>
        /// <param name="sensor">设备传感器</param>
        //public void CheckMachineSensor(Sensor sensor)
        //{
        //    if (sensor == null)
        //    {
        //        return;
        //    }

        //    RetryCommand:
        //    if (!sensor.GetInputValue())
        //    {
        //        MachineStateModel.GetInstance().MachineState = MachineStateEnum.Stop;
        //        DialogResult dialogResult = AKRSMessageBoxExt.Show(
        //            $"${sensor.HardwareName}报警，请检查气路以及传感器状态",
        //            "报警",
        //            new[] { "重试", "退出" },
        //            new[] { DialogResult.Retry, DialogResult.Abort });

        //        if (dialogResult == DialogResult.Retry)
        //        {
        //            goto RetryCommand;
        //        }
        //    }
        //}

        /// <summary>
        /// 初始化PR
        /// </summary>
        /// <returns>结果</returns>
        public Task InitPR() 
        {
            // 在窗体加载的时候先初始化PR模板
           Task t =  Task.Run(
                () =>
                {
                    try
                    {
                        LogHelper.Post(Level.Info, $"准备加载PR", LogCategory.PR, ViewType.InFileAndUI);

                        // 清理旧PR方案
                        VmSolution.Instance.CloseSolution();

                        LogHelper.Post(Level.Info, $" 清理旧PR方案完成", LogCategory.PR, ViewType.InFileAndUI);

                        // 创建新方案
                        VmSolution.CreatSolInstance();

                        LogHelper.Post(Level.Info, $"创建新方案完成", LogCategory.PR, ViewType.InFileAndUI);

                        List<string> lists = System2CommonService.GetCurrentRecipePREntityNameList();


                        if (VisionEntityRepository.GetInstance().PRVisionList.Count == 0)
                        {
                            if (File.Exists(PathConfig.DeviceDirPath + "\\备份TemplateConfigRepository.json"))
                            {
                                File.Delete(PathConfig.TemplateConfigRepositoryPath);
                                File.Copy(PathConfig.DeviceDirPath + "\\备份TemplateConfigRepository.json", PathConfig.TemplateConfigRepositoryPath);
                            }
                        }
                        foreach (string prName in lists)
                        {
                            if (string.IsNullOrEmpty(prName))
                            {
                                continue;
                            }

                            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(prName);
                            if (prEntity != null)
                            {
                                prEntity.Alg?.InitVmProcedure();
                                LogHelper.Post(Level.Info, $"{prEntity?.GetName()}:视觉方案加载成功", LogCategory.PR, ViewType.InFileAndUI);
                            }
                            else
                            {
                                LogHelper.Post(Level.Info, $"视觉方案加载失败，未找到：{prName}", LogCategory.PR, ViewType.InFileAndUI);
                            }
                        }

                        DispenseRunTimeProvider.LoadDispensePr();

                        Machine.GetInstance().PrInitSuccess = true;
                    }
                    catch (Exception e)
                    {
                        string message = "设备初始化视觉模板失败，请检查加密狗或联系设备人员!";
                        AKRSXtraMessageBox.Show(message);
                        LogHelper.Post(Level.Error, message+"异常信息：" +e.Message, LogCategory.PR,ViewType.InFileAndUI);
                        throw;
                    }
                });

            return t;
        }

        /// <summary>
        /// 检查C盘容量
        /// </summary>
        /// <returns>结果</returns>
        public bool IsDiskCapacitySufficient()
        {
            DriveInfo driveInfoC = new DriveInfo("C");

            double totalFreeSpace = driveInfoC.TotalFreeSpace / 1024.0 / 1024.0 / 1024.0;

            if (totalFreeSpace < 10)
            {
                AKRSXtraMessageBox.Show($"C盘内存不足10G，当前为{totalFreeSpace},请清理后重试");
                return false;
            }

            DriveInfo driveInfoD = new DriveInfo("D");

            totalFreeSpace = driveInfoD.TotalFreeSpace / 1024.0 / 1024.0 / 1024.0;

            if (totalFreeSpace < 200)
            {
                AKRSXtraMessageBox.Show($"D盘内存不足200G，当前为{totalFreeSpace},请清理后重试");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 定期删除
        /// </summary>
        /// <param name="day">日期</param>
        public void CleanFile(int day)
        {
            try
            {
                DevExpress.Utils.WaitDialogForm waitBeforeLogin = new DevExpress.Utils.WaitDialogForm("请稍后", $"正在删除{day}天前的图片等数据");

                #region 删除文件

                if (!Directory.Exists("D:\\ZX2200存储数据\\"))
                {
                    // 创建文件夹
                    Directory.CreateDirectory("D:\\ZX2200存储数据\\");
                }

                DirectoryInfo dirs = new DirectoryInfo("D:\\ZX2200存储数据\\");

                foreach (DirectoryInfo dir in dirs.GetDirectories())
                {
                    FileSystemInfo[] infos = dir.GetFileSystemInfos();
                    foreach (FileSystemInfo file in infos)
                    {
                        if (DateTime.TryParseExact(
                                file.ToString(),
                                "yyyy-MM-dd",
                                System.Globalization.CultureInfo.CurrentCulture,
                                DateTimeStyles.None,
                                out DateTime dt))
                        {
                            if (dt < DateTime.Now.AddDays(-day))
                            {
                                string newPath = Path.Combine(dir.FullName, file.ToString());
                                Directory.Delete(newPath, true);
                            }
                        }
                    }
                }

                #endregion

                waitBeforeLogin.Close();
            }
            catch (Exception e)
            {
                throw new Exception("定期删除图片失败，请检查是否文件是否已经打开 " + e.Message);
            }
        }

        /// <summary>
        /// 获取设备标准存储位置地址，设备会滚动删除
        /// </summary>
        /// <param name="directoryName">文件夹名称</param>
        /// <returns>地址</returns>
        public string GetMachineDatePath(string directoryName)
        {
            string path = Path.Combine("D:\\ZX2200存储数据\\", directoryName, DateTime.Now.ToString("yyyy-MM-dd"));
            return path;
        }

        /// <summary>
        /// 开启安全门任务
        /// </summary>
        public void StartCheckSafeDoorTask()
        {
            if (!MachineHardwareConfiguration.GetInstance().IsSafeDoorConfigured)
            {
                return;
            }

            if (MachineHardwareConfiguration.GetInstance().IsSafeDoorConfigured
                                && Machine.GetInstance().IsWorking()
                                && !Machine.GetInstance().IsSafeDoorClose)
            {
                Machine.GetInstance().Pause();
            }
            //else if (MachineHardwareConfiguration.GetInstance().IsSafeDoorConfigured
            //         && Machine.GetInstance().IsPause()
            //         && Machine.GetInstance().IsSafeDoorClose)
            //{
            //    DialogResult dialog = AKRSXtraMessageBox.Show("是否继续工作", "提示", MessageBoxButtons.YesNo);
            //    if (dialog == DialogResult.Yes
            //        && Machine.GetInstance().IsPause()
            //        && Machine.GetInstance().IsSafeDoorClose)
            //    {
            //        Machine.GetInstance().SetState(MachineStateEnum.Working);
            //    }
            //    else
            //    {
            //        Machine.GetInstance().Stop();
            //    }
            //}
        }

        /// <summary>
        /// 开启激光干涉尺任务
        /// </summary>
        public void StartLaserEncoderCheckTask()
        {
            bool ret = this.LaserEncoderCheck();

           if (ret == false)
           {
               Machine.GetInstance().Stop();
            }

            Task.Delay(100);
        }

        /// <summary>
        /// 激光干涉尺检查
        /// </summary>
        /// <returns>true :正常 false:异常</returns>
        public bool LaserEncoderCheck()
        {
            LaserEncoder laserEncoder = HardwareRepositoryService.GetHardware<LaserEncoder>("激光干涉尺");

            // 如果不配置，直接返回
            if (laserEncoder == null || MachineHardwareConfiguration.GetInstance().IsLaserEncoderConfigured == false) 
            {
                return true;
            }

            try
            {
                bool status;

                //bool status = laserEncoder.GetWarmupStatus();

                //if (status == false)
                //{
                //    DialogResult dialog = AKRSMessageBoxExt.Show(
                //        $"激光干涉尺未预热完成，不允许工作！",
                //        "报警",
                //        new string[] { "确认", "退出软件" },
                //        new DialogResult[] { DialogResult.OK, DialogResult.Abort });

                //    if (dialog == DialogResult.Abort)
                //    {
                //        this.ExitSoftware();
                //    }

                //    return false;
                //}

                status = laserEncoder.GetEnvironmentSensorStatus();

                if (status == false)
                {
                    DialogResult dialog = AKRSMessageBoxExt.Show(
                        $"激光干涉尺未环境传感器未连接，请检查环境传感器！",
                        "报警",
                        new string[] { "确认", "退出软件" },
                        new DialogResult[] { DialogResult.OK, DialogResult.Abort });

                    if (dialog == DialogResult.Abort)
                    {
                        this.ExitSoftware();
                    }

                    return false;
                }

                status = laserEncoder.GetAlgorithmStatus();

                if (status == false)
                {
                    DialogResult dialog = AKRSMessageBoxExt.Show(
                        $"激光干涉尺算法工作异常,请退出软件并断电重启！",
                        "报警",
                        new string[] { "确认", "退出软件" },
                        new DialogResult[] { DialogResult.OK, DialogResult.Abort });

                    if (dialog == DialogResult.Abort)
                    {
                        this.ExitSoftware();
                    }

                    return false;
                }

                int lightIntensityStatus = laserEncoder.GetLightIntensityStatus();

                if (lightIntensityStatus != 3)
                {
                    DialogResult dialog = AKRSMessageBoxExt.Show(
                        $"激光干涉尺光强不佳，请退出软件并断电重启！",
                        "报警",
                        new string[] { "确认", "退出软件" },
                        new DialogResult[] { DialogResult.OK, DialogResult.Abort });

                    if (dialog == DialogResult.Abort)
                    {
                        this.ExitSoftware();
                    }
                }
            }
            catch (Exception e) 
            {
                DialogResult dialog = AKRSMessageBoxExt.Show(
                       $"激光干涉尺通讯异常，{e.ToString()}！",
                       "报警",
                       new string[] { "确认", "退出软件" },
                       new DialogResult[] { DialogResult.OK, DialogResult.Abort });

                return false;
            }

            return true;
        }
    }
}
