using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.DispenseSystem.Modules;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.TransportSystem.Modules;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.Infrastructure.Controls.Currency
{
    using Accord.IO;
    using AKRS.Galaxy2.Component.Simple.MoveControl;
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.Main.Machine.Parameter;
    using AKRS.ZX2200.WaferSubSystem.Controllers;

    using DevExpress.CodeParser;

    /// <summary>
    /// 方向盘-摇杆
    /// </summary>
    public partial class UcGuideMove
    {
        /// <summary>
        /// 轴运动线程
        /// </summary>
        private Task axiaMoveTask;

        /// <summary>
        /// 摇杆是否激活
        /// </summary>
        public static bool IsJoystickEnable = false;

        /// <summary>
        ///  当前模组
        /// </summary>
        public static string CurModule = string.Empty;

        /// <summary>
        /// 焊头模组
        /// </summary>
        private BondModule bondModule = new BondModule();

        /// <summary>
        /// 点胶模组
        /// </summary>
        private DispenseModule dispenseModule = new DispenseModule();

        /// <summary>
        ///  上料模组
        /// </summary>
        private LoaderBinModule loaderBinModule = new();

        /// <summary>
        ///  下料模组
        /// </summary>
        private UnloaderBinModule unLoaderBinModule = new();

        /// <summary>
        ///  下料模组
        /// </summary>
        private FlipModule flipModule = new();

        /// <summary>
        /// 机器模组
        /// </summary>
        private MachineModule machineModule = new MachineModule();

        /// <summary>
        /// 速度比例
        /// </summary>
        private double velProportion = 2500;

        /// <summary>
        ///  最大速度
        /// </summary>
        private double maxVel = 50;

        /// <summary>
        /// 摇杆参数
        /// </summary>
        private JoystickPara joystickPara => MachineDevicePara.GetInstance().JoystickPara;

        /// <summary>
        /// 摇杆X状态
        /// </summary>
        private JoystickStateEnum joystickXState;

        /// <summary>
        /// 摇杆Y状态
        /// </summary>
        private JoystickStateEnum joystickYState;

        /// <summary>
        /// 摇杆T状态
        /// </summary>
        private JoystickStateEnum joystickTState;

        /// <summary>
        /// 摇杆X状态
        /// </summary>
        private JoystickStateEnum joystickXStateTemp = JoystickStateEnum.Idle;

        /// <summary>
        /// 摇杆Y状态
        /// </summary>
        private JoystickStateEnum joystickYStateTemp = JoystickStateEnum.Idle;

        /// <summary>
        /// 摇杆T状态
        /// </summary>
        private JoystickStateEnum joystickTStateTemp = JoystickStateEnum.Idle;

        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshJoystick()
        {
            short rtn = GTN.mc.GTN_GetAuAdc(
    this.machineModule.JoystickXRead.AxisCardNum,
    (short)this.machineModule.JoystickXRead.ElectricIO,
    out double xValue,
    1,
    out UInt32 pXClock);

            this.joystickPara.XInitialVal= Math.Round(xValue, 6) * 1000.0;

             rtn = GTN.mc.GTN_GetAuAdc(
this.machineModule.JoystickYRead.AxisCardNum,
(short)this.machineModule.JoystickYRead.ElectricIO,
out double yValue,
1,
out UInt32 pYClock);

             this.joystickPara.YInitialVal = Math.Round(yValue, 6) * 1000.0;

            rtn = GTN.mc.GTN_GetAuAdc(
this.machineModule.JoystickTRead.AxisCardNum,
(short)this.machineModule.JoystickTRead.ElectricIO,
out double tValue,
1,
out UInt32 pTClock);

            this.joystickPara.TInitialVal = Math.Round(tValue, 6) * 1000.0;

            MachineDevicePara.GetInstance().Save();
        }

        /// <summary>
        /// 获取轴速度
        /// </summary>
        /// <param name="electric">模拟量读取</param>
        /// <returns>速度</returns>
        public double GetSpeedFromJoystick(Electric electric)
        {
            short rtn = GTN.mc.GTN_GetAuAdc(
                electric.AxisCardNum,
                (short)electric.ElectricIO,
                out double pValue,
                1,
                out UInt32 p2Clock);

            double value = Math.Round(pValue, 6) * 1000.0;

            // 初始值上界
            double initialValUpperLimit = 2600;

            // 初始值下界
            double initialValLowerLimit = 2450 /*2500*/;

            double range = 0;

            double vel = 0;

            // 偏移量
            double offset = 0;

            // 速度比例每个方向可能不一样，暂时先这样写
            switch (electric.HardwareName)
            {
                case "摇杆X方向模拟量读取":

                    initialValUpperLimit = this.joystickPara.XInitialVal + this.joystickPara.BlindRange;
                    initialValLowerLimit = this.joystickPara.XInitialVal - this.joystickPara.BlindRange;

                    range = Math.Abs(this.joystickPara.XUpperLimit - this.joystickPara.XInitialVal)
                            - this.joystickPara.BlindRange;

                    // 在初始值范围内直接返回0
                    if (value >= initialValLowerLimit && value <= initialValUpperLimit)
                    {
                        this.joystickXState = JoystickStateEnum.Idle;

                        return 0;
                    }
                    else if (value > initialValUpperLimit)
                    {
                        offset = value - initialValUpperLimit;

                        this.joystickXState = JoystickStateEnum.Rising;
                    }
                    else
                    {
                        offset = value - initialValLowerLimit;

                        this.joystickXState = JoystickStateEnum.Falling;
                    }


                    break;


                case "摇杆Y方向模拟量读取":

                    initialValUpperLimit = this.joystickPara.YInitialVal + this.joystickPara.BlindRange;
                    initialValLowerLimit = this.joystickPara.YInitialVal - this.joystickPara.BlindRange;

                    range = Math.Abs(this.joystickPara.YUpperLimit - this.joystickPara.YInitialVal)
                            - this.joystickPara.BlindRange;

                    // 在初始值范围内直接返回0
                    if (value >= initialValLowerLimit && value <= initialValUpperLimit)
                    {
                        this.joystickYState = JoystickStateEnum.Idle;

                        return 0;
                    }
                    else if (value > initialValUpperLimit)
                    {
                        offset = initialValUpperLimit-value;

                        this.joystickYState = JoystickStateEnum.Rising;
                    }
                    else
                    {
                        offset = initialValLowerLimit- value;

                        this.joystickYState = JoystickStateEnum.Falling;
                    }


                    break;

                case "摇杆T方向模拟量读取":

                    initialValUpperLimit = this.joystickPara.TInitialVal + this.joystickPara.BlindRange;
                    initialValLowerLimit = this.joystickPara.TInitialVal - this.joystickPara.BlindRange;

                    range = Math.Abs(this.joystickPara.TUpperLimit - this.joystickPara.TInitialVal)
                            - this.joystickPara.BlindRange;

                    // 在初始值范围内直接返回0
                    if (value >= initialValLowerLimit && value <= initialValUpperLimit)
                    {
                        this.joystickTState = JoystickStateEnum.Idle;

                        return 0;
                    }
                    else if (value > initialValUpperLimit)
                    {
                        offset = value - initialValUpperLimit;

                        this.joystickTState = JoystickStateEnum.Rising;
                    }
                    else
                    {
                        offset = value - initialValLowerLimit;

                        this.joystickTState = JoystickStateEnum.Falling;
                    }

                    break;
            }
        
            double proPortion = offset / range;

            if (Math.Abs(proPortion) > 1.2)
            {
                throw new Exception($"{electric.HardwareName}摇杆模拟量读取超过上界，当前模拟量{value},模拟量上界{initialValUpperLimit+range}，请重新示教摇杆！");
            }

            vel = proPortion * this.maxVel;

            return vel;
        }

        /// <summary>
        /// 轴移动并更新速度
        /// </summary>
        /// <param name="axis">轴</param>
        /// <param name="vel">速度</param>
        private void AxisMoveAndUpdateVel(Axis axis, double vel, Electric electric)
        {
            // 状态刷新
            switch (electric.HardwareName)
            {
                case "摇杆X方向模拟量读取":

                    if(this.joystickXStateTemp != JoystickStateEnum.Idle && this.joystickXState == JoystickStateEnum.Idle)
                    {
                        // 轴停止
                        axis.StopMove();

                        this.joystickXStateTemp = this.joystickXState;

                        Console.WriteLine($" 轴{axis.HardwareName}停止,joystickXStateTemp:{this.joystickXStateTemp.ToString()}");

                        return;
                    }
                    else
                    {
                        this.joystickXStateTemp = this.joystickXState;

                        break;
                    }

                case "摇杆Y方向模拟量读取":

                    if (this.joystickYStateTemp != JoystickStateEnum.Idle && this.joystickYState == JoystickStateEnum.Idle)
                    {
                        // 轴停止
                        axis.StopMove();

                        Console.WriteLine($" 轴{axis.HardwareName}停止, joystickYStateTemp:{ this.joystickXStateTemp.ToString()}");

                        this.joystickYStateTemp = this.joystickYState;

                        return;
                    }
                    else
                    {
                        this.joystickYStateTemp = this.joystickYState;

                        break;
                    }


                case "摇杆T方向模拟量读取":

                    if (this.joystickTStateTemp != JoystickStateEnum.Idle && this.joystickTState == JoystickStateEnum.Idle)
                    {
                        // 轴停止
                        axis.StopMove();

                        Console.WriteLine($" 轴{axis.HardwareName}停止, joystickTStateTemp:{this.joystickXStateTemp.ToString()}");

                        this.joystickTStateTemp = this.joystickTState;

                        return;
                    }
                    else
                    {
                        this.joystickTStateTemp = this.joystickTState;

                        break;
                    }
            }

            if (vel == 0)
            {
                //if (axis.IsInRealPosition() == false)
                //{
                //    // 轴停止
                //    axis.StopMove();

                //    Console.WriteLine($" 轴{axis.HardwareName}停止");
                //}
            }
            else
            {
                // 判断轴状态
                if (axis.IsInRealPosition())
                {
                    MoveDirection moveDirection = vel < 0 ? MoveDirection.Negative : MoveDirection.Positive;

                    Console.WriteLine($"轴{axis.HardwareName}第一次发送运动指令速度{vel}");

                    // 第一次移动的速度按最大速度的百分之一
                    double initialVel = this.maxVel * 0.01;

                    // 移动
                    //axis.JogByAbsoluteMove(moveDirection, initialVel);
                }
                else
                {
                    // 更新速度
                    axis.UpdateVelOnLine(Math.Abs(vel));

                    Console.WriteLine($"轴{axis.HardwareName}速度刷新：{vel}");
                }
            }
        }

        /// <summary>
        /// 获取轴速度
        /// </summary>
        private void MoveAxisByJoystick()
        {
            double vel;

            try
            {
                while (IsJoystickEnable)
                {
                    switch (CurModule)
                    {
                        case "点胶XY":
                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickXRead);

                            this.AxisMoveAndUpdateVel(this.dispenseModule.GetDispenseXAxis(), vel, this.machineModule.JoystickXRead);

                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickYRead);

                            this.AxisMoveAndUpdateVel(this.dispenseModule.GetDispenseYAxis(), vel, this.machineModule.JoystickYRead);

                            break;

                        case "点胶Z":
                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickYRead);

                            this.AxisMoveAndUpdateVel(this.dispenseModule.GetDispenseZAxis(), vel, this.machineModule.JoystickYRead);

                            break;

                        case "固晶XY":
                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickXRead);

                            this.AxisMoveAndUpdateVel(this.bondModule.BondAxisX, vel, this.machineModule.JoystickXRead);

                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickYRead);

                            this.AxisMoveAndUpdateVel(this.bondModule.BondAxisY, vel, this.machineModule.JoystickYRead);

                            break;

                        case "固晶Z":
                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickYRead);

                            this.AxisMoveAndUpdateVel(this.bondModule.BondHead.AxisZ, vel, this.machineModule.JoystickYRead);

                            break;

                        case "固晶T":
                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickTRead);

                            this.AxisMoveAndUpdateVel(this.bondModule.BondHead.AxisT, vel, this.machineModule.JoystickTRead);

                            break;

                        case "吸嘴架Y":
                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickYRead);

                            this.AxisMoveAndUpdateVel(System2Module.GetInstance().NozzleShelfModule.AxisY, vel, this.machineModule.JoystickYRead);

                            break;

                        case "晶圆台XY":
                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickXRead) * 0.5;

                            this.AxisMoveAndUpdateVel(WaferSubModule.GetInstance().WaferTable.WaferTableAxisX, vel, this.machineModule.JoystickXRead);

                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickYRead) * 0.5;

                            this.AxisMoveAndUpdateVel(WaferSubModule.GetInstance().WaferTable.WaferTableAxisY, vel, this.machineModule.JoystickYRead);

                            break;

                        case "晶圆夹Y":
                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickYRead);

                            this.AxisMoveAndUpdateVel(WaferSubModule.GetInstance().WaferTable.WaferClampAxisY, vel, this.machineModule.JoystickYRead);

                            break;

                        case "晶圆台扩晶Z":

                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickYRead) * 0.01;

                            this.AxisMoveAndUpdateVel(WaferSubModule.GetInstance().WaferTable.ExpandAxisZ, vel, this.machineModule.JoystickYRead);

                            break;

                        case "晶圆相机Z":
                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickYRead);

                            this.AxisMoveAndUpdateVel(WaferSubModule.GetInstance().WaferTable.WaferCameraAxisZ, vel, this.machineModule.JoystickYRead);

                            break;

                        case "晶圆料盒Z":
                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickYRead);

                            this.AxisMoveAndUpdateVel(WaferSubModule.GetInstance().MagazineBox.MagazineAxisZ, vel, this.machineModule.JoystickYRead);

                            break;

                        case "顶针台Z":
                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickYRead);

                            this.AxisMoveAndUpdateVel(WaferSubModule.GetInstance().Eject.EjectionTableAxisZ, vel, this.machineModule.JoystickYRead);

                            break;


                        case "顶针架T":
                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickTRead);

                            this.AxisMoveAndUpdateVel(WaferSubModule.GetInstance().Eject.EjectionBankAxisT, vel, this.machineModule.JoystickTRead);

                            break;

                        case "顶针Z":
                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickYRead) * 0.1;

                            this.AxisMoveAndUpdateVel(WaferSubModule.GetInstance().Eject.EjectionAxisZ, vel, this.machineModule.JoystickYRead);

                            break;

                        case "自动上料模组XY":
                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickXRead);

                            this.AxisMoveAndUpdateVel(this.loaderBinModule.LoaderPushRod, vel, this.machineModule.JoystickXRead);

                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickYRead);

                            this.AxisMoveAndUpdateVel(this.loaderBinModule.LoaderTableAxisY, vel, this.machineModule.JoystickYRead);

                            break;

                        case "自动上料模组Z":

                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickYRead);

                            this.AxisMoveAndUpdateVel(this.loaderBinModule.LoaderTableAxisZ, vel, this.machineModule.JoystickYRead);

                            break;

                        case "自动下料模组XY":
                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickXRead);

                            this.AxisMoveAndUpdateVel(this.unLoaderBinModule.UnLoaderPushRod, vel, this.machineModule.JoystickXRead);

                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickYRead);

                            this.AxisMoveAndUpdateVel(this.unLoaderBinModule.UnLoaderTableAxisY, vel, this.machineModule.JoystickYRead);

                            break;

                        case "自动下料模组Z":

                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickYRead);

                            this.AxisMoveAndUpdateVel(this.unLoaderBinModule.UnLoaderTableAxisZ, vel, this.machineModule.JoystickYRead);

                            break;

                        case "翻转台T":
                            vel = this.GetSpeedFromJoystick(this.machineModule.JoystickTRead);

                            this.AxisMoveAndUpdateVel(this.flipModule.FlipTableAxisT, vel, this.machineModule.JoystickTRead);

                            break;
                    }

                    Thread.Sleep(5);
                }
            }
            catch (Exception e)
            {
                AKRSXtraMessageBox.Show($"摇杆控制轴运动异常！\r\n{e.ToString()}");
                throw;
            }
            finally
            {
                this.StopCurAxis();
            }
        }

        /// <summary>
        /// 摇杆
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnJoystick_Click(object sender, EventArgs e)
        {
            // 切换摇杆
            IsJoystickEnable = !IsJoystickEnable;
            this.BtnJoystick.Appearance.BackColor = IsJoystickEnable ? Color.Yellow : default;

            this.RefreshControl();

            if (IsJoystickEnable)
            {
                CurModule = "固晶XY";
                this.CbChangeModule.SelectedItem = CurModule;
                this.RefreshJoystick();

                // 开启线程
                if (this.axiaMoveTask == null || this.axiaMoveTask.IsCompleted)
                {
                    this.axiaMoveTask = Task.Run(
                        () =>
                            {
                                CommonUtil.SetCurrentThreadName("摇杆控制轴运动线程");

                                // 读取模拟量
                                this.MoveAxisByJoystick();
                            });
                }
            }
            else
            {
                CurModule = "固晶模组";
                this.CbChangeModule.SelectedItem = CurModule;
            }
        }

        /// <summary>
        /// 停止当前轴
        /// </summary>
        private void StopCurAxis()
        {
            // 轴停止
            switch (CurModule)
            {
                case "点胶XY":
                    this.dispenseModule.GetDispenseXAxis().StopMove();
                    this.dispenseModule.GetDispenseYAxis().StopMove();

                    break;

                case "点胶Z":
                    this.dispenseModule.GetDispenseZAxis().StopMove();

                    break;

                case "固晶XY":
                    this.bondModule.BondAxisX.StopMove();
                    this.bondModule.BondAxisY.StopMove();

                    break;

                case "固晶Z":
                    this.bondModule.BondHead.AxisZ.StopMove();

                    break;

                case "固晶T":
                    this.bondModule.BondHead.AxisT.StopMove();

                    break;

                case "吸嘴架Y":
                    System2Module.GetInstance().NozzleShelfModule.AxisY.StopMove();

                    break;

                case "晶圆台XY":
                    WaferSubModule.GetInstance().WaferTable.WaferTableAxisX.StopMove();
                    WaferSubModule.GetInstance().WaferTable.WaferTableAxisY.StopMove();

                    break;

                case "晶圆夹Y":
                    WaferSubModule.GetInstance().WaferTable.WaferClampAxisY.StopMove();

                    break;

                case "晶圆台扩晶Z":
                    WaferSubModule.GetInstance().WaferTable.ExpandAxisZ.StopMove();

                    break;

                case "晶圆相机Z":
                    WaferSubModule.GetInstance().WaferTable.WaferCameraAxisZ.StopMove();

                    break;

                case "晶圆料盒Z":
                    WaferSubModule.GetInstance().MagazineBox.MagazineAxisZ.StopMove();

                    break;

                case "顶针台Z":
                    WaferSubModule.GetInstance().Eject.EjectionTableAxisZ.StopMove();

                    break;

                case "顶针架T":
                    WaferSubModule.GetInstance().Eject.EjectionBankAxisT.StopMove();

                    break;

                case "顶针Z":
                    WaferSubModule.GetInstance().Eject.EjectionAxisZ.StopMove();

                    break;

                case "自动上料模组XYZ":
                    this.loaderBinModule.LoaderPushRod.StopMove();
                    this.loaderBinModule.LoaderTableAxisY.StopMove();
                    this.loaderBinModule.LoaderTableAxisZ.StopMove();

                    break;

                case "自动下料模组XYZ":
                    this.unLoaderBinModule.UnLoaderPushRod.StopMove();
                    this.unLoaderBinModule.UnLoaderTableAxisY.StopMove();
                    this.unLoaderBinModule.UnLoaderTableAxisZ.StopMove();

                    break;

                case "翻转台T":
                    this.flipModule.FlipTableAxisT.StopMove();

                    break;
            }
        }

        /// <summary>
        ///  速度切换
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void rGSpeedMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rGSpeedMode.EditValue != null)
            {
                this.maxVel = Convert.ToInt32(rGSpeedMode.EditValue);
            }
        }

        /// <summary>
        /// 绑定硬件
        /// </summary>
        private void JoystickBindHardware()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                // 脱机模式不绑定
                return;
            }

            AxisConfig axisConfig = new(null, null, null, null, null, null);
            VisionModule visionModule = new();

            if(this.moduleName== "点胶XY"|| this.moduleName == "点胶Z")
            {
                this.axisX = dispenseModule.GetDispenseXAxis();
                this.axisY = dispenseModule.GetDispenseYAxis();
                this.axisZ = dispenseModule.GetDispenseZAxis();
                axisConfig = new(this.axisX, this.axisY, this.axisZ, null, null, null);

                System1Domain.GetInstance().DispenseVisionController.InitLight(this.ucLight1, this.ucLight2);

                this.coordinateSystem = MachineCoordinateSystem.GetInstance().CoordinateSystems
                    .Find(it => it.Name == "DispenseCoordinateSystem");
                this.ChangeCamera(CameraEnum.DispenseCamera);

                this.camera = visionModule.DispenseCamera;
            }
            else if(this.moduleName == "固晶XY" || this.moduleName == "固晶Z" || this.moduleName == "固晶T")
            {
                this.axisX = System2Module.GetInstance().BondModule.BondAxisX;
                this.axisY = System2Module.GetInstance().BondModule.BondAxisY;
                this.axisZ = System2Module.GetInstance().BondModule.BondHead.AxisZ;
                this.axisU = System2Module.GetInstance().BondModule.BondHead.AxisT;
                axisConfig = new(this.axisX, this.axisY, this.axisZ, this.axisU, null, null);

                // Bond光源配置
                this.ucLight1.Init("固晶点光", "邦头三色点光-红", "邦头三色点光-绿", "邦头三色点光-蓝");
                this.ucLight2.Init("固晶环光", "邦头三色环光-红", "邦头三色环光-绿", "邦头三色环光-蓝");

                this.CbChangeModule.SelectedItem = "固晶模组";

                this.coordinateSystem = MachineCoordinateSystem.GetInstance().CoordinateSystems
                    .Find(it => it.Name == "BondCoordinateSystem");
                this.ChangeCamera(CameraEnum.BondCamera);

                this.camera = System2Module.GetInstance().BondModule.BondCamera;
            }
            else if (this.moduleName == "吸嘴架Y" )
            {
                this.axisY = HardwareRepositoryService.GetHardware<Axis>("焊头放置架Y");
                axisConfig = new(null, this.axisY, null, null, null, null);

                this.ChangeCamera(CameraEnum.BondCamera);

                this.camera = System2Module.GetInstance().BondModule.BondCamera;

                this.coordinateSystem = MachineCoordinateSystem.GetInstance().CoordinateSystems
                    .Find(it => it.Name == "ToolBankCoordinateSystem");
            }
            else if (this.moduleName == "晶圆台XY"|| this.moduleName == "晶圆台扩晶Z" )
            {
                this.axisX = WaferSubModule.GetInstance().WaferTable.WaferTableAxisX;
                this.axisY = WaferSubModule.GetInstance().WaferTable.WaferTableAxisY;
                this.axisZ = WaferSubModule.GetInstance().WaferTable.ExpandAxisZ;
                axisConfig = new(this.axisX, this.axisY, this.axisZ, null, null, null);

                // 晶圆光源配置
                this.ucLight1.Init("晶圆点光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRedSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetGreenSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetBlueSpotLightHardwareName()}");
                this.ucLight2.Init("晶圆环光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}");

                this.coordinateSystem = MachineCoordinateSystem.GetInstance().CoordinateSystems
                    .Find(it => it.Name == "WaferTableCoordinateSystem");
                this.ChangeCamera(CameraEnum.WaferCamera);

                this.camera = WaferSubModule.GetInstance().WaferTable.WaferCamera;
            }
            else if (this.moduleName == "晶圆相机Z")
            {
                this.axisX = WaferSubModule.GetInstance().WaferTable.WaferTableAxisX;
                this.axisY = WaferSubModule.GetInstance().WaferTable.WaferTableAxisY;
                this.axisZ = WaferSubModule.GetInstance().WaferTable.WaferCameraAxisZ;
                axisConfig = new(this.axisX, this.axisY, this.axisZ, null, null, null);

                // 晶圆光源配置
                this.ucLight1.Init("晶圆点光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRedSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetGreenSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetBlueSpotLightHardwareName()}");
                this.ucLight2.Init("晶圆环光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}");

                this.ChangeCamera(CameraEnum.WaferCamera);
                this.camera = WaferSubModule.GetInstance().WaferTable.WaferCamera;
            }
            else if (this.moduleName == "晶圆料盒Z"|| this.moduleName == "晶圆夹Y")
            {
                this.axisX = WaferSubModule.GetInstance().WaferTable.WaferTableAxisX;
                this.axisY = WaferSubModule.GetInstance().WaferTable.WaferClampAxisY;
                this.axisZ = WaferSubModule.GetInstance().MagazineBox.MagazineAxisZ;
                axisConfig = new(this.axisX, this.axisY, this.axisZ, null, null, null);

                // 晶圆光源配置
                this.ucLight1.Init("晶圆点光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRedSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetGreenSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetBlueSpotLightHardwareName()}");
                this.ucLight2.Init("晶圆环光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}");

                this.ChangeCamera(CameraEnum.WaferCamera);
                this.camera = WaferSubModule.GetInstance().WaferTable.WaferCamera;
            }
            else if (this.moduleName == "顶针台Z" || this.moduleName == "顶针架T")
            {
                this.axisX = WaferSubModule.GetInstance().WaferTable.WaferTableAxisX;
                this.axisY = WaferSubModule.GetInstance().WaferTable.WaferTableAxisY;
                this.axisZ = WaferSubModule.GetInstance().Eject.EjectionTableAxisZ;
                this.axisU = WaferSubModule.GetInstance().Eject.EjectionBankAxisT;

                axisConfig = new(this.axisX, this.axisY, this.axisZ, this.axisU, null, null);

                // 晶圆光源配置
                this.ucLight1.Init("晶圆点光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRedSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetGreenSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetBlueSpotLightHardwareName()}");
                this.ucLight2.Init("晶圆环光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}");

                this.ChangeCamera(CameraEnum.WaferCamera);
                this.camera = WaferSubModule.GetInstance().WaferTable.WaferCamera;
            }
            else if (this.moduleName == "顶针Z")
            {
                this.axisX = WaferSubModule.GetInstance().WaferTable.WaferTableAxisX;
                this.axisY = WaferSubModule.GetInstance().WaferTable.WaferTableAxisY;
                this.axisZ = WaferSubModule.GetInstance().Eject.EjectionAxisZ;
                this.axisU = WaferSubModule.GetInstance().Eject.EjectionBankAxisT;

                axisConfig = new(this.axisX, this.axisY, this.axisZ, this.axisU, null, null);

                // 晶圆光源配置
                this.ucLight1.Init("晶圆点光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRedSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetGreenSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetBlueSpotLightHardwareName()}");
                this.ucLight2.Init("晶圆环光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}");

                this.ChangeCamera(CameraEnum.WaferCamera);
                this.camera = WaferSubModule.GetInstance().WaferTable.WaferCamera;
            }
            else if (this.moduleName == "自动上料模组XY"|| this.moduleName == "自动上料模组Z")
            {
                this.axisX = loaderBinModule.LoaderPushRod;
                this.axisY = loaderBinModule.LoaderTableAxisY;
                this.axisZ = loaderBinModule.LoaderTableAxisZ;
                axisConfig = new(this.axisX, this.axisY, this.axisZ, null, null, null);

                this.camera = null;
            }
            else if (this.moduleName == "自动下料模组XY" || this.moduleName == "自动下料模组Z")
            {
                this.axisX = unLoaderBinModule.UnLoaderPushRod;
                this.axisY = unLoaderBinModule.UnLoaderTableAxisY;
                this.axisZ = unLoaderBinModule.UnLoaderTableAxisZ;
                axisConfig = new(this.axisX, this.axisY, this.axisZ, null, null, null);

                this.camera = null;
            }
            else if (this.moduleName == "翻转台T")
            {
                this.axisU = WaferSubModule.GetInstance().FlipModule.FlipTableAxisT;
                axisConfig = new(null, null, null, this.axisU, null, null);
            }

            // 如果有传进来的相机 则重置相机
            if (this.cameraEnum != CameraEnum.None)
            {
                this.ChangeCamera(this.cameraEnum);
            }

            this.SetExposureTime();

            this.ucMoveControl.Init(axisConfig, this.sourceName);
        }
    }

    /// <summary>
    /// 摇杆状态
    /// </summary>
    public enum JoystickStateEnum
    {
        /// <summary>
        /// 静止状态：无变化或变化小于阈值
        /// </summary>
        Idle = 0,

        /// <summary>
        /// 上升状态（Rising）：检测到上升沿
        /// </summary>
        Rising = 1,

        /// <summary>
        /// 下降状态（Falling）：检测到下降沿
        /// </summary>
        Falling = 2
    }
}
