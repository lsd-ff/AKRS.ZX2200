using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.Infrastructure.EventBus;

using AKRS.Galaxy2.Log;
using DevExpress.XtraEditors;
using log4net.Core;
using Action = System.Action;

/// <summary>
/// Winform 平台工具
/// </summary>
public class WinFormsPlatformProvider : IPlatformProvider
{
    /// <summary>
    /// 上下文
    /// </summary>
    private readonly SynchronizationContext context;

    /// <summary>
    /// 判断当前是否在设计模式
    /// </summary>
    public bool InDesignMode =>
        LicenseManager.UsageMode == LicenseUsageMode.Designtime || Process.GetCurrentProcess().ProcessName.Contains("devenv");

    /// <summary>
    /// WinForms中通常在UI线程更新属性
    /// </summary>
    public bool PropertyChangeNotificationsOnUIThread => true;

    /// <summary>
    /// 构造函数
    /// </summary>
    public WinFormsPlatformProvider()
    {
        this.context = SynchronizationContext.Current;
    }

    /// <inheritdoc />
    public void BeginOnUIThread(System.Action action)
    {
        if (action == null)
        {
            return;
        }

        if (this.context != null)
        {
            this.context.Post(
                _ =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception e)
                    {
                    }
                },
                null
            );
        }
    }


    /// <inheritdoc />
    public Task OnUIThreadAsync(Func<Task> action)
    {
        if (action == null)
        {
            return Task.CompletedTask;
        }

        return Task.Run(
            () =>
                this.context.Send(
                    _ =>
                    {
                        try
                        {
                            action();
                        }
                        catch (Exception e)
                        {
                           
                        }
                    },
                    null
                )
        );
    }

    /// <inheritdoc />
    public void OnUIThread(System.Action action)
    {
        if (action == null)
        {
            return;
        }

        if (this.context == null)
        {
            return;
        }

        this.context.Send(
            _ =>
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    //MessageBox.Show($"UI线程异常 \r\n {e.Message}", "异常提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogHelper.Post(Level.Info, $"UI线程异常 \r\n {e.Message}", LogCategory.Global);
                }
            },
            null
        );
    }

    /// <inheritdoc />
    public void OnUIThread(Delegate action, params object[] args)
    {
        if (action == null)
        {
            return;
        }

        this.context.Send(
            _ =>
            {
                try
                {
                    action?.DynamicInvoke(args);
                }
                catch (Exception e)
                {
                    throw new Exception($"UI线程异常 \r\n {e.Message}");
                }
            },
            null
        );
    }

    /// <inheritdoc />
    public Task OnUIThreadAsync(Delegate action, params object[] args)
    {
        if (action == null)
        {
            return Task.CompletedTask;
        }

        return Task.Run(() => this.context.Post(
            _ =>
            {
                try
                {
                    action?.DynamicInvoke(args);
                }
                catch (Exception e)
                {
                    throw new Exception($"UI线程异常 \r\n {e.Message}");
                }
            },
            null
        ));
    }

    /// <inheritdoc />
    public object GetFirstNonGeneratedView(object view)
    {
        return view;
    }

    /// <inheritdoc />
    public void ExecuteOnFirstLoad(object view, Action<object> handler)
    {
        if (view is Form form)
        {
            EventHandler onLoad = null;
            onLoad = (sender, args) =>
            {
                form.Load -= onLoad;
                handler(view);
            };
            form.Load += onLoad;
        }

        if (view is Control control)
        {
            EventHandler onHandleCreated = null;
            onHandleCreated = (sender, args) =>
            {
                control.HandleCreated -= onHandleCreated;
                handler(view);
            };
            control.HandleCreated += onHandleCreated;
        }
    }

    /// <inheritdoc />
    public void ExecuteOnLayoutUpdated(object view, Action<object> handler)
    {
        if (view is Control control)
        {
            EventHandler onSizeChanged = null;
            onSizeChanged = (sender, args) =>
            {
                control.SizeChanged -= onSizeChanged;
                handler(view);
            };
            control.SizeChanged += onSizeChanged;
        }
    }

    /// <inheritdoc />
    public Func<CancellationToken, Task> GetViewCloseAction(object viewModel, ICollection<object> views, bool? dialogResult)
    {
        return cancellationToken =>
        {
            foreach (object view in views)
            {
                if (view is Form form)
                {
                    if (dialogResult.HasValue)
                    {
                        form.DialogResult = dialogResult == true ? DialogResult.OK : DialogResult.Cancel;
                    }
                    else
                    {
                        form.Close();
                    }
                }
            }

            return Task.CompletedTask;
        };
    }
}