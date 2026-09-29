using System.Collections.Generic;

namespace AKRS.ZX2200.DispenseSystem.Controllers
{
    using AKRS.Galaxy2.AutoFocusing;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
    using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.DispenseSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Services;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 点胶视觉控制器
    /// </summary>
    public class DispenseVisionController
    {
        /// <summary>
        /// 点胶视觉模组
        /// </summary>
        private readonly VisionModule visionModule = new VisionModule();

        /// <summary>
        /// 点胶控制器
        /// </summary>
        private DispenseController dispenseController => System1Domain.GetInstance().DispenseController;

        /// <summary>
        /// 点胶设备参数
        /// </summary>
        private DispenseDevicePara DevicePara => DispenseDevicePara.GetInstance();

        /// <summary>
        /// 获取相机硬件
        /// </summary>
        /// <returns>硬件集合</returns>
        public PRHardware GetHardware()
        {
            List<string> list = new List<string>
                                    {
                                        this.visionModule.SpotLightRed.HardwareName,
                                        this.visionModule.SpotLightGreed.HardwareName,
                                        this.visionModule.SpotLightBlue.HardwareName,
                                        this.visionModule.RingLightRed.HardwareName,
                                        this.visionModule.RingLightGreed.HardwareName,
                                        this.visionModule.RingLightBlue.HardwareName
                                    };

            PRHardware hardware = new PRHardware(
                this.visionModule.DispenseCamera.HardwareName,
                this.dispenseController.DispenseModule.GetDispenseXAxis().HardwareName,
                this.dispenseController.DispenseModule.GetDispenseYAxis().HardwareName,
                this.dispenseController.DispenseModule.GetDispenseZAxis().HardwareName,
                list);

            return hardware;
        }

       
        /// <summary>
        /// 设置PR参数
        /// </summary>
        /// <param name="pREntity">模板</param>
       
        public void SetDefaultHardwareParameter(PREntity pREntity)
        {
            pREntity.SetHardware(this.GetHardware());
            pREntity.Exposure = pREntity.Camera.GetExposureTime();
            pREntity.Gain = Math.Round(pREntity.Camera.GetGain());
            pREntity.Gamma =/* pREntity.Camera.GetGamma()*/ 40;
            foreach (var item in pREntity.PRLightList)
            {
                item.IsUse = true;
                item.LightIntensity = item.Light.GetIntensity();
            }
        }

        /// <summary>
        /// 初始化光源
        /// </summary>
        /// <param name="light1">光源控件1</param>
        /// <param name="light2">光源控件2</param>
        public void InitLight(UcLight light1, UcLight light2)
        {
            // 点胶光源配置
            light1.Init(
                "点胶点光",
                this.visionModule.SpotLightRed.HardwareName,
                this.visionModule.SpotLightGreed.HardwareName,
                this.visionModule.SpotLightBlue.HardwareName);

            light2.Init(
                "点胶环光",
                this.visionModule.RingLightRed.HardwareName,
                this.visionModule.RingLightGreed.HardwareName,
                this.visionModule.RingLightBlue.HardwareName);
        }

        /// <summary>
        /// 点胶定位
        /// </summary>
        /// <param name="visionPosition">定位的点位</param>
        /// <param name="pRName">pr的名称</param>
        /// <returns>结果</returns>
        public BaseAlgResult DispenseVision(AKRSPoint3D visionPosition, string pRName, bool autofocus = false)
        {
            if (visionPosition != null)
            {
                DispenseRunTimeProvider.RecordTime($"系统1-{pRName}定位", $"准备运动到拍照位并设置光源");

                // 异步设置光源
                Task task = new Task(
                    () =>
                        {
                            // 寻找Pr模板
                            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(pRName);
                            // PREntity pREntity = DispenseRunTimeProvider.GetDispensePREntity(pREntityTemp);
                            if (pREntity != null)
                            {
                                //  List<Light> lights = pREntity.PRLightList.Where(a => a.IsUse).Select(a => a.Light).ToList();
                                List<Light> lights = this.GetLights();
                                List<int> intensities = pREntity.PRLightList.Where(a => a.IsUse).Select(a => a.LightIntensity).ToList();
                                List<int> bondIntensities = LightCalibrationPara.GetInstance().ApplyLightMapping(lights, intensities);
                                pREntity.SetLight(this.GetHardware(), lights, bondIntensities);
                            }
                        });

                task.Start();

                // 点胶轴移动到拍照位
                this.dispenseController.MoveToG0Pos3D(visionPosition);

                DispenseRunTimeProvider.RecordTime($"系统1-{pRName}定位", $"准备运动到拍照位结束");

                task.Wait();

                DispenseRunTimeProvider.RecordTime($"系统1-{pRName}定位", $"等待设置光源完成");
            }

            if (autofocus)
            {
                // 自动聚焦
                this.AutoFocus();
                visionPosition = this.dispenseController.GetG0Pos();
            }

            // 如果是空跑模式，直接返回定位结果
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DryCycle)
            {
                return new MatchResult();
            }

            // 拍照停留
            DelayHelper.Delay(this.DevicePara.DispenseModulePara.VisionDelay);

            DispenseRunTimeProvider.RecordTime($"系统1-{pRName}定位", $"定位延时：{this.DevicePara.DispenseModulePara.VisionDelay}ms");

            string bitmapName = System1Domain.GetInstance().ActionNodeController.CurrentMatter();

            // 执行定位
            BaseAlgResult baseAlg = VisionService.Vision(pRName, this.GetHardware(), "Dispense", bitmapName, false);

            bool isSuccess = baseAlg != null;

            DispenseRunTimeProvider.RecordTime($"系统1-{pRName}定位", $"定位完成，isSuccess：{isSuccess}");

            return baseAlg;
        }


        /// <summary>
        /// 点胶定位
        /// </summary>
        /// <param name="visionPosition">定位的点位</param>
        /// <param name="pRName">pr的名称</param>
        /// <returns>结果</returns>
        public List<BaseAlgResult> DispenseVisionDefect(AKRSPoint3D visionPosition, string pRName)
        {
            if (visionPosition != null)
            {
                DispenseRunTimeProvider.RecordTime("系统1胶量检测", "开始设置灯光");

                // 异步设置光源
                Task task = new Task(
                    () =>
                    {
                        // 寻找Pr模板
                        PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(pRName);
                        if (pREntity != null)
                        {
                            List<Light> lights = this.GetLights();
                            List<int> intensities = pREntity.PRLightList.Where(a => a.IsUse).Select(a => a.LightIntensity).ToList();
                            List<int> bondIntensities = LightCalibrationPara.GetInstance().ApplyLightMapping(lights, intensities);
                            pREntity.SetLight(this.GetHardware(), lights, bondIntensities);
                        }
                    });

                task.Start();

                // 点胶轴移动到拍照位
                this.dispenseController.MoveToG0Pos3D(visionPosition);

                DispenseRunTimeProvider.RecordTime("系统1胶量检测", "运动到拍照位完成");

                task.Wait();

                DispenseRunTimeProvider.RecordTime("系统1胶量检测", "设置灯光结束");
            }

            // 如果是空跑模式，直接返回定位结果
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DryCycle)
            {
                return new List<BaseAlgResult>();
            }

            // 拍照停留
            DelayHelper.Delay(this.DevicePara.DispenseModulePara.VisionDelay);

            DispenseRunTimeProvider.RecordTime("系统1胶量检测", $"定位延时{this.DevicePara.DispenseModulePara.VisionDelay}");

            string bitmapName = System1Domain.GetInstance().ActionNodeController.CurrentMatter();

            // 执行定位
            List<BaseAlgResult> baseAlgs = VisionService.VisionDefect(pRName, this.GetHardware(), "Dispense", bitmapName);

            DispenseRunTimeProvider.RecordTime("系统1胶量检测", "$定位结束");
            return baseAlgs;
        }

        /// <summary>
        /// 获取灯光集合
        /// </summary>
        /// <returns>结果</returns>
        public List<Light> GetLights()
        {
            return new List<Light>()
                       {
                           this.visionModule.SpotLightRed,
                           this.visionModule.SpotLightGreed,
                           this.visionModule.SpotLightBlue,
                           this.visionModule.RingLightRed,
                           this.visionModule.RingLightGreed,
                           this.visionModule.RingLightBlue
                       };
        }

        /// <summary>
        /// 自动对焦
        /// </summary>
        /// <param name="cameraTypeEnum">相机类型</param>
        public void AutoFocus()
        {
            // 正负限位
            double pos = this.dispenseController.DispenseModule.GetDispenseZAxis().GetRealPosition();

            double pLimit = pos + 3;
            double nLimit = pos - 3;

            AutoFocusing autofocus = new AutoFocusing();
            AKRSCamera camera = this.visionModule.DispenseCamera;

            autofocus.AutoFocus(camera, this.dispenseController.DispenseModule.GetDispenseZAxis(), pLimit, nLimit);
        }
    }
}
