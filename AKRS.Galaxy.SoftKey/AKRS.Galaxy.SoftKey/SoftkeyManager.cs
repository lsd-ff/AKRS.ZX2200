using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using USL.DRV.Key;
using System.Windows;
using log4net;
using System.Threading;
using System.Diagnostics;
using Authenticator;
using System.Windows.Forms;
using Sunny.UI;

namespace AKRS.Galaxy.SoftKey
{
    public class SoftkeyManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SoftkeyManager));

        public static bool CheckYtSoftKey()
        {
            bool success = Galaxy.SoftKey.SoftkeyManager.InitSoftKey();
            if (!success)
            {
                return false;
            }

            bool permanent = false;
            DateTime authDate = new DateTime();
            success = Galaxy.SoftKey.SoftkeyManager.GetDateDue(out permanent, out authDate);
            if (!success)
            {
                return false;
            }

            int userId = 0;
            success = Galaxy.SoftKey.SoftkeyManager.GetUserId(out userId);
            if (!success)
            {
                return false;
            }

            if (!permanent)
            {
                FormSoftKey frmSoftkey = new FormSoftKey();
                frmSoftkey.UserId = userId.ToString();
                frmSoftkey.AuthorizeDate = authDate;
                DialogResult result = frmSoftkey.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return false;
                }
            }

            return success;
        }

        static SoftkeyManager()
        {
        }

        private static SoftkeyConnection softkeyConn;


        public static bool InitSoftKey()
        {
            try
            {
                if (softkeyConn != null) softkeyConn.Release();
                softkeyConn = new SoftkeyConnection();
                bool success = softkeyConn.CreateSoftkeyProcess();
                if (!success)
                {
                    string errorMessage;
                    CheckErrorMessage(out errorMessage);
                    log.Error(errorMessage);
                    MessageBox.Show(errorMessage);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                string errorMsg = "加密锁初始化出现错误！";
                log.Error(errorMsg);
                log.Error(ex);
                MessageBox.Show(errorMsg);
                return false;
            }
        }

        public static void ReleaseSoftKey()
        {
            if (softkeyConn != null) softkeyConn.Release();
        }


        /// <summary>
        /// 获取授权日期
        /// </summary>
        /// <param name="permanent">是否永久授权</param>
        /// <param name="dateTime">授权日期</param>
        /// <returns>获取是否成功</returns>
        public static bool GetUserId(out int userId)
        {
            userId = 0;

            userId = softkeyConn.UserId;
            if (softkeyConn.UserId == 0)
            {
                string errorMessage;
                CheckErrorMessage(out errorMessage);
                log.Error(errorMessage);
                MessageBox.Show(errorMessage);
                return false;
            }

            return true;
        }

        private static bool CheckErrorMessage(out string errorMessage)
        {
            errorMessage = "";
            switch (softkeyConn.ErrorMessage)
            {
                case SoftkeyErrorMessage.Nokey:
                    errorMessage = "未找到加密锁";
                    return false;
                //break;
                case SoftkeyErrorMessage.ConnectError:
                    errorMessage = "加密锁通信失败";
                    return false;
                //break;
                case SoftkeyErrorMessage.TimeError:
                    errorMessage = "加密锁授权时间格式错误";
                    return false;
                //break;
                default:
                    errorMessage = "";
                    break;

            }
            return true;
        }


        /// <summary>
        /// 获取授权日期
        /// </summary>
        /// <param name="permanent">是否永久授权</param>
        /// <param name="dateTime">授权日期</param>
        /// <returns>获取是否成功</returns>
        public static bool GetDateDue(out bool permanent, out DateTime dateTime)
        {
            permanent = false;
            dateTime = softkeyConn.LimitTime;
            if (dateTime == new DateTime())
                permanent = true;
            return true;

        }


    }

    enum SoftkeyErrorMessage
    {
        NoError,
        Nokey,
        ConnectError,
        TimeError,
        OtherError
    }

    /// <summary>
    /// 加密锁进程通信类
    /// </summary>
    class SoftkeyConnection
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SoftkeyConnection));

        public SoftkeyConnection()
        {
            this.firstRead = true;
            this.errorMessage = SoftkeyErrorMessage.NoError;
        }

        const string SOFTKEY_PROCESS_NAME = "AuthenticatorBG_x86";

        const int TIME_DELAY = 6000;

        const int MAX_NOKEY_TIME = 10;

        const int MAX_ERROR_TIME = 10;

        private Process hGBx86;
        private string szMmfId;

        private Thread threadReadkey;

        private DateTime limitTime;

        private DateTime lastTime;

        private int userId;

        private bool firstRead;

        private SoftkeyErrorMessage errorMessage;
        public SoftkeyErrorMessage ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }
        }

        public int UserId
        {
            get { return this.userId; }
        }

        public DateTime LimitTime
        {
            get { return this.limitTime; }
        }

        public DateTime LastTime
        {
            get { return this.LastTime; }
        }

        public bool CreateSoftkeyProcess()
        {
            try
            {
                Process[] ps = Process.GetProcessesByName(SOFTKEY_PROCESS_NAME);
                if (ps != null && ps.Length > 0)
                {
                    foreach (Process p in ps)
                    {
                        if (p != null && !p.HasExited)
                        {
                            p.Kill();
                            p.Dispose();
                        }
                    }
                }
                //启用BGx86进程
                hGBx86 = new Process();
                //启动命令中写入mmf名称和更新间隔时间
                szMmfId = Guid.NewGuid().ToString();
                hGBx86.StartInfo = new ProcessStartInfo()
                {
                    FileName = SOFTKEY_PROCESS_NAME + ".exe",
                    Arguments = szMmfId + " " + TIME_DELAY.ToString()
                };
                hGBx86.Start();

                threadReadkey = new Thread(ThreadReadKey);
                threadReadkey.IsBackground = true;
                threadReadkey.Start();
                while (firstRead)
                {
                    if (this.errorMessage != SoftkeyErrorMessage.NoError)
                    {
                        return false;
                    }
                    Thread.Sleep(10);
                }
            }
            catch (Exception e)
            {
                log.Error("加密锁通信进程启动失败");
                log.Error(e);
                this.errorMessage = SoftkeyErrorMessage.ConnectError;
                return false;
            }
            return true;
        }

        public void ThreadReadKey()
        {
            int curErrorTimes = 0;
            int curNoKeyTimes = 0;
            while (true)
            {
                try
                {
                    Authenticator_BGx86.ST_AUTH_RESULT st = Authenticator_BGx86.ReadBGx86Result(szMmfId);
                    if (st.m_iUserId == 0)
                    {
                        curNoKeyTimes++;
                        if (curNoKeyTimes < MAX_NOKEY_TIME)
                        {
                            //可能是偶尔发的读错，重新读
                            Thread.Sleep(10);
                            continue;
                        }
                    }
                    try
                    {
                        userId = st.m_iUserId;
                        if (userId == 0)
                        {
                            this.errorMessage = SoftkeyErrorMessage.Nokey;
                            return;
                        }
                        curNoKeyTimes = 0;
                        limitTime = new DateTime(st.m_lLimitTime);
                        lastTime = new DateTime(st.m_lLastCheckTime);
                    }
                    catch (Exception e)
                    {
                        log.Error("授权时间格式错误");
                        log.Error(e);
                        this.errorMessage = SoftkeyErrorMessage.TimeError;
                        return;
                    }
                    firstRead = false;
                    Thread.Sleep(TIME_DELAY);
                }
                catch (Exception e)
                {
                    curErrorTimes++;
                    if (curErrorTimes < MAX_ERROR_TIME)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    log.Error("加密锁通信失败");
                    log.Error(e);
                    this.errorMessage = SoftkeyErrorMessage.ConnectError;
                    return;
                }
            }
        }

        public void Release()
        {
            if (threadReadkey != null)
            {
                threadReadkey.Abort();
                threadReadkey.Join();

            }
            if (hGBx86 != null && !hGBx86.HasExited) { hGBx86.Kill(); hGBx86.Dispose(); }
        }

    }
}
