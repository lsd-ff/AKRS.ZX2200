using AKRS.ZX2200.Models;

using Newtonsoft.Json;
using System.Collections.Generic;

namespace AKRS.ZX2200.TransportSystem.Models.DatasetModels.StockBin
{
    using System;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using System;

    /// <summary>
    /// 下料盒
    /// </summary>
    [Serializable]
    public class UnloaderBin : BaseDsSetting
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnloaderBin()
        {
            this.ResetAssistantStates();
        }

        /// <summary>
        /// 第一层料片高度
        /// </summary>
        [TreeProgramListArgs("第一层料片高度", (string)null, false, "mm")]
        public double FirstTabletLevel { get; set; }

        /// <summary>
        /// 最后一层料片高度
        /// </summary>
        [TreeProgramListArgs("最后一层料片高度", (string)null, false, "mm")]
        public double LastTabletLevel { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        [TreeProgramListArgs("层数", (string)null, false)]
        public double LayerNum { get; set; }

        /// <summary>
        /// 料片间距
        /// </summary>
        [TreeProgramListArgs("料片间距", (string)null, false, "mm")]
        public double TabletPitch { get; set; }

        /// <summary>
        /// A料盒位置(Y向)
        /// </summary>
        [TreeProgramListArgs("A料盒位置(Y向)", (string)null, -10000, 10000, "mm")]
        public double BinAPosY { get; set; }

        /// <summary>
        /// B料盒位置(Y向)
        /// </summary>
        [TreeProgramListArgs("B料盒位置(Y向)", (string)null, -10000, 10000, "mm")]
        public double BinBPosY { get; set; }

        /// <summary>
        /// 推杆退料位(X向)
        /// </summary>
        [TreeProgramListArgs("推杆推料位(X向)", (string)null, -10000, 10000, "mm")]
        public double PushPos { get; set; }

        /// <summary>
        /// Unloader
        /// </summary>
        public AssistantState Unloader { get; set; }

        /// <summary>
        /// Unloader是否示教完成
        /// </summary>
        [JsonIgnore]
        public bool IsAssistantSucceed => this.GetAssistantResult();

        /// <summary>
        /// Unloader是否示教完成
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
        /// 获取Unloader所有示教的状态
        /// </summary>
        /// <returns>return</returns>
        public List<AssistantState> GetAssistantStates()
        {
            List<AssistantState> list = new List<AssistantState>();
            list.Add(this.Unloader);

            return list;
        }

        /// <summary>
        /// 初始化Unloader示教状态
        /// </summary>
        public void ResetAssistantStates()
        {
            this.Unloader = new AssistantState() { Name = "Unloader", State = AssistantStateEnum.UnAble };
        }
    }
}
