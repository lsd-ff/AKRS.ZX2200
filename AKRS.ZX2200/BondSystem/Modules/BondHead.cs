using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.GT;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using ch.etel.edi.dmd.v40;
using ch.etel.edi.dsa.v40;
using DevExpress.DashboardWin.Native;
using DevExpress.Data.Linq.Helpers;
using DevExpress.XtraEditors;
using log4net.Core;
using Newtonsoft.Json;
using static GTN.mc;

namespace AKRS.ZX2200.BondSystem.Modules
{
    using System.Diagnostics;

    using AKRS.ZX2200.BondSystem.BondForce;
    using AKRS.ZX2200.BondSystem.BondForce.Models.DevicePara;
    using AKRS.ZX2200.BondSystem.BondForce.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using DevExpress.XtraRichEdit.Model;
    using GTN;

    /// <summary>
    /// 焊头
    /// </summary>
    public class BondHead
    {
        #region 硬件

        /// <summary>
        /// 焊头Z轴
        /// </summary>
        [JsonIgnore]
        public Axis AxisZ => HardwareRepositoryService.GetHardware<Axis>("BondZ");

        /// <summary>
        ///  焊头T轴
        /// </summary>
        [JsonIgnore]
        public Axis AxisT => HardwareRepositoryService.GetHardware<Axis>("焊头T");

        /// <summary>
        /// 检测真空 漏晶传感器,这个传感器已经取反了，漏晶是无输出
        /// </summary>
        [JsonIgnore]
        public Sensor CheckVaccumSensor => HardwareRepositoryService.GetHardware<Sensor>("焊头吸芯片负压源检测");

        ///// <summary>
        ///// 漏晶检测(模拟量)
        ///// </summary>
        //[JsonIgnore]
        //public Sensor 漏晶检测 => HardwareRepositoryService.GetHardware<Sensor>("漏晶检测");

        /// <summary>
        /// 焊头吸吸嘴检测
        /// </summary>
        [JsonIgnore]
        public Sensor CheckToolSensor => HardwareRepositoryService.GetHardware<Sensor>("焊头吸吸嘴检测");

        /// <summary>
        /// 焊头气浮正压检测
        /// </summary>
        [JsonIgnore]
        public Sensor BondheadPositivePressureDetection => HardwareRepositoryService.GetHardware<Sensor>("焊头气浮正压检测");

        /// <summary>
        /// 弱吹电磁阀
        /// </summary>
        [JsonIgnore]
        public Electric WeakBlowElectric => HardwareRepositoryService.GetHardware<Electric>("焊头吹气电磁阀");

        /// <summary>
        /// 吹气比例阀设置
        /// </summary>
        [JsonIgnore]
        public Electric WeakBlowProportionalElectric => HardwareRepositoryService.GetHardware<Electric>("吹气比例阀设置");

        /// <summary>
        /// 吹气比例阀检测
        /// </summary>
        //[JsonIgnore]
        //public Sensor WeakBlowCheck => HardwareRepositoryService.GetHardware<Sensor>("吹气比例阀检测");

        /// <summary>
        /// 真空电磁阀
        /// </summary>
        [JsonIgnore]
        public Electric VaccumElectric => HardwareRepositoryService.GetHardware<Electric>("焊头取片真空电磁阀");

        /// <summary>
        /// 焊头吸附
        /// </summary>
        [JsonIgnore]
        public Electric BondHeadVaccumElectric => HardwareRepositoryService.GetHardware<Electric>("焊头吸吸嘴真空电磁阀");

        /// <summary>
        /// 焊头力清零
        /// </summary>
        [JsonIgnore]
        public Electric BondHeadForceResetZeroElectric => HardwareRepositoryService.GetHardware<Electric>("焊头压力传感器清零");

        /// <summary>
        /// 标定台压力传感器清零，弃用
        /// </summary>
        [JsonIgnore]
        public Electric CalibrateTableForceResetZeroElectric => HardwareRepositoryService.GetHardware<Electric>("标定台压力传感器清零");

        /// <summary>
        /// 校正台压力表模拟量读取
        /// </summary>
        public Sensor ReadCalibrateTableForce => HardwareRepositoryService.GetHardware<Sensor>("校正台压力表模拟量读取");

        /// <summary>
        /// 焊头压力表模拟量读取
        /// </summary>
        public Sensor ReadBondheadForce => HardwareRepositoryService.GetHardware<Sensor>("焊头压力表模拟量读取");

        /// <summary>
        /// 当前焊头上的吸嘴名
        /// </summary>
        [JsonIgnore]
        public string CurrentNozzleName => BondDevicePara.GetInstance().BondHeadParam.CurrentNozzleName;

        /// <summary>
        /// 当前焊头上的吸嘴
        /// </summary>
        [JsonIgnore]
        public Nozzle CurrentNozzle => NozzleRepository.GetInstance().GetNozzle(this.CurrentNozzleName);

        /// <summary>
        /// LVDT
        /// </summary>
        [JsonIgnore]
        public Sensor LVDT => HardwareRepositoryService.GetHardware<Sensor>("焊头LVDT");

        /// <summary>
        /// 焊头压力放大器
        /// </summary>
        [JsonIgnore]
        public Sensor 焊头压力放大器 => HardwareRepositoryService.GetHardware<Sensor>("焊头压力放大器");

        /// <summary>
        /// 标定台压力放大器
        /// </summary>
        [JsonIgnore]
        public Sensor 标定台压力放大器 => HardwareRepositoryService.GetHardware<Sensor>("标定台压力放大器");

        /// <summary>
        /// 焊头点胶气缸
        /// </summary>
        public Electric DispenseMeasureHeightCylinder => HardwareRepositoryService.GetHardware<Electric>("邦头点胶测高气缸电磁阀");
        
        /// <summary>
        /// 点胶测高正限位
        /// </summary>        
        public Sensor DispenseMeasureHeightCylinderPLimit { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("邦头点胶测高气缸原点升起检测");

        /// <summary>
        /// 点胶测高原点
        /// </summary>        
        public Sensor DispenseMeasureHeightCylinderNLimit { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("邦头点胶测高气缸动点下降检测");

        #endregion

        #region 方法

        /// <summary>
        /// Etel力控模式下压
        /// </summary>
        /// <param name="forceValue">力值-N</param>
        /// <param name="pos">位置</param>
        /// <param name="speed">速度</param>
        /// <param name="acc">加速度</param>
        /// <param name="timeOut">超时时间/ms</param>
        /// <exception cref="NullReferenceException">没有获取到指定力配置参数，抛出此异常</exception>
        public void EtelForceControlSet(double forceValue, double pos, double speed, double acc, int timeOut = 10)
        {
            try
            {
                // 获取力控配置,输入参数是g（这里乘以100是把N转成g）
                ForceConfigItem configItem = ForceConfig.GetInstance().GetLargeForceConfigItem(forceValue * 100);

                // 获取失败
                if (configItem == null)
                {
                    AKRSXtraMessageBox.Show($"获取力控参数失败! 输入力：{forceValue * 100}g!", "异常", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    LogHelper.Post(Level.Error, $"获取力控参数失败！", LogCategory.Bond);

                    throw new Exception(
                        $"获取力控参数失败! 输入力：{forceValue * 100}g!");
                }

                // 获取轴
                DsaDrive dsaDrive = ((ETELAxis)this.AxisZ.AxisDrive).GetDrive();

                // KIF.axis = 0.1；
                // 慢速探底速度决定参数
                var conv0 = Dsa.KFConv(306, 0);
                var conv = dsaDrive.convertFloat32FromIso(configItem.LowSpeedDuringTouchDown, conv0);
                dsaDrive.setRegisterFloat32(DmdData.TYP_PPK_FLOAT32, 306, 0, conv);
    
                // kf299.axis = 0.5;
                // 力控阈值
                conv0 = Dsa.KFConv(299, 0);
                conv = dsaDrive.convertFloat32FromIso(configItem.ForceControlLimit, conv0);
                dsaDrive.setRegisterFloat32(DmdData.TYP_PPK_FLOAT32, 299, 0, conv);

                // KF306: 1.axis = 0.0232;
                // 力控阶段I参数
                conv0 = Dsa.KFConv(306, 1);
                conv = dsaDrive.convertFloat32FromIso(configItem.Ki, conv0);
                dsaDrive.setRegisterFloat32(DmdData.TYP_PPK_FLOAT32, 306, 1, conv);

                // KF305: 1.axis = 1.056431561732399e-7;
                // 力控阶段P参数
                conv0 = Dsa.KFConv(305, 1);
                conv = dsaDrive.convertFloat32FromIso(configItem.Kp, conv0);
                dsaDrive.setRegisterFloat32(DmdData.TYP_PPK_FLOAT32, 305, 1, conv);

                // 驱动器往下是正，这里需要取反！
                double targetPos = -pos / 1000.0;

                // SETFC.axis =0.3,0,0.036,0.05,0.1,0.1,1.0;
                // 运动到目标位
                // 这里加速度默认是速度的10倍
                dsaDrive.forceControlSet(forceValue, 0, targetPos, configItem.ForceRange, configItem.ForceDuration, speed / 1000.0, speed / 100.0, timeOut);

                // WTF.axis;
                // 等轴到位，单位是ms
                dsaDrive.forceControlWait(10000);

                // WTT.axis = 5.0;
                // 保压时间-s
                // 保压时间必须大于时间窗口！
                // dsaDrive.waitTime(0.5, -1);
            }
            catch(Exception ex)
            {
                // 退出力控模式
                this.EtelForceControlReset(BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z, 50, 500);

                LogHelper.Post(Level.Info, $"力控下压报错，异常{ex.ToString()}", LogCategory.Global);

                throw ex;
            }
        }

        /// <summary>
        /// Etel力控模式下轴抬起
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="speed">速度</param>
        /// <param name="acc">加速度</param>
        /// <param name="option">退出力控模式</param>
        public void EtelForceControlReset(double pos, double speed, double acc, int option = 0, int timeOut = 10000)
        {
            try
            {
                // 获取轴
                DsaDrive dsaDrive = ((ETELAxis)this.AxisZ.AxisDrive).GetDrive();

                // 驱动器往下是正，这里需要取反！
                double targetPos = -pos / 1000.0;

                // RESETFC.axis = 0,0.05,1.0,15.0;
                // 轴运动
                // 这里加速度默认是速度的10倍
                // 这一句不等Z轴到位
                dsaDrive.forceControlReset(option, targetPos, speed / 1000.0, speed / 100.0, timeOut);

                // 等轴到位
                dsaDrive.waitMovement(timeOut);
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Info, $"退出力控报错，异常{e.ToString()}", LogCategory.Global);
                throw e;
            }
        }

        /// <summary>
        /// 读焊头吸附真空模拟量
        /// </summary>
        /// <returns>值</returns>
        public int ReadBondheadVaccumValue()
        {
            return this.CheckToolSensor.ReadTxPDO((ushort)this.CheckToolSensor.SensorIO, 1);
        }

        #endregion
    }
}
