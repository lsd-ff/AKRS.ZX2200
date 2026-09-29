using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.AutoFocusing;
using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.LogicHardware.Services;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Controls;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
using AKRS.ZX2200.WaferSubSystem.Modules;
using AKRS.ZX2200.WaferSubSystem.Services;
using DevExpress.XtraEditors;
using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Controllers
{
    using AKRS.Galaxy2.Machine.Enums;

    using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
    using System.Threading;
    using System.Threading.Tasks;

    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using PostSharp;

    /// <summary>
    /// WaferTableController
    /// </summary>
    public class WaferTableController
    {
        /// <summary>
        /// 是否取消传感器等待
        /// </summary>
        [JsonIgnore]
        private bool isCancelWait;

        /// <summary>
        /// 提示窗口
        /// </summary>
        [JsonIgnore]
        private DialogResult res;

        /// <summary>
        /// 允许误差
        /// </summary>
        private double range = 1;

        /// <summary>
        /// 异步移动magazine去安全位置委托
        /// </summary>
        private Action asyncMoveMagazineToSafePosition;

        /// <summary>
        /// 异步移动magazine去安全位置委托结果
        /// </summary>
        private IAsyncResult asyncMoveMagazineToSafePositionResult;

        /// <summary>
        /// 异步移动晶圆台去自动换料位委托
        /// </summary>
        private Action asyncMoveWaferTableToAutoChangePosition;

        /// <summary>
        /// 异步移动晶圆台去自动换料位委托结果
        /// </summary>
        private IAsyncResult asyncMoveWaferTableToAutoChangePositionResult;

        /// <summary>
        /// 异步移动扩晶到压膜位委托
        /// </summary>
        private Action asyncMoveExpandToDownPosition;

        /// <summary>
        /// 异步移动扩晶到压膜位委托结果
        /// </summary>
        private IAsyncResult asyncMoveExpandToDownPositionResult;

        /// <summary>
        /// 是否是手动更换料片
        /// </summary>
        [JsonIgnore]
        private bool isManualChangeTablet;

        /// <summary>
        /// 晶圆感应器是否有信号
        /// </summary>
        [JsonIgnore]
        public bool IsWaferSensor => this.IsExistTabletOnWaferTable();

        /// <summary>
        /// 晶圆台模组
        /// </summary>
        [JsonIgnore]
        private WaferTableModule WaferTableModule => WaferSubModule.GetInstance().WaferTable;

        /// <summary>
        /// 晶圆台设备参数
        /// </summary>
        [JsonIgnore]
        private WaferTableDevicePara WaferTableDevicePara => WaferSubDevicePara.GetInstance().WaferTableDevicePara;

        /// <summary>
        /// Magazine设备参数
        /// </summary>
        [JsonIgnore]
        private MagazineBoxDevicePara MagazineBoxDevicePara => WaferSubDevicePara.GetInstance().MagazineDevicePara;

        /// <summary>
        /// 晶圆台当前位置
        /// </summary>
        [JsonIgnore]
        public AKRSPoint3D WaferTableG0Pos
        {
            get
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return new AKRSPoint3D(0, 0, 0);
                }

                return MachineCoordinateSystem.GetInstance().WaferCoordinateSystem.SelfPosToG0(new AKRSPoint3D(this.WaferTableModule.WaferTableAxisX.GetRealPosition(), this.WaferTableModule.WaferTableAxisY.GetRealPosition(), 0));
            }
        }

        /// <summary>
        /// 晶圆台当前位置
        /// </summary>
        [JsonIgnore]
        public AKRSPoint3D WaferTableMachinePos
        {
            get
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return new AKRSPoint3D(0, 0, 0);
                }

                return new AKRSPoint3D(this.WaferTableModule.WaferTableAxisX.GetRealPosition(), this.WaferTableModule.WaferTableAxisY.GetRealPosition(), 0);
            }
        }

        /// <summary>
        /// 晶圆台X轴当前位置
        /// </summary>
        [JsonIgnore]
        public AKRSPoint3D WaferTableAxisXG0Pos
        {
            get
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return new AKRSPoint3D(0, 0, 0);
                }

                return MachineCoordinateSystem.GetInstance().WaferCoordinateSystem.SelfPosToG0(new AKRSPoint3D(this.WaferTableModule.WaferTableAxisX.GetRealPosition(), 0, 0));
            }
        }

        /// <summary>
        /// 晶圆台Y轴当前位置
        /// </summary>
        [JsonIgnore]
        public AKRSPoint3D WaferTableAxisYG0Pos
        {
            get
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return new AKRSPoint3D(0, 0, 0);
                }

                return MachineCoordinateSystem.GetInstance().WaferCoordinateSystem.SelfPosToG0(new AKRSPoint3D(0, this.WaferTableModule.WaferTableAxisY.GetRealPosition(), 0));
            }
        }

        /// <summary>
        /// 晶圆夹Y轴当前位置
        /// </summary>
        [JsonIgnore]
        public AKRSPoint3D WaferClampAxisYG0Pos
        {
            get
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return new AKRSPoint3D(0, 0, 0);
                }

                if (MachineHardwareConfiguration.GetInstance().IsMagazineConfigured == false)
                {
                    return new AKRSPoint3D(0, 0, 0);
                }

                return MachineCoordinateSystem.GetInstance().NullCoordinateSystem.SelfPosToG0(new AKRSPoint3D(0, this.WaferTableModule.WaferClampAxisY.GetRealPosition(), 0));
            }
        }

        /// <summary>
        /// 扩晶电机当前位置
        /// </summary>
        [JsonIgnore]
        public AKRSPoint3D ExpandAxisZG0Pos
        {
            get
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return new AKRSPoint3D(0, 0, 0);
                }

                return MachineCoordinateSystem.GetInstance().NullCoordinateSystem.SelfPosToG0(new AKRSPoint3D(0, 0, this.WaferTableModule.ExpandAxisZ.GetRealPosition()));
            }
        }

        /// <summary>
        /// 晶圆相机Z当前位置
        /// </summary>
        [JsonIgnore]
        public AKRSPoint3D WaferCameraAxisZG0Pos
        {
            get
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return new AKRSPoint3D(0, 0, 0);
                }

                return MachineCoordinateSystem.GetInstance().WaferCoordinateSystem.SelfPosToG0(new AKRSPoint3D(0, 0, this.WaferTableModule.WaferCameraAxisZ.GetRealPosition()));
            }
        }

        /// <summary>
        /// 移动晶圆台到手动换料位
        /// </summary>
        public void MoveWaferTableToManualChangePosition()
        {
            if (Math.Abs(this.WaferTableAxisXG0Pos.X - WaferTableDevicePara.ManualChangePosition.X) > this.range || Math.Abs(this.WaferTableAxisYG0Pos.Y - WaferTableDevicePara.ManualChangePosition.Y) > this.range)
            {
                this.MoveWaferTableToG0Pos(WaferTableDevicePara.ManualChangePosition);
            }
        }

        /// <summary>
        /// 移动晶圆台到自动换料位
        /// </summary>
        public void MoveWaferTableToAutoChangePosition()
        {
            if (Math.Abs(this.WaferTableAxisXG0Pos.X - WaferTableDevicePara.AutoChangePosition.X) > this.range || Math.Abs(this.WaferTableAxisYG0Pos.Y - WaferTableDevicePara.AutoChangePosition.Y) > this.range)
            {
                this.MoveWaferTableToG0Pos(WaferTableDevicePara.AutoChangePosition);
            }
        }

        /// <summary>
        /// 移动晶圆台到准备位
        /// </summary>
        public void MoveWaferTableToReadyPosition()
        {
            if (Math.Abs(this.WaferTableAxisXG0Pos.X - WaferTableDevicePara.ReadyPosition.X) > this.range || Math.Abs(this.WaferTableAxisYG0Pos.Y - WaferTableDevicePara.ReadyPosition.Y) > this.range)
            {
                this.MoveWaferTableToG0Pos(WaferTableDevicePara.ReadyPosition);
            }
        }

        /// <summary>
        /// 移动晶圆台到扫码位
        /// </summary>
        public void MoveWaferTableToScanPosition()
        {
            if (Math.Abs(this.WaferTableAxisXG0Pos.X - WaferTableDevicePara.ScanPosition.X) > this.range || Math.Abs(this.WaferTableAxisYG0Pos.Y - WaferTableDevicePara.ScanPosition.Y) > this.range)
            {
                this.MoveWaferTableToG0Pos(WaferTableDevicePara.ScanPosition);
            }
        }

        /// <summary>
        /// 移动晶圆夹到等待取料位
        /// </summary>
        public void MoveWaferClampToWaitPickPosition()
        {
            if (Math.Abs(this.WaferClampAxisYG0Pos.Y - WaferTableDevicePara.WaferClampWaitPickPosition.Y) > this.range)
            {
                this.MoveWaferClampAxisYToG0Pos(WaferTableDevicePara.WaferClampWaitPickPosition);
            }
        }

        /// <summary>
        /// 移动晶圆夹到晶圆台
        /// </summary>
        public void MoveWaferClampToWaferTable()
        {
            AKRSPoint3D temp = WaferTableDevicePara.WaferClampAtWaferTablePosition.Offset(0, -this.WaferTableDevicePara.WaferClampOffset, 0);
            if (Math.Abs(this.WaferClampAxisYG0Pos.Y - temp.Y) > this.range)
            {
                this.MoveWaferClampAxisYToG0Pos(temp, 30);
            }
        }

        /// <summary>
        /// 移动晶圆夹到magazine
        /// </summary>
        public void MoveWaferClampToMagazine()
        {
            if (Math.Abs(this.WaferClampAxisYG0Pos.Y - WaferTableDevicePara.WaferClampAtMagazinePosition.Y) > this.range)
            {
                this.MoveWaferClampAxisYToG0Pos(WaferTableDevicePara.WaferClampAtMagazinePosition);
            }
        }

        /// <summary>
        /// 移动晶圆夹到安全位
        /// </summary>
        public void MoveWaferClampToSafePosition()
        {
            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsShieldEjectModule)
            {
                return;
            }

            if (Math.Abs(this.WaferClampAxisYG0Pos.Y - WaferTableDevicePara.WaferClampSafePosition.Y) > this.range)
            {
                this.MoveWaferClampAxisYToG0Pos(WaferTableDevicePara.WaferClampSafePosition);
            }
        }

        /// <summary>
        /// 移动扩晶到低位
        /// </summary>
        public void MoveExpandToDownPosition()
        {
            if (!MachineStateModel.GetInstance().IsOffLineWork)
            {
                WaferTableModule.ExpandAxisZ.ResetError();
            }
            
            if (Math.Abs(this.ExpandAxisZG0Pos.Z - this.WaferTableDevicePara.ExpandDownPosition.Z) > this.range)
            {
                this.MoveExpandAxisZToG0Pos(WaferTableDevicePara.ExpandDownPosition, WaferTableDevicePara.ExpandSpeed);
                if (this.WaferTableDevicePara.IsNeedExpandZero)
                {
                    WaferTableModule.ExpandAxisZ.GoHome();
                }
            }
        }

        /// <summary>
        /// 移动扩晶到高位
        /// </summary>
        /// <param name="carrierWithWaferConfig">carrierWithWaferConfig</param>
        public void MoveExpandToUpPosition(CarrierWithWaferConfig carrierWithWaferConfig)
        {
            if (carrierWithWaferConfig.Name == string.Empty)
            {
                //throw new Exception("The carrierWithWaferConfig name is empty, expand axis move failed!");
                throw new Exception("晶圆载具配置为空, 扩晶运动到工作位置失败！");
            }

            if (Math.Abs(this.ExpandAxisZG0Pos.Z - carrierWithWaferConfig.ExpandUpPosition.Z) > this.range)
            {
                this.MoveExpandAxisZToG0Pos(carrierWithWaferConfig.ExpandUpPosition, WaferTableDevicePara.ExpandSpeed);
            }
        }

        /// <summary>
        /// 移动晶圆相机到工作位
        /// </summary>
        /// <param name="baseCarrierConfig">baseCarrierConfig</param>
        public void MoveWaferCameraToWorkPosition(BaseCarrierConfig baseCarrierConfig)
        {
            if (baseCarrierConfig.Name == string.Empty)
            {
                //throw new Exception("The baseCarrierConfig name is empty, camera axis move failed!");
                throw new Exception("载具配置为空, 晶圆相机运动到工作位置失败！");
            }

            // Bond相机搜晶不动晶圆相机Z轴
            if (baseCarrierConfig.SearchCamera == SearchCameraEnum.BondCamera)
            {
                return;
            }

            this.MoveWaferCameraAxisZToG0Pos(baseCarrierConfig.WaferAxisZPosition);
        }

        /// <summary>
        /// 移动晶圆台到参考点1
        /// </summary>
        public void MoveToReferencePoint1()
        {
            this.MoveWaferTableToG0PosWithSearch(WaferSubModule.GetInstance().PointConvertAkrsPoint3D(Block.GetInstance().GetReferencePoint1()));
        }

        /// <summary>
        /// 夹紧晶圆夹气缸
        /// </summary>
        public void SetWaferClampCylinder()
        {
            if (MachineHardwareConfiguration.GetInstance().IsMagazineConfigured == false)
            {
                return;
            }

            if (MachineStateModel.GetInstance().IsNormalWork)
            {
                WaferSubController.GetInstance().SetCyc(WaferTableModule.WaferClampCylinder, WaferTableModule.WaferClampPLimitSensor, 0, WaferTableDevicePara.DelayClampCycActionAfter);

                // 后期需删除
                // WaferSubController.GetInstance().SetCyc(WaferTableModule.WaferClampCylinder, WaferTableDevicePara.DelayClampCycAction);
            }
            else
            {
                WaferSubController.GetInstance().SetCyc(WaferTableModule.WaferClampCylinder, 0, WaferTableDevicePara.DelayClampCycActionAfter);
            }
        }

        /// <summary>
        /// 松开晶圆夹气缸
        /// </summary>
        public void ResetWaferClampCylinder()
        {
            if (MachineHardwareConfiguration.GetInstance().IsMagazineConfigured == false)
            {
                return;
            }

            WaferSubController.GetInstance().ResetCyc(WaferTableModule.WaferClampCylinder, 0, WaferTableDevicePara.DelayClampCycActionAfter);
        }

        /// <summary>
        /// 抬起晶圆夹持气缸
        /// </summary>
        public void SetBlockCylinder()
        {
            WaferSubController.GetInstance().SetCyc(WaferTableModule.BlockCylinder, 0, WaferTableDevicePara.DelayBlockCycActionAfter);
        }

        /// <summary>
        /// 放下晶圆夹持气缸
        /// </summary>
        public void ResetBlockCylinder()
        {
            WaferSubController.GetInstance().ResetCyc(WaferTableModule.BlockCylinder, WaferTableModule.BlockCylinderNLimitSensor, 0, WaferTableDevicePara.DelayBlockCycActionAfter);
        }

        /// <summary>
        /// 打开静态华夫盒真空
        /// </summary>
        public void OpenStaticWaffleVacuum()
        {
            if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.IsUseStaticWaffleVacuum)
            {
                if (WaferSubModule.GetInstance().WaferTable.StaticWaffleVacuum == null)
                {
                    AKRSXtraMessageBox.Show("请检查静态华夫盒硬件状态，或屏蔽静态华夫盒？", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                return;
            }              

            WaferSubController.GetInstance().SetCyc(WaferSubModule.GetInstance().WaferTable.StaticWaffleVacuum);
        }

        /// <summary>
        /// 关闭静态华夫盒真空
        /// </summary>
        public void CloseStaticWaffleVacuum()
        {
            if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.IsUseStaticWaffleVacuum)
            {
                if (WaferSubModule.GetInstance().WaferTable.StaticWaffleVacuum == null)
                {
                    AKRSXtraMessageBox.Show("请检查静态华夫盒硬件状态，或屏蔽静态华夫盒？", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                return;
            }

            WaferSubController.GetInstance().ResetCyc(WaferSubModule.GetInstance().WaferTable.StaticWaffleVacuum);
        }

        /// <summary>
        /// 检查并打开华夫盒真空
        /// </summary>
        public void CheckAndOpenWaffleVacuum()
        {
            if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is AdapterTablet ad)
            {
                if (ad.AdapterSetting.AdapterType == AdapterTypeEnum.WaferTable & WaferSubDevicePara.GetInstance().WaferTableDevicePara.IsUseWaferTableWaffleVacuum & WaferSubModule.GetInstance().WaferTable.WaffleVacuum != null)
                {
                    WaferSubController.GetInstance().WaferTableController.OpenWaffleVacuum();
                }
            }
        }

        /// <summary>
        /// 检查并关闭华夫盒真空
        /// </summary>
        public void CheckAndOpenStaticWaffleVacuum()
        {
            if (WaferSystemProgram.GetInstance().StaticAdapterProgram.Name != string.Empty & WaferSubModule.GetInstance().WaferTable.StaticWaffleVacuum != null)
            {
                WaferSubController.GetInstance().WaferTableController.OpenStaticWaffleVacuum();
            }
        }

        /// <summary>
        /// 打开华夫盒真空
        /// </summary>
        public void OpenWaffleVacuum()
        {
            if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.IsUseWaferTableWaffleVacuum)
            {
                if (WaferSubModule.GetInstance().WaferTable.WaffleVacuum == null)
                {
                    AKRSXtraMessageBox.Show("请检查晶圆华夫盒硬件状态，或屏蔽晶圆华夫盒？", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                return;
            }

            WaferSubController.GetInstance().SetCyc(WaferSubModule.GetInstance().WaferTable.WaffleVacuum);
        }

        /// <summary>
        /// 关闭华夫盒真空
        /// </summary>
        public void CloseWaffleVacuum()
        {
            if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.IsUseWaferTableWaffleVacuum)
            {
                if (WaferSubModule.GetInstance().WaferTable.WaffleVacuum == null)
                {
                    AKRSXtraMessageBox.Show("请检查晶圆华夫盒硬件状态，或屏蔽晶圆华夫盒？", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                return;
            }

            WaferSubController.GetInstance().ResetCyc(WaferSubModule.GetInstance().WaferTable.WaffleVacuum);
        }

        /// <summary>
        /// 打开热吹风
        /// </summary>
        public void OpenHotBlower()
        {
            WaferSubController.GetInstance().SetCyc(WaferSubModule.GetInstance().WaferTable.HotBlower);
        }

        /// <summary>
        /// 关闭热吹风
        /// </summary>
        public void CloseHotBlower()
        {
            WaferSubController.GetInstance().ResetCyc(WaferSubModule.GetInstance().WaferTable.HotBlower);
        }

        /// <summary>
        /// 热吹风动作-上料
        /// </summary>
        public void ActionHotBlowerOfLoad()
        {
            this.OpenHotBlower();
            Thread.Sleep(WaferTableDevicePara.DelayHotBlowerHold);
            this.CloseHotBlower();
        }

        /// <summary>
        /// 热吹风动作-下料
        /// </summary>
        public void ActionHotBlowerOfUnload()
        {
            this.OpenHotBlower();
            Thread.Sleep(WaferTableDevicePara.DelayHotBlowerHold);
            this.CloseHotBlower();
            Thread.Sleep(WaferTableDevicePara.DelayHotBlowerAfterClose);
        }

        /// <summary>
        /// 更换料片前的准备动作
        /// </summary>
        public void PreChangeTablet()
        {
            // 先归还顶针
            WaferSubController.GetInstance().EjectController.ReturnEjection();

            // 晶圆台到更换位
            this.asyncMoveWaferTableToAutoChangePosition = this.MoveWaferTableToAutoChangePosition;
            this.asyncMoveWaferTableToAutoChangePositionResult = this.asyncMoveWaferTableToAutoChangePosition.BeginInvoke(null, "succeed");

            this.asyncMoveExpandToDownPosition = () =>
                {
                    // 扩晶到低位
                    this.MoveExpandToDownPosition();
                    if (WaferTableDevicePara.CurrentTablet is WaferTablet)
                    {
                        // 晶圆夹持气缸落下
                        this.ResetBlockCylinder();
                    }
                };

            this.asyncMoveExpandToDownPositionResult = this.asyncMoveExpandToDownPosition.BeginInvoke(null, "succeed");
        }

        /// <summary>
        /// 归还料片（未涉及数据存储）
        /// </summary>
        public void ActionWaferClampPlaceWaferInSlot()
        {
            if (MachineHardwareConfiguration.GetInstance().IsMagazineConfigured == false)
            {
                return;
            }

            // 料夹松开
            this.ResetWaferClampCylinder();

            AKRSPoint3D temp = this.WaferTableDevicePara.WaferClampAtWaferTablePosition.Offset(0, 10, 0);
            this.MoveWaferClampAxisYToG0Pos(temp, 100);

            // 料夹去晶圆台取料
            this.MoveWaferClampToWaferTable();

            if (MachineStateModel.GetInstance().IsNormalWork)
            {
            Retry:
                if (!WaferTableModule.WaferClampCheckSensor.CheckStateForNums(true, 3, 80))
                {
                    //this.res = AKRSMessageBoxExt.Show("Wafer clamp does not feel the tablet, confirm the tablet on the wafer table? \r\n" + "Retry：Retry check\r\n" + "Cancel：Over\r\n", "Prompt", new string[] { "Retry", "Cancel" }, new DialogResult[] { DialogResult.Retry, DialogResult.Cancel });
                    this.res = AKRSMessageBoxExt.Show("晶圆夹没有感应到料片, 请人工确认晶圆台中的料片！ \r\n" + "Retry：重新检测\r\n" + "Cancel：结束\r\n", "Prompt", new string[] { "Retry", "Cancel" }, new DialogResult[] { DialogResult.Retry, DialogResult.Cancel });
                    switch (this.res)
                    {
                        case DialogResult.Retry:
                            goto Retry;
                        case DialogResult.Cancel:
                            //throw new Exception("Wafer clamp does not feel the tablet, please check manually!");
                            throw new Exception("晶圆夹没有感应到料片, 请人工确认晶圆台中的料片！");
                    }
                }
            }

            // 料夹夹紧
            this.SetWaferClampCylinder();

            // 新动作需要
            {
                // 料夹到预备还料位
                this.MoveWaferClampAxisYToG0Pos(WaferTableDevicePara.WaferClampAtMagazinePosition + 10);

                // 料夹松开
                this.ResetWaferClampCylinder();

                // 料夹抽回一点
                this.MoveWaferClampAxisYToG0Pos(WaferTableDevicePara.WaferClampAtMagazinePosition + 30);

                Retry:
                // 检查是否带料
                if (WaferTableModule.WaferClampCheckSensor.CheckStateForNums(true, 3, 80))
                {
                    //this.res = AKRSMessageBoxExt.Show("Wafer clamp feel the tablet, confirm the sheet in the wafer clamp slot? \r\n" + "Retry：Retry check\r\n" + "Cancel：Over\r\n", "Prompt", new string[] { "Retry", "Cancel" }, new DialogResult[] { DialogResult.Retry, DialogResult.Cancel });
                    this.res = AKRSMessageBoxExt.Show("晶圆夹感应到料片, 请人工确认晶圆夹中没有料片！ \r\n" + "Retry：重新检测\r\n" + "Cancel：结束\r\n", "Prompt", new string[] { "Retry", "Cancel" }, new DialogResult[] { DialogResult.Retry, DialogResult.Cancel });
                    switch (this.res)
                    {
                        case DialogResult.Retry:
                            goto Retry;
                        case DialogResult.Cancel:
                            //throw new Exception("Wafer clamp feel the tablet, please check manually!");
                            throw new Exception("晶圆夹感应到料片, 请人工确认晶圆夹中没有料片！");
                    }
                }

                // 料夹夹紧
                this.SetWaferClampCylinder();
            }

            // 料夹到夹料位
            this.MoveWaferClampToMagazine();            

            // 料夹去等待取料位
            this.MoveWaferClampToWaitPickPosition();

            // 料夹松开
            this.ResetWaferClampCylinder();
        }

        /// <summary>
        /// 提取料片（未涉及数据存储）
        /// </summary>
        public void ActionWaferClampRemoveWaferFromSlot()
        {
            if (MachineHardwareConfiguration.GetInstance().IsMagazineConfigured == false)
            {
                return;
            }

            // 料夹去等待取料位
            this.MoveWaferClampToWaitPickPosition();

            // 料夹松开
            this.ResetWaferClampCylinder();

            // 料夹到夹料位
            {
                if (WaferTableDevicePara.IsNeedSlowTravel)
                {
                    AKRSPoint3D g0Pos = WaferTableDevicePara.WaferClampAtMagazinePosition.Offset(0, WaferTableDevicePara.SlowDistance, 0);
                    this.MoveWaferClampAxisYToG0Pos(g0Pos);

                    // 晶圆夹y按照慢速走余下的距离，中途遇见信号就停下
                    this.MoveWaferClampAxisYToG0PosWithCommand(WaferTableDevicePara.WaferClampAtMagazinePosition.Offset(0, -WaferTableDevicePara.WaferClampSlotDepth, 0), this.WaferTableDevicePara.SlowFeedrate);
                Retry:
                    if (!this.WaferTableModule.WaferClampCheckSensor.WaitSignal(true, this.WaferTableDevicePara.DelayClampCheckSensor, ref this.isCancelWait))
                    {
                        this.WaferTableModule.WaferClampAxisY.StopMove(false);
                        //Thread.Sleep(100);

                        //this.res = AKRSMessageBoxExt.Show("Wafer clamp does not feel the tablet, confirm the tablet in the magazine slot? \r\n" + "Retry：Retry check\r\n" + "Cancel：Over\r\n", "Prompt", new string[] { "Retry", "Cancel" }, new DialogResult[] { DialogResult.Retry, DialogResult.Cancel });
                        this.res = AKRSMessageBoxExt.Show("晶圆夹没有感应到料片, 请人工确认magazine槽内是否有料片！ \r\n" + "Retry：重新检测\r\n" + "Cancel：结束\r\n", "Prompt", new string[] { "Retry", "Cancel" }, new DialogResult[] { DialogResult.Retry, DialogResult.Cancel });
                        switch (this.res)
                        {
                            case DialogResult.Retry:
                                goto Retry;
                            case DialogResult.Cancel:
                                //throw new Exception("Wafer clamp does not feel the tablet, please check manually!");
                                throw new Exception("晶圆夹没有感应到料片, 请人工确认magazine槽内是否有料片！");
                        }
                    }

                    this.WaferTableModule.WaferClampAxisY.StopMove(false);
                    //Thread.Sleep(100);
                }
                else
                {                
                    this.MoveWaferClampAxisYToG0PosWithCommand(WaferTableDevicePara.WaferClampAtMagazinePosition.Offset(0, -WaferTableDevicePara.WaferClampSlotDepth, 0), 100);

                    if (MachineStateModel.GetInstance().IsNormalWork)
                    {
                    Retry:
                        if (!this.WaferTableModule.WaferClampCheckSensor.WaitSignal(true, this.WaferTableDevicePara.DelayClampCheckSensor, ref this.isCancelWait))
                        {                           
                            this.WaferTableModule.WaferClampAxisY.StopMove(false);
                            //Thread.Sleep(100);

                            //this.res = AKRSMessageBoxExt.Show("Wafer clamp does not feel the tablet, confirm the tablet in the magazine slot? \r\n" + "Retry：Retry check\r\n" + "Cancel：Over\r\n", "Prompt", new string[] { "Retry", "Cancel" }, new DialogResult[] { DialogResult.Retry, DialogResult.Cancel });
                            this.res = AKRSMessageBoxExt.Show("晶圆夹没有感应到料片, 请人工确认magazine槽内是否有料片！ \r\n" + "Retry：Retry check\r\n" + "Cancel：Over\r\n", "Prompt", new string[] { "Retry", "Cancel" }, new DialogResult[] { DialogResult.Retry, DialogResult.Cancel });
                            switch (this.res)
                            {
                                case DialogResult.Retry:
                                    goto Retry;
                                case DialogResult.Cancel:
                                    //throw new Exception("Wafer clamp does not feel the tablet, please check manually!");
                                    throw new Exception("晶圆夹没有感应到料片, 请人工确认magazine槽内是否有料片！");
                            }
                        }

                        this.WaferTableModule.WaferClampAxisY.StopMove(false);
                        //Thread.Sleep(100);
                    }
                }

                if (this.WaferTableDevicePara.IsNeedRegrip)
                {
                    // 夹住
                    this.SetWaferClampCylinder();

                    // 往出抽
                    AKRSPoint3D g0Pos = this.WaferClampAxisYG0Pos.Offset(0, this.WaferTableDevicePara.FirstPullDistance, 0);
                    this.MoveWaferClampAxisYToG0Pos(g0Pos, this.WaferTableDevicePara.RegripFeedrate);

                    // 夹子松开
                    this.ResetWaferClampCylinder();

                    // 往进送
                    g0Pos = this.WaferClampAxisYG0Pos.Offset(0, -this.WaferTableDevicePara.RegripDistance, 0);
                    this.MoveWaferClampAxisYToG0Pos(g0Pos, this.WaferTableDevicePara.RegripFeedrate);
                }
                else
                {
                    this.MoveWaferClampAxisYToG0Pos(
                        this.WaferClampAxisYG0Pos.Offset(
                            0,
                            WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.Offset,
                            0));
                }

                // 夹住
                this.SetWaferClampCylinder();

                if (this.WaferTableDevicePara.IsNeedSlowTravel)
                {
                    AKRSPoint3D g0Pos = this.WaferClampAxisYG0Pos.Offset(0, this.WaferTableDevicePara.SlowDistance, 0);
                    this.MoveWaferClampAxisYToG0Pos(g0Pos, this.WaferTableDevicePara.SlowFeedrate);
                }
            }

            // 夹子到晶圆台
            if (MachineStateModel.GetInstance().IsNormalWork)
            {
                AKRSPoint3D temp = this.WaferTableDevicePara.WaferClampAtWaferTablePosition.Offset(0, -10, 0);                
                this.MoveWaferClampAxisYToG0Pos(temp, 100);

                temp = this.WaferTableDevicePara.WaferClampAtWaferTablePosition.Offset(0, this.WaferTableDevicePara.WaferClampOffset, 0);
                this.MoveWaferClampAxisYToG0PosWithCommand(temp, 30);

                Retry:
                if (!this.WaferTableModule.WaferSensor.WaitSignal(true, this.WaferTableDevicePara.DelayWaferTableSensor, ref this.isCancelWait))
                {
                    this.WaferTableModule.WaferClampAxisY.StopMove(false);
                    //Thread.Sleep(100);

                    //this.res = AKRSMessageBoxExt.Show("Wafer table does not feel the tablet, confirm the tablet on the wafer table? \r\n" + "Retry：Retry check\r\n" + "Cancel：Over\r\n", "Prompt", new string[] { "Retry", "Cancel" }, new DialogResult[] { DialogResult.Retry, DialogResult.Cancel });
                    this.res = AKRSMessageBoxExt.Show("晶圆台没有感应到料片, 请人工确认晶圆台内有料！ \r\n" + "Retry：重新检测\r\n" + "Cancel：结束\r\n", "Prompt", new string[] { "Retry", "Cancel" }, new DialogResult[] { DialogResult.Retry, DialogResult.Cancel });
                    switch (this.res)
                    {
                        case DialogResult.Retry:
                            goto Retry;
                        case DialogResult.Cancel:
                            //throw new Exception("Wafer table does not feel the tablet, please check manually!");
                            throw new Exception("晶圆台没有感应到料片, 请人工确认晶圆台内有料！");
                    }
                }

                this.WaferTableModule.WaferClampAxisY.StopMove(false);
                //Thread.Sleep(100);
            }
            else
            {
                this.MoveWaferClampToWaferTable();
            }

            // magazine去安全的位置
            this.AsyncMoveMagazineToSafePosition();

            // 夹子松开
            this.ResetWaferClampCylinder();

            // 料夹去安全位
            this.MoveWaferClampToSafePosition();
        }

        /// <summary>
        /// 异步移动magazine去安全位置
        /// </summary>
        private void AsyncMoveMagazineToSafePosition()
        {
            this.asyncMoveMagazineToSafePosition = WaferSubController.GetInstance().MagazineController.MoveMagazineToSafePosition;
            this.asyncMoveMagazineToSafePositionResult = this.asyncMoveMagazineToSafePosition.BeginInvoke(null, "succeed");
        }

        /// <summary>
        /// 判断晶圆台上的料片
        /// </summary>
        /// <param name="isHave">判断是否有</param>
        public void JudgeTabletOnWaferTable(bool isHave)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            if (isHave)
            {
                Retry:
                if (!this.WaferTableModule.WaferSensor.CheckStateForNums(true, 3, 200))
                {
                    //this.res = AKRSMessageBoxExt.Show("Wafer table does not feel the tablet, please check manually? \r\n" + "Retry：Retry check\r\n" + "Cancel：Over\r\n", "Prompt", new string[] { "Retry", "Cancel" }, new DialogResult[] { DialogResult.Retry, DialogResult.Cancel });
                    this.res = AKRSMessageBoxExt.Show("晶圆台没有感应到料片, 请人工确保晶圆台内有料！ \r\n" + "Retry：重新检测\r\n" + "Cancel：结束\r\n", "Prompt", new string[] { "Retry", "Cancel" }, new DialogResult[] { DialogResult.Retry, DialogResult.Cancel });
                    switch (this.res)
                    {
                        case DialogResult.Retry:
                            goto Retry;
                        case DialogResult.Cancel:
                            //throw new Exception("Wafer table does not feel the tablet, please check manually!");
                            throw new Exception("晶圆台没有感应到料片, 请人工确保晶圆台内有料！");
                    }
                }
            }
            else
            {
                Retry:
                if (this.WaferTableModule.WaferSensor.CheckStateForNums(true, 3, 200))
                {
                    WaferSubController.GetInstance().EjectController.ReturnEjection();
                    WaferSubController.GetInstance().WaferTableController.MoveExpandToDownPosition();
                    //WaferSubController.GetInstance().WaferTableController.ResetBlockCylinder();

                    if (WaferTableDevicePara.IsUseWaferTableWaffleVacuum)
                    {
                        //this.res = AKRSMessageBoxExt.Show($"是否降下晶圆夹持气缸？\r\n" + "降下请点击 OK.\r\n" + "不降请点击 CANCEL.", "Prompt", new string[] { "OK", "Cancel" }, new DialogResult[] { DialogResult.OK, DialogResult.Cancel });
                        //switch (this.res)
                        //{
                        //    case DialogResult.OK:
                        //        WaferSubController.GetInstance().WaferTableController.ResetBlockCylinder();
                        //        break;
                        //    case DialogResult.Cancel:
                        //        break;
                        //}
                    }
                    else
                    {
                        // 放下晶圆夹持气缸
                        WaferSubController.GetInstance().WaferTableController.ResetBlockCylinder();
                        //this.res = AKRSMessageBoxExt.Show("Wafer table feel the tablet, please remove manually? \r\n" + "Retry：Retry check\r\n" + "Cancel：Over\r\n", "Prompt", new string[] { "Retry", "Cancel" }, new DialogResult[] { DialogResult.Retry, DialogResult.Cancel });
                        this.res = AKRSMessageBoxExt.Show("晶圆台感应到料片, 请人工确保晶圆台内无料！ \r\n" + "Retry：重新检测\r\n" + "Cancel：结束\r\n", "Prompt", new string[] { "Retry", "Cancel" }, new DialogResult[] { DialogResult.Retry, DialogResult.Cancel });
                        switch (this.res)
                        {
                            case DialogResult.Retry:
                                goto Retry;
                            case DialogResult.Cancel:
                                //throw new Exception("Wafer table feel the tablet, please remove manually!");
                                throw new Exception("晶圆台感应到料片, 请人工确保晶圆台内无料！");
                        }
                    }             
                }
            }
        }

        /// <summary>
        /// 判断晶圆台上是否有料
        /// </summary>
        /// <returns>true:有 false:无</returns>
        public bool JudgeTabletOnWaferTable()
        {
            return this.WaferTableModule.WaferSensor.CheckStateForNums(true, 3, 200);
        }

        /// <summary>
        /// 归还料片（涉及数据存储）
        /// </summary>
        public void PlaceWaferInSlot()
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                if (MagazineBoxDevicePara.CurrentLayerNo > 0)
                {
                    WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[MagazineBoxDevicePara.CurrentLayerNo - 1] = WaferTableDevicePara.CurrentTablet;
                    MagazineAllocationsConfigRepository.GetInstance().Save();
                    WaferTableDevicePara.CurrentTablet = new NullTablet();
                    MagazineBoxDevicePara.CurrentLayerNo = 0;
                    WaferSubDevicePara.GetInstance().Save();
                }

                return;
            }

            int currentLayerNo = MagazineBoxDevicePara.CurrentLayerNo;
            if (currentLayerNo > 0)
            {
                if (WaferTableDevicePara.CurrentTablet is WaferTablet && WaferTableDevicePara.IsUseHotBlower)
                {
                    // 先归还顶针
                    WaferSubController.GetInstance().EjectController.ReturnEjection();

                    // 扩晶到低位
                    this.MoveExpandToDownPosition();

                    // 晶圆夹持气缸落下
                    this.ResetBlockCylinder();

                    // 热吹
                    this.ActionHotBlowerOfUnload();

                    // 自动换料位
                    this.MoveWaferTableToAutoChangePosition();

                    // magazine到位后
                    WaferSubController.GetInstance().MagazineController.SlotScan(currentLayerNo - 1);
                }
                else
                {
                    this.PreChangeTablet();

                    this.asyncMoveWaferTableToAutoChangePosition.EndInvoke(this.asyncMoveWaferTableToAutoChangePositionResult);
                    if (WaferTableDevicePara.CurrentTablet is AdapterTablet)
                    {
                        // 晶圆夹持气缸落下
                        this.ResetBlockCylinder();
                    }

                    // magazine到位后
                    WaferSubController.GetInstance().MagazineController.SlotScan(currentLayerNo - 1);

                    this.asyncMoveExpandToDownPosition.EndInvoke(this.asyncMoveExpandToDownPositionResult);
                }

                // 防止扩晶电机未实际到位，导致晶圆夹Y检测安全状态时报错
                //Thread.Sleep(100);

                this.isManualChangeTablet = this.IsManualChangeTabletMode(WaferTableDevicePara.CurrentTablet);
                if (this.isManualChangeTablet)
                {
                    //this.res = XtraMessageBox.Show("Please return the sheet to magazine slot form wafer table with manually!\r\n" + "Click OK: continue,\r\n" + "Click Cancel: stop", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    this.res = AKRSXtraMessageBox.Show("请手动将料片从晶圆台塞回magazine槽中！\r\n" + "点击 OK: 继续,\r\n" + "点击 Cancel: 停止", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    switch (this.res)
                    {
                        case DialogResult.OK:
                            this.JudgeTabletOnWaferTable(false);
                            break;
                        case DialogResult.Cancel:
                            // 终止
                            //throw new Exception($"Return the sheet to magazine slot failed!");
                            throw new Exception($"归还料片失败！");
                    }
                }
                else
                {
                    AKRSPoint3D point = WaferSubController.GetInstance().MagazineController.MagazineAxisZG0Pos.Offset(0, 0, -this.MagazineBoxDevicePara.MagazineLiftDistanceWithClamp);
                    WaferSubController.GetInstance().MagazineController.MoveMagazineAxisZToG0Pos(point);
                    this.ActionWaferClampPlaceWaferInSlot();
                }

                WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[currentLayerNo - 1] = this.WaferTableDevicePara.CurrentTablet;
                MagazineAllocationsConfigRepository.GetInstance().Save();
                this.WaferTableDevicePara.CurrentTablet = new NullTablet();
                this.MagazineBoxDevicePara.CurrentLayerNo = 0;
                WaferSubDevicePara.GetInstance().Save();
            }
        }

        /// <summary>
        /// 提取料片（涉及数据存储）
        /// </summary>
        /// <param name="targetIndex">目标层</param>
        /// <param name="isOnlyAction">是否只进行单纯的抽拉动作</param>
        public void RemoveWaferFromSlot(int targetIndex, bool isOnlyAction = false)
        {
            if (MachineHardwareConfiguration.GetInstance().IsMagazineConfigured == false)
            {
                return;
            }

            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                this.MagazineBoxDevicePara.CurrentLayerNo = targetIndex + 1;
                WaferSubDevicePara.GetInstance().Save();

                // 给晶圆台料片赋值
                MagazineAllocationsConfig magazineAllocationsConfig = JsonFormatHelper<MagazineAllocationsConfig>.DeepGenericCopy<MagazineAllocationsConfig>(WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig);
                this.WaferTableDevicePara.CurrentTablet = magazineAllocationsConfig.TabletArray[targetIndex];
                WaferSubDevicePara.GetInstance().Save();
                return;
            }

            if (this.MagazineBoxDevicePara.CurrentLayerNo == targetIndex + 1)
            {
                return;
            }
            else
            {
                PlaceWaferInSlot();
            }

            this.PreChangeTablet();

            this.asyncMoveWaferTableToAutoChangePosition.EndInvoke(this.asyncMoveWaferTableToAutoChangePositionResult);

            this.asyncMoveExpandToDownPosition.EndInvoke(this.asyncMoveExpandToDownPositionResult);

            this.isManualChangeTablet = this.IsManualChangeTabletMode(WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[targetIndex]);
            if (this.isManualChangeTablet)
            {
                // 料夹去安全位
                this.MoveWaferClampToSafePosition();
            }
            else
            {
                // 料夹去等待位
                this.MoveWaferClampToWaitPickPosition();
            }

            // magazine到换料层
            WaferSubController.GetInstance().MagazineController.SlotScan(targetIndex);
                       
            if (this.isManualChangeTablet)
            {
                // 料夹去安全位
                this.MoveWaferClampToSafePosition();

                //this.res = XtraMessageBox.Show("Please remove the sheet from magazine slot to wafer table with manually!\r\n" + "Click OK: continue,\r\n" + "Click Cancel: stop", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                this.res = AKRSXtraMessageBox.Show("请人工将料片从magazine槽移入晶圆台中！\r\n" + "点击 OK: 继续,\r\n" + "点击 Cancel: 停止", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                switch (this.res)
                {
                    case DialogResult.OK:
                        this.JudgeTabletOnWaferTable(true);
                        break;
                    case DialogResult.Cancel:
                        // 终止
                        //throw new Exception($"Remove the sheet from magazine slot failed!");
                        throw new Exception($"从magazine槽移除料片失败！");
                }

                this.AsyncMoveMagazineToSafePosition();
            }
            else
            {
                if (WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.Push)
                {
                    WaferSubController.GetInstance().MagazineController.ActionPushCyc();
                }

                this.ActionWaferClampRemoveWaferFromSlot();
            }

            // 晶圆夹持气缸抬起
            this.SetBlockCylinder();

            this.MagazineBoxDevicePara.CurrentLayerNo = targetIndex + 1;
            WaferSubDevicePara.GetInstance().Save();

            // 给晶圆台料片赋值
            MagazineAllocationsConfig temp = JsonFormatHelper<MagazineAllocationsConfig>.DeepGenericCopy<MagazineAllocationsConfig>(WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig);
            this.WaferTableDevicePara.CurrentTablet = temp.TabletArray[targetIndex];
            WaferSubDevicePara.GetInstance().Save();

            if (WaferTableDevicePara.CurrentTablet is WaferTablet wt)
            {
                // 热吹风
                if (WaferTableDevicePara.IsUseHotBlower)
                {
                    this.asyncMoveMagazineToSafePosition.EndInvoke(this.asyncMoveMagazineToSafePositionResult);

                    // 晶圆台到准备位
                    this.MoveWaferTableToReadyPosition();

                    // 热吹
                    this.ActionHotBlowerOfLoad();

                    // 扩晶
                    this.MoveExpandToUpPosition(wt.CarrierConfigWithWafer);
                }
                else
                {
                    // 扩晶到扩晶位
                    //WaferTablet wt = (WaferTablet)WaferTableDevicePara.CurrentTablet;

                    Action<CarrierWithWaferConfig> action = this.MoveExpandToUpPosition;
                    IAsyncResult result = action.BeginInvoke(wt.CarrierConfigWithWafer, null, "succeed");

                    this.asyncMoveMagazineToSafePosition.EndInvoke(this.asyncMoveMagazineToSafePositionResult);

                    //Thread.Sleep(100);

                    // 晶圆台到准备位
                    this.MoveWaferTableToReadyPosition();

                    action.EndInvoke(result);
                }
            }
            else
            {
                this.asyncMoveMagazineToSafePosition.EndInvoke(this.asyncMoveMagazineToSafePositionResult);

                //Thread.Sleep(100);

                // 晶圆台到准备位
                this.MoveWaferTableToReadyPosition();
            }
        }

        /// <summary>
        /// 晶圆相机自动聚焦
        /// </summary>
        public void Autofocus()
        {
            AutoFocusing autofocus = new AutoFocusing();
            double temp = CalibrateRunPara.GetInstance().WaferTableLeftMarkWCVisionMachinePos.Z;
            autofocus.AutoFocus(WaferSubModule.GetInstance().WaferTable.WaferCamera, WaferTableModule.WaferCameraAxisZ, temp + 5, temp - 5);
        }

        /// <summary>
        /// 清除晶圆模块记忆
        /// </summary>
        public void ClearWaferSubMemory(bool isClearMagazineState = true, bool isClearStaticAdapterState = true)
        {
            // 设备记忆清除
            WaferSubDevicePara.GetInstance().EjectDevicePara.CurrentSlotConfig = null;
            WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet = new NullTablet();
            //WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentStaticAdapterTablet = new AdapterTablet();
            WaferSubDevicePara.GetInstance().MagazineDevicePara.CurrentLayerNo = 0;

            if (isClearMagazineState)
            {
                WaferSubDevicePara.GetInstance().MagazineDevicePara.SetNeedReCreateStateSignal();
            }

            if (isClearStaticAdapterState)
            {
                WaferSubDevicePara.GetInstance().WaferTableDevicePara.SetNeedReCreateStateWithStaticAdapterTabletSignal();
            }
        }

        /// <summary>
        /// 获取晶圆夹气缸状态
        /// </summary>
        /// <returns>return</returns>
        public bool GetWaferClampCylinder()
        {
            return this.WaferTableModule.WaferClampCylinder.CurrentOutputValue;
        }

        /// <summary>
        /// 获取晶圆夹持气缸状态
        /// </summary>
        /// <returns>return</returns>
        public bool GetBlockCylinder()
        {
            return this.WaferTableModule.BlockCylinder.CurrentOutputValue;
        }

        /// <summary>
        /// 获取红色点光硬件名称
        /// </summary>
        /// <returns>result</returns>
        public string GetRedSpotLightHardwareName()
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return null;
            }

            if (this.WaferTableModule.RedSpotLight == null)
            {
                return null;
            }

            return this.WaferTableModule.RedSpotLight.HardwareName;
        }

        /// <summary>
        /// 获取绿色点光硬件名称
        /// </summary>
        /// <returns>result</returns>
        public string GetGreenSpotLightHardwareName()
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return null;
            }

            if (this.WaferTableModule.GreenSpotLight == null)
            {
                return null;
            }

            return this.WaferTableModule.GreenSpotLight.HardwareName;
        }

        /// <summary>
        /// 获取蓝色点光硬件名称
        /// </summary>
        /// <returns>result</returns>
        public string GetBlueSpotLightHardwareName()
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return null;
            }

            if (this.WaferTableModule.BlueSpotLight == null)
            {
                return null;
            }

            return this.WaferTableModule.BlueSpotLight.HardwareName;
        }

        /// <summary>
        /// 获取环光硬件名称
        /// </summary>
        /// <returns>result</returns>
        public string GetRingLightHardwareName()
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return null;
            }

            return this.WaferTableModule.RingLight.HardwareName;
        }

        /// <summary>
        /// 移动晶圆台XY轴到指定位置
        /// </summary>
        /// <param name="g0Pos">g0Pos</param>
        /// <param name="isSkipCheckEject">是否检查顶针在一个安全的状态</param>
        public void MoveWaferTableToG0Pos(AKRSPoint3D g0Pos, bool isSkipCheckEject = false)
        {
            WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();

            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            WaferSystemDomain.GetInstance().SingleStep();

            this.CheckWaferClampAxisYAtSafePosition();
            WaferSubController.GetInstance().MagazineController.CheckMagazineAxisZAtSafePosition();
            if (!isSkipCheckEject)
            {
                WaferSubController.GetInstance().EjectController.CheckEjectionTableAxisZAtSafePosition();
                WaferSubController.GetInstance().EjectController.CheckEjectionAxisZAtSafePosition();
            }

            #region 旧
            //g0Pos = MachineCoordinateSystem.GetInstance().WaferCoordinateSystem.G0PosToSelf(g0Pos);

            //double speedPercent = WaferSubDevicePara.GetInstance().WaferTableDevicePara.WaferTableSpeedPercent;
            //double speedX = this.WaferTableModule.WaferTableAxisX.AxisMovePara.AbsoluteMoveSpeed * speedPercent / 100;
            //double speedY = this.WaferTableModule.WaferTableAxisY.AxisMovePara.AbsoluteMoveSpeed * speedPercent / 100;
            //bool signalXArrived = false, signalYArrived = false, error = false;
            //Task.Run(
            //    () =>
            //        {
            //            ExcuteResult ret = this.WaferTableModule.WaferTableAxisX.AbsoluteMove(g0Pos.X, true, speedX, AccuracyMode.HighAccuracy);
            //            if (ret == ExcuteResult.Success)
            //            {
            //                signalXArrived = true;
            //            }
            //            else
            //            {
            //                error = true;
            //            }
            //        });

            //Task.Run(
            //    () =>
            //        {
            //            ExcuteResult ret = this.WaferTableModule.WaferTableAxisY.AbsoluteMove(g0Pos.Y, true, speedY, AccuracyMode.HighAccuracy);
            //            if (ret == ExcuteResult.Success)
            //            {
            //                signalYArrived = true;
            //            }
            //            else
            //            {
            //                error = true;
            //            }
            //        });

            //while (!signalXArrived || !signalYArrived)
            //{
            //    if (error)
            //    {
            //        throw new Exception("晶圆台XY移动到目标位置失败！");
            //    }

            //    continue;
            //}


            #endregion

            #region 新

            // 转到轴坐标
            AKRSPoint3D pos = MachineCoordinateSystem.GetInstance().WaferCoordinateSystem.G0PosToSelf(g0Pos);

            // 开始运动
            this.WaferTableModule.WaferTableAxisX.SendAbsoluteMoveCommand(pos.X);
            this.WaferTableModule.WaferTableAxisY.SendAbsoluteMoveCommand(pos.Y);

            // 等轴到位
            MotionService.WaitAxesArrival(
                (this.WaferTableModule.WaferTableAxisX, true, pos.X, AccuracyMode.HighAccuracy),
                (this.WaferTableModule.WaferTableAxisY, true, pos.Y, AccuracyMode.HighAccuracy));

            #endregion

            //if (MachineStateModel.GetInstance().IsOffLineWork)
            //{
            //    return;
            //}

            //WaferSystemDomain.GetInstance().SingleStep();

            //this.CheckWaferClampAxisYAtSafePosition();
            //WaferSubController.GetInstance().MagazineController.CheckMagazineAxisZAtSafePosition();
            //if (!isSkipCheckEject)
            //{
            //    WaferSubController.GetInstance().EjectController.CheckEjectionTableAxisZAtSafePosition();
            //    WaferSubController.GetInstance().EjectController.CheckEjectionAxisZAtSafePosition();
            //}

            //g0Pos = MachineCoordinateSystem.GetInstance().WaferCoordinateSystem.G0PosToSelf(g0Pos);
            //MotionService.MoveAxesToTargetPosition((this.WaferTableModule.WaferTableAxisX, true, g0Pos.X, AccuracyMode.HighAccuracy), (this.WaferTableModule.WaferTableAxisY, true, g0Pos.Y, AccuracyMode.HighAccuracy));
        }

        /// <summary>
        /// 移动晶圆台XY轴到指定位置
        /// </summary>
        /// <param name="g0Pos">g0Pos</param>
        public void MoveWaferTableToG0PosWithSearch(AKRSPoint3D g0Pos, bool isCheckCollision = true)
        {
            WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();

            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            WaferSystemDomain.GetInstance().SingleStep();

            //this.CheckWaferClampAxisYAtSafePosition();
            //WaferSubController.GetInstance().MagazineController.CheckMagazineAxisZAtSafePosition();

            // 新版-判断是否在扩晶环限位内
            if (isCheckCollision)
            {
                if (!this.IsInCollisionCircle(g0Pos))
                {
                    //throw new Exception($"WaferTable Location out of CollisionCircle!");
                    throw new Exception($"晶圆台目标位置超出环限位！");
                }
            }

            if (Block.GetInstance().GetSearchMode() == SearchMode.WaferMap || Block.GetInstance().GetSearchMode() == SearchMode.Single || Block.GetInstance().GetSearchMode() == SearchMode.Nine)
            {
                WaferSubController.GetInstance().EjectController.CheckEjectionAxisZAtReadyLiftPosition();
            }

            #region 旧

            //g0Pos = MachineCoordinateSystem.GetInstance().WaferCoordinateSystem.G0PosToSelf(g0Pos);
            //double speedX = this.WaferTableModule.WaferTableAxisX.AxisMovePara.AbsoluteMoveSpeed /** MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage*/;
            //double speedY = this.WaferTableModule.WaferTableAxisY.AxisMovePara.AbsoluteMoveSpeed /** MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage*/;
            //bool signalXArrived = false, signalYArrived = false, error = false;
            //Task.Run(
            //    () =>
            //        {
            //            ExcuteResult ret = this.WaferTableModule.WaferTableAxisX.AbsoluteMove(g0Pos.X, true, speedX, AccuracyMode.HighAccuracy);
            //            if (ret == ExcuteResult.Success)
            //            {
            //                signalXArrived = true;
            //            }
            //            else
            //            {
            //                error = true;
            //            }
            //        });

            //Task.Run(
            //    () =>
            //        {
            //            ExcuteResult ret = this.WaferTableModule.WaferTableAxisY.AbsoluteMove(g0Pos.Y, true, speedY, AccuracyMode.HighAccuracy);
            //            if (ret == ExcuteResult.Success)
            //            {
            //                signalYArrived = true;
            //            }
            //            else
            //            {
            //                error = true;
            //            }
            //        });

            //while (!signalXArrived || !signalYArrived)
            //{
            //    if (error)
            //    {
            //        throw new Exception("晶圆台XY移动到目标位置失败！");
            //    }

            //    continue;
            //}

            #endregion

            #region 新

            // 转到轴坐标
            AKRSPoint3D pos = MachineCoordinateSystem.GetInstance().WaferCoordinateSystem.G0PosToSelf(g0Pos);

            // 开始运动
            this.WaferTableModule.WaferTableAxisX.SendAbsoluteMoveCommand(pos.X);
            this.WaferTableModule.WaferTableAxisY.SendAbsoluteMoveCommand(pos.Y);

            // 等轴到位
            MotionService.WaitAxesArrival(
                (this.WaferTableModule.WaferTableAxisX, true, pos.X, AccuracyMode.HighAccuracy),
                (this.WaferTableModule.WaferTableAxisY, true, pos.Y, AccuracyMode.HighAccuracy));

            #endregion
        }

        /// <summary>
        /// 移动扩晶电机到指定位置
        /// </summary>
        /// <param name="g0Pos">g0Pos</param>
        /// <param name="vel">速度</param>
        public void MoveExpandAxisZToG0Pos(AKRSPoint3D g0Pos, double vel)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            WaferSystemDomain.GetInstance().SingleStep();

            this.CheckWaferClampAxisYAtSafePosition();
            double z = MachineCoordinateSystem.GetInstance().NullCoordinateSystem.G0PosToSelf(g0Pos).Z;
            ExcuteResult ret = this.WaferTableModule.ExpandAxisZ.AbsoluteMove(z, true, vel, AccuracyMode.HighAccuracy);
            if (ret != ExcuteResult.Success)
            {
                //throw new Exception("ExpandAxisZ positioning failure.");
                throw new Exception("扩晶移动到目标位置失败！");
            }
        }

        /// <summary>
        /// 移动扩晶电机到指定位置
        /// </summary>
        /// <param name="g0Pos">g0Pos</param>
        public void MoveExpandAxisZToG0Pos(AKRSPoint3D g0Pos)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            WaferSystemDomain.GetInstance().SingleStep();

            MovePara movePara = new MovePara()
                                    {
                                        Vel = this.WaferTableModule.ExpandAxisZ.AxisMovePara.AbsoluteMoveSpeed,
                                        Acc = this.WaferTableModule.ExpandAxisZ.AxisMovePara.ACC,
                                        Dec = this.WaferTableModule.ExpandAxisZ.AxisMovePara.DEC,
                                        Jerk = this.WaferTableModule.ExpandAxisZ.AxisMovePara.Jerk,
                                        TargetPosition = MachineCoordinateSystem.GetInstance().NullCoordinateSystem.G0PosToSelf(g0Pos).Z
            };
            ExcuteResult ret = this.WaferTableModule.ExpandAxisZ.AbsoluteMove(movePara, false, AccuracyMode.HighAccuracy);
            if (ret != ExcuteResult.Success)
            {
                //throw new Exception("ExpandAxisZ positioning failure.");
                throw new Exception("扩晶移动到目标位置失败！");
            }
        }

        /// <summary>
        /// 移动晶圆夹电机到指定位置
        /// </summary>
        /// <param name="g0Pos">g0Pos</param>
        public void MoveWaferClampAxisYToG0Pos(AKRSPoint3D g0Pos)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            if (MachineHardwareConfiguration.GetInstance().IsMagazineConfigured == false)
            {
                return;
            }

            WaferSystemDomain.GetInstance().SingleStep();

            this.CheckExpandAxisZAtDownPosition();
            this.CheckBlockCylinderAtNLimit();
            this.CheckWaferTableAtAutoChangePosition();

            MovePara movePara = new MovePara()
                                    {
                                        Vel = this.WaferTableModule.WaferClampAxisY.AxisMovePara.AbsoluteMoveSpeed * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage,
                                        Acc = this.WaferTableModule.WaferClampAxisY.AxisMovePara.ACC,
                                        Dec = this.WaferTableModule.WaferClampAxisY.AxisMovePara.DEC,
                                        Jerk = this.WaferTableModule.WaferClampAxisY.AxisMovePara.Jerk,
                                        TargetPosition = MachineCoordinateSystem.GetInstance().NullCoordinateSystem.G0PosToSelf(g0Pos).Y
            };
            ExcuteResult ret = this.WaferTableModule.WaferClampAxisY.AbsoluteMove(movePara, false, AccuracyMode.HighAccuracy);
            if (ret != ExcuteResult.Success)
            {
                //throw new Exception("WaferClampAxisY positioning failure.");
                throw new Exception("晶圆夹移动到目标位置失败！");
            }
        }

        /// <summary>
        /// 移动晶圆夹电机到指定位置
        /// </summary>
        /// <param name="g0Pos">g0Pos</param>
        /// <param name="velPercent">百分比</param>
        public void MoveWaferClampAxisYToG0Pos(AKRSPoint3D g0Pos, double velPercent)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            if (MachineHardwareConfiguration.GetInstance().IsMagazineConfigured == false)
            {
                return;
            }

            WaferSystemDomain.GetInstance().SingleStep();

            double speed = this.WaferTableModule.WaferClampAxisY.AxisMovePara.AbsoluteMoveSpeed * velPercent / 100;
            ExcuteResult ret = this.WaferTableModule.WaferClampAxisY.AbsoluteMove(MachineCoordinateSystem.GetInstance().NullCoordinateSystem.G0PosToSelf(g0Pos).Y, false, speed, AccuracyMode.HighAccuracy);
            if (ret != ExcuteResult.Success)
            {
                //throw new Exception("WaferClampAxisY positioning failure.");
                throw new Exception("晶圆夹移动到目标位置失败！");
            }
        }

        /// <summary>
        /// 移动晶圆夹电机到指定位置
        /// </summary>
        /// <param name="g0Pos">g0Pos</param>
        /// <param name="velPercent">百分比</param>
        public void MoveWaferClampAxisYToG0PosWithCommand(AKRSPoint3D g0Pos, double velPercent)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            if (MachineHardwareConfiguration.GetInstance().IsMagazineConfigured == false)
            {
                return;
            }

            WaferSystemDomain.GetInstance().SingleStep();

            double speed = this.WaferTableModule.WaferClampAxisY.AxisMovePara.AbsoluteMoveSpeed * velPercent / 100;
            ExcuteResult ret = this.WaferTableModule.WaferClampAxisY.SendAbsoluteMoveCommand(MachineCoordinateSystem.GetInstance().NullCoordinateSystem.G0PosToSelf(g0Pos).Y, speed);
            if (ret != ExcuteResult.Success)
            {
                //throw new Exception("WaferClampAxisY positioning failure.");
                throw new Exception("晶圆夹移动到目标位置失败！");
            }
        }

        /// <summary>
        /// 移动晶圆相机Z轴到指定位置
        /// </summary>
        /// <param name="g0Pos">g0Pos</param>
        public void MoveWaferCameraAxisZToG0Pos(AKRSPoint3D g0Pos)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            WaferSystemDomain.GetInstance().SingleStep();
            MovePara movePara = new MovePara()
                                    {
                                        Vel = this.WaferTableModule.WaferCameraAxisZ.AxisMovePara.AbsoluteMoveSpeed /** MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage*/,
                                        Acc = this.WaferTableModule.WaferCameraAxisZ.AxisMovePara.ACC,
                                        Dec = this.WaferTableModule.WaferCameraAxisZ.AxisMovePara.DEC,
                                        Jerk = this.WaferTableModule.WaferCameraAxisZ.AxisMovePara.Jerk,
                                        TargetPosition = MachineCoordinateSystem.GetInstance().WaferCoordinateSystem.G0PosToSelf(g0Pos).Z
            };
            ExcuteResult ret = this.WaferTableModule.WaferCameraAxisZ.AbsoluteMove(movePara, false, AccuracyMode.HighAccuracy);
            if (ret != ExcuteResult.Success)
            {
                //throw new Exception("WaferCameraAxisZ positioning failure.");
                throw new Exception("晶圆相机移动到目标位置失败！");
            }
        }

        /// <summary>
        /// 顶针台与晶圆相机同步运动
        /// </summary>
        /// <param name="offset">移动距离</param>
        public void MoveEjectTableAndWaferCameraSync(double offset)
        {
            AKRSPoint3D tempET = new AKRSPoint3D(0, 0, WaferSubController.GetInstance().EjectController.EjectionTableAxisZG0Pos.Z + offset);
            AKRSPoint3D tempWC = new AKRSPoint3D(0, 0, WaferSubController.GetInstance().WaferTableController.WaferCameraAxisZG0Pos.Z + offset);
            WaferSubController.GetInstance().EjectController.MoveEjectionTableAxisZToG0Pos(tempET);
            WaferSubController.GetInstance().WaferTableController.MoveWaferCameraAxisZToG0Pos(tempWC);
        }

        /// <summary>
        /// 环限位范围限制
        /// </summary>
        /// <param name="g0Pos">晶圆台将要去的地方</param>
        /// <returns>是否在环限位内</returns>
        public bool IsInCollisionCircle(AKRSPoint3D g0Pos)
        {
            AKRSPoint3D center = WaferTableDevicePara.WaferTableCenter;
            double r = (long)WaferTableDevicePara.WaferTableRadius;
            double area = (g0Pos.X - center.X) * (g0Pos.X - center.X) + (g0Pos.Y - center.Y) * (g0Pos.Y - center.Y);
            if (area > r * r)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 载具是否是手动更换
        /// </summary>
        /// <param name="baseTablet">baseTablet</param>
        /// <returns>result</returns>
        public bool IsManualChangeTabletMode(BaseTablet baseTablet)
        {
            if (baseTablet is WaferTablet wt)
            {
                if (wt.CarrierConfigWithWafer.TabletChangeType == TabletChangeTypeEnum.ManualChange)
                {
                    return true;
                }
            }
            else if (baseTablet is AdapterTablet adt)
            {
                foreach (var item in adt.AdapterSetting.WaffleArray)
                {
                    if (item.Name != string.Empty)
                    {
                        if (item.CarrierWithWaffleConfig.TabletChangeType == TabletChangeTypeEnum.ManualChange)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 获取晶圆相机中心的g0位置
        /// </summary>
        /// <returns>result</returns>
        public AKRSPoint3D GetWaferCameraG0Position()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return new AKRSPoint3D();
            }

            AKRSPoint2D akrsPoint2D = CalibrateRunPara.GetInstance().BondHeadRotateCenterInWC;
            AKRSPoint3D waferCameraG0Position = new AKRSPoint3D(akrsPoint2D.X, akrsPoint2D.Y, 0);
            return System2Domain.GetInstance().BondModuleController.ConvertMachineToG0Pos(waferCameraG0Position);
        }

        /// <summary>
        /// 是否不允许晶圆相机拍照
        /// </summary>
        /// <param name="cameraType">相机类型</param>
        /// <returns>result</returns>
        public bool IsNotAllowWaferCameraAction(CameraTypeEnum cameraType)
        {
            AKRSPoint3D bondG0Position = System2Domain.GetInstance().BondModuleController.GetG0RealPosition();

            if (cameraType == CameraTypeEnum.UpLookCamera)
            {
                // 上视
                if (bondG0Position.X < this.WaferTableDevicePara.BondAvoidWaferCameraPositionWithUpLook.X || bondG0Position.Y > this.WaferTableDevicePara.BondAvoidWaferCameraPositionWithUpLook.Y)
                {
                    return false;
                }
            }
            else
            {
                // IPT
                if (bondG0Position.X < this.WaferTableDevicePara.BondAvoidWaferCameraPositionWithIPT.X || bondG0Position.Y > this.WaferTableDevicePara.BondAvoidWaferCameraPositionWithIPT.Y)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 晶圆台内是否存在料片
        /// </summary>
        /// <returns>return</returns>
        private bool IsExistTabletOnWaferTable()
        {
            if (MachineStateModel.GetInstance().IsOffLineWork || WaferTableDevicePara.IsUnCheckTablet)
            {
                return true;
            }

            if (this.WaferTableModule.WaferSensor.CheckStateForNums(true, 3, 200))
            {
                return true;
            }
            else
            {
                goto Prompt;
            }

        Prompt:
            //this.res = XtraMessageBox.Show("Confirm the tablet on the wafer table?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            this.res = AKRSXtraMessageBox.Show("请确认晶圆台上是否有料片？或者料片是否放反？", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            switch (this.res)
            {
                case DialogResult.Yes:
                    return this.IsExistTabletOnWaferTable();
                case DialogResult.No:
                    break;
            }

            return false;
        }

        /// <summary>
        /// 获取相机硬件
        /// </summary>
        /// <returns>硬件集合</returns>
        public PRHardware GetPrHardware(CarrierTypeEnum carrierType)
        {
            List<string> list = new List<string>{};
            PRHardware hardware;

            if (carrierType == CarrierTypeEnum.StaticWaffle)
            {
                BondModule bondModule = new BondModule();

                if (MachineHardwareConfiguration.GetInstance().LightConfig == LightConfigEnum.TrichromaticLight)
                {
                    list = new List<string>
                               {
                                   bondModule.SpotLightGreen?.HardwareName,
                                   bondModule.SpotLightRed?.HardwareName,
                                   bondModule.SpotLightBlue?.HardwareName,
                                   bondModule.AmbientLightGreen?.HardwareName,
                                   bondModule.AmbientLightRed?.HardwareName,
                                   bondModule.AmbientLightBlue?.HardwareName,
                               };
                }
                else
                {
                    list = new List<string>
                               {
                                   bondModule.SpotLightRed?.HardwareName,
                                   bondModule.AmbientLightRed?.HardwareName,
                               };
                }

                hardware = new PRHardware(
                bondModule.BondCamera.HardwareName,
                bondModule.BondAxisX.HardwareName,
                bondModule.BondAxisY.HardwareName,
                bondModule.BondHead.AxisZ.HardwareName,
                list);
            }
            else
            {
                if (MachineHardwareConfiguration.GetInstance().LightConfig == LightConfigEnum.TrichromaticLight)
                {
                    list = new List<string>
                               {
                                   this.WaferTableModule.GreenSpotLight?.HardwareName,
                                   this.WaferTableModule.RedSpotLight?.HardwareName,
                                   this.WaferTableModule.BlueSpotLight?.HardwareName,
                                   this.WaferTableModule.RingLight?.HardwareName,
                               };
                }
                else
                {
                    list = new List<string>
                               {
                                   this.WaferTableModule.RedSpotLight?.HardwareName,
                                   this.WaferTableModule.RingLight?.HardwareName,
                               };
                }

                hardware = new PRHardware(
                this.WaferTableModule.WaferCamera.HardwareName,
                this.WaferTableModule.WaferTableAxisX.HardwareName,
                this.WaferTableModule.WaferTableAxisY.HardwareName,
                this.WaferTableModule.ExpandAxisZ.HardwareName,
                list);
            }            

            return hardware;
        }

        /// <summary>
        /// 制作PR
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="algBeLong">algBeLong</param>
        public void EditPr(string name, CarrierTypeEnum carrierType = CarrierTypeEnum.Wafer, AlgBeLongEnum algBeLong = AlgBeLongEnum.Component)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);
            BaseVisionEntity visionEntity = null;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity(name);
                prEntity.Alg.AlgBeLong = algBeLong;
                visionEntity = prEntity;
                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
                prEntity.SetHardware(this.GetPrHardware(carrierType));
            }
            
            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);
            editor.ShowDialog();
            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 晶圆台是否在自动换料位置
        /// </summary>
        public void CheckWaferTableAtAutoChangePosition()
        {
            if (Math.Abs(this.WaferTableAxisXG0Pos.X - WaferTableDevicePara.AutoChangePosition.X) > this.range || Math.Abs(this.WaferTableAxisYG0Pos.Y - WaferTableDevicePara.AutoChangePosition.Y) > this.range)
            {
                //throw new Exception("WaferTable not at AutoChangePosition.");
                DialogResult res = AKRSMessageBoxExt.Show($"晶圆台不在自动换料位！" + "\r\n" + "点击OK，即将移动到位，" + "\r\n" + "点击Abort，即将终止！", "Prompt", new string[] { "OK", "Abort" }, new DialogResult[] { DialogResult.OK, DialogResult.Abort });
                switch (res)
                {
                    case DialogResult.OK:
                        this.MoveWaferTableToAutoChangePosition();
                        return;
                    case DialogResult.Abort:
                        throw new Exception("晶圆台不在自动换料位！");
                }
            }
        }

        /// <summary>
        /// 夹持气缸是否在负限位
        /// </summary>
        public void CheckBlockCylinderAtNLimit()
        {
            if (!this.WaferTableModule.BlockCylinderNLimitSensor.CheckStateForNums(true, 3, 200))
            {
                //throw new Exception("Clamping cylinder is not in negative limit!");
                DialogResult res = AKRSMessageBoxExt.Show($"晶圆夹持气缸没有落下！" + "\r\n" + "点击OK，即将移动到位，" + "\r\n" + "点击Abort，即将终止！", "Prompt", new string[] { "OK", "Abort" }, new DialogResult[] { DialogResult.OK, DialogResult.Abort });
                switch (res)
                {
                    case DialogResult.OK:
                        this.ResetBlockCylinder();
                        return;
                    case DialogResult.Abort:
                        throw new Exception("晶圆夹持气缸没有落下！");
                }
            }
        }

        /// <summary>
        /// 晶圆夹电机是否在安全位置
        /// </summary>
        public void CheckWaferClampAxisYAtSafePosition()
        {
            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsShieldEjectModule)
            {
                return;
            }

            if (Math.Abs(this.WaferClampAxisYG0Pos.Y - WaferTableDevicePara.WaferClampSafePosition.Y) > this.range)
            {
                //throw new Exception("WaferClampAxisY not at SafePosition.");
                DialogResult res = AKRSMessageBoxExt.Show($"晶圆夹不在安全位置！" + "\r\n" + "点击OK，即将移动到位，" + "\r\n" + "点击Abort，即将终止！", "Prompt", new string[] { "OK", "Abort" }, new DialogResult[] { DialogResult.OK, DialogResult.Abort });
                switch (res)
                {
                    case DialogResult.OK:
                        this.MoveWaferClampToSafePosition();
                        return;
                    case DialogResult.Abort:
                        throw new Exception("晶圆夹不在安全位置！");
                }
            }
        }

        /// <summary>
        /// 扩晶电机是否在压膜位置
        /// </summary>
        public void CheckExpandAxisZAtDownPosition()
        {
            if (Math.Abs(this.ExpandAxisZG0Pos.Z - this.WaferTableDevicePara.ExpandDownPosition.Z) > this.range)
            {
                //throw new Exception("ExpandAxisZ not at ExpandDownPosition.");
                DialogResult res = AKRSMessageBoxExt.Show($"扩晶环未落下！" + "\r\n" + "点击OK，即将移动到位，" + "\r\n" + "点击Abort，即将终止！", "Prompt", new string[] { "OK", "Abort" }, new DialogResult[] { DialogResult.OK, DialogResult.Abort });
                switch (res)
                {
                    case DialogResult.OK:
                        this.MoveExpandToDownPosition();
                        return;
                    case DialogResult.Abort:
                        throw new Exception("扩晶环未落下！");
                }
            }
        }

        /// <summary>
        /// 晶圆台允许magazine动作
        /// </summary>
        public void CheckWaferTableAllowMagazineAction()
        {
            if (this.WaferTableAxisYG0Pos.Y + this.range < this.WaferTableDevicePara.AutoChangePosition.Y)
            {
                //throw new Exception("The wafer table is not in a secure area!");
                DialogResult res = AKRSMessageBoxExt.Show($"晶圆台不在安全的位置！" + "\r\n" + "点击OK，即将移动到位，" + "\r\n" + "点击Abort，即将终止！", "Prompt", new string[] { "OK", "Abort" }, new DialogResult[] { DialogResult.OK, DialogResult.Abort });
                switch (res)
                {
                    case DialogResult.OK:
                        this.MoveWaferTableToAutoChangePosition();
                        return;
                    case DialogResult.Abort:
                        throw new Exception("晶圆台不在安全的位置，magazine不允许运动！");
                }
            }
        }

        /// <summary>
        /// 检查晶圆台当前位置在环限位内
        /// </summary>       
        public void CheckWaferTableInCollisionCircle()
        {
            AKRSPoint3D center = WaferTableDevicePara.WaferTableCenter;
            double r = (long)WaferTableDevicePara.WaferTableRadius;
            double area = (this.WaferTableG0Pos.X - center.X) * (this.WaferTableG0Pos.X - center.X) + (this.WaferTableG0Pos.Y - center.Y) * (this.WaferTableG0Pos.Y - center.Y);
            if (area > r * r)
            {
                //throw new Exception("WaferTable current position out of CollisionCircle!");
                DialogResult res = AKRSMessageBoxExt.Show($"晶圆台当前位置超出了环限位！" + "\r\n" + "点击OK，即将继续动作，" + "\r\n" + "点击Abort，即将终止！", "Prompt", new string[] { "OK", "Abort" }, new DialogResult[] { DialogResult.OK, DialogResult.Abort });
                switch (res)
                {
                    case DialogResult.OK:
                        return;
                    case DialogResult.Abort:
                        throw new Exception("晶圆台当前位置超出了环限位！");
                }
            }
        }

        /// <summary>
        /// 关闭光源
        /// </summary>
        public void CloseLight()
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            if (MachineHardwareConfiguration.GetInstance().LightConfig == LightConfigEnum.TrichromaticLight)
            {
                this.WaferTableModule.GreenSpotLight.SetIntensity(0);
                this.WaferTableModule.RedSpotLight.SetIntensity(0);
                this.WaferTableModule.BlueSpotLight.SetIntensity(0);
                this.WaferTableModule.RingLight.SetIntensity(0);
            }
            else
            {
                this.WaferTableModule.RedSpotLight.SetIntensity(0);
                this.WaferTableModule.RingLight.SetIntensity(0);
            }
        }

        public void CheckWaferClampAllowMagazineAction()
        {
            if (this.WaferClampAxisYG0Pos.Y + this.range < WaferTableDevicePara.WaferClampWaitPickPosition.Y)
            {
                //throw new Exception("WaferClampAxisY is not in a secure area.");
                DialogResult res = AKRSMessageBoxExt.Show($"晶圆夹不在安全区域！" + "\r\n" + "点击OK，即将移动到位，" + "\r\n" + "点击Abort，即将终止！", "Prompt", new string[] { "OK", "Abort" }, new DialogResult[] { DialogResult.OK, DialogResult.Abort });
                switch (res)
                {
                    case DialogResult.OK:
                        this.MoveWaferClampToWaitPickPosition();
                        return;
                    case DialogResult.Abort:
                        throw new Exception("晶圆夹不在安全区域！");
                }
            }
        }
    }
}
