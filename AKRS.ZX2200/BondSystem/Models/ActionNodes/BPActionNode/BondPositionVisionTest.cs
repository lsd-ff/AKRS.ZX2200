using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using log4net.Core;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.BPActionNode
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.TransportUnitSystem.Model;

    /// <summary>
    /// 这个是验证精度的测试类
    /// 主要是代替焊头
    /// 用相机去看需要贴片的位置
    /// 如果相机看到焊点的精度足够，则考虑是否是贴片的问题
    /// </summary>
    public class BondPositionVisionTest : ActionNode
    {
        /// <summary>
        /// 执行动作
        /// </summary>
        /// <returns>结果</returns>
        public override ExcuteResult DoWork()
        {
            try
            {
                // 获取当前焊点
                BondPosition bondPosition = System2Domain.GetInstance().ActionNodesService.GetCurrentBondPosition();

                // 获取当前焊点的位置
                AKRSPoint3D point3D = bondPosition.CoordinateSystem.SelfPosToG0(new AKRSPoint3D());

                // 定位
                MatchResult matchResult1 =
                    (MatchResult)System2Domain.GetInstance().System2Controller.BondCameraVision(point3D, "1");

                if (matchResult1 == null)
                {
                    return ExcuteResult.Abort;
                }

                // 记录定位结果

                return ExcuteResult.Success;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Info, $"流程{this.Name}运行故障", ex, LogCategory.Bond);
                AKRSMessageBoxExt.Show(ex.Message, "Exception", new string[] { "Exception" }, new DialogResult[] { DialogResult.Yes });
                return ExcuteResult.Exception;
            }
            finally
            {
                this.State = RunStateEnum.Stop;
                this.WorkStop?.Invoke();
            }
        }

        /// <summary>
        /// 完成动作的条件是否满足
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsDoWork()
        {
            // 焊点
            BondPosition bp = System2Domain.GetInstance().ActionNodesService.GetCurrentBondPosition();

            // 当前焊点如果成功了或者失败了都不会贴片
            if (bp.EntityState != EntityState.Process)
            {
                return false;
            }

            // 判断当前工艺制程是否开启
            if (bp.MatterProductState == MatterProductState.Disable
                || bp.MatterProductState == MatterProductState.EnableInSystem1)
            {
                return false;
            }

            //// 已经贴过片则退出
            //if (bp.BondPositionInfo.IsBondCompleted)
            //{
            //    return false;
            //}

            // 点胶没有完成不点胶
            if (!bp.IsFinishedInSystem1)
            {
                return false;
            }

            return true;
        }
    }
}
