using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.ZX2200.Infrastructure.Models.BaseModels;

namespace AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.SupportFeature.Consumables;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using Newtonsoft.Json;

    /// <summary>
    /// 翻转工具（倒装）
    /// </summary>
    [Serializable]

    public class FlipTool : BaseDsSetting
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public FlipTool()
        {
            this.ResetAssistantStates();
        }

        /// <summary>
        /// 测高吸嘴
        /// </summary>
        [TreeProgramListArgs("翻转工具测高用的吸嘴", "翻转工具")]
        public string MeasureHeightNozzle { get; set; } = string.Empty;

        ///// <summary>
        ///// 测高吸嘴
        ///// </summary>
        //[TreeProgramListArgs("翻转工具测高用的顶针", "翻转工具")]
        //[JsonIgnore]
        //public string MeasureHeightEjection { get; set; } = string.Empty;

        ///// <summary>
        ///// 测高吸嘴
        ///// </summary>
        //[TreeProgramListArgs("需要翻转的芯片", "翻转工具")]
        //public string FlipChipName { get; set; } = string.Empty;

        /// <summary>
        /// 翻转工具交接位
        /// </summary>
        [TreeProgramListArgs("翻转工具交接位", "翻转工具")]
        public double FlipToolTransferPos { get; set; } = 0;

        /// <summary>
        /// XY位置
        /// </summary>
        [TreeProgramListArgs("XY位置", "翻转工具")]
        public AKRSPoint2D FlipToolPositionXY { get; set; } = new AKRSPoint2D();

        /// <summary>
        /// Z位置
        /// </summary>
        [TreeProgramListArgs("Z位置", "翻转工具")]
        public double FlipToolPositionZ{ get; set; }

        /// <summary>
        /// 单位是°应该是T轴转多少度
        /// 默认180度
        /// </summary>
        [TreeProgramListArgs("芯片接触位置", "翻转工具")]
        public double FlipToolWaferHeight { get; set; } = 180;

        /// <summary>
        /// 拾取位置
        /// </summary>
        public AKRSPoint2D FlipToolPickupPosition { get; set; } = new AKRSPoint2D();

        /// <summary>
        /// 标定位置
        /// </summary>
        public AKRSPoint3D FlipToolCalibrationPosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 翻转工具耗材
        /// </summary>
        public FrequencyConsumables Frequency { get; set; }

        /// <summary>
        /// DispenserAssistant
        /// </summary>
        public AssistantState FlipToolAssistant { get; set; }

        /// <summary>
        /// 吹气比例
        /// </summary>
        [TreeProgramListArgs("吹气比例", "翻转工具", 1, 200000.0)]
        public int BlowProportion { get; set; } = 150;


        /// <summary>
        /// 吹气延时
        /// </summary>
        [TreeProgramListArgs("吹气延时", "翻转工具", 0, 200000.0)]
        public int BlowDelay { get; set; } = 150;

        /// <summary>
        /// 漏晶检测值
        /// </summary>
        [TreeProgramListArgs("漏晶检测值", "翻转工具", 0, 200000.0)]
        public int ComponentCheckVal { get; set; }


        /// <summary>
        /// 漏晶检测延时
        /// </summary>
        [TreeProgramListArgs("漏晶检测延时", "翻转工具", 0, 200000.0, UnitHelper.ms)]
        public int ComponentCheckDelay { get; set; }

        /// <summary>
        /// 是否示教完成
        /// </summary>
        [JsonIgnore]
        public bool IsAssistantSucceed => this.GetAssistantResult();

        /// <summary>
        /// 是否示教完成
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
        /// 获取翻转工具所有示教的状态
        /// </summary>
        /// <returns>return</returns>
        public List<AssistantState> GetAssistantStates()
        {
            List<AssistantState> list = new List<AssistantState>();
            list.Add(this.FlipToolAssistant);

            return list;
        }

        /// <summary>
        /// 初始化翻转工具示教状态
        /// </summary>
        public void ResetAssistantStates()
        {
            this.FlipToolAssistant = new AssistantState() { Name = "FlipTool", State = AssistantStateEnum.UnAble };
        }
    }
}
