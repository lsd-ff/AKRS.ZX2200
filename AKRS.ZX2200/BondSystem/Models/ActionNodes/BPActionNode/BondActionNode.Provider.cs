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
using AKRS.ZX2200.BondSystem.Models.ActionNodes.Commons;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Utils;
using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.SupportFeature.Compensate.DefectCompensate;
using AKRS.ZX2200.SupportFeature.Compensate.TemperatureCompensate;
using AKRS.ZX2200.SupportFeature.Statistics;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using DevExpress.XtraEditors;
using log4net.Core;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.BPActionNode
{
    using AKRS.ZX2200.BondSystem.BondForce.Services;
    using AKRS.ZX2200.BondSystem.Controls.Manual;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.SupportFeature.Compensate.MotionAreaCompensate;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using DevExpress.DashboardCommon.Native;
    using System.Linq;

    /// <summary>
    ///  BondActionNode帮助类
    /// </summary>
    public partial class BondActionNode
    {
        /// <summary>
        /// 取片前的准备动作
        /// </summary>
        private void PrepareBeforeBondAction()
        {
            // 获取当前芯片
            this.component = System2Domain.GetInstance().ActionNodesService.GetCurrentComponent().component;

            // 获取当前吸嘴
            this.nozzle = this.bondHeadController.GetCurrentNozzle();

            // 获取当前焊点
            this.bondPosition = this.system2Domain.ActionNodesService.GetCurrentBondPosition();

            // 固精前的速度
            speed = this.bondHeadController.GetAxisZAbsoluteSpeed();
        }

        /// <summary>
        /// 看点的胶印
        /// </summary>
        /// <param name="component">芯片</param>
        /// <param name="bondPos">贴片位</param>
        /// <returns>结果</returns>
        private ExcuteResult DipFluxOnBondPositionVision(BaseCarrierConfig component, AKRSPoint4D bondPos)
        {
            System2RunTimeProvider.RecordTime("Bond", "准备去看焊点的蘸胶印子");

            if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured == false)
            {
                throw new Exception("未配置刮胶盘硬件！");
            }

            if (component.DipVisionPos.IsEmpty)
            {
                throw new Exception("未视角胶印模板！");
            }

            // 拍照位(相对于焊点)
            AKRSPoint3D visionPos =
                bondPosition.CoordinateSystem.SelfPosToG0(new AKRSPoint3D()) + component.DipVisionPos;

            try
            {
                // 去拍照位拍照
                List<BaseAlgResult> baseAlgResults = this.system2Controller.BondCameraVisionDefect(
                    visionPos,
                    component.DipPRName, false, true);

                // 蘸胶结果处理
                ExcuteResult excuteResult = this.DipOnTUEpoxyCheck(component, baseAlgResults);

                switch (excuteResult)
                {
                    case ExcuteResult.Success:

                        // 去贴片位
                        bondPos.Z = component.IsActivateSlowTravelBeforeBonding
                                        ? (bondPos.Z + component.SlowTravelDistanceBeforeBonding)
                                        : bondPos.Z;

                        AKRSPoint3D bondPos3D = new AKRSPoint3D() { X = bondPos.X, Y = bondPos.Y, Z = bondPos.Z };

                        // 移动到贴片位
                        this.bondModuleController.MoveSafeBondXYZ(bondPos3D);

                        // 重新贴片
                        excuteResult = this.Bond(BondTypeEnum.BondOnTU);

                        return excuteResult;

                    case ExcuteResult.Fail:
                        bondPosition.SetMatterDisable();
                        return ExcuteResult.Success;

                    case ExcuteResult.Abort:
                        MachineStateModel.GetInstance().MachineState = MachineStateEnum.Stop;
                        return ExcuteResult.Abort;

                    default:
                        throw new Exception("胶量检测未知错误");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"固晶流程：定位胶印失败", ex, LogCategory.Bond);

                return ExcuteResult.Exception;
            }
        }

        /// <summary>
        /// 保存二维补偿数据
        /// </summary>
        /// <param name="offset">补偿</param>
        /// <param name="bpName">焊点</param>
        /// <param name="moduleIndex">基岛</param>
        public void SaveGlobalCalibrationData(AKRSPoint2D offset, string bpName, int moduleIndex)
        {
            try
            {
                // 添加一个工作表
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(new FileInfo(@"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--GlobalCalibrationOffset.xlsx"));

                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                               ? excelPackage.Workbook.Worksheets[0]
                                               : excelPackage.Workbook.Worksheets.Add("DataSheet");

                // 设置列宽
                for (int i = 1; i <= 7; i++)
                {
                    worksheet.Column(i).Width = 15;
                }

                // 添加标题行
                if (worksheet.Dimension == null)
                {
                    worksheet.Cells[1, 1].Value = "时间";

                    worksheet.Cells[1, 2].Value = "基岛";

                    worksheet.Cells[1, 3].Value = "焊点";

                    worksheet.Cells[1, 4].Value = "Offset-X";

                    worksheet.Cells[1, 5].Value = "Offset-Y";
                }

                int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

                worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

                worksheet.Cells[lastUsedRow + 1, 2].Value = moduleIndex;

                worksheet.Cells[lastUsedRow + 1, 3].Value = bpName;

                worksheet.Cells[lastUsedRow + 1, 4].Value = offset.X;

                worksheet.Cells[lastUsedRow + 1, 5].Value =
                    offset.Y;

                excelPackage.Save();
            }
            catch (Exception e)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"文件已被打开，保存二维补偿数据失败!\r\n Retry:关掉文件重新保存\r\nCancel:不保存数据",
                    "报警",
                    MessageBoxButtons.RetryCancel,
                    MessageBoxIcon.Warning);

                if (dialog == DialogResult.Retry)
                {
                    this.SaveGlobalCalibrationData(offset, bpName, moduleIndex);
                }
            }
        }

        /// <summary>
        /// 胶印检测结果处理
        /// </summary>
        /// <param name="component">芯片</param>
        /// <param name="baseAlgResults">定位结果</param>
        /// <returns>结果</returns>
        public ExcuteResult DipOnTUEpoxyCheck(BaseCarrierConfig component, List<BaseAlgResult> baseAlgResults)
        {
        Recheck:
            string alarmMessage = string.Empty;

            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(component.DipPRName);

            if (baseAlgResults == null)
            {
                alarmMessage = this.Name + "：胶量检测未找到识别点";
            }
            else if (baseAlgResults.Count != (int)component.DipOnTUFluxNum)
            {
                alarmMessage = this.Name + "：胶印检测识别到的个数不对";
            }
            else
            {
                for (int i = 0; i < baseAlgResults.Count; i++)
                {
                    BlobResult blobResult = (BlobResult)baseAlgResults[i];
                    MatchResult matchResult = new MatchResult(blobResult.CenterX, blobResult.CenterY, blobResult.Area);

                    // 定位结果和示教位置之间的差值
                    AKRSPoint3D pointResult = System2Module.GetInstance().BondModule.BondCameraCoordinateSystem
                        .ForwardConvertCoordinate(new AKRSPoint3D(matchResult.CenterX, matchResult.CenterY, 0));

                    double offsetX = pointResult.X - component.DipOnTUFluxCenterX[i] - component.FluxDistanceX;

                    double offsetY = pointResult.Y - component.DipOnTUFluxCenterY[i] - -component.FluxDistanceY;

                    //// 面积判断
                    //double limitArea = Math.Abs(blobResult.Area / component.DipOnTUFluxArea[i]);
                    //if (limitArea > component.TolerantFluxMax
                    //    || limitArea < component.TolerantFluxMin)
                    //{
                    //    alarmMessage +=
                    //        $"胶量检测点 {i + 1} 的面积的检测最大阈值为{component.TolerantFluxMax},"
                    //        + $"最小阈值为{component.TolerantFluxMin}\r\n,当前值为{limitArea}\r\n";
                    //    break;
                    //}

                    // X方向判断
                    if (Math.Abs(offsetX) > component.FluxLimitX)
                    {
                        alarmMessage =
                            $"胶量检测点 {i + 1} 的X方向的检测阈值为{component.FluxLimitX}\r\n,当前值为{offsetX}\r\n";
                        break;
                    }

                    // Y方向判断
                    if (Math.Abs(offsetY) > component.FluxLimitY)
                    {
                        alarmMessage +=
                            $"胶量检测点 {i + 1} 的Y方向的检测阈值为{component.FluxLimitY}\r\n,当前值为{offsetY}\r\n";
                        break;
                    }
                }
            }

            if (alarmMessage != string.Empty)
            {
                (DialogResult dialogResult, BaseAlgResult match) result = UcMainSystem.VisionAlarmFunc(
                    pREntity,
                    component.DipPRName + "胶印检测失败\r\n" + "原因：" + alarmMessage,
                    "胶印检测失败");

                if (result.dialogResult == DialogResult.OK)
                {
                    baseAlgResults = pREntity.AlgResults;
                    goto Recheck;
                }
                else if (result.dialogResult == DialogResult.Ignore)
                {
                    return ExcuteResult.Fail;
                }
                else if (result.dialogResult == DialogResult.Abort)
                {
                    return ExcuteResult.Abort;
                }
                else
                {
                    throw new Exception("检测未知错误");
                }
            }

            return ExcuteResult.Success;
        }

        /// <summary>
        ///  检查贴片颗数是否超上限
        /// </summary>
        private void CheckBondedNumber()
        {
            if (System2Configuration.GetInstance().IsActivateBondLimitWaring
                && StatisticsDomain.GetInstance().AllBondedNumber
                > System2Configuration.GetInstance().BondWarningLimit)
            {
                this.bondHeadController.MoveBondZToSafePos();

                Machine.GetInstance().Pause();

                DialogResult dialog = AKRSMessageBoxExt.Show(
                    $"贴片达到上限：{System2Configuration.GetInstance().BondWarningLimit}颗,设备已暂停！请清零后继续工作！",
                    "固晶报警",
                    new string[] { "清零" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);

                if (dialog == DialogResult.OK)
                {
                    StatisticsDomain.GetInstance().AllBondedNumber = 0;
                }
            }
        }

        /// <summary>
        /// 异步开吹气
        /// </summary>
        /// <param name="bondActionParameter">固精参数</param>
        private void BlowAsyn(BondActionParameter bondActionParameter)
        {
            Task.Run(
                () =>
                    {
                        // 打开弱吹气
                        if (bondActionParameter.BlowingDelay != 0)
                        {
                            CommonUtil.SetCurrentThreadName("固晶弱吹线程");

                            System2RunTimeProvider.RecordTime("Bond", $"准备异步吹气{bondActionParameter.BlowingDelay}ms");

                            this.bondHeadController.OpenToolBlowEle(component.WeakBlowProportion);

                            // 弱吹气延迟
                            DelayHelper.Delay(bondActionParameter.BlowingDelay);

                            // 关闭弱吹气
                            this.bondHeadController.CloseToolBlowEle();
                        }
                    });
        }

        /// <summary>
        ///  开始设置力控实时曲线
        /// </summary>
        private void StartSetForceRealTimeCurve()
        {
            if (FrmForceRealTimeCurve.ForceReadTiming == ForceReadTimingEnum.Pick)
            {
                return;
            }

            bool isSmallForce = ForceCalibrationService.JudgeIsSmallForce(component.BondingForce);

            System2RunTimeProvider.BondHeadInitialVal = this.bondHeadController.GetBondForceCurrentVal(isSmallForce);

            // 设置限定线
            UcMainSystem.SetChartControlConstantLine(component.BondingForce);

            // 开始读焊头力
            UcMainSystem.ActiveReadBondForce(true);
        }

        /// <summary>
        ///  停止设置力控实时曲线
        /// </summary>
        private void StopSetForceRealTimeCurve()
        {
            if (FrmForceRealTimeCurve.ForceReadTiming == ForceReadTimingEnum.Pick)
            {
                return;
            }

            // 停止读焊头力
            Task.Run(() =>
                {
                    Thread.Sleep(300);
                    UcMainSystem.ActiveReadBondForce(false);
                });
        }

        /// <summary>
        ///  打印力值数据
        /// </summary>
        private void ExportForceData()
        {
            if (System2Configuration.GetInstance().IsExportComponentForceData == false)
            {
                return;
            }

            if (TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit == null)
            {
                return;
            }

            string allPath = Machine.GetInstance().GetMachineDatePath("芯片取贴片受力表");

            allPath = Path.Combine(
                allPath,
                TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit.Name);

            if (!Directory.Exists(allPath))
            {
                Directory.CreateDirectory(allPath);
            }

            allPath = Path.Combine(
                allPath,
                this.component.Name + DateTime.Now.ToString("yyMMdd") + "贴片过程力值数据" + ".xlsx");

            // 非法字符剔除
            foreach (char rInvalidChar in Path.GetInvalidPathChars())
            {
                if (allPath.Contains(rInvalidChar.ToString()))
                {
                    allPath = allPath.Replace(rInvalidChar.ToString(), string.Empty);
                }
            }

            try
            {
                // 添加一个工作表
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excelPackage = new ExcelPackage(new FileInfo(allPath));

                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                                   ? excelPackage.Workbook.Worksheets[0]
                                                   : excelPackage.Workbook.Worksheets.Add("DataSheet");

                    // 设置列宽
                    for (int i = 1; i <= 10; i++)
                    {
                        worksheet.Column(i).Width = 15;
                    }

                    // 添加标题行
                    if (worksheet.Dimension == null)
                    {
                        worksheet.Cells[1, 1].Value = "时间";

                        worksheet.Cells[1, 2].Value = "芯片";

                        worksheet.Cells[1, 3].Value = "取片力";

                        worksheet.Cells[1, 4].Value = "贴片力";
                    }

                    int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

                    worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

                    worksheet.Cells[lastUsedRow + 1, 2].Value =this.component.Name;

                    worksheet.Cells[lastUsedRow + 1, 3].Value = this.bondPosition.BondPositionInfo.ActualPickForce;

                    worksheet.Cells[lastUsedRow + 1, 4].Value = this.bondPosition.BondPositionInfo.ActualBondForce;

                    excelPackage.Save();
            }
            catch (Exception e)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"文件已被打开，保存取贴片力值数据失败!\r\n Retry:关掉文件重新保存\r\nCancel:不保存数据",
                    "报警",
                    MessageBoxButtons.RetryCancel,
                    MessageBoxIcon.Warning);

                if (dialog == DialogResult.Retry)
                {
                    ExportForceData();
                }
            }
        }

        /// <summary>
        /// 计算固精位置
        /// </summary>
        /// <returns>固精位</returns>
        private AKRSPoint4D CalculateBondPos()
        {
            #region 贴片角度计算

            // 当前焊点的角度
            double bpAngle = this.bondPosition.CoordinateSystem.DegreeInG0() * 180.0 / Math.PI;

            // 角度计算 = 当前角度 + 回正角度 + 下视定位结果 + 焊点的角度 + 贴片角度硬补偿
            double needRotaryAngle = this.bondHeadController.GetAxisTRealPos()
                                     + this.bondPosition.BondPositionInfo.ComponentAngleOffSet + bpAngle
                                     + this.bondPosition.GetRotaryCompensate();

            #endregion

            #region 焊点所在的位置计算

            // 焊点XY位置硬补偿
            AKRSPoint3D bondPosOffset = new AKRSPoint3D(this.bondPosition.GetPositionCompensate().X, this.bondPosition.GetPositionCompensate().Y, 0);

            // 焊点在G0中的位置（硬补偿随角度旋转） 
            AKRSPoint3D bondPosAddOffset = this.bondPosition.CoordinateSystem.SelfPosToG0(bondPosOffset);

            // 焊后补偿
            AKRSPoint3D defectCompensation = DefectCompensate.GetPostBondCompensation(this.bondPosition.Name);

            // 单科焊点的单独补偿
            AKRSPoint3D singleBpCompensation = DefectCompensate.GetBpSeparateOffset(this.bondPosition);
   
            // 真实贴片的位置为焊点的焊点的位置+焊点硬补偿 - 上视的偏移值 + 温漂补偿
            this.realBondPositionInG0 = bondPosAddOffset + defectCompensation + singleBpCompensation;

            #endregion

            #region 芯片位置计算

            AKRSPoint3D point3D = new AKRSPoint3D();

            // 判断纠偏相机类型
            if (component.AccuracyMode == AccuracyModeEnum.Off)
            {
                // 盲贴模式下只能默认芯片和吸嘴重合
                // 盲贴模式吸嘴还需要回到原来的位置
                needRotaryAngle -= component.PickAngleOffset;

                AKRSPoint2D nozzleOffsetRotated = this.bondHeadController.GetNozzleOffset(
                    nozzle.Name,
                    needRotaryAngle - nozzle.AlignAngle);

                realBondPositionInG0.X -= nozzleOffsetRotated.X;

                realBondPositionInG0.Y -= nozzleOffsetRotated.Y;
            }
            else if (component.AdjustCamera == CameraTypeEnum.BondCamera)
            {
                // 中转台模式下芯片和吸嘴重合
                AKRSPoint2D nozzleOffsetRotated = this.bondHeadController.GetNozzleOffset(
                    nozzle.Name,
                    needRotaryAngle - nozzle.AlignAngle);

                realBondPositionInG0.X -= nozzleOffsetRotated.X;

                realBondPositionInG0.Y -= nozzleOffsetRotated.Y;

                point3D = RealTimeCorrection.GetInstance().GetCompensatePoint3D(
                this.bondPosition.BondPositionInfo.IPTVisionPos,
                this.bondPosition.BondPositionInfo.IPTPickPos,
               realBondPositionInG0 - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset,
               realBondPositionInG0);

                this.bondPosition.BondPositionInfo.TpMarkCompensate = point3D;
            }
            else if (component.AdjustCamera == CameraTypeEnum.UpLookCamera)
            {
                // 上视计算芯片和旋转中心的距离(旋转相对距离)
                double angle = needRotaryAngle - this.bondHeadController.GetAxisTRealPos();

                AKRSPoint3D upLookOff = MathHelper.RotateCenter(this.bondPosition.BondPositionInfo.ComponentOffSet, angle / 180.0 * Math.PI);
                upLookOff = new AKRSPoint3D(upLookOff.X, upLookOff.Y, 0);
                realBondPositionInG0 += upLookOff;
            }

            realBondPositionInG0 -= TransportUnitCompensate.GetInstance().GetBondCompensate(
                this.realBondPositionInG0.X,
                this.realBondPositionInG0.Y);

            if (this.component.IsConfirmReferencePoint)
            {
                realBondPositionInG0 += this.component.IPTComponentReference;
            }

            this.bondPosition.BondPositionInfo.RealBondPoint3D = realBondPositionInG0;

            

            // 真实贴片位，换到轴坐标
            AKRSPoint3D realBondPosition = this.bondModuleController.ConvertG0ToMachinePos(realBondPositionInG0) + point3D;
            
            System2RunTimeProvider.RecordTime("BondAction", "真实贴片位转到轴坐标" + $" X：{realBondPosition.X}," + $" Y: {realBondPosition.Y}");

            #endregion

            #region 固精高度

            // 力控模式不用这个硬补偿
            double bondingDistance =
                component.BondingForceMode == ForceModeEnum.Distance ? component.BondingDistance : 0;

            // 计算最终固晶高度=焊点示教高度+当前吸嘴测高补偿+芯片厚度+焊点贴片补偿
            this.bondLevel = this.bondModuleController.ConvertG0ToMachinePos(bondPosAddOffset).Z
                             + this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset + component.ComponentThickness
                             + this.bondPosition.GetPositionCompensate().Z + bondingDistance;

            // 蘸胶安全高度
            double dipSafeLevel =
                this.bondModuleController
                    .ConvertG0ToMachinePos(BondDevicePara.GetInstance().SlideFluxerParam.SlideFluxerRightBottomPos).Z
                + this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset + 3;

            // 计算抬起高度
            liftLevel = component.DipMode == DipModeEnum.AfterAccuracyMode
                            ? dipSafeLevel
                            : this.bondModuleController.GetBondheadSafeLevel();

            double offsetAngle = 0;

            #endregion

            // 固晶位置
            return new AKRSPoint4D()
                       {
                           X = realBondPosition.X, Y = realBondPosition.Y, Z = this.bondLevel, T = needRotaryAngle
                       };
        }

        /// <summary>
        /// 去看已经贴完的BondPosition
        /// </summary>
        private void VisionPreviousBondPosition()
        {
            if (MachineStateModel.GetInstance().IsVisionPreviousBondPositionInSystem2)
            {
                this.bondModuleController.CameraMoveToG0Pos(realBondPositionInG0 + this.bondPosition.SingleBondPositionConfig.BondPosOffset);

                System2Domain.GetInstance().SetVisionBondPositionHardware(this.bondPosition);

                // 弹出视觉窗体
                UcMainSystem.VmVisionShow();
                UcMainSystem.ChangeCameraVision("Bond相机");

                while (MachineStateModel.GetInstance().IsVisionPreviousBondPositionInSystem2)
                {
                    if (MachineStateModel.GetInstance().MachineState == MachineStateEnum.Stop)
                    {
                        MachineStateModel.GetInstance().IsVisionPreviousBondPositionInSystem2 = false;
                        break;
                    }

                    Thread.Sleep(100);
                }

                System2Domain.GetInstance().SaveVisionHardwareParameter(this.bondPosition.SingleBondPositionConfig);
            }
        }

        /// <summary>
        /// 发信号给刮胶盘
        /// </summary>
        private void SendSignalToSlideFluxer()
        {
            if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured)
            {
                if (component.DipMode != DipModeEnum.Off)
                {
                    // 两点定位默认是大芯片，用另一种时序
                    if (this.component.IsTwoPointAdjust)
                    {
                        // 给刮胶盘发允许伸出信号
                        SignalPool.GetInstance().AllowSlideOutSignal.Set();
                    }
                    else
                    {
                        // 给刮胶盘发蘸胶完成信号
                        SignalPool.GetInstance().BondDipFluxFinishSignal.Set();

                        // 给刮胶盘发允许伸出信号
                        SignalPool.GetInstance().AllowSlideOutSignal.Set();
                    }
                }

                string nextComponentName = System2Domain.GetInstance().ActionNodesService.GetNextComponentName();

                // 产品做完
                if (String.IsNullOrEmpty(nextComponentName))  
                {
                    System2RunTimeProvider.IsNeedDip = false;
                    return;
                }

                BaseCarrierConfig nextComponent = (BaseCarrierConfig)CarrierConfigRepository.GetInstance().Find(nextComponentName);

                System2RunTimeProvider.IsNeedDip = nextComponent.DipMode != DipModeEnum.Off;
            }
        }
    }
}
