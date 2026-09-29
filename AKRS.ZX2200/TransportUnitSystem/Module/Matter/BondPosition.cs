namespace AKRS.ZX2200.TransportUnitSystem.Module.Matter
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.TransportUnitSystem.Module.Information;
    using AKRS.ZX2200.TransportUnitSystem.Service;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// 焊点示教
    /// </summary>
    public class BondPosition : BaseMatter
    {
        /// <summary>
        /// 坐标系
        /// </summary>
        [JsonIgnore]
        public override GeneralCoordinateSystem CoordinateSystem { get; set; } = new GeneralCoordinateSystem();

        /// <summary>
        /// Module的配置对象
        /// </summary>
        [JsonIgnore]
        public SingleBondPositionConfig SingleBondPositionConfig => this.Config as SingleBondPositionConfig;

        /// <summary>
        /// 是否为镜像
        /// </summary>
        public bool IsMirror { get; set; } = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="index">序号</param>
        /// <param name="upperCoordinateSystem">上层坐标系</param>
        public BondPosition(string name, int index, GeneralCoordinateSystem upperCoordinateSystem)
        {
            this.Name = name;

            this.Index = index;

            this.MatterTypeEnum = EntityTypeEnum.BondPosition;

            this.BaseInfo = new BondPositionInfo();

            TuService.CreateMatterCoordinate(
                this,
                upperCoordinateSystem,
                this.SingleBondPositionConfig.ElementCoordinate);

            this.IsMirror = this.SingleBondPositionConfig.IsMirror;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index">序号</param>
        /// <param name="upperCoordinateSystem">上层坐标系</param>
        /// <param name="oppositeSex">异性配置</param>
        public BondPosition(int index, GeneralCoordinateSystem upperCoordinateSystem, OppositeSex oppositeSex)
        {
            this.Name = oppositeSex.Name;

            this.OppositeSex = oppositeSex;

            this.Index = index;

            this.MatterTypeEnum = EntityTypeEnum.BondPosition;

            this.BaseInfo = new BondPositionInfo();
            
            TuService.CreateMatterCoordinate(
                this,
                upperCoordinateSystem,
                oppositeSex.ElementCoordinate);

            this.IsMirror = this.OppositeSex.Config.IsMirror;
        }

        /// <summary>
        /// 刷新坐标系
        /// </summary>
        /// <param name="upperCoordinateSystem">上层的名称</param>
        public void Refresh(GeneralCoordinateSystem upperCoordinateSystem)
        {
            if (this.OppositeSex != null)
            {
                TuService.CreateMatterCoordinate(
                    this,
                    upperCoordinateSystem,
                    this.OppositeSex.ElementCoordinate);
            }
            else
            {
                TuService.CreateMatterCoordinate(
                    this,
                    upperCoordinateSystem,
                    this.SingleBondPositionConfig.ElementCoordinate);
            }
        }

        /// <summary>
        /// 运行信息
        /// </summary>
        public BondPositionInfo BondPositionInfo => (BondPositionInfo)this.BaseInfo;

        /// <summary>
        /// 所在的基板号
        /// </summary>
        public string TuName { get; set; }

        /// <summary>
        /// 所在的基板号
        /// </summary>
        public int SubstrateNum { get; set; }

        /// <summary>
        /// 所在的基岛号
        /// </summary>
        public int ModuleNum { get; set; }
        
        /// <summary>
        /// 系统1的制程是否完成
        /// </summary>
        public bool IsFinishedInSystem1 => this.MatterProductState == MatterProductState.EnableInSystem2
                                           || this.MatterProductState == MatterProductState.Disable
                                           || this.BondPositionInfo.IsFinishedInSystem1();


        /// <summary>
        /// 系统2的制程是否完成
        /// </summary>
        public bool IsFinishedInSystem2 => this.MatterProductState == MatterProductState.EnableInSystem1
                                           || this.MatterProductState == MatterProductState.Disable
                                           || this.BondPositionInfo.IsFinishedInSystem2();

        /// <summary>
        /// 是否需要贴片
        /// </summary>
        /// <param name="stepName">步骤名称</param>
        /// <returns>结果</returns>
        public bool IsProduct(string stepName)
        {
            // 判断当前工艺制程是否开启
            if (this.MatterProductState == MatterProductState.Disable
                || this.MatterProductState == MatterProductState.EnableInSystem1)
            {
                return false;
            }

            // 已经贴过片则退出
            if (!this.IsS2NeedStep(stepName))
            {
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// 焊点完成系统1制程
        /// </summary>
        /// <param name="stepName">步骤名称</param>
        public void S1FinishedStep(string stepName)
        {
            this.BondPositionInfo.S1RemainingSteps[stepName] = true;
        }

        /// <summary>
        /// 焊点完成系统2制程
        /// </summary>
        /// <param name="stepName">步骤名称</param>
        public void S2FinishedStep(string stepName)
        {
            this.BondPositionInfo.S2RemainingSteps[stepName] = true;
        }

        /// <summary>
        /// 系统1是否需要完成这个步骤
        /// </summary>
        /// <param name="stepName">步骤名称</param>
        /// <returns>结果</returns>
        public bool IsS1NeedStep(string stepName)
        {
            return !this.BondPositionInfo.S1RemainingSteps[stepName];
        }

        /// <summary>
        /// 系统1是否需要完成这个步骤
        /// </summary>
        /// <param name="stepName">步骤名称</param>
        /// <returns>结果</returns>
        public bool IsS2NeedStep(string stepName)
        {
            return !this.BondPositionInfo.S2RemainingSteps[stepName];
        }

        /// <summary>
        /// 设置状态为不启用
        /// </summary>
        public override void SetMatterDisable()
        {
            this.MatterProductState = MatterProductState.Disable;
        }

        /// <summary>
        /// 设置状态为只点胶
        /// </summary>
        public override void SetMatterOnlyDispense()
        {
            this.MatterProductState = MatterProductState.EnableInSystem1;
        }


        /// <summary>
        /// 设置状态为只贴片
        /// </summary>
        public override void SetMatterOnlyBond()
        {
            this.MatterProductState = MatterProductState.EnableInSystem2;
        }

        /// <summary>
        /// 设置状态为正常
        /// </summary>
        public override void SetMatterEnable()
        {
            this.MatterProductState = MatterProductState.Enable;
        }

        /// <summary>
        /// 设置状态为正常
        /// </summary>
        public void SetProcessUnable()
        {
            List<string> keys = new List<string>();

            keys.AddRange(this.BondPositionInfo.S1RemainingSteps.Keys);

            for (int i = 0; i < this.BondPositionInfo.S1RemainingSteps.Count; i++)
            {
                this.BondPositionInfo.S1RemainingSteps[keys[i]] = false;
            }
        }

        /// <summary>
        /// 获取角度
        /// </summary>
        /// <returns>结果</returns>
        public double GetRotaryCompensate()
        {
            return this.SingleBondPositionConfig == null
                       ? this.OppositeSex.Config.AngleCompensate
                       : this.SingleBondPositionConfig.RotaryPosition;
        }

        /// <summary>
        /// 获取位置补偿
        /// </summary>
        /// <returns>结果</returns>
        public AKRSPoint3D GetPositionCompensate()
        {
            return this.SingleBondPositionConfig == null
                       ? this.OppositeSex.Config.PositionCompensate
                       : this.SingleBondPositionConfig.BondPosOffset;
        }

        /// <summary>
        /// 获取角度
        /// </summary>
        /// <returns>结果</returns>
        public double GetDispenseRotaryCompensate()
        {
            return this.SingleBondPositionConfig == null
                       ? this.OppositeSex.Config.DispenseAngleCompensate
                       : this.SingleBondPositionConfig.DispenseRotaryPosition;
        }

        /// <summary>
        /// 获取位置补偿
        /// </summary>
        /// <returns>结果</returns>
        public AKRSPoint3D GetDispensePositionCompensate()
        {
            return this.SingleBondPositionConfig == null
                       ? this.OppositeSex.Config.DispensePositionCompensate
                       : this.SingleBondPositionConfig.DispensePosOffset;
        }
    }
}
