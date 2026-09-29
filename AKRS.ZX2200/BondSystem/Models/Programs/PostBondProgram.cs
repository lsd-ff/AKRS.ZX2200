using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.ZX2200.Models;

namespace AKRS.ZX2200.BondSystem.Models.Programs
{
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using Newtonsoft.Json;

    /// <summary>
    /// 焊后程式
    /// </summary>
    public class PostBondProgram : BaseTool
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
        }

        /// <summary>
        /// 焊后集合
        /// </summary>
        [JsonIgnore]
        public List<PostBondInspection> PostBondInspections => this.Find(this.PostBondInspectionLists);

        /// <summary>
        /// 焊后集合
        /// </summary>
        public List<string> PostBondInspectionLists { get; set; } = new List<string>();


        /// <summary>
        /// 获取
        /// </summary>
        /// <returns>结果</returns>
        public List<string> GetPostBondInspectionNames()
        {
            List<string> strings = new List<string>();

            foreach (PostBondInspection PostBondInspection in PostBondInspections)
            {
                strings.Add(PostBondInspection.VisionConfig.P1PRName);
                strings.Add(PostBondInspection.VisionConfig.BacsideCrackDetectPRName);
            }

            return strings;
        }

        /// <summary>
        /// 根据名称是寻找对应的图形
        /// </summary>
        /// <param name="postBondInspectionLists">图形名称</param>
        /// <returns>图形集</returns>
        private List<PostBondInspection> Find(List<string> postBondInspectionLists)
        {
            List<PostBondInspection> postBondInspections = new List<PostBondInspection>();

            if (postBondInspectionLists != null)
            {
                for (int i = 0; i < postBondInspectionLists.Count; i++)
                {
                    PostBondInspection postBondInspection =
                        (PostBondInspection)PostBondInspectionRepository.GetInstance().Find(postBondInspectionLists[i]);
                    if (postBondInspection != null)
                    {
                        postBondInspections.Add(postBondInspection);
                    }
                    else
                    {
                        // 如果没有找到，说明库里面已经不存在了，直接删除
                        postBondInspectionLists.RemoveAt(i);
                    }
                }
            }

            return postBondInspections;
        }

        /// <summary>
        /// 判断焊后是否能正常工作
        /// </summary>
        /// <returns>结果</returns>
        public override bool CanWorkProperly()
        {
            foreach (var postBond in this.PostBondInspections)
            {
                // 先判断一下这个焊后的参数有没有编辑好
                if (postBond.EditState != EditStateEnum.Completely)
                {
                    return false;
                }
            }
            
            return true;
        }

        /// <summary>
        /// 补偿是否开始
        /// </summary>
        /// <returns>结构</returns>
        public bool IsCompensateOpen()
        {
            foreach (PostBondInspection postBondInspection in this.PostBondInspections)
            {
                if (postBondInspection.IsBondPostCompensation)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
