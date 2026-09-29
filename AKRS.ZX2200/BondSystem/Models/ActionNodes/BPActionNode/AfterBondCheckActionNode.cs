using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
using AKRS.ZX2200.Main.Machine.Process;
using AKRS.ZX2200.SupportFeature.Compensate.TemperatureCompensate;
using AKRS.ZX2200.SupportFeature.Statistics;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using LanguageExt;
using log4net.Core;
using TransportUnit = AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.BPActionNode
{
    using System.Linq;
    using System.Threading;
    using Accord.Math;
    using AKRS.ZX2200.Main.Machine.MachineSupport;

    /// <summary>
    /// 焊后检测流程
    /// </summary>
    [Serializable]
    public class AfterBondCheckActionNode : ActionNode
    {
        /// <summary>
        /// Bond域
        /// </summary>
        private System2Domain System2Domain => System2Domain.GetInstance();

        /// <summary>
        /// System2Controller
        /// </summary>
        private System2Controller system2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 系统2动作节点排序
        /// </summary>
        private S2ActionNodeController ActionNodesService => System2Domain.GetInstance().ActionNodesService;

        /// <summary>
        /// 焊后检测对象
        /// </summary>
        public PostBondInspection PostBondInspection { get; set; }
        
        /// <summary>
        /// 当前传输单元/载具 
        /// </summary>
        private TransportUnit TransportUnit =>
            TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

        /// <summary>
        /// 当前基板
        /// </summary>
        public Substrate Substrate { get; set; }

        /// <summary>
        /// 当前基岛
        /// </summary>
        public Module Module { get; set; }

        /// <summary>
        /// 当前焊点
        /// </summary>
        public BondPosition BondPosition { get; set; }

        /// <summary>
        /// 是不是为测试
        /// </summary>
        public bool IsEditTest { get; set; } = false;

        /// <summary>
        /// 检测结果
        /// </summary>
        private ExcuteResult defectResult;

        /// <summary>
        /// 芯片
        /// </summary>
        private BaseCarrierConfig component;

        /// <summary>
        /// 执行动作
        /// </summary>
        /// <returns>结果</returns>
        public override ExcuteResult DoWork()
        {
            this.WorkStart?.Invoke();
            this.State = RunStateEnum.Running;
            this.defectResult = ExcuteResult.Success;
            try
            {
                // 判断是自动模式还是非自动模式
                if (!this.IsEditTest)
                {
                    this.Substrate = System2Domain.GetInstance().ActionNodesService.GetCurrentSubstrate();
                    this.Module = System2Domain.GetInstance().ActionNodesService.GetCurrentModule();
                    this.BondPosition = System2Domain.GetInstance().ActionNodesService.GetCurrentBondPosition();
                    this.PostBondInspection =
                        System2Domain.GetInstance().ActionNodesService.GetCurrentPostBondInspection();

                    this.component = System2Domain.GetInstance().ActionNodesService.GetCurrentComponent().component;

                    // 跳过检测数量
                    if (!this.PostBondInspection.IsNeedDefect())
                    {
                        // this.PostBondInspection.FinishedDefect();
                        this.BondPosition.S2FinishedStep(this.ActionNodesService.GetCurrentProcessStepName());
                        return ExcuteResult.Success;
                    }
                }

                System2RunTimeProvider.RecordTime("系统2焊后检测", $"焊点：{BondPosition.Name}焊后检测开始");

                // 离线模式
                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    this.BondPosition.S2FinishedStep(this.System2Domain.ActionNodesService.GetCurrentProcessStepName());
                    return ExcuteResult.Success;
                }

                // 空跑模式
                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DryCycle)
                {
                    return this.PostBondActionDryRun();
                }

                // 胶量检测
                if (PostBondInspection.ApplicationSystem == PostBondApplicationSystemEnum.EpoxyCheck)
                {
                    // 芯片点位1定位
                    AKRSPoint3D visionPos1 =
                        BondPosition.CoordinateSystem.SelfPosToG0(this.PostBondInspection.VisionConfig.P1VisionPos)
                        - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                    // 去拍照位置执行拍照
                    List<BaseAlgResult> baseAlgResults = this.system2Controller.BondCameraVisionDefect(
                        visionPos1,
                        this.PostBondInspection.VisionConfig.P1PRName, false, true);

                    // 结果检测
                    return this.EpoxyApplicationDefect(baseAlgResults);
                }
                else if (PostBondInspection.ApplicationSystem == PostBondApplicationSystemEnum.AfterBondCheck)
                {
                    // 焊后检测
                    switch (PostBondInspection.PostBondInspectionMode)
                    {
                        // 如果是没有参考点的情况下定位
                        case PostBondInspectionModeEnum.WithoutReferenceSearch:

                            #region 一点定位，无参考点

                            AKRSPoint4D g0Point4D1 = this.DefectVision(
                                this.PostBondInspection.VisionConfig.P1VisionPos,
                                this.PostBondInspection.VisionConfig.P1PRName);

                            if (PostBondInspection.MeasurePointNumber == MeasurePointNumberEnum.OnePoint
                                && PostBondInspection.PostBondInspectionMode
                                == PostBondInspectionModeEnum.WithoutReferenceSearch)
                            {
                                // 判断是否检测背崩
                                if (!this.PostBondInspection.IsDetectBacksideCrack)
                                {
                                    return this.WithoutReferResultDeal(
                                        this.PostBondInspection.VisionConfig.P1PRName,
                                        g0Point4D1);
                                }
                                else
                                {
                                    // 定位结果处理
                                    if (this.WithoutReferResultDeal(
                                            this.PostBondInspection.VisionConfig.P1PRName,
                                            g0Point4D1) != ExcuteResult.Success)
                                    {
                                        return this.WithoutReferResultDeal(
                                            this.PostBondInspection.VisionConfig.P1PRName,
                                            g0Point4D1);
                                    }

                                    PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance()
                                        .Find(this.PostBondInspection.VisionConfig.P1PRName);

                                    MatchResult matchResult = (MatchResult)pREntity.AlgResult;

                                    // todo:移动到芯片中心

                                    retry:

                                    // 背崩检测
                                    bool isCrack = this.system2Controller.BacksideCrackDetect(
                                        matchResult,
                                        this.PostBondInspection.VisionConfig.BacsideCrackDetectPRName);

                                    // 处理结果
                                    ExcuteResult ret = this.BacksideCrackResultDeal(
                                        this.PostBondInspection.VisionConfig.BacsideCrackDetectPRName,
                                        isCrack);

                                    if (ret == ExcuteResult.Retry)
                                    {
                                        goto retry;
                                    }
                                    else
                                    {
                                        return ret;
                                    }
                                }
                            }

                            #endregion

                            #region 两点定位，无参考点

                            AKRSPoint4D g0Point3D2 = g0Point4D1;

                            if (PostBondInspection.MeasurePointNumber == MeasurePointNumberEnum.TwoPoint)
                            {
                                g0Point3D2 = this.DefectVision(
                                    this.PostBondInspection.VisionConfig.P2VisionPos,
                                    this.PostBondInspection.VisionConfig.P2PRName);
                            }

                            if (g0Point3D2 == null)
                            {
                                return this.defectResult;
                            }

                            if (PostBondInspection.MeasurePointNumber == MeasurePointNumberEnum.TwoPoint
                                && PostBondInspection.PostBondInspectionMode
                                == PostBondInspectionModeEnum.WithoutReferenceSearch)
                            {
                                return this.WithoutReferResultDeal(
                                    this.PostBondInspection.VisionConfig.P2PRName,
                                    (g0Point4D1 + g0Point3D2) / 2);

                            }

                            #endregion

                            #region 一点定位，有参考点

                            AKRSPoint4D g0Point3D3 = this.DefectVision(
                                this.PostBondInspection.VisionConfig.P1ReferVisionPos,
                                this.PostBondInspection.VisionConfig.P1ReferName);

                            if (g0Point3D3 == null)
                            {
                                return this.defectResult;
                            }

                            if (PostBondInspection.ReferencePointNumber == MeasurePointNumberEnum.OnePoint
                                && PostBondInspection.PostBondInspectionMode
                                == PostBondInspectionModeEnum.WithReferenceSearch)
                            {
                                return this.WithReferResultDeal(
                                    this.PostBondInspection.VisionConfig.P1ReferName,
                                    (g0Point4D1 + g0Point3D2) / 2,
                                    g0Point3D3);

                            }

                            #endregion

                            #region 两点定位，有参考点

                            AKRSPoint4D g0Point3D4 = this.DefectVision(
                                this.PostBondInspection.VisionConfig.P2ReferVisionPos,
                                this.PostBondInspection.VisionConfig.P2ReferName);

                            if (g0Point3D4 == null)
                            {
                                return this.defectResult;
                            }

                            return this.WithReferResultDeal(
                                this.PostBondInspection.VisionConfig.P2ReferName,
                                (g0Point4D1 + g0Point3D2) / 2,
                                (g0Point3D3 + g0Point3D4) / 2);

                        #endregion
                    }
                }
                else if (PostBondInspection.ApplicationSystem == PostBondApplicationSystemEnum.BLTMeasure)
                {
                    // 芯片周围点位
                    List<AKRSPoint3D> aroundPosList = this.PostBondInspection.ComponentAroundMeasureHeightPosList
                        .Select(it => BondPosition.CoordinateSystem.SelfPosToG0(it)).ToList();

                    // 激光测高
                    List<double> height1 = this.system2Controller.LaserMeasureHeight(aroundPosList);

                    // 芯片表面点位
                    List<AKRSPoint3D> surfacePosList = this.PostBondInspection.ComponentSurfaceMeasureHeightPosList
                        .Select(it => BondPosition.CoordinateSystem.SelfPosToG0(it)).ToList();

                    // 激光测高
                    List<double> height2 = this.system2Controller.LaserMeasureHeight(surfacePosList);

                    // 数值检测
                    return this.BLTCheck(height1, height2);
                }

                System2RunTimeProvider.RecordTime("系统2焊后检测", $"焊点：{BondPosition.Name}焊后检测结束");

                return ExcuteResult.Success;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程{this.Name}运行故障", ex, LogCategory.Bond);
                AKRSMessageBoxExt.Show(ex.Message, "Exception", new string[] { "Exception" }, new DialogResult[] { DialogResult.Yes });
                return ExcuteResult.Exception;
            }
            finally
            {
                this.IsEditTest = false;
                this.State = RunStateEnum.Stop;
                this.WorkStop?.Invoke();
                UcMainSystem.ReFreshSystem2TuAction();
            }
        }


        /// <summary>
        /// 完成动作的条件是否满足
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsDoWork()
        {
            // 焊点
            BondPosition bp = System2Domain.GetInstance().ActionNodesService.GetCurrentBondPosition();

            // 判断当前工艺制程是否开启
            if (bp.MatterProductState == MatterProductState.Disable
                || bp.MatterProductState == MatterProductState.EnableInSystem1)
            {
                return false;
            }

            // 如果当前焊点已经点过胶则退出
            if (!bp.IsS2NeedStep(this.System2Domain.ActionNodesService.GetCurrentProcessStepName()))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 焊后失败执行的方法
        /// </summary>
        /// <param name="alarmMessage">报警信息</param>
        /// <returns>结果</returns>
        private (DialogResult, MatchResult) PostBondVisionFail(string alarmMessage)
        {
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.PostBondInspection.VisionConfig.P1PRName);

            // 报警
            (DialogResult dialog, BaseAlgResult matchResult) result = UcMainSystem.VisionAlarmLockFunc(
                pREntity,
                alarmMessage,
                "焊后定位失败", System2Domain.GetInstance().BondModuleController.GetHardware());

            if (result.dialog == DialogResult.Abort)
            {
                StatisticsDomain.GetInstance().PostBondFailedCount++;
                Machine.GetInstance().Stop();
                return (DialogResult.Abort, null);
            }
            else if (result.dialog == DialogResult.Ignore)
            {
                StatisticsDomain.GetInstance().PostBondFailedCount++;
                this.BondPosition.SetMatterDisable();
                return (DialogResult.Ignore, new MatchResult());
            }
            else if (result.dialog == DialogResult.OK)
            {
                return (DialogResult.OK, (MatchResult)result.matchResult);
            }

            StatisticsDomain.GetInstance().PostBondFailedCount++;
            Machine.GetInstance().Stop();
            return (DialogResult.Abort, null);
        }

        /// <summary>
        /// 焊后定位
        /// </summary>
        /// <param name="visionPoint3D">定位位置</param>
        /// <param name="pRName">视觉模板名称</param>
        /// <returns>G0的坐标</returns>
        private AKRSPoint4D DefectVision(AKRSPoint3D visionPoint3D,string pRName)
        {
            // 芯片点位1定位
            AKRSPoint3D visionPos1 =
                BondPosition.CoordinateSystem.SelfPosToG0(
                    visionPoint3D) - BondDevicePara.GetInstance()
                    .BondHeadParam.HeadToCameraOffset;

            MatchResult result1 = (MatchResult)this.System2Domain.System2Controller.BondCameraVision(
                visionPos1,
                pRName, false, true);

            if (result1 == null)
            {
                (DialogResult dialogResult, MatchResult matchResult) visionRetryResult =
                    this.PostBondVisionFail("焊后未识别成功");

                if (visionRetryResult.dialogResult == DialogResult.Ignore)
                {
                    this.defectResult = ExcuteResult.Success;
                    this.BondPosition.MatterProductState = MatterProductState.Disable;
                    return null;
                }
                else if (visionRetryResult.dialogResult == DialogResult.OK)
                {
                    if (visionRetryResult.matchResult == null)
                    {
                        visionRetryResult.matchResult = new MatchResult() { CenterX = 1224, CenterY = 1024 };
                    }

                    result1 = visionRetryResult.matchResult;
                }
                else
                {
                    this.defectResult = ExcuteResult.Abort;
                    return null;
                }
            }
           
            AKRSPoint3D pos = System2Module.GetInstance().BondModule.ConvertPixelToG0Pos(
                                  System2Module.GetInstance().BondModule.Get3DRealPosition(),
                                  result1) + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

            AKRSPoint3D posInBp = BondPosition.CoordinateSystem.G0PosToSelf(pos) - visionPoint3D;

            return new AKRSPoint4D(posInBp.X, posInBp.Y, posInBp.Z, result1.Angle);
        }

        /// <summary>
        /// 结果处理
        /// </summary>
        /// <param name="pRName">视觉名称</param>
        /// <param name="point4D">定位结果点位</param>
        /// <returns>执行结果</returns>
        private ExcuteResult WithoutReferResultDeal(string pRName,AKRSPoint4D point4D)
        {
            // 如果没有没有结果直接返回
            if (point4D == null)
            {
                return ExcuteResult.Fail;
            }

            // 定位的角度加上基板的角度
            point4D.T += BondPosition.CoordinateSystem.DegreeInG0() * 180.0 / Math.PI;

            this.LogDefect(point4D);

            return this.PostBondDefect(pRName, point4D);
        }

        /// <summary>
        /// 背崩检测结果处理
        /// </summary>
        /// <param name="edgePR">视觉名称</param>
        /// <param name="isCrack">是否崩边</param>
        /// <returns>执行结果</returns>
        private ExcuteResult BacksideCrackResultDeal(string edgePR, bool isCrack)
        {
            if (isCrack)
            {
                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(edgePR);

                (DialogResult dialog, BaseAlgResult matchResult) visionRetryResult = UcMainSystem.VisionAlarmLockFunc(
                    pREntity,
                    "检测到背崩!",
                    "背崩检测", System2Domain.GetInstance().BondModuleController.GetHardware());

                if (visionRetryResult.dialog == DialogResult.Abort)
                {
                    StatisticsDomain.GetInstance().PostBondFailedCount++;
                    Machine.GetInstance().Stop();
                    return ExcuteResult.Abort;
                }
                else if (visionRetryResult.dialog == DialogResult.Ignore)
                {
                    StatisticsDomain.GetInstance().PostBondFailedCount++;
                    this.BondPosition.SetMatterDisable();
                    return ExcuteResult.Success;
                }
                else if (visionRetryResult.dialog == DialogResult.OK)
                {
                    return ExcuteResult.Retry;
                }
            }

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 结果处理
        /// </summary>
        /// <param name="point4D1">定位结果点位1</param>
        /// <param name="point4D2">定位结果点位2</param>
        private ExcuteResult WithReferResultDeal(string pRName,AKRSPoint4D point4D1, AKRSPoint4D point4D2)
        {
            double distanceX = Math.Round(
                Math.Abs(point4D1.X - point4D2.X) - this.PostBondInspection.PostBondDistanceX,
                3);

            double distanceY = Math.Round(
                Math.Abs(point4D1.Y - point4D2.Y) - this.PostBondInspection.PostBondDistanceY,
                3);

            double distanceAngle = Math.Round(
                Math.Abs(point4D1.T - point4D2.T) - this.PostBondInspection.PostBondDistanceAngle,
                3) - BondPosition.CoordinateSystem.DegreeInG0();

            AKRSPoint4D point4D = new AKRSPoint4D(distanceX, distanceY, 0, distanceAngle);

            this.LogDefect(point4D);

            return this.PostBondDefect(pRName, point4D);
        }

        /// <summary>
        /// 加入日志
        /// </summary>
        /// <param name="point4D">检测结果</param>
        private void LogDefect(AKRSPoint4D point4D)
        {
            double distanceX = Math.Round(point4D.X - this.PostBondInspection.PostBondDistanceX, 5);

            double distanceY = Math.Round(point4D.Y - this.PostBondInspection.PostBondDistanceY, 5);

            double distanceAngle = Math.Round(point4D.T - this.PostBondInspection.PostBondDistanceAngle, 5);

            DefectStatisticsEntity defect = new DefectStatisticsEntity(
                DateTime.Now,
                this.PostBondInspection.Name,
                TransportUnit.Name,
                Substrate.Index,
                Module.Index,
                BondPosition.Name,
                distanceX * 1000.0,
                distanceY * 1000.0,
                distanceAngle,
                0,
                this.BondPosition.BondPositionInfo.TpMarkCompensate.X,
                this.BondPosition.BondPositionInfo.TpMarkCompensate.Y,
                this.BondPosition.BondPositionInfo.ComponentOffSet.X,
                this.BondPosition.BondPositionInfo.ComponentOffSet.Y,
                BondPosition.BondPositionInfo.RealBondPoint3D.X,
                BondPosition.BondPositionInfo.RealBondPoint3D.Y,
                BondPosition.GetPositionCompensate().X,
                BondPosition.GetPositionCompensate().Y,
                BondPosition.BondPositionInfo.SubstrateCameraCompensate.X,
                BondPosition.BondPositionInfo.SubstrateCameraCompensate.Y);

            StatisticsDomain.GetInstance().AddDefectResult(defect);
        }

        /// <summary>
        /// 补胶
        /// </summary>
        private void ReDispense()
        {
            // 重置前面的动作节点,重新取贴
            List<ActionNode> actionNodeList = new List<ActionNode>
                                                  {
                                                      System2Domain.GetInstance().BondActionNodeRepository.S2DispenseActionNode,
                                                      System2Domain.GetInstance().BondActionNodeRepository.AfterBondCheckActionNode,
                                                  };

            List<string> strings = new List<string>();

            List<(ActionNode, string)> actionNodes = new List<(ActionNode, string)>();

            // 将所有的都变成没有完成，后面后修改
            foreach (var key in BondPosition.BondPositionInfo.S2RemainingSteps.Keys)
            {
                // 找到步骤名称
                SingleProcessStep singleProcessStep = ProcessDomain.GetInstance().S2SystemProcess.ProcessSteps.Find(it => it.ProcessStepName == key);

                // 如果步骤存在点胶则置为未完成
                if (singleProcessStep.EpoxyNameApplicationName != "Null")
                {
                    actionNodes.Add((actionNodeList[0], key));

                    strings.Add(key);
                }

                if (singleProcessStep.DefectName == this.PostBondInspection.Name)
                {
                    actionNodes.Add((actionNodeList[1], key));
                    strings.Add(key);
                }
            }

            foreach (string key in strings)
            {
                BondPosition.BondPositionInfo.S2RemainingSteps[key] = false;
            }

            System2Domain.GetInstance().ActionNodesService.SetActionNodeReWork(actionNodes);
        }

        /// <summary>
        /// 检测结果处理
        /// </summary>
        /// <returns>结果</returns>
        private ExcuteResult EpoxyApplicationDefect(List<BaseAlgResult> baseAlgResults)
        {
            ExcuteResult excuteResult = this.PostBondInspection.PostBondEpoxyCheck(baseAlgResults,BondPosition, this.System2Domain.BondModuleController.GetHardware());

            if (excuteResult == ExcuteResult.Success)
            {
                if (!this.IsEditTest)
                {
                    this.BondPosition.S2FinishedStep(this.ActionNodesService.GetCurrentProcessStepName());

                    this.PostBondInspection.FinishedDefect();
                }

                return ExcuteResult.Success;
            }
            else if (excuteResult == ExcuteResult.Fail)
            {
                BondPosition.SetMatterDisable();
                return ExcuteResult.Success;
            }
            else if (excuteResult == ExcuteResult.Abort)
            {
                Machine.GetInstance().Stop();
                return ExcuteResult.Abort;
            }
            else if (excuteResult == ExcuteResult.Retry)
            {
                this.ReDispense();
                return ExcuteResult.Retry;
            }
            else
            {
                throw new Exception("胶量检测未知错误");
            }
        }

        /// <summary>
        /// 将芯片取起来
        /// </summary>
        private void PickUpDie()
        {
            BondHead bondHead = new BondHead();

            AKRSPoint3D point3D = this.BondPosition.BondPositionInfo.RealBondPos;
            System2Domain.GetInstance().BondModuleController.MoveSafeBondXYZ(point3D);

            // 力控模式直接从当前位置进入力控模式
            System2Domain.GetInstance().BondHeadController.ForceControlSet(
                80,
                bondHead.AxisZ.GetCmdPosition() - 0.01,
                10);

            // 关闭吸嘴真空
            System2Domain.GetInstance().BondHeadController.OpenToolVaccum();

            Thread.Sleep(500);

            // 加这句是为了防止Z轴抬起时没有恢复正常速度
            System2Domain.GetInstance().BondHeadController.SetAxisZSpeed(10);

            double liftPreLevel = bondHead.AxisZ.GetCmdPosition() + 1;

            // 力控模式低速速上抬
            System2Domain.GetInstance().BondHeadController.ForceControlReset(
                liftPreLevel,
               10);

        }

        /// <summary>
        /// 检测结果处理
        /// </summary>
        /// <returns>结果</returns>
        private ExcuteResult PostBondDefect(string prName, AKRSPoint4D point4D)
        {
            ExcuteResult excuteResult = this.PostBondInspection.PostBondCheck(prName, point4D,this.System2Domain.BondModuleController.GetHardware());

            if (MachineStateModel.GetInstance().IsCompensateWork)
            {
                this.PickUpDie();
            }

            if (excuteResult == ExcuteResult.Success)
            {
                if (!this.IsEditTest)
                {
                    this.BondPosition.S2FinishedStep(this.ActionNodesService.GetCurrentProcessStepName());

                    this.PostBondInspection.FinishedDefect();
                }

                return ExcuteResult.Success;
            }
            else if (excuteResult == ExcuteResult.Fail)
            {
                BondPosition.SetMatterDisable();
                return ExcuteResult.Success;
            }
            else if (excuteResult == ExcuteResult.Abort)
            {
                Machine.GetInstance().Stop();
                return ExcuteResult.Abort;
            }
            else
            {
                throw new Exception("焊后检测未知错误");
            }
        }

        /// <summary>
        /// 焊后动作空跑
        /// </summary>
        /// <returns>结果</returns>
        private ExcuteResult PostBondActionDryRun()
        {
            // 拍照位
            AKRSPoint3D visionPos1 =
                BondPosition.CoordinateSystem.SelfPosToG0(
                    this.PostBondInspection.VisionConfig.P1VisionPos) - BondDevicePara.GetInstance()
                    .BondHeadParam.HeadToCameraOffset;

            this.bondModuleController.MoveToG0Pos(visionPos1);

            // 拍照停留
            DelayHelper.Delay((int)System2Configuration.GetInstance().DownLookVisionDelay);

            // 结束当前制程
            this.BondPosition.S2FinishedStep(this.System2Domain.ActionNodesService.GetCurrentProcessStepName());

            return ExcuteResult.Success;
        }

        /// <summary>
        /// BLT检查
        /// </summary>
        /// <param name="average">平均值</param>
        /// <param name="range">极差</param>
        /// <returns>结果</returns>
        private ExcuteResult BLTCheck(List<double> height1, List<double> height2)
        {
            // 计算差值
            List<double> heightOffsetList = new List<double>();
            for (int i = 0; i < height2.Count; i++)
            {
                heightOffsetList.Add(Math.Round(height2[i] - height1[i], 3));
            }

            // 计算平均值
            double average = heightOffsetList.Average();

            // 计算极差
            double range = heightOffsetList.Max() - heightOffsetList.Min();

            string alarmMessage = null;

            if (Math.Abs(average - this.PostBondInspection.BLTStandard) > this.PostBondInspection.MaxBLTOffset)
            {
                alarmMessage += $"当前厚度为{average}mm，标准厚度为{this.PostBondInspection.BLTStandard}mm，高度差超出最大BLT偏移量{this.PostBondInspection.MaxBLTOffset}\r\n";
            }

            // 计算极差
            if (Math.Abs(range) > this.PostBondInspection.MaxSurfaceSlope)
            {
                alarmMessage += $"当前4个角的高度差分别为{heightOffsetList[0]}mm，{heightOffsetList[1]}mm，{heightOffsetList[2]}mm,{heightOffsetList[3]}mm,\r\n 极差为{range}mm，倾斜最大值为{this.PostBondInspection.MaxSurfaceSlope}mm";
            }

            if (string.IsNullOrEmpty(alarmMessage))
            {
                this.BondPosition.S2FinishedStep(this.ActionNodesService.GetCurrentProcessStepName());
                this.PostBondInspection.FinishedDefect();
                return ExcuteResult.Success;
            }

            // 相机移动到这个位置，方便用户查看
            System2Domain.GetInstance().SetVisionBLTHardware(this.PostBondInspection);
            System2Domain.GetInstance().S2DispenseController.CloseDispenseHeightMeasurementCylinder();
            System2Domain.GetInstance().BondModuleController.CameraMoveToG0Pos(this.BondPosition.CoordinateSystem.SelfPosToG0(new Galaxy2.Infrastructure.CommonModel.AKRSPoint3D(0, 0, height2.Average())));

            (DialogResult dialog, BaseAlgResult matchResult) result = UcMainSystem.VisionAlarmLockFunc(
                null,
                alarmMessage,
                "BLT检查", System2Domain.GetInstance().BondModuleController.GetHardware());
            if (result.dialog == DialogResult.Abort)
            {
                StatisticsDomain.GetInstance().PostBondFailedCount++;
                Machine.GetInstance().Stop();
                return ExcuteResult.Abort;
            }
            else if (result.dialog == DialogResult.Ignore)
            {
                StatisticsDomain.GetInstance().PostBondFailedCount++;
                this.BondPosition.SetMatterDisable();
                return ExcuteResult.Success;
            }
            else if (result.dialog == DialogResult.OK)
            {
                this.BondPosition.S2FinishedStep(this.ActionNodesService.GetCurrentProcessStepName());
                this.PostBondInspection.FinishedDefect();
                return ExcuteResult.Success;
            }
            else
            {
                StatisticsDomain.GetInstance().PostBondFailedCount++;
                Machine.GetInstance().Stop();
                return ExcuteResult.Abort;
            }
        }
    }
}

