namespace AKRS.ZX2200.Infrastructure.Models.CommonModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AKRS.ZX2200.Infrastructure.Models.Enums;

    using Newtonsoft.Json;

    /// <summary>
    /// 示教状态类
    /// </summary>
    [Serializable]
    public class AssistantState
    {
        /// <summary>
        /// 示教名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 示教状态
        /// </summary>
        public AssistantStateEnum State { get; set; } = AssistantStateEnum.UnAble;

        /// <summary>
        /// 示教的中文名称
        /// </summary>
        [JsonIgnore]
        public string ChName => this.GetChName();

        /// <summary>
        /// 中英对应字典
        /// </summary>
        [JsonIgnore]
        private static Dictionary<string, string> chNameDic;

        /// <summary>
        /// 获取中文名称
        /// </summary>
        /// <returns>中文名称</returns>
        private string GetChName()
        {
            string chName = GetAllChNameDic()[this.Name];

            if (chName == null)
            {
                throw new Exception($"未找到名为 {this.Name} 对应的中文示教名称");
            }

            return chName;
        }

        /// <summary>
        /// 获取所有对应的中英互译
        /// </summary>
        /// <returns>结果</returns>
        public static Dictionary<string, string> GetAllChNameDic()
        {
            if (chNameDic != null)
            {
                return chNameDic;
            }

            chNameDic = new Dictionary<string, string>
                            {
                                // 基板
                                { "TU position", "框架位置示教" },
                                { "TU adjust", "框架视觉矫正示教" },
                                { "TU identification", "框架ID识别示教" },
                                { "TU height measurement", "框架测高示教" },
                                { "Substrate position", "基板位置示教" },
                                { "Substrate adjust", "基板视觉矫正示教" },
                                { "Substrate ink dot", "基板墨点示教" },
                                { "Substrate identification", "基板ID识别示教" },
                                { "Substrate height measurement", "基板测高示教" },
                                { "Module position", "基岛位置示教" },
                                { "Module adjust", "基岛视觉矫正示教" },
                                { "Module ink dot", "基岛墨点示教" },
                                { "Module height measurement", "基岛测高示教" },
                                { "Bad module table", "不良基岛示教" },
                                { "Mapping", "Mapping" },
                                { "Module identification", "基岛ID识别示教" },

                                // 焊点
                                { "Teach bonding position", "焊点位置示教" },
                                { "Move to bonding position", "移动到焊点位置" },
                                { "BondPosition Adjust", "焊点视觉矫正示教" },
                                { "BondPosition MeasureHeight", "焊点测高示教" },
                                { "BondPosition identification", "焊点ID识别示教" },

                                // 吸嘴
                                { "Tool height", "吸嘴高度示教" },
                                { "Tool alignment", "吸嘴旋转中心示教" },
                                { "Tool geometry", "吸嘴几何中心示教" },

                                // 顶针
                                { "Height measurement", "顶针高度示教" },
                                { "Needle zero position", "顶针零位示教" },
                                { "XY position", "顶针XY位置示教" },

                                // 点胶针
                                { "Dispenser", "点胶针示教" },

                                // 翻转工具
                                { "FlipTool", "翻转工具示教" },

                                // 焊后
                                { "Post-bond", "检测示教" },
                                { "BacksideCrackDetection", "背崩检测" },

                                // 上下料
                                { "Loader", "上料位置示教" },
                                { "Unloader", "下料位置示教" },

                                // 晶圆系统
                                { "Wafer magazine geometry", "晶圆料盒示教" },
                                { "Wafer change", "换晶圆示教" },
                                { "Wafer table collision circle", "晶圆台环限位示教" },

                                // 晶圆
                                { "Component geometry", "芯片形状示教" },
                                { "Flip to wafer", "翻转工具取晶位示教" },
                                { "Component wafer map", "芯片图示教" },
                                { "Ink dot", "芯片墨点示教" },
                                { "Wafer edge", "芯片环形限位" },
                                { "Reference die", "参考点" },
                                { "Component transportUnit geometry", "芯片载具形状" },
                                { "bad-component table", "不良芯片图" },
                                { "Component transportUnit adjust", "芯片矫正" },
                                { "Component pickup", "取芯片" },
                                { "Component accuracy mode", "芯片精度模式" },
                                { "Component placement", "芯片贴片" }
                            };
            return chNameDic;
        }
    }
}
