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
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models.ActionNodes;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.BondSystem.Tasks;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.CalibSystem.Services;
using AKRS.ZX2200.DispenseSystem.Services;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Service;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using AKRS.ZX2200.TransportUnitSystem.Service;
using DevExpress.XtraEditors;
using log4net.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.Models
{
    using AKRS.Galaxy2.Component.Simple.MessageBox;
    using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
    using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.ZX2200.BondSystem.BondForce.Models.DevicePara;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Process;
    using AKRS.ZX2200.SupportFeature.Statistics;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using DevExpress.Charts.Native;
    using DevExpress.Utils.Internal.DTE;
    using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Media.Media3D;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
    using TransportUnit = AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit;
    using UcMainSystem = AKRS.ZX2200.Main.Controls.Ucmain.MainControls.UcMainSystem;

    /// <summary>
    /// 固晶域
    /// </summary>
    public class System2Domain : SingletonNoSave<System2Domain>
    {
        /// <summary>
        /// 系统2控制器
        /// </summary>
        public System2Controller System2Controller { get; set; } = new System2Controller();

        /// <summary>
        /// 吸嘴架控制器
        /// </summary>
        public NozzleShelfController NozzleShelfController { get; set; } = new NozzleShelfController();

        /// <summary>
        /// 上视控制器
        /// </summary>
        public UpLookController UpLookController { get; set; } = new UpLookController();

        /// <summary>
        /// Bond头控制器
        /// </summary>
        public BondHeadController BondHeadController { get; set; } = new BondHeadController();

        /// <summary>
        /// Bond模组控制器
        /// </summary>
        public BondModuleController BondModuleController { get; set; } = new BondModuleController();

        /// <summary>
        /// 中转台控制器
        /// </summary>
        public IPTController IPTController { get; set; } = new IPTController();

        /// <summary>
        /// 刮胶盘控制器
        /// </summary>
        public SlideFluxerController SlideFluxerController { get; set; } = new SlideFluxerController();

        /// <summary>
        /// 标定控制器
        /// </summary>
        private CalibController calibController = new CalibController();

        /// <summary>
        /// 系统2动作节点排序
        /// </summary>
        public S2ActionNodeController ActionNodesService { get; set; } = new S2ActionNodeController();

        /// <summary>
        /// 点胶控制器
        /// </summary>
        public S2DispenseController S2DispenseController { get; set; } = new S2DispenseController();

        /// <summary>
        /// 固晶程式
        /// </summary>
        [JsonIgnore]
        public BondProgram BondProgram => BondProgram.GetInstance();

        /// <summary>
        /// 固晶工作线程
        /// </summary>
        [JsonIgnore]
        public BondTask BondTask { get; set; } = new BondTask();

        /// <summary>
        /// 刮胶工作线程
        /// </summary>
        [JsonIgnore]
        public SlideFluxerTask SlideFluxerTask { get; set; } = new SlideFluxerTask();

        /// <summary>
        /// 固晶动作节点库
        /// </summary>
        [JsonIgnore]
        public BondActionNodeRepository BondActionNodeRepository { get; set; } =
            new BondActionNodeRepository(new List<string>());

        /// <summary>
        /// 当前传输单元/载具 
        /// </summary>
        [JsonIgnore]
        public TransportUnit TransportUnit =>
            TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

        /// <summary>
        /// 固晶配置参数，应用于所有程式
        /// </summary>
        [JsonIgnore]
        public System2Configuration System2Configuration => System2Configuration.GetInstance();

        /// <summary>
        /// 固晶设备参数
        /// </summary>
        [JsonIgnore]
        public BondDevicePara BondDevicePara => BondDevicePara.GetInstance();

        /// <summary>
        /// 吸嘴库
        /// </summary>
        [JsonIgnore]
        public NozzleRepository NozzleRepository => NozzleRepository.GetInstance();

        /// <summary>
        /// 锁
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// 系统2通用定位流程
        /// </summary>
        /// <param name="visionPosition">拍照点位G0</param>
        /// <param name="visionName">定位流程名称</param>
        /// <param name="pRName">pR名字</param>
        /// <param name="isZSafe">Z轴是否安全</param>
        /// <param name="cameraType">相机</param>
        /// <returns>结果</returns>
        public BaseAlgResult System2CommonVision(
            AKRSPoint3D visionPosition,
            string visionName,
            string pRName,
            bool isZSafe = false,
            CameraTypeEnum cameraType = CameraTypeEnum.BondCamera)
        {
            var hardware = cameraType == CameraTypeEnum.BondCamera ? this.BondModuleController.GetHardware() : this.UpLookController.GetHardware();

            if (visionPosition != null)
            {
                System2RunTimeProvider.RecordTime($"系统2{pRName}定位", $"准备运动到拍照位并设置光源");

                // 异步设置光源
                Task task = new Task(
                    () =>
                        {
                            // 寻找Pr模板
                            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(pRName);
                            if (pREntity != null)
                            {
                                // pREntity.SetHardware(hardware);
                                pREntity.SetLight(hardware);
                            }
                        });

                task.Start();

                if (isZSafe)
                {
                    this.BondModuleController.MoveToG0Pos(visionPosition.X, visionPosition.Y);
                }
                else
                {
                    // 轴移动到拍照位
                    this.BondModuleController.MoveToG0Pos(visionPosition);
                }

                System2RunTimeProvider.RecordTime($"系统2{pRName}定位", $"运动到拍照位结束");

                task.Wait();

                System2RunTimeProvider.RecordTime($"系统2{pRName}定位", $"等光源设置完成结束");
            }
            else
            {
                // 寻找Pr模板
                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(pRName);
                if (pREntity != null)
                {
                   //  pREntity.SetHardware(hardware);
                    pREntity.SetLight(hardware);
                }
            }

            // 如果是空跑模式，直接返回定位结果
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DryCycle)
            {
                return new MatchResult();
            }

            System2RunTimeProvider.RecordTime($"系统2{pRName}定位", $"定位延时：{System2Domain.GetInstance().System2Configuration.DownLookVisionDelay}ms");

            BaseAlgResult result = null;

            // 拍照到位停留,临时屏蔽
            DelayHelper.Delay(System2Domain.GetInstance().System2Configuration.DownLookVisionDelay);

            // 获取PR定位的结果,纠偏
            result = this.System2Controller.AdjustAction(visionName, pRName, cameraType, false);


            System2RunTimeProvider.RecordTime($"系统2{pRName}定位", $"定位完成，IsSuccess：{result?.IsSuccess}");

            return result;
        }


        /// <summary>
        /// 系统2（载具、基板、基岛）定位
        /// </summary>
        /// <param name="baseEntity">（载具、基板、基岛）实体</param>
        /// <param name="byMapping">是否根据Mapping走</param>
        public bool System2ObjectVision(BaseMatter baseEntity, bool byMapping = true)
        {
            return this.System2MatterVision(baseEntity, byMapping);
        }


        /// <summary>
        /// 系统2（载具、基板、基岛）定位
        /// </summary>
        /// <param name="baseEntity">（载具、基板、基岛）实体</param>
        /// <param name="byMapping">是否根据Mapping走</param>
        /// <returns>结果</returns>
        public bool System2MatterVision(BaseMatter baseEntity, bool byMapping = true)
        {
            // 如果没有开启定位，直接退出
            if (baseEntity.Config.LocateConfig.AdjustType == AdjustTypeEnum.None)
            {
                baseEntity.BaseInfo.IsVisioned = true;
                return true;
            }

            // 被屏蔽则不做
            if (baseEntity.MatterProductState == MatterProductState.Disable
                && byMapping)
            {
                return true;
            }

            #region 一点定位

            System2RunTimeProvider.RecordTime("系统2定位", $"视觉Mark1定位开始");

            // P1定位
            if (!this.System2VisionByPos(baseEntity, TuService.NormalVisionType.P1Vision))
            {
                StatisticsDomain.GetInstance().AdjustFailedCount++;

                System2RunTimeProvider.RecordTime("系统2定位", $"视觉Mark1定位失败");

                return false;
            }

            System2RunTimeProvider.RecordTime("系统2定位", $"视觉Mark1定位成功");

            if (baseEntity.Config.LocateConfig.AdjustType == AdjustTypeEnum.OnePoint)
            {
                // 纠偏计算
                TuService.UpdateEntity(baseEntity, TuService.NormalVisionType.P1Vision);
                return true;
            }

            #endregion

            #region 两点定位

            System2RunTimeProvider.RecordTime("系统2定位", $"视觉Mark2定位开始");

            // P2定位
            if (!this.System2VisionByPos(baseEntity, TuService.NormalVisionType.P2Vision))
            {
                StatisticsDomain.GetInstance().AdjustFailedCount++;

                System2RunTimeProvider.RecordTime("系统2定位", $"视觉Mark2定位失败");
                return false;
            }

            System2RunTimeProvider.RecordTime("系统2定位", $"视觉Mark2定位成功");

            if (baseEntity.Config.LocateConfig.AdjustType == AdjustTypeEnum.TwoPoints)
            {
                if (baseEntity.Config.LocateConfig.DistanceCheck)
                {
                    System2RunTimeProvider.RecordTime("系统2定位", $"两点定位距离检查开始");
                ReCheck:
                    // 距离检查
                    DialogResult dialogResult = VisionService.DistanceCheck(baseEntity);

                    if (dialogResult == DialogResult.Abort)
                    {
                        Machine.GetInstance().Stop();
                        return false;
                    }
                    else if (dialogResult == DialogResult.Retry)
                    {
                        if (!this.System2VisionByPos(baseEntity, TuService.NormalVisionType.P1Vision))
                        {
                            Machine.GetInstance().Stop();
                            return false;
                        }

                        if (!this.System2VisionByPos(baseEntity, TuService.NormalVisionType.P2Vision))
                        {
                            Machine.GetInstance().Stop();
                            return false;
                        }

                        goto ReCheck;
                    }
                    else if (dialogResult == DialogResult.None)
                    {
                        baseEntity.MatterProductState = MatterProductState.Disable;
                        return true;
                    }

                    System2RunTimeProvider.RecordTime(
                        "系统2定位",
                        $"两点定位距离检查结束。示教时的距离为{baseEntity.BaseInfo.LocateResultInfo.OldTwoPointDistance}，现在距离为{baseEntity.BaseInfo.LocateResultInfo.TwoPointDistance}");
                }

                // 纠偏计算
                TuService.UpdateEntity(baseEntity, TuService.NormalVisionType.P2Vision);

                return true;
            }

            #endregion

            #region 三点定位

            System2RunTimeProvider.RecordTime("系统2定位", $"视觉Mark3定位开始");

            // P3定位
            if (!this.System2VisionByPos(baseEntity, TuService.NormalVisionType.P3Vision))
            {
                StatisticsDomain.GetInstance().AdjustFailedCount++;

                System2RunTimeProvider.RecordTime("系统2定位", $"视觉Mark3定位失败");
                return false;
            }

            System2RunTimeProvider.RecordTime("系统2定位", $"视觉Mark3定位成功");

            if (baseEntity.Config.LocateConfig.AdjustType == AdjustTypeEnum.ThreePoints)
            {
                // 纠偏计算
                TuService.UpdateEntity(baseEntity, TuService.NormalVisionType.P3Vision);

                return true;
            }

            #endregion

            #region 四点定位

            // P4定位
            if (!this.System2VisionByPos(baseEntity, TuService.NormalVisionType.P4Vision))
            {
                StatisticsDomain.GetInstance().AdjustFailedCount++;

                System2RunTimeProvider.RecordTime("系统2定位", $"视觉Mark4定位失败");
                return false;
            }

            System2RunTimeProvider.RecordTime("系统2定位", $"视觉Mark4定位开始");

            if (baseEntity.Config.LocateConfig.AdjustType == AdjustTypeEnum.FourPoints)
            {
                // 纠偏计算
                TuService.UpdateEntity(baseEntity, TuService.NormalVisionType.P4Vision);

                return true;
            }

            System2RunTimeProvider.RecordTime("系统2定位", $"视觉Mark4定位成功");

            return false;

            #endregion
        }

        /// <summary>
        /// 系统2产品定位
        /// </summary>
        /// <param name="baseEntity">实体</param>
        /// <param name="type">类型</param>
        /// <param name="manuelVision">手动对点</param>
        /// <returns>结果</returns>
        private bool System2VisionByPos(BaseMatter baseEntity, TuService.NormalVisionType type, bool manuelVision = false)
        {
            // 根据信息获取坐标
            (AKRSPoint3D point, string pRName,bool autofocus) = TuService.GetVisionInfo(baseEntity, type);

            // 转换到G0坐标上面去
            AKRSPoint3D p1VisionPosInG01 =
                baseEntity.CoordinateSystem.SelfPosToG0(point);

            // 添加差值
            p1VisionPosInG01 = p1VisionPosInG01 - BondDevicePara.BondHeadParam.HeadToCameraOffset;

            // 定位结果
            MatchResult matchResult1 = null;

            #region 定位

            if (!manuelVision)
            {
                System2RunTimeProvider.RecordTime("系统2定位", $"准备移动到点位：{p1VisionPosInG01} 执行定位");

                matchResult1 = (MatchResult)this.System2Controller.BondCameraVision(p1VisionPosInG01, pRName, false, false, autofocus);

                System2RunTimeProvider.RecordTime("系统2定位", $"准备移动到点位：{p1VisionPosInG01} 完成定位");

                #region 四周定位

                if (baseEntity.Config.LocateConfig.IsAroundLocate && matchResult1 == null)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (i == 0)
                        {
                            // 执行定位
                            matchResult1 = (MatchResult)this.System2Controller.BondCameraVision(
                                new AKRSPoint3D(
                                    p1VisionPosInG01.X + baseEntity.Config.LocateConfig.AroundLocateDistance,
                                    p1VisionPosInG01.Y,
                                    p1VisionPosInG01.Z),
                                pRName);

                            if (matchResult1 != null)
                            {
                                break;
                            }
                        }
                        else if (i == 1)
                        {
                            // 执行定位
                            matchResult1 = (MatchResult)this.System2Controller.BondCameraVision(
                                new AKRSPoint3D(p1VisionPosInG01.X - baseEntity.Config.LocateConfig.AroundLocateDistance, p1VisionPosInG01.Y, p1VisionPosInG01.Z),
                                pRName);

                            if (matchResult1 != null)
                            {
                                break;
                            }
                        }
                        else if (i == 2)
                        {
                            // 执行定位
                            matchResult1 = (MatchResult)this.System2Controller.BondCameraVision(
                                new AKRSPoint3D(
                                    p1VisionPosInG01.X,
                                    p1VisionPosInG01.Y + baseEntity.Config.LocateConfig.AroundLocateDistance,
                                    p1VisionPosInG01.Z),
                                pRName);

                            if (matchResult1 != null)
                            {
                                break;
                            }
                        }
                        else if (i == 3)
                        {
                            // 执行定位
                            matchResult1 = (MatchResult)this.System2Controller.BondCameraVision(
                                new AKRSPoint3D(
                                    p1VisionPosInG01.X,
                                    p1VisionPosInG01.Y - baseEntity.Config.LocateConfig.AroundLocateDistance,
                                    p1VisionPosInG01.Z),
                                pRName);

                            if (matchResult1 != null)
                            {
                                break;
                            }
                        }
                    }
                }

                #endregion
            }
            else
            {
                this.BondModuleController.MoveToG0Pos(p1VisionPosInG01);
            }

            // 结果判断
            if (matchResult1 == null)
            {
                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(pRName);

                (DialogResult dialog, BaseAlgResult matchResult) result =
                    UcMainSystem.VisionAlarmLockFunc(pREntity, $"{baseEntity.MatterTypeEnum}定位失败。 位置：" + baseEntity.Name, baseEntity.Name, this.BondModuleController.GetHardware());

                if (result.dialog == DialogResult.OK)
                {
                    matchResult1 = (MatchResult)result.matchResult;
                }
                else if (result.dialog == DialogResult.Ignore)
                {
                    baseEntity.SetMatterDisable();
                    return false;
                }
                else if (result.dialog == DialogResult.Abort)
                {
                    Machine.GetInstance().Stop();
                    return false;
                }
                else
                {
                    return false;
                }
            }

            #endregion

            #region 信息保存

            // 定位结果转换
            AKRSPoint3D realPoint3DInG0 = this.System2Controller.GetBondVisionResultPos(matchResult1);

            // 转到自己的坐标系
            realPoint3DInG0 = realPoint3DInG0 + BondDevicePara.BondHeadParam.HeadToCameraOffset;

            AKRSPoint3D realPoint3D = baseEntity.CoordinateSystem.G0PosToSelf(realPoint3DInG0);

            VisionResult visionResult = TuService.ChooseInfoByType(type, baseEntity);

            visionResult.SaveInfo(matchResult1, realPoint3D, realPoint3DInG0, System2Module.GetInstance().BondModule.GetG0RealPosition());
            return true;

            #endregion
        }

        /// <summary>
        /// 线程启动前自检
        /// </summary>
        /// <returns>结果</returns>
        public bool CheckIsReady()
        {
            // 开焊头吸附
            this.BondHeadController.OpenBondHeadVaccum();

            if (!System2Configuration.GetInstance().IsActiveSelfCheck)
            {
                return true;
            }

            bool ret;

            // 程式检查
            ret = this.BondProgram.SelfCheck();
            if (!ret)
            {
                return false;
            }

            // ProcessStepProgram检查
            ret = this.ActionNodesService.SelfCheck();
            if (!ret)
            {
                return false;
            }

            // 设备参数检查
            if (this.BondDevicePara.ULMPara.IsEmpty())
            {
                // 报警
                AKRSXtraMessageBox.Show(
                    $"启动前请先示教ULM运动的点位!",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            // 设备参数检查
            if (this.BondDevicePara.BondHeadParam.IsEmpty())
            {
                AKRSXtraMessageBox.Show(
                    $"启动前请先示教系统2的点位!",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            // 设备参数检查
            if (this.BondDevicePara.NozzleShelfParam.IsEmpty() && System2Configuration.GetInstance().IsToolBankEnable)
            {
                AKRSXtraMessageBox.Show(
                    $"启动前请先示教吸嘴架!",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            // 设备参数检查
            if (ForceConfig.GetInstance().IsEmpty())
            {
                foreach (var item in WaferSystemProgram.GetInstance().GetCarriers())
                {
                    if (item.IsUseForceControl)
                    {
                        AKRSXtraMessageBox.Show(
                            $"芯片:{item.Name} 有使用力控功能，启动前请先进行力控标定!",
                            "错误",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        return false;
                    }
                }
            }

            // 检查温漂点位
            if (System2Configuration.GetInstance().IsActiveDriftCompensate)
            {
                if (this.BondDevicePara.BondHeadParam.UpLookMarkVisionPos1.IsEmpty)
                {
                    AKRSXtraMessageBox.Show(
                        $"启动前请先示教温漂定位的点位!",
                        "错误",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return false;
                }
            }

            // 检查温漂点位
            if (System2Configuration.GetInstance().IsActivePickUpCompensate)
            {
                if (this.BondDevicePara.BondHeadParam.PickMarkVisionPos1.IsEmpty)
                {
                    AKRSXtraMessageBox.Show(
                        $"启动前请先示教取片偏移的点位!",
                        "错误",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return false;
                }
            }

            // 先检查吸嘴架硬件
            if (!this.NozzleShelfController.CheckToolBankConfiguration())
            {
                return false;
            }

            if (System2Configuration.GetInstance().IsActiveToolDetection)
            {
                // 1.是否有touchdown  2.所需要的吸嘴是否在吸嘴架上
                // 判断焊头上有没有吸嘴， 如果有和tool bank的配置是否对应 如果不对应则报警，如果对应则先放回去。
                bool isToolOnHeadLogic = this.BondHeadController.IsToolOnBondheadLogic();
                bool isToolOnHeadPhy = this.BondHeadController.IsToolOnBondheadPhy();

                ret = isToolOnHeadLogic == isToolOnHeadPhy;

                if (isToolOnHeadPhy == true && isToolOnHeadLogic == false)
                {
                    AKRSXtraMessageBox.Show(
                        $"焊头上检测到吸嘴，与程式冲突!",
                        "错误",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return false;
                }
                else if (isToolOnHeadPhy == false && isToolOnHeadLogic == true)
                {
                    // 如果焊头检测没有，但记忆中有吸嘴
                    if (AKRSXtraMessageBox.Show(
                            $"焊头上没有检测到吸嘴，与程式冲突!\r\nYes:设置焊头上没有吸嘴\r\nNo:终止工作",
                            "错误",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.BondHeadController.SetCurrentNozzleName(string.Empty);
                    }
                    else
                    {
                        return false;
                    }
                }
            }


            // 检查焊头上是否有料
            if (System2RunTimeProvider.IsMaterialOnBondhead)
            {
                DialogResult dia = AKRSXtraMessageBox.Show(
                    $"记忆中吸嘴上有料，请选择如何处理!\r\nOK:抛料\r\nNo:忽略",
                    "错误",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question);

                if (dia == DialogResult.OK)
                {
                    this.BondModuleController.ThrowAction();
                }
            }

            // 预点胶是否可以执行
            if (this.BondActionNodeRepository.S2PreDispenseActionNode.IsAlarm())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 单步工作
        /// </summary>
        /// <returns>结果</returns>
        public bool WaitSingleStep()
        {
            if (MachineStateModel.GetInstance().IsSingleStepWork)
            {
                if (!SignalPool.GetInstance().System2SingleStepSignal.WaitSingleStep())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 系统2测高
        /// 针对于TU，Sub，Module，BP
        /// </summary>
        /// <param name="baseEntity">对象</param>
        /// <returns>结果</returns>
        public ExcuteResult MatterHeightMeasurePoints(BaseMatter baseEntity)
        {
            // 没有定位点直接返回
            if (baseEntity.Config.HeightMeasurementPoints.Count == 0)
            {
                return ExcuteResult.Success;
            }

            // 没有测高功能没有开启，直接返回
            if (!baseEntity.Config.MeasureHeightInSystem2)
            {
                return ExcuteResult.Success;
            }

            System2RunTimeProvider.RecordTime($"系统2{baseEntity.Name}测高", $"{baseEntity.Name}测高结束");

            // 结果的集合
            List<double> resultList = new List<double>();

            if (MachineHardwareConfiguration.GetInstance().IsSystem2ConfigLaserMh)
            {
                foreach (AKRSPoint3D pos in baseEntity.Config.HeightMeasurementPoints)
                {
                    // 单步工作
                    if (!System1Domain.GetInstance().WaitSingleStep())
                    {
                        return ExcuteResult.Abort;
                    }

                    // 计算G0坐标
                    AKRSPoint3D g0Pos = baseEntity.CoordinateSystem.SelfPosToG0(pos);

                    #region 移动到位

                    System2RunTimeProvider.RecordTime($"系统2{baseEntity.Name}测高", $"轴开始移动到测高位置{g0Pos}");

                    // 移动到该位置
                    System2Domain.GetInstance().S2DispenseController.MeasureHeightMoveToG0Pos(g0Pos);

                    System2RunTimeProvider.RecordTime($"系统2{baseEntity.Name}测高", $"轴移动到测高位置完成{g0Pos}");

                    #endregion

                    #region 打开测高气缸

                    System2RunTimeProvider.RecordTime($"系统2{baseEntity.Name}测高", $"准备打开测高气缸");

                    // 打开测高气缸
                    System2Domain.GetInstance().S2DispenseController.OpenDispenseHeightMeasurementCylinder();

                    System2RunTimeProvider.RecordTime($"系统2{baseEntity.Name}测高", $"打开测高气缸完成");

                    #endregion

                    #region 测高延迟

                    System2RunTimeProvider.RecordTime($"系统2{baseEntity.Name}测高", $"开始到位延迟");

                    // 到位等待
                    Thread.Sleep(BondDevicePara.S2DispenseDevicePara.LaserMhDelayTime);

                    System2RunTimeProvider.RecordTime(
                        $"系统2{baseEntity.Name}测高",
                        $"结束到位延迟完成，延时为{BondDevicePara.S2DispenseDevicePara.LaserMhDelayTime}");

                #endregion

                RetryCommand:

                    #region 测高

                    System2RunTimeProvider.RecordTime($"系统2{baseEntity.Name}测高", $"开始测高");

                    // 测高
                    double heightInMachine = this.S2DispenseController.LaserMeasureHeight();

                    System2RunTimeProvider.RecordTime($"系统2{baseEntity.Name}测高", $"测高完成，高度为{heightInMachine}");

                    #endregion

                    #region 数值报警

                    // 高度预警
                    if (Math.Abs(heightInMachine) > BondDevicePara.S2DispenseDevicePara.MeasureHeightAlarmDistance
                        || double.IsNaN(heightInMachine))
                    {
                        DialogResult dialogResult = AKRSMessageBoxExt.Show(
                            $"测高数值为{heightInMachine}mm，超出设置阈值{BondDevicePara.S2DispenseDevicePara.MeasureHeightAlarmDistance}\r\n"
                            + $"请选择如何处理",
                            "测高高度过大报警",
                            new string[] { "重试", "跳过", "终止" },
                            new DialogResult[] { DialogResult.Retry, DialogResult.Ignore, DialogResult.Abort },
                            AlarmLevel.FirstLevel);

                        switch (dialogResult)
                        {
                            case DialogResult.Retry:
                                goto RetryCommand;
                            case DialogResult.Ignore:
                                baseEntity.SetMatterDisable();
                                return ExcuteResult.Success;
                            case DialogResult.Abort:
                                return ExcuteResult.Exception;
                        }
                    }

                    #endregion

                    resultList.Add(-heightInMachine + pos.Z);
                }
            }
            else
            {
                foreach (AKRSPoint3D pos in baseEntity.Config.HeightMeasurementPoints)
                {
                    // 单步工作
                    if (!System1Domain.GetInstance().WaitSingleStep())
                    {
                        return ExcuteResult.Abort;
                    }

                    // 计算G0坐标
                    AKRSPoint3D g0Pos = baseEntity.CoordinateSystem.SelfPosToG0(pos);

                    #region 移动到位

                    System2RunTimeProvider.RecordTime($"系统2{baseEntity.Name}测高", $"轴开始移动到测高位置{g0Pos}");

                    // 移动到该位置
                    System2Domain.GetInstance().BondModuleController
                        .MoveToG0Pos(new AKRSPoint3D(g0Pos.X, g0Pos.Y, g0Pos.Z + this.BondDevicePara.BondHeadParam.MeasureHeightStartDistance));

                    System2RunTimeProvider.RecordTime($"系统2{baseEntity.Name}测高", $"轴移动到测高位置完成{g0Pos}");

                #endregion

                RetryCommand:

                    #region 测高

                    System2RunTimeProvider.RecordTime($"系统2{baseEntity.Name}测高", $"开始测高");

                    double left = System2Module.GetInstance().BondModule.BondHead.AxisZ.GetRealPosition();
                    (ExcuteResult result, double height) item = System2Domain.GetInstance().BondHeadController.MeasureHeight(
                        left,
                        HeightMeasurementFunctionEnum.WithTDSensor,
                        this.BondDevicePara.BondHeadParam.MeasureHeightStartDistance + this.BondDevicePara.BondHeadParam.MeasureHeightEndDistance,
                         this.BondDevicePara.BondHeadParam.MeasureHeightSpeed);

                    // 转到G0
                    double heightInG0 = System2Module.GetInstance().BondModule
                                                 .ConvertMachineToG0Pos(new AKRSPoint3D(0, 0, item.height)).Z;

                    System2RunTimeProvider.RecordTime($"系统2{baseEntity.Name}测高", $"测高完成，高度为{heightInG0}");

                    #endregion

                    #region 报警

                    // 高度预警
                    if (item.result != ExcuteResult.Success || Math.Abs(heightInG0 - g0Pos.Z) > BondDevicePara.S2DispenseDevicePara.MeasureHeightAlarmDistance)
                    {
                        DialogResult dialogResult = AKRSMessageBoxExt.Show(
                            $"测高偏差数值为{heightInG0 - g0Pos.Z}mm，超出设置阈值{BondDevicePara.S2DispenseDevicePara.MeasureHeightAlarmDistance}\r\n"
                            + $"请选择如何处理",
                            "测高高度过大报警",
                            new string[] { "重试", "跳过", "终止" },
                            new DialogResult[] { DialogResult.Retry, DialogResult.Ignore, DialogResult.Abort },
                            AlarmLevel.FirstLevel);

                        switch (dialogResult)
                        {
                            case DialogResult.Retry:
                                goto RetryCommand;
                            case DialogResult.Ignore:
                                baseEntity.SetMatterDisable();
                                return ExcuteResult.Success;
                            case DialogResult.Abort:
                                return ExcuteResult.Exception;
                        }
                    }

                    #endregion

                    resultList.Add(heightInG0);
                }
            }


            // 计算测高点的
            double averageHeight = resultList.Average();

            // 计算之前的高度
            double averageHeightOld = baseEntity.Config.HeightMeasurementPoints.Select(it => it.Z).Average();

            // 现在的高度减去之前的高度
            double addHeight = averageHeight - averageHeightOld;

            // 保存结果
            baseEntity.BaseInfo.MeasureHeightResult = addHeight;

            System2RunTimeProvider.RecordTime($"系统2{baseEntity.Name}测高", $"全部测高完成，均值为{addHeight}");

            if (double.IsNaN(addHeight))
            {
                throw new Exception("测高结果为NaN，请检查激光传感器是否存在问题");
            }

            // 更新差值
            baseEntity.CoordinateSystem.Distance.Z += addHeight;

            baseEntity.BaseInfo.IsMeasureHeight = true;

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 系统2身份识别
        /// 针对于TU，Sub，Module，BP
        /// </summary>
        /// <param name="baseEntity">对象</param>
        /// <returns>结果</returns>
        public ExcuteResult MatterIdentity(BaseMatter baseEntity)
        {
            #region 数据判断和准备

            // 没有定位点直接返回
            if (baseEntity.Config.IdentityConfig.Identification == IdentificationEnum.Off)
            {
                return ExcuteResult.Success;
            }

            AKRSPoint3D point = baseEntity.Config.IdentityConfig.P1VisionRelativePos;

            string pRName = baseEntity.Config.IdentityConfig.P1PRName;

            // 转换到G0坐标上面去
            AKRSPoint3D p1VisionPosInG01 =
                baseEntity.CoordinateSystem.SelfPosToG0(point);

            // 添加差值
            p1VisionPosInG01 = p1VisionPosInG01 - BondDevicePara.BondHeadParam.HeadToCameraOffset;

            #endregion

            #region 定位

            System2RunTimeProvider.RecordTime("系统2ID识别", $"准备移动到点位：{p1VisionPosInG01} 执行定位");

            CodeResult codeResult = (CodeResult)this.System2Controller.BondCameraVision(p1VisionPosInG01, pRName);

            System2RunTimeProvider.RecordTime("系统2ID识别", $"{p1VisionPosInG01} 完成定位");

            #endregion

            #region 定位失败结果处理

            // 结果判断
            if (codeResult == null)
            {
                System2RunTimeProvider.RecordTime("系统2ID识别", $"{p1VisionPosInG01}身份识别失败，报警手动处理");

                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(pRName);

                (DialogResult dialog, BaseAlgResult matchResult) result =
                    UcMainSystem.VisionAlarmLockFunc(pREntity, $"{baseEntity.MatterTypeEnum}ID识别失败。" + baseEntity.Name, baseEntity.Name, this.BondModuleController.GetHardware());

                if (result.dialog == DialogResult.OK)
                {
                    System2RunTimeProvider.RecordTime("系统2ID识别", $"{p1VisionPosInG01}身份识别失败，手动识别成功");
                    codeResult = (CodeResult)result.matchResult;
                }
                else if (result.dialog == DialogResult.Ignore)
                {
                    System2RunTimeProvider.RecordTime("系统2ID识别", $"{p1VisionPosInG01}身份识别失败，手动识别失败");
                    baseEntity.SetMatterDisable();
                    return ExcuteResult.Success;
                }
                else if (result.dialog == DialogResult.Abort)
                {
                    System2RunTimeProvider.RecordTime("系统2ID识别", $"{p1VisionPosInG01}身份识别失败，停止");
                    Machine.GetInstance().Stop();
                    return ExcuteResult.Abort;
                }
                else
                {
                    return ExcuteResult.Abort;
                }
            }

            #endregion

            baseEntity.BaseInfo.IsIdentity = true;
            baseEntity.BaseInfo.IdentityCode = codeResult.CodeValue;
            return ExcuteResult.Success;
        }

        /// <summary>
        /// 设置看焊点的硬件
        /// </summary>
        /// <param name="bondPosition">焊点</param>
        public void SetVisionBondPositionHardware(BondPosition bondPosition)
        {
            try
            {
                foreach (var lightName in bondPosition.SingleBondPositionConfig.VisionLightValue)
                {
                    Galaxy2.LogicHardware.Hardwares.LightControllers.Light light = HardwareRepositoryService.GetHardware<Galaxy2.LogicHardware.Hardwares.LightControllers.Light>(lightName.Key);

                    if (light != null)
                    {
                        light.SetIntensity(lightName.Value);
                    }
                }

                AKRSCamera camera = System2Module.GetInstance().BondModule.BondCamera;

                bool isExposeSuccess = camera.SetExposureTime(bondPosition.SingleBondPositionConfig.VisionExposure);

                bool isGainSuccess = camera.SetGain(bondPosition.SingleBondPositionConfig.VisionGain);

                bool isGammaSuccess = camera.SetGamma(bondPosition.SingleBondPositionConfig.VisionGamma / 10.0);

                if (!isExposeSuccess || !isGainSuccess || !isGammaSuccess)
                {
                    throw new Exception("设置相机参数失败");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("查看焊点失败" + ex.Message);
            }
        }

        /// <summary>
        /// 设置BLT焊点硬件
        /// </summary>
        /// <param name="postBondInspection">检测对象</param>
        /// <exception cref="Exception">异常</exception>
        public void SetVisionBLTHardware(PostBondInspection postBondInspection)
        {
            try
            {
                foreach (var lightName in postBondInspection.BLTVisionLightValue)
                {
                    Galaxy2.LogicHardware.Hardwares.LightControllers.Light light = HardwareRepositoryService.GetHardware<Galaxy2.LogicHardware.Hardwares.LightControllers.Light>(lightName.Key);

                    if (light != null)
                    {
                        light.SetIntensity(lightName.Value);
                    }
                }

                AKRSCamera camera = System2Module.GetInstance().BondModule.BondCamera;

                bool isExposeSuccess = camera.SetExposureTime(postBondInspection.BLTVisionExposure);

                bool isGainSuccess = camera.SetGain(postBondInspection.BLTVisionGain);

                bool isGammaSuccess = camera.SetGamma(postBondInspection.BLTVisionGamma / 10.0);

                if (!isExposeSuccess || !isGainSuccess || !isGammaSuccess)
                {
                    throw new Exception("设置相机参数失败");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("查看BLT芯片失败" + ex.Message);
            }
        }

        /// <summary>
        /// 保存视觉硬件参数
        /// </summary>
        public void SaveVisionHardwareParameter(SingleBondPositionConfig singleBondPositionConfig)
        {
            BondModule bondModule = System2Module.GetInstance().BondModule;
            List<Galaxy2.LogicHardware.Hardwares.LightControllers.Light> lights = bondModule.GetLights();
            Dictionary<string, int> VisionLightValue = new Dictionary<string, int>();

            foreach (Galaxy2.LogicHardware.Hardwares.LightControllers.Light light in lights)
            {
                if (light != null)
                {
                    VisionLightValue.Add(light.HardwareName, light.GetCacheIntensity());
                }      
            }

            singleBondPositionConfig.VisionLightValue = VisionLightValue;
            singleBondPositionConfig.VisionExposure = bondModule.BondCamera.GetExposureTime();
            singleBondPositionConfig.VisionGain = bondModule.BondCamera.GetGain();
            singleBondPositionConfig.VisionGamma = bondModule.BondCamera.GetGamma();
        }

        /// <summary>
        /// 保存视觉硬件参数
        /// </summary>
        public void SaveBLTHardwareParameter(PostBondInspection postBondInspection)
        {
            BondModule bondModule = System2Module.GetInstance().BondModule;
            List<Galaxy2.LogicHardware.Hardwares.LightControllers.Light> lights = bondModule.GetLights();
            Dictionary<string, int> VisionLightValue = new Dictionary<string, int>();

            foreach (Galaxy2.LogicHardware.Hardwares.LightControllers.Light light in lights)
            {
                if (light != null)
                {
                    VisionLightValue.Add(light.HardwareName, light.GetCacheIntensity());
                }
            }

            postBondInspection.BLTVisionLightValue = VisionLightValue;
            postBondInspection.BLTVisionExposure = bondModule.BondCamera.GetExposureTime();
            postBondInspection.BLTVisionGain = bondModule.BondCamera.GetGain();
            postBondInspection.BLTVisionGamma = bondModule.BondCamera.GetGamma();
        }
    }
}