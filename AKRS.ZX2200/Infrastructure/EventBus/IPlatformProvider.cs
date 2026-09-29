using System.Collections.Generic;
using System.Threading;
using System;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Infrastructure.EventBus;

/// <summary>
/// 平台特定操作的接口
/// </summary>
public interface IPlatformProvider
{
    /// <summary>
    ///   指示框架是否处于设计模式。
    /// </summary>
    bool InDesignMode { get; }

    /// <summary>
    /// 是否应在UI线程上执行属性更改通知。
    /// </summary>
    bool PropertyChangeNotificationsOnUIThread { get; }

    /// <summary>
    ///   异步在UI线程上执行操作。
    /// </summary>
    /// <param name="action">要执行的操作。</param>
    void BeginOnUIThread(System.Action action);

    /// <summary>
    ///   异步在UI线程上执行操作。
    /// </summary>
    /// <param name = "action">要执行的操作。</param>
    Task OnUIThreadAsync(Func<Task> action);

    /// <summary>
    ///   在UI线程上执行操作。
    /// </summary>
    /// <param name = "action">要执行的操作。</param>
    void OnUIThread(System.Action action);

    /// <summary>
    /// 在UI线程上执行操作。
    /// </summary>
    /// <param name="action"></param>
    void OnUIThread(Delegate action, params object[] args);

    /// <summary>
    /// 异步在UI线程上执行操作。
    /// </summary>
    /// <param name="action"></param>
    Task OnUIThreadAsync(Delegate action, params object[] args);

    /// <summary>
    /// 用于检索根非框架创建的视图。
    /// </summary>
    /// <param name="view">要搜索的视图。</param>
    /// <returns>未由框架创建的根元素。</returns>
    /// <remarks>在某些情况下，服务会创建UI元素。
    /// 例如，如果您要求窗口管理器将UserControl显示为对话框，它会创建一个窗口来承载UserControl。
    /// 窗口管理器将该元素标记为框架创建的元素，以便它可以确定它创建的内容与开发人员预期的内容。
    /// 调用GetFirstNonGeneratedView允许框架发现原始元素是什么。
    /// </remarks>
    object GetFirstNonGeneratedView(object view);

    /// <summary>
    /// 在视图首次加载时执行处理程序。
    /// </summary>
    /// <param name="view">视图。</param>
    /// <param name="handler">处理程序。</param>
    void ExecuteOnFirstLoad(object view, Action<object> handler);

    /// <summary>
    /// 在视图的LayoutUpdated事件下次触发时执行处理程序。
    /// </summary>
    /// <param name="view">视图。</param>
    /// <param name="handler">处理程序。</param>
    void ExecuteOnLayoutUpdated(object view, Action<object> handler);

    /// <summary>
    /// 获取指定视图模型的关闭操作。
    /// </summary>
    /// <param name="viewModel">要关闭的视图模型。</param>
    /// <param name="views">关联的视图。</param>
    /// <param name="dialogResult">对话框结果。</param>
    /// <returns>关闭视图模型的<see cref="Func{T, TResult}"/>。</returns>
    Func<CancellationToken, Task> GetViewCloseAction(object viewModel, ICollection<object> views, bool? dialogResult);
}
