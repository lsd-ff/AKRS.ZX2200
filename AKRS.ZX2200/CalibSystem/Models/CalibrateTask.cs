using AKRS.Galaxy2.AxisCompensate.AxisCompensate;
using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.CalibSystem.Controls;
using AKRS.ZX2200.CalibSystem.Services;
using AKRS.ZX2200.DispenseSystem.Controllers;
using AKRS.ZX2200.DispenseSystem.Modules;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using DevExpress.XtraEditors;
using HalconDotNet;
using LanguageExt;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace AKRS.ZX2200.CalibSystem.Models
{
    using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.EventBus;
    using AKRS.ZX2200.Main.Controls;
    using DevExpress.CodeParser;
    using Newtonsoft.Json;

    /// <summary>
    /// 标定类
    /// </summary>WaferTableModule
    public class CalibrateTask : Singleton<CalibrateTask>
    {
        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// System2Controller
        /// </summary>
        private System2Controller system2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// 点胶测高控制器
        /// </summary>
        private DispenseMeasureHeightController dispenseMeasureHeightController = new DispenseMeasureHeightController();

        /// <summary>
        /// 点胶测高模组
        /// </summary>
        private readonly MeasureHeightModule measureHeightModule = new MeasureHeightModule();

        /// <summary>
        /// bond 模组
        /// </summary>
        private BondModule bondModule = new BondModule();

        /// <summary>
        /// 点胶控制器
        /// </summary>
        private DispenseController dispenseController = new DispenseController();

        /// <summary>
        /// 晶圆台控制器
        /// </summary>
        private WaferTableController waferTableController = new WaferTableController();

        /// <summary>
        /// 标定控制器
        /// </summary>
        private CalibController calibController = new CalibController();

        /// <summary>
        /// Bond相机
        /// </summary>
        [JsonIgnore]
        public AKRSCamera BondCamera => HardwareRepositoryService.GetHardware<AKRSCamera>("BOND相机");

        /// <summary>
        /// BMC设备参数
        /// </summary>
        private BMCDevicePara bMCDevicePara => BondDevicePara.GetInstance().BMCDevicePara;

        /// <summary>
        /// 运行参数
        /// </summary>
        private CalibrateRunPara calibratePara => CalibrateRunPara.GetInstance();

        /// <summary>
        /// 设备坐标系系统
        /// </summary>
        public MachineCoordinateSystem MachineCoordinateSystem => MachineCoordinateSystem.GetInstance();

        /// <summary>
        /// 点胶标定点位列表
        /// </summary>
        private List<AKRSPoint2D> dispenseCalibPoints;

        /// <summary>
        /// BMC标定点位列表
        /// </summary>
        private List<AKRSPoint2D> bmcCalibPoints;

        /// <summary>
        /// 上视标定点位列表
        /// </summary>
        private List<AKRSPoint2D> upLookCalibPoints;

        /// <summary>
        /// 晶圆台标定点位列表
        /// </summary>
        private List<AKRSPoint2D> waferTableCalibPoints;

        /// <summary>
        /// 系统一点位
        /// </summary>
        private List<AKRSPoint2D> dispenseList2Ds;

        private List<AKRSPoint3D> dispenseList3Ds;

        /// <summary>
        /// 系统二点位
        /// </summary>
        private List<AKRSPoint2D> bondList2Ds;

        private List<AKRSPoint3D> bondList3Ds;

        /// <summary>
        /// 像素比结果对象列表
        /// </summary>
        public List<CalibCamScaleResult> CalibCamScales { get; set; } = new List<CalibCamScaleResult>();

        /// <summary>
        /// 像素比对象
        /// </summary>
        private CalibCamScaleResult calibCamScale;

        /// <summary>
        /// 转换结果对象列表
        /// </summary>
        public List<CalibTransResult> CalibTransResults { get; set; } = new List<CalibTransResult>();

        /// <summary>
        /// 转换结果对象
        /// </summary>
        private CalibTransResult calibTransResult;

        /// <summary>
        /// 点胶平台自动标定流程
        /// </summary>
        /// <returns>是否成功</returns>
        public bool StartDispenseCalibTask()
        {
            try
            {
                this.calibCamScale = new CalibCamScaleResult();
                this.calibTransResult = new CalibTransResult();

                this.dispenseMeasureHeightController.CloseDispenseHeightMeasurementCylinder();

                Thread.Sleep(1000);

                // 点胶相机移动到点胶标定点中心上方
                this.calibController.MoveDispenseToMachinePos(this.calibratePara.DispenseMarkVisionMachinePos);

                // 创建点胶N点标定点位列表
                this.dispenseCalibPoints = CalibService.GeneratePointList(this.calibratePara.DispenseMarkVisionMachinePos, this.calibratePara.DispenseCalibDistance, 3, 3);

                List<AKRSPoint2D> imagePoints = new List<AKRSPoint2D>();
                int marksNum = this.dispenseCalibPoints.Count;
                float[] imageX = new float[marksNum];
                float[] imageY = new float[marksNum];
                float[] worldX = new float[marksNum];
                float[] worldY = new float[marksNum];

                List<AKRSPoint3D> imgPoints = new List<AKRSPoint3D>();
                List<AKRSPoint3D> realPoints = new List<AKRSPoint3D>();

                //// 点胶相机遍历点位开始定位并标定
                for (int i = 0; i < marksNum; i++)
                {
                    // 轴移动到位
                    this.calibController.MoveDispenseToMachinePos(new AKRSPoint3D(this.dispenseCalibPoints[i].X, this.dispenseCalibPoints[i].Y, this.calibratePara.DispenseMarkVisionMachinePos.Z));

                    Thread.Sleep(200);

                    // 获取Pr实体
                    PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find("点胶标定模板");

                    // PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.calibratePara.DispensePRName);

                    // 开始定位
                    ExcuteResult excuteResult = pREntity.DoWork();

                    // 拍照失败，直接返回错误
                    if (excuteResult != ExcuteResult.Success)
                    {
                        return false;
                    }

                    // 获取定位结果
                    MatchResult matchResult = (MatchResult)pREntity.AlgResult;

                    imagePoints.Add(new AKRSPoint2D(matchResult.CenterX, matchResult.CenterY));

                    imageX[i] = (float)matchResult.CenterX;
                    imageY[i] = (float)matchResult.CenterY;
                    worldX[i] = (float)(this.dispenseCalibPoints[i].X - this.calibratePara.DispenseMarkVisionMachinePos.X);
                    worldY[i] = (float)(this.dispenseCalibPoints[i].Y - this.calibratePara.DispenseMarkVisionMachinePos.Y);

                    imgPoints.Add(new AKRSPoint3D(matchResult.CenterX, matchResult.CenterY, 0));
                    realPoints.Add(new AKRSPoint3D(worldX[i], worldY[i], 0));
                }
                //// 坐标系转换
                MachineCoordinateSystem.GetInstance().CreateCoordinateSystem("DispenseCameraCoordinateSystem", "DispenseCoordinateSystem", false, CoordinateSystemTypeEnum.Vm);

                DependentCoordinateSystem a = (DependentCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name == "DispenseCameraCoordinateSystem");
                a.Init(realPoints, imgPoints);
                MachineCoordinateSystem.GetInstance().Save();
                MachineCoordinateSystem.Refresh();

                // 调用9点标定的接口(Halcon)（发送定位结果和相对坐标）
                List<AKRSPoint2D> physicalPoints = this.dispenseCalibPoints.Select(p => new AKRSPoint2D(p.X - this.calibratePara.DispenseMarkVisionMachinePos.X, p.Y - this.calibratePara.DispenseMarkVisionMachinePos.Y)).ToList();

                HTuple homMatTrans;
                HTuple scaleX, scaleY, phi, slant, xTrans, yTrans;

                CalibService.NPointTrans(imagePoints, physicalPoints, out homMatTrans);
                this.calibratePara.DispenseHomMatTrans = homMatTrans;
                HOperatorSet.HomMat2dToAffinePar(homMatTrans, out scaleX, out scaleY, out phi, out slant, out xTrans, out yTrans);

                // 移动探针到测高位
                this.calibController.MoveDispenseToMachinePos(this.calibratePara.DispenseMeasureHeightSearchMachinePos);

                this.dispenseMeasureHeightController.OpenDispenseHeightMeasurementCylinder();

                (ExcuteResult result, double height) = this.dispenseMeasureHeightController.HeightMeasurement(this.measureHeightModule.DispenseAltimemetrySensor);

                if (result != ExcuteResult.Success)
                {
                    return false;
                }

                this.calibratePara.DispenseMeasureRealHeightMachinePos = this.dispenseController.GetAxisPos();
                this.calibratePara.DispenseMeasureRealHeightMachinePos.Z = height;

                this.calibController.MoveDispenseZToSafePos();

                this.calibratePara.Save();

                this.dispenseMeasureHeightController.CloseDispenseHeightMeasurementCylinder();

                this.calibCamScale = CalibService.CreateCalibCamScale("点胶相机像素比", Math.Round(scaleX.D, 6), Math.Round(scaleY.D, 6), Math.Round(phi.TupleDeg().D, 6));
                this.CalibCamScales.Add(this.calibCamScale);

                this.calibTransResult = CalibService.CreateCalibTransResult("点胶相机坐标转换信息", Math.Round(xTrans.D, 6), Math.Round(yTrans.D, 6), Math.Round(slant.TupleDeg().D, 6));
                this.CalibTransResults.Add(this.calibTransResult);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.Dispense);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return false;
            }
        }

        /// <summary>
        /// Bond相机N点标定线程
        /// </summary>
        /// <returns>是否成功</returns>
        public bool StartBondCalibTask()
        {
            try
            {
                //this.bondHeadController.OpenBondHeadVaccum();

                // Bond相机移动到BMC测高位置
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCMeasureHeightSearchMachinePos);

                double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

                // 测高
                (ExcuteResult Ret, double HeightValue) res = this.bondHeadController.MeasureHeight(liftLevel, HeightMeasurementFunctionEnum.WithTDSensor);

                if (res.Ret == ExcuteResult.Success)
                {
                    
                    BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult = res.HeightValue;
                    BondDevicePara.GetInstance().Save();
                }
                else
                {
                    AKRSXtraMessageBox.Show("测高失败");
                    return false;
                }

                // Bond Z 和 相机的Z轴偏移量
                this.calibratePara.BondRotateCenterToCamOffset.Z =
                    -(this.calibratePara.BMCVisionMachinePos.Z - BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult);

                this.bondHeadController.CloseToolVaccum();

                // Bond相机移动到BMC中心点上方
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCVisionMachinePos);

                // 定位BMC 中心点
                MatchResult bigMatchResult = this.LocatePosition(this.calibratePara.BmcPRName);

                // 移动到相机中心点
                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, bigMatchResult);

                this.calibController.LocateAndMoveToTargetPixel(this.calibratePara.BmcPRName, CalibController.CamCoordinateType.Bond, 1, 2);

                this.calibratePara.BMCVisionMachinePos = this.bondModuleController.Get3DRealPosition();

                // 创建BMC标定点位列表
                this.bmcCalibPoints = CalibService.GeneratePointList(this.calibratePara.BMCVisionMachinePos, 0.1, 0.05, 6, 5);

                List<AKRSPoint2D> imagePoints = new List<AKRSPoint2D>();
                int marksNum = this.bmcCalibPoints.Count;
              
                float[] worldX = new float[marksNum];
                float[] worldY = new float[marksNum];

                List<AKRSPoint3D> imgPoints = new List<AKRSPoint3D>();
                List<AKRSPoint3D> realPoints = new List<AKRSPoint3D>();

                // Bond相机遍历点位开始定位并标定
                for (int i = 0; i < this.bmcCalibPoints.Count; i++)
                {
                    // 轴移动到位
                    this.bondModuleController.MoveSafeBondXY(this.bmcCalibPoints[i].X, this.bmcCalibPoints[i].Y);
                    this.bondHeadController.MoveAxisZ(this.calibratePara.BMCVisionMachinePos.Z);

                    Thread.Sleep(1000);

                    // 获取Pr实体
                    PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.calibratePara.BmcPRName);

                    // 开始定位
                    ExcuteResult excuteResult = pREntity.DoWork();

                    // 拍照失败，直接返回错误
                    if (excuteResult != ExcuteResult.Success)
                    {
                        return false;
                    }

                    // 获取定位结果
                    MatchResult matchResult = (MatchResult)pREntity.AlgResult;

                    imagePoints.Add(new AKRSPoint2D(matchResult.CenterX, matchResult.CenterY));

                    worldX[i] = (float)(this.bmcCalibPoints[i].X - this.calibratePara.BMCVisionMachinePos.X);
                    worldY[i] = (float)(this.bmcCalibPoints[i].Y - this.calibratePara.BMCVisionMachinePos.Y);

                    imgPoints.Add(new AKRSPoint3D(matchResult.CenterX, matchResult.CenterY, 0));
                    realPoints.Add(new AKRSPoint3D(worldX[i], worldY[i], 0));
                }

                // 坐标系转换
                MachineCoordinateSystem.GetInstance().CreateCoordinateSystem("BondCameraCoordinateSystem", "BondCoordinateSystem", false, CoordinateSystemTypeEnum.Vm);

                DependentCoordinateSystem dependentCoordinateSystem = (DependentCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name == "BondCameraCoordinateSystem");
                dependentCoordinateSystem.Init(realPoints, imgPoints);

                MachineCoordinateSystem.GetInstance().Save();
                MachineCoordinateSystem.Refresh();

                // 调用9点标定的接口(Halcon)（发送定位结果和相对坐标）
                List<AKRSPoint2D> physicalPoints = this.bmcCalibPoints.Select(p => new AKRSPoint2D(p.X - this.calibratePara.BMCVisionMachinePos.X, p.Y - this.calibratePara.BMCVisionMachinePos.Y)).ToList();

                HTuple homMatTrans;
                HTuple scaleX, scaleY, phi, slant, xTrans, yTrans;

                CalibService.NPointTrans(imagePoints, physicalPoints, out homMatTrans);
                this.calibratePara.BondHomMatTrans = homMatTrans;
                HOperatorSet.HomMat2dToAffinePar(homMatTrans, out scaleX, out scaleY, out phi, out slant, out xTrans, out yTrans);

                // Bond相机移动到BMC中心点上方
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCVisionMachinePos);

                MatchResult matchResult1 = this.LocatePosition(this.calibratePara.BmcPRName);

                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, matchResult1);

                this.calibController.LocateAndMoveToTargetPixel(this.calibratePara.BmcPRName, CalibController.CamCoordinateType.Bond, 1, 2);

                this.calibratePara.BMCVisionMachinePos = this.bondModuleController.Get3DRealPosition();

                this.calibCamScale = CalibService.CreateCalibCamScale("Bond相机像素比", Math.Round(scaleX.D, 6), Math.Round(scaleY.D, 6), Math.Round(phi.TupleDeg().D, 6));
                this.CalibCamScales.Add(this.calibCamScale);

                this.calibTransResult = CalibService.CreateCalibTransResult("邦头坐标转换信息", Math.Round(xTrans.D, 6), Math.Round(yTrans.D, 6), Math.Round(slant.TupleDeg().D, 6));
                this.CalibTransResults.Add(this.calibTransResult);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return false;
            }
        }

        /// <summary>
        /// Bond相机上视相机三点一线
        /// </summary>
        /// <returns>是否成功</returns>
        public bool StartBondToCamBySameMarkTask()
        {
            try
            {
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BondCamUpLookCamMachinePos);

                MatchResult bondCamMatchResult = this.LocatePosition("BondCamUpLookCamAlign-Bond");

                MatchResult upLookCamMatchResult = this.LocatePosition("BondCamUpLookCamAlign");

                AKRSPoint2D curMarkBondPos = CalibService.GetMachinePosByPixelPos(
                    this.bondModuleController.Get2DRealPosition(),
                    bondCamMatchResult,
                    "BondCameraCoordinateSystem");

                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, bondCamMatchResult);

                AKRSPoint2D curBondCamCenterPos = this.bondModuleController.Get2DRealPosition();

                AKRSPoint2D curMarkUpLookPos = CalibService.GetMachinePosByPixelPos(
                    this.bondModuleController.Get2DRealPosition(),
                    upLookCamMatchResult,
                    "UpLookCameraCoordinateSystem");

                AKRSPoint2D upLookCameraCenterPos = CalibService.GetMachinePosByPixelPos(
                    this.bondModuleController.Get2DRealPosition(),
                    new MatchResult(1224, 1024, 0),
                    "UpLookCameraCoordinateSystem");

                AKRSPoint2D bondCamUpLookCamAlignPos = curBondCamCenterPos + curMarkUpLookPos - upLookCameraCenterPos;

                double offSetX = this.calibratePara.GlassUpLookVisionMachinePos.X - bondCamUpLookCamAlignPos.X;
                double offSetY = this.calibratePara.GlassUpLookVisionMachinePos.Y - bondCamUpLookCamAlignPos.Y;

                this.calibratePara.BondRotateCenterToCamOffset.X = offSetX;
                this.calibratePara.BondRotateCenterToCamOffset.Y = offSetY;

                this.calibratePara.BondCamUpLookCamMachinePos.X = bondCamUpLookCamAlignPos.X;
                this.calibratePara.BondCamUpLookCamMachinePos.Y = bondCamUpLookCamAlignPos.Y;

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return false;
            }

        }

        /// <summary>
        /// 上视N点标定线程
        /// </summary>
        /// <returns>是否成功</returns>
        public bool StartUpLookCalibTask()
        {
            try
            {
                // CalibrateRunPara.Load();

                // 看小标定片定位并移动到中心
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassUpLookVisionMachinePos);
                MatchResult matchResult1 = this.LocatePosition(this.calibratePara.UpLookPRName);
                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.UpLook, matchResult1);

                this.calibController.LocateAndMoveToTargetPixel(this.calibratePara.UpLookPRName, CalibController.CamCoordinateType.UpLook, 1, 2);

                AKRSPoint3D curMachinePos = this.bondModuleController.Get3DRealPosition();

                this.calibratePara.GlassUpLookVisionMachinePos = this.bondModuleController.Get3DRealPosition();

                // 生成上视标定点位列表
                this.upLookCalibPoints = CalibService.GeneratePointList(this.calibratePara.GlassUpLookVisionMachinePos, this.calibratePara.UpLookCalibDistance, 6, 5);

                List<AKRSPoint2D> imagePoints = new List<AKRSPoint2D>();
                int marksNum = this.upLookCalibPoints.Count;
                float[] imageX = new float[marksNum];
                float[] imageY = new float[marksNum];
                float[] worldX = new float[marksNum];
                float[] worldY = new float[marksNum];

                List<AKRSPoint3D> imgPoints = new List<AKRSPoint3D>();
                List<AKRSPoint3D> realPoints = new List<AKRSPoint3D>();

                for (int i = 0; i < this.upLookCalibPoints.Count; i++)
                {
                    AKRSPoint3D upLookCalibPoint = new AKRSPoint3D(
                        this.upLookCalibPoints[i].X,
                        this.upLookCalibPoints[i].Y,
                        this.calibratePara.GlassUpLookVisionMachinePos.Z);

                    // 轴移动到位
                    this.bondModuleController.MoveSafeBondXYZ(upLookCalibPoint);

                    Thread.Sleep(1000);

                    // MatchResult matchResult = this.LocatePosition("测试模板");
                    MatchResult matchResult = this.LocatePosition(this.calibratePara.UpLookPRName);

                    imagePoints.Add(new AKRSPoint2D(matchResult.CenterX, matchResult.CenterY));
                    imageX[i] = (float)matchResult.CenterX;
                    imageY[i] = (float)matchResult.CenterY;
                    worldX[i] = (float)(this.upLookCalibPoints[i].X - this.calibratePara.GlassUpLookVisionMachinePos.X);
                    worldY[i] = (float)(this.upLookCalibPoints[i].Y - this.calibratePara.GlassUpLookVisionMachinePos.Y);

                    imgPoints.Add(new AKRSPoint3D(matchResult.CenterX, matchResult.CenterY, 0));
                    realPoints.Add(new AKRSPoint3D(worldX[i], worldY[i], 0));
                }

                // 坐标系转换
                MachineCoordinateSystem.GetInstance().CreateCoordinateSystem("UpLookCameraCoordinateSystem", "BondCoordinateSystem", false, CoordinateSystemTypeEnum.Vm);

                DependentCoordinateSystem dependentCoordinateSystem = (DependentCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name == "UpLookCameraCoordinateSystem");
                dependentCoordinateSystem.Init(realPoints, imgPoints, TransformTool.CalibModuleEnum.CameraStaticDown);
                MachineCoordinateSystem.GetInstance().Save();
                MachineCoordinateSystem.Refresh();

                // 调用12点标定的接口（发送定位结果和相对坐标）
                // Correction.GetInstance().UpLookCamCalib(imageX, imageY, worldX, worldY, out cameraScale, out transX, out transY);

                // 调用9点标定的接口(Halcon)（发送定位结果和相对坐标）
                List<AKRSPoint2D> physicalPoints = this.upLookCalibPoints.Select(p => new AKRSPoint2D(p.X - this.calibratePara.GlassUpLookVisionMachinePos.X, p.Y - this.calibratePara.GlassUpLookVisionMachinePos.Y)).ToList();

                HTuple homMatTrans;
                HTuple scaleX, scaleY, phi, slant, xTrans, yTrans;

                CalibService.NPointTrans(imagePoints, physicalPoints, out homMatTrans);
                this.calibratePara.UpLookHomMatTrans = homMatTrans;
                HOperatorSet.HomMat2dToAffinePar(homMatTrans, out scaleX, out scaleY, out phi, out slant, out xTrans, out yTrans);

                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassUpLookVisionMachinePos);

                int[] angleArray = new int[] { -180, -90, 0, 90, 180 };
                List<AKRSPoint2D> circlePointList = new List<AKRSPoint2D>();
                AKRSPoint2D curMachinePos2D = this.bondModuleController.Get2DRealPosition();
                List<AKRSPoint2D> matchResultList2D = new List<AKRSPoint2D>();

                foreach (int angle in angleArray)
                {
                    // T轴旋转一定角度
                    this.bondHeadController.RotateAxisT(angle);
                    MatchResult matchResult = LocatePosition(this.calibratePara.UpLookPRName);

                    matchResultList2D.Add(new AKRSPoint2D(matchResult.CenterX, matchResult.CenterY));

                    AKRSPoint2D circlePoint = CalibService.GetMachinePosByPixelPos(
                        curMachinePos2D,
                        matchResult,
                        "UpLookCameraCoordinateSystem");

                    circlePointList.Add(circlePoint);
                }

                // 拟合像素圆心
                CalibService.FitCircle(matchResultList2D, out double circleCenterPixelX, out double circleCenterPixelY, out double circleRadiusPixel);
                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.UpLook, new MatchResult(circleCenterPixelX, circleCenterPixelY, 0));

                AKRSPoint3D curPos = this.bondModuleController.Get3DRealPosition();

                // 记录旋转中心坐标
                this.calibratePara.GlassUpLookVisionMachinePos.X = curPos.X;
                this.calibratePara.GlassUpLookVisionMachinePos.Y = curPos.Y;

                MatchResult matchResult5 = this.LocatePosition(this.calibratePara.UpLookPRName);

                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.UpLook, matchResult5);

                this.calibController.LocateAndMoveToTargetPixel(this.calibratePara.UpLookPRName, CalibController.CamCoordinateType.UpLook, 1, 2);

                AKRSPoint3D curMachinePos1 = this.bondModuleController.Get3DRealPosition();

                AKRSPoint3D bondOffset = curMachinePos1 - this.calibratePara.GlassUpLookVisionMachinePos;

                //this.calibratePara.BondRotateCenterToCamOffset.X -= bondOffset.X;
                //this.calibratePara.BondRotateCenterToCamOffset.Y -= bondOffset.Y;

                this.calibratePara.Save();

                AKRSPoint3D placePos = new AKRSPoint3D(
                    this.calibratePara.GlassVisionMachinePos.X + this.calibratePara.BondRotateCenterToCamOffset.X,
                    this.calibratePara.GlassVisionMachinePos.Y + this.calibratePara.BondRotateCenterToCamOffset.Y,
                    0);

                this.bondHeadController.MoveBondZToSafePos();

                // 轴移动至放标位放置标定片
                this.bondModuleController.MoveSafeBondXYZ(placePos);

                this.bondHeadController.RotateAxisT(0);

                // 取放片参数
                BaseCarrierConfig component = new BaseCarrierConfig()
                {
                    // 取片
                    IsActivateSlowTravelBeforePickup = true,
                    SlowTravelSpeedBeforePickup =
                                                          this.bMCDevicePara.SlowTravelSpeedBeforePickup,
                    SlowTravelDistanceBeforePickup =
                                                          this.bMCDevicePara.SlowTravelDistanceBeforePickup,
                    IsActivateSlowTravelAfterPickup = true,
                    SlowTravelSpeedAfterPickup =
                                                          this.bMCDevicePara.SlowTravelSpeedAfterPickup,
                    SlowTravelDistanceAfterPickup =
                                                          this.bMCDevicePara.SlowTravelDistanceAfterPickup,
                    VacuumOffDelay = this.bMCDevicePara.VacuumOffDelay,

                    // todo:力控暂时没接
                    PickupForceMode = ForceModeEnum.Distance,

                    // 真空延时
                    //IPTVacuumOffDelay = 500,
                    //IPTVacuumBuildUpDelay = 500,

                    // 放片
                    IsActivateSlowTravelBeforeBonding = true,
                    SlowTravelSpeedBeforeBonding = this.bMCDevicePara.SlowTravelSpeedBeforeBonding,
                    SlowTravelDistanceBeforeBonding = this.bMCDevicePara.SlowTravelDistanceBeforeBonding,
                    IsActivateSlowTravelAfterBonding = true,
                    SlowTravelSpeedAfterBonding = this.bMCDevicePara.SlowTravelSpeedAfterBonding,
                    SlowTravelDistanceAfterBonding = this.bMCDevicePara.SlowTravelDistanceAfterBonding,
                    BondingBlowDelay = this.bMCDevicePara.BondingBlowDelay,
                    PlacementDelay = this.bMCDevicePara.PlacementDelay,
                    IsActiveComponentDetection = true,
                    BondingForceMode = ForceModeEnum.Distance,
                };
                double pickLevel = this.calibratePara.GlassPickZMachinePos + 5;
                double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

                // 放片
                this.bondHeadController.BondAction(
                    pickLevel,
                    liftLevel,
                    component,
                    BondTypeEnum.BondOnTU);

                this.calibCamScale = CalibService.CreateCalibCamScale("上视相机像素比", Math.Round(scaleX.D, 6), Math.Round(scaleY.D, 6), Math.Round(phi.TupleDeg().D, 6));
                this.CalibCamScales.Add(this.calibCamScale);

                this.calibTransResult = CalibService.CreateCalibTransResult("上视相机坐标转换信息系", Math.Round(xTrans.D, 6), Math.Round(yTrans.D, 6), Math.Round(slant.TupleDeg().D, 6));
                this.CalibTransResults.Add(this.calibTransResult);

                this.calibTransResult = CalibService.CreateCalibTransResult("上视相机当前旋转中心r", Math.Round(this.calibratePara.GlassUpLookVisionMachinePos.X, 6), Math.Round(this.calibratePara.GlassUpLookVisionMachinePos.Y, 6), Math.Round(this.calibratePara.GlassUpLookVisionMachinePos.Z, 6));
                this.CalibTransResults.Add(this.calibTransResult);

                // 结果赋值
                this.calibTransResult = CalibService.CreateCalibTransResult("邦头到邦头相机偏移", Math.Round(this.calibratePara.BondRotateCenterToCamOffset.X, 6), Math.Round(this.calibratePara.BondRotateCenterToCamOffset.Y, 6), 0);
                this.CalibTransResults.Add(this.calibTransResult);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return false;
            }
        }

        /// <summary>
        /// 标定上视相机位置
        /// </summary>
        /// <returns>是否成功</returns>
        public bool StartUpLookMachinePosTask()
        {
            try
            {
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.UpLookMarkMachinePos);

                MatchResult matchResult = this.LocatePosition(this.calibratePara.WaferLeftPRName);

                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, matchResult);

                this.calibController.LocateAndMoveToTargetPixel(this.calibratePara.WaferLeftPRName, CalibController.CamCoordinateType.Bond, 1, 2);

                this.calibratePara.UpLookMarkMachinePos = this.bondModuleController.Get3DRealPosition();

                this.calibTransResult = CalibService.CreateCalibTransResult("上视相机位置", Math.Round(this.calibratePara.UpLookMarkMachinePos.X, 6), Math.Round(this.calibratePara.UpLookMarkMachinePos.Y, 6), Math.Round(this.calibratePara.UpLookMarkMachinePos.Z, 6));
                this.CalibTransResults.Add(this.calibTransResult);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return false;
            }
        }

        /// <summary>
        /// 晶圆Mark点距离标定
        /// </summary>
        /// <returns>是否成功</returns>
        public bool StartWaferTableCalibTask()
        {
            try
            {
                // CalibrateRunPara.Load();

                // 晶圆台移动到安全位置
                // this.waferTableController.MoveWaferTableToMachinePos(this.calibratePara.WaferTableReadyMachinePos);
                this.calibController.MoveWaferTableToMachinePos(this.calibratePara.WaferTableReadyMachinePos);

                // 邦头相机移动到左标记点中心
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.WaferTableLeftMarkBCVisionMachinePos);

                // MatchResult matchResultLeft = this.LocatePosition("晶圆左标定模板");
                MatchResult matchResultLeft = this.LocatePosition(this.calibratePara.WaferLeftPRName);

                // 计算左标记点绝对位置
                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, matchResultLeft);

                this.calibController.LocateAndMoveToTargetPixel(this.calibratePara.WaferLeftPRName, CalibController.CamCoordinateType.Bond, 1, 2);

                AKRSPoint2D leftAbsPosition = this.bondModuleController.Get2DRealPosition();

                this.calibratePara.WaferTableLeftMarkBCVisionMachinePos.X = leftAbsPosition.X;
                this.calibratePara.WaferTableLeftMarkBCVisionMachinePos.Y = leftAbsPosition.Y;

                //AKRSPoint2D leftAbsPosition = CalibService.GetMachinePosByPixelPos(this.bondModuleController.Get2DRealPosition(), matchResultLeft, "BondCameraCoordinateSystem");

                // 邦头相机移动到右标记点中心
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.WaferTableRightMarkVisionMachinePos);

                // MatchResult matchResultRight = this.LocatePosition("晶圆右标定模板");
                MatchResult matchResultRight = this.LocatePosition(this.calibratePara.WaferRightPRName);

                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, matchResultRight);

                this.calibController.LocateAndMoveToTargetPixel(this.calibratePara.WaferRightPRName, CalibController.CamCoordinateType.Bond, 1, 2);

                AKRSPoint2D rightAbsPosition = this.bondModuleController.Get2DRealPosition();

                this.calibratePara.WaferTableRightMarkVisionMachinePos.X = rightAbsPosition.X;
                this.calibratePara.WaferTableRightMarkVisionMachinePos.Y = rightAbsPosition.Y;

                // 计算右标记点绝对位置
                //AKRSPoint2D rightAbsPosition = CalibService.GetMachinePosByPixelPos(this.bondModuleController.Get2DRealPosition(), matchResultRight, "BondCameraCoordinateSystem");

                this.calibratePara.WaferTableMarksDistance = new AKRSPoint2D { X = rightAbsPosition.X - leftAbsPosition.X, Y = rightAbsPosition.Y - leftAbsPosition.Y };

                // 计算两标定点物理间距
                // calibratePara.WaferTableMarksDistance.X = RightAbsPosition.X - LeftAbsPosition.X;
                //// calibratePara.WaferTableMarksDistance.Y = RightAbsPosition.Y - LeftAbsPosition.Y;

                this.calibTransResult = CalibService.CreateCalibTransResult("晶圆标记点间距", Math.Round(this.calibratePara.WaferTableMarksDistance.X, 6), Math.Round(this.calibratePara.WaferTableMarksDistance.Y, 6), 0);
                this.CalibTransResults.Add(this.calibTransResult);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return false;
            }
        }

        /// <summary>
        /// 晶圆台N点标定线程
        /// </summary>
        /// <returns>是否成功</returns>
        public bool StartWaferNPointCalibTask()
        {
            try
            {
                // Bond移开到安全位置
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCVisionMachinePos);

                // 晶圆台左Mark移动到晶圆相机中心

                // this.waferTableModule.MoveXY(this.calibratePara.WaferTableLeftMarkWCVisionMachinePos);
                // this.waferTableController.MoveWaferTableToMachinePos(this.calibratePara.WaferTableLeftMarkWCVisionMachinePos);
                this.calibController.MoveWaferTableToMachinePos(this.calibratePara.WaferTableLeftMarkWCVisionMachinePos);

                this.calibController.MoveWaferCameraZ(this.calibratePara.WaferTableLeftMarkWCVisionMachinePos.Z);

                // 生成晶圆台标定点位列表
                this.waferTableCalibPoints = CalibService.GeneratePointList(
                    this.calibratePara.WaferTableLeftMarkWCVisionMachinePos,
                    this.calibratePara.WaferTableCalibDistance,
                    5,
                    5);

                List<AKRSPoint2D> imagePoints = new List<AKRSPoint2D>();
                int marksNum = this.waferTableCalibPoints.Count;
                float[] imageX = new float[marksNum];
                float[] imageY = new float[marksNum];
                float[] worldX = new float[marksNum];
                float[] worldY = new float[marksNum];

                List<AKRSPoint3D> imgPoints = new List<AKRSPoint3D>();
                List<AKRSPoint3D> realPoints = new List<AKRSPoint3D>();

                for (int i = 0; i < this.waferTableCalibPoints.Count; i++)
                {
                    // 轴移动到位
                    // this.waferTableModule.MoveXY(
                    //    new AKRSPoint3D(this.waferTableCalibPoints[i].X, this.waferTableCalibPoints[i].Y, 0));
                    // this.waferTableController.MoveWaferTableToMachinePos(this.waferTableCalibPoints[i]);
                    this.calibController.MoveWaferTableToMachinePos(new AKRSPoint3D(this.waferTableCalibPoints[i].X, this.waferTableCalibPoints[i].Y, 0));

                    Thread.Sleep(1000);

                    // MatchResult matchResult = this.LocatePosition("晶圆相机标定模板");
                    MatchResult matchResult = this.LocatePosition(this.calibratePara.WaferPRName);

                    imagePoints.Add(new AKRSPoint2D(matchResult.CenterX, matchResult.CenterY));
                    imageX[i] = (float)matchResult.CenterX;
                    imageY[i] = (float)matchResult.CenterY;
                    worldX[i] = (float)(this.waferTableCalibPoints[i].X
                                        - this.calibratePara.WaferTableLeftMarkWCVisionMachinePos.X);
                    worldY[i] = (float)(this.waferTableCalibPoints[i].Y
                                        - this.calibratePara.WaferTableLeftMarkWCVisionMachinePos.Y);

                    imgPoints.Add(new AKRSPoint3D(matchResult.CenterX, matchResult.CenterY, this.calibratePara.WaferTableLeftMarkWCVisionMachinePos.Z));
                    realPoints.Add(new AKRSPoint3D(worldX[i], worldY[i], 0));
                }

                // 调用9点标定的接口（发送定位结果和相对坐标）
                // Correction.GetInstance().WaferCamCalib(
                // imageX,
                // imageY,
                // worldX,
                // worldY,
                // out double cameraScale,
                // out double transX,
                // out double transY);

                MachineCoordinateSystem.GetInstance().CreateCoordinateSystem(
                    "WaferCameraCoordinateSystem",
                    "WaferTableCoordinateSystem",
                    false,
                    CoordinateSystemTypeEnum.Vm);

                DependentCoordinateSystem a = (DependentCoordinateSystem)MachineCoordinateSystem.GetInstance()
                    .CoordinateSystems.Find(it => it.Name == "WaferCameraCoordinateSystem");
                a.Init(realPoints, imgPoints, TransformTool.CalibModuleEnum.CameraStaticUp);
                MachineCoordinateSystem.GetInstance().Save();
                MachineCoordinateSystem.Refresh();

                // 调用9点标定的接口(Halcon)（发送定位结果和相对坐标）
                List<AKRSPoint2D> physicalPoints = this.waferTableCalibPoints.Select(
                    p => new AKRSPoint2D(
                        p.X - this.calibratePara.WaferTableLeftMarkWCVisionMachinePos.X,
                        p.Y - this.calibratePara.WaferTableLeftMarkWCVisionMachinePos.Y)).ToList();

                HTuple homMatTrans;
                HTuple scaleX, scaleY, phi, slant, xTrans, yTrans;
                CalibService.NPointTrans(imagePoints, physicalPoints, out homMatTrans);
                this.calibratePara.WaferHomMatTrans = homMatTrans;
                HOperatorSet.HomMat2dToAffinePar(
                    homMatTrans,
                    out scaleX,
                    out scaleY,
                    out phi,
                    out slant,
                    out xTrans,
                    out yTrans);

                // 回起点
                // this.waferTableModule.MoveXY(this.calibratePara.WaferTableLeftMarkWCVisionMachinePos);

                // this.waferTableController.MoveWaferTableToMachinePos(this.calibratePara.WaferTableLeftMarkWCVisionMachinePos);
                this.calibController.MoveWaferTableToMachinePos(this.calibratePara.WaferTableLeftMarkWCVisionMachinePos);

                // MatchResult matchResult1 = this.LocatePosition("晶圆相机标定模板");
                MatchResult matchResult1 = this.LocatePosition(this.calibratePara.WaferPRName);

                // 晶圆台Mark点回纠正后相机中心位置
                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Wafer, matchResult1);

                this.calibController.LocateAndMoveToTargetPixel(this.calibratePara.WaferPRName, CalibController.CamCoordinateType.Wafer, 1, 2);


                // 更新Mark点在晶圆相机中心位置
                this.calibratePara.WaferTableLeftMarkWCVisionMachinePos = this.calibController.GetWaferTableRealPos();


                // Bond头移动到测高位进行测高
                // this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.WaferTableMeasureHeightSearchMachinePos);

                // 测高
                /*(ExcuteResult Ret, double HeightValue) res = BondModule.MeasureHeight(
                BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightSearchHeight,
                BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightLiftLevel,
                BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightSearchDistance,
                BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightSearchVel,
                 "接触传感器");
    
                if (res.Ret == ExcuteResult.Success)
                {
                    //c+alibratePara.GlassPickHeight = res.HeightValue;
                }*/

                // Bond相机移动到右Mark点中心位置
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.WaferTableRightMarkBCVisionMachinePos);

                Thread.Sleep(2000);

                // 定位获取绝对位置 
                // MatchResult bondMatchResult = this.LocatePosition("晶圆右标定模板");
                MatchResult bondMatchResult = this.LocatePosition(this.calibratePara.WaferRightPRName);

                calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, bondMatchResult);
                //AKRSPoint2D leftAbsPosition = CalibService.GetMachinePosByPixelPos(this.bondModuleController.Get2DRealPosition(), bondMatchResult, "BondCameraCoordinateSystem");

                this.calibController.LocateAndMoveToTargetPixel(this.calibratePara.WaferRightPRName, CalibController.CamCoordinateType.Bond, 1, 2);

                AKRSPoint2D rightAbsPosition = this.bondModuleController.Get2DRealPosition();

                // 更新位置
                // this.calibratePara.WaferTableRightMarkBCVisionMachinePos.X = BondModule.BondAxisX.GetRealPosition();
                // this.calibratePara.WaferTableRightMarkBCVisionMachinePos.Y = BondModule.BondAxisY.GetRealPosition();
                this.calibratePara.WaferTableRightMarkBCVisionMachinePos.X = rightAbsPosition.X;
                this.calibratePara.WaferTableRightMarkBCVisionMachinePos.Y = rightAbsPosition.Y;

                this.calibratePara.BondHeadRotateCenterInWC = new AKRSPoint2D
                {
                                                // 晶圆台右标记点位Bond相机拍照位
                    X = this.calibratePara.WaferTableRightMarkBCVisionMachinePos.X
                                                                  // 晶圆台左右两标定点间距
                                                                  - this.calibratePara.WaferTableMarksDistance.X
                                                                  //  邦头旋转中心到相机中心偏移 
                                                                  + this.calibratePara.BondRotateCenterToCamOffset.X,
                    Y = this.calibratePara.WaferTableRightMarkBCVisionMachinePos.Y
                                                                  - this.calibratePara.WaferTableMarksDistance.Y
                                                                  + this.calibratePara.BondRotateCenterToCamOffset.Y
                };

                this.calibratePara.Save();

                MachineCoordinateSystem.GetInstance().CreateCoordinateSystem("WaferTableCoordinateSystem", "G0", true, CoordinateSystemTypeEnum.General);
                GeneralCoordinateSystem waferCoordinateSystem = (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name == "WaferTableCoordinateSystem");

                GeneralCoordinateSystem bondCoordinateSystem = (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name == "BondCoordinateSystem");

                AKRSPoint3D aKRSAKRS = bondCoordinateSystem.ForwardConvertCoordinate(new AKRSPoint3D(this.calibratePara.BondHeadRotateCenterInWC.X, this.calibratePara.BondHeadRotateCenterInWC.Y, this.bondHeadController.GetAxisZRealPos()));

                waferCoordinateSystem.Init(aKRSAKRS, this.calibController.GetWaferTableRealPos(), 0);
                MachineCoordinateSystem.GetInstance().Save();
                MachineCoordinateSystem.Refresh();

                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCVisionMachinePos);

                this.bondHeadController.MoveBondZToSafePos();

                this.calibController.MoveWaferCameraZ(0);

                // calibratePara.BondHeadRotateCenterInWC.X = calibratePara.WaferTableRightMarkBCVisionMachinePos.X - calibratePara.WaferTableMarksDistance.X + calibratePara.BondRotateCenterToCamOffset.X;
                //// calibratePara.BondHeadRotateCenterInWC.Y = calibratePara.WaferTableRightMarkBCVisionMachinePos.Y - calibratePara.WaferTableMarksDistance.Y + calibratePara.BondRotateCenterToCamOffset.Y;

                this.calibCamScale = CalibService.CreateCalibCamScale(
                    "晶圆相机像素比",
                    Math.Round(scaleX.D, 6),
                    Math.Round(scaleY.D, 6),
                    Math.Round(phi.TupleDeg().D, 6));
                this.CalibCamScales.Add(this.calibCamScale);

                this.calibTransResult = CalibService.CreateCalibTransResult(
                    "晶圆相机坐标转换信息",
                    Math.Round(xTrans.D, 6),
                    Math.Round(yTrans.D, 6),
                    Math.Round(slant.TupleDeg().D, 6));
                this.CalibTransResults.Add(this.calibTransResult);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return false;
            }
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
                throw new ArgumentNullException(patternName,  patternName + " 定位失败.");
            }

            // 获取定位结果
            MatchResult matchResult = (MatchResult)pREntity.AlgResult;

            return matchResult;
        }

        /// <summary>
        /// transport到G0
        /// </summary>
        public void StartTransportCalib()
        {
            GeneralCoordinateSystem a = (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CreateCoordinateSystem(
                            "TransportCoordinateSystem",
                            "G0",
                            true,
                            CoordinateSystemTypeEnum.General);

            GeneralCoordinateSystem b = (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance()
                .CoordinateSystems.Find(it => it.Name == "BondCoordinateSystem");

            // 测高

            (ExcuteResult Ret, double HeightValue) res = this.bondHeadController.MeasureHeight(1, HeightMeasurementFunctionEnum.WithTDSensor);

            if (res.Ret == ExcuteResult.Success)
            {
            }
            else
            {
                AKRSXtraMessageBox.Show("MeasureHeight fail");
            }

            AKRSPoint3D transportPosition = new AKRSPoint3D()
            {
                X = this.bondModuleController.GetAxisXRealPos() + this.calibratePara.BondRotateCenterToCamOffset.X,
                Y = this.bondModuleController.GetAxisYRealPos() + this.calibratePara.BondRotateCenterToCamOffset.Y,
                Z = res.HeightValue
            };
            a.Init(b.ForwardConvertCoordinate(transportPosition), new AKRSPoint3D());

            MachineCoordinateSystem.GetInstance().Save();
        }

        /// <summary>
        /// 点胶相机单步标定
        /// </summary>
        public void OnlyDispenseCameraCalib()
        {
            this.calibCamScale = new CalibCamScaleResult();
            this.calibTransResult = new CalibTransResult();

            this.CalibCamScales.Clear();
            this.CalibTransResults.Clear();

            this.dispenseMeasureHeightController.CloseDispenseHeightMeasurementCylinder();

            // 点胶相机移动到点胶标定点中心上方
            this.calibController.MoveDispenseToMachinePos(this.calibratePara.DispenseMarkVisionMachinePos);

            // 创建点胶N点标定点位列表
            this.dispenseCalibPoints = CalibService.GeneratePointList(this.calibratePara.DispenseMarkVisionMachinePos, this.calibratePara.DispenseCalibDistance, 3, 3);

            List<AKRSPoint2D> imagePoints = new List<AKRSPoint2D>();
            int marksNum = this.dispenseCalibPoints.Count;
            float[] imageX = new float[marksNum];
            float[] imageY = new float[marksNum];
            float[] worldX = new float[marksNum];
            float[] worldY = new float[marksNum];

            List<AKRSPoint3D> imgPoints = new List<AKRSPoint3D>();
            List<AKRSPoint3D> realPoints = new List<AKRSPoint3D>();

            //// 点胶相机遍历点位开始定位并标定
            for (int i = 0; i < marksNum; i++)
            {
                // 轴移动到位
                this.calibController.MoveDispenseToMachinePos(new AKRSPoint2D(this.dispenseCalibPoints[i].X, this.dispenseCalibPoints[i].Y));

                Thread.Sleep(200);

                // 获取Pr实体
                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find("点胶标定模板");

                // PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.calibratePara.DispensePRName);

                // 开始定位
                ExcuteResult excuteResult = pREntity.DoWork();

                // 拍照失败，直接返回错误
                if (excuteResult != ExcuteResult.Success)
                {
                    return;
                }

                // 获取定位结果
                MatchResult matchResult = (MatchResult)pREntity.AlgResult;

                imagePoints.Add(new AKRSPoint2D(matchResult.CenterX, matchResult.CenterY));

                imageX[i] = (float)matchResult.CenterX;
                imageY[i] = (float)matchResult.CenterY;
                worldX[i] = (float)(this.dispenseCalibPoints[i].X - this.calibratePara.DispenseMarkVisionMachinePos.X);
                worldY[i] = (float)(this.dispenseCalibPoints[i].Y - this.calibratePara.DispenseMarkVisionMachinePos.Y);

                imgPoints.Add(new AKRSPoint3D(matchResult.CenterX, matchResult.CenterY, 0));
                realPoints.Add(new AKRSPoint3D(worldX[i], worldY[i], 0));
            }

            // 调用9点标定的接口(Halcon)（发送定位结果和相对坐标）
            List<AKRSPoint2D> physicalPoints = this.dispenseCalibPoints.Select(p => new AKRSPoint2D(p.X - this.calibratePara.DispenseMarkVisionMachinePos.X, p.Y - this.calibratePara.DispenseMarkVisionMachinePos.Y)).ToList();

            HTuple homMatTrans;
            HTuple scaleX, scaleY, phi, slant, xTrans, yTrans;

            CalibService.NPointTrans(imagePoints, physicalPoints, out homMatTrans);
            HOperatorSet.HomMat2dToAffinePar(homMatTrans, out scaleX, out scaleY, out phi, out slant, out xTrans, out yTrans);

            this.calibCamScale = CalibService.CreateCalibCamScale("点胶相机像素比", Math.Round(scaleX.D, 6), Math.Round(scaleY.D, 6), Math.Round(phi.TupleDeg().D, 6));
            this.CalibCamScales.Add(this.calibCamScale);

            this.calibTransResult = CalibService.CreateCalibTransResult("点胶相机坐标转换信息", Math.Round(xTrans.D, 6), Math.Round(yTrans.D, 6), Math.Round(slant.TupleDeg().D, 6));
            this.CalibTransResults.Add(this.calibTransResult);

            MainForm.SendUiAction(
                () =>
                {
                    FrmCalibResult frmCalibResult = new FrmCalibResult();
                    frmCalibResult.ShowDialog();
                    frmCalibResult.Dispose();
                });
        }

        /// <summary>
        /// Bond相机单步标定
        /// </summary>
        public void OnlyBondCameraCalib()
        {
            this.CalibCamScales.Clear();
            this.CalibTransResults.Clear();

            // Bond相机移动到BMC中心点上方
            this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCVisionMachinePos);

            // MatchResult bigMatchResult = this.LocatePosition("大标定片模板");
            MatchResult bigMatchResult = this.LocatePosition(this.calibratePara.BmcPRName);

            this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, bigMatchResult);

            this.calibratePara.BMCVisionMachinePos = this.bondModuleController.Get3DRealPosition();

            List<AKRSPoint2D> bmcCalibPoints = new List<AKRSPoint2D>();

            // 创建BMC标定点位列表
            bmcCalibPoints = CalibService.GeneratePointList(this.calibratePara.BMCVisionMachinePos, 0.4, 0.3, 6, 5);

            List<AKRSPoint2D> imagePoints = new List<AKRSPoint2D>();
            int marksNum = bmcCalibPoints.Count;
            float[] imageX = new float[marksNum];
            float[] imageY = new float[marksNum];
            float[] worldX = new float[marksNum];
            float[] worldY = new float[marksNum];

            List<AKRSPoint3D> imgPoints = new List<AKRSPoint3D>();
            List<AKRSPoint3D> realPoints = new List<AKRSPoint3D>();

            // Bond相机遍历点位开始定位并标定
            for (int i = 0; i < bmcCalibPoints.Count; i++)
            {
                // 轴移动到位
                this.bondModuleController.MoveSafeBondXY(bmcCalibPoints[i].X, bmcCalibPoints[i].Y);
                this.bondHeadController.MoveAxisZ(this.calibratePara.BMCVisionMachinePos.Z);
                Thread.Sleep(300);

                // 获取Pr实体
                // PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find("大标定片模板");
                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.calibratePara.BmcPRName);

                // 开始定位
                ExcuteResult excuteResult = pREntity.DoWork();

                // 拍照失败，直接返回错误
                if (excuteResult != ExcuteResult.Success)
                {
                    return;
                }

                // 获取定位结果
                MatchResult matchResult = (MatchResult)pREntity.AlgResult;

                imagePoints.Add(new AKRSPoint2D(matchResult.CenterX, matchResult.CenterY));
                imageX[i] = (float)matchResult.CenterX;
                imageY[i] = (float)matchResult.CenterY;
                worldX[i] = (float)(bmcCalibPoints[i].X - this.calibratePara.BMCVisionMachinePos.X);
                worldY[i] = (float)(bmcCalibPoints[i].Y - this.calibratePara.BMCVisionMachinePos.Y);

                imgPoints.Add(new AKRSPoint3D(matchResult.CenterX, matchResult.CenterY, 0));
                realPoints.Add(new AKRSPoint3D(worldX[i], worldY[i], 0));
            }

            // 调用9点标定的接口(Halcon)（发送定位结果和相对坐标）
            List<AKRSPoint2D> physicalPoints = bmcCalibPoints.Select(p => new AKRSPoint2D(p.X - this.calibratePara.BMCVisionMachinePos.X, p.Y - this.calibratePara.BMCVisionMachinePos.Y)).ToList();

            HTuple homMatTrans;
            HTuple scaleX, scaleY, phi, slant, xTrans, yTrans;

            CalibService.NPointTrans(imagePoints, physicalPoints, out homMatTrans);
            this.calibratePara.BondHomMatTrans = homMatTrans;
            HOperatorSet.HomMat2dToAffinePar(homMatTrans, out scaleX, out scaleY, out phi, out slant, out xTrans, out yTrans);

            // Bond相机移动到BMC中心点上方
            this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCVisionMachinePos);

            this.calibCamScale = CalibService.CreateCalibCamScale("Bond相机像素比", Math.Round(scaleX.D, 6), Math.Round(scaleY.D, 6), Math.Round(phi.TupleDeg().D, 6));
            this.CalibCamScales.Add(this.calibCamScale);

            this.calibTransResult = CalibService.CreateCalibTransResult("邦头相机坐标转换信息", Math.Round(xTrans.D, 6), Math.Round(yTrans.D, 6), Math.Round(slant.TupleDeg().D, 6));
            this.CalibTransResults.Add(this.calibTransResult);

            MainForm.SendUiAction(
                () =>
                {
                    FrmCalibResult frmCalibResult = new FrmCalibResult();
                    frmCalibResult.ShowDialog();
                    frmCalibResult.Dispose();
                });
        }

        /// <summary>
        /// UpLook相机单步标定
        /// </summary>
        public void OnlyUpLookCameraCalib()
        {
            this.CalibCamScales.Clear();
            this.CalibTransResults.Clear();

            // this.bondHeadController.OpenBondHeadVaccum();

            // 先取标定片 
            this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassVisionMachinePos);

            // 定位小圆并移动至相机中心
            MatchResult smallMatchResult = this.LocatePosition(this.calibratePara.GlassPRName);

            CalibService.MoveToCamCenterCoo(
                this.bondModule.BondAxisX,
                this.bondModule.BondAxisY,
                smallMatchResult,
                "BondCameraCoordinateSystem");

            this.calibController.LocateAndMoveToTargetPixel(this.calibratePara.GlassPRName, CalibController.CamCoordinateType.Bond, 1, 2);

            Thread.Sleep(5000);

            this.bondHeadController.RotateAxisT(-180);

            AKRSPoint3D point3D = this.bondModuleController.Get3DRealPosition()
                                  + this.calibratePara.BondRotateCenterToCamOffset;
            point3D.Z = 0;
            this.bondModuleController.MoveSafeBondXYZ(point3D);

            #region 取放标定片参数

            // 取放片参数
            BaseCarrierConfig component = new BaseCarrierConfig()
            {
                // 取片
                IsActivateSlowTravelBeforePickup = true,
                SlowTravelSpeedBeforePickup =
                                                      this.bMCDevicePara.SlowTravelSpeedBeforePickup,
                SlowTravelDistanceBeforePickup =
                                                      this.bMCDevicePara.SlowTravelDistanceBeforePickup,
                IsActivateSlowTravelAfterPickup = true,
                SlowTravelSpeedAfterPickup =
                                                      this.bMCDevicePara.SlowTravelSpeedAfterPickup,
                SlowTravelDistanceAfterPickup =
                                                      this.bMCDevicePara.SlowTravelDistanceAfterPickup,
                VacuumOffDelay = this.bMCDevicePara.VacuumOffDelay,
                PickupDelay = this.bMCDevicePara.PickupDelay,

                // todo:力控暂时没接
                PickupForceMode = ForceModeEnum.Distance,


                // 放片
                IsActivateSlowTravelBeforeBonding = true,
                SlowTravelSpeedBeforeBonding =
                                                      this.bMCDevicePara.SlowTravelSpeedBeforeBonding,
                SlowTravelDistanceBeforeBonding =
                                                      this.bMCDevicePara.SlowTravelDistanceBeforeBonding,
                IsActivateSlowTravelAfterBonding = true,
                SlowTravelSpeedAfterBonding =
                                                      this.bMCDevicePara.SlowTravelSpeedAfterBonding,
                SlowTravelDistanceAfterBonding =
                                                      this.bMCDevicePara.SlowTravelDistanceAfterBonding,
                BondingBlowDelay = this.bMCDevicePara.BondingBlowDelay,
                PlacementDelay = this.bMCDevicePara.PlacementDelay,
                IsActiveComponentDetection = true,
                BondingForceMode = ForceModeEnum.Distance,

                WeakBlowProportion = 1500,
            };
            double pickLevel = this.calibratePara.GlassPickZMachinePos;
            double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

            #endregion

            // 取标
            this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.CarrierWithWaffle);

            this.bondHeadController.MoveBondZToSafePos();

            #region 焊点拍照

            // BMC拍照位
            AKRSPoint3D p1MarkPos = this.bondModuleController.ConvertMachineToG0Pos(
                CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos);
            p1MarkPos.Z = this.bondModuleController
                .ConvertMachineToG0Pos(CalibrateRunPara.GetInstance().BMCVisionMachinePos).Z;
            AKRSPoint3D p2MarkPos = this.bondModuleController.ConvertMachineToG0Pos(
                CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos);
            p2MarkPos.Z = this.bondModuleController
                .ConvertMachineToG0Pos(CalibrateRunPara.GetInstance().BMCVisionMachinePos).Z;

        RetryBMCP1:
            // P1定位
            MatchResult matchResult2 = (MatchResult)this.system2Controller.BondCameraVision(
                p1MarkPos,
                CalibrateRunPara.GetInstance().BmcPRName);

            DialogResult dialog;

            if (matchResult2 == null)
            {
                dialog = AKRSMessageBoxExt.Show(
                    $"Downlook  {CalibrateRunPara.GetInstance().BmcPRName}  P1 adjust failed!\r\nRetry:retry point  adjust\r\nAbort: Exit program\r\n Ignore: Ignore this failure.\r\n",
                    "Alarm",
                    new string[] { "Retry", "Abort", "Ignore" },
                    new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore },
                    AlarmLevel.SecondLevel);

                switch (dialog)
                {
                    case DialogResult.Retry:

                        goto RetryBMCP1;

                    case DialogResult.Abort:

                        throw new Exception($"{CalibrateRunPara.GetInstance().BmcPRName}  adjust  failed!");

                    case DialogResult.Ignore:
                        matchResult2 = new MatchResult();
                        break;
                }
            }

            AKRSPoint3D p1VisionPos = this.system2Controller.GetBondVisionResultPos(matchResult2);

        RetryBMCP2:
            // P2定位
            MatchResult matchResult3 = (MatchResult)this.system2Controller.BondCameraVision(
                p2MarkPos,
                CalibrateRunPara.GetInstance().BmcPRName,
                true);

            if (matchResult3 == null)
            {
                dialog = AKRSMessageBoxExt.Show(
                    $"Downlook  {CalibrateRunPara.GetInstance().BmcPRName}  P2 adjust failed!\r\nRetry:retry point  adjust\r\nAbort: Exit program\r\n Ignore: Ignore this failure.\r\n",
                    "Alarm",
                    new string[] { "Retry", "Abort", "Ignore" },
                    new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore },
                    AlarmLevel.SecondLevel);

                switch (dialog)
                {
                    case DialogResult.Retry:

                        goto RetryBMCP2;

                    case DialogResult.Abort:

                        throw new Exception($"{CalibrateRunPara.GetInstance().BmcPRName}  adjust  failed!");

                    case DialogResult.Ignore:
                        matchResult3 = new MatchResult();
                        break;
                }
            }

            AKRSPoint3D p2VisionPos = this.system2Controller.GetBondVisionResultPos(matchResult3);

            #endregion

            #region 计算贴片位

            // 计算放片位
            AKRSPoint3D placePos = (p1VisionPos + p2VisionPos) / 2
                                   + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

            #endregion

            #region 贴片

            // 运动到放片位
            this.bondModuleController.MoveToG0Pos(placePos.X, placePos.Y);

            // 放片参数,标定片厚度：2
            double placeLevel = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.7;
            liftLevel = CalibrateRunPara.GetInstance().GlassCenterCurVisionMachinePos.Z;

        // 放片
        ReBond:
            bool ret = this.bondHeadController.BondAction(placeLevel, liftLevel, component, BondTypeEnum.BondOnBMC);

            if (ret == false)
            {
                dialog = AKRSMessageBoxExt.Show(
                    $"Component place failed!",
                    "Alarm",
                    new string[] { "Retry", "Ignore" },
                    new DialogResult[] { DialogResult.Retry, DialogResult.Ignore },
                    AlarmLevel.SecondLevel);

                switch (dialog)
                {
                    // 重新Bond
                    case DialogResult.Retry:

                        goto ReBond;

                    // 终止
                    case DialogResult.Abort:
                        return;

                    // 忽略
                    case DialogResult.Ignore:
                        matchResult2 = new MatchResult();
                        break;
                }

                return;
            }

        #endregion

        RetryPostBondGlassP1:
            AKRSPoint3D postBondP1Pos = this.bondModuleController.ConvertMachineToG0Pos(
                CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos);
            postBondP1Pos.Z = this.bondModuleController
                .ConvertMachineToG0Pos(CalibrateRunPara.GetInstance().GlassCenterCurVisionMachinePos).Z;

            // 小标定片P1定位
            MatchResult glassMatchResult1 = (MatchResult)this.system2Controller.BondCameraVision(
                postBondP1Pos,
                CalibrateRunPara.GetInstance().GlassPRName,
                true);

            if (glassMatchResult1 == null)
            {
                dialog = AKRSMessageBoxExt.Show(
                    $"Downlook  {CalibrateRunPara.GetInstance().GlassPRName}  P1 adjust failed!\r\nRetry:retry point  adjust\r\nAbort: Exit program\r\n Ignore: Ignore this failure.\r\n",
                    "Alarm",
                    new string[] { "Retry", "Abort", "Ignore" },
                    new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore },
                    AlarmLevel.SecondLevel);

                switch (dialog)
                {
                    case DialogResult.Retry:

                        goto RetryPostBondGlassP1;

                    case DialogResult.Abort:

                        throw new Exception($"{CalibrateRunPara.GetInstance().GlassPRName}  adjust  failed!");

                    case DialogResult.Ignore:
                        glassMatchResult1 = new MatchResult();
                        break;
                }
            }


            AKRSPoint3D glassVisionPos1 = this.system2Controller.GetBondVisionResultPos(glassMatchResult1);

        RetryPostBondGlassP2:
            // 小标定片P2定位
            AKRSPoint3D postBondP2Pos = this.bondModuleController.ConvertMachineToG0Pos(
                CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos);
            postBondP2Pos.Z = this.bondModuleController
                .ConvertMachineToG0Pos(CalibrateRunPara.GetInstance().GlassCenterCurVisionMachinePos).Z;

            MatchResult glassMatchResult2 = (MatchResult)this.system2Controller.BondCameraVision(
                postBondP2Pos,
                CalibrateRunPara.GetInstance().GlassPRName,
                true);

            if (glassMatchResult2 == null)
            {
                dialog = AKRSMessageBoxExt.Show(
                    $"Downlook  {CalibrateRunPara.GetInstance().GlassPRName}  P1 adjust failed!\r\nRetry:retry point  adjust\r\nAbort: Exit program\r\n Ignore: Ignore this failure.\r\n",
                    "Alarm",
                    new string[] { "Retry", "Abort", "Ignore" },
                    new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore },
                    AlarmLevel.SecondLevel);

                switch (dialog)
                {
                    case DialogResult.Retry:

                        goto RetryPostBondGlassP2;

                    case DialogResult.Abort:

                        throw new Exception($"{CalibrateRunPara.GetInstance().GlassPRName}  adjust  failed!");

                    case DialogResult.Ignore:
                        glassMatchResult2 = new MatchResult();
                        break;
                }
            }

            AKRSPoint3D glassVisionPos2 = this.system2Controller.GetBondVisionResultPos(glassMatchResult2);

            // 计算小标定片中心点
            AKRSPoint3D glassCenterPos = (glassVisionPos1 + glassVisionPos2) / 2
                                         + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

            this.bondModuleController.MoveToG0Pos(glassCenterPos.X, glassCenterPos.Y);
            this.bondHeadController.PickAction(placeLevel, component, liftLevel, PickTypeEnum.BMC);

            // 移动到上视相机中心位置
            // this.calibratePara.GlassUpLookVisionMachinePos.Z = -32.0455;
            this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassUpLookVisionMachinePos);
            MatchResult matchResult1 = this.LocatePosition(this.calibratePara.UpLookPRName);

            this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.UpLook, matchResult1);

            AKRSPoint3D curMachinePos = this.bondModuleController.Get3DRealPosition();

            AKRSPoint3D curUpLookCenter = new AKRSPoint3D { X = this.bondModuleController.GetAxisXRealPos(), Y = this.bondModuleController.GetAxisYRealPos(), Z = this.bondHeadController.GetAxisZRealPos() };

            // 生成上视标定点位列表
            this.upLookCalibPoints = CalibService.GeneratePointList(curUpLookCenter, this.calibratePara.UpLookCalibDistance, 6, 5);

            List<AKRSPoint2D> imagePoints = new List<AKRSPoint2D>();
            int marksNum = this.upLookCalibPoints.Count;
            float[] imageX = new float[marksNum];
            float[] imageY = new float[marksNum];
            float[] worldX = new float[marksNum];
            float[] worldY = new float[marksNum];

            List<AKRSPoint3D> imgPoints = new List<AKRSPoint3D>();
            List<AKRSPoint3D> realPoints = new List<AKRSPoint3D>();

            for (int i = 0; i < this.upLookCalibPoints.Count; i++)
            {
                AKRSPoint3D upLookCalibPoint = new AKRSPoint3D(
                    this.upLookCalibPoints[i].X,
                    this.upLookCalibPoints[i].Y,
                    this.calibratePara.GlassUpLookVisionMachinePos.Z);

                // 轴移动到位
                this.bondModuleController.MoveSafeBondXYZ(upLookCalibPoint);

                Thread.Sleep(1000);

                // MatchResult matchResult = this.LocatePosition("测试模板");
                MatchResult matchResult = this.LocatePosition(this.calibratePara.UpLookPRName);

                imagePoints.Add(new AKRSPoint2D(matchResult.CenterX, matchResult.CenterY));
                imageX[i] = (float)matchResult.CenterX;
                imageY[i] = (float)matchResult.CenterY;
                worldX[i] = (float)(this.upLookCalibPoints[i].X - this.calibratePara.GlassUpLookVisionMachinePos.X);
                worldY[i] = (float)(this.upLookCalibPoints[i].Y - this.calibratePara.GlassUpLookVisionMachinePos.Y);

                imgPoints.Add(new AKRSPoint3D(matchResult.CenterX, matchResult.CenterY, 0));
                realPoints.Add(new AKRSPoint3D(worldX[i], worldY[i], 0));
            }

            // 调用9点标定的接口(Halcon)（发送定位结果和相对坐标）
            List<AKRSPoint2D> physicalPoints = this.upLookCalibPoints.Select(p => new AKRSPoint2D(p.X - curUpLookCenter.X, p.Y - curUpLookCenter.Y)).ToList();

            HTuple homMatTrans;
            HTuple scaleX, scaleY, phi, slant, xTrans, yTrans;

            CalibService.NPointTrans(imagePoints, physicalPoints, out homMatTrans);
            HOperatorSet.HomMat2dToAffinePar(homMatTrans, out scaleX, out scaleY, out phi, out slant, out xTrans, out yTrans);

            this.bondModuleController.MoveSafeBondXYZ(curUpLookCenter);

            this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassUpLookVisionMachinePos);

            int[] angleArray = new int[] { -180, -90, 0, 90, 180 };
            List<AKRSPoint2D> circlePointList = new List<AKRSPoint2D>();
            List<AKRSPoint2D> matchResultList2D = new List<AKRSPoint2D>();

            AKRSPoint2D curMachinePos2D = this.bondModuleController.Get2DRealPosition();

            foreach (int angle in angleArray)
            {
                // T轴旋转一定角度
                this.bondHeadController.RotateAxisT(angle);
                MatchResult matchResult = LocatePosition(this.calibratePara.UpLookPRName);

                matchResultList2D.Add(new AKRSPoint2D(matchResult.CenterX, matchResult.CenterY));

                AKRSPoint2D circlePoint = CalibService.GetMachinePosByPixelPos(
                    curMachinePos2D,
                    matchResult,
                    "UpLookCameraCoordinateSystem");

                circlePointList.Add(circlePoint);
            }
            CalibService.FitCircle(matchResultList2D, out double circleCenterPixelX, out double circleCenterPixelY, out double circleRadiusPixel);
            this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.UpLook, new MatchResult(circleCenterPixelX, circleCenterPixelY, 0));
            AKRSPoint3D curPos = this.bondModuleController.Get3DRealPosition();
            this.calibratePara.GlassUpLookVisionMachinePos = curPos;

            // 拟合圆心
            CalibService.FitCircle(circlePointList, out double circleCenterX, out double circleCenterY, out double circleRadius);

            // 记录旋转中心坐标
            this.calibratePara.GlassUpLookVisionMachinePos.X = curPos.X;
            this.calibratePara.GlassUpLookVisionMachinePos.Y = curPos.Y;

            MatchResult matchResult5 = this.LocatePosition(this.calibratePara.UpLookPRName);

            this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.UpLook, matchResult5);

            AKRSPoint3D curMachinePos1 = this.bondModuleController.Get3DRealPosition();

            AKRSPoint3D bondOffset = curMachinePos1 - this.calibratePara.GlassUpLookVisionMachinePos;

            this.calibratePara.BondRotateCenterToCamOffset.X -= bondOffset.X;
            this.calibratePara.BondRotateCenterToCamOffset.Y -= bondOffset.Y;

            this.calibratePara.Save();

            AKRSPoint3D placePos1 = new AKRSPoint3D(this.calibratePara.GlassVisionMachinePos.X + this.calibratePara.BondRotateCenterToCamOffset.X, this.calibratePara.GlassVisionMachinePos.Y + this.calibratePara.BondRotateCenterToCamOffset.Y, 0);

            this.bondHeadController.MoveBondZToSafePos();

            // 轴移动至放标位放置标定片
            this.bondModuleController.MoveSafeBondXYZ(placePos1);

            this.bondHeadController.RotateAxisT(0);

            // 放标
            pickLevel = this.calibratePara.GlassPickZMachinePos + 5;
            liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

            // 放片
            this.bondHeadController.BondAction(
                pickLevel,
                liftLevel,
                component,
                BondTypeEnum.BondOnTU);

            Thread.Sleep(500);

            this.calibCamScale = CalibService.CreateCalibCamScale("上视相机像素比", Math.Round(scaleX.D, 6), Math.Round(scaleY.D, 6), Math.Round(phi.TupleDeg().D, 6));
            this.CalibCamScales.Add(this.calibCamScale);

            this.calibTransResult = CalibService.CreateCalibTransResult("上视相机坐标转换信息", Math.Round(xTrans.D, 6), Math.Round(yTrans.D, 6), Math.Round(slant.TupleDeg().D, 6));
            this.CalibTransResults.Add(this.calibTransResult);

            this.calibTransResult = CalibService.CreateCalibTransResult("上视相机当前旋转中心", Math.Round(this.calibratePara.UpLookMarkMachinePos.X, 6), Math.Round(this.calibratePara.UpLookMarkMachinePos.Y, 6), Math.Round(this.calibratePara.UpLookMarkMachinePos.Z, 6));
            this.CalibTransResults.Add(this.calibTransResult);

            this.StartUpLookMachinePosTask();

            MainForm.SendUiAction(
                () =>
                {
                    FrmCalibResult frmCalibResult = new FrmCalibResult();
                    frmCalibResult.ShowDialog();
                    frmCalibResult.Dispose();
                });
        }

        /// <summary>
        /// 晶圆相机单步标定
        /// </summary>
        public void OnlyWaferCameraCaib()
        {
            this.CalibCamScales.Clear();
            this.CalibTransResults.Clear();

            // Bond移开到安全位置
            this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCVisionMachinePos);

            // 晶圆台左Mark移动到晶圆相机中心
            // this.waferTableModule.MoveXY(this.calibratePara.WaferTableLeftMarkWCVisionMachinePos);

            // this.waferTableController.MoveWaferTableToMachinePos(this.calibratePara.WaferTableLeftMarkWCVisionMachinePos);
            this.calibController.MoveWaferTableToMachinePos(this.calibratePara.WaferTableLeftMarkWCVisionMachinePos);

            this.calibController.MoveWaferCameraZ(this.calibratePara.WaferTableLeftMarkWCVisionMachinePos.Z);

            // 生成晶圆台标定点位列表
            this.waferTableCalibPoints = CalibService.GeneratePointList(
                this.calibratePara.WaferTableLeftMarkWCVisionMachinePos,
                this.calibratePara.WaferTableCalibDistance,
                5,
                5);

            List<AKRSPoint2D> imagePoints = new List<AKRSPoint2D>();
            int marksNum = this.waferTableCalibPoints.Count;
            float[] imageX = new float[marksNum];
            float[] imageY = new float[marksNum];
            float[] worldX = new float[marksNum];
            float[] worldY = new float[marksNum];

            List<AKRSPoint3D> imgPoints = new List<AKRSPoint3D>();
            List<AKRSPoint3D> realPoints = new List<AKRSPoint3D>();

            for (int i = 0; i < this.waferTableCalibPoints.Count; i++)
            {
                this.calibController.MoveWaferTableToMachinePos(new AKRSPoint3D(this.waferTableCalibPoints[i].X, this.waferTableCalibPoints[i].Y, 0));

                Thread.Sleep(1000);

                // MatchResult matchResult = this.LocatePosition("晶圆相机标定模板");
                MatchResult matchResult = this.LocatePosition(this.calibratePara.WaferPRName);

                imagePoints.Add(new AKRSPoint2D(matchResult.CenterX, matchResult.CenterY));
                imageX[i] = (float)matchResult.CenterX;
                imageY[i] = (float)matchResult.CenterY;
                worldX[i] = (float)(this.waferTableCalibPoints[i].X
                                    - this.calibratePara.WaferTableLeftMarkWCVisionMachinePos.X);
                worldY[i] = (float)(this.waferTableCalibPoints[i].Y
                                    - this.calibratePara.WaferTableLeftMarkWCVisionMachinePos.Y);

                imgPoints.Add(new AKRSPoint3D(matchResult.CenterX, matchResult.CenterY, this.calibratePara.WaferTableLeftMarkWCVisionMachinePos.Z));
                realPoints.Add(new AKRSPoint3D(worldX[i], worldY[i], 0));
            }

            // 调用9点标定的接口(Halcon)（发送定位结果和相对坐标）
            List<AKRSPoint2D> physicalPoints = this.waferTableCalibPoints.Select(
                p => new AKRSPoint2D(
                    p.X - this.calibratePara.WaferTableLeftMarkWCVisionMachinePos.X,
                    p.Y - this.calibratePara.WaferTableLeftMarkWCVisionMachinePos.Y)).ToList();

            HTuple homMatTrans;
            HTuple scaleX, scaleY, phi, slant, xTrans, yTrans;
            CalibService.NPointTrans(imagePoints, physicalPoints, out homMatTrans);

            HOperatorSet.HomMat2dToAffinePar(
                homMatTrans,
                out scaleX,
                out scaleY,
                out phi,
                out slant,
                out xTrans,
                out yTrans);

            // 回起点
            // this.waferTableModule.MoveXY(this.calibratePara.WaferTableLeftMarkWCVisionMachinePos);
            // this.waferTableController.MoveWaferTableToMachinePos(this.calibratePara.WaferTableLeftMarkWCVisionMachinePos);
            this.calibController.MoveWaferTableToMachinePos(this.calibratePara.WaferTableLeftMarkWCVisionMachinePos);

            this.calibCamScale = CalibService.CreateCalibCamScale(
                "晶圆相机像素比",
                Math.Round(scaleX.D, 6),
                Math.Round(scaleY.D, 6),
                Math.Round(phi.TupleDeg().D, 6));
            this.CalibCamScales.Add(this.calibCamScale);

            this.calibTransResult = CalibService.CreateCalibTransResult(
                "晶圆相机坐标转换信息",
                Math.Round(xTrans.D, 6),
                Math.Round(yTrans.D, 6),
                Math.Round(slant.TupleDeg().D, 6));
            this.CalibTransResults.Add(this.calibTransResult);


            MainForm.SendUiAction(
                () =>
                {
                    FrmCalibResult frmCalibResult = new FrmCalibResult();
                    frmCalibResult.ShowDialog();
                    frmCalibResult.Dispose();
                });
        }

        /// <summary>
        /// Bond到相机偏移单步标定
        /// </summary>
        public void OnlyBondToCameraOffsetCaib()
        {
            this.CalibCamScales.Clear();
            this.CalibTransResults.Clear();

            this.StartBondToCamCalibTask1();

            // 结果赋值
            this.calibTransResult = CalibService.CreateCalibTransResult("邦头到邦头相机偏移", Math.Round(this.calibratePara.BondRotateCenterToCamOffset.X, 6), Math.Round(this.calibratePara.BondRotateCenterToCamOffset.Y, 6), 0);
            this.CalibTransResults.Add(this.calibTransResult);

            FrmCalibResult frmCalibResult = new FrmCalibResult();
            frmCalibResult.ShowDialog();
            frmCalibResult.Dispose();

        }

        public bool StartBondToCamCalibTask1()
        {
            try
            {
                // this.bondHeadController.OpenBondHeadVaccum();

                this.bondHeadController.RotateAxisT(0);

                // Bond移动到小标定片中心点拍照位置
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassVisionMachinePos);

                // 定位小圆并移动至相机中心
                MatchResult smallMatchResult = this.LocatePosition("小标定片模板");

                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, smallMatchResult);

                this.calibController.LocateAndMoveToTargetPixel(this.calibratePara.GlassPRName, CalibController.CamCoordinateType.Bond, 1, 2);

                // 更新小圆中心位置
                this.calibratePara.GlassVisionMachinePos = this.bondModuleController.Get3DRealPosition();

                AKRSPoint3D targetPos = new AKRSPoint3D()
                {
                    X = this.calibratePara.GlassVisionMachinePos.X
                                                    + this.calibratePara.BondRotateCenterToCamOffset.X,
                    Y = this.calibratePara.GlassVisionMachinePos.Y
                                                    + this.calibratePara.BondRotateCenterToCamOffset.Y,
                    Z = this.calibratePara.GlassVisionMachinePos.Z
                };

                // 移动到取标位置
                this.bondModuleController.MoveSafeBondXYZ(targetPos);

                this.calibratePara.GlassPickZMachinePos = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult - 3.0;

                // 取放片参数
                BaseCarrierConfig component = new BaseCarrierConfig()
                {
                    // 取片
                    IsActivateSlowTravelBeforePickup = true,
                    SlowTravelSpeedBeforePickup =
                                                          this.bMCDevicePara.SlowTravelSpeedBeforePickup,
                    SlowTravelDistanceBeforePickup =
                                                          this.bMCDevicePara.SlowTravelDistanceBeforePickup,
                    IsActivateSlowTravelAfterPickup = true,
                    SlowTravelSpeedAfterPickup =
                                                          this.bMCDevicePara.SlowTravelSpeedAfterPickup,
                    SlowTravelDistanceAfterPickup =
                                                          this.bMCDevicePara.SlowTravelDistanceAfterPickup,
                    VacuumOffDelay = this.bMCDevicePara.VacuumOffDelay,
                    PickupDelay = this.bMCDevicePara.PickupDelay,

                    // todo:力控暂时没接
                    PickupForceMode = ForceModeEnum.Distance,

                    // 放片
                    IsActivateSlowTravelBeforeBonding = true,
                    SlowTravelSpeedBeforeBonding =
                                                          this.bMCDevicePara.SlowTravelSpeedBeforeBonding,
                    SlowTravelDistanceBeforeBonding =
                                                          this.bMCDevicePara.SlowTravelDistanceBeforeBonding,
                    IsActivateSlowTravelAfterBonding = true,
                    SlowTravelSpeedAfterBonding =
                                                          this.bMCDevicePara.SlowTravelSpeedAfterBonding,
                    SlowTravelDistanceAfterBonding =
                                                          this.bMCDevicePara.SlowTravelDistanceAfterBonding,
                    BondingBlowDelay = this.bMCDevicePara.BondingBlowDelay,
                    PlacementDelay = this.bMCDevicePara.PlacementDelay,
                    IsActiveComponentDetection = true,
                    BondingForceMode = ForceModeEnum.Distance,

                    WeakBlowProportion = 1000,
                };

                double pickLevel = this.calibratePara.GlassPickZMachinePos;
                double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

                // 取片
                this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.CarrierWithWaffle);

                Thread.Sleep(100);

                this.calibratePara.PlaceToBMCMachinePos = new AKRSPoint3D(this.calibratePara.BMCVisionMachinePos.X, this.calibratePara.BMCVisionMachinePos.Y, BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.7);

                AKRSPoint3D targetPos2 = new AKRSPoint3D(
                    this.calibratePara.PlaceToBMCMachinePos.X + this.calibratePara.BondRotateCenterToCamOffset.X,
                    this.calibratePara.PlaceToBMCMachinePos.Y + this.calibratePara.BondRotateCenterToCamOffset.Y,
                    this.calibratePara.PlaceToBMCMachinePos.Z);

                // 移至放标位(BMC左下角位置，高度暂定)
                this.bondModuleController.MoveSafeBondXY(targetPos2.X, targetPos2.Y);

                // 放片参数,标定片厚度：2
                double placeLevel = this.calibratePara.PlaceToBMCMachinePos.Z;
                //liftLevel = CalibrateRunPara.GetInstance().BMCVisionMachinePos.Z;

                // 放片
                bool ret = this.bondHeadController.BondAction(
                    placeLevel,
                    liftLevel,
                    component,
                    BondTypeEnum.BondOnBMC);

                Thread.Sleep(500);

                // 更新当前小标定片中心位置
                this.calibratePara.GlassCenterCurVisionMachinePos = new AKRSPoint3D { X = this.calibratePara.PlaceToBMCMachinePos.X, Y = this.calibratePara.PlaceToBMCMachinePos.Y, Z = -8.2244 };

                // Bond相机移动定位透明片Mark左上点
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassMarkTopLeftCurVisionPos);

                this.calibController.AutoFocus(CalibController.CamCoordinateType.Bond, this.calibratePara.BMCVisionMachinePos.Z + 2, this.calibratePara.BMCVisionMachinePos.Z - 3);

                double z = this.bondModuleController.Get3DRealPosition().Z;

                this.calibratePara.GlassCenterCurVisionMachinePos.Z = z;

                MatchResult topLeftMatchResult = this.LocatePosition(this.calibratePara.GlassPRName);

                // Bond相机移动,Mark点到相机中心
                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, topLeftMatchResult);

                this.calibController.LocateAndMoveToTargetPixel(this.calibratePara.GlassPRName, CalibController.CamCoordinateType.Bond, 1, 2);

                // 更新此时左上Mark点坐标
                AKRSPoint3D glassMarkTopLeftAbsPos = this.bondModuleController.Get3DRealPosition();


                // Bond相机移动定位透明片Mark右下点
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassMarkBotRightCurVisionPos);

                MatchResult botRightMatchResult = this.LocatePosition(this.calibratePara.GlassPRName);

                // Bond相机移动,Mark点到相机中心
                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, botRightMatchResult);

                this.calibController.LocateAndMoveToTargetPixel(this.calibratePara.GlassPRName, CalibController.CamCoordinateType.Bond, 1, 2);

                // 更新此时右下Mark点坐标
                AKRSPoint3D glassMarkBotRightAbsPos = this.bondModuleController.Get3DRealPosition();


                this.calibratePara.GlassCenterCurVisionMachinePos.X = (glassMarkTopLeftAbsPos.X + glassMarkBotRightAbsPos.X) / 2;
                this.calibratePara.GlassCenterCurVisionMachinePos.Y = (glassMarkTopLeftAbsPos.Y + glassMarkBotRightAbsPos.Y) / 2;

                // 更新取料(放料)位置
                this.calibratePara.PlaceToBMCMachinePos.X = this.calibratePara.GlassCenterCurVisionMachinePos.X + this.calibratePara.BondRotateCenterToCamOffset.X;
                this.calibratePara.PlaceToBMCMachinePos.Y = this.calibratePara.GlassCenterCurVisionMachinePos.Y + this.calibratePara.BondRotateCenterToCamOffset.Y;
                this.calibratePara.PlaceToBMCMachinePos.Z = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.8;

                #region 重复标定旋转中心

                 int count = 0;
                List<(double, double)> rotateList = new List<(double, double)>();
            retry:
                List<AKRSPoint2D> centerList = new List<AKRSPoint2D>();
                this.bondHeadController.RotateAxisT(0);

                int[] rotateAngle = new int[] { 0, -180 };

                for (int i = 0; i < 2; i++)
                {
                    centerList.Clear();
                    foreach (var angle in rotateAngle)
                    {
                        // 移动到取料位
                        this.bondModuleController.MoveSafeBondXY(this.calibratePara.PlaceToBMCMachinePos.X, this.calibratePara.PlaceToBMCMachinePos.Y);

                        // 计算参数
                        pickLevel = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.8;

                        // 取片
                        this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.BMC);

                        // Z轴升起
                        this.bondHeadController.MoveBondZToSafePos();

                        // 焊头旋转
                        this.bondHeadController.RotateAxisT(angle);

                        // 放标
                        ret = this.bondHeadController.BondAction(
                            placeLevel,
                            liftLevel,
                            component,
                            BondTypeEnum.BondOnBMC);

                        Thread.Sleep(800);

                        // Bond相机移动定位透明片Mark左上点
                        this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassMarkTopLeftCurVisionPos);

                        MatchResult topLeftMatchResult1 = this.LocatePosition(this.calibratePara.GlassPRName);

                        // Bond相机移动,Mark点到相机中心
                        this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, topLeftMatchResult1);

                        this.calibController.LocateAndMoveToTargetPixel(this.calibratePara.GlassPRName, CalibController.CamCoordinateType.Bond, 1, 2);

                        // 更新此时左上Mark点坐标
                        glassMarkTopLeftAbsPos = this.bondModuleController.Get3DRealPosition();


                        // Bond相机移动定位透明片Mark右下点
                        this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassMarkBotRightCurVisionPos);

                        MatchResult botRightMatchResult1 = this.LocatePosition(this.calibratePara.GlassPRName);

                        // Bond相机移动,Mark点到相机中心
                        this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, botRightMatchResult1);

                        this.calibController.LocateAndMoveToTargetPixel(this.calibratePara.GlassPRName, CalibController.CamCoordinateType.Bond, 1, 2);

                        // 更新此时右下Mark点坐标
                        glassMarkBotRightAbsPos = this.bondModuleController.Get3DRealPosition();

                        double glassCenterX = (glassMarkTopLeftAbsPos.X + glassMarkBotRightAbsPos.X) / 2;
                        double glassCenterY = (glassMarkTopLeftAbsPos.Y + glassMarkBotRightAbsPos.Y) / 2;

                        this.calibratePara.PlaceToBMCMachinePos.X = glassCenterX + this.calibratePara.BondRotateCenterToCamOffset.X;
                        this.calibratePara.PlaceToBMCMachinePos.Y = glassCenterY + this.calibratePara.BondRotateCenterToCamOffset.Y;

                        centerList.Add(new AKRSPoint2D(glassCenterX, glassCenterY));
                    }

                    // double rotateCenterX = (centerList[0].X + centerList[1].X) / 2;
                    // double rotateCenterY = (centerList[0].Y + centerList[1].Y) / 2;

                    double rotateCenterX = (centerList[1].X - centerList[0].X) / 2;
                    double rotateCenterY = (centerList[1].Y - centerList[0].Y) / 2;

                    rotateList.Add((rotateCenterX, rotateCenterY));

                    this.calibratePara.BondRotateCenterToCamOffset.X -= rotateCenterX;
                    this.calibratePara.BondRotateCenterToCamOffset.Y -= rotateCenterY;
                }

                if (count == 2)
                {
                    goto retry1;
                }

                foreach (var rotateCenter in rotateList)
                {
                    if (Math.Abs(rotateCenter.Item1) > 0.003 || Math.Abs(rotateCenter.Item2) >0.003)
                    {
                        count++;
                        rotateList.Clear();
                        goto retry;
                    }
                }
            // 更新bond到相机距离
            // this.calibratePara.BondRotateCenterToCamOffset.X += rotateCenterX - this.calibratePara.GlassCenterCurVisionMachinePos.X;
            // this.calibratePara.BondRotateCenterToCamOffset.Y += rotateCenterY - this.calibratePara.GlassCenterCurVisionMachinePos.Y;


            //if (Math.Abs(rotateCenterX) < 0.003 && Math.Abs(rotateCenterY) < 0.003 && count <5)
            //{
            //    count++;

            //    if (count >= 5 )
            //    {

            //    }
            //    goto retry;
            //}
            retry1:
                this.calibratePara.GlassCenterCurVisionMachinePos.X = centerList[1].X;
                this.calibratePara.GlassCenterCurVisionMachinePos.Y = centerList[1].Y;

                #endregion

                //// 转G0坐标
                AKRSPoint3D upPoint = new AKRSPoint3D { X = 200, Y = -300, Z = 100 };

                // BMC 中心点
                AKRSPoint3D downPoint = new AKRSPoint3D { X = this.calibratePara.BMCVisionMachinePos.X + this.calibratePara.BondRotateCenterToCamOffset.X, Y = this.calibratePara.BMCVisionMachinePos.Y + this.calibratePara.BondRotateCenterToCamOffset.Y, Z = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult };

                MachineCoordinateSystem.GetInstance().CreateCoordinateSystem("BondCoordinateSystem", "G0", true, CoordinateSystemTypeEnum.General);

                GeneralCoordinateSystem aCoordinateSystem = (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name == "BondCoordinateSystem");

                aCoordinateSystem.Init(upPoint, downPoint, 0);

                MachineCoordinateSystem.Save();
                MachineCoordinateSystem.Refresh();

                int step = 0;
                while (true)
                {
                    #region 校正偏移
                    int[] rotateAngle2 = new int[] { 0, -180 };
                    double angleOffset = 0;
                    AKRSPoint2D centerOffset = new AKRSPoint2D();
                    AKRSPoint2D glassAbsCenter = new AKRSPoint2D();

                    List<AKRSPoint2D> centerOffsetList = new List<AKRSPoint2D>();

                    foreach (var angle in rotateAngle2)
                    {
                        AKRSPoint3D targetPos3 = new AKRSPoint3D(
                            this.calibratePara.GlassCenterCurVisionMachinePos.X + this.calibratePara.BondRotateCenterToCamOffset.X,
                            this.calibratePara.GlassCenterCurVisionMachinePos.Y + this.calibratePara.BondRotateCenterToCamOffset.Y,
                            this.calibratePara.PlaceToBMCMachinePos.Z);

                        // 移至放标位
                        this.bondModuleController.MoveSafeBondXY(targetPos3.X, targetPos3.Y);

                        // 取标
                        this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.BMC);

                        if (angle == 0)
                        {
                            this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCMarkTopLeftVisionMachinePos);
                            MatchResult bmcLeftMatchResult = this.LocatePosition(this.calibratePara.BmcPRName);
                            AKRSPoint2D bmcLeftAbs = CalibService.GetMachinePosByPixelPos(new AKRSPoint2D(this.calibratePara.BMCMarkTopLeftVisionMachinePos.X, this.calibratePara.BMCMarkTopLeftVisionMachinePos.Y),
                                bmcLeftMatchResult, "BondCameraCoordinateSystem");

                            this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCMarkBotRightVisionMachinePos);
                            MatchResult bmcRightMatchResult = this.LocatePosition(this.calibratePara.BmcPRName);
                            AKRSPoint2D bmcRightAbs = CalibService.GetMachinePosByPixelPos(new AKRSPoint2D(this.calibratePara.BMCMarkBotRightVisionMachinePos.X, this.calibratePara.BMCMarkBotRightVisionMachinePos.Y),
                            bmcRightMatchResult, "BondCameraCoordinateSystem");

                            double bmcAbsAngle = CalibService.CalculateAngle(bmcLeftAbs, bmcRightAbs);
                            double glassAbsAngle = CalibService.CalculateAngle(new AKRSPoint2D(glassMarkTopLeftAbsPos.X, glassMarkTopLeftAbsPos.Y), new AKRSPoint2D(glassMarkBotRightAbsPos.X, glassMarkBotRightAbsPos.Y));

                            angleOffset = glassAbsAngle - bmcAbsAngle;
                            //this.bondHeadController.RotateAxisT(angle - angleOffset);
                        }
                        this.bondHeadController.RotateAxisT(angle - angleOffset);

                        {
                            AKRSPoint3D targetPos4 = new AKRSPoint3D(this.calibratePara.BMCVisionMachinePos.X + this.calibratePara.BondRotateCenterToCamOffset.X, this.calibratePara.BMCVisionMachinePos.Y + this.calibratePara.BondRotateCenterToCamOffset.Y, this.calibratePara.BMCVisionMachinePos.Z);

                            // 移动到BMC中点位置
                            this.bondModuleController.MoveSafeBondXYZ(targetPos4);

                            // 放标
                            ret = this.bondHeadController.BondAction(
                                placeLevel,
                                liftLevel,
                                component,
                                BondTypeEnum.BondOnBMC);

                            Thread.Sleep(100);
                        }

                        this.calibratePara.BMCMarkTopLeftVisionMachinePos.Z = this.calibratePara.GlassCenterCurVisionMachinePos.Z;

                        this.calibratePara.BMCMarkBotRightVisionMachinePos.Z = this.calibratePara.GlassCenterCurVisionMachinePos.Z;

                        // Bond相机移动定位BMC左上点Mark
                        this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCMarkTopLeftVisionMachinePos);
                        this.bondHeadController.MoveAxisZ(this.calibratePara.GlassCenterCurVisionMachinePos.Z);

                        MatchResult glassTopLeftMatchResult = this.LocatePosition(this.calibratePara.GlassPRName);

                        MatchResult bMCTopLeftMatchResult = this.LocatePosition(this.calibratePara.BmcPRName);

                        AKRSPoint2D glassTopAbs = CalibService.GetMachinePosByPixelPos(new AKRSPoint2D(this.calibratePara.BMCMarkTopLeftVisionMachinePos.X, this.calibratePara.BMCMarkTopLeftVisionMachinePos.Y), glassTopLeftMatchResult, "BondCameraCoordinateSystem");

                        AKRSPoint2D bMCTopAbs = CalibService.GetMachinePosByPixelPos(new AKRSPoint2D(this.calibratePara.BMCMarkTopLeftVisionMachinePos.X, this.calibratePara.BMCMarkTopLeftVisionMachinePos.Y), bMCTopLeftMatchResult, "BondCameraCoordinateSystem");

                        // Bond相机移动定位BMC右下点Mark
                        this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCMarkBotRightVisionMachinePos);
                        this.bondHeadController.MoveAxisZ(this.calibratePara.GlassCenterCurVisionMachinePos.Z);

                        MatchResult glassBotRightMatchResult = this.LocatePosition(this.calibratePara.GlassPRName);

                        MatchResult bMCBotRightMatchResult = this.LocatePosition(this.calibratePara.BmcPRName);

                        AKRSPoint2D glassBotAbs = CalibService.GetMachinePosByPixelPos(new AKRSPoint2D(this.calibratePara.BMCMarkBotRightVisionMachinePos.X, this.calibratePara.BMCMarkBotRightVisionMachinePos.Y), glassBotRightMatchResult, "BondCameraCoordinateSystem");

                        AKRSPoint2D bMCBotAbs = CalibService.GetMachinePosByPixelPos(new AKRSPoint2D(this.calibratePara.BMCMarkBotRightVisionMachinePos.X, this.calibratePara.BMCMarkBotRightVisionMachinePos.Y), bMCBotRightMatchResult, "BondCameraCoordinateSystem");

                        glassAbsCenter = new AKRSPoint2D((glassTopAbs.X + glassBotAbs.X) / 2, (glassTopAbs.Y + glassBotAbs.Y) / 2);
                        AKRSPoint2D bMCAbsCenter = new AKRSPoint2D((bMCTopAbs.X + bMCBotAbs.X) / 2, (bMCTopAbs.Y + bMCBotAbs.Y) / 2);

                        //double glassAngle = CalibService.CalculateAngle(glassTopAbs, glassBotAbs);
                        //double bmcAngle = CalibService.CalculateAngle(bMCTopAbs, bMCBotAbs);

                        //angleOffset = glassAngle - bmcAngle;

                        centerOffset.X = glassAbsCenter.X - bMCAbsCenter.X;
                        centerOffset.Y = glassAbsCenter.Y - bMCAbsCenter.Y;

                        AKRSPoint2D centerOffsetTemp = new AKRSPoint2D(
                            glassAbsCenter.X - bMCAbsCenter.X,
                            glassAbsCenter.Y - bMCAbsCenter.Y);

                        centerOffsetList.Add(centerOffsetTemp);

                        this.calibratePara.GlassCenterCurVisionMachinePos.X = glassAbsCenter.X;
                        this.calibratePara.GlassCenterCurVisionMachinePos.Y = glassAbsCenter.Y;
                    }

                    if (centerOffsetList[0].X <= 0.004 && centerOffsetList[0].Y <= 0.004 && centerOffsetList[1].X <= 0.004 && centerOffsetList[1].Y <= 0.004)
                    {
                        break;
                    }

                    if (step > 2)
                    {
                        //this.calibratePara.BondRotateCenterToCamOffset -= new AKRSPoint3D(
                        //    (centerOffsetList[0].X + centerOffsetList[1].X) / 2,
                        //    (centerOffsetList[0].Y + centerOffsetList[1].Y) / 2,
                        //    0);
                    }

                    if (step == 0)
                    {
                        break;
                    }

                    step++;
                    #endregion
                }

                AKRSPoint3D targetPos5 = new AKRSPoint3D(this.calibratePara.GlassCenterCurVisionMachinePos.X
                    + this.calibratePara.BondRotateCenterToCamOffset.X, this.calibratePara.GlassCenterCurVisionMachinePos.Y + this.calibratePara.BondRotateCenterToCamOffset.Y, this.calibratePara.PlaceToBMCMachinePos.Z);

                // 移至放标位
                this.bondModuleController.MoveSafeBondXY(targetPos5.X, targetPos5.Y);

                // 取标
                this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.BMC);


                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return false;
            }
        }

        /// <summary>
        /// 一键自动标定
        /// </summary>
        public void StartAutoCalib()
        {
            this.CalibCamScales.Clear();
            this.CalibTransResults.Clear();

            DialogResult dialog = AKRSXtraMessageBox.Show(
                "System2: \n" + "检查以下项: \r\n+" +               
                                     "1.清理bmc表面.\r\n" +
                                     "2.清理玻璃片表面.\r\n" +
                                     "3.清理所有标记点表面.\r\n",
                "Prompt",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information);

            if (dialog == DialogResult.OK)
            {
                this.bondHeadController.MoveBondZToSafePos();

                this.bondHeadController.CloseBondHeadVaccum();

                DialogResult dialog1 = AKRSXtraMessageBox.Show(
                    "System2: \n" + " 将 Touchdown 吸嘴放置在焊头上.\r\n",
                    "Prompt",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information);

                if (dialog1 == DialogResult.OK)
                {
                    if (!this.StartBondCalibTask())
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                this.bondHeadController.MoveBondZToSafePos();

                this.bondHeadController.CloseBondHeadVaccum();

                DialogResult dialog2 = AKRSXtraMessageBox.Show(
                    "System2: \n"
                    + "请移除Touchdown吸嘴并更换BMC吸嘴.\r\n",
                    "Prompt",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information);
                if (dialog2 == DialogResult.OK)
                {
                    if (!this.StartBondToCamCalibTask1())
                    {
                        return;
                    }

                    if (!this.StartUpLookCalibTask())
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BondCamUpLookCamMachinePos);

                DialogResult dialog4 = AKRSXtraMessageBox.Show(
                    "System2: \n"
                    + "将标定片放置于Bond相机和上视相机之间，两相机中心对准标定片中心。（此步可选择Cancel跳过）.\r\n",
                    "Prompt",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information);
                if (dialog4 == DialogResult.OK)
                {
                    if (!this.StartBondToCamBySameMarkTask())
                    {
                        return;
                    }
                }


                if (!this.StartUpLookMachinePosTask())
                {
                    return;
                }

                this.bondHeadController.MoveBondZToSafePos();
                DialogResult dialog3 = AKRSXtraMessageBox.Show(
                    "System2: \n"
                    + "请移除Touchdown吸嘴并更换BMC吸嘴.\r\n",
                    "Prompt",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information);

                if (dialog3 == DialogResult.OK)
                {
                    if (!this.StartWaferTableCalibTask())
                    {
                        return;
                    }

                    if (!this.StartWaferNPointCalibTask())
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }

            this.calibratePara.Save();

            MainForm.SendUiAction(
                () =>
                {
                    FrmCalibResult frmCalibResult = new FrmCalibResult();
                    frmCalibResult.ShowDialog();
                    frmCalibResult.Dispose();
                });

         
        }
    }
}
