using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.ZX2200.TransportUnitSystem.Module.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Matter
{
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Information;
    using AKRS.ZX2200.TransportUnitSystem.Service;
    using Newtonsoft.Json;

    /// <summary>
    /// Module
    /// </summary>
    public class Module : BaseMatter
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
        public ModuleConfig ModuleConfig => this.Config as ModuleConfig;

        /// <summary>
        /// module的信息
        /// </summary>
        public ModuleInfo ModuleInfo => (ModuleInfo)this.BaseInfo;

        /// <summary>
        /// 基板号
        /// </summary>
        public int SubstrateNum { get; set; }

        /// <summary>
        /// Module的制程状态
        /// </summary>
        [JsonIgnore]
        public override MatterProductState MatterProductState => this.GetModuleMatterProductState();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index">序号</param>
        /// <param name="upperCoordinateSystem">上层坐标系</param>
        public Module(int index, GeneralCoordinateSystem upperCoordinateSystem)
        {
            this.MatterTypeEnum = EntityTypeEnum.Module;

            this.Index = index;

            this.WorkIndex = TuService.GetIndex(
                index,
                this.ModuleConfig.ColumnCount,
                this.ModuleConfig.RowCount,
                this.ModuleConfig.Arrangement,
                this.ModuleConfig.WorkOrderEnum);
            
            this.Name = $"基岛{index}";

            TuService.CreateMatterCoordinate(
                this,
                upperCoordinateSystem,
                this.ModuleConfig.ElementCoordinates[index - 1]);

            this.BaseInfo = new ModuleInfo();

            this.CreateBondPosition();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index">序号</param>
        /// <param name="upperCoordinateSystem">上层坐标系</param>
        /// <param name="oppositeSex">异性基板配置</param>
        public Module(int index, GeneralCoordinateSystem upperCoordinateSystem, OppositeSex oppositeSex)
        {
            this.MatterTypeEnum = EntityTypeEnum.Module;

            this.OppositeSex = oppositeSex;

            this.Index = index;

            this.WorkIndex = index;

            this.Name = oppositeSex.Name;

            TuService.CreateMatterCoordinate(this, upperCoordinateSystem, oppositeSex.ElementCoordinate);

            this.BaseInfo = new ModuleInfo();

            this.CreateBondPositionByOppositeSex();
        }

        /// <summary>
        /// 是否初始化自己
        /// </summary>
        public void CreateBondPosition()
        {
            BondPositionConfig bondPositionConfig = ProductConfiguration.GetInstance().BondPositionConfig;

            this.BondPositions.Clear();


            for (int i = 0; i < bondPositionConfig.SingleBpPositionConfigList.Count; i++)
            {
                BondPosition bondPosition = new BondPosition(
                    bondPositionConfig.SingleBpPositionConfigList[i].Name,
                    i + 1,
                    this.CoordinateSystem);

                this.BondPositions.Add(bondPosition);
            }
        }

        /// <summary>
        /// 是否初始化自己
        /// </summary>
        public void CreateBondPositionByOppositeSex()
        {
            this.BondPositions.Clear();

            for (int i = 0; i < this.OppositeSex.DownConfigs.Count; i++)
            {
                BondPosition bondPosition = new BondPosition(
                    i + 1,
                    this.CoordinateSystem,
                    this.OppositeSex.DownConfigs[i]);

                this.BondPositions.Add(bondPosition);
            }
        }

        /// <summary>
        /// 刷新坐标系
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="upperCoordinateSystem">上层的名称</param>
        public void Refresh(int index, GeneralCoordinateSystem upperCoordinateSystem)
        {
            this.Index = index;

            if (this.OppositeSex != null)
            {
                TuService.CreateMatterCoordinate(this, upperCoordinateSystem, this.OppositeSex.ElementCoordinate);
            }
            else
            {
                this.WorkIndex = TuService.GetIndex(
                    index,
                    this.ModuleConfig.ColumnCount,
                    this.ModuleConfig.RowCount,
                    this.ModuleConfig.Arrangement,
                    this.ModuleConfig.WorkOrderEnum);
                TuService.CreateMatterCoordinate(
                    this,
                    upperCoordinateSystem,
                    this.ModuleConfig.ElementCoordinates[index - 1]);
            }

            foreach (BondPosition bondPosition in this.BondPositions)
            {
                bondPosition.Refresh(this.CoordinateSystem);
            }
        }

        /// <summary>
        /// 屏蔽当前module以及module里面的焊点
        /// </summary>
        /// <param name="matterProductState">状态</param>
        public void ChangeBondPositionProductInformation(MatterProductState matterProductState)
        {
            if (matterProductState == MatterProductState.EnableInSystem1 || matterProductState == MatterProductState.EnableInSystem2)
            {
                if (this.MatterProductState == MatterProductState.EnableInSystem1 || this.MatterProductState == MatterProductState.EnableInSystem2)
                {
                    return;
                }
            }

            if (this.MatterProductState == MatterProductState.Disable)
            {
                return;
            }

            this.MatterProductState = matterProductState;

            foreach (BondPosition bondPosition in this.BondPositions)
            {
                bondPosition.MatterProductState = matterProductState;
            }
        }

        /// <summary>
        /// 焊点集合
        /// </summary>
        public List<BondPosition> BondPositions { get; set; } = new List<BondPosition>();

        /// <summary>
        /// 行索引
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// 列索引
        /// </summary>
        public int ColumnIndex { get; set; }

        /// <summary>
        /// 该module里面的所有焊点的在系统1里面的制程是否完成
        /// </summary>
        public bool IsModuleProcessFinishedInSystem1
        {
            get
            {
                return this.IsProcessFinishedInSystem1();
            }
        }

        /// <summary>
        /// 该module里面的所有焊点的在系统1里面的制程是否完成
        /// </summary>
        public bool IsModuleProcessFinishedInSystem2
        {
            get
            {
                return this.IsProcessFinishedInSystem2();
            }
        }

        /// <summary>
        /// Module在系统1的制程是否全部完成
        /// </summary>
        /// <returns>结果</returns>
        private bool IsProcessFinishedInSystem1()
        {
            foreach (BondPosition bondPosition in this.BondPositions)
            {
                if (!bondPosition.IsFinishedInSystem1)
                {
                    return false;
                }
            }
            
            return true;
        }

        /// <summary>
        /// Module在系统2的制程是否全部完成
        /// </summary>
        /// <returns>结果</returns>
        private bool IsProcessFinishedInSystem2()
        {
            foreach (BondPosition bondPosition in this.BondPositions)
            {
                if (!bondPosition.IsFinishedInSystem2)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取module的制程状态
        /// </summary>
        /// <returns>结果</returns>
        private MatterProductState GetModuleMatterProductState()
        {
            if (this.BondPositions.Exists(it => it.MatterProductState == MatterProductState.Enable))
            {
                return MatterProductState.Enable;
            }
            else if (this.BondPositions.Exists(it => it.MatterProductState == MatterProductState.EnableInSystem1))
            {
                if (!this.BondPositions.Exists(it => it.MatterProductState == MatterProductState.EnableInSystem2))
                {
                    return MatterProductState.EnableInSystem1;
                }
                else
                {
                    return MatterProductState.Enable;
                }
            }
            else if (this.BondPositions.Exists(it => it.MatterProductState == MatterProductState.EnableInSystem2))
            {
                if (!this.BondPositions.Exists(it => it.MatterProductState == MatterProductState.EnableInSystem1))
                {
                    return MatterProductState.EnableInSystem2;
                }
                else
                {
                    return MatterProductState.Enable;
                }
            }
            else
            {
                return MatterProductState.Disable;
            }
        }

        /// <summary>
        /// 获取module的制程状态
        /// </summary>
        /// <returns>结果</returns>
        public bool IsFail()
        {
            if (this.BondPositions.Exists(it => it.MatterProductState == MatterProductState.Disable))
            {
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// 所有设置为不启用
        /// </summary>
        public override void SetMatterDisable()
        {
            foreach (var it in this.BondPositions) 
            {
                it.SetMatterDisable();
            }
        }

        /// <summary>
        /// 所有设置为只点胶
        /// </summary>
        public override void SetMatterOnlyDispense()
        {
            foreach (var it in this.BondPositions)
            {
                it.SetMatterOnlyDispense();
            }
        }

        /// <summary>
        /// 所有设置为只贴片
        /// </summary>
        public override void SetMatterOnlyBond()
        {
            foreach (var it in this.BondPositions)
            {
                it.SetMatterOnlyBond();
            }
        }

        /// <summary>
        /// 所有设置为正常
        /// </summary>
        public override void SetMatterEnable()
        {
            foreach (var it in this.BondPositions)
            {
                it.SetMatterEnable();
            }
        }
    }
}
