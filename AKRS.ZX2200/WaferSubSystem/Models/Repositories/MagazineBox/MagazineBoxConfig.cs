using System;
using System.Collections.Generic;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBoxGeo;
using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBox
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    /// <summary>
    /// 料架类
    /// </summary>
    [Serializable]
    public class MagazineBoxConfig : BaseDsSetting
    {
        /// <summary>
        /// MagazineBoxGeo名称
        /// </summary>
        public string MagazineBoxGeoName { get; set; }

        /// <summary>
        /// MagazineBoxGeo
        /// </summary>
        [JsonIgnore]
        public MagazineBoxGeoConfig MagazineBoxGeo => MagazineBoxGeoConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == this.MagazineBoxGeoName);

        /// <summary>
        /// 推杆
        /// </summary>
        [TreeProgramListArgs("是否使用推杆", "上料盒功能设置")]
        public bool Push { get; set; } = false;

        /// <summary>
        /// 方向
        /// </summary>
        [TreeProgramListArgs("层数方向", "上料盒功能设置")]
        public MagazineDirectionEnum Direction { get; set; } = MagazineDirectionEnum.TopToDrow;

        /// <summary>
        /// 偏移量
        /// </summary>
        [TreeProgramListArgs("感应到料片后的偏移", "上料盒功能设置", UnitHelper.mm)]
        public double Offset { get; set; }

        /// <summary>
        /// MagazineBoxConfig
        /// </summary>
        public MagazineBoxConfig()
        {
            this.ResetAssistantStates();
        }

        /// <summary>
        /// WaferMagazineGeometry
        /// </summary>
        public AssistantState WaferMagazineGeometry { get; set; }

        /// <summary>
        /// WaferChange
        /// </summary>
        public AssistantState WaferChange { get; set; }

        /// <summary>
        /// WaferTableCollisionCircle
        /// </summary>
        public AssistantState WaferTableCollisionCircle { get; set; }

        /// <summary>
        /// MagazineBox是否示教完成
        /// </summary>
        [JsonIgnore]
        public bool IsAssistantSucceed => this.GetAssistantResult();

        /// <summary>
        /// MagazineBox是否示教完成
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
        /// 获取MagazineBox所有示教的状态
        /// </summary>
        /// <returns>return</returns>
        public List<AssistantState> GetAssistantStates()
        {
            List<AssistantState> list = new List<AssistantState>();
            list.Add(this.WaferMagazineGeometry);
            list.Add(this.WaferChange);
            list.Add(this.WaferTableCollisionCircle);

            return list;
        }

        /// <summary>
        /// 初始化MagazineBox示教状态
        /// </summary>
        public void ResetAssistantStates()
        {
            this.WaferMagazineGeometry = new AssistantState() { Name = "Wafer magazine geometry", State = AssistantStateEnum.UnAble };
            this.WaferChange = new AssistantState() { Name = "Wafer change", State = AssistantStateEnum.UnAble };
            this.WaferTableCollisionCircle = new AssistantState() { Name = "Wafer table collision circle", State = AssistantStateEnum.UnAble };
        }
    }
}
