#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2024/3/25 13:17:22
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

using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportSystem.Models.Programs;
using AKRS.ZX2200.TransportSystem.Modules;

namespace AKRS.ZX2200.TransportSystem.Controllers
{
    /// <summary>
    /// 描述：载台控制器基类
    /// </summary>
    public abstract class BaseSubSectionController
    {
        /// <summary>
        /// 载台模组
        /// </summary>
        public BaseSubSectionModule SubSectionModule { get; set; }

        /// <summary>
        /// 程式
        /// </summary>
        public BaseSubSectionProgram SubSectionProgram { get; set; }

        /// <summary>
        /// 皮带停止
        /// </summary>
        public void StopMove()
        {
            this.SubSectionModule.StopMove();
        }

        /// <summary>
        /// 皮带是否在运动
        /// </summary>
        /// <returns>是否转动</returns>
        public bool IsMoving()
        {
            return this.SubSectionModule.IsMoving();
        }

        /// <summary>
        ///  搜索等待下料区是否有料
        /// </summary>
        /// <returns>
        /// true: 搜索到并传料成功，
        /// false：没搜索到或者搜索到了后传料失败
        /// 里面不涉及任何状态参数的修改
        /// </returns>
        public abstract bool SearchBelt();

        /// <summary>
        /// 搜寻Belt 并和记忆做比较，并修改记忆
        /// </summary>
        public abstract void MapBelt(bool AutoWork);

        /// <summary>
        /// 转动皮带不等待到位
        /// </summary>
        public abstract void MoveBeltNoWaitArrive();

        /// <summary>
        /// 转动皮带等待到位
        /// </summary>
        public abstract void MoveBeltWaitArrive();

        /// <summary>
        /// 载台是否有料
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool HasMaterialInModule()
        {
            return this.SubSectionModule.HasMaterialInModule();
        }
    }
}
