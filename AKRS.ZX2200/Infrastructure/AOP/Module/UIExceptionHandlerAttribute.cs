using AKRS.ZX2200.Infrastructure.Controls.Currency;
using DevExpress.Tutorials.Controls;
using DevExpress.XtraEditors;
using MethodBoundaryAspect.Fody.Attributes;
using System;
using System.Windows.Forms;

namespace AKRS.ZX2200.Infrastructure.AOP.Module;

/// <summary>
/// UI异常处理特性
/// </summary>
[Serializable]
[AttributeUsage(AttributeTargets.Method)]
public sealed class UIExceptionHandlerAttribute: OnMethodBoundaryAspect
{
    /// <summary>
    /// UI方法异常处理
    /// </summary>
    /// <param name="arg"></param>
    public override void OnException(MethodExecutionArgs arg)
    {
        // 标记异常已处理，防止抛出
        arg.FlowBehavior = FlowBehavior.Continue;

        // 弹出提示框
        AKRSXtraMessageBox.Show($"程序运行错误：{arg.Exception.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

    }
}