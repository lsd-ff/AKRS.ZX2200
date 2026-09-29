#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2024/3/23 18:06:59
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

using System;
using System.Windows.Forms;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.InOutPut;
using AKRS.ZX2200.TransportSystem.Models.Programs;
using AKRS.ZX2200.TransportSystem.Modules;

namespace AKRS.ZX2200.TransportSystem.Controllers
{
    /// <summary>
    /// 描述：下料载台控制器
    /// </summary>
    public class UnloadingSubSectionController : BaseSubSectionController
    {
        /// <summary>
        /// 下料载台模组
        /// </summary>
        private UnloadingSubSectionModule unloadingSubSectionModule => TransportModule.GetInstance().UnloadingSubSectionModule;

        /// <summary>
        /// 下料载台程式
        /// </summary>
        private UnloadingSubSectionProgram unloadingSubSectionProgram => TransportProgram.GetInstance().UnloadingSubSectionProgram;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UnloadingSubSectionController()
        {
            this.SubSectionModule = TransportModule.GetInstance().UnloadingSubSectionModule;
            this.SubSectionProgram = TransportProgram.GetInstance().UnloadingSubSectionProgram;
        }

        /// <summary>
        ///  搜索下料区是否有料
        /// </summary>
        /// <returns>
        /// true: 搜索到并传料成功，
        /// false：没搜索到或者搜索到了后传料失败
        /// 里面不涉及任何状态参数的修改
        /// </returns>
        public override bool SearchBelt()
        {
           throw new NotImplementedException();
        }

        /// <summary>
        /// 搜寻Belt 并和记忆做比较，并修改记忆
        /// </summary>
        public override void MapBelt(bool AutoWork)
        {
            bool hasMaterial = this.SearchBelt();

            // 实际检查到有料 但记忆中没有料  或者  实际没检查到有料 但记忆中有料 
            if (
                (hasMaterial && this.unloadingSubSectionProgram.SubSectionState == SubSectionStateEnum.NoMaterial)
                || (!hasMaterial && this.unloadingSubSectionProgram.SubSectionState == SubSectionStateEnum.HasMaterial))
            {
                // 报警
                DialogResult dr = AKRSMessageBoxExt.Show(
                    @" The actual search results don't match the memory on unloading table, please confirm there is material on unloading table ?
                              Click
                              Yes:  Has Material
                              No: No Material",
                    $"Warning",
                    new[] { "Yes", "No" },
                    new[] { DialogResult.Yes, DialogResult.No });

                switch (dr)
                {
                    case DialogResult.Yes:
                        this.unloadingSubSectionProgram.SubSectionState = SubSectionStateEnum.HasMaterial;
                        if (this.unloadingSubSectionProgram.TransportUnit == null)
                        {
                            // todo : 要生成一个 Transport Unit 吗？
                        }

                        break;

                    case DialogResult.No:
                        this.unloadingSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;
                        this.unloadingSubSectionProgram.TransportUnit = null;
                        break;
                }
            }
        }

        /// <summary>
        /// 转动皮带不等待到位
        /// </summary>
        public override void MoveBeltNoWaitArrive()
        {
            // 按时间传送， 时间配置在参数里
            InOutPutBeltSetting inOutPutBeltSetting = this.unloadingSubSectionProgram.InOutPutBeltSetting;

            if (inOutPutBeltSetting == null)
            {
                throw new Exception("unload section belt setting is not exist");
            }

            double transportDistance = inOutPutBeltSetting.ForwardTransportDistance;

            // 皮带1相对运动指令
            this.unloadingSubSectionModule.SendRelativeMoveCommandBelt(transportDistance, inOutPutBeltSetting.ForwardVelRate);
        }

        /// <summary>
        /// 转动皮带等待到位
        /// </summary>
        public override void MoveBeltWaitArrive()
        {
            // 按时距离送
            InOutPutBeltSetting inOutPutBeltSetting = this.unloadingSubSectionProgram.InOutPutBeltSetting;

            if (inOutPutBeltSetting == null)
            {
                throw new Exception("unload section belt setting is not exist");
            }

            double transportDistance = inOutPutBeltSetting.ForwardTransportDistance;

            // 皮带1相对运动指令
            this.unloadingSubSectionModule.RelativeMoveBelt(transportDistance, inOutPutBeltSetting.ForwardVelRate);
        }

        /// <summary>
        /// 入料口是否有料
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool HasMaterialInFeedingInlet()
        {
            return this.unloadingSubSectionModule.HasMaterialInFeedingInlet();
        }

        /// <summary>
        /// 出料口是否有料
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool HasMaterialInOutlet()
        {
            return this.unloadingSubSectionModule.HasMaterialInOutlet();
        }

        /// <summary>
        /// 获取收板机是否收料成功
        /// </summary>
        /// <returns>结果</returns>
        public bool GetAutoUnLoaderNeedMaterialSignal()
        {
            return this.unloadingSubSectionModule.GetAutoUnLoaderNeedMaterialSignal();
        }
    }
}
