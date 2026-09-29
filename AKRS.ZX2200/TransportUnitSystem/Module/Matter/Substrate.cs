using AKRS.ZX2200.TransportUnitSystem.Module.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Matter
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Information;
    using AKRS.ZX2200.TransportUnitSystem.Service;
    using DevExpress.Utils.Extensions;
    using Newtonsoft.Json;

    /// <summary>
    /// Substrate
    /// </summary>
    public class Substrate : BaseMatter
    {
        /// <summary>
        /// 坐标系
        /// </summary>
        [JsonIgnore]
        public override GeneralCoordinateSystem CoordinateSystem { get; set; } = new GeneralCoordinateSystem();

        /// <summary>
        /// Tu的配置对象
        /// </summary>
        [JsonIgnore]
        public SubstrateConfig SubstrateConfig => this.Config as SubstrateConfig;

        /// <summary>
        /// 产品的工艺
        /// </summary>
        [JsonIgnore]
        public override MatterProductState MatterProductState => this.GetSubstrateProductState();

        /// <summary>
        /// Tu的信息
        /// </summary>
        public SubstrateInfo SubstrateInfo => (SubstrateInfo)this.BaseInfo;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index">序号</param>
        /// <param name="upperCoordinateSystem">上层坐标系</param>
        public Substrate(int index, GeneralCoordinateSystem upperCoordinateSystem)
        {
            this.MatterTypeEnum = EntityTypeEnum.Substrate;
            
            this.Index = index;

            this.WorkIndex = TuService.GetIndex(
                index,
                this.SubstrateConfig.ColumnCount,
                this.SubstrateConfig.RowCount,
                this.SubstrateConfig.Arrangement,
                this.SubstrateConfig.WorkOrderEnum);

            this.Name = $"基板{index}";

            this.BaseInfo = new SubstrateInfo();
            
            TuService.CreateMatterCoordinate(
                this,
                upperCoordinateSystem,
                this.SubstrateConfig.ElementCoordinates[index - 1]);

            this.CreateModule();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index">序号</param>
        /// <param name="upperCoordinateSystem">上层坐标系</param>
        /// <param name="opposite">异性基板配置</param>
        public Substrate(int index, GeneralCoordinateSystem upperCoordinateSystem, OppositeSex opposite)
        {
            this.MatterTypeEnum = EntityTypeEnum.Substrate;

            this.OppositeSex = opposite;

            this.Index = index;

            this.WorkIndex = index;

            this.Name = opposite.Name;

            this.BaseInfo = new SubstrateInfo();

            TuService.CreateMatterCoordinate(this, upperCoordinateSystem, opposite.ElementCoordinate);

            this.InitModuleByOppositeSex();
        }

        /// <summary>
        /// 是否初始化自己
        /// </summary>
        public void CreateModule()
        {
            ModuleConfig moduleConfig = ProductConfiguration.GetInstance().ModuleConfig;

            if (!moduleConfig.IsMultiple)
            {
                moduleConfig.Count = 1;
                moduleConfig.ColumnCount = 1;
                moduleConfig.RowCount = 1;
                moduleConfig.ElementCoordinates.Clear();
                moduleConfig.ElementCoordinates.Add(new ElementCoordinate());
            }

            // 如果没有，初始化一个
            if (moduleConfig.ElementCoordinates.Count == 0)
            {
                moduleConfig.ElementCoordinates.Add(new ElementCoordinate());
            }

            if (moduleConfig.IsAssistanceDistanceByInput)
            {
                ElementCoordinate elementCoordinateFirst = moduleConfig.ElementCoordinates[0];
                moduleConfig.ElementCoordinates.Clear();

                for (int i = 0; i < moduleConfig.RowCount; i++)
                {
                    for (int j = 0; j < moduleConfig.ColumnCount; j++)
                    {
                        // Z轴的位置是跟随TU的，如果需要设置，需要到单独的测高界面
                        ElementCoordinate elementCoordinate = new ElementCoordinate();
                        elementCoordinate.Point = new AKRSPoint3D(
                            elementCoordinateFirst.Point.X + moduleConfig.ColumnSpacing * j,
                            elementCoordinateFirst.Point.Y + moduleConfig.RowSpacing * i,
                            elementCoordinateFirst.Point.Z);
                        elementCoordinate.Degree = 0;
                        moduleConfig.ElementCoordinates.Add(elementCoordinate);
                    }
                }
            }

            for (int i = 0; i < moduleConfig.Count; i++)
            {
                Module module = new Module(
                    i + 1,
                    this.CoordinateSystem);

                // 判断行列数
                if (moduleConfig.Multiplication == MultiplicationEnum.Matrix)
                {
                    (module.RowIndex, module.ColumnIndex) = TuService.RowAndColumn(
                        moduleConfig.RowCount,
                        moduleConfig.ColumnCount,
                        i + 1,
                        moduleConfig.Arrangement);
                }

                this.Modules.Add(module);
            }
        }

        /// <summary>
        /// 初始化坐标系
        /// </summary>
        private void InitModuleByOppositeSex()
        {
            this.Modules.Clear();
            for (int i = 0; i < this.OppositeSex.DownConfigs.Count; i++)
            {
                Module module = new Module(i + 1, this.CoordinateSystem, this.OppositeSex.DownConfigs[i]);
                this.Modules.Add(module);
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
                    this.SubstrateConfig.ColumnCount,
                    this.SubstrateConfig.RowCount,
                    this.SubstrateConfig.Arrangement,
                    this.SubstrateConfig.WorkOrderEnum);

                TuService.CreateMatterCoordinate(
                    this,
                    upperCoordinateSystem,
                    this.SubstrateConfig.ElementCoordinates[index - 1]);
            }

            for (int i = 0; i < this.Modules.Count; i++)
            {
                this.Modules[i].Refresh(i + 1, this.CoordinateSystem);
            }
        }

        /// <summary>
        /// Module的集合
        /// </summary>
        public List<Module> Modules { get; set; } = new List<Module>();

        /// <summary>
        /// 行索引
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// 列索引
        /// </summary>
        public int ColumnIndex { get; set; }

        /// <summary>
        /// 该module里面的所有焊点的制程是否完成
        /// </summary>
        public bool IsSubstrateProcessFinishedInSystem1
        {
            get
            {
                return this.IsProcessFinishInSystem1();
            }
        }

        /// <summary>
        /// 该module里面的所有焊点的制程是否完成
        /// </summary>
        public bool IsSubstrateProcessFinishInSystem2
        {
            get
            {
                return this.IsProcessFinishInSystem2();
            }
        }

        /// <summary>
        /// 该module下的制程是否都已经完成
        /// </summary>
        /// <returns>结果</returns>
        private bool IsProcessFinishInSystem1()
        {
            foreach (Module module in this.Modules)
            {
                if (!module.IsModuleProcessFinishedInSystem1)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 该module下的制程是否都已经完成
        /// </summary>
        /// <returns>结果</returns>
        private bool IsProcessFinishInSystem2()
        {
            foreach (Module module in this.Modules)
            {
                if (!module.IsModuleProcessFinishedInSystem2)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取Substrate的制程状态
        /// </summary>
        /// <returns>制程状态</returns>
        private MatterProductState GetSubstrateProductState()
        {
            if (this.Modules.Exists(it => it.MatterProductState == MatterProductState.Enable))
            {
                return MatterProductState.Enable;
            }
            else if (this.Modules.Exists(it => it.MatterProductState == MatterProductState.EnableInSystem1))
            {
                if (!this.Modules.Exists(it => it.MatterProductState == MatterProductState.EnableInSystem2))
                {
                    return MatterProductState.EnableInSystem1;
                }
                else
                {
                    return MatterProductState.Enable;
                }
            }
            else if (this.Modules.Exists(it => it.MatterProductState == MatterProductState.EnableInSystem2))
            {
                if (!this.Modules.Exists(it => it.MatterProductState == MatterProductState.EnableInSystem1))
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
        /// 所有设置为不启用
        /// </summary>
        public override void SetMatterDisable()
        {
            foreach (var it in this.Modules)
            {
                it.SetMatterDisable();
            }
        }

        /// <summary>
        /// 所有设置为只点胶
        /// </summary>
        public override void SetMatterOnlyDispense()
        {
            foreach (var it in this.Modules)
            {
                it.SetMatterOnlyDispense();
            }
        }

        /// <summary>
        /// 所有设置为只贴片
        /// </summary>
        public override void SetMatterOnlyBond()
        {
            foreach (var it in this.Modules)
            {
                it.SetMatterOnlyBond();
            }
        }

        /// <summary>
        /// 所有设置为正常
        /// </summary>
        public override void SetMatterEnable()
        {
            foreach (var it in this.Modules)
            {
                it.SetMatterEnable();
            }
        }
    }
}
