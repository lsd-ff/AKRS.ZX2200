namespace AKRS.ZX2200.Infrastructure.Interface
{
    public interface ITool
    {
        /// <summary>
        /// 是否能够正常工作
        /// </summary>
        bool CanWorkProperly();

        /// <summary>
        /// 工具的初始化方法
        /// </summary>
        void Init();
    }
}
