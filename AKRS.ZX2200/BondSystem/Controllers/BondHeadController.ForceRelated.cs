using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.GT;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.ZX2200.BondSystem.BondForce.Modbus;
using AKRS.ZX2200.BondSystem.BondForce.Models.DevicePara;
using AKRS.ZX2200.BondSystem.BondForce.Services;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using GTN;
using LanguageExt;
using log4net.Core;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using static DevExpress.XtraPrinting.Native.ExportOptionsPropertiesNames;
using static GTN.mc;

namespace AKRS.ZX2200.BondSystem.Controllers
{
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Utils;
    //using DevExpress.Diagram.Core.Native.Ribbon;

    /// <summary>
    /// bond force 相关
    /// </summary>
    public partial class BondHeadController
    {
        /// <summary>
        ///  通道号，
        /// 默认用大力通道
        /// </summary>
        private static short channel = 1;

        /// <summary>
        /// 输入源的量程，默认用大力量程
        /// </summary>
        private static double maxPress = ForceConfig.GetInstance().BondheadMaxPress;

        /// <summary>
        ///  读取力控当前模拟量
        /// </summary>
        /// <param name="forceValue">力值</param>
        /// <returns>初始值</returns>
        public double GetBondForceCurrentVal(double forceValue)
        {
            bool isSmallForce = ForceCalibrationService.JudgeIsSmallForce(forceValue);

            return this.GetBondForceCurrentVal(isSmallForce);
        }

        /// <summary>
        ///  读取力控当前模拟量
        /// </summary>
        /// <param name="isSmallForce">是否兄小力</param>
        /// <returns>初始值</returns>
        public double GetBondForceCurrentVal(bool isSmallForce)
        {
            if (this.bondHead.AxisZ.AxisDrive is ETELAxis)
            {
                double read = ModbusService.GetInstance().ReadBondForce()[0] / 10.0;
                return read;
            }
            else
            {
                // 56号机是通道1
                short rtn = GTN.mc.GTN_GetAuAdc(
                    (short)this.bondHead.AxisZ.CardNum,
                    channel,
                    out double pValue,
                    1,
                    out UInt32 p2Clock);

                // 下压之前先读一下模拟量初始值
                double initialValue = Math.Round((pValue / 32767) * maxPress, 8) /** 1000.0*/;

                // kg换成g
                return initialValue * 1000;
            }
        }

        /// <summary>
        ///  读取LVDT模拟量
        /// </summary>
        /// <returns>初始值</returns>
        public double GetLVDTVal()
        {
            short rtn = GTN.mc.GTN_GetAuAdc(
                (short)this.bondHead.AxisZ.CardNum,
                (short)this.bondHead.LVDT.SensorIO,
                out double pValue,
                1,
                out UInt32 p2Clock);

            // 下压之前先读一下模拟量初始值
            double initialValue = Math.Round((pValue / 32767) * ForceConfig.GetInstance().LVDTMaxPress, 8) /** 1000.0*/;

            // kg换成g
            return initialValue * 1000;
        }

        /// <summary>
        /// 力控模式下压
        /// </summary>
        /// <param name="forceValue">实际力值-g</param>
        /// <param name="pos">位置</param>
        /// <param name="speed">速度</param>
        /// <param name="timeOut">超时时间/ms</param>
        /// <param name="slowTravelDistanceBeforeTouch">慢速距离/ms</param>
        /// <exception cref="NullReferenceException">没有获取到指定力配置参数，抛出此异常</exception>
        public void ForceControlSet(double forceValue, double pos, double speed, int timeOut = 5, double slowTravelDistanceBeforeTouch = 0)
        {
            this.CheckZAxisSoftLimit(pos);

            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            // 力值转换,Etel的单位是N，固高的单位是g
            double forceReference;
            double angle = this.GetAxisTRealPos();

            // 判断驱动器类型 
            if (this.bondHead.AxisZ.AxisDrive is ETELAxis)
            {
                double initialValue = this.GetBondForceCurrentVal(false);

                forceReference = ForceCalibrationService.ActualForceToInputForce(forceValue, initialValue, angle);

                // 这里加速度默认是速度的10倍
                this.bondHead.EtelForceControlSet(forceReference, pos, speed, speed * 10.0, timeOut);
            }
            else
            {            
                // 判定是否小力
                bool isSmallforce = ForceCalibrationService.JudgeIsSmallForce(forceValue);

                // 切换通道、设置压力参数
                this.ChangeChannelAndSetForceControlPara(isSmallforce);

                double initialValue = this.GetBondForceCurrentVal(isSmallforce);

                // 出来的单位是N要换算成g,，所以乘以100
                forceReference = ForceCalibrationService.ActualForceToInputForce(forceValue, initialValue, angle) * 100.0;

                this.GTForceControlSet(forceReference, pos, slowTravelDistanceBeforeTouch, isSmallforce);
            }
        }

        /// <summary>
        /// 固高力控下压
        /// 下压之前要先切换通道
        /// </summary>
        /// <param name="forceValue">力值(g)</param>
        /// <param name="pos">位置</param>
        /// <param name="slowTravelBeforeTouchDown">慢速距离</param>
        /// <param name="isSmallForce">是否是小力</param>
        /// <param name="isUseActualInitial">是否使用实时的模拟量残值</param>
        public void GTForceControlSet(
            double forceValue,
            double pos,
            double slowTravelBeforeTouchDown,
            bool isSmallForce = false,
            bool isUseActualInitial = false)
        {
            try
            {
               this.CheckBeforeForceControlSet(forceValue, slowTravelBeforeTouchDown);

                // 探底位置=焊点测高位置-二段速距离-固定值
                // 对应固高demoC点
                double limitPos = pos - slowTravelBeforeTouchDown - ForceConfig.GetInstance().ForceControlDebugBToCDistance;

                double angle = this.GetAxisTRealPos();

                // 模拟量残值
                // 标定时用的是实时值，其他情况使用固定值
                double initialVal = isUseActualInitial ? this.GetBondForceCurrentVal(isSmallForce) : ForceCalibrationService.GetInitialValByAngle(angle);

                //Console.WriteLine($"{initialVal}");

                // 增量
                double increment = forceValue - initialVal;

                // 获取力控配置,输入参数是g
                ForceConfigItem configItem = isSmallForce
                                                 ? ForceConfig.GetInstance().GetSmallForceConfigItem(increment)
                                                 : ForceConfig.GetInstance().GetLargeForceConfigItem(increment);

                short axisNum = (short)this.bondHead.AxisZ.AxisNum;

                //设置压力参数
                short rtn;

                GTN.mc.TAdcConfig adcCfg = new GTN.mc.TAdcConfig();

                // 核号即卡号
                short coreTemp = (short)this.bondHead.AxisZ.CardNum;
                short i = 0;

                // 设定力控闭环所关联的轴
                double[] gearRatio = new double[24];

                // 设置压力闭环的工作空间，超过工作空间，切换到位置闭环。压力闭环模式才有效
                short centerSynchEnable = 0;
                int pressRangeTemp, centerPos = 0;

                GTN.mc.TListInfo listInfoNull = new GTN.mc.TListInfo();
                BondModule bond = new BondModule();

                // 等待指令流结束
                TCommandListStatus pStatus;
                do
                {
                    rtn = GTN.mc.GTN_GetCommandListStatus(coreTemp, 1, out pStatus);
                }
                while (/*pStatus.stopInfo != 10*/pStatus.execute != 0
                && bond.BondAxisX.IsInCommandPosition()
                && bond.BondAxisY.IsInCommandPosition()
                && bond.BondHead.AxisZ.IsInCommandPosition());

                LogHelper.Post(Level.Info, $"画胶指令完成", LogCategory.Dispense, ViewType.InFileAndUI);


                rtn = GuGaoDrive.GTN_GroupStop(coreTemp, 1, ref listInfoNull);
                rtn = GuGaoDrive.GTN_GroupStop(coreTemp, 2, ref listInfoNull);
                rtn = GTN.mc.GTN_StopCommandList(coreTemp, 1, (short)0, ref listInfoNull);
                rtn = GTN.mc.GTN_ClearCommandListData(coreTemp, 1, ref listInfoNull);
                rtn = GTN.mc.GTN_StopCommandList(coreTemp, 2, (short)0, ref listInfoNull);
                rtn = GTN.mc.GTN_ClearCommandListData(coreTemp, 2, ref listInfoNull);
                rtn = GTN.mc.GTN_GroupDisable(coreTemp, 1, ref listInfoNull);
                rtn = GTN.mc.GTN_UngroupAllAxes(coreTemp, 1, ref listInfoNull);
                rtn = GTN.mc.GTN_GroupDisable(coreTemp, 2, ref listInfoNull);
                rtn = GTN.mc.GTN_UngroupAllAxes(coreTemp, 2, ref listInfoNull);

                // 清除力控状态
                rtn = GTN.mc.GTN_ClearPressStatus(coreTemp, axisNum);

                #region 设置力控参数


                listInfoNull.reserve1 = new short[2];
                listInfoNull.reserve2 = new short[3];
                listInfoNull.reserve3 = new double[4];
                listInfoNull.list = 0;
                GTN.mc.TPressLoopControlPrmCs pressLoopPrm = new GTN.mc.TPressLoopControlPrmCs();
                short setPrmType = GTN.mc.SET_PRESS_LOOP_PRM_TYPE_PRESS_LOOP_CONTROL_PRM;
                pressLoopPrm.controlValueReverse = 1;
                rtn = GTN.mc.GTN_SetPressLoopPrm(coreTemp, axisNum, setPrmType, ref pressLoopPrm, ref listInfoNull);

                rtn = GTN.mc.GTN_GetPressRange(coreTemp, axisNum, out centerPos, out pressRangeTemp);
                pressRangeTemp = 50000;//50000
                rtn = GTN.mc.GTN_SetPressRange(coreTemp, axisNum, centerSynchEnable, pressRangeTemp);

                //设定主卡资源个数，MC_AU_ADC --- 非轴模拟量 输入，资源个数为8.
                rtn = GTN.mc.GTN_SetResCount(coreTemp, GTN.mc.MC_AU_ADC, 8);
                //获取模拟量配置参数设定	
                //TAdcConfig adcConfig = new TAdcConfig();
                rtn = GTN.mc.GTN_GetAuAdcConfig(coreTemp, /*1*/channel, out adcCfg);
                short adc = GTN.mc.GTN_GetAdcValue(coreTemp,  /*1*/channel, out short pValue, 1, out uint pClock);
                adcCfg.a = 1;
                adcCfg.b = pValue;
                //关联模拟量输入和力矩闭环的实际力矩值，这里可能需要通过查表来计算实际电流的值比较靠谱，因为模拟量是
                //会有波动的。
                rtn = GTN.mc.GTN_SetAuAdcConfig(coreTemp,  /*1*/channel, ref adcCfg);
                GTN.mc.TPressPrm pressPrm = new GTN.mc.TPressPrm();
                rtn = GTN.mc.GTN_GetPressPrm(coreTemp, axisNum, out pressPrm);
                pressPrm.active = 1;// 压力控制功能打开
                pressPrm.scale = 1;// 保留，物理单位到数子量的脉冲当量
                pressPrm.linkAxis = axisNum;  // 映射力矩控制的关联轴
                rtn = GTN.mc.GTN_SetPressPrm(coreTemp, axisNum, ref pressPrm);
                // 压力控制反馈来源于第1路AU_ADC
                rtn = GTN.mc.GTN_GetPressFeedbackType(coreTemp, axisNum, out short pressFbType, out short pressFbIndex);
                pressFbType = GTN.mc.MC_AU_ADC;

                pressFbIndex = /*1*/channel;//模拟量输入的第一通道
                rtn = GTN.mc.GTN_SetPressFeedbackType(coreTemp, axisNum, pressFbType, pressFbIndex);

                // 规划压力清零
                rtn = GTN.mc.GTN_SetPrfPress(coreTemp, axisNum, 0);

                //设置闭环压力的PID参数
                GTN.mc.TPressPid pressPid, pressPid1;
                rtn = GTN.mc.GTN_GetPressPid(coreTemp, axisNum, out pressPid);
                pressPid.kp = configItem.Kp;
                pressPid.ki = configItem.Ki;
                pressPid.kd = configItem.Kd;
                pressPid.kvff = 0;
                pressPid.kaff = 0;
                pressPid.derivativeLimit = 10000;
                pressPid.integralLimit = 10000;
                pressPid.limitMax = 20000;// 控制器输出最大值
                pressPid.limitMin = -20000;// 控制器输出最小值
                rtn = GTN.mc.GTN_SetPressPid(coreTemp, axisNum, ref pressPid);
                rtn = GTN.mc.GTN_GetPressPid(coreTemp, axisNum, out pressPid1);

                // 模拟量初始值
                //double initialValue = this.GetBondForceCurrentVal(isSmallForce);

                // 设置压力保护参数，位置闭环模式、压力闭环模式，保护都有效
                // 这个是切换力值，要从标定数据中获取
                // 大力小力的算法不一样
                //double modelChange = isSmallForce == false
                //                         ? ForceCalibrationService.GetForceModelChange(forceValue, configItem)
                //                         : ForceCalibrationService.GetSmallForceModelChange(forceValue, initialValue);

                double modelChange = ForceCalibrationService.GetForceModelChange(forceValue, configItem);

                if (modelChange < 0)
                {
                    string info = isSmallForce ? "LVDT" : "应变片";
                    throw new Exception($"力控切换阈值为负数，请检查{info}模拟量是否为负数！");
                }


                rtn = GTN.mc.GTN_GetPressAutoSwitchPrm(coreTemp, axisNum, out GTN.mc.TPressAutoSwitchPrm autoSwitchPrm0);
                autoSwitchPrm0.limit1 = (short)(32767 / maxPress * modelChange - 5);
                autoSwitchPrm0.limit2 = (short)(32767 / maxPress * modelChange);
                autoSwitchPrm0.time = 2000;//爬升时间
                autoSwitchPrm0.triggerCondition = 0;// PRESS_GREATER_THAN_LIMIT--大于等于;
                autoSwitchPrm0.loopMode = 1;//力矩闭环轴的闭环模式：位置闭环模式=0 & 力矩闭环模式=1
                                            //力矩闭环模式下，力矩升压模式：NONE（0）--保持；TARGET（1）--单斜线升压模式；ARRAY（2）--数组斜线升压模式

                autoSwitchPrm0.pressProfileMode = 1;// GTN.mc.PRESS_PROFILE_TARGET;//单斜线升压模
                rtn = GTN.mc.GTN_SetPressAutoSwitchPrm(2, axisNum, ref autoSwitchPrm0);

                //使能力位切换功能
                // rtn = GTN.mc.GTN_PressAutoSwitchEnable(2, axisNum, 1);//使能力位混合控制

                GTN.mc.TPressLimit pressLimit = new GTN.mc.TPressLimit();
                rtn = GTN.mc.GTN_GetLimit(coreTemp, axisNum, GTN.mc.MC_PRESS, out pressLimit);


                pressLimit.limit1 = 29000;   // 超过Limit1经过time个周期停止 
                pressLimit.limit2 = 30000;   // 超过Limit2马上停止 
                pressLimit.time = 7000;    // Limit2必须大于Limit1，否则返回7 

                pressLimit.triggerCondition = GTN.mc.PRESS_GREATER_THAN_LIMIT; // Limit2必须大于Limit1，否则返回7 
                rtn = GTN.mc.GTN_SetLimit(coreTemp, axisNum, GTN.mc.MC_PRESS, ref pressLimit);

                rtn = GTN.mc.GTN_LmtsOn(coreTemp, axisNum, GTN.mc.MC_PRESS);

                GTN.mc.TTorqueLimit lpressLimit = new GTN.mc.TTorqueLimit();
                // rtn = GTN.mc.GTN_GetLimit(coreTemp, axisNum, GTN.mc.MC_TORQUE, out lpressLimit);

                lpressLimit.limit1 = 29000;   // 超过Limit1经过time个周期停止 
                lpressLimit.limit2 = 30000;   // 超过Limit2马上停止 
                lpressLimit.time = 7000;    // Limit2必须大于Limit1，否则返回7 

                rtn = GTN.mc.GTN_SetLimit(coreTemp, axisNum, GTN.mc.MC_TORQUE, ref lpressLimit);
                rtn = GTN.mc.GTN_LmtsOn(coreTemp, axisNum, GTN.mc.MC_TORQUE);

                #endregion

                // 设置目标压力
                GTN.mc.TPressTargetPrm pressTargetPrm = new GTN.mc.TPressTargetPrm();

                // 单位是kg
                double pressTarget = forceValue * 32.767 / maxPress;
                short listTemp, axisTemp;

                // 规划压力清零
                rtn = GTN.mc.GTN_SetPrfPress(coreTemp, axisNum, 0);

                // rtn = GTN.mc.GTN_GetPressTarget(coreTemp, 4, out pressTarget, out pressTargetPrm);
                //pressTarget = Pressure;
                pressTargetPrm.acc = configItem.Acc;
                pressTargetPrm.dec = configItem.Dec;
                pressTargetPrm.smoothTime = configItem.SmoothTime;
                pressTargetPrm.pressStart = 0;
                rtn = GTN.mc.GTN_SetPressTarget(coreTemp, axisNum, pressTarget, ref pressTargetPrm);//目标压力
                                                                                                    //rtn = GTN.mc.GTN_GetPressTarget(coreTemp, axisNum, out pressTarget, out pressTargetPrm);

                                                                                                    // 重新设置切换阈值
                // adcCfg = new GTN.mc.TAdcConfig();

                //coreTemp = 2;
                listTemp = 1;

                // 设定力控闭环所关联的轴
                //double velMax, accMax, jerkMax;

                // 设置压力保护参数，位置闭环模式、压力闭环模式，保护都有效
                rtn = GTN.mc.GTN_GetPressAutoSwitchPrm(coreTemp, axisNum, out GTN.mc.TPressAutoSwitchPrm autoSwitchPrm);
                autoSwitchPrm.limit1 = (short)(32767 / maxPress * modelChange - 5);
                autoSwitchPrm.limit2 = (short)(32767 / maxPress * modelChange);
                autoSwitchPrm.time = 3000;//爬升时间ms
                autoSwitchPrm.triggerCondition = 0;// PRESS_GREATER_THAN_LIMIT--大于等于;
                autoSwitchPrm.loopMode = 1;//力矩闭环轴的闭环模式：位置闭环模式=0 & 力矩闭环模式=1
                                           //力矩闭环模式下，力矩升压模式：NONE（0）--保持；TARGET（1）--单斜线升压模式；ARRAY（2）--数组斜线升压模式

                autoSwitchPrm0.pressProfileMode = 1;// GTN.mc.PRESS_PROFILE_TARGET;//单斜线升压模
                rtn = GTN.mc.GTN_SetPressAutoSwitchPrm(coreTemp, axisNum, ref autoSwitchPrm);

                // 使能力位切换功能
                //rtn = GTN.mc.GTN_PressAutoSwitchEnable(coreTemp, axisNum, 1);//使能力位混合控制


                GTN.mc.TAxisMotionConstraint[] axisMotionConstraint = new GTN.mc.TAxisMotionConstraint[24];
                GTN.mc.TListInfo listInfo = new GTN.mc.TListInfo();

                listInfo.reserve1 = new short[2];
                listInfo.reserve2 = new short[3];
                listInfo.reserve3 = new double[4];
                listInfo.reserve1 = new short[2];
                listInfo.reserve2 = new short[3];
                listInfo.reserve3 = new double[4];
                listInfoNull.reserve1 = new short[2];
                listInfoNull.reserve2 = new short[3];
                listInfoNull.reserve3 = new double[4];
                GTN.mc.TMoveContinuousPrmsmooth moveContinuousAbsolutePrm = new GTN.mc.TMoveContinuousPrmsmooth();
                GTN.mc.TVelprofileModeSmooth smooth = new GTN.mc.TVelprofileModeSmooth();

                // 声明结构体数组长度
                GTN.mc.TProfileScale[] scale = new GTN.mc.TProfileScale[24];
                GTN.mc.TProfileScale[] scaleRead = new GTN.mc.TProfileScale[24];
                GTN.mc.TWaitTimeout timeout = new GTN.mc.TWaitTimeout();

                //velMax = 2000;
                //accMax = 10000;
                //jerkMax = 100000;
                // 1、基本轴初始化
                short j = (short)(axisNum - 1);
                {
                    scale[j].alpha = new double[4];
                    scale[j].beta = new double[4];
                    scale[j].reverse1 = new short[3];

                    scale[j].count = 2;
                    scale[j].alpha[0] = 1;                          // 脉冲当量，alpha可以认为是mm的单位，beta是脉冲的单位。beta / alpha
                    scale[j].beta[0] = 10000;
                    scale[j].alpha[1] = 1;
                    scale[j].beta[1] = 1;

                    listInfoNull.modal = 0;

                    // 设置脉冲当量，毫米 对应的 脉冲数
                    rtn = GTN.mc.GTN_SetAxisScale(coreTemp, Convert.ToInt16(j + 1), ref scale[j], ref listInfoNull);
                    // rtn = GTN.mc.GTN_GetAxisScale(coreTemp, Convert.ToInt16(j + 1), out scaleRead[j]);

                    //axisMotionConstraint[j].reserve1 = new short[3];
                    //axisMotionConstraint[j].reserve2 = new double[8];
                    //axisMotionConstraint[j].velMax = velMax;        // 单位：mm/s   或者 度/s
                    //axisMotionConstraint[j].accMax = accMax;        // 单位：mm/s^2 或者 度/s^2
                    //axisMotionConstraint[j].decMax = accMax;        // 单位：mm/s^2 或者 度/s^2
                    //axisMotionConstraint[j].jerkMax = jerkMax;      // 单位：mm/s^3 或者 度/s^3
                    //axisMotionConstraint[j].dvMax = 10;             // 单位：mm/s   或者 度/s   轴的最大速度跳变量
                    //rtn = GTN.mc.GTN_SetAxisMotionConstraint(coreTemp, Convert.ToInt16(j + 1), ref axisMotionConstraint[j], ref listInfoNull);
                    //rtn = GTN.mc.GTN_GetAxisMotionConstraint(coreTemp, Convert.ToInt16(j + 1), out axisMotionConstraint[j]);

                    // 设置Z 的平滑时间，否则在插补的时候Z会响动
                    short ret = mc.GTN_SetAxisMotionSmooth((short)this.bondHead.AxisZ.CardNum, (short)this.bondHead.AxisZ.AxisNum, 0, 0);

                    if (ret != 0)
                    {
                        throw new Exception();
                    }
                }

                listInfo.modal = 1;
                listInfo.segNum = 0;
                listInfo.list = 1;
                //rtn = GTN.mc.GTN_StopCommandList(coreTemp, 1, 0, ref listInfo);
                //rtn = GTN.mc.GTN_ClearCommandListData(coreTemp, listTemp, ref listInfoNull);

              //  rtn = GTN.mc.GTN_StopCommandList(coreTemp, 1, 0, ref listInfoNull);
              //  rtn = GTN.mc.GTN_ClearCommandListData(coreTemp, listTemp, ref listInfoNull);

                //rtn = GTN.mc.GTN_UngroupAllAxes(coreTemp, 1, ref listInfoNull);
                //rtn = GTN.mc.GTN_GroupDisable(coreTemp, 1, ref listInfoNull);

                mc.TCommandListStatus tc;

                rtn = GTN.mc.GTN_GetCommandListStatus(coreTemp, listTemp, out tc);

                System2RunTimeProvider.RecordTime("固高力控", $"力控信息为{tc.stopInfo}");

                smooth.reserve = new double[18];

                // 新增打开力控模式
                GTN.mc.TPressLoopPrmUnion switchEnable = new mc.TPressLoopPrmUnion();
                switchEnable.loopSwitchEnable.enable = 1;
                listInfo.modal = 1;
                listInfo.segNum++;
                listInfo.list = 1;
                rtn = GTN.mc.GTN_SetPressLoopPrm(coreTemp, axisNum, 3, ref switchEnable, ref listInfo);

                // B-C
                // 低速探测，最大速度为velOffsetTemp / 2，终点速度为0
                listInfo.modal = 1;
                listInfo.segNum++;

                // BC加速度
                moveContinuousAbsolutePrm.acc = configItem.SlowTouchAcc /*500*/;
                moveContinuousAbsolutePrm.dec = configItem.SlowTouchDec /* 500*/;
                moveContinuousAbsolutePrm.overrideSelect = 0;
                moveContinuousAbsolutePrm.velProfileMode = GTN.mc.VEL_PROFILE_MODE_TRAP;//平滑模式  

                moveContinuousAbsolutePrm.vel = configItem.LowSpeedDuringTouchDown;//BCSpeed
                moveContinuousAbsolutePrm.velEnd = 0;

                // C点
                moveContinuousAbsolutePrm.pos = limitPos;

                // B-C
                rtn = GTN.mc.GTN_MoveContinuousAbsolute(coreTemp, axisNum, ref moveContinuousAbsolutePrm, ref listInfo, 0); //第二段慢速

                // 3 等待力矩到位
                short conditionCount = 2;
                short operation = 0;//GTN.mc.WATCH_OPERATION_AND;  // WATCH_OPERATION_AND两个条件相与，WATCH_OPERATION_OR相或
                GTN.mc.TWatchCondition[] condition = new GTN.mc.TWatchCondition[2];
                condition[0].var.type = 61102;      // 压力闭环信息矩到位变量
                condition[0].var.index = 1;         // 压力闭环信息中的环路模式信息
                condition[0].var.id = Convert.ToUInt16(axisNum);   // 第4组压力闭环中的环路模式信息为LOOP_MODE_PRESS
                condition[0].condition = (ushort)GTN.mc.WATCH_CONDITION_EQ;
                condition[0].value = GTN.mc.LOOP_MODE_PRESS;

                condition[1].var.type = 61102;      // 压力闭环信息矩到位变量
                condition[1].var.index = 2;         // 压力闭环信息中的压力规划完成信息
                condition[1].var.id = Convert.ToUInt16(axisNum);   // 第4组压力闭环中的环路模式信息为LOOP_MODE_PRESS//和前面pressaxis对应，一共只有6个
                condition[1].condition = (ushort)GTN.mc.WATCH_CONDITION_EQ;
                condition[1].value = 0;
                listInfo.modal = 1;// 模态的
                                   // timeout.mode = 0; // 超时后停止指令流
                listInfo.segNum++;

                rtn = GTN.mc.GTN_WaitForCondition(coreTemp, ref condition[0], conditionCount, operation, ref timeout, ref listInfo);

                Stopwatch sw = Stopwatch.StartNew();

                do
                {
                    rtn = GTN.mc.GTN_CommandListDataEnd(coreTemp, listTemp);
                    // rtn = GuGaoDrive.GTN_PrintCommandInfo(2, "测试.txt", 0, 0);
                    // rtn = GTN.mc.GTN_GetLastCommandError(coreTemp,out TCommandInfoData data,-1 ,100);

                    if (sw.Elapsed.Seconds > 20)
                    {
                        //throw new Exception($"固高：{forceValue}g力等指令流压入控制器超时！");
                    }
                } while (0 != rtn);
                GTN.mc.TCommandListStatus stat;

                // 指令流开始
                listInfo.list = 0;
                rtn = GTN.mc.GTN_StartCommandList(coreTemp, listTemp, ref listInfo);

                GTN.mc.TCommandListStatus Stat = new GTN.mc.TCommandListStatus();
                short sRtn;

                string excuteMessage = string.Empty;

                sw.Restart();
                do
                {
                    sRtn = GTN.mc.GTN_GetCommandListStatus(coreTemp, 1, out Stat);

                    if (sw.Elapsed.Seconds > 20)
                    {
                        int pstatus;
                        GTN.mc.GTN_GetPressStatus(coreTemp, axisNum, out pstatus);
                        excuteMessage = $"GTForceControlSet 固高力控：{forceValue}g力等指令流执行结束超时！GTN_GetPressStatus状态：{pstatus}";
                        break;
                    }
                }
                while (0 != Stat.execute); //等待指令流执行结束

                System2RunTimeProvider.RecordTime("GT力控", "等待指令流执行结束完成");

                rtn = GTN.mc.GTN_StopCommandList(coreTemp, 1, 0, ref listInfo);
                sRtn = GTN.mc.GTN_GetCommandListStatus(coreTemp, 1, out Stat);
                rtn = GTN.mc.GTN_ClearCommandListData(coreTemp, listTemp, ref listInfoNull);
                sRtn = GTN.mc.GTN_GetCommandListStatus(coreTemp, 1, out Stat);

                if (!string.IsNullOrEmpty(excuteMessage))
                {
                    Thread.Sleep(10);
                    this.GTForceControlReset(this.bondHeadParam.AxisSafePos.Z, 50);
                    throw new Exception(excuteMessage);
                }

                System2RunTimeProvider.RecordTime("GT力控", "清除指令流完成");
            }
            catch (Exception ex)
            {
                // 关闭力控
                this.CloseForceControl();

                LogHelper.Post(Level.Error, $"固高力控下压失败!" + ex.ToString(), ex, LogCategory.Bond);
                throw ex;
            }
        }

        /// <summary>
        ///  力控下压前检查
        /// </summary>
        /// <param name="forceValue">力控模拟量</param>
        /// <param name="slowTravelBeforeTouchDown">二段距离</param>
        /// <exception cref="Exception">异常</exception>
        private void CheckBeforeForceControlSet(double forceValue, double slowTravelBeforeTouchDown)
        {
            if (forceValue <= 0)
            {
                this.MoveBondZToSafePos();

                throw new Exception("输入模拟量小于0，力控下压失败！");
            }

            double distance = ForceConfig.GetInstance().ForceControlDebugBToCDistance;

            if (distance <= 0)
            {
                this.MoveBondZToSafePos();

                throw new Exception($"固高力控B点到C点距离：{distance} 为负，力控下压失败！");
            }

            if (slowTravelBeforeTouchDown < 0)
            {
                this.MoveBondZToSafePos();

                throw new Exception($"固高力控二段速距离为负值：{slowTravelBeforeTouchDown} ，力控下压失败！");
            }
        }

        /// <summary>
        /// 固高退出力控
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="speed">速度</param>
        public void GTForceControlReset(double pos, double speed)
        {
            string excuteMessage = string.Empty;

            try
            {
                short rtn;
                short axisNum = (short)this.bondHead.AxisZ.AxisNum;

                // 核号即卡号
                short coreTemp = (short)this.bondHead.AxisZ.CardNum;

                short listTemp;
                listTemp = 1;

                GTN.mc.TAxisMotionConstraint[] axisMotionConstraint = new GTN.mc.TAxisMotionConstraint[24];
                GTN.mc.TListInfo listInfo = new GTN.mc.TListInfo();
                GTN.mc.TListInfo listInfoNull = new GTN.mc.TListInfo();
                listInfo.reserve1 = new short[2];
                listInfo.reserve2 = new short[3];
                listInfo.reserve3 = new double[4];
                listInfo.reserve1 = new short[2];
                listInfo.reserve2 = new short[3];
                listInfo.reserve3 = new double[4];
                listInfoNull.reserve1 = new short[2];
                listInfoNull.reserve2 = new short[3];
                listInfoNull.reserve3 = new double[4];

                GTN.mc.TMoveContinuousPrmsmooth moveContinuousAbsolutePrm = new GTN.mc.TMoveContinuousPrmsmooth();
                GTN.mc.TVelprofileModeSmooth smooth = new GTN.mc.TVelprofileModeSmooth();

                listInfo.list = 1;
                listInfo.segNum = 0;

                //声明结构体数组长度
                GTN.mc.TProfileScale[] scale = new GTN.mc.TProfileScale[24];
                GTN.mc.TProfileScale[] scaleRead = new GTN.mc.TProfileScale[24];
                GTN.mc.TWaitTimeout timeout = new GTN.mc.TWaitTimeout();

                double velMax = 2000;
                double accMax = 10000;
                double jerkMax = 100000;
                // 1、基本轴初始化
                short j = (short)(axisNum - 1);
                {
                    scale[j].alpha = new double[4];
                    scale[j].beta = new double[4];
                    scale[j].reverse1 = new short[3];

                    scale[j].count = 2;
                    scale[j].alpha[0] = 1; // 脉冲当量，alpha可以认为是mm的单位，beta是脉冲的单位。beta / alpha
                    scale[j].beta[0] = 10000;
                    scale[j].alpha[1] = 1;
                    scale[j].beta[1] = 1;

                    // 设置脉冲当量，毫米 对应的 脉冲数
                    rtn = GTN.mc.GTN_SetAxisScale(coreTemp, Convert.ToInt16(j + 1), ref scale[j], ref listInfoNull);
                    rtn = GTN.mc.GTN_GetAxisScale(coreTemp, Convert.ToInt16(j + 1), out scaleRead[j]);
                    axisMotionConstraint[j].reserve1 = new short[3];
                    axisMotionConstraint[j].reserve2 = new double[8];
                    axisMotionConstraint[j].velMax = velMax; // 单位：mm/s   或者 度/s
                    axisMotionConstraint[j].accMax = accMax; // 单位：mm/s^2 或者 度/s^2
                    axisMotionConstraint[j].decMax = accMax; // 单位：mm/s^2 或者 度/s^2
                    axisMotionConstraint[j].jerkMax = jerkMax; // 单位：mm/s^3 或者 度/s^3
                    axisMotionConstraint[j].dvMax = 10; // 单位：mm/s   或者 度/s   轴的最大速度跳变量
                    rtn = GTN.mc.GTN_SetAxisMotionConstraint(
                        coreTemp,
                        Convert.ToInt16(j + 1),
                        ref axisMotionConstraint[j],
                        ref listInfoNull);
                    rtn = GTN.mc.GTN_GetAxisMotionConstraint(
                        coreTemp,
                        Convert.ToInt16(j + 1),
                        out axisMotionConstraint[j]);
                }

                rtn = GTN.mc.GTN_StopCommandList(coreTemp, 1, 0, ref listInfo);
                rtn = GTN.mc.GTN_ClearCommandListData(coreTemp, listTemp, ref listInfoNull);

                // 切换到位置闭环，
                GTN.mc.TPressLoopPrmUnion pressLoopPrmUnion = new GTN.mc.TPressLoopPrmUnion();

                pressLoopPrmUnion.loopModeSwitch.loopMode = GTN.mc.LOOP_MODE_POSITION;
                //istInfo.segNum++;
                //listInfo.modal = 1;

                // 单独执行，不能加入指令流，2025/05/10 刘江宪该
                rtn = GTN.mc.GTN_SetPressLoopPrm(
                    coreTemp,
                    axisNum,
                    GTN.mc.SET_PRESS_LOOP_PRM_TYPE_LOOP_MODE_SWITCH,
                    ref pressLoopPrmUnion,
                    ref listInfoNull);

                // 判断压力状态
                int pstatus0;
                GTN.mc.GTN_GetPressStatus(coreTemp, axisNum, out pstatus0);
                int eightBit = (pstatus0 >> 7) & 1;
                if (eightBit == 1)
                {
                    System2RunTimeProvider.RecordTime("固高力控", $"力控抬起切换到位置环失败，将再次切换");
                    Thread.Sleep(20);
                    rtn = GTN.mc.GTN_SetPressLoopPrm(
                        coreTemp,
                        axisNum,
                        GTN.mc.SET_PRESS_LOOP_PRM_TYPE_LOOP_MODE_SWITCH,
                        ref pressLoopPrmUnion,
                        ref listInfoNull);
                }

                // 最大速度为velMax，终点速度为 0，高速返回
                // 减速度和加速度默认*10
                moveContinuousAbsolutePrm.acc = speed * 10;
                moveContinuousAbsolutePrm.dec = speed * 10;
                moveContinuousAbsolutePrm.overrideSelect = 0;
                moveContinuousAbsolutePrm.velProfileMode = GTN.mc.VEL_PROFILE_MODE_TRAP; //平滑模式  

                // BCSpeed
                moveContinuousAbsolutePrm.vel = speed;
                moveContinuousAbsolutePrm.velEnd = 0;
                moveContinuousAbsolutePrm.pos = pos;
                listInfo.modal = 0;
                listInfo.segNum++;
                rtn = GTN.mc.GTN_MoveContinuousAbsolute(
                    coreTemp,
                    axisNum,
                    ref moveContinuousAbsolutePrm,
                    ref listInfo,
                    0);

                Stopwatch sw = Stopwatch.StartNew();

                do
                {
                    rtn = GTN.mc.GTN_CommandListDataEnd(coreTemp, listTemp);

                    if (sw.Elapsed.Seconds > 20)
                    {
                        throw new Exception("固高：等指令流压入控制器超时！");
                    }
                }
                while (0 != rtn);

                // 指令流开始
                listInfo.list = 0;
                rtn = GTN.mc.GTN_StartCommandList(coreTemp, listTemp, ref listInfo);
                Thread.Sleep(10);

                GTN.mc.TCommandListStatus stat = new GTN.mc.TCommandListStatus();

                sw.Restart();

                // 等待指令流执行结束,轴到位
                do
                {
                    GTN.mc.GTN_GetCommandListStatus(coreTemp, 1, out stat);

                    if (sw.Elapsed.Seconds > 20)
                    {
                        int pstatus;
                        GTN.mc.GTN_GetPressStatus(coreTemp, axisNum, out pstatus);
                        excuteMessage = $"固高力控：等指令流执行结束超时！GTN_GetPressStatus:{pstatus}";
                        break;
                    }
                }
                while (0 != stat.execute || this.bondHead.AxisZ.IsInRealPosition() == false
                      /* Math.Abs(this.bondHead.AxisZ.GetRealPosition() - pos) > 0.01*/);

                // 设置Z 的平滑时间，否则在插补的时候Z会响动,不然会影响插补
                ((GTAxis)this.bondHead.AxisZ.AxisDrive).SetMotionSmooth(30, 15);

                rtn = GTN.mc.GTN_LmtsOff(coreTemp, axisNum, GTN.mc.MC_PRESS);
                rtn = GTN.mc.GTN_LmtsOff(coreTemp, axisNum, GTN.mc.MC_TORQUE);

                if (!string.IsNullOrEmpty(excuteMessage))
                {
                    throw new Exception(excuteMessage);
                }

                double curPos = this.GetAxisZRealPos();

                if (Math.Abs(curPos - pos) > 3)
                {
                    int pstatus;
                    rtn = GTN.mc.GTN_GetPressStatus(coreTemp, axisNum, out pstatus);
                    rtn = GTN.mc.GTN_GetSts(coreTemp, axisNum, out int Axistate, 1, out uint CLOCK);
                  
                    throw new Exception($"力控抬起目标位和实际位置相差过大，目标位:{pos},实际位置：{curPos},GTN_GetPressStatus:{pstatus},GTN_GetPressStatus:{Axistate}");
                }

                System2RunTimeProvider.RecordTime("固高力控",$"力控抬起目标位:{pos},实际位置：{curPos}");
            }
            catch (Exception ex)
            {
                excuteMessage += ex.ToString();
                LogHelper.Post(Level.Error, $"固高退出力控失败：" + excuteMessage, ex, LogCategory.Bond);
            }
            finally
            {
                // 关闭力控
                this.CloseForceControl();
            }
        }

        /// <summary>
        /// 力控模式下轴抬起
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="speed">速度</param>
        /// <param name="timeOut">力控超时时间ms</param>
        public void ForceControlReset(double pos, double speed, int timeOut = 10000)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            this.CheckZAxisSoftLimit(pos);

            // 机器速度百分比
            // 这里不需要不然二段速上抬会超时
            double vel = speed;

            int option = this.bondHeadParam.ForceResetMode;

            // 判断驱动器类型 
            if (this.bondHead.AxisZ.AxisDrive is ETELAxis)
            {
                // 这里加速度默认是速度的10倍
                this.bondHead.EtelForceControlReset(pos, vel, speed * 10, option, timeOut);
            }
            else
            {
                this.GTForceControlReset(pos, vel);
            }
        }

        /// <summary>
        /// 焊头压力表清零
        /// </summary>
        /// <returns>结果</returns>
        public bool ResetBondhead()
        {
            try
            {
                // 判断清零方式 
                if (ForceConfig.GetInstance().ForceZeroMode == ForceZeroModeEnum.Modbus) 
                {
                    // 焊头压力表清零
                    ModbusService.GetInstance().ResetBondhead();
                }
                else
                {
                    this.bondHead.BondHeadForceResetZeroElectric.SetOutputValue(true);

                    // 5号机不加延时清不掉
                    Thread.Sleep(30);
                    this.bondHead.BondHeadForceResetZeroElectric.SetOutputValue(false);
                }

                return true;
            }
            catch (Exception e)
            {
                LogHelper.Post(
                    Level.Error,
                    $"焊头力控清零失败!\r\n{e.ToString()}",
                    LogCategory.Hardware,
                    ViewType.InFileAndUI);

                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"焊头力控清零失败!\r\n{e.ToString()}",
                    "报警",
                    new string[] { "重试", "忽略", "终止" },
                    new DialogResult[] { DialogResult.Retry, DialogResult.Ignore, DialogResult.Abort },
                    AlarmLevel.FirstLevel);

                switch (dialogResult)
                {
                    case DialogResult.Retry:

                        this.ResetBondhead();

                        return true;

                    case DialogResult.Ignore:
                        return true;

                    case DialogResult.Abort:

                        Machine.GetInstance().Stop();

                        return false;

                    default:
                        return false;
                }
            }
        }

        /// <summary>
        ///  读LVDT值
        /// </summary>
        /// <returns>值</returns>
        public double ReadLVDT()
        {
            return this.bondHead.LVDT.ReadTxPDO((ushort)this.bondHead.LVDT.SensorIO, 1);
        }

        /// <summary>
        /// 切换力控通道
        /// </summary>
        /// <param name="isSmallForce">是否是小力</param>
        private void ChangeForceControlChannel(bool isSmallForce)
        {
            // 模拟量读取通道号
            channel = isSmallForce ? (short)this.bondHead.LVDT.SensorIO : (short)this.bondHead.ReadBondheadForce.SensorIO;

            // 压力表量程
            maxPress = isSmallForce
                           ? ForceConfig.GetInstance().LVDTMaxPress
                           : ForceConfig.GetInstance().BondheadMaxPress; // 单位是kg
        }

        /// <summary>
        /// 设置压力参数
        /// </summary>
        private void SetForceControlPara()
        {
            //设置压力参数
            short rtn, coreTemp,axis;
            // GTN.mc .TAdcConfig adcCfg = new GTN.mc.TAdcConfig();

            // 核号即卡号
            coreTemp = (short)this.bondHead.AxisZ.CardNum;
            axis = (short)this.bondHead.AxisZ.AxisNum;

            short i = 0;
            // rtn = GTN_SetPrfPress(coreTemp, (short)this.AxisZ.AxisNum, 0);
            // 设定力控闭环所关联的轴
            double[] gearRatio = new double[24];

            // 设置压力闭环的工作空间，超过工作空间，切换到位置闭环。压力闭环模式才有效
            short centerSynchEnable = 0;
            int pressRangeTemp, centerPos;
            rtn = GTN.mc.GTN_GetPressRange(coreTemp, axis, out centerPos, out pressRangeTemp);
            pressRangeTemp = 50000;//50000
            rtn = GTN.mc.GTN_SetPressRange(coreTemp, axis, centerSynchEnable, pressRangeTemp);

            //设定主卡资源个数，MC_AU_ADC --- 非轴模拟量 输入，资源个数为1.
            rtn = GTN.mc.GTN_SetResCount(coreTemp, GTN.mc.MC_AU_ADC, 8);

            //获取模拟量配置参数设定	
            //TAdcConfig adcConfig = new TAdcConfig();
            GTN.mc.TAdcConfig adcCfg = new GTN.mc.TAdcConfig();
            //rtn = GTN.mc.GTN_GetAuAdcConfig(2, 1, out adcCfg);
            //short adc = GTN.mc.GTN_GetAdcValue(2, 1, out short pValue, 1, out uint pClock);
            rtn = GTN.mc.GTN_GetAuAdcConfig(coreTemp, channel, out adcCfg);
            short adc = GTN.mc.GTN_GetAdcValue(coreTemp, channel, out short pValue, 1, out uint pClock);

            adcCfg.a = 1;
            adcCfg.b = pValue;
            //关联模拟量输入和力矩闭环的实际力矩值，这里可能需要通过查表来计算实际电流的值比较靠谱，因为模拟量是
            //会有波动的。
            // rtn = GTN.mc.GTN_SetAuAdcConfig(2, 1, ref adcCfg);
            rtn = GTN.mc.GTN_SetAuAdcConfig(coreTemp, channel, ref adcCfg);
            GTN.mc.TPressPrm pressPrm = new GTN.mc.TPressPrm();
            rtn = GTN.mc.GTN_GetPressPrm(coreTemp, axis, out pressPrm);
            pressPrm.active = 1;// 压力控制功能打开
            pressPrm.scale = 1;// 保留，物理单位到数子量的脉冲当量
            pressPrm.linkAxis = axis;  // 映射力矩控制的关联轴
            rtn = GTN.mc.GTN_SetPressPrm(coreTemp, axis, ref pressPrm);
            // 压力控制反馈来源于第1路AU_ADC
            rtn = GTN.mc.GTN_GetPressFeedbackType(coreTemp, axis, out short pressFbType, out short pressFbIndex);
            pressFbType = GTN.mc.MC_AU_ADC;
            //临时注掉  压力传感器的通道号
            // pressFbIndex = 2;
            pressFbIndex = channel;//模拟量输入的第一通道
            rtn = GTN.mc.GTN_SetPressFeedbackType(coreTemp, axis, pressFbType, pressFbIndex);
        }

        /// <summary>
        /// 切换力控通道并设置压力参数
        /// </summary>
        /// <param name="isSmallForce">是否是小力</param>
        public void ChangeChannelAndSetForceControlPara(bool isSmallForce)
        {
            // 判断驱动器类型 
            if (this.bondHead.AxisZ.AxisDrive is GTAxis)
            {
                this.ChangeForceControlChannel(isSmallForce);

                this.SetForceControlPara();
            }
        }

        /// <summary>
        /// 关闭力控
        /// </summary>
        public void CloseForceControl()
        {
            GTN.mc.TListInfo listInfo = new GTN.mc.TListInfo();
            GTN.mc.TListInfo listInfoNull = new GTN.mc.TListInfo();
            listInfo.reserve1 = new short[2];
            listInfo.reserve2 = new short[3];
            listInfo.reserve3 = new double[4];
            listInfo.reserve1 = new short[2];
            listInfo.reserve2 = new short[3];
            listInfo.reserve3 = new double[4];
            listInfoNull.reserve1 = new short[2];
            listInfoNull.reserve2 = new short[3];
            listInfoNull.reserve3 = new double[4];

            short rtn;

            // 核号即卡号
            short coreTemp = (short)this.bondHead.AxisZ.CardNum;
            short axis = (short)this.bondHead.AxisZ.AxisNum;

            GTN.mc.TPressLoopPrmUnion switchEnable = new mc.TPressLoopPrmUnion();
            switchEnable.loopSwitchEnable.enable = 0;

            listInfo.list = 0;

            rtn = GTN.mc.GTN_SetPressLoopPrm(coreTemp, axis, 3, ref switchEnable, ref listInfo);
        }

        /// <summary>
        /// 焊头清零
        /// </summary>
        public void ZeroBondhead(bool isSmallForce)
        {
            if (System2Configuration.GetInstance().IsZeroBondForceBeforeDown == false)
            {
                return;
            }

            if (this.bondHead.AxisZ.AxisDrive is ETELAxis)
            {
                // 2号机暂时不清零
                //double read = ModbusService.GetInstance().ReadBondForce()[0] / 10.0;
                //if (read < 0)
                //{
                //    ModbusService.GetInstance().ResetBondhead();
                //}
            }
            else
            {
                int resetCount = 0;

                // 压大力且应变片为负值需要清零
                if (isSmallForce == false && this.GetBondForceCurrentVal(isSmallForce) < -3)
                {
                resetZero:
                    this.ResetBondhead();

                    double bondheadForceInitial = this.GetBondForceCurrentVal(isSmallForce);

                    // 判断清零是否成功
                    if (bondheadForceInitial < -3)
                    {
                        if (resetCount < 5)
                        {
                            resetCount++;
                            Thread.Sleep(20 * resetCount);
                            goto resetZero;
                        }
                        else
                        {
                            DialogResult dialog = AKRSMessageBoxExt.Show(
                                $"焊头清零失败！",
                                "报警",
                                new string[] { "重试", "忽略", "终止" },
                                new DialogResult[] { DialogResult.Retry, DialogResult.Ignore, DialogResult.Abort },
                                AlarmLevel.SecondLevel);

                            switch (dialog)
                            {
                                case DialogResult.Retry:
                                    resetCount = 0;
                                    goto resetZero;

                                case DialogResult.Ignore:
                                    break;

                                case DialogResult.Abort:
                                    throw new Exception("力控标定焊头力控清零失败！");
                            }
                        }
                    }
                }
            }
        }
    }
}
