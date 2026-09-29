using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.WaferSubSystem.Controllers
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using System.Threading;

    using AKRS.ZX2200.Infrastructure.Models.CommonModels;

    using DevExpress.XtraBars.Navigation;
    using DevExpress.Utils.Extensions;
    using DevExpress.XtraEditors;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Log;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool;
    using log4net.Core;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using System.Windows.Forms;

    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using System.Diagnostics;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using System.ComponentModel;

    /// <summary>
    /// WaferSubController
    /// </summary>
    public class WaferSubController : SingletonNoSave<WaferSubController>
    {
        /// <summary>
        /// 晶圆台控制器
        /// </summary>
        public WaferTableController WaferTableController { get; set; } = new WaferTableController();

        /// <summary>
        /// Magazine控制器
        /// </summary>
        public MagazineController MagazineController { get; set; } = new MagazineController();

        /// <summary>
        /// Eject控制器
        /// </summary>
        public EjectController EjectController { get; set; } = new EjectController();

        /// <summary>
        /// 翻转台控制器
        /// </summary>
        public FlipTableController FlipController { get; set; } = new FlipTableController();

        /// <summary>
        /// 气缸去原位
        /// </summary>
        /// <param name="electric">气缸</param>
        /// <param name="sensor">磁开</param>
        /// <param name="delayBefore">delayBefore</param>
        /// <param name="delayAfter">delayAfter</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="isCancelWait">isCancelWait</param>
        public void ResetCyc(Electric electric, Sensor sensor, int delayBefore = 0, int delayAfter = 0, int timeOut = 5000, bool isCancelWait = false)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            WaferSystemDomain.GetInstance().SingleStep();

            //if (!electric.CurrentOutputValue && sensor.CheckStateForNums(true, 1, 0))
            //{
            //    return;
            //}

            Thread.Sleep(delayBefore);
            electric.SetOutputValue(false);
            bool ret = sensor.WaitSignal(true, timeOut, ref isCancelWait);
            if (ret == false)
            {
                //throw new Exception($"{electric.HardwareName} Cylinder reset operation timeout");
                throw new Exception($"{electric.HardwareName} 气缸复位操作超时！");
            }

            Thread.Sleep(delayAfter);
        }

        /// <summary>
        /// 气缸去到位
        /// </summary>
        /// <param name="electric">气缸</param>
        /// <param name="sensor">磁开</param>
        /// <param name="delayBefore">delayBefore</param>
        /// <param name="delayAfter">delayAfter</param>
        /// <param name="timeOut">timeOut</param>
        /// <param name="isCancelWait">isCancelWait</param>
        public void SetCyc(Electric electric, Sensor sensor, int delayBefore = 0, int delayAfter = 0, int timeOut = 5000, bool isCancelWait = false)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            WaferSystemDomain.GetInstance().SingleStep();

            //if (electric.CurrentOutputValue && sensor.CheckStateForNums(true, 1, 0))
            //{
            //    return;
            //}

            Thread.Sleep(delayBefore);
            electric.SetOutputValue(true);
            bool ret = sensor.WaitSignal(true, timeOut, ref isCancelWait);
            if (ret == false)
            {
                //throw new Exception($"{electric.HardwareName} Cylinder set operation timeout");
                throw new Exception($"{electric.HardwareName} 气缸置位操作超时！");
            }

            Thread.Sleep(delayAfter);
        }

        /// <summary>
        /// 气缸去原位
        /// </summary>
        /// <param name="electric">气缸</param>
        /// <param name="delayBefore">delayBefore</param>
        /// <param name="delayAfter">delayAfter</param>
        public void ResetCyc(Electric electric, int delayBefore = 0, int delayAfter = 0)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            WaferSystemDomain.GetInstance().SingleStep();

            //if (!electric.CurrentOutputValue)
            //{
            //    return;
            //}

            Thread.Sleep(delayBefore);
            electric.SetOutputValue(false);
            Thread.Sleep(delayAfter);
        }

        /// <summary>
        /// 气缸去到位
        /// </summary>
        /// <param name="electric">气缸</param>
        /// <param name="delayBefore">delayBefore</param>
        /// <param name="delayAfter">delayAfter</param>
        public void SetCyc(Electric electric, int delayBefore = 0, int delayAfter = 0)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            WaferSystemDomain.GetInstance().SingleStep();

            //if (electric.CurrentOutputValue)
            //{
            //    return;
            //}

            Thread.Sleep(delayBefore);
            electric?.SetOutputValue(true);
            Thread.Sleep(delayAfter);
        }

        /// <summary>
        /// SetUIControl
        /// </summary>
        /// <param name="tileBar">tileBar</param>
        /// <param name="tileBarGroup">tileBarGroup</param>
        /// <param name="list">list</param>
        /// <param name="stepIndex">stepIndex</param>
        /// <param name="bt1">bt1</param>
        /// <param name="bt2">bt2</param>
        /// <param name="bt3">bt3</param>
        /// <param name="lc1">lc1</param>
        public void SetUIControl(TileBar tileBar, TileBarGroup tileBarGroup, List<AssistantConfig> list, int stepIndex, SimpleButton bt1, SimpleButton bt2, SimpleButton bt3, LabelControl lc1)
        {
            if (list == null || stepIndex + 1 > list.Count)
            {
                return;
            }

            tileBarGroup.Items.ForEach(
                a =>
                    {
                        TileBarItem b = (TileBarItem)a;
                        b.ImageAlignment = TileItemContentAlignment.MiddleLeft;
                        b.ImageScaleMode = TileItemImageScaleMode.ZoomInside;
                        b.ImageToTextAlignment = TileControlImageToTextAlignment.Left;
                    });

            tileBar.SelectedItem = tileBarGroup.Items[stepIndex];
            AssistantConfig assistantConfig = list[stepIndex];
            bt1.Visible = assistantConfig.IsShowBack;
            bt2.Visible = assistantConfig.IsShowNext;
            bt3.Visible = assistantConfig.IsShowDone;
            lc1.Text = assistantConfig.Descritpion;
            lc1.Visible = assistantConfig.IsShowTitle;
        }

        /// <summary>
        /// 倒装动作
        /// </summary>
        /// <param name="component">芯片</param>
        /// <param name="notifyWafer">委托</param>
        /// <returns>结果</returns>
        public ExcuteResult FlipChipAction(BaseCarrierConfig component, Action notifyWafer = null)
        {
            try
            {
                // 获取翻转工具
                FlipTool flipTool = WaferSystemProgram.GetInstance().FlipModuleProgram.CurrentFlipTool;

                // 最终取片位
                double pickPos = flipTool.FlipToolWaferHeight + component.FlipArmOverTravelDistanceWafer;

                this.PrepareBeforePick(component);

                // 二段速下降
                if (component.IsActivateSlowTravelBeforeFlip)
                {
                    double prePickPos = pickPos + component.SlowTravelDistanceBeforeFlip;

                    // 高速段
                    this.FlipController.MoveFlipTAxis(prePickPos);

                    Static.RecordTime("翻转台流程", $"高速运动到预取片位");

                    // 低速段
                    this.FlipController.MoveFlipTAxis(pickPos, component.SlowTravelSpeedBeforeFlip);

                    Static.RecordTime(
                        "翻转台流程",
                        $"低速运动到取片位,二段速距离{component.SlowTravelDistanceBeforeFlip}mm,二段速速度{component.SlowTravelSpeedBeforeFlip}mm/s");
                }
                else
                {
                    // 翻转到取片位
                    this.FlipController.MoveFlipTAxis(pickPos);

                    Static.RecordTime("翻转台流程", $"翻转到取片位完成");
                }

                int pickupDelay = (component.FlipToolPickupDelay - component.VacuumOffDelay > 0) ? (component.FlipToolPickupDelay - component.VacuumOffDelay) : 0;

                // 拾取延时
                Thread.Sleep(pickupDelay);

                // 翻转台真空打开
                this.FlipController.OpenFlipTableVacuum();

                Static.RecordTime("翻转台流程", $"翻转台真空打开完成");

                // 蓝膜芯片
                if (component is CarrierWithWaferConfig)
                {
                    // 顶针顶起
                    this.EjectController.MoveEjectToLiftPosition();

                    Static.RecordTime("翻转台流程", $"顶针顶起完成");
                }

                // 真空延时
                Thread.Sleep(component.FlipToolVacuumAtEndOfProcess);

                Static.RecordTime("翻转台流程", $"翻转台真空延时{component.FlipToolVacuumAtEndOfProcess}ms");

                // 二段速上抬
                if (component.IsActivateSlowTravelAfterFlip)
                {
                    double preLiftPos = pickPos + component.SlowTravelDistanceAfterFlip;

                    // 低速段
                    this.FlipController.MoveFlipTAxis(preLiftPos, component.SlowTravelSpeedAfterFlip);

                    Static.RecordTime(
                        "翻转台流程",
                        $"低速运动到预抬起位，二段速距离{component.SlowTravelDistanceBeforeFlip}mm,二段速速度{component.SlowTravelSpeedBeforeFlip}mm/s");

                    // 漏晶检测
                    if (component.IsCheckComponentDuringFlip)
                    {
                        ReCheckVacuum:

                        if (this.FlipController.IsComponentOnFlipTool() == false) 
                        {
                            // 真空报警芯片+1
                            component.AddCountOfVacuumError();

                            DialogResult dialogResult = AKRSMessageBoxExt.Show(
                                $"翻转台取片失败！请选择如何处理! \r\n",
                                "翻转台报警",
                                new string[] { "重新检测", "终止", "忽略", "重取", "下一颗" },
                                new DialogResult[]
                                    {
                                        DialogResult.Yes, DialogResult.Abort, DialogResult.Ignore, DialogResult.Retry,
                                        DialogResult.No
                                    },
                                AlarmLevel.SecondLevel);

                            switch (dialogResult)
                            {
                                // 重新检测
                                case DialogResult.Yes:
                                    goto ReCheckVacuum;

                                // 退出
                                case DialogResult.Abort:
                                    Machine.GetInstance().Stop();

                                    return ExcuteResult.Abort;

                                // 忽略
                                case DialogResult.Ignore:
                                    break;

                                // 重取
                                case DialogResult.Retry:

                                    // 翻转台真空关闭
                                    this.FlipController.CloseFlipTableVacuum();

                                    // 低速下压
                                    this.FlipController.MoveFlipTAxis(pickPos, component.SlowTravelSpeedBeforeFlip);

                                    // 拾取延时
                                    Thread.Sleep(pickupDelay);

                                    // 翻转台真空打开
                                    this.FlipController.OpenFlipTableVacuum();

                                    // 真空延时
                                    Thread.Sleep(component.FlipToolVacuumAtEndOfProcess);

                                    // 低速段上抬
                                    this.FlipController.MoveFlipTAxis(preLiftPos, component.SlowTravelSpeedAfterFlip);

                                    goto ReCheckVacuum;

                                // 下一颗
                                case DialogResult.No:

                                    // 通知晶圆台搜晶
                                    notifyWafer?.Invoke();

                                    // 回0,防止挡到搜晶
                                    this.FlipController.FlipTableGoHome();

                                    return ExcuteResult.NoStart;
                            }
                        }
                    }

                    // 等低速段走完再通知晶圆台搜晶，避免超时报警
                    notifyWafer?.Invoke();

                    // 翻转到焊头交接位
                    this.FlipController.MoveFlipToTransferPos();

                    Static.RecordTime("翻转台流程", $"翻转到焊头交接位");
                }
                else
                {
                    // 通知晶圆台搜晶
                    notifyWafer?.Invoke();

                    // 翻转到焊头交接位
                    this.FlipController.MoveFlipToTransferPos();

                    Static.RecordTime("翻转台流程", $"翻转到焊头交接位");
                }

                return ExcuteResult.Success;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"翻转动作运行故障！", e, LogCategory.Component);
                return ExcuteResult.Exception;
            }
        }

        /// <summary>
        /// 倒装动作空跑
        /// </summary>
        /// <param name="component">芯片</param>
        /// <param name="notifyWafer">委托</param>
        /// <returns>结果</returns>
        public ExcuteResult FlipChipActionDryRun(BaseCarrierConfig component, Action notifyWafer = null)
        {
            try
            {
                // 获取翻转工具
                FlipTool flipTool = WaferSystemProgram.GetInstance().FlipModuleProgram.CurrentFlipTool;

                // 最终取片位
                double pickPos = flipTool.FlipToolWaferHeight + component.FlipArmOverTravelDistanceWafer;

                this.PrepareBeforePick(component);

                // 取片位抬高3mm
                double prePickPos = pickPos + 3;

                this.FlipController.MoveFlipTAxis(prePickPos);

                Static.RecordTime("翻转台流程", $"翻转到取片位完成");

                int pickupDelay = (component.FlipToolPickupDelay - component.VacuumOffDelay > 0) ? (component.FlipToolPickupDelay - component.VacuumOffDelay) : 0;

                // 拾取延时
                Thread.Sleep(pickupDelay);

                // 翻转台真空打开
                this.FlipController.OpenFlipTableVacuum();
                Static.RecordTime("翻转台流程", $"翻转台真空打开完成");

                // 蓝膜芯片
                if (component is CarrierWithWaferConfig)
                {
                    // 顶针顶起
                    this.EjectController.MoveEjectToLiftPosition();

                    Static.RecordTime("翻转台流程", $"顶针顶起完成");
                }

                // 真空延时
                Thread.Sleep(component.FlipToolVacuumAtEndOfProcess);

                Static.RecordTime("翻转台流程", $"翻转台真空延时{component.FlipToolVacuumAtEndOfProcess}ms");

                // 通知晶圆台搜晶
                notifyWafer?.Invoke();

                // 翻转到焊头交接位
                this.FlipController.MoveFlipToTransferPos();

                Static.RecordTime("翻转台流程", $"翻转到焊头交接位");

                return ExcuteResult.Success;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"翻转动作运行故障！", e, LogCategory.Component);
                return ExcuteResult.Exception;
            }
        }

        /// <summary>
        /// 取片前准备
        /// </summary>
        /// <param name="component">芯片</param>
        private void PrepareBeforePick(BaseCarrierConfig component)
        {
            if (component.IsOpenWaferTableVacuumBeforePick)
            {
                switch (component.CarrierType)
                {
                    case CarrierTypeEnum.Wafer:
                        WaferSubController.GetInstance().EjectController.OpenEjectionTableVacuum();
                        break;
                    case CarrierTypeEnum.Waffle:
                        WaferSubController.GetInstance().WaferTableController.OpenWaffleVacuum();
                        break;
                }

                // 开真空延时
                Thread.Sleep(component.OpenWaferTableVacuumBeforePickDelay);
            }
            else
            {
                switch (component.CarrierType)
                {
                    case CarrierTypeEnum.Wafer:
                        WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();
                        break;
                    case CarrierTypeEnum.Waffle:
                        WaferSubController.GetInstance().WaferTableController.CloseWaffleVacuum();
                        break;
                }
            }
        }

        /// <summary>
        /// 翻转台翻转芯片
        /// </summary>
        /// <param name="component">芯片</param>
        /// <returns>结果</returns>
        public bool FlipFromWaferTable(BaseCarrierConfig component)
        {
            // 去避让位
            System2Domain.GetInstance().BondModuleController.MoveToSafePos();

            if (this.FlipController.IsFlipTableAtHome() == false)
            {
                // 去0位
                this.FlipController.FlipTableGoHome();
            }

            #region 翻转吸嘴堵塞判断

            //if (this.FlipController.IsComponentOnFlipTool())
            if (false)
            {
                DialogResult dialogResult = AKRSMessageBoxExt.ShowWarn(
                    $"翻转吸嘴检测到有芯片残留!\r\n",
                    "Warn",
                    new string[] { "忽略", "抛料" },
                    new DialogResult[] { DialogResult.Yes, DialogResult.No },
                    AlarmLevel.SecondLevel);

                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        break;

                    case DialogResult.No:

                        //抛料
                        WaferSubController.GetInstance().FlipController.Throw();
                        break;
                    default:
                        break;
                }
            }

            // 关闭吸嘴真空
            this.FlipController.CloseFlipTableVacuum();

            #endregion

            // 开搜晶线程
            WaferSystemDomain.GetInstance().WaferSubSystemTask.Start();

        #region 取料

        NextDie:

            // 晶圆上料准备判断（从信号池获取）,这个信号会自动复位
            if (!SignalPool.GetInstance().IsWaferAllowPickSignal.Wait())
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"等晶圆台允许取料信号失败，即将退出取片流程！!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            // 判断芯片类型
            if (component is CarrierWithWaferConfig)
            {
                // 顶针真空打开
                this.EjectController.OpenEjectionTableVacuum();
            }

            ExcuteResult ret = this.FlipChipAction(component);

            if (ret != ExcuteResult.Success)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"翻转工具取片失败！",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            // 如果晶圆台当前料片为晶圆料片，则顶针回预顶起位
            if (component is CarrierWithWaferConfig)
            {
                this.EjectController.MoveEjectToReadyLiftPositionAndBlow();
            }

            #endregion

            // 刷新Map信息
            WaferSystemDomain.GetInstance().Block.RefreshMap(true);

            Static.IsComponentOnFlipTool = true;

            return true;
        }

        /// <summary>
        /// 获取硬件集合
        /// </summary>
        /// <returns>结果</returns>
        public List<string> GetHardWareNames()
        {
            List<string> list = new List<string>();

            #region 顶针

            list.Add("顶针台Z");
            list.Add("顶针Z");
            list.Add("顶针放置架T");

            list.Add("顶针台气缸");
            list.Add("顶针换头气缸负限位");
            list.Add("顶针换头气缸正限位");

            list.Add("晶圆顶针台真空电磁阀");
            list.Add("晶圆顶针台吹气电磁阀");

            list.Add("顶针台接触传感器1");
            list.Add("顶针台接触传感器2");
            //list.Add("顶针负压检测");


            #endregion

            #region 翻转台

            if (MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated)
            {
                list.Add("翻转台T");
                list.Add("翻转台真空电磁阀");
                list.Add("FC比例阀设置");

                list.Add("翻转台吹气");
                list.Add("翻转台漏晶检测");
            }

            #endregion

            #region 料架模组

            list.Add("上晶圆Z");
            list.Add("上晶圆提篮夹紧气缸电磁阀");
            list.Add("上晶圆提篮夹紧气缸原点松开检测");

            list.Add("上晶圆提篮夹紧气缸动点夹紧检测");
            list.Add("上晶圆推料气缸电磁阀");


            list.Add("上晶圆推料气缸原点退回检测");
            list.Add("上晶圆推料气缸动点推出检测");

            list.Add("上晶圆提篮料层检测");
            list.Add("上晶圆提篮检测");

            #endregion

            #region 晶圆台

            if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.IsUseHotBlower)
            {
                list.Add("扩晶热吹风");
            }
            
            list.Add("晶圆夹持气缸电磁阀");
            list.Add("晶圆夹持气缸原点松开检测");
            list.Add("晶圆夹持气缸动点夹紧检测");

            list.Add("晶圆台X");
            list.Add("晶圆台Y");

            list.Add("晶圆台扩晶Z");
            list.Add("晶圆环检测");

            list.Add("晶圆夹Y");
            list.Add("上晶圆夹子气缸电磁阀");
            list.Add("上晶圆夹子气缸动点夹紧检测");
            list.Add("上晶圆夹子有料检测");

            list.Add("晶圆相机");
            list.Add("下视环光");
            list.Add("晶圆三色点光-绿");
            list.Add("晶圆三色点光-红");
            list.Add("晶圆三色点光-蓝");
            list.Add("晶圆相机Z");

            list.Add("晶圆台Tray盘真空电磁阀");
            //list.Add("晶圆台Tray盘负压检测");


            if(MachineHardwareConfiguration.GetInstance().IsStaticWaffleConfigrated)
            {
                list.Add("静态华夫盒真空电磁阀");
                list.Add("静态华夫盒真空检测");
            }

            #endregion


            return list;
        }
    }
}
