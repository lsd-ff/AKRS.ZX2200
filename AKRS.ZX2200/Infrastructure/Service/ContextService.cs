using AKRS.ZX2200.Infrastructure.Controls.Currency;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.Infrastructure.Service;

/// <summary>
/// 上下文服务
/// </summary>
[Obsolete("Use IPlatformProvider instead.")]
public class ContextService
{
    private SynchronizationContext mainContext;

    private static readonly Lazy<ContextService> lazyInstance = new(() => new());

    private ContextService()
    {
        this.mainContext = SynchronizationContext.Current;
    }

    /// <summary>
    /// 单例
    /// </summary>
    public static ContextService Instance => lazyInstance.Value;

    /// <summary>
    /// 在UI线程上执行
    /// </summary>
    /// <param name="action"></param>
    public static void OnUI(System.Action action)
    {
        if (Instance.mainContext == null)
        {
            return;
        }

        Instance.mainContext.Send(
            _ =>
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    AKRSXtraMessageBox.Show($"推送到UI线程异常：{e.Message}");
                }
            },
            null
        );
    }

    /// <summary>
    /// 在UI线程异步执行
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Task OnUIAsync(System.Action action)
    {
        if (Instance.mainContext == null)
        {
            return Task.CompletedTask;
        }

        return Task.Run(() => Instance.mainContext.Post(
            _ =>
            {
                try
                {
                    action?.DynamicInvoke();
                }
                catch (Exception e)
                {
                    throw new Exception($"UI线程异常 \r\n {e.Message}");
                }
            },
            null
        ));
    }
}