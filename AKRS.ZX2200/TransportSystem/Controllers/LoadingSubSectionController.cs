#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2024/3/23 18:03:00
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
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.InOutPut;
using AKRS.ZX2200.TransportSystem.Models.Programs;
using AKRS.ZX2200.TransportSystem.Modules;

namespace AKRS.ZX2200.TransportSystem.Controllers
{
    /// <summary>
    /// 描述：上料载台控制器
    /// </summary>
    public class LoadingSubSectionController : BaseSubSectionController
    {
        /// <summary>
        /// 上料载台模组
        /// </summary>
        private LoadingSubSectionModule loadingSubSectionModule => TransportModule.GetInstance().LoadingSubSectionModule;

        /// <summary>
        /// 上料程式
        /// </summary>
        private LoadingSubSectionProgram loadingSubSectionProgram => TransportProgram.GetInstance().LoadingSubSectionProgram;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LoadingSubSectionController()
        {
            this.SubSectionModule = TransportModule.GetInstance().LoadingSubSectionModule;
            this.SubSectionProgram = TransportProgram.GetInstance().LoadingSubSectionProgram;
        }

        /// <summary>
        ///  搜索等待下料区是否有料
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// 转动皮带不等待到位
        /// </summary>
        public override void MoveBeltNoWaitArrive()
        {
            // 按时间传送， 时间配置在参数里
            InOutPutBeltSetting inOutPutBeltSetting = this.loadingSubSectionProgram.InOutPutBeltSetting;

            if (inOutPutBeltSetting == null)
            {
                throw new Exception("进料/出料数据集不存在");
            }

            double transportDistance = inOutPutBeltSetting.ForwardTransportDistance;

            // 皮带1相对运动指令
            this.loadingSubSectionModule.SendRelativeMoveCommandBelt(transportDistance, inOutPutBeltSetting.ForwardVelRate);
        }

        /// <summary>
        /// 转动皮带等待到位
        /// </summary>
        public override void MoveBeltWaitArrive()
        {
            // 按时间传送， 时间配置在参数里
            InOutPutBeltSetting inOutPutBeltSetting = this.loadingSubSectionProgram.InOutPutBeltSetting;

            if (inOutPutBeltSetting == null)
            {
                throw new Exception("进料/出料数据集不存在");
            }

            double transportDistance = inOutPutBeltSetting.ForwardTransportDistance;

            // 皮带1相对运动指令
            this.loadingSubSectionModule.RelativeMoveBelt(transportDistance, inOutPutBeltSetting.ForwardVelRate);
        }

        /// <summary>
        /// 设置需料信号
        /// </summary>
        /// <param name="isNeedTablet">是否要料</param>
        public void SetLoadingTableNeedTabletSignal(bool isNeedTablet)
        {
            this.loadingSubSectionModule.SetLoadingTableNeedTabletSignal(isNeedTablet);
        }

        /// <summary>
        /// 前段是否有料
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool HasMaterialOnFront()
        {
            return this.loadingSubSectionModule.HasMaterialOnFront();
        }
    }
}
