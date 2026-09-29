namespace AKRS.ZX2200.DispenseSystem.Controllers
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Drive.Common;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.LogicHardware.Hardwares.DispenseControllers;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.MeasureHeight;
    using AKRS.Galaxy2.MeasureHeight.IOMeasureHeight;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.DispenseSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using log4net.Core;
    using System;
    using System.Threading;
    using System.Windows.Forms;

    /// <summary>
    /// 点胶测高控制器
    /// </summary>
    public class DispenseMeasureHeightController
    {
        /// <summary>
        /// 点胶模组
        /// </summary>
        private readonly MeasureHeightModule measureHeightModule = new MeasureHeightModule();

        /// <summary>
        /// 点胶控制器
        /// </summary>
        private DispenseController dispenseController => System1Domain.GetInstance().DispenseController;

        /// <summary>
        /// 点胶域的坐标系
        /// </summary>
        public GeneralCoordinateSystem DispenseCoordinateSystem =>
            (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name == "DispenseCoordinateSystem");

        /// <summary>
        /// 点胶测高气缸打开并延时
        /// </summary>
        public void OpenDispenseHeightMeasurementCylinder()
        {
            // 激光测高没有测高气缸
            if (MachineHardwareConfiguration.GetInstance().IsSystem1ConfigLaserMh)
            {
                return;
            }

            DispenseRunTimeProvider.RecordTime("系统1测高流程", $"打开点胶测高气缸");

            this.OpenDispenseHeightMeasurementCylinderAndDelay(
                DispenseDevicePara.GetInstance().DispenseModulePara.CylinderDelay);

            DispenseRunTimeProvider.RecordTime("系统1测高流程", $"打开点胶测高气缸完成");
        }

        /// <summary>
        /// 点胶气缸关闭并延时
        /// </summary>
        public void CloseDispenseHeightMeasurementCylinder()
        {
            // 激光测高没有测高气缸
            if (MachineHardwareConfiguration.GetInstance().IsSystem1ConfigLaserMh)
            {
                return;
            }

            DispenseRunTimeProvider.RecordTime("系统1测高流程", $"关闭点胶测高气缸");

            this.CloseDispenseHeightMeasurementCylinderAndDelay(
                DispenseDevicePara.GetInstance().DispenseModulePara.CylinderDelay);

            DispenseRunTimeProvider.RecordTime("系统1测高流程", $"关闭点胶测高气缸完成");
        }

        /// <summary>
        /// 点胶测高气缸打开并延时
        /// </summary>
        /// <param name="delay">延时</param>
        private void OpenDispenseHeightMeasurementCylinderAndDelay(int delay)
        {
        RetryCommand:

            // 打开点胶气缸
            this.measureHeightModule.OpenDispenseAltimemetryCyclinder();

            DateTime dateTime = DateTime.Now;
            while (!this.measureHeightModule.IsDispenseAltimemetryCyclinderOpened())
            {
                // 如果在规定时间还没收到信号则报警
                if ((DateTime.Now - dateTime).Milliseconds > delay)
                {
                    DialogResult dialogResult = AKRSMessageBoxExt.Show(
                       $"点胶测高气缸打开失败，请选择如何处理\n\r" +
                       $"重试: 重新打开点胶测高气缸\n\r" +
                       $"退出: 设备停止",
                       "点胶气缸打开失败报警",
                       new string[] { "重试", "退出" },
                       new DialogResult[] { DialogResult.Retry, DialogResult.Abort },
                       AlarmLevel.SecondLevel);

                    if (dialogResult == DialogResult.Retry)
                    {
                        goto RetryCommand;
                    }
                    else
                    {
                        throw new Exception("点胶测高气缸打开失败");
                    }
                }

                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// 点胶气缸关闭并延时
        /// </summary>
        /// <param name="delay">延时</param>
        private void CloseDispenseHeightMeasurementCylinderAndDelay(int delay)
        {
        RetryCommand:
            // 关闭点胶气缸
            this.measureHeightModule.CloseDispenseAltimemetryCyclinder();

            DateTime dateTime = DateTime.Now;
            while (!this.measureHeightModule.IsDispenseAltimemetryCyclinderClosed())
            {
                // 如果在规定时间还没收到信号则报警
                if ((DateTime.Now - dateTime).TotalMilliseconds > delay)
                {
                    DialogResult dialogResult = AKRSMessageBoxExt.Show(
                       $"点胶测高气缸收回失败，请选择如何处理\n\r" +
                       $"重试: 重新收回点胶测高气缸\n\r" +
                       $"退出: 设备停止",
                       "点胶气缸收回失败报警",
                       new string[] { "重试", "退出" },
                       new DialogResult[] { DialogResult.Retry, DialogResult.Abort },
                       AlarmLevel.SecondLevel);

                    if (dialogResult == DialogResult.Retry)
                    {
                        goto RetryCommand;
                    }
                    else
                    {
                        throw new Exception("点胶测高气缸收回失败");
                    }
                }

                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// 该点在G0坐标系下的高度
        /// </summary>
        /// <param name="measHeightTool">测高工具</param>
        /// <param name="posG0">测高的位置</param>
        /// <param name="searchPosition">预测高高度</param>
        /// <param name="searchSpeed">搜索高度</param>
        /// <param name="closeCylinder">是否收回点胶测高气缸</param>
        /// <returns>在G0坐标系下的结果</returns>
        public (ExcuteResult Ret, double HeightValue) DispenserHeightMeasurementG0(
            System1MeasHeightToolEnum measHeightTool,
            AKRSPoint3D posG0,
            double searchPosition,
            double searchSpeed = double.NaN,
            bool closeCylinder = true)
        {
            RetryCommand:
            DispenseRunTimeProvider.RecordTime("系统1测高流程", $"开始点胶测高");
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return (ExcuteResult.Success, 0);
            }

            if (MachineStateModel.GetInstance().IsDryCycle)
            {
                if (posG0 != null)
                {
                    // 移动到测高的位置
                    dispenseController.MoveToG0Pos3D(posG0, true);
                }

                double level = this.DispenseCoordinateSystem.SelfPosToG0(dispenseController.GetAxisPos()).Z;

                return (ExcuteResult.Success, level);
            }

            // 判断测高类型
            Sensor sensor = measHeightTool == System1MeasHeightToolEnum.Dispenser ? 
                this.measureHeightModule.PreDispensePressureSensor : this.measureHeightModule.DispenseAltimemetrySensor;
            DispenseRunTimeProvider.RecordTime("系统1测高流程", $"测高类型为{sensor?.HardwareName}");

            if (posG0 != null)
            {
                // 移动到测高的位置
                this.dispenseController.MoveToG0Pos3D(posG0, true);
                DispenseRunTimeProvider.RecordTime("系统1测高流程", $"移动到测高位置完成");
            }

            // 单步工作
            if (!System1Domain.GetInstance().WaitSingleStep())
            {
                return (ExcuteResult.Abort, 0);
            }

            // 如果是测高针测高则需要打开气缸
            if (measHeightTool == System1MeasHeightToolEnum.HeightSensor)
            {
                // 打开测高气缸
                this.OpenDispenseHeightMeasurementCylinder();

                DispenseRunTimeProvider.RecordTime("系统1测高流程", $"打开点胶测高气缸完成");
            }


            (ExcuteResult ret, double height) = this.HeightMeasurement(sensor, searchPosition, searchSpeed);

            DispenseRunTimeProvider.RecordTime("系统1测高流程", $"测高流程完成");

            // 测高失败，报警提示
            if (ret != ExcuteResult.Success)
            {
                DispenseRunTimeProvider.RecordTime("系统1测高流程", $"测高失败");
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    "点胶模块测高失败，请选择怎么处理",
                    "报警",
                    new[] { "重试", "退出" },
                    new[] { DialogResult.Retry, DialogResult.Abort });

                if (dialogResult == DialogResult.Abort)
                {
                    DispenseRunTimeProvider.RecordTime("系统1测高流程", $"测高失败退出");
                    return (ExcuteResult.Abort, 0);
                }
                else
                {
                    DispenseRunTimeProvider.RecordTime("系统1测高流程", $"测高失败重试");
                    goto RetryCommand;
                }
            }

            if (measHeightTool == System1MeasHeightToolEnum.HeightSensor && closeCylinder)
            {
                // 关闭测高气缸
                this.CloseDispenseHeightMeasurementCylinder();
                DispenseRunTimeProvider.RecordTime("系统1测高流程", $"测高流程完成后关闭点胶测高气缸");
            }

            DispenseRunTimeProvider.RecordTime("系统1测高流程", $"测高成功，高度为{height}");

            double height1 = this.DispenseCoordinateSystem.SelfPosToG0(new AKRSPoint3D(0, 0, height)).Z;

            DispenseRunTimeProvider.RecordTime("系统1测高流程", $"测高成功，G0高度为{height1}");

            return (ret, Math.Round(height1, 4));
        }

        /// <summary>
        /// 点胶头/测高方法
        /// </summary>
        /// <param name="sensor">测高传感器</param>
        /// <param name="searchPosition">搜索高度 如果是 NaN 则取当前高度</param>
        /// <param name="searchSpeed">搜索速度，如果为NaN慢速</param>
        /// <returns>在轴坐标系下的结果</returns>
        public (ExcuteResult Ret, double HeightValue) HeightMeasurement(Sensor sensor, double searchPosition = double.NaN, double searchSpeed = double.NaN)
        {         
            // 设置当前高度为搜索高度
            if (double.IsNaN(searchPosition))
            {
                searchPosition = this.measureHeightModule.DispenseAxisZ.GetRealPosition();
            }

            DispenseRunTimeProvider.RecordTime("系统1测高流程", $"起始高度为{searchPosition}");

            if (double.IsNaN(searchSpeed))
            {
                searchSpeed = DispenseDevicePara.GetInstance().DispenseModulePara.MeasureHeightSpeed;
            }

            DispenseRunTimeProvider.RecordTime("系统1测高流程", $"速度为{searchSpeed}");

            LogHelper.Post(Level.Debug, $"准备测高的时候的高度 Z:{searchPosition}", LogCategory.Dispense);

            IOJudgeCondition condition = new IOJudgeCondition()
                                             {
                                                 MeasureHeightSensorName = sensor.HardwareName,

                                                 // 默认值
                                                 IsCalcLVDT = false,

                                                 // 默认值 不使用 lvdt
                                                 IsUseLVDTJudgeState = false,

                                                 // 当传感器 的状态为 TriggeIoValue 时 说明已经到达测高位置
                                                 TriggeIoValue = true
                                             };

            // 测高传感器测高
            if (sensor.HardwareName == this.measureHeightModule.DispenseAltimemetrySensor.HardwareName)
            {
                if (MachineHardwareConfiguration.GetInstance().IsSystem1ConfigLaserMh)
                {
                    DispenseRunTimeProvider.RecordTime("系统1测高流程", $"激光测高开始");

                    // 到位停留
                    Thread.Sleep(DispenseDevicePara.GetInstance().DispenseModulePara.LaserMhDelayTime);

                    DispenseRunTimeProvider.RecordTime("系统1测高流程", $"激光到位停留结束");

                    // 获取位置
                    double height = this.measureHeightModule.LaserMeasureHeight.GetValue();

                    DispenseRunTimeProvider.RecordTime("系统1测高流程", $"激光测高数值为{height}");

                    // 执行获取结果
                    ExcuteResult result = double.IsNaN(height) ? ExcuteResult.Abort : ExcuteResult.Success;

                    // 高度计算(激光测高距离越远越大)
                    height = this.measureHeightModule.DispenseAxisZ.GetRealPosition() - height;

                    DispenseRunTimeProvider.RecordTime("系统1测高流程", $"激光测高高度为{height}");

                    // 返回结果
                    return (result, height);
                }
                else
                {
                    // 如果还没有下探就触发感应器，则抛出异常
                    if (sensor.GetInputValue())
                    {
                        DispenseRunTimeProvider.RecordTime("系统1测高流程", $"点胶测高还未执行时测高传感器已收到信号");
                        DialogResult dialog = AKRSMessageBoxExt.Show(
                                        $"点胶测高还未执行时测高传感器已收到信号，请检查硬件后重试！",
                                        "异常",
                                        new string[] { "确定" },
                                        new DialogResult[] { DialogResult.OK, },
                                        AlarmLevel.SecondLevel);
                        return (ExcuteResult.Abort, 0);
                    }

                    // 探针测高
                    condition.MeasureHeightSensorName = "点胶测高传感器";

                    // 测高实体
                    MeasureHeightEntity measureHeightEntity = new MeasureHeightEntity()
                    {
                        AxisName = "点胶Z",
                        MeasureHeightAxis = this.measureHeightModule.DispenseAxisZ,
                        SearchPosition = searchPosition,
                        LiftPosition = searchPosition,
                        SearchVel = searchSpeed,
                        MeasureType = MeasureHeightTypeEnum.Probe,
                        SearchDir = MoveDirection.Negative,
                        SearchDistance = DispenseDevicePara.GetInstance().DispenseModulePara.MeasureHeightDownDistance + DispenseDevicePara.GetInstance().DispenseModulePara.MeasureHeightDistance,
                        TimeOutMillSeconds = 10000,
                        IsWaitStopArrive = false,
                        JudgeArrive = condition,

                        // 探针地址
                        ProbeAddress = 4,

                        // 打开探针  的地址
                        ProbeOpenValue = 4532,

                        // 探针测出高度存储的地址
                        ProbeValueAddress = 5
                    };

                    DispenseRunTimeProvider.RecordTime("系统1测高流程", $"点胶测高针开始执行测高");

                    // 返回结果
                    return measureHeightEntity.DoWork();
                }
            }
            else
            {
                if(sensor.GetInputValue())
                {
                    DispenseRunTimeProvider.RecordTime("系统1测高流程", $"点胶头还未执行时测高传感器已收到信号");
                    DialogResult dialog = AKRSMessageBoxExt.Show(
                                       $"点胶测高还未执行时测高传感器已收到信号，请检查硬件后重试！",
                                       "异常",
                                       new string[] { "确定" },
                                       new DialogResult[] { DialogResult.OK, },
                                       AlarmLevel.SecondLevel);
                    return (ExcuteResult.Abort, 0);
                }

                // IO测高
                MeasureHeightEntity measureHeightEntity = new MeasureHeightEntity()
                {
                    AxisName = "点胶Z",
                    MeasureHeightAxis = this.measureHeightModule.DispenseAxisZ,
                    SearchPosition = searchPosition,
                    LiftPosition = searchPosition,
                    SearchVel = searchSpeed,
                    MeasureType = MeasureHeightTypeEnum.IOMeasureHeight,
                    SearchDir = MoveDirection.Negative,
                    SearchDistance = 3,
                    TimeOutMillSeconds = 10000,
                    IsWaitStopArrive = false,
                    JudgeArrive = condition,
                };

                DispenseRunTimeProvider.RecordTime("系统1测高流程", $"点胶头开始执行测高");

                // 返回结果
                return measureHeightEntity.DoWork();
            }
        }
    }
}
