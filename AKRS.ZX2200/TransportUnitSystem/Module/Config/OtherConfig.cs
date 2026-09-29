using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Config
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    /// <summary>
    /// 其他的配置
    /// 主要是一些集中的处理
    /// </summary>
    public class OtherConfig
    {
        /// <summary>
        /// 屏蔽的焊点集合
        /// </summary>
        public List<MatterProductInformation> MatterProductInfo { get; set; } = new List<MatterProductInformation>();

        /// <summary>
        /// 焊点的单独偏移量
        /// </summary>
        public List<BpOffsetSeparateInformation> BpSingleSeparates { get; set; } = new List<BpOffsetSeparateInformation>();

        /// <summary>
        /// 屏蔽的焊点集合
        /// </summary>
        public List<MatterProductInformationMin>[][] MatterProductInfoMax { get; set; }

        /// <summary>
        /// 清楚信息
        /// </summary>
        public void ClearInformation()
        {
            this.MatterProductInfo.Clear();
            this.MatterProductInfoMax = new List<MatterProductInformationMin>[ProductConfiguration.GetInstance().SubstrateConfig.Count][];
            int bpCount = ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList.Count;
            for (int i = 0; i < ProductConfiguration.GetInstance().SubstrateConfig.Count; i++)
            {
                this.MatterProductInfoMax[i] = new List<MatterProductInformationMin>[ProductConfiguration.GetInstance().ModuleConfig.Count];
                for (int j = 0; j < ProductConfiguration.GetInstance().ModuleConfig.Count; j++)
                {
                    this.MatterProductInfoMax[i][j] = new List<MatterProductInformationMin>();
                    for (int k = 0; k < bpCount; k++)
                    {
                        this.MatterProductInfoMax[i][j].Add(new MatterProductInformationMin(ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList[k].Name, MatterProductState.Enable));
                    }
                }
            }
        }

        /// <summary>
        /// 修改焊点名称
        /// </summary>
        /// <param name="oldName">原来的名称</param>
        /// <param name="newName">新的名称</param>
        public void ChangeBpName(string oldName, string newName)
        {
            foreach (BpOffsetSeparateInformation bpOffsetSeparateInformation in this.BpSingleSeparates)
            {
                if (bpOffsetSeparateInformation.BondPositionName == oldName)
                {
                    bpOffsetSeparateInformation.BondPositionName = newName;
                }
            }

            List<MatterProductInformation> list = new List<MatterProductInformation>();

            foreach (MatterProductInformation matterProductInformation in this.MatterProductInfo)
            {
                if (matterProductInformation.BondPositionName == oldName)
                {
                    list.Add(
                        new MatterProductInformation(
                            matterProductInformation.SubstrateIndex,
                            matterProductInformation.ModuleIndex,
                            newName,
                            matterProductInformation.MatterProductState));
                }
            }

            list.RemoveAll(it => it.BondPositionName == oldName);
            list.AddRange(list);
        }

        /// <summary>
        /// 根据TransportUnit保存信息
        /// </summary>
        /// <param name="transportUnit">信息</param>
        public void SaveInformation(TransportUnit transportUnit)
        {
            try
            {
                this.MatterProductInfo.Clear();
                this.MatterProductInfoMax = new List<MatterProductInformationMin>[ProductConfiguration.GetInstance().SubstrateConfig.Count][];
                for (int i = 0; i < ProductConfiguration.GetInstance().SubstrateConfig.Count; i++)
                {
                    this.MatterProductInfoMax[i] = new List<MatterProductInformationMin>[ProductConfiguration.GetInstance().ModuleConfig.Count];
                }

                if (ProductConfiguration.GetInstance().TransportUnitConfig.LagerNumber)
                {
                    for (int i = 0; i < transportUnit.Substrates.Count; i++)
                    {
                        Substrate substrate = transportUnit.Substrates[i];
                        for (int j = 0; j < substrate.Modules.Count; j++)
                        {
                            Module module = substrate.Modules[j];
                            this.MatterProductInfoMax[i][j] = new List<MatterProductInformationMin>();
                            for (int k = 0; k < module.BondPositions.Count; k++)
                            {
                                BondPosition bondPosition = module.BondPositions[k];
                                this.MatterProductInfoMax[i][j].Add(new MatterProductInformationMin(bondPosition.Name, bondPosition.MatterProductState));
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < transportUnit.Substrates.Count; i++)
                    {
                        Substrate substrate = transportUnit.Substrates[i];
                        for (int j = 0; j < substrate.Modules.Count; j++)
                        {
                            Module module = substrate.Modules[j];
                            this.MatterProductInfoMax[i][j] = new List<MatterProductInformationMin>();
                            for (int k = 0; k < module.BondPositions.Count; k++)
                            {
                                BondPosition bondPosition = module.BondPositions[k];

                                if (bondPosition.MatterProductState != MatterProductState.Enable)
                                {
                                    this.MatterProductInfo.Add(new MatterProductInformation(substrate.Index, module.Index, bondPosition.Name, bondPosition.MatterProductState));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show("保存载具信息时发生错误: " + ex.Message);
            }

        }
    }


    /// <summary>
    /// 屏蔽的结构体
    /// </summary>
    public struct MatterProductInformation
    {
        /// <summary>
        /// 创建信息
        /// </summary>
        /// <param name="substrateIndex">sub的索引</param>
        /// <param name="moduleIndex">module的索引</param>
        /// <param name="bondPositionName">焊点的索引</param>
        /// <param name="matterProductState">实体信息</param>
        public MatterProductInformation(int substrateIndex, int moduleIndex, string bondPositionName, MatterProductState matterProductState)
        {
            this.SubstrateIndex = substrateIndex;
            this.ModuleIndex = moduleIndex;
            this.BondPositionName = bondPositionName;
            this.BondPositionIndex = 0;
            this.MatterProductState = matterProductState;
        }

        /// <summary>
        /// Substrate的索引
        /// </summary>
        public int SubstrateIndex { get; set; }

        /// <summary>
        /// Module的索引
        /// </summary>
        public int ModuleIndex { get; set; }

        /// <summary>
        /// BondPosition的索引
        /// </summary>
        public int BondPositionIndex { get; set; }

        /// <summary>
        /// 物体的制程状态
        /// </summary>
        public MatterProductState MatterProductState { get; set; }

        /// <summary>
        /// 被屏蔽焊点的名称
        /// </summary>
        public string BondPositionName { get; set; }
    }

    /// <summary>
    /// 屏蔽的结构体
    /// </summary>
    public class BpOffsetSeparateInformation
    {
        /// <summary>
        /// 创建信息
        /// </summary>
        /// <param name="substrateIndex">sub的索引</param>
        /// <param name="moduleIndex">module的索引</param>
        /// <param name="bondPositionName">焊点的索引</param>
        /// <param name="offsetX">偏移X</param>
        /// <param name="offsetY">偏移Y</param>
        /// <param name="offsetZ">偏移Z</param>
        /// <param name="offsetAngle">偏移角度</param>
        public BpOffsetSeparateInformation(int substrateIndex, int moduleIndex, string bondPositionName, double offsetX, double offsetY, double offsetZ, double offsetAngle)
        {
            this.SubstrateIndex = substrateIndex;
            this.ModuleIndex = moduleIndex;
            this.BondPositionName = bondPositionName;
            this.BondPositionIndex = 0;
            this.OffsetX = offsetX;
            this.OffsetY = offsetY;
            this.OffsetZ = offsetZ;
            this.OffsetAngle = offsetAngle;
        }

        /// <summary>
        /// Substrate的索引
        /// </summary>
        public int SubstrateIndex { get; set; }

        /// <summary>
        /// Module的索引
        /// </summary>
        public int ModuleIndex { get; set; }

        /// <summary>
        /// BondPosition的索引
        /// </summary>
        public int BondPositionIndex { get; set; }

        /// <summary>
        /// 偏移量X
        /// </summary>
        public double OffsetX { get; set; }

        /// <summary>
        /// 偏移量Y
        /// </summary>
        public double OffsetY { get; set; }

        /// <summary>
        /// 偏移量Z
        /// </summary>
        public double OffsetZ { get; set; }

        /// <summary>
        /// 偏移量角度
        /// </summary>
        public double OffsetAngle { get; set; }

        /// <summary>
        /// 被屏蔽焊点的名称
        /// </summary>
        public string BondPositionName { get; set; }
    }

    /// <summary>
    /// 屏蔽的结构体
    /// </summary>
    public struct MatterProductInformationMin
    {
        /// <summary>
        /// 创建信息
        /// </summary>
        /// <param name="bondPositionName">焊点的索引</param>
        /// <param name="matterProductState">实体信息</param>
        public MatterProductInformationMin(string bondPositionName, MatterProductState matterProductState)
        {
            this.BondPositionName = bondPositionName;
            this.MatterProductState = matterProductState;
        }

        /// <summary>
        /// 物体的制程状态
        /// </summary>
        public MatterProductState MatterProductState { get; set; }

        /// <summary>
        /// 被屏蔽焊点的名称
        /// </summary>
        public string BondPositionName { get; set; }
    }


}
