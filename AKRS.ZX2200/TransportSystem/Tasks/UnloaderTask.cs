using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.TransportSystem.Controllers;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.TransportSystem.Tasks
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using Newtonsoft.Json;

    /// <summary>
    /// 下料仓线程
    /// </summary>
    public class UnloaderTask
    {
        /// <summary>
        /// 下料仓控制器
        /// </summary>
        public UnLoaderBinController UnloaderBinController = new UnLoaderBinController();

        /// <summary>
        /// 下料仓线程
        /// </summary>
        public Task unloaderTask;

        /// <summary>
        /// 是否启用
        /// </summary>
        [JsonIgnore]
        public bool Enable { get; set; } = true;

        /// <summary>
        /// 流道线程启动
        /// </summary>
        public void Start()
        {
            if (!this.Enable)
            {
                return;
            }

            // 空跑模式
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DryCycle)
            {
                // 空跑不动作
                return;
            }

            // 防止线程多次启动
            if (this.unloaderTask == null || this.unloaderTask.Status != TaskStatus.Running)
            {
                this.unloaderTask = Task.Run(() =>
                {
                    CommonUtil.SetCurrentThreadName("下料仓下料线程");

                    bool firstStart = true;
                    try
                    {
                        // 正在工作
                        while (Machine.GetInstance().IsWorking())
                        {
                            // 暂停状态
                            if (Machine.GetInstance().IsPause())
                            {
                                Thread.Sleep(1000);
                                continue;
                            }

                            if (firstStart)
                            {
                                // 移动到当前料片
                                ExcuteResult ret = this.UnloaderBinController.MoveToCurrentPlaceLayer();

                                switch (ret)
                                {
                                    case ExcuteResult.Abort:

                                        Machine.GetInstance().Stop();
                                        return;

                                    case ExcuteResult.Success:
                                        break;
                                }

                                // 发送允许推料信号
                                SignalPool.GetInstance().UnloaderAllowPushSignal.Set();
                                firstStart = false;
                            }

                            // 等允许下料信号
                            if (!SignalPool.GetInstance().AllowUnloaderMoveSignal.Wait())
                            {
                                return;
                            }

                            // 移动到当前放置层
                            ExcuteResult ret2 = this.UnloaderBinController.MoveToCurrentPlaceLayer();

                            switch (ret2)
                            {
                                case ExcuteResult.Abort:

                                    Machine.GetInstance().Stop();
                                    return;

                                case ExcuteResult.Success:
                                    break;
                            }

                            // 发送允许推料信号
                            SignalPool.GetInstance().UnloaderAllowPushSignal.Set();
                        }
                    }
                    catch (Exception ex)
                    {
                        AKRSXtraMessageBox.Show($"下料仓下料线程异常：{ex.Message}");
                    }
                });

                this.unloaderTask.ConfigureAwait(false);
            }
        }
    }
}
