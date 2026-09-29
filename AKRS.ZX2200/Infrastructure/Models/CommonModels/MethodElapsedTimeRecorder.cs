namespace AKRS.ZX2200.Infrastructure.Models.CommonModels
{
    using System.Diagnostics;

    using AKRS.Galaxy2.Log;

    using log4net.Core;

    /// <summary>
    /// 方法耗时记录器
    /// </summary>
    public class MethodElapsedTimeRecorder
    {
        public static bool IsEnable { get; set; } = true; 
      
        private Stopwatch stopwatch = new Stopwatch();
        
        public MethodElapsedTimeRecorder() { }

        public void Reset() 
        {
            this.stopwatch.Reset();  
        }

        public void Restart()
        {
            this.stopwatch.Restart();
        }

        public void RecordTime(string item, string subItem)
        {
            if (IsEnable)
            {
                LogHelper.Post(Level.Info,
                    $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.fff")}: {item} - {subItem} 耗时：{this.stopwatch.ElapsedMilliseconds} ms"
                    , LogCategory.MainSoftWare);

                this.Restart(); 
            }
        }
    }
}
