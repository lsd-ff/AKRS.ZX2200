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

namespace AKRS.ZX2200.BondSystem.Controllers
{
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

    /// <summary>
    ///  用于存放系统2定位相关方法
    /// </summary>
    public partial class System2Controller
    {
        /// <summary>
        /// 视觉纠偏定位动作
        /// </summary>
        /// <param name="visionName">视觉流程名称</param>
        /// <param name="pRName">pR名称</param>
        /// <param name="cameraTypeEnum">相机类型</param>
        /// <param name="isSetLight">是否设置灯光</param>
        /// <returns>结果</returns>
        public BaseAlgResult AdjustAction(string visionName, string pRName, CameraTypeEnum cameraTypeEnum,
            bool isSetLight = true)
        {
            try
            {
                // 定位结果，如果为null，则是定位失败了
                BaseAlgResult matchResult = null;

                // 寻找Pr模板
                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(pRName);

                if (pREntity == null)
                {
                    AKRSMessageBoxExt.Show(
                        $"在{visionName}定位流程中未找到PR模板：{pRName}  , 设备将停止工作！\r\n",
                        "Alarm",
                        new string[] { "Yes" },
                        new DialogResult[] { DialogResult.Abort },
                        AlarmLevel.SecondLevel);

                    return null;
                }

                //// 设置硬件
                //if (cameraTypeEnum == CameraTypeEnum.UpLookCamera)
                //{
                //    this.upLookController.SetHardware(pRName);
                //}
                //else
                //{
                //    this.bondModuleController.SetHardware(pRName);
                //}

            Retry:
                PRHardware pRHardware = cameraTypeEnum == CameraTypeEnum.UpLookCamera ?
                    this.upLookController.GetHardware() : this.bondModuleController.GetHardware();

                // 开始定位
                ExcuteResult p1Result = pREntity.DoWork(pRHardware);

                // 结果判断
                if (p1Result != ExcuteResult.Success)
                {
                    (DialogResult dialog, BaseAlgResult matchResult) result = UcMainSystem.VisionAlarmLockFunc(
                        pREntity,
                        visionName,
                        $"视觉模板：固晶{pREntity.GetName()}定位失败", pRHardware);

                    if (result.dialog == DialogResult.OK)
                    {
                        matchResult = result.matchResult;
                    }
                    else if (result.dialog == DialogResult.Ignore)
                    {
                        return null;
                    }
                    else if (result.dialog == DialogResult.Abort)
                    {
                        Machine.GetInstance().Stop();
                        return null;
                    }
                    else
                    {
                        Machine.GetInstance().Stop();
                        return null;
                    }
                }

                //if (System2Configuration.GetInstance().IsActiveSaveImg)
                //{
                //    // 保存结果图片
                //    VisionService.SaveVisionImage(
                //        pREntity.AlgResult.OutPutImg1,
                //        new List<string>()
                //            {
                //                "Bond",
                //                TransportProgram.GetInstance().BondSubSectionProgram.TransportUnit?.Name,
                //                pREntity.GetName()
                //            },
                //        System2Domain.GetInstance().ActionNodesService.CurrentMatter(),
                //        pREntity.OriginalBmp);
                //}

                // 返回定位结果
                matchResult = pREntity.AlgResult;
                return matchResult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// bond定位
        /// </summary>
        /// <param name="visionPosition">定位的点位(G0)</param>
        /// <param name="pRName">pr的名称</param>
        /// <param name="isZSafe">Z向是否安全</param>
        /// <returns>结果</returns>
        public BaseAlgResult BondCameraVision(AKRSPoint3D visionPosition, string pRName, bool isZSafe = false, bool isDefect = false, bool autofocus = false)
        {
            bool needSetLight = true;

            // 寻找Pr模板
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(pRName);
            pREntity.IsDefectPR = isDefect;

            if (visionPosition != null)
            {
                #region 异步设置光源

                // 异步设置光源
                Task task = new(
                    () =>
                        {
                            System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"异步设置光源", true);

                            pREntity.SetLight(this.bondModuleController.GetHardware());

                            System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"异步设置光源完成", true);
                        });

                task.Start();

                #endregion

                #region 轴移动

                System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"准备移动到拍照位置");

                if (!isZSafe)
                {
                    this.bondModuleController.MoveToG0Pos(visionPosition);
                }
                else
                {
                    this.bondModuleController.MoveToG0PosWithoutSafe(visionPosition);
                }

                System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"准备运动到拍照位结束 + 点位{visionPosition}");

                task.Wait();

                System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"等待设置光源完成");

                needSetLight = false;

                #endregion

            }
            else
            {
                #region 同步设置光源

                System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"设置光源");
                pREntity.SetLight(this.bondModuleController.GetHardware());
                System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"设置光源完成");

                #endregion
            }

            if (autofocus)
            {
                // 自动聚焦
                this.bondHeadController.AutoFocus(CameraTypeEnum.BondCamera);
                visionPosition = this.bondModuleController.GetG0RealPosition();
            }

            // 如果是空跑模式，直接返回定位结果
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DryCycle)
            {
                return new MatchResult();
            }

            // 重试次数
            for (int i = 0; i < this.system2Configuration.VisionRetryTimesLimit; i++)
            {
                #region 延时

                System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"定位延时开始");

                // 拍照停留
                DelayHelper.Delay((int)this.system2Configuration.DownLookVisionDelay);

                System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"定位延时结束 ，延时为：{this.system2Configuration.DownLookVisionDelay}ms");

                #endregion

                #region 定位

                System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"第{i + 1}次定位开始");

                string matterName = System2Domain.GetInstance().ActionNodesService.CurrentMatter();

                // 执行定位
                BaseAlgResult baseAlg = VisionService.Vision(
                    pRName,
                    this.bondModuleController.GetHardware(),
                    "Bond",
                    matterName,
                    needSetLight);

                bool isSuccess = baseAlg != null;

                System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"第{i + 1}次定位完成，是否成功：{isSuccess}");

                #endregion
                
                // 结果判断
                if (baseAlg != null)
                {
                    return baseAlg;
                }

                System2RunTimeProvider.RecordTime($"系统2{pRName}定位", $"定位失败准备重试");
            }

            System2RunTimeProvider.RecordTime(
                $"系统2{pRName}定位",
                $"定位失败{this.system2Configuration.VisionRetryTimesLimit}次，认为处理");

            return null;
        }

        /// <summary>
        /// Bond相机胶量检测
        /// </summary>
        /// <param name="visionPosition">定位的点位(G0)</param>
        /// <param name="pRName">pr的名称</param>
        /// <param name="isZSafe">Z向是否安全</param>
        /// <returns>结果</returns>
        public List<BaseAlgResult> BondCameraVisionDefect(AKRSPoint3D visionPosition, string pRName, bool isZSafe = false,bool isDefect = false)
        {
            bool needSetLight = true;

            // 寻找Pr模板
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(pRName);
            pREntity.IsDefectPR = isDefect;

            if (visionPosition != null)
            {
                System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"准备运动到拍照位并设置光源");

                // 异步设置光源
                Task task = new(
                    () =>
                    {
                        pREntity.SetLight(this.bondModuleController.GetHardware());
                    });

                task.Start();

                if (!isZSafe)
                {
                    this.bondModuleController.MoveToG0Pos(visionPosition);

                    System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"准备运动到拍照位结束 + 点位{visionPosition}");
                }
                else
                {
                    // 直接移动XY轴
                    AKRSPoint3D targetPos = this.bondModuleController.ConvertG0ToMachinePos(visionPosition);
                    this.bondModuleController.MoveBondXY(targetPos.X, targetPos.Y);
                }

                task.Wait();

                System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"等待设置光源完成");

                needSetLight = false;
            }
            else
            {
                pREntity.SetLight(this.bondModuleController.GetHardware());
            }

            // 如果是空跑模式，直接返回定位结果
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DryCycle)
            {
                return null;
            }

            // 重试次数
            for (int i = 0; i < this.system2Configuration.VisionRetryTimesLimit; i++)
            {
                // 拍照停留
                DelayHelper.Delay((int)this.system2Configuration.DownLookVisionDelay);

                System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"定位延时：{this.system2Configuration.DownLookVisionDelay}ms");

                string matterName = System2Domain.GetInstance().ActionNodesService.CurrentMatter();

                // 执行定位
                List<BaseAlgResult> baseAlg = VisionService.VisionDefect(
                    pRName,
                    this.bondModuleController.GetHardware(),
                    "Bond",
                    matterName,
                    needSetLight);

                bool isSuccess = baseAlg != null;

                System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"第{i + 1}次定位完成，isSuccess：{isSuccess}");

                // 结果判断
                if (baseAlg != null)
                {
                    return baseAlg;
                }

                System2RunTimeProvider.RecordTime($"系统2{pRName}定位", $"定位失败准备重试");

                LogHelper.Post(Level.Info, $" {pRName}定位失败！重试：第{i + 1}次", LogCategory.Bond);
            }

            return null;
        }

        /// <summary>
        /// bond定位
        /// </summary>
        /// <param name="visionPosition">定位的点位</param>
        /// <param name="pRName">pr的名称</param>
        /// <param name="visionTime">拍照次数</param>
        /// <param name="isZSafe">Z向是否安全</param>
        /// <returns>结果</returns>
        public MatchResult BondCameraVision(AKRSPoint3D visionPosition, string pRName, int visionTime, bool isZSafe = false,
            IDataLog dataLog = null, string dataLogColName = "")
        {
            bool needSetLight = true;

            if (visionPosition != null)
            {
                System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"准备运动到拍照位并设置光源");

                // 异步设置光源
                Task task = new(
                    () =>
                    {
                        // 寻找Pr模板
                        PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(pRName);
                        if (pREntity != null)
                        {
                            pREntity.SetLight(this.bondModuleController.GetHardware());
                        }
                    });

                task.Start();


                if (!isZSafe)
                {
                    this.bondModuleController.MoveToG0Pos(visionPosition);

                    System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"准备运动到拍照位结束 + 点位{visionPosition}");
                }
                else
                {
                    // 直接移动XY轴
                    AKRSPoint3D targetPos = this.bondModuleController.ConvertG0ToMachinePos(visionPosition);
                    this.bondModuleController.MoveBondXY(targetPos.X, targetPos.Y);
                }

                task.Wait();

                System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"等待设置光源完成");

                needSetLight = false;
            }
            else
            {
                // 寻找Pr模板
                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(pRName);
                if (pREntity != null)
                {
                    pREntity.SetLight(this.bondModuleController.GetHardware());
                }
            }

            // 如果是空跑模式，直接返回定位结果
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DryCycle)
            {
                return new();
            }


            List<MatchResult> matchResultList = new();

            for (int j = 0; j < visionTime; j++)
            {
                // 重试次数
                for (int i = 0; i < this.system2Configuration.VisionRetryTimesLimit; i++)
                {
                    // 拍照停留
                    DelayHelper.Delay((int)this.system2Configuration.DownLookVisionDelay);

                    System2RunTimeProvider.RecordTime($"系统2-{pRName}定位",
                        $"定位延时：{this.system2Configuration.DownLookVisionDelay}ms");

                    string matterName = System2Domain.GetInstance().ActionNodesService.CurrentMatter();

                    // 执行定位
                    BaseAlgResult baseAlg = VisionService.Vision(
                        pRName,
                        this.bondModuleController.GetHardware(),
                        "Bond",
                        matterName,
                        needSetLight);

                    bool isSuccess = baseAlg != null;

                    System2RunTimeProvider.RecordTime($"系统2-{pRName}定位", $"第{i + 1}次定位完成，isSuccess：{isSuccess}");

                    // 结果判断
                    if (baseAlg != null)
                    {
                        matchResultList.Add((MatchResult)baseAlg);
                        dataLog?.AddData($"{dataLogColName}{pRName}第{j + 1}次定位",
                            new { X = ((MatchResult)baseAlg).CenterX, Y = ((MatchResult)baseAlg).CenterY });
                        break;
                    }

                    System2RunTimeProvider.RecordTime($"系统2{pRName}定位", $"定位失败准备重试");

                    LogHelper.Post(Level.Info, $" {pRName}定位失败！重试：第{i + 1}次", LogCategory.Bond);
                }
            }

            List<MatchResult> dataHandled = matchResultList.RemoveAbnormalDataIQR();
            MatchResult matchResult = new()
            {
                CenterX = dataHandled.Average(it => it.CenterX),
                CenterY = dataHandled.Average(it => it.CenterY),
                Angle = dataHandled.Average(it => it.Angle)
            };

            return matchResult;
        }

        /// <summary>
        /// 上视定位
        /// </summary>
        /// <param name="visionPosition">定位的点位</param>
        /// <param name="pRName">pr的名称</param>
        /// <param name="isZSafe">Z向是否安全</param>
        /// <returns>结果</returns>
        public BaseAlgResult UpLookCameraVision(AKRSPoint3D visionPosition, string pRName, bool isZSafe = false)
        {
            if (visionPosition != null)
            {
                if (!isZSafe)
                {
                    // 安全移动到拍照位
                    this.bondModuleController.MoveToG0Pos(visionPosition);
                }
                else
                {
                    // 直接移动XY轴
                    AKRSPoint3D targetPos = this.bondModuleController.ConvertG0ToMachinePos(visionPosition);
                    this.bondModuleController.MoveBondXY(targetPos.X, targetPos.Y);
                }

                // 打印
                LogHelper.Post(Level.Info,
                    $" {pRName}定位流程-移动到拍照位完成，此时各轴坐标：" +
                    $" X：{this.bondModuleController.GetAxisXRealPos()}," +
                    $" Y: {this.bondModuleController.GetAxisYRealPos()}" +
                    $" T: {this.bondHeadController.GetAxisTRealPos()}," +
                    $" Z: {this.bondHeadController.GetAxisZRealPos()} "
                    , LogCategory.Bond);
            }

            // 如果是空跑模式，直接返回定位结果
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DryCycle)
            {
                return new MatchResult();
            }

            // 重试次数
            for (int i = 0; i < this.system2Configuration.VisionRetryTimesLimit; i++)
            {
                // 拍照停留
                DelayHelper.Delay((int)this.system2Configuration.DownLookVisionDelay);

                string matterName = System2Domain.GetInstance().ActionNodesService.CurrentMatter();

                // 执行定位
                BaseAlgResult baseAlg = VisionService.Vision(
                    pRName,
                    this.upLookController.GetHardware(),
                    "UpLook",
                    matterName);

                // 结果判断
                if (baseAlg != null)
                {
                    return baseAlg;
                }
            }

            return null;
        }

        /// <summary>
        /// 上视定位（多次拍照取平均值）
        /// </summary>
        /// <param name="visionPosition">定位的点位</param>
        /// <param name="pRName">pr的名称</param>
        /// <param name="visionTime">拍照次数</param>
        /// <param name="isZSafe">Z向是否安全</param>
        /// <returns>结果</returns>
        public MatchResult UpLookCameraVision(AKRSPoint3D visionPosition, string pRName, int visionTime,
            bool isZSafe = false, IDataLog dataLog = null, string dataLogColName = "")
        {
            if (visionPosition != null)
            {
                if (!isZSafe)
                {
                    // 安全移动到拍照位
                    this.bondModuleController.MoveToG0Pos(visionPosition);
                }
                else
                {
                    // 直接移动XY轴
                    AKRSPoint3D targetPos = this.bondModuleController.ConvertG0ToMachinePos(visionPosition);
                    this.bondModuleController.MoveBondXY(targetPos.X, targetPos.Y);
                }

                // 打印
                LogHelper.Post(Level.Info,
                    $" {pRName}定位流程-移动到拍照位完成，此时各轴坐标：" +
                    $" X：{this.bondModuleController.GetAxisXRealPos()}," +
                    $" Y: {this.bondModuleController.GetAxisYRealPos()}" +
                    $" T: {this.bondHeadController.GetAxisTRealPos()}," +
                    $" Z: {this.bondHeadController.GetAxisZRealPos()} "
                    , LogCategory.Bond);
            }

            // 如果是空跑模式，直接返回定位结果
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DryCycle)
            {
                return new();
            }


            List<MatchResult> matchResultList = new();

            for (int j = 0; j < visionTime; j++)
            {
                // 重试次数
                for (int i = 0; i < this.system2Configuration.VisionRetryTimesLimit; i++)
                {
                    // 拍照停留
                    DelayHelper.Delay((int)this.system2Configuration.DownLookVisionDelay);

                    string matterName = System2Domain.GetInstance().ActionNodesService.CurrentMatter();

                    // 执行定位
                    BaseAlgResult baseAlg = VisionService.Vision(
                        pRName,
                        this.upLookController.GetHardware(),
                        "UpLook",
                        matterName);

                    // 结果判断
                    if (baseAlg != null)
                    {
                        matchResultList.Add((MatchResult)baseAlg);
                        dataLog?.AddData($"{dataLogColName}{pRName}第{j + 1}次定位",
                            new { X = ((MatchResult)baseAlg).CenterX, Y = ((MatchResult)baseAlg).CenterY });
                        break;
                    }
                }
            }

            List<MatchResult> dataHandled = matchResultList.RemoveAbnormalDataIQR();
            MatchResult matchResult = new()
            {
                CenterX = dataHandled.Average(it => it.CenterX),
                CenterY = dataHandled.Average(it => it.CenterY),
                Angle = dataHandled.Average(it => it.Angle)
            };

            return matchResult;
        }

        /// <summary>
        /// 通过Bond相机定位结果转换出来的坐标(G0)
        /// </summary>
        /// <param name="result">定位结果</param>
        /// <param name="visionPos">位置</param>
        /// <returns>结果</returns>
        public AKRSPoint3D GetBondVisionResultPos(MatchResult result, AKRSPoint3D visionPos = null)
        {
            if (visionPos == null)
            {
                // 获取当前位置为定位位置
                visionPos = this.bondModuleController.GetG0RealPosition();
            }

            return visionPos + System2Module.GetInstance().BondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(
                new AKRSPoint3D(result.CenterX, result.CenterY, 0));
        }

        /// <summary>
        /// 通过Bond相机定位结果转换出来的坐标(轴坐标)
        /// 全局标定时使用，其他误用
        /// </summary>
        /// <param name="result">定位结果</param>
        /// <param name="visionPos">位置</param>
        /// <returns>结果</returns>
        public AKRSPoint3D GetBondVisionResultPosInMachine(MatchResult result, AKRSPoint3D visionPos = null)
        {
            if (visionPos == null)
            {
                // 获取当前位置为定位位置
                visionPos = this.bondModuleController.Get3DRealPosition();
            }

            return visionPos + System2Module.GetInstance().BondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(
                new AKRSPoint3D(result.CenterX, result.CenterY, 0));
        }

        /// <summary>
        /// 通过上视相机定位结果转换出来的坐标(G0)
        /// </summary>
        /// <param name="result">定位结果</param>
        /// <param name="visionPos">位置</param>
        /// <returns>结果</returns>
        public AKRSPoint3D GetUplookVisionResultPos(MatchResult result, AKRSPoint3D visionPos = null)
        {
            if (visionPos == null)
            {
                // 获取当前位置为定位位置
                visionPos = this.bondModuleController.GetG0RealPosition();
            }

            return visionPos + System2Module.GetInstance().UpLookModule.UpLookCameraCoordinateSystem
                .ForwardConvertCoordinate(
                    new AKRSPoint3D(result.CenterX, result.CenterY, 0));
        }

        /// <summary>
        /// 机械定位结果，不要使用，测试专用
        /// </summary>
        /// <param name="result">定位结果</param>
        /// <param name="visionPos">位置</param>
        /// <returns>结果</returns>
        public AKRSPoint3D GetVisionResultMachinePos(MatchResult result, AKRSPoint3D visionPos = null)
        {
            if (visionPos == null)
            {
                // 获取当前位置为定位位置
                visionPos = this.bondModuleController.GetG0RealPosition();
            }

            return visionPos - System2Module.GetInstance().BondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(
                new AKRSPoint3D(result.CenterX, result.CenterY, 0));
        }

        /// <summary>
        /// 获取上视相机中心在G0中的位置
        /// </summary>
        /// <returns>位置</returns>
        public AKRSPoint3D UpLookRealPosInG0()
        {
            AKRSPoint3D point3D = CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos;

            return System2Module.GetInstance().BondModule.BondCoordinateSystem.SelfPosToG0(point3D);
        }

        /// <summary>
        /// 背崩检测
        /// </summary>
        /// <param name="matchResult">定位结果</param>
        /// <param name="edgeTemplateName">背崩检测模板 </param>
        /// <returns>执行结果 true:背崩</returns>
        public bool BacksideCrackDetect(MatchResult matchResult,string edgeTemplateName)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork || MachineStateModel.GetInstance().IsDryCycle)
            {
                return new();
            }

            // 背崩PR
            PREntity edgePR = (PREntity)VisionEntityRepository.GetInstance().Find(edgeTemplateName);

            if (edgePR == null)
            {
                throw new Exception($"视觉模板-{edgePR} 不存在！设备将停止工作！");
            }

                // 设置位置修正系数
                edgePR.Alg.SetFix((float)matchResult.CenterX, (float)matchResult.CenterY, (float)matchResult.Angle);

                ExcuteResult prResult = edgePR.DoWork();
                if (prResult == ExcuteResult.Success)
                {
                    List<BaseAlgResult> matchResults = edgePR.AlgResults;
                    if (matchResults[0].IsSuccess)
                    {
                        // 有崩边
                        return true;
                    }
                    else
                    {

                        // 无崩边
                        return false;
                    }
                }
                else
                {
                    // 执行不成功当崩边处理
                    return  true;
                }
        }

        /// <summary>
        /// 获取相机硬件
        /// </summary>
        /// <param name="cameraType">相机类型</param>
        /// <returns>硬件集合</returns>
        public PRHardware GetHardware(CameraTypeEnum cameraType)
        {
            if (cameraType == CameraTypeEnum.BondCamera)
            {
                return this.bondModuleController.GetHardware();
            }
            else if (cameraType == CameraTypeEnum.ZWithXYCamera)
            {
                return this.bondModuleController.GetHardwareZWithXY();
            }
            else
            {
                return this.upLookController.GetHardware();
            }
        }

        /// <summary>
        /// 设置PR参数
        /// </summary>
        /// <param name="pREntity">模板</param>

        public void SetDefaultHardwareParameter(PREntity pREntity, CameraTypeEnum cameraType)
        {
            pREntity.SetHardware(this.GetHardware(cameraType));
            pREntity.Exposure = pREntity.Camera.GetExposureTime();
            pREntity.Gain = Math.Round(pREntity.Camera.GetGain());
            pREntity.Gamma = /*pREntity.Camera.GetGamma()*/40;
            foreach (var item in pREntity.PRLightList)
            {
                item.IsUse = true;
                item.LightIntensity = item.Light.GetIntensity();
            }
        }

        /// <summary>
        /// 相机设置硬件
        /// </summary>
        /// <param name="pRName">模版名称</param>
        /// <param name="cameraType">相机类型</param>
        public void SetHardware(string pRName, CameraTypeEnum cameraType)
        {
            if (cameraType == CameraTypeEnum.BondCamera)
            {
                this.bondModuleController.SetHardware(pRName);
            }
            else
            {
                this.upLookController.SetHardware(pRName);
            }
        }


        /// <summary>
        /// 通过图片获取定位结果
        /// </summary>
        /// <param name="pRName">PR名称</param>
        /// <param name="bmp">图片</param>
        /// <returns>结果</returns>
        public BaseAlgResult GetVisionResult(string pRName, Bitmap bmp)
        {
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(pRName);
            if (pREntity == null)
            {
                AKRSMessageBoxExt.Show(
                    $"视觉定位流程未找到PR： {pRName} ，将停止自动工作！\r\n",
                    "异常",
                    new string[] { "终止" },
                    new DialogResult[] { DialogResult.Abort },
                    AlarmLevel.SecondLevel);

                Machine.GetInstance().Stop();
                return null;
            }

        Retry:
            Bitmap bitmap = new(2448, 2048);
            bitmap = (Bitmap)bmp.Clone();

            ExcuteResult ret = pREntity.DoWork(bitmap);
            if (ret != ExcuteResult.Success)
            {
                DialogResult res = AKRSMessageBoxExt.Show(
                    $"PR ：{pRName} 定位失败! 请选择如何处理!\r\n",
                    "报警",
                    new string[] { "重试", "忽略", "终止" },
                    new DialogResult[] { DialogResult.Retry, DialogResult.Ignore, DialogResult.Abort },
                    AlarmLevel.SecondLevel);

                switch (res)
                {
                    case DialogResult.Retry:
                        goto Retry;

                    case DialogResult.Ignore:

                        break;

                    case DialogResult.Abort:
                        Machine.GetInstance().Stop();
                        break;

                    default:
                        return null;
                }
            }

            return pREntity.AlgResult;
        }

        /// <summary>
        /// 系统2温漂补偿
        /// </summary>
        /// <param name="firstTu">是否为第一次</param>
        /// <returns>结果</returns>
        public  bool System2TemperatureCompensation(bool firstTu)
        {
            // 如果不开启温漂补偿，直接返回
            if (!System2Configuration.GetInstance().IsActiveDriftCompensate)
            {
                return true;
            }

            if ((DateTime.Now - TpMarkCompensate.LastUpLookVisionTime).Minutes >= System2Configuration.GetInstance().DriftCompensateIntervalTime
                || firstTu)
            {
                if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured)
                {
                    // 刮胶盘缩回防止干涉
                    if (this.slideFluxerController.IsSlideFluxerAtPLimit())
                    {
                        this.slideFluxerController.SlideFluxerHomeWaitArrive();
                    }
                }

                // 温漂补偿
                TpMarkCompensate.TemperatureCompensation(firstTu);
            }

            return true;
        }
    }
}
