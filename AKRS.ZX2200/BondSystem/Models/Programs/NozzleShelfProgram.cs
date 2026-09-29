#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/11/8 9:48:44
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>

using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.Models;
using DevExpress.XtraEditors;
using Newtonsoft.Json;

namespace AKRS.ZX2200.BondSystem.Models.Programs
{
    using System;
    using System.Linq;

    using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
    using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
    using AKRS.ZX2200.Consumables;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.SupportFeature.Consumables;

    /// <summary>
    /// 吸嘴架程式
    /// </summary>
    public class NozzleShelfProgram : BaseTool
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
        }

        /// <summary>
        /// 吸嘴架名称
        /// </summary>
        public string NozzleShelfName { get; set; }

        /// <summary>
        /// 吸嘴架是否安全
        /// </summary>
        public bool IsSafe { get; set; }

        /// <summary>
        /// 吸嘴架对象
        /// </summary>
        [JsonIgnore]
        public NozzleShelf NozzleShelf =>
            (NozzleShelf)NozzleShelfRepository.GetInstance().Find(this.NozzleShelfName);

        /// <summary>
        /// 判断吸嘴架是否能正常工作
        /// </summary>
        /// <returns>结果</returns>
        public override bool CanWorkProperly()
        {
            // 先判断一下这个吸嘴架的参数有没有编辑好
            if (this.NozzleShelf.EditState != EditStateEnum.Completely)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取吸嘴架上所有的吸嘴名
        /// </summary>
        /// <returns>吸嘴架上所有的吸嘴名</returns>
        public List<string> GetNozzleNameList()
        {
            List<string> nozzleNameList = new List<string>();
            if (this.NozzleShelf != null)
            {
                foreach (var item in this.NozzleShelf.NozzleShelfSlots)
                {
                    if (!string.IsNullOrEmpty(item.NozzleName))
                    {
                        nozzleNameList.Add(item.NozzleName);
                    }
                }
            }

            return nozzleNameList;
        }

        /// <summary>
        /// 获取吸嘴架上所有的吸嘴对象
        /// </summary>
        /// <returns>吸嘴架上所有的吸嘴对象</returns>
        public List<Nozzle> GetNozzleList()
        {
            List<Nozzle> nozzleList = new List<Nozzle>();
            if (this.GetNozzleNameList() != null)
            {
                nozzleList = this.GetNozzleNameList()
                    .Select(nozzleName => NozzleRepository.GetInstance().GetNozzle(nozzleName))
                    .Where(nozzle => nozzle.NozzleType == NozzleTypeEnum.PickAndPlace).ToList();
                //foreach (var item in this.GetNozzleNameList())
                //{
                //    nozzleList.Add(NozzleRepository.GetInstance().GetNozzle(item));
                //}
            }

            return nozzleList;
        }

        /// <summary>
        /// 获取当前吸嘴名
        /// </summary>
        /// <returns>吸嘴名</returns>
        public string GetToolOnBondhead()
        {
            if (this.NozzleShelf.NozzleShelfSlots.Where(it => it.NozzleState == NozzleStateEnum.OnBondHead).Count() > 1)
            {
                throw new Exception("吸嘴架配置错误：有超过一个吸嘴在焊头上！");
            }

            foreach (NozzleShelfSlot nozzleShelfSlot in this.NozzleShelf.NozzleShelfSlots)
            {
                if (nozzleShelfSlot.NozzleState == NozzleStateEnum.OnBondHead)
                {
                    return nozzleShelfSlot.NozzleName;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取当前吸嘴的槽位
        /// </summary>
        /// <returns>吸嘴槽</returns>
        public NozzleShelfSlot GetCurrentToolNozzleShelfSlot()
        {
            foreach (NozzleShelfSlot nozzleShelfSlot in this.NozzleShelf.NozzleShelfSlots)
            {
                if (nozzleShelfSlot.NozzleState == NozzleStateEnum.OnBondHead)
                {
                    return nozzleShelfSlot;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取当前吸嘴名
        /// </summary>
        /// <returns>吸嘴名</returns>
        public List<FrequencyConsumables> GetToolConsumable()
        {
            List<FrequencyConsumables> frequencyConsumablesList = new List<FrequencyConsumables>();
            if (this.GetNozzleNameList() != null)
            {
                foreach (var item in this.GetNozzleNameList())
                {
                    if (NozzleRepository.GetInstance().GetNozzle(item) == null)
                    {
                        continue;
                    }

                    FrequencyConsumables frequencyConsumables = NozzleRepository.GetInstance().GetNozzle(item)?.FrequencyConsumables;
                    if (frequencyConsumables == null || frequencyConsumables.Name != NozzleRepository.GetInstance().GetNozzle(item).Name)
                    {
                        NozzleRepository.GetInstance().GetNozzle(item).FrequencyConsumables = new FrequencyConsumables(NozzleRepository.GetInstance().GetNozzle(item).Name);
                        frequencyConsumablesList.Add(NozzleRepository.GetInstance().GetNozzle(item).FrequencyConsumables);
                        BondProgram.GetInstance().Save();
                    }
                    else
                    {
                        frequencyConsumablesList.Add(frequencyConsumables);
                    }
                }
            }

            return frequencyConsumablesList;
        }

        /// <summary>
        /// 获取吸嘴架上所有的吸嘴PR名
        /// </summary>
        /// <returns>吸嘴架上所有的吸嘴名</returns>
        public List<string> GetNozzlePRNameList()
        {
            List<string> nozzleNameList = new List<string>();
            if (this.NozzleShelf != null)
            {
                foreach (var item in this.NozzleShelf.NozzleShelfSlots)
                {
                    if (!string.IsNullOrEmpty(item.NozzleName))
                    {
                        nozzleNameList.Add(item.NozzleName+"NozzlePR");
                    }
                }
            }

            return nozzleNameList;
        }

        /// <summary>
        /// 程式自检
        /// </summary>
        /// <returns>结果</returns>
        public bool SelfCheck()
        {
            if (this.NozzleShelf.GetNozzlesOnToolBank()
                    .Any(nozzle => nozzle.Name != "TouchDown" && nozzle.Name != "BMC" && nozzle.IsAssistantSucceed == false) == true)
            {
                string nozzle = this.NozzleShelf.GetNozzlesOnToolBank().FindLast(
                    nozzle => nozzle.Name != "TouchDown" && nozzle.IsAssistantSucceed == false).Name;

                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"吸嘴:{nozzle}未示教完成!\r\n  请先示教吸嘴!",
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);


                return false;
            }

            if (this.NozzleShelf.NozzleShelfSlots.Find(slot => slot.NozzleState == NozzleStateEnum.OnBondHead).Count()
                > 1)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"配置错误：在焊头上的吸嘴超过1个！",
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                throw new Exception("配置错误：在焊头上的吸嘴超过1个！");
            }

            return true;
        }
    }
}