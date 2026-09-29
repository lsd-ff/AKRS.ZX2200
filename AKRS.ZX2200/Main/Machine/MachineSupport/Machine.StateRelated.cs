using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.MachineSupport;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.SupportFeature.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DevExpress.CodeParser.CodeStyle.Formatting.Rules.Spacing.Parentheses;

namespace AKRS.ZX2200.Main.Machine.MachineSupport
{
    /// <summary>
    /// 设备状态相关
    /// </summary>
    public partial class Machine
    {
        /// <summary>
        /// 是否在工作状态
        /// </summary>
        /// <returns>是：正在工作，否：不在自动工作</returns>
        public bool IsWorking()
        {
            return MachineStateModel.GetInstance().MachineState == MachineStateEnum.Working;
        }

        /// <summary>
        /// 是否在停止状态
        /// </summary>
        /// <returns>是：正在工作，否：不在自动工作</returns>
        public bool IsStop()
        {
            return MachineStateModel.GetInstance().MachineState == MachineStateEnum.Stop;
        }

        /// <summary>
        /// 是否在暂停状态
        /// </summary>
        /// <returns>是：正在工作，否：不在自动工作</returns>
        public bool IsPause()
        {
            return MachineStateModel.GetInstance().MachineState == MachineStateEnum.Pause;
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <returns>是：正在工作，否：不在自动工作</returns>
        public void SetState(MachineStateEnum machineState)
        {
            MachineStateEnum oldState = MachineStateModel.GetInstance().MachineState;

            if (oldState != machineState)
            {
                ParameterChangeLogEntity parameterChangeLogEntity = new ParameterChangeLogEntity(
            DateTime.Now,
             RuntimeProvider.CurrentLoginUser?.Name,
            "设备状态",
            $"设备状态 : 状态值由  {oldState.GetDescription()} ===>  {machineState.GetDescription()}");

                MachineStateModel.GetInstance().MachineState = machineState;
                StatisticsService.DbEntityBlockingCollection.Add(parameterChangeLogEntity);
            }

        }

    }
}
