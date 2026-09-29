using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.Galaxy2.Machine.Models
{
    using System.IO;
    using System.Runtime.CompilerServices;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.MachineSupport.Config;

    using DevExpress.XtraEditors;

    using Newtonsoft.Json;

    /// <summary>
    /// 设备底层状态机 单例
    /// </summary>
    public class MachineStateModel
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static MachineStateModel instance;

        /// <summary>
        /// 锁
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// 保存路径
        /// </summary>
        private static string FilePath => Path.Combine(PathConfig.DeviceDirPath, "MachineStateModelPath.json");


        /// <summary>
        /// 私有化
        /// </summary>
        private MachineStateModel()
        {
            this.MachineState = MachineStateEnum.Stop;
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static MachineStateModel GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        // 加载
                        MachineStateModel.instance = JsonFormatHelper<MachineStateModel>.ReadGenericObject(MachineStateModel.FilePath);

                        if (instance == null)
                        {
                            instance = new MachineStateModel();
                        }
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save() =>
            JsonFormatHelper<MachineStateModel>.SaveGenericObject(
                MachineStateModel.instance,
                MachineStateModel.FilePath);

        /// <summary>
        /// 设备状态：停止，暂停，自动工作中，手动操作
        /// </summary>
        public MachineStateEnum MachineState { get; set; } = MachineStateEnum.Stop;

        /// <summary>
        /// 工作模式
        /// </summary>
        public MachineWorkModeEnum MachineWorkMode { get; set; } = MachineWorkModeEnum.NormalWork;

        /// <summary>
        /// 当前选择的系统
        /// </summary>
        [JsonIgnore]
        public CurrentMachineSystemEnum CurrentMachineSystem { get; set; } = CurrentMachineSystemEnum.System2;

        /// <summary>
        /// 是否是离线模式
        /// </summary>
        [JsonIgnore]
        public bool IsOffLineWork => this.MachineWorkMode == MachineWorkModeEnum.OffLineWork;

        /// <summary>
        /// 是否是空跑模式
        /// </summary>
        [JsonIgnore]
        public bool IsDryCycle => this.MachineWorkMode == MachineWorkModeEnum.DryCycle;

        /// <summary>
        /// 是否是补偿模式
        /// </summary>
        [JsonIgnore]
        public bool IsCompensateWork => this.MachineWorkMode == MachineWorkModeEnum.CompensateWork;

        /// <summary>
        /// 是否是单步模式
        /// </summary>
        [JsonIgnore]
        public bool IsSingleStepWork = false;

        /// <summary>
        /// 是否是正常工作模式
        /// </summary>
        [JsonIgnore]
        public bool IsNormalWork => this.MachineWorkMode == MachineWorkModeEnum.NormalWork;

        /// <summary>
        /// 是否观看上一颗焊点
        /// </summary>
        [JsonIgnore]
        public bool IsVisionPreviousBondPositionInSystem2 { get; set; } = false;

        /// <summary>
        /// 是否观看上一颗焊点
        /// </summary>
        [JsonIgnore]
        public bool IsVisionPreviousBondPositionInSystem1 { get; set; } = false;

        /// <summary>
        /// 是否蘸胶
        /// </summary>
        [JsonIgnore]
        public bool IsVisionDippedBondPosition { get; set; } = false;

        /// <summary>
        /// 观看上一颗点胶点的怎么样
        /// </summary>
        public void VisionPreviousBondPositionSystem1()
        {
            // 如果设备不在自动工作，返回
            if (this.MachineState != MachineStateEnum.Working)
            {
                XtraMessageBox.Show("设备没有在自动工作，无法查看");
                return;
            }

            // 如果已经在查看，直接返回
            if (this.IsVisionPreviousBondPositionInSystem1)
            {
                this.IsVisionPreviousBondPositionInSystem1 = false;
                return;
            }
            else
            {
                this.IsVisionPreviousBondPositionInSystem1 = true;
            }
        }

        /// <summary>
        /// 观看上一颗贴的怎么样
        /// </summary>
        public void VisionPreviousBondPositionSystem2()
        {
            // 如果设备不在自动工作，返回
            if (this.MachineState != MachineStateEnum.Working)
            {
                XtraMessageBox.Show("设备没有在自动工作，无法查看");
                return;
            }

            // 如果已经在查看，直接返回
            if (this.IsVisionPreviousBondPositionInSystem2)
            {
                this.IsVisionPreviousBondPositionInSystem2 = false;
                return;
            }
            else
            {
                this.IsVisionPreviousBondPositionInSystem2 = true;
            }
        }

        /// <summary>
        /// 观看蘸过胶的焊点
        /// </summary>
        public void VisionDippedBondPosition()
        {
            // 如果设备不在自动工作，返回
            if (this.MachineState != MachineStateEnum.Working)
            {
                XtraMessageBox.Show("The machine is not Working ,can not be viewed");
                return;
            }

            // 如果已经在查看，直接返回
            if (this.IsVisionDippedBondPosition)
            {
                this.IsVisionDippedBondPosition = false;
                return;
            }
            else
            {
                this.IsVisionDippedBondPosition = true;
            }
        }
    }
}
