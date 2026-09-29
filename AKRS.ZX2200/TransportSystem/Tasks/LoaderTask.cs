using AKRS.Galaxy2.Component.Simple.MessageBox;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.TransportSystem.Controllers;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportSystem.Models.Enums;
using AKRS.ZX2200.TransportSystem.Models.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.TransportSystem.Tasks
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using Newtonsoft.Json;

    /// <summary>
    /// 上料仓线程
    /// </summary>
    public class LoaderTask
    {
        /// <summary>
        /// 上料仓控制器
        /// </summary>
        private LoaderBinController loaderBinController = new LoaderBinController();

        /// <summary>
        /// 上料仓线程
        /// </summary>
        public Task loaderTask;

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
            if (this.loaderTask == null || this.loaderTask.Status != TaskStatus.Running)
            {
                this.loaderTask = Task.Run(() =>
                {
                    CommonUtil.SetCurrentThreadName("上料仓上料线程");

                    bool firstStart = true;
                    try
                    {
                        // 正在工作
                        while (Machine.GetInstance().IsWorking())
                        {
                            if (firstStart)
                            {
                                // 移动到当前料片
                                ExcuteResult ret = this.loaderBinController.MoveToCurrentPlaceLayer();

                                switch (ret)
                                {
                                    case ExcuteResult.Abort:

                                        Machine.GetInstance().Stop();
                                        return;

                                    case ExcuteResult.Success:
                                        break;
                                }

                                // 发送允许推料信号
                                SignalPool.GetInstance().LoaderAllowPushSignal.Set();

                                firstStart = false;
                            }

                            // 等允许上料信号
                            if (!SignalPool.GetInstance().AllowLoaderMoveSignal.Wait())
                            {
                                return;
                            }

                            // 移动到下一料片
                            ExcuteResult ret2 = this.loaderBinController.MoveToNextPlaceLayer();

                            switch (ret2)
                            {
                                case ExcuteResult.Abort:

                                    Machine.GetInstance().Stop();
                                    return;

                                case ExcuteResult.Success:
                                    break;
                            }

                            // 发送允许推料信号
                            SignalPool.GetInstance().LoaderAllowPushSignal.Set();
                        }
                    }
                    catch (Exception ex)
                    {
                        AKRSXtraMessageBox.Show($"上料仓上料线程异常：{ex.Message}");
                    }
                });

                this.loaderTask.ConfigureAwait(false);
            }
        }
    }
}
