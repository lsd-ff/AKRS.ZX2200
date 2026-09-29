using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
using DevExpress.Office.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities
{
    using System.Drawing;

    /// <summary>
    /// 华夫盒槽状态
    /// </summary>
    [Serializable]
    public class AdapterSlotEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="wafflePlateSlot">wafflePlateSlot</param>
        public AdapterSlotEntity(AdapterSlotConfig wafflePlateSlot)
        {
            if (wafflePlateSlot == null)
            {
                return;
            }

            this.AdapterSlotConfig = wafflePlateSlot;

            this.WaffleSlots = new AcupointEntity[this.AdapterSlotConfig.CarrierWithWaffleConfig.RowCount, this.AdapterSlotConfig.CarrierWithWaffleConfig.ColumnCount];
            for (int i = 0; i < this.AdapterSlotConfig.CarrierWithWaffleConfig.RowCount; i++)
            {
                for (int j = 0; j < this.AdapterSlotConfig.CarrierWithWaffleConfig.ColumnCount; j++)
                {
                    int index = this.SortToSnakeHorizontal(this.AdapterSlotConfig.CarrierWithWaffleConfig.RowCount, this.AdapterSlotConfig.CarrierWithWaffleConfig.ColumnCount, i, j);
                    this.WaffleSlots[i, j] = new AcupointEntity() { Index = index, RowIndex = i, ColumnIndex = j };
                    this.WaffleSlots[i, j].SlotPosition = this.AdapterSlotConfig.FirstPosition - this.AdapterSlotConfig.CarrierWithWaffleConfig.CarrierColSpacing * j - this.AdapterSlotConfig.CarrierWithWaffleConfig.CarrierRowSpacing * i;
                }
            }
        }               

        /// <summary>
        /// 获取槽位
        /// </summary>
        /// <returns>WaffleSlot</returns>
        public AcupointEntity GetWaffleSlot()
        {
            return this.WaffleSlotsList.Find(item => item.Index == this.CurrentIndex);
        }

        /// <summary>
        /// 设置槽位状态
        /// </summary>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="columnIndex">columnIndex</param>
        /// <param name="slotStatu">slotStatu</param>
        public void SetSlotState(int rowIndex, int columnIndex, SlotStatuEnum slotStatu)
        {
            try
            {
                if (this.WaffleSlots[rowIndex, columnIndex] != null)
                {
                    this.WaffleSlots[rowIndex, columnIndex].SlotState = slotStatu;
                }
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
                foreach (var item in this.WaffleSlots)
                {
                    if (item != null)
                    {
                        item.SlotState = slotStatu;
                    }
                }
            }
            catch (Exception e)
            {
                //throw new Exception("Error updating slot status!");
                throw new Exception("更新槽状态失败！");
            }
            finally
            {
                this.CurrentIndex = 0;
            }
        }

        /// <summary>
        /// 槽索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 华夫盘槽位配置
        /// </summary>
        public AdapterSlotConfig AdapterSlotConfig { get; set; }

        /// <summary>
        /// 槽位状态
        /// </summary>
        public SlotStatuEnum SlotState { get; set; } = SlotStatuEnum.None;

        /// <summary>
        /// 当前Index
        /// </summary>
        public int CurrentIndex { get; set; }

        /// <summary>
        /// 华夫盒槽位
        /// </summary>
        public AcupointEntity[,] WaffleSlots { get; set; }

        /// <summary>
        /// 华夫盒槽位List
        /// </summary>
        [JsonIgnore]
        public List<AcupointEntity> WaffleSlotsList => this.ConvertList(this.WaffleSlots);

        /// <summary>
        /// 水平蛇形排序
        /// </summary>
        /// <param name="rowC">rowC</param>
        /// <param name="colC">colC</param>
        /// <param name="rowI">rowI</param>
        /// <param name="colI">colI</param>
        /// <returns>int</returns>
        public int SortToSnakeHorizontal(int rowC, int colC, int rowI, int colI)
        {
            if (rowI % 2 == 0)
            {
                return rowI * colC + colI;
            }
            else
            {
                return rowI * colC + colC - colI - 1;
            }
        }

        /// <summary>
        /// 槽位列表
        /// </summary>
        /// <param name="waffleSlots">waffleSlots</param>
        /// <returns>List</returns>
        private List<AcupointEntity> ConvertList(AcupointEntity[,] waffleSlots)
        {
            List <AcupointEntity> temp = new List<AcupointEntity>();
            foreach (var item in waffleSlots)
            {
                temp.Add(item);
            }

            return temp;
        }

        /// <summary>
        /// 行数
        /// </summary>
        [JsonIgnore]
        public int RowCount => AdapterSlotConfig.CarrierWithWaffleConfig.RowCount;

        /// <summary>
        /// 列数
        /// </summary>
        [JsonIgnore]
        public int ColumnCount => AdapterSlotConfig.CarrierWithWaffleConfig.ColumnCount;

        /// <summary>
        /// GetPosSpec
        /// </summary>
        /// <param name="acupoint">acupoint</param>
        /// <returns>result</returns>
        internal PointF GetPosSpec(AcupointEntity acupoint)
        {
            if (this.WaffleSlots != null)
            {
                PointF p = acupoint.SpecPos;

                return p;
            }

            return PointF.Empty;
        }

        /// <summary>
        /// 获取穴位
        /// </summary>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="columnIndex">columnIndex</param>
        /// <returns>result</returns>
        public AcupointEntity GetAcupoint(int rowIndex, int columnIndex)
        {
            if (this.WaffleSlots == null)
            {
                return null;
            }

            if (rowIndex < 0 || columnIndex < 0)
            {
                return null;
            }

            if (rowIndex >= this.WaffleSlots.GetLength(0) || columnIndex >= this.WaffleSlots.GetLength(1))
            {
                return null;
            }

            return this.WaffleSlots[rowIndex, columnIndex];
        }

        /// <summary>
        /// 获取穴位矩形
        /// </summary>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="columnIndex">columnIndex</param>
        /// <returns>result</returns>
        internal RectangleF GetAcupointRect(int rowIndex, int columnIndex)
        {
            if (this.WaffleSlots != null)
            {
                if (rowIndex >= this.WaffleSlots.GetLength(0) || columnIndex >= this.WaffleSlots.GetLength(1))
                {
                    return Rectangle.Empty;
                }

                return this.WaffleSlots[rowIndex, columnIndex].Rect;
            }
            else
            {
                return Rectangle.Empty;
            }
        }

        /// <summary>
        /// 获取穴位矩形
        /// </summary>
        /// <param name="index">rowIndex</param>
        /// <returns>result</returns>
        internal RectangleF GetAcupointRect(int index)
        {
            if (this.WaffleSlots != null)
            {
                return this.WaffleSlotsList[index].Rect;
            }
            else
            {
                return Rectangle.Empty;
            }
        }
    }
}
