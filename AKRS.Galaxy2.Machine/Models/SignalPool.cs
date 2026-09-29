using System.Threading;

namespace AKRS.Galaxy2.Machine.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// 信号池
    /// </summary>
    public class SignalPool
    {
        /// <summary>
        /// 设备底层信号池
        /// </summary>
        private static SignalPool instance;

        /// <summary>
        /// 锁
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// 集合
        /// </summary>
        public List<Signal> SignalList = new List<Signal>();

        /// <summary>
        /// 私有化
        /// </summary>
        private SignalPool()
        {
           this.Init();
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>单例</returns>
        public static SignalPool GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new SignalPool();
                    }
                }
            }

            return instance;
        }

        #region  公共

        /// <summary>
        /// 系统2单步信号
        /// </summary>
        public Signal System2SingleStepSignal { get; set; } = new Signal("系统2单步运行信号", 100, EventResetMode.AutoReset, false);

        /// <summary>
        /// 系统1单步信号
        /// </summary>
        public Signal System1SingleStepSignal { get; set; } = new Signal("系统1单步运行信号", 100, EventResetMode.AutoReset, false);

        #endregion

        #region 基板流道信号

        /// <summary>
        /// 允许上料仓移动信号
        /// </summary>
        public Signal AllowLoaderMoveSignal { get; set; } = new Signal("允许上料信号", 100, EventResetMode.AutoReset, false);

        /// <summary>
        /// 料仓允许推料信号
        /// </summary>
        public Signal LoaderAllowPushSignal { get; set; } = new Signal("上料仓允许推料信号", 100, EventResetMode.ManualReset, false);

        /// <summary>
        /// 点胶1点完，允许点胶1段传送到点胶2段
        /// </summary>
        public Signal AllowDispense1TransferToDispense2Signal { get; set; } = new Signal("允许点胶1段传送到点胶2段信号", 100, EventResetMode.AutoReset, false);

        /// <summary>
        /// 点胶做完,允许点胶区传送到Bond区
        /// </summary>
        public Signal AllowDispenseTransferToBondSignal { get; set; } = new Signal("允许点胶区传送到Bond区信号", 100, EventResetMode.AutoReset, false);

        /// <summary>
        /// Bond做完,允许Bond区传送到WaitingUnload区
        /// </summary>
        public Signal AllowBondTransferToWaitingUnloadSignal { get; set; } = new Signal("允许Bond区传送到下料等待区信号", 100, EventResetMode.AutoReset, false);

        /// <summary>
        /// 允许等待下料区传送到出料等待区
        /// </summary>
        public Signal AllowWaitingUnloadTransferToOutloadSignal { get; set; } = new Signal("允许等待下料区传送到出料等待区信号", 100, EventResetMode.AutoReset, false);

        /// <summary>
        /// 传送到点胶1完成信号
        /// </summary>
        public Signal TransferToDispense1FinishedSignal { get; set; } = new Signal("传输到点胶1完成信号", 100, EventResetMode.AutoReset, false);

        /// <summary>
        /// 传送到点胶2完成信号
        /// </summary>
        public Signal TransferToDispense2FinishedSignal { get; set; } = new Signal("传输到点胶2完成信号", 100, EventResetMode.AutoReset, false);

        /// <summary>
        /// 传送到Bond完成信号
        /// </summary>
        public Signal TransferToBondFinishedSignal { get; set; } = new Signal("传输到Bond完成信号", 100, EventResetMode.AutoReset, false);

        /// <summary>
        /// 允许下料仓移动信号
        /// </summary>
        public Signal AllowUnloaderMoveSignal { get; set; } = new Signal("允许下料信号", 100, EventResetMode.AutoReset, false);

        /// <summary>
        /// 下料仓允许推料信号，手动复位
        /// </summary>
        public Signal UnloaderAllowPushSignal { get; set; } = new Signal("下料仓允许推料信号", 100, EventResetMode.ManualReset, false);
        #endregion

        #region 点胶域

        #endregion

        #region 上晶圆域

        /// <summary>
        /// Bond是否取料成功信号
        /// </summary>
        public Signal IsBondPickSucceedSignal { get; set; } = new Signal("Bond是否取料成功信号", 100, EventResetMode.AutoReset, false);

        /// <summary>
        /// 晶圆允许取料信号
        /// todo:改超时时间
        /// </summary>
        public Signal IsWaferAllowPickSignal { get; set; } = new Signal("晶圆允许取料信号", 100, EventResetMode.AutoReset, false);

        /// <summary>
        /// Bond需料信号,手动复位
        /// </summary>
        public Signal IsBondNeedChipSignal { get; set; } = new Signal("Bond需料信号", 100, EventResetMode.ManualReset, false);

        /// <summary>
        /// Bond需顶针动作信号
        /// </summary>
        public Signal IsBondNeedEjectionSignal { get; set; } = new Signal("Bond需顶针动作信号", 100, EventResetMode.AutoReset, false);

        /// <summary>
        /// 顶针顶起完成信号
        /// </summary>
        public Signal IsEjectionComplete { get; set; } = new Signal("顶针顶起完成信号", 100, EventResetMode.AutoReset, false);

        /// <summary>
        /// 晶圆图加载完成信号
        /// </summary>
        public Signal IsLoadWaferMap { get; set; } = new Signal("晶圆图加载完成信号", 100, EventResetMode.AutoReset, false);

        /// <summary>
        /// 翻转台允许取料信号
        /// </summary>
        public Signal IsFlipTableAllowPickSignal { get; set; } = new Signal("翻转台允许取料信号", 100, EventResetMode.AutoReset, false);

        #endregion

        #region 固晶域

        /// <summary>
        /// 允许Bond蘸胶信号
        /// </summary>
        public Signal IsAllowDipFluxSignal { get; set; } = new Signal("允许Bond蘸胶信号", 100, EventResetMode.AutoReset, false);

        /// <summary>
        /// Bond蘸胶完成信号
        /// </summary>
        public Signal BondDipFluxFinishSignal { get; set; } = new Signal("Bond蘸胶完成信号", 100, EventResetMode.AutoReset, false);

        /// <summary>
        /// 允许刮胶盘伸出信号
        /// </summary>
        public Signal AllowSlideOutSignal { get; set; } = new Signal("允许刮胶盘伸出信号", 100, EventResetMode.AutoReset, false);

        #endregion

        /// <summary>
        ///  初始化
        /// </summary>
        private void Init()
        {
            this.SignalList.Clear();
            this.SignalList.Add(this.AllowDispense1TransferToDispense2Signal);

            this.SignalList.Add(this.AllowDispenseTransferToBondSignal);

            this.SignalList.Add(this.AllowBondTransferToWaitingUnloadSignal);

            this.SignalList.Add(this.AllowWaitingUnloadTransferToOutloadSignal); 

            this.SignalList.Add(this.TransferToDispense1FinishedSignal);

            this.SignalList.Add(this.TransferToDispense2FinishedSignal); 
            this.SignalList.Add(this.TransferToBondFinishedSignal);
            this.SignalList.Add(this.IsBondPickSucceedSignal);
            this.SignalList.Add(this.IsWaferAllowPickSignal);
            this.SignalList.Add(this.IsBondNeedChipSignal);
            this.SignalList.Add(this.IsBondNeedEjectionSignal);

            this.SignalList.Add(this.IsLoadWaferMap);
        }
    }
}
