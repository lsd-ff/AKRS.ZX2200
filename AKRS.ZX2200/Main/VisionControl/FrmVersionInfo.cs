using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.Main.VisionControl;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.Main.Controls
{
    /// <summary>
    /// 版本信息
    /// </summary>
    public partial class FrmVersionInfo : XtraForm
    {
        /// <summary>
        /// 当前版本信息
        /// </summary>
        private Version CurrentVersion;

        /// <summary>
        /// 版本信息
        /// </summary>
        public FrmVersionInfo()
        {
            this.InitializeComponent();
            this.InitInfo();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// 初始化信息
        /// </summary>
        private void InitInfo()
        {
            this.CurrentVersion = new Version(MachineSoftwareConfiguration.GetInstance().CurrentVersionNumber);
            this.label6.Text = this.CurrentVersion.ToString();
            this.label7.Text = VersionUpdateAnnouncement.VersionInfos.Find(it => it.VersionNumber == this.CurrentVersion)?.UpdataTime.ToString("d");
            this.label8.Text = new Version(MachineSoftwareConfiguration.GetInstance().LastVersionNumber).ToString();
            this.label9.Text = MachineSoftwareConfiguration.GetInstance().LastVersionUpdataTime.ToString("d");


            // 需要更新的版本集合
            Version lastVersion = new Version(MachineSoftwareConfiguration.GetInstance().LastVersionNumber);
            List<VersionInfo> updates = VersionUpdateAnnouncement.VersionInfos.Where(v => v.VersionNumber > lastVersion).OrderByDescending(it => it.VersionNumber).ToList();

            // 注入信息
            foreach (VersionInfo versionInfo in updates)
            {
                this.memoEdit1.AppendText("版本号:" + versionInfo.VersionNumber + "\r\n");
                foreach (string info in versionInfo.UpdateContent)
                {
                    this.memoEdit1.AppendText("  " + info + "\r\n");
                }
                this.memoEdit1.AppendText("\r\n");
            }

            // 所有更新
            List<VersionInfo> allUpdates = VersionUpdateAnnouncement.VersionInfos.OrderByDescending(it => it.VersionNumber).ToList();
            foreach (VersionInfo versionInfo in allUpdates)
            {
                this.memoEdit2.AppendText("版本号:" + versionInfo.VersionNumber + "\r\n");
                foreach (string info in versionInfo.UpdateContent)
                {
                    this.memoEdit2.AppendText("  " + info + "\r\n");
                }
                this.memoEdit2.AppendText("\r\n");
            }
        }
    }
}
