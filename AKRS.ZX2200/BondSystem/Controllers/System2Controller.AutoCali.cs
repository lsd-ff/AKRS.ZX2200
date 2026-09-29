using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Service;
using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
using DataAnalysis.Acquisition;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.CalibSystem.Test;
using AKRS.ZX2200.Infrastructure.Controls.Common;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.SupportFeature.Compensate.TemperatureCompensate;
using AKRS.ZX2200.SupportFeature.Statistics;
using AKRS.ZX2200.TransportSystem.Models;
using DevExpress.XtraEditors;

using PostSharp.Aspects.Advices;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.SupportFeature.Calibrate;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
using AKRS.ZX2200.CalibSystem.Services;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
using Accord.IO;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;

namespace AKRS.ZX2200.BondSystem.Controllers
{
    using AKRS.ZX2200.Infrastructure.Models.Enums;

    /// <summary>
    ///  用于存放系统2三点一点自动校准会用到的方法
    /// </summary>
    public partial class System2Controller
    {
        /// <summary>
        /// 吸嘴校准前检查
        /// </summary>
        /// <param name="nozzleName">吸嘴名</param>
        /// <returns>结果</returns>
        public bool PrepareBeforceNozzleAutoCali(string nozzleName)
        {
            Nozzle nozzle = NozzleRepository.GetInstance().GetNozzle(nozzleName);

            if (nozzle == null)
            {
                AKRSXtraMessageBox.Show("选择的吸嘴为空，请重新选择！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (nozzle.IsAssistantSucceed == false)
            {
                AKRSXtraMessageBox.Show($"吸嘴：{nozzle.Name} 自动校准前请先完成示教！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (AKRSXtraMessageBox.Show("是否需要重新做吸嘴模板？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                == DialogResult.Yes)
            {
                FrmNozzleCaliTeach frmNozzleCaliTeach = new FrmNozzleCaliTeach(nozzle);
                if (frmNozzleCaliTeach.IsShowDialog())
                {
                    DialogResult dialog = frmNozzleCaliTeach.ShowDialog();

                    if (dialog == DialogResult.Cancel)
                    {
                        return false;
                    }
                    else
                    {
                        if (AKRSXtraMessageBox.Show("是否开始校正吸嘴？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                            != DialogResult.Yes)
                        {
                            System2Domain.GetInstance().BondModuleController.MoveToSafePos();
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }

            // 寻找Pr模板
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(nozzle.NozzlePRName);

            if (pREntity == null)
            {
                AKRSXtraMessageBox.Show($"吸嘴：{nozzle.Name} 自动校准前请先编辑吸嘴PR！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (nozzle.NozzleToUplookCenterPos.IsEmpty)
            {
                AKRSXtraMessageBox.Show($"吸嘴：{nozzle.Name} 自动校准前请先完成示教！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 校准顶针前准备
        /// </summary>
        public bool PrepareBeforeEjectionAutoCali(string ejectionName)
        {
            #region 检查

            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsUseWaferCameraAssistantEjectCenter == false
                && MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated)
            {
                throw new Exception("FC硬件配置下不能使用Bond相机示教顶针！");
            }

            EjectionBankSlotConfig ejectionBankSlotConfig = WaferSystemProgram.GetInstance()
                .GetDistinctEjectionBankSlotConfig().Find(it => it.EjectionConfig.Name == ejectionName);

            if (ejectionBankSlotConfig == null)
            {
                AKRSXtraMessageBox.Show("选择的顶针为空，请重新选择！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (ejectionBankSlotConfig.EjectionConfig.IsAssistantSucceed == false)
            {
                AKRSXtraMessageBox.Show($"顶针：{ejectionBankSlotConfig.EjectionConfig.Name} 自动校准前请先完成示教！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            // 寻找Pr模板
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(ejectionBankSlotConfig.EjectionConfig.EjectMatchName);

            if (pREntity == null)
            {
                throw new Exception($"顶针：{ejectionBankSlotConfig.EjectionConfig.Name} 未找到PR！");
            }

            if (ejectionBankSlotConfig.EjectionConfig.EjectionCaliVsionPos.IsEmpty)
            {
                AKRSXtraMessageBox.Show($"顶针：{ejectionBankSlotConfig.EjectionConfig.Name} 自动校准前请先完成示教！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return false;
            }

            #endregion

            #region 准备动作

            // 归还吸嘴（防撞）
            bool putBackRes = this.PutbackNozzleAssitance();

            if (putBackRes == false)
            {
                return false;
            }

            WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();

            // 确保顶针在安全状态
            WaferSubController.GetInstance().EjectController.ReturnEjection();
            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToReadyPosition();

            // 判断是否有料片
            WaferSubController.GetInstance().WaferTableController.JudgeTabletOnWaferTable(false);

            // 顶针座升起
            WaferSubController.GetInstance().EjectController.MoveEjectionTableToWorkPosition();

            #endregion

            return true;
        }

        /// <summary>
        /// 校准顶针前准备
        /// </summary>
        public void PrepareBeforeCaliAllEjection()
        {
            #region 检查

            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsUseWaferCameraAssistantEjectCenter == false
                && MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated)
            {
                throw new Exception("FC硬件配置下不能使用Bond相机示教顶针！");
            }

            EjectionBankConfig currentBank = EjectionBankConfigRepository.GetInstance().BaseDsSettingList.Find(
                item => item.Name == WaferSystemDomain.GetInstance().WaferSystemProgram.EjectionBankProgram.Name);

            for (int i = 0; i < currentBank.EjectionBankSlots.Length; i++)
            {
                if (WaferSystemProgram.GetInstance().EjectionBankProgram.EjectionBankEntity
                        .EjectionBankSlotEntities[i].SlotState != EjectSlotStatuEnum.Empty)
                {
                    EjectionBankSlotConfig currentBankSlotConfig =
                        currentBank.EjectionBankSlots[i];

                    if (currentBankSlotConfig.EjectionConfig.EjectionPRTeach.State != AssistantStateEnum.Able)
                    {
                        throw new Exception($"顶针：{currentBankSlotConfig.EjectionConfig.Name} 自动校准前请先完成示教！");
                    }
                }
            }

            #endregion

            #region 准备动作

            // 归还吸嘴（防撞）
            bool putBackRes = this.PutbackNozzleAssitance();

            if (putBackRes == false)
            {
               throw new Exception("吸嘴归位失败，无法进行顶针校准！");
            }

            WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();

            // 确保顶针在安全状态
            WaferSubController.GetInstance().EjectController.ReturnEjection();
            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToReadyPosition();

            // 判断是否有料片
            WaferSubController.GetInstance().WaferTableController.JudgeTabletOnWaferTable(false);

            // 顶针座升起
            //WaferSubController.GetInstance().EjectController.MoveEjectionTableToWorkPosition();

            #endregion
        }

        /// <summary>
        /// 校准所有顶针前准备
        /// </summary>
        public bool PrepareBeforeAllEjectionsCali()
        {
            #region 检查

            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsUseWaferCameraAssistantEjectCenter == false
                && MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated)
            {
                throw new Exception("FC硬件配置下不能使用Bond相机示教顶针！");
            }

            // 顶针架子上所有顶针配置
            List<EjectionBankSlotConfig> ejectionBankConfigList =
                WaferSystemProgram.GetInstance().GetDistinctEjectionBankSlotConfig();

            // 遍历所有顶针配置进行校准
            for (int i = 0; i < ejectionBankConfigList.Count; i++)
            {
                EjectionBankSlotConfig ejectionBankSlotConfig = WaferSystemProgram.GetInstance()
                    .GetDistinctEjectionBankSlotConfig().Find(it => it.EjectionConfig.Name == ejectionBankConfigList[i].Name);

                if (ejectionBankSlotConfig.EjectionConfig.IsAssistantSucceed == false)
                {
                    AKRSXtraMessageBox.Show($"顶针：{ejectionBankSlotConfig.EjectionConfig.Name} 自动校准前请先完成示教！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                // 寻找Pr模板
                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(ejectionBankSlotConfig.EjectionConfig.EjectMatchName);

                if (pREntity == null)
                {
                    throw new Exception($"顶针：{ejectionBankSlotConfig.EjectionConfig.Name} 未找到PR！");
                }

                if (ejectionBankSlotConfig.EjectionConfig.EjectionCaliVsionPos.IsEmpty)
                {
                    AKRSXtraMessageBox.Show($"顶针：{ejectionBankSlotConfig.EjectionConfig.Name} 自动校准前请先完成示教！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            #endregion

            #region 准备动作


            // 归还吸嘴（防撞）
            bool putBackRes = this.PutbackNozzleAssitance();

            if (putBackRes == false)
            {
                return false;
            }

            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsUseWaferCameraAssistantEjectCenter)
            {
                System2Domain.GetInstance().BondModuleController.MoveToSafePos();
            }

            WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();

            // 确保顶针在安全状态
            WaferSubController.GetInstance().EjectController.ReturnEjection();
            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToReadyPosition();

            // 判断是否有料片
            WaferSubController.GetInstance().WaferTableController.JudgeTabletOnWaferTable(false);

            // 顶针座升起
            WaferSubController.GetInstance().EjectController.MoveEjectionTableToWorkPosition();

            #endregion

            return true;
        }

        /// <summary>
        ///  取片偏移自动校准前准备
        /// </summary>
        /// <param name="component">芯片</param>
        public void PrepareBeforePickOffsetAutoCali(BaseCarrierConfig component)
        {
            // 寻找Pr模板
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(component.UpLookAdjustConfig.P1PRName);

            if (pREntity == null)
            {
                throw new Exception($"芯片：{component.Name} 未找到上视PR！");
            }

            // 换吸嘴
            if (this.ChangeNozzleAssistance(component.NozzleName) == false)
            {
                throw new Exception($"吸嘴：{component.NozzleName}更换失败！");
            }

            if (!System2RunTimeProvider.IsWaferCheckSucceed)
            {
                // 检查
                if (!WaferSystemDomain.GetInstance().CheckIsReady(true))
                {
                    return;
                }

                System2RunTimeProvider.IsWaferCheckSucceed = true;
            }

            // 复位信号
            Machine.GetInstance().ResetMachineSignal();

            // 芯片名称传给晶圆台
            WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

            // 给晶圆台发要料信号
            SignalPool.GetInstance().IsBondNeedChipSignal.Set();

            // 初始化搜精
            Block.GetInstance().StartInit();
        }

        /// <summary>
        /// 吸嘴数据校正
        /// </summary>
        /// <param name="nozzle">吸嘴</param>
        /// <returns>结果</returns>
        public ExcuteResult NozzleCaliAssistance(Nozzle nozzle)
        {
            #region 自动测高，暂时不用

            //// 获取BMC测高位置
            //AKRSPoint3D point = CalibrateRunPara.GetInstance().BMCMeasureHeightSearchMachinePos;

            //// 目标位置往上抬8mm
            //AKRSPoint3D targetPos = point + new AKRSPoint3D(0, 0, nozzle.MeasureHeightOffset + 5);

            //// 移动到测高位置
            //this.bondModuleController.MoveSafeBondXYZ(targetPos);

            //// 执行测高
            //(ExcuteResult Ret, double HeightValue) res = this.bondHeadController.MeasureHeight(
            //    BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
            //    HeightMeasurementFunctionEnum.WithTDSensor);

            //if (res.Ret == ExcuteResult.Success)
            //{
            //    // 保存测高结果
            //    nozzle.MeasureHeightOffset =
            //        res.HeightValue - BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult;
            //}
            //else
            //{
            //    return res.Ret;
            //}

            #endregion

            // 去旋转中心
            this.bondHeadController.RotateAxisT(nozzle.AlignAngle);

            // 吸嘴定位结果(坐标)
            AKRSPoint3D wordPos1, wordPos2;

            // 执行定位
            MatchResult result = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                    nozzle.NozzleToUplookCenterPos,
                    "NozzleCali",
                    nozzle.NozzlePRName,
                    false,
                    CameraTypeEnum.UpLookCamera);

            if (result == null || !result.IsSuccess)
            {
                AKRSXtraMessageBox.Show("定位失败，请检查吸嘴视觉模板！");
                return ExcuteResult.Exception;
            }

            // 获取G0坐标
            nozzle.NozzleToUplookCenterPos = wordPos1 = this.upLookController.ConvertPixelToG0Pos(
                                                 this.bondModuleController.Get3DRealPosition(),
                                                 result);

            double angle = this.bondHeadController.GetAxisTRealPos();

            if (this.bondHeadController.GetAxisTRealPos() <= 0)
            {
                // T轴转180度
                this.bondHeadController.RotateAxisT(angle + 180);
            }
            else
            {
                // T轴转-180度
                this.bondHeadController.RotateAxisT(angle - 180);
            }

            // 计算焊头旋转后的吸嘴偏移
            AKRSPoint2D nozzleOffsetRotated =
                this.bondHeadController.GetNozzleOffset(nozzle.Name, this.bondHeadController.GetAxisTRealPos() - nozzle.AlignAngle);

            AKRSPoint2D newCenter = this.bondModuleController.Get2DRealPosition() - nozzleOffsetRotated;

            // 再次移到吸嘴中心
            this.bondModuleController.MoveBondXY(newCenter.X, newCenter.Y);

            // 执行定位
            MatchResult result2 = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                null,
                "NozzleCali",
                nozzle.NozzlePRName,
                true,
                CameraTypeEnum.UpLookCamera);

            if (result2 == null || !result2.IsSuccess)
            {
                AKRSXtraMessageBox.Show("定位失败，请检查吸嘴视觉模板！");
                return ExcuteResult.Exception;
            }

            // 获取G0坐标
            wordPos2 = this.upLookController.ConvertPixelToG0Pos(
                this.bondModuleController.Get3DRealPosition(),
                result2);

            // 重新计算
            nozzle.NozzleOffset.X = (wordPos2.X - wordPos1.X) / 2;
            nozzle.NozzleOffset.Y = (wordPos2.Y - wordPos1.Y) / 2;

            this.bondModuleController.MoveToSafePos();

            NozzleRepository.GetInstance().Save();

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 顶针数据校正
        /// </summary>
        /// <param name="currentBankSlotConfig">顶针</param>
        /// <returns>结果</returns>
        public ExcuteResult EjectionConfigCaliAssistance(EjectionBankSlotConfig currentBankSlotConfig)
        {
            try
            {
                if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsUseWaferCameraAssistantEjectCenter)
                {
                    System2Domain.GetInstance().BondModuleController.MoveToSafePos();

                    // 顶针到校准高度
                    WaferSubController.GetInstance().EjectController.MoveEjectionAxisZToG0Pos(currentBankSlotConfig.EjectionConfig.EjectionCaliLevel);

                    // 相机到拍照位
                    WaferSubController.GetInstance().WaferTableController.MoveWaferCameraAxisZToG0Pos(currentBankSlotConfig.EjectionConfig.EjectionCaliVsionPos);

                    (bool isSucceed, MatchResult[] matchResults) result = Block.GetInstance().MatchResult(currentBankSlotConfig.EjectionConfig.EjectMatchName, false, false);

                    if (result.isSucceed)
                    {
                        // 重新计算
                        currentBankSlotConfig.EjectionConfig.DeviationWithEjectionCenterAndWaferCameraCenter = Block.GetInstance().MatchResultToWorld(result.matchResults)[0];
                    }
                    else
                    {
                        AKRSXtraMessageBox.Show("顶针定位失败！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        return ExcuteResult.Exception;
                    }
                }
                else
                {
                    // 归还吸嘴（防撞）
                    bool putBackRes = this.PutbackNozzleAssitance();

                    if (putBackRes == false)
                    {
                        return ExcuteResult.Fail;
                    }

                    // 顶针到校准高度
                    WaferSubController.GetInstance().EjectController.MoveEjectionAxisZToG0Pos(currentBankSlotConfig.EjectionConfig.EjectionCaliLevel);

                    // 相机到拍照位
                    // 执行定位
                    MatchResult res = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                        currentBankSlotConfig.EjectionConfig.EjectionCaliVsionPos,
                        "EjectionCali",
                        currentBankSlotConfig.EjectionConfig.EjectMatchName,
                        false,
                        CameraTypeEnum.BondCamera);

                    if (res == null || !res.IsSuccess)
                    {
                        AKRSXtraMessageBox.Show("顶针定位失败！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return ExcuteResult.Exception;
                    }

                    // 重新计算
                    //currentBankSlotConfig.EjectionConfig
                    //        .DeviationWithEjectionCenterAndWaferCameraCenter = this.bondModuleController.ConvertPixelToG0Pos(
                    //    this.bondModuleController.Get3DRealPosition(),
                    //    res) - this.bondModuleController.GetG0RealPosition();

                    AKRSPoint2D point = CalibService.GetMachinePosByPixelPos(
                        new AKRSPoint2D(0, 0),
                        res,
                        "BondCameraCoordinateSystem");
                    currentBankSlotConfig.EjectionConfig.DeviationWithEjectionCenterAndWaferCameraCenter =
                        new AKRSPoint3D(point.X, point.Y, 0);
                }
            }
            catch (Exception ex)
            {
                return ExcuteResult.Exception;
            }
            finally
            {
                System2Domain.GetInstance().BondModuleController.MoveToSafePos();

                // 机械问题还未解决这里暂时不剥离
                // WaferSubController.GetInstance().EjectController.ReturnEjection();
                EjectionConfigRepository.GetInstance().Save();
            }

            return ExcuteResult.Success;
        }

        /// <summary>
        ///  校准顶针架子上所有吸嘴（默认Bond相机）
        /// </summary>
        /// <returns>结果</returns>
        [Obsolete]
        public ExcuteResult AllEjectionsCaliAssistance()
        {
            try
            {
                // 顶针架子上所有顶针配置
                List<EjectionBankSlotConfig> ejectionBankConfigList =
                    WaferSystemProgram.GetInstance().GetDistinctEjectionBankSlotConfig();

                // 换第一个顶针
                WaferSubController.GetInstance().EjectController
                    .ChangeEjection(ejectionBankConfigList[0].SlotNum);

                if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsUseWaferCameraAssistantEjectCenter)
                {
                    #region 使用晶圆相机

                    for (int i = 0; i < ejectionBankConfigList.Count; i++)
                    {
                        // 顶针到校准高度
                        WaferSubController.GetInstance().EjectController.MoveEjectionAxisZToG0Pos(ejectionBankConfigList[i].EjectionConfig.EjectionCaliLevel);

                        // 相机到拍照位
                        WaferSubController.GetInstance().WaferTableController.MoveWaferCameraAxisZToG0Pos(ejectionBankConfigList[i].EjectionConfig.EjectionCaliVsionPos);

                        (bool isSucceed, MatchResult[] matchResults) result = Block.GetInstance().MatchResult(ejectionBankConfigList[i].EjectionConfig.EjectMatchName, false, false);

                        if (result.isSucceed)
                        {
                            // 重新计算
                            ejectionBankConfigList[i].EjectionConfig.DeviationWithEjectionCenterAndWaferCameraCenter = Block.GetInstance().MatchResultToWorld(result.matchResults)[0];
                        }
                        else
                        {
                            AKRSXtraMessageBox.Show("顶针定位失败！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            return ExcuteResult.Exception;
                        }

                        // 换下一个顶针
                        WaferSubController.GetInstance().EjectController
                            .ChangeEjection(ejectionBankConfigList[i + 1].SlotNum);
                    }

                    #endregion
                }
                else
                {
                    #region 使用Bond相机

                    // todo：焊头不用每次都移动


                    for (int i = 0; i < ejectionBankConfigList.Count; i++)
                    {
                        // 顶针到校准高度
                        WaferSubController.GetInstance().EjectController.MoveEjectionAxisZToG0Pos(ejectionBankConfigList[i].EjectionConfig.EjectionCaliLevel);

                        // 执行定位
                        MatchResult res = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                            ejectionBankConfigList[i].EjectionConfig.EjectionCaliVsionPos,
                            "EjectionCali",
                            ejectionBankConfigList[i].EjectionConfig.EjectMatchName);

                        if (res == null || !res.IsSuccess)
                        {
                            AKRSXtraMessageBox.Show("顶针定位失败！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return ExcuteResult.Exception;
                        }

                        AKRSPoint2D point = CalibService.GetMachinePosByPixelPos(
                            new AKRSPoint2D(0, 0),
                            res,
                            "BondCameraCoordinateSystem");
                        ejectionBankConfigList[i].EjectionConfig.DeviationWithEjectionCenterAndWaferCameraCenter =
                            new AKRSPoint3D(point.X, point.Y, 0);

                        // 换下一个顶针
                        WaferSubController.GetInstance().EjectController
                            .ChangeEjection(ejectionBankConfigList[i + 1].SlotNum);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                return ExcuteResult.Exception;
            }
            //finally
            //{
            //    System2Domain.GetInstance().BondModuleController.MoveToSafePos();

            //    // 归还顶针
            //    WaferSubController.GetInstance().EjectController.ReturnEjection();
            //    EjectionConfigRepository.GetInstance().Save();
            //}

            return ExcuteResult.Success;
        }

        /// <summary>
        ///  校准取片补偿
        /// </summary>
        /// <param name="component">芯片</param>
        /// <returns>结果</returns>
        public ExcuteResult PickOffsetCaliAssistance(BaseCarrierConfig component)
        {
            bool ret;

            // 上视P1P2位置
            AKRSPoint3D p1Pos = new AKRSPoint3D();
            AKRSPoint3D p2Pos = new AKRSPoint3D();

            // 芯片中心
            AKRSPoint3D componentCenter = new AKRSPoint3D();

        Repick:

            // 取片
            if (component.IsUseFlipTable == false)
            {
                ret = this.PickupFromWaferTable(component);
            }
            else
            {
                ret = this.PickupFromFlipTable(component);
            }

            if (!ret)
            {
                return ExcuteResult.Fail;
            }

            #region 芯片定位

            // 计算真实拍照位
            Nozzle nozzle = this.bondHeadController.GetCurrentNozzle();
            double angle = component.UpLookAdjustConfig.PRVisionAngle
                           + nozzle.AlignAngle;

            // 拍照位和吸嘴绑定，不同的吸嘴高度不同
            AKRSPoint3D p1RealVisionPos = component.UpLookAdjustConfig.P1VisionPos + new AKRSPoint3D(
                                              0,
                                              0,
                                              nozzle.MeasureHeightOffset);

            // 转到轴坐标
            AKRSPoint3D p1RealVisionPosInAxis = this.bondModuleController.ConvertG0ToMachinePos(p1RealVisionPos);

            AKRSPoint3D upLookPos = new AKRSPoint3D(
                p1RealVisionPosInAxis.X,
                p1RealVisionPosInAxis.Y,
                p1RealVisionPosInAxis.Z);

            // 去上视觉拍照位
            this.bondModuleController.MoveSafeBondXYZ(upLookPos);
            this.bondHeadController.RotateAxisT(angle);

            if (!component.IsTwoPointAdjust)
            {
                // P1定位
                (ExcuteResult result, MatchResult matchResult) result1 = this.UpLookVisionAsissitant(
                    component,
                    component.UpLookAdjustConfig.P1VisionPos,
                    component.UpLookAdjustConfig.P1PRName);

                switch (result1.result)
                {
                    case ExcuteResult.Success:
                        break;

                    case ExcuteResult.Retry:
                        this.bondModuleController.MoveToSafePos();
                        goto Repick;

                    default:
                        return result1.result;
                }

                System2RunTimeProvider.RecordTime(
                    "UpLookAction",
                    $"上视定位P1定位完成，定位结果:X :{result1.matchResult.CenterX},Y :{result1.matchResult.CenterY},角度 :{result1.matchResult.Angle}");

                componentCenter = p1Pos = this.upLookController.ConvertPixelToG0Pos(
                                      this.bondModuleController.Get3DRealPosition(),
                                      result1.matchResult);
            }
            else
            {
                // P2拍照位
                AKRSPoint3D p2RealVisionPos = component.UpLookAdjustConfig.P2VisionPos + new AKRSPoint3D(
                                                  0,
                                                  0,
                                                  nozzle.MeasureHeightOffset);

                // 直接移动XY
                this.bondModuleController.MoveBondXYToG0Pos(p2RealVisionPos.X, p2RealVisionPos.Y);

                (ExcuteResult result, MatchResult matchResult) result2 = this.UpLookVisionAsissitant(
                    component,
                    component.UpLookAdjustConfig.P1VisionPos,
                    component.UpLookAdjustConfig.P1PRName);

                switch (result2.result)
                {
                    case ExcuteResult.Success:
                        break;

                    case ExcuteResult.Retry:
                        this.bondModuleController.MoveToSafePos();
                        goto Repick;

                    default:
                        return result2.result;
                }

                System2RunTimeProvider.RecordTime(
                    "UpLookAction",
                    $"上视定位P2定位完成，定位结果:X :{result2.matchResult.CenterX},Y :{result2.matchResult.CenterY},角度 :{result2.matchResult.Angle}");

                p2Pos = this.upLookController.ConvertPixelToG0Pos(
                                       this.bondModuleController.Get3DRealPosition(),
                                       result2.matchResult);

                componentCenter = (p1Pos + p2Pos) / 2;
            }

            #endregion

            // 计算
            // 取片偏移=吸嘴中心-芯片中心
            component.PickupOffset.X += nozzle.NozzleToUplookCenterPos.X - componentCenter.X;
            component.PickupOffset.Y += nozzle.NozzleToUplookCenterPos.Y - componentCenter.Y;

            // 抛料
            this.bondModuleController.ThrowAction();
            this.bondModuleController.MoveToSafePos();

            CarrierConfigRepository.GetInstance().Save();

            return ExcuteResult.Success;
        }
    }
}
