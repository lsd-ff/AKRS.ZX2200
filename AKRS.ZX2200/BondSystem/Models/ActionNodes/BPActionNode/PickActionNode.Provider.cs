using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.BondForce.Modbus;
using AKRS.ZX2200.BondSystem.BondForce.Services;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Service;
using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.BPActionNode
{
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
    using AKRS.Galaxy2.Log;
    using AKRS.ZX2200.BondSystem.Controls.Manual;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using log4net.Core;
    using ZedGraph;

    /// <summary>
    /// PickActionNode帮助类
    /// </summary>
    public partial class PickActionNode
    {
        /// <summary>
        /// 取片前的准备动作
        /// </summary>
        private void PrepareBeforePickAction()
        {
            this.bp = this.actionNodesService.GetCurrentBondPosition();

            this.nextComponentName = this.actionNodesService.GetNextComponentName();

            // 获取当前吸嘴
            this.nozzle = this.bondHeadController.GetCurrentNozzle();

            this.initialSpeed = this.bondHeadController.GetAxisZAbsoluteSpeed();

            isSkipComponent = false;

            // 判断是否第一次启动
            if (System2RunTimeProvider.IsBondTaskFirstStart)
            {
                // 芯片名称传给晶圆台
                WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

                // 给晶圆台发要料信号
                SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                System2RunTimeProvider.RecordTime("取片信号交互", $"第一次启动，给晶圆发要料信号，芯片名称{component.Name}");
                System2RunTimeProvider.IsBondTaskFirstStart = false;
                System2RunTimeProvider.IsNewProduct = false;
            }
            else
            {
                if (this.component.IsUseFlipTable)
                {
                    if (System2RunTimeProvider.IsNewProduct)
                    {
                        if (this.component.CarrierType == CarrierTypeEnum.Wafer)
                        {
                            // 顶针缩回
                            WaferSubController.GetInstance().EjectController.MoveEjectToReadyLiftPositionAndBlow();
                        }

                        // 发要料信号
                        // 刷新Map信息
                        WaferSystemDomain.GetInstance().Block.RefreshMap();

                        // 芯片名称传给晶圆台
                        WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

                        // 给晶圆台发要料信号
                        SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                        System2RunTimeProvider.IsNewProduct = false;

                        Static.RecordTime("取片信号交互", $"取片完成，给晶圆发要料信号，芯片名称{component.Name}");
                    }
                }
            }

            if (this.component.DipMode != DipModeEnum.Off)
            {
                System2RunTimeProvider.IsNeedDip = true;
            }
        }

        /// <summary>
        /// 判断并更换吸嘴
        /// </summary>
        /// <returns>结果</returns>
        private ExcuteResult JudgeAndChangeNozzle()
        {
            // 如果和当前吸嘴不一样就需要换吸嘴
            if (this.system2Controller.IsNeedToChangeNozzle(component.NozzleName))
            {
                // 换吸嘴动作
                bool ret = this.system2Controller.ChangeNozzle(component.NozzleName);
                if (!ret)
                {
                    return ExcuteResult.Abort;
                }
            }

            this.nozzle = this.bondHeadController.GetCurrentNozzle();

            if (this.nozzle == null)
            {
                throw new Exception("PickActionNode:当前吸嘴为空！");
            }

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 判断并更换吸嘴
        /// </summary>
        private void JudgeAndCleanNozzle()
        {
            if (this.bondProgram.CleanNozzleProgram.IsCleanNozzleBeforePickup)
            {
                // 参数防呆
                if (BondDevicePara.GetInstance().BondHeadParam.NozzleCleanTableLeftTopPos.IsEmpty)
                {
                    throw new Exception("吸嘴清洁位未示教！");
                }

                if (System2RunTimeProvider.PickupNumAfterLastCleanNozzle
                    >= BondProgram.GetInstance().CleanNozzleProgram.CleanNozzleAfterPickupNum)
                {
                    this.system2Controller.CleanNozzle();

                    // 如果挡到搜晶就去避让位
                    if (WaferSubController.GetInstance().WaferTableController
                            .IsNotAllowWaferCameraAction(this.component.AdjustCamera) == false)
                    {
                        this.bondModuleController.MoveToSafePos();
                    }

                    System2RunTimeProvider.PickupNumAfterLastCleanNozzle = 0;
                }

                System2RunTimeProvider.PickupNumAfterLastCleanNozzle++;
            }
        }

        /// <summary>
        /// 静态华夫盒芯片定位
        /// </summary>
        /// <param name="isSkip">是否为跳过芯片</param>
        /// <returns>结果</returns>
        private (ExcuteResult, MatchResult) StaticWaffleVision(bool isSkip)
        {
            // 提前开关静态华夫盒真空      
            if (this.component.IsMatchWithVacuum)
            {
                WaferSubController.GetInstance().WaferTableController.OpenStaticWaffleVacuum();
            }
            else
            {
                WaferSubController.GetInstance().WaferTableController.CloseStaticWaffleVacuum();
            }

            System2RunTimeProvider.RecordTime(
                "静态华夫盒定位",
                $"提前开关静态华夫盒真空 ,IsMatchWithVacuum:{this.component.IsMatchWithVacuum}");

            // 获取拍照位
            AKRSPoint3D componentVisionPosInG0 = Block.GetInstance().GetResultDieG0Pos();
            AKRSPoint3D componentVision3DPos =
                this.bondModuleController.ConvertG0ToMachinePos(componentVisionPosInG0);

            AKRSPoint4D componentVision4DPos = new AKRSPoint4D()
            {
                X = componentVision3DPos.X,
                Y = componentVision3DPos.Y,
                Z = componentVision3DPos.Z,
                T = this.nozzle.AlignAngle
            };

            if (this.isSkipComponent == false)
            {
                // 去静态华夫盒拍照位
                ExcuteResult ret = this.MoveToStaticWaffleVisionPos(componentVision4DPos);
                if (ret != ExcuteResult.Success)
                {
                    return (ret, null);
                }
            }
            else
            {
                double curLevel = this.bondHeadController.GetAxisZRealPos();

                if (Math.Abs(curLevel - componentVision4DPos.Z) < 0.01)
                {
                    // 直接移动XY轴去下一片拍照位
                    this.bondModuleController.MoveBondXY(componentVision3DPos.X, componentVision3DPos.Y);
                }
                else
                {
                    this.bondModuleController.MoveSafeBondXYZT(componentVision4DPos);
                }
            }

            System2RunTimeProvider.RecordTime("PickActionNode", $"运动到静态华夫盒拍照位完成");

            // Bond定位
            MatchResult matchResult = (MatchResult)this.system2Controller.BondCameraVision(
               null,
               component.DieMatchName);

            System2RunTimeProvider.RecordTime("PickActionNode", $"静态华夫盒拍照位完成");

            // 结果判断
            if (matchResult == null)
            {
                if (component.BlankDieAutoSkipMaxTimes != 0
                    && System2RunTimeProvider.StaticWaffleComponentAutoSkipCount < component.BlankDieAutoSkipMaxTimes)
                {
                    // 刷新Map信息
                    WaferSystemDomain.GetInstance().Block.RefreshMap();

                    // 芯片名称传给晶圆台
                    WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

                    // 给晶圆台发要料信号
                    SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                    System2RunTimeProvider.RecordTime("取片信号交互", $"定位报警，给晶圆发要料信号，芯片名称{component.Name}");

                    this.isSkipComponent = true;

                    // 跳过次数+1
                    System2RunTimeProvider.StaticWaffleComponentAutoSkipCount++;

                    return (ExcuteResult.Retry, null);
                }

                // 跳过次数清零
                System2RunTimeProvider.StaticWaffleComponentAutoSkipCount = 0;

                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(component.DieMatchName);

                // 报警
                (DialogResult dialog, BaseAlgResult baseAlgResult) result =
                    UcMainSystem.VisionAlarmFunc(pREntity, bp.Name, "静态华夫盒拍照失败");

                if (result.dialog == DialogResult.OK)
                {
                    matchResult = (MatchResult)result.baseAlgResult;
                }
                // 跳过
                else if (result.dialog == DialogResult.Ignore)
                {
                    // 刷新Map信息
                    WaferSystemDomain.GetInstance().Block.RefreshMap();

                    // 芯片名称传给晶圆台
                    WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

                    // 给晶圆台发要料信号
                    SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                    System2RunTimeProvider.RecordTime("取片信号交互", $"定位报警，给晶圆发要料信号，芯片名称{component.Name}");

                    this.isSkipComponent = true;

                    return (ExcuteResult.Retry, null);
                }
                else if (result.dialog == DialogResult.Abort)
                {
                    return (ExcuteResult.Abort, null);
                }
                else
                {
                    return (ExcuteResult.Abort, null);
                }
            }

            System2RunTimeProvider.RecordTime("PickActionNode", $"Bond相机定位芯片成功，定位结果X： {matchResult.CenterX},Y： {matchResult.CenterX},Angle： {matchResult.Angle}");

            return (ExcuteResult.Success, matchResult);
        }

        /// <summary>
        /// 去搜晶位置并等待信号
        /// </summary>
        /// <param name="posInG0">G0坐标</param>
        /// <returns>结果</returns>
        private ExcuteResult MoveToSearchPosAndWaitSignal(AKRSPoint3D posInG0)
        {
            AKRSPoint3D pos = this.bondModuleController.ConvertG0ToMachinePos(posInG0);

            // 去搜晶位置
            ExcuteResult ret = this.MoveToWaferTableSearchPos(pos);
            if (ret != ExcuteResult.Success)
            {
                return ret;
            }

            System2RunTimeProvider.IsBondArriveSearchPos = true;

            // 晶圆上料准备判断（从信号池获取）,这个信号会自动复位
            if (!SignalPool.GetInstance().IsWaferAllowPickSignal.Wait())
            {
                return ExcuteResult.Abort;
            }

            return ExcuteResult.Success;
        }

        /// <summary>
        ///  开始设置力控实时曲线
        /// </summary>
        private void StartSetBondForceRealTimeCurve()
        {
            if (FrmForceRealTimeCurve.ForceReadTiming == ForceReadTimingEnum.Place)
            {
                return;
            }

            bool isSmallForce = ForceCalibrationService.JudgeIsSmallForce(component.PickupForce);

            System2RunTimeProvider.BondHeadInitialVal = this.bondHeadController.GetBondForceCurrentVal(isSmallForce);

            // 设置限定线
            UcMainSystem.SetChartControlConstantLine(component.PickupForce);

            // 开始读焊头力
            UcMainSystem.ActiveReadBondForce(true);
        }

        /// <summary>
        ///  停止设置力控实时曲线
        /// </summary>
        private void StopSetBondForceRealTimeCurve()
        {
            if (FrmForceRealTimeCurve.ForceReadTiming == ForceReadTimingEnum.Place)
            {
                return;
            }

            // 停止读焊头力
            Task.Run(() =>
                {
                    CommonUtil.SetCurrentThreadName($"停止读焊头力线程");
                    this.isReadPickupForce = false;
                    Thread.Sleep(300);
                    UcMainSystem.ActiveReadBondForce(false);
                });
        }

        /// <summary>
        ///  采集取片力值
        /// </summary>
        private Task StartCollectPickupForce()
        {
            if (System2Configuration.GetInstance().IsActiveSavePickupForce)
            {
                return Task.Run(() =>
                 {
                     // etel机台没办法读模拟量
                     if (System2Module.GetInstance().BondModule.BondHead.AxisZ.AxisDrive is ETELAxis)
                     {
                         return;
                     }

                     if (this.component.PickupForceMode != ForceModeEnum.Force)
                     {
                         return;
                     }

                     CommonUtil.SetCurrentThreadName($"取片力值采集线程");
                     this.isReadPickupForce = true;

                     bool isSmallForce = ForceCalibrationService.JudgeIsSmallForce(this.component.PickupForce);
                     double angle = this.bondHeadController.GetAxisTRealPos();

                     this.ExportPickupForce(isSmallForce, angle);
                 });
            }

            return null;
        }

        /// <summary>
        /// 打印取片力值
        /// </summary>
        /// <param name="isSmallForce">是否是小力</param>
        /// <param name="angle">角度</param>
        private void ExportPickupForce(bool isSmallForce, double angle)
        {
            List<(DateTime time, double forceVal)> printSource = new();

            if (TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit == null)
            {
                return;
            }

            string allPath = Machine.GetInstance().GetMachineDatePath("取片芯片受力表");

            allPath = Path.Combine(
                allPath,
                TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit.Name);

            if (!Directory.Exists(allPath))
            {
                Directory.CreateDirectory(allPath);
            }

            allPath = Path.Combine(
                        allPath,
            this.component.Name + DateTime.Now.ToString("yyMMddHHmmssfff") + "取片过程力值数据" + ".xlsx");

            while (this.isReadPickupForce)
            {
                double curVal = this.bondHeadController.GetBondForceCurrentVal(isSmallForce);
                printSource.Add(
                    new()
                    {
                        time = DateTime.Now,
                        forceVal = ForceCalibrationService.ForceIncrementToActualForce(
                            curVal, isSmallForce, angle)
                    });
            }

            // 非法字符剔除
            foreach (char rInvalidChar in Path.GetInvalidPathChars())
            {
                if (allPath.Contains(rInvalidChar.ToString()))
                {
                    allPath = allPath.Replace(rInvalidChar.ToString(), string.Empty);
                }
            }

            printSource.Select(
                (tuple, index) =>
                {
                    return new
                    {
                        时间 = $"{tuple.time:HH:mm:ss.fff}",
                        力值 = tuple.forceVal,
                    };
                }).ExportToXlsx(allPath);
        }

        /// <summary>
        ///  保存焊头LVDT值
        /// </summary>
        private void SaveLVDTValueDuringEjection()
        {
            lvdtValueList.Clear();
            List<string> paths = new List<string>();

            string allPath = "D:\\顶针顶起过程LVDT值\\";

            if (!Directory.Exists(allPath))
            {
                Directory.CreateDirectory(allPath);
            }

            if (TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit == null)
            {
                return;
            }

            string tuId = TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit.Name;

            // 当前配方名称
            paths.Add(MachineConfigContext.GetInstance().CurrentRecipe.RecipeName);

            paths.Add(DateTime.Now.Year.ToString() + "年");
            paths.Add(DateTime.Now.Month.ToString() + "月");
            paths.Add(DateTime.Now.Day.ToString() + "日");
            paths.Add(tuId);

            foreach (string path in paths)
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    return;
                }

                allPath = Path.Combine(allPath, path);
                if (!Directory.Exists(allPath))
                {
                    Directory.CreateDirectory(allPath);
                }
            }

            allPath = Path.Combine(
                        allPath,
            this.component.Name + DateTime.Now.ToString("yyMMddHHmmssfff") + "顶针顶起过程LVDT值" + ".xlsx");

            while (this.isReadPickupForce)
            {
                lvdtValueList.Add(
                    new()
                    {
                        time = DateTime.Now,
                        lvdtVal = this.bondHeadController.ReadLVDT()
                    });
            }

            // 非法字符剔除
            foreach (char rInvalidChar in Path.GetInvalidPathChars())
            {
                if (allPath.Contains(rInvalidChar.ToString()))
                {
                    allPath = allPath.Replace(rInvalidChar.ToString(), string.Empty);
                }
            }

            lvdtValueList.Select(
                (tuple, index) =>
                {
                    return new
                    {
                        时间 = $"{tuple.time:HH:mm:ss.fff}",
                        LVDT值 = tuple.lvdtVal,
                    };
                }).ExportToXlsx(allPath);

            if (System2Configuration.GetInstance().IsActiveLVDTWarning)
            {
                // 如果有超的就报警
                if (this.lvdtValueList.Any(it => it.lvdtVal > System2Configuration.GetInstance().LVDTWarningLimit))
                {
                    this.lvdtValueList.Clear();
                    DialogResult dialogResult = AKRSMessageBoxExt.Show(
                            $"芯片顶起过程中LVDT超过阀值{System2Configuration.GetInstance().LVDTWarningLimit}!\r\n",
                            "报警",
                            new string[] { "确定" },
                            new DialogResult[] { DialogResult.Yes },
                            AlarmLevel.SecondLevel);
                }
            }
        }

        /// <summary>
        /// 给翻转台发信号
        /// </summary>
        private void SendSignalToFlipTable()
        {
            // 翻转台左上位置
            AKRSPoint3D flipTableLeftTopPos =
                this.bondModuleController.ConvertG0ToMachinePos(
                    flipDevicePara.FlipTableLeftTopPos);

            // 翻转台右下位置
            AKRSPoint3D flipTableRightBottomPos =
                this.bondModuleController.ConvertG0ToMachinePos(
                    flipDevicePara.FlipTableRightBottomPos);

            var startTime = DateTime.Now;
            while (true)
            {
                if (MachineStateModel.GetInstance().MachineState == MachineStateEnum.Pause)
                {
                    startTime = DateTime.Now;
                    Thread.Sleep(20);
                    continue;
                }

                if (DateTime.Now - startTime > TimeSpan.FromSeconds(10))
                {
                    DialogResult dialogResult = AKRSMessageBoxExt.Show(
                        $"给翻转台发取料成功信号超时! \r\n",
                        "真空报警",
                        new string[] { "忽略", "退出" },
                        new DialogResult[]
                            {
                                DialogResult.Ignore, DialogResult.Abort
                            },
                        AlarmLevel.SecondLevel);

                    switch (dialogResult)
                    {
                        case DialogResult.Ignore:
                            startTime = DateTime.Now;
                            break;

                        case DialogResult.Abort:
                            Machine.GetInstance().Stop();
                            return;
                    }
                }

                if (GeometryService.IsBond2DPosInRange(flipTableLeftTopPos, flipTableRightBottomPos))
                {
                    Thread.Yield();
                    continue;
                }

                // 给翻转台发取料成功信号
                SignalPool.GetInstance().IsBondPickSucceedSignal.Set();

                break;
            }
        }

        /// <summary>
        /// 给晶圆台发信号
        /// </summary>
        private void SendSignalToWaferTable()
        {
            if (nextComponentName == component.Name && nextComponentName != null)
            {
                // 判断是不是最后一颗芯片，如果不需要切换芯片就在这里发要料信号
                // 刷新Map信息
                WaferSystemDomain.GetInstance().Block.RefreshMap();

                // 芯片名称传给晶圆台
                // 这里不切换芯片，切换芯片的判断在上视/中转台
                WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

                // 给晶圆台发要料信号
                SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                LogHelper.Post(
                    Level.Info,
                    $"取片信号交互-取片完成，给晶圆发要料信号，芯片名称{component.Name}",
                    LogCategory.Component,
                    ViewType.InFileAndUI);
            }
            else if (component.AccuracyMode == AccuracyModeEnum.Off)
            {
                // 刷新Map信息
                WaferSystemDomain.GetInstance().Block.RefreshMap();

                // 下一颗芯片名称传给晶圆台
                // 盲贴的话直接在这里切换芯片，不考虑重取
                WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(nextComponentName);

                // 给晶圆台发要料信号
                SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                LogHelper.Post(
                    Level.Info,
                    $"取片信号交互-取片完成，给晶圆发要料信号，芯片名称{nextComponentName}",
                    LogCategory.Component,
                    ViewType.InFileAndUI);
            }
        }

        /// <summary>
        /// 设置纠偏相机灯光
        /// </summary>
        private void SetAcurracyCameraLight()
        {
            if (component.AccuracyMode != AccuracyModeEnum.Off)
            {
                // 模板名称
                string prName = component.AdjustCamera == CameraTypeEnum.UpLookCamera
                                    ? component.UpLookAdjustConfig.P1PRName
                                    : component.DownLookAdjustConfig.P1PRName;

                // 寻找Pr模板
                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(prName);

                // 提前设置上、下视灯光
                pREntity.SetLight();
            }
        }

        /// <summary>
        /// 开始异步采集LVDT值
        /// </summary>
        private Task StartAsynchronousCollectionLVDT()
        {
            if (System2Configuration.GetInstance().IsActiveSaveLVDTValue)
            {
                return Task.Run(() =>
                       {
                           CommonUtil.SetCurrentThreadName($"LVDT值采集线程");
                           this.SaveLVDTValueDuringEjection();
                       });

            }

            return null;
        }

        /// <summary>
        /// 顶针顶起
        /// </summary>
        private void EjectionLift()
        {
            CarrierWithWaferConfig carrierWithWafer = component as CarrierWithWaferConfig;

            // 力控模式下没有同步顶，后续看有没有此工艺需求
            if (carrierWithWafer.IsActivateSynchronousEjection)
            {
                // 顶针同步顶
                this.bondHeadController.SynchronousEjection(carrierWithWafer.EjectLiftSpeed);

                System2RunTimeProvider.RecordTime("Pick", $"顶针同步顶完成");
            }
            else
            {
                this.StartAsynchronousCollectionLVDT();

                // 这里改成异步是为了提高UPH
                Task ejectTask = Task.Run(
                     () =>
                         {
                             System2RunTimeProvider.RecordTime(
                                 "Pick",
                                 $"顶针准备顶起，顶针G0坐标{WaferSubController.GetInstance().EjectController.EjectionAxisZG0Pos.Z}");

                             System2RunTimeProvider.EjectStopwatch.Restart();

                             // 顶针顶起
                             WaferSubController.GetInstance().EjectController.MoveEjectToLiftPosition();

                             System2RunTimeProvider.EjectionLiftTime.Add(
                                 new()
                                 {
                                     time = DateTime.Now,
                                     liftTime = System2RunTimeProvider.EjectStopwatch.ElapsedMilliseconds
                                 });
                         });

                if (component.IsResetForceControlAdvanceDuringPickup
                    && component.PickupForceMode == ForceModeEnum.Force)
                {
                    // 这里往上抬一点是为了防止芯片顶裂
                    double pos = this.bondHeadController.GetAxisZRealPos()
                                 + carrierWithWafer.ForecResetDistanceDuringForceMode;

                    // 原地退出力控
                    this.bondHeadController.ForceControlReset(pos, this.initialSpeed);

                    System2RunTimeProvider.RecordTime("Pick", $"原地退出力控完成");
                }

                // 等顶针顶起到位
                ejectTask.Wait();
            }
        }

        /// <summary>
        ///  取片前检查
        /// </summary>
        /// <param name="component">芯片</param>
        /// <returns>结果</returns>
        public ExcuteResult CheckBeforePick(BaseCarrierConfig component)
        {
            // 判断翻转台是否在0位
            if (component.IsUseFlipTable && WaferSubController.GetInstance().FlipController.IsFlipTableAtTransferPos() == false)
            {
                throw new Exception("翻转台不在交接位，取片失败！");
            }

            if (this.bondHeadController.GetNozzleBlowState())
            {
                this.bondHeadController.CloseToolBlowEle();
            }

            if (this.component.PickupForceMode == ForceModeEnum.Force)
            {
                bool isSmallForce = ForceCalibrationService.JudgeIsSmallForce(this.component.PickupForce);

                this.bondHeadController.ZeroBondhead(isSmallForce);

                System2RunTimeProvider.RecordTime("PickActionNode", "焊头清零 完成");
            }

            string name = WaferSystemDomain.GetInstance().WaferSubSystemTask.GetCurrentNeedChipName();

            if (name != this.component.Name)
            {
                throw new Exception($"动作排序算法异常：跟晶圆要料的芯片：{name},当前准备取片的芯片：{this.component.Name}，请联系软件工程师！");
            }

            return ExcuteResult.Success;
        }
    }
}
