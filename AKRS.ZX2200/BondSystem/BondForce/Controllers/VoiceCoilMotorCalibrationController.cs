using System;

using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Modules;

using DevExpress.XtraEditors;

using Newtonsoft.Json;

namespace AKRS.ZX2200.BondSystem.BondForce.Controllers
{
    using AKRS.ZX2200.BondSystem.BondForce.Models.VoiceCoilModels;

    /// <summary>
    /// 音圈电机力控控制器
    /// </summary>
    public class VoiceCoilMotorCalibrationController : BaseCalibrationController
    {
        /// <summary>
        /// Bond头
        /// </summary> 
        private BondHead BondHead => System2Module.GetInstance().BondModule.BondHead;

        /// <summary>
        /// 力控参数
        /// </summary>
        private VoiceCoilMotorForceCalibrationPara voiceCoilMotorForceCalibrationPara =
            VoiceCoilMotorForceCalibrationPara.GetInstance();

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController = System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 力传感器读数
        /// </summary>
        [JsonIgnore]
        private int[] holdRegisters;

        /// <summary>
        /// 抬起高度
        /// </summary>
        [JsonIgnore]
        private double LiftPosition { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        private ushort cardNo = 0;

        /// <summary>
        /// 开启转矩运动
        /// </summary>
        public override void TurnOn()
        {
            this.TorqueTurnOn();
        }

        /// <summary>
        /// 关闭转矩运动
        /// </summary>
        public override void TurnOff()
        {
        }

        /// <summary>
        /// 启动转矩运动，测试用，此方法应该在底层封装，后续修改
        /// </summary>
        private void TorqueTurnOn()
        {
            //short iret = 0;

            //// 轴号
            //UInt16 axis = (UInt16)0;

            //// 转矩值
            //Int32 Torque = (Int32)70;

            //ushort ECATPort = 2;

            //// 从站节点号
            //ushort NodeNum = (ushort)(axis + 1001);

            //ushort index = 24704;   //修改参数6080h（十进制24704）,最大速度限制
            //ushort subindex = 0;

            //// 速度限制
            //int paradata = 100;

            //iret = LTDMC.nmc_set_node_od(cardNo, ECATPort, NodeNum, index, subindex, 32, paradata);

            //// 位置限制启用：
            //// 0 - 限位不起作用  
            //// 1 - 停止在限位位置
            //// 2 - 超出限位停止

            //UInt16 PosLimitValid = (UInt16)0;

            //// 位置限制值
            //double PosLimitValue = 100;

            //// 位置模式：0-相对位置，1-绝对位置
            //UInt16 PosMode = (UInt16)0;

            //iret = LTDMC.nmc_torque_move(cardNo, axis, Torque, PosLimitValid, PosLimitValue, PosMode);
        }

        /// <summary>
        /// 标定
        /// </summary>
        public override void DoWork()
        {
            //try
            //{
            //    // 连接摩尔力设备
            //    ModbusService.GetInstance().ConnectManometer();

            //    // 先记录抬起位置
            //    this.LiftPosition = BondHead.AxisZ.GetRealPosition();

            //    // 测高
            //    (ExcuteResult Ret, double HeightValue) res = this.bondHeadController.MeasureHeight(
            //        this.LiftPosition,
            //        HeightMeasurementFunctionEnum.WithTDSensor);

            //    if (res.Ret == ExcuteResult.Success)
            //    {
            //        // 下压要完全接触传感器，所以下压高度等于测高值加一小段距离
            //        VoiceCoilMotorForceCalibrationPara.GetInstance().PressingDownPosition = res.HeightValue + VoiceCoilMotorForceCalibrationPara.GetInstance().CompensationValue;
            //    }
            //    else
            //    {
            //        XtraMessageBox.Show("Measure Height Fail");
            //        ModbusService.GetInstance().ManometerClient.Disconnect();
            //        return;
            //    }

            //    // 标定前先清空数据
            //    foreach (ForceTorque forceTorque in VoiceCoilMotorForceCalibrationPara.GetInstance().ForceTorqueList)
            //    {
            //        forceTorque.Force = 0.0;
            //    }

            //    MovePara movePara = new MovePara()
            //    {
            //        TargetPosition = VoiceCoilMotorForceCalibrationPara.GetInstance().PressingDownPosition,
            //        Acc = BondHead.AxisZ.AxisMovePara.ACC,
            //        Dec = BondHead.AxisZ.AxisMovePara.DEC,
            //        Jerk = BondHead.AxisZ.AxisMovePara.Jerk,
            //        Vel = 1
            //    };

            //    BondHead.AxisZ.AbsoluteMove(this.LiftPosition);

            //    // 获取力值数据
            //    foreach (ForceTorque forceTorque in VoiceCoilMotorForceCalibrationPara.GetInstance().ForceTorqueList)
            //    {
            //       this.TorqueMotion(forceTorque.Torque);
            //       BondHead.AxisZ.AbsoluteMove(movePara);

            //        // 下压到位后停一段时间稳定力
            //        Thread.Sleep(2000);
            //        //this.holdRegisters = ModbusService.GetInstance().ManometerClient.ReadHoldingRegisters(33, 1);

            //        //Thread.Sleep(2000);
            //        forceTorque.Force = this.holdRegisters[0] / 10.0;

            //        // 记录完后回到抬起位置
            //        BondHead.AxisZ.AbsoluteMove(this.LiftPosition);
            //    }

            //    // 标定完成，保存标定数据
            //    VoiceCoilMotorForceCalibrationPara.GetInstance().Save(VoiceCoilMotorForceCalibrationPara.GetInstance(), VoiceCoilMotorForceCalibrationPara.FilePath);
            //    BondHead.AxisZ.AbsoluteMove(BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z);
            //    //ModbusService.GetInstance().RtuDisConnect();
            //    XtraMessageBox.Show("Calibration completed");
            //}
            //catch (Exception e)
            //{
            //    XtraMessageBox.Show("Calibrate failed", e.Message);
            //}
        }

        /// <summary>
        /// 音圈电机加转矩
        /// </summary>
        /// <param name="torque">转矩</param>
        public void TorqueMotion(double torque)
        {
            // BondHead.VoiceCoilMotor.Force(forceTorque.Torque);
        }
    }
}
