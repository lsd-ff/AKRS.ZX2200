#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/11/8 10:26:00
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>

using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.WaferSubSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.SupportFeature.Parameters.Attributes;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using AKRS.ZX2200.WaferSubSystem.Tasks;

    using DevExpress.XtraReports.UI;

    using Newtonsoft.Json;

    /// <summary>
    /// 描述：WaferSystemDomain
    /// </summary>
    public class WaferSystemDomain : SingletonNoSave<WaferSystemDomain>
    {
        /// <summary>
        /// 模组
        /// </summary>
        [JsonIgnore]
        public WaferSubModule WaferSubModule => WaferSubModule.GetInstance();

        /// <summary>
        /// 设备参数
        /// </summary>
        [JsonIgnore]
        public WaferSubDevicePara WaferSubDevicePara => WaferSubDevicePara.GetInstance();

        /// <summary>
        /// 程式
        /// </summary>
        [JsonIgnore]
        public WaferSystemProgram WaferSystemProgram => WaferSystemProgram.GetInstance();

        /// <summary>
        /// 搜晶
        /// </summary>
        [JsonIgnore]
        public Block Block => Block.GetInstance();

        /// <summary>
        /// 线程
        /// </summary>
        [JsonIgnore]
        public WaferSubSystemTask WaferSubSystemTask { get; set; } = new WaferSubSystemTask();

        /// <summary>
        /// 翻转台线程
        /// </summary>
        [JsonIgnore]
        public FlipTask FlipTask { get; set; } = new FlipTask();

        /// <summary>
        /// 提示窗口
        /// </summary>
        private DialogResult res;

        /// <summary>
        /// Initialization
        /// </summary>
        public void Initialization()
        {
            // 提前让BondZ去安全位置
            System2Domain.GetInstance().BondHeadController.MoveBondZToSafePos();

            // FlipChipModule.GetInstance().AxisT.GoHome();
            WaferSubController.GetInstance().WaferTableController.ResetWaferClampCylinder();
            WaferSubController.GetInstance().MagazineController.ResetWaferPushCylinder();
            WaferSubController.GetInstance().EjectController.ResetFixedCylinder();
            WaferSubModule.Eject.EjectionAxisZ.GoHome();
            WaferSubModule.Eject.EjectionTableAxisZ.GoHome();
            WaferSubModule.Eject.EjectionBankAxisT.GoHome();
            WaferSubController.GetInstance().EjectController.MoveEjectionBankToSlotPosition(0);

            if (MachineHardwareConfiguration.GetInstance().IsMagazineConfigured)
            {
                WaferSubModule.WaferTable.WaferClampAxisY.GoHome();
                WaferSubModule.MagazineBox.MagazineAxisZ.GoHome();
            }

            WaferSubModule.WaferTable.WaferTableAxisX.GoHome();
            WaferSubModule.WaferTable.WaferTableAxisY.GoHome();
            WaferSubModule.WaferTable.ExpandAxisZ.GoHome();
            WaferSubModule.WaferTable.WaferCameraAxisZ.GoHome();
        }

        /// <summary>
        /// SingleStep
        /// </summary>
        public void SingleStep()
        {
            if (MachineStateModel.GetInstance().IsSingleStepWork)
            {
                if (!SignalPool.GetInstance().System2SingleStepSignal.WaitSingleStep())
                {
                    //throw new Exception("Wait SingleStepSignal failed");
                    throw new Exception("等待单步信号失败！");
                }
            }
        }

        /// <summary>
        /// 晶圆模块是否检查完成
        /// </summary>
        /// <param name="isNoJudgeAssistant">是否屏蔽判断示教</param>
        /// <param name="isAloneDryCycle">是否单独空泡</param>
        /// <returns>return</returns>
        public bool CheckIsReady(bool isNoJudgeAssistant = false, bool isAloneDryCycle = false, bool isRecordSearchData = false, bool isSearchOne = false)
        {
            try
            {
                WaferSubSystemTask.IsAloneDryCycle = isAloneDryCycle;
                WaferSubSystemTask.IsRecordSearchData = isRecordSearchData;
                WaferSubSystemTask.IsSearchOne = isSearchOne;

                // 提前让BondZ去安全位置
                System2Domain.GetInstance().BondHeadController.MoveBondZToSafePos();

                WaferSubController.GetInstance().MagazineController.ResetWaferPushCylinder();
                WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();

                if (!MachineStateModel.GetInstance().IsOffLineWork)
                {
                    if (!WaferSubDevicePara.GetInstance().EjectDevicePara.IsShieldEjectModule)
                    {        
                        // 防止magazine未配置
                        if (this.WaferSystemProgram.MagazineProgram.MagazineBoxConfig == null)
                        {
                            AKRSXtraMessageBox.Show("MagazineBox料盒未配置，请先配置料盒！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }

                        // 防止magazine硬件未配置
                        if (this.WaferSystemProgram.MagazineProgram.MagazineBoxConfig.MagazineBoxGeo == null)
                        {
                            AKRSXtraMessageBox.Show("MagazineBox料盒硬件未配置，请先配置料盒硬件！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }

                        // 检测magazine示教状态
                        if (!this.WaferSystemProgram.MagazineProgram.MagazineBoxConfig.IsAssistantSucceed)
                        {
                            //XtraMessageBox.Show("MagazineBox assistant is not succeed, Please assistant!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            AKRSXtraMessageBox.Show("MagazineBox料盒没有示教完成，请示教后重试！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                    }
                   
                    if (!isNoJudgeAssistant)
                    {
                        // 检测顶针示教状态
                        foreach (var item in this.WaferSystemProgram.GetDistinctEjectionSettings())
                        {
                            if (!item.IsAssistantSucceed)
                            {
                                //XtraMessageBox.Show("Ejection assistant is not succeed, Please assistant!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                AKRSXtraMessageBox.Show("顶针没有示教完成，请示教后重试！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return false;
                            }
                        }

                        // 检测芯片示教状态
                        foreach (var item in WaferSystemProgram.GetInstance().GetCarriers())
                        {
                            if (!item.IsAssistantSucceed)
                            {
                                //XtraMessageBox.Show("Component assistant is not succeed, Please assistant!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                AKRSXtraMessageBox.Show("芯片没有示教完成，请示教后重试！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return false;
                            }

                            if (item is CarrierWithWaferConfig carrier)
                            {
                                if (carrier.IsMapping)
                                {
                                    if (!Block.GetInstance().IsInSearchRange(carrier.ReferencePosition, carrier))
                                    {
                                        AKRSXtraMessageBox.Show($"芯片{item.Name}参考点1物理位置设置超出搜晶范围，请修改参数后重试！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return false;
                                    }
                                }

                                if (carrier.WaferRadius > WaferSubDevicePara.WaferTableDevicePara.WaferTableRadius)
                                {
                                    DialogResult res = AKRSMessageBoxExt.Show($"芯片{item.Name}的限位比环限位大，请检查！" + "\r\n" + "点击OK，即将继续，" + "\r\n" + "点击Abort，即将终止！", "Prompt", new string[] { "OK", "Abort" }, new DialogResult[] { DialogResult.OK, DialogResult.Abort });
                                    switch (res)
                                    {
                                        case DialogResult.OK:
                                            break;
                                        case DialogResult.Abort:
                                            return false;
                                    }
                                }

                                if (carrier.IsActivateSynchronousEjection
                                    && carrier.PickupForceMode == ForceModeEnum.Force)
                                {
                                    AKRSXtraMessageBox.Show($"芯片:{item.Name} 力控模式下不能选择同步顶，请修改取片参数！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    return false;
                                }

                                if (carrier.IsUseFlipTable && carrier.SearchCamera == SearchCameraEnum.BondCamera)
                                {
                                    AKRSXtraMessageBox.Show($"芯片:{item.Name} 不能同时选择用Bond相机搜晶和使用翻转模组！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    return false;
                                }

                                if (carrier.AdjustCamera != CameraTypeEnum.BondCamera && carrier.AdjustCamera != CameraTypeEnum.UpLookCamera)
                                {
                                    AKRSXtraMessageBox.Show($"芯片:{item.Name} 的纠偏相机为{carrier.AdjustCamera.GetDescription()},请重新选择纠偏相机！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    return false;
                                }
                            }

                            if (item.IsResetForceControlAdvanceDuringPickup
                                && item.PickupForceMode == ForceModeEnum.Distance)
                            {
                                AKRSXtraMessageBox.Show($"芯片:{item.Name} 距离模式下不能选择取片提前退出力控，请修改取片参数！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return false;
                            }

                            if (item.IsResetForceControlAdvanceDuringPickup
                                && item.IsActivateSlowTravelAfterPickup)
                            {
                                AKRSXtraMessageBox.Show($"芯片:{item.Name} 不能同时选择取片提前退出力控和开启取料后慢速上升，请修改取片参数！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return false;
                            }

                            if (BondProgram.GetInstance().NozzleList.Exists(it => it.Name == item.NozzleName) == false)
                            {
                                AKRSXtraMessageBox.Show($"芯片:{item.Name} 所绑定的吸嘴：{item.NozzleName}不在吸嘴架上，请重新选择！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return false;
                            }

                            if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured == false
                                && item.DipMode != DipModeEnum.Off)
                            {
                                AKRSXtraMessageBox.Show($"机台未配置刮胶盘，芯片:{item.Name} 不能设置为蘸胶，请重新设置参数！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return false;
                            }

                            if (System2Domain.GetInstance().NozzleShelfController.IsConfiguredOnToolBank(item.NozzleName) == false)
                            {
                                AKRSXtraMessageBox.Show($"芯片{item.Name}对应的吸嘴:{item.NozzleName}未配置到吸嘴架，请重新选择吸嘴！","Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return false;
                            }
                        }

                        // 检查翻转台示教状态
                        if (MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated
                            && WaferSubDevicePara.GetInstance().FlipChipDevicePara.IsCompleted == false) 
                        {
                            return false;
                        }
                    }
                }


                if (WaferSubDevicePara.GetInstance().MagazineDevicePara.IsUseMagazineLift)
                {
                    // 动前检测
                    WaferSubController.GetInstance().WaferTableController.CheckWaferClampAxisYAtSafePosition();
                    WaferSubController.GetInstance().MagazineController.CheckMagazineAxisZAtSafePosition();

                    // 自动模式下--如果检测到晶圆台内有手动换上去的料片
                    if (!(WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is NullTablet) && WaferSubDevicePara.GetInstance().MagazineDevicePara.CurrentLayerNo <= 0)
                    {
                        WaferSubController.GetInstance().EjectController.ReturnEjection();
                        WaferSubController.GetInstance().WaferTableController.MoveExpandToDownPosition();
                        WaferSubController.GetInstance().WaferTableController.ResetBlockCylinder();
                        //this.res = XtraMessageBox.Show("There is a tablet on the wafer table, please manually remove it!", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        this.res = AKRSXtraMessageBox.Show("晶圆台内有一片料，请手动移除它！", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (this.res == DialogResult.OK)
                        {
                            WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet = new NullTablet();
                            WaferSubDevicePara.GetInstance().Save();
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is NullTablet && WaferSubDevicePara.GetInstance().MagazineDevicePara.CurrentLayerNo <= 0)
                    {
                        // 虽然记忆没有料片
                        WaferSubController.GetInstance().WaferTableController.JudgeTabletOnWaferTable(false);
                    }

                    // 动后检测
                    WaferSubController.GetInstance().WaferTableController.CheckWaferClampAxisYAtSafePosition();
                    WaferSubController.GetInstance().MagazineController.CheckMagazineAxisZAtSafePosition();
                }
                else
                {
                    // 动前检测
                    WaferSubController.GetInstance().WaferTableController.CheckWaferClampAxisYAtSafePosition();
                    WaferSubController.GetInstance().MagazineController.CheckMagazineAxisZAtSafePosition();

                    // 手动模式下--如果检测到晶圆台内有自动换上去的料片
                    if (WaferSubDevicePara.GetInstance().MagazineDevicePara.CurrentLayerNo > 0)
                    {
                        WaferSubController.GetInstance().WaferTableController.PlaceWaferInSlot();
                        WaferSubController.GetInstance().WaferTableController.MoveWaferClampToSafePosition();
                        WaferSubController.GetInstance().MagazineController.MoveMagazineToSafePosition();
                    }
                    else if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is NullTablet)
                    {
                        // 虽然记忆没有料片
                        WaferSubController.GetInstance().WaferTableController.JudgeTabletOnWaferTable(false);
                    }

                    // 动后检测
                    WaferSubController.GetInstance().WaferTableController.CheckWaferClampAxisYAtSafePosition();
                    WaferSubController.GetInstance().MagazineController.CheckMagazineAxisZAtSafePosition();
                }

                // 生成静态华夫盘状态
                if (MachineHardwareConfiguration.GetInstance().IsStaticWaffleConfigrated & WaferSubDevicePara.GetInstance().WaferTableDevicePara.IsNeedReCreateStateWithStaticAdapterTablet)
                {
                    WaferSubDevicePara.GetInstance().WaferTableDevicePara.CreateStateWithStaticAdapterTablet();
                    WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentStaticAdapterTablet.SlotState = SlotStatuEnum.Good;
                    WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentStaticAdapterTablet?.AdapterState?.SetAllSlotState(SlotStatuEnum.Good);
                    WaferSubDevicePara.GetInstance().WaferTableDevicePara.ResetNeedReCreateStateWithStaticAdapterTabletSignal();
                }

                // 生成magazine状态
                if (WaferSubDevicePara.GetInstance().MagazineDevicePara.IsNeedReCreateState/* && WaferSubDevicePara.GetInstance().MagazineDevicePara.IsUseMagazineLift*/)
                {
                    this.WaferSystemProgram.MagazineAllocationsProgram.CurrentAllocationsConfig?.SetAllSlotState(SlotStatuEnum.Good);

                    if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is WaferTablet wt)
                    {
                        wt.InitTabletInfo();
                    }
                    else if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is AdapterTablet adt)
                    {
                        adt.CreateAdapterEntity();
                        adt.AdapterState.SetAllSlotState(SlotStatuEnum.Good);
                    }

                    WaferSubDevicePara.GetInstance().MagazineDevicePara.ResetNeedReCreateStateSignal();
                }

                if (!WaferSubController.GetInstance().WaferTableController.IsInCollisionCircle(WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos))
                {
                    // 确保顶针在安全状态
                    WaferSubController.GetInstance().EjectController.ReturnEjection();
                    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToReadyPosition();
                }

                return true;
            }
            catch (Exception e)
            {
                AKRSXtraMessageBox.Show($"{e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 智能化示教前检查
        /// </summary>
        /// <returns>检查结果</returns>
        public bool CheckTablet()
        {
            try
            {
                if (WaferSubDevicePara.GetInstance().MagazineDevicePara.IsUseMagazineLift)
                {
                    // 动前检测
                    WaferSubController.GetInstance().WaferTableController.CheckWaferClampAxisYAtSafePosition();
                    WaferSubController.GetInstance().MagazineController.CheckMagazineAxisZAtSafePosition();

                    // 自动模式下--如果检测到晶圆台内有手动换上去的料片
                    if (!(WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is NullTablet) && WaferSubDevicePara.GetInstance().MagazineDevicePara.CurrentLayerNo <= 0)
                    {
                        WaferSubController.GetInstance().EjectController.ReturnEjection();
                        WaferSubController.GetInstance().WaferTableController.MoveExpandToDownPosition();
                        WaferSubController.GetInstance().WaferTableController.ResetBlockCylinder();
                        //this.res = XtraMessageBox.Show("There is a tablet on the wafer table, please manually remove it!", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        this.res = AKRSXtraMessageBox.Show("晶圆台内有一片料，请手动移除它！", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (this.res == DialogResult.OK)
                        {
                            WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet = new NullTablet();
                            WaferSubDevicePara.GetInstance().Save();
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is NullTablet && WaferSubDevicePara.GetInstance().MagazineDevicePara.CurrentLayerNo <= 0)
                    {
                        // 虽然记忆没有料片
                        WaferSubController.GetInstance().WaferTableController.JudgeTabletOnWaferTable(false);
                    }

                    // 动后检测
                    WaferSubController.GetInstance().WaferTableController.CheckWaferClampAxisYAtSafePosition();
                    WaferSubController.GetInstance().MagazineController.CheckMagazineAxisZAtSafePosition();
                }
                else
                {
                    // 动前检测
                    WaferSubController.GetInstance().WaferTableController.CheckWaferClampAxisYAtSafePosition();
                    WaferSubController.GetInstance().MagazineController.CheckMagazineAxisZAtSafePosition();

                    // 手动模式下--如果检测到晶圆台内有自动换上去的料片
                    if (WaferSubDevicePara.GetInstance().MagazineDevicePara.CurrentLayerNo > 0)
                    {
                        WaferSubController.GetInstance().WaferTableController.PlaceWaferInSlot();
                        WaferSubController.GetInstance().WaferTableController.MoveWaferClampToSafePosition();
                        WaferSubController.GetInstance().MagazineController.MoveMagazineToSafePosition();
                    }
                    else if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is NullTablet)
                    {
                        // 虽然记忆没有料片
                        WaferSubController.GetInstance().WaferTableController.JudgeTabletOnWaferTable(false);
                    }

                    // 动后检测
                    WaferSubController.GetInstance().WaferTableController.CheckWaferClampAxisYAtSafePosition();
                    WaferSubController.GetInstance().MagazineController.CheckMagazineAxisZAtSafePosition();
                }

                return true;
            }
            catch (Exception e)
            {
                AKRSXtraMessageBox.Show($"{e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
