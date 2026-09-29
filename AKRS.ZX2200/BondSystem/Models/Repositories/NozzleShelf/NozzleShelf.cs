using System;
using System.Collections.Generic;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.Models;
using DevExpress.Office.Utils;
using Newtonsoft.Json;

namespace AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;

    using Nozzle = AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle.Nozzle;

    /// <summary>
    /// 吸嘴架模型
    /// </summary>
    [Serializable]
    public class NozzleShelf : BaseDsSetting
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NozzleShelf()
        {
            this.CreatNozzleShelfSlots();
        }

        /// <summary>
        /// 所有穴位,第一个穴位属性用来测试，后续会删掉
        /// </summary>
        public NozzleShelfSlot[] NozzleShelfSlots { get; set; } = new[]
                                                                      {
                                                                          new NozzleShelfSlot { Index = 0, MaximumToolSize = MaximumToolSizeEnum.SmallTool, NozzleState = NozzleStateEnum.Empty },
                                                                          new NozzleShelfSlot { Index = 1, MaximumToolSize = MaximumToolSizeEnum.LargeTool, NozzleState = NozzleStateEnum.Empty },
                                                                          new NozzleShelfSlot { Index = 2, MaximumToolSize = MaximumToolSizeEnum.SmallTool, NozzleState = NozzleStateEnum.Empty },
                                                                          new NozzleShelfSlot { Index = 3, MaximumToolSize = MaximumToolSizeEnum.LargeTool, NozzleState = NozzleStateEnum.Empty },
                                                                          new NozzleShelfSlot { Index = 4, MaximumToolSize = MaximumToolSizeEnum.SmallTool, NozzleState = NozzleStateEnum.Empty },
                                                                          new NozzleShelfSlot { Index = 5, MaximumToolSize = MaximumToolSizeEnum.LargeTool, NozzleState = NozzleStateEnum.Empty },
                                                                          new NozzleShelfSlot { Index = 6, MaximumToolSize = MaximumToolSizeEnum.SmallTool, NozzleState = NozzleStateEnum.Empty }
                                                                      };

        #region  方法

        /// <summary>
        /// 创建槽位
        /// </summary>
        private void CreatNozzleShelfSlots()
        {
            int slotNum = BondDevicePara.GetInstance().NozzleShelfParam.SlotNum;
            this.NozzleShelfSlots = new NozzleShelfSlot[slotNum];

            for (int i = 0; i < slotNum; i++)
            {
                this.NozzleShelfSlots[i] = new NozzleShelfSlot { Index = i, MaximumToolSize = MaximumToolSizeEnum.SmallTool, NozzleState = NozzleStateEnum.Empty };
            }
        }

        /// <summary>
        /// 根据吸嘴名获取取吸嘴的角度
        /// </summary>
        /// <param name="name">吸嘴名</param>
        /// <returns>结果</returns>
        public double GetPickNozzleSlotAngle(string name)
        {
            for (int i = 0; i < this.NozzleShelfSlots.Length; i++)
            {
                if (this.NozzleShelfSlots[i].NozzleName == name)
                {
                    return BondDevicePara.GetInstance().NozzleShelfParam.PickNozzleAngle[i];
                }
            }

            return 0;
        }

        /// <summary>
        /// 根据吸嘴名获取放置吸嘴的角度
        /// </summary>
        /// <param name="name">吸嘴名</param>
        /// <returns>结果</returns>
        public double GetPlaceNozzleSlotAngle(string name)
        {
            for (int i = 0; i < this.NozzleShelfSlots.Length; i++)
            {
                if (this.NozzleShelfSlots[i].NozzleName == name)
                {
                    return BondDevicePara.GetInstance().NozzleShelfParam.PlaceNozzleAngle[i];
                }
            }

            return 0;
        }

        /// <summary>
        /// 指定吸嘴是否在吸嘴架上
        /// </summary>
        /// <param name="name">吸嘴名</param>
        /// <returns>结果</returns>
        public bool IsOnToolBank(string name)
        {
            foreach (var slot in this.NozzleShelfSlots)
            {
                if (slot.NozzleName == name)
                { 
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 根据吸嘴名获取取吸嘴的位置(G0)
        /// </summary>
        /// <param name="name">吸嘴名</param>
        /// <returns>结果</returns>
        public AKRSPoint3D GetPickNozzleSlotPos(string name)
        {
            for (int i = 0; i < this.NozzleShelfSlots.Length; i++)
            {
                if (this.NozzleShelfSlots[i].NozzleName == name)
                {
                    AKRSPoint3D pos = new AKRSPoint3D(
                        BondDevicePara.GetInstance().NozzleShelfParam.PickNozzlePos[i].X,
                        BondDevicePara.GetInstance().NozzleShelfParam.PickNozzlePos[i].Y,
                        BondDevicePara.GetInstance().NozzleShelfParam.PickNozzlePos[i].Z + BondDevicePara.GetInstance().NozzleShelfParam.ChangeToolDistanceZ);

                    return pos;
                }
            }

            return new AKRSPoint3D();
        }

        /// <summary>
        /// 根据吸嘴名获取还吸嘴的位置（G0）
        /// </summary>
        /// <param name="name">吸嘴名</param>
        /// <returns>结果</returns>
        public AKRSPoint3D GetPlaceNozzleSlotPos(string name)
        {
            for (int i = 0; i < this.NozzleShelfSlots.Length; i++)
            {
                if (this.NozzleShelfSlots[i].NozzleName == name)
                {
                    AKRSPoint3D pos = new AKRSPoint3D(
                        BondDevicePara.GetInstance().NozzleShelfParam.PlaceNozzlePos[i].X,
                        BondDevicePara.GetInstance().NozzleShelfParam.PlaceNozzlePos[i].Y - BondDevicePara.GetInstance().NozzleShelfParam.ChangeToolDistanceY,
                        BondDevicePara.GetInstance().NozzleShelfParam.PlaceNozzlePos[i].Z);

                    return pos;
                }
            }

            return new AKRSPoint3D();
        }

        /// <summary>
        /// 获取吸嘴架上所有吸嘴
        /// </summary>
        /// <returns>吸嘴</returns>
        public List<Nozzle> GetNozzlesOnToolBank()
        {
            List<Nozzle> nozzles = new List<Nozzle>();

            foreach (var nozzleShelfSlot in this.NozzleShelfSlots)
            {
                Nozzle nozzle = (Nozzle)NozzleRepository.GetInstance().Find(nozzleShelfSlot.NozzleName);

                if (nozzle != null)
                {
                    nozzles.Add(nozzle);
                }
            }

            return nozzles;
        }

        #endregion
    }


    /// <summary>
    /// 吸嘴架穴位模型
    /// </summary>
    [Serializable]
    public class NozzleShelfSlot
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 穴位名
        /// </summary>
        public string SlotName => $"Slot: {Index}";

        /// <summary>
        /// 吸嘴槽的位置
        /// </summary>
        [JsonIgnore]
        public AKRSPoint3D SlotPosition => BondDevicePara.GetInstance().NozzleShelfParam.PlaceNozzlePos[this.Index];

        /// <summary>
        /// 穴位号
        /// </summary>
        [JsonIgnore]
        public int SlotNum => this.Index + 1;

        /// <summary>
        /// 此穴位上的吸嘴名称
        /// </summary>
        public string NozzleName { get; set; } = string.Empty;

        /// <summary>
        /// 此穴位上的吸嘴对象
        /// </summary>
        [JsonIgnore]
        public Nozzle Nozzle => NozzleRepository.GetInstance().GetNozzle(this.NozzleName);

        /// <summary>
        /// 此穴位上的吸嘴类型
        /// </summary>
        [JsonIgnore]
        public string NozzleType => this.Nozzle?.NozzleType.ToString();

        /// <summary>
        /// 吸嘴槽能容纳的最大吸嘴尺寸
        /// </summary>
        public MaximumToolSizeEnum MaximumToolSize { get; set; }

        /// <summary>
        /// 吸嘴槽状态枚举
        /// </summary>
        public NozzleStateEnum NozzleState { get; set; }
    }
}
