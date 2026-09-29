using DevExpress.CodeParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Config
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using DevExpress.Utils.Extensions;

    /// <summary>
    /// 异性配置类
    /// </summary>
    public class OppositeSexConfiguration
    {
        /// <summary>
        /// 基板的配置对象
        /// </summary>s
        public OppositeSex OppositeSexConfigs { get; set; } = new ()
                                                                  {
                                                                      Name = "传输单元",
                                                                      ConfigName = "传输单元",
                                                                      SizeX = 300,
                                                                      SizeY = 300,
                                                                      MatterTypeEnum = EntityTypeEnum.TransportUnit,
                                                                      ElementCoordinate = new ElementCoordinate(
                                                                          0,
                                                                          new AKRSPoint3D(0, 0, 20)),
                                                                  };

        /// <summary>
        /// 配置文件集合
        /// </summary>
        public List<OppositeSexConfig> BaseConfigs { get; set; } = new List<OppositeSexConfig> { };

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>结果</returns>
        public OppositeSexConfig GetOppositeSexConfig(string name)
        {
            OppositeSexConfig oppositeSexConfig = this.BaseConfigs.Find(x => x.Name == name);
            if (oppositeSexConfig == null) 
            {
                oppositeSexConfig = new OppositeSexConfig();
                this.BaseConfigs.Add(oppositeSexConfig);
                ProductConfiguration.GetInstance().Save();
            }

            return oppositeSexConfig;
        }

        /// <summary>
        /// 初始化索引
        /// </summary>
        public void InitOppositeSexConfigIndex()
        {
            this.OppositeSexConfigs.Id = 1;

            int count = 2;

            for (int i = 0; i < this.OppositeSexConfigs.DownConfigs.Count; i++)
            {
                this.OppositeSexConfigs.DownConfigs[i].Id = count;
                this.OppositeSexConfigs.DownConfigs[i].ParentId = 1;
                this.OppositeSexConfigs.DownConfigs[i].SubstrateId = count;
                this.OppositeSexConfigs.DownConfigs[i].ModuleId = 0;
                this.OppositeSexConfigs.DownConfigs[i].BondPositionId = 0;
                count++;
            }

            for (int i = 0; i < this.OppositeSexConfigs.DownConfigs.Count; i++)
            {
                for (int j = 0; j < this.OppositeSexConfigs.DownConfigs[i].DownConfigs.Count; j++)
                {
                    this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].Id = count;
                    this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].ParentId = this.OppositeSexConfigs.DownConfigs[i].Id;
                    this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].SubstrateId = this.OppositeSexConfigs.DownConfigs[i].SubstrateId;
                    this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].ModuleId = j;
                    this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].BondPositionId = 0;
                    count++;
                }
            }

            for (int i = 0; i < this.OppositeSexConfigs.DownConfigs.Count; i++)
            {
                for (int j = 0; j < this.OppositeSexConfigs.DownConfigs[i].DownConfigs.Count; j++)
                {
                    for (int k = 0; k < this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].DownConfigs.Count; k++)
                    {
                        this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].DownConfigs[k].Id = count;
                        this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].DownConfigs[k].ParentId = this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].Id;
                        this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].DownConfigs[k].SubstrateId = this.OppositeSexConfigs.DownConfigs[i].SubstrateId;
                        this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].DownConfigs[k].ModuleId = this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].ModuleId;
                        this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].DownConfigs[k].BondPositionId = k;
                        count++;
                    }
                }
            }
        }

        /// <summary>
        /// 初始化绝对位置
        /// </summary>
        public void InitOppositeSexConfigPosition()
        {
            this.OppositeSexConfigs.AbsoluteCoordinate = new ElementCoordinate(0, new AKRSPoint3D());
            for (int i = 0; i < this.OppositeSexConfigs.DownConfigs.Count; i++)
            {
                this.OppositeSexConfigs.DownConfigs[i].AbsoluteCoordinate =
                    this.OppositeSexConfigs.AbsoluteCoordinate + this.OppositeSexConfigs.DownConfigs[i].ElementCoordinate;
                double x = this.OppositeSexConfigs.DownConfigs[i].CenterOffsetX - this.OppositeSexConfigs.CenterOffsetX;
                double y = this.OppositeSexConfigs.DownConfigs[i].CenterOffsetY - this.OppositeSexConfigs.CenterOffsetY;
                this.OppositeSexConfigs.DownConfigs[i].AbsoluteCenterCoordinate =
                    this.OppositeSexConfigs.DownConfigs[i].AbsoluteCoordinate
                    + new ElementCoordinate(0, new AKRSPoint3D(x, y, 0));
            }

            for (int i = 0; i < this.OppositeSexConfigs.DownConfigs.Count; i++)
            {
                for (int j = 0; j < this.OppositeSexConfigs.DownConfigs[i].DownConfigs.Count; j++)
                {
                    this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].AbsoluteCoordinate =
                        this.OppositeSexConfigs.DownConfigs[i].AbsoluteCoordinate
                        + this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].ElementCoordinate;

                    double x = this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].CenterOffsetX - this.OppositeSexConfigs.DownConfigs[i].CenterOffsetX;
                    double y = this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].CenterOffsetY - this.OppositeSexConfigs.DownConfigs[i].CenterOffsetY;
                    this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].AbsoluteCenterCoordinate =
                        this.OppositeSexConfigs.DownConfigs[i].AbsoluteCenterCoordinate + this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].ElementCoordinate
                        + new ElementCoordinate(0, new AKRSPoint3D(x, y, 0));
                }
            }

            for (int i = 0; i < this.OppositeSexConfigs.DownConfigs.Count; i++)
            {
                for (int j = 0; j < this.OppositeSexConfigs.DownConfigs[i].DownConfigs.Count; j++)
                {
                    for (int k = 0; k < this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].DownConfigs.Count; k++)
                    {
                        double x = this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].DownConfigs[k].CenterOffsetX - this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].CenterOffsetX;
                        double y = this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].DownConfigs[k].CenterOffsetY - this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].CenterOffsetY;
                        this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].DownConfigs[k].AbsoluteCenterCoordinate =
                            this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].AbsoluteCenterCoordinate
                            + this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].DownConfigs[k].ElementCoordinate
                            + new ElementCoordinate(0, new AKRSPoint3D(x, y, 0));
                    }
                }
            }
        }

        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <returns>结果</returns>
        public List<OppositeSex> GetOppositeSexConfigs()
        {
            List<OppositeSex> list = new List<OppositeSex>();
            list.Add(this.OppositeSexConfigs);
            for (int i = 0; i < this.OppositeSexConfigs.DownConfigs.Count; i++)
            {
                list.Add(this.OppositeSexConfigs.DownConfigs[i]);
                for (int j = 0; j < this.OppositeSexConfigs.DownConfigs[i].DownConfigs.Count; j++)
                {
                    list.Add(this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j]);
                    for (int k = 0; k < this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].DownConfigs.Count; k++)
                    {
                        list.Add(this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].DownConfigs[k]);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 获取所有的基板配置
        /// </summary>
        /// <returns>结果</returns>
        public List<OppositeSex> GetAllSubstrateConfigs()
        {
            return this.OppositeSexConfigs.DownConfigs;
        }

        /// <summary>
        /// 获取所有的基岛配置
        /// </summary>
        /// <returns>结果</returns>
        public List<OppositeSex> GetAllModuleConfigs()
        {
            List<OppositeSex> list = new List<OppositeSex>();
            for (int i = 0; i < this.OppositeSexConfigs.DownConfigs.Count; i++)
            {
                for (int j = 0; j < this.OppositeSexConfigs.DownConfigs[i].DownConfigs.Count; j++)
                {
                    list.Add(this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j]);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取所有的焊点配置
        /// </summary>
        /// <returns>结果</returns>
        public List<OppositeSex> GetAllBondPositionConfigs()
        {
            List<OppositeSex> list = new List<OppositeSex>();
            for (int i = 0; i < this.OppositeSexConfigs.DownConfigs.Count; i++)
            {
                for (int j = 0; j < this.OppositeSexConfigs.DownConfigs[i].DownConfigs.Count; j++)
                {
                    for (int k = 0; k < this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].DownConfigs.Count; k++)
                    {
                        list.Add(this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j]);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="oppositeSexConfig">节点</param>
        public void RemoveOppositeSexConfig(OppositeSex oppositeSexConfig)
        {
            for (int i = 0; i < this.OppositeSexConfigs.DownConfigs.Count; i++)
            {
                if (this.OppositeSexConfigs.DownConfigs[i] == oppositeSexConfig)
                {
                    this.OppositeSexConfigs.DownConfigs.RemoveAt(i);
                    goto Remove;
                }

                for (int j = 0; j < this.OppositeSexConfigs.DownConfigs[i].DownConfigs.Count; j++)
                {
                    if (this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j] == oppositeSexConfig)
                    {
                        this.OppositeSexConfigs.DownConfigs[i].DownConfigs.RemoveAt(j);
                        goto Remove;
                    }

                    for (int k = 0; k < this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].DownConfigs.Count; k++)
                    {
                        if (this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].DownConfigs[k] == oppositeSexConfig)
                        {
                            this.OppositeSexConfigs.DownConfigs[i].DownConfigs[j].DownConfigs.RemoveAt(k);
                            goto Remove;
                        }
                    }
                }
            }

        Remove:

            if (!this.GetOppositeSexConfigs().Exists(it => it.ConfigName == oppositeSexConfig.ConfigName))
            {
                this.BaseConfigs.RemoveAll(it => it.Name == oppositeSexConfig.ConfigName);

                List<string> list = new List<string>() { "Mark1", "Mark2", "Mark3", "Mark4" };

                foreach (string mark in list)
                {
                    PREntity.DeletePREntity(MachineConfigContext.GetInstance().RecipeName + oppositeSexConfig.ConfigName + mark);
                    VisionEntityRepository.GetInstance().PRVisionList.RemoveAll(it => it.GetName() == MachineConfigContext.GetInstance().RecipeName + oppositeSexConfig.ConfigName + mark);
                }
            }
        }

        /// <summary>
        /// 移除不存在的配置文件
        /// </summary>
        public void RemoveNotExistConfig()
        {
            for (int i = 0; i < this.BaseConfigs.Count; i++)
            {
                if (!this.GetOppositeSexConfigs().Exists(it => it.ConfigName == this.BaseConfigs[i].Name))
                {
                    this.BaseConfigs.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// 获取基板的数量
        /// </summary>
        /// <returns>结果</returns>
        public int GetSubstrateCount()
        {
            return this.OppositeSexConfigs.DownConfigs.Count;
        }

        /// <summary>
        /// 获取基岛的数量
        /// </summary>
        /// <returns>结果</returns>
        public int GetModuleCount()
        {
            return this.OppositeSexConfigs.DownConfigs.Max(it => it.DownConfigs.Count);
        }

        /// <summary>
        /// 获取对象的父对象
        /// </summary>
        /// <param name="oppositeSex">配置对象</param>
        /// <returns>父类配置对象</returns>
        public OppositeSex GetParentOppositeSex(OppositeSex oppositeSex)
        {
            return this.GetOppositeSexConfigs().Find(it => it.Id == oppositeSex.ParentId);
        }
    }
}
