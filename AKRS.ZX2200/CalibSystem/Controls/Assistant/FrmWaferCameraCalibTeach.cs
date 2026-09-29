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
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AKRS.ZX2200.CalibSystem.Controls.Assistant
{
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
    using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Models.Services;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Modules;
    using HalconDotNet;
    using IMVSIntensityMeasureModuCs;
    using Newtonsoft.Json;
    using OfficeOpenXml;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using VM.PlatformSDKCS;

    /// <summary>
    /// Bond标定示教窗体
    /// </summary>
    public partial class FrmWaferCameraCalibTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 晶圆台模组控制器
        /// </summary>
        private WaferTableController waferTableController = new WaferTableController();

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 运行参数
        /// </summary>
        private LightCalibrationPara lightCalibrationPara => LightCalibrationPara.GetInstance();

        /// <summary>
        /// 标定控制器
        /// </summary>
        private CalibController calibController = new CalibController();

        /// <summary>
        /// 当前顶针槽位
        /// </summary>
        private EjectionBankSlotConfig currentBankSlotConfig;

        /// <summary>
        /// 顶针模组
        /// </summary>
        [JsonIgnore]
        private EjectModule EjectModule => WaferSubModule.GetInstance().Eject;

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
        private int stepCount = 1;

        /// <summary>
        /// 晶圆相机Z
        /// </summary>
        private double waferCamerZPos = 0;

        /// <summary>
        /// 顶针Z
        /// </summary>
        private double ejectorZPos = 0;

        /// <summary>
        /// Bond相机位置
        /// </summary>
        private AKRSPoint3D bondCameraPos;

        /// <summary>
        /// 晶圆相机顶针标定模板
        /// </summary>
        private string waferAndEjectorPatternName = "晶圆相机顶针标定模板";

        /// <summary>
        /// 标定点数（建议15-20组）
        /// </summary>
        private const int CALIB_POINT_COUNT = 5;

        /// <summary>
        /// 晶圆相机基准点
        /// </summary>
        private MatchResult waferCamerastartPos;

        /// <summary>
        /// Bond相机基准点
        /// </summary>
        private MatchResult bondCamerastartPos;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmBondCalibTeach"/> class.
        /// </summary>
        public FrmWaferCameraCalibTeach()
        {
            this.InitializeComponent();
            this.TileBarTeach.SelectedItem = this.TbiWaferCamera;
            this.InitControl();
            this.SetUiControl(this.stepIndex);
            this.calibController.MoveWaferCameraZ(CalibrateRunPara.GetInstance().WaferTableLeftMarkWCVisionMachinePos.Z);
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
                        descritpion: $"1/{stepCount} :请将晶圆相机置于顶针台上方并对焦。\r\n",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                           this.waferCamerZPos = this.calibController.GetWaferTableRealPos().Z;
                           this.ejectorZPos = this.EjectModule.EjectionTableAxisZ.GetCmdPosition();

                           this.TileBarTeach.SelectedItem = this.TbiBondCamera;
                        },
                       doneAction: () =>
                       {
                       }),
                     // 相机BMC中心
                    new AssistantConfig(
                        index: 1,
                        descritpion: $"1/{stepCount} :请将Bond相机置于顶针台上方并对焦。\r\n",
                        isShowTitle: true,
                        isShowBack: false,
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
                           this.bondCameraPos= this.bondModuleController.Get3DRealPosition();

                           this.bondCamerastartPos = this.LocatePosition(CalibrateRunPara.GetInstance().GlassPRName);

                           this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.GetInstance().BMCVisionMachinePos);

                           double zStep = 1; // Z轴步长
                           // 采集标定数据（同步运动+拍摄+计算偏移，逻辑不变）
                                                     
                           List<(double ZCamBond, double DxBond, double DyBond)> calibDataBond = new List<(double, double, double)>();
                           List<(double ZCamWafer, double DxWafer, double DyWafer)> calibDataWafer = new List<(double, double, double)>();

                           this.waferCamerastartPos = this.LocatePosition(this.waferAndEjectorPatternName);
                           OffsetFitter.GetInstance().calibData.Clear();

                           for (int i = 0; i < 10; i++)
                           {
                               // 同步移动双Z轴到目标位置
                               WaferSubController.GetInstance().WaferTableController.MoveEjectTableAndWaferCameraSync(zStep);

                               double zCamResp = this.calibController.GetWaferTableRealPos().Z;

                               // 计算当前位置的定位偏移（dx=实际X-理想X0，dy=实际Y-理想Y0）（晶圆视觉）
                               var (dx, dy) = this.CalculateOffset();

                               calibDataWafer.Add((zCamResp, dx, dy));

                               this.bondModuleController.MoveSafeBondXYZ(new AKRSPoint3D(this.bondCameraPos.X, this.bondCameraPos.Y, this.bondCameraPos.Z + zStep));

                               this.bondCameraPos.Z += zStep;

                               // 计算当前位置的定位偏移（dx=实际X-理想X0，dy=实际Y-理想Y0）（晶圆视觉）
                               var (dxBond, dyBond) = this.CalculateOffsetBond();

                               calibDataBond.Add((this.bondCameraPos.Z, dxBond, dyBond));

                               this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.GetInstance().BMCVisionMachinePos);

                               (double offsetX,double offsetY) = (dx - dxBond, dy - dyBond);


                               // 添加到标定数据列表（过滤异常值：偏移绝对值>0.1mm视为无效）
                               if (Math.Abs(offsetX) < 100 && Math.Abs(offsetY) < 100)
                               {
                                   OffsetFitter.GetInstance().calibData.Add((zCamResp, offsetX, offsetY));
                                   Console.WriteLine($"采集成功：ZCam={zCamResp:F3}mm，dx={offsetX:F6}mm，dy={offsetY:F6}mm");
                               }
                               else
                               {
                                   Console.WriteLine($"偏移异常（dx={offsetX:F6}mm/dy={offsetY:F6}mm），跳过该组");
                               }

                               Thread.Sleep(100);
                           }

                           // 检查有效标定点数量（至少2组）
                           if (OffsetFitter.GetInstance().calibData.Count < 2)
                               throw new Exception($"有效标定点仅{OffsetFitter.GetInstance().calibData.Count}组，不足2组，无法拟合");

                           // 3. 基于最小二乘法手动拟合线性模型（核心替换点）

                           OffsetFitter.GetInstance().Fit(OffsetFitter.GetInstance().calibData);

                           OffsetFitter.GetInstance().Save();

                           this.TileBarTeach.SelectedItem = this.TbiWaferCamera;
                       }),
                };

            UcGuideMove ucGuideMove = new UcGuideMove("晶圆相机偏移标定");
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
                case 0:
                    prName = this.waferAndEjectorPatternName;
                    break;
                case 1:
                    prName = "Bond相机顶针标定模板";
                    break;
            }

            this.EditPr(prName);
        }

        /// <summary>
        /// 计算当前定位偏移（实际坐标 - 理想坐标）
        /// </summary>
        public (double Dx, double Dy) CalculateOffset()
        {
            MatchResult result = this.LocatePosition(this.waferAndEjectorPatternName);
            double dx = (result.CenterX - this.waferCamerastartPos.CenterX) * 3.45;
            double dy = (result.CenterY - this.waferCamerastartPos.CenterY) * 3.45;
            Console.WriteLine($"当前定位偏移：Dx={dx:0.000}mm，Dy={dy:0.000}mm");
            return (dx, dy);
        }

        /// <summary>
        /// 计算Bond相机当前定位偏移（实际坐标 - 理想坐标）
        /// </summary>
        public (double Dx, double Dy) CalculateOffsetBond()
        {
            MatchResult result = this.LocatePosition(CalibrateRunPara.GetInstance().GlassPRName);
            double dx = (result.CenterX - this.bondCamerastartPos.CenterX) * 1.73;
            double dy = (result.CenterY - this.bondCamerastartPos.CenterY) * 1.73;
            Console.WriteLine($"当前定位偏移：Dx={dx:0.000}mm，Dy={dy:0.000}mm");
            return (dx, dy);
        }


        public void saveToExcel(List<(double, double, double)> calibList)
        {

            //ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\ExperimentSystem\RepeatPositionAccuracyTest" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx"));
            //ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

            //worksheet.Cells[1, 1].Value = "次数";
            //worksheet.Cells[1, 2].Value = "速度";
            //worksheet.Cells[1, 3].Value = "加速度";

            //for (int row = 0; row < resultA1List.Count; row++)
            //{
            //    Result ret = resultA1List[row];
            //    worksheet.Cells[row + 2, 1].Value = row + 1;
            //    worksheet.Cells[row + 2, 2].Value = ret.Speed;
            //    worksheet.Cells[row + 2, 3].Value = ret.AccSpeed;
            //}

            //// 保存Excel文件
            //package.Save();
        }

        /// <summary>
        /// 模板定位
        /// </summary>
        /// <param name="patternName">模板名称</param>
        /// <returns>定位结果</returns>
        public MatchResult LocatePosition(string patternName)
        {
            // 获取Pr实体
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(patternName);

            Thread.Sleep(2000);
            ExcuteResult excuteResult = pREntity.DoWork();

            // 拍照失败，直接返回错误
            if (excuteResult != ExcuteResult.Success)
            {
                throw new ArgumentNullException(patternName, "The" + patternName + " excuteResult is fail.");
            }

            // 获取定位结果
            MatchResult matchResult = (MatchResult)pREntity.AlgResult;

            return matchResult;
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

        /// <summary>
        /// 顶针上下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEjectorUpDown_Click(object sender, EventArgs e)
        {

        }
    }
}