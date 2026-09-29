using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.Galaxy2.Machine.Models
{
    using System.Threading;

    using AKRS.Galaxy2.Machine.Enums;

    /// <summary>
    /// 线程的交互信号
    /// </summary>
    public class Signal
    {
        /// <summary>
        /// 信号名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 信号状态
        /// </summary>
        public SignalStateEnum State { get; set; } = SignalStateEnum.ReSet;

        /// <summary>
        /// 自动重置还是手动
        /// </summary>
        private EventResetMode resetMode { get; set; }

        /// <summary>
        /// 自动信号
        /// </summary>
        private EventWaitHandle signalEvent;

        /// <summary>
        /// 循环周期
        /// </summary>
        private int recycleTime = 5;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">信号名称</param>
        public Signal(string name) : this(name, 100, EventResetMode.AutoReset, false)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">信号名称</param>
        /// <param name="mode">
        ///    信号模式：
        ///      EventResetMode.AutoReset 创建AutoResetEvent
        ///      EventResetMode.ManualReset 创建ManualResetEvent
        /// </param>
        public Signal(string name, EventResetMode mode) : this(name, 100, mode, false)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">信号名称</param>
        /// <param name="recycleTime">循环周期</param>
        /// <param name="mode">
        ///    信号模式：
        ///      EventResetMode.AutoReset 创建AutoResetEvent
        ///      EventResetMode.ManualReset 创建ManualResetEvent
        /// </param>
        /// <param name="initialState">信号名称</param>
        public Signal(string name, int recycleTime, EventResetMode mode, bool initialState)
        {
            this.name = name;
            this.signalEvent = new EventWaitHandle(initialState, mode);
            this.recycleTime = recycleTime;
            this.resetMode = mode;
        }

        /// <summary>
        /// 发送信号
        /// </summary>
        /// <exception cref="InvalidOperationException">信号发送失败异常</exception>
        public void Set()
        {
            if (!this.signalEvent.Set())
            {
                throw new InvalidOperationException($"{this.name} 信号发送失败");
            }

            this.State = SignalStateEnum.Set;
        }

        /// <summary>
        /// 信号重置
        /// </summary>
        /// <exception cref="InvalidOperationException">信号发送失败异常</exception>
        public void ReSet()
        {
            if (!this.signalEvent.Reset())
            {
                throw new InvalidOperationException($"{this.name} 信号发送失败");
            }

            this.State = SignalStateEnum.ReSet;
        }

        /// <summary>
        /// 等待信号  如果设备处于暂停状态，循环阻塞接收信号，如果设备停止 则返回false
        /// 所以在启动的时候 需要先设置设备状态，再启动线程
        /// </summary>
        /// <returns>等待成功</returns>
        public bool Wait()
        {
            bool res = WaitStart();
             
            // 等于false 则点了停止按钮，否则是从暂停恢复到工作状态
            if (res == false) 
            { 
                return false; 
            }

            // 先判断设备状态，如果设备状态为停止，则不用判断信号量了
            do
            {
                if (MachineStateModel.GetInstance().MachineState == MachineStateEnum.Stop)
                {
                    return false;
                }

                Thread.Sleep(5);
            }
            while (!this.signalEvent.WaitOne(this.recycleTime));

            // 唐鹏添加
            if (this.resetMode == EventResetMode.AutoReset)
            {
                this.State = SignalStateEnum.ReSet;
            }

            return true;
        }

        /// <summary>
        /// 等待信号  如果设备处于暂停状态，循环阻塞接收信号，如果设备停止 则返回false
        /// 所以在启动的时候 需要先设置设备状态，再启动线程
        /// </summary>
        /// <param name="outTime">超时时间</param>
        /// <returns>结果</returns>
        public bool Wait(int outTime)
        {
            bool res = WaitStart();

            // 等于false 则点了停止按钮，否则是从暂停恢复到工作状态
            if (res == false)
            {
                return false;
            }

            if (MachineStateModel.GetInstance().MachineState == MachineStateEnum.Stop)
            {
                return false;
            }

            return this.signalEvent.WaitOne(this.recycleTime);
        }

        /// <summary>
        /// 等待单步信号  
        /// </summary>
        /// <returns>等待成功</returns>
        public bool WaitSingleStep()
        {
            do
            {
                // 如果不是单步模式，直接退出
                if (MachineStateModel.GetInstance().IsSingleStepWork == false) 
                {
                    return false;
                }

                Thread.Sleep(5);
            }
            while (!this.signalEvent.WaitOne(this.recycleTime));

            // 唐鹏添加
            if (this.resetMode == EventResetMode.AutoReset)
            {
                this.State = SignalStateEnum.ReSet;
            }

            return true;
        }

        /// <summary>
        ///  等待开始  用于暂停时候的阻塞
        /// </summary>
        /// <returns>结果</returns>
        public static bool WaitStart()
        {
            // 维护模式 和调试模式 直接返回true
            //if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.MaintenceWork 
            //    || MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.Debug)
            //{
            //   // return true;               
            //}

            if (MachineStateModel.GetInstance().MachineState == MachineStateEnum.Stop)
            {
                return false;
            }

            // 暂停状态 阻塞 
            while (MachineStateModel.GetInstance().MachineState == MachineStateEnum.Pause)
            {
                if (MachineStateModel.GetInstance().MachineState == MachineStateEnum.Stop)
                {
                    return false;
                }

                Thread.Sleep(20);
            }

            return true;
        }
    }
}
