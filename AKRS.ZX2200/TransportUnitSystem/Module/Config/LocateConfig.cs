using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.TransportUnitSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Config
{
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    /// <summary>
    /// 定位配置对象
    /// </summary>
    public class LocateConfig
    {
        /// <summary>
        /// 定位配置的程序
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 校正类型
        /// </summary>
        [TreeProgramListArgs("AdjustType", (string)null)]
        public AdjustTypeEnum AdjustType { get; set; } = AdjustTypeEnum.None;

        /// <summary>
        /// SearchType
        /// </summary>
        [TreeProgramListArgs("SearchType", (string)null)]
        public SearchTypeEnum SearchType { get; set; } = SearchTypeEnum.StandardSearch;

        /// <summary>
        /// P1 PR名称
        /// </summary>
        [TreeProgramListArgs("P1PRName", (string)null)]
        public string P1PRName => MachineConfigContext.GetInstance().RecipeName + this.Name + "Mark1";

        /// <summary>
        /// P1 视觉定位点位
        /// 相对坐标
        /// </summary>
        [TreeProgramListArgs("P1VisionRelativePos", (string)null)]
        public AKRSPoint3D P1VisionRelativePos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// P2 PR名称
        /// </summary>
        [TreeProgramListArgs("P2PRName", (string)null)]
        public string P2PRName => MachineConfigContext.GetInstance().RecipeName + this.Name + "Mark2";

        /// <summary>
        /// P2 视觉定位点位
        /// 相对坐标
        /// </summary>
        [TreeProgramListArgs("P2VisionRelativePos", (string)null)]
        public AKRSPoint3D P2VisionRelativePos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// P3 PR名称
        /// </summary>
        [TreeProgramListArgs("P3PRName", (string)null)]
        public string P3PRName => MachineConfigContext.GetInstance().RecipeName + this.Name + "Mark3";

        /// <summary>
        /// P3 视觉定位点位
        /// 相对坐标
        /// </summary>
        [TreeProgramListArgs("P3VisionRelativePos", (string)null)]
        public AKRSPoint3D P3VisionRelativePos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// P3 PR名称
        /// </summary>
        [TreeProgramListArgs("P4PRName", (string)null)]
        public string P4PRName => MachineConfigContext.GetInstance().RecipeName + this.Name + "Mark4";

        /// <summary>
        /// P3 视觉定位点位
        /// 相对坐标
        /// </summary>
        [TreeProgramListArgs("P4VisionRelativePos", (string)null)]
        public AKRSPoint3D P4VisionRelativePos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 距离容差
        /// </summary>
        [TreeProgramListArgs("DistanceCheck", (string)null)]
        public bool DistanceCheck { get; set; }

        /// <summary>
        /// 距离容差
        /// </summary>
        [TreeProgramListArgs("DistanceTolerance", (string)null)]
        public double DistanceTolerance { get; set; }

        /// <summary>
        /// 对称排列
        /// </summary>
        [TreeProgramListArgs("IsSymmetric", (string)null)]
        public bool IsSymmetric { get; set; }

        /// <summary>
        /// 普通类型
        /// </summary>
        [TreeProgramListArgs("Common DataSet", (string)null)]
        public bool CommonDataSet { get; set; }

        /// <summary>
        /// 定位1使用情况
        /// </summary>
        public LocateUseConfig LocateUseConfig1 { get; set; } = new LocateUseConfig();

        /// <summary>
        /// 定位2使用情况
        /// </summary>
        public LocateUseConfig LocateUseConfig2 { get; set; } = new LocateUseConfig();

        /// <summary>
        /// 定位3使用情况
        /// </summary>
        public LocateUseConfig LocateUseConfig3 { get; set; } = new LocateUseConfig();

        /// <summary>
        /// 定位4使用情况
        /// </summary>
        public LocateUseConfig LocateUseConfig4 { get; set; } = new LocateUseConfig();

        /// <summary>
        /// 四周定位
        /// </summary>
        public bool IsAroundLocate { get; set; } = false;

        /// <summary>
        /// 四周定位的半径
        /// </summary>
        public double AroundLocateDistance { get; set; } = 1.0;

        /// <summary>
        /// 两点确定角度
        /// </summary>
        public bool IsCalculateAngleByTwoPoint { get; set; } = false;

        /// <summary>
        /// 三点拟合圆心
        /// </summary>
        public bool IsThreePointFittingCircle { get; set; } = false;

        /// <summary>
        /// 获取当前配置的所有定位点的名称
        /// </summary>
        /// <returns>名称的合集</returns>
        public List<string> GetPrNames()
        {
            List<string> list = new List<string>();

            if (this.P1PRName != string.Empty)
            {
                list.Add(this.P1PRName);
            }

            if (this.P2PRName != string.Empty)
            {
                list.Add(this.P2PRName);
            }

            if (this.P3PRName != string.Empty)
            {
                list.Add(this.P3PRName);
            }

            return list;
        }

        /// <summary>
        /// 拍照点位偏移
        /// </summary>
        /// <param name="offset">偏移量</param>
        public void VisionPosOffSet(AKRSPoint3D offset)
        {
            this.P1VisionRelativePos += offset;
            this.P2VisionRelativePos += offset;
            this.P3VisionRelativePos += offset;
            this.P4VisionRelativePos += offset;
        }

        /// <summary>
        /// 是否使用了圆的角度
        /// </summary>
        /// <returns>结果</returns>
        public bool IsUseCircleAngel()
        {
            return this.IsUseCircleAngelSingle(P1PRName, LocateUseConfig1) 
                || this.IsUseCircleAngelSingle(P2PRName, LocateUseConfig2) 
                || this.IsUseCircleAngelSingle(P3PRName, LocateUseConfig3) 
                || this.IsUseCircleAngelSingle(P4PRName, LocateUseConfig4);
        }

        /// <summary>
        /// 是否使用了圆的角度
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="locateUseConfig">配置对象</param>
        /// <returns>结果</returns>
        private bool IsUseCircleAngelSingle(string name, LocateUseConfig locateUseConfig)
        {
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);

            if (pREntity != null
                && pREntity.GetAlgFlowType() == AlgFlowTypeEnum.CircleFindAlg
                && locateUseConfig.UseAngle)
            {
                return true;
            }

            return false;
        }
    }


    /// <summary>
    /// 定位使用信息
    /// </summary>
    [Serializable]
    public class LocateUseConfig
    {
        /// <summary>
        /// 是否使用X
        /// </summary>
        public bool UseX { get; set; } = true;

        /// <summary>
        /// 是否使用X
        /// </summary>
        public bool UseY { get; set; } = true;

        /// <summary>
        /// 是否使用角度
        /// </summary>
        public bool UseAngle { get; set; } = true;

        /// <summary>
        /// 自动聚焦
        /// </summary>
        public bool Autofocus { get; set; } = false;
    }
}
