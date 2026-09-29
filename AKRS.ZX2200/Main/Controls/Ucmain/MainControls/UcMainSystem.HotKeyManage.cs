using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Machine.Models;

namespace AKRS.ZX2200.Main.Controls.Ucmain.MainControls
{
    /// <summary>
    ///  用于管理热键
    /// </summary>
    public partial class UcMainSystem
    {
        /// <summary>
        /// 热键消息常量
        /// </summary>
        private const int WM_HOTKEY = 0x0312;

        /// <summary>
        /// 热键ID(切换单步模式)
        /// </summary>
        private const int ChangeSingleStepMode = 1;

        /// <summary>
        /// 热键ID(发送单步信号)
        /// </summary>
        private const int SendSingleStepSignal = 2;

        /// <summary>
        /// 修饰键常量（Alt）
        /// </summary>
        private const int MOD_ALT = 0x0001;

        /// <summary>
        /// 修饰键常量（CONTROL）
        /// </summary>
        private const int MOD_CONTROL = 0x0002;

        /// <summary>
        /// 修饰键常量（SHIFT）
        /// </summary>
        private const int MOD_SHIFT = 0x0004;

        /// <summary>
        /// 修饰键常量（WIN ）
        /// </summary>
        private const int MOD_WIN = 0x0008;

        /// <summary>
        /// 修饰键常量 - 无修饰键使用0
        /// </summary>
        private const int MOD_NONE = 0x0000;

        // 注册热键API
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        // 注销热键API
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// 注册全局热键
        /// </summary>
        private void RegisterGlobalHotkeys()
        {
            // 注册Shift+F9 用于撤销操作
            RegisterHotKey(this.Handle, ChangeSingleStepMode, MOD_SHIFT, (int)Keys.F9);

            // 注册Shift+F9 用于撤销操作
            RegisterHotKey(this.Handle, SendSingleStepSignal , MOD_NONE, (int)Keys.F9);
        }

        /// <summary>
        /// 注销全局热键
        /// 暂时不用，程序退出的时候会自动注销
        /// </summary>
        private void UnregisterGlobalHotkeys()
        {
            // 注销所有热键
            UnregisterHotKey(this.Handle, SendSingleStepSignal);
            UnregisterHotKey(this.Handle, ChangeSingleStepMode);
        }

        /// <summary>
        /// 用于处理热键消息
        /// </summary>
        /// <param name="m">消息</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                int id = m.WParam.ToInt32();

                switch (id)
                {
                    // 切换单步模式
                    case ChangeSingleStepMode:
                        MachineStateModel.GetInstance().IsSingleStepWork = !MachineStateModel.GetInstance().IsSingleStepWork;
                        break;

                    // 发送单步信号
                    case SendSingleStepSignal:
                        if (MachineStateModel.GetInstance().IsSingleStepWork)
                        {
                            SignalPool.GetInstance().System2SingleStepSignal.Set();
                        }

                        break;

                    default:
                        break;
                }
            }

            base.WndProc(ref m);
        }
    }
}
