using System;
using System.Threading;
using System.Windows.Forms;
using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;
using log4net.Core;
using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Controllers
{
    using System.Diagnostics;

    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
    using AKRS.ZX2200.BondSystem.Models;

    using OfficeOpenXml;
    using System.IO;
    using System.Threading.Tasks;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;

    /// <summary>
    /// EjectController
    /// </summary>
    public class EjectController
    {
        /// <summary>
        /// 允许误差
        /// </summary>
        private double range = 0.1;

        /// <summary>
        /// 顶针模组
        /// </summary>
        [JsonIgnore]
        private EjectModule EjectModule => WaferSubModule.GetInstance().Eject;

        /// <summary>
        /// 顶针设备参数
        /// </summary>
        [JsonIgnore]
        private EjectDevicePara EjectDevicePara => WaferSubDevicePara.GetInstance().EjectDevicePara;

        /// <summary>
        /// 顶针架当前位置
        /// </summary>
        [JsonIgnore]
        public AKRSPoint3D EjectionBankAxisTG0Pos
        {
            get
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return new AKRSPoint3D(0, 0, 0);
                }

                return MachineCoordinateSystem.GetInstance().NullCoordinateSystem.SelfPosToG0(new AKRSPoint3D(this.EjectModule.EjectionBankAxisT.GetRealPosition(), 0, 0));
            }
        }

        /// <summary>
        /// 顶针升降电机当前位置
        /// </summary>
        [JsonIgnore]
        public AKRSPoint3D EjectionTableAxisZG0Pos
        {
            get
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return new AKRSPoint3D(0, 0, 0);
                }

                return MachineCoordinateSystem.GetInstance().NullCoordinateSystem.SelfPosToG0(new AKRSPoint3D(0, 0, this.EjectModule.EjectionTableAxisZ.GetRealPosition()));
            }
        }

        /// <summary>
        /// 顶针电机当前位置
        /// </summary>
        [JsonIgnore]
        public AKRSPoint3D EjectionAxisZG0Pos
        {
            get
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return new AKRSPoint3D(0, 0, 0);
                }

                return MachineCoordinateSystem.GetInstance().NullCoordinateSystem.SelfPosToG0(new AKRSPoint3D(0, 0, this.EjectModule.EjectionAxisZ.GetRealPosition()));
            }
        }

        /// <summary>
        /// 初始化当前槽配置
        /// </summary>
        public void InitCurrentSlotConfig()
        {
            WaferSubDevicePara.GetInstance().EjectDevicePara.CurrentSlotConfig = null;
            WaferSubDevicePara.GetInstance().Save();
        }

        /// <summary>
        /// 移动顶针到安全位置
        /// </summary>
        public void MoveEjectToSafePosition()
        {
            if (Math.Abs(this.EjectionAxisZG0Pos.Z - EjectDevicePara.EjectSafePosition.Z) > this.range)
            {
                this.MoveEjectionAxisZToG0Pos(EjectDevicePara.EjectSafePosition, false);
                if (EjectDevicePara.IsNeedEjectZero)
                {
                    EjectModule.EjectionAxisZ.GoHome();
                }
            }
        }

        /// <summary>
        /// 移动顶针台到安全位置
        /// </summary>
        public void MoveEjectionTableToSafePosition()
        {
            if (Math.Abs(this.EjectionTableAxisZG0Pos.Z - EjectDevicePara.UpDownSafePosition.Z) > this.range)
            {
                this.MoveEjectionTableAxisZToG0Pos(EjectDevicePara.UpDownSafePosition, false);
                if (EjectDevicePara.IsNeedEjectTableZero)
                {
                    EjectModule.EjectionTableAxisZ.GoHome();
                }
            }
        }

        /// <summary>
        /// 移动顶针台到mark位置
        /// </summary>
        public void MoveEjectionTableToMarkPosition()
        {
            if (Math.Abs(this.EjectionTableAxisZG0Pos.Z - EjectDevicePara.UpDownMarkPosition.Z) > this.range)
            {
                this.MoveEjectionTableAxisZToG0Pos(EjectDevicePara.UpDownMarkPosition);
            }
        }

        /// <summary>
        /// 移动顶针台到气缸动作位置
        /// </summary>
        /// <param name="ejectionBankSlotConfig">顶针槽</param>
        /// <param name="isTest">是否是测试</param>
        public void MoveEjectionTableToFixedCylinderActionPosition(bool isTest)
        {
            if (EjectDevicePara.UpDownMarkPosition.Z < EjectDevicePara.CycActionPosition.Z)
            {
                //throw new Exception("EjectionTableMarkPosition is low to CycActionPosition");
                throw new Exception("顶针台的Mark位置比气缸动作位置低");
            }

            if (Block.GetInstance().GetCurrentCarrier() is CarrierWithWaferConfig carrierWithWaferConfig)
            {
                if (carrierWithWaferConfig.EjectionTableWorkPosition.Z < EjectDevicePara.CycActionPosition.Z && !isTest)
                {
                    //throw new Exception("EjectionTableWorkPosition is low to CycActionPosition");
                    throw new Exception("顶针台的工作位置比气缸动作位置低");
                }
            }

            if (Math.Abs(this.EjectionTableAxisZG0Pos.Z - EjectDevicePara.CycActionPosition.Z) > this.range)
            {
                this.MoveEjectionTableAxisZToG0Pos(EjectDevicePara.CycActionPosition);
            }
        }

        /// <summary>
        /// 移动顶针到预顶起位置
        /// </summary>
        /// <param name="isChangeEject">是否是更换顶针</param>
        public void MoveEjectToReadyLiftPosition(bool isChangeEject = false)
        {
            if (EjectDevicePara.CurrentSlotConfig == null || EjectDevicePara.CurrentSlotConfig.Name == string.Empty)
            {
                //throw new Exception("Current eject is null!");
                throw new Exception("当前顶针为空！");
            }

            if (isChangeEject)
            {
                if (Math.Abs(this.EjectionAxisZG0Pos.Z - EjectDevicePara.CurrentSlotConfig.EjectionConfig.ReadyLiftPosition.Z) > this.range)
                {
                    this.MoveEjectionAxisZToG0Pos(EjectDevicePara.CurrentSlotConfig.EjectionConfig.ReadyLiftPosition);
                }
            }
            else
            {
                if (Block.GetInstance().GetCurrentCarrier() is CarrierWithWaferConfig carrierWithWaferConfig)
                {
                    if (Math.Abs(this.EjectionAxisZG0Pos.Z - EjectDevicePara.CurrentSlotConfig.EjectionConfig.ReadyLiftPosition.Z) > this.range)
                    {
                        this.MoveEjectionAxisZToG0Pos(EjectDevicePara.CurrentSlotConfig.EjectionConfig.ReadyLiftPosition, carrierWithWaferConfig.EjectLiftSpeed);
                    }
                }
                else
                {
                    //throw new Exception($"Current carrier is not wafer carrier - {Block.GetInstance().GetCurrentCarrier().Name}!");
                    throw new Exception($"当前的载具不是晶圆载具 - {Block.GetInstance().GetCurrentCarrier().Name}!");
                }
            }
        }

        /// <summary>
        /// 移动顶针到顶起位置
        /// </summary>
        public void MoveEjectToLiftPosition()
        {
            LogHelper.Post(Level.Info, $"顶针准备顶起，顶针G0坐标{WaferSubController.GetInstance().EjectController.EjectionAxisZG0Pos.Z}，机械坐标{this.EjectModule.EjectionAxisZ.GetRealPosition()}", LogCategory.Global);

            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            if (Block.GetInstance().GetCurrentCarrier() is CarrierWithWaferConfig carrierWithWaferConfig)
            {
                if (Math.Abs(this.EjectionAxisZG0Pos.Z - EjectDevicePara.CurrentSlotConfig.EjectionConfig.ReadyLiftPosition.Offset(0, 0, carrierWithWaferConfig.RelativeHeightWithEjectionReadyLiftPosition).Z) > this.range)
                {
                    this.MoveEjectionAxisZToG0Pos(EjectDevicePara.CurrentSlotConfig.EjectionConfig.ReadyLiftPosition.Offset(0, 0, carrierWithWaferConfig.RelativeHeightWithEjectionReadyLiftPosition), carrierWithWaferConfig.EjectLiftSpeed);
                }
            }
            else
            {
                //throw new Exception($"Current carrier is not wafer carrier - {Block.GetInstance().GetCurrentCarrier().Name}!");
                throw new Exception($"当前的载具不是晶圆载具 - {Block.GetInstance().GetCurrentCarrier().Name}!");
            }

            LogHelper.Post(Level.Info, $"顶针顶起结束，顶针G0坐标{WaferSubController.GetInstance().EjectController.EjectionAxisZG0Pos.Z}，机械坐标{this.EjectModule.EjectionAxisZ.GetRealPosition()}", LogCategory.Global);
        }

        /// <summary>
        /// 获取顶针顶起位置
        /// </summary>
        /// <returns>顶针顶起位置</returns>
        public AKRSPoint3D GetEjectLiftMachinePosition()
        {
            AKRSPoint3D result = new AKRSPoint3D();

            if (Block.GetInstance().GetCurrentCarrier() is CarrierWithWaferConfig carrierWithWaferConfig)
            {
                result = MachineCoordinateSystem.GetInstance().NullCoordinateSystem.G0PosToSelf(EjectDevicePara.CurrentSlotConfig.EjectionConfig.ReadyLiftPosition.Offset(0, 0, carrierWithWaferConfig.RelativeHeightWithEjectionReadyLiftPosition));
            }

            return result;
        }

        /// <summary>
        /// 移动顶针台到工作位置
        /// </summary>
        /// <param name="ejectionBankSlotConfig">顶针槽</param>
        public void MoveEjectionTableToWorkPosition()
        {
            if (Block.GetInstance().GetCurrentCarrier() is CarrierWithWaferConfig carrierWithWaferConfig)
            {
                if (Math.Abs(this.EjectionTableAxisZG0Pos.Z - carrierWithWaferConfig.EjectionTableWorkPosition.Z) > this.range)
                {
                    this.MoveEjectionTableAxisZToG0Pos(carrierWithWaferConfig.EjectionTableWorkPosition);
                }
            }
            else
            {
                //throw new Exception("Current carrier is not wafer carrier!");
                throw new Exception("当前的载具不是晶圆载具！");
            }
        }

        /// <summary>
        /// 移动顶针架到某个槽位
        /// </summary>
        /// <param name="i">i</param>
        public void MoveEjectionBankToSlotPosition(int i)
        {
            this.MoveEjectionBankAxisTToG0Pos(WaferSubDevicePara.GetInstance().EjectDevicePara.SlotPosition[i]);
        }

        /// <summary>
        /// 打开顶针固定气缸
        /// </summary>
        public void SetFixedCylinder()
        {
            if (!WaferSubDevicePara.GetInstance().EjectDevicePara.IsUseEjectFixedCylinder)
            {
                return;
            }

            //WaferSubController.GetInstance().SetCyc(EjectModule.FixedCylinder, EjectModule.FixedCylinderPLimitSensor, EjectDevicePara.DelayFixedCycAction);
            WaferSubController.GetInstance().SetCyc(EjectModule.FixedCylinder, 0, EjectDevicePara.DelayFixedCycActionAfter);
        }

        /// <summary>
        /// 关闭顶针固定气缸
        /// </summary>
        public void ResetFixedCylinder()
        {
            if (!WaferSubDevicePara.GetInstance().EjectDevicePara.IsUseEjectFixedCylinder)
            {
                return;
            }

            //WaferSubController.GetInstance().ResetCyc(EjectModule.FixedCylinder, EjectModule.FixedCylinderNLimitSensor, EjectDevicePara.DelayFixedCycAction);
            WaferSubController.GetInstance().ResetCyc(EjectModule.FixedCylinder, 0, EjectDevicePara.DelayFixedCycActionAfter);
        }

        /// <summary>
        /// 打开顶针台真空
        /// </summary>
        public void OpenEjectionTableVacuum()
        {
            WaferSubController.GetInstance().ResetCyc(EjectModule.EjectionTableBlow, 0, EjectDevicePara.DelayCloseEjectBlowAfter);
            WaferSubController.GetInstance().SetCyc(EjectModule.EjectionTableVacuum,0, EjectDevicePara.DelayOpenEjectInhaleAfter);

            LogHelper.Post(Level.Info, $" 取片动作--顶针真空打开", LogCategory.Component);
        }

        /// <summary>
        /// 关闭顶针台真空
        /// </summary>
        public void CloseEjectionTableVacuum()
        {
            WaferSubController.GetInstance().ResetCyc(EjectModule.EjectionTableBlow, 0, EjectDevicePara.DelayCloseEjectBlowAfter);
            WaferSubController.GetInstance().ResetCyc(EjectModule.EjectionTableVacuum, 0, EjectDevicePara.DelayCloseEjectInhaleAfter);
        }

        /// <summary>
        /// 打开顶针台吹气
        /// </summary>
        public void OpenEjectionTableBlow()
        {
            WaferSubController.GetInstance().ResetCyc(EjectModule.EjectionTableVacuum, 0, EjectDevicePara.DelayCloseEjectInhaleAfter);
            WaferSubController.GetInstance().SetCyc(EjectModule.EjectionTableBlow, 0);
        }

        /// <summary>
        /// 关闭顶针台吹气
        /// </summary>
        public void CloseEjectionTableBlow()
        {
            WaferSubController.GetInstance().ResetCyc(EjectModule.EjectionTableVacuum, 0, EjectDevicePara.DelayCloseEjectInhaleAfter);
            WaferSubController.GetInstance().ResetCyc(EjectModule.EjectionTableBlow, 0, EjectDevicePara.DelayCloseEjectBlowAfter);
        }

        ///// <summary>
        ///// 移动顶针到预顶位并吹气
        ///// </summary>
        //public void MoveEjectToReadyLiftPositionAndBlow()
        //{
        //    if (MachineStateModel.GetInstance().IsOffLineWork)
        //    {
        //        return;
        //    }

        //    LogHelper.Post(Level.Info, $"顶针准备回预顶位并吹气，顶针G0坐标{WaferSubController.GetInstance().EjectController.EjectionAxisZG0Pos.Z}，机械坐标{this.EjectModule.EjectionAxisZ.GetRealPosition()}", LogCategory.Component);

        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();
        //    bool isMoveEjectToReadyLiftPosition = false, isEjectionTableBlow = false;
        //    Task.Run(
        //        () =>
        //            {
        //                this.MoveEjectToReadyLiftPosition();
        //                isMoveEjectToReadyLiftPosition = true;
        //            });

        //    Task.Run(
        //        () =>
        //            {
        //                this.OpenEjectionTableBlow();
        //                Thread.Sleep(EjectDevicePara.DelayEjectBlowHold);
        //                this.CloseEjectionTableBlow();
        //                isEjectionTableBlow = true;
        //            });

        //    while (!isMoveEjectToReadyLiftPosition || !isEjectionTableBlow)
        //    {
        //        if (sw.ElapsedMilliseconds > 5000)
        //        {
        //            sw.Stop();
        //            sw.Reset();
        //            throw new Exception($"移动顶针到预顶位并吹气操作超时!");
        //        }

        //        continue;
        //    }

        //    LogHelper.Post(Level.Info, $"顶针准备回预顶位并吹气，顶针G0坐标{WaferSubController.GetInstance().EjectController.EjectionAxisZG0Pos.Z}，机械坐标{this.EjectModule.EjectionAxisZ.GetRealPosition()}", LogCategory.Component);

        //    sw.Stop();
        //    sw.Reset();
        //}

        /// <summary>
        /// 移动顶针到预顶位并吹气
        /// </summary>
        public void MoveEjectToReadyLiftPositionAndBlow()
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            LogHelper.Post(Level.Info, $"顶针准备回预顶位，顶针G0坐标{this.EjectionAxisZG0Pos.Z}，机械坐标{this.EjectModule.EjectionAxisZ.GetRealPosition()}", LogCategory.Global);

            try
            {
                var moveTask = Task.Run(() => this.MoveEjectToReadyLiftPosition());
                var blowTask = Task.Run(() =>
                    {
                        this.OpenEjectionTableBlow();
                        Thread.Sleep(EjectDevicePara.DelayEjectBlowHold);
                        this.CloseEjectionTableBlow();
                    });

                // 等待所有任务完成
                moveTask.Wait();
                blowTask.Wait();

                LogHelper.Post(Level.Info, $"顶针回预顶位并吹气完成，顶针G0坐标{this.EjectionAxisZG0Pos.Z}，机械坐标{this.EjectModule.EjectionAxisZ.GetRealPosition()}", LogCategory.Component);
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"顶针去预顶位失败!" + ex.ToString(), ex, LogCategory.Bond);

                throw;
            }
        }

        /// <summary>
        /// 更换顶针
        /// </summary>
        public void ChangeEjection()
        {
            WaferTablet wt = (WaferTablet)WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet;
            string name = wt.CarrierConfigWithWafer.EjectionName;

            EjectionBankSlotConfig preSlotConfig = WaferSubDevicePara.GetInstance().EjectDevicePara.CurrentSlotConfig;

            // 记忆
            if (preSlotConfig != null && preSlotConfig?.Name != string.Empty && preSlotConfig?.Name == name && (Math.Abs(this.EjectionTableAxisZG0Pos.Z - ((CarrierWithWaferConfig)Block.GetInstance().GetCurrentCarrier()).EjectionTableWorkPosition.Z) < this.range))
            {
                return;
            }
            else
            {
                name = WaferSystemDomain.GetInstance().WaferSystemProgram.EjectionBankProgram.Name;
                if (!EjectionBankConfigRepository.GetInstance().BaseDsSettingList.Exists(item => item.Name == name))
                {
                    //throw new Exception($"This EjectionBank does not exist in the library!");
                    throw new Exception($"仓库中不存在当前使用的顶针架！");
                }

                name = wt.CarrierConfigWithWafer.EjectionName;
                foreach (var slot in WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig.EjectionBankSlots)
                {
                    if (slot.Name == name)
                    {
                        this.ChangeEjection(slot.Index);
                        return;
                    }
                }

                //throw new Exception($"This Ejection does not exist in the current EjectionBank!");
                throw new Exception($"当前顶针架中不存在目标顶针！");
            }
        }

        /// <summary>
        /// 更换顶针（即更换槽位）
        /// </summary>
        /// <param name="i">i</param>
        /// <param name="isTest">IsTest</param>
        public void ChangeEjection(int i, bool isTest = false)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                this.ReturnEjection();
                EjectDevicePara.CurrentSlotConfig = WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig.EjectionBankSlots[i];
                WaferSystemProgram.GetInstance().EjectionBankProgram.EjectionBankEntity.EjectionBankSlotEntities[EjectDevicePara.CurrentSlotConfig.Index].SlotState = EjectSlotStatuEnum.Using;
                WaferSystemProgram.GetInstance().Save();
                EjectionBankConfigRepository.GetInstance().Save();
                WaferSubDevicePara.GetInstance().Save();
                return;
            }

            this.ReturnEjection();

            // 晶圆台去准备位
            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToReadyPosition();

            // 旋转电机去目标顶针位
            WaferSubController.GetInstance().EjectController.MoveEjectionBankToSlotPosition(i);

            // 升降电机去气缸动作位
            this.MoveEjectionTableToFixedCylinderActionPosition(isTest);

            //// 磁吸感应器感应顶针
            //if (!EjectModule.ContactSensor1.CheckStateForNums(true, 3, 200) || !EjectModule.ContactSensor2.CheckStateForNums(true, 3, 200))
            //{
            //    throw new Exception("磁吸感应器未感应到顶针！");
            //}

            if (EjectDevicePara.IsUseEjectFixedCylinder)
            {
                // 气缸去正限位
                this.SetFixedCylinder();
            }

            // 升降电机去工作位
            if (isTest)
            {
                this.MoveEjectionTableToMarkPosition();
            }
            else
            {
                this.MoveEjectionTableToWorkPosition();
            }

            EjectDevicePara.CurrentSlotConfig = WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig.EjectionBankSlots[i];
            WaferSystemProgram.GetInstance().EjectionBankProgram.EjectionBankEntity.EjectionBankSlotEntities[EjectDevicePara.CurrentSlotConfig.Index].SlotState = EjectSlotStatuEnum.Using;
            WaferSystemProgram.GetInstance().Save();
            EjectionBankConfigRepository.GetInstance().Save();
            WaferSubDevicePara.GetInstance().Save();
            Thread.Sleep(10);

            // 当前顶针去预顶位
            this.MoveEjectToReadyLiftPosition(true);
        }

        /// <summary>
        /// 升起顶针台（不还回顶针）
        /// </summary>
        /// <param name="i">i</param>
        //public void RiseEjectionWithoutReturnEjection(int i)
        //{
        //    if (MachineStateModel.GetInstance().IsOffLineWork)
        //    {
        //        this.ReturnEjection();
        //        EjectDevicePara.CurrentSlotConfig = WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig.EjectionBankSlots[i];
        //        WaferSystemProgram.GetInstance().EjectionBankProgram.EjectionBankEntity.EjectionBankSlotEntities[EjectDevicePara.CurrentSlotConfig.Index].SlotState = EjectSlotStatuEnum.Using;
        //        WaferSystemProgram.GetInstance().Save();
        //        EjectionBankConfigRepository.GetInstance().Save();
        //        WaferSubDevicePara.GetInstance().Save();
        //        return;
        //    }

        //    // 晶圆台去准备位
        //    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToReadyPosition();

        //    // 旋转电机去目标顶针位
        //    WaferSubController.GetInstance().EjectController.MoveEjectionBankToSlotPosition(i);

        //    // 升降电机去气缸动作位
        //    this.MoveEjectionTableToFixedCylinderActionPosition(false);

        //    //// 磁吸感应器感应顶针
        //    //if (!EjectModule.ContactSensor1.CheckStateForNums(true, 3, 200) || !EjectModule.ContactSensor2.CheckStateForNums(true, 3, 200))
        //    //{
        //    //    throw new Exception("磁吸感应器未感应到顶针！");
        //    //}

        //    if (EjectDevicePara.IsUseEjectFixedCylinder)
        //    {
        //        // 气缸去正限位
        //        this.SetFixedCylinder();
        //    }

        //    // 升降电机去工作位
        //    this.MoveEjectionTableToWorkPosition();

        //    EjectDevicePara.CurrentSlotConfig = WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig.EjectionBankSlots[i];
        //    WaferSystemProgram.GetInstance().EjectionBankProgram.EjectionBankEntity.EjectionBankSlotEntities[EjectDevicePara.CurrentSlotConfig.Index].SlotState = EjectSlotStatuEnum.Using;
        //    WaferSystemProgram.GetInstance().Save();
        //    EjectionBankConfigRepository.GetInstance().Save();
        //    WaferSubDevicePara.GetInstance().Save();
        //    Thread.Sleep(10);

        //    // 当前顶针去预顶位
        //    //this.MoveEjectToReadyLiftPosition(true);
        //}

        /// <summary>
        /// 归还顶针
        /// </summary>
        public void ReturnEjection()
        {
            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsShieldEjectModule)
            {
                return;
            }

            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                if (EjectDevicePara.CurrentSlotConfig != null && EjectDevicePara.CurrentSlotConfig.Name != string.Empty)
                {
                    WaferSystemProgram.GetInstance().EjectionBankProgram.EjectionBankEntity.EjectionBankSlotEntities[EjectDevicePara.CurrentSlotConfig.Index].SlotState = EjectSlotStatuEnum.UnUsing;
                    WaferSystemProgram.GetInstance().Save();
                    EjectionBankConfigRepository.GetInstance().Save();
                    EjectDevicePara.CurrentSlotConfig = null;
                    WaferSubDevicePara.GetInstance().Save();
                }

                return;
            }

            // 气缸去负限位
            this.ResetFixedCylinder();

            // 顶针去安全位
            this.MoveEjectToSafePosition();

            // 升降电机去安全位
            this.MoveEjectionTableToSafePosition();

            if (EjectDevicePara.CurrentSlotConfig != null && EjectDevicePara.CurrentSlotConfig.Name != string.Empty)
            {
                WaferSystemProgram.GetInstance().EjectionBankProgram.EjectionBankEntity.EjectionBankSlotEntities[EjectDevicePara.CurrentSlotConfig.Index].SlotState = EjectSlotStatuEnum.UnUsing;
                WaferSystemProgram.GetInstance().Save();
                EjectionBankConfigRepository.GetInstance().Save();
                EjectDevicePara.CurrentSlotConfig = null;
                WaferSubDevicePara.GetInstance().Save();
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// 顶针台去不剥离位置
        /// </summary>
        public void MoveEjectionToInseparablePos()
        {
            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsShieldEjectModule)
            {
                return;
            }

            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                if (EjectDevicePara.CurrentSlotConfig != null && EjectDevicePara.CurrentSlotConfig.Name != string.Empty)
                {
                    WaferSystemProgram.GetInstance().EjectionBankProgram.EjectionBankEntity.EjectionBankSlotEntities[EjectDevicePara.CurrentSlotConfig.Index].SlotState = EjectSlotStatuEnum.UnUsing;
                    WaferSystemProgram.GetInstance().Save();
                    EjectionBankConfigRepository.GetInstance().Save();
                    EjectDevicePara.CurrentSlotConfig = null;
                    WaferSubDevicePara.GetInstance().Save();
                }

                return;
            }

            // 气缸去负限位
            this.ResetFixedCylinder();

            // 顶针去安全位
            this.MoveEjectToSafePosition();

            // 升降电机去安全位
            this.MoveEjectionTableAxisZToG0Pos(EjectDevicePara.EjectionInseparablePos, false);
        }

        /// <summary>
        /// 是否归还顶针
        /// </summary>
        /// <returns>return</returns>
        public bool IsReturnEject()
        {
            if (EjectDevicePara.CurrentSlotConfig == null
                || EjectDevicePara.CurrentSlotConfig.Name == string.Empty)
            {
                return true;
            }
            else
            {
                //XtraMessageBox.Show("Please return the eject first!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AKRSXtraMessageBox.Show("请先归还顶针！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        /// <summary>
        /// 获取顶针台气缸状态
        /// </summary>
        /// <returns>return</returns>
        public bool GetFixedCylinder()
        {
            return this.EjectModule.FixedCylinder.CurrentOutputValue;
        }

        /// <summary>
        /// 移动升降Z轴电机到指定位置
        /// </summary>
        /// <param name="g0Pos">g0Pos</param>
        /// <param name="isJudgeWaferTable">是否判断晶圆台位置</param>
        public void MoveEjectionTableAxisZToG0Pos(AKRSPoint3D g0Pos, bool isJudgeWaferTable = true)
        {
            //if (MachineStateModel.GetInstance().IsOffLineWork)
            //{
            //    return;
            //}

            //WaferSystemDomain.GetInstance().SingleStep();

            //this.CheckEjectionBankAxisTAtWorkPosition();

            //if (isJudgeWaferTable)
            //{
            //    WaferSubController.GetInstance().WaferTableController.CheckWaferTableInCollisionCircle();
            //}
            
            //double z = MachineCoordinateSystem.GetInstance().NullCoordinateSystem.G0PosToSelf(g0Pos).Z;
            //ExcuteResult ret = this.EjectModule.EjectionTableAxisZ.AbsoluteMove(z, AccuracyMode.HighAccuracy);
            //if (ret != ExcuteResult.Success)
            //{
            //    //throw new Exception("EjectionTableAxisZ positioning failure.");
            //    throw new Exception("顶针台移动到目标位置失败！");
            //}

            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return;
                }

                WaferSystemDomain.GetInstance().SingleStep();

                this.CheckEjectionBankAxisTAtWorkPosition();

                if (isJudgeWaferTable)
                {
                    WaferSubController.GetInstance().WaferTableController.CheckWaferTableInCollisionCircle();
                }

                MovePara movePara = new MovePara()
                                        {
                                            Vel = this.EjectModule.EjectionTableAxisZ.AxisMovePara.AbsoluteMoveSpeed * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage,
                                            Acc = this.EjectModule.EjectionTableAxisZ.AxisMovePara.ACC,
                                            Dec = this.EjectModule.EjectionTableAxisZ.AxisMovePara.DEC,
                                            Jerk = this.EjectModule.EjectionTableAxisZ.AxisMovePara.Jerk,
                                            TargetPosition = MachineCoordinateSystem.GetInstance().NullCoordinateSystem.G0PosToSelf(g0Pos).Z
                };
                ExcuteResult ret = this.EjectModule.EjectionTableAxisZ.AbsoluteMove(movePara, false, AccuracyMode.HighAccuracy);
                if (ret != ExcuteResult.Success)
                {
                    //throw new Exception("EjectionTableAxisZ positioning failure.");
                    throw new Exception("顶针台移动到目标位置失败！");
                }
            }
        }

        /// <summary>
        /// 移动顶针Z轴电机到指定位置
        /// </summary>
        /// <param name="g0Pos">g0Pos</param>
        /// <param name="isJudgeWaferTable">是否判断晶圆台位置</param>
        public void MoveEjectionAxisZToG0Pos(AKRSPoint3D g0Pos, bool isJudgeWaferTable = true)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            WaferSystemDomain.GetInstance().SingleStep();

            this.CheckEjectionBankAxisTAtWorkPosition();

            if (isJudgeWaferTable)
            {
                WaferSubController.GetInstance().WaferTableController.CheckWaferTableInCollisionCircle();
            }

            MovePara movePara = new MovePara()
                                    {
                                        Vel = this.EjectModule.EjectionAxisZ.AxisMovePara.AbsoluteMoveSpeed,
                                        Acc = this.EjectModule.EjectionAxisZ.AxisMovePara.ACC,
                                        Dec = this.EjectModule.EjectionAxisZ.AxisMovePara.DEC,
                                        Jerk = this.EjectModule.EjectionAxisZ.AxisMovePara.Jerk,
                                        TargetPosition = MachineCoordinateSystem.GetInstance().NullCoordinateSystem.G0PosToSelf(g0Pos).Z
            };

            ExcuteResult ret = this.EjectModule.EjectionAxisZ.AbsoluteMove(movePara, false, AccuracyMode.HighAccuracy);
            if (ret != ExcuteResult.Success)
            {
                //throw new Exception("EjectionAxisZ positioning failure.");
                throw new Exception("顶针移动到目标位置失败！");
            }
        }

        /// <summary>
        /// 移动顶针Z轴电机到指定位置
        /// </summary>
        /// <param name="g0Pos">g0Pos</param>
        /// <param name="vel">速度</param>
        public void MoveEjectionAxisZToG0Pos(AKRSPoint3D g0Pos, double vel)
        {
            //LogHelper.Post(Level.Info, $"-----------------------顶针准备移动，顶针G0坐标{WaferSubController.GetInstance().EjectController.EjectionAxisZG0Pos.Z}，机械坐标{this.EjectModule.EjectionAxisZ.GetRealPosition()}", LogCategory.Global);

            //if (MachineStateModel.GetInstance().IsOffLineWork)
            //{
            //    return;
            //}

            //WaferSystemDomain.GetInstance().SingleStep();

            //this.CheckEjectionBankAxisTAtWorkPosition();
            //WaferSubController.GetInstance().WaferTableController.CheckWaferTableInCollisionCircle();

            //MovePara movePara;
            //if (EjectDevicePara.IsUseEjectHardwarePara)
            //{
            //    movePara = new MovePara()
            //                   {
            //                       Vel = this.EjectModule.EjectionAxisZ.AxisMovePara.AbsoluteMoveSpeed,
            //                       Acc = this.EjectModule.EjectionAxisZ.AxisMovePara.ACC,
            //                       Dec = this.EjectModule.EjectionAxisZ.AxisMovePara.DEC,
            //                       Jerk = this.EjectModule.EjectionAxisZ.AxisMovePara.Jerk,
            //                       TargetPosition = MachineCoordinateSystem.GetInstance().NullCoordinateSystem.G0PosToSelf(g0Pos).Z
            //                   };
            //}
            //else
            //{
            //    movePara = new MovePara()
            //                   {
            //                       Vel = vel,
            //                       Acc = vel * 10,
            //                       Dec = vel * 10,
            //                       Jerk = this.EjectModule.EjectionAxisZ.AxisMovePara.Jerk,
            //                       TargetPosition = MachineCoordinateSystem.GetInstance().NullCoordinateSystem.G0PosToSelf(g0Pos).Z
            //                   };
            //}

            //ExcuteResult ret = this.EjectModule.EjectionAxisZ.AbsoluteMove(movePara, false, AccuracyMode.HighAccuracy);
            //if (ret != ExcuteResult.Success)
            //{
            //    //throw new Exception("EjectionAxisZ positioning failure.");
            //    throw new Exception("顶针移动到目标位置失败！");
            //}

            //LogHelper.Post(Level.Info, $"--------------顶针移动完成，顶针G0坐标{WaferSubController.GetInstance().EjectController.EjectionAxisZG0Pos.Z}，机械坐标{this.EjectModule.EjectionAxisZ.GetRealPosition()}", LogCategory.Global);

            {
                LogHelper.Post(Level.Info, $"-----------------------顶针准备移动，顶针G0坐标{WaferSubController.GetInstance().EjectController.EjectionAxisZG0Pos.Z}，机械坐标{this.EjectModule.EjectionAxisZ.GetRealPosition()}", LogCategory.Global);

                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return;
                }

                WaferSystemDomain.GetInstance().SingleStep();

                this.CheckEjectionBankAxisTAtWorkPosition();
                WaferSubController.GetInstance().WaferTableController.CheckWaferTableInCollisionCircle();

                MovePara movePara;
                if (EjectDevicePara.IsUseEjectHardwarePara)
                {
                    movePara = new MovePara()
                                   {
                                       Vel = this.EjectModule.EjectionAxisZ.AxisMovePara.AbsoluteMoveSpeed,
                                       Acc = this.EjectModule.EjectionAxisZ.AxisMovePara.ACC,
                                       Dec = this.EjectModule.EjectionAxisZ.AxisMovePara.DEC,
                                       Jerk = this.EjectModule.EjectionAxisZ.AxisMovePara.Jerk,
                                       TargetPosition = MachineCoordinateSystem.GetInstance().NullCoordinateSystem.G0PosToSelf(g0Pos).Z
                                   };
                }
                else
                {
                    movePara = new MovePara()
                                   {
                                       Vel = vel,
                                       Acc = vel * 10,
                                       Dec = vel * 10,
                                       Jerk = this.EjectModule.EjectionAxisZ.AxisMovePara.Jerk,
                                       TargetPosition = MachineCoordinateSystem.GetInstance().NullCoordinateSystem.G0PosToSelf(g0Pos).Z
                                   };
                }

                ExcuteResult ret = this.EjectModule.EjectionAxisZ.AbsoluteMove(movePara, false, AccuracyMode.HighAccuracy);
                if (ret != ExcuteResult.Success)
                {
                    //throw new Exception("EjectionAxisZ positioning failure.");
                    throw new Exception("顶针移动到目标位置失败！");
                }

                LogHelper.Post(Level.Info, $"--------------顶针移动完成，顶针G0坐标{WaferSubController.GetInstance().EjectController.EjectionAxisZG0Pos.Z}，机械坐标{this.EjectModule.EjectionAxisZ.GetRealPosition()}", LogCategory.Global);
            }
        }

        /// <summary>
        /// 移动顶针旋转电机到指定位置
        /// </summary>
        /// <param name="g0Pos">g0Pos</param>
        public void MoveEjectionBankAxisTToG0Pos(AKRSPoint3D g0Pos)
        {
            //if (MachineStateModel.GetInstance().IsOffLineWork)
            //{
            //    return;
            //}

            //WaferSystemDomain.GetInstance().SingleStep();

            //this.CheckEjectionTableAxisZAtSafePosition();
            //this.CheckEjectionAxisZAtSafePosition();
            //double x = MachineCoordinateSystem.GetInstance().NullCoordinateSystem.G0PosToSelf(g0Pos).X;
            //ExcuteResult ret = this.EjectModule.EjectionBankAxisT.AbsoluteMove(x, AccuracyMode.HighAccuracy);
            //if (ret != ExcuteResult.Success)
            //{
            //    //throw new Exception("EjectionBankAxisT positioning failure.");
            //    throw new Exception("顶针架移动到目标位置失败！");
            //}

            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return;
                }

                WaferSystemDomain.GetInstance().SingleStep();

                this.CheckEjectionTableAxisZAtSafePosition();
                this.CheckEjectionAxisZAtSafePosition();
                MovePara movePara = new MovePara()
                                        {
                                            Vel = this.EjectModule.EjectionBankAxisT.AxisMovePara.AbsoluteMoveSpeed,
                                            Acc = this.EjectModule.EjectionBankAxisT.AxisMovePara.ACC,
                                            Dec = this.EjectModule.EjectionBankAxisT.AxisMovePara.DEC,
                                            Jerk = this.EjectModule.EjectionBankAxisT.AxisMovePara.Jerk,
                                            TargetPosition = MachineCoordinateSystem.GetInstance().NullCoordinateSystem.G0PosToSelf(g0Pos).X
                                        };
                ExcuteResult ret = this.EjectModule.EjectionBankAxisT.AbsoluteMove(movePara, false, AccuracyMode.HighAccuracy);
                if (ret != ExcuteResult.Success)
                {
                    //throw new Exception("EjectionBankAxisT positioning failure.");
                    throw new Exception("顶针架移动到目标位置失败！");
                }
            }
        }

        /// <summary>
        /// 顶针升降电机是否在安全位置
        /// </summary>
        public void CheckEjectionTableAxisZAtSafePosition()
        {
            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsShieldEjectModule)
            {
                return;
            }

            if (Math.Abs(this.EjectionTableAxisZG0Pos.Z - EjectDevicePara.UpDownSafePosition.Z) > this.range)
            {
                //throw new Exception("EjectionTableAxisZ not at SafePosition.");
                DialogResult res = AKRSMessageBoxExt.Show($"顶针台不在安全位置！" + "\r\n" + "点击OK，即将移动到位，" + "\r\n" + "点击Abort，即将终止！", "Prompt", new string[] { "OK", "Abort" }, new DialogResult[] { DialogResult.OK, DialogResult.Abort });
                switch (res)
                {
                    case DialogResult.OK:
                        this.MoveEjectionTableToSafePosition();
                        return;
                    case DialogResult.Abort:
                        throw new Exception("顶针台不在安全位置！");
                }
            }
        }

        /// <summary>
        /// 顶针电机是否在安全位置
        /// </summary>
        public void CheckEjectionAxisZAtSafePosition()
        {
            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsShieldEjectModule)
            {
                return;
            }

            if (Math.Abs(this.EjectionAxisZG0Pos.Z - EjectDevicePara.EjectSafePosition.Z) > this.range)
            {
                //throw new Exception("EjectionAxisZ not at SafePosition.");
                DialogResult res = AKRSMessageBoxExt.Show($"顶针不在安全位置！" + "\r\n" + "点击OK，即将移动到位，" + "\r\n" + "点击Abort，即将终止！", "Prompt", new string[] { "OK", "Abort" }, new DialogResult[] { DialogResult.OK, DialogResult.Abort });
                switch (res)
                {
                    case DialogResult.OK:
                        this.MoveEjectToSafePosition();
                        return;
                    case DialogResult.Abort:
                        throw new Exception("顶针不在安全位置！");
                }
            }
        }

        /// <summary>
        /// 顶针电机是否在预顶位置
        /// </summary>
        public void CheckEjectionAxisZAtReadyLiftPosition()
        {
            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsShieldEjectModule)
            {
                return;
            }

            // 检查比预顶位置低即可
            if (this.EjectionAxisZG0Pos.Z >= EjectDevicePara.CurrentSlotConfig.EjectionConfig.ReadyLiftPosition.Z + this.range)
            {
                //Console.WriteLine($"顶针当前位置{this.EjectionAxisZG0Pos.Z}");

                //throw new Exception("EjectionAxisZ not at ReadyLiftPosition, Please reset EjectionAxisZ!");
                DialogResult res = AKRSMessageBoxExt.Show($"顶针不在预顶位置！" + "\r\n" + "点击OK，即将移动到位，" + "\r\n" + "点击Abort，即将终止！", "Prompt", new string[] { "OK", "Abort" }, new DialogResult[] { DialogResult.OK, DialogResult.Abort });
                switch (res)
                {
                    case DialogResult.OK:
                        this.MoveEjectToReadyLiftPosition();
                        return;
                    case DialogResult.Abort:
                        throw new Exception("顶针不在预顶位置, 请复位顶针！");
                }
            }
        }

        /// <summary>
        /// 顶针旋转电机是否在顶针位置
        /// </summary>
        public void CheckEjectionBankAxisTAtWorkPosition()
        {
            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsShieldEjectModule)
            {
                return;
            }

            for (int i = 0; i < WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig.EjectionBankSlots.Length; i++)
            {
                if (Math.Abs(this.EjectionBankAxisTG0Pos.X - WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig.EjectionBankSlots[i].SlotPosition.X) < this.range)
                {
                    return;
                }
            }

            //throw new Exception("EjectionBankAxisT not at WorkPosition!");
        }

        /// <summary>
        ///  顶针使用次数+1
        /// </summary>
        public void AddCountOfUseable()
        {
            if (this.EjectDevicePara.IsShieldEjectModule == false)
            {
                this.EjectDevicePara.CurrentSlotConfig.EjectionConfig.Frequency.CurrentUseTimes++;
            }
        }
    }
}
