using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.PR.Controls;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.CalibSystem.Services;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.CalibSystem.Controls.Assistant
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
    using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.PR.Models.Services;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using HalconDotNet;

    using IMVSIntensityMeasureModuCs;

    using Newtonsoft.Json;

    using VM.PlatformSDKCS;

    /// <summary>
    /// Bond标定示教窗体
    /// </summary>
    public partial class FrmLightnessCalibTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 晶圆台模组控制器
        /// </summary>
        private WaferTableController waferTableController = new WaferTableController();

        /// <summary>
        /// 运行参数
        /// </summary>
        private LightCalibrationPara lightCalibrationPara => LightCalibrationPara.GetInstance();

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// 点胶控制器
        /// </summary>
        private DispenseController dispenseController = new DispenseController();

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// 标定控制器
        /// </summary>
        private CalibController calibController = new CalibController();

        /// <summary>
        /// 界面配置集
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 2;

        /// <summary>
        /// 点胶相机亮度校正位
        /// </summary>
        private AKRSPoint3D dispenseCameraVisionPos;

        /// <summary>
        /// Bond相机亮度校正位
        /// </summary>
        private AKRSPoint3D bondCameraVisionPos;

        #region 光源硬件
        /// <summary>
        /// Bond相机
        /// </summary>
        public AKRSCamera BondCamera => HardwareRepositoryService.GetHardware<AKRSCamera>("BOND相机");

        /// <summary>
        /// Dispense相机
        /// </summary>
        public AKRSCamera DispenseCamera => HardwareRepositoryService.GetHardware<AKRSCamera>("点胶相机");

        /// <summary>
        /// Bond相机环光
        /// </summary>
        public Light BondRingLightRed => HardwareRepositoryService.GetHardware<Light>("邦头三色环光-红");

        /// <summary>
        /// Bond相机环光
        /// </summary>
        public Light BondRingLightGreen => HardwareRepositoryService.GetHardware<Light>("邦头三色环光-绿");

        /// <summary>
        /// Bond相机环光
        /// </summary>
        public Light BondRingLightBlue => HardwareRepositoryService.GetHardware<Light>("邦头三色环光-蓝");

        /// <summary>
        /// Bond相机点光
        /// </summary>
        public Light BondSpotLightRed => HardwareRepositoryService.GetHardware<Light>("邦头三色点光-红");

        /// <summary>
        /// Bond相机点光
        /// </summary>
        public Light BondSpotLightGreen => HardwareRepositoryService.GetHardware<Light>("邦头三色点光-绿");

        /// <summary>
        /// Bond相机点光
        /// </summary>
        public Light BondSpotLightBlue => HardwareRepositoryService.GetHardware<Light>("邦头三色点光-蓝");

        /// <summary>
        /// 点光红光
        /// </summary>
        public Light DispenseSpotLightRed => HardwareRepositoryService.GetHardware<Light>("点胶三色点光-红");

        /// <summary>
        /// 点光绿光
        /// </summary>
        public Light DispenseSpotLightGreen => HardwareRepositoryService.GetHardware<Light>("点胶三色点光-绿");

        /// <summary>
        /// 点光蓝光
        /// </summary>
        public Light DispenseSpotLightBlue => HardwareRepositoryService.GetHardware<Light>("点胶三色点光-蓝");

        /// <summary>
        /// 环光红光
        /// </summary>
        public Light DispenseRingLightRed => HardwareRepositoryService.GetHardware<Light>("点胶三色环光-红");

        /// <summary>
        /// 环光绿光
        /// </summary>
        public Light DispenseRingLightGreen => HardwareRepositoryService.GetHardware<Light>("点胶三色环光-绿");

        /// <summary>
        /// 环光蓝光
        /// </summary>
        public Light DispenseRingLightBlue => HardwareRepositoryService.GetHardware<Light>("点胶三色环光-蓝");

        #endregion

        #region 点胶光源亮度列表
        /// <summary>
        /// 点胶光亮度列表
        /// </summary>
        public List<double> DispenseLightnessList = new List<double>();

        #endregion

        #region Bond光源亮度列表
        /// <summary>
        /// Bond光亮度列表
        /// </summary>
        public List<double> BondLightnessList = new List<double>();

        #endregion

        private int LightControlStep = 20;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmBondCalibTeach"/> class.
        /// </summary>
        public FrmLightnessCalibTeach()
        {
            this.InitializeComponent();
            this.TileBarTeach.SelectedItem = this.TbiDispenseCamera;
            this.InitControl();
            this.SetUiControl(this.stepIndex);
        }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnNext_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
             assistantConfig.NextAction();
            this.stepIndex++;
            this.SetUiControl(this.stepIndex);
        }

        /// <summary>
        /// 上一步
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnBack_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.BackAction();
            this.stepIndex--;
            this.SetUiControl(this.stepIndex);
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        public void InitControl()
        {
            this.stepCount = 2;
            this.BtnDone.Visible = false;

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 相机BMC中心
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"1/{stepCount} :请将点胶相机置于白纸上方并对焦。\r\n",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            this.dispenseCameraVisionPos = this.dispenseController.GetAxisPos();

                            this.TileBarTeach.SelectedItem = this.TbiBondCamera;
                        },
                       doneAction: () =>
                       {
                       }),
                 
                    // 吸嘴BMC中心
                    new AssistantConfig(
                        index: 1,
                        descritpion: $"2/{stepCount}：请将Bond相机置于白纸上方并对焦。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,  
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                        },
                        doneAction: () =>
                        {
                            this.bondCameraVisionPos = this.bondModuleController.Get3DRealPosition();

                            Task.Run(this.LightnessCalibration);

                        }),
                };

            UcGuideMove ucGuideMove = new UcGuideMove("系统1系统2灯光标定");
            ucGuideMove.Dock = DockStyle.Fill;
            this.panelControl4.Controls.Add(ucGuideMove);
            this.LbDescription.Text = this.assistantConfigList[0].Descritpion;
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        private void SetUiControl(int stepIndex)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[stepIndex];

            this.LbDescription.Text = assistantConfig.Descritpion;
            this.BtnBack.Visible = assistantConfig.IsShowBack;
            this.BtnNext.Visible = assistantConfig.IsShowNext;      
            this.BtnDone.Visible = assistantConfig.IsShowDone;
        }

        /// <summary>
        /// 模版匹配界面
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnPattern_Click(object sender, EventArgs e)
        {
            string prName = string.Empty;
            switch (this.stepIndex)
            {
                // case 0:
                //    prName = this.CalibrateRunPara.BmcPRName;
                //    break;
                // case 2:
                //    prName = this.CalibrateRunPara.GlassPRName;
                //    break;
            }

            this.EditPr(prName);
        }

        /// <summary>
        /// 编辑模板
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="algBeLong">模板类型</param>
        public void EditPr(string name, AlgBeLongEnum algBeLong = AlgBeLongEnum.Calibration)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);
            BaseVisionEntity visionEntity;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity(name);
                prEntity.Alg.AlgBeLong = algBeLong;
                visionEntity = prEntity;
                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            }

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);
            editor.ShowDialog();
            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 光源亮度标定
        /// </summary>
        public void LightnessCalibration()
        {
            this.calibController.MoveDispenseToMachinePos(this.dispenseCameraVisionPos);
            this.bondModuleController.MoveSafeBondXYZ(this.bondCameraVisionPos);

            this.DispenseCamera.SetExposureTime(4000);
            this.BondCamera.SetExposureTime(4000);
            this.DispenseCamera.SetGamma(1);
            this.BondCamera.SetGamma(1);
            this.DispenseCamera.SetGain(0);
            this.BondCamera.SetGain(0);

            Light[] lights = { this.BondRingLightRed, this.BondRingLightBlue, this.BondRingLightGreen, this.BondSpotLightRed, this.BondSpotLightBlue, this.BondSpotLightGreen, this.DispenseRingLightRed, this.DispenseRingLightBlue, this.DispenseRingLightGreen, this.DispenseSpotLightRed, this.DispenseSpotLightBlue, this.DispenseSpotLightGreen };
            int[] indentities = new int[12];
            LightController.SetIntensities(lights.ToList(), indentities.ToList());
            foreach (var light in lights)
            {
                light.SetIntensity(0);
            }

            // 标定每个光源对
            this.lightCalibrationPara.RedRingLightMap = this.CalibrateLightPair(
                this.BondRingLightRed,
                this.DispenseRingLightRed,
                this.LightControlStep);

            this.lightCalibrationPara.BlueRingLightMap = this.CalibrateLightPair(
                this.BondRingLightBlue,
                this.DispenseRingLightBlue,
                this.LightControlStep);

            this.lightCalibrationPara.GreenRingLightMap = this.CalibrateLightPair(
                this.BondRingLightGreen,
                this.DispenseRingLightGreen,
                this.LightControlStep);

            this.lightCalibrationPara.RedSpotLightMap = this.CalibrateLightPair(
                this.BondSpotLightRed,
                this.DispenseSpotLightRed,
                this.LightControlStep);

            this.lightCalibrationPara.BlueSpotLightMap = this.CalibrateLightPair(
                this.BondSpotLightBlue,
                this.DispenseSpotLightBlue,
                this.LightControlStep);

            this.lightCalibrationPara.GreenSpotLightMap = this.CalibrateLightPair(
                this.BondSpotLightGreen,
                this.DispenseSpotLightGreen,
                this.LightControlStep);


            this.lightCalibrationPara.Save();
        }

        /// <summary>
        /// 计算亮度比值
        /// </summary>
        /// <param name="dispenseLight">点胶光源</param>
        /// <param name="bondLight">Bond光源</param>
        /// <param name="step">光源步长</param>
        /// <returns>光源比值</returns>
        public double LightnessRatio(Light dispenseLight, Light bondLight, int step)
        {
            this.DispenseLightnessList.Clear();
            this.BondLightnessList.Clear();

            for (int i = 5; i <= 255; i = i + step)
            {
                dispenseLight.SetIntensity(i);
                Thread.Sleep(100);
                Bitmap bitmap = this.DispenseCamera.SnapImage(false, false);
                double curImageLight = this.ImageMeanGray(bitmap);
                this.DispenseLightnessList.Add(curImageLight);

                bondLight.SetIntensity(i);
                Thread.Sleep(100);
                bitmap = this.BondCamera.SnapImage(false, false);
                curImageLight = this.ImageMeanGray(bitmap);
                this.BondLightnessList.Add(curImageLight);
            }

            dispenseLight.SetIntensity(0);
            bondLight.SetIntensity(0);

   
            double ratio = this.DispenseLightnessList.Average() / this.BondLightnessList.Average();

            return ratio;
        }

        /// <summary>
        /// 标定光源对并生成亮度映射表
        /// </summary>
        /// <param name="bondLight">Bond光源</param>
        /// <param name="dispenseLight">点胶光源</param>
        /// <param name="step">光源步长</param>
        /// <returns>亮度映射表</returns>
        public List<(int bondIntensity, int dispenseIntensity)> CalibrateLightPair(
            Light bondLight,
            Light dispenseLight,
            int step)
        {
            var lightMap = new List<(int, int)>();
            var bondLightnessList = new List<double>();
            var dispenseLightnessList = new List<double>();
             
            // 定义测试点（包括边界和中间点）
            int[] testPoints = { 0, 64, 128, 192, 255 };

            foreach (int intensity in testPoints)
            {
                // 1. 设置Bond光源并测量Bond相机亮度
                bondLight.SetIntensity(intensity);
                Thread.Sleep(500);
                Bitmap bondBitmap = this.BondCamera.SnapImage(false, false);
                double bondLightness = this.ImageMeanGray(bondBitmap);
                bondLightnessList.Add(bondLightness);

                // 2. 设置点胶光源并测量点胶相机亮度
                //dispenseLight.SetIntensity(intensity);
                //Thread.Sleep(100);
                //Bitmap dispenseBitmap = this.DispenseCamera.SnapImage(false, false);
                //double dispenseLightness = this.ImageMeanGray(dispenseBitmap);
                //dispenseLightnessList.Add(dispenseLightness);

                // 3. 找到使点胶相机亮度匹配Bond相机亮度的点胶光源强度
                int bestMatch = FindMatchingIntensity(dispenseLight, bondLightness, step);
                lightMap.Add((intensity, bestMatch));
            }

            // 关闭光源
            bondLight.SetIntensity(0);
            dispenseLight.SetIntensity(0);

            return lightMap;
        }

        /// <summary>
        /// 查找匹配的亮度强度
        /// </summary>
        private int FindMatchingIntensity(Light light, double targetLightness, int step)
        {
            int low = 0;
            int high = 255;
            int bestMatch = 0;
            double minDiff = double.MaxValue;

            // 使用二分查找法找到最佳匹配
            for (int i = 0; i < 8; i++) // 8次迭代
            {
                int mid = (low + high) / 2;
                light.SetIntensity(mid);
                Thread.Sleep(500);

                Bitmap bitmap = this.DispenseCamera.SnapImage(false, false);
                //bitmap.Save("D;\\test.bmp");
                double currentLightness = this.ImageMeanGray(bitmap);

                double diff = Math.Abs(currentLightness - targetLightness);
                if (diff < minDiff)
                {
                    minDiff = diff;
                    bestMatch = mid;
                }

                if (currentLightness < targetLightness)
                {
                    low = mid;
                }
                else
                {
                    high = mid;
                }
            }

            return bestMatch;
        }


        /// <summary>
        /// 计算图像平均灰度
        /// </summary>
        /// <param name="inputImage">输入图像</param>
        /// <returns>图像灰度</returns>
        public double ImageMeanGray(Bitmap inputImage)
        {
            HTuple image_Mean, image_Deviation;
            this.Bitmap2HObjectBpp8(inputImage, out HObject image);
            HOperatorSet.GenRectangle1(out HObject rectangle, 612, 512, 1836, 1536);
            HOperatorSet.Intensity(rectangle, image, out image_Mean, out image_Deviation);

            return image_Mean.D;
        }

        /// <summary>
        /// bitmap转halcon
        /// </summary>
        /// <param name="bmp">bmp</param>
        /// <param name="image">hobject</param>
        public void Bitmap2HObjectBpp8(Bitmap bmp, out HObject image)
        {
            try
            {
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

                BitmapData srcBmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);

                HOperatorSet.GenImage1(out image, "byte", bmp.Width, bmp.Height, srcBmpData.Scan0);
                bmp.UnlockBits(srcBmpData);
            }
            catch (Exception ex)
            {
                image = null;
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void MyUserControl_CloseParentForm(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 完成事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnDone_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.DoneAction();
        }
    }
}