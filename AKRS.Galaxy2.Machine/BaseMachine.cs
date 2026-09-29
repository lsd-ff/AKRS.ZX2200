using System;
using Newtonsoft.Json;

namespace AKRS.Galaxy2.Machine
{
    using System.Windows.Forms;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.MachineSupport;

    /// <summary>
    /// 设备底层上下文
    /// 基础设备模型，上层的设备必须继承此设备
    /// </summary>
    public class BaseMachine
    {
        /// <summary>
        /// 单颗工作的UI线程通知
        /// </summary>
        [JsonIgnore]
        private Action SigleWorkUIAction { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseMachine()
        {
            this.TraceSignal = new Signal("单步跟踪信号");
            this.SingleSignal = new Signal("单颗信号");
            this.PauseSignal = new Signal("暂停信号");
        }

        #region 信号及控制
        /// <summary>
        /// 单步跟踪信号
        /// </summary>
        [JsonIgnore]
        public Signal TraceSignal { get; set; }

        /// <summary>
        /// 设备暂停信号
        /// </summary>
        [JsonIgnore]
        public Signal PauseSignal { get; set; }

        /// <summary>
        /// 单颗信号
        /// </summary>
        [JsonIgnore]
        public Signal SingleSignal { get; set; }

        /// <summary>
        /// 是否是单步跟踪模式
        /// </summary>
        [JsonIgnore]
        private bool IsTraceMode => MachineStateModel.GetInstance().IsSingleStepWork;

        /// <summary>
        /// 单颗工作的通知
        /// </summary>
        /// <param name="action">通知Action</param>
        /// <param name="control">通知控件</param>
        public void SetSingleWorkAction(Action action, Control control)
        {
            this.SigleWorkUIAction = () => control.BeginInvoke(action);
        }

        public void ClearWorkAction()
        {
            this.SigleWorkUIAction = null;
        }

        /// <summary>
        /// 设置成单步跟踪模式
        /// </summary>
        public void TraceWork()
        {
            MachineStateModel.GetInstance().MachineState = MachineStateEnum.Working;
            MachineStateModel.GetInstance().IsSingleStepWork = true;
        }

        /// <summary>
        /// 单步等待
        /// </summary>
        /// <returns>是否等到信号</returns>
        public bool TraceWait()
        {
            if (this.IsTraceMode)
            {
                this.TraceSignal.Wait();
            }

            return true;
        }
        
        /// <summary>
        /// 下一步
        /// </summary>
        /// <returns>发送信号的结果</returns>
        /// <exception cref="InvalidOperationException">信号发送失败异常</exception>
        public bool NextStep()
        {
            if (this.IsTraceMode)
            {
                this.TraceSignal.Set();
            }

            return true;
        }

        /// <summary>
        /// 暂停工作
        /// </summary>
        public void PauseWork()
        {
            MachineStateModel.GetInstance().MachineState = MachineStateEnum.Pause;
        }

        /// <summary>
        /// 开始工作
        /// </summary>
        public void StartWork()
        {
            MachineStateModel.GetInstance().MachineState = MachineStateEnum.Working;
            //Alarmer.SetRunState();
        }

        /// <summary>
        /// 继续工作
        /// </summary>
        public void ContinueWork()
        {
            MachineStateModel.GetInstance().MachineState = MachineStateEnum.Working;
            this.PauseSignal.Set();
            //Alarmer.SetRunState();
        }

        /// <summary>
        /// 停止工作
        /// </summary>
        public void StopWork()
        {
            MachineStateModel.GetInstance().MachineState = MachineStateEnum.Stop;
            //MachineStateModeSingleton.GetInstance().MachineWorkMode = MachineWorkModeEnum.StepWork;
            this.ClearWorkAction();
            //Alarmer.SetStopState();
        }

        #endregion

        /// <summary>
        /// 是否退出软件
        /// </summary>
        [JsonIgnore]
        public bool IsExitApp { get; set; } 

        /// <summary>
        /// 程序退出时执行
        /// </summary>
        public void ExitApp()
        {
            MachineStateModel.GetInstance().MachineState = MachineStateEnum.Stop;
            RuntimeProvider.ThreadFlag = false;
            this.IsExitApp = true;
        }
    }
}
