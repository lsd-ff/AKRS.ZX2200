using System;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Infrastructure.Utils;

/// <summary>
/// 异步帮助类
/// </summary>
public static class AsyncHelper
{
    /// <summary>
    /// 异步执行指定的动作，支持同步和异步方法。
    /// </summary>
    /// <param name="action">要执行的动作，可以是返回Task的异步方法或同步方法。</param>
    /// <param name="exceptionHandler">异常处理器。</param>
    /// <param name="parameters">参数</param>
    /// <returns>表示异步操作的任务。</returns>
    public static async Task ExecuteAsync(Delegate action, Action<Exception> exceptionHandler = null, params object[] parameters)
    {
        try
        {
            if (action is Func<Task> asyncFunc)
            {
                await ((Task)asyncFunc.DynamicInvoke()).ConfigureAwait(false);
            }
            else if (action is Func<object[], Task> asyncFuncWithParams)
            {
                await ((Task)asyncFuncWithParams.DynamicInvoke(parameters)).ConfigureAwait(false);
            }
            else
            {
                await Task.Run(() => { action.DynamicInvoke(parameters); }).ConfigureAwait(false);
            }
        }
        catch (Exception e)
        {
            exceptionHandler?.Invoke(e);
        }
    }
}