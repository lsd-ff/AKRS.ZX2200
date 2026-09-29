using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.CalibSystem.Models;

using Newtonsoft.Json;

namespace AKRS.ZX2200.BondSystem.Models.DeviceParams
{
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    /// <summary>
    /// BMC平台参数
    /// </summary>
    public class BMCDevicePara
    {
        /// <summary>
        /// BMC标准吸嘴测高结果
        /// </summary>
        [TreeProgramListArgs("BMC标准吸嘴测高结果", (string)null, false, UnitHelper.mm)]
        public double MeasureHeightResult { get; set; } = 0;

        /// <summary>
        /// BMC平台测高位置
        /// </summary>
        [JsonIgnore]
        [TreeProgramListArgs("BMC平台测高位置", (string)null, false, UnitHelper.mm)]
        public AKRSPoint3D MeasureHeightPos => CalibrateRunPara.GetInstance().BMCMeasureHeightSearchMachinePos;

        /// <summary>
        /// 取料前二段速速度
        /// </summary>
        [TreeProgramListArgs("取片前二段速速度", "取片", 0, 500, UnitHelper.speed)]
        public double SlowTravelSpeedBeforePickup { get; set; } = 2;

        /// <summary>
        /// 取料前二段速距离
        /// </summary>
        [TreeProgramListArgs("取片前二段速距离", "取片", 0, 500, UnitHelper.mm)]
        public double SlowTravelDistanceBeforePickup { get; set; } = 2;

        /// <summary>
        /// 取料后二段速速度
        /// </summary>
        [TreeProgramListArgs("取片后二段速速度", "取片", 0, 500, UnitHelper.speed)]
        public double SlowTravelSpeedAfterPickup { get; set; } = 2;

        /// <summary>
        /// 取料后二段速距离
        /// </summary>
        [TreeProgramListArgs("取片后二段速距离", "取片", 0, 500, UnitHelper.mm)]
        public double SlowTravelDistanceAfterPickup { get; set; } = 2;

        /// <summary>
        /// 放片力值
        /// </summary>
        [TreeProgramListArgs("放片力值", "放片", 0, 1500, UnitHelper.force)]
        public double BondingForce { get; set; } = 50;

        /// <summary>
        /// 放片距离补偿
        /// </summary>
        [TreeProgramListArgs("放片距离补偿", "放片", -500, 500, UnitHelper.mm)]
        public double BondingDistance { get; set; }

        /// <summary>
        /// 放片停留延时
        /// </summary>
        [TreeProgramListArgs("放片停留延时", "放片", 0, 10000000, UnitHelper.ms)]
        public int PlacementDelay { get; set; } = 100;

        /// <summary>
        /// 吸嘴关真空延时
        /// </summary>
        [TreeProgramListArgs("吸嘴关真空延时", "放片", 0, 10000000, UnitHelper.ms)]
        public int VacuumOffDelay { get; set; } = 100;

        /// <summary>
        /// 弱吹延时
        /// </summary>
        [TreeProgramListArgs("弱吹延时", "放片", 0, 10000000, UnitHelper.ms)]
        public int BondingBlowDelay { get; set; } = 100;

        /// <summary>
        /// 放片前二段速速度
        /// </summary>
        [TreeProgramListArgs("放片前二段速速度", "放片", 0, 500, UnitHelper.speed)]
        public double SlowTravelSpeedBeforeBonding { get; set; } = 2;

        /// <summary>
        /// 放片前二段速距离
        /// </summary>
        [TreeProgramListArgs("放片前二段速距离", "放片", 0, 500, UnitHelper.mm)]
        public double SlowTravelDistanceBeforeBonding { get; set; } = 3;

        /// <summary>
        /// 放片后二段速速度
        /// </summary>
        [TreeProgramListArgs("放片后二段速速度", "放片", 0, 500, UnitHelper.speed)]
        public double SlowTravelSpeedAfterBonding { get; set; } = 2;

        /// <summary>
        /// 放片后二段速距离
        /// </summary>
        [TreeProgramListArgs("放片后二段速距离", "放片", 0, 500, UnitHelper.mm)]
        public double SlowTravelDistanceAfterBonding { get; set; } = 3;

        /// <summary>
        /// 取料力值
        /// </summary>
        [TreeProgramListArgs("取片力值", "取片", 0, 1500, UnitHelper.force)]
        public double PickupForce { get; set; } = 50;

        /// <summary>
        /// 取料停留延时
        /// </summary>
        [TreeProgramListArgs("取片停留延时", "取片", 0, 10000000, UnitHelper.ms)]
        public int PickupDelay { get; set; } = 100;

        /// <summary>
        /// XY公差界限
        /// </summary>
        public double AccuracyXY { get; set; }

        /// <summary>
        /// 角度公差界限
        /// </summary>
        public double AccuracyAngle { get; set; }

        /// <summary>
        /// 补偿X
        /// </summary>
        public double CompensateX { get; set; }

        /// <summary>
        /// 补偿Y
        /// </summary>
        public double CompensateY { get; set; }

        /// <summary>
        /// 补偿角度
        /// </summary>
        public double CompensateAngle { get; set; }

        /// <summary>
        /// 吹气比例
        /// </summary>
        public int BlowProportion { get; set; }

        /// <summary>
        /// CMK实验焊后补偿
        /// </summary>
        public AKRSPoint3D PostBondCompensate { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 力控标定位置,G0坐标
        /// </summary>
        [TreeProgramListArgs("力控标定位置（G0）", (string)null,false, UnitHelper.mm)]
        public AKRSPoint3D ForceCalibratePos { get; set; } = new AKRSPoint3D();
    }
}
