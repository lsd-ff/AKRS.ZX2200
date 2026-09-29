using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.TransportUnitSystem.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EditStateEnum = AKRS.ZX2200.TransportUnitSystem.Model.EditStateEnum;
using AKRS.Galaxy2.MeasureHeight;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Config
{
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.ZX2200.Infrastructure.AOP.Module;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    /// <summary>
    /// 单个焊点的配置对象
    /// </summary>
    public class SingleBondPositionConfig : SingleConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 贴片位置硬补偿
        /// </summary>
        [TreeProgramListArgs("焊点贴片位置偏移", "固晶偏移", "mm")]
        public AKRSPoint3D BondPosOffset { get; set; } = new AKRSPoint3D();
        
        /// <summary>
        /// 贴片角度，硬补偿
        /// </summary>
        [TreeProgramListArgs("固晶角度偏移", "固晶偏移", -180, 180, "°")]
        public double RotaryPosition { get; set; }

        /// <summary>
        /// 贴片位置硬补偿
        /// </summary>
        [TreeProgramListArgs("点胶位置偏移", "点胶偏移", "mm")]
        public AKRSPoint3D DispensePosOffset { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 贴片角度，硬补偿
        /// </summary>
        [TreeProgramListArgs("点胶角度偏移", "点胶偏移", -180, 180, "°")]
        public double DispenseRotaryPosition { get; set; }

        /// <summary>
        /// 是不是镜像
        /// </summary>
        [TreeProgramListArgs("镜像焊点")]
        public bool IsMirror { get; set; } = false;

        /// <summary>
        /// 是否示教角度
        /// </summary>
        public bool AssistanceAngle { get; set; } = false;

        /// <summary>
        /// 焊点示教的基岛号
        /// 仅用于界面参数传递
        /// </summary>
        [JsonIgnore]
        public int TeachModuleNum { get; set; } = 1;

        /// <summary>
        /// 焊点示教的基板号
        /// 仅用于界面参数传递
        /// </summary>
        [JsonIgnore]
        public int TeachSubstrateNum { get; set; } = 1;

        /// <summary>
        /// 相对固晶方式枚举
        /// </summary>
        [TreeProgramListArgs("Relative Bonding", (string)null)]
        public RelativeBondingEnum RelativeBonding { get; set; } = RelativeBondingEnum.Off;

        /// <summary>
        /// 相对固精
        /// </summary>
        [TreeProgramListArgs("Relative BondPosition", (string)null)]
        public bool RelativeBondPosition { get; set; } = false;

        /// <summary>
        /// 相对固晶两点搜索方式枚举
        /// </summary>
        public SearchTypeEnum SearchType { get; set; }

        /// <summary>
        /// 焊点点胶应用枚举
        /// </summary>
        public EpoxyApplicationEnum EpoxyApplication { get; set; }

        /// <summary>
        /// 是否先点胶后装片
        /// </summary>
        public bool IsEpoxyAndComponent { get; set; }

        /// <summary>
        /// 编辑状态
        /// </summary>
        public EditStateEnum EditState { get; set; } = EditStateEnum.NewCreate;

        /// <summary>
        /// 焊点回看时的灯光
        /// </summary>
        public Dictionary<string, int> VisionLightValue { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// 相机曝光
        /// </summary>
        [TreeProgramListArgs("相机曝光", "焊点回看")]
        public double VisionExposure { get; set; } = 500;

        /// <summary>
        /// 相机增益
        /// </summary>
        [TreeProgramListArgs("相机增益", "焊点回看")]
        public double VisionGain { get; set; } = 1;

        /// <summary>
        /// 相机gamma
        /// </summary>
        [TreeProgramListArgs("相机伽马", "焊点回看")]
        public double VisionGamma { get; set; } = 40;

        /// <summary>
        /// 回看位置偏移
        /// </summary>
        [TreeProgramListArgs("回看位置偏移", "焊点回看", "mm")]
        public AKRSPoint3D AKRSPoint3D { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// SingleBondPositionConfig
        /// </summary>
        public SingleBondPositionConfig()
        {
            this.ResetAssistantStates();
        }

        /// <summary>
        /// TeachBondingPosition
        /// </summary>
        public AssistantState TeachBondingPosition { get; set; }

        /// <summary>
        /// MoveToBondingPosition
        /// </summary>
        public AssistantState MoveToBondingPosition { get; set; }

        /// <summary>
        /// BondPositionAdjust
        /// </summary>
        public AssistantState BondPositionAdjust { get; set; }

        /// <summary>
        /// BondPositionMeasureHeight
        /// </summary>
        public AssistantState BondPositionMeasureHeight { get; set; }

        /// <summary>
        /// BondPositionMeasureHeight
        /// </summary>
        public AssistantState BondPositionIdentity { get; set; }

        /// <summary>
        /// BondPosition是否示教完成
        /// </summary>
        [JsonIgnore]
        public bool IsAssistantSucceed => this.GetAssistantResult();

        /// <summary>
        /// BondPosition是否示教完成
        /// </summary>
        /// <returns>return</returns>
        private bool GetAssistantResult()
        {
            List<AssistantState> list = this.GetAssistantStates();
            
            foreach (var item in list)
            {
                if (item == null)
                {
                    return false;
                }

                if (item.State == AssistantStateEnum.UnAble)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取BondPosition所有示教的状态
        /// </summary>
        /// <returns>return</returns>
        public List<AssistantState> GetAssistantStates()
        {
            List<AssistantState> list = new List<AssistantState>();
            list.Add(this.TeachBondingPosition);
            list.Add(this.MoveToBondingPosition);
            list.Add(this.BondPositionAdjust);
            list.Add(this.BondPositionMeasureHeight);
            list.Add(this.BondPositionIdentity);

            return list;
        }

        /// <summary>
        /// 初始化BondPosition示教状态
        /// </summary>
        public void ResetAssistantStates()
        {
            this.TeachBondingPosition = new AssistantState()
                                            {
                                                Name = "Teach bonding position",
                                                State = AssistantStateEnum.UnAble,
                                            };
            this.MoveToBondingPosition = new AssistantState()
                                             {
                                                 Name = "Move to bonding position",
                                                 State = AssistantStateEnum.UnAble,
                                             };
            this.BondPositionAdjust = new AssistantState()
                                          {
                                              Name = "BondPosition Adjust",
                                              State = AssistantStateEnum.UnAble,
                                          };
            this.BondPositionMeasureHeight = new AssistantState()
                                                 {
                                                     Name = "BondPosition MeasureHeight",
                                                     State = AssistantStateEnum.ForBidden,
                                                 };
            this.BondPositionIdentity = new AssistantState()
            {
                Name = "BondPosition identification",
                State = AssistantStateEnum.ForBidden,
            };
        }
    }
}
