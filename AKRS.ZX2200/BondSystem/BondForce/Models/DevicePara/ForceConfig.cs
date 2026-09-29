using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Infrastructure.AOP.Module;
using AKRS.ZX2200.Infrastructure.Models.Path;
using AKRS.ZX2200.SupportFeature.Parameters.Model;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace AKRS.ZX2200.BondSystem.BondForce.Models.DevicePara
{
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using DevExpress.DashboardCommon.DataProcessing;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// 力控配置对象
    /// </summary>
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class ForceConfig : Singleton<ForceConfig>, INotifyPropertyChanged
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        static ForceConfig()
        {
            Singleton<ForceConfig>.FilePath = ZX2200PathConfig.ForceConfig;
        }

        /// <summary>
        /// 大力控配置对象集合
        /// </summary>
        public List<ForceConfigItem> LargeForceConfigItemList { get; set; } = new List<ForceConfigItem>();

        /// <summary>
        /// 小力力控配置对象集合
        /// </summary>
        public List<ForceConfigItem> SmallForceConfigItemList { get; set; } = new List<ForceConfigItem>();

        /// <summary>
        /// 小力力控标定间隔
        /// </summary>
        [TreeProgramListArgs("小力力控标定间隔", (string)null, null, RoleEnum.Admin)]
        public double SmallForceCalibrateInterval { get; set; } = 1;

        /// <summary>
        /// 力控标定间隔
        /// </summary>
        [TreeProgramListArgs("大力力控标定间隔", (string)null, null, RoleEnum.Admin)]
        public double LargeForceCalibrateInterval { get; set; } = 5;

        /// <summary>
        /// 小力上限
        /// </summary>
        [TreeProgramListArgs("大小力分界线（g）", (string)null, null, RoleEnum.Admin)]
        public double ForceBoundary { get; set; } = 0;

        /// <summary>
        /// 是否开启力控检查
        /// </summary>
        [TreeProgramListArgs("是否开启力控检查", (string)null)]
        public bool IsActiveBondForceCheck { get; set; } = true;

        /// <summary>
        ///  压力表量程kg（大力）
        /// </summary>
        [TreeProgramListArgs("焊头压力表量程(kg)", (string)null, null, RoleEnum.Admin)]
        public double BondheadMaxPress { get; set; } = 2;

        /// <summary>
        /// LVDT量程kg（小力）
        /// </summary>
        [TreeProgramListArgs("LVDT量程(kg)", (string)null)]
        public double LVDTMaxPress { get; set; } = 2;

        /// <summary>
        ///  压力表量程kg
        /// </summary>
        [TreeProgramListArgs("校正台压力表量程(kg)", (string)null, null, RoleEnum.Admin)]
        public double CalibrateTableMaxPress { get; set; } = 2;

        /// <summary>
        ///  小力标定初始增量
        /// </summary>
        [TreeProgramListArgs("小力标定初始增量", (string)null, null, RoleEnum.Admin)]
        public double SmallForceCalibrateInitialIncrement { get; set; } = 15;

        /// <summary>
        ///  大力标定初始增量
        /// </summary>
        [TreeProgramListArgs("大力标定初始增量", (string)null, null, RoleEnum.Admin)]
        public double LargeForceCalibrateInitialIncrement { get; set; } = 7.5;

        /// <summary>
        ///  力控标定模式
        /// </summary>
        [TreeProgramListArgs("力控标定模式", (string)null, null, RoleEnum.Admin)]
        public ForceCaliModeEnum ForceCaliMode = ForceCaliModeEnum.SingleAngle;

        /// <summary>
        /// 标定范围枚举
        /// </summary>
        [TreeProgramListArgs("标定范围", (string)null, null, RoleEnum.Admin)]
        public ForceRangeEnum ForceRange = ForceRangeEnum.LargeForce;

        /// <summary>
        /// 小力标定角度间距
        /// </summary>
        [TreeProgramListArgs("小力标定角度间距（°）", (string)null, null, RoleEnum.Admin)]
        public double SmallForceCaliAngleDistance { get; set; } = 30;

        /// <summary>
        /// 大力标定角度间距
        /// </summary>
        [TreeProgramListArgs("大力标定角度间距（°）", (string)null, null, RoleEnum.Admin)]
        public double LargeForceCaliAngleDistance { get; set; } = 30;

        /// <summary>
        /// 标定力保持延时
        /// </summary>
        [TreeProgramListArgs("力保持延时（ms）", (string)null, null, RoleEnum.Admin)]
        public int ForceKeepDelay { get; set; } = 20;

        /// <summary>
        /// LVDT模拟量最大值
        /// </summary>
        [TreeProgramListArgs("LVDT模拟量最大值", (string)null, null, RoleEnum.Admin)]
        public double LVDTMaxInputForce { get; set; } = 100;

        /// <summary>
        /// 力控清零方式
        /// </summary>
        [TreeProgramListArgs("力控清零方式", (string)null, null, RoleEnum.Admin)]
        public ForceZeroModeEnum ForceZeroMode { get; set; } = ForceZeroModeEnum.Modbus;

        /// <summary>
        /// LVDT力控下界
        /// </summary>
        [TreeProgramListArgs("LVDT力控下界", (string)null, null, RoleEnum.Admin)]
        public double LVDTForceControlLowerlimit { get; set; } = 0;

        /// <summary>
        /// LVDT力控上界
        /// </summary>
        [TreeProgramListArgs("LVDT力控上界", (string)null, null, RoleEnum.Admin)]
        public double LVDTForceControlUpperlimit { get; set; } = 0;

        /// <summary>
        /// 应变片力控下界
        /// </summary>
        [TreeProgramListArgs("应变片力控下界", (string)null, null, RoleEnum.Admin)]
        public double StrainGaugeForceControlLowerlimit { get; set; } = 0;

        /// <summary>
        /// 应变片力控上界
        /// </summary>
        [TreeProgramListArgs("应变片力控上界", (string)null, null, RoleEnum.Admin)]
        public double StrainGaugeForceControlUpperlimit { get; set; } = 0;

        #region 力控调试

        /// <summary>
        /// 力控调试A点位置
        /// </summary>
        public double ForceControlDebugAPos { get; set; } = 0;

        /// <summary>
        /// 力控调试B点位置
        /// </summary>
        public double ForceControlDebugBPos { get; set; } = -40.1;

        /// <summary>
        /// 力控调试C点位置
        /// </summary>
        public double ForceControlDebugCPos { get; set; } = -43.1;

        /// <summary>
        /// 力控调试B点到C点的距离
        /// </summary>
        [TreeProgramListArgs("力控调试B点到C点的距离", (string)null, "mm", RoleEnum.Admin)]
        public double ForceControlDebugBToCDistance { get; set; } = 4;

        #endregion

        /// <summary>
        /// 获取大力力控配置对象
        /// </summary>
        /// <param name="forceValue">力值</param>
        /// <returns>力控配置对象</returns>
        public ForceConfigItem GetLargeForceConfigItem(double forceValue)
        {
            // 排序
            this.LargeForceConfigItemList = this.LargeForceConfigItemList.OrderBy(it => it.ForceLowerLimit).ToList();

            foreach (var forceConfigItem in this.LargeForceConfigItemList)
            {
                if (forceValue >= forceConfigItem.ForceLowerLimit && forceValue <= forceConfigItem.ForceUpperLimit)
                {
                    return forceConfigItem;
                }
            }

            // 小于最小模拟量就用最小模拟量的PID
            if (forceValue < this.LargeForceConfigItemList[0].ForceLowerLimit)
            {
                return this.LargeForceConfigItemList[0];
            }

            int count = this.LargeForceConfigItemList.Count;
            if (forceValue > this.LargeForceConfigItemList.Select(it => it.ForceUpperLimit).Max())
            {              
                // 大于最大模拟量就用最大模拟量的PID
                return this.LargeForceConfigItemList[count - 1];
            }

            return null;
        }

        /// <summary>
        /// 获取小力力控配置对象
        /// </summary>
        /// <param name="forceValue">力值</param>
        /// <returns>力控配置对象</returns>
        public ForceConfigItem GetSmallForceConfigItem(double forceValue)
        {
            // 排序
            this.SmallForceConfigItemList = this.SmallForceConfigItemList.OrderBy(it => it.ForceLowerLimit).ToList();

            foreach (var forceConfigItem in this.SmallForceConfigItemList)
            {
                if (forceValue >= forceConfigItem.ForceLowerLimit && forceValue <= forceConfigItem.ForceUpperLimit)
                {
                    return forceConfigItem;
                }
            }

            // 小于最小模拟量就用最小模拟量的PID
            if (forceValue < this.SmallForceConfigItemList[0].ForceLowerLimit)
            {
                return this.SmallForceConfigItemList[0];
            }

            int count = this.SmallForceConfigItemList.Count;


            if (forceValue > this.SmallForceConfigItemList.Select(it => it.ForceUpperLimit).Max())
            {
                // 大于最小模拟量就用最大模拟量的PID
                return this.SmallForceConfigItemList[count - 1];
            }

            return null;
        }

        /// <summary>
        /// 排序刷新
        /// </summary>
        public void Order()
        {
            // 排序
            this.SmallForceConfigItemList = this.SmallForceConfigItemList.OrderBy(it => it.ForceLowerLimit).ToList();

            // 排序
            this.LargeForceConfigItemList = this.LargeForceConfigItemList.OrderBy(it => it.ForceLowerLimit).ToList();
        }

        /// <summary>
        /// 参数是否为空
        /// </summary>
        /// <returns>结果</returns>
        public bool IsEmpty()
        {
            return this.SmallForceConfigItemList.Count == 0 && this.LargeForceConfigItemList.Count == 0;
        }

        /// <summary>
        /// 属性更改事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性更改记录
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="before">开始</param>
        /// <param name="after">结束</param>
        protected virtual void OnPropertyChanged(string propertyName, object before, object after)
        {
            PropertyChangeAop.OnPropertyChangedEvent(this, propertyName, before, after);
        }
    }

    /// <summary>
    /// 配置条目对象
    /// </summary>
    [Serializable]
    public class ForceConfigItem
    {
        /// <summary>
        /// 力值上限
        /// </summary>
        public double ForceUpperLimit { get; set; }

        /// <summary>
        /// 力值下限
        /// </summary>
        public double ForceLowerLimit { get; set; }

        /// <summary>
        /// KIF值
        /// 慢速探底速度决定参数
        /// </summary>
        [TreeProgramListArgs("慢速探底速度决定参数", (string)null)]
        public double LowSpeedDuringTouchDown { get; set; }

        #region Etel

        /// <summary>
        /// KF299值
        /// 力控阈值 
        /// </summary>
        [TreeProgramListArgs("力控阈值", (string)null)]
        public double ForceControlLimit { get; set; }

        /// <summary>
        /// KF305值
        /// 力控阶段P参数
        /// </summary>
        [TreeProgramListArgs("力控阶段P参数", (string)null)]
        public double Kp { get; set; }

        /// <summary>
        /// KF306值
        /// 力控阶段I参数
        /// </summary>
        [TreeProgramListArgs("力控阶段I参数", (string)null)]
        public double Ki { get; set; }

        /// <summary>
        /// 力窗口
        /// </summary>
        [TreeProgramListArgs("力窗口", (string)null)]
        public double ForceRange { get; set; }

        /// <summary>
        /// 时间窗口
        /// </summary>
        [TreeProgramListArgs("时间窗口", (string)null)]
        public double ForceDuration { get; set; }

        #endregion


        #region GT

        /// <summary>
        /// 切换阈值上界
        /// </summary>
        public double ChangeForceUpperLimit { get; set; }

        /// <summary>
        /// 切换阈值下界
        /// </summary>
        public double ChangeForceLowerLimit { get; set; }

        /// <summary>
        ///  力控阶段D参数
        /// </summary>
        [TreeProgramListArgs("力控阶段D参数", (string)null)]
        public double Kd { get; set; }

        /// <summary>
        ///  平滑时间
        /// </summary>
        [TreeProgramListArgs("平滑时间", (string)null)]
        public short SmoothTime { get; set; }

        /// <summary>
        ///  加速度
        /// </summary>
        [TreeProgramListArgs("加速度", (string)null)]
        public double Acc { get; set; }

        /// <summary>
        ///  减速度
        /// </summary>
        [TreeProgramListArgs("减速度", (string)null)]
        public double Dec { get; set; }

        /// <summary>
        ///  切换力值计算斜率
        /// </summary>
        [TreeProgramListArgs("切换力值计算斜率", (string)null)]
        public double ChangeForceK { get; set; }

        /// <summary>
        ///  切换力值计算B值
        /// </summary>
        [TreeProgramListArgs("切换力值计算b", (string)null)]
        public double ChangeForceB { get; set; }

        /// <summary>
        /// BC段加速度
        /// </summary>
        [TreeProgramListArgs("BC段加速度", (string)null)]
        public double SlowTouchAcc { get; set; } = 500;

        /// <summary>
        /// BC段减速度
        /// </summary>
        [TreeProgramListArgs("BC段减速度", (string)null)]
        public double SlowTouchDec { get; set; } = 500;

        #endregion
    }
}
