using AKRS.ZX2200.Infrastructure.EventBus;
using AKRS.ZX2200.Infrastructure.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraCharts.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.Infrastructure.Service;

using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Main.Controls;

public static class ControlService
{
    #region 控件执行封装

    /// <summary>
    /// 屏蔽控件执行异步操作
    /// </summary>
    /// <param name="controlObject">控件事件触发源</param>
    /// <param name="action">动作</param>
    /// <param name="controls">关联控件</param>
    public static async Task UIActionAsync(
        this object controlObject,
        Delegate action,
        List<Control> controls = null, params object[] objects)
    {
        if (controlObject is Control control)
        {
            await OnControlRunAsync(control, action, controls, objects).ConfigureAwait(false);
        }
        else if (controlObject is BarItem item)
        {
            await OnBarItemRunAsync(item, action, controls, objects).ConfigureAwait(false);
        }
    }


    #region 异步重载

    /// <summary>
    /// 屏蔽控件执行异步操作
    /// </summary>
    /// <param name="controlObject">控件事件触发源</param>
    /// <param name="action">动作</param>
    /// <param name="controls">关联控件</param>
    public static void UIActionAsync<T>(this object controlObject,
        Func<T, Task> action,
        List<Control> controls = null, T param = default)
    {
        controlObject.UIActionAsync((Delegate)action, controls, param);
    }

    public static void UIActionAsync<T1, T2>(this object controlObject,
        Func<T1, T2, Task> action,
        List<Control> controls = null, T1 param1 = default, T2 param2 = default)
    {
        controlObject.UIActionAsync((Delegate)action, controls, param1, param2);
    }

    public static void UIActionAsync<T1, T2, T3>(this object controlObject,
        Func<T1, T2, T3, Task> action,
        List<Control> controls = null, T1 param1 = default, T2 param2 = default, T3 param3 = default)
    {
        controlObject.UIActionAsync((Delegate)action, controls, param1, param2, param3);
    }

    public static void UIActionAsync<T1, T2, T3, T4>(this object controlObject,
        Func<T1, T2, T3, T4, Task> action,
        List<Control> controls = null, T1 param1 = default, T2 param2 = default, T3 param3 = default,
        T4 param4 = default)
    {
        controlObject.UIActionAsync((Delegate)action, controls, param1, param2, param3, param4);
    }

    public static async void UIActionAsync<T1, T2, T3, T4, T5>(this object controlObject,
        Func<T1, T2, T3, T4, T5, Task> action,
        List<Control> controls = null, T1 param1 = default, T2 param2 = default, T3 param3 = default,
        T4 param4 = default, T5 param5 = default)
    {
        controlObject.UIActionAsync((Delegate)action, controls, param1, param2, param3, param4, param5);
    }

    #endregion


    #region void 重载

    /// <summary>
    /// 屏蔽控件执行异步操作
    /// </summary>
    /// <param name="controlObject">控件事件触发源</param>
    /// <param name="action">动作</param>
    /// <param name="controls">关联控件</param>
    public static void UIActionAsync<T>(this object controlObject,
        Action<T> action,
        List<Control> controls = null, T param = default)
    {
        controlObject.UIActionAsync((Delegate)action, controls, param);
    }

    public static void UIActionAsync<T1, T2>(this object controlObject,
        Action<T1, T2> action,
        List<Control> controls = null, T1 param1 = default, T2 param2 = default)
    {
        controlObject.UIActionAsync((Delegate)action, controls, param1, param2);
    }

    public static void UIActionAsync<T1, T2, T3>(this object controlObject,
        Action<T1, T2, T3> action,
        List<Control> controls = null, T1 param1 = default, T2 param2 = default, T3 param3 = default)
    {
        controlObject.UIActionAsync((Delegate)action, controls, param1, param2, param3);
    }

    public static void UIActionAsync<T1, T2, T3, T4>(this object controlObject,
        Action<T1, T2, T3, T4> action,
        List<Control> controls = null, T1 param1 = default, T2 param2 = default, T3 param3 = default,
        T4 param4 = default)
    {
        controlObject.UIActionAsync((Delegate)action, controls, param1, param2, param3, param4);
    }

    public static void UIActionAsync<T1, T2, T3, T4, T5>(this object controlObject,
        Action<T1, T2, T3, T4, T5> action,
        List<Control> controls = null, T1 param1 = default, T2 param2 = default, T3 param3 = default,
        T4 param4 = default, T5 param5 = default)
    {
        controlObject.UIActionAsync((Delegate)action, controls, param1, param2, param3, param4, param5)
            ;
    }

    #endregion


    /// <summary>
    /// 屏蔽控件执行异步操作
    /// </summary>
    /// <param name="controlObject"></param>
    /// <param name="action"></param>
    /// <param name="controls"></param>
    /// <param name="objects"></param>
    /// <returns></returns>
    public static void UIActionAsync(this object controlObject, Func<Task> action,
        List<Control> controls = null, params object[] objects)
    {
        controlObject.UIActionAsync((Delegate)action, controls, objects);
    }


    /// <summary>
    /// 屏蔽控件执行异步操作
    /// </summary>
    /// <param name="controlObject"></param>
    /// <param name="action"></param>
    /// <param name="controls"></param>
    /// <param name="objects"></param>
    /// <returns></returns>
    public static void UIActionAsync(this object controlObject, System.Action action,
        List<Control> controls = null, params object[] objects)
    {
        controlObject.UIActionAsync((Delegate)action, controls, objects);
    }

    private static async Task OnBarItemRunAsync(BarItem item, Delegate action, List<Control> controls = null,
        params object[] objects)
    {
        if (signal.CurrentCount == 0)
        {
            AKRSXtraMessageBox.Show("当前正在执行其他动作！", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            await signal.WaitAsync().ConfigureAwait(false);

            ToggleControlsEnabled(item, controls, false);

            string actionInfo = $"{action?.Method.Name}--{action?.Method.DeclaringType?.FullName}";

            try
            {
                await Task.Run(async () =>
                {
                    await AsyncHelper.ExecuteAsync(action, ex => { HandleException(ex, actionInfo); }, objects)
                        .ConfigureAwait(false);
                }).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleException(e, actionInfo);
            }

            ToggleControlsEnabled(item, controls, true);
        }
        catch (Exception e)
        {
        }
        finally
        {
            ToggleControlsEnabled(item, controls, true);
            signal.Release();
        }
    }

    private static SemaphoreSlim signal = new(1, 1);

    private static void ToggleControlsEnabled(Control mainControl, List<Control> additionalControls, bool enabled)
    {
        if (mainControl.InvokeRequired)
        {
            mainControl.BeginInvoke(ToggleAction);
        }
        else
        {
            ToggleAction();
        }

        // PlatformProvider.Current.OnUIThread(action);
        return;

        void ToggleAction()
        {
            mainControl.Enabled = enabled;
            additionalControls?.ForEach(it => it.Enabled = enabled);
        }
    }

    private static void ToggleControlsEnabled(BarItem mainControl, List<Control> additionalControls, bool enabled)
    {
        MainForm.SendUiAction((System.Action)ToggleAction);
        return;

        void ToggleAction()
        {
            mainControl.Enabled = enabled;
            additionalControls?.ForEach(it => it.Enabled = enabled);
        }
    }

    private static async Task OnControlRunAsync(
        Control control,
        Delegate action,
        List<Control> controls = null,
        params object[] objects)
    {
        if (signal.CurrentCount == 0)
        {
            AKRSXtraMessageBox.Show("当前正在执行其他动作！", "后台任务执行提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            await signal.WaitAsync().ConfigureAwait(false);
            Control parent = control.Parent;
            controls?.ForEach(it => parent = FindNearestCommonParent(parent, it.Parent));
            parent?.SuspendLayout();

            ToggleControlsEnabled(control, controls, false);

            string actionInfo = $"{action?.Method.Name}--{action?.Method.DeclaringType?.FullName}";

            try
            {
                await Task.Run(async () =>
                {
                    await AsyncHelper.ExecuteAsync(action, ex => { HandleException(ex, actionInfo); }, objects)
                        .ConfigureAwait(false);
                }).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleException(e, actionInfo);
            }

            ToggleControlsEnabled(control, controls, true);

            parent?.ResumeLayout(false);
        }
        catch (Exception e)
        {
        }
        finally
        {
            signal.Release();
        }
    }

    private static void HandleException(Exception e, string actionInfo)
    {
        StringBuilder sb = new();
        sb.Append(e.Message);
        sb.Append("\t");
        sb = ConnectInnerExceptionMessage(sb, e);

        MainForm.SendUiAction(() =>
        {
            AKRSXtraMessageBox.Show(sb.ToString(), "后台任务执行异常提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        });
    }

    /// <summary>
    /// 内部异常信息拼接
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    private static StringBuilder ConnectInnerExceptionMessage(StringBuilder sb, Exception exception)
    {
        while (true)
        {
            if (exception.InnerException == null)
            {
                return sb;
            }

            sb.Append(exception.InnerException.Message);
            sb.Append("\r\n");
            exception = exception.InnerException;
        }
    }

    /// <summary>
    /// 找到最近的公共父控件
    /// </summary>
    /// <param name="control1"></param>
    /// <param name="control2"></param>
    /// <returns></returns>
    private static Control FindNearestCommonParent(Control control1, Control control2)
    {
        List<Control> parents1 = GetParents(control1).ToList();
        List<Control> parents2 = GetParents(control2).ToList();
        return parents1.Intersect(parents2).FirstOrDefault();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="control"></param>
    /// <returns></returns>
    private static IEnumerable<Control> GetParents(Control control)
    {
        while (control != null)
        {
            yield return control;
            control = control.Parent;
        }
    }

    #endregion

    /// <summary>
    /// 模态显示一个窗体，并返回执行结果
    /// </summary>
    /// <typeparam name="T">窗体类型</typeparam>
    /// <param name="args">构造函数的参数</param>
    /// <returns>窗体返回的结果</returns>
    public static DialogResult ShowDialogForm<T>(params object[] args)
        where T : Form
    {
        lock (AKRSMessageBoxExt.UiLock)
        {
            DialogResult result = DialogResult.Abort;
            MainForm.SendUiAction(
                () =>
                    {
                        T form = (T)Activator.CreateInstance(typeof(T), args);
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog();
                        form.Dispose();
                        result = form.DialogResult;
                    });

            return result;
        }
    }
}