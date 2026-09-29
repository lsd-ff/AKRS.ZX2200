using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.TransportUnitSystem.Module.Config;
using AKRS.ZX2200.TransportUnitSystem.Module.Information;
using DevExpress.CodeParser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Matter
{
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Services;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Setting;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Service;
    using DevExpress.ClipboardSource.SpreadsheetML;
    using DevExpress.Utils.Text.Internal;
    using DevExpress.XtraEditors;
    using LanguageExt;
    using OfficeOpenXml;
    using System.IO;
    using System.Windows.Media.TextFormatting;

    /// <summary>
    /// TU
    /// </summary>
    public class TransportUnit : BaseMatter
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
        public TransportUnitConfig TransportUnitConfig => this.Config as TransportUnitConfig;

        /// <summary>
        /// Tu的信息
        /// </summary>
        public TransportUnitInfo TransportUnitInfo => (TransportUnitInfo)this.BaseInfo;

        /// <summary>
        /// 原因
        /// </summary>
        private readonly string reason;

        /// <summary>
        /// 不要调用
        /// 为TU反序列化准备的无参构造函数
        /// </summary>
        public TransportUnit()
        {
        }

        /// <summary>
        /// 创建载具单元
        /// </summary>
        /// <param name="reason">原因</param>
        public TransportUnit(string reason)
        {
            this.reason = reason;
            this.BaseInfo = new TransportUnitInfo();
            this.Init();
            this.Name = DateTime.Now.ToString("yyMMddHHmmssfff");
            this.reason = reason;
        }

        /// <summary>
        /// 载具构造方法
        /// </summary>
        /// <param name="currentMachineSystem">系统</param>
        public TransportUnit(CurrentMachineSystemEnum currentMachineSystem)
        {
            this.BaseInfo = new TransportUnitInfo();
            this.Init();
            this.Name = DateTime.Now.ToString("yyMMddHHmmssfff");

            // 设置载具偏移
            if (currentMachineSystem == CurrentMachineSystemEnum.System1)
            {
                this.SetSystem1OffSet();
            }
            else
            {
                this.SetSystem2OffSet();
            }
        }


        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            this.OppositeSex = ProductConfiguration.GetInstance().OppositeSexConfiguration.OppositeSexConfigs;
            this.InitTransportUnitCoordinateSystem();

            if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
            {
                this.InitSubstrateByOppositeSex();
            }
            else
            {
                this.InitSubstrateCoordinateSystem();
            }

            TuService.ModifyMatter(this, ProductConfiguration.GetInstance().OtherConfig);

            System2Domain.GetInstance().ActionNodesService.InjectSteps(this);
            System1Domain.GetInstance().ActionNodeController.InjectSteps(this);
        }

        /// <summary>
        /// Substrate的集合
        /// </summary>
        public List<Substrate> Substrates { get; set; } = new List<Substrate>();

        /// <summary>
        /// 刷新坐标系
        /// </summary>
        public void Refresh()
        {
            this.InitTransportUnitCoordinateSystem();
            this.RefreshSubstrate();
            this.ClearVision();
            this.ClearMeasureHeight();
            this.TransportUnitInfo.IsSystem1Offset = false;
            this.TransportUnitInfo.IsSystem2Offset = false;
        }

        /// <summary>
        /// 获取基板
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>基板</returns>
        public Substrate GetSubstrate(int index)
        {
            if (this.Substrates == null)
            {
                return null;
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException($"Index 不能小于0. Index: {index}");
            }

            return this.Substrates.Find(it => it.Index == index);
        }

        /// <summary>
        /// 获取基岛
        /// </summary>
        /// <param name="substrateIndex">基板索引</param>
        /// <param name="moduleIndex">基岛索引</param>
        /// <returns>穴位</returns>
        public Module GetModule(int substrateIndex, int moduleIndex)
        {
            Substrate substrate = this.GetSubstrate(substrateIndex);

            if (substrate == null)
            {
                return null;
            }

            if (moduleIndex < 0)
            {
                throw new ArgumentOutOfRangeException($"Index 不能小于0. Index: {moduleIndex}");
            }

            return substrate.Modules.Find(it => it.Index == moduleIndex);
        }

        /// <summary>
        /// 获取焊点
        /// </summary>
        /// <param name="substrateIndex">基板索引</param>
        /// <param name="moduleIndex">基岛索引</param>
        /// <param name="bondPositionName">焊点名</param>
        /// <returns>焊点</returns>
        public BondPosition GetBondPosition(int substrateIndex, int moduleIndex, string bondPositionName)
        {
            Module module = this.GetModule(substrateIndex, moduleIndex);

            if (module == null)
            {
                return null;
            }

            if (bondPositionName == null)
            {
                throw new ArgumentOutOfRangeException($"bondPositionName 不能为空");
            }

            return module.BondPositions.Find(it => it.Name == bondPositionName);
        }

        /// <summary>
        /// 获取所有焊点
        /// </summary>
        /// <param name="bpName">焊点名称</param>
        /// <returns>集合</returns>
        public List<BondPosition> GetAllBondPositions(string bpName)
        {
            List<BondPosition> bpList = new List<BondPosition>();
            for (int i = 1; i < this.GetSubstrateCount(); i++)
            {
                for (int j = 1; j < this.GetModuleCount(); j++)
                {
                    Module module = this.GetModule(i, j);

                    foreach (var bondPosition in module.BondPositions)
                    {
                        if (bondPosition.Name == bpName)
                        {
                            bpList.Add(bondPosition);
                        }
                    }
                }
            }

            return bpList;
        }

        /// <summary>
        /// 获取基板的数量
        /// </summary>
        /// <returns>结果</returns>
        public int GetSubstrateCount()
        {
            return ProductConfiguration.GetInstance().SubstrateConfig.Count;
        }


        /// <summary>
        /// 获取基岛数量
        /// </summary>
        /// <returns>结果</returns>
        public int GetModuleCount()
        {
            return ProductConfiguration.GetInstance().ModuleConfig.Count;
        }

        /// <summary>
        /// 初始化产品坐标系系
        /// </summary>
        public void InitTransportUnitCoordinateSystem()
        {
            GeneralCoordinateSystem transportCoordinateSystem = (GeneralCoordinateSystem)MachineCoordinateSystem
                .GetInstance().CoordinateSystems.Find(it => it.Name == "TransportCoordinateSystem");

            if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
            {
                TuService.CreateMatterCoordinate(
                    this,
                    transportCoordinateSystem,
                    this.OppositeSex.ElementCoordinate);
            }
            else
            {
                TuService.CreateMatterCoordinate(
                    this,
                    transportCoordinateSystem,
                    this.TransportUnitConfig.ElementCoordinate);
            }
        }

        /// <summary>
        /// 初始化坐标系
        /// </summary>
        public void InitSubstrateCoordinateSystem()
        {
            SubstrateConfig substrateConfig = ProductConfiguration.GetInstance().SubstrateConfig;

            this.Substrates.Clear();

            if (!substrateConfig.IsMultiple)
            {
                substrateConfig.Count = 1;
                substrateConfig.ColumnCount = 1;
                substrateConfig.RowCount = 1;
                substrateConfig.ElementCoordinates.Clear();
                substrateConfig.ElementCoordinates.Add(new ElementCoordinate());
            }

            // 如果没有，初始化一个
            if (substrateConfig.ElementCoordinates.Count == 0)
            {
                substrateConfig.ElementCoordinates.Add(new ElementCoordinate());
            }

            if (substrateConfig.IsAssistanceDistanceByInput)
            {
                ElementCoordinate elementCoordinateFirst = substrateConfig.ElementCoordinates[0];
                substrateConfig.ElementCoordinates.Clear();

                for (int i = 0; i < substrateConfig.RowCount; i++)
                {
                    for (int j = 0; j <substrateConfig.ColumnCount; j++)
                    {
                        // Z轴的位置是跟随TU的，如果需要设置，需要到单独的测高界面
                        ElementCoordinate elementCoordinate = new ElementCoordinate();
                        elementCoordinate.Point = new AKRSPoint3D(
                            elementCoordinateFirst.Point.X + substrateConfig.ColumnSpacing * j,
                            elementCoordinateFirst.Point.Y + substrateConfig.RowSpacing * i,
                            elementCoordinateFirst.Point.Z);
                        elementCoordinate.Degree = 0;
                        substrateConfig.ElementCoordinates.Add(elementCoordinate);
                    }
                }
            }

            for (int i = 0; i < substrateConfig.Count; i++)
            {
                Substrate substrate = new Substrate(i + 1, this.CoordinateSystem);

                // 判断行列数
                if (substrateConfig.Multiplication == MultiplicationEnum.Matrix)
                {
                    (substrate.RowIndex, substrate.ColumnIndex) = TuService.RowAndColumn(
                        substrateConfig.RowCount,
                        substrateConfig.ColumnCount,
                        i + 1,
                        substrateConfig.Arrangement);
                }

                this.Substrates.Add(substrate);
            }
        }

        /// <summary>
        /// 初始化坐标系
        /// </summary>
        private void InitSubstrateByOppositeSex()
        {
            this.Substrates.Clear();

            for (int i = 0; i < this.OppositeSex.DownConfigs.Count; i++)
            {
                Substrate substrate = new Substrate(i + 1, this.CoordinateSystem, this.OppositeSex.DownConfigs[i]);
                this.Substrates.Add(substrate);
            }
        }

        /// <summary>
        /// 刷新坐标系
        /// </summary>
        private void RefreshSubstrate()
        {
            for (int i = 0; i < this.Substrates.Count; i++)
            {
                this.Substrates[i].Refresh(i + 1, this.CoordinateSystem);
            }
        }

        /// <summary>
        /// 点胶1传输
        /// </summary>
        public void System1MoveDistance()
        {
            // 点胶分段的时候坐标系要往前移动
            if (ProductConfiguration.GetInstance().TransportUnitConfig.SubstrateProcessing == SubstrateProcessingEnum.Divide)
            {
                this.CoordinateSystem.Distance.X = this.CoordinateSystem.Distance.X + ProductConfiguration.GetInstance().TransportUnitConfig.DistanceToRight;
            }
        }

        /// <summary>
        /// 点胶1传输
        /// </summary>
        public void System1BackMoveDistance()
        {
            // 点胶分段往后移动
            if (ProductConfiguration.GetInstance().TransportUnitConfig.SubstrateProcessing == SubstrateProcessingEnum.Divide)
            {
                this.CoordinateSystem.Distance.X = this.CoordinateSystem.Distance.X - ProductConfiguration.GetInstance().TransportUnitConfig.DistanceToRight;
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="index">系统</param>
        public void SaveDataToExcel(int index)
        {
            string path = FileHelper.CreateFileByDate("产品数据");

            List<SubstrateInfo> list = new List<SubstrateInfo>();

            foreach (Substrate substrate in this.Substrates)
            {
                list.Add(substrate.SubstrateInfo);
            }

            FileHelper.Export(
                list,
                path + "\\" + $"系统{index}" + DateTime.Now.ToString("yyyyMMddHHMMss") + "Substrate" + ".xlsx");

            List<ModuleInfo> modules = new List<ModuleInfo>();

            foreach (Substrate substrate in this.Substrates)
            {
                foreach (Module module in substrate.Modules)
                {
                    modules.Add(module.ModuleInfo);
                }
            }

            FileHelper.Export(modules, path + "\\" + $"系统{index}" + DateTime.Now.ToString("yyyyMMddHHMMss") + "Module" + ".xlsx");

            List<BondPositionInfo> bondPositionInfos = new List<BondPositionInfo>();

            foreach (Substrate substrate in this.Substrates)
            {
                foreach (Module module in substrate.Modules)
                {
                    foreach (BondPosition bondPosition in module.BondPositions)
                    {
                        bondPositionInfos.Add(bondPosition.BondPositionInfo);
                    }
                }
            }

            FileHelper.Export(bondPositionInfos, path + "\\" + $"系统{index}" + DateTime.Now.ToString("yyyyMMddHHMMss") + "bondPosition" + ".xlsx");
        }

        /// <summary>
        /// 该module里面的所有焊点的制程是否完成
        /// </summary>
        public bool IsTuProcessFinishedInSystem1
        {
            get
            {
                return this.IsProcessFinishedInSystem1();
            }
        }

        /// <summary>
        /// 该module里面的所有焊点的制程是否完成
        /// </summary>
        public bool IsTuProcessFinishedInSystem2
        {
            get
            {
                return this.IsProcessFinishedInSystem2();
            }
        }

        /// <summary>
        /// Tu在系统1下的制程有没有完成
        /// </summary>
        /// <returns>结果</returns>
        private bool IsProcessFinishedInSystem1()
        {
            foreach (Substrate substrate in this.Substrates)
            {
                if (!substrate.IsSubstrateProcessFinishedInSystem1)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Tu在系统2下的制程有没有完成
        /// </summary>
        /// <returns>结果</returns>
        private bool IsProcessFinishedInSystem2()
        {
            foreach (Substrate substrate in this.Substrates)
            {
                if (!substrate.IsSubstrateProcessFinishInSystem2)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取分离的sub
        /// 默认一半一半
        /// </summary>
        /// <returns>结果1：点胶第一段的sub集合 结果2：点胶第二段的sub集合</returns>
        public (List<int>, List<int>) GetDivideSubstrate()
        {
            List<int> subList1 = new List<int>();
            List<int> subList2 = new List<int>();
            double divideX = this.Substrates[ProductConfiguration.GetInstance().SubstrateConfig.ColumnCount / 2]
                .CoordinateSystem.Distance.X;
            foreach (Substrate substrate in this.Substrates)
            {
                if (substrate.CoordinateSystem.Distance.X >= divideX)
                {
                    subList1.Add(substrate.Index);
                }
                else
                {
                    subList2.Add(substrate.Index);
                }
            }

            return (subList1, subList2);
        }
        
        /// <summary>
        /// 设置系统1的偏移
        /// </summary>
        public void SetSystem1OffSet()
        {
            if (this.TransportUnitInfo.IsSystem1Offset)
            {
                return;
            }

            this.CoordinateSystem.Distance += ProductConfiguration.GetInstance().TransportUnitConfig.System1Offset;

            this.TransportUnitInfo.IsSystem1Offset = true;
        }

        /// <summary>
        /// 设置系统2的偏移
        /// </summary>
        public void SetSystem2OffSet()
        {
            if (this.TransportUnitInfo.IsSystem2Offset)
            {
                return;
            }

            this.CoordinateSystem.Distance += ProductConfiguration.GetInstance().TransportUnitConfig.System2Offset;

            this.TransportUnitInfo.IsSystem2Offset = true;
        }

        /// <summary>
        /// 设置实体不启用
        /// </summary>
        public override void SetMatterDisable()
        {
            foreach (var it in this.Substrates)
            {
                it.SetMatterDisable();
            }
        }

        /// <summary>
        /// 所有设置为只点胶
        /// </summary>
        public override void SetMatterOnlyDispense()
        {
            foreach (var it in this.Substrates)
            {
                it.SetMatterOnlyDispense();
            }
        }

        /// <summary>
        /// 所有设置为只贴片
        /// </summary>
        public override void SetMatterOnlyBond()
        {
            foreach (var it in this.Substrates)
            {
                it.SetMatterOnlyBond();
            }
        }

        /// <summary>
        /// 所有设置为正常
        /// </summary>
        public override void SetMatterEnable()
        {
            foreach (var it in this.Substrates)
            {
                it.SetMatterEnable();
            }
        }

        /// <summary>
        /// 获取所有的实体对象
        /// </summary>
        /// <returns>结果</returns>
        public List<BaseMatter> GetAllBaseMatter()
        {
            List<BaseMatter> list = new List<BaseMatter>();
            foreach (Substrate substrate in this.Substrates)
            {
                list.Add(substrate);
                foreach (Module module in substrate.Modules)
                {
                    list.Add(module);
                    foreach (BondPosition bondPosition in module.BondPositions)
                    {
                        list.Add(bondPosition);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 清楚视觉定位结果
        /// </summary>
        public void ClearVision()
        {
            this.BaseInfo.IsVisioned = false;
            foreach (BaseMatter baseMatter in this.GetAllBaseMatter())
            {
                baseMatter.BaseInfo.IsVisioned = false;
            }
        }

        /// <summary>
        /// 清楚测高
        /// </summary>
        public void ClearMeasureHeight()
        {
            this.BaseInfo.IsMeasureHeight = false;
            foreach (BaseMatter baseMatter in this.GetAllBaseMatter())
            {
                baseMatter.BaseInfo.IsMeasureHeight = false;
            }
        }

        /// <summary>
        /// 根据异性配置返回实体
        /// </summary>
        /// <param name="opposite">异性</param>
        /// <returns>结果</returns>
        public BaseMatter GetBaseMatterByOppositeSex(OppositeSex opposite)
        {
            if (this.OppositeSex == opposite)
            {
                return this;
            }

            foreach (Substrate substrate in this.Substrates)
            {
                if (substrate.OppositeSex == opposite)
                {
                    return substrate;
                }

                foreach (Module module in substrate.Modules)
                {
                    if (module.OppositeSex == opposite)
                    {
                        return module;
                    }

                    foreach (BondPosition bondPosition in module.BondPositions)
                    {
                        if (bondPosition.OppositeSex == opposite)
                        {
                            return bondPosition;
                        }
                    }
                }
            }

            throw new Exception("未找到实体对象，请检查配置是否正确");
        }
    }
}
