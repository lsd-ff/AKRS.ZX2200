namespace AKRS.ZX2200.Infrastructure.EventBus;

/// <summary>
/// 应用平台
/// </summary>
public static class PlatformProvider
{
    /// <summary>
    /// 当前应用平台
    /// </summary>
    public static IPlatformProvider Current { get; set; } = new WinFormsPlatformProvider();
}