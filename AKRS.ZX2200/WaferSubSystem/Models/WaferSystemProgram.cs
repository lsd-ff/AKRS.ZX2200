using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Programs;
using DevExpress.XtraEditors;
using System.Windows.Forms;

namespace AKRS.ZX2200.WaferSubSystem.Models
{
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AKRS.ZX2200.Infrastructure.Models.Path;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
    using AKRS.ZX2200.SupportFeature.Parameters.Attributes;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using Newtonsoft.Json;

    /// <summary>
    /// 上晶圆程式
    /// </summary>
    public class WaferSystemProgram : Singleton<WaferSystemProgram>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static WaferSystemProgram()
        {
            Singleton<WaferSystemProgram>.FilePath = ZX2200PathConfig.WaferSystemProgram;
        }

        #region 程式

        /// <summary>
        /// EjectionBankProgram
        /// </summary>
        public EjectionBankProgram EjectionBankProgram { get; set; } = new EjectionBankProgram();

        /// <summary>
        /// MagazineBoxProgram
        /// </summary>
        public MagazineBoxProgram MagazineProgram { get; set; } = new MagazineBoxProgram();

        /// <summary>
        /// MagazineAllocationsProgram
        /// </summary>
        public MagazineAllocationsProgram MagazineAllocationsProgram { get; set; } = new MagazineAllocationsProgram();

        /// <summary>
        /// StaticAdapterProgram
        /// </summary>
        public StaticAdapterProgram StaticAdapterProgram { get; set; } = new StaticAdapterProgram();

        /// <summary>
        /// FlipModuleProgram
        /// </summary>
        public FlipModuleProgram FlipModuleProgram { get; set; }= new FlipModuleProgram();

        /// <summary>
        /// 初始化晶圆子系统程式
        /// </summary>
        public void InitWaferSystemProgram()
        {
            try
            {
                if (MachineSoftwareConfiguration.GetInstance().IsClearStaticAdapter)
                {
                    WaferSubController.GetInstance().WaferTableController.ClearWaferSubMemory();
                }
                else
                {
                    WaferSubController.GetInstance().WaferTableController.ClearWaferSubMemory(true, false);
                }

                Singleton<WaferSystemProgram>.FilePath = ZX2200PathConfig.WaferSystemProgram;
                instance = null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// 获取当前程式所使用的芯片
        /// </summary>
        /// <returns>result</returns>
        public List<BaseCarrierConfig> GetCarriers()
        {
            List<BaseCarrierConfig> list = new List<BaseCarrierConfig>();

            if (this.MagazineAllocationsProgram.GetCarriers() != null)
            {
                foreach (var item in this.MagazineAllocationsProgram.GetCarriers())
                {
                    if (CarrierConfigRepository.GetInstance().IsExists(item.Name))
                    {
                        list.Add(item);
                    }
                }
            }

            if (this.StaticAdapterProgram.GetCarriers() != null)
            {
                foreach (var item in this.StaticAdapterProgram.GetCarriers())
                {
                    if (CarrierConfigRepository.GetInstance().IsExists(item.Name))
                    {
                        list.Add(item);
                    }
                }
            }
                       
            list = list.Filter(a => a != null).Distinct().ToList();

            return list;
        }

        /// <summary>
        /// 获取华夫盘配置
        /// </summary>
        /// <returns>result</returns>
        public List<AdapterConfig> GetAdapterConfig()
        {
            List<AdapterConfig> list = new List<AdapterConfig>();

            if (this.MagazineAllocationsProgram.GetAdapterConfig() != null)
            {
                foreach (var item in this.MagazineAllocationsProgram.GetAdapterConfig())
                {
                    if (AdapterConfigRepository.GetInstance().IsExists(item.Name))
                    {
                        list.Add(item);
                    }
                }
            }

            if (this.StaticAdapterProgram != null)
            {
                foreach (var item in this.StaticAdapterProgram.GetAdapterConfig())
                {
                    if (AdapterConfigRepository.GetInstance().IsExists(item.Name))
                    {
                        list.Add(item);
                    }
                }
            }
                        
            list = list.Filter(a => a != null).Distinct().ToList();

            return list;
        }

        /// <summary>
        /// 获取所有芯片的pr模板
        /// </summary>
        /// <returns>result</returns>
        public List<string> GetComponentCarriersPREntityNameList()
        {
            List<string> list = new List<string>();
            foreach (var item in this.GetCarriers())
            {
                if (item.IsTwoPointSearch)
                {
                    list.Add(item.DieMatchName);
                    list.Add(item.DieMatchNameP2);
                }
                else
                {
                    list.Add(item.DieMatchName);
                }     
                
                list.Add(item.DieBlobName);

                if (item.IsFrameSearch)
                {
                    list.Add(item.DieFrameName);
                }
                
                if (item.IsTwoPointAdjust)
                {
                    if (item.AdjustCamera == CameraTypeEnum.BondCamera)
                    {
                        
                        list.Add(item.Name + "DownLookMatch1");
                        list.Add(item.Name + "DownLookMatch2");

                        list.Add(item.Name + "UpLookMatch1");
                        list.Add(item.Name + "UpLookMatch2");
                    }
                    else if (item.AdjustCamera == CameraTypeEnum.UpLookCamera)
                    {                     
                        list.Add(item.Name + "UpLookMatch1");
                        list.Add(item.Name + "UpLookMatch2");

                        list.Add(item.Name + "DownLookMatch1");
                        list.Add(item.Name + "DownLookMatch2");
                    }
                    else
                    {
                        throw new Exception("The camera type is not exist!");
                    }
                }
                else
                {
                    if (item.AdjustCamera == CameraTypeEnum.BondCamera)
                    {   
                        list.Add(item.Name + "DownLookMatch1");

                        list.Add(item.Name + "UpLookMatch1");
                    }
                    else if (item.AdjustCamera == CameraTypeEnum.UpLookCamera)
                    {                  
                        list.Add(item.Name + "UpLookMatch1");

                        list.Add(item.Name + "DownLookMatch1");
                    }
                    else
                    {
                        //throw new Exception("The camera type is not exist!");
                        throw new Exception("相机类型不存在！");
                    }
                }

                if (item.IsDipOnTU)
                {
                    list.Add(item.DipPRName);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取所有顶针
        /// </summary>
        /// <returns>去过重的顶针</returns>
        public List<EjectionConfig> GetDistinctEjectionSettings()
        {
            List<EjectionConfig> list = new List<EjectionConfig>();

            if (EjectionBankConfigRepository.GetInstance().IsExists(this.EjectionBankProgram.Name))
            {
                foreach (var item in this.EjectionBankProgram.CurrentBankConfig.EjectionBankSlots)
                {
                    if (EjectionConfigRepository.GetInstance().IsExists(item.Name))
                    {
                        list.Add(item.EjectionConfig);
                    }
                }
            }

            list = list.Filter(a => a != null)
                .Distinct()
                .ToList();

            return list;
        }

        /// <summary>
        /// 获取所有顶针
        /// </summary>
        /// <returns>去过重的顶针</returns>
        public List<EjectionBankSlotConfig> GetDistinctEjectionBankSlotConfig()
        {
            List<EjectionBankSlotConfig> list = new List<EjectionBankSlotConfig>();

            if (EjectionBankConfigRepository.GetInstance().IsExists(this.EjectionBankProgram.Name))
            {
                foreach (var item in this.EjectionBankProgram.CurrentBankConfig.EjectionBankSlots)
                {
                    if (EjectionConfigRepository.GetInstance().IsExists(item.Name))
                    {
                        list.Add(item);
                    }
                }
            }

            list = list.Filter(a => a != null)
                .Distinct()
                .ToList();

            return list;
        }

        /// <summary>
        /// 获取所有顶针的pr模板
        /// </summary>
        /// <returns>result</returns>
        public List<string> GetEjectionPREntityNameList()
        {
            List<string> list = new List<string>();
            foreach (var item in this.GetDistinctEjectionSettings())
            {
                list.Add(item.EjectMatchName);
            }

            return list;
        }

        #endregion
    }
}
