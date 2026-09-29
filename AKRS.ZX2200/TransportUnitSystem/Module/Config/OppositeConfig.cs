using AKRS.Galaxy2.Infrastructure.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Config
{
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    using Newtonsoft.Json;

    /// <summary>
    /// 异性配置文件
    /// </summary>
    public class OppositeSexConfig : BaseConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否点胶
        /// </summary>
        public bool Dispense { get; set; }

        /// <summary>
        /// 是否贴片
        /// </summary>
        public bool BondComponent { get; set; }

        /// <summary>
        /// 是否胶检
        /// </summary>
        public bool EpoxyCheck { get; set; }

        /// <summary>
        /// 是否焊后
        /// </summary>
        public bool AfterBond { get; set; }

        /// <summary>
        /// 点胶名称
        /// </summary>
        public string DispenseName { get; set; }

        /// <summary>
        /// 贴片名称
        /// </summary>
        public string BondComponentName { get; set; }

        /// <summary>
        /// 胶检名称
        /// </summary>
        public string EpoxyCheckName { get; set; }

        /// <summary>
        /// 焊后名称
        /// </summary>
        public string AfterBondName { get; set; }

        /// <summary>
        /// 视觉否示教成功
        /// </summary>
        public bool IsPrAssistant { get; set; } = true;

        /// <summary>
        /// 测高是否示教成功
        /// </summary>
        public bool IsMeasureHeightAssistant { get; set; } = true;

        /// <summary>
        /// 胶量检测是否成功
        /// </summary>
        [JsonIgnore]
        public bool IsEpoxyCheckAssistant => this.GetIsEpoxyCheckAssistant();

        /// <summary>
        /// 焊后检测是否成功
        /// </summary>
        [JsonIgnore]
        public bool IsAfterBondAssistant => this.GetIsIsAfterBondAssistant();

        /// <summary>
        /// 胶量检测是否示教成功
        /// </summary>
        /// <returns>结果</returns>
        public bool GetIsEpoxyCheckAssistant()
        {
            if (!this.EpoxyCheck)
            {
                return true;
            }
            else
            {
                PostBondInspection postBondInspection =
                    (PostBondInspection)PostBondInspectionRepository.GetInstance().Find(this.EpoxyCheckName);
                if (postBondInspection == null)
                {
                    return false;
                }

                return postBondInspection.IsAssistantSucceed;
            }
        }

        /// <summary>
        /// 胶量检测是否示教成功
        /// </summary>
        /// <returns>结果</returns>
        public bool GetIsIsAfterBondAssistant()
        {
            if (!this.AfterBond)
            {
                return true;
            }
            else
            {
                PostBondInspection postBondInspection =
                    (PostBondInspection)PostBondInspectionRepository.GetInstance().Find(this.AfterBondName);
                if (postBondInspection == null)
                {
                    return false;
                }

                return postBondInspection.IsAssistantSucceed;
            }
        }

        /// <summary>
        /// 位置补偿
        /// </summary>
        public AKRSPoint3D PositionCompensate { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 角度补偿
        /// </summary>
        public double AngleCompensate { get; set; }

        /// <summary>
        /// 点胶位置
        /// </summary>
        public AKRSPoint3D DispensePositionCompensate { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 点胶角度补偿
        /// </summary>
        public double DispenseAngleCompensate { get; set; }

        /// <summary>
        /// 是不是镜像
        /// </summary>
        [TreeProgramListArgs("镜像焊点")]
        public bool IsMirror { get; set; } = false;
    }
}
