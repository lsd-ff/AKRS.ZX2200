using System;
using MethodBoundaryAspect.Fody.Attributes;

namespace AKRS.ZX2200.Infrastructure.AOP.Module
{
    using System.Windows.Forms;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.MachineSupport;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.SupportFeature.Statistics;
    using DevExpress.XtraBars;
    using DevExpress.XtraEditors;
    using DevExpress.XtraTreeList;

    /// <summary>
    /// 方法上的注解
    /// </summary>
    public class MethodAopAttribute : OnMethodBoundaryAspect
    {
     
        /// <summary>
        /// 当进入时发生
        /// </summary>
        /// <param name="arg">方法信息</param>
        public override void OnEntry(MethodExecutionArgs arg)
        {
            if (arg.Arguments.Length != 2)
            {
               return;
            }

            // BarButton按钮事件
            if (arg.Arguments[1] is ItemClickEventArgs itemClickEventArgs
                && itemClickEventArgs.Item is BarButtonItem barButtonItem)
            {
                RoleEnum role = barButtonItem.Tag == null ? RoleEnum.Engineer : (RoleEnum)(int)barButtonItem.Tag;

                RoleEnum currentRole = (RoleEnum)Machine.GetInstance().CurrentRoleLevel;

                // 判断权限
                if (role.CompareTo(currentRole) < 0 && MachineSoftwareConfiguration.GetInstance().IsPermissionOpen)
                {
                    AKRSMessageBoxExt.ShowWarn(
                        $"权限不足，无法执行!当前用户为:{ObjectHelper.GetEnumDescription(currentRole)}，需要权限为:{ObjectHelper.GetEnumDescription(role)}\r\n",
                        "权限不足",
                        new string[] { "确定" },
                        new DialogResult[] { DialogResult.Yes },
                        AlarmLevel.SecondLevel);
                    arg.FlowBehavior = FlowBehavior.Return;
                    return;
                }

                StatisticsService.SaveEntity(
                    $"点击了主界面按钮：{barButtonItem.Caption}",
                    RuntimeProvider.CurrentLoginUser?.Name);
            }
            else if (arg.Arguments[1] is FocusedNodeChangedEventArgs focusedNodeChangedEventArgs)
            {
                // 编程选项事件
                string tagStr = focusedNodeChangedEventArgs?.Node?.GetDisplayText("Name");

                StatisticsService.SaveEntity(
                    $"点击了 {tagStr} 选项",
                    RuntimeProvider.CurrentLoginUser?.Name);
            }
            else if (arg.Arguments[1] is EventArgs eventArgs 
                     && arg.Arguments[0] is ImageListBoxControl imageListBoxControl)
            {
                // 编程选项事件
                if (imageListBoxControl.SelectedItem == null)
                {
                    return;
                }

                string tagStr = imageListBoxControl.SelectedItem?.ToString();

                StatisticsService.SaveEntity(
                    $"点击了 {tagStr} 选项",
                     RuntimeProvider.CurrentLoginUser?.Name);
            }
            else if (arg.Arguments[1] is EventArgs eventArgsClick
                     && arg.Arguments[0] is SimpleButton simpleButton)
            {
                // 按钮事件
                StatisticsService.SaveEntity(
                    $"点击了 {simpleButton.Text} 按钮",
                     RuntimeProvider.CurrentLoginUser?.Name);
            }
        }

        /// <summary>
        /// 当退出时发生
        /// </summary>
        /// <param name="arg">方法信息</param>
        public override void OnExit(MethodExecutionArgs arg)
        {
        }

        /// <summary>
        /// 当异常时发生
        /// </summary>
        /// <param name="arg">方法信息</param>
        public override void OnException(MethodExecutionArgs arg)
        {
            StatisticsService.SaveEntity(
                $"界面操作异常发生",
                 RuntimeProvider.CurrentLoginUser?.Name);
        }
    }
}
