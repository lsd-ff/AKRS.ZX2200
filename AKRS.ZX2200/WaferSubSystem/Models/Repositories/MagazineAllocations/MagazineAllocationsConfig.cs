using System;
using AKRS.Galaxy2.Infrastructure.Helper;
using System.Collections.Generic;
using System.Linq;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
using DevExpress.Utils;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBox;
using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations
{
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;

    /// <summary>
    /// 料架内容配置
    /// </summary>
    [Serializable]
    public class MagazineAllocationsConfig : BaseDsSetting
    {
        /// <summary>
        /// 最大使用层数
        /// </summary>
        [TreeProgramListArgs("最大使用层数", "magazine配置", UnitHelper.times)]
        public int MaxUseLayerCount { get; set; } = 25;

        /// <summary>
        /// 料片数组
        /// </summary>
        public BaseTablet[] TabletArray { get; set; } = new BaseTablet[]
                                                            {
                                                                new NullTablet() { Index = 0 },
                                                                new NullTablet() { Index = 1 },
                                                                new NullTablet() { Index = 2 },
                                                                new NullTablet() { Index = 3 },
                                                                new NullTablet() { Index = 4 },
                                                                new NullTablet() { Index = 5 },
                                                                new NullTablet() { Index = 6 },
                                                                new NullTablet() { Index = 7 },
                                                                new NullTablet() { Index = 8 },
                                                                new NullTablet() { Index = 9 },
                                                                new NullTablet() { Index = 10 },
                                                                new NullTablet() { Index = 11 },
                                                                new NullTablet() { Index = 12 },
                                                                new NullTablet() { Index = 13 },
                                                                new NullTablet() { Index = 14 },
                                                                new NullTablet() { Index = 15 },
                                                                new NullTablet() { Index = 16 },
                                                                new NullTablet() { Index = 17 },
                                                                new NullTablet() { Index = 18 },
                                                                new NullTablet() { Index = 19 },
                                                                new NullTablet() { Index = 20 },
                                                                new NullTablet() { Index = 21 },
                                                                new NullTablet() { Index = 22 },
                                                                new NullTablet() { Index = 23 },
                                                                new NullTablet() { Index = 24 }
                                                            };

        /// <summary>
        /// 只设置料片状态
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="slotStatu">slotStatu</param>
        public void SetSlotState(int index, SlotStatuEnum slotStatu)
        {
            try
            {
                if (this.TabletArray[index] is NullTablet)
                {
                    return;
                }

                this.TabletArray[index].SlotState = slotStatu;
            }
            catch (Exception e)
            {
                //throw new Exception("Error updating slot status!");
                throw new Exception("更新槽状态失败！");
            }
        }

        /// <summary>
        /// 设置料片及旗下所有槽位状态
        /// </summary>
        /// <param name="baseTablet">baseTablet</param>
        /// <param name="slotStatu">slotStatu</param>
        public void SetSlotState(BaseTablet baseTablet, SlotStatuEnum slotStatu)
        {
            try
            {
                if (baseTablet is NullTablet)
                {
                    return;
                }

                if (baseTablet is WaferTablet wt)
                {
                    if (wt.CarrierConfigWithWafer != null)
                    {
                        wt.SlotState = slotStatu;
                        wt.InitTabletInfo();
                    }
                }
                else if (baseTablet is AdapterTablet adt)
                {
                    if(adt.AdapterSetting != null)
                    {
                        adt.CreateAdapterEntity();
                        adt.SlotState = slotStatu;
                        adt.AdapterState.SetAllSlotState(slotStatu);
                    }                    
                }
            }
            catch (Exception e)
            {
                //throw new Exception("Error updating slot status!");
                throw new Exception("更新槽状态失败！");
            }
        }

        /// <summary>
        /// 设置整个magazine料片的状态
        /// </summary>
        /// <param name="slotStatu">slotStatu</param>
        public void SetAllSlotState(SlotStatuEnum slotStatu)
        {
            try
            {
                for (int i = 0; i < this.MaxUseLayerCount; i++)
                {
                    this.SetSlotState(this.TabletArray[i], slotStatu);
                }

                MagazineAllocationsConfigRepository.GetInstance().Save();
            }
            catch (Exception e)
            {
                //throw new Exception("Error updating slot status!");
                throw new Exception("更新槽状态失败！");
            }
        }
    }
}
