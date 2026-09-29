using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.DispenseSystem.Models.DispenseAction;
using AKRS.ZX2200.DispenseSystem.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AKRS.ZX2200.DispenseSystem.Models
{
    using AKRS.Galaxy2.Component.Simple.MessageBox;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.Galaxy2.UserManager.Models;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Process;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Information;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using AKRS.ZX2200.TransportUnitSystem.Service;
    using DevExpress.DataProcessing.InMemoryDataProcessor;
    using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
    using DevExpress.XtraCharts.Native;
    using DevExpress.XtraEditors;
    using log4net.Core;
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using UcMainSystem = AKRS.ZX2200.Main.Controls.Ucmain.MainControls.UcMainSystem;

    /// <summary>
    /// 点胶域
    /// </summary>
    public class System1Domain : SingletonNoSave<System1Domain>
    {
        /// <summary>
        /// 点胶程式
        /// </summary>
        public System1Program System1Program => System1Program.GetInstance();

        /// <summary>
        /// 点胶工作线程
        /// </summary>
        [JsonIgnore]
        public DispenseWorkTask DispenseWorkTask { get; set; } = new DispenseWorkTask();

        /// <summary>
        /// 点胶动作集合
        /// </summary>
        [JsonIgnore]
        public DispenseActionNodes DispenseActionNodes { get; set; } = new DispenseActionNodes(new List<string>());

        /// <summary>
        /// 点胶控制器
        /// </summary>
        public DispenseController DispenseController { get; set; } = new DispenseController();

        /// <summary>
        /// 点胶视觉模块控制器
        /// </summary>
        public DispenseVisionController DispenseVisionController { get; set; } = new DispenseVisionController();

        /// <summary>
        /// 点胶测高控制器
        /// </summary>
        public DispenseMeasureHeightController DispenseMeasureHeightController { get; set; } =
            new DispenseMeasureHeightController();

        /// <summary>
        /// 系统1动作控制器
        /// </summary>
        public S1ActionNodeController ActionNodeController { get; set; } = new S1ActionNodeController();

        /// <summary>
        /// 系统实体1定位
        /// 针对于TU，Sub，Module，BP
        /// </summary>
        /// <param name="baseEntity">实体</param>
        /// <param name="byMapping">是否根据Mapping走</param>
        /// <returns>结果</returns>
        public bool System1MatterVision(BaseMatter baseEntity, bool byMapping = true)
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

            bool visionAlarm = false;

            RetryCommand:

            // P1定位
            if (!this.System1VisionByPos(baseEntity, TuService.NormalVisionType.P1Vision, visionAlarm))
            {
                return false;
            }

            if (baseEntity.Config.LocateConfig.AdjustType == AdjustTypeEnum.OnePoint && baseEntity.EntityState != EntityState.Fail)
            {
                // 纠偏计算
                TuService.UpdateEntity(baseEntity, TuService.NormalVisionType.P1Vision);
                return true;
            }

            // P2定位
            if (!this.System1VisionByPos(baseEntity, TuService.NormalVisionType.P2Vision, visionAlarm))
            {
                return false;
            }

            if (baseEntity.Config.LocateConfig.AdjustType == AdjustTypeEnum.TwoPoints && baseEntity.EntityState != EntityState.Fail)
            {
                if (!visionAlarm)
                {
                    // 距离检查
                    DialogResult dialogResult = VisionService.DistanceCheck(baseEntity);

                    if (dialogResult == DialogResult.Abort)
                    {
                        Machine.GetInstance().Stop();
                        return false;
                    }
                    else if (dialogResult == DialogResult.Retry)
                    {
                        visionAlarm = true;
                        goto RetryCommand;
                    }
                    else if (dialogResult == DialogResult.None)
                    {
                        baseEntity.MatterProductState = MatterProductState.Disable;
                    }
                }
                
                // 纠偏计算
                TuService.UpdateEntity(baseEntity, TuService.NormalVisionType.P2Vision);

                return true;
            }

            // P3定位
            if (!this.System1VisionByPos(baseEntity, TuService.NormalVisionType.P3Vision))
            {
                return false;
            }

            if (baseEntity.Config.LocateConfig.AdjustType == AdjustTypeEnum.ThreePoints && baseEntity.EntityState != EntityState.Fail)
            {
                // 距离检查

                // 纠偏计算
                TuService.UpdateEntity(baseEntity, TuService.NormalVisionType.P3Vision);

                return true;
            }

            // P3定位
            if (!this.System1VisionByPos(baseEntity, TuService.NormalVisionType.P4Vision))
            {
                return false;
            }

            if (baseEntity.Config.LocateConfig.AdjustType == AdjustTypeEnum.FourPoints && baseEntity.EntityState != EntityState.Fail)
            {
                // 距离检查

                // 纠偏计算
                TuService.UpdateEntity(baseEntity, TuService.NormalVisionType.P4Vision);

                return true;
            }

            return false;
        }
        
        /// <summary>
        /// 系统1定位
        /// </summary>
        /// <param name="baseEntity">实体对象</param>
        /// <param name="type">定位类型</param>
        /// <returns>结果</returns>
        private bool System1VisionByPos(BaseMatter baseEntity, TuService.NormalVisionType type,bool alarm = false)
        {
            // 单步工作
            if (!System1Domain.GetInstance().WaitSingleStep())
            {
                return false;
            }

            LogHelper.Post(Level.Info, $"点胶{type}定位开始", LogCategory.Dispense);

            #region 先决条件判断

            //if (MachineStateModel.GetInstance().MachineState == MachineStateEnum.Stop)
            //{
            //    return false;
            //}

            if (baseEntity.EntityState == EntityState.Fail)
            {
                return false;
            }

            #endregion

            // 根据信息获取坐标
            (AKRSPoint3D point, string pRName, bool autofocus) = TuService.GetVisionInfo(baseEntity, type);

            // 转换到G0坐标上面去
            AKRSPoint3D p1VisionPosInG01 =
                baseEntity.CoordinateSystem.SelfPosToG0(point);

            // 添加差值
            p1VisionPosInG01 = this.DispenseController.GetG0VisionPos(p1VisionPosInG01);

            MatchResult matchResult1 = null;

            if (!alarm)
            {
                matchResult1 = (MatchResult)this.DispenseVisionController.DispenseVision(p1VisionPosInG01, pRName,autofocus);

                #region 四周定位

                if (baseEntity.Config.LocateConfig.IsAroundLocate && matchResult1 == null)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (i == 0)
                        {
                            // 执行定位
                            matchResult1 = (MatchResult)this.DispenseVisionController.DispenseVision(
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
                            matchResult1 = (MatchResult)this.DispenseVisionController.DispenseVision(
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
                            matchResult1 = (MatchResult)this.DispenseVisionController.DispenseVision(
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
                            matchResult1 = (MatchResult)this.DispenseVisionController.DispenseVision(
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
            
            // 结果判断
            if (matchResult1 == null)
            {
                this.DispenseController.MoveToG0Pos3D(p1VisionPosInG01);

                PREntity pREntityTemp = (PREntity)VisionEntityRepository.GetInstance().Find(pRName);
                // PREntity pREntity = DispenseRunTimeProvider.GetDispensePREntity(pREntityTemp);

                (DialogResult dialogResult, BaseAlgResult match) result = UcMainSystem.VisionAlarmLockFunc(pREntityTemp, baseEntity.Name, $"{baseEntity.MatterTypeEnum}点胶定位失败", this.DispenseVisionController.GetHardware());
                if (result.dialogResult == DialogResult.OK)
                {
                    matchResult1 = (MatchResult)result.match;
                }
                else if (result.dialogResult == DialogResult.Ignore)
                {
                    baseEntity.SetMatterDisable();
                    return false;
                }
                else if (result.dialogResult == DialogResult.Abort)
                {
                    Machine.GetInstance().Stop();
                    return false;
                }
                else
                {
                    return false;
                }
            }

            #region 信息保存

            // 定位结果转换
            AKRSPoint3D realPoint3DInG0 = this.DispenseController.ConvertVisionResultInG0(matchResult1);

            // 转到G0
            realPoint3DInG0 = this.DispenseController.GetG0PosFromVision(realPoint3DInG0);

            // 转到自己的坐标系
            AKRSPoint3D realPoint3D = baseEntity.CoordinateSystem.G0PosToSelf(realPoint3DInG0);

            VisionResult visionResult = TuService.ChooseInfoByType(type, baseEntity);

            visionResult.SaveInfo(matchResult1, realPoint3D, realPoint3DInG0, this.DispenseController.GetG0Pos());

            LogHelper.Post(Level.Info, $"点胶{type}定位结束", LogCategory.Dispense);

            return true;
            #endregion
        }

        /// <summary>
        /// 系统1测高
        /// 针对于TU，Sub，Module，BP
        /// </summary>
        /// <param name="baseEntity">对象</param>
        /// <param name="closeCy">是否收回气缸</param>
        /// <returns>结果</returns>
        public ExcuteResult MatterHeightMeasurePoints(BaseMatter baseEntity, bool closeCy = false)
        {
            // 没有定位点直接返回
            if (baseEntity.Config.HeightMeasurementPoints.Count == 0)
            {
                return ExcuteResult.Success;
            }

            // 没有测高功能没有开启，直接返回
            if (!baseEntity.Config.MeasureHeightInSystem1)
            {
                return ExcuteResult.Success;
            }

            LogHelper.Post(Level.Info, "点胶测高开始", LogCategory.Dispense);

            // 结果的集合
            List<double> resultList = new List<double>();

            foreach (AKRSPoint3D pos in baseEntity.Config.HeightMeasurementPoints)
            {
                // 单步工作
                if (!System1Domain.GetInstance().WaitSingleStep())
                {
                    return ExcuteResult.Abort;
                }

                // 计算G0坐标
                AKRSPoint3D g0Pos = baseEntity.CoordinateSystem.SelfPosToG0(pos);

                double addDistance = MachineHardwareConfiguration.GetInstance().IsSystem1ConfigLaserMh
                               ? 0
                               : DispenseDevicePara.GetInstance().DispenseModulePara.MeasureHeightDistance;

                // 实际测高位置计算
                AKRSPoint3D measHeightPos = new AKRSPoint3D(g0Pos.X, g0Pos.Y, g0Pos.Z + addDistance);

            RetryCommand:

                // 执行测高
                (ExcuteResult excuteResult, double height) result =
                    this.DispenseMeasureHeightController.DispenserHeightMeasurementG0(
                        System1MeasHeightToolEnum.HeightSensor,
                        measHeightPos,
                        double.NaN,
                        double.NaN,
                        closeCy);

                double height = baseEntity.CoordinateSystem.G0PosToSelf(new AKRSPoint3D(0, 0, result.height)).Z;

                resultList.Add(height);

                // 高度预警
                if (result.excuteResult != ExcuteResult.Success 
                    || Math.Abs(result.height - g0Pos.Z) > DispenseDevicePara.GetInstance().DispenseModulePara.MeasureHeightAlarmDistance
                    || double.IsNaN(result.height))
                {
                    DialogResult dialogResult = AKRSMessageBoxExt.Show(
                        $"测高偏差数值为{height - g0Pos.Z}mm，超出设置阈值{DispenseDevicePara.GetInstance().DispenseModulePara.MeasureHeightAlarmDistance}\r\n"
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

            }

            // 计算测高点的
            double averageHeight = resultList.Average();

            // 计算之前的高度
            double averageHeightOld = baseEntity.Config.HeightMeasurementPoints.Select(it => it.Z).Average();

            // 现在的高度减去之前的高度
            double addHieght = averageHeight - averageHeightOld;

            // 保存结果
            baseEntity.BaseInfo.MeasureHeightResult = addHieght;

            if (double.IsNaN(addHieght))
            {
                throw new Exception("测高结果为NaN，请检查激光传感器是否存在问题");
            }

            // 更新差值
            baseEntity.CoordinateSystem.Distance.Z += addHieght;

            LogHelper.Post(Level.Info, $"点胶测高结束,高度为{averageHeight}", LogCategory.Dispense);

            baseEntity.BaseInfo.IsMeasureHeight = true;

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 系统1是否准备好
        /// </summary>
        /// <returns>是否准备好</returns>
        public bool CheckIsReady()
        {
            if (!MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                return true;
            }

            // 点胶头是否准备好
            if (!this.System1Program.DispenserProgram.IsReady())
            {
                return false;
            }

            // 预点胶在开启的情况下，判断预点胶板是否示教完成
            if (this.System1Program.DispenserProgram.Dispenser.PreDispense.PreDispensingMode != PreDispensingModeEnum.Off)
            {
                // 预点胶板是否准备好
                if (!this.System1Program.PreDispensePlateProgram.IsReady())
                {
                    AKRSXtraMessageBox.Show("预点胶板未示教完成");

                    return false;
                }

                // 判断有没有预画胶板的名称
                if (this.System1Program.DispenserProgram.Dispenser.PreDispense.EpoxyApplicationName == null)
                {
                    AKRSXtraMessageBox.Show("预点胶开启，但未配置预点胶所需的胶型");
                    return false;
                }

                if (!this.System1Program.EpoxyApplicationProgram.EpoxyApplications.Exists(it => it.Name 
                        == this.System1Program.DispenserProgram.Dispenser.PreDispense.EpoxyApplicationName))
                {
                    AKRSXtraMessageBox.Show(
                        $"预点胶开启，但预点胶胶型未找到：{this.System1Program.DispenserProgram.Dispenser.PreDispense.EpoxyApplicationName}");
                    return false;
                }
            }

            // 点胶材料是否准备好
            if (!this.System1Program.EpoxyMaterialProgram.IsReady())
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
                if (!SignalPool.GetInstance().System1SingleStepSignal.WaitSingleStep())
                {
                    return false;
                }
            }

            return true;
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
            p1VisionPosInG01 = this.DispenseController.GetG0VisionPos(p1VisionPosInG01);

            #endregion

            #region 定位

            System2RunTimeProvider.RecordTime("系统1ID识别", $"准备移动到点位：{p1VisionPosInG01} 执行定位");

            CodeResult codeResult = (CodeResult)this.DispenseVisionController.DispenseVision(p1VisionPosInG01, pRName);

            System2RunTimeProvider.RecordTime("系统1ID识别", $"{p1VisionPosInG01} 完成定位");

            #endregion

            #region 定位失败结果处理

            // 结果判断
            if (codeResult == null)
            {
                System2RunTimeProvider.RecordTime("系统1ID识别", $"{p1VisionPosInG01}身份识别失败，报警手动处理");

                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(pRName);

                (DialogResult dialog, BaseAlgResult matchResult) result =
                    UcMainSystem.VisionAlarmLockFunc(pREntity, $"{baseEntity.MatterTypeEnum}ID识别失败。" + baseEntity.Name, baseEntity.Name, this.DispenseVisionController.GetHardware());

                if (result.dialog == DialogResult.OK)
                {
                    System2RunTimeProvider.RecordTime("系统1ID识别", $"{p1VisionPosInG01}身份识别失败，手动识别成功");
                    codeResult = (CodeResult)result.matchResult;
                }
                else if (result.dialog == DialogResult.Ignore)
                {
                    System2RunTimeProvider.RecordTime("系统1ID识别", $"{p1VisionPosInG01}身份识别失败，手动识别失败");
                    baseEntity.SetMatterDisable();
                    return ExcuteResult.Success;
                }
                else if (result.dialog == DialogResult.Abort)
                {
                    System2RunTimeProvider.RecordTime("系统1ID识别", $"{p1VisionPosInG01}身份识别失败，停止");
                    Machine.GetInstance().Stop();
                    return ExcuteResult.Abort;
                }
                else
                {
                    return ExcuteResult.Abort;
                }
            }

            #endregion

            // 保存结果
            baseEntity.BaseInfo.IsIdentity = true;
            baseEntity.BaseInfo.IdentityCode = codeResult.CodeValue;
            return ExcuteResult.Success;
        }
    }
}
