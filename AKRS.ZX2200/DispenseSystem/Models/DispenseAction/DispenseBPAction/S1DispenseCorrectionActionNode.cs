using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.ZX2200.Models;
using log4net.Core;
using Newtonsoft.Json;
using System;
using System.Windows.Forms;
using AKRS.ZX2200.DispenseSystem.Modules;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.DispenseSystem.Services;

namespace AKRS.ZX2200.DispenseSystem.Models.DispenseAction.DispenseBPAction
{
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Process;
    using AKRS.ZX2200.Main.Machine.Product;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Setting;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    /// <summary>
    /// 点胶拍照动作节点
    /// </summary>
    public class S1DispenseCorrectionActionNode : ActionNode
    {
        /// <summary>
        /// 系统1动作控制器
        /// </summary>
        private S1ActionNodeController ActionNodeController => System1Domain.GetInstance().ActionNodeController;

        /// <summary>
        /// 当前焊点
        /// </summary>
        private BondPosition BondPosition => System1Domain.GetInstance().ActionNodeController.CurrentBondPosition;

        /// <summary>
        /// 拍照动作运行
        /// </summary>
        /// <returns>结果</returns>
        public override ExcuteResult DoWork()
        {
            this.WorkStart?.Invoke();
            this.State = RunStateEnum.Running;
            try
            {
                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    // 修改当前焊点的状态为点胶完成
                    this.BondPosition.BondPositionInfo.IsVisioned = true;
                }

                if (!ProductDomain.GetInstance().ProductConfig.TransportUnitConfig.IsOppositeSex)
                {
                    // 如果在相对固晶的情况下则点胶区不会定位
                    if (this.BondPosition.SingleBondPositionConfig.RelativeBondPosition)
                    {
                        return ExcuteResult.Success;
                    }
                }

                DispenseRunTimeProvider.RecordTime($"系统1焊点定位", $"焊点信息 名称:{this.BondPosition.Name},基板号{this.BondPosition.SubstrateNum},基岛号{this.BondPosition.ModuleNum}");

                DispenseRunTimeProvider.RecordTime("系统1焊点定位", $"系统1焊点定位开始");

                System1Domain.GetInstance().System1MatterVision(this.BondPosition);

                DispenseRunTimeProvider.RecordTime("系统1焊点定位", $"系统1焊点定位结束");


                return ExcuteResult.Success;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程{this.Name}运行故障！", ex, LogCategory.Dispense);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
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
            BondPosition bp = System1Domain.GetInstance().ActionNodeController.CurrentBondPosition;

            if (bp == null)
            {
                throw new Exception("获取当前焊点失败！请检查当前焊点是否已经被删除或者工作步骤配置有问题。\r\n 请清空载具后重试");
            }

            // 焊点点位是否开启
            if (bp.Config.LocateConfig.AdjustType == AdjustTypeEnum.None)
            {
                return false;
            }
            
            // 判断当前工艺制程是否开启
            if (bp.MatterProductState == MatterProductState.Disable
                || bp.MatterProductState == MatterProductState.EnableInSystem2)
            {
                return false;
            }

            // 定位过则不再定位
            if (bp.BaseInfo.IsVisioned)
            {
                return false;
            }

            // 没有工作步骤
            if (bp.IsFinishedInSystem1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 是否报警
        /// 这个一般是程序中出现空指针的时候会采用这个方法
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsAlarm()
        {
            // 获取当前的carrier对象
            BondPosition bondPosition =
                System1Domain.GetInstance().ActionNodeController.CurrentBondPosition;

            if (bondPosition == null)
            {
                AKRSMessageBoxExt.Show(
                    $"Don not exist bondPosition , Please check TransportUnit Program\r\n" + $"{this.Name}",
                    "Alarm",
                    new string[] { "Yes" },
                    new DialogResult[] { DialogResult.Abort },
                    AlarmLevel.SecondLevel);
                return false;
            }

            return true;
        }
    }
}
