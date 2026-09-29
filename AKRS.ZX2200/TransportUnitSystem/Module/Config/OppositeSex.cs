using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Config
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.TransportUnitSystem.Model;

    using Newtonsoft.Json;

    /// <summary>
    /// 异性配置
    /// </summary>
    [Serializable]
    public class OppositeSex
    {
        /// <summary>
        /// 基板索引
        /// </summary>
        public int SubstrateId { get; set; }

        /// <summary>
        /// 基岛索引
        /// </summary>
        public int ModuleId { get; set; }

         /// <summary>
        /// 焊点索引
        /// </summary>
        public int BondPositionId { get; set; }

        /// <summary>
        /// 自己的索引
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 父类的索引
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 配置文件的名称
        /// </summary>
        public string ConfigName { get; set; }

        /// <summary>
        /// 大小X
        /// </summary>
        public double SizeX { get; set; }

        /// <summary>
        /// 大小Y
        /// </summary>
        public double SizeY { get; set; }

        /// <summary>
        /// 中心对于参考点的偏移X
        /// </summary>
        public double CenterOffsetX { get; set; }

        /// <summary>
        /// 是否示教了位置
        /// </summary>
        public bool AssistantPos { get; set; } = false;

        /// <summary>
        /// 中心对于参考点的偏移Y
        /// </summary>
        public double CenterOffsetY { get; set; }

        /// <summary>
        /// 是否锁住
        /// </summary>
        public bool IsLock { get; set; } = true;

        /// <summary>
        /// 对上一层坐标系的偏移量
        /// </summary>
        public ElementCoordinate ElementCoordinate { get; set; } = new ElementCoordinate();

        /// <summary>
        /// 相对于上一层参考点的绝对位置
        /// </summary>
        public ElementCoordinate AbsoluteCoordinate { get; set; } = new ElementCoordinate();

        /// <summary>
        /// 相对于上一层中心点的绝对位置
        /// </summary>
        public ElementCoordinate AbsoluteCenterCoordinate { get; set; } = new ElementCoordinate();

        /// <summary>
        /// 下层配置文件
        /// </summary>
        public List<OppositeSex> DownConfigs { get; set; } = new List<OppositeSex>();

        /// <summary>
        /// 类型
        /// </summary>
        public EntityTypeEnum MatterTypeEnum { get; set; }

        /// <summary>
        /// 实体状态
        /// </summary>
        public MatterProductState MatterProductState { get; set; }

        /// <summary>
        /// 配置文件
        /// </summary>
        [JsonIgnore]
        public OppositeSexConfig Config =>
            this.GetConfig();

        /// <summary>
        /// 创建配置文件
        /// </summary>
        /// <returns>结果</returns>
        private OppositeSexConfig GetConfig()
        {
            OppositeSexConfig oppositeSexConfig = ProductConfiguration.GetInstance().OppositeSexConfiguration.BaseConfigs.Find(it => it.Name == this.ConfigName);

            if (oppositeSexConfig == null)
            {
                oppositeSexConfig = new OppositeSexConfig() { Name = this.ConfigName, EntityType = this.MatterTypeEnum };
                ProductConfiguration.GetInstance().OppositeSexConfiguration.BaseConfigs.Add(oppositeSexConfig);
                ProductConfiguration.GetInstance().Save();
            }

            return oppositeSexConfig;
        }

        /// <summary>
        /// 获取默认名称
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>结果</returns>
        public static string GetDefaultName(string name)
        {
            List<string> listName = ProductConfiguration.GetInstance().OppositeSexConfiguration.GetOppositeSexConfigs()
                .Select(it => it.Name).ToList();
            for (int i = 1; i < int.MaxValue; i++)
            {
                string newName = name + i.ToString();
                if (listName.Exists(it => it == newName))
                {
                    continue;
                }

                return newName;
            }

            throw new Exception("名称超范围");
        }

        /// <summary>
        /// 是否存在名称
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>结果</returns>
        public static bool IsExistName(string name)
        {
            return ProductConfiguration.GetInstance().OppositeSexConfiguration.GetOppositeSexConfigs()
                .Exists(it => it.Name == name);
        }

        /// <summary>
        /// 是否示教完成
        /// </summary>
        /// <returns>事件源</returns>
        public bool AssistantSuccess()
        {
            if (this.MatterTypeEnum == EntityTypeEnum.BondPosition)
            {
                return this.Config.IsAfterBondAssistant && this.Config.IsEpoxyCheckAssistant
                                                        && this.Config.IsMeasureHeightAssistant
                                                        && this.Config.IsPrAssistant
                                                        && this.AssistantPos;
            }
            else
            {
                if (this.DownConfigs.Count == 0)
                {
                    return false;
                }

                return !this.DownConfigs.Exists(it => !it.AssistantSuccess());
            }
        }

        /// <summary>
        /// 获取实体状态
        /// </summary>
        /// <returns>结果</returns>
        public MatterProductState GetMatterProductState()
        {
            List<OppositeSex> list = this.GetOppositeSexList(this);

            if (list.Exists(it => it.MatterTypeEnum == EntityTypeEnum.BondPosition && it.MatterProductState == MatterProductState.Enable))
            {
                return MatterProductState.Enable;
            }
            else
            {
                return MatterProductState.Disable;
            }
        }

        /// <summary>
        /// 设置实体状态
        /// </summary>
        /// <param name="matterProductState">名称</param>
        public void SetMatterProductState(MatterProductState matterProductState)
        {
            List<OppositeSex> list = this.GetOppositeSexList(this);

            foreach (OppositeSex oppositeSex in list)
            {
                oppositeSex.MatterProductState = matterProductState;
            }
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="oppositeSex">配置对象</param>
        /// <returns>结果</returns>
        private List<OppositeSex> GetOppositeSexList(OppositeSex oppositeSex)
        {
            List<OppositeSex> list = new List<OppositeSex>();
            list.Add(oppositeSex);
            foreach (OppositeSex opposite in oppositeSex.DownConfigs)
            {
                list.AddRange(this.GetOppositeSexList(opposite));
            }

            return list;
        }
    }
}
