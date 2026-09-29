using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using AKRS.ZX2200.Controls.ToolControls.Programming;
using AKRS.ZX2200.TransportSystem.Models;

using DevExpress.XtraEditors;

namespace AKRS.ZX2200.TransportSystem.Controls.Setting
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Interface;
    using AKRS.ZX2200.TransportSystem.Models.DatasetModels.StockBin;

    /// <summary>
    /// 上料器设置
    /// </summary>
    public partial class UcUnloaderSetting : DevExpress.XtraEditors.XtraUserControl, IProgrammingControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UcUnloaderSetting()
        {
            InitializeComponent();
            this.Init();
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        public void Init()
        {
            this.RefreshCmb();

            this.CmbCurUnLoader.EditValue = TransportProgram.GetInstance().UnLoaderBinProgram.CurUnLoaderName;
        }

        /// <summary>
        /// 刷新下拉框
        /// </summary>
        public void RefreshCmb()
        {
            this.CmbCurUnLoader.Properties.Items.Clear();
            List<string> nameList = UnLoaderBinRepository.GetInstance().Filter(string.Empty, false).Select(a => a.Name).ToList();

            // 控件绑定数据源
            this.CmbCurUnLoader.Properties.Items.AddRange(nameList);
        }

        /// <summary>
        /// 下拉框按钮点击
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbCurUnLoader_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmRepository<UnloaderBin> frmRepository = new FrmRepository<UnloaderBin>(UnLoaderBinRepository.GetInstance());

                if (frmRepository.ShowDialog() == DialogResult.OK)
                {
                    UnloaderBin temp = (UnloaderBin)frmRepository.DsSetting;

                    if (temp != null)
                    {
                        ((ComboBoxEdit)sender).Text = temp.Name;
                    }
                }

                this.RefreshCmb();
            }
        }

        /// <summary>
        /// 确认和赋值 并保存
        /// </summary>
        public void Confirm()
        {
            TransportProgram.GetInstance().UnLoaderBinProgram.CurUnLoaderName = this.CmbCurUnLoader.Text;

            TransportProgram.GetInstance().Save();
        }


        /// <summary>
        /// 下拉框改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbCurUnLoader_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.Confirm();
        }
    }
}
