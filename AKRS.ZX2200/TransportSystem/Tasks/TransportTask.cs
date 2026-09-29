#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/18 19:33:26
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.TransportSystem.Controllers;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportSystem.Models.Programs;

namespace AKRS.ZX2200.TransportSystem.Tasks
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportSystem.Models.Enums;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    using Machine = AKRS.ZX2200.Main.Machine.MachineSupport.Machine;

    /// <summary>
    /// 描述：基板流道线程
    /// </summary>
    public class TransportTask
    {
        /// <summary>
        /// 上料皮带传到Bond,弃用
        /// </summary>
        public Task transferLoadBeltToBondTask;

        /// <summary>
        /// 送板机传到上料皮带
        /// </summary>
        public Task transferAutoLoaderToLoadingBeltTask;

        /// <summary>
        /// 上料皮带传到点胶
        /// </summary>
        public Task transferLoadBeltToDispense1Task;

        /// <summary>
        /// 上料仓传到点胶
        /// </summary>
        public Task transferLoaderBinToDispense1Task;

        /// <summary>
        /// 点胶1传到点胶2 
        /// </summary>
        public Task transferDispense1ToDispense2Task;

        /// <summary>
        /// 点胶2 传到Bond
        /// </summary>
        public Task transferDispenseToBondTask;

        /// <summary>
        /// Bond传到下料等待
        /// </summary>
        public Task transferBondToWaitingUnloadTask;

        /// <summary>
        /// 下料等待传到下料皮带
        /// </summary>
        public Task transferWaitingUnloadToUnloadBeltTask;

        /// <summary>
        /// 下料等待传到下料仓
        /// </summary>
        public Task transferWaitingUnloadToUnloadBinTask;

        /// <summary>
        /// 下料皮带到送板机
        /// </summary>
        public Task transferUnloadBeltToAutoUnloaderTask;

        /// <summary>
        /// 流道传输控制器
        /// </summary>
        private TransportController transportController = new TransportController();

        /// <summary>
        /// 上料仓控制器
        /// </summary>
        public LoaderBinController LoaderBinController = new LoaderBinController();

        /// <summary>
        /// 下料仓控制器
        /// </summary>
        public UnLoaderBinController UnLoaderBinController = new UnLoaderBinController();

        /// <summary>
        /// 流道线程启动
        /// </summary>
        public void Start()
        {
            #region 送板机到入料段(联机)

            // 联机
            if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.Online
                || MachineHardwareConfiguration.GetInstance().LoadConfiguration
                == LoadConfigurationEnum.OnlineWithAutoLoader)
            {
                if (this.transferAutoLoaderToLoadingBeltTask == null
                    || this.transferAutoLoaderToLoadingBeltTask.Status != TaskStatus.Running)
                {
                    this.transferAutoLoaderToLoadingBeltTask = Task.Run(() =>
                    {
                        CommonUtil.SetCurrentThreadName("送板机传入料段线程");

                        bool firstStart = true;
                        try
                        {
                            // 正在工作
                            while (Signal.WaitStart())
                            {
                                // 如果在单片上料模式下，没有准备好的料片，就继续等待
                                if (!TransportProvider.PrepareFeeding)
                                {
                                    Thread.Sleep(1000);
                                    continue;
                                }

                                ExcuteResult ret = this.transportController.TransferAutoLoaderToLoading();

                                switch (ret)
                                {
                                    case ExcuteResult.NoStart:
                                        Thread.Sleep(500);
                                        break;

                                    case ExcuteResult.Abort:
                                        Machine.GetInstance().Stop();
                                        break;

                                    case ExcuteResult.Success:

                                        // 传送成功，在单片上料模式下清除准备好的料片
                                        TransportProvider.PrepareFeeding = false;

                                        break;
                                }

                                Thread.Sleep(500);
                            }
                        }
                        catch (Exception ex)
                        {
                            AKRSXtraMessageBox.Show($"送板机传入料段线程：{ex.Message}");
                        }
                    });
                    this.transferAutoLoaderToLoadingBeltTask.ConfigureAwait(false);
                }
            }

            #endregion

            #region 入料段到点胶

            // 未配置自动上下料
            if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.None)
            {
                if (TransportProgram.GetInstance().DispenseSubSectionProgram.SubSectionState
                                           == SubSectionStateEnum.HasMaterial)
                {
                    DialogResult dr = AKRSMessageBoxExt.ShowWarn(
                        @" 检测到点胶载台上有料，是否继续工作?
                                        Click
                                        Yes: 继续做料
                                        No: 停止工作",
                        $"Warning",
                        new[] { "Yes", "No" },
                        new[] { DialogResult.Yes, DialogResult.No });

                    switch (dr)
                    {
                        case DialogResult.Yes:
                            SignalPool.GetInstance().TransferToDispense1FinishedSignal.Set();
                            break;

                        case DialogResult.No:
                            Machine.GetInstance().Stop();
                            break;
                    }
                }
            }
            else if (MachineHardwareConfiguration.GetInstance().LoadConfiguration != LoadConfigurationEnum.LoaderBin)
            {
                // 皮带上料
                // 防止线程多次启动
                if (this.transferLoadBeltToDispense1Task == null || this.transferLoadBeltToDispense1Task.Status != TaskStatus.Running)
                {
                    this.transferLoadBeltToDispense1Task = Task.Run(() =>
                    {
                        CommonUtil.SetCurrentThreadName("入料皮带传入点胶1线程");

                        bool firstStart = true;
                        try
                        {
                            // 正在工作
                            while (Signal.WaitStart())
                            {
                                // 第一次启动时，如果dispense table上有料 则直接开始干
                                if (!TransportDevicePara.GetInstance().IsReStartWarn && firstStart)
                                {
                                    firstStart = false;
                                    if (TransportProgram.GetInstance().DispenseSubSectionProgram.SubSectionState
                                        == SubSectionStateEnum.HasMaterial)
                                    {
                                        SignalPool.GetInstance().TransferToDispense1FinishedSignal.Set();
                                    }
                                }

                                // 第一次启动时，如果dispense table上有料 则直接开始干
                                if (firstStart)
                                {
                                    firstStart = false;
                                    if (TransportProgram.GetInstance().DispenseSubSectionProgram.SubSectionState
                                        == SubSectionStateEnum.HasMaterial)
                                    {
                                        DialogResult dr = AKRSMessageBoxExt.ShowWarn(
                                            @" 检测到点胶载台上有料，是否继续工作?
                                        Click
                                        Yes: 继续做料
                                        No: 停止工作",
                                            $"Warning",
                                            new[] { "Yes", "No" },
                                            new[] { DialogResult.Yes, DialogResult.No });

                                        switch (dr)
                                        {
                                            case DialogResult.Yes:
                                                SignalPool.GetInstance().TransferToDispense1FinishedSignal.Set();
                                                break;

                                            case DialogResult.No:
                                                Machine.GetInstance().Stop();
                                                break;
                                        }
                                    }

                                    continue;
                                }

                                // 如果在连续上料模式下，客户点击了不在上料，就继续等待
                                if (!TransportProvider.ContinuousFeeding)
                                {
                                    Thread.Sleep(1000);
                                    continue;
                                }

                                ExcuteResult ret = this.transportController.TransferLoadBeltToDispense();

                                // 如果有料 直接发信号
                                switch (ret)
                                {
                                    case ExcuteResult.NoStart:
                                        break;

                                    case ExcuteResult.Abort:
                                        Machine.GetInstance().Stop();
                                        break;

                                    case ExcuteResult.Success:
                                        // 传送到点胶1结束
                                        SignalPool.GetInstance().TransferToDispense1FinishedSignal.Set();

                                        // 传送成功，在单片上料模式下清除准备好的料片
                                        TransportProvider.PrepareFeeding = false;
                                        break;
                                }

                                if (MachineHardwareConfiguration.GetInstance().IsSystem1Configrated == false)
                                {
                                    SignalPool.GetInstance().AllowDispenseTransferToBondSignal.Set();
                                }

                                Thread.Sleep(100);
                            }
                        }
                        catch (Exception ex)
                        {
                            AKRSXtraMessageBox.Show($"入料传入点胶1线程异常：{ex.Message}");
                        }
                    });

                    this.transferLoadBeltToDispense1Task.ConfigureAwait(false);
                }
            }
            else
            {
                // 配置自动上下料
                // 防止线程多次启动
                if (this.transferLoaderBinToDispense1Task == null || this.transferLoaderBinToDispense1Task.Status != TaskStatus.Running)
                {
                    this.transferLoaderBinToDispense1Task = Task.Run(() =>
                    {
                        CommonUtil.SetCurrentThreadName("上料仓传入点胶1线程");

                        bool firstStart = true;
                        try
                        {
                            // 正在工作
                            while (Signal.WaitStart())
                            {
                                // 第一次启动时，如果dispense table上有料 则直接开始干
                                if (!TransportDevicePara.GetInstance().IsReStartWarn && firstStart)
                                {
                                    firstStart = false;
                                    if (TransportProgram.GetInstance().DispenseSubSectionProgram.SubSectionState
                                        == SubSectionStateEnum.HasMaterial)
                                    {
                                        SignalPool.GetInstance().TransferToDispense1FinishedSignal.Set();
                                    }
                                }

                                if (firstStart)
                                {
                                    firstStart = false;
                                    if (TransportProgram.GetInstance().DispenseSubSectionProgram.SubSectionState
                                        == SubSectionStateEnum.HasMaterial)
                                    {
                                        DialogResult dr = AKRSMessageBoxExt.ShowWarn(
                                            @" 检测到点胶载台上有料，是否继续工作?
                                        Click
                                        Yes: 继续做料
                                        No: 停止工作",
                                            $"Warning",
                                            new[] { "Yes", "No" },
                                            new[] { DialogResult.Yes, DialogResult.No });

                                        switch (dr)
                                        {
                                            case DialogResult.Yes:
                                                SignalPool.GetInstance().TransferToDispense1FinishedSignal.Set();
                                                break;

                                            case DialogResult.No:
                                                Machine.GetInstance().Stop();
                                                break;
                                        }
                                    }

                                    continue;
                                }

                                // 点击不再上料就一直等待
                                if (!TransportProvider.ContinuousFeeding)
                                {
                                    Thread.Sleep(1000);
                                    continue;
                                }

                                // 等允许推料信号
                                if (!SignalPool.GetInstance().LoaderAllowPushSignal.Wait())
                                {
                                    return;
                                }

                                // 推料到点胶
                                ExcuteResult ret = this.transportController.TransferLoadBinToDispense();

                                switch (ret)
                                {
                                    case ExcuteResult.NoStart:
                                        Thread.Sleep(500);
                                        continue;

                                    case ExcuteResult.Abort:
                                        Machine.GetInstance().Stop();
                                        return;

                                    case ExcuteResult.Success:

                                        // 传料完成
                                        SignalPool.GetInstance().TransferToDispense1FinishedSignal.Set();

                                        // 传送成功，在单片上料模式下清除准备好的料片
                                        TransportProvider.PrepareFeeding = false;
                                        break;

                                    case ExcuteResult.Exception:
                                        Machine.GetInstance().Stop();
                                        return;

                                    // 下一片
                                    case ExcuteResult.None:
                                        break;
                                }

                                // 手动复位
                                SignalPool.GetInstance().LoaderAllowPushSignal.ReSet();

                                // 允许料仓移动
                                SignalPool.GetInstance().AllowLoaderMoveSignal.Set();
                            }
                        }
                        catch (Exception ex)
                        {
                            AKRSXtraMessageBox.Show($"入料仓传入点胶1线程异常：{ex.Message}");
                        }
                    });

                    this.transferLoaderBinToDispense1Task.ConfigureAwait(false);
                }
            }

            #endregion

            #region 点胶1 传到 点胶2
            if (this.transferDispense1ToDispense2Task == null || this.transferDispense1ToDispense2Task.Status != TaskStatus.Running)
            {
                // 本次节点先不用， 下一迭代 加上
                this.transferDispense1ToDispense2Task = Task.Run(() =>
                {
                    CommonUtil.SetCurrentThreadName("点胶1传入点胶2线程");

                    try
                    {
                        while (Signal.WaitStart())
                        {
                            ExcuteResult ret = this.transportController.TransferDispense1ToDispense2();

                            switch (ret)
                            {
                                case ExcuteResult.NoStart:
                                    Thread.Sleep(500);
                                    break;

                                case ExcuteResult.Abort:
                                    Machine.GetInstance().Stop();
                                    break;

                                case ExcuteResult.Success:
                                    // 给Bond 域发信号, Bond轨道传送完成
                                    SignalPool.GetInstance().TransferToDispense2FinishedSignal.Set();
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AKRSXtraMessageBox.Show($"点胶1传入点胶2线程：{ex.Message}");
                    }
                });
                this.transferDispense1ToDispense2Task.ConfigureAwait(false);
            }
            #endregion

            if (this.transferDispenseToBondTask == null || this.transferDispenseToBondTask.Status != TaskStatus.Running)
            {
                this.transferDispenseToBondTask = Task.Run(() =>
                {
                    CommonUtil.SetCurrentThreadName("点胶传入Bond线程");

                    bool firstStart = true;
                    try
                    {
                        // 正在工作
                        while (Signal.WaitStart())
                        {
                            // 第一次启动时，如果dispense table上有料 则直接开始干
                            if (!TransportDevicePara.GetInstance().IsReStartWarn && firstStart)
                            {
                                firstStart = false;
                                if (TransportProgram.GetInstance().BondSubSectionProgram.SubSectionState
                                    == SubSectionStateEnum.HasMaterial)
                                {
                                    SignalPool.GetInstance().TransferToBondFinishedSignal.Set();
                                }
                            }

                            // 第一次启动时，如果Bond table上有料 则直接开始干
                            if (firstStart)
                            {
                                firstStart = false;
                                if (TransportProgram.GetInstance().BondSubSectionProgram.SubSectionState
                                    == SubSectionStateEnum.HasMaterial)
                                {
                                    DialogResult dr = AKRSMessageBoxExt.ShowWarn(
                                        @" 检测到Bond载台上有料，是否继续工作?
                                        Click
                                        Yes: 继续做料
                                        No: 停止工作",
                                        $"Warning",
                                        new[] { "Yes", "No" },
                                        new[] { DialogResult.Yes, DialogResult.No });

                                    switch (dr)
                                    {
                                        case DialogResult.Yes:
                                            SignalPool.GetInstance().TransferToBondFinishedSignal.Set();
                                            break;

                                        case DialogResult.No:
                                            Machine.GetInstance().Stop();
                                            break;
                                    }
                                }

                                continue;
                            }


                            ExcuteResult ret = this.transportController.TransferDispenseToBond();

                            switch (ret)
                            {
                                case ExcuteResult.NoStart:
                                    // 提速先注释掉
                                    //Thread.Sleep(50);
                                    break;

                                case ExcuteResult.Abort:
                                    Machine.GetInstance().Stop();
                                    break;

                                case ExcuteResult.Success:
                                    // 给Bond 域发信号, Bond轨道传送完成
                                    SignalPool.GetInstance().TransferToBondFinishedSignal.Set();
                                    break;
                            }

                            Thread.Sleep(50);
                        }
                    }
                    catch (Exception ex)
                    {
                        AKRSXtraMessageBox.Show($"点胶传入Bond线程：{ex.Message}");
                    }
                });
                this.transferDispenseToBondTask.ConfigureAwait(false);
            }

            if (this.transferBondToWaitingUnloadTask == null || this.transferBondToWaitingUnloadTask.Status != TaskStatus.Running)
            {
                this.transferBondToWaitingUnloadTask = Task.Run(() =>
                    {
                        CommonUtil.SetCurrentThreadName("Bond传入等待区线程");

                        try
                        {
                            // 正在工作
                            while (Signal.WaitStart())
                            {
                                ExcuteResult ret = this.transportController.TransferBondToWaitingUnload();

                                switch (ret)
                                {
                                    case ExcuteResult.NoStart:

                                        break;

                                    case ExcuteResult.Abort:
                                        TransportProvider.RecordTime("Bond传入等待区线程", "返回Abort");

                                        Machine.GetInstance().Stop();
                                        break;

                                    case ExcuteResult.Success:
                                        TransportProvider.RecordTime("Bond传入等待区线程", "返回Success");
                                        break;

                                    case ExcuteResult.Exception:
                                        TransportProvider.RecordTime("Bond传入等待区线程", "返回Exception");
                                        Machine.GetInstance().Stop();
                                        break;
                                }

                                Thread.Sleep(100);
                            }
                        }
                        catch (Exception ex)
                        {
                            TransportProvider.RecordTime("Bond传入等待区线程异常", $"{ex.Message}");
                            AKRSXtraMessageBox.Show($"Bond传入等待区线程异常：{ex.Message}");
                        }
                    });
                this.transferBondToWaitingUnloadTask.ConfigureAwait(false);
            }

            #region 等待区到出料

            // 未配置自动上下料
            if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.None)
            {
                return;
            }
            else if (MachineHardwareConfiguration.GetInstance().LoadConfiguration != LoadConfigurationEnum.LoaderBin)
            {
                if (this.transferWaitingUnloadToUnloadBeltTask == null || this.transferWaitingUnloadToUnloadBeltTask.Status != TaskStatus.Running)
                {
                    this.transferWaitingUnloadToUnloadBeltTask = Task.Run(
                        () =>
                            {
                                CommonUtil.SetCurrentThreadName("等待区传入出料区线程");
                                try
                                {
                                    while (Signal.WaitStart())
                                    {
                                        ExcuteResult ret = this.transportController.TransferWaitingUnloadToUnloadBelt();

                                        switch (ret)
                                        {
                                            case ExcuteResult.NoStart:

                                                break;

                                            case ExcuteResult.Abort:

                                                TransportProvider.RecordTime("等待区传入出料区线程", "返回Abort");

                                                Machine.GetInstance().Stop();

                                                TransportProvider.RecordTime("等待区传入出料区线程", "设备停止");
                                                break;

                                            case ExcuteResult.Success:

                                                TransportProvider.RecordTime("等待区传入出料区线程", "返回Success");
                                                break;

                                            case ExcuteResult.Exception:

                                                TransportProvider.RecordTime("等待区传入出料区线程", "返回Exception");

                                                Machine.GetInstance().Stop();
                                                break;
                                        }

                                        Thread.Sleep(100);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    AKRSXtraMessageBox.Show($"等待区传入出料区线程：{ex.Message}");
                                }
                            });
                    this.transferWaitingUnloadToUnloadBeltTask.ConfigureAwait(false);
                }
            }
            else   // 配置自动上下料
            {
                if (this.transferWaitingUnloadToUnloadBinTask == null || this.transferWaitingUnloadToUnloadBinTask.Status != TaskStatus.Running)
                {
                    this.transferWaitingUnloadToUnloadBinTask = Task.Run(
                        () =>
                        {
                            CommonUtil.SetCurrentThreadName("等待区传入下料仓线程");
                            bool firstStart = true;
                            try
                            {
                                // 正在工作
                                while (Signal.WaitStart())
                                {
                                    // 等允许推料信号
                                    if (!SignalPool.GetInstance().UnloaderAllowPushSignal.Wait())
                                    {
                                        return;
                                    }

                                    // 推料到下料仓
                                    ExcuteResult ret = this.transportController.TransferWaitingUnloadToUnloadBin();

                                    switch (ret)
                                    {
                                        case ExcuteResult.NoStart:
                                            Thread.Sleep(500);
                                            continue;

                                        case ExcuteResult.Abort:
                                            Machine.GetInstance().Stop();
                                            return;

                                        case ExcuteResult.Success:
                                            break;

                                        case ExcuteResult.Exception:
                                            Machine.GetInstance().Stop();
                                            return;
                                    }

                                    Thread.Sleep(200);

                                    // 手动复位
                                    SignalPool.GetInstance().UnloaderAllowPushSignal.ReSet();

                                    // 允许下料仓移动
                                    SignalPool.GetInstance().AllowUnloaderMoveSignal.Set();
                                }
                            }
                            catch (Exception ex)
                            {
                                AKRSXtraMessageBox.Show($"等待区传入下料仓线程：{ex.Message}");
                            }
                        });
                    this.transferWaitingUnloadToUnloadBinTask.ConfigureAwait(false);
                }
            }

            #endregion

            #region 出料段到收板机(联机)

            // 联机
            if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.Online || MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.OnlineWithAutoUnloader)
            {
                if (this.transferUnloadBeltToAutoUnloaderTask == null
                    || this.transferUnloadBeltToAutoUnloaderTask.Status != TaskStatus.Running)
                {
                    this.transferUnloadBeltToAutoUnloaderTask = Task.Run(() =>
                    {
                        CommonUtil.SetCurrentThreadName("出料段到收板机线程");

                        bool firstStart = true;
                        try
                        {
                            // 正在工作
                            while (Signal.WaitStart())
                            {
                                ExcuteResult ret = this.transportController.TransferUnloadBeltToAutoUnloader();

                                switch (ret)
                                {
                                    case ExcuteResult.NoStart:
                                        Thread.Sleep(100);
                                        break;

                                    case ExcuteResult.Abort:
                                        Machine.GetInstance().Stop();
                                        break;

                                    case ExcuteResult.Success:


                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            AKRSXtraMessageBox.Show($"出料段到收板机线程：{ex.Message}");
                        }
                    });
                    this.transferUnloadBeltToAutoUnloaderTask.ConfigureAwait(false);
                }
            }

            #endregion
        }

        /// <summary>
        ///  开启流道空跑
        /// </summary>
        public void StartDryRun()
        {
            // 轨道顶升气缸降下
            this.transportController.DispenseSubSectionController.Clamp();
            this.transportController.BondSubSectionController.Clamp();

            // 点胶载台置为有料
            TransportProgram.GetInstance().DispenseSubSectionProgram.SubSectionState =
                SubSectionStateEnum.HasMaterial;
            SignalPool.GetInstance().TransferToDispense1FinishedSignal.Set();

            TransportProgram.GetInstance().DispenseSubSectionProgram.TransportUnit =
                    new TransportUnit(CurrentMachineSystemEnum.System1);
            //TransportProgram.GetInstance().DispenseSubSectionProgram.TransportUnit =
            //    new TransportUnit();

            // 固晶载台置为有料
            TransportProgram.GetInstance().BondSubSectionProgram.SubSectionState =
                SubSectionStateEnum.HasMaterial;
            SignalPool.GetInstance().TransferToBondFinishedSignal.Set();
            TransportProgram.GetInstance().BondSubSectionProgram.TransportUnit =
              new TransportUnit(CurrentMachineSystemEnum.System2);
            //TransportProgram.GetInstance().BondSubSectionProgram.TransportUnit =
            //    new TransportUnit();

            if (this.transferDispenseToBondTask == null || this.transferDispenseToBondTask.Status != TaskStatus.Running)
            {
                this.transferDispenseToBondTask = Task.Run(() =>
                {
                    CommonUtil.SetCurrentThreadName("点胶传入Bond线程");

                    bool firstStart = true;
                    try
                    {
                        // 正在工作
                        while (Signal.WaitStart())
                        {
                            if (Machine.GetInstance().IsStop())
                            {
                                return;
                            }

                            // 等点胶线程给信号
                            bool rt = SignalPool.GetInstance().AllowDispenseTransferToBondSignal.Wait();

                            if (rt == false)
                            {
                                // 如果点了停止 则退出
                                return;
                            }
                            else
                            {
                                // 点胶载台置为有料
                                TransportProgram.GetInstance().DispenseSubSectionProgram.SubSectionState =
                                    SubSectionStateEnum.HasMaterial;
                                SignalPool.GetInstance().TransferToDispense1FinishedSignal.Set();
                                TransportProgram.GetInstance().DispenseSubSectionProgram.TransportUnit =
                       new TransportUnit(CurrentMachineSystemEnum.System1);
                            }

                            Thread.Sleep(500);
                        }
                    }
                    catch (Exception ex)
                    {
                        AKRSXtraMessageBox.Show($"点胶传入Bond线程：{ex.Message}");
                    }
                });
                this.transferDispenseToBondTask.ConfigureAwait(false);
            }

            if (this.transferBondToWaitingUnloadTask == null || this.transferBondToWaitingUnloadTask.Status != TaskStatus.Running)
            {
                this.transferBondToWaitingUnloadTask = Task.Run(() =>
                    {
                        CommonUtil.SetCurrentThreadName("Bond传入等待区线程");

                        try
                        {
                            // 正在工作
                            while (Signal.WaitStart())
                            {
                                if (Machine.GetInstance().IsStop())
                                {
                                    return;
                                }

                                // 等固晶线程给信号
                                bool rt = SignalPool.GetInstance().AllowBondTransferToWaitingUnloadSignal.Wait();

                                if (rt == false)
                                {
                                    // 如果点了停止 则退出
                                    return;
                                }
                                else
                                {
                                    // 固晶载台置为有料
                                    TransportProgram.GetInstance().BondSubSectionProgram.SubSectionState =
                                        SubSectionStateEnum.HasMaterial;
                                    SignalPool.GetInstance().TransferToBondFinishedSignal.Set();
                                    TransportProgram.GetInstance().BondSubSectionProgram.TransportUnit =
                    new TransportUnit(CurrentMachineSystemEnum.System2);
                                }

                                Thread.Sleep(500);
                            }
                        }
                        catch (Exception ex)
                        {
                            TransportProvider.RecordTime("Bond传入等待区线程异常", $"{ex.Message}");
                            AKRSXtraMessageBox.Show($"Bond传入等待区线程异常：{ex.Message}");
                        }
                    });
                this.transferBondToWaitingUnloadTask.ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 线程是否存活
        /// </summary>
        /// <returns>结果</returns>
        public bool IsAlive()
        {
            if (MachineHardwareConfiguration.GetInstance().LoadConfiguration != LoadConfigurationEnum.LoaderBin)
            {
                if (this.transferBondToWaitingUnloadTask != null && this.transferDispense1ToDispense2Task != null
                                                                 && this.transferDispenseToBondTask != null
                                                                 && this.transferLoadBeltToDispense1Task != null
                                                                 && this.transferWaitingUnloadToUnloadBeltTask != null)
                {
                    return this.transferBondToWaitingUnloadTask.Status == TaskStatus.Running
                           && this.transferDispense1ToDispense2Task.Status == TaskStatus.Running
                           && this.transferDispenseToBondTask.Status == TaskStatus.Running
                           && this.transferLoadBeltToDispense1Task.Status == TaskStatus.Running
                           && this.transferWaitingUnloadToUnloadBeltTask.Status == TaskStatus.Running;
                }
            }
            else
            {
                if (this.transferLoaderBinToDispense1Task != null && this.transferDispense1ToDispense2Task != null
                                                                 && this.transferDispenseToBondTask != null
                                                                 && this.transferLoadBeltToDispense1Task != null
                                                                 && this.transferWaitingUnloadToUnloadBinTask != null)
                {
                    return this.transferLoaderBinToDispense1Task.Status == TaskStatus.Running
                           && this.transferDispense1ToDispense2Task.Status == TaskStatus.Running
                           && this.transferDispenseToBondTask.Status == TaskStatus.Running
                           && this.transferLoadBeltToDispense1Task.Status == TaskStatus.Running
                           && this.transferWaitingUnloadToUnloadBinTask.Status == TaskStatus.Running;
                }
            }

            return false;
        }
    }
}
