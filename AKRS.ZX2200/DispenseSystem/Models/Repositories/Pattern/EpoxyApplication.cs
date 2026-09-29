using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;

namespace AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern
{
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.DispenseSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    using Newtonsoft.Json;

    /// <summary>
    /// 点胶时参数
    /// </summary>
    [Serializable]
    public class EpoxyApplication : BaseDsSetting
    {
        /// <summary>
        /// 点胶时候的模式
        /// </summary>
        [TreeProgramListArgs("点胶模式", (string)null)]
        public DispensingTypeEnum DispensingType { get; set; }
        
        /// <summary>
        /// 点胶模式
        /// </summary>
        [TreeProgramListArgs("点胶类型", (string)null)]
        public EpoxyApplicationTypeEnum EpoxyApplicationStrategy { get; set; }

        /// <summary>
        /// 点位集合
        /// </summary>
        public List<EpoxyApplicationLocation[]> DispensePatternParas { get; set; } = new List<EpoxyApplicationLocation[]>();

        /// <summary>
        /// 画胶图形是否可用
        /// 主要是为了防止加速度，减速度，速度为0
        /// </summary>
        [JsonIgnore]
        public bool EpoxyIsReasonable => this.IsDataReasonAble();

        #region 点胶

        /// <summary>
        /// 准备开始点胶的高度
        /// </summary>
        [TreeProgramListArgs("准备开始点胶高度", "点胶", 0, 50, "mm")]
        public double SecurityHeightForEpoxyApplication { get; set; } = 5;

        /// <summary>
        /// 点胶压力值
        /// </summary>
        [TreeProgramListArgs("点胶时正压", "点胶", 0, 500, "kp")]
        public double DispensePressure { get; set; } = 100;

        /// <summary>
        /// 点胶真空值
        /// </summary>
        [TreeProgramListArgs("点胶时负压", "点胶", 0, 500, "kp")]
        public double Vacuum { get; set; }

        /// <summary>
        /// 轴速度的百分比，所有轴都会乘上这个速度
        /// </summary>
        [TreeProgramListArgs("画胶速度百分比", "点胶", 1, 500, "%")]
        public double SpindleSpeedPercentage { get; set; } = 50;

        /// <summary>
        /// 点胶到位延迟
        /// </summary>
        [TreeProgramListArgs("点胶到位延迟", "点胶")]
        public bool IsDispenserLeadTimeOpen { get; set; } = true;

        /// <summary>
        /// 点胶到位延迟
        /// </summary>
        [TreeProgramListArgs("点胶到位延迟时间", "点胶", 0, 5000, "ms")]
        public int DispenserLeadTime { get; set; } = 0;

        /// <summary>
        /// 点胶缓慢下降的速度
        /// </summary>
        [TreeProgramListArgs("点胶前下降速度", "点胶", 1, 500, "mm/s")]
        public double SlowTravelSpeedBeforeDispensing { get; set; } = 10;

        /// <summary>
        /// 提前开胶时间
        /// </summary>
        // [TreeProgramListArgs("提前开胶距离", "点胶", 0, 50, "mm")]
        public int AdvanceOpenDistance { get; set; } = 0;

        /// <summary>
        /// 提前开胶时间
        /// </summary>
        [TreeProgramListArgs("提前开胶距离", "点胶", 0, 50, "mm")]
        public double AdvanceOpenDistanceNew { get; set; } = 0;

        #endregion

        #region 偏移

        /// <summary>
        /// 针对于Module原点的偏移值X
        /// </summary>
        [TreeProgramListArgs("偏移X", "偏移", "mm")]
        public double OffsetX { get; set; } = 0;

        /// <summary>
        /// 针对于Module原点的偏移值X
        /// </summary>
        [TreeProgramListArgs("偏移Y", "偏移", "mm")]
        public double OffsetY { get; set; } = 0;

        /// <summary>
        /// 画胶时Z轴距离基板的距离
        /// 不可为负数，前端矫正
        /// </summary>
        [TreeProgramListArgs("偏移高度", "偏移", "mm")]
        public double OffsetZ { get; set; } = 0;

        /// <summary>
        /// 在预点胶上进行点胶时的高度偏移
        /// </summary>
        [TreeProgramListArgs("预点胶高度补偿", "偏移", "mm")]
        public double PrePlantOffSetZ { get; set; } = 1;

        #endregion

        #region 一段断尾

        /// <summary>
        /// 第一段是否打开
        /// </summary>
        [TreeProgramListArgs("一段断尾是否打开", "一段断尾")]
        public bool TearOff1Open { get; set; } = false;

        /// <summary>
        /// 一段断尾高度
        /// </summary>
        [TreeProgramListArgs("一段断尾高度", "一段断尾", 0, 50, "mm")]
        public double TearOff1Height { get; set; } = 2;

        /// <summary>
        /// 一段断尾速度
        /// </summary>
        [TreeProgramListArgs("一段断尾速度", "一段断尾", 0, 500, "mm/s")]
        public double TearOff1Speed { get; set; } = 10;

        /// <summary>
        /// 一段短尾延迟
        /// </summary>
        [TreeProgramListArgs("一段短尾延迟", "一段断尾", 0, 5000, "ms")]
        public int TearOff1Delay { get; set; } = 10;

        #endregion

        #region 二段断尾

        /// <summary>
        /// 第二段是否打开
        /// </summary>
        [TreeProgramListArgs("二段断尾是否打开", "二段断尾")]
        public bool TearOff2Open { get; set; } = false;

        /// <summary>
        /// 二段断尾高度
        /// </summary>
        [TreeProgramListArgs("二段断尾高度", "二段断尾", 0, 50, "mm")]
        public double TearOff2Height { get; set; } = 4;

        /// <summary>
        /// 二段断尾速度
        /// </summary>
        [TreeProgramListArgs("二段断尾速度", "二段断尾", 0, 500, "mm/s")]
        public double TearOff2Speed { get; set; } = 0;

        /// <summary>
        /// 二段短尾延迟
        /// </summary>
        [TreeProgramListArgs("二段短尾延迟", "二段断尾", 0, 5000, "ms")]
        public int TearOff2Delay { get; set; } = 10;

        #endregion

        #region 图形形状

        /// <summary>
        /// 画胶图形的X方向的大小
        /// 单位为mm
        /// </summary>
        [TreeProgramListArgs("图形长", "图形形状", 0, 100, "mm")]
        public double SizeX { get; set; } = 1;

        /// <summary>
        /// 画胶图形Y方向的大小
        /// 单位为mm
        /// </summary>
        [TreeProgramListArgs("图形宽", "图形形状", 0, 100, "mm")]
        public double SizeY { get; set; } = 1;

        /// <summary>
        /// 芯片的X方向的大小
        /// 单位为mm
        /// </summary>
        public double ComponentSizeX { get; set; } = 2;

        /// <summary>
        /// 芯片的Y方向的大小
        /// 单位为mm
        /// </summary>
        public double ComponentSizeY { get; set; } = 2;

        /// <summary>
        /// 画胶图像的角度
        /// 单位为度
        /// </summary>
        [TreeProgramListArgs("图形角度", "图形形状", "度")]
        public double Angle { get; set; } = 0;

        #endregion

        #region 蘸胶参数

        /// <summary>
        /// 蘸胶下降高度
        /// </summary>
        [TreeProgramListArgs("蘸胶下降高度", "蘸胶参数", 0, 5, "mm")]
        public double PrintSlowDownHeight { get; set; } = 2;

        /// <summary>
        /// 蘸胶下降速度
        /// </summary>
        [TreeProgramListArgs("蘸胶下降速度", "蘸胶参数", 0, 500, "mm/s")]
        public double PrintSlowDownSpeed { get; set; } = 100;

        /// <summary>
        /// 蘸胶停留
        /// </summary>
        [TreeProgramListArgs("蘸胶停留", "蘸胶参数", 0, 5000, "ms")]
        public int PrintDelayTime { get; set; } = 100;

        /// <summary>
        /// 蘸胶参数
        /// </summary>
        [TreeProgramListArgs("蘸胶深度", "蘸胶参数", -5, 5, "mm")]
        public double PrintOffsetZ { get; set; } = 0;

        /// <summary>
        /// 蘸胶上升高度
        /// </summary>
        [TreeProgramListArgs("蘸胶上升高度", "蘸胶参数", 0, 5, "mm")]
        public double PrintSlowUpHeight { get; set; } = 2;

        /// <summary>
        /// 蘸胶上升速度
        /// </summary>
        [TreeProgramListArgs("蘸胶上升速度", "蘸胶参数", 0, 5000, "mm/s")]
        public double PrintSlowUpSpeed { get; set; } = 100;

        /// <summary>
        /// 蘸胶上升后延时
        /// </summary>
        [TreeProgramListArgs("蘸胶上升后延时", "蘸胶参数", 0, 5000, "ms")]
        public int PrintSlowUpDelayTime { get; set; } = 100;

        /// <summary>
        /// 蘸胶吸嘴的名称
        /// </summary>
        public string PrintNozzleName { get; set; } = "Null";

        #endregion

        /// <summary>
        /// 数据是否合理
        /// 主要避免速度为0和过大
        /// </summary>
        /// <returns>结果</returns>
        public bool IsDataReasonAble()
        {
            for (int i = 0; i < this.DispensePatternParas.Count; i++)
            {
                for (int j = 0; j < this.DispensePatternParas[i].Length; j++)
                {
                    if (this.DispensePatternParas[i][j].Acc == 0)
                    {
                        this.DispensePatternParas[i][j].Acc = 100;
                    }

                    if (this.DispensePatternParas[i][j].Dec == 0)
                    {
                        this.DispensePatternParas[i][j].Dec = 100;
                    }

                    if (this.DispensePatternParas[i][j].Speed == 0)
                    {
                        this.DispensePatternParas[i][j].Speed = 10;
                    }
                }
            }

            for (int i = 0; i < this.DispensePatternParas.Count; i++)
            {
                for (int j = 0; j < this.DispensePatternParas[i].Length; j++)
                {
                    if (j != this.DispensePatternParas[i].Length - 1)
                    {
                        if (!DispenserMoveHelper.IsEpoxyDataReasonable(this.DispensePatternParas[i][j].Acc))
                        {
                            AKRSXtraMessageBox.Show($"线{i + 1} ,点{j + 1} 加速度 超过极限 10000");
                            return false;
                        }

                        if (!DispenserMoveHelper.IsEpoxyDataReasonable(this.DispensePatternParas[i][j].Dec))
                        {
                            AKRSXtraMessageBox.Show($"线{i + 1} ,点{j + 1} 减速度 超过极限 10000");
                            return false;
                        }

                        if (!DispenserMoveHelper.IsEpoxyDataReasonableSpeed(this.DispensePatternParas[i][j].Speed))
                        {
                            AKRSXtraMessageBox.Show($"线{i + 1} ,点{j + 1} 速度超过极限 1000");
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
