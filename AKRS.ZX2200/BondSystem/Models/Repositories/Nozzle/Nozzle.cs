using System;
using System.Collections.Generic;
using System.Text;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.Consumables;
using AKRS.ZX2200.Models;

using Newtonsoft.Json;

namespace AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.SupportFeature.Consumables;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    /// <summary>
    /// 吸嘴
    /// </summary>
    [Serializable]
    public class Nozzle : BaseDsSetting
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">吸嘴名称</param>
        public Nozzle(string name)
        {
            this.Name = name;
            this.FrequencyConsumables = new FrequencyConsumables(name);
            this.ResetAssistantStates();
        }

        /// <summary>
        /// Nozzle
        /// </summary>
        public Nozzle()
        {
            this.ResetAssistantStates();
        }

        /// <summary>
        /// 吸嘴类型枚举
        /// </summary>
        [TreeProgramListArgs("吸嘴类型", (string)null)]
        public NozzleTypeEnum NozzleType { get; set; } = NozzleTypeEnum.PickAndPlace;

        /// <summary>
        /// 吸嘴尺寸枚举
        /// </summary>
        [TreeProgramListArgs("吸嘴尺寸", (string)null, false)]
        public NozzleSizeEnum NozzleSize { get; set; }


        /// <summary>
        /// 吸嘴几何数据（长、宽、半径）
        /// </summary>
        [TreeProgramListArgs("吸嘴几何尺寸", (string)null, false)]
        public AKRSPoint3D ToolGeometry { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 吸嘴形状枚举
        /// </summary>
        [TreeProgramListArgs("吸嘴形状", (string)null)]
        public NozzleShapeEnum NozzleShape { get; set; }


        /// <summary>
        /// 此吸嘴和BMC平台标准吸嘴的高度补偿  表示吸嘴比BMC吸嘴长了多少
        /// </summary>
        [TreeProgramListArgs("吸嘴测高结果", (string)null, false)]
        public double MeasureHeightOffset { get; set; } = 0;

        /// <summary>
        /// 吸嘴校准角度
        /// </summary>
        [TreeProgramListArgs("吸嘴校准角度", (string)null, false)]
        public double AlignAngle { get; set; } = 0;

        /// <summary>
        /// 漏晶检测值
        /// </summary>
        [TreeProgramListArgs("漏晶检测值", (string)null)]
        public int AfterPickupVacuumCheckValue { get; set; } = 4000;

        /// <summary>
        /// 回带检测值
        /// </summary>
        [TreeProgramListArgs("回带检测值", (string)null)]
        public int AfterBondingVacuumCheckValue { get; set; } = 4000;

        /// <summary>
        /// 吸嘴漏晶检测延时
        /// </summary>
        [TreeProgramListArgs("吸嘴漏晶检测延时", (string)null, 0, 10000000, UnitHelper.ms)]
        public int AfterPickupVacuumCheckDelay { get; set; }

        /// <summary>
        /// 吸嘴回带检测延时
        /// </summary>
        [TreeProgramListArgs("吸嘴回带检测延时", (string)null, 0, 10000000, UnitHelper.ms)]
        public int AfterBondingVacuumCheckDelay { get; set; }

        /// <summary>
        /// 在吸嘴架上的槽位号
        /// </summary>
        [TreeProgramListArgs("在吸嘴架上的槽位号", (string)null, false)]
        public int SlotIdentification { get; set; } = -1;

        /// <summary>
        /// 吸嘴在此BondHead上的旋转半径 偏移数据
        /// </summary>
        [TreeProgramListArgs("吸嘴XY校准数据", (string)null, false)]
        public AKRSPoint2D NozzleOffset { get; set; } = new AKRSPoint2D();

        /// <summary>
        /// 矩形吸嘴的角跟焊头旋转中心的偏移
        /// </summary>
        public AKRSPoint2D RectangleNozzleCornerOffset { get; set; } = new AKRSPoint2D();

        /// <summary>
        /// 吸嘴PR模板名称
        /// </summary>
        public string NozzlePRName => this.GetNozzlePRName();

        /// <summary>
        /// 吸嘴使用次数
        /// </summary>
        public int NozzleUseCount { get; set; } = 0;

        /// <summary>
        /// GetDieMatchName
        /// </summary>
        /// <returns>result</returns>
        private string GetNozzlePRName()
        {
            return this.Name + "NozzlePR";
        }

        /// <summary>
        /// 吸嘴测高模式枚举
        /// </summary>
        public HeightMeasurementFunctionEnum NozzleHeightMeasurementFunction { get; set; } =
            HeightMeasurementFunctionEnum.WithTDSensor;

        /// <summary>
        /// 吸嘴测高方式枚举
        /// </summary>
        public ZDeterminationMethodEnum ZDeterminationMethod { get; set; } = ZDeterminationMethodEnum.MiniBMC;

        /// <summary>
        /// XY轴测定方法枚举
        /// </summary>
        public XYDeterminationMethodEnum XYDeterminationMethod { get; set; }

        /// <summary>
        /// 吸嘴旋转中心位置,关于G0
        /// </summary>
        public AKRSPoint3D RotateCenterPos { get; set; } = new AKRSPoint3D();


        /// <summary>
        /// 吸嘴中心对准相机中心的位置,关于G0
        /// </summary>
        [TreeProgramListArgs("吸嘴中心对准相机中心的位置", (string)null, false)]
        public AKRSPoint3D NozzleToUplookCenterPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 是否对吸嘴进行重新编程，暂时不用
        /// </summary>
        public bool IsReprogramTool { get; set; } = false;

        /// <summary>
        /// 是否启用吸嘴测高数据，针对深腔吸嘴，暂时不用
        /// </summary>
        public bool IsHeightMeasurement { get; set; }


        /// <summary>
        /// 用反射输出此吸嘴所有属性值 
        /// </summary>
        /// <returns>属性值</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            // 遍历吸嘴属性
            foreach (System.Reflection.PropertyInfo p in this.GetType().GetProperties())
            {
                stringBuilder.Append(p.Name + ": " + p.GetValue(this) + "\r\n");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// ToolHeight
        /// </summary>
        public AssistantState ToolHeight { get; set; }

        /// <summary>
        /// ToolAlignment
        /// </summary>
        public AssistantState ToolAlignment { get; set; }

        /// <summary>
        /// ToolGeometryAssistant
        /// </summary>
        public AssistantState ToolGeometryAssistant { get; set; }

        /// <summary>
        /// 使用频率
        /// </summary>
        public FrequencyConsumables FrequencyConsumables { get; set; }

        /// <summary>
        /// 吸嘴是否示教完成
        /// </summary>
        [JsonIgnore]
        public bool IsAssistantSucceed => this.GetAssistantResult();

        /// <summary>
        /// 吸嘴是否示教完成
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
        /// 获取吸嘴所有示教的状态
        /// </summary>
        /// <returns>return</returns>
        public List<AssistantState> GetAssistantStates()
        {
            List<AssistantState> list = new List<AssistantState>();
            list.Add(this.ToolHeight);
            list.Add(this.ToolAlignment);
            list.Add(this.ToolGeometryAssistant);

            return list;
        }

        /// <summary>
        /// 初始化吸嘴示教状态
        /// </summary>
        public void ResetAssistantStates()
        {
            this.ToolHeight = new AssistantState() { Name = "Tool height", State = AssistantStateEnum.UnAble };
            this.ToolAlignment = new AssistantState() { Name = "Tool alignment", State = AssistantStateEnum.UnAble };
            this.ToolGeometryAssistant = new AssistantState() { Name = "Tool geometry", State = AssistantStateEnum.UnAble };
        }
    }
}
