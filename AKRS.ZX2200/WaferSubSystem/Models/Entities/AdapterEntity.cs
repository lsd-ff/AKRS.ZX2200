using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities
{
    /// <summary>
    /// 华夫盘槽位状态
    /// </summary>
    [Serializable]
    public class AdapterEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxUseWaffleCount">maxUseWaffleCount</param>
        public AdapterEntity(int maxUseWaffleCount)
        {
            this.WafflePlateSlotsState = new AdapterSlotEntity[maxUseWaffleCount];
        }

        /// <summary>
        /// 华夫盘槽位状态
        /// </summary>
        public AdapterSlotEntity[] WafflePlateSlotsState { get; set; }

        /// <summary>
        /// 设置槽位状态
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="slotStatu">slotStatu</param>
        public void SetSlotState(int index, SlotStatuEnum slotStatu)
        {
            try
            {
                if (this.WafflePlateSlotsState[index] != null)
                {
                    this.WafflePlateSlotsState[index].SlotState = slotStatu;
                }
            }
            catch (Exception e)
            {
                //throw new Exception("Error updating slot status!");
                throw new Exception("更新槽状态失败！");
            }
        }

        /// <summary>
        /// 设置槽位状态
        /// </summary>
        /// <param name="adapterSlotEntity">wafflePlateSlotsState</param>
        /// <param name="slotStatu">slotStatu</param>
        public void SetSlotState(AdapterSlotEntity adapterSlotEntity, SlotStatuEnum slotStatu)
        {
            try
            {
                if (adapterSlotEntity == null)
                {
                    return;
                }

                adapterSlotEntity.SlotState = slotStatu;
                adapterSlotEntity.SetAllSlotState(slotStatu);
            }
            catch (Exception e)
            {
                //throw new Exception("Error updating slot status!");
                throw new Exception("更新槽状态失败！");
            }
        }

        /// <summary>
        /// 设置所有槽位状态
        /// </summary>
        /// <param name="slotStatu">slotStatu</param>
        public void SetAllSlotState(SlotStatuEnum slotStatu)
        {
            try
            {
                foreach (var item in this.WafflePlateSlotsState)
                {
                    this.SetSlotState(item, slotStatu);
                }
            }
            catch (Exception e)
            {
                //throw new Exception("Error updating slot status!");
                throw new Exception("更新槽状态失败！");
            }
        }
    }
}
