using AKRS.ZX2200.Models;

namespace AKRS.ZX2200.DispenseSystem.Models.Repositories.Dispenser
{
    using System;
    using System.Collections.Generic;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.Consumables;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.SupportFeature.Consumables;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    using Newtonsoft.Json;

    /// <summary>
    /// 点胶头
    /// </summary>
    [Serializable]
    public class Dispenser : BaseDsSetting
    {
        /// <summary>
        /// 点胶器类型
        /// </summary>
        public DispenserTypeEnum DispenserType { get; set; }
        
        /// <summary>
        /// 流动速度
        /// </summary>
        public bool EpoxyFlowRate { get; set; }


        /// <summary>
        /// 标定前是不是需要去预点胶
        /// 这个仅在做程式的时候去使用
        /// </summary>
        public bool PreDispenseBeforeCalibration { get; set; }


        /// <summary>
        /// 预点胶参数
        /// </summary>
        public PreDispense PreDispense { get; set; } = new PreDispense();

        #region Dispenser Calibration

        /// <summary>
        /// 搜索模式
        /// </summary>
        public DispenseSearchMethodEnum DispenseSearchMethod { get; set; }

        /// <summary>
        /// 点胶测高方式
        /// 这个仅在页面进行展示
        /// </summary>
        public DispenseDeterminationZTypeEnum DispenseDeterminationZType { get; set; }

        /// <summary>
        /// 点胶标定方式
        /// </summary>
        public MeasureDistanceDispenserAndVisionTypeEnum MeasureDistanceDispenserAndVisionType { get; set; }


        #endregion


        #region 已使用参数

        /// <summary>
        /// 点胶头相对于相机中心的差值
        /// </summary>
        [TreeProgramListArgs("点胶头相对于相机中心的差值", (string)null)]
        public AKRSPoint3D DispensingNeedleOffset { get; set; } = new AKRSPoint3D(0, -62, 15);

        #endregion

        /// <summary>
        /// 构造方法
        /// </summary>
        public Dispenser()
        {
            this.ResetAssistantStates();

            this.DispenseTimes.Name = this.Name;
        }

        /// <summary>
        /// 焊材
        /// </summary>
        public FrequencyConsumables DispenseTimes { get; set; } = new FrequencyConsumables("点胶头");

        /// <summary>
        /// DispenserAssistant
        /// </summary>
        public AssistantState DispenserAssistant { get; set; }

        /// <summary>
        /// 点胶器是否示教完成
        /// </summary>
        [JsonIgnore]
        public bool IsAssistantSucceed => this.GetAssistantResult();

        /// <summary>
        /// 点胶器是否示教完成
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
        /// 获取点胶器所有示教的状态
        /// </summary>
        /// <returns>return</returns>
        public List<AssistantState> GetAssistantStates()
        {
            List<AssistantState> list = new List<AssistantState>();
            list.Add(this.DispenserAssistant);

            return list;
        }

        /// <summary>
        /// 初始化点胶器示教状态
        /// </summary>
        public void ResetAssistantStates()
        {
            this.DispenserAssistant = new AssistantState() { Name = "Dispenser", State = AssistantStateEnum.UnAble };
        }
    }
}
