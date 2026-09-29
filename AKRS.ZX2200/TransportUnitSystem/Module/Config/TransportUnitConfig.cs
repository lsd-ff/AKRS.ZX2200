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

namespace AKRS.ZX2200.TransportUnitSystem.Module.Config
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Controls.Feature.EditProgram.UcEditProductProgram;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using DevExpress.XtraEditors;

    /// <summary>
    /// 传输单元配置文件
    /// </summary>
    public class TransportUnitConfig : SingleConfig
    {
        /// <summary>
        /// 是否为异性基板
        /// </summary>
        [TreeProgramListArgs("是否为异性基板", (string)null)]
        public bool IsOppositeSex { get; set; } = false;

        /// <summary>
        /// 数量较多
        /// </summary>
        [TreeProgramListArgs("焊点数量较多", (string)null)]
        public bool LagerNumber { get; set; } = false;

        /// <summary>
        /// 安全高度
        /// </summary>
        [TreeProgramListArgs("安全高度", "距离", 3, 30, "mm")]
        public double SafeHeight { get; set; } = 5;

        /// <summary>
        /// 增加的安全高度的形式
        /// </summary>
        [TreeProgramListArgs("增加的安全高度类型", (string)null)]
        public AdditionalSafetyHeightType AdditionalSafetyHeightType { get; set; }

        /// <summary>
        /// 增加的安全高度
        /// </summary>
        [TreeProgramListArgs("增加的安全高度", "", 3, 20, "mm")]
        public double AdditionalSafeHeight { get; set; }

        /// <summary>
        /// Substrate类型，主要用于System1是否分两端传料
        /// </summary>
        [TreeProgramListArgs("基板形态", (string)null)]
        public SubstrateProcessingEnum SubstrateProcessing { get; set; }

        /// <summary>
        /// 右边的距离
        /// 就是点胶挡料气缸2和Bond气缸的差值
        /// </summary>
        [TreeProgramListArgs("点胶挡料气缸1到挡料气缸2的距离", "距离", "mm")]
        public double DistanceToRight { get; set; }

        /// <summary>
        /// 右边定位配置
        /// </summary>
        public LocateConfig RightLocateConfig { get; set; } = new LocateConfig();

        /// <summary>
        /// 左边定位配置
        /// </summary>
        public LocateConfig LeftLocateConfig { get; set; } = new LocateConfig();

        /// <summary>
        /// ID搜索的数量
        /// </summary>
        [TreeProgramListArgs("ID搜索的数量", (string)null)]
        public int IdSearchNumber { get; set; }

        /// <summary>
        /// 预搜索
        /// </summary>
        [TreeProgramListArgs("预搜索", (string)null)]
        public bool IdPreSearch { get; set; }

        /// <summary>
        /// Mapping图是否打开
        /// </summary>
        [TreeProgramListArgs("Mapping图是否打开", (string)null)]
        public bool TuMappingEnable { get; set; }

        /// <summary>
        /// Mapping图类型
        /// </summary>
        [TreeProgramListArgs("Mapping图类型", (string)null)]
        public TuMappingEnum TuMapping { get; set; }

        /// <summary>
        /// 是否使用新的Mapping UI
        /// </summary>
        [TreeProgramListArgs("使用新的框架Mapping图界面", (string)null)]
        public bool UseNewMappingUi { get; set; } = false;

        /// <summary>
        /// 系统1偏移
        /// </summary>
        [TreeProgramListArgs("点胶1偏移", (string)null)]
        public AKRSPoint3D System1Offset { get; set; } = new AKRSPoint3D();


        /// <summary>
        /// 系统1偏移
        /// </summary>
        [TreeProgramListArgs("固晶偏移", (string)null)]
        public AKRSPoint3D System2Offset { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 根据上一块基板补偿下一块基板
        /// </summary>
        [TreeProgramListArgs("根据上一块基板补偿下一块基板","补偿功能")]
        public bool IsCompensateAutoByLast { get; set; } = false;

        /// <summary>
        /// 补偿百分比
        /// </summary>
        [TreeProgramListArgs("补偿百分比", "补偿功能", 0, 100)]
        public double CompensateAutoByLastRation { get; set; } = 50;

        /// <summary>
        /// 根据上一块基板补偿下一块基板
        /// </summary>
        [TreeProgramListArgs("每个焊点单独补偿", "补偿功能")]
        public bool IsCompensateSingleBp { get; set; } = false;

        /// <summary>
        /// 示教所有高度
        /// </summary>
        public bool AssistantAllHeight { get; set; }

        /// <summary>
        /// 示教所有角度
        /// </summary>
        public bool AssistantAllRotation { get; set; }

        /// <summary>
        /// 示教所有角度
        /// </summary>
        public bool AssistantAllForce { get; set; }

        /// <summary>
        /// TransportUnitConfig
        /// </summary>
        public TransportUnitConfig()
        {
            //this.ResetAssistantStates();
        }

        /// <summary>
        /// TUPosition
        /// </summary>
        public AssistantState TUPosition { get; set; } =
            new AssistantState() { Name = "TU position", State = AssistantStateEnum.UnAble};

        /// <summary>
        /// TUAdjust
        /// </summary>
        public AssistantState TUAdjust { get; set; } = new AssistantState() { Name = "TU adjust", State = AssistantStateEnum.ForBidden };

        /// <summary>
        /// TUIdentification
        /// </summary>
        public AssistantState TUIdentification { get; set; } = new AssistantState() { Name = "TU identification", State = AssistantStateEnum.ForBidden };

        /// <summary>
        /// TUHeightMeasurement
        /// </summary>
        public AssistantState TUHeightMeasurement { get; set; } = new AssistantState() { Name = "TU height measurement", State = AssistantStateEnum.ForBidden };

        /// <summary>
        /// SubstratePosition
        /// </summary>
        public AssistantState SubstratePosition { get; set; } = new AssistantState() { Name = "Substrate position", State = AssistantStateEnum.ForBidden };

        /// <summary>
        /// SubstrateAdjust
        /// </summary>
        public AssistantState SubstrateAdjust { get; set; } = new AssistantState() { Name = "Substrate adjust", State = AssistantStateEnum.ForBidden };

        /// <summary>
        /// SubstrateInkDot
        /// </summary>
        public AssistantState SubstrateInkDot { get; set; } = new AssistantState() { Name = "Substrate ink dot", State = AssistantStateEnum.ForBidden };

        /// <summary>
        /// SubstrateIdentification
        /// </summary>
        public AssistantState SubstrateIdentification { get; set; } = new AssistantState() { Name = "Substrate identification", State = AssistantStateEnum.ForBidden };

        /// <summary>
        /// SubstrateHeightMeasurement
        /// </summary>
        public AssistantState SubstrateHeightMeasurement { get; set; } = new AssistantState() { Name = "Substrate height measurement", State = AssistantStateEnum.ForBidden };

        /// <summary>
        /// ModulePosition
        /// </summary>
        public AssistantState ModulePosition { get; set; } = new AssistantState() { Name = "Module position", State = AssistantStateEnum.ForBidden };

        /// <summary>
        /// ModuleAdjust
        /// </summary>
        public AssistantState ModuleAdjust { get; set; } = new AssistantState() { Name = "Module adjust", State = AssistantStateEnum.ForBidden };

        /// <summary>
        /// ModuleInkDot
        /// </summary>
        public AssistantState ModuleInkDot { get; set; } = new AssistantState() { Name = "Module ink dot", State = AssistantStateEnum.ForBidden };

        /// <summary>
        /// ModuleHeightMeasurement
        /// </summary>
        public AssistantState ModuleHeightMeasurement { get; set; } = new AssistantState() { Name = "Module height measurement", State = AssistantStateEnum.ForBidden};

        /// <summary>
        /// BadModuleTable
        /// </summary>
        public AssistantState BadModuleTable { get; set; } = new AssistantState() { Name = "Bad module table", State = AssistantStateEnum.ForBidden };

        /// <summary>
        /// ModuleIdentification
        /// </summary>
        public AssistantState ModuleIdentification { get; set; } = new AssistantState() { Name = "Module identification", State = AssistantStateEnum.ForBidden };

        /// <summary>
        /// Mapping
        /// </summary>
        public AssistantState Mapping { get; set; } =
            new AssistantState() { Name = "Mapping", State = AssistantStateEnum.ForBidden };

        /// <summary>
        /// TU是否示教完成
        /// </summary>
        [JsonIgnore]
        public bool IsAssistantSucceed => this.GetAssistantResult();

        /// <summary>
        /// TU是否示教完成
        /// </summary>
        /// <returns>return</returns>
        private bool GetAssistantResult()
        {
            if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
            {
                return ProductConfiguration.GetInstance().OppositeSexConfiguration.OppositeSexConfigs.AssistantSuccess();
            }
            else
            {
                List<AssistantState> list = this.GetAssistantStates();
                foreach (var item in list)
                {
                    if (item.State == AssistantStateEnum.UnAble)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// 获取TU所有示教的状态
        /// </summary>
        /// <returns>return</returns>
        public List<AssistantState> GetAssistantStates()
        {
            List<AssistantState> list = new List<AssistantState>();
            list.Add(this.TUPosition);
            list.Add(this.TUAdjust);
            list.Add(this.TUIdentification);
            list.Add(this.TUHeightMeasurement);
            list.Add(this.SubstratePosition);
            list.Add(this.SubstrateAdjust);
            list.Add(this.SubstrateInkDot);
            list.Add(this.SubstrateIdentification);
            list.Add(this.SubstrateHeightMeasurement);
            list.Add(this.ModulePosition);
            list.Add(this.ModuleAdjust);
            list.Add(this.ModuleInkDot);
            list.Add(this.ModuleHeightMeasurement);
            list.Add(this.BadModuleTable);
            list.Add(this.Mapping); 
            list.Add(this.ModuleIdentification);

            return list;
        }

        /// <summary>
        /// 是否存在跳跃的示教
        /// </summary>
        /// <param name="name">当前需要示教的名称</param>
        /// <returns>结果</returns>
        public bool IsNormalOrderAssistant(string name)
        {
            if (this.IsAssistantSucceed)
            {
                return true;
            }

            List<AssistantState> list = this.GetAssistantStates();

            AssistantState assistantState = list.Find(it => it.ChName == name);

            int index = list.IndexOf(assistantState);

            for (int i = 0; i < index; i++)
            {
                if (list[i].State == AssistantStateEnum.UnAble)
                {
                    AKRSXtraMessageBox.Show($"请先完成步骤：{list[i].ChName}");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 初始化TU示教状态
        /// </summary>
        public void ResetAssistantStates()
        {
            foreach (AssistantState assistantState in this.GetAssistantStates())
            {
                if (assistantState.State == AssistantStateEnum.Able)
                {
                    assistantState.State = AssistantStateEnum.UnAble;
                }
            }

            this.TUPosition.State = AssistantStateEnum.UnAble;
            this.TUAdjust.State = this.LocateConfig.AdjustType == AdjustTypeEnum.None
                                      ? AssistantStateEnum.ForBidden
                                      : AssistantStateEnum.UnAble;

            this.TUHeightMeasurement.State = this.MeasureHeightInSystem1 || this.MeasureHeightInSystem2
                                                 ? AssistantStateEnum.UnAble
                                                 : AssistantStateEnum.ForBidden;
            this.SubstratePosition.State = ProductConfiguration.GetInstance().SubstrateConfig.IsMultiple 
                                               ? AssistantStateEnum.UnAble
                                               : AssistantStateEnum.ForBidden;
            this.SubstrateAdjust.State = ProductConfiguration.GetInstance().SubstrateConfig.LocateConfig.AdjustType == AdjustTypeEnum.None
                                             ? AssistantStateEnum.ForBidden
                                             : AssistantStateEnum.UnAble;
            this.SubstrateHeightMeasurement.State =
                ProductConfiguration.GetInstance().SubstrateConfig.MeasureHeightInSystem1
                || ProductConfiguration.GetInstance().SubstrateConfig.MeasureHeightInSystem2
                    ? AssistantStateEnum.UnAble
                    : AssistantStateEnum.ForBidden;

            this.ModulePosition.State = ProductConfiguration.GetInstance().ModuleConfig.IsMultiple
                                               ? AssistantStateEnum.UnAble
                                               : AssistantStateEnum.ForBidden;
            this.ModuleAdjust.State = ProductConfiguration.GetInstance().ModuleConfig.LocateConfig.AdjustType == AdjustTypeEnum.None
                                             ? AssistantStateEnum.ForBidden
                                             : AssistantStateEnum.UnAble;
            this.ModuleHeightMeasurement.State =
                ProductConfiguration.GetInstance().ModuleConfig.MeasureHeightInSystem1
                || ProductConfiguration.GetInstance().ModuleConfig.MeasureHeightInSystem2
                    ? AssistantStateEnum.UnAble
                    : AssistantStateEnum.ForBidden;

            // TODO 以下功能未开发
            this.TUIdentification.State = AssistantStateEnum.ForBidden;
            this.SubstrateIdentification.State = AssistantStateEnum.ForBidden;
            this.SubstrateInkDot.State = AssistantStateEnum.ForBidden;
            this.ModuleInkDot.State = AssistantStateEnum.ForBidden;
            this.BadModuleTable.State = AssistantStateEnum.ForBidden;
        }
    }
}
