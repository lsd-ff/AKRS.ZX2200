#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2024/3/23 20:17:22
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

using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.InOutPut;
using AKRS.ZX2200.TransportSystem.Models.Programs;
using AKRS.ZX2200.TransportUnitSystem;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using DevExpress.CodeParser;
using DevExpress.XtraEditors;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Machine = AKRS.ZX2200.Main.Machine.MachineSupport.Machine;

namespace AKRS.ZX2200.TransportSystem.Controllers
{
    /// <summary>
    /// 描述：流道系统控制器
    /// </summary>
    public class TransportController
    {
        /// <summary>
        /// 流道程式
        /// </summary>
        public TransportProgram TransportProgram => TransportProgram.GetInstance();

        /// <summary>
        /// 上料仓控制器
        /// </summary>
        public LoaderBinController LoaderBinController = new LoaderBinController();

        /// <summary>
        /// load section 控制器
        /// </summary>
        public LoadingSubSectionController LoadingSubSectionController = new LoadingSubSectionController();

        /// <summary>
        /// load section 控制器
        /// </summary>
        public DispenseSubSectionController DispenseSubSectionController = new DispenseSubSectionController();

        /// <summary>
        /// load section 控制器
        /// </summary>
        public BondSubSectionController BondSubSectionController = new BondSubSectionController();

        /// <summary>
        /// load section 控制器
        /// </summary>
        public WaitingUnloadSubSectionController WaitingUnloadSubSectionController = new WaitingUnloadSubSectionController();

        /// <summary>
        /// load section 控制器
        /// </summary>
        public UnloadingSubSectionController UnloadingSubSectionController = new UnloadingSubSectionController();

        /// <summary>
        /// 下料仓控制器
        /// </summary>
        public UnLoaderBinController UnLoaderBinController = new UnLoaderBinController();

        /// <summary>
        /// 程式
        /// </summary>
        private TransportProgram transportProgram => TransportProgram.GetInstance();

        /// <summary>
        ///  入料到点胶调试线程
        /// </summary>
        private Task transferLoadBeltToDispenseTask = null;

        /// <summary>
        ///  点胶1到点胶2调试线程
        /// </summary>
        Task transferDispense1ToDispense2Task = null;

        /// <summary>
        /// 点胶到固精调试线程
        /// </summary>
        Task transferDispenseToBondTask = null;

        /// <summary>
        /// 固精到出料等待段调试线程
        /// </summary>
        Task transferBondToWaitingUnloadTask = null;

        /// <summary>
        /// 出料等待段到出料段调试线程
        /// </summary>
        Task transferWaitingUnloadToUnloadBeltTask = null;

        /// <summary>
        ///  出料等待段到下料仓调试线程
        /// </summary>
        Task transferWaitingUnloadToUnloadBinTask = null;

        /// <summary>
        /// 检查流道和记忆是否一样
        /// </summary>
        public void MapMaterial()
        {
            // 1. 倒转检测 点胶和 Bond载台上是有有料, 并判断在是否和载台记忆是否符合，不符合则报警
            this.BondSubSectionController.MapBelt(true);
            if (MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                this.DispenseSubSectionController.MapBelt(true);
            }

            if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.Online || MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.OnlineWithAutoLoader)
            {
                this.transportProgram.LoadingSubSectionProgram.SubSectionState = this.LoadingSubSectionController.HasMaterialOnFront() ? SubSectionStateEnum.HasMaterial : SubSectionStateEnum.NoMaterial;
            }
        }

        /// <summary>
        /// 是否存在TU
        /// </summary>
        public bool IsExistTu => this.transportProgram.LoadingSubSectionProgram.TransportUnit != null
            || this.transportProgram.DispenseSubSectionProgram.TransportUnit != null
            || this.transportProgram.BondSubSectionProgram.TransportUnit != null
            || this.transportProgram.WaitingUnloadSubSectionProgram.TransportUnit != null
            || this.transportProgram.UnloadingSubSectionProgram.TransportUnit != null;

        /// <summary>
        /// 流道复位
        /// </summary>
        public void InitializeTS()
        {
            // 如果在机器处于工作中，不能初始化流道
            if (Machine.GetInstance().IsWorking()
                || Machine.GetInstance().IsPause())
            {
                AKRSXtraMessageBox.Show("设备正在自动运行中，不能清空载具", "警告");

                return;
            }

            DialogResult dr = AKRSXtraMessageBox.Show("清空轨道设备将不能记忆已经生产的信息，确认是否执行此操作", "提示", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:

                    if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.LoaderBin)
                    {
                        // 上下料仓推杆缩回
                        this.LoaderBinController.MovePushRodHome();
                        this.UnLoaderBinController.MovePushRodHome();
                    }

                    if (MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
                    {
                        System1Domain.GetInstance().DispenseController.MoveToSafePos();
                    }

                    // 点胶和bond轴移动到安全位置
                    System2Domain.GetInstance().BondModuleController.MoveToSafePos();

                    this.DispenseSubSectionController.UnClamp();

                    // 点胶挡料气缸 降下
                    this.DispenseSubSectionController.BlockCylinder1DownAndDelay(50);
                    this.DispenseSubSectionController.BlockCylinder2DownAndDelay(50);


                    this.BondSubSectionController.UnClamp();

                    // bond 挡料气缸 降下
                    this.BondSubSectionController.BlockCylinderDownAndDelay(50);

                    // 清空记忆状态
                    this.transportProgram.LoadingSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;
                    this.transportProgram.DispenseSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;
                    this.transportProgram.BondSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;
                    this.transportProgram.WaitingUnloadSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;
                    this.transportProgram.UnloadingSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;

                    // 清空载具对象
                    this.transportProgram.LoadingSubSectionProgram.TransportUnit = null;
                    this.transportProgram.DispenseSubSectionProgram.TransportUnit = null;
                    this.transportProgram.BondSubSectionProgram.TransportUnit = null;
                    this.transportProgram.WaitingUnloadSubSectionProgram.TransportUnit = null;
                    this.transportProgram.UnloadingSubSectionProgram.TransportUnit = null;
                    this.transportProgram.Save();

                    AKRSXtraMessageBox.Show("传输系统清空载具完成，请手动将所有载具拿走", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    break;

                case DialogResult.No:
                    break;
            }
        }

        /// <summary>
        /// 点胶复位
        /// </summary>
        public void InitializeTSInDispense()
        {
            // 如果在机器处于工作中，不能初始化流道
            if (Machine.GetInstance().IsWorking()
                || Machine.GetInstance().IsPause())
            {
                AKRSXtraMessageBox.Show("设备正在自动运行中，不能清空载具", "警告");

                return;
            }

            DialogResult dr = AKRSXtraMessageBox.Show("清空轨道设备将不能记忆已经生产的信息，确认是否执行此操作", "提示", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:

                    if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.LoaderBin)
                    {
                        // 上下料仓推杆缩回
                        this.LoaderBinController.MovePushRodHome();
                        this.UnLoaderBinController.MovePushRodHome();
                    }

                    if (MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
                    {
                        System1Domain.GetInstance().DispenseController.MoveToSafePos();
                    }

                    this.DispenseSubSectionController.UnClamp();

                    // 点胶挡料气缸 降下
                    this.DispenseSubSectionController.BlockCylinder1DownAndDelay(50);
                    this.DispenseSubSectionController.BlockCylinder2DownAndDelay(50);

                    // 清空记忆状态
                    this.transportProgram.DispenseSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;

                    // 清空载具对象
                    this.transportProgram.DispenseSubSectionProgram.TransportUnit = null;
                    this.transportProgram.Save();

                    AKRSXtraMessageBox.Show("传输系统清空载具完成，请手动将所有载具拿走", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    break;

                case DialogResult.No:
                    break;
            }
        }

        /// <summary>
        /// Bond段复位
        /// </summary>
        public void InitializeTSInBond()
        {
            // 如果在机器处于工作中，不能初始化流道
            if (Machine.GetInstance().IsWorking()
                || Machine.GetInstance().IsPause())
            {
                AKRSXtraMessageBox.Show("设备正在自动运行中，不能清空载具", "警告");

                return;
            }

            DialogResult dr = AKRSXtraMessageBox.Show("清空轨道设备将不能记忆已经生产的信息，确认是否执行此操作", "提示", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:

                    if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.LoaderBin)
                    {
                        // 上下料仓推杆缩回
                        this.LoaderBinController.MovePushRodHome();
                        this.UnLoaderBinController.MovePushRodHome();
                    }

                    // 先移到安全位
                    System2Domain.GetInstance().BondModuleController.MoveToSafePos();

                    this.BondSubSectionController.UnClamp();

                    // bond 挡料气缸 降下
                    this.BondSubSectionController.BlockCylinderDownAndDelay(50);

                    // 清空记忆状态
                    this.transportProgram.BondSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;

                    // 清空载具对象
                    this.transportProgram.BondSubSectionProgram.TransportUnit = null;

                    this.transportProgram.Save();

                    AKRSXtraMessageBox.Show("传输系统清空载具完成，请手动将所有载具拿走", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    break;

                case DialogResult.No:
                    break;
            }
        }

        /// <summary>
        /// 传送载具
        /// </summary>
        public void TransportUnitIndex()
        {
            // 未配置上下料
            if (MachineHardwareConfiguration.GetInstance().LoadConfiguration != LoadConfigurationEnum.LoaderBin)
            {
                if (transferLoadBeltToDispenseTask == null || transferLoadBeltToDispenseTask.IsCompleted)
                {
                    transferLoadBeltToDispenseTask = Task.Run(() =>
                    {
                        CommonUtil.SetCurrentThreadName("入料到点胶调试线程");

                        this.TransferLoadBeltToDispense(true);
                    });
                }

                if (transferDispense1ToDispense2Task == null || transferDispense1ToDispense2Task.IsCompleted)
                {
                    CommonUtil.SetCurrentThreadName("点胶1到点胶2调试线程");

                    transferDispense1ToDispense2Task = Task.Run(() => this.TransferDispense1ToDispense2(true));
                }

                if (transferDispenseToBondTask == null || transferDispenseToBondTask.IsCompleted)
                {
                    CommonUtil.SetCurrentThreadName("点胶到固精调试线程");

                    transferDispenseToBondTask = Task.Run(() => this.TransferDispenseToBond(true));
                }

                if (transferBondToWaitingUnloadTask == null || transferBondToWaitingUnloadTask.IsCompleted)
                {
                    CommonUtil.SetCurrentThreadName("固精到出料等待段调试线程");

                    transferBondToWaitingUnloadTask = Task.Run(() => this.TransferBondToWaitingUnload(true));
                }

                if (transferWaitingUnloadToUnloadBeltTask == null || transferWaitingUnloadToUnloadBeltTask.IsCompleted)
                {
                    CommonUtil.SetCurrentThreadName("出料等待段到出料段调试线程");

                    transferWaitingUnloadToUnloadBeltTask = Task.Run(this.TransferWaitingUnloadToUnloadBelt);
                }


                // 这里wait会导致报警的时候死锁
                //t1.Wait();
                //t2.Wait();
                //t3.Wait();
                //t4.Wait();
                //t5.Wait();
            }
            else
            {
                //// 配置上下料
                //// 先去准备位
                // ExcuteResult ret = this.transportController.MoveLoaderToPreparePos();

                //Task t1 = Task.Run(() =>
                //    {
                //        this.transportController.TransferLoadBeltToDispense(true);
                //    });

                if (transferDispense1ToDispense2Task == null || transferDispense1ToDispense2Task.IsCompleted)
                {
                    CommonUtil.SetCurrentThreadName("点胶1到点胶2调试线程");

                    transferDispense1ToDispense2Task = Task.Run(() => this.TransferDispense1ToDispense2(true));
                }

                if (transferDispenseToBondTask == null || transferDispenseToBondTask.IsCompleted)
                {
                    CommonUtil.SetCurrentThreadName("点胶到固精调试线程");

                    transferDispenseToBondTask = Task.Run(() => this.TransferDispenseToBond(true));
                }

                if (transferBondToWaitingUnloadTask == null || transferBondToWaitingUnloadTask.IsCompleted)
                {
                    CommonUtil.SetCurrentThreadName("固精到出料等待段调试线程");

                    transferBondToWaitingUnloadTask = Task.Run(() => this.TransferBondToWaitingUnload(true));
                }

                if (TransportDomain.GetInstance().UnloaderTask.Enable)
                {
                    if (transferWaitingUnloadToUnloadBinTask == null || transferWaitingUnloadToUnloadBinTask.IsCompleted)
                    {
                        CommonUtil.SetCurrentThreadName("出料等待段到下料仓调试线程");

                        transferWaitingUnloadToUnloadBinTask = Task.Run(() => this.TransferWaitingUnloadToUnloadBin(true));
                    }
                }

                //t1.Wait();
                //t2.Wait();
                //t3.Wait();
                //t4.Wait();
                //t5.Wait();
            }
        }

        /// <summary>
        /// 送板机传到上料段
        /// </summary>
        /// <param name="isManual">是否手动</param>
        /// <returns> 执行结果 NoStart: 不满足条件， 进入下一个循环， Success：成功， Abort : 退出 </returns>
        public ExcuteResult TransferAutoLoaderToLoading(bool isManual = false)
        {
            try
            {
                // 检查loading段有没有板
                if (this.TransportProgram.LoadingSubSectionProgram.SubSectionState == SubSectionStateEnum.NoMaterial)
                {
                Retry:

                    // 没有的话就给送板机发信号
                    this.LoadingSubSectionController.SetLoadingTableNeedTabletSignal(true);

                    Stopwatch sp = Stopwatch.StartNew();

                    while (true)
                    {
                        // 送板机是否传料成功
                        bool hasMaterial = this.LoadingSubSectionController.HasMaterialOnFront();

                        if (hasMaterial)
                        {
                            // 同时皮带传送
                            this.TransportProgram.LoadingSubSectionProgram.SubSectionTransferState =
                                SubSectionStateTransferEnum.Transfering;

                            // 皮带1转动
                            this.LoadingSubSectionController.MoveBeltNoWaitArrive();

                            Thread.Sleep(200);

                            // 关要料信号
                            this.LoadingSubSectionController.SetLoadingTableNeedTabletSignal(false);

                            Thread.Sleep(500);

                            // 皮带停止
                            this.LoadingSubSectionController.StopMove();

                            break;
                        }

                        // 超时报警,暂定5s
                        if (sp.ElapsedMilliseconds > 5000)
                        {
                            this.TransportProgram.LoadingSubSectionProgram.SubSectionTransferState =
                                SubSectionStateTransferEnum.Alarm;

                            // 关要料信号,防止换料时板子一直流
                            this.LoadingSubSectionController.SetLoadingTableNeedTabletSignal(false);

                            DialogResult dr = AKRSMessageBoxExt.Show(
                                @"上料超时!",
                                $"报警",
                                new[] { "重试", "终止", "不再上料" },
                                new[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore });

                            switch (dr)
                            {
                                case DialogResult.Retry:
                                    goto Retry;

                                case DialogResult.Abort:
                                    // 停止
                                    return ExcuteResult.Abort;

                                // 不再上料
                                case DialogResult.Ignore:

                                    TransportProvider.ContinuousFeeding = false;

                                    return ExcuteResult.NoStart;
                            }
                        }

                        Thread.Sleep(1);
                    }
                }
                else
                {
                    // 有的话不发信号
                    this.LoadingSubSectionController.SetLoadingTableNeedTabletSignal(false);

                    return ExcuteResult.NoStart;
                }

                // 状态改成有料
                this.TransportProgram.LoadingSubSectionProgram.SubSectionState =
                    SubSectionStateEnum.HasMaterial;
                this.TransportProgram.LoadingSubSectionProgram.TransportUnit = new TransportUnit(CurrentMachineSystemEnum.System2);

                this.TransportProgram.Save();
                return ExcuteResult.Success;
            }
            catch (Exception ex)
            {
                LogHelper.Post(
                    Level.Error,
                    $"产品从送板机传到上料载台出现异常",
                    ex,
                    LogCategory.Transport);

                AKRSXtraMessageBox.Show(
                    $@"产品从送板机传到上料载台出现异常: \r\n
                                          Exception message:{ex.Message} \r\n
                                          StackTrace: {ex.StackTrace}",
                    "异常",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return ExcuteResult.Exception;
            }
        }

        /// <summary>
        /// 从上料皮带传送到点胶
        /// 是否不在上料
        /// </summary>
        /// <param name="isManual"> 是否手动 </param>
        /// <returns> 执行结果 </returns>
        public ExcuteResult TransferLoadBeltToDispense(bool isManual = false)
        {
            try
            {
            retry:
                if (this.TransportProgram.DispenseSubSectionProgram.SubSectionState ==
                    SubSectionStateEnum.HasMaterial)
                {
                    // 如果不满足条件
                    return ExcuteResult.NoStart;
                }

                // 联机
                if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.Online)
                {
                    // 如果Loading段无料
                    if (this.TransportProgram.LoadingSubSectionProgram.SubSectionState ==
                        SubSectionStateEnum.NoMaterial)
                    {
                        Thread.Sleep(50);
                        return ExcuteResult.NoStart;
                    }
                }

                // 开始尝试传送产品
                this.TransportProgram.LoadingSubSectionProgram.SubSectionTransferState =
                    SubSectionStateTransferEnum.Transfering;

                // 按时间传送， 时间配置在参数里
                InOutPutBeltSetting inOutPutBeltSetting = TransportProgram.LoadingSubSectionProgram.InOutPutBeltSetting;

                if (inOutPutBeltSetting == null)
                {
                    DialogResult dr = AKRSMessageBoxExt.Show(
                        @"进料皮带数据集不存在\r\n
                          重试 : 重新进料
                          终止 :  退出工作
                          忽略:  忽略此异常 ",
                        $"报警",
                        new[] { "重试", "终止", "忽略" },
                        new[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore },
                        AlarmLevel.FirstLevel);

                    switch (dr)
                    {
                        case DialogResult.Retry:
                            goto retry;

                        case DialogResult.Abort:
                            return ExcuteResult.Abort;

                        case DialogResult.Ignore:
                            break;
                    }
                }

                List<Task> tasks = new List<Task>();
                tasks.Add(Task.Run(
                    () =>
                        {
                            // 点胶气缸1顶起，准备接料
                            this.DispenseSubSectionController.BlockCylinder1UpAndDelay(10);
                        }));

                tasks.Add(Task.Run(
                    this.DispenseSubSectionController.UnClamp));

                // 点胶气缸2顶起，准备接料
                // this.DispenseSubSectionController.BlockCylinder1UpAndDelay(20);

                // 等待所有任务完成
                Task.WaitAll(tasks.ToArray());

                // 皮带1转动
                this.LoadingSubSectionController.MoveBeltNoWaitArrive();

                Thread.Sleep(200);

                bool hasMaterialInModule = false;

                // 如果 A传感器 没感应到，则报警（载台1 无料）
                while (this.LoadingSubSectionController.IsMoving())
                {
                    // 判断传感器是否感应到有料
                    hasMaterialInModule = this.LoadingSubSectionController.HasMaterialInModule();

                    // 如果感应到了 则跳出循环
                    if (hasMaterialInModule)
                    {
                        // 生成一个Transport Unit 对象
                        if (this.TransportProgram.LoadingSubSectionProgram.TransportUnit == null)
                        {
                            this.TransportProgram.LoadingSubSectionProgram.TransportUnit =
                                new TransportUnitSystem.Module.Matter.TransportUnit("上料载具对象生成");

                            this.TransportProgram.Save();
                        }

                        if (this.TransportProgram.LoadingSubSectionProgram.TransportUnit == null)
                        {
                            Machine.GetInstance().Stop();

                            throw new Exception("create transport unit fail");
                        }

                        break;
                    }

                    Thread.Sleep(30);
                }

                // 如果载台都转动到位 且传感器A还没有感应到 则报警
                if (hasMaterialInModule == false)
                {
                    if (isManual == false)
                    {
                        this.TransportProgram.LoadingSubSectionProgram.SubSectionState =
                            SubSectionStateEnum.NoMaterial;

                        // 报警 载台1 无料
                        DialogResult dr = AKRSMessageBoxExt.Show(
                                $"检测到进料载台无料！ \r\n",
                                "报警",
                                new[] { "重新上料", "不再上料", "终止" },
                                new DialogResult[]
                                    {
                                        DialogResult.Retry, DialogResult.Cancel,DialogResult.Abort
                                    },
                                AlarmLevel.SecondLevel);

                        switch (dr)
                        {
                            case DialogResult.Retry:
                                goto retry;

                            case DialogResult.Cancel:

                                // 关闭持续上料
                                TransportProvider.ContinuousFeeding = false;
                                return ExcuteResult.NoStart;

                            case DialogResult.Abort:
                                return ExcuteResult.Abort;
                        }
                    }
                    else
                    {
                        // 手动模式 不需要报警
                        return ExcuteResult.None;
                    }
                }

                // 修改点胶传料状态
                this.TransportProgram.DispenseSubSectionProgram.SubSectionTransferState =
                    SubSectionStateTransferEnum.Transfering;

                // 点胶皮带 转动
                this.DispenseSubSectionController.MoveBeltNoWaitArrive();
                Thread.Sleep(200);

                while (this.DispenseSubSectionController.IsMoving())
                {
                    // 物料检测（传感器C） 
                    hasMaterialInModule = this.DispenseSubSectionController.HasMaterialInModule();

                    // 如果 C 感应到了 则跳出循环
                    if (hasMaterialInModule)
                    {
                        break;
                    }

                    Thread.Sleep(200);
                }

                // 皮带转完了 或者C感应到了 跳出循环
                // 如果 C没感应到则说明是皮带转完了停下来的
                if (hasMaterialInModule == false)
                {
                    this.TransportProgram.DispenseSubSectionProgram.SubSectionState =
                        SubSectionStateEnum.NoMaterial;

                    this.DispenseSubSectionController.BlockCylinder1DownAndDelay(50);

                    this.TransportProgram.LoadingSubSectionProgram.SubSectionTransferState =
                            SubSectionStateTransferEnum.Alarm;

                    this.TransportProgram.DispenseSubSectionProgram.SubSectionTransferState =
                            SubSectionStateTransferEnum.Alarm;

                    // 如果B感应到，则报警：点胶台进料卡料
                    DialogResult dr = AKRSMessageBoxExt.Show(
                            @"点胶载台卡料
                              点击
                              重试 : 重新上料
                              终止 ：退出工作
                              忽略: 忽略此报警",
                            $"报警",
                            new[] { "重试", "终止", "忽略" },
                            new[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore });

                    switch (dr)
                    {
                        case DialogResult.Abort:
                            // 停止
                            return ExcuteResult.Abort;

                        case DialogResult.Retry:
                            goto retry;

                        case DialogResult.Ignore:
                            break;
                    }
                }
                else // C 感应到了
                {
                    // 是C感应到 才停下来的
                    while (this.DispenseSubSectionController.IsMoving())
                    {
                        Thread.Sleep(500);
                    }

                    // 物料检测（传感器C） 
                    hasMaterialInModule = this.DispenseSubSectionController.HasMaterialInModule();

                    if (hasMaterialInModule)
                    {
                        this.TransportProgram.LoadingSubSectionProgram.SubSectionTransferState =
                            SubSectionStateTransferEnum.Alarm;

                        this.TransportProgram.DispenseSubSectionProgram.SubSectionTransferState =
                            SubSectionStateTransferEnum.Alarm;

                        // 如果B 感应到，则报警：点胶料台入料卡料
                        DialogResult dr = AKRSMessageBoxExt.Show(
                            @"点胶载台卡料
                              点击
                              重试 : 重新上料
                              终止 ：退出工作
                              忽略: 忽略此报警",
                            $"报警",
                            new[] { "重试", "终止", "忽略" },
                            new[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore });

                        switch (dr)
                        {
                            case DialogResult.Abort:
                                // 停止
                                return ExcuteResult.Abort;

                            case DialogResult.Retry:
                                goto retry;

                            case DialogResult.Ignore:
                                break;
                        }
                    }

                    this.LoadingSubSectionController.StopMove();

                    // 没配置系统1 气缸就不降落
                    if (MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
                    {
                        // 传输成功 
                        // 顶升气缸降落
                        this.DispenseSubSectionController.Clamp(true);
                    }


                    this.TransportProgram.DispenseSubSectionProgram.TransportUnit =
                        this.TransportProgram.LoadingSubSectionProgram.TransportUnit;

                    this.TransportProgram.DispenseSubSectionProgram.TransportUnit.Refresh();

                    this.TransportProgram.DispenseSubSectionProgram.SubSectionState =
                        SubSectionStateEnum.HasMaterial;

                    this.TransportProgram.LoadingSubSectionProgram.TransportUnit = null;

                    // 修改记忆状态
                    this.TransportProgram.LoadingSubSectionProgram.SubSectionState =
                        SubSectionStateEnum.NoMaterial;

                    this.TransportProgram.LoadingSubSectionProgram.SubSectionTransferState =
SubSectionStateTransferEnum.Ready;
                    this.TransportProgram.DispenseSubSectionProgram.SubSectionTransferState =
               SubSectionStateTransferEnum.Ready;

                    // 点胶传送到位
                    this.TransportProgram.DispenseSubSectionProgram.IsTuInDispense1 = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"产品传入点胶载台出现异常 ", ex, LogCategory.Transport);

                AKRSXtraMessageBox.Show(
                    $@"产品传入点胶载台出现异常: \r\n
                                          Exception message:{ex.Message} \r\n
                                          StackTrace: {ex.StackTrace}",
                    "Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return ExcuteResult.Exception;
            }

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 从上料仓推料到点胶
        /// 是否不在上料
        /// </summary>
        /// <param name="isManual"> 是否手动 </param>
        /// <returns> 执行结果 </returns>
        public ExcuteResult TransferLoadBinToDispense(bool isManual = false)
        {
            try
            {
                // 如果点胶载台有料 则什么都不做  返回
                if (this.TransportProgram.DispenseSubSectionProgram.SubSectionState == SubSectionStateEnum.HasMaterial)
                {
                    Thread.Sleep(500);
                    return ExcuteResult.NoStart;
                }

                // 点胶气缸1顶起，准备接料
                this.DispenseSubSectionController.BlockCylinder1UpAndDelay(10);

                // 点胶气缸2顶起，准备接料
                // this.DispenseSubSectionController.BlockCylinder1UpAndDelay(20);

                // 点胶载台气缸2顶起，准备接料
                this.DispenseSubSectionController.UnClamp();

            RePush:
                this.DispenseSubSectionController.BlockCylinder1UpAndDelay(10);

                // 推杆先缩回 再推料  保险起见
                ExcuteResult res = this.LoaderBinController.MovePushRodHome();
                res = this.LoaderBinController.PushTabletToDispenseNoWait();

                // 卡料;
                if (res != ExcuteResult.Success)
                {
                    DialogResult dr = AKRSMessageBoxExt.Show(
                        @"产品传入点胶载台失败!                                
                                  重试: 重新推料
                                  忽略: 继续工作
                                  下一片:料仓上下一片料,
                                  终止: 停止工作，
                                  不再上料:不再上料",
                        $"报警",
                        new[] { "重试", "忽略", "下一片", "终止", "不再上料" },
                        new[]
                            {
                                DialogResult.Retry, DialogResult.Ignore, DialogResult.Yes, DialogResult.Abort,
                                DialogResult.No
                            },
                        AlarmLevel.FirstLevel);

                    switch (dr)
                    {
                        case DialogResult.Retry:

                            // 清错上使能
                            this.LoaderBinController.PushRodResetError();
                            Thread.Sleep(50);

                            this.LoaderBinController.PushRodPushRodServoOn();
                            Thread.Sleep(50);

                            goto RePush;

                        case DialogResult.Ignore:
                            break;

                        case DialogResult.Yes:

                            // 下一片
                            return ExcuteResult.None;

                        case DialogResult.Abort:
                            // 停止
                            return ExcuteResult.Abort;

                        case DialogResult.No:

                            // 不再上料
                            TransportProvider.ContinuousFeeding = false;

                            return ExcuteResult.NoStart;
                    }
                }

                // 循环判断 推料状态
                while (true)
                {
                    // 如果点胶载台入料口感应到料了
                    bool hasMaterialInFeedingInlet = this.DispenseSubSectionController.HasMaterialInFeedingInlet();
                    if (hasMaterialInFeedingInlet)
                    {
                        break;
                    }

                    // 如果等推杆到位 点胶入料口还没感应到 则报警推料失败 
                    if (this.LoaderBinController.IsPushRodInPosition())
                    {
                        this.LoaderBinController.MovePushRodHome();

                        DialogResult dr = AKRSMessageBoxExt.Show(
                            @"产品传入点胶载台失败!                                
                                  重试: 重新推料
                                  忽略: 继续工作
                                  下一片:料仓上下一片料,
                                  终止: 停止工作，
                                  不再上料:不再上料",
                            $"报警",
                            new[] { "重试", "忽略", "下一片", "终止", "不再上料" },
                            new[]
                                {
                                    DialogResult.Retry, DialogResult.Ignore, DialogResult.Yes, DialogResult.Abort,
                                    DialogResult.No
                                },
                            AlarmLevel.FirstLevel);

                        switch (dr)
                        {
                            case DialogResult.Retry:

                                // 清错上使能
                                this.LoaderBinController.PushRodResetError();
                                Thread.Sleep(50);

                                this.LoaderBinController.PushRodPushRodServoOn();
                                Thread.Sleep(50);

                                goto RePush;

                            case DialogResult.Ignore:
                                break;

                            case DialogResult.Yes:

                                // 下一片
                                return ExcuteResult.None;

                            case DialogResult.Abort:
                                // 停止
                                return ExcuteResult.Abort;

                            case DialogResult.No:

                                // 不再上料
                                TransportProvider.ContinuousFeeding = false;

                                return ExcuteResult.NoStart;
                        }
                    }
                }

            //Recheck:

            /*
            // 如果入料口无料则推料失败
            if (this.DispenseSubSectionController.HasMaterialInFeedingInlet() == false)
            {
                // 报警：入料卡料
                DialogResult dr = AKRSMessageBoxExt.Show(
                    @"The material get stuck on the loader  bin
                          Click
                          Retry : Repush  material
                          Abort: Stop  work ",
                    $"Warning",
                    new[] { "Retry" , "Abort" },
                    new[] { DialogResult.Retry, DialogResult.Abort });

                switch (dr)
                {
                    case DialogResult.Retry:
                        goto RePush;

                    case DialogResult.Abort:
                        return ExcuteResult.Abort;
                }
            }*/

            // 到这里说明推料成功， 点胶入料检测到了 下面点胶皮带开始传动， 此时推杆还是在一直往前推

            ReTransfer:

                // 开启一个线程 
                Task movePushRodHomeTask = Task.Run(() =>
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();

                    // 如果推到位了 推杆自己缩回去， 也不用管当前的推杆在什么位置
                    while (true)
                    {
                        if (this.LoaderBinController.IsPushRodInPosition())
                        {
                            this.LoaderBinController.MovePushRodHome();
                            break;
                        }

                        if (stopwatch.ElapsedMilliseconds > 10000)
                        {
                            break;
                        }
                    }
                });

                // 修改点胶传料状态
                this.TransportProgram.DispenseSubSectionProgram.SubSectionTransferState =
                    SubSectionStateTransferEnum.Transfering;

                // 点胶皮带 转动并等待到位
                this.DispenseSubSectionController.MoveBeltWaitArrive();
                Thread.Sleep(200);

                // 如果入料口还是感应到载具，则认为传送到点胶载台失败
                if (this.DispenseSubSectionController.HasMaterialInFeedingInlet())
                {
                    this.TransportProgram.DispenseSubSectionProgram.SubSectionState =
                        SubSectionStateEnum.NoMaterial;

                    // this.DispenseSubSectionController.BlockCylinder1DownAndDelay(50);

                    this.TransportProgram.DispenseSubSectionProgram.SubSectionTransferState =
                        SubSectionStateTransferEnum.Alarm;

                    DialogResult dr = AKRSMessageBoxExt.Show(
                        @"点胶载台卡料!                                
                                  重试: 重新推料
                                  终止: 停止工作，
                                  忽略: 继续工作
                                  不再上料:不再上料",
                        $"报警",
                        new[] { "重试", "终止", "忽略", "不再上料" },
                        new[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore, DialogResult.No });

                    switch (dr)
                    {
                        case DialogResult.Abort:
                            // 停止
                            return ExcuteResult.Abort;

                        case DialogResult.Retry:
                            goto RePush;

                        case DialogResult.Ignore:
                            break;

                        case DialogResult.No:

                            // 不再上料
                            TransportProvider.ContinuousFeeding = false;

                            return ExcuteResult.NoStart;
                    }
                }
                else
                {
                    // 传输成功
                    // 顶升气缸降落
                    this.DispenseSubSectionController.Clamp(true);

                    // 点胶段生成一个Transport Unit 对象
                    if (this.TransportProgram.DispenseSubSectionProgram.TransportUnit == null)
                    {
                        this.TransportProgram.DispenseSubSectionProgram.TransportUnit =
                            new TransportUnitSystem.Module.Matter.TransportUnit(CurrentMachineSystemEnum.System1);

                        this.TransportProgram.Save();
                    }

                    if (this.TransportProgram.DispenseSubSectionProgram.TransportUnit == null)
                    {
                        Machine.GetInstance().Stop();

                        throw new Exception("create transport unit fail");
                    }

                    this.TransportProgram.DispenseSubSectionProgram.TransportUnit.Refresh();

                    this.TransportProgram.DispenseSubSectionProgram.SubSectionState =
                        SubSectionStateEnum.HasMaterial;

                    this.TransportProgram.LoadingSubSectionProgram.TransportUnit = null;

                    // 修改记忆状态
                    this.TransportProgram.LoadingSubSectionProgram.SubSectionState =
                        SubSectionStateEnum.NoMaterial;

                    this.TransportProgram.LoadingSubSectionProgram.SubSectionTransferState =
                        SubSectionStateTransferEnum.Ready;
                    this.TransportProgram.DispenseSubSectionProgram.SubSectionTransferState =
                        SubSectionStateTransferEnum.Ready;

                    this.TransportProgram.DispenseSubSectionProgram.IsTuInDispense1 = true;

                    // 等待推杆缩回
                    movePushRodHomeTask.Wait();
                }

                return ExcuteResult.Success;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"产品传入点胶载台出现异常", ex, LogCategory.Transport);

                AKRSXtraMessageBox.Show(
                    $@"产品传入点胶载台出现异常 : \r\n
                                          Exception message:{ex.Message} \r\n
                                          StackTrace: {ex.StackTrace}",
                    "异常",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return ExcuteResult.Exception;
            }
        }

        /// <summary>
        /// 点胶1传到点胶2位置
        /// </summary>
        /// <param name="isManual">是否手动</param>
        /// <returns>是否成功</returns>
        public ExcuteResult TransferDispense1ToDispense2(bool isManual = false)
        {
            // 如果Dispense基板不是空闲状态
            if (this.TransportProgram.DispenseSubSectionProgram.SubSectionState !=
                SubSectionStateEnum.HasMaterial)
            {
                // 如果不满足条件
                return ExcuteResult.NoStart;
            }

            // 如果不是手动调试模式
            if (isManual == false)
            {
                // 等点胶线程给信号
                bool rt = SignalPool.GetInstance().AllowDispense1TransferToDispense2Signal.Wait();
                if (rt == false)
                {
                    // 如果点了停止 则退出
                    return ExcuteResult.Abort;
                }
            }
            else
            {
                if (ProductConfiguration.GetInstance().TransportUnitConfig.SubstrateProcessing == TransportUnitSystem.Model.SubstrateProcessingEnum.Divide)
                {
                    if (!this.TransportProgram.DispenseSubSectionProgram.IsTuInDispense1)
                    {
                        return ExcuteResult.NoStart;
                    }
                }
                else
                {
                    return ExcuteResult.NoStart;
                }
            }


            LogHelper.Post(Level.Info, $"点胶流道收到信号可以传送到点胶2", LogCategory.Transport);
            this.TransportProgram.DispenseSubSectionProgram.SubSectionTransferState =
                    SubSectionStateTransferEnum.Transfering;

            // 轨道顶升气缸顶起
            this.DispenseSubSectionController.UnClamp();

            // 点胶挡料1气缸降下 点胶挡料2气缸升起
            this.DispenseSubSectionController.BlockCylinder1DownAndDelay(50);
            this.DispenseSubSectionController.BlockCylinder2UpAndDelay(50);

            // 皮带转动
            this.DispenseSubSectionController.MoveBeltWaitArrive();

            Thread.Sleep(200);

            // 轨道顶升气缸降下
            this.DispenseSubSectionController.Clamp();

            // 状态改成有料
            this.TransportProgram.DispenseSubSectionProgram.SubSectionState =
                SubSectionStateEnum.HasMaterial;

            this.TransportProgram.DispenseSubSectionProgram.SubSectionTransferState =
                  SubSectionStateTransferEnum.Ready;

            // 将载具放在第二段
            this.TransportProgram.DispenseSubSectionProgram.IsTuInDispense1 = false;

            // 坐标系发生改变
            this.TransportProgram.DispenseSubSectionProgram.TransportUnit.System1MoveDistance();

            TransportDomain.GetInstance().TransportController.transportProgram.DispenseSubSectionProgram.TransportUnit
                        .TransportUnitInfo.IsNeedPreDispense = true;

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 点胶传到Bond
        /// </summary>
        /// <param name="isManual">是否手动</param>
        /// <returns> 执行结果 NoStart: 不满足条件， 进入下一个循环， Success：成功， Abort : 退出 </returns>
        public ExcuteResult TransferDispenseToBond(bool isManual = false)
        {
            try
            {
                // 如果Bond基板不是空闲状态
                if (this.TransportProgram.BondSubSectionProgram.SubSectionState !=
                    SubSectionStateEnum.NoMaterial ||
                    this.TransportProgram.DispenseSubSectionProgram.SubSectionState !=
                    SubSectionStateEnum.HasMaterial)
                {
                    // 如果不满足条件
                    return ExcuteResult.NoStart;
                }

                TransportProvider.RecordTime("流道调试", "检测到Bond有料");

                // 如果不是手动调试模式
                if (isManual == false)
                {
                    // 等点胶线程给信号
                    bool rt = SignalPool.GetInstance().AllowDispenseTransferToBondSignal.Wait();
                    if (rt == false)
                    {
                        // 如果点了停止 则退出
                        return ExcuteResult.Abort;
                    }
                }
                else
                {
                    if (ProductConfiguration.GetInstance().TransportUnitConfig.SubstrateProcessing == TransportUnitSystem.Model.SubstrateProcessingEnum.Divide)
                    {
                        if (this.TransportProgram.DispenseSubSectionProgram.IsTuInDispense1)
                        {
                            return ExcuteResult.NoStart;
                        }
                    }
                }

            retry:
                TransportProvider.RecordTime("流道调试", "点胶流道收到信号可以传送到Bond");
                this.TransportProgram.DispenseSubSectionProgram.SubSectionTransferState =
                        SubSectionStateTransferEnum.Transfering;

                List<Task> tasks = new List<Task>();

                if (MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
                {
                    tasks.Add(
                        Task.Run(
                            () =>
                                {
                                    // 轨道顶升气缸顶起
                                    this.DispenseSubSectionController.UnClamp();
                                }));
                }

                tasks.Add(Task.Run(
                    () =>
                        {
                            // 轨道顶升气缸顶起
                            this.BondSubSectionController.UnClamp();
                        }));


                tasks.Add(Task.Run(
                    () =>
                        {
                            // 点胶挡料气缸降下
                            this.DispenseSubSectionController.BlockCylinder1DownAndDelay(10);
                        }));

                tasks.Add(Task.Run(
                    () =>
                        {
                            // 点胶挡料气缸降下
                            this.DispenseSubSectionController.BlockCylinder2DownAndDelay(10);
                        }));

                tasks.Add(Task.Run(
                    () =>
                        {
                            // Bond 挡料气缸升起
                            this.BondSubSectionController.BlockCylinderUpAndDelay(10);
                        }));


                // 等待所有任务完成
                Task.WaitAll(tasks.ToArray());

                TransportProvider.RecordTime("流道调试", "所有气缸动作完成，点胶皮带开始传动");

                // 皮带转动
                this.DispenseSubSectionController.MoveBeltNoWaitArrive();

                Thread.Sleep(200);

                // 入料口是否有料
                bool hasMaterialInOutlet = false;
                while (this.DispenseSubSectionController.IsMoving())
                {
                    // 出料料检测（传感器D）
                    hasMaterialInOutlet =
                        this.DispenseSubSectionController.HasMaterialInOutlet();

                    // 如果 D 感应到了 则跳出循环
                    if (hasMaterialInOutlet)
                    {
                        break;
                    }

                    Thread.Sleep(300);
                }

                // 皮带转完了 出料传感器 D还没有感应到料， 则报警
                if (hasMaterialInOutlet == false)
                {
                    this.TransportProgram.DispenseSubSectionProgram.SubSectionTransferState =
                        SubSectionStateTransferEnum.Alarm;

                    // 如果感应到，则报警：上料台入料卡料
                    DialogResult dr = AKRSMessageBoxExt.Show(
                        @"点胶载台卡料
                          点击
                         重试: 重新上料
                         终止：停止工作
                         忽略: 忽略此报警",
                        $"报警",
                        new[] { "重试", "终止", "忽略" },
                        new[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore });

                    switch (dr)
                    {
                        case DialogResult.Retry:
                            goto retry;

                        case DialogResult.Abort:
                            // 停止
                            return ExcuteResult.Abort;

                        case DialogResult.Ignore:
                            // 停止
                            break;
                    }
                }
                else
                {
                    this.TransportProgram.BondSubSectionProgram.SubSectionTransferState =
                        SubSectionStateTransferEnum.Transfering;

                    // bond 皮带转动  并等待转完
                    this.BondSubSectionController.MoveBeltWaitArrive();

                    hasMaterialInOutlet =
                        this.DispenseSubSectionController.HasMaterialInOutlet();

                    // 如果 D 感应到了 报警
                    if (hasMaterialInOutlet)
                    {
                        this.TransportProgram.BondSubSectionProgram.SubSectionTransferState =
                            SubSectionStateTransferEnum.Alarm;

                        // 如果B 感应到，则报警：上料台入料卡料
                        DialogResult dr = AKRSMessageBoxExt.Show(
                            @"Bond载台卡料
                          点击
                         重试: 重新上料
                         终止：停止工作
                         忽略: 忽略此报警",
                            $"报警",
                            new[] { "重试", "终止", "忽略" },
                            new[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore });

                        switch (dr)
                        {
                            case DialogResult.Retry:
                                goto retry;

                            case DialogResult.Abort:
                                // 停止
                                return ExcuteResult.Abort;

                            case DialogResult.Ignore:
                                break;
                        }
                    }
                }

                tasks.Clear();

                tasks.Add(Task.Run(
                    () =>
                        {
                            // 轨道顶升气缸降下
                            // this.TransportModule.DispenseSubSectionModule.TrackLiftCylinderUp(50);
                            this.BondSubSectionController.Clamp();
                        }));

                tasks.Add(Task.Run(
                    () =>
                        {
                            this.DispenseSubSectionController.BlockCylinder1UpAndDelay(10);
                        }));

                tasks.Add(Task.Run(
                    () =>
                        {
                            this.DispenseSubSectionController.BlockCylinder2UpAndDelay(10);
                        }));

                // 等待所有任务完成
                Task.WaitAll(tasks.ToArray());

                this.TransportProgram.BondSubSectionProgram.TransportUnit =
                    this.TransportProgram.DispenseSubSectionProgram.TransportUnit;

                if (this.TransportProgram.BondSubSectionProgram.TransportUnit == null && isManual)
                {
                    this.TransportProgram.BondSubSectionProgram.TransportUnit = new TransportUnit(CurrentMachineSystemEnum.System2);
                }

                this.TransportProgram.BondSubSectionProgram.TransportUnit.Refresh();

                this.TransportProgram.DispenseSubSectionProgram.TransportUnit = null;

                // 状态改成有料
                this.TransportProgram.BondSubSectionProgram.SubSectionState =
                    SubSectionStateEnum.HasMaterial;

                this.TransportProgram.DispenseSubSectionProgram.TransportUnit = null;
                this.TransportProgram.DispenseSubSectionProgram.SubSectionState =
                    SubSectionStateEnum.NoMaterial;

                this.TransportProgram.DispenseSubSectionProgram.SubSectionTransferState =
                SubSectionStateTransferEnum.Ready;
                this.TransportProgram.BondSubSectionProgram.SubSectionTransferState =
                SubSectionStateTransferEnum.Ready;

                this.TransportProgram.Save();
                return ExcuteResult.Success;
            }
            catch (Exception ex)
            {
                LogHelper.Post(
                    Level.Error,
                    $"产品传入Bond载台出现异常",
                    ex,
                    LogCategory.Transport);

                AKRSXtraMessageBox.Show(
                    $@"产品传入Bond载台出现异常: \r\n
                                          Exception message:{ex.Message} \r\n
                                          StackTrace: {ex.StackTrace}",
                    "Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return ExcuteResult.Exception;
            }
        }

        /// <summary>
        /// Bond传到等待区域
        /// </summary>
        /// <param name="isManual">是否手动</param>
        /// <returns> 执行结果 NoStart: 不满足条件， 进入下一个循环， Success：成功， Abort : 退出 </returns>
        public ExcuteResult TransferBondToWaitingUnload(bool isManual = false)
        {
            try
            {
                // 如果Bond区是空闲状态
                if (this.TransportProgram.BondSubSectionProgram.SubSectionState !=
                    SubSectionStateEnum.HasMaterial)
                {
                    return ExcuteResult.NoStart;
                }

                TransportProvider.RecordTime("Bond传到等待区域线程", "Bond区为有料状态");

                // 如果等待区不是空闲状态
                if (this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionState !=
                    SubSectionStateEnum.NoMaterial)
                {
                    return ExcuteResult.NoStart;
                }

                TransportProvider.RecordTime("Bond传到等待区域线程", "等待段为无料状态");

                // 如果不是手动调试模式
                if (isManual == false)
                {
                    // 等Bond线程给信号
                    bool rt = SignalPool.GetInstance().AllowBondTransferToWaitingUnloadSignal.Wait();

                    if (rt == false)
                    {
                        // 如果点了停止 则退出
                        return ExcuteResult.Abort;
                    }

                    TransportProvider.RecordTime("Bond传到等待区域线程", "流道接收到Bond线程信号");
                }

            retry1:

                this.TransportProgram.BondSubSectionProgram.SubSectionTransferState = SubSectionStateTransferEnum.Transfering;

                List<Task> tasks = new List<Task>();

                tasks.Add(
                    Task.Run(
                        () =>
                            {
                                // 轨道顶升气缸顶起
                                this.BondSubSectionController.UnClamp();
                            }));

                tasks.Add(Task.Run(
                    () =>
                        {
                            // Bond挡料气缸降下
                            this.BondSubSectionController.BlockCylinderDownAndDelay(10);
                        }));

                // 等待所有任务完成
                Task.WaitAll(tasks.ToArray());

                TransportProvider.RecordTime("Bond传到等待区域线程", "气缸动作完成");

                // 皮带转动
                this.BondSubSectionController.MoveBeltNoWaitArrive();

                TransportProvider.RecordTime("Bond传到等待区域线程", "固晶段皮带开始转动");

                Thread.Sleep(100);

                // 出料口是否有料
                bool hasMaterialInOutlet = false;
                while (this.BondSubSectionController.IsMoving())
                {
                    // 出料料检测（传感器E）
                    hasMaterialInOutlet =
                        this.BondSubSectionController.HasMaterialInOutlet();

                    // 如果 D 感应到了 则跳出循环
                    if (hasMaterialInOutlet)
                    {
                        TransportProvider.RecordTime("Bond传到等待区域线程", "固晶段出料端感应到有料");
                        break;
                    }

                    Thread.Sleep(50);
                }

                // 皮带转完了 出料传感器 E还没有感应到料， 则报警
                if (hasMaterialInOutlet == false)
                {
                    this.TransportProgram.BondSubSectionProgram.SubSectionTransferState = SubSectionStateTransferEnum.Alarm;

                    TransportProvider.RecordTime("Bond传到等待区域线程", "Bond载台卡料报警");

                    // 如果E 感应到，则报警：上料台入料卡料
                    DialogResult dr = AKRSMessageBoxExt.Show(
                        @"Bond载台卡料
                          点击
                         重试: 重新上料
                         终止：停止工作
                         忽略: 忽略此报警",
                        $"报警",
                        new[] { "重试", "终止", "忽略" },
                        new[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore });

                    switch (dr)
                    {
                        case DialogResult.Retry:
                            goto retry1;

                        case DialogResult.Abort:
                            // 停止
                            return ExcuteResult.Abort;

                        case DialogResult.Ignore:
                            break;
                    }
                }
                else
                {
                retry2:
                    // Bond 出料口传感器感应到了 准备传送
                    this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionTransferState =
                        SubSectionStateTransferEnum.Transfering;

                    // waiting unload 皮带转动 
                    // 按时间传送， 时间配置在参数里
                    this.WaitingUnloadSubSectionController.MoveBeltNoWaitArrive();

                    TransportProvider.RecordTime("Bond传到等待区域线程", "下料等待皮带开始传动");

                    bool hasMaterialInFeedingInlet = false;

                    while (this.WaitingUnloadSubSectionController.IsMoving())
                    {
                        // Bond段出料传感器（传感器E）
                        hasMaterialInOutlet =
                            this.BondSubSectionController.HasMaterialInOutlet();

                        if (hasMaterialInOutlet == false)
                        {
                            TransportProvider.RecordTime("Bond传到等待区域线程", " Bond段出料传感器感应到无料");

                            Thread.Sleep(50);


                            // 这里如果提前设置状态会导致等待段皮带还没停就再次发送运动指令
                            // 运输单元/产品 传送到下料等待载台
                            //this.TransportProgram.WaitingUnloadSubSectionProgram.TransportUnit =
                            //    this.TransportProgram.BondSubSectionProgram.TransportUnit;
                            //this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionState =
                            //    SubSectionStateEnum.HasMaterial;
                            //this.TransportProgram.WaitingUnloadSubSectionProgram.TransportUnit?.Refresh();

                            //TransportProvider.RecordTime("Bond传到等待区域线程", " TransportUnit刷新完成");

                            //// 一旦流出Bond载台就设置为无料
                            //this.TransportProgram.BondSubSectionProgram.SubSectionState =
                            //    SubSectionStateEnum.NoMaterial;
                            //this.TransportProgram.BondSubSectionProgram.TransportUnit = null;

                            //TransportProvider.RecordTime("Bond传到等待区域线程", " Bond载台状态设置完成");
                        }

                        // 物料检测（传感器F）
                        hasMaterialInFeedingInlet =
                            this.UnloadingSubSectionController.HasMaterialInFeedingInlet();

                        // 如果 F 感应到了 则皮带停止运动，跳出循环
                        if (hasMaterialInFeedingInlet)
                        {
                            TransportProvider.RecordTime("Bond传到等待区域线程", " 等待段载台入料区感应到物料");

                            // Bond皮带停止
                            this.BondSubSectionController.StopMove();

                            // 未配置上下料 两段皮带停止
                            this.WaitingUnloadSubSectionController.StopMove();

                            TransportProvider.RecordTime("Bond传到等待区域线程", " Bond皮带和等待段皮带停止传动");
                            break;
                        }

                        Thread.Sleep(50);
                    }

                    // 查看出料等待区 出料口有没有料 传感器 E
                    bool hasMaterialInBondOutlet =
                        this.BondSubSectionController.HasMaterialInOutlet();

                    // 没有感应到过 
                    // 如果 E 感应到 或者F 没感应到 报警
                    if (hasMaterialInBondOutlet == true || hasMaterialInFeedingInlet == false)
                    {
                        //this.TransportProgram.BondSubSectionProgram.SubSectionTransferState =
                        //    SubSectionStateTransferEnum.Alarm;
                        this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionTransferState =
                            SubSectionStateTransferEnum.Alarm;

                        TransportProvider.RecordTime("Bond传到等待区域线程", " 出料等待载台卡料报警");

                        // 如果E 感应到，则报警：上料台入料卡料
                        DialogResult dr = AKRSMessageBoxExt.Show(
                            @"出料等待载台卡料
                          点击
                         重试: 重新上料
                         终止：停止工作
                         忽略: 忽略此报警",
                            $"报警",
                            new[] { "重试", "终止", "忽略" },
                            new[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore });

                        switch (dr)
                        {
                            case DialogResult.Retry:

                                // 这里只转等待段皮带
                                goto retry2;

                            case DialogResult.Abort:
                                // 停止
                                return ExcuteResult.Abort;

                            case DialogResult.Ignore:
                                break;
                        }
                    }
                }

                #region 判断传输完成移动这里 2025年11月17日

                // 运输单元/产品 传送到下料等待载台
                this.TransportProgram.WaitingUnloadSubSectionProgram.TransportUnit =
                    this.TransportProgram.BondSubSectionProgram.TransportUnit;
                this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionState =
                    SubSectionStateEnum.HasMaterial;
                this.TransportProgram.WaitingUnloadSubSectionProgram.TransportUnit?.Refresh();

                this.TransportProgram.BondSubSectionProgram.SubSectionState =
                    SubSectionStateEnum.NoMaterial;
                this.TransportProgram.BondSubSectionProgram.TransportUnit = null;

                TransportProvider.RecordTime("Bond传到等待区域线程", " Bond载台状态设置完成");
                #endregion

                this.TransportProgram.BondSubSectionProgram.SubSectionTransferState = SubSectionStateTransferEnum.Ready;
                this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionTransferState =
                    SubSectionStateTransferEnum.Ready;

                if (isManual)
                {
                    // 轨道顶升气缸降下
                    this.BondSubSectionController.UnClamp();
                }

                this.TransportProgram.Save();

                TransportProvider.RecordTime("Bond传到等待区域线程", " 程式保存完成");
                return ExcuteResult.Success;
            }
            catch (Exception ex)
            {
                LogHelper.Post(
                    Level.Error,
                    $"产品从Bond载台传入下料等待载台出现异常",
                    ex,
                    LogCategory.Transport);

                AKRSXtraMessageBox.Show(
                    $@"产品从Bond载台传入下料等待载台出现异常: \r\n
                                          Exception message:{ex.Message} \r\n
                                          StackTrace: {ex.StackTrace}",
                    "Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return ExcuteResult.Exception;
            }
        }

        /// <summary>
        /// 等待区域传到出料区
        /// </summary>
        /// <returns> 执行结果 NoStart: 不满足条件， 进入下一个循环， Success：成功， Abort : 退出 </returns>
        public ExcuteResult TransferWaitingUnloadToUnloadBelt()
        {
            SubSectionStateEnum stateTemp = this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionState;

            try
            {
                // 如果下料等待区没有料
                if (this.WaitingUnloadSubSectionController.HasMaterialInModule() == false)
                {
                    return ExcuteResult.NoStart;
                }

                TransportProvider.RecordTime("等待区传入出料区线程", "等待区感应有料");

                if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.Online || MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.OnlineWithAutoUnloader)
                {
                    // 如果下料区不是空闲状态，通过传感器判断
                    if (this.UnloadingSubSectionController.HasMaterialInModule())
                    {
                        return ExcuteResult.NoStart;
                    }

                    TransportProvider.RecordTime("等待区传入出料区线程", "下料无料（传感器判断）");
                }

                // 如果下料区不是空闲状态
                if (this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionState ==
                    SubSectionStateEnum.NoMaterial)
                {
                    return ExcuteResult.NoStart;
                }

                TransportProvider.RecordTime("等待区传入出料区线程", "下料无料（状态判断）");

                if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.Online)
                {
                    if (this.TransportProgram.UnloadingSubSectionProgram.SubSectionTransferState ==
                        SubSectionStateTransferEnum.Transfering || this.TransportProgram.UnloadingSubSectionProgram.SubSectionState ==
                    SubSectionStateEnum.HasMaterial)
                    {
                        // 等下料段流完
                        return ExcuteResult.NoStart;
                    }

                    TransportProvider.RecordTime("等待区传入出料区线程", "下料区无料（状态判断）");
                }

            retry:
                this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionTransferState =
                    SubSectionStateTransferEnum.Transfering;

                TransportProvider.RecordTime("等待区传入出料区线程", $"等待区皮带this.WaitingUnloadSubSectionController.IsMoving();{this.WaitingUnloadSubSectionController.IsMoving().ToString()}");

                // 皮带转动
                this.WaitingUnloadSubSectionController.MoveBeltNoWaitArrive();

                TransportProvider.RecordTime("等待区传入出料区线程", "等待区皮带开始转动");
                Thread.Sleep(200);

                // 传感器G
                bool hasMaterialInFeedingInletUnload = false;

                while (this.WaitingUnloadSubSectionController.IsMoving())
                {
                    hasMaterialInFeedingInletUnload =
                           this.UnloadingSubSectionController.HasMaterialInFeedingInlet();

                    // 传感器G感应到了 
                    if (hasMaterialInFeedingInletUnload)
                    {
                        TransportProvider.RecordTime("等待区传入出料区线程", "检测到等待区入口有料");
                        break;
                    }

                    Thread.Sleep(300);
                }

                // 皮带转完了 出料传感器 G还没有感应到料， 则报警 送料失败
                if (hasMaterialInFeedingInletUnload == false)
                {
                    this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionTransferState =
                        SubSectionStateTransferEnum.Alarm;

                    DialogResult dr = AKRSMessageBoxExt.Show(
                        @"下料载台卡料
                          点击
                         重试: 重新上料
                         终止：停止工作
                         忽略: 忽略此报警",
                        $"报警",
                        new[] { "重试", "终止", "忽略" },
                        new[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore });

                    switch (dr)
                    {
                        case DialogResult.Retry:
                            goto retry;

                        case DialogResult.Abort:
                            // 停止
                            return ExcuteResult.Abort;

                        case DialogResult.Ignore:
                            break;
                    }
                }
                else
                {
                    // 出料等待区出口传感器G感应到了
                    // unloading 皮带转动 
                    this.TransportProgram.UnloadingSubSectionProgram.SubSectionTransferState =
                        SubSectionStateTransferEnum.Transfering;

                    this.UnloadingSubSectionController.MoveBeltNoWaitArrive();

                    TransportProvider.RecordTime("等待区传入出料区线程", "下料区皮带开始转动");

                    while (this.UnloadingSubSectionController.IsMoving())
                    {
                        hasMaterialInFeedingInletUnload =
                           this.UnloadingSubSectionController.HasMaterialInFeedingInlet();

                        // 下料区入料检测不到了， 停止
                        if (hasMaterialInFeedingInletUnload == false)
                        {
                            TransportProvider.RecordTime("等待区传入出料区线程", "等待区入口检测到无料");

                            Thread.Sleep(100);

                            if (!MachineHardwareConfiguration.GetInstance().IsUnloadingSubSectionContinueMove)
                            {
                                this.UnloadingSubSectionController.StopMove();
                            }

                            this.WaitingUnloadSubSectionController.StopMove();

                            TransportProvider.RecordTime("等待区传入出料区线程", "等待区皮带停止转动");
                        }
                    }

                    bool hasMaterialInUnloading;

                    // 查看unloading table 入料口有没有料 传感器 H
                    if (MachineHardwareConfiguration.GetInstance().LoadConfiguration != LoadConfigurationEnum.Belt)
                    {
                        // 为了区分1号机和其他机台，1号机没这个传感器
                        hasMaterialInUnloading =
                    this.UnloadingSubSectionController.HasMaterialInModule();
                    }
                    else
                    {
                        // 传感器 G
                        hasMaterialInFeedingInletUnload =
                            this.UnloadingSubSectionController.HasMaterialInFeedingInlet();
                    }

                    // 物料检测（传感器F）
                    bool hasMaterialInOutletWaitingUnload =
                        this.WaitingUnloadSubSectionController.HasMaterialInModule();

                    // 如果 G 感应到 或者H 没感应到 报警
                    if (hasMaterialInFeedingInletUnload == true || hasMaterialInOutletWaitingUnload == true)
                    {
                        TransportProvider.RecordTime("等待区传入出料区线程", $"等待区卡料报警，hasMaterialInFeedingInletUnload{hasMaterialInFeedingInletUnload},hasMaterialInOutletWaitingUnload{hasMaterialInOutletWaitingUnload}");
                        this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionTransferState =
                            SubSectionStateTransferEnum.Alarm;
                        this.TransportProgram.UnloadingSubSectionProgram.SubSectionTransferState =
                            SubSectionStateTransferEnum.Alarm;

                        // 等待下料区卡料
                        DialogResult dr = AKRSMessageBoxExt.Show(
                            @"下料等待载台卡料
                          点击
                         重试: 重新上料
                         终止：停止工作
                         忽略: 忽略此报警",
                            $"报警",
                            new[] { "重试", "终止", "忽略" },
                            new[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore });

                        switch (dr)
                        {
                            case DialogResult.Retry:
                                goto retry;

                            case DialogResult.Abort:
                                // 停止
                                return ExcuteResult.Abort;

                            case DialogResult.Ignore:
                                break;
                        }
                    }
                }

                this.TransportProgram.UnloadingSubSectionProgram.TransportUnit =
                        this.TransportProgram.WaitingUnloadSubSectionProgram.TransportUnit;

                this.TransportProgram.UnloadingSubSectionProgram.TransportUnit.Refresh();

                this.TransportProgram.WaitingUnloadSubSectionProgram.TransportUnit = null;

                // 状态改成有料
                this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionState =
                    SubSectionStateEnum.NoMaterial;
                this.TransportProgram.UnloadingSubSectionProgram.SubSectionState =
                    SubSectionStateEnum.HasMaterial;

                this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionTransferState =
                SubSectionStateTransferEnum.Ready;
                this.TransportProgram.UnloadingSubSectionProgram.SubSectionTransferState =
   SubSectionStateTransferEnum.Ready;

                TransportProvider.RecordTime("等待区传入出料区线程", "等待段和下料段状态设置完成");

                this.TransportProgram.Save();

                TransportProvider.RecordTime("等待区传入出料区线程", "程式保存");
                return ExcuteResult.Success;
            }
            catch (Exception ex)
            {
                this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionState = stateTemp;

                LogHelper.Post(
                    Level.Error,
                    $"产品从下料等待载台传入下料载台出现异常",
                    ex,
                    LogCategory.Transport);

                AKRSXtraMessageBox.Show(
                    $@"产品从下料等待载台传入下料载台出现异常: \r\n
                                          Exception message:{ex.Message} \r\n
                                          StackTrace: {ex.StackTrace}",
                    "Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return ExcuteResult.Exception;
            }
        }

        /// <summary>
        /// 等待区域传到出料仓
        /// </summary>
        /// <param name="isManual">是否手动</param>
        /// <returns> 执行结果 NoStart: 不满足条件， 进入下一个循环， Success：成功， Abort : 退出 </returns>
        public ExcuteResult TransferWaitingUnloadToUnloadBin(bool isManual = false)
        {
            SubSectionStateEnum stateTemp = this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionState;

            try
            {
                // 如果下料等待区没有料
                if (this.WaitingUnloadSubSectionController.HasMaterialInModule() == false)
                {
                    Thread.Sleep(500);
                    return ExcuteResult.NoStart;
                }

                // 如果下料等待区无料
                if (this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionState ==
                    SubSectionStateEnum.NoMaterial)
                {
                    Thread.Sleep(500);
                    return ExcuteResult.NoStart;
                }

                // 如果皮带还在转动， Bond 传到 WaitUnload Table 没有结束
                if (this.WaitingUnloadSubSectionController.IsMoving())
                {
                    Thread.Sleep(500);
                    return ExcuteResult.NoStart;
                }

                // 联机
                if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.Online || MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.OnlineWithAutoUnloader)
                {
                    // 如果下料区有料
                    if (this.TransportProgram.UnloadingSubSectionProgram.SubSectionState ==
                        SubSectionStateEnum.HasMaterial)
                    {
                        Thread.Sleep(500);
                        return ExcuteResult.NoStart;
                    }
                }

            retry:
                this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionTransferState =
                    SubSectionStateTransferEnum.Transfering;

                LogHelper.Post(Level.Info, $"出料等待段皮带开始转动", new Exception(), LogCategory.Transport);

                // 满足条件，准备传料， 皮带转动
                this.WaitingUnloadSubSectionController.MoveBeltNoWaitArrive();
                Thread.Sleep(200);

                // 检测出料等待区正上方
                bool hasMaterialInWaitingUnload = false;

                while (this.WaitingUnloadSubSectionController.IsMoving())
                {
                    hasMaterialInWaitingUnload = this.WaitingUnloadSubSectionController.HasMaterialInModule();

                    // 物料传感器从感应到变成未感应到  则启动推料
                    if (hasMaterialInWaitingUnload == false)
                    {
                        // this.WaitingUnloadSubSectionController.StopMove();
                        LogHelper.Post(Level.Info, $"物料传感器从感应到变成未感应到  启动推料", new Exception(), LogCategory.Transport);
                        break;
                    }

                    Thread.Sleep(300);
                }

                // 皮带转完了  物料感应器还是感应到  则传料卡住了
                if (hasMaterialInWaitingUnload == true)
                {
                    this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionTransferState =
                        SubSectionStateTransferEnum.Alarm;

                    LogHelper.Post(Level.Info, $"皮带转完了  物料感应器还是感应到  传料卡料", new Exception(), LogCategory.Transport);

                    DialogResult dr = AKRSMessageBoxExt.Show(
                        @"下料等待载台卡料
                          点击
                         重试: 重新上料
                         终止：停止工作
                         忽略: 忽略此报警",
                        $"报警",
                        new[] { "重试", "终止", "忽略" },
                        new[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore });

                    switch (dr)
                    {
                        case DialogResult.Retry:
                            goto retry;

                        case DialogResult.Abort:
                            // 停止
                            return ExcuteResult.Abort;

                        case DialogResult.Ignore:
                            break;
                    }
                }
                else
                {
                // 没有卡料  启动推料
                RePush:

                    // 推杆退出
                    ExcuteResult ret = this.UnLoaderBinController.MovePushRodHome();
                    LogHelper.Post(Level.Info, $"下料推杆缩回结束，准备推料", new Exception(), LogCategory.Transport);
                    ret = this.UnLoaderBinController.PushTabletToUnloader();

                    // 卡料
                    if (ret != ExcuteResult.Success)
                    {
                        DialogResult dr = AKRSMessageBoxExt.Show(
                            @"下料仓下料失败!
                              点击
                              重试 : 重新推料
                              忽略 : 继续工作
                              终止: 停止工作",
                            $"报警",
                            new[] { "重试", "忽略", "终止" },
                            new[] { DialogResult.Retry, DialogResult.Ignore, DialogResult.Abort },
                            AlarmLevel.FirstLevel);

                        switch (dr)
                        {
                            case DialogResult.Retry:
                                goto RePush;

                            case DialogResult.Ignore:
                                break;

                            case DialogResult.Abort:
                                // 停止
                                return ExcuteResult.Abort;
                        }
                    }

                    // 出料口物料检测（传感器F）
                    bool hasMaterialInOutletWaitingUnload =
                        this.WaitingUnloadSubSectionController.HasMaterialInModule();

                    // 如果 等待区感应到有载具
                    if (hasMaterialInOutletWaitingUnload == true)
                    {
                        this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionTransferState =
                            SubSectionStateTransferEnum.Alarm;
                        this.TransportProgram.UnloadingSubSectionProgram.SubSectionTransferState =
                            SubSectionStateTransferEnum.Alarm;

                        // 等待下料区卡料
                        DialogResult dr = AKRSMessageBoxExt.Show(
                            @"下料等待载台卡料
                          点击
                         重试: 重新上料
                         终止：停止工作
                         忽略: 忽略此报警",
                            $"报警",
                            new[] { "重试", "终止", "忽略" },
                            new[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore });

                        switch (dr)
                        {
                            case DialogResult.Retry:
                                // 推杆缩回
                                this.UnLoaderBinController.MovePushRodHome();
                                goto retry;

                            case DialogResult.Abort:
                                // 停止
                                return ExcuteResult.Abort;

                            case DialogResult.Ignore:
                                break;
                        }
                    }
                }

                // 推杆缩回
                this.UnLoaderBinController.MovePushRodHome();
                this.WaitingUnloadSubSectionController.StopMove();

                LogHelper.Post(Level.Info, $"下料推杆缩回完成，皮带停止传动", new Exception(), LogCategory.Transport);

                this.TransportProgram.WaitingUnloadSubSectionProgram.TransportUnit = null;

                // 修改状态
                this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionState =
                    SubSectionStateEnum.NoMaterial;
                this.TransportProgram.UnloadingSubSectionProgram.SubSectionState =
                    SubSectionStateEnum.HasMaterial;

                this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionTransferState =
                SubSectionStateTransferEnum.Ready;
                this.TransportProgram.UnloadingSubSectionProgram.SubSectionTransferState =
   SubSectionStateTransferEnum.Ready;

                // 刷新料片
                this.UnLoaderBinController.AddCurPlaceLayer();

                this.TransportProgram.Save();
                return ExcuteResult.Success;
            }
            catch (Exception ex)
            {
                this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionState = stateTemp;

                LogHelper.Post(
                    Level.Error,
                    $"产品从下料等待载台传到下料仓出现异常",
                    ex,
                    LogCategory.Transport);

                AKRSXtraMessageBox.Show(
                    $@"产品从下料等待载台传到下料仓出现异常: \r\n
                                          Exception message:{ex.Message} \r\n
                                          StackTrace: {ex.StackTrace}",
                    "异常",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return ExcuteResult.Exception;
            }
        }

        /// <summary>
        /// 下料段传到收板机
        /// </summary>
        /// <param name="isManual">是否手动</param>
        /// <returns> 执行结果 NoStart: 不满足条件， 进入下一个循环， Success：成功， Abort : 退出 </returns>
        public ExcuteResult TransferUnloadBeltToAutoUnloader(bool isManual = false)
        {
            try
            {
                // 如果下料段无料
                if (this.TransportProgram.UnloadingSubSectionProgram.SubSectionState ==
                    SubSectionStateEnum.NoMaterial || this.TransportProgram.UnloadingSubSectionProgram.SubSectionTransferState ==
             SubSectionStateTransferEnum.Transfering)
                {
                    // 如果不满足条件
                    return ExcuteResult.NoStart;
                }

                // 判断能否传料
                bool isNeedMaterial = this.UnloadingSubSectionController.GetAutoUnLoaderNeedMaterialSignal();

                if (isNeedMaterial == false)
                {
                    return ExcuteResult.NoStart;
                }

            //// 这个延时是防止收板机皮带没有传送完
            //Thread.Sleep(500);

            retry:

                this.TransportProgram.UnloadingSubSectionProgram.SubSectionTransferState =
                    SubSectionStateTransferEnum.Transfering;

                // 如果接收到要料信号就开始传料
                this.UnloadingSubSectionController.MoveBeltNoWaitArrive();
                Thread.Sleep(50);

                bool hasMaterialInOutlet = false;
                while (this.UnloadingSubSectionController.IsMoving())
                {
                    // 出料料检测
                    hasMaterialInOutlet =
                        this.UnloadingSubSectionController.HasMaterialInOutlet();

                    // 如果 感应到了 则跳出循环
                    if (hasMaterialInOutlet)
                    {
                        break;
                    }

                    Thread.Sleep(50);
                }

                // 转完了还没感应到就报警
                if (hasMaterialInOutlet == false)
                {
                    this.TransportProgram.UnloadingSubSectionProgram.SubSectionTransferState =
                           SubSectionStateTransferEnum.Alarm;

                    DialogResult dr = AKRSMessageBoxExt.Show(
                        @"下料载台卡料
                          点击
                         重试: 重新上料
                         终止：停止工作
                         忽略: 忽略此报警",
                        $"报警",
                        new[] { "重试", "终止", "忽略" },
                        new[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore });

                    switch (dr)
                    {
                        case DialogResult.Retry:
                            goto retry;

                        case DialogResult.Abort:
                            // 停止
                            return ExcuteResult.Abort;

                        case DialogResult.Ignore:
                            break;
                    }
                }
                else
                {
                    bool hasMaterial = true;
                    while (this.UnloadingSubSectionController.IsMoving())
                    {
                        hasMaterial =
                           this.UnloadingSubSectionController.HasMaterialInOutlet();

                        // 下料区入料检测不到了， 停止
                        if (hasMaterial == false)
                        {
                            Thread.Sleep(500);

                            this.UnloadingSubSectionController.StopMove();

                            break;
                        }
                    }

                    if (hasMaterial == true)
                    {
                        this.TransportProgram.UnloadingSubSectionProgram.SubSectionTransferState =
                          SubSectionStateTransferEnum.Alarm;

                        DialogResult dr = AKRSMessageBoxExt.Show(
                            @"下料载台卡料
                          点击
                         重试: 重新上料
                         终止：停止工作
                         忽略: 忽略此报警",
                            $"报警",
                            new[] { "重试", "终止", "忽略" },
                            new[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore });

                        switch (dr)
                        {
                            case DialogResult.Retry:
                                goto retry;

                            case DialogResult.Abort:
                                // 停止
                                return ExcuteResult.Abort;

                            case DialogResult.Ignore:
                                break;
                        }
                    }
                }

                this.TransportProgram.UnloadingSubSectionProgram.TransportUnit = null;

                // 状态改成无料
                this.TransportProgram.UnloadingSubSectionProgram.SubSectionState =
                    SubSectionStateEnum.NoMaterial;
                this.TransportProgram.UnloadingSubSectionProgram.SubSectionTransferState =
                    SubSectionStateTransferEnum.Ready;

                this.TransportProgram.Save();
                return ExcuteResult.Success;
            }
            catch (Exception ex)
            {
                LogHelper.Post(
                    Level.Error,
                    $"产品从下料载台传入收板机出现异常",
                    ex,
                    LogCategory.Transport);

                AKRSXtraMessageBox.Show(
                    $@"产品从下料载台传入收板机出现异常: \r\n
                                          Exception message:{ex.Message} \r\n
                                          StackTrace: {ex.StackTrace}",
                    "异常",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return ExcuteResult.Exception;
            }
        }

        /// <summary>
        /// 料仓移动前检查流道出入口是否有载具
        /// </summary>
        /// <returns>结果</returns>
        public bool CheckCarrierBeforeLoaderMove()
        {
            // 出料、入料口是否有载具
            bool hasMaterialInOutlet = false;
            bool hasMaterialInFeedinglet = false;

        Retry:
            // 出料口检测
            hasMaterialInOutlet =
                this.UnloadingSubSectionController.HasMaterialInFeedingInlet();

            if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.LoaderBin
                || MachineHardwareConfiguration.GetInstance().LoadConfiguration
                == LoadConfigurationEnum.OnlineWithAutoUnloader)
            {
                if (hasMaterialInOutlet)
                {
                    DialogResult dr = AKRSMessageBoxExt.Show(
                        @"出料口有载具，下料仓不允许移动!请先移除出料口的载具！
                              点击
                              重试 : 重新检测
                              终止: 停止工作",
                        $"提示",
                        new[] { "重试", "终止" },
                        new[] { DialogResult.Retry, DialogResult.Abort },
                        AlarmLevel.SecondLevel);

                    switch (dr)
                    {
                        case DialogResult.Retry:
                            goto Retry;

                        case DialogResult.Abort:
                            // 停止
                            return false;
                    }
                }
            }

            // 入料口检测
            hasMaterialInFeedinglet =
                this.DispenseSubSectionController.HasMaterialInFeedingInlet();

            if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.LoaderBin
                || MachineHardwareConfiguration.GetInstance().LoadConfiguration
                == LoadConfigurationEnum.OnlineWithAutoLoader)
            {
                if (hasMaterialInFeedinglet)
                {
                    DialogResult dr = AKRSMessageBoxExt.Show(
                        @"入料口有载具，上料仓不允许移动!请先移除入料口的载具！
                              点击
                              重试 : 重新检测
                              终止: 停止工作",
                        $"提示",
                        new[] { "重试", "终止" },
                        new[] { DialogResult.Retry, DialogResult.Abort },
                        AlarmLevel.SecondLevel);

                    switch (dr)
                    {
                        case DialogResult.Retry:
                            goto Retry;

                        case DialogResult.Abort:
                            // 停止
                            return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 获取硬件集合
        /// </summary>
        /// <returns>结果</returns>
        public List<string> GetHardWareNames()
        {
            List<string> list = new List<string>();

            if (!MachineHardwareConfiguration.GetInstance().IsTransportConfigured)
            {
                return list;
            }

            #region 上料区

            if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.Belt) 
            {
                list.Add("入料区有料检测右");
                list.Add("进料工作台X");
            }
            else if(MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.LoaderBin)
            {
                list.Add("上料Y");
                list.Add("上料Z");
                list.Add("上料推料");
                list.Add("前上料盒检测");
                list.Add("后上料盒检测");
            }
            else
            {
                list.Add("入料区入料检测（左）");
                list.Add("上游SMEMA要料信号");

                list.Add("入料区有料检测右");
                list.Add("进料工作台X");
            }

            #endregion

            #region 点胶区

            if (MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                list.Add("点胶工作台X");
                list.Add("点胶区入料检测1");
                list.Add("点胶区有料检测2");
                list.Add("固晶区入料检测1");
                list.Add("点胶区压料气缸电磁阀");
                list.Add("点胶区挡料气缸1电磁阀");
                list.Add("点胶区挡料气缸2电磁阀");

                list.Add("点胶区吸料真空电磁阀");
                list.Add("点胶区附加吸料真空");
                list.Add("点胶区压料气缸动点升起检测1");
                list.Add("点胶区压料气缸原点下降检测1");

              
                // 新设备没有
                //list.Add("点胶区压料气缸动点升起检测2");
                //list.Add("点胶区压料气缸原点下降检测2");
                //list.Add("点胶区压料气缸动点升起检测3");
                //list.Add("点胶区压料气缸原点下降检测3");
            }

            #endregion

            #region 固精区

            // 加热模块
            if (MachineHardwareConfiguration.GetInstance().IsTransportHeatConfigured)
            {
                list.Add("工作台加热");
                list.Add("加热器1");
            }

            // 大载板
            if (MachineHardwareConfiguration.GetInstance().IsTransportConfigured==false)
            {
                list.Add("工作台负压检测中");
                list.Add("工作台负压检测前");
                list.Add("工作台负压检测右");

                list.Add("工作台负压中电磁阀");
                list.Add("工作台负压前电磁阀");
                list.Add("工作台负压右电磁阀");
            }
            else
            {
                list.Add("固晶工作台X");
                list.Add("固晶区出料检测2");
                list.Add("固晶区挡料气缸电磁阀");
                list.Add("固晶区压料气缸电磁阀");
                list.Add("固晶区吸料真空电磁阀");
                list.Add("固晶区压料气缸动点升起检测1");
                list.Add("固晶区压料气缸原点下降检测1");

                // 新设备没有
                //list.Add("固晶区压料气缸动点升起检测2");
                //list.Add("固晶区压料气缸原点下降检测2");
                //list.Add("固晶区压料气缸动点升起检测3");
                //list.Add("固晶区压料气缸原点下降检测3");
            }

            #endregion

            #region 出料等待区

            list.Add("固晶出料等待区X");
            list.Add("出料等待区有料检测1");

            #endregion

            #region 下料区

            if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.Belt)
            {
                list.Add("下料区入料检测");
                list.Add("下料区有料检测");
                list.Add("出料工作台X");
            }
            else if(MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.LoaderBin)
            {
                list.Add("下料Y");
                list.Add("下料Z");
                list.Add("下料推料");
                list.Add("前下料盒检测");
                list.Add("后下料盒检测");
            }
            else
            {
                list.Add("下料区入料检测");
                list.Add("下料区有料检测");
                list.Add("出料工作台X");

                list.Add("下游Smema要料信号");
                list.Add("下料区后端物料检测");
            }

            #endregion

            return list;
        }
    }
}
