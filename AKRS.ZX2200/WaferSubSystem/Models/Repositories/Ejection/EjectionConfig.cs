using System;
using System.Collections.Generic;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection
{
    using AKRS.ZX2200.Consumables;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.SupportFeature.Consumables;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;

    /// <summary>
    /// 顶针类
    /// </summary>
    [Serializable]
    public class EjectionConfig : BaseDsSetting
    {
        /// <summary>
        /// 顶针类型
        /// </summary>
        [TreeProgramListArgs("顶针类型")]
        public EjectionTypeEnum EjectionType { get; set; }

        /// <summary>
        /// 顶针帽直径
        /// </summary>
        [TreeProgramListArgs("顶针帽直径", "", UnitHelper.mm)]
        public double CapDiameter { get; set; }

        /// <summary>
        /// 顶针台测高位置
        /// </summary>
        [TreeProgramListArgs("顶针台测高位置", "顶针设置", false, UnitHelper.mm)]
        public AKRSPoint3D EjectionTableMeasureHeightPosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 顶针台工作位置
        /// </summary>
        [TreeProgramListArgs("顶针台工作位置", "顶针设置", false, UnitHelper.mm)]
        public AKRSPoint3D EjectionTableWorkPosition { get; set; } = new AKRSPoint3D(0, 0, WaferSubDevicePara.GetInstance().EjectDevicePara.UpDownMarkPosition.Z);

        /// <summary>
        /// 预顶起位置-与顶针帽平齐位
        /// </summary>
        [TreeProgramListArgs("顶针预顶起位置", "顶针设置", UnitHelper.mm)]
        public AKRSPoint3D ReadyLiftPosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// Bond预取料位置
        /// </summary>
        [TreeProgramListArgs("Bond预取料位置", "顶针设置", false, UnitHelper.mm)]
        public AKRSPoint3D ReadyBondPickPosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 顶针中心与晶圆相机中心的相对偏差
        /// </summary>
        [TreeProgramListArgs("顶针中心与晶圆相机中心的相对偏差", "顶针设置", UnitHelper.mm)]
        public AKRSPoint3D DeviationWithEjectionCenterAndWaferCameraCenter { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 顶针像素坐标
        /// </summary>
        public AKRSPoint3D EjectionPixelPos { get; set; } = new AKRSPoint3D(1224, 1024, 0);

        /// <summary>
        /// 顶针中心识别模板
        /// </summary>
        [JsonIgnore]
        [TreeProgramListArgs("顶针中心识别模板名称", "顶针校准", false)]
        public string EjectMatchName => this.GetEjectMatchName();

        /// <summary>
        /// 顶针校准高度 G0
        /// </summary>
        [TreeProgramListArgs("顶针校准高度", "顶针校准", false, UnitHelper.mm)]
        public AKRSPoint3D EjectionCaliLevel { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 顶针校准拍照位 G0
        /// 相机拍照的位置
        /// </summary>
        [TreeProgramListArgs("顶针校准拍照位", "顶针校准", false, UnitHelper.mm)]
        public AKRSPoint3D EjectionCaliVsionPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 顶针耗材
        /// </summary>
        public FrequencyConsumables Frequency { get; set; }

        /// <summary>
        /// 获取顶针中心识别模板名称
        /// </summary>
        /// <returns>result</returns>
        private string GetEjectMatchName()
        {
            return this.Name + "EjectMatch";
        }

        /// <summary>
        /// EjectionConfig
        /// </summary>
        public EjectionConfig()
        {
            this.ResetAssistantStates();
        }

        /// <summary>
        /// HeightMeasurement
        /// </summary>
        public AssistantState HeightMeasurement { get; set; }

        /// <summary>
        /// NeedleZeroPosition
        /// </summary>
        public AssistantState NeedleZeroPosition { get; set; }

        /// <summary>
        /// XYPosition
        /// </summary>
        public AssistantState XYPosition { get; set; }

        /// <summary>
        /// 顶针模板示教
        /// </summary>
        public AssistantState EjectionPRTeach { get; set; } =new AssistantState() { Name = "EjectionPRTeach", State = AssistantStateEnum.UnAble };

        /// <summary>
        /// 顶针是否示教完成
        /// </summary>
        [JsonIgnore]
        public bool IsAssistantSucceed => this.GetAssistantResult();

        /// <summary>
        /// 顶针是否示教完成
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
        /// 获取顶针所有示教的状态
        /// </summary>
        /// <returns>return</returns>
        public List<AssistantState> GetAssistantStates()
        {
            List<AssistantState> list = new List<AssistantState>();
            list.Add(this.HeightMeasurement);
            list.Add(this.NeedleZeroPosition);
            list.Add(this.XYPosition);

            return list;
        }

        /// <summary>
        /// 初始化顶针示教状态
        /// </summary>
        public void ResetAssistantStates()
        {
            this.HeightMeasurement = new AssistantState() { Name = "Height measurement", State = AssistantStateEnum.UnAble };
            this.NeedleZeroPosition = new AssistantState() { Name = "Needle zero position", State = AssistantStateEnum.UnAble };
            this.XYPosition = new AssistantState() { Name = "XY position", State = AssistantStateEnum.UnAble };
            this.EjectionPRTeach = new AssistantState() { Name = "EjectionPRTeach", State = AssistantStateEnum.UnAble };
        }
    }
}
